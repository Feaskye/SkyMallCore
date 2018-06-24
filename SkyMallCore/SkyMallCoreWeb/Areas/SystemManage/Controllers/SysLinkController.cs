using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class SysLinkController : SysBaseController
    {

        private ILinkService _LinkService;

        public SysLinkController(ILinkService LinkService)
        {
            _LinkService = LinkService;
        }


        [HttpGet]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _LinkService.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _LinkService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Link link, string permissionIds, string keyValue)
        {
            _LinkService.SubmitForm(link, permissionIds.Split(','), keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _LinkService.DeleteForm(keyValue);
            return Success("删除成功。");
        }

    }
}
