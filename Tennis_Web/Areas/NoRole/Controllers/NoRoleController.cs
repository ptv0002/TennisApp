using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.IO;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class NoRoleController : Controller
    {
        private readonly TennisContext _context;
        public NoRoleController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            FileInfo file = new("D:/Giai_Dau.xlsx");
            return View();
        }
    }
}
