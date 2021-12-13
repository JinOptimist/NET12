using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class NewColumnGameDevicesDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreaterId",
                table: "GameDevices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameDevices_CreaterId",
                table: "GameDevices",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameDevices_Users_CreaterId",
                table: "GameDevices",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameDevices_Users_CreaterId",
                table: "GameDevices");

            migrationBuilder.DropIndex(
                name: "IX_GameDevices_CreaterId",
                table: "GameDevices");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "GameDevices");
        }
    }
}
