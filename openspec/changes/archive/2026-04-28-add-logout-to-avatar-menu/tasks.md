## 1. UI Implementation

- [x] 1.1 Add `isOpen` state to `TopNavBar` to manage avatar menu visibility
- [x] 1.2 Update avatar `div` to trigger `setIsOpen(!isOpen)` on click
- [x] 1.3 Implement the dropdown menu UI with a "Logout" item, styled with Tailwind CSS
- [x] 1.4 Add a click-away listener to close the menu when clicking outside the avatar area

## 2. Logout Functionality

- [x] 2.1 Import `authApi` into `TopNavBar.tsx`
- [x] 2.2 Implement the `handleLogout` function that calls `authApi.logout()` and performs a `window.location.reload()`
- [x] 2.3 Connect the "Logout" menu item to the `handleLogout` function

## 3. Verification

- [x] 3.1 Verify that clicking the avatar opens the menu
- [x] 3.2 Verify that clicking outside the menu closes it
- [x] 3.3 Verify that clicking "Logout" clears tokens and reloads the page
- [x] 3.4 Verify that the user is in an unauthenticated state after reload
