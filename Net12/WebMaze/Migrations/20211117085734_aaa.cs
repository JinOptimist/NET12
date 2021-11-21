using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "MazeDifficultProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MazeDifficultProfiles_CreaterId",
                table: "MazeDifficultProfiles",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MazeDifficultProfiles_Users_CreaterId",
                table: "MazeDifficultProfiles",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MazeDifficultProfiles_Users_CreaterId",
                table: "MazeDifficultProfiles");

            migrationBuilder.DropIndex(
                name: "IX_MazeDifficultProfiles_CreaterId",
                table: "MazeDifficultProfiles");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "MazeDifficultProfiles");
        }
    }
}
