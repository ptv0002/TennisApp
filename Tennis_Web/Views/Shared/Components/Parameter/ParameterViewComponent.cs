using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Parameter
{
    public class ParameterViewComponent : ViewComponent 
    {
        public ParameterViewComponent() { }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel model)
        {
            if (model.IsCurrent == true)
            {
                ViewBag.test = "Current";
            }
            else ViewBag.test = "Previous";
            return View();
        }
    }
}
