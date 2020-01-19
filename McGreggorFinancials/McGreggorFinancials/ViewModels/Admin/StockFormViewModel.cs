using McGreggorFinancials.Models.Stocks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Admin
{
    public class StockFormViewModel
    {
        public Stock Stock { get; set; }
        public SelectList Sectors { get; set; }
    }
}
