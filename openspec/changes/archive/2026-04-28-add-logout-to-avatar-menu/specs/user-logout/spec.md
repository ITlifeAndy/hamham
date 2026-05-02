## ADDED Requirements

### Requirement: Logout via Avatar Menu
The system SHALL provide a logout option in a menu that appears when the user clicks on their profile avatar in the `TopNavBar`.

#### Scenario: Open logout menu
- **WHEN** the user clicks the profile avatar in the `TopNavBar`
- **THEN** a dropdown menu appears containing a "Logout" option

#### Scenario: Trigger logout
- **WHEN** the user clicks the "Logout" option in the avatar menu
- **THEN** the system calls the logout API to clear the session tokens from local storage
- **THEN** the system redirects the user to the login screen or displays the `AuthModal`
- **THEN** the avatar menu is closed
