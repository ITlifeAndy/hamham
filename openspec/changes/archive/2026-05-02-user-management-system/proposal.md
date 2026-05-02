## Why

The application currently lacks an administrative interface for managing system users. To ensure proper maintenance of the `Users` database and control over user access, a dedicated Backend Management System is required.

## What Changes

- **Frontend Entry Point**: Add a settings icon in the top-right corner of the application that navigates the user to the Backend Management System.
- **Backend Layout**: Implement a management shell featuring a side navigation bar with links to "User Management" and "Public Bookmark Library".
- **User Management Page**: Create a comprehensive user maintenance interface based on the provided design (`/tmp/Users.html`):
    - **User List Table**: Display name, account ID, email, last sync time, role, and status.
    - **Search & Filter**: Implement search by name/email/ID and filtering/exporting capabilities.
    - **User Actions**: Ability to create new users, edit existing user details, and delete users.
    - **Pagination**: Implement page navigation for large user sets.
- **API Integration**: Develop new API endpoints to support full CRUD operations on the `Users` table.

## Capabilities

### New Capabilities
- `backend-shell`: The overall layout, theme, and navigation structure for the administrative area.
- `user-management`: The functional interface and logic for managing the `Users` table (listing, searching, adding, editing, and deleting).
- `backend-navigation`: The mechanism to trigger the transition from the main user interface to the backend system.

### Modified Capabilities
- None.

## Impact

- **Frontend**: New routes and components for the backend shell and user management page.
- **API**: New `userApi` services and corresponding backend controllers/services for the `Users` table.
- **Dependencies**: Addition of table and form components matching the specified design.
