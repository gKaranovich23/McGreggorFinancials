using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Targets.Repositories
{
    public interface ITargetTypeRepository
    {
        IQueryable<TargetType> TargetTypes { get; }

        void Save(TargetType type);
    }
}
