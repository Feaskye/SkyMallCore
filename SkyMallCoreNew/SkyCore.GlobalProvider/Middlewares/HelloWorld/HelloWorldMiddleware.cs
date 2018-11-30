using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyCore.GlobalProvider
{
    /// <summary>
    /// 创建中间件 demo
    /// </summary>
    public class HelloWorldMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloWorldMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("middle ware demo");
            await _next(context);
        }
    }


    public static class UseHelloWorldExtensions
    {
        public static IApplicationBuilder UseHelloWorld(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HelloWorldMiddleware>();
        }

    }


}
