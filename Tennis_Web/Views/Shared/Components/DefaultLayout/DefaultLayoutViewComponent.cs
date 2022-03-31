using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Views.Shared.Components.DefaultLayout
{
    public class DefaultLayoutViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public DefaultLayoutViewComponent(TennisContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.HasCurrent = _context.DS_Giais.Any(m => m.Giai_Moi);
            return View();
        }
    }
}
