using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Donations;
using McGreggorFinancials.Models.Donations.Repository;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.Models.Targets;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.Donations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace McGreggorFinancials.Controllers
{
    public class DonationController : Controller
    {
        private IDonationRepository _repo;
        private ICharityRepository _catRepo;
        private ITargetAmountRepository _goalRepo;
        private ITargetTypeRepository _goalTypeRepo;
        private IAccountTypeRepository _saveTypeRepo;
        private IAccountRepository _saveRepo;
        private IPaymentMethodRepository _payRepo;
        private ICreditBalanceRepository _creditRepo;
        private IIncomeEntryRespository _incomeRepo;

        public DonationController(IDonationRepository repo, ICharityRepository catRepo,
            ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo, IAccountTypeRepository saveTypeRepo,
            IAccountRepository saveRepo, IPaymentMethodRepository payRepo, ICreditBalanceRepository creditRepo,
            IIncomeEntryRespository incomeRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
            _saveTypeRepo = saveTypeRepo;
            _saveRepo = saveRepo;
            _payRepo = payRepo;
            _creditRepo = creditRepo;
            _incomeRepo = incomeRepo;
        }

        public ViewResult Create()
        {
            ViewBag.FormTitle = "Create Donation";

            return View("Edit", new DonationFormViewModel
            {
                Donation = new Donation(),
                Charities = new SelectList(_catRepo.Charities.ToList(), "ID", "Name"),
                PaymentMethods = new SelectList(_payRepo.PaymentMethods.ToList(), "ID", "Method")
            });
        }

        public ViewResult Edit(int expenseId)
        {
            ViewBag.FormTitle = "Edit Donation";

            return View(new DonationFormViewModel
            {
                Donation = _repo.Donations.FirstOrDefault(e => e.ID == expenseId),
                Charities = new SelectList(_catRepo.Charities.ToList(), "ID", "Name"),
                PaymentMethods = new SelectList(_payRepo.PaymentMethods.ToList(), "ID", "Method")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DonationFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account s = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
                CreditBalance c = _payRepo.PaymentMethods.Where(x => x.ID == model.Donation.PaymentMethodID).First().CreditBalance;
                if (model.Donation.ID == 0)
                {
                    if (_payRepo.PaymentMethods.Where(x => x.ID == model.Donation.PaymentMethodID).FirstOrDefault().IsCredit)
                    {
                        c.Amount += model.Donation.Amount;
                    }
                    else
                    {
                        s.Amount -= model.Donation.Amount;
                    }
                }
                else
                {
                    Donation i = _repo.Donations.Where(x => x.ID == model.Donation.ID).FirstOrDefault();

                    if (_payRepo.PaymentMethods.Where(x => x.ID == model.Donation.PaymentMethodID).FirstOrDefault().IsCredit)
                    {
                        c.Amount += model.Donation.Amount - i.Amount;
                    }
                    else
                    {
                        s.Amount = s.Amount + i.Amount - model.Donation.Amount;
                    }
                }

                int id = _repo.Save(model.Donation);

                if (s != null)
                    _saveRepo.Save(s);

                if (c != null)
                    _creditRepo.Save(c);

                TempData["message"] = $"{model.Donation.Description} has been saved";
                return RedirectToAction("MonthlyReport");
            }
            else
            {
                if (model.Donation.ID == 0)
                {
                    ViewBag.FormTitle = "Create Donation";
                }
                else
                {
                    ViewBag.FormTitle = "Edit Donation";
                }

                model.Charities = new SelectList(_catRepo.Charities.ToList(), "ID", "Name");
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

            List<Donation> donations = _repo.Donations.Where(e => e.Date.Month == date.Value.Month && e.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<Charity> cats = _catRepo.Charities.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(donations.Where(e => e.CharityID == cat.ID).Select(e => e.Amount).Sum())
                });
            }

            int daysInMonth = DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            for (int i = 1; i <= daysInMonth; i++)
            {
                sum += donations.Where(e => e.Date.Day == i).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = i.ToString(),
                    YData = Convert.ToString(sum)
                });
            }

            List<IncomeEntry> incomes = _incomeRepo.IncomeEntries.Where(i => i.Date.Month == date.Value.Month && i.Date.Year == date.Value.Year).ToList();

            TargetAmount donationsGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Charity")).FirstOrDefault();
            decimal targetDonationsAmount = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum()) * donationsGoal.Percentage / 100;

            return View(new DonationsListViewModel
            {
                Donations = donations,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(donations.Select(e => e.Amount).Sum()),
                DonationsGoal = targetDonationsAmount,
                DonationsPercentage = donationsGoal.Percentage
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

            List<Donation> donations = _repo.Donations.Where(e => e.Date.Year == date.Value.Year).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<Charity> cats = _catRepo.Charities.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(donations.Where(e => e.CharityID == cat.ID).Select(e => e.Amount).Sum())
                });
            }

            int month = 1;
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            while (month <= 12)
            {
                sum += donations.Where(e => e.Date.Month == month).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = DateTimeFormatInfo.CurrentInfo.GetMonthName(month).Substring(0, 3),
                    YData = Convert.ToString(sum)
                });

                month++;
            }

            return View(new DonationsListViewModel
            {
                Donations = donations,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(donations.Select(e => e.Amount).Sum()),
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

            List<Donation> donations = _repo.Donations.Where(e => e.Date.Year <= date.Value.Year && e.Date.Year >= date.Value.Year - 5).ToList();
            List<PieChartData> data = new List<PieChartData>();
            List<Charity> cats = _catRepo.Charities.ToList();
            foreach (var cat in cats)
            {
                data.Add(new PieChartData
                {
                    Category = cat.Name,
                    Data = Convert.ToString(donations.Where(e => e.CharityID == cat.ID).Select(e => e.Amount).Sum())
                });
            }

            int startYear = date.Value.Year - 5;
            List<LineChartData> lineData = new List<LineChartData>();
            double sum = 0;

            while (startYear <= date.Value.Year)
            {
                sum += donations.Where(e => e.Date.Year == startYear).Select(e => e.Amount).Sum();
                lineData.Add(new LineChartData
                {
                    XData = startYear.ToString(),
                    YData = Convert.ToString(sum)
                });

                startYear++;
            }

            return View(new DonationsListViewModel
            {
                Donations = donations,
                Date = date.Value,
                PieChartData = data,
                LineChartData = lineData,
                Total = Convert.ToDecimal(donations.Select(e => e.Amount).Sum()),
            });
        }

        public ViewResult AllDonations()
        {
            List<Donation> donations = _repo.Donations.ToList();

            return View(new DonationsListViewModel
            {
                Donations = donations,
            });
        }

        [HttpPost]
        public IActionResult Delete(int donationId)
        {
            Account s = _saveRepo.Accounts.Where(x => x.TypeID == _saveTypeRepo.AccountTypes.Where(y => y.Name.Equals("Personal")).FirstOrDefault().ID).FirstOrDefault();
            Donation deletedDonation = _repo.Delete(donationId);
            s.Amount = s.Amount + deletedDonation.Amount;
            _saveRepo.Save(s);
            if (deletedDonation != null)
            {
                TempData["message"] = $"{deletedDonation.Description} was deleted";
            }

            return RedirectToAction("MonthlyReport");
        }
    }
}