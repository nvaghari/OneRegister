using Microsoft.EntityFrameworkCore.Migrations;

namespace OneRegister.Web.Migrations
{
    public partial class changeNotificationSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationTask",
                schema: "Base",
                newName: "NotificationTask",
                newSchema: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationJob",
                schema: "Base",
                newName: "NotificationJob",
                newSchema: "Notification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "NotificationTask",
                schema: "Notification",
                newName: "NotificationTask",
                newSchema: "Base");

            migrationBuilder.RenameTable(
                name: "NotificationJob",
                schema: "Notification",
                newName: "NotificationJob",
                newSchema: "Base");
        }
    }
}
