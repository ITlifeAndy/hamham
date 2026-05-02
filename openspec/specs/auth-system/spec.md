# Specification: Auth System

## Overview
The `auth-system` provides a secure way to manage user identities, control access to the application, and allow administrators to manage user account status. It serves as the primary security layer for all other capabilities.

## Requirements

### 1. User Authentication
- **Registration**: Users must be able to create an account using a unique email/username and a secure password.
- **Login**: Users must be able to authenticate using their credentials.
- **Session Management**: 
    - Use JSON Web Tokens (JWT) for stateless authentication.
    - Implement access tokens (short-lived) and refresh tokens (long-lived).
    - Tokens must be securely transmitted and stored on the client side.
- **Logout**: Invalidate the current session/token on the client and server side (where applicable).

### 2. Authorization & Role-Based Access Control (RBAC)
- **Roles**:
    - `User`: Standard access to personal bookmark and wallpaper features.
    - `Admin`: All `User` permissions plus access to the User Management dashboard.
- **Permission Checks**: Every API request requiring authentication must verify the JWT and the user's role.

### 3. Administrative Controls
- **User List**: Administrators must be able to view a list of all registered users.
- **Account Status Control**: Administrators must be able to toggle the `IsActive` status of any user account.
- **Access Denial**: If a user's `IsActive` status is set to `false`, they must be immediately blocked from accessing the system, even if they have a valid JWT (via middleware check).
- **Role Assignment**: Administrators must be able to promote or demote users between `User` and `Admin` roles.

### 4. Security Requirements
- **Password Storage**: Passwords must never be stored in plain text. Use a strong salted hashing algorithm (e.g., BCrypt or Argon2).
- **Input Validation**: All authentication inputs must be validated for length, format, and common injection patterns.
- **Token Expiration**: Implement a strict expiration policy for JWTs to minimize the window of opportunity for stolen tokens.

## Acceptance Criteria
- A new user can register and login successfully.
- A user with a valid JWT can access their own bookmarks but not the admin panel.
- An admin can disable a user's account, and that user is immediately locked out of the system.
- An admin can promote a user to admin status.
- Attempting to login with incorrect credentials returns a generic "Invalid credentials" error.
