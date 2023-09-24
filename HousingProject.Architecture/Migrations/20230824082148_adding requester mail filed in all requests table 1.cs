using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingProject.Infrastructure.Migrations
{
    public partial class addingrequestermailfiledinallrequeststable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Worker_phone",
                table: "Add_User_Request",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Worker_phone",
                table: "Add_User_Request");
        }
    }
}
