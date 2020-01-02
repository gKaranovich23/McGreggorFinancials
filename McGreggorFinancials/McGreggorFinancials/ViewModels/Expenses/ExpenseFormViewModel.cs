using McGreggorFinancials.Models.Expenses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Expenses
{
    public class ExpenseFormViewModel
    {
        public Expense Expense { get; set; }
        public SelectList ExpenseCategories { get; set; }
        public SelectList PaymentMethods { get; set; }
        public DateTime CurrDateLoc { get; set; }
    }
}
