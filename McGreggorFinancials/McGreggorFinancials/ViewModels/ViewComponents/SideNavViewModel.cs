using McGreggorFinancials.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.ViewComponents
{
    public class SideNavViewModel
    {
        public SideNavViewModel()
        {
            Modules = new List<Module>
            {
                new HomeModule(),
                new AdminModule(),
                new CreditModule(),
                new DonationModule(),
                new ExpenseModule(),
                new IncomeModule(),
                new StockModule()
            };
        }

        public List<Module> Modules { get; set; }
        public string CurrentController { get; set; }
        public string CurrentAction { get; set; }
    }
}
