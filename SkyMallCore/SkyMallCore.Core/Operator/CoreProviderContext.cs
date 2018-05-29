
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace SkyMallCore.Core
{
    /// <summary>
    /// Core 全局支持上下文
    /// 配置、Service、HttpContext、用户
    /// （可以改为注入的方式，这里为了避免传参麻烦）
    /// </summary>
    public class CoreProviderContext
    {
        //全局，获取运行时相关信息
        public static IHttpContextAccessor HttpContextAccessor { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static IServiceCollection ServiceCollection { get; set; }

        public static IHostingEnvironment HostingEnvironment { get; set; }

        public static CoreProviderContext Provider
        {
            get { return new CoreProviderContext(); }
        }
        private string LoginUserKey = ConstParameters.SysLoginUserKey;
        private string LoginProvider = ConstParameters.SysLoginProvider;

        
        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public OperatorModel CurrentSysUser
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


        public static HttpContext HttpContext
        {
            get
            {
                object factory = GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                Microsoft.AspNetCore.Http.HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

        public static object GetService(Type type)
        {
            return HttpContextAccessor.HttpContext.RequestServices.GetService(type);
        }






    }
}
