using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelephant.Migrations
{
    public partial class AddIDToTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                columns: new[] { "ID", "UserID", "BusID" });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserID",
                table: "Ticket",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_UserID",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Ticket");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                columns: new[] { "UserID", "BusID" });
        }
    }
}
