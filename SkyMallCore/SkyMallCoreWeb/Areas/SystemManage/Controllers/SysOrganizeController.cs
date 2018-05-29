

using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;
using System.Collections.Generic;
using System.Linq;


namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class SysOrganizeController : SysBaseController
    {
        ISysOrganizeService _Service;
        public SysOrganizeController(ISysOrganizeService sysOrganizeService)
        {
            _Service = sysOrganizeService;
        }

        [HttpGet]
        public ActionResult GetTreeSelectJson()
        {
            var data = _Service.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.FullName;
                treeModel.parentId = item.ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        public ActionResult GetTreeJson()
        {
            var data = _Service.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (SysOrganize item in data)
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
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = _Service.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(t => t.FullName.Contains(keyword)).ToList();
            }
            var treeList = new List<TreeGridModel>();
            foreach (SysOrganize item in data)
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
            var data = _Service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysOrganize SysOrganize, string keyValue)
        {
            _Service.SubmitForm(SysOrganize, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _Service.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
