## Context

Currently, the HamHam application has a standard layout without user-configurable wallpapers. User settings are managed via a backend service, and the frontend is a React-based extension. We need to introduce a mechanism to store, retrieve, and apply wallpapers to the main application workspace.

## Goals / Non-Goals

**Goals:**
- Enable users to set custom uploaded images as wallpapers.
- Enable users to set solid hex colors as wallpapers.
- Integrate with the Unsplash API for dynamic, scheduled wallpaper updates.
- Maintain high performance during application load and wallpaper transitions.
- Provide an intuitive UI for wallpaper management via the avatar dropdown and settings page.

**Non-Goals:**
- In-app image editing (e.g., cropping, filters).
- Social sharing of custom wallpapers.
- Support for animated wallpapers (GIFs, videos).

## Decisions

### 1. Storage and Data Model
- **Preference Storage**: Add `WallpaperType` (`Image`, `Color`, `Unsplash`) and `WallpaperValue` (URL or Hex code) to the `UserSettings` table.
- **Image Hosting**: Custom uploaded images will be stored in cloud storage (e.g., S3/Azure Blob), with the resulting public URL stored in `WallpaperValue`.

### 2. Unsplash Integration
- **API Usage**: Use the `/photos/random` endpoint to fetch images.
- **Scheduling**: Implement a "Daily Refresh" by storing a `WallpaperLastUpdated` timestamp. When the app loads, if the current time exceeds the last update by 24 hours, a new request to Unsplash is triggered.

### 3. Frontend Implementation
- **Application**: Apply the wallpaper using a global CSS variable (e.g., `--app-wallpaper`) applied to the root container.
- **Visuals**: Use `background-size: cover` and `background-position: center`.
- **Accessibility**: Implement a configurable semi-transparent overlay to ensure text contrast regardless of the wallpaper choice.

### 4. Upload Process
- Use a standard HTML file input and a multipart upload API to the backend for custom image uploads.

## Risks / Trade-offs

- **[Risk] Unsplash API Rate Limits** $\rightarrow$ **Mitigation**: Cache the fetched image URL for 24 hours; fallback to a default wallpaper if the API limit is reached or a request fails.
- **[Risk] Large image uploads impacting performance** $\rightarrow$ **Mitigation**: Implement client-side image resizing/compression before uploading.
- **[Risk] Readability issues on diverse backgrounds** $\rightarrow$ **Mitigation**: Provide a background overlay opacity slider in the settings.
