## Why

Establish the foundation for HamHam, a collaborative bookmark management extension. The goal is to transition from a static prototype to a full-stack application that provides personalized, synchronized, and aesthetically pleasing bookmark organization across different browsers.

## What Changes

- Initialize a .NET Core backend project optimized for Linux deployment (Ubuntu VM).
- Set up a PostgreSQL database to store user data, bookmark hierarchies, and shared libraries.
- Implement a React-based Chrome Extension using Manifest V3, specifically targeting the New Tab Page.
- Integrate Redis for caching external wallpaper API responses and managing session data.
- Establish a SignalR hub to enable real-time synchronization of bookmarks across multiple browser instances.
- Integrate Unsplash and Pexels APIs for dynamic, automated background image rotation.

## Capabilities

### New Capabilities
- `auth-system`: Custom account/password authentication, JWT-based authorization, and administrator controls to enable/disable user accounts.
- `bookmark-core`: CRUD operations for personal bookmarks and categories, including the ability to import existing bookmarks from the browser.
- `shared-library`: Creation of shared bookmark pools and a mechanism for users to discover and "pick" bookmarks from these pools into their personal collection.
- `wallpaper-engine`: Support for custom background uploads and automated image rotation via integration with Unsplash and Pexels.
- `realtime-sync`: Real-time synchronization of bookmark changes across all authenticated browser sessions using WebSockets/SignalR.
- `extension-ui`: A high-performance React interface implemented as a New Tab Page, adhering to the Miro-inspired design system.

### Modified Capabilities
- None

## Impact

- **Infrastructure**: Requires a Linux environment (Ubuntu) with PostgreSQL and Redis installed.
- **Browser APIs**: Extensive use of `chrome.bookmarks` for data migration, `chrome.storage` for local persistence, `chrome.alarms` for wallpaper rotation, and `chrome.tabs` for New Tab overrides.
- **External Dependencies**: Reliance on Unsplash and Pexels API availability and rate limits.
