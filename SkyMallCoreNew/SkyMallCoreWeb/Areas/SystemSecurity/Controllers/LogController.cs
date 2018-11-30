


using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using System;

namespace SkyMallCoreWeb.Areas.SystemSecurity.Controllers
{
    public class LogController : SysSecBaseController
    {
        private ISysLogService _LogApp;

        public LogController(ISysLogService logApp)
        {
            _LogApp = logApp;
        }


        [HttpGet]
        public ActionResult RemoveLog()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetGridJson(LogSearchView search, int page = 1)
        {
            if (!search.StartTime.HasValue)
            {
                search.TimeType = "1";
                search.StartTime =Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-01 00:00:00"));
                search.EndTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd 23:59:59"));
            }
            var data = _LogApp.GetList(search, page, PageSize);
            return Content(new
            {
                rows = data,
                total = data.PageCount,
                page = data.PageIndex,
                records = data.TotalCount
            }.ToJson());
        }
        [HttpPost]
        [TypeFilter(typeof(SysRoleAuthAttribute))]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRemoveLog(string keepTime)
        {
            _LogApp.RemoveLog(keepTime);
            return Success("清空成功。");
        }
    }
}
