## 1. Backend Database Setup

- [x] 1.1 Create `SharePools` table (Id, Name).
- [x] 1.2 Create `SharePoolBookMarks` table (Id, PoolId, Name, Url).
- [x] 1.3 Add uniqueness constraint on `(PoolId, Url)` in `SharePoolBookMarks`.

## 2. Backend API Implementation

- [x] 2.1 Implement `SharePoolService` for CRUD operations on categories.
- [x] 2.2 Implement `SharePoolBookmarkService` for CRUD operations on bookmarks.
- [x] 2.3 Create `SharePoolController` and `SharePoolBookmarkController` to expose APIs.
- [x] 2.4 Implement `GET /api/public-bookmarks` endpoint with optional `poolId` filter.
- [x] 2.5 Apply `[Authorize(Roles = "Admin")]` to all new endpoints.

## 3. Frontend UI Implementation

- [x] 3.1 Add "Public Bookmarks" link and route `/admin/public-bookmarks` in `BackendShell`.
- [x] 3.2 Create `PublicBookmarkManagement` page component.
- [x] 3.3 Implement the data table displaying Category, Name, and URL.
- [x] 3.4 Implement category filtering dropdown that triggers API refresh.
- [x] 3.5 Create "Add/Edit Pool" and "Add/Edit Bookmark" modals.
- [x] 3.6 Integrate modals with backend APIs.
- [x] 3.7 Align styling with User Management UI (colors, padding, alignment).

## 4. Verification & Polishing

- [x] 4.1 Verify creating and deleting categories.
- [x] 4.2 Verify adding and editing bookmarks within categories.
- [x] 4.3 Verify that category filtering correctly updates the table.
- [x] 4.4 Run `npm run build` to ensure no frontend regressions.
