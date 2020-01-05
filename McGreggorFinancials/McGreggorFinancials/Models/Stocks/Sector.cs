using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks
{
    public class Sector
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
