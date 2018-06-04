using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SkyMallCore.Core;
using SkyMallCore.Models;

namespace SkyMallCoreWeb.Areas
{

    /// <summary>
    /// 后台管理基类
    /// </summary>
    [Area("SystemManage")]
    public class SysBaseController : SysControllerBase
    { }

    /// <summary>
    /// 后台安全管理基类
    /// </summary>
    [Area("SystemSecurity")]
    public class SysSecBaseController : SysControllerBase
    {  }




    /// <summary>
    /// 后台全局管理控制器基类
    /// </summary>
    [SysManageAuth]
    public class SysControllerBase : Controller
    {
        public ILogger _Logger;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _Logger = CoreContextProvider.GetLogger(this.ControllerContext.ActionDescriptor.ControllerTypeInfo.Name);
            base.OnActionExecuting(context);
        }


        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult Details()
        {
            return View();
        }


        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }




    }
}