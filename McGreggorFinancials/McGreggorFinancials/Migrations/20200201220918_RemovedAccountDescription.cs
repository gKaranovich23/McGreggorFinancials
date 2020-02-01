using Microsoft.EntityFrameworkCore.Migrations;

namespace McGreggorFinancials.Migrations
{
    public partial class RemovedAccountDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accounts",
                nullable: true);
        }
    }
}
