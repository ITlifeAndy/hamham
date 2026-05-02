## 1. Database Updates

- [x] 1.1 Add `Avatar` (string) and `LastSyncTime` (DateTimeOffset?) properties to the `User` model in the backend.
- [x] 1.2 Create a new Entity Framework Core migration to update the `Users` table.
- [x] 1.3 Apply the migration to the database.

## 2. Backend API Implementation

- [x] 2.1 Create `UserProfileUpdateDto` to handle profile update requests.
- [x] 2.2 Implement a GET endpoint (e.g., `/api/users/profile`) to retrieve the current authenticated user's profile.
- [x] 2.3 Implement the `PUT /api/users/profile` endpoint to handle updates to Name, Email, and Password.
- [x] 2.4 Implement file upload logic for the user avatar, storing the file in `uploads/avatars` and saving the path in the database.
- [x] 2.5 Ensure password updates are hashed using the existing BCrypt implementation.

## 3. Frontend Implementation

- [x] 3.1 Create the `ProfileSettingsModal` component with fields for Name, Email, and Password.
- [x] 3.2 Set the Username field to read-only in the `ProfileSettingsModal`.
- [x] 3.3 Implement the avatar upload UI and integration with the backend API.
- [x] 3.4 Add the "Settings" option to the user avatar dropdown menu to open the `ProfileSettingsModal`.
- [x] 3.5 Integrate the modal with the profile retrieval and update APIs.

## 4. Verification and Testing

- [x] 4.1 Verify that personal information can be updated and persists after refresh.
- [x] 4.2 Verify that the username cannot be edited.
- [x] 4.3 Verify that avatar images can be uploaded and are displayed correctly.
- [x] 4.4 Test error handling for invalid emails or password updates.
