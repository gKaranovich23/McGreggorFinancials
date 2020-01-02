using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public class CreditBalanceRepository : ICreditBalanceRepository
    {
        private ApplicationDbContext context;

        public CreditBalanceRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<CreditBalance> CreditBalance => context.CreditBalance;

        public void Save(CreditBalance e)
        {
            if (e.ID == 0)
            {
                context.CreditBalance.Add(e);
            }
            else
            {
                CreditBalance dbEntry = context.CreditBalance.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Amount = e.Amount;
                }
            }
            context.SaveChanges();
        }

        public CreditBalance Delete(int id)
        {
            CreditBalance dbEntry = context.CreditBalance.FirstOrDefault(e => e.ID == id);

            if (dbEntry != null)
            {
                context.CreditBalance.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
