using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Employees_Id",
                table: "Owners");

            migrationBuilder.AddColumn<string>(
                name: "SuperSecretWord",
                table: "Owners",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Users_Id",
                table: "Owners",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Users_Id",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "SuperSecretWord",
                table: "Owners");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Employees_Id",
                table: "Owners",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
