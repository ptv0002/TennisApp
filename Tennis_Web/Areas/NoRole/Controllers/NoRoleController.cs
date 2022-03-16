using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.IO;
using Library;
using Models;
using Tennis_Web.Models;
using Tennis_Web.Areas.NoRole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using Library.FileInitializer;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class NoRoleController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public NoRoleController(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
//        public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            //new Initializer(_webHost).RoundGeneratorAsync();
            //new Initializer(_webHost).Special1stRoundGenerator();


            //FileStream outStream = System.IO.File.OpenRead(path);
            //var roundFile = await JsonSerializer.DeserializeAsync<List<Special1stViewModel>>(outStream);
            return View();
        }
        public IActionResult Player(bool isCurrent)
        {
            List<DS_VDV> model = new();
            if (isCurrent)
            {
                model = _context.DS_VDVs.Where(m => m.Tham_Gia).OrderByDescending(m => m.Diem).ToList();
                ViewBag.IsCurrent = true;
                ViewBag.Tournament = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi).Ten;
            }
            else
            {
                model = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList();
                ViewBag.IsCurrent = false;
            }
            return View(model);
        }
        public IActionResult Announcement()
        {
            return View();
        }
        public IActionResult Result(bool isCurrent)
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Giai_Moi == isCurrent).OrderByDescending(m => m.DS_Giai.Ngay).ThenBy(m => m.Trinh).ToList();
            ViewBag.IsCurrent = isCurrent;
            if (isCurrent) ViewBag.Tournament = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi).Ten;
            return View(model);
        }
    }
}