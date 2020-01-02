using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses
{
    public class PaymentMethod
    {
        public int ID { get; set; }

        [Required]
        public string Method { get; set; }
        public bool IsCredit { get; set; }

        public virtual CreditBalance CreditBalance { get; set; }
    }
}
