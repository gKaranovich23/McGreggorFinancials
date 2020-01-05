using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Stock> Stocks => _context.Stocks.Include(e => e.Sector);

        public void Save(Stock e)
        {
            if (e.ID == 0)
            {
                _context.Stocks.Add(e);
            }
            else
            {
                Stock dbEntry = _context.Stocks.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.SectorID = e.SectorID;
                    dbEntry.Ticker = e.Ticker;
                    dbEntry.Company = e.Company;
                }
            }
            _context.SaveChanges();
        }
    }
}
