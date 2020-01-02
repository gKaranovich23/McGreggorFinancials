using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using McGreggorFinancials.Models.Targets.Repositories;
using McGreggorFinancials.ViewModels.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class ExpenseVsBudgetViewComponent : ViewComponent
    {
        private IExpenseRepository _repo;
        private ITargetAmountRepository _goalRepo;
        private readonly ITargetTypeRepository _goalTypeRepo;

        public ExpenseVsBudgetViewComponent(IExpenseRepository repo, ITargetAmountRepository goalRepo, ITargetTypeRepository goalTypeRepo)
        {
            _repo = repo;
            _goalRepo = goalRepo;
            _goalTypeRepo = goalTypeRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Expense> expenses = _repo.Expenses.Where(e => e.Date.Month == date.Month && e.Date.Year == date.Year).ToList();

            return View(new ExpenseVsBudgetViewModel
            {
                ExpenseTotal = Convert.ToDecimal(expenses.Select(e => e.Amount).Sum()),
                BudgetGoal = _goalRepo.TargetAmounts.Where(g => g.TargetType.Name.Equals("Expenses")).FirstOrDefault()
            });
        }
    }
}
