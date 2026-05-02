## 1. Frontend Entry & Navigation

- [x] 1.1 Add "Import Public Bookmarks" option to the user information dropdown menu.
- [x] 1.2 Create the `PublicBookmarkImport` page component and define its routing.
- [x] 1.3 Implement the page layout, providing a split view for the public library browsing and the import configuration panel.

## 2. Public Library Multi-Selection

- [x] 2.1 Update the Public Library UI to include a "Selection Mode" toggle.
- [x] 2.2 Add checkboxes or selection indicators to bookmark items in the public library.
- [x] 2.3 Implement state management to track the set of currently selected public bookmark IDs.

## 3. Destination & Category Management

- [x] 3.1 Create the `ImportCategorySelector` component for choosing the target personal category.
- [x] 3.2 Implement search and filtering within the category selector to handle large numbers of categories.
- [x] 3.3 Implement the "Quick Add" category UI to allow creating new categories/sub-categories during the import flow.
- [x] 3.4 Create the UI for selecting visual properties (Category Color, Glass Effect) for the imported bookmarks.

## 4. Backend Implementation

- [x] 4.1 Implement a `BookmarkImportService` in the backend to handle the cloning of bookmark metadata.
- [x] 4.2 Create a bulk-import API endpoint that accepts the list of public IDs, the target category ID, and the chosen visual properties.
- [x] 4.3 Add server-side validation to verify that the target category is valid and owned by the requesting user.

## 5. Integration & Verification

- [x] 5.1 Integrate the frontend import trigger with the bulk-import backend API.
- [x] 5.2 Verify the full end-to-end flow: selecting multiple bookmarks $\rightarrow$ choosing/creating a category $\rightarrow$ importing $\rightarrow$ confirming existence in personal library.
- [x] 5.3 Verify that visual properties (color/glass) are correctly applied to the cloned bookmarks.
- [x] 5.4 Run `npm run build` to ensure no frontend regressions occurred.
