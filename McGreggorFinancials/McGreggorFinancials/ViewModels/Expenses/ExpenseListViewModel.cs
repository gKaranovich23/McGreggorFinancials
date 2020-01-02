using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Expenses
{
    public class ExpenseListViewModel
    {
        public IEnumerable<Expense> Expenses { get; set; }
        public DateTime Date { get; set; }
        public List<PieChartData> PieChartData { get; set; }
        public List<LineChartData> LineChartData { get; set; }
        public decimal Total { get; set; }
        public TargetAmount Target { get; set; }
    }
}
