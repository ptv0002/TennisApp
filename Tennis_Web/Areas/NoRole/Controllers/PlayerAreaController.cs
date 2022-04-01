﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    public class PlayerAreaController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly INotyfService _notyf;
        public PlayerAreaController(TennisContext context, IWebHostEnvironment webHost, INotyfService notyf)
        {
            _context = context;
            _webHost = webHost;
            _notyf = notyf;
        }
        public IActionResult Index(bool isCurrent, bool isGuest, bool participate)
        {
            List<DS_VDV> model = new();
            if (isCurrent)
            {
                model = _context.DS_VDVs.Where(m => m.Tham_Gia == participate).OrderByDescending(m => m.Diem).ToList();
                ViewBag.IsCurrent = true;
                ViewBag.Title = participate ? "VĐV tham gia" : "Đăng kí tham gia";
                ViewBag.Info = participate;
                ViewBag.Tournament = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi).Ten;
                // If register tab is active, display error or success message after registering
                if (!participate)
                {
                    bool? success = (bool?)TempData["SuccessfulRegister"];
                    if (success == true)
                    {
                        _notyf.Success("Đã đăng kí thành công!");
                        _notyf.Information("Chờ ban quản trị phê duyệt", 30);
                    }
                    else if (success == false) _notyf.Error("Có lỗi xảy ra khi đang đăng kí!");
                }
            }
            else
            {
                model = _context.DS_VDVs.OrderByDescending(m => m.Diem).Where(m => m.Khach_Moi == isGuest).ToList();
                ViewBag.Title = isGuest ? "Hội viên khách mời" : "Hội viên chính thức";
                ViewBag.Info = true;
                ViewBag.IsCurrent = false;
            }
            return View(model);
        }
        public IActionResult History(int id)
        {
            var matches = _context.DS_Trans.Include(m => m.DS_Cap1.VDV1).Include(m => m.DS_Cap1.VDV2)
                .Include(m => m.DS_Cap2.VDV1).Include(m => m.DS_Cap2.VDV2)
                .Include(m => m.DS_Trinh.DS_Giai).Include(m => m.DS_Trinh)
                .Where(m => m.DS_Cap1.VDV1.Id == id || m.DS_Cap1.VDV2.Id == id || m.DS_Cap2.VDV1.Id == id || m.DS_Cap2.VDV2.Id == id)
                .OrderByDescending(m => m.DS_Trinh.DS_Giai.Ngay).ThenByDescending(m => m.Ma_Vong)
                .ToList();

            var DS_VDVDiem = _context.DS_VDVDiems.Where(m => m.ID_Vdv == id).OrderBy(m => m.Ngay).Select(m => new
            Extended_VDVDiem
            {
                Id = m.Id,
                ID_Trinh = m.ID_Trinh,
                ID_Vdv = m.ID_Vdv,
                Diem = m.Diem,
                Ngay = m.Ngay,
                Ghi_Chu = m.Ghi_Chu,
                Diem_Moi = 0,
                Diem_Cu = 0
            }).ToList();
            DS_VDVDiem[0].Diem_Moi = DS_VDVDiem[0].Diem_Cu + DS_VDVDiem[0].Diem;
            for (int i = 1; i < DS_VDVDiem.Count; i++)
            {
                DS_VDVDiem[i].Diem_Cu = DS_VDVDiem[i - 1].Diem_Moi;
                DS_VDVDiem[i].Diem_Moi = DS_VDVDiem[i].Diem_Cu + DS_VDVDiem[i].Diem;
            }

            return View(new PlayerHistoryViewModel
            {
                VDV = _context.DS_VDVs.Find(id),
                DS_Tran = matches,
                DS_VDVDiem = DS_VDVDiem.OrderByDescending(m => m.Ngay).ToList()
            });
        }
    }
}