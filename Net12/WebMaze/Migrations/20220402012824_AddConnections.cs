using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DifficultProfileId",
                table: "MazeLevelsUser",
                type: "bigint",
                nullable: true);

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
        }
    }
}
