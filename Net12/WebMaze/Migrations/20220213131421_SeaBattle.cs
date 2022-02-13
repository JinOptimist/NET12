using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class SeaBattle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeaBattleDifficults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    TwoSizeShip = table.Column<int>(type: "int", nullable: false),
                    ThreeSizeShip = table.Column<int>(type: "int", nullable: false),
                    FourSizeShip = table.Column<int>(type: "int", nullable: false),
                    GamerId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleDifficults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleDifficults_Users_GamerId",
                        column: x => x.GamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleGames",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HitInShipId = table.Column<long>(type: "bigint", nullable: false),
                    DirectionToShoot = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleEnemyFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    ShipCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleEnemyFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleEnemyFields_SeaBattleGames_GameId",
                        column: x => x.GameId,
                        principalTable: "SeaBattleGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleMyFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastHitToShip = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    ShipCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleMyFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleMyFields_SeaBattleGames_GameId",
                        column: x => x.GameId,
                        principalTable: "SeaBattleGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleEnemyCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    ShipHere = table.Column<bool>(type: "bit", nullable: false),
                    Hit = table.Column<bool>(type: "bit", nullable: false),
                    ShipLength = table.Column<int>(type: "int", nullable: false),
                    ShipNumber = table.Column<int>(type: "int", nullable: false),
                    ShipDirection = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleEnemyCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleEnemyCells_SeaBattleEnemyFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "SeaBattleEnemyFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleMyCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    ShipHere = table.Column<bool>(type: "bit", nullable: false),
                    Hit = table.Column<bool>(type: "bit", nullable: false),
                    ShipLength = table.Column<int>(type: "int", nullable: false),
                    ShipNumber = table.Column<int>(type: "int", nullable: false),
                    ShipDirection = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleMyCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleMyCells_SeaBattleMyFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "SeaBattleMyFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleDifficults_GamerId",
                table: "SeaBattleDifficults",
                column: "GamerId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleEnemyCells_FieldId",
                table: "SeaBattleEnemyCells",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleEnemyFields_GameId",
                table: "SeaBattleEnemyFields",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleGames_UserId",
                table: "SeaBattleGames",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleMyCells_FieldId",
                table: "SeaBattleMyCells",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleMyFields_GameId",
                table: "SeaBattleMyFields",
                column: "GameId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeaBattleDifficults");

            migrationBuilder.DropTable(
                name: "SeaBattleEnemyCells");

            migrationBuilder.DropTable(
                name: "SeaBattleMyCells");

            migrationBuilder.DropTable(
                name: "SeaBattleEnemyFields");

            migrationBuilder.DropTable(
                name: "SeaBattleMyFields");

            migrationBuilder.DropTable(
                name: "SeaBattleGames");
        }
    }
}
