using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Donations.Repository
{
    public interface ICharityRepository
    {
        IQueryable<Charity> Charities { get; }

        void Save(Charity charity);
    }
}
