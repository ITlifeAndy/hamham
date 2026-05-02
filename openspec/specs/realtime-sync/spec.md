# Specification: Realtime Sync

## Overview
The `realtime-sync` capability ensures that all user data, specifically bookmarks and categories, are consistently synchronized across all authenticated browser sessions in real-time. It eliminates the need for manual page refreshes and ensures a seamless collaborative experience.

## Requirements

### 1. SignalR Integration
- **WebSocket Communication**: Use ASP.NET Core SignalR as the primary transport layer for real-time bidirectional communication.
- **Hub Architecture**: Implement a `SyncHub` that manages active connections and routes messages based on `UserId`.
- **Connection Lifecycle**: 
    - Establish connection upon successful login.
    - Automatically handle reconnection logic if the network is interrupted.

### 2. Synchronization Events
The system must broadcast updates for the following events:
- **Bookmark Changes**: 
    - `BookmarkCreated`: Notify other sessions to add a new bookmark.
    - `BookmarkUpdated`: Notify other sessions to update metadata (Title, URL) or category.
    - `BookmarkDeleted`: Notify other sessions to remove a bookmark.
- **Category Changes**: 
    - `CategoryCreated`, `CategoryUpdated`, `CategoryDeleted`: Sync folder hierarchy changes.
- **Shared Library Updates**: 
    - `PoolUpdated`: Notify users browsing a shared pool that new content has been added or removed.

### 3. Sync Strategy: Push vs. Pull
- **Push Notification (Real-time)**: The server pushes a lightweight event (e.g., `{ event: "BOOKMARK_UPDATED", id: "123" }`) to the client.
- **Selective Pull (Update)**: Upon receiving a push notification, the client can either:
    - Apply the change directly if the payload contains the data.
    - Fetch the updated data for that specific entity from the REST API to ensure consistency.

### 4. Conflict Resolution
- **Last-Write-Wins (LWW)**: Implement a simple "Last-Write-Wins" strategy using a `UpdatedAt` timestamp on all synced entities.
- **Local Optimistic UI**: The UI should update immediately (Optimistic Update), but roll back if the server returns an error during the sync process.

### 5. Resource Efficiency
- **Filtered Broadcasting**: Messages must be sent only to connections associated with the specific `UserId` affected by the change, not to all users.
- **Connection Management**: Use a heartbeat mechanism to detect and clean up stale connections.

## Acceptance Criteria
- A user has the extension open in two different browser windows. Adding a bookmark in window A causes it to appear in window B within < 500ms without a page refresh.
- Deleting a category in one session immediately removes all nested bookmarks in another session.
- Updating a bookmark title in session A is reflected in session B in real-time.
- The system recovers automatically from a temporary network disconnect and resumes synchronization.
