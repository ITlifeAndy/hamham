# Capability: Import Category Selector

## Purpose
TBD

## Requirements

### Requirement: Target Category Selection
The system SHALL require the user to select a personal category or sub-category as the destination for imported bookmarks.

#### Scenario: Selecting existing category
- **WHEN** the user is prompted for a destination
- **THEN** the system SHALL display a searchable list of the user's current categories and sub-categories.
- **WHEN** the user selects "Work/Research"
- **THEN** the imported bookmarks SHALL be placed inside the "Research" sub-category of "Work".

### Requirement: On-the-Fly Category Creation
The system SHALL allow the user to create a new category or sub-category directly within the import flow.

#### Scenario: Creating new category during import
- **WHEN** the user cannot find a suitable category in the selector
- **THEN** the system SHALL provide an "Add New Category" option.
- **WHEN** the user enters "AI Tools 2026" and saves
- **THEN** the system SHALL create the category and automatically select it as the import destination.
