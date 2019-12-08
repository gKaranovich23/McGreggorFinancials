using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Account> Accounts => _context.Accounts.Include(x => x.Type).Include(x => x.TargetAmount);

        public void Save(Account a)
        {
            if (a.ID == 0)
            {
                _context.Accounts.Add(a);
            }
            else
            {
                Account dbEntry = _context.Accounts.FirstOrDefault(x => x.ID == a.ID);
                if (dbEntry != null)
                {
                    dbEntry.Amount = a.Amount;
                    dbEntry.TypeID = a.TypeID;
                    dbEntry.TargetID = a.TargetID;
                }
            }
            _context.SaveChanges();
        }
    }
}
