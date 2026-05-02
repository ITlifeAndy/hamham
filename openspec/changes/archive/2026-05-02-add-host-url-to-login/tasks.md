## 1. Storage and API Client Infrastructure

- [x] 1.1 Implement logic to retrieve `api_host_url` from `chrome.storage.local` in the API client.
- [x] 1.2 Update the API client to use the retrieved `api_host_url` as the base URL for all requests.
- [x] 1.3 Add a fallback or initialization check to handle cases where no host URL is stored yet.

## 2. Login Page UI Enhancements

- [x] 2.1 Add a mandatory Host URL text input field to the login page.
- [x] 2.2 Implement frontend validation to ensure the Host URL is a valid URL (starts with http/https).
- [x] 2.3 Implement logic to pre-fill the Host URL field from `chrome.storage.local` on page load.

## 3. Authentication Flow Integration

- [x] 3.1 Modify the login function to use the host URL provided in the UI field for the authentication API call.
- [x] 3.2 Implement logic to save the Host URL to `chrome.storage.local` immediately after a successful login response.
- [x] 3.3 Verify that the logout process clears auth tokens but preserves the `api_host_url` in storage.

## 4. Verification and Testing

- [x] 4.1 Verify that login fails with a clear error when the Host URL is missing or invalid.
- [x] 4.2 Verify that the application correctly communicates with a specified host.
- [x] 4.3 Verify that the Host URL persists across browser restarts.
- [x] 4.4 Verify that the Host URL is pre-filled correctly after logging out and returning to the login page.
