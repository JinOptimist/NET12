using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ConnecteUserToCell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "NewCellSuggestions");

            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "NewCellSuggestions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "NewCellSuggestions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewCellSuggestions_ApproverId",
                table: "NewCellSuggestions",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_NewCellSuggestions_CreaterId",
                table: "NewCellSuggestions",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewCellSuggestions_Users_ApproverId",
                table: "NewCellSuggestions",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewCellSuggestions_Users_CreaterId",
                table: "NewCellSuggestions",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewCellSuggestions_Users_ApproverId",
                table: "NewCellSuggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_NewCellSuggestions_Users_CreaterId",
                table: "NewCellSuggestions");

            migrationBuilder.DropIndex(
                name: "IX_NewCellSuggestions_ApproverId",
                table: "NewCellSuggestions");

            migrationBuilder.DropIndex(
                name: "IX_NewCellSuggestions_CreaterId",
                table: "NewCellSuggestions");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "NewCellSuggestions");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "NewCellSuggestions");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "NewCellSuggestions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
