using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddStatusExitStatusMaze : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DifficultProfileId",
                table: "MazeLevelsUser",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExitIsOpen",
                table: "MazeLevelsUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MazeStatus",
                table: "MazeLevelsUser",
                type: "int",
                nullable: false,
                defaultValue: 0);           

            migrationBuilder.AddColumn<int>(
                name: "CoinsToOpenTheDoor",
                table: "MazeDifficultProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MazeLevelsUser_DifficultProfileId",
                table: "MazeLevelsUser",
                column: "DifficultProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_MazeLevelsUser_MazeDifficultProfiles_DifficultProfileId",
                table: "MazeLevelsUser",
                column: "DifficultProfileId",
                principalTable: "MazeDifficultProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MazeLevelsUser_MazeDifficultProfiles_DifficultProfileId",
                table: "MazeLevelsUser");

            migrationBuilder.DropIndex(
                name: "IX_MazeLevelsUser_DifficultProfileId",
                table: "MazeLevelsUser");

            migrationBuilder.DropColumn(
                name: "DifficultProfileId",
                table: "MazeLevelsUser");

            migrationBuilder.DropColumn(
                name: "ExitIsOpen",
                table: "MazeLevelsUser");

            migrationBuilder.DropColumn(
                name: "MazeStatus",
                table: "MazeLevelsUser");            

            migrationBuilder.DropColumn(
                name: "CoinsToOpenTheDoor",
                table: "MazeDifficultProfiles");
        }
    }
}
