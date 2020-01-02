using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private ApplicationDbContext context;

        public ExpenseRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Expense> Expenses => context.Expenses.Include(e => e.ExpenseCategory)
            .Include(x => x.PaymentMethod).Include(x => x.Receipt);

        public int Save(Expense e)
        {
            if (e.ID == 0)
            {
                context.Expenses.Add(e);
            }
            else
            {
                Expense dbEntry = context.Expenses.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Description = e.Description;
                    dbEntry.Amount = e.Amount;
                    dbEntry.Date = e.Date;
                    dbEntry.ExpenseCategoryID = e.ExpenseCategoryID;
                }
            }
            context.SaveChanges();

            return e.ID;
        }

        public Expense Delete(int expenseId)
        {
            Expense dbEntry = context.Expenses.FirstOrDefault(e => e.ID == expenseId);

            if (dbEntry != null)
            {
                context.Expenses.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
