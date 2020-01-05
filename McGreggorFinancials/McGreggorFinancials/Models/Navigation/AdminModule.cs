using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class AdminModule : Module
    {
        public AdminModule()
        {
            Controller = "Admin";
            Name = "Admin";
            Links = new List<ModuleLink>
            {
                new ModuleLink
                {
                    Action = "ExpenseCategories",
                    Name = "Expense Categories"
                },
                new ModuleLink
                {
                    Action = "Targets",
                    Name = "Targets"
                },
                new ModuleLink
                {
                    Action = "TargetTypes",
                    Name = "Target Types"
                },
                new ModuleLink
                {
                    Action = "IncomeCategories",
                    Name = "Income Categories"
                },
                new ModuleLink
                {
                    Action = "PaymentMethods",
                    Name = "Payment Methods"
                },
                new ModuleLink
                {
                    Action = "Accounts",
                    Name = "Accounts"
                },
                new ModuleLink
                {
                    Action = "AccountTypes",
                    Name = "Account Types"
                }
            };
        }
    }
}
