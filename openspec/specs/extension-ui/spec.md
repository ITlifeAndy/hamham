# Specification: Extension UI

## Overview
The `extension-ui` is the visual face of HamHam, implemented as a Chrome Extension "New Tab" override. It transforms the browser's new tab into a personalized, aesthetically pleasing dashboard based on the Miro-inspired design system. It serves as the primary interface for interacting with bookmarks, categories, and the wallpaper engine.

## Requirements

### 1. Layout & Structure (New Tab Page)
- **Main Canvas**: A full-screen responsive layout with a radial-dot background pattern (`canvas-bg`).
- **Top Navigation Bar**:
    - **Left**: Brand Logo and "HamHam" title.
    - **Center**: Real-time date and time display.
    - **Right**: Settings trigger and User Profile avatar.
- **Welcome Hero**: A personalized greeting section (e.g., "Hello, Hamster!") to establish a friendly tone.
- **Bento/Masonry Grid**:
    - A dynamic grid of "Category Cards" that organize bookmarks.
    - Responsive behavior: 4 columns on desktop $\rightarrow$ 2 on tablet $\rightarrow$ 1 on mobile.
- **Add Collection Entry**: A distinct, dashed-border card acting as a CTA for creating new categories.

### 2. Component Design (Based on prototype)
- **Category Card**:
    - Theme-based background colors (e.g., `primary-fixed`, `accent-teal-light`, `accent-pink-soft`).
    - Header containing a category icon, title, and an expand/collapse toggle.
    - Content area displaying a list or grid of `BookmarkItems`.
- **Bookmark Item**:
    - Compact card design with a small icon/favicon, title, and source domain.
    - Hover state: Subtle lift effect (`-translate-y-0.5`) and visibility of the "Open in New Tab" icon.
    - Active state: Scale-down effect (`scale-95`).
- **Moodboard/Gallery Component**:
    - A specialized grid for visual bookmarks (images), utilizing a square aspect ratio and a "Favorite" overlay on hover.
- **Settings Panel**: A slide-over or modal interface for managing account, wallpaper preferences, and API keywords.

### 3. Visual & Design System
- **Typography**:
    - Display/Headings: `Epilogue` (font-weight 500-700).
    - Body/Standard: `Inter` (font-weight 400).
- **Color Palette**: Adhere strictly to the `DESIGN.md` specifications, including surface colors, pastel accents, and the primary blue (`#2f4dd5`).
- **Border & Radius**: Use generous rounded corners (8px for buttons, 24px for cards) and ring-shadow borders (`rgba(224, 226, 232, 1)`).
- **Animations**: Implement smooth transitions for hover states and layout shifts using Tailwind CSS transitions.

### 4. Interaction Logic
- **Navigation**: Clicking a `BookmarkItem` opens the URL in a new tab.
- **Organization**: Support for drag-and-drop of bookmarks between categories (to be integrated with `bookmark-core`).
- **Wallpaper Sync**: The background must update seamlessly when the `wallpaper-engine` triggers a change, without flashing white.
- **Real-time Updates**: UI components must re-render selectively when a `realtime-sync` event is received (e.g., a new bookmark appearing in a category).

## Acceptance Criteria
- The New Tab page loads instantly with the user's set wallpaper and correct layout.
- The Bento grid correctly wraps and adjusts based on screen width (Desktop/Tablet/Mobile).
- Hovering over a bookmark item shows the "external link" icon and triggers a slight upward translation.
- Changing a setting in the settings panel is immediately reflected in the UI (e.g., changing the greeting name).
- The design strictly matches the colors and typography defined in the `DESIGN.md` and `code.html` prototype.
