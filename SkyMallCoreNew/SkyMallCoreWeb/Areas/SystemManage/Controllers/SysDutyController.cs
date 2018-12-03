using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Services;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class SysDutyController : SysBaseController
    {
        private ISysRoleService SysRoleService;

        public SysDutyController(ISysRoleService sysRoleService)
        {
            SysRoleService = sysRoleService;
        }


        [HttpGet]
        public ActionResult GetGridJson(string keyword)
        {
            var data = SysRoleService.GetList(keyword,true);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SysRoleService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysRole roleEntity, string keyValue)
        {
            SysRoleService.SubmitForm(roleEntity);
            return Success("操作成功。");
        }
        [HttpPost]
        [TypeFilter(typeof(SysRoleAuthAttribute))]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SysRoleService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}