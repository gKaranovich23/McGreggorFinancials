using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses
{
    public class Expense
    {
        public int ID { get; set; }

        [DisplayName("Item Description")]
        [Required(ErrorMessage = "Please enter in a description")]
        public string Description { get; set; }

        [DisplayName("Amount Spent")]
        [Required(ErrorMessage = "Please enter in the amount spent.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive amount.")]
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Category")]
        [ForeignKey("ExpenseCategory")]
        public int ExpenseCategoryID { get; set; }

        public virtual ExpenseCategory ExpenseCategory { get; set; }

        public int PaymentMethodID { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
