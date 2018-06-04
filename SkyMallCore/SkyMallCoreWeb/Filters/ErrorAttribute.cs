using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SkyMallCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb
{
    public class ErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = CoreContextProvider.GetLogger(context.RouteData.Values["controller"].ToString());
            logger.LogError(context.Exception.Message);

            base.OnException(context);

            if (!CoreContextProvider.HostingEnvironment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };
            }
          
        }


    }
}
