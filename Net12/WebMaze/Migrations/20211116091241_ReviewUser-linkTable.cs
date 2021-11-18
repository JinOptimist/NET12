using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class ReviewUserlinkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreaterId",
                table: "Reviews",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_CreaterId",
                table: "Reviews",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_CreaterId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CreaterId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "Reviews");
        }
    }
}
