## 1. Backend Setup

- [x] 1.1 Update `UserSettings` model to include `WallpaperType` and `WallpaperValue`.
- [x] 1.2 Add database migration to update `UserSettings` table.
- [x] 1.3 Implement `UpdateWallpaperAsync` endpoint to save user preference.
- [x] 1.4 Implement image upload endpoint (multipart) for custom wallpapers, integrating with cloud storage.

## 2. Unsplash Integration

- [x] 2.1 Create `UnsplashService` to handle API calls to `/photos/random`.
- [x] 2.2 Implement logic to check `WallpaperLastUpdated` and trigger refresh if > 24h.
- [x] 2.3 Add error handling and fallback for Unsplash API failures.

## 3. Frontend UI - Menu & Navigation

- [x] 3.1 Add "Set Wallpaper" item to the avatar dropdown menu.
- [x] 3.2 Create `WallpaperSettingsPage` component.
- [x] 3.3 Implement the UI for selecting Wallpaper Type (Custom, Color, Unsplash).

## 4. Frontend UI - Wallpaper Controls

- [x] 4.1 Implement solid color picker (Hex input/palette).
- [x] 4.2 Implement image upload interface with preview.
- [x] 4.3 Implement Unsplash dynamic wallpaper toggle and status display.

## 5. Application Integration

- [x] 5.1 Create a global wallpaper provider or state to store current wallpaper.
- [x] 5.2 Update the main app layout to apply the wallpaper as a background via CSS variables.
- [x] 5.3 Implement the background overlay opacity slider for readability.

## 6. Testing & Verification

- [x] 6.1 Verify solid color application.
- [x] 6.2 Verify custom image upload and application.
- [x] 6.3 Verify Unsplash dynamic refresh logic.
- [x] 6.4 Verify overlay opacity works as expected.
