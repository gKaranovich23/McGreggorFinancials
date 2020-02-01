using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Crypto
{
    public class CryptoCurrency
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Ticker { get; set; }
    }
}
