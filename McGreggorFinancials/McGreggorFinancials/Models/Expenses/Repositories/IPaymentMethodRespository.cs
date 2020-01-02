using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Expenses.Repositories
{
    public interface IPaymentMethodRepository
    {
        IQueryable<PaymentMethod> PaymentMethods { get; }

        void Save(PaymentMethod method);

        PaymentMethod Delete(int id);
    }
}
