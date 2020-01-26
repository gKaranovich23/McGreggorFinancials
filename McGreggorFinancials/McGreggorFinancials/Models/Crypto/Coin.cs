using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Crypto
{
    public class Coin
    {
        public int ID { get; set; }

        [Required]
        public decimal NumOfCoins { get; set; }

        [Required]
        public double PurchasePrice { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("CryptoCurrency")]
        public int CryptoCurrencyID { get; set; }
        public virtual CryptoCurrency CryptoCurrency { get; set; }
    }
}
