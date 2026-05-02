# Specification: Bookmark Core

## Overview
The `bookmark-core` is the central engine for managing personal bookmarks. It allows users to create, organize, and import bookmarks into a hierarchical structure that is synchronized across their accounts.

## Requirements

### 1. Bookmark Management (CRUD)
- **Creation**: Users can manually add bookmarks by providing a title and URL.
- **Reading**: 
    - Users can list bookmarks within a specific category.
    - Users can view all bookmarks across all categories.
- **Updating**: Users can edit the title, URL, or the category assignment of an existing bookmark.
- **Deletion**: Users can delete individual bookmarks.

### 2. Category & Folder Hierarchy
- **Nested Categories**: Users can create folders (categories) and sub-folders to organize bookmarks hierarchically.
- **Category Management**: 
    - Create, rename, and delete categories.
    - Moving a category into another category (re-parenting).
- **Recursive Deletion**: Deleting a category must either move its children to a parent category or delete all nested bookmarks and sub-categories (configurable or system-default).

### 3. Browser Integration (Import)
- **Chrome/Edge Sync**: The system must leverage the `chrome.bookmarks` API to read the user's native browser bookmarks.
- **Selective Import**: Users should be able to choose specific folders or individual bookmarks from their browser to import into HamHam.
- **Structural Mapping**: The import process must preserve the folder hierarchy from the browser when transferring to the HamHam cloud storage.
- **Deduplication**: The system should detect and prevent duplicate bookmarks based on the URL during the import process.

### 4. Search & Filtering
- **Keyword Search**: Users can search for bookmarks by title or URL.
- **Filtering**: Users can filter bookmarks by category or date created.

### 5. Data Ownership & Security
- **User Isolation**: Bookmarks and categories must be strictly tied to a `UserId`. No user should be able to access or modify another user's bookmarks.
- **Integrity**: Deleting a user account must trigger a cascading delete of all associated bookmarks and categories.

## Acceptance Criteria
- A user can create a folder structure (e.g., "Work" $\rightarrow$ "Projects" $\rightarrow$ "HamHam") and add bookmarks to it.
- A user can import their existing Chrome bookmarks, and the folder structure is maintained in the app.
- Searching for a keyword returns all matching bookmarks across different categories.
- Moving a bookmark from one folder to another updates immediately in the UI.
- A user cannot access bookmarks belonging to another `UserId` via API manipulation.
