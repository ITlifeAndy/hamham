using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamHam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAvatarAndSyncTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastSyncTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OverlayOpacity",
                table: "UserPreferences",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastSyncTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OverlayOpacity",
                table: "UserPreferences");
        }
    }
}
