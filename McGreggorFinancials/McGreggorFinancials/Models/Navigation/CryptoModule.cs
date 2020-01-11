using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class CryptoModule : Module
    {
        public CryptoModule()
        {
            Name = "Crypte";
            Controller = "Crypto";
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
                    Action = "CoinLog"
                }
            };
        }
    }
}
