using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ZumaGameOneGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZumaGameCells_ZumaGameFields_FieldId",
                table: "ZumaGameCells");

            migrationBuilder.DropForeignKey(
                name: "FK_ZumaGameColors_ZumaGameFields_FieldId",
                table: "ZumaGameColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ZumaGameFields_Users_GamerId",
                table: "ZumaGameFields");

            migrationBuilder.DropIndex(
                name: "IX_ZumaGameFields_GamerId",
                table: "ZumaGameFields");

            migrationBuilder.AlterColumn<long>(
                name: "GamerId",
                table: "ZumaGameFields",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameFields_GamerId",
                table: "ZumaGameFields",
                column: "GamerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ZumaGameCells_ZumaGameFields_FieldId",
                table: "ZumaGameCells",
                column: "FieldId",
                principalTable: "ZumaGameFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZumaGameColors_ZumaGameFields_FieldId",
                table: "ZumaGameColors",
                column: "FieldId",
                principalTable: "ZumaGameFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZumaGameFields_Users_GamerId",
                table: "ZumaGameFields",
                column: "GamerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZumaGameCells_ZumaGameFields_FieldId",
                table: "ZumaGameCells");

            migrationBuilder.DropForeignKey(
                name: "FK_ZumaGameColors_ZumaGameFields_FieldId",
                table: "ZumaGameColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ZumaGameFields_Users_GamerId",
                table: "ZumaGameFields");

            migrationBuilder.DropIndex(
                name: "IX_ZumaGameFields_GamerId",
                table: "ZumaGameFields");

            migrationBuilder.AlterColumn<long>(
                name: "GamerId",
                table: "ZumaGameFields",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_ZumaGameFields_GamerId",
                table: "ZumaGameFields",
                column: "GamerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZumaGameCells_ZumaGameFields_FieldId",
                table: "ZumaGameCells",
                column: "FieldId",
                principalTable: "ZumaGameFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ZumaGameColors_ZumaGameFields_FieldId",
                table: "ZumaGameColors",
                column: "FieldId",
                principalTable: "ZumaGameFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ZumaGameFields_Users_GamerId",
                table: "ZumaGameFields",
                column: "GamerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
