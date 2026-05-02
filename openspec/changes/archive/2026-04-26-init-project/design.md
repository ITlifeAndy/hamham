# Design: Project Initialization (init-project)

## Overview
This document translates the requirements from the six core specifications into a technical implementation plan. The architecture focuses on scalability, real-time responsiveness, and a seamless user experience for the New Tab Page.

## 1. Database Schema (PostgreSQL)

### General Design Principles
- **Primary Key**: Every table uses `Id` (UUID) as the Primary Key.
- **Auditing**: Every table must contain the following columns:
    - `CreationTime` (Timestamp)
    - `CreatorUser` (UUID)
    - `LastModifyTime` (Timestamp)
    - `LastModifyUser` (UUID)
- **Soft Delete**: Every table must include the following columns for soft deletion:
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
- **Foreign Key Naming**: FK columns follow the pattern `SourceTable_SourceColumn` (e.g., `Users_Id`).

### User Management
- **`Users`**:
    - `Id` (UUID, PK)
    - `Username` (VARCHAR, Unique)
    - `Email` (VARCHAR, Unique)
    - `PasswordHash` (TEXT)
    - `Role` (Enum: `User`, `Admin`)
    - `IsActive` (BOOLEAN, Default: true)
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`
- **`UserPreferences`**:
    - `Id` (UUID, PK)
    - `Users_Id` (UUID, FK $\rightarrow$ Users.Id)
    - `WallpaperSource` (Enum: `Custom`, `Unsplash`, `Pexels`, `Mixed`)
    - `WallpaperKeywords` (TEXT[])
    - `RotationInterval` (INT, hours)
    - `IsWallpaperLocked` (BOOLEAN)
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`

### Bookmark System
- **`Categories`**:
    - `Id` (UUID, PK)
    - `Users_Id` (UUID, FK $\rightarrow$ Users.Id)
    - `Categories_Id` (UUID, FK $\rightarrow$ Categories.Id, Nullable - Parent)
    - `IconLibrary_Id` (UUID, FK $\rightarrow$ IconLibrary.Id, Nullable)
    - `Name` (VARCHAR)
    - `Color` (VARCHAR)
    - `SortOrder` (INT)
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`
- **`Bookmarks`**:
    - `Id` (UUID, PK)
    - `Users_Id` (UUID, FK $\rightarrow$ Users.Id)
    - `Categories_Id` (UUID, FK $\rightarrow$ Categories.Id)
    - `IconLibrary_Id` (UUID, FK $\rightarrow$ IconLibrary.Id, Nullable)
    - `Title` (VARCHAR)
    - `Url` (TEXT)
    - `FaviconUrl` (TEXT)
    - `IsFavorite` (BOOLEAN)
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`

### Shared Library System
- **`SharedPools`**:
    - `Id` (UUID, PK)
    - `Users_Id` (UUID, FK $\rightarrow$ Users.Id - Creator)
    - `IconLibrary_Id` (UUID, FK $\rightarrow$ IconLibrary.Id, Nullable)
    - `Name` (VARCHAR)
    - `Description` (TEXT)
    - `IsPublic` (BOOLEAN)
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`
- **`SharedPoolBookmarks`**:
    - `Id` (UUID, PK)
    - `SharedPools_Id` (UUID, FK $\rightarrow$ SharedPools.Id)
    - `IconLibrary_Id` (UUID, FK $\rightarrow$ IconLibrary.Id, Nullable)
    - `BookmarkTitle` (VARCHAR)
    - `BookmarkUrl` (TEXT)
    - `CategoryId` (UUID, Nullable - Internal pool category)
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`

### Asset Management
- **`IconLibrary`**:
    - `Id` (UUID, PK)
    - `Name` (VARCHAR)
    - `FilePath` (TEXT) - Path to the image file in storage.
    - `Category` (VARCHAR) - e.g., "work", "social", "shopping".
    - `IsDeleted` (BOOLEAN)
    - `DeleteUser` (UUID, Nullable)
    - `DeleteTime` (Timestamp, Nullable)
    - `CreationTime`, `CreatorUser`, `LastModifyTime`, `LastModifyUser`

## 2. API Design (REST)

### Auth API (`/api/auth`)
- `POST /register`: Create new account.
- `POST /login`: Authenticate and return JWT + Refresh Token.
- `POST /refresh`: Use refresh token to get a new access token.
- `POST /logout`: Invalidate session.

### User Management API (`/api/admin`) - *Admin Only*
- `GET /users`: List all users.
- `PATCH /users/{id}/status`: Toggle `IsActive` or update `Role`.

### Bookmark API (`/api/bookmarks`)
- `GET /`: List all categories and bookmarks (hierarchical).
- `POST /categories`: Create folder.
- `PATCH /categories/{id}`: Update name/parent.
- `DELETE /categories/{id}`: Delete folder (with recursive logic).
- `POST /`: Create bookmark.
- `PATCH /bookmarks/{id}`: Update bookmark.
- `DELETE /bookmarks/{id}`: Delete bookmark.
- `POST /import`: Batch import from browser bookmarks.

### Shared Library API (`/api/shared`)
- `GET /pools`: List available shared pools.
- `GET /pools/{id}`: Get bookmarks within a pool.
- `POST /pick`: Copy a bookmark from a pool to a personal category.

### Wallpaper API (`/api/wallpaper`)
- `GET /current`: Get current active wallpaper URL.
- `POST /upload`: Upload custom image.
- `PATCH /preferences`: Update keywords/source/interval.
- `POST /refresh`: Trigger manual rotation and return new URL.

## 3. Real-time Sync (SignalR)

### Hub: `SyncHub`
- **Group Mapping**: Each user is added to a group named after their `UserId` upon connection.
- **Events (Server $\rightarrow$ Client)**:
    - `NotifyBookmarkChanged(BookmarkEvent event)`: Payload includes `{ type: "CREATE|UPDATE|DELETE", entityId: "...", userId: "..." }`.
    - `NotifyCategoryChanged(CategoryEvent event)`: Payload includes `{ type: "CREATE|UPDATE|DELETE", entityId: "...", userId: "..." }`.
    - `NotifyPoolUpdated(PoolEvent event)`: Broadcast to all users browsing a shared pool.

## 4. Caching Strategy (Redis)

- **Wallpaper Cache**: 
    - Key: `wallpaper:pool:{keyword}`
    - Value: JSON array of image URLs from Unsplash/Pexels.
    - TTL: 24 Hours.
- **Session/Token Cache**: Store revoked JWTs (blacklist) or refresh token mappings for fast verification.

## 5. Infrastructure (Linux/Ubuntu VM)

- **Runtime**: .NET 8.0 SDK/Runtime.
- **Database**: PostgreSQL 16.
- **Cache**: Redis 7.
- **Proxy**: Nginx as a reverse proxy for the .NET Kestrel server.
- **Containerization**: Docker Compose to manage API, DB, and Redis.
