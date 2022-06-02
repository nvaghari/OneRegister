using Microsoft.EntityFrameworkCore.Migrations;

namespace OneRegister.Web.Migrations
{
    public partial class AddedSubjectToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "Notification",
                table: "NotificationTask",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "Notification",
                table: "NotificationTask");
        }
    }
}
