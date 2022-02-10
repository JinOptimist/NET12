using Microsoft.EntityFrameworkCore.Migrations;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Migrations
{
    public partial class AddLocale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultLocale",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: Language.Ru);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultLocale",
                table: "Users");
        }
    }
}
