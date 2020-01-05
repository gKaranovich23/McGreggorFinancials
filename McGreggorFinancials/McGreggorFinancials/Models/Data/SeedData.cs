using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Donations;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Targets;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (!context.TargetTypes.Any())
            {
                context.TargetTypes.AddRange(
                    new TargetType { Name = "Expenses" },
                    new TargetType { Name = "Charity" },
                    new TargetType { Name = "Stock" },
                    new TargetType { Name = "Personal Savings" }
                );

                context.SaveChanges();
            }

            if (!context.TargetAmounts.Any())
            {
                context.TargetAmounts.AddRange(
                    new TargetAmount
                    {
                        Amount = 1000,
                        TypeID = context.TargetTypes.Where(g => g.Name.Equals("Expenses")).First().ID
                    },
                    new TargetAmount
                    {
                        Amount = 100,
                        TypeID = context.TargetTypes.Where(g => g.Name.Equals("Charity")).First().ID
                    },
                    new TargetAmount
                    {
                        Amount = 400,
                        TypeID = context.TargetTypes.Where(g => g.Name.Equals("Stock")).First().ID
                    },
                    new TargetAmount
                    {
                        Amount = 400,
                        TypeID = context.TargetTypes.Where(g => g.Name.Equals("Personal Savings")).First().ID
                    }
                );

                context.SaveChanges();
            }

            if (!context.AccountTypes.Any())
            {
                context.AccountTypes.AddRange(
                    new AccountType { Name = "Personal" },
                    new AccountType { Name = "Investments" }
                );

                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(
                    new Account
                    {
                        Amount = 1000,
                        TypeID = context.AccountTypes.Where(x => x.Name.Equals("Personal")).FirstOrDefault().ID,
                        TargetID = context.TargetAmounts.Where(x => x.TypeID == context.TargetTypes.Where(y => y.Name.Equals("Personal Savings")).FirstOrDefault().ID).FirstOrDefault().ID
                    }
                );

                context.SaveChanges();
            }

            if (!context.IncomeCategories.Any())
            {
                context.IncomeCategories.AddRange(
                        new IncomeCategory { Name = "Amazon Salary" },
                        new IncomeCategory { Name = "Gift" }
                    );
                context.SaveChanges();
            }

            if (!context.IncomeEntries.Any())
            {
                context.IncomeEntries.AddRange(
                        new IncomeEntry
                        {
                            Description = "Paycheck #1",
                            Amount = 2000.00,
                            Date = DateTime.Now,
                            CategoryID = context.IncomeCategories.Where(i => i.Name.Equals("Amazon Salary")).First().ID
                        },
                        new IncomeEntry
                        {
                            Description = "Birthday Gift",
                            Amount = 100.00,
                            Date = DateTime.Now,
                            CategoryID = context.IncomeCategories.Where(i => i.Name.Equals("Gift")).First().ID
                        }
                    );

                context.SaveChanges();
            }

            if (!context.ExpenseCategories.Any())
            {
                context.ExpenseCategories.AddRange(
                        new ExpenseCategory { Name = "Rent" },
                        new ExpenseCategory { Name = "Groceries" },
                        new ExpenseCategory { Name = "Phone" },
                        new ExpenseCategory { Name = "Gas" },
                        new ExpenseCategory { Name = "Electricity" },
                        new ExpenseCategory { Name = "Health/Fitness" }
                    );

                context.SaveChanges();
            }

            if (!context.PaymentMethods.Any())
            {
                context.PaymentMethods.AddRange(
                        new PaymentMethod { Method = "Cash", IsCredit = false },
                        new PaymentMethod { Method = "Discover Card", IsCredit = true },
                        new PaymentMethod { Method = "Visa Card", IsCredit = true }
                    );

                context.SaveChanges();
            }

            if (!context.CreditBalance.Any())
            {
                context.CreditBalance.AddRange(
                        new CreditBalance { Amount = 0.00, PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Discover Card")).FirstOrDefault().ID },
                        new CreditBalance { Amount = 0.00, PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Visa Card")).FirstOrDefault().ID }
                    );

                context.SaveChanges();
            }

            if (!context.Expenses.Any())
            {
                context.Expenses.AddRange(
                    new Expense
                    {
                        Description = "Rent",
                        Amount = 704,
                        Date = DateTime.Now,
                        ExpenseCategoryID = context.ExpenseCategories.Where(c => c.Name == "Rent").First().ID,
                        PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Discover Card")).First().ID
                    },
                    new Expense
                    {
                        Description = "Groceries",
                        Amount = 100,
                        Date = DateTime.Now,
                        ExpenseCategoryID = context.ExpenseCategories.Where(c => c.Name == "Groceries").First().ID,
                        PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Discover Card")).First().ID
                    },
                    new Expense
                    {
                        Description = "Gym",
                        Amount = 40,
                        Date = DateTime.Now,
                        ExpenseCategoryID = context.ExpenseCategories.Where(c => c.Name == "Health/Fitness").First().ID,
                        PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Discover Card")).First().ID
                    }
                );

                context.SaveChanges();
            }

            if (!context.Charities.Any())
            {
                context.Charities.AddRange(
                        new Charity { Name = "Fuck Cancer" },
                        new Charity { Name = "Humane Society" }
                    );

                context.SaveChanges();
            }

            if (!context.Donations.Any())
            {
                context.Donations.AddRange(
                        new Donation
                        {
                            Description = "Fuck Cancer Donation",
                            Amount = 50.00,
                            Date = DateTime.Now,
                            CharityID = context.Charities.Where(i => i.Name.Equals("Fuck Cancer")).First().ID,
                            PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Discover Card")).First().ID
                        },
                        new Donation
                        {
                            Description = "Humane Society Donation",
                            Amount = 50.00,
                            Date = DateTime.Now,
                            CharityID = context.Charities.Where(i => i.Name.Equals("Humane Society")).First().ID,
                            PaymentMethodID = context.PaymentMethods.Where(x => x.Method.Equals("Discover Card")).First().ID
                        }
                    );

                context.SaveChanges();
            }
        }
    }
}
