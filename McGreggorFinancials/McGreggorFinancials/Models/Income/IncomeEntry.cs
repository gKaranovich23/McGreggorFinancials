using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McGreggorFinancials.Models.Income
{
    public class IncomeEntry
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter in a description")]
        public string Description { get; set; }

        [DisplayName("Amount Received")]
        [Required(ErrorMessage = "Please enter in the amount received.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive amount.")]
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Category")]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual IncomeCategory Category { get; set; }
    }
}
