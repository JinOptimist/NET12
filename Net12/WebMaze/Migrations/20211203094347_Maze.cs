using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class Maze : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MazeLevelsUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    HeroMaxHp = table.Column<int>(type: "int", nullable: false),
                    HeroMaxFatigure = table.Column<int>(type: "int", nullable: false),
                    HeroX = table.Column<int>(type: "int", nullable: false),
                    HeroY = table.Column<int>(type: "int", nullable: false),
                    HeroNowHp = table.Column<int>(type: "int", nullable: false),
                    HeroNowFatigure = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MazeLevelsUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MazeLevelsUser_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CellsModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeCell = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    HpCell = table.Column<int>(type: "int", nullable: false),
                    MazeLevelId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellsModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CellsModels_MazeLevelsUser_MazeLevelId",
                        column: x => x.MazeLevelId,
                        principalTable: "MazeLevelsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CellsModels_MazeLevelId",
                table: "CellsModels",
                column: "MazeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MazeLevelsUser_CreatorId",
                table: "MazeLevelsUser",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CellsModels");

            migrationBuilder.DropTable(
                name: "MazeLevelsUser");
        }
    }
}
