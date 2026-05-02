using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamHam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookmarkAndCategorySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_IconLibrary_IconLibraryId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_IconLibrary_IconLibraryId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedPoolBookmarks_IconLibrary_IconLibraryId",
                table: "SharedPoolBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedPools_IconLibrary_IconLibraryId",
                table: "SharedPools");

            migrationBuilder.DropIndex(
                name: "IX_SharedPools_IconLibraryId",
                table: "SharedPools");

            migrationBuilder.DropIndex(
                name: "IX_SharedPoolBookmarks_IconLibraryId",
                table: "SharedPoolBookmarks");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IconLibraryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Bookmarks_IconLibraryId",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "IconLibraryId",
                table: "SharedPools");

            migrationBuilder.DropColumn(
                name: "IconLibraryId",
                table: "SharedPoolBookmarks");

            migrationBuilder.DropColumn(
                name: "IconLibraryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IconLibraryId",
                table: "Bookmarks");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "SharedPools",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "SharedPoolBookmarks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Bookmarks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Bookmarks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Bookmarks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "Bookmarks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "SharedPools");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "SharedPoolBookmarks");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "Bookmarks");

            migrationBuilder.AddColumn<Guid>(
                name: "IconLibraryId",
                table: "SharedPools",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IconLibraryId",
                table: "SharedPoolBookmarks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IconLibraryId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IconLibraryId",
                table: "Bookmarks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedPools_IconLibraryId",
                table: "SharedPools",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPoolBookmarks_IconLibraryId",
                table: "SharedPoolBookmarks",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IconLibraryId",
                table: "Categories",
                column: "IconLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_IconLibraryId",
                table: "Bookmarks",
                column: "IconLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_IconLibrary_IconLibraryId",
                table: "Bookmarks",
                column: "IconLibraryId",
                principalTable: "IconLibrary",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_IconLibrary_IconLibraryId",
                table: "Categories",
                column: "IconLibraryId",
                principalTable: "IconLibrary",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedPoolBookmarks_IconLibrary_IconLibraryId",
                table: "SharedPoolBookmarks",
                column: "IconLibraryId",
                principalTable: "IconLibrary",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedPools_IconLibrary_IconLibraryId",
                table: "SharedPools",
                column: "IconLibraryId",
                principalTable: "IconLibrary",
                principalColumn: "Id");
        }
    }
}
