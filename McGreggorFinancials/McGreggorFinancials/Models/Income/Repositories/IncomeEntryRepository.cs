using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Income.Repositories
{
    public class IncomeEntryRepository : IIncomeEntryRespository
    {
        private ApplicationDbContext context;

        public IncomeEntryRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<IncomeEntry> IncomeEntries => context.IncomeEntries.Include(i => i.Category);

        public void Save(IncomeEntry i)
        {
            if (i.ID == 0)
            {
                context.IncomeEntries.Add(i);
            }
            else
            {
                IncomeEntry dbEntry = context.IncomeEntries.FirstOrDefault(x => x.ID == i.ID);
                if (dbEntry != null)
                {
                    dbEntry.Description = i.Description;
                    dbEntry.Amount = i.Amount;
                    dbEntry.Date = i.Date;
                    dbEntry.CategoryID = i.CategoryID;
                }
            }
            context.SaveChanges();
        }

        public IncomeEntry Delete(int incomeId)
        {
            IncomeEntry dbEntry = context.IncomeEntries.FirstOrDefault(i => i.ID == incomeId);

            if (dbEntry != null)
            {
                context.IncomeEntries.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
