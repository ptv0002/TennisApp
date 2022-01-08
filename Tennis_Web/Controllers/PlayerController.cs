using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly TennisContext _context;
        public PlayerController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.DS_VDVs.OrderBy(m => m.Diem).ToList();
            return View(model);
        }
        public async Task<IActionResult> Update(int? id)
        {
            var model = new DS_VDV();
            if (id != null)
            {
                var item = await _context.DS_VDVs.FindAsync(id);
                foreach (var prop in model.GetType().GetProperties())
                {
                    //prop.SetValue(model, item.GetType().GetProperty(prop.Name).GetValue(item));
                    prop.SetValue(model, prop.GetValue(item));
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, DS_VDV model)
        {

            return View(model);
        }
    }
}
