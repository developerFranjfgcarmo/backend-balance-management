using Microsoft.EntityFrameworkCore.Migrations;

namespace BalanceManagement.Data.Migrations
{
    public partial class addcolumnIsDeletedToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Account",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Account");
        }
    }
}
