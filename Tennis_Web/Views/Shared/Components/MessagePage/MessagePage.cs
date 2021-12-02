using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tennis_Web
{
    [ViewComponent]
    public class MessagePage : ViewComponent
    {
        public const string COMPONENTNAME = "MessagePage";
        // Dữ liệu nội dung trang thông báo
        public class Message
        {
            public string Title { set; get; } = "Thông báo";     // Tiêu đề của Box hiện thị
            public string Htmlcontent { set; get; } = "";         // Nội dung HTML hiện thị
            public string Urlredirect { set; get; } = "/";        // Url chuyển hướng đến
            public int Secondwait { set; get; } = 3;              // Sau secondwait giây thì chuyển
        }
        public IViewComponentResult Invoke(Message message)
        {
            // Thiết lập Header của HTTP Respone - chuyển hướng về trang đích
            this.HttpContext.Response.Headers.Add("REFRESH", $"{message.Secondwait};URL={message.Urlredirect}");
            return View(message);
        }
    }
}
