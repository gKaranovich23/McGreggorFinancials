using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.Expenses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace McGreggorFinancials.Controllers
{
    public class ExpenseController : Controller
    {
        private IExpenseRepository _repo;
        private IExpenseCategoryRepository _catRepo;
        private ITargetAmountRepository _goalRepo;
        private ITargetTypeRepository _goalTypeRepo;
        private IAccountTypeRepository _saveTypeRepo;
        private IAccountRepository _saveRepo;
        private IPaymentMethodRepository _payRepo;
        private ICreditBalanceRepository _creditRepo;

        public ExpenseController(IExpenseRepository repo, IExpenseCategoryRepository catRepo,
            ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo, IAccountTypeRepository saveTypeRepo,
            IAccountRepository saveRepo, IPaymentMethodRepository payRepo, ICreditBalanceRepository creditRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
            _saveTypeRepo = saveTypeRepo;
            _saveRepo = saveRepo;
            _payRepo = payRepo;
            _creditRepo = creditRepo;
        }

        public ViewResult Create()
        {
            ViewBag.FormTitle = "Create Expense";

            return View("Edit", new ExpenseFormViewModel
            {
                Expense = new Expense(),
                ExpenseCategories = new SelectList(_catRepo.ExpenseCategories.ToList(), "ID", "Name"),
                PaymentMethods = new SelectList(_payRepo.PaymentMethods.ToList(), "ID", "Method")
            });
        }

        public ViewResult Edit(int expenseId)
        {
            ViewBag.FormTitle = "Edit Expense";

            return View(new ExpenseFormViewModel
            {
                Expense = _repo.Expenses.FirstOrDefault(e => e.ID == expenseId),
                ExpenseCategories = new SelectList(_catRepo.ExpenseCategories.ToList(), "ID", "Name"),
                PaymentMethods = new SelectList(_payRepo.PaymentMethods.ToList(), "ID", "Method")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ExpenseFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account s = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
                CreditBalance c = _payRepo.PaymentMethods.Where(x => x.ID == model.Expense.PaymentMethodID).First().CreditBalance;
                if (model.Expense.ID == 0)
                {
                    if (_payRepo.PaymentMethods.Where(x => x.ID == model.Expense.PaymentMethodID).FirstOrDefault().IsCredit)
                    {
                        c.Amount = Math.Round(c.Amount + model.Expense.Amount, 2);
                    }
                    else
                    {
                        s.Amount = Math.Round(s.Amount - model.Expense.Amount, 2);
                    }
                }
                else
                {
                    Expense i = _repo.Expenses.Where(x => x.ID == model.Expense.ID).FirstOrDefault();

                    if (_payRepo.PaymentMethods.Where(x => x.ID == model.Expense.PaymentMethodID).FirstOrDefault().IsCredit)
                    {
                        c.Amount = Math.Round(c.Amount + model.Expense.Amount - i.Amount, 2);
                    }
                    else
                    {
                        s.Amount = Math.Round(s.Amount + i.Amount - model.Expense.Amount, 2);
                    }
                }

                int id = _repo.Save(model.Expense);

                if (s != null)
                    _saveRepo.Save(s);

                if (c != null)
                    _creditRepo.Save(c);

                TempData["message"] = $"{model.Expense.Description} has been saved";
                return RedirectToAction("MonthlyReport");
            }
            else
            {
                if (model.Expense.ID == 0)
                {
                    ViewBag.FormTitle = "Create Expense";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Expense";
                }

                model.ExpenseCategories = new SelectList(_catRepo.ExpenseCategories.ToList(), "ID", "Name");
                model.PaymentMethods = new SelectList(_payRepo.PaymentMethods.ToList(), "ID", "Method");
                return View(model);
            }
        }

        public ViewResult MonthlyReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<Expense> expenses = _repo.Expenses.Where(e => e.Date.Month == date.Value.Month && e.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<ExpenseCategory> cats = _catRepo.ExpenseCategories.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(expenses.Where(e => e.ExpenseCategoryID == cat.ID).Select(e => e.Amount).Sum())
                });
            }

            int daysInMonth = DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            for (int i = 1; i <= daysInMonth; i++)
            {
                sum += expenses.Where(e => e.Date.Day == i).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = i.ToString(),
                    YData = Convert.ToString(sum)
                });
            }

            return View(new ExpenseListViewModel
            {
                Expenses = expenses,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(expenses.Select(e => e.Amount).Sum()),
                Target = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Expenses")).FirstOrDefault()
            });
        }

        [HttpPost]
        public RedirectToActionResult MonthlyReport(string dateStr)
        {
            DateTime date = Convert.ToDateTime(dateStr);

            return RedirectToAction("MonthlyReport", new { date });
        }

        public ViewResult YearlyReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<Expense> expenses = _repo.Expenses.Where(e => e.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<ExpenseCategory> cats = _catRepo.ExpenseCategories.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(expenses.Where(e => e.ExpenseCategoryID == cat.ID).Select(e => e.Amount).Sum())
                });
            }

            int month = 1;
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            while (month <= 12)
            {
                sum += expenses.Where(e => e.Date.Month == month).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = DateTimeFormatInfo.CurrentInfo.GetMonthName(month).Substring(0, 3),
                    YData = Convert.ToString(sum)
                });

                month++;
            }

            return View(new ExpenseListViewModel
            {
                Expenses = expenses,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(expenses.Select(e => e.Amount).Sum()),
                Target = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Expenses")).FirstOrDefault()
            });
        }

        [HttpPost]
        public RedirectToActionResult YearlyReport(string dateStr)
        {
            DateTime date = new DateTime(int.Parse(dateStr), 1, 1);

            return RedirectToAction("YearlyReport", new { date });
        }

        public ViewResult FiveYRReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<Expense> expenses = _repo.Expenses.Where(e => e.Date.Year <= date.Value.Year && e.Date.Year >= date.Value.Year - 5).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<ExpenseCategory> cats = _catRepo.ExpenseCategories.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(expenses.Where(e => e.ExpenseCategoryID == cat.ID).Select(e => e.Amount).Sum())
                });
            }

            int startYear = date.Value.Year - 5;
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            while (startYear <= date.Value.Year)
            {
                sum += expenses.Where(e => e.Date.Year == startYear).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = startYear.ToString(),
                    YData = Convert.ToString(sum)
                });

                startYear++;
            }

            return View(new ExpenseListViewModel
            {
                Expenses = expenses,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(expenses.Select(e => e.Amount).Sum()),
                Target = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Expenses")).FirstOrDefault()
            });
        }

        public ViewResult AllExpenses()
        {
            List<Expense> expenses = _repo.Expenses.ToList();

            return View(new ExpenseListViewModel
            {
                Expenses = expenses,
            });
        }

        [HttpPost]
        public IActionResult Delete(int expenseId)
        {
            Account s = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
            Expense deletedExpense = _repo.Delete(expenseId);
            s.Amount = Math.Round(s.Amount + deletedExpense.Amount, 2);
            _saveRepo.Save(s);
            if (deletedExpense != null)
            {
                TempData["message"] = $"{deletedExpense.Description} was deleted";
            }

            return RedirectToAction("MonthlyReport");
        }
    }
}