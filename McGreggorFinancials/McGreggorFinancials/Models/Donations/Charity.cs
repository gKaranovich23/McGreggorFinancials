using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Donations
{
    public class Charity
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
