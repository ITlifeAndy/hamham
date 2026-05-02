## ADDED Requirements

### Requirement: Admin Entry Point
The main user interface SHALL feature a settings icon in the top-right corner that allows access to the Backend Management System.

#### Scenario: Accessing admin system
- **WHEN** the user clicks the settings icon
- **THEN** the system SHALL navigate the user to the `/admin` route.

### Requirement: Admin Access Control
The system SHALL restrict access to the backend management area to users with an "Admin" role.

#### Scenario: Unauthorized access attempt
- **WHEN** a non-admin user attempts to access `/admin` directly via URL
- **THEN** the system SHALL redirect them to the main dashboard and show an access denied message.
