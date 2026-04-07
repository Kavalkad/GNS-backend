using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyberClubWorkingHoursEntity");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_CyberClubId_DayOfWeek",
                table: "WorkingHours",
                columns: new[] { "CyberClubId", "DayOfWeek" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHours_CyberClubs_CyberClubId",
                table: "WorkingHours",
                column: "CyberClubId",
                principalTable: "CyberClubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHours_CyberClubs_CyberClubId",
                table: "WorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHours_CyberClubId_DayOfWeek",
                table: "WorkingHours");

            migrationBuilder.CreateTable(
                name: "CyberClubWorkingHoursEntity",
                columns: table => new
                {
                    CyberClubId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkingHoursId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyberClubWorkingHoursEntity", x => new { x.CyberClubId, x.WorkingHoursId });
                    table.ForeignKey(
                        name: "FK_CyberClubWorkingHoursEntity_CyberClubs_CyberClubId",
                        column: x => x.CyberClubId,
                        principalTable: "CyberClubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CyberClubWorkingHoursEntity_WorkingHours_WorkingHoursId",
                        column: x => x.WorkingHoursId,
                        principalTable: "WorkingHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyberClubWorkingHoursEntity_WorkingHoursId",
                table: "CyberClubWorkingHoursEntity",
                column: "WorkingHoursId");
        }
    }
}
