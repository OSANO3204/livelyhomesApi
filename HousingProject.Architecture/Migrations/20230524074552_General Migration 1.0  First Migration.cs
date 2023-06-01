using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class GeneralMigration10FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddCities",
                columns: table => new
                {
                    SubCountyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddCities", x => x.SubCountyId);
                });

            migrationBuilder.CreateTable(
                name: "AddCounty",
                columns: table => new
                {
                    CountyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddCounty", x => x.CountyId);
                });

            migrationBuilder.CreateTable(
                name: "AddCountyArea",
                columns: table => new
                {
                    CountyAreaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddCountyArea", x => x.CountyAreaId);
                });

            migrationBuilder.CreateTable(
                name: "AdminContacts",
                columns: table => new
                {
                    contactsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminContacts", x => x.contactsId);
                });

            migrationBuilder.CreateTable(
                name: "Aggrement",
                columns: table => new
                {
                    AggreementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseID = table.Column<int>(type: "int", nullable: false),
                    EnforceAggreement = table.Column<bool>(type: "bit", nullable: false),
                    LandlordName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeastStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeastEndDateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintainceAndRepairDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentIncreasePeriod = table.Column<int>(type: "int", nullable: false),
                    RentDepositAmount = table.Column<int>(type: "int", nullable: false),
                    Rentincreasepercentage = table.Column<int>(type: "int", nullable: false),
                    Renincreaseflatrate = table.Column<int>(type: "int", nullable: false),
                    Serviceffeedeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnyOtherTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggreeToAggreement = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggreementStatus = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    HouseAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Landlordphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tenantphone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aggrement", x => x.AggreementID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ContacusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Useremail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedMessages = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.ContacusId);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedIdHolder",
                columns: table => new
                {
                    GeneratorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratorHolder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedIdHolder", x => x.GeneratorId);
                });

            migrationBuilder.CreateTable(
                name: "HouseUnit",
                columns: table => new
                {
                    HouseunitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseID = table.Column<int>(type: "int", nullable: false),
                    HouseUnitNumber = table.Column<int>(type: "int", nullable: false),
                    Occupied = table.Column<bool>(type: "bit", nullable: false),
                    Vacant = table.Column<bool>(type: "bit", nullable: false),
                    GeneratedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseUnitFloor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseUnit", x => x.HouseunitId);
                });

            migrationBuilder.CreateTable(
                name: "HouseUnitsStatus",
                columns: table => new
                {
                    HouseidstatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoorNumber = table.Column<int>(type: "int", nullable: false),
                    Occupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseUnitsStatus", x => x.HouseidstatusID);
                });

            migrationBuilder.CreateTable(
                name: "HouseUsers",
                columns: table => new
                {
                    HouseuserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LasstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountActivated = table.Column<bool>(type: "bit", nullable: false),
                    HouseID = table.Column<int>(type: "int", nullable: false),
                    Creatormail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetypePassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseUsers", x => x.HouseuserId);
                });

            migrationBuilder.CreateTable(
                name: "ImaageUploadClass",
                columns: table => new
                {
                    imagedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImaageUploadClass", x => x.imagedId);
                });

            migrationBuilder.CreateTable(
                name: "Landlordmodel",
                columns: table => new
                {
                    LandlordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentCollection_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LondLord_HouseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paybill_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Till_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LasstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlordmodel", x => x.LandlordId);
                });

            migrationBuilder.CreateTable(
                name: "PayRent",
                columns: table => new
                {
                    Payrentid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    InternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseID = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayRent", x => x.Payrentid);
                });

            migrationBuilder.CreateTable(
                name: "RegisterProfessional",
                columns: table => new
                {
                    ProfessionalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsandConditions = table.Column<bool>(type: "bit", nullable: false),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    OperationArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Upvotes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Downvotes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVotes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterProfessional", x => x.ProfessionalId);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LasstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHouseUsers = table.Column<bool>(type: "bit", nullable: false),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Landlord = table.Column<bool>(type: "bit", nullable: false),
                    Is_CareTaker = table.Column<bool>(type: "bit", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Agent = table.Column<bool>(type: "bit", nullable: false),
                    Is_Tenant = table.Column<bool>(type: "bit", nullable: false),
                    Is_Admin = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Registration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReminderTable",
                columns: table => new
                {
                    ReminderTableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TenantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderSent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoorNumber = table.Column<int>(type: "int", nullable: false),
                    SendByNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTable", x => x.ReminderTableId);
                });

            migrationBuilder.CreateTable(
                name: "Rentpayment",
                columns: table => new
                {
                    RentpaidId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentPaid = table.Column<int>(type: "int", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    Datepaid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentpayment", x => x.RentpaidId);
                });

            migrationBuilder.CreateTable(
                name: "replyModel",
                columns: table => new
                {
                    ReplyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageID = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_replyModel", x => x.ReplyID);
                });

            migrationBuilder.CreateTable(
                name: "SectionMapper",
                columns: table => new
                {
                    SectionMapperID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggreemenID = table.Column<int>(type: "int", nullable: false),
                    AggreementSectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionMapper", x => x.SectionMapperID);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    AggreementSectiondID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsTrue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.AggreementSectiondID);
                });

            migrationBuilder.CreateTable(
                name: "UploadImage",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadImage", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "WorkIdModel",
                columns: table => new
                {
                    WorkIdKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkIdSaved = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkIdModel", x => x.WorkIdKey);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Registration_UserId",
                        column: x => x.UserId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Registration_UserId",
                        column: x => x.UserId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Registration_UserId",
                        column: x => x.UserId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Registration_UserId",
                        column: x => x.UserId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "House_Registration",
                columns: table => new
                {
                    HouseiD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    House_Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Units = table.Column<int>(type: "int", nullable: false),
                    Owner_Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner_LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner_id_Number = table.Column<int>(type: "int", nullable: false),
                    House_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Estimated_Maximum_Capacity = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailSent = table.Column<bool>(type: "bit", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_House_Registration", x => x.HouseiD);
                    table.ForeignKey(
                        name: "FK_House_Registration_Registration_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantClass",
                columns: table => new
                {
                    RenteeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseFloor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cars = table.Column<int>(type: "int", nullable: false),
                    ServicesFees = table.Column<int>(type: "int", nullable: false),
                    Rentee_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BedRoom_Number = table.Column<int>(type: "int", nullable: false),
                    Email_Confirmed = table.Column<bool>(type: "bit", nullable: false),
                    Number0f_Occupants = table.Column<int>(type: "int", nullable: false),
                    House_Rent = table.Column<float>(type: "real", nullable: false),
                    RentPaid = table.Column<float>(type: "real", nullable: false),
                    RentArrears = table.Column<float>(type: "real", nullable: false),
                    RentOverpayment = table.Column<float>(type: "real", nullable: false),
                    Agent_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingCareTaker_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Appartment_DoorNumber = table.Column<int>(type: "int", nullable: false),
                    HouseiD = table.Column<int>(type: "int", nullable: false),
                    House_RegistrationHouseiD = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentPayDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    currentMonthRent = table.Column<double>(type: "float", nullable: false),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    RemindersentCount = table.Column<int>(type: "int", nullable: false),
                    HouseDoorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantClass", x => x.RenteeId);
                    table.ForeignKey(
                        name: "FK_TenantClass_House_Registration_House_RegistrationHouseiD",
                        column: x => x.House_RegistrationHouseiD,
                        principalTable: "House_Registration",
                        principalColumn: "HouseiD",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantSummary",
                columns: table => new
                {
                    SummaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseDoornumber = table.Column<int>(type: "int", nullable: false),
                    HouseRent = table.Column<float>(type: "real", nullable: false),
                    FlatNumberId = table.Column<int>(type: "int", nullable: false),
                    DateOfRentPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentArrears = table.Column<float>(type: "real", nullable: false),
                    overpayment = table.Column<float>(type: "real", nullable: false),
                    RentPaid = table.Column<bool>(type: "bit", nullable: false),
                    OverDueRent = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLandlord = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    HouseiD = table.Column<int>(type: "int", nullable: false),
                    HouseregistrationHouseiD = table.Column<int>(type: "int", nullable: true),
                    LandlordId = table.Column<int>(type: "int", nullable: false),
                    TenantClassRenteeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSummary", x => x.SummaryId);
                    table.ForeignKey(
                        name: "FK_TenantSummary_House_Registration_HouseregistrationHouseiD",
                        column: x => x.HouseregistrationHouseiD,
                        principalTable: "House_Registration",
                        principalColumn: "HouseiD",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantSummary_Landlordmodel_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "Landlordmodel",
                        principalColumn: "LandlordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantSummary_TenantClass_TenantClassRenteeId",
                        column: x => x.TenantClassRenteeId,
                        principalTable: "TenantClass",
                        principalColumn: "RenteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_House_Registration_CreatedById",
                table: "House_Registration",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Registration",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Registration",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TenantClass_House_RegistrationHouseiD",
                table: "TenantClass",
                column: "House_RegistrationHouseiD");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSummary_HouseregistrationHouseiD",
                table: "TenantSummary",
                column: "HouseregistrationHouseiD");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSummary_LandlordId",
                table: "TenantSummary",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSummary_TenantClassRenteeId",
                table: "TenantSummary",
                column: "TenantClassRenteeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddCities");

            migrationBuilder.DropTable(
                name: "AddCounty");

            migrationBuilder.DropTable(
                name: "AddCountyArea");

            migrationBuilder.DropTable(
                name: "AdminContacts");

            migrationBuilder.DropTable(
                name: "Aggrement");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "GeneratedIdHolder");

            migrationBuilder.DropTable(
                name: "HouseUnit");

            migrationBuilder.DropTable(
                name: "HouseUnitsStatus");

            migrationBuilder.DropTable(
                name: "HouseUsers");

            migrationBuilder.DropTable(
                name: "ImaageUploadClass");

            migrationBuilder.DropTable(
                name: "PayRent");

            migrationBuilder.DropTable(
                name: "RegisterProfessional");

            migrationBuilder.DropTable(
                name: "ReminderTable");

            migrationBuilder.DropTable(
                name: "Rentpayment");

            migrationBuilder.DropTable(
                name: "replyModel");

            migrationBuilder.DropTable(
                name: "SectionMapper");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "TenantSummary");

            migrationBuilder.DropTable(
                name: "UploadImage");

            migrationBuilder.DropTable(
                name: "WorkIdModel");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Landlordmodel");

            migrationBuilder.DropTable(
                name: "TenantClass");

            migrationBuilder.DropTable(
                name: "House_Registration");

            migrationBuilder.DropTable(
                name: "Registration");
        }
    }
}
