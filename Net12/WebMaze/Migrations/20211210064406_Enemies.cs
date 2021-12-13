using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class Enemies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MazeEnemyWeb",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeEnemy = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Obj1 = table.Column<int>(type: "int", nullable: false),
                    Obj2 = table.Column<int>(type: "int", nullable: false),
                    MazeLevelId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MazeEnemyWeb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MazeEnemyWeb_MazeLevelsUser_MazeLevelId",
                        column: x => x.MazeLevelId,
                        principalTable: "MazeLevelsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MazeEnemyWeb_MazeLevelId",
                table: "MazeEnemyWeb",
                column: "MazeLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MazeEnemyWeb");
        }
    }
}
