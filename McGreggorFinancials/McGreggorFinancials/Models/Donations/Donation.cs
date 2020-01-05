using McGreggorFinancials.Models.Expenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Donations
{
    public class Donation
    {
        public int ID { get; set; }

        [DisplayName("Item Description")]
        [Required(ErrorMessage = "Please enter in a description")]
        public string Description { get; set; }

        [DisplayName("Amount Donated")]
        [Required(ErrorMessage = "Please enter in the amount donated.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive amount.")]
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Category")]
        [ForeignKey("Charity")]
        public int CharityID { get; set; }

        public virtual Charity Charity { get; set; }

        [DisplayName("Payment Method")]
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodID { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
