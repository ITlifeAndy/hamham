CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "UserPreferences" (
    "Id" uuid NOT NULL,
    "UsersId" uuid NOT NULL,
    "WallpaperSource" integer NOT NULL,
    "WallpaperKeywords" text[] NOT NULL,
    "RotationInterval" integer NOT NULL,
    "IsWallpaperLocked" boolean NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_UserPreferences" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserPreferences_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_UserPreferences_UsersId" ON "UserPreferences" ("UsersId");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260426022343_InitialCreate', '8.0.0');

COMMIT;

START TRANSACTION;

CREATE TABLE "IconLibrary" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "FilePath" text NOT NULL,
    "Category" text NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_IconLibrary" PRIMARY KEY ("Id")
);

CREATE TABLE "Categories" (
    "Id" uuid NOT NULL,
    "UsersId" uuid NOT NULL,
    "CategoriesId" uuid,
    "IconLibraryId" uuid,
    "Name" text NOT NULL,
    "Color" text NOT NULL,
    "SortOrder" integer NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Categories_Categories_CategoriesId" FOREIGN KEY ("CategoriesId") REFERENCES "Categories" ("Id"),
    CONSTRAINT "FK_Categories_IconLibrary_IconLibraryId" FOREIGN KEY ("IconLibraryId") REFERENCES "IconLibrary" ("Id"),
    CONSTRAINT "FK_Categories_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "SharedPools" (
    "Id" uuid NOT NULL,
    "UsersId" uuid NOT NULL,
    "IconLibraryId" uuid,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "IsPublic" boolean NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_SharedPools" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_SharedPools_IconLibrary_IconLibraryId" FOREIGN KEY ("IconLibraryId") REFERENCES "IconLibrary" ("Id"),
    CONSTRAINT "FK_SharedPools_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Bookmarks" (
    "Id" uuid NOT NULL,
    "UsersId" uuid NOT NULL,
    "CategoriesId" uuid NOT NULL,
    "IconLibraryId" uuid,
    "Title" text NOT NULL,
    "Url" text NOT NULL,
    "FaviconUrl" text NOT NULL,
    "IsFavorite" boolean NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_Bookmarks" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Bookmarks_Categories_CategoriesId" FOREIGN KEY ("CategoriesId") REFERENCES "Categories" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Bookmarks_IconLibrary_IconLibraryId" FOREIGN KEY ("IconLibraryId") REFERENCES "IconLibrary" ("Id"),
    CONSTRAINT "FK_Bookmarks_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "SharedPoolBookmarks" (
    "Id" uuid NOT NULL,
    "SharedPoolsId" uuid NOT NULL,
    "IconLibraryId" uuid,
    "BookmarkTitle" text NOT NULL,
    "BookmarkUrl" text NOT NULL,
    "CategoryId" uuid,
    "CreationTime" timestamp with time zone NOT NULL,
    "CreatorUser" uuid NOT NULL,
    "LastModifyTime" timestamp with time zone NOT NULL,
    "LastModifyUser" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeleteUser" uuid,
    "DeleteTime" timestamp with time zone,
    CONSTRAINT "PK_SharedPoolBookmarks" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_SharedPoolBookmarks_IconLibrary_IconLibraryId" FOREIGN KEY ("IconLibraryId") REFERENCES "IconLibrary" ("Id"),
    CONSTRAINT "FK_SharedPoolBookmarks_SharedPools_SharedPoolsId" FOREIGN KEY ("SharedPoolsId") REFERENCES "SharedPools" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Bookmarks_CategoriesId" ON "Bookmarks" ("CategoriesId");

CREATE INDEX "IX_Bookmarks_IconLibraryId" ON "Bookmarks" ("IconLibraryId");

CREATE INDEX "IX_Bookmarks_UsersId" ON "Bookmarks" ("UsersId");

CREATE INDEX "IX_Categories_CategoriesId" ON "Categories" ("CategoriesId");

CREATE INDEX "IX_Categories_IconLibraryId" ON "Categories" ("IconLibraryId");

CREATE INDEX "IX_Categories_UsersId" ON "Categories" ("UsersId");

CREATE INDEX "IX_SharedPoolBookmarks_IconLibraryId" ON "SharedPoolBookmarks" ("IconLibraryId");

CREATE INDEX "IX_SharedPoolBookmarks_SharedPoolsId" ON "SharedPoolBookmarks" ("SharedPoolsId");

CREATE INDEX "IX_SharedPools_IconLibraryId" ON "SharedPools" ("IconLibraryId");

CREATE INDEX "IX_SharedPools_UsersId" ON "SharedPools" ("UsersId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260426022849_AddBookmarkSystem', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Bookmarks" DROP CONSTRAINT "FK_Bookmarks_IconLibrary_IconLibraryId";

ALTER TABLE "Categories" DROP CONSTRAINT "FK_Categories_IconLibrary_IconLibraryId";

ALTER TABLE "SharedPoolBookmarks" DROP CONSTRAINT "FK_SharedPoolBookmarks_IconLibrary_IconLibraryId";

ALTER TABLE "SharedPools" DROP CONSTRAINT "FK_SharedPools_IconLibrary_IconLibraryId";

DROP INDEX "IX_SharedPools_IconLibraryId";

DROP INDEX "IX_SharedPoolBookmarks_IconLibraryId";

DROP INDEX "IX_Categories_IconLibraryId";

DROP INDEX "IX_Bookmarks_IconLibraryId";

ALTER TABLE "SharedPools" DROP COLUMN "IconLibraryId";

ALTER TABLE "SharedPoolBookmarks" DROP COLUMN "IconLibraryId";

ALTER TABLE "Categories" DROP COLUMN "IconLibraryId";

ALTER TABLE "Bookmarks" DROP COLUMN "IconLibraryId";

ALTER TABLE "SharedPools" ADD "Icon" text;

ALTER TABLE "SharedPoolBookmarks" ADD "Icon" text;

ALTER TABLE "Categories" ADD "Icon" text;

ALTER TABLE "Bookmarks" ADD "Color" text;

ALTER TABLE "Bookmarks" ADD "Icon" text;

ALTER TABLE "Bookmarks" ADD "SortOrder" integer NOT NULL DEFAULT 0;

ALTER TABLE "Bookmarks" ADD "Subtitle" text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260427145511_UpdateBookmarkAndCategorySchema', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Users" ADD "Name" text NOT NULL DEFAULT '';

UPDATE "Categories" SET "Icon" = '' WHERE "Icon" IS NULL;
ALTER TABLE "Categories" ALTER COLUMN "Icon" SET NOT NULL;
ALTER TABLE "Categories" ALTER COLUMN "Icon" SET DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260428134843_FixCategoryEntity', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "UserPreferences" ADD "WallpaperType" integer NOT NULL DEFAULT 0;

ALTER TABLE "UserPreferences" ADD "WallpaperValue" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260429153211_AddWallpaperSettings', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "UserPreferences" ADD "WallpaperLastUpdated" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260429153602_AddWallpaperLastUpdated', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Users" ADD "Avatar" text;

ALTER TABLE "Users" ADD "LastSyncTime" timestamp with time zone;

ALTER TABLE "UserPreferences" ADD "OverlayOpacity" double precision NOT NULL DEFAULT 0.0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260501125656_AddUserAvatarAndSyncTime', '8.0.0');

COMMIT;

START TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260501131331_AddUserAvatarAndSyncTime_V2', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "SharedPools" DROP CONSTRAINT "FK_SharedPools_Users_UsersId";

DROP INDEX "IX_SharedPools_UsersId";

DROP INDEX "IX_SharedPoolBookmarks_SharedPoolsId";

ALTER TABLE "SharedPools" DROP COLUMN "UsersId";

ALTER TABLE "SharedPoolBookmarks" RENAME COLUMN "BookmarkUrl" TO "Url";

ALTER TABLE "SharedPoolBookmarks" RENAME COLUMN "BookmarkTitle" TO "Name";

CREATE UNIQUE INDEX "IX_SharedPoolBookmarks_SharedPoolsId_Url" ON "SharedPoolBookmarks" ("SharedPoolsId", "Url");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260501164957_RemoveUsersIdFromSharedPools', '8.0.0');

COMMIT;

