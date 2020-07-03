using McGreggorFinancials.Models.Stocks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Stocks
{
    public class ShareFormViewModel
    {
        public Share Share { get; set; }
        public SelectList Stocks { get; set; }
        public DateTime CurrDateLoc { get; set; }
        public string ReturnUrl { get; set; }

        public int StockID { get; set; }
        public String StockCompany { get; set; }
    }
}
