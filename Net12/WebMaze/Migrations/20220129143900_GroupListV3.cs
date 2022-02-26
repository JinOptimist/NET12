using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class GroupListV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersInGroup");

            migrationBuilder.CreateTable(
                name: "UserInGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInGroup_GroupList_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInGroup_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInGroup_GroupId",
                table: "UserInGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInGroup_UserId",
                table: "UserInGroup",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInGroup");

            migrationBuilder.CreateTable(
                name: "UsersInGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersInGroup_GroupList_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersInGroup_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_GroupId",
                table: "UsersInGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup",
                column: "UserId");
        }
    }
}
