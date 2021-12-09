using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddObjCells : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HpCell",
                table: "CellsModels",
                newName: "Obj2");

            migrationBuilder.AddColumn<int>(
                name: "Obj1",
                table: "CellsModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Obj1",
                table: "CellsModels");

            migrationBuilder.RenameColumn(
                name: "Obj2",
                table: "CellsModels",
                newName: "HpCell");
        }
    }
}
