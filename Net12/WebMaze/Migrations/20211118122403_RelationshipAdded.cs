using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class RelationshipAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "FavGames");

            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "FavGames",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavGames_CreaterId",
                table: "FavGames",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavGames_Users_CreaterId",
                table: "FavGames",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavGames_Users_CreaterId",
                table: "FavGames");

            migrationBuilder.DropIndex(
                name: "IX_FavGames_CreaterId",
                table: "FavGames");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "FavGames");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "FavGames",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
