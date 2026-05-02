## Why

Currently, the application uses a fixed API endpoint for authentication and data requests. To support different environments (development, staging, production) or self-hosted instances, users must be able to specify the API host URL during login.

## What Changes

- Add a mandatory Host URL text box to the login page.
- Update the login request to use the host URL specified in the input field.
- Save the successfully used Host URL to local storage (e.g., `chrome.storage.local`) after a successful login.
- Configure the API client to dynamically use the stored Host URL for all subsequent API calls.
- Modify the logout process to persist the Host URL in local storage.
- Pre-fill the Host URL field on the login page using the value stored in local storage.

## Capabilities

### New Capabilities
- `login-host-configuration`: Capability to define, store, and utilize the API host URL for application connectivity.

### Modified Capabilities
- `user-authentication`: Update authentication flow to incorporate the dynamic host URL.

## Impact

- **UI**: Login page needs a new input field and validation logic.
- **API Client**: The base URL logic must change from a static constant to a dynamic value retrieved from storage.
- **Storage**: New key-value pair for `api_host_url` in local storage.
- **Auth Flow**: Login and logout sequences must handle host URL persistence.
