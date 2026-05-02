## Context

The `TopNavBar` component currently displays a user profile avatar but lacks any interactivity. Users are authenticated via tokens stored in local storage, managed by the `authApi` service. There is currently no mechanism for users to log out from the user interface.

## Goals / Non-Goals

**Goals:**
- Provide a visual and interactive way for users to log out via the profile avatar.
- Ensure that session tokens are correctly removed from storage upon logout.
- Return the user to an unauthenticated state (e.g., showing the login modal or redirecting to a login page).

**Non-Goals:**
- implementing a full user profile page (out of scope for this change).
- adding more options to the avatar menu (e.g., "Account Settings") at this time.

## Decisions

- **UI Implementation**: 
  - A local `isOpen` state will be added to `TopNavBar` to toggle the visibility of a dropdown menu.
  - The dropdown will be positioned absolutely relative to the avatar container.
  - Styling will be handled with Tailwind CSS to match the existing "glassmorphism" and clean aesthetic of the `TopNavBar`.
- **Logout Logic**: 
  - The "Logout" menu item will call `authApi.logout()`.
  - After logout, `window.location.reload()` will be used as a simple and effective way to reset the entire application state and ensure all authenticated views are updated. This avoids the need for a complex global state update across all components in the current architecture.
- **User Experience**: 
  - Added a transition for the menu appearance for a polished feel.
  - Added a click-away listener to close the menu when clicking outside.

## Risks / Trade-offs

- **Hard Reload**: Using `window.location.reload()` is a "brute force" approach to state reset. While effective, it causes a full page flicker. 
  - *Mitigation*: This is acceptable for a logout action where a clear transition to an unauthenticated state is expected.
- **Z-Index Conflicts**: The dropdown menu must be carefully positioned to avoid being hidden by other elements or overlapping important UI.
  - *Mitigation*: Set a high `z-index` and use `absolute` positioning relative to the avatar's parent container.
