using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Navigation
{
    public class Module
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string CssClasses { get; set; }
        public List<ModuleLink> Links { get; set; }
    }
}
