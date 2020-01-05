using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Donations.Repository
{
    public interface IDonationRepository
    {
        IQueryable<Donation> Donations { get; }

        int Save(Donation donation);

        Donation Delete(int donationId);
    }
}
