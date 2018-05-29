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
    public class ItemsTypeController : SysBaseController
    {
        private ISysItemsService SysItemsService;

        public ItemsTypeController(ISysItemsService sysItemsService)
        {
            SysItemsService = sysItemsService;
        }


        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = SysItemsService.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
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
        
        public ActionResult GetTreeJson()
        {
            var data = SysItemsService.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (var item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        
        public ActionResult GetTreeGridJson()
        {
            var data = SysItemsService.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
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
            var data = SysItemsService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysItems SysItems, string keyValue)
        {
            SysItemsService.SubmitForm(SysItems, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SysItemsService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}