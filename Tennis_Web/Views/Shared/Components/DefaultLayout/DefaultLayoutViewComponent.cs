﻿using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ViewBag.HasLevel = _context.DS_Trinhs.Include(m => m.DS_Giai).Any(m => m.DS_Giai.Giai_Moi);
            return View();
        }
    }
}