using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ChangeNameOfColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfNew",
                table: "News",
                newName: "EventDate");

            migrationBuilder.RenameColumn(
                name: "DatNow",
                table: "News",
                newName: "CreationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "News",
                newName: "DateOfNew");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "News",
                newName: "DatNow");
        }
    }
}
