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
            //var temp = new ExcelMethod<DS_Vong>();
            //ResultModel<DS_Vong> a = temp.ExcelToList("C:/TennisApp/Excel/Giai_Dau.xlsx", "DS_Vong");
            //_context.AddRange(a.List);
            //_context.SaveChanges();
            return View();
        }
        public IActionResult Member()
        {
            var model = _context.DS_VDVs.Where(m => !m.Khach_Moi).OrderByDescending(m => m.Diem).ToList();
            return View(model);
        }
        public IActionResult Guest()
        {
            var model = _context.DS_VDVs.Where(m => m.Khach_Moi).OrderByDescending(m => m.Diem).ToList();
            return View(model);
        }
        public IActionResult Result(int id)
        {
            return View();
        }
        public IActionResult Announcement()
        {
            return View();
        }
        
    }
}