# Public Bookmark Management

## Purpose
TBD

## Requirements

### Requirement: Public Bookmark Pool Management
The system SHALL allow administrators to manage bookmark pools (categories).

#### Scenario: Create new pool
- **WHEN** administrator creates a new pool with a unique name
- **THEN** system saves the pool to the `SharePools` table

#### Scenario: Delete pool
- **WHEN** administrator deletes a pool
- **THEN** system removes the pool and all associated bookmarks from `SharePoolBookMarks`

### Requirement: Public Bookmark Entry Management
The system SHALL allow administrators to manage individual bookmark entries within a pool.

#### Scenario: Add bookmark to pool
- **WHEN** administrator adds a bookmark (Name, URL) to a specific pool
- **THEN** system saves the entry to `SharePoolBookMarks` associated with the pool ID

#### Scenario: Update bookmark entry
- **WHEN** administrator updates the name or URL of an existing public bookmark
- **THEN** system persists the changes in `SharePoolBookMarks`

### Requirement: Public Bookmark Retrieval and Filtering
The system MUST provide a way to retrieve public bookmarks with optional filtering.

#### Scenario: List all public bookmarks
- **WHEN** requested without filter
- **THEN** system returns all bookmarks from `SharePoolBookMarks` including their category name from `SharePools`

#### Scenario: Filter bookmarks by category
- **WHEN** a specific category is selected as a filter
- **THEN** system returns only bookmarks belonging to that category

### Requirement: Management UI Display
The admin interface SHALL provide a dedicated page for managing public bookmarks.

#### Scenario: Table data display
- **WHEN** the Public Bookmark Management page is loaded
- **THEN** the system displays a table with the following columns: "Category", "Name", and "URL"

#### Scenario: User Management design alignment
- **WHEN** viewing the page
- **THEN** the layout, colors, and table styling must match the existing User Management interface
