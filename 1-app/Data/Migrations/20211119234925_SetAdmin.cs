using Microsoft.EntityFrameworkCore.Migrations;

namespace _1_app.Data.Migrations
{
    public partial class SetAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ImagePath]) VALUES (N'cffaf2af-f885-408e-b59a-c1c3e05876b1', N'Admin', N'ADMIN', N'Admin@gmail.com', N'ADMIN@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEMUvOy8pm9ZcZUkAFYWCLVrcPiAV2CB8zAmd2P5nnlMdueOSbKBxtYARPSTvg6KMgg==', N'T677G7TYSFXSUATYJA3SAWO7E2ZZCYCY', N'57492508-b147-452c-b23a-4b26787608dd', NULL, 0, 0, NULL, 1, 0, N'Qasem', N'Bayan', NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] where id='cffaf2af-f885-408e-b59a-c1c3e05876b1' ");
        }
    }
}
