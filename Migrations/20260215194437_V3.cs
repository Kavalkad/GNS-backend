using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "CyberClubs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Owners_Employees_Id",
                        column: x => x.Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyberClubs_OwnerId",
                table: "CyberClubs",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CyberClubs_Owners_OwnerId",
                table: "CyberClubs",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyberClubs_Owners_OwnerId",
                table: "CyberClubs");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_CyberClubs_OwnerId",
                table: "CyberClubs");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CyberClubs");
        }
    }
}
