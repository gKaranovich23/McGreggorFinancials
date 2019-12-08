using McGreggorFinancials.ViewModels.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewComponents
{
    public class SideNavViewComponent : ViewComponent
    {
        public SideNavViewComponent() { }

        public IViewComponentResult Invoke()
        {
            string controller = RouteData.Values["controller"].ToString();
            string action = RouteData.Values["action"].ToString();

            SideNavViewModel model = new SideNavViewModel
            {
                CurrentAction = action,
                CurrentController = controller
            };

            return View(model);
        }
    }
}
