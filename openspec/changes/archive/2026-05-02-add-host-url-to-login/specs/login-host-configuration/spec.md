## ADDED Requirements

### Requirement: Host URL Specification
The system SHALL provide a mandatory input field on the login page for the user to enter the API Host URL.

#### Scenario: Successful Host URL entry
- **WHEN** the user enters a valid URL (e.g., `https://api.hamham.com`)
- **THEN** the system allows the user to proceed with the login attempt

#### Scenario: Missing Host URL
- **WHEN** the user leaves the Host URL field empty and attempts to login
- **THEN** the system displays a validation error "Host URL is required" and prevents the API call

#### Scenario: Invalid Host URL format
- **WHEN** the user enters a string that is not a valid URL (e.g., `not-a-url`)
- **THEN** the system displays a validation error "Please enter a valid URL (starting with http:// or https://)"

### Requirement: Host URL Persistence
The system SHALL persist the Host URL used for a successful login in local storage.

#### Scenario: Save Host URL after login
- **WHEN** the user successfully authenticates with a specific Host URL
- **THEN** the system saves this URL to `chrome.storage.local` under the key `api_host_url`

#### Scenario: Pre-fill Host URL on login page
- **WHEN** the user navigates to the login page and a Host URL exists in local storage
- **THEN** the system pre-fills the Host URL input field with the stored value

#### Scenario: Persist Host URL after logout
- **WHEN** the user logs out of the system
- **THEN** the system removes the authentication tokens but retains the `api_host_url` in local storage
