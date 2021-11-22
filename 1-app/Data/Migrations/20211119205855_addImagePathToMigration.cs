using Microsoft.EntityFrameworkCore.Migrations;

namespace _1_app.Data.Migrations
{
    public partial class addImagePathToMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "security",
                table: "Users");
        }
    }
}
