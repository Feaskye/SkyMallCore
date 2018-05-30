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
    public class ItemsDataController : SysBaseController
    {
        private ISysItemsDetailService SysItemsDetailService;

        public ItemsDataController(ISysItemsDetailService sysItemsDetailService)
        {
               SysItemsDetailService = sysItemsDetailService;
        }


        [HttpGet]
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = SysItemsDetailService.GetList(itemId, keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetSelectJson(string enCode)
        {
            var data = SysItemsDetailService.GetItemList(enCode);
            List<object> list = new List<object>();
            foreach (SysItemsDetail item in data)
            {
                list.Add(new { id = item.ItemCode, text = item.ItemName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SysItemsDetailService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysItemsDetail SysItemsDetail, string keyValue)
        {
            SysItemsDetailService.SubmitForm(SysItemsDetail, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [SysRoleAuth]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SysItemsDetailService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}