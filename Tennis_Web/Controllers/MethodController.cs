using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Controllers
{
    public class MethodController : Controller
    {
        public class ConfirmViewModal
        {
            public string BtnMsg { get; set; }
            public string Message { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public string Id { get; set; }
        }
        
        public IActionResult ConfirmModal(ConfirmViewModal model)
        {
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ConfirmModal(ConfirmViewModal model, string any = "")
        {
            return RedirectToAction(model.ActionName, model.ControllerName, new { id = model.Id });
        }
    }
}
