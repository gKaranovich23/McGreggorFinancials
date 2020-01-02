using McGreggorFinancials.Models.Expenses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Admin
{
    public class PayOffCreditViewModel
    {
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public double AmountToPay { get; set; }
    }
}
