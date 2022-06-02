using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneRegister.Web.Migrations
{
    public partial class AddedIsSystemicProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemic",
                schema: "Security",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemic",
                schema: "Security",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemic",
                schema: "Base",
                table: "Organization",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemic",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSystemic",
                schema: "Security",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsSystemic",
                schema: "Base",
                table: "Organization");
        }
    }
}
