using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perrmissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perrmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerrmissionUser",
                columns: table => new
                {
                    PerrmissionsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersWhichHasThePermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerrmissionUser", x => new { x.PerrmissionsId, x.UsersWhichHasThePermissionId });
                    table.ForeignKey(
                        name: "FK_PerrmissionUser_Perrmissions_PerrmissionsId",
                        column: x => x.PerrmissionsId,
                        principalTable: "Perrmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerrmissionUser_Users_UsersWhichHasThePermissionId",
                        column: x => x.UsersWhichHasThePermissionId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerrmissionUser_UsersWhichHasThePermissionId",
                table: "PerrmissionUser",
                column: "UsersWhichHasThePermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerrmissionUser");

            migrationBuilder.DropTable(
                name: "Perrmissions");
        }
    }
}
