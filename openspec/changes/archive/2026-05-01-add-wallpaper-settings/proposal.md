## Why

Users want more personalization for their workspace. Allowing custom wallpapers (images, colors, or curated from Unsplash) enhances the aesthetic experience and allows users to customize their environment to their preference.

## What Changes

- Add "Set Wallpaper" entry to the user avatar dropdown menu in the extension.
- Create a "Wallpaper Settings" UI where users can choose between:
    - Uploading a local image.
    - Selecting a solid hex color.
    - Enabling "Unsplash Dynamic Wallpaper" with scheduling options (e.g., daily).
- Implement backend storage for wallpaper preferences in the user profile/settings.
- Update the application layout to apply the selected wallpaper.
- Integrate Unsplash API to fetch random/curated images.

## Capabilities

### New Capabilities
- `wallpaper-settings`: Managing user wallpaper preferences (upload, color, Unsplash).
- `unsplash-integration`: Fetching and scheduling images from the Unsplash API.

### Modified Capabilities

## Impact

- Frontend: `AvatarDropdown`, `Settings` pages, Global layout.
- Backend: `UserSettings` model, new API endpoints for wallpaper management.
- Integration: Unsplash API.
