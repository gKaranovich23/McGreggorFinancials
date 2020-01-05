using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class StockModule : Module
    {
        public StockModule()
        {
            Name = "Stocks";
            Controller = "Stock";
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
                    Name = "Logged Activity",
                    Action = "SharesLog"
                }
            };
        }
    }
}
