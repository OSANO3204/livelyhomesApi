using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class addingchangesonsentratedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HouseId",
                table: "ReminderTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "ReminderTable");
        }
    }
}
