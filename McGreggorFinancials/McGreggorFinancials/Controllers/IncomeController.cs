using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.ViewModels.Incomes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace McGreggorFinancials.Controllers
{
    public class IncomeController : Controller
    {
        private IIncomeEntryRespository _repo;
        private IIncomeCategoryRepository _catRepo;
        private IAccountTypeRepository _accountTypeRepo;
        private IAccountRepository _accountRepo;

        public IncomeController(IIncomeEntryRespository repo, IIncomeCategoryRepository catRepo, IAccountTypeRepository accountTypeRepo,
            IAccountRepository accountRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
            _accountTypeRepo = accountTypeRepo;
            _accountRepo = accountRepo;
        }

        public ViewResult Create()
        {
            ViewBag.FormTitle = "Create Income";

            return View("Edit", new IncomeFormViewModel
            {
                Income = new IncomeEntry(),
                IncomeCategories = new SelectList(_catRepo.Categories.ToList(), "ID", "Name")
            });
        }

        public ViewResult Edit(int incomeId)
        {
            ViewBag.FormTitle = "Edit Income";

            return View(new IncomeFormViewModel
            {
                Income = _repo.IncomeEntries.FirstOrDefault(i => i.ID == incomeId),
                IncomeCategories = new SelectList(_catRepo.Categories.ToList(), "ID", "Name")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IncomeFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account s = _accountRepo.Accounts.Where(x => x.TypeID == _accountTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
                if (model.Income.ID == 0)
                {
                    s.Amount = Math.Round(s.Amount + model.Income.Amount, 2);
                }
                else
                {
                    IncomeEntry i = _repo.IncomeEntries.Where(x => x.ID == model.Income.ID).FirstOrDefault();
                    s.Amount = Math.Round(s.Amount - i.Amount + model.Income.Amount, 2);
                }

                _repo.Save(model.Income);
                _accountRepo.Save(s);
                TempData["message"] = $"{model.Income.Description} has been saved";
                return RedirectToAction("MonthlyReport");
            }
            else
            {
                if (model.Income.ID == 0)
                {
                    ViewBag.FormTitle = "Create Income";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Income";
                }

                model.IncomeCategories = new SelectList(_catRepo.Categories.ToList(), "ID", "Name");
                return View(model);
            }
        }

        public ViewResult MonthlyReport(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<IncomeEntry> incomes = _repo.IncomeEntries.Where(i => i.Date.Month == date.Value.Month && i.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<IncomeCategory> cats = _catRepo.Categories.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(incomes.Where(i => i.CategoryID == cat.ID).Select(i => i.Amount).Sum())
                });
            }

            int daysInMonth = DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            for (int j = 1; j <= daysInMonth; j++)
            {
                sum += incomes.Where(i => i.Date.Day == j).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = j.ToString(),
                    YData = Convert.ToString(sum)
                });
            }

            return View(new IncomeListViewModel
            {
                Incomes = incomes,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum())
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

            List<IncomeEntry> incomes = _repo.IncomeEntries.Where(i => i.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<IncomeCategory> cats = _catRepo.Categories.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(incomes.Where(i => i.CategoryID == cat.ID).Select(i => i.Amount).Sum())
                });
            }

            int month = 1;
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            while (month <= 12)
            {
                sum += incomes.Where(i => i.Date.Month == month).Select(i => i.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = DateTimeFormatInfo.CurrentInfo.GetMonthName(month).Substring(0, 3),
                    YData = Convert.ToString(sum)
                });

                month++;
            }

            return View(new IncomeListViewModel
            {
                Incomes = incomes,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum())
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

            List<IncomeEntry> incomes = _repo.IncomeEntries.Where(i => i.Date.Year <= date.Value.Year && i.Date.Year >= date.Value.Year - 5).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<IncomeCategory> cats = _catRepo.Categories.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(incomes.Where(i => i.CategoryID == cat.ID).Select(i => i.Amount).Sum())
                });
            }

            int startYear = date.Value.Year - 5;
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            while (startYear <= date.Value.Year)
            {
                sum += incomes.Where(i => i.Date.Year == startYear).Select(i => i.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = startYear.ToString(),
                    YData = Convert.ToString(sum)
                });

                startYear++;
            }

            return View(new IncomeListViewModel
            {
                Incomes = incomes,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum())
            });
        }

        public ViewResult AllIncome()
        {
            List<IncomeEntry> incomes = _repo.IncomeEntries.ToList();

            return View(new IncomeListViewModel
            {
                Incomes = incomes,
            });
        }

        [HttpPost]
        public IActionResult Delete(int incomeId)
        {
            Account s = _accountRepo.Accounts.Where(x => x.TypeID == _accountTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
            IncomeEntry deletedIncome = _repo.Delete(incomeId);
            s.Amount = Math.Round(s.Amount - deletedIncome.Amount, 2);
            _accountRepo.Save(s);
            if (deletedIncome != null)
            {
                TempData["message"] = $"{deletedIncome.Description} was deleted";
            }

            return RedirectToAction("MonthlyReport");
        }
    }
}