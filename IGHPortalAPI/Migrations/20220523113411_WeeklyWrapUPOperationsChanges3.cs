using Microsoft.EntityFrameworkCore.Migrations;

namespace IGHportalAPI.Migrations
{
    public partial class WeeklyWrapUPOperationsChanges3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Out_of_order_room_notes",
                table: "WeeklyWrapUpOperations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Out_of_order_room_notes",
                table: "WeeklyWrapUpOperations");
        }
    }
}
