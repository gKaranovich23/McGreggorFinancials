using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace McGreggorFinancials.Migrations
{
    public partial class Expenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Method = table.Column<string>(nullable: false),
                    IsCredit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CreditBalance",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    PaymentMethodID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditBalance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CreditBalance_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ExpenseCategoryID = table.Column<int>(nullable: false),
                    PaymentMethodID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCategories_ExpenseCategoryID",
                        column: x => x.ExpenseCategoryID,
                        principalTable: "ExpenseCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditBalance_PaymentMethodID",
                table: "CreditBalance",
                column: "PaymentMethodID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseCategoryID",
                table: "Expenses",
                column: "ExpenseCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PaymentMethodID",
                table: "Expenses",
                column: "PaymentMethodID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditBalance");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "PaymentMethods");
        }
    }
}
