using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class v20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_BloomBytes_BloomBytesId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_BloomBytes_BloomBytesId",
                table: "Users",
                column: "BloomBytesId",
                principalTable: "BloomBytes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_BloomBytes_BloomBytesId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_BloomBytes_BloomBytesId",
                table: "Users",
                column: "BloomBytesId",
                principalTable: "BloomBytes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
