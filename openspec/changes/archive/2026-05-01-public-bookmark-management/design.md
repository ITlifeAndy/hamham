## Context

Currently, the application allows users to manage their own bookmarks, but there is no mechanism for administrators to provide a curated set of "Public Bookmarks" that are accessible to all users. This change introduces a management system for shared bookmark pools and their entries within the admin backend.

## Goals / Non-Goals

**Goals:**
- Implement backend storage for shared bookmark categories (`SharePools`) and bookmark entries (`SharePoolBookMarks`).
- Create a management interface in the admin backend to perform CRUD operations on these bookmarks.
- Implement category-based filtering to make navigation easier as the number of public bookmarks grows.
- Ensure UI consistency by following the design of the User Management page.

**Non-Goals:**
- Implementing a "Share" feature for users to create their own public pools.
- Adding advanced bookmark metadata (e.g., tags, descriptions, ratings).
- Implementing a user-facing discovery page (this change focuses on the *management* side).

## Decisions

### Data Model
The system will use a one-to-many relationship between `SharePools` and `SharePoolBookMarks`:
- `SharePools`: Contains `Id` (GUID) and `Name` (String).
- `SharePoolBookMarks`: Contains `Id` (GUID), `PoolId` (FK to `SharePools`), `Name` (String), and `Url` (String).
Rationale: This structure allows admins to group bookmarks logically and makes filtering by category efficient.

### UI Implementation
The management interface will be implemented as a new page in the `BackendShell`. It will reuse the Tailwind-based table component pattern seen in `UserManagement.tsx` to maintain visual cohesion.

## Risks / Trade-offs

- **[Risk]**: Duplicate bookmarks across different pools or within the same pool.
- **[Mitigation]**: Implement a uniqueness constraint on the `Url` column within the same `PoolId` to prevent exact duplicates.
- **[Risk]**: Large numbers of bookmarks causing UI lag.
- **[Mitigation]**: Use server-side pagination if the dataset grows significantly, though for public pools, the number of entries is expected to remain manageable.
