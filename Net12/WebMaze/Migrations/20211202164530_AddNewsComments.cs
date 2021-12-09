using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class AddNewsComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedNewsId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsComments_News_CreatedNewsId",
                        column: x => x.CreatedNewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsComments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_AuthorId",
                table: "NewsComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_CreatedNewsId",
                table: "NewsComments",
                column: "CreatedNewsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsComments");
        }
    }
}
