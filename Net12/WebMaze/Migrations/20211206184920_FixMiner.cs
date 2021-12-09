using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class FixMiner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinerCell_MinerCell_FieldId",
                table: "MinerCell");

            migrationBuilder.DropForeignKey(
                name: "FK_MinerCell_MinerField_MinerFieldId",
                table: "MinerCell");

            migrationBuilder.DropIndex(
                name: "IX_MinerCell_MinerFieldId",
                table: "MinerCell");

            migrationBuilder.DropColumn(
                name: "MinerFieldId",
                table: "MinerCell");

            migrationBuilder.AddForeignKey(
                name: "FK_MinerCell_MinerField_FieldId",
                table: "MinerCell",
                column: "FieldId",
                principalTable: "MinerField",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinerCell_MinerField_FieldId",
                table: "MinerCell");

            migrationBuilder.AddColumn<long>(
                name: "MinerFieldId",
                table: "MinerCell",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MinerCell_MinerFieldId",
                table: "MinerCell",
                column: "MinerFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinerCell_MinerCell_FieldId",
                table: "MinerCell",
                column: "FieldId",
                principalTable: "MinerCell",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MinerCell_MinerField_MinerFieldId",
                table: "MinerCell",
                column: "MinerFieldId",
                principalTable: "MinerField",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
