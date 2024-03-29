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
using Library;
using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Tennis_Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TennisContext _context;
        private readonly INotyfService _notyf;
        public TournamentController(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IActionResult Index(bool isCurrent)
        {
            var model = _context.DS_Giais.OrderByDescending(m => m.Ngay).Where(m => m.Giai_Moi == isCurrent).ToList();
            ViewBag.isCurrent = isCurrent;
            return View(model);
        }
        public IActionResult TournamentInfo(TabViewModel model, bool isCurrent, int giaiID, int trinhID)
        {
            bool a1 = giaiID == 0;
            bool a2 = trinhID == 0;
            bool a3 = model.ID == 0;
            // Assign default value for first time access
            if (!a1 && a3)
            {
                model = new TabViewModel
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
            var tournament = _context.DS_Giais.Find(model.ID);
            ViewBag.TournamentTitle = tournament != null ? tournament.Ten : "mới";
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateInfo(DS_Giai item)
        {
            // Find and update Tournament Info
            item.Giai_Moi = true;
            var columnsToSave = new List<string> { "Ten", "Ghi_Chu", "Ngay", "Giai_Moi" };
            var result = new DatabaseMethod<DS_Giai>(_context).SaveObjectToDB(item.Id, item, columnsToSave);
            _context.SaveChanges();
            var idGiai = item.Id == 0 ? _context.DS_Giais.FirstOrDefault(m => m.Ten == item.Ten && m.Ngay == item.Ngay).Id : item.Id;
            // Assign value for view model
            //var vm = new TabViewModel
            //{
            //    ActiveTab = Tab.Info,
            //    IsCurrent = true,
            //    ID = item.Id == 0 ? _context.DS_Giais.FirstOrDefault(m => m.Ten == item.Ten && m.Ngay == item.Ngay).Id : item.Id,
            //    Succeeded = result.Succeeded

            //};
            // If save successfully, view error and display View with model from DB 
            //return RedirectToAction(nameof(TournamentInfo), vm);
            return new MethodController(_context).TabVMGenerator_Tournament(idGiai, result.Succeeded,
               Tab.Info);
        }
        public IActionResult EndTournament(string id)
        {
            var intId = Convert.ToInt32(id);
            // Find the current Tournament and set IsCurrent to false
            var item = _context.DS_Giais.Find(intId);
            var mTrinhs = _context.DS_Trinhs.Where(m => m.ID_Giai == intId).ToList();
            item.Giai_Moi = false;
            _context.Update(item);
            // Phân bổ điểm vào file DS_VDVDiem, đồng thời cập nhật điểm trong DS_VDV
            foreach (var mTrinh in mTrinhs)
            {
                new ScoreCalculation(_context).Player_PointDistribution(mTrinh.Id);
            }
            // Reset Participation status to all false and Pair code to null
            var list = _context.DS_VDVs.ToList();
            list.ForEach(m => { m.Tham_Gia = false; m.Phe_Duyet = false; });
            _context.UpdateRange(list);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), true);
        }
        public IActionResult AddLevel(string newLevel, string idGiai)
        {
            _context.Add(new DS_Trinh
            {
                Trinh = Convert.ToInt32(newLevel),
                ID_Giai = Convert.ToInt32(idGiai)
            });
            _context.SaveChanges();
            // Assign value for view model
            //var vm = new TabViewModel
            //{
            //    ActiveTab = Tab.LevelList,
            //    IsCurrent = true,
            //    ID = Convert.ToInt32(idGiai)
            //};
            //return RedirectToAction(nameof(TournamentInfo), vm);
            return new MethodController(_context).TabVMGenerator_Tournament(Convert.ToInt32(idGiai), true,
               Tab.LevelList);
        }
        public IActionResult DeleteLevel(string id)
        {
            var intId = Convert.ToInt32(id);
            var item = _context.DS_Trinhs.Find(intId);
            _context.Remove(item);
            _context.SaveChanges();
            // Assign value for view model
            //var vm = new TabViewModel
            //{
            //    ActiveTab = Tab.LevelList,
            //    IsCurrent = true,
            //    ID = item.ID_Giai,
            //    Succeeded = true
            //};
            //return RedirectToAction(nameof(TournamentInfo), vm);
            return new MethodController(_context).TabVMGenerator_Tournament(item.ID_Giai, true,
                Tab.LevelList);
        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 8000)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public IActionResult SavePlayerState(List<DS_VDV> list, int idGiai)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Tham_Gia" }).Succeeded;
                    if (!result) break;
                }
                if (result) _context.SaveChanges();
            }
            else result = false;
            // Assign value for view model
            //var vm = new TabViewModel
            //{
            //    ActiveTab = Tab.Player,
            //    IsCurrent = true,
            //    ID = idGiai,
            //    Succeeded = result
            //};
            ////CurrentModel = JsonSerializer.Serialize(list)
            ////TempData["PlayerList"] = JsonSerializer.Serialize(list);
            //return RedirectToAction(nameof(TournamentInfo), vm);
            return new MethodController(_context).TabVMGenerator_Tournament(idGiai, result,
                Tab.Player);
        }
        public IActionResult ApprovePlayer()
        {
            var model = _context.DS_VDVs.Where(m => m.Phe_Duyet == true).ToList();
            bool? success = (bool?)TempData["PlayerApproval"];
            if (success == true) _notyf.Success("Đã lưu phê duyệt thành công!");
            else if (success == false) _notyf.Error("Có lỗi xảy ra khi đang lưu!");
            return View(model);
        }
        //[HttpPost]
        //public IActionResult ApprovePlayer(List<DS_VDV> list)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool result = false;
        //        foreach (var item in list)
        //        {
        //            item.Phe_Duyet = false;
        //            //if (item.Phe_Duyet == true) 
        //            //{ 
        //            //    item.Tham_Gia = true;
        //            //    item.Phe_Duyet = false;
        //            //}
        //            result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Phe_Duyet", "Tham_Gia" }).Succeeded;
        //            if (!result) break;
        //        }
        //        if (result)
        //        {
        //            _context.SaveChanges();
        //            TempData["PlayerApproval"] = true;
        //        }
        //        else TempData["PlayerApproval"] = false;
        //    }
        //    return RedirectToAction(nameof(ApprovePlayer));
        //}
        public IActionResult DeletePair(string id)
        {
            var pair = _context.DS_Caps.Find(Convert.ToInt32(id));
            new MethodController(_context).DeletePair_Method(pair);
            return RedirectToAction(nameof(ApprovePair));
        }
        public IActionResult ApprovePair()
        {
            var model = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m =>m.DS_Trinh)
                .Where(m => m.Xac_Nhan == true && m.Phe_Duyet).ToList();
            ViewBag.NotEnough = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m => m.DS_Trinh)
                .Where(m => m.Xac_Nhan == false && m.Phe_Duyet).ToList();
            bool? success = (bool?)TempData["PairApproval"];
            if (success == true) _notyf.Success("Đã lưu phê duyệt thành công!");
            else if (success == false) _notyf.Error("Có lỗi xảy ra khi đang lưu!");
            return View(model);
        }
        public IActionResult ApprovePairAction(int id)
        {
            var item = new DS_Cap
            {
                Id = id,
                Phe_Duyet = false,
                Xac_Nhan = false
            };
            bool result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Phe_Duyet", "Xac_Nhan" }).Succeeded;

            if (result)
            {
                _context.SaveChanges();
                TempData["PairApproval"] = true;
            }
            else TempData["PairApproval"] = false;

            return RedirectToAction(nameof(ApprovePair));
        }
        //public IActionResult TabVMGenerator (Tab tabName, int idTrinh)
        //{
        //    var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == idTrinh).FirstOrDefault();
        //    // Assign value for view model
        //    var vm = new TabViewModel
        //    {
        //        ActiveTab = tabName,
        //        IsCurrent = true,
        //        ID = temp.Id,
        //        DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
        //    };
        //    return RedirectToAction(nameof(LevelInfo), vm);
        //}
        //[HttpPost]
        //public IActionResult ApprovePair(List<DS_Cap> list)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool result = false;
        //        foreach (var item in list)
        //        {
        //            // Save approved status to DB
        //            if (item.Phe_Duyet)
        //            {
        //                result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Phe_Duyet" }).Succeeded;
        //            }
        //            // Delete pair if not approved
        //            else
        //            {
        //                _context.Remove(item);
        //                result = true;
        //            }
        //            if (!result) break;
        //        }
        //        if (result)
        //        {
        //            _context.SaveChanges();
        //            TempData["PairApproval"] = true;
        //        }
        //        else TempData["PairApproval"] = false;
        //    }
        //    return RedirectToAction(nameof(ApprovePlayer));
        //}
    }
}
