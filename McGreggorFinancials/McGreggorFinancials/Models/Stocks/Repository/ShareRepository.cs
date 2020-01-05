using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks.Repository
{
    public class ShareRepository : IShareRepository
    {
        private ApplicationDbContext context;

        public ShareRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Share> Shares => context.Shares.Include(e => e.Stock);

        public void Save(Share e)
        {
            if (e.ID == 0)
            {
                context.Shares.Add(e);
            }
            else
            {
                Share dbEntry = context.Shares.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.NumOfShares = e.NumOfShares;
                    dbEntry.PurchasePrice = e.PurchasePrice;
                    dbEntry.Date = e.Date;
                    dbEntry.StockID = e.StockID;
                }
            }
            context.SaveChanges();
        }

        public Share Delete(int shareId)
        {
            Share dbEntry = context.Shares.FirstOrDefault(e => e.ID == shareId);

            if (dbEntry != null)
            {
                context.Shares.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
