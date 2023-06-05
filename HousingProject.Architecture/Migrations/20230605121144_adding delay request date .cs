using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class addingdelayrequestdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentDelayRequestTable",
                columns: table => new
                {
                    delay_request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestPaymentDatedate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequesterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requestermail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    DoorNumber = table.Column<int>(type: "int", nullable: false),
                    AdditionDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantRentPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentDelayRequestTable", x => x.delay_request_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentDelayRequestTable");
        }
    }
}
