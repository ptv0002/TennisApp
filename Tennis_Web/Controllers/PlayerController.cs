﻿using DataAccess;
using Library;
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
            var destination = await new DatabaseMethod<DS_VDV>(_context).GetOjectFromDBAsync(id);
            return View(destination);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, DS_VDV source)
        {
            var columnsToSave = new List<string> { "Ho", "Ten", "Ten_Tat", "CLB", "KhachMoi", "FileAnh", "Tel", "Email", "Status", "CongTy", "ChucVu" };
            var result = await new DatabaseMethod<DS_VDV>(_context).SaveObjectToDBAsync(id, source, columnsToSave);
            return RedirectToAction(nameof(Index));
        }
    }
}