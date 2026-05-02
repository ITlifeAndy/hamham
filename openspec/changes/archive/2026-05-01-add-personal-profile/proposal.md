## Why

Currently, users cannot manage their own profile information (name, email, password, avatar). Adding this capability improves user autonomy and personalization within the HamHam system.

## What Changes

- **Database**: Add `Avatar` (image path/URL) and `LastSyncTime` (DateTime) columns to the `Users` table.
- **Frontend**: Add a "Settings" option to the user avatar dropdown menu.
- **Frontend**: Implement a Profile Settings UI (modal or page) allowing users to edit their Name, Email, Password, and upload an Avatar image.
- **Frontend**: Ensure the Username field is displayed as read-only.
- **Backend**: Implement or update APIs to support retrieving the current user's profile and updating profile details.

## Capabilities

### New Capabilities
- `user-profile-management`: Handles the retrieval and updating of a user's personal profile information, including avatar management.

### Modified Capabilities
None.

## Impact

- **Database**: Schema change for the `Users` table.
- **Backend**: Modifications to the `User` model and the addition/update of profile-related API endpoints.
- **Frontend**: Changes to the navigation/dropdown components and the addition of a new Profile Settings view.
