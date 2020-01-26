using Microsoft.EntityFrameworkCore.Migrations;

namespace McGreggorFinancials.Migrations
{
    public partial class NumOfCoinType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NumOfCoins",
                table: "Coins",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumOfCoins",
                table: "Coins",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
