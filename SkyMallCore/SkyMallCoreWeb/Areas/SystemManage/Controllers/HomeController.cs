using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : SysBaseController
    {


        public IActionResult About()
        {
            var descrition = new StringBuilder();
            descrition.Append("1：该项目是由NFine开源项目转化而来    \r\n");
            descrition.Append("2：该项目基本框架开发平台是在Asp.net Core 2.0基础上编写\r\n");
            descrition.Append("3：旨在促进.Net Core跨平台学习交流，提高开发效率");
            descrition.Append("4：.Net Core Mvc + EFCore 等技术，该项目仍会继续完善！");
            return Content(descrition.ToString());
        }


    }
}
