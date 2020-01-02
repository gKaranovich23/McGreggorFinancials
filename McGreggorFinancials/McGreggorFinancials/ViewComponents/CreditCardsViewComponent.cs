using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class CreditCardsViewComponent : ViewComponent
    {
        private IPaymentMethodRepository _payRepo;
        private ICreditBalanceRepository _creditRepo;

        public CreditCardsViewComponent(IPaymentMethodRepository payRepo, ICreditBalanceRepository creditRepo)
        {
            _payRepo = payRepo;
            _creditRepo = creditRepo;
        }

        public IViewComponentResult Invoke()
        {
            List<PaymentMethod> methods = _payRepo.PaymentMethods.Where(x => x.IsCredit).ToList();

            return View(methods);
        }
    }
}
