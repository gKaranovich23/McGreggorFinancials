using McGreggorFinancials.Models.Charting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Crypto
{
    public class CryptoListViewModel
    {
        public IEnumerable<CryptoViewModel> Stocks { get; set; }
        public DateTime Date { get; set; }
        public List<PieChartData> PieChartData { get; set; }
        public List<LineChartData> LineChartData { get; set; }
        public decimal AmountInvested { get; set; }
        public decimal CryptoGoal { get; set; }
        public decimal CryptoPercentage { get; set; }
    }
}
