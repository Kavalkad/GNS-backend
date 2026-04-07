using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGamingPlaces");

            migrationBuilder.AddColumn<Guid>(
                name: "GamingPlaceEntityId",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OnPc",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnPlayStation",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnXbox",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "OnPc",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OnPlayStation",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OnXbox",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameGamingPlaces",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GamingPlaceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGamingPlaces", x => new { x.GameId, x.GamingPlaceId });
                    table.ForeignKey(
                        name: "FK_GameGamingPlaces_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGamingPlaces_GamingPlaces_GamingPlaceId",
                        column: x => x.GamingPlaceId,
                        principalTable: "GamingPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGamingPlaces_GamingPlaceId",
                table: "GameGamingPlaces",
                column: "GamingPlaceId");
        }
    }
}
