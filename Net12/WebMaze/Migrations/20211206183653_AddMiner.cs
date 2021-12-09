using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddMiner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinerField",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    GamerId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinerField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinerField_Users_GamerId",
                        column: x => x.GamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MinerCell",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    IsBomb = table.Column<bool>(type: "bit", nullable: false),
                    NearBombsCount = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<long>(type: "bigint", nullable: true),
                    MinerFieldId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinerCell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinerCell_MinerCell_FieldId",
                        column: x => x.FieldId,
                        principalTable: "MinerCell",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MinerCell_MinerField_MinerFieldId",
                        column: x => x.MinerFieldId,
                        principalTable: "MinerField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MinerCell_FieldId",
                table: "MinerCell",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_MinerCell_MinerFieldId",
                table: "MinerCell",
                column: "MinerFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_MinerField_GamerId",
                table: "MinerField",
                column: "GamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinerCell");

            migrationBuilder.DropTable(
                name: "MinerField");
        }
    }
}
