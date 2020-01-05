using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class CreditModule : Module
    {
        public CreditModule()
        {
            Controller = "Credit";
            Name = "Credit";
            Links = new List<ModuleLink>
            {
                new ModuleLink
                {
                    Action = "CreditCards",
                    Name = "Credit Cards"
                }
            };
        }
    }
}
