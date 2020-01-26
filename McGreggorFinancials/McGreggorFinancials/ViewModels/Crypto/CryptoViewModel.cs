using McGreggorFinancials.Models.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Crypto
{
    public class CryptoViewModel
    {
        public CryptoCurrency Currency { get; set; }
        public double CurrentValue { get; set; }
        public decimal TotalNumOfCoins { get; set; }
        public double TotalValue { get; set; }
    }
}
