using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Info
{
    public class InfoViewComponent : ViewComponent
    {
        public InfoViewComponent() { }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel model)
        {

            return View();
        }
    }
}
