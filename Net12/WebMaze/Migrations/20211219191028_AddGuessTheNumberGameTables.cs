using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddGuessTheNumberGameTables : Migration
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
                    GameDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    GameStatus = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    ParametersId = table.Column<long>(type: "bigint", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuessTheNumberGames_Users_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuessTheNumberGameAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntroducedAnswer = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GuessTheNumberGameParameters",
                columns: new[] { "Id", "Difficulty", "GameCost", "IsActive", "MaxAttempts", "MaxRangeNumber", "MinRangeNumber", "RewardForWinningTheGame" },
                values: new object[] { 1L, 1, 1, true, 4, 10, 1, 2 });

            migrationBuilder.InsertData(
                table: "GuessTheNumberGameParameters",
                columns: new[] { "Id", "Difficulty", "GameCost", "IsActive", "MaxAttempts", "MaxRangeNumber", "MinRangeNumber", "RewardForWinningTheGame" },
                values: new object[] { 2L, 2, 2, true, 7, 100, 1, 4 });

            migrationBuilder.InsertData(
                table: "GuessTheNumberGameParameters",
                columns: new[] { "Id", "Difficulty", "GameCost", "IsActive", "MaxAttempts", "MaxRangeNumber", "MinRangeNumber", "RewardForWinningTheGame" },
                values: new object[] { 3L, 3, 3, true, 10, 1000, 1, 6 });

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
