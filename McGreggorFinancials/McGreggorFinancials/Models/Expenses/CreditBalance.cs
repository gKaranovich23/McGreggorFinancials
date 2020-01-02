using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses
{
    public class CreditBalance
    {
        public int ID { get; set; }

        public double Amount { get; set; }

        public int PaymentMethodID { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
