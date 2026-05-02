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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "super_secret_key_for_hamham_1234567890"))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed default admin account if database is empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HamHamDbContext>();
    Console.WriteLine("Applying migrations...");
    // await context.Database.MigrateAsync();
    Console.WriteLine("Migrations skipped to prevent crash due to existing columns.");


    var seeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "uploads")),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/users/profile", async (ClaimsPrincipal userPrincipal, HamHamDbContext db) =>
{
    try 
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
        if (user == null) return Results.NotFound(new { message = "User not found" });

        return Results.Ok(new
        {
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            Avatar = user.Avatar,
            Role = user.Role,
            LastSyncTime = user.LastSyncTime
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.RequireAuthorization()
.WithName("GetUserProfile")
.WithOpenApi();

app.MapPut("/api/users/profile", async (ClaimsPrincipal userPrincipal, HamHam.Api.Models.UserProfileUpdateDto dto, HamHamDbContext db) =>
{
    try
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
        if (user == null) return Results.NotFound(new { message = "User not found" });

        user.Name = dto.Name;
        user.Email = dto.Email;

        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        if (!string.IsNullOrEmpty(dto.AvatarUrl))
        {
            user.Avatar = dto.AvatarUrl;
        }

        user.LastSyncTime = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();
        return Results.Ok(new { Message = "Profile updated successfully" });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.RequireAuthorization()
.WithName("UpdateUserProfile")
.WithOpenApi();

app.MapPost("/api/users/avatar", async (ClaimsPrincipal userPrincipal, HttpRequest request, HamHamDbContext db, IWebHostEnvironment env) =>
{
    try 
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

        if (!request.HasFormContentType) return Results.BadRequest("Invalid content type");

        var form = await request.ReadFormAsync();
        var file = form.Files.FirstOrDefault();
        if (file == null || file.Length == 0) return Results.BadRequest("No file uploaded");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension)) return Results.BadRequest("Invalid file type");

        var uploadDir = Path.Combine(env.ContentRootPath, "uploads", "avatars");
        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
        if (user == null) return Results.NotFound("User not found");

        user.Avatar = $"/uploads/avatars/{fileName}";
        await db.SaveChangesAsync();

        return Results.Ok(new { AvatarUrl = user.Avatar });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.RequireAuthorization()
.WithName("UploadAvatar")
.WithOpenApi();

app.MapGet("/api/public-bookmarks", async (Guid? poolId, ISharePoolBookmarkService bookmarkService, ISharePoolService poolService) =>
{
    var bookmarks = await bookmarkService.GetBookmarksAsync(poolId);
    var pools = await poolService.GetPoolsAsync();
    
    var result = bookmarks.Select(b => new {
        b.Id,
        b.Name,
        b.Url,
        Category = pools.FirstOrDefault(p => p.Id == b.SharedPoolsId)?.Name ?? "Unknown"
    });

    return Results.Ok(result);
})
.WithName("GetPublicBookmarks")
.WithOpenApi();

app.MapPost("/api/bookmarks/import-public", async (ClaimsPrincipal userPrincipal, HamHam.Api.Models.PublicImportRequest request, IBookmarkImportService importService) =>
{
    try
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

        var success = await importService.ImportPublicBookmarksAsync(
            userId, 
            request.PublicBookmarkIds, 
            request.TargetCategoryId, 
            request.Color, 
            request.Glass
        );

        if (success) return Results.Ok(new { Message = "Bookmarks imported successfully" });
        return Results.BadRequest(new { Message = "Failed to import bookmarks" });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.RequireAuthorization()
.WithName("ImportPublicBookmarks")
.WithOpenApi();

app.MapHub<SyncHub>("/hubs/sync");
app.MapControllers();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
