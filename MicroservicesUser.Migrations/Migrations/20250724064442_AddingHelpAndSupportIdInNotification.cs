using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroservicesUser.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddingHelpAndSupportIdInNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HelpAndSupportId",
                table: "Notifications",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HelpAndSupportId",
                table: "Notifications",
                column: "HelpAndSupportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_HelpAndSupport_HelpAndSupportId",
                table: "Notifications",
                column: "HelpAndSupportId",
                principalTable: "HelpAndSupport",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_HelpAndSupport_HelpAndSupportId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_HelpAndSupportId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "HelpAndSupportId",
                table: "Notifications");
        }
    }
}
