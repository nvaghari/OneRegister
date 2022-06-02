using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneRegister.Web.Migrations
{
    public partial class AddAccountToMerchantInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Account",
                schema: "Merchant",
                table: "MerchantInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("update Merchant.MerchantInfo set Account = CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                schema: "Merchant",
                table: "MerchantInfo");
        }
    }
}
