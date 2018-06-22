using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCoreWeb.Models;

namespace SkyMallCoreWeb.Controllers
{
    
    public class HomeController : FBaseController
    {
        ISysUserService _SysUserService;
        public HomeController(ISysUserService sysUserService)
        {
            _SysUserService = sysUserService;
        }

        //首页
        public IActionResult Index()
        {
            var users = _SysUserService.GetUsers();
            return View();
        }

        [MemberAuth]
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
