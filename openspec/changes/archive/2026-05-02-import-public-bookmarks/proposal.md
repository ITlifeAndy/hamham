## Why

Users currently have to manually copy bookmarks from the public library. This feature allows users to efficiently discover and save multiple public bookmarks into their personalized collection, enhancing the value of the shared library.

## What Changes

- **UI Entry Point**: Add an "Import Public Bookmarks" option to the user information dropdown menu in the top-right corner.
- **Import Interface**: A new interface allowing users to browse the public bookmark library and multi-select bookmarks for import.
- **Target Assignment**: A mechanism to select an existing personal category or sub-category as the destination for the imported bookmarks.
- **In-line Category Creation**: Ability to create new categories or sub-categories directly within the import flow without leaving the page.
- **Import Logic**: Implementation of "cloning" where the name, subtitle, and URL are copied to the user's personal bookmarks.
- **Customization**: Allow users to choose the color and glass effect for the imported bookmarks during the import process.

## Capabilities

### New Capabilities
- `public-bookmark-import`: Covers the UI and logic for browsing, multi-selecting public bookmarks, and cloning them to a personal account.
- `import-category-selector`: Covers the logic for selecting target categories and the capability to create new categories/sub-categories on-the-fly during import.

### Modified Capabilities
- `shared-library`: Modification to the public library view to support multi-selection mode.

## Impact

- **Frontend**:
    - User info dropdown component.
    - New `PublicBookmarkImport` page component.
    - Modification to the Public Library UI to support selection.
    - Category selection and creation modals/components.
- **Backend**:
    - New API endpoint (or modification of existing ones) to handle the bulk cloning of bookmarks from the public library to a specific user's category.
- **Database**:
    - Increased entries in the personal bookmarks table.
