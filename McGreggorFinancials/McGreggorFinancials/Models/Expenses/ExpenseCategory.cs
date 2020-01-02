using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses
{
    public class ExpenseCategory
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
