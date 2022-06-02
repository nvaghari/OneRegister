using Microsoft.EntityFrameworkCore.Migrations;

namespace OneRegister.Web.Migrations
{
    public partial class AddedDeliveryTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RejectRemark",
                schema: "Merchant",
                table: "MerchantInfo",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryTime",
                schema: "Merchant",
                table: "MerchantInfo",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                schema: "Merchant",
                table: "MerchantInfo");

            migrationBuilder.AlterColumn<string>(
                name: "RejectRemark",
                schema: "Merchant",
                table: "MerchantInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);
        }
    }
}
