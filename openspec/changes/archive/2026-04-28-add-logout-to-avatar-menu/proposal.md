## Why

Users need a way to securely log out of their accounts, especially when using shared devices or when they wish to switch user accounts. Currently, there is no visible logout option in the user interface.

## What Changes

- **User Interface**:
  - Update the profile avatar in the `TopNavBar` to act as a trigger for a dropdown menu.
  - Implement a dropdown menu containing a "Logout" option.
  - Ensure the menu is styled consistently with the existing design system (using Tailwind CSS).
- **Functionality**:
  - Integrate the `authApi.logout()` function to clear authentication tokens from storage upon clicking "Logout".
  - Trigger a redirection to the login screen or reopen the `AuthModal` after successful logout to ensure the user is returned to an unauthenticated state.

## Capabilities

### New Capabilities
- `user-logout`: Handles the process of terminating the user session by clearing local authentication tokens and updating the UI state.

### Modified Capabilities
- None

## Impact

- **Components**: `src/extension/src/components/layout/TopNavBar.tsx` will be modified to include the avatar menu.
- **API**: `src/extension/src/api/auth.ts` will be utilized for the logout logic.
- **State**: The application's authentication state will be reset, affecting all components that depend on the user's login status.
