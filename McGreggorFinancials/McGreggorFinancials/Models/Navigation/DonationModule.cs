using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class DonationModule : Module
    {
        public DonationModule()
        {
            Name = "Donations";
            Controller = "Donation";
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
                    Name = "All Donations",
                    Action = "AllDonations"
                }
            };
        }
    }
}
