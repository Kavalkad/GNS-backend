using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyberClubs_Owners_OwnerId",
                table: "CyberClubs");

            migrationBuilder.DropIndex(
                name: "IX_CyberClubs_OwnerId",
                table: "CyberClubs");

            migrationBuilder.AddForeignKey(
                name: "FK_CyberClubs_Owners_Id",
                table: "CyberClubs",
                column: "Id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyberClubs_Owners_Id",
                table: "CyberClubs");

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
    }
}
