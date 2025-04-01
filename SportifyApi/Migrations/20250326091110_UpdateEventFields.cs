using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportifyApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Comment out or remove the lines trying to add existing columns
        // migrationBuilder.AddColumn<string>(
        //     name: "Status",
        //     table: "Events",
        //     type: "text",
        //     nullable: true);

        // migrationBuilder.AddColumn<string>(
        //     name: "Type",
        //     table: "Events",
        //     type: "text",
        //     nullable: true);

        // migrationBuilder.AddColumn<string>(
        //     name: "Visibility",
        //     table: "Events",
        //     type: "text",
        //     nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // These drop commands should remain because if you roll back, you need to drop the columns
        migrationBuilder.DropColumn(
            name: "Status",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "Visibility",
            table: "Events");

        // Optionally, if you need to rename the column "event_id" back to "Id" during rollback:
        // migrationBuilder.RenameColumn(
        //     name: "event_id",
        //     table: "Events",
        //     newName: "Id");

        // migrationBuilder.AddColumn<string>(
        //     name: "Description",
        //     table: "Events",
        //     type: "text",
        //     nullable: false,
        //     defaultValue: "");
    }
}


}
