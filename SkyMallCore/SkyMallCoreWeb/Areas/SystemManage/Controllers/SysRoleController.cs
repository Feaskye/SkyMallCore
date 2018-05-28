using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCore.Core;
using SkyMallCore.Models;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class SysRoleController : SysControllerBase
    {
        private ISysRoleService SysRoleService;
        private ISysRoleAuthorizeService SysRoleAuthorizeService ;
        private ISysModuleService SysModuleService ;
        private ISysModuleButtonService SysModuleButtonService ;

        public SysRoleController(
            ISysRoleService sysRoleService,
        ISysRoleAuthorizeService sysRoleAuthorizeService,
            ISysModuleService sysModuleService,
        ISysModuleButtonService sysModuleButtonService
            )
        {
            SysRoleService = sysRoleService;
            SysRoleAuthorizeService = sysRoleAuthorizeService;
            SysModuleService = sysModuleService;
            SysModuleButtonService = sysModuleButtonService;
        }



        [HttpGet]
        public ActionResult GetGridJson(string keyword)
        {
            var data = SysRoleService.GetListBykeyword(keyword);
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
        public ActionResult SubmitForm(SysRole roleEntity, string permissionIds, string keyValue)
        {
            SysRoleService.SubmitForm(roleEntity, permissionIds.Split(','), keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SysRoleService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}