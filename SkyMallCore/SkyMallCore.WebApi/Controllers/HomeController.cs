using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.WebApi.Models;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 获取首页内容  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
        

    }











}
