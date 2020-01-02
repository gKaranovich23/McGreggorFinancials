using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public interface IExpenseRepository
    {
        IQueryable<Expense> Expenses { get; }

        int Save(Expense expense);

        Expense Delete(int expenseId);
    }
}
