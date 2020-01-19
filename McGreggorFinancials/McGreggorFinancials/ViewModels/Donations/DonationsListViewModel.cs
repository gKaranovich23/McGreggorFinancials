using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Donations;
using McGreggorFinancials.Models.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Donations
{
    public class DonationsListViewModel
    {
        public IEnumerable<Donation> Donations { get; set; }
        public DateTime Date { get; set; }
        public List<PieChartData> PieChartData { get; set; }
        public List<LineChartData> LineChartData { get; set; }
        public decimal Total { get; set; }
        public decimal DonationsGoal { get; set; }
        public decimal DonationsPercentage { get; set; }
    }
}
