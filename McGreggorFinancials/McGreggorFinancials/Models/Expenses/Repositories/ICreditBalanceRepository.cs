using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public interface ICreditBalanceRepository
    {
        IQueryable<CreditBalance> CreditBalance { get; }

        void Save(CreditBalance credit);

        CreditBalance Delete(int id);
    }
}
