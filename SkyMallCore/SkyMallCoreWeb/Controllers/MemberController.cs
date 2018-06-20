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
    
    public class MemberController : BaseController
    {
        IMemberService _MemberService;
        public MemberController(IMemberService memberService)
        {
            _MemberService = memberService;
        }

        //首页
        public IActionResult Index()
        {
            var users = _MemberService.GetList();
            return Content("Member中心");
        }
    }
}
