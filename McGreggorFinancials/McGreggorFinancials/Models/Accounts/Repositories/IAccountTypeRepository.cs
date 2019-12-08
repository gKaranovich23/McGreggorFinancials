using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Accounts.Repositories
{
    public interface IAccountTypeRepository
    {
        IQueryable<AccountType> AccountTypes { get; }

        void Save(AccountType type);
    }
}
