using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Targets.Repositories
{
    public class TargetTypeRepository : ITargetTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TargetTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TargetType> TargetTypes => _context.TargetTypes;

        public void Save(TargetType e)
        {
            if (e.ID == 0)
            {
                _context.TargetTypes.Add(e);
            }
            else
            {
                TargetType dbEntry = _context.TargetTypes.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = e.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}
