using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks.Repository
{
    public interface IShareRepository
    {
        IQueryable<Share> Shares { get; }

        void Save(Share share);

        Share Delete(int shareId);
    }
}
