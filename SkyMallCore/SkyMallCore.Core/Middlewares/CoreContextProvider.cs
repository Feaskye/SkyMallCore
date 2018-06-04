using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SkyMallCore.Core
{

    /// <summary>
    /// Core 全局支持上下文
    /// 配置、Service、HttpContext、用户
    /// （可以改为注入的方式，这里为了避免传参麻烦）
    /// </summary>
    public class CoreContextProvider
    {
        private string LoginUserKey = ConstParameters.SysLoginUserKey;
        private string LoginProvider = ConstParameters.SysLoginProvider;

        private static IHttpContextAccessor _accessor;

        public static IConfiguration Configuration { get; set; }
        //private static IServiceCollection ServiceCollection { get; set; }

        public static IHostingEnvironment HostingEnvironment { get; set; }

        public static Microsoft.AspNetCore.Http.HttpContext HttpContext => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor, IConfiguration configuration,IHostingEnvironment hostingEnvironment)
        {
            _accessor = accessor;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        
        /// <summary>
        /// 获取MemCache上下文
        /// </summary>
        public static IMemCache MemCache
        {
            get
            {
                return GetService<IMemCache>();
            }
        }


        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public static OperatorModel CurrentSysUser
        {
            get
            {
                //HttpContext.User.Identities.Where(w => w.AuthenticationType == SysManageAuthAttribute.SysManageAuthScheme).FirstOrDefault();
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                if (claimsIdentity == null)
                {
                    throw new Exception("用户未登录");
                }
                var claims = claimsIdentity.Claims;
                return new OperatorModel()
                {
                    UserId = claims.Where(w => w.Type == ClaimTypes.Sid).Select(u => u.Value).FirstOrDefault(),
                    Account = claims.Where(w => w.Type == ClaimTypes.Name).Select(u => u.Value).FirstOrDefault(),
                    RealName = claims.Where(w => w.Type == ClaimTypes.GivenName).Select(u => u.Value).FirstOrDefault(),
                    OrganizeId = claims.Where(w => w.Type == ClaimTypes.PrimarySid).Select(u => u.Value).FirstOrDefault(),
                    DepartmentId = claims.Where(w => w.Type == ClaimTypes.PrimaryGroupSid).Select(u => u.Value).FirstOrDefault(),
                    RoleId = claims.Where(w => w.Type == ClaimTypes.Role).Select(u => u.Value).FirstOrDefault(),
                    LoginIPAddress = claims.Where(w => w.Type == ClaimTypes.Dns).Select(u => u.Value).FirstOrDefault(),
                    IsSystem = claims.Where(w => w.Type == ClaimTypes.IsPersistent).Select(u => u.Value).FirstOrDefault().ToBool()
                };
            }
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILogger GetLogger(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return GetService<ILoggerFactory>().CreateLogger(name);
            }
            return GetService<ILogger>();
        }


        public static T GetService<T>()
        {
            return (T)HttpContext.RequestServices.GetService(typeof(T));
        }
    }


    public static class StaticCoreContextExtensions
    {

        public static void AddCoreContextProvider(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<IMemCache, MemCache>();
        }

        /// <summary>
        /// 公用上下文
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCoreContextProvider(this IApplicationBuilder app)
        {
               var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

            var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            CoreContextProvider.Configure(httpContextAccessor, configuration, hostingEnvironment);
            return app;
        }
    }


}
