# Tasks: Project Initialization (init-project)

## Phase 1: Infrastructure & Project Setup
- [x] Setup development environment on Ubuntu VM (PostgreSQL 16, Redis 7, .NET 8 SDK).
- [x] Initialize .NET Core Web API project with a clean architecture.
- [x] Initialize React project for Chrome Extension using Vite/Webpack.
- [x] Configure Docker Compose for local development (API, Postgres, Redis).
- [x] Set up basic CI/CD pipeline or build scripts for Linux deployment.

## Phase 2: Backend Core - Auth & User Management
- [x] Implement PostgreSQL database migrations for `Users` and `UserPreferences` tables.
- [x] Implement `auth-system` logic:
    - [x] Password hashing using BCrypt/Argon2.
    - [x] JWT generation and validation middleware.
    - [x] Refresh token rotation mechanism.
- [x] Implement User Management API:
    - [x] Registration and Login endpoints.
    - [x] Admin-only user list and `IsActive` toggle endpoints.
- [x] Implement global `AuditMiddleware` to automatically handle `CreationTime`, `CreatorUser`, `LastModifyTime`, and `LastModifyUser`.
- [x] Implement global `SoftDeleteFilter` for all DB queries to exclude `IsDeleted = true`.
- [x] Implement database migrations for `Categories`, `Bookmarks`, `SharedPools`, `SharedPoolBookmarks`, and `IconLibrary`.
- [x] Implement `bookmark-core` API:
    - [x] Hierarchical category CRUD (including recursive delete).
    - [x] Personal bookmark CRUD.
    - [x] Browser bookmark import endpoint.
- [x] Implement `shared-library` API:
    - [x] Shared pool management for Admins.
    - [x] "Picking" logic to duplicate shared bookmarks into personal libraries.
- [x] Implement `IconLibrary` management API for uploading and listing icons.

## Phase 4: Real-time & Cache Integration
- [x] Implement `SyncHub` using SignalR.
- [x] Integrate SignalR events into Bookmark and Category services to push real-time updates.
- [x] Implement `wallpaper-engine` backend:
    - [x] Integration with Unsplash and Pexels APIs.
    - [x] Redis caching layer for wallpaper URL pools.
    - [x] Custom wallpaper upload and storage logic.
- [x] Configure `manifest.json` (V3) with `chrome_url_overrides` for New Tab page.
- [x] Implement the base New Tab Page layout using React and Tailwind CSS (referencing `code.html`).
- [x] Implement the Design System (Typography, Colors, Rounded Corners) as per `DESIGN.md`.
- [x] Build the Bento Grid and Category Card components.
- [x] Implement the Bookmark Item and Moodboard Gallery components.

## Phase 6: Full-Stack Feature Integration
- [x] Connect Extension UI to Auth API (Login/Register/Session persistence).
- [x] Integrate `bookmark-core` API with the UI (Loading/Creating/Moving bookmarks).
- [x] Implement the "Shared Pool" browser and picking interface.
- [x] Integrate `wallpaper-engine` with `chrome.alarms` for automated rotation.
- [x] Connect the UI to SignalR Hub for real-time updates without page refresh.

## Phase 7: Verification & Polishing
- [x] Conduct end-to-end testing: Registration $\rightarrow$ Import Bookmarks $\rightarrow$ Sync across browsers.
- [x] Verify Admin account controls (disabling a user) instantly blocks access.
- [x] Performance audit for New Tab Page load time.
- [x] Final UI/UX polishing and alignment with `DESIGN.md`.
