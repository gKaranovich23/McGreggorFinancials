using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Donations.Repository
{
    public class DonationRepository : IDonationRepository
    {
        private ApplicationDbContext context;

        public DonationRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Donation> Donations => context.Donations.Include(e => e.Charity)
            .Include(x => x.PaymentMethod);

        public int Save(Donation e)
        {
            if (e.ID == 0)
            {
                context.Donations.Add(e);
            }
            else
            {
                Donation dbEntry = context.Donations.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Description = e.Description;
                    dbEntry.Amount = e.Amount;
                    dbEntry.Date = e.Date;
                    dbEntry.CharityID = e.CharityID;
                }
            }
            context.SaveChanges();

            return e.ID;
        }

        public Donation Delete(int donationId)
        {
            Donation dbEntry = context.Donations.FirstOrDefault(e => e.ID == donationId);

            if (dbEntry != null)
            {
                context.Donations.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
