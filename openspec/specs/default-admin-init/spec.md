# Default Admin Initialization

## Purpose
Ensures that the system always has an administrative account available for initial configuration.

## Requirements

### Requirement: Default Admin Account Initialization
The system SHALL check for the existence of any user accounts upon startup or upon the first authentication request. If no users are found in the database, the system MUST automatically create a default administrative account.

#### Scenario: First-time system launch with no users
- **WHEN** the system is started for the first time and the Users table is empty
- **THEN** the system creates a user with username `admin` and password `hamham` and assigns the `Admin` role.

#### Scenario: System launch with existing users
- **WHEN** the system is started and one or more users already exist in the database
- **THEN** the system does not create any additional default accounts.

#### Scenario: Concurrent initialization requests
- **WHEN** multiple simultaneous requests trigger the initialization check while the database is empty
- **THEN** the system ensures that only one default admin account is created.
