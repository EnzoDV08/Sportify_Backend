using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportifyApi.Migrations
{
    public partial class RecreateProfileAndAdminTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Profiles table
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", "Identity"), // Correct way to specify identity
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.user_id);
                });

            // Create Admins table
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", "Identity"), // Correct way to specify identity
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.admin_id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop Admins table
            migrationBuilder.DropTable(
                name: "Admins");

            // Drop Profiles table
            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
