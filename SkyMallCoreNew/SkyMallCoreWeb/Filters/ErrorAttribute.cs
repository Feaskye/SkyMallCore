using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SkyMallCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb
{
    public class ErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var controller = context.RouteData.Values["controller"].ToString();
            var area= "";
            if (context.RouteData.Values.Keys.Contains("area"))
            {
                area = context.RouteData.Values["area"].ToString();
            }
            var logger = CoreContextProvider.GetLogger(controller);
            logger.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);

            if (!CoreContextProvider.HostingEnvironment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;
                if (context.HttpContext.Request.IsAjax())
                {
                    context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };
                }
                else
                {
                    context.Result = new RedirectToActionResult("ErrorPage", controller, new { area, Message = "您的请求发生异常，请确认输入正确重试！" });
                }
            }

        }


    }
}
