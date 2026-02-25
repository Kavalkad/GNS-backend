using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BloomBytesId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_BloomBytesId",
                table: "Users",
                column: "BloomBytesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_BloomBytes_BloomBytesId",
                table: "Users",
                column: "BloomBytesId",
                principalTable: "BloomBytes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_BloomBytes_BloomBytesId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BloomBytesId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BloomBytesId",
                table: "Users");
        }
    }
}
