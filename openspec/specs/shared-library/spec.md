# Specification: Shared Library

## Overview
The `shared-library` capability allows for the creation of collective bookmark pools that act as a discovery mechanism. Unlike personal libraries, the shared library is a "pool" where curated bookmarks are hosted, and users can selectively "pick" specific bookmarks to add to their own personal collections.

## Requirements

### 1. Shared Pool Management
- **Pool Creation**: Administrators (or authorized users) can create a shared bookmark pool (e.g., "Developer Essentials", "Design Inspiration").
- **Curated Content**: Administrators can add, edit, or remove bookmarks within the shared pool.
- **Organization**: Shared pools can have their own internal category structure to help users browse content efficiently.
- **Visibility**: Shared pools can be marked as public (visible to all authenticated users) or restricted to specific groups/roles.

### 2. The "Picking" Mechanism (Selective Acquisition)
- **Discovery**: Users can browse the content of shared pools without these bookmarks being added to their personal account.
- **Bulk Selective Pick**: Users SHALL be able to multi-select individual bookmarks or entire categories from a shared pool and "pick" them in bulk into their personal library.
- **Import Logic**: When bookmarks are "picked":
    - A copy of the bookmark metadata (Title, Subtitle, URL) is created in the user's personal bookmark table.
    - The user SHALL be prompted to choose a personal category/folder for the batch, with the option to create a new one.
    - The user SHALL be allowed to specify visual properties (color, glass effect) for the picked items.
- **No Automatic Sync**: Adding a bookmark to a shared pool does NOT automatically add it to all users' personal libraries.

#### Scenario: Bulk picking bookmarks
- **WHEN** a user selects 10 bookmarks from the "Developer Essentials" pool
- **THEN** the system SHALL allow them to pick all 10 in a single operation.
- **WHEN** the user selects a destination folder and confirms
- **THEN** all 10 bookmarks SHALL be cloned into the user's personal account.


### 3. Source Tracking (Optional but Recommended)
- **Origin Reference**: The system should track that a personal bookmark was originally "picked" from a specific shared pool.
- **Update Notification**: If a bookmark in a shared pool is updated (e.g., URL changed), the system may optionally notify users who have picked that bookmark, allowing them to update their personal copy.

### 4. Permissions & Access
- **Read Access**: All authenticated users (or those with specific roles) can read shared pools.
- **Write Access**: Only Admins or designated curators can modify the contents of a shared pool.

## Acceptance Criteria
- An admin can create a shared pool named "AI Tools" and add 10 high-quality bookmarks to it.
- A user can open the "AI Tools" pool, browse the bookmarks, and select only 2 of them to add to their "My AI" personal folder.
- The other 8 bookmarks remain in the shared pool but are NOT present in the user's personal library.
- A user cannot delete or modify a bookmark directly within the shared pool.
- The "picking" action correctly duplicates the bookmark into the user's private data space.
