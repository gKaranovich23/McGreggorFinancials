using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Accounts.Repositories
{
    public interface IAccountRepository
    {
        IQueryable<Account> Accounts { get; }

        void Save(Account a);
    } 
}
