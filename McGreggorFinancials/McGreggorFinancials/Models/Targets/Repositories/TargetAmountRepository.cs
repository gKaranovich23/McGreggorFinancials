using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Targets.Repositories
{
    public class TargetAmountRepository : ITargetAmountRepository
    {
        private ApplicationDbContext context;

        public TargetAmountRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<TargetAmount> TargetAmounts => context.TargetAmounts.Include(g => g.TargetType);

        public void Save(TargetAmount g)
        {
            if (g.ID == 0)
            {
                context.TargetAmounts.Add(g);
            }
            else
            {
                TargetAmount dbEntry = context.TargetAmounts.FirstOrDefault(x => x.ID == g.ID);
                if (dbEntry != null)
                {
                    dbEntry.Amount = g.Amount;
                    dbEntry.Percentage = g.Percentage;
                    dbEntry.TypeID = g.TypeID;
                }
            }
            context.SaveChanges();
        }

        public TargetAmount Delete(int goalId)
        {
            TargetAmount dbEntry = context.TargetAmounts.FirstOrDefault(g => g.ID == goalId);

            if (dbEntry != null)
            {
                context.TargetAmounts.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
