using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneRegister.Web.Migrations
{
    public partial class RemoveMerchantExtraInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantExtraInfo",
                schema: "Merchant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MerchantExtraInfo",
                schema: "Merchant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BPCodeAP = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BPCodeAR = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ChequeNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CompanyRegisterNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MCC = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MonthlyRental = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RefNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RiskComments = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RiskLevel = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantExtraInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantExtraInfo_Organization_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantExtraInfo_MerchantId",
                schema: "Merchant",
                table: "MerchantExtraInfo",
                column: "MerchantId",
                unique: true);
        }
    }
}
