using McGreggorFinancials.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private ApplicationDbContext context;

        public PaymentMethodRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<PaymentMethod> PaymentMethods => context.PaymentMethods.Include(x => x.CreditBalance);

        public void Save(PaymentMethod e)
        {
            if (e.ID == 0)
            {
                context.PaymentMethods.Add(e);
            }
            else
            {
                PaymentMethod dbEntry = context.PaymentMethods.FirstOrDefault(x => x.ID == e.ID);
                if (dbEntry != null)
                {
                    dbEntry.Method = e.Method;
                    dbEntry.IsCredit = e.IsCredit;
                }
            }
            context.SaveChanges();
        }

        public PaymentMethod Delete(int id)
        {
            PaymentMethod dbEntry = context.PaymentMethods.FirstOrDefault(e => e.ID == id);

            if (dbEntry != null)
            {
                context.PaymentMethods.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
