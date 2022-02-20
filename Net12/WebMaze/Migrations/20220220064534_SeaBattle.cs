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
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleDifficults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleGames",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "SeaBattleFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    ShipCount = table.Column<int>(type: "int", nullable: false),
                    LastHitToShip = table.Column<long>(type: "bigint", nullable: false),
                    IsField = table.Column<bool>(type: "bit", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleFields_SeaBattleGames_GameId",
                        column: x => x.GameId,
                        principalTable: "SeaBattleGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeaBattleCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    ShipHere = table.Column<bool>(type: "bit", nullable: false),
                    Hit = table.Column<bool>(type: "bit", nullable: false),
                    ShipLength = table.Column<int>(type: "int", nullable: false),
                    ShipNumber = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaBattleCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaBattleCells_SeaBattleFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "SeaBattleFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleCells_FieldId",
                table: "SeaBattleCells",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleFields_GameId",
                table: "SeaBattleFields",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_SeaBattleGames_UserId",
                table: "SeaBattleGames",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeaBattleCells");

            migrationBuilder.DropTable(
                name: "SeaBattleDifficults");

            migrationBuilder.DropTable(
                name: "SeaBattleFields");

            migrationBuilder.DropTable(
                name: "SeaBattleGames");

        }
    }
}
