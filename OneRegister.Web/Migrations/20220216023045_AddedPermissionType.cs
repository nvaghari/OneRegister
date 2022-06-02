using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneRegister.Web.Migrations
{
    public partial class AddedPermissionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttributeType",
                schema: "Security",
                table: "Permissions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeType",
                schema: "Security",
                table: "Permissions");
        }
    }
}
