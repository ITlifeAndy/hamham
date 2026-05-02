# Capability: Public Bookmark Import

## Purpose
TBD

## Requirements

### Requirement: Public Bookmark Multi-Selection
The system SHALL provide a mechanism to select multiple bookmarks from the public library for bulk import.

#### Scenario: Selecting multiple bookmarks
- **WHEN** the user is in the Public Library view and activates "Import Mode"
- **THEN** the system SHALL display checkboxes or selection indicators for each bookmark item.
- **WHEN** the user selects five different bookmarks
- **THEN** the system SHALL track these five IDs as the current import set.

### Requirement: Bookmark Cloning
The system SHALL create personal copies of selected public bookmarks, preserving the Name, Subtitle, and URL.

#### Scenario: Executing the import
- **WHEN** the user confirms the import of selected bookmarks
- **THEN** the system SHALL create new entries in the user's personal bookmark table for each selected item.
- **WHEN** the user modifies an imported bookmark
- **THEN** the original public bookmark SHALL remain unchanged.

### Requirement: Visual Property Customization
The system SHALL allow the user to specify the color and glass effect for the imported bookmarks during the import process.

#### Scenario: Customizing imported look
- **WHEN** the user is finalizing the import
- **THEN** the system SHALL provide options to choose a category color and whether to apply a glass effect.
- **WHEN** the user selects "Blue" and "Glass: On"
- **THEN** all imported bookmarks in that batch SHALL be created with those visual properties.
