using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddedColumnsToMinerCellAndMinerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlayingNow",
                table: "MinerField",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FirstOpenedBomb",
                table: "MinerCell",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlayingNow",
                table: "MinerField");

            migrationBuilder.DropColumn(
                name: "FirstOpenedBomb",
                table: "MinerCell");
        }
    }
}
