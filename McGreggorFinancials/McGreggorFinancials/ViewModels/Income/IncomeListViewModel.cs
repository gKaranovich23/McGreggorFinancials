using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Targets;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Incomes
{
    public class IncomeListViewModel
    {
        public IEnumerable<IncomeEntry> Incomes { get; set; }
        public DateTime Date { get; set; }
        public List<PieChartData> PieChartData { get; set; }
        public List<LineChartData> LineChartData { get; set; }
        public decimal Total { get; set; }
        public TargetAmount Target { get; set; }
    }
}
