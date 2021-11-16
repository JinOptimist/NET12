using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class BugReportAddConnectionToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BugReports");

            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "BugReports",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BugReports_CreaterId",
                table: "BugReports",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_BugReports_Users_CreaterId",
                table: "BugReports",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugReports_Users_CreaterId",
                table: "BugReports");

            migrationBuilder.DropIndex(
                name: "IX_BugReports_CreaterId",
                table: "BugReports");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "BugReports");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BugReports",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
