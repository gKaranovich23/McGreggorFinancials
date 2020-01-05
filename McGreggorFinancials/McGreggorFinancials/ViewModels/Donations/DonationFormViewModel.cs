using McGreggorFinancials.Models.Donations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Donations
{
    public class DonationFormViewModel
    {
        public Donation Donation { get; set; }
        public SelectList Charities { get; set; }
        public SelectList PaymentMethods { get; set; }

        public DateTime CurrDateLoc { get; set; }
    }
}
