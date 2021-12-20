using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class CreateConnectionBetweenMovieAndGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameMovie");
        }
    }
}
