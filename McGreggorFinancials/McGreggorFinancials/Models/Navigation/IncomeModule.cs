using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class IncomeModule : Module
    {
        public IncomeModule()
        {
            Name = "Income";
            Controller = "Income";
            Links = new List<ModuleLink>
            {
                new ModuleLink
                {
                    Name = "Monthly Report",
                    Action = "MonthlyReport"
                },
                new ModuleLink
                {
                    Name = "Yearly Report",
                    Action = "YearlyReport"
                },
                new ModuleLink
                {
                    Name = "5YR Report",
                    Action = "FiveYRReport"
                },
                new ModuleLink
                {
                    Name = "All Income",
                    Action = "AllIncome"
                }
            };
        }
    }
}
