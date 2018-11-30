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
        public ActionResult SubmitForm(Link link, string keyValue)
        {
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _LinkService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该分组不存在，请刷新重试！");
                }
                data.LinkName = link.LinkName;
                data.LinkUrl = link.LinkUrl;
                data.EnabledMark = link.EnabledMark;
                data.Description = link.Description;
                data.SortCode = link.SortCode;
                link = data;
            }
            _LinkService.SubmitForm(link);
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
