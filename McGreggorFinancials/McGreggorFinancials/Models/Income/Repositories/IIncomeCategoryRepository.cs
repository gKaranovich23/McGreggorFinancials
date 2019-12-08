using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Income.Repositories
{
    public interface IIncomeCategoryRepository
    {
        IQueryable<IncomeCategory> Categories { get; }

        void Save(IncomeCategory cat);
    }
}
