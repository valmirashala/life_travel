using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelephant.Migrations
{
    public partial class AddedAtributeBusinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusInfo");
        }
    }
}
