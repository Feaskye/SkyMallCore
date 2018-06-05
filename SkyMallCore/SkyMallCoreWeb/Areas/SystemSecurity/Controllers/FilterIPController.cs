
using SkyMallCore.Core;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Models;
using SkyMallCore.Services;

namespace SkyMallCoreWeb.Areas.SystemSecurity.Controllers
{
    public class FilterIPController : SysSecBaseController
    {
        private IFilterIPService FilterIPApp;

        public FilterIPController(IFilterIPService filterIPApp)
        {
            FilterIPApp = filterIPApp;
        }


        [HttpGet]
        public ActionResult GetGridJson(string keyword)
        {
            var data = FilterIPApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = FilterIPApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FilterIP filterIPEntity, string keyValue)
        {
            var result = FilterIPApp.SubmitForm(filterIPEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [SysRoleAuth]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            FilterIPApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
