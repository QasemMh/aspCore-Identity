using Microsoft.EntityFrameworkCore.Migrations;

namespace _1_app.Data.Migrations
{
    public partial class SetAdminUserToAllRoot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into [security].[UserRole] SELECT 'cffaf2af-f885-408e-b59a-c1c3e05876b1', Id from [security].Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRole] WHERE UserId='cffaf2af-f885-408e-b59a-c1c3e05876b1' ");
        }
    }
}
