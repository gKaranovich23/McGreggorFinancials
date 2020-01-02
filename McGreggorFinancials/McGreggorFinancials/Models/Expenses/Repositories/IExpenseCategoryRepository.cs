using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public interface IExpenseCategoryRepository
    {
        IQueryable<ExpenseCategory> ExpenseCategories { get; }

        void Save(ExpenseCategory cat);
    }
}
