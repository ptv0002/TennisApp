﻿using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Tennis_Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _environment;
        public TournamentController(TennisContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index(bool isCurrent)
        {
            var model = _context.DS_Giais.OrderByDescending(m => m.Ngay).Where(m => m.GiaiMoi == isCurrent).ToList();
            ViewBag.isCurrent = isCurrent;
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool isCurrent, int? id, string detailedTitle)
        {
            var vm = new TournamentTabViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                DetailedTitle = detailedTitle
            };
            switch (tabname)
            {
                case "Parameter":
                    vm.ActiveTab = Tab.Parameter;
                    return RedirectToAction(nameof(LevelInfo), vm);
                case "Division":
                    vm.ActiveTab = Tab.Division;
                    return RedirectToAction(nameof(LevelInfo), vm);
                case "Info":
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                case "LevelList":
                    vm.ActiveTab = Tab.LevelList;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                case "Player":
                    vm.ActiveTab = Tab.Player;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                default:
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction(nameof(TournamentInfo), vm);
            }
        }
        public IActionResult TournamentInfo(TournamentTabViewModel model, bool isCurrent, int? giaiID, int? trinhID)
        {
            bool a1 = giaiID == null;
            bool a2 = trinhID == null;
            bool a3 = model.ID == null;
            // Assign default value for first time access
            if (!a1 && a3)
            {
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Info,
                    IsCurrent = isCurrent,
                    ID = giaiID
                };
            }
            if (a1 && !a2)
            {
                var temp = _context.DS_Trinhs.Find(trinhID);
                model.ID = temp.ID_Giai;
            }
            else if (a1 && a2 && a3)
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
                return View(model);
            }
            //ViewBag.LevelList = _context.DS_Trinhs.OrderByDescending(m => m.Trinh).Where(m => m.ID_Giai == model.ID).ToList();
            ViewBag.TournamentTitle = _context.DS_Giais.Find(model.ID).Ten;
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateInfo(DS_Giai model)
        {
            // Find and update Tournament Info
            var item = _context.DS_Giais.Find(model.Id);
            item.Ten = model.Ten;
            item.GhiChu = model.GhiChu;
            item.Ngay = model.Ngay;
            _context.Update(item);
            // Assign value for view model
            var vm = new TournamentTabViewModel
            {
                ActiveTab = Tab.Info,
                IsCurrent = true,
                ID = model.Id
            };
            return RedirectToAction(nameof(TournamentInfo), vm);
        }

        public async Task<IActionResult> EndTournament(int id)
        {
            // Find the current Tournament and set IsCurrent to false
            var item = _context.DS_Giais.Find(id);
            item.GiaiMoi = false;
            _context.Update(item);
            // Reset Participation status to all false
            var list = _context.DS_VDVs.Where(m => m.Tham_Gia == true).ToList();
            list.ForEach(m => m.Tham_Gia = false);
            _context.Update(list);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), true);
        }
        // ----------------------------------------------------- Level Related -----------------------------------------------------
        public IActionResult LevelInfo(TournamentTabViewModel model, bool isCurrent, int? trinhID, string detailedTitle)
        {
            if (model.ID == null)
            {
                // Assign default value for first time access
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Parameter,
                    IsCurrent = isCurrent,
                    ID = trinhID,
                    DetailedTitle = detailedTitle
                };
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateParameter(DS_Trinh item)
        {
            //var temp = new MethodController(_context, _environment);
            //var excel = temp.GetExcel();
            //var row = item.Id;
            //using (excel)
            //{
            //    var sheet = excel.Workbook.Worksheets["DS_Trinh"];
            //    sheet.Cells[row, temp.GetColumn("DiemTru", sheet)].Value = item.DiemTru;
            //    sheet.Cells[row, temp.GetColumn("Diem_PB", sheet)].Value = item.Diem_PB;
            //    sheet.Cells[row, temp.GetColumn("TL_BanKet", sheet)].Value = item.TL_BanKet;
            //    sheet.Cells[row, temp.GetColumn("TL_Bang", sheet)].Value = 100 - item.TL_VoDich - item.TL_ChungKet - item.TL_BanKet - item.TL_TuKet; // TL_Bang
            //    sheet.Cells[row, temp.GetColumn("TL_ChungKet", sheet)].Value = item.TL_ChungKet;
            //    sheet.Cells[row, temp.GetColumn("TL_TuKet", sheet)].Value = item.TL_TuKet;
            //    sheet.Cells[row, temp.GetColumn("TL_VoDich", sheet)].Value = item.TL_VoDich;
            //    excel.Save();
            //}
            //var tourSheet = temp.GetWorkSheet("DS_Giai");
            //var levSheet = temp.GetWorkSheet("DS_Trinh");
            //string tournament = tourSheet.Cells[2, temp.GetColumn("Ten", tourSheet)].Text;
            //string level = levSheet.Cells[row, temp.GetColumn("Trinh", levSheet)].Text; 

            var model = _context.DS_Trinhs.Find(item.Id);
            model.Trinh = item.Trinh;
            model.DiemTru = item.DiemTru;
            model.Diem_PB = item.Diem_PB;
            model.TL_VoDich = item.TL_VoDich;
            model.TL_ChungKet = item.TL_ChungKet;
            model.TL_BanKet = item.TL_BanKet;
            model.TL_TuKet = item.TL_TuKet;
            model.TL_Bang = 100 - item.TL_VoDich - item.TL_ChungKet - item.TL_BanKet - item.TL_TuKet; // TL_Bang
            _context.Update(item);
            await _context.SaveChangesAsync();

            var temp = _context.DS_Giais.Find(model.ID_Giai);
            // Assign value for view model
            var vm = new TournamentTabViewModel
            {
                ActiveTab = Tab.Parameter,
                IsCurrent = true,
                ID = model.Id,
                DetailedTitle = "Giải " + temp.Ten + " - Trình " + model.Trinh
            };
            return RedirectToAction(nameof(LevelInfo), vm);
        }
        //[HttpPost]
        public async Task<IActionResult> AddLevel(string newLevel, string idGiai)
        {
            _context.Add(new DS_Trinh { 
                Trinh = Convert.ToInt32(newLevel),
                ID_Giai = Convert.ToInt32(idGiai)
            });
            await _context.SaveChangesAsync();
            // Assign value for view model
            var vm = new TournamentTabViewModel
            {
                ActiveTab = Tab.LevelList,
                IsCurrent = true,
                ID = Convert.ToInt32(idGiai)
            };
            return RedirectToAction(nameof(TournamentInfo), vm);
        }
        public async Task<IActionResult> DeleteLevel(int id)
        {
            var item = await _context.DS_Trinhs.FindAsync(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();
            // Assign value for view model
            var vm = new TournamentTabViewModel
            {
                ActiveTab = Tab.LevelList,
                IsCurrent = true,
                ID = item.ID_Giai
            };
            return RedirectToAction(nameof(TournamentInfo), vm);
        }
    }
}
