using McGreggorFinancials.Models.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.ViewComponents
{
    public class DonationsVsGoalViewModel
    {
        public decimal DonationsTotal { get; set; }
        public TargetAmount DonationsGoal { get; set; }
    }
}
