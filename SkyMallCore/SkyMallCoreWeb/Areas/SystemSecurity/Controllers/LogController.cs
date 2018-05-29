


using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Services;

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
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _LogApp.GetList(pagination, queryJson),
                pagination.total,
                pagination.page,
                pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpPost]
        //[HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRemoveLog(string keepTime)
        {
            _LogApp.RemoveLog(keepTime);
            return Success("清空成功。");
        }
    }
}
