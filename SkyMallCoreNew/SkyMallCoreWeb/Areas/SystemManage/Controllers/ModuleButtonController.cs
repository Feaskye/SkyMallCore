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
    public class ModuleButtonController : SysBaseController
    {

        ISysModuleButtonService ModuleButtonService;
        ISysModuleService SysModuleService;
        public ModuleButtonController(ISysModuleButtonService moduleButtonService
            , ISysModuleService sysModuleService)
        {
            ModuleButtonService = moduleButtonService;
            SysModuleService = sysModuleService;
        }

        [HttpGet]
        public ActionResult GetTreeSelectJson(string moduleId)
        {
            var data = ModuleButtonService.GetList(moduleId);
            var treeList = new List<TreeSelectModel>();
            foreach (SysModuleButton item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.FullName;
                treeModel.parentId = item.ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        public ActionResult GetTreeGridJson(string moduleId)
        {
            var data = ModuleButtonService.GetList(moduleId);
            var treeList = new List<TreeGridModel>();
            foreach (SysModuleButton item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = ModuleButtonService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysModuleButton SysModuleButton, string keyValue)
        {
            ModuleButtonService.SubmitForm(SysModuleButton, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            ModuleButtonService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpGet]
        public ActionResult CloneButton()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCloneButtonTreeJson()
        {
            var moduledata = SysModuleService.GetList();
            var buttondata = ModuleButtonService.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (var item in moduledata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = moduledata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                treeList.Add(tree);
            }
            foreach (SysModuleButton item in buttondata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = buttondata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.ModuleId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.hasChildren = hasChildren;
                if (item.Icon != "")
                {
                    tree.img = item.Icon;
                }
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpPost]
        public ActionResult SubmitCloneButton(string moduleId, string Ids)
        {
            ModuleButtonService.SubmitCloneButton(moduleId, Ids);
            return Success("克隆成功。");
        }
    }
}