using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCoreWeb.Models;

namespace SkyMallCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        ISysUserService _SysUserService;
        public HomeController(ISysUserService sysUserService)
        {
            _SysUserService = sysUserService;
        }

        public IActionResult Index()
        {
            var users = _SysUserService.GetUsers();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
