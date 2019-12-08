using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Income.Repositories
{
    public class IncomeCategoryRepository : IIncomeCategoryRepository
    {
        private ApplicationDbContext _context;

        public IncomeCategoryRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<IncomeCategory> Categories => _context.IncomeCategories;

        public void Save(IncomeCategory c)
        {
            if (c.ID == 0)
            {
                _context.IncomeCategories.Add(c);
            }
            else
            {
                IncomeCategory dbEntry = _context.IncomeCategories.FirstOrDefault(x => x.ID == c.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = c.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}
