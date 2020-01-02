using McGreggorFinancials.Models.Accounts;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Targets.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class SavingsViewComponent : ViewComponent
    {
        private IAccountRepository _repo;
        private IAccountTypeRepository _typeRepo;
        private ITargetAmountRepository _goalRepo;
        private readonly ITargetTypeRepository _goalTypeRepo;

        public SavingsViewComponent(IAccountRepository repo, IAccountTypeRepository typeRepo,
            ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo)
        {
            _repo = repo;
            _typeRepo = typeRepo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Account> savings = _repo.Accounts.ToList();

            return View(savings.Select(x => x.Amount).Sum());
        }
    }
}
