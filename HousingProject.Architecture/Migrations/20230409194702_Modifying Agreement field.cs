using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class ModifyingAgreementfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnyOtherTerms",
                table: "Aggrement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeastEndDateDate",
                table: "Aggrement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LeastStartDate",
                table: "Aggrement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "MaintainceAndRepairDeposit",
                table: "Aggrement",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Renincreaseflatrate",
                table: "Aggrement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RentAmount",
                table: "Aggrement",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RentDepositAmount",
                table: "Aggrement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentIncreasePeriod",
                table: "Aggrement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rentincreasepercentage",
                table: "Aggrement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Serviceffeedeposit",
                table: "Aggrement",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnyOtherTerms",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "LeastEndDateDate",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "LeastStartDate",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "MaintainceAndRepairDeposit",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "Renincreaseflatrate",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "RentAmount",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "RentDepositAmount",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "RentIncreasePeriod",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "Rentincreasepercentage",
                table: "Aggrement");

            migrationBuilder.DropColumn(
                name: "Serviceffeedeposit",
                table: "Aggrement");
        }
    }
}
