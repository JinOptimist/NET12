using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ZumaGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZumaGameField",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    ColorCount = table.Column<int>(type: "int", nullable: false),
                    GamerId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZumaGameField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZumaGameField_Users_GamerId",
                        column: x => x.GamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZumaGameCell",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZumaGameCell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZumaGameCell_ZumaGameField_FieldId",
                        column: x => x.FieldId,
                        principalTable: "ZumaGameField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameCell_FieldId",
                table: "ZumaGameCell",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameField_GamerId",
                table: "ZumaGameField",
                column: "GamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZumaGameCell");

            migrationBuilder.DropTable(
                name: "ZumaGameField");

        }
    }
}
