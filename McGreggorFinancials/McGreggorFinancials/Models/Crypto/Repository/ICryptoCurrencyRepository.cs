using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Crypto.Repository
{
    public interface ICryptoCurrencyRepository
    {
        IQueryable<CryptoCurrency> CryptoCurrencies { get; }

        void Save(CryptoCurrency cryptoCurrency);
    }
}
