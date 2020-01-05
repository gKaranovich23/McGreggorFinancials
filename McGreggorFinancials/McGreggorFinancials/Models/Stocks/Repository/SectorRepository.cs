using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks.Repository
{
    public class SectorRepository : ISectorRepository
    {
        private readonly ApplicationDbContext _context;

        public SectorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Sector> Sectors => _context.Sectors;

        public void Save(Sector e)
        {
            if (e.ID == 0)
            {
                _context.Sectors.Add(e);
            }
            else
            {
                Sector dbEntry = _context.Sectors.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = e.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}
