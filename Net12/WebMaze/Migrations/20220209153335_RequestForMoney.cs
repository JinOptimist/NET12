using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMaze.Migrations
{
    public partial class RequestForMoney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestForMoneys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestAmount = table.Column<int>(type: "int", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    RequestCreatorId = table.Column<long>(type: "bigint", nullable: true),
                    RequestRecipientId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForMoneys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestForMoneys_Users_RequestCreatorId",
                        column: x => x.RequestCreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestForMoneys_Users_RequestRecipientId",
                        column: x => x.RequestRecipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestForMoneys_RequestCreatorId",
                table: "RequestForMoneys",
                column: "RequestCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForMoneys_RequestRecipientId",
                table: "RequestForMoneys",
                column: "RequestRecipientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestForMoneys");
        }
    }
}
