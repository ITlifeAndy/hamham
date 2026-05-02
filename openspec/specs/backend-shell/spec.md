# Backend Shell Spec

## Purpose
TBD

## Requirements

### Requirement: Backend Layout Structure
The system SHALL provide a dedicated layout for the administrative area containing a side navigation bar and a main content area.

#### Scenario: Navigating between admin modules
- **WHEN** the administrator clicks on a module link in the side navigation (e.g., "User Management")
- **THEN** the main content area SHALL render the corresponding module's view without refreshing the entire page.

### Requirement: Side Navigation Menu
The side navigation SHALL include links to at least "User Management" and "Public Bookmark Library".

#### Scenario: Menu visibility
- **WHEN** an admin page is loaded
- **THEN** the side navigation SHALL be visible on desktop views and converted to a bottom navigation or hamburger menu on mobile views.

### Requirement: Admin Page Header
Each admin page SHALL have a header containing the page title and a primary action button (e.g., "Add User").

#### Scenario: Action button visibility
- **WHEN** the User Management page is active
- **THEN** the header SHALL display "使用者管理" and a "person_add" button.
