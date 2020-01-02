using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ExpenseCategory> ExpenseCategories => _context.ExpenseCategories;

        public void Save(ExpenseCategory e)
        {
            if (e.ID == 0)
            {
                _context.ExpenseCategories.Add(e);
            }
            else
            {
                ExpenseCategory dbEntry = _context.ExpenseCategories.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = e.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}
