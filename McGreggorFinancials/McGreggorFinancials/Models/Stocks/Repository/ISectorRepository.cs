using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks.Repository
{
    public interface ISectorRepository
    {
        IQueryable<Sector> Sectors { get; }

        void Save(Sector sector);
    }
}
