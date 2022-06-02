using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneRegister.Web.Migrations
{
    public partial class RemoveRefIDNotiTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                schema: "Notification",
                table: "NotificationTask");

            migrationBuilder.DropColumn(
                name: "RefId",
                schema: "Notification",
                table: "NotificationTask");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "From",
                schema: "Notification",
                table: "NotificationTask",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefId",
                schema: "Notification",
                table: "NotificationTask",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
