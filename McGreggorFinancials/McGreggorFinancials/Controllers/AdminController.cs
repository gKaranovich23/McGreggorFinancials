using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.Models.Targets;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace McGreggorFinancials.Controllers
{
    public class AdminController : Controller
    {
        private IIncomeCategoryRepository _incomeCatRepo;
        private IExpenseCategoryRepository _expenseCatRepo;
        private ITargetAmountRepository _targetAmountRepo;
        private ITargetTypeRepository _targetTypeRepo;
        private IAccountRepository _accountRepo;
        private IAccountTypeRepository _accountTypeRepo;
        private IPaymentMethodRepository _payRepo;
        private ICreditBalanceRepository _creditRepo;

        public AdminController(IIncomeCategoryRepository incomeCatRepo, IExpenseCategoryRepository expenseCatRepo,
            ITargetAmountRepository targetAmountRepo, ITargetTypeRepository targetTypeRepo, IAccountRepository accountRepo,
            IAccountTypeRepository accountTypeRepo, IPaymentMethodRepository payRepo, ICreditBalanceRepository creditRepo)
        {
            _incomeCatRepo = incomeCatRepo;
            _expenseCatRepo = expenseCatRepo;
            _targetAmountRepo = targetAmountRepo;
            _targetTypeRepo = targetTypeRepo;
            _accountRepo = accountRepo;
            _accountTypeRepo = accountTypeRepo;
            _payRepo = payRepo;
            _creditRepo = creditRepo;
        }

        public ViewResult CreateIncomeCategory()
        {
            ViewBag.FormTitle = "Create Income Category";

            return View("EditIncomeCategory", new IncomeCategory());
        }

        public ViewResult EditIncomeCategory(int id)
        {
            ViewBag.FormTitle = "Edit Income Type";

            IncomeCategory cat = _incomeCatRepo.Categories.Where(x => x.ID == id).FirstOrDefault();

            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditIncomeCategory(IncomeCategory model)
        {
            if (ModelState.IsValid)
            {
                _incomeCatRepo.Save(model);
                TempData["message"] = $"{model.Name} has been saved";
                return RedirectToAction("IncomeCategories");
            }
            else
            {
                if (model.ID == 0)
                {
                    ViewBag.FormTitle = "Create Income Category";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Income Category";
                }

                return View(model);
            }
        }

        public ViewResult IncomeCategories()
        {
            List<IncomeCategory> cats = _incomeCatRepo.Categories.ToList();

            return View(cats);
        }

        public ViewResult CreateExpenseCategory()
        {
            ViewBag.FormTitle = "Create Expense Category";

            return View("EditExpenseCategory", new ExpenseCategory());
        }

        public ViewResult EditExpenseCategory(int id)
        {
            ViewBag.FormTitle = "Edit Expense Category";

            ExpenseCategory cat = _expenseCatRepo.ExpenseCategories.Where(x => x.ID == id).FirstOrDefault();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditExpenseCategory(ExpenseCategory model)
        {
            if (ModelState.IsValid)
            {
                _expenseCatRepo.Save(model);
                TempData["message"] = $"{model.Name} has been saved";
                return RedirectToAction("ExpenseCategories");
            }
            else
            {
                if (model.ID == 0)
                {
                    ViewBag.FormTitle = "Create Expense Category";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Expense Category";
                }

                return View(model);
            }
        }

        public ViewResult ExpenseCategories()
        {
            List<ExpenseCategory> cats = _expenseCatRepo.ExpenseCategories.ToList();

            return View(cats);
        }

        public ViewResult CreateTarget()
        {
            ViewBag.FormTitle = "Create Target";

            return View("EditTarget", new TargetFormViewModel
            {
                Target = new TargetAmount(),
                TargetTypes = new SelectList(_targetTypeRepo.TargetTypes.ToList(), "ID", "Name")
            });
        }

        public ViewResult EditTarget(int id)
        {
            ViewBag.FormTitle = "Edit Target";

            TargetAmount g = _targetAmountRepo.TargetAmounts.Where(x => x.ID == id).FirstOrDefault();

            return View(new TargetFormViewModel
            {
                Target = g,
                TargetTypes = new SelectList(_targetTypeRepo.TargetTypes.ToList(), "ID", "Name")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTarget(TargetFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                _targetAmountRepo.Save(model.Target);
                TempData["message"] = $"Target #{model.Target.ID} has been saved";
                return RedirectToAction("Targets");
            }
            else
            {
                if (model.Target.ID == 0)
                {
                    ViewBag.FormTitle = "Create Target";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Target";
                }

                model.TargetTypes = new SelectList(_targetTypeRepo.TargetTypes.ToList(), "ID", "Name");
                return View(model);
            }
        }

        public ViewResult Targets()
        {
            List<TargetAmount> goals = _targetAmountRepo.TargetAmounts.ToList();

            return View(goals);
        }

        public ViewResult CreateTargetType()
        {
            ViewBag.FormTitle = "Create Target Type";

            return View("EditTargetType", new TargetType());
        }

        public ViewResult EditTargetType(int id)
        {
            ViewBag.FormTitle = "Edit Target Type";

            TargetType type = _targetTypeRepo.TargetTypes.Where(x => x.ID == id).FirstOrDefault();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTargetType(TargetType model)
        {
            if (ModelState.IsValid)
            {
                _targetTypeRepo.Save(model);
                TempData["message"] = $"{model.Name} has been saved";
                return RedirectToAction("TargetTypes");
            }
            else
            {
                if (model.ID == 0)
                {
                    ViewBag.FormTitle = "Create Target Type";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Target Type";
                }

                return View(model);
            }
        }

        public ViewResult TargetTypes()
        {
            List<TargetType> types = _targetTypeRepo.TargetTypes.ToList();

            return View(types);
        }

        public ViewResult CreateAccount()
        {
            ViewBag.FormTitle = "Create Account";

            return View("EditSavings", new AccountFormViewModel
            {
                Account = new Account(),
                AccountTypes = new SelectList(_accountTypeRepo.AccountTypes.ToList(), "ID", "Name"),
                TargetTypes = new SelectList(_targetTypeRepo.TargetTypes.ToList(), "ID", "Name")
            });
        }

        public ViewResult EditAccount(int id)
        {
            ViewBag.FormTitle = "Edit Account";

            return View(new AccountFormViewModel
            {
                Account = _accountRepo.Accounts.FirstOrDefault(e => e.ID == id),
                AccountTypes = new SelectList(_accountTypeRepo.AccountTypes.ToList(), "ID", "Name"),
                TargetTypes = new SelectList(_targetTypeRepo.TargetTypes.ToList(), "ID", "Name")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(AccountFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Account.TargetID = _accountRepo.Accounts.Where(x => x.TargetID == model.SelectedTarget).First().ID;
                _accountRepo.Save(model.Account);
                TempData["message"] = $"Account #{model.Account.ID} has been saved";
                return RedirectToAction("MonthlyReport");
            }
            else
            {
                if (model.Account.ID == 0)
                {
                    ViewBag.FormTitle = "Create Account";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Account";
                }

                model.AccountTypes = new SelectList(_accountTypeRepo.AccountTypes.ToList(), "ID", "Name");
                model.TargetTypes = new SelectList(_targetTypeRepo.TargetTypes.ToList(), "ID", "Name");
                return View(model);
            }
        }

        public ViewResult Accounts()
        {
            List<Account> saves = _accountRepo.Accounts.ToList();

            return View(saves);
        }

        public ViewResult CreateAccountType()
        {
            ViewBag.FormTitle = "Create Account Type";

            return View("EditAccountType", new AccountType());
        }

        public ViewResult EditAccountType(int id)
        {
            ViewBag.FormTitle = "Edit Account Type";

            AccountType type = _accountTypeRepo.AccountTypes.Where(x => x.ID == id).FirstOrDefault();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccountType(AccountType model)
        {
            if (ModelState.IsValid)
            {
                _accountTypeRepo.Save(model);
                TempData["message"] = $"{model.Name} has been saved";
                return RedirectToAction("AccountTypes");
            }
            else
            {
                if (model.ID == 0)
                {
                    ViewBag.FormTitle = "Create Account Type";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Account Type";
                }

                return View(model);
            }
        }

        public ViewResult AccountTypes()
        {
            List<AccountType> types = _accountTypeRepo.AccountTypes.ToList();

            return View(types);
        }

        public ViewResult CreatePaymentMethod()
        {
            ViewBag.FormTitle = "Create Payment Method";

            return View("EditPaymentMethod", new PaymentMethod());
        }

        public ViewResult EditPaymentMethod(int id)
        {
            ViewBag.FormTitle = "Edit Payment Method";

            PaymentMethod p = _payRepo.PaymentMethods.Where(x => x.ID == id).FirstOrDefault();

            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPaymentMethod(PaymentMethod model)
        {
            if (ModelState.IsValid)
            {
                if (model.ID > 0)
                {
                    PaymentMethod old = _payRepo.PaymentMethods.Where(x => x.ID == model.ID).First();
                    if (old.IsCredit && !model.IsCredit)
                    {
                        _creditRepo.Delete(old.CreditBalance.ID);
                    }
                }

                _payRepo.Save(model);

                if (model.IsCredit)
                {
                    CreditBalance c = new CreditBalance
                    {
                        Amount = 0.00,
                        PaymentMethodID = _payRepo.PaymentMethods.Last().ID
                    };

                    _creditRepo.Save(c);
                }

                TempData["message"] = $"{model.Method} has been saved";
                return RedirectToAction("PaymentMethods");
            }
            else
            {
                if (model.ID == 0)
                {
                    ViewBag.FormTitle = "Create Payment Method";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Payment Method";
                }

                return View(model);
            }
        }

        public ViewResult PaymentMethods()
        {
            List<PaymentMethod> methods = _payRepo.PaymentMethods.ToList();

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
            Account s = _accountRepo.Accounts.Where(x => x.TypeID == _accountTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                CreditBalance c = p.CreditBalance;
                c.Amount -= model.AmountToPay;
                s.Amount -= model.AmountToPay;
                _creditRepo.Save(c);
                _accountRepo.Save(s);
                TempData["message"] = $"{p.Method} has had ${model.AmountToPay} paid off";
                return RedirectToAction("PaymentMethods");
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