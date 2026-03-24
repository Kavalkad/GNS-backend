using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHours_CyberClubs_CyberClubId",
                table: "WorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHours_CyberClubId",
                table: "WorkingHours");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Orders",
                newName: "DateTimeStart");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Orders",
                newName: "DateTimeEnd");

            migrationBuilder.CreateTable(
                name: "CyberClubWorkingHoursEntity",
                columns: table => new
                {
                    WorkingHoursId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CyberClubId = table.Column<Guid>(type: "TEXT", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyberClubWorkingHoursEntity");

            migrationBuilder.RenameColumn(
                name: "DateTimeStart",
                table: "Orders",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "DateTimeEnd",
                table: "Orders",
                newName: "EndTime");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_CyberClubId",
                table: "WorkingHours",
                column: "CyberClubId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHours_CyberClubs_CyberClubId",
                table: "WorkingHours",
                column: "CyberClubId",
                principalTable: "CyberClubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
