# Specification: Wallpaper Engine

## Overview
The `wallpaper-engine` manages the visual background of the New Tab Page. It provides a blend of user-curated content and automated discovery via external high-quality image APIs, ensuring the user's experience remains fresh and personalized.

## Requirements

### 1. Wallpaper Sources
- **Custom Upload**:
    - Users can upload their own images from their local machine.
    - Images are stored on the server (or a cloud storage provider) and associated with the user's account.
- **API Integration**:
    - **Unsplash**: Integration via official API to fetch high-resolution images based on keywords.
    - **Pexels**: Integration via official API to fetch high-resolution images.
- **Source Priority**: Users can choose their preferred source (e.g., "Only my uploads", "Only Unsplash", or "Mixed").

### 2. Automated Rotation Logic
- **Scheduled Updates**:
    - The system must support automated image rotation based on a user-defined or system-default interval (e.g., every 1 hour, 12 hours, or 24 hours).
- **Trigger Mechanism**:
    - Use a Chrome Extension `Background Service Worker` combined with `chrome.alarms` to trigger a request for a new wallpaper image.
- **Keyword-Based Discovery**:
    - Users can specify a set of "Interest Keywords" (e.g., "nature", "minimalism", "cyberpunk") that the engine uses to fetch images from Unsplash/Pexels.

### 3. Performance & Caching (Redis)
- **API Response Caching**:
    - To avoid hitting external API rate limits and to improve response speed, the backend must cache a pool of image URLs in Redis.
- **Cache TTL**: Implement a Time-To-Live (TTL) for cached images to ensure the pool is refreshed periodically.
- **Lazy Loading**: The frontend should load the image asynchronously to ensure the New Tab Page remains responsive.

### 4. User Customization & Controls
- **Manual Refresh**: Users can trigger an immediate wallpaper change via a "Refresh" button in the UI.
- **Image Locking**: Users can "lock" a current image they like, which disables the automatic rotation until unlocked.
- **Preference Management**: A settings panel to manage uploaded images and API keywords.

## Acceptance Criteria
- A user can upload a local JPG/PNG, and it is correctly set as the background.
- The system automatically changes the background every 24 hours using an Unsplash image based on the keyword "architecture".
- When the background refreshes, the image URL is retrieved from the Redis cache if available, reducing the call to the external API.
- A user can click "Refresh" and immediately see a new image from the selected source.
- Locking the image prevents the `chrome.alarms` trigger from changing the wallpaper.
