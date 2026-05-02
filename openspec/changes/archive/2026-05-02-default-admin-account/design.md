## Context

The system currently has no mechanism to provide an initial administrative user. In a fresh installation, the `Users` table is empty, meaning no one can log in to the system to create other users or manage bookmarks.

## Goals / Non-Goals

**Goals:**
- Automatically create a default administrative user (`admin` / `hamham`) when the system is first installed and no users exist.
- Ensure the process is idempotent and thread-safe.
- Minimize performance impact on subsequent system startups.

**Non-Goals:**
- Implementing a comprehensive data seeding framework for all entities.
- Creating a complex set of default roles beyond the primary `Admin` role.

## Decisions

### Trigger Mechanism: Startup-based Initialization
The initialization check will be implemented as a startup task within the .NET API. 
- **Rationale**: By checking for users during the application startup sequence (e.g., using a dedicated seeding method called in `Program.cs`), we ensure that the admin account is available before any external requests are processed. This is more predictable than checking per-request.
- **Alternative Considered**: Checking during the first authentication request. While lazy, it adds overhead to the request and could lead to race conditions if multiple users attempt to log in simultaneously on a fresh install.

### Idempotency and Concurrency
The system will utilize the database's unique constraint on the `Username` column to prevent duplicate admin accounts.
- **Rationale**: Database-level constraints are the most reliable way to ensure uniqueness across multiple API instances in a scaled environment.
- **Implementation**: The seeding logic will attempt to find a user with the username `admin`. If not found, it will attempt to create one. If a `DbUpdateException` occurs due to a unique constraint violation (indicating another instance created the user), the exception will be caught and ignored.

## Risks / Trade-offs

- **[Security] Well-known Default Credentials** $\rightarrow$ The use of `admin`/`hamham` is a security risk. **Mitigation**: This is a development/initial setup convenience. The administrative user is expected to change the password immediately upon first login.
- **[Startup Overhead] Database query on every start** $\rightarrow$ A query is performed every time the API starts. **Mitigation**: The query is a simple `AnyAsync()` or `FirstOrDefaultAsync()` on the `Users` table, which is indexed and extremely fast.
