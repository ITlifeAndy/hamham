## Why

When a new system is installed, no user accounts exist by default, which prevents any user from logging in to perform initial configuration. A default admin account is necessary to allow immediate access to the system for setup and administrative tasks.

## What Changes

- Add a check during system startup or during the first authentication request to verify if any users exist in the database.
- If no users are found, automatically create a default administrative account.
- Default credentials:
  - Username: `admin`
  - Password: `hamham`
- Ensure the creation process is idempotent and thread-safe to avoid duplicate accounts in concurrent environments.

## Capabilities

### New Capabilities
- `default-admin-init`: Handles the detection of an empty user database and the subsequent creation of the default administrator account.

### Modified Capabilities
- None

## Impact

- **Database**: `Users` table will be populated with the default admin account.
- **Backend API**: The user creation logic will be triggered during the initialization phase.
- **Authentication Flow**: The first user to access the system will trigger the admin account creation if the database is empty.
