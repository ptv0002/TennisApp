using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Controllers
{
    public class MatchController : Controller
    {
        private readonly TennisContext _context;
        public MatchController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenBy(m => m.Trinh).ToList();
            return View(model);
        }
    }
}
