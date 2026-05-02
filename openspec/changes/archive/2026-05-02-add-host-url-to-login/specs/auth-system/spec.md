## MODIFIED Requirements

### Requirement: Login
The system SHALL authenticate users using their credentials and the specified Host URL.

#### Scenario: Successful login with custom host
- **WHEN** the user provides valid credentials and a reachable Host URL
- **THEN** the system sends the authentication request to the specified Host URL and grants access upon success

#### Scenario: Login failure due to unreachable host
- **WHEN** the user provides valid credentials but the Host URL is unreachable or invalid
- **THEN** the system returns a connection error message indicating the Host URL could not be reached
