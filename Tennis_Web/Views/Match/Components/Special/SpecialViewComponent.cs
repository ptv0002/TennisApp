using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Match.Components.Special
{
    public class SpecialViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        public SpecialViewComponent(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            return View();
        }
    }
}
