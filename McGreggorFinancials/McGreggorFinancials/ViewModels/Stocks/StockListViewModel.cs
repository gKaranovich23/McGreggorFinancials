using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Stocks
{
    public class StockListViewModel
    {
        public IEnumerable<StockViewModel> Stocks { get; set; }
        public DateTime Date { get; set; }
        public List<PieChartData> PieChartData { get; set; }
        public List<LineChartData> LineChartData { get; set; }
        public decimal AmountInvested { get; set; }
        public decimal StockGoal { get; set; }
        public decimal GoldGoal { get; set; }
        public decimal BondGoal { get; set; }
        public decimal StockGoalPercentage { get; set; }
        public decimal GoldGoalPercentage { get; set; }
        public decimal BondGoalPercentage { get; set; }
    }
}
