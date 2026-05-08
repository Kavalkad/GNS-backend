using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CyberClubs_Name",
                table: "CyberClubs");

            migrationBuilder.DropColumn(
                name: "EquipmentName",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "Equipment",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CyberClubs_Name",
                table: "CyberClubs",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CyberClubs_Name",
                table: "CyberClubs");

            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentName",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CyberClubs_Name",
                table: "CyberClubs",
                column: "Name");
        }
    }
}
