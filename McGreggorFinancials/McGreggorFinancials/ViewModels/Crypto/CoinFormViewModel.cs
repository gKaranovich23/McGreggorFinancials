using McGreggorFinancials.Models.Crypto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Crypto
{
    public class CoinFormViewModel
    {
        public Coin Coin { get; set; }
        public SelectList CryptoCurrencies { get; set; }
        public DateTime CurrDateLoc { get; set; }
        public string ReturnUrl { get; set; }
    }
}
