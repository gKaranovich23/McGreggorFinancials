using McGreggorFinancials.Models.Targets;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Admin
{
    public class TargetFormViewModel
    {
        public TargetAmount Target { get; set; }
        public SelectList TargetTypes { get; set; }
    }
}
