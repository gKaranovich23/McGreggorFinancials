using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks
{
    public class Share
    {
        public int ID { get; set; }

        [Required]
        public int NumOfShares { get; set; }

        [Required]
        public double PurchasePrice { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("Stock")]
        public int StockID { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
