using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroservicesUser.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsDeletedInProxyVpnDetectionAndEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProxyVpnDetections",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailVerifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProxyVpnDetections");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailVerifications");
        }
    }
}
