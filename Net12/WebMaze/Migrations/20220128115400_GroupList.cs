using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class GroupList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GroupListId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupList_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupListId",
                table: "Users",
                column: "GroupListId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupList_CreatorId",
                table: "GroupList",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GroupList_GroupListId",
                table: "Users",
                column: "GroupListId",
                principalTable: "GroupList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_GroupList_GroupListId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GroupList");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupListId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupListId",
                table: "Users");
        }
    }
}
