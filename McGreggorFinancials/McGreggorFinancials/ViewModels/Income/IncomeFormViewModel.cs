using McGreggorFinancials.Models.Income;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Incomes
{
    public class IncomeFormViewModel
    {
        public IncomeEntry Income { get; set; }
        public SelectList IncomeCategories { get; set; }
        public DateTime CurrDateLoc { get; set; }
    }
}
