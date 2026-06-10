using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringUselessProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GamingPlaces_GamingPlaceEntityId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GamingPlaceEntityId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GamingPlaceEntityId",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GamingPlaceEntityId",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GamingPlaceEntityId",
                table: "Games",
                column: "GamingPlaceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GamingPlaces_GamingPlaceEntityId",
                table: "Games",
                column: "GamingPlaceEntityId",
                principalTable: "GamingPlaces",
                principalColumn: "Id");
        }
    }
}
