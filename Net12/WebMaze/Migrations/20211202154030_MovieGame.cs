using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class MovieGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameMovie");

            migrationBuilder.AddColumn<long>(
                name: "GameId",
                table: "Movies",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GameId",
                table: "Movies",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_FavGames_GameId",
                table: "Movies",
                column: "GameId",
                principalTable: "FavGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_FavGames_GameId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_GameId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "GameMovie",
                columns: table => new
                {
                    GamesId = table.Column<long>(type: "bigint", nullable: false),
                    MyMoviesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMovie", x => new { x.GamesId, x.MyMoviesId });
                    table.ForeignKey(
                        name: "FK_GameMovie_FavGames_GamesId",
                        column: x => x.GamesId,
                        principalTable: "FavGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameMovie_Movies_MyMoviesId",
                        column: x => x.MyMoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameMovie_MyMoviesId",
                table: "GameMovie",
                column: "MyMoviesId");
        }
    }
}
