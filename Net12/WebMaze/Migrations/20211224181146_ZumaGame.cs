using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ZumaGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZumaGameDifficults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    ColorCount = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZumaGameDifficults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZumaGameDifficults_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZumaGameFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    ColorCount = table.Column<int>(type: "int", nullable: false),
                    GamerId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZumaGameFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZumaGameFields_Users_GamerId",
                        column: x => x.GamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZumaGameCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZumaGameCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZumaGameCells_ZumaGameFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "ZumaGameFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZumaGameColors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZumaGameColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZumaGameColors_ZumaGameFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "ZumaGameFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameCells_FieldId",
                table: "ZumaGameCells",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameColors_FieldId",
                table: "ZumaGameColors",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameDifficults_AuthorId",
                table: "ZumaGameDifficults",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameFields_GamerId",
                table: "ZumaGameFields",
                column: "GamerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZumaGameCells");

            migrationBuilder.DropTable(
                name: "ZumaGameColors");

            migrationBuilder.DropTable(
                name: "ZumaGameDifficults");

            migrationBuilder.DropTable(
                name: "ZumaGameFields");
        }
    }
}
