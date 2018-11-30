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
    public class SysAreaController : SysBaseController
    {
        private ISysAreaService SysAreaService;

        public SysAreaController(ISysAreaService sysAreaService)
        {
            SysAreaService = sysAreaService;
        }


        [HttpGet]
        public ActionResult GetTreeSelectJson()
        {
            var data = SysAreaService.GetList();
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
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = SysAreaService.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id;
                treeModel.text = item.FullName;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.ParentId;
                treeModel.expanded = true;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SysAreaService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysArea SysArea, string keyValue)
        {
            SysAreaService.SubmitForm(SysArea, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [TypeFilter(typeof(SysRoleAuthAttribute))]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SysAreaService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}