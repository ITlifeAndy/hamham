using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using HamHam.Infrastructure.Persistence;
using HamHam.Infrastructure.Services;
using HamHam.Application.Interfaces;
using HamHam.Infrastructure.Hubs;
using HamHam.Domain.Entities;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<HamHamDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<ISharedLibraryService, SharedLibraryService>();
builder.Services.AddScoped<IIconLibraryService, IconLibraryService>();
builder.Services.AddScoped<IWallpaperService, WallpaperService>();
builder.Services.AddScoped<IUnsplashService, UnsplashService>();
builder.Services.AddScoped<ISharePoolService, SharePoolService>();
builder.Services.AddScoped<ISharePoolBookmarkService, SharePoolBookmarkService>();
builder.Services.AddScoped<IBookmarkImportService, BookmarkImportService>();
builder.Services.AddScoped<UserSeeder>();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "super_secret_key_for_hamham_1234567890"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- MIDDLEWARE PIPELINE START ---

// 1. 全局異常處理 (最優先)
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { 
            error = "Internal Server Error", 
            message = "An unexpected error occurred on the server." 
        });
    });
});

// 2. CORS (必須在所有路由之前)
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "uploads")),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

// --- ENDPOINTS ---

app.MapGet("/debug-ping", () => "pong");

 // Seed default admin account
 using (var scope = app.Services.CreateScope())
 {
     var context = scope.ServiceProvider.GetRequiredService<HamHamDbContext>();
     
     // Automatically apply pending migrations on startup
     await context.Database.MigrateAsync();
     
     var seeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
     await seeder.SeedAsync();
 }

// Auth Endpoints (Minimal API)
app.MapPost("/api/auth/register", async (HamHamDbContext db, IAuthService authService, HamHam.Api.Controllers.RegisterRequest request) =>
{
    if (await db.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
    {
        return Results.BadRequest("Username or email already exists.");
    }
    var passwordHash = await authService.HashPassword(request.Password);
    var user = new User { Name = request.Name, Username = request.Username, Email = request.Email, PasswordHash = passwordHash, Role = UserRole.User, IsActive = true };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok(new { Message = "User registered successfully." });
}).WithName("Register").WithOpenApi();

app.MapPost("/api/auth/login", async (HamHamDbContext db, IAuthService authService, HamHam.Api.Controllers.LoginRequest request) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);
    if (user == null || !await authService.VerifyPassword(request.Password, user.PasswordHash)) return Results.Unauthorized();
    if (!user.IsActive) return Results.Unauthorized();
    var token = await authService.GenerateJwtToken(user);
    var refreshToken = await authService.GenerateRefreshToken();
    return Results.Ok(new { Token = token, RefreshToken = refreshToken, Name = user.Name });
}).WithName("Login").WithOpenApi();

app.MapGet("/api/users/profile", async (ClaimsPrincipal userPrincipal, HamHamDbContext db) =>
{
    var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
    if (user == null) return Results.NotFound();
    return Results.Ok(new { Username = user.Username, Name = user.Name, Email = user.Email, Avatar = user.Avatar, Role = user.Role, LastSyncTime = user.LastSyncTime });
}).RequireAuthorization().WithName("GetUserProfile").WithOpenApi();

app.MapPut("/api/users/profile", async (ClaimsPrincipal userPrincipal, HamHam.Api.Models.UserProfileUpdateDto dto, HamHamDbContext db) =>
{
    var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
    if (user == null) return Results.NotFound();
    user.Name = dto.Name; user.Email = dto.Email;
    if (!string.IsNullOrEmpty(dto.Password)) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    if (!string.IsNullOrEmpty(dto.AvatarUrl)) user.Avatar = dto.AvatarUrl;
    user.LastSyncTime = DateTimeOffset.UtcNow;
    await db.SaveChangesAsync();
    return Results.Ok(new { Message = "Profile updated successfully" });
}).RequireAuthorization().WithName("UpdateUserProfile").WithOpenApi();

app.MapPost("/api/users/avatar", async (ClaimsPrincipal userPrincipal, HttpRequest request, HamHamDbContext db, IWebHostEnvironment env) =>
{
    var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();
    if (!request.HasFormContentType) return Results.BadRequest("Invalid content type");
    var form = await request.ReadFormAsync();
    var file = form.Files.FirstOrDefault();
    if (file == null || file.Length == 0) return Results.BadRequest("No file uploaded");
    var uploadDir = Path.Combine(env.ContentRootPath, "uploads", "avatars");
    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLowerInvariant()}";
    using (var stream = new FileStream(Path.Combine(uploadDir, fileName), FileMode.Create)) await file.CopyToAsync(stream);
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
    if (user == null) return Results.NotFound();
    user.Avatar = $"/uploads/avatars/{fileName}";
    await db.SaveChangesAsync();
    return Results.Ok(new { AvatarUrl = user.Avatar });
}).RequireAuthorization().WithName("UploadAvatar").WithOpenApi();

app.MapGet("/api/public-bookmarks", async (Guid? poolId, ISharePoolBookmarkService bookmarkService, ISharePoolService poolService) =>
{
    var bookmarks = await bookmarkService.GetBookmarksAsync(poolId) ?? Enumerable.Empty<HamHam.Domain.Entities.SharedPoolBookmark>();
    var pools = await poolService.GetPoolsAsync() ?? Enumerable.Empty<HamHam.Domain.Entities.SharedPool>();
    var result = bookmarks.Select(b => new {
        b.Id, b.Name, b.Url,
        Category = pools.FirstOrDefault(p => p.Id == b.SharedPoolsId)?.Name ?? "Unknown"
    });
    return Results.Ok(result);
}).WithName("GetPublicBookmarks").WithOpenApi();

app.MapPost("/api/bookmarks/import-public", async (ClaimsPrincipal userPrincipal, HamHam.Api.Models.PublicImportRequest request, IBookmarkImportService importService) =>
{
    var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();
    var success = await importService.ImportPublicBookmarksAsync(userId, request.PublicBookmarkIds, request.TargetCategoryId, request.Color, request.Glass);
    return success ? Results.Ok(new { Message = "Bookmarks imported successfully" }) : Results.BadRequest(new { Message = "Failed to import bookmarks" });
}).RequireAuthorization().WithName("ImportPublicBookmarks").WithOpenApi();

app.MapHub<SyncHub>("/hubs/sync");
app.MapControllers();

app.MapGet("/weatherforecast", () => Results.Ok(new { Message = "NEW_VERSION_ACTIVE" })).WithName("GetWeatherForecast").WithOpenApi();

app.Run();

public record SharePoolCreateDto(string Name);
public record SharePoolUpdateDto(string Name);
public record SharePoolBookmarkCreateDto(Guid SharedPoolsId, string Name, string Url);
public record SharePoolBookmarkUpdateDto(string Name, string Url);
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) { public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); }
