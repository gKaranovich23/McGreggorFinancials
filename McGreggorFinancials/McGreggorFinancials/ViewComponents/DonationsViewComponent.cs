using McGreggorFinancials.Models.Donations;
using McGreggorFinancials.Models.Donations.Repository;
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
        private ITargetAmountRepository _goalRepo;
        private readonly ITargetTypeRepository _goalTypeRepo;

        public DonationsViewComponent(IDonationRepository repo, ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo)
        {
            _repo = repo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Donation> donations = _repo.Donations.Where(e => e.Date.Month == date.Month && e.Date.Year == date.Year).ToList();

            return View(new DonationsVsGoalViewModel
            {
                DonationsTotal = Convert.ToDecimal(donations.Select(e => e.Amount).Sum()),
                DonationsGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Charity")).FirstOrDefault()
            });
        }
    }
}
