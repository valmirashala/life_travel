using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeTravel.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartCiti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StopOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StopOneTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StopTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StopTwoTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndCiti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lines");
        }
    }
}
