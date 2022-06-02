using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneRegister.Web.Migrations
{
    public partial class AddedOrganizationDepth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                schema: "Base",
                table: "Organization",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sequencer",
                schema: "Base",
                table: "Organization",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                schema: "Base",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "Sequencer",
                schema: "Base",
                table: "Organization");
        }
    }
}
