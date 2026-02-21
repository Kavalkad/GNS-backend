using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CyberClubs_CyberClubId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CyberClubId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CyberClubId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecretWord",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    SecretWord = table.Column<string>(type: "TEXT", nullable: false),
                    Salary = table.Column<decimal>(type: "TEXT", nullable: false),
                    Bonus = table.Column<decimal>(type: "TEXT", nullable: false),
                    Penalty = table.Column<decimal>(type: "TEXT", nullable: false),
                    CyberClubId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_CyberClubs_CyberClubId",
                        column: x => x.CyberClubId,
                        principalTable: "CyberClubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employees_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CyberClubId",
                table: "Employees",
                column: "CyberClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.AddColumn<decimal>(
                name: "Bonus",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CyberClubId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Penalty",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretWord",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CyberClubId",
                table: "Users",
                column: "CyberClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CyberClubs_CyberClubId",
                table: "Users",
                column: "CyberClubId",
                principalTable: "CyberClubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
