using McGreggorFinancials.Models.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.ViewComponents
{
    public class ExpenseVsBudgetViewModel
    {
        public decimal ExpenseTotal { get; set; }
        public TargetAmount BudgetGoal { get; set; }
    }
}
