using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class WithoutAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_CreaterId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "CreaterId",
                table: "Reviews",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CreaterId",
                table: "Reviews",
                newName: "IX_Reviews_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_CreatorId",
                table: "Reviews",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_CreatorId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Reviews",
                newName: "CreaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CreatorId",
                table: "Reviews",
                newName: "IX_Reviews_CreaterId");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_CreaterId",
                table: "Reviews",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
