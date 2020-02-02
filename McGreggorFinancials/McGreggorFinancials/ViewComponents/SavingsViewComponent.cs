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

        public SavingsViewComponent(IAccountRepository repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Account> savings = _repo.Accounts.ToList();

            return View(savings.Select(x => x.Amount).Sum());
        }
    }
}
