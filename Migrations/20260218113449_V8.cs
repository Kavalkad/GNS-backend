using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SuperSecretWord",
                table: "Owners",
                newName: "HashedSecretWord");

            migrationBuilder.RenameColumn(
                name: "SecretWord",
                table: "Employees",
                newName: "HashedSecretWord");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedSecretWord",
                table: "Owners",
                newName: "SuperSecretWord");

            migrationBuilder.RenameColumn(
                name: "HashedSecretWord",
                table: "Employees",
                newName: "SecretWord");
        }
    }
}
