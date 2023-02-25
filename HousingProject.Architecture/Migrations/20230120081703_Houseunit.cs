using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class Houseunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    HouseUnitFloor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseUnit", x => x.HouseunitId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseUnit");
        }
    }
}
