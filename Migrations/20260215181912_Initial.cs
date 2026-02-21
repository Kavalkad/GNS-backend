using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CyberClubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyberClubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    HashedPassword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CyberClubId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    HashedPassword = table.Column<string>(type: "TEXT", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "GamingPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "TEXT", nullable: false),
                    Equipment = table.Column<int>(type: "INTEGER", nullable: false),
                    CyberClubId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamingPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamingPlaces_CyberClubs_CyberClubId",
                        column: x => x.CyberClubId,
                        principalTable: "CyberClubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    StartHour = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    EndHour = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    IsOpen = table.Column<bool>(type: "INTEGER", nullable: false),
                    CyberClubId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHours_CyberClubs_CyberClubId",
                        column: x => x.CyberClubId,
                        principalTable: "CyberClubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TotalSum = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GamingPlaceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_GamingPlaces_GamingPlaceId",
                        column: x => x.GamingPlaceId,
                        principalTable: "GamingPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyberClubs_Name",
                table: "CyberClubs",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CyberClubId",
                table: "Employees",
                column: "CyberClubId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGamingPlaces_GamingPlaceId",
                table: "GameGamingPlaces",
                column: "GamingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GamingPlaces_CyberClubId",
                table: "GamingPlaces",
                column: "CyberClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GamingPlaceId",
                table: "Orders",
                column: "GamingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_CyberClubId",
                table: "WorkingHours",
                column: "CyberClubId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "GameGamingPlaces");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "WorkingHours");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GamingPlaces");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CyberClubs");
        }
    }
}
