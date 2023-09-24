using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class Addingpaymentsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "paymentCodes",
                columns: table => new
                {
                    paymentCodeid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseID = table.Column<int>(type: "int", nullable: false),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stk_shortCode = table.Column<int>(type: "int", nullable: false),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentCodes", x => x.paymentCodeid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "paymentCodes");
        }
    }
}
