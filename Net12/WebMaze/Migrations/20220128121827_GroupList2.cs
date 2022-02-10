using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class GroupList2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_GroupList_GroupListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupListId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupListId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UsersInGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersInGroup_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupListUsersInGroup",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupListUsersInGroup", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupListUsersInGroup_GroupList_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "GroupList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupListUsersInGroup_UsersInGroup_UsersId",
                        column: x => x.UsersId,
                        principalTable: "UsersInGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupListUsersInGroup_UsersId",
                table: "GroupListUsersInGroup",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupListUsersInGroup");

            migrationBuilder.DropTable(
                name: "UsersInGroup");

            migrationBuilder.AddColumn<long>(
                name: "GroupListId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupListId",
                table: "Users",
                column: "GroupListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GroupList_GroupListId",
                table: "Users",
                column: "GroupListId",
                principalTable: "GroupList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
