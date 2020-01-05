using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Stocks.Repository
{
    public interface IStockRepository
    {
        IQueryable<Stock> Stocks { get; }

        void Save(Stock stock);
    }
}
