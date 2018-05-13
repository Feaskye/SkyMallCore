using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class HomeController : Controller
    {
        [Area("SystemManage")]
        public IActionResult Index()
        {
            return Content("SystemManage Page");
            //return View();
        }
    }
}
