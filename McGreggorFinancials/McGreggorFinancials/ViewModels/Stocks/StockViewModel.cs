using McGreggorFinancials.Models.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Stocks
{
    public class StockViewModel
    {
        public Stock Stock { get; set; }
        public double CurrentValue { get; set; }
        public int TotalNumOfShares { get; set; }
        public double TotalValue { get; set; }
    }
}
