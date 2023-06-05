using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class addingdatetimeinunitsarea2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequested",
                table: "RentDelayRequestTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "RentDelayRequestTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RentDelayRequestTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRequested",
                table: "RentDelayRequestTable");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "RentDelayRequestTable");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RentDelayRequestTable");
        }
    }
}
