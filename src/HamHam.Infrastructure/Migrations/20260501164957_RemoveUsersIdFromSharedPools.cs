using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamHam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUsersIdFromSharedPools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedPools_Users_UsersId",
                table: "SharedPools");

            migrationBuilder.DropIndex(
                name: "IX_SharedPools_UsersId",
                table: "SharedPools");

            migrationBuilder.DropIndex(
                name: "IX_SharedPoolBookmarks_SharedPoolsId",
                table: "SharedPoolBookmarks");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "SharedPools");

            migrationBuilder.RenameColumn(
                name: "BookmarkUrl",
                table: "SharedPoolBookmarks",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "BookmarkTitle",
                table: "SharedPoolBookmarks",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPoolBookmarks_SharedPoolsId_Url",
                table: "SharedPoolBookmarks",
                columns: new[] { "SharedPoolsId", "Url" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SharedPoolBookmarks_SharedPoolsId_Url",
                table: "SharedPoolBookmarks");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "SharedPoolBookmarks",
                newName: "BookmarkUrl");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SharedPoolBookmarks",
                newName: "BookmarkTitle");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId",
                table: "SharedPools",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SharedPools_UsersId",
                table: "SharedPools",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPoolBookmarks_SharedPoolsId",
                table: "SharedPoolBookmarks",
                column: "SharedPoolsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedPools_Users_UsersId",
                table: "SharedPools",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
