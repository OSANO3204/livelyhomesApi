using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class changingnumbergenerator2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Rent_Monthly_Update");

            migrationBuilder.AddColumn<string>(
                name: "Internal_ReferenceNumber",
                table: "Rent_Monthly_Update",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Paid",
                table: "Rent_Monthly_Update",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Rent_Monthly_Update",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider_Reference",
                table: "Rent_Monthly_Update",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Internal_ReferenceNumber",
                table: "Rent_Monthly_Update");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Rent_Monthly_Update");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Rent_Monthly_Update");

            migrationBuilder.DropColumn(
                name: "Provider_Reference",
                table: "Rent_Monthly_Update");

            migrationBuilder.AddColumn<DateTime>(
                name: "MyProperty",
                table: "Rent_Monthly_Update",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
