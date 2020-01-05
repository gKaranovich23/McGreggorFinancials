using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Donations.Repository
{
    public class CharityRepository : ICharityRepository
    {
        private readonly ApplicationDbContext _context;

        public CharityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Charity> Charities => _context.Charities;

        public void Save(Charity e)
        {
            if (e.ID == 0)
            {
                _context.Charities.Add(e);
            }
            else
            {
                Charity dbEntry = _context.Charities.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = e.Name;
                }
            }
            _context.SaveChanges();
        }

        public Charity Delete(int id)
        {
            Charity dbEntry = _context.Charities.FirstOrDefault(e => e.ID == id);

            if (dbEntry != null)
            {
                _context.Charities.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
