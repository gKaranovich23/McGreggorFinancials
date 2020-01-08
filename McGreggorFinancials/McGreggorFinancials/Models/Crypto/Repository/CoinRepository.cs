using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Crypto.Repository
{
    public class CoinRepository : ICoinRepository
    {
        private ApplicationDbContext context;

        public CoinRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Coin> Coins => context.Coins.Include(e => e.CryptoCurrency);

        public void Save(Coin e)
        {
            if (e.ID == 0)
            {
                context.Coins.Add(e);
            }
            else
            {
                Coin dbEntry = context.Coins.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.NumOfCoins = e.NumOfCoins;
                    dbEntry.PurchasePrice = e.PurchasePrice;
                    dbEntry.Date = e.Date;
                    dbEntry.CryptoCurrencyID = e.CryptoCurrencyID;
                }
            }
            context.SaveChanges();
        }

        public Coin Delete(int id)
        {
            Coin dbEntry = context.Coins.FirstOrDefault(e => e.ID == id);

            if (dbEntry != null)
            {
                context.Coins.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
