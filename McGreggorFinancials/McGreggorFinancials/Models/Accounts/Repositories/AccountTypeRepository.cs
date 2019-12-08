using McGreggorFinancials.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Accounts.Repositories
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<AccountType> AccountTypes => _context.AccountTypes;

        public void Save(AccountType e)
        {
            if (e.ID == 0)
            {
                _context.AccountTypes.Add(e);
            }
            else
            {
                AccountType dbEntry = _context.AccountTypes.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = e.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}
