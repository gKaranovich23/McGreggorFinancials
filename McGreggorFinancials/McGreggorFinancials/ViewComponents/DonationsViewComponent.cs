using McGreggorFinancials.Models.Donations;
using McGreggorFinancials.Models.Donations.Repository;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.Models.Targets;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class DonationsViewComponent : ViewComponent
    {
        private IDonationRepository _repo;
        private IIncomeEntryRespository _incomeRepo;
        private ITargetAmountRepository _goalRepo;
        private readonly ITargetTypeRepository _goalTypeRepo;

        public DonationsViewComponent(IDonationRepository repo, ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo,
            IIncomeEntryRespository incomeRepo)
        {
            _repo = repo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
            _incomeRepo = incomeRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Donation> donations = _repo.Donations.Where(e => e.Date.Month == date.Month && e.Date.Year == date.Year).ToList();

            List<IncomeEntry> incomes = _incomeRepo.IncomeEntries.Where(i => i.Date.Month == date.Month && i.Date.Year == date.Year).ToList();

            TargetAmount donationsGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Charity")).FirstOrDefault();
            decimal targetDonationsAmount = Convert.ToDecimal(incomes.Select(i => i.Amount).Sum()) * donationsGoal.Percentage / 100;

            return View(new DonationsVsGoalViewModel
            {
                DonationsTotal = Math.Round(Convert.ToDecimal(donations.Select(e => e.Amount).Sum()), 2),
                DonationsGoal = targetDonationsAmount
            });
        }
    }
}
