using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroservicesUser.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatusFieldInEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EmailVerifications",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmailVerifications");
        }
    }
}
