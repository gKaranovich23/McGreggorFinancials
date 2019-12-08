using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace McGreggorFinancials.Migrations
{
    public partial class AccountsIncomeTargetsAndMore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IncomeCategories",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IncomeEntries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeEntries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IncomeEntries_IncomeCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "IncomeCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TargetAmounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(nullable: false),
                    Percentage = table.Column<int>(nullable: false),
                    TypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAmounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TargetAmounts_TargetTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "TargetTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    TypeID = table.Column<int>(nullable: false),
                    TargetID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Accounts_TargetAmounts_TargetID",
                        column: x => x.TargetID,
                        principalTable: "TargetAmounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "AccountTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TargetID",
                table: "Accounts",
                column: "TargetID");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TypeID",
                table: "Accounts",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntries_CategoryID",
                table: "IncomeEntries",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TargetAmounts_TypeID",
                table: "TargetAmounts",
                column: "TypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "IncomeEntries");

            migrationBuilder.DropTable(
                name: "TargetAmounts");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "TargetTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IncomeCategories",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
