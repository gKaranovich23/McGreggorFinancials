using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class HomeModule : Module
    {
        public HomeModule()
        {
            Name = "Dashboard";
            Controller = "Home";
            Action = "Dashboard";
            Links = new List<ModuleLink>();
        }
    }
}
