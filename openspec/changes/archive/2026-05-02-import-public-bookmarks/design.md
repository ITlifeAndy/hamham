## Context

The current system separates the Public Bookmark Library from Personal Bookmark Management. While users can browse the Public Library, there is no integrated way to "save" or "import" these bookmarks into their own personalized collection. Currently, users must manually copy details, which is a high-friction process.

## Goals / Non-Goals

**Goals:**
- Provide a direct entry point from the user profile menu to the import feature.
- Enable bulk selection of bookmarks from the Public Library.
- Allow flexible destination assignment, including the ability to create new categories/sub-categories on the fly.
- Permit customization of visual attributes (color, glass effect) for the cloned bookmarks.
- Ensure a "clone" operation so users can modify imported bookmarks without affecting the source.

**Non-Goals:**
- Establishing a live sync between the public source and the personal clone.
- Importing bookmarks from external URLs or files.
- Automatically organizing imported bookmarks based on tags.

## Decisions

### 1. Cloning over Linking
**Decision**: Implement a "clone" mechanism that copies Name, Subtitle, and URL.
**Rationale**: Users typically want to rename bookmarks or change their subtitles to fit their personal context. Linking would make them dependent on the public library's version and prevent personalization.

### 2. Dedicated Import Page
**Decision**: Use a dedicated page (`/admin/import-public` or similar) rather than a modal for the main import flow.
**Rationale**: The process involves browsing a large library, multi-selecting items, and managing category hierarchies. A dedicated page provides the necessary screen real estate for a usable UX.

### 3. Inline Category Creation
**Decision**: Integrate a "Quick Add" category UI within the target selection dropdown/component.
**Rationale**: Forcing users to navigate to a separate category management page to create a folder before importing creates significant friction. Inline creation keeps the user in the flow.

### 4. Bulk-Import API Endpoint
**Decision**: Create a specialized backend endpoint that accepts a list of public bookmark IDs, a target category ID, and visual preferences.
**Rationale**: Individual API calls for 20+ bookmarks would be inefficient and could lead to partial imports if the network fails mid-process. A single transactional request ensures atomicity.

## Risks / Trade-offs

- **[Risk]**: Category clutter caused by frequent "on-the-fly" creation. $\rightarrow$ **[Mitigation]**: Ensure the category creation UI is distinct and encourage selecting existing categories first.
- **[Risk]**: Potential for database bloat if users import massive amounts of public data. $\rightarrow$ **[Mitigation]**: Implement a reasonable limit on the number of bookmarks that can be imported in a single bulk operation.
- **[Risk]**: Visual inconsistency if users import many bookmarks with random colors. $\rightarrow$ **[Mitigation]**: Provide a set of suggested color presets during the import process.
