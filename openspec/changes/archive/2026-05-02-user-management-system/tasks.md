## 1. Backend API Implementation

- [x] 1.1 Create `UserController` and `UserService` in the backend.
- [x] 1.2 Implement `GET /api/users` endpoint with server-side pagination, searching, and filtering.
- [x] 1.3 Implement `POST /api/users` endpoint for creating new users.
- [x] 1.4 Implement `PUT /api/users/{id}` endpoint for updating user details.
- [x] 1.5 Implement `DELETE /api/users/{id}` endpoint for removing users.
- [x] 1.6 Add role-based authorization to ensure only 'Admin' users can access these endpoints.

## 2. Frontend Foundation & Navigation

- [ ] 2.1 Add a settings icon to the top-right corner of the main application UI.
- [ ] 2.2 Implement `/admin` routing and the `BackendShell` component.
- [x] 2.3 Build the side navigation bar with links to "User Management" and "Public Bookmark Library".
- [x] 2.4 Implement a route guard to prevent non-admin users from accessing `/admin` routes.

## 3. User Management UI

- [x] 3.1 Create the `UserManagement` page component.
- [x] 3.2 Implement the User List table using Tailwind CSS, matching the design in `/tmp/Users.html`.
- [x] 3.3 Integrate the table with the `GET /api/users` endpoint for data fetching.
- [x] 3.4 Implement the search field and filtering logic.
- [x] 3.5 Implement pagination controls.
- [x] 3.6 Create the "Add User" modal and integrate it with the `POST` API.
- [x] 3.7 Create the "Edit User" modal and integrate it with the `PUT` API.
- [x] 3.8 Implement the delete user functionality with a confirmation prompt.

## 4. Verification & Polishing

- [ ] 4.1 Verify all CRUD operations in the User Management interface.
- [ ] 4.2 Ensure visual fidelity matches the reference HTML mockup.
- [ ] 4.3 Run `npm run build` to ensure no frontend regressions.
- [ ] 4.4 Verify backend stability using `docker-compose logs`.
