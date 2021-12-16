using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ZumaGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZumaGameFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    ColorCount = table.Column<int>(type: "int", nullable: false),
                    GamerId = table.Column<long>(type: "bigint", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_ZumaGameFields_GamerId",
                table: "ZumaGameFields",
                column: "GamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZumaGameCells");

            migrationBuilder.DropTable(
                name: "ZumaGameColors");

            migrationBuilder.DropTable(
                name: "ZumaGameFields");
        }
    }
}
