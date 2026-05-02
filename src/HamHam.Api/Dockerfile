FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/HamHam.Api/HamHam.Api.csproj src/HamHam.Api/
COPY src/HamHam.Application/HamHam.Application.csproj src/HamHam.Application/
COPY src/HamHam.Domain/HamHam.Domain.csproj src/HamHam.Domain/
COPY src/HamHam.Infrastructure/HamHam.Infrastructure.csproj src/HamHam.Infrastructure/
RUN dotnet restore src/HamHam.Api/HamHam.Api.csproj

# Copy everything else and build
COPY . .
WORKDIR /app/src/HamHam.Api
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HamHam.Api.dll"]
