using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamHam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookmarkSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IconLibrary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorUser = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifyUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteUser = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IconLibrary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: true),
                    IconLibraryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorUser = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifyUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteUser = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_IconLibrary_IconLibraryId",
                        column: x => x.IconLibraryId,
                        principalTable: "IconLibrary",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedPools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false),
                    IconLibraryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorUser = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifyUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteUser = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedPools_IconLibrary_IconLibraryId",
                        column: x => x.IconLibraryId,
                        principalTable: "IconLibrary",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedPools_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    IconLibraryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FaviconUrl = table.Column<string>(type: "text", nullable: false),
                    IsFavorite = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorUser = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifyUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteUser = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookmarks_IconLibrary_IconLibraryId",
                        column: x => x.IconLibraryId,
                        principalTable: "IconLibrary",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookmarks_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedPoolBookmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SharedPoolsId = table.Column<Guid>(type: "uuid", nullable: false),
                    IconLibraryId = table.Column<Guid>(type: "uuid", nullable: true),
                    BookmarkTitle = table.Column<string>(type: "text", nullable: false),
                    BookmarkUrl = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorUser = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifyUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteUser = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPoolBookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedPoolBookmarks_IconLibrary_IconLibraryId",
                        column: x => x.IconLibraryId,
                        principalTable: "IconLibrary",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedPoolBookmarks_SharedPools_SharedPoolsId",
                        column: x => x.SharedPoolsId,
                        principalTable: "SharedPools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_CategoriesId",
                table: "Bookmarks",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_IconLibraryId",
                table: "Bookmarks",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UsersId",
                table: "Bookmarks",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoriesId",
                table: "Categories",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IconLibraryId",
                table: "Categories",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UsersId",
                table: "Categories",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPoolBookmarks_IconLibraryId",
                table: "SharedPoolBookmarks",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPoolBookmarks_SharedPoolsId",
                table: "SharedPoolBookmarks",
                column: "SharedPoolsId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPools_IconLibraryId",
                table: "SharedPools",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPools_UsersId",
                table: "SharedPools",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "SharedPoolBookmarks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SharedPools");

            migrationBuilder.DropTable(
                name: "IconLibrary");
        }
    }
}
