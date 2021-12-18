using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Views.Shared.Components.Parameter
{
    public class ParameterViewComponent : ViewComponent 
    {
        public ParameterViewComponent() { }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
