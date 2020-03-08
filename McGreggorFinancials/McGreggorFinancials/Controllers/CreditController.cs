using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using McGreggorFinancials.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace McGreggorFinancials.Controllers
{
    public class CreditController : Controller
    {
        private IAccountTypeRepository _saveTypeRepo;
        private IAccountRepository _saveRepo;
        private IPaymentMethodRepository _payRepo;
        private ICreditBalanceRepository _creditRepo;

        public CreditController(IAccountTypeRepository saveTypeRepo, IAccountRepository saveRepo, IPaymentMethodRepository payRepo,
            ICreditBalanceRepository creditRepo)
        {
            _saveTypeRepo = saveTypeRepo;
            _saveRepo = saveRepo;
            _payRepo = payRepo;
            _creditRepo = creditRepo;
        }

        public ViewResult CreditCards()
        {
            List<PaymentMethod> methods = _payRepo.PaymentMethods.Where(x => x.IsCredit).ToList();

            return View(methods);
        }

        public ViewResult PayOffCredit(int id)
        {
            ViewBag.FormTitle = "Pay Off Credit";

            PaymentMethod p = _payRepo.PaymentMethods.Where(x => x.ID == id).FirstOrDefault();

            return View(new PayOffCreditViewModel
            {
                PaymentMethod = p,
                AmountToPay = 0.00
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PayOffCredit(PayOffCreditViewModel model)
        {
            PaymentMethod p = _payRepo.PaymentMethods.Where(x => x.ID == model.PaymentMethod.ID).First();
            Account s = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                CreditBalance c = p.CreditBalance;
                c.Amount -= model.AmountToPay;
                s.Amount -= model.AmountToPay;
                _creditRepo.Save(c);
                _saveRepo.Save(s);
                TempData["message"] = $"{p.Method} has had ${model.AmountToPay} paid off";
                return RedirectToAction("CreditCards");
            }
            else
            {
                ViewBag.FormTitle = "Pay Off Credit";

                model.PaymentMethod = p;
                return View(model);
            }
        }
    }
}