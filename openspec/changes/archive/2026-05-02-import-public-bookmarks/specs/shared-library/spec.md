## MODIFIED Requirements

### 2. The "Picking" Mechanism (Selective Acquisition)
- **Discovery**: Users can browse the content of shared pools without these bookmarks being added to their personal account.
- **Bulk Selective Pick**: Users SHALL be able to multi-select individual bookmarks or entire categories from a shared pool and "pick" them in bulk into their personal library.
- **Import Logic**: When bookmarks are "picked":
    - A copy of the bookmark metadata (Title, Subtitle, URL) is created in the user's personal bookmark table.
    - The user SHALL be prompted to choose a personal category/folder for the batch, with the option to create a new one.
    - The user SHALL be allowed to specify visual properties (color, glass effect) for the picked items.
- **No Automatic Sync**: Adding a bookmark to a shared pool does NOT automatically add it to all users' personal libraries.

#### Scenario: Bulk picking bookmarks
- **WHEN** a user selects 10 bookmarks from the "Developer Essentials" pool
- **THEN** the system SHALL allow them to pick all 10 in a single operation.
- **WHEN** the user selects a destination folder and confirms
- **THEN** all 10 bookmarks SHALL be cloned into the user's personal account.
