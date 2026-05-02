## Why

To allow administrators to manage a shared pool of bookmarks that all users can access, facilitating discovery of useful resources and ensuring consistent access to important links across the user base.

## What Changes

- **Backend**:
  - Create `SharePools` table for bookmark categories.
  - Create `SharePoolBookMarks` table for public bookmark entries.
  - Implement CRUD API endpoints for managing Share Pools and Bookmarks.
  - Implement a retrieval API for public bookmarks with support for category filtering.
- **Frontend**:
  - Create a "Public Bookmark Management" page within the Admin shell.
  - Implement a data table displaying Category, Name, and URL.
  - Implement category-based filtering for the bookmark list.
  - Align UI/UX with the existing User Management interface.

## Capabilities

### New Capabilities
- `public-bookmark-management`: CRUD operations for shared bookmark pools and their associated bookmarks, including retrieval with category filtering.

### Modified Capabilities
(None)

## Impact

- **Database**: Introduction of two new tables (`SharePools`, `SharePoolBookMarks`).
- **Backend API**: New endpoints for managing public bookmarks.
- **Frontend**: New admin route and UI components for bookmark management.
