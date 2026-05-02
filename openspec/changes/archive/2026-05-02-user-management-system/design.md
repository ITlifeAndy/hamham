## Context

The HamHam extension requires an administrative interface to manage users. Currently, user data exists in the database, but there is no UI to maintain this data. The design is guided by a provided HTML mockup (`/tmp/Users.html`).

## Goals / Non-Goals

**Goals:**
- Create a separate "Backend Management System" area within the application.
- Implement a User Management page with full CRUD capabilities for the `Users` table.
- Provide a search, filter, and pagination system for users.
- Ensure the UI matches the reference design in terms of layout and styling.
- Add a trigger in the main UI (settings icon) to enter the admin area.

**Non-Goals:**
- Implementing a full-blown permission system (only simple Admin vs User role is required initially).
- Implementing other backend modules (e.g., detailed log analysis) unless specified in the navigation.

## Decisions

### 1. Routing Strategy
The backend system will be hosted under a specific route prefix (e.g., `/admin`). This allows for a distinct layout (the `BackendShell`) to be applied to all administrative pages without affecting the main user application.

### 2. Layout Architecture
A `BackendShell` component will be created. It will include:
- A sticky side navigation bar for switching between management modules (User Management, Public Bookmark Library).
- A main content area that renders the active module.
- A consistent header for page titles and primary actions.

### 3. Data Fetching & Pagination
To ensure scalability, user listing will be implemented with server-side pagination and filtering. The `UserManagement` component will request a page of data and the total count from the backend, reducing the payload size.

### 4. UI Framework
Tailwind CSS will be used to replicate the provided mockup. Specific colors (e.g., `surface-container`, `primary`) defined in the mockup's Tailwind config will be added to the project's `tailwind.config.js` to ensure visual consistency.

## Risks / Trade-offs

- **[Risk]** Unauthorized access to the admin area. $\rightarrow$ **[Mitigation]** Implement a route guard that checks the current user's role before allowing access to `/admin` routes.
- **[Risk]** UI discrepancy between HTML mockup and React implementation. $\rightarrow$ **[Mitigation]** Use the exact Tailwind classes and spacing defined in the reference HTML.
- **[Risk]** Performance degradation with many users. $\rightarrow$ **[Mitigation]** Use indexed searches on the `Users` table in the database and strict server-side pagination.
