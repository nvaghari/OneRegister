using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OneRegister.Web.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Merchant");

            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.CreateTable(
                name: "NotificationJob",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_Organization_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTask",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    From = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotificationJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTask_NotificationJob_NotificationJobId",
                        column: x => x.NotificationJobId,
                        principalSchema: "Base",
                        principalTable: "NotificationJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantExtraInfo",
                schema: "Merchant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CompanyRegisterNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BPCodeAR = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BPCodeAP = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    RefNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MCC = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ChequeNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    RiskLevel = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    RiskComments = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    MonthlyRental = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "MerchantInfo",
                schema: "Merchant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Services = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassedStates = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BusinessNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SstId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Town = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AreaState = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    BusinessType = table.Column<int>(type: "int", nullable: true),
                    Principal = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OperatingDaysHours = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    TickectSize = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MonthlyTurnover = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TelNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FaxNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MerchantState = table.Column<int>(type: "int", nullable: false),
                    MerchantStatus = table.Column<int>(type: "int", nullable: false),
                    FormNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FormNumberBase = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BankAccountName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BankAccountNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BankAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    BankPromoAgreeable = table.Column<bool>(type: "bit", nullable: true),
                    ChannelAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ChannelUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChannelEmail = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SalesPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantInfo_Organization_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationFile",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    DmsId = table.Column<long>(type: "bigint", nullable: true),
                    DmsUrl = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Thumbnail = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationFile_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Lat = table.Column<decimal>(type: "decimal(8,6)", nullable: true),
                    Lng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    OperatingDaysHours = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RegionState = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Town = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ContactTel = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ContactMobile = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    OutletType = table.Column<int>(type: "int", nullable: true),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Site_Organization_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Site_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IdentityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PlotNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CompanyNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    NatureOfBusiness = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PurposeOfTransaction = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VisaExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlksExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermOfService = table.Column<int>(type: "int", nullable: true),
                    AgroOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ParentName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ParentPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomeRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_Organization_AgroOrganizationId",
                        column: x => x.AgroOrganizationId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Member_Organization_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Member_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Member_Organization_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "Base",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Member_Site_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalSchema: "Base",
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Member_Site_HomeRoomId",
                        column: x => x.HomeRoomId,
                        principalSchema: "Base",
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Security",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Security",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Security",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberAddress",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberAddress_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Base",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberFile",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    DmsId = table.Column<long>(type: "bigint", nullable: true),
                    DmsUrl = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Thumbnail = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberFile_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Base",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Member_AgroOrganizationId",
                schema: "Base",
                table: "Member",
                column: "AgroOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ClassRoomId",
                schema: "Base",
                table: "Member",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_HomeRoomId",
                schema: "Base",
                table: "Member",
                column: "HomeRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_MerchantId",
                schema: "Base",
                table: "Member",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_OrganizationId",
                schema: "Base",
                table: "Member",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_SchoolId",
                schema: "Base",
                table: "Member",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAddress_MemberId",
                schema: "Base",
                table: "MemberAddress",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberFile_MemberId",
                schema: "Base",
                table: "MemberFile",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantExtraInfo_MerchantId",
                schema: "Merchant",
                table: "MerchantExtraInfo",
                column: "MerchantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchantInfo_MerchantId",
                schema: "Merchant",
                table: "MerchantInfo",
                column: "MerchantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTask_NotificationJobId",
                schema: "Base",
                table: "NotificationTask",
                column: "NotificationJobId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ParentId",
                schema: "Base",
                table: "Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationFile_OrganizationId",
                schema: "Base",
                table: "OrganizationFile",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Security",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_OrganizationId",
                schema: "Security",
                table: "Roles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Security",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Site_MerchantId",
                schema: "Base",
                table: "Site",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Site_OrganizationId",
                schema: "Base",
                table: "Site",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Security",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Security",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Security",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Security",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                schema: "Security",
                table: "Users",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Security",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberAddress",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "MemberFile",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "MerchantExtraInfo",
                schema: "Merchant");

            migrationBuilder.DropTable(
                name: "MerchantInfo",
                schema: "Merchant");

            migrationBuilder.DropTable(
                name: "NotificationTask",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "OrganizationFile",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Member",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "NotificationJob",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Site",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Organization",
                schema: "Base");
        }
    }
}
