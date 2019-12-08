using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Targets.Repositories
{
    public interface ITargetAmountRepository
    {
        IQueryable<TargetAmount> TargetAmounts { get; }

        void Save(TargetAmount target);

        TargetAmount Delete(int id);
    }
}
