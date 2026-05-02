## Context

The application currently utilizes a static base URL for all API communications. This prevents the application from being used across different environments (e.g., development, staging, and production) or with self-hosted instances. As a Chrome extension, the application has access to `chrome.storage.local`, which provides a persistent way to store user preferences.

## Goals / Non-Goals

**Goals:**
- Allow users to specify the API host URL on the login page.
- Ensure the specified host URL is used for the authentication request.
- Persist the host URL in local storage upon successful login.
- Dynamically load the host URL from storage for all subsequent API requests.
- Maintain the host URL in storage during logout to facilitate easier re-login.

**Non-Goals:**
- Supporting multiple saved environments/profiles for switching.
- Automatic discovery or detection of the host environment.
- Implementing a complex configuration management system.

## Decisions

- **Storage Mechanism**: Use `chrome.storage.local` to store the `api_host_url`. This ensures persistence across extension restarts and browser sessions.
- **API Client Integration**: Modify the API request utility (e.g., an Axios instance or a fetch wrapper) to resolve the base URL dynamically. The client will attempt to retrieve the URL from storage before executing a request.
- **UI Validation**: Implement basic URL validation on the login page to ensure the input starts with `http://` or `https://` before allowing the login attempt.
- **Persistence Logic**: The `api_host_url` will be written to storage immediately after a successful 200 OK response from the login API.

## Risks / Trade-offs

- **[Risk] Invalid Host URL**: Users may enter a malformed URL or an unreachable host.
  - **Mitigation**: Implement frontend validation. Provide a clear error message if the API call fails due to a connection error (DNS failure, etc.), prompting the user to check the host URL.
- **[Risk] Race Condition on Init**: API calls might be triggered before the host URL is retrieved from asynchronous storage.
  - **Mitigation**: Implement an initialization check in the API client that ensures the host URL is loaded before the first request is sent.
