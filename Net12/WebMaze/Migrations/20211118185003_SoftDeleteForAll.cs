using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class SoftDeleteForAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsActive",
            //    table: "News",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NewCellSuggestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "IsActive",
            //    table: "News");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NewCellSuggestions");
        }
    }
}
