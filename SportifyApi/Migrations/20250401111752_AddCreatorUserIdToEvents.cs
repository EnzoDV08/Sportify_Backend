using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportifyApi.Migrations
{
    public partial class AddCreatorUserIdToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Events",
                type: "integer",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatorUserId",
                table: "Events",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_CreatorUserId",
                table: "Events",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_CreatorUserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CreatorUserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Events");
        }
    }
}
