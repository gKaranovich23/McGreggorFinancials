using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Crypto.Repository
{
    public class CryptoCurrencyRepository : ICryptoCurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CryptoCurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<CryptoCurrency> CryptoCurrencies => _context.CryptoCurrencies;

        public void Save(CryptoCurrency e)
        {
            if (e.ID == 0)
            {
                _context.CryptoCurrencies.Add(e);
            }
            else
            {
                CryptoCurrency dbEntry = _context.CryptoCurrencies.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Ticker = e.Ticker;
                    dbEntry.Name = e.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}