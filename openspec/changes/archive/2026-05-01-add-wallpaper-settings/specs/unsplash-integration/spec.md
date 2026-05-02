## ADDED Requirements

### Requirement: Unsplash Image Retrieval
The system SHALL fetch a random high-quality image from the Unsplash API based on the user's dynamic wallpaper setting.

#### Scenario: Fetching Random Image
- **WHEN** dynamic wallpaper is enabled and the image needs refreshing
- **THEN** system calls the Unsplash `/photos/random` endpoint and retrieves a valid image URL

### Requirement: Dynamic Wallpaper Scheduling
The system MUST refresh the Unsplash wallpaper based on a defined schedule (e.g., every 24 hours).

#### Scenario: Daily Refresh
- **WHEN** the app is loaded and the last wallpaper update was more than 24 hours ago
- **THEN** system triggers a new Unsplash image retrieval and updates the user's current wallpaper URL

### Requirement: Unsplash API Error Handling
The system SHALL handle Unsplash API failures gracefully without breaking the application UI.

#### Scenario: API Rate Limit Exceeded
- **WHEN** the Unsplash API returns a 429 (Too Many Requests) error
- **THEN** system retains the previous wallpaper and logs a warning, or falls back to a default system wallpaper
