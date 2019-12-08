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
                    Action = "IncomeCategories",
                    Name = "Income Categories"
                }
            };
        }
    }
}
