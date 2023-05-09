using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class Addremindersenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTable", x => x.ReminderTableId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderTable");
        }
    }
}
