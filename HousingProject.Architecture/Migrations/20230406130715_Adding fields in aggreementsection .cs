using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class Addingfieldsinaggreementsection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "AggrementSections");

            migrationBuilder.AddColumn<bool>(
                name: "show_AgentName",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_HouseLocation",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_HouseName",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_Increase_flat_rate",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_Increasepercentage",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_LandlordName",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_Maintainance_and_Repairs",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_Other_Aggreement",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_Rent_Increased_After_in_years",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_SecurityDeposit",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_ServiceFee",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_TenantNmae",
                table: "AggrementSections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "show_AgentName",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_HouseLocation",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_HouseName",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_Increase_flat_rate",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_Increasepercentage",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_LandlordName",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_Maintainance_and_Repairs",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_Other_Aggreement",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_Rent_Increased_After_in_years",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_SecurityDeposit",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_ServiceFee",
                table: "AggrementSections");

            migrationBuilder.DropColumn(
                name: "show_TenantNmae",
                table: "AggrementSections");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "AggrementSections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
