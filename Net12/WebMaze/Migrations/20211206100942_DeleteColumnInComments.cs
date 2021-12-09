using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class DeleteColumnInComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_News_CreatedNewsId",
                table: "NewsComments");

            migrationBuilder.RenameColumn(
                name: "CreatedNewsId",
                table: "NewsComments",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_CreatedNewsId",
                table: "NewsComments",
                newName: "IX_NewsComments_NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "NewsComments",
                newName: "CreatedNewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_NewsId",
                table: "NewsComments",
                newName: "IX_NewsComments_CreatedNewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_News_CreatedNewsId",
                table: "NewsComments",
                column: "CreatedNewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
