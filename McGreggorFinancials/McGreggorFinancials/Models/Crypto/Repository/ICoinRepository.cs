using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Crypto.Repository
{
    public interface ICoinRepository
    {
        IQueryable<Coin> Coins { get; }

        void Save(Coin coin);

        Coin Delete(int id);
    }
}
