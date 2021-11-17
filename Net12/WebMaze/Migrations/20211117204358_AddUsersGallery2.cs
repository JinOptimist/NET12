using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddUsersGallery2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Gallery");

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Gallery",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_AuthorId",
                table: "Gallery",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gallery_Users_AuthorId",
                table: "Gallery",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gallery_Users_AuthorId",
                table: "Gallery");

            migrationBuilder.DropIndex(
                name: "IX_Gallery_AuthorId",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Gallery");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Gallery",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
