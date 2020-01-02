using McGreggorFinancials.Models.Charting;
using McGreggorFinancials.Models.Expenses;
using McGreggorFinancials.Models.Expenses.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class ExpenseCategoriesViewComponent : ViewComponent
    {
        private IExpenseRepository _repo;
        private IExpenseCategoryRepository _catRepo;

        public ExpenseCategoriesViewComponent(IExpenseRepository repo, IExpenseCategoryRepository catRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
        }

        public IViewComponentResult Invoke()
        {
            DateTime date = DateTime.Now;

            List<Expense> expenses = _repo.Expenses.Where(e => e.Date.Month == date.Month && e.Date.Year == date.Year).ToList();
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

            return View(data);
        }
    }
}
