using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Income;
using McGreggorFinancials.Models.Income.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace McGreggorFinancials.Controllers
{
    public class AdminController : Controller
    {
        private IIncomeCategoryRepository _incomeCatRepo;

        public AdminController(IIncomeCategoryRepository incomeCatRepo)
        {
            _incomeCatRepo = incomeCatRepo;
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
    }
}