using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks
{
    public class Stock
    {
        public int ID { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Ticker { get; set; }

        [ForeignKey("Sector")]
        [Required]
        public int SectorID { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
