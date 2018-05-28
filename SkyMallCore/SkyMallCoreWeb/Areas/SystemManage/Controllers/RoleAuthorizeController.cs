using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class RoleAuthorizeController : SysControllerBase
    {
        private ISysRoleAuthorizeService SysRoleAuthorizeService;
        private ISysModuleService SysModuleService;
        private ISysModuleButtonService SysModuleButtonService;

        public RoleAuthorizeController(ISysRoleAuthorizeService sysRoleAuthorizeService,
        ISysModuleService sysModuleService,
        ISysModuleButtonService sysModuleButtonService)
        {
            SysRoleAuthorizeService = sysRoleAuthorizeService;
            SysModuleService = sysModuleService;
            SysModuleButtonService = sysModuleButtonService;
        }


        public ActionResult GetPermissionTree(string roleId)
        {
            var moduledata = SysModuleService.GetList();
            var buttondata = SysModuleButtonService.GetList();
            var authorizedata = new List<SysRoleAuthorize>();
            if (!string.IsNullOrEmpty(roleId))
            {
                authorizedata = SysRoleAuthorizeService.GetList(roleId);
            }
            var treeList = new List<TreeViewModel>();
            foreach (SysModule item in moduledata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = moduledata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = authorizedata.Count(t => t.ItemId == item.Id);
                tree.hasChildren = true;
                tree.img = item.Icon == "" ? "" : item.Icon;
                treeList.Add(tree);
            }
            foreach (SysModuleButton item in buttondata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = buttondata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId == "0" ? item.ModuleId : item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = authorizedata.Count(t => t.ItemId == item.Id);
                tree.hasChildren = hasChildren;
                tree.img = item.Icon == "" ? "" : item.Icon;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
    }
}