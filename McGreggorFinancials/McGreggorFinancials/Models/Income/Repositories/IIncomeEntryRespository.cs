using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Income.Repositories
{
    public interface IIncomeEntryRespository
    {
        IQueryable<IncomeEntry> IncomeEntries { get; }

        void Save(IncomeEntry income);

        IncomeEntry Delete(int incomeId);  
    }
}
