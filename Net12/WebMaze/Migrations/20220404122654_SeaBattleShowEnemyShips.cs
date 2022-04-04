using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class SeaBattleShowEnemyShips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FourSizeShip",
                table: "SeaBattleGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThreeSizeShip",
                table: "SeaBattleGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoSizeShip",
                table: "SeaBattleGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FourSizeShip",
                table: "SeaBattleGames");

            migrationBuilder.DropColumn(
                name: "ThreeSizeShip",
                table: "SeaBattleGames");

            migrationBuilder.DropColumn(
                name: "TwoSizeShip",
                table: "SeaBattleGames");
        }
    }
}
