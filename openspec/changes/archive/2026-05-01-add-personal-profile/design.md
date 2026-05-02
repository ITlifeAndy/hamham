## Context

The HamHam system currently provides administrative user management but lacks a self-service profile management feature. Users are unable to update their personal details, such as their display name, email, or profile picture. The system consists of a .NET backend API and a React-based extension frontend.

## Goals / Non-Goals

**Goals:**
- Provide a user-facing interface to edit Name, Email, and Password.
- Implement avatar upload and display functionality.
- Ensure the Username field is read-only to maintain system identity consistency.
- Update the database schema to support avatar storage and synchronization tracking.

**Non-Goals:**
- Implementing a complex email verification flow for email changes.
- Adding multi-factor authentication (MFA) during password changes.
- Creating a public-facing profile page.

## Decisions

### 1. Database Schema Update
The `Users` table will be extended with two new columns:
- `Avatar` (nvarchar(max)): Stores the URL or file path to the user's profile image.
- `LastSyncTime` (datetimeoffset): Tracks the last time the user's profile was synchronized.
*Rationale*: These additions are minimal and support the required functionality without requiring new tables.

### 2. Avatar Storage Strategy
Avatars will be uploaded to a dedicated `uploads/avatars` directory on the server, and the resulting path will be stored in the database.
*Rationale*: Simple to implement and efficient for small-to-medium scale storage.

### 3. API Endpoint Design
A new `PUT /api/users/profile` endpoint will be implemented.
- **Auth**: Required (JWT).
- **Payload**: `{ "name": string, "email": string, "password": string (optional), "avatar": file/url }`
- **Behavior**: The server will resolve the user identity from the token rather than trusting a user ID in the request body.
*Rationale*: Ensures security and prevents users from updating other users' profiles.

### 4. Frontend Implementation
A "Settings" option will be added to the user avatar dropdown. This will trigger a `ProfileSettingsModal` containing a form with the specified fields.
*Rationale*: Modals provide a lightweight way to handle settings without navigating away from the current page.

## Risks / Trade-offs

- **[File Upload Security]** $\rightarrow$ Mitigation: Implement strict file type validation (e.g., only .jpg, .png, .webp) and size limits (e.g., 2MB) on the backend.
- **[Password Security]** $\rightarrow$ Mitigation: Utilize the existing `BCrypt.Net-Next` implementation for hashing new passwords.
- **[Avatar Pathing]** $\rightarrow$ Mitigation: Store relative paths in the DB and resolve the full URL in the API layer to avoid issues if the server base URL changes.
