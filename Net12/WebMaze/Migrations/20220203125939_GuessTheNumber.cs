using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class GuessTheNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuessTheNumberGameParameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    RewardForWinningTheGame = table.Column<int>(type: "int", nullable: false),
                    GameCost = table.Column<int>(type: "int", nullable: false),
                    MaxAttempts = table.Column<int>(type: "int", nullable: false),
                    MinRangeNumber = table.Column<int>(type: "int", nullable: false),
                    MaxRangeNumber = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuessTheNumberGameParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuessTheNumberGames",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuessedNumber = table.Column<int>(type: "int", nullable: false),
                    StartDateGame = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    GameStatus = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: true),
                    ParametersId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuessTheNumberGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuessTheNumberGames_GuessTheNumberGameParameters_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "GuessTheNumberGameParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuessTheNumberGames_Users_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuessTheNumberGameAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntroducedAnswer = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuessTheNumberGameAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuessTheNumberGameAnswers_GuessTheNumberGames_GameId",
                        column: x => x.GameId,
                        principalTable: "GuessTheNumberGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuessTheNumberGameAnswers_GameId",
                table: "GuessTheNumberGameAnswers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GuessTheNumberGames_ParametersId",
                table: "GuessTheNumberGames",
                column: "ParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_GuessTheNumberGames_PlayerId",
                table: "GuessTheNumberGames",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuessTheNumberGameAnswers");

            migrationBuilder.DropTable(
                name: "GuessTheNumberGames");

            migrationBuilder.DropTable(
                name: "GuessTheNumberGameParameters");
        }
    }
}
