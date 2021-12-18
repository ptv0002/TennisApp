using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Views.Shared.Components.Division
{
    public class DivisionViewComponent : ViewComponent
    {
        public DivisionViewComponent() { }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
