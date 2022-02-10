using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class GroupList3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInGroup_Users_UserId",
                table: "UsersInGroup");

            migrationBuilder.DropTable(
                name: "GroupListUsersInGroup");

            migrationBuilder.DropIndex(
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UsersInGroup",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "UsersInGroup",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_GroupId",
                table: "UsersInGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInGroup_GroupList_GroupId",
                table: "UsersInGroup",
                column: "GroupId",
                principalTable: "GroupList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInGroup_Users_UserId",
                table: "UsersInGroup",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInGroup_GroupList_GroupId",
                table: "UsersInGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInGroup_Users_UserId",
                table: "UsersInGroup");

            migrationBuilder.DropIndex(
                name: "IX_UsersInGroup_GroupId",
                table: "UsersInGroup");

            migrationBuilder.DropIndex(
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "UsersInGroup");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UsersInGroup",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupListUsersInGroup_UsersId",
                table: "GroupListUsersInGroup",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInGroup_Users_UserId",
                table: "UsersInGroup",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
