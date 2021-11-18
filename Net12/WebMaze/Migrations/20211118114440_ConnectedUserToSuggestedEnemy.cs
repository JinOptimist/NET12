using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ConnectedUserToSuggestedEnemy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "SuggestedEnemys",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "SuggestedEnemys",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedEnemys_ApproverId",
                table: "SuggestedEnemys",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedEnemys_CreaterId",
                table: "SuggestedEnemys",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedEnemys_Users_ApproverId",
                table: "SuggestedEnemys",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedEnemys_Users_CreaterId",
                table: "SuggestedEnemys",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedEnemys_Users_ApproverId",
                table: "SuggestedEnemys");

            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedEnemys_Users_CreaterId",
                table: "SuggestedEnemys");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedEnemys_ApproverId",
                table: "SuggestedEnemys");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedEnemys_CreaterId",
                table: "SuggestedEnemys");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "SuggestedEnemys");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "SuggestedEnemys");
        }
    }
}
