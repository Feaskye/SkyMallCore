using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SkyCoreLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb
{
    /// <summary>
    /// 系统后台登录授权认证方案
    /// </summary>
    public class SysManageAuthAttribute : AuthorizeAttribute
    {
        public SysManageAuthAttribute()
        {
            base.AuthenticationSchemes = ConstParameters.SysManageAuthScheme;
        }
    }


    /// <summary>
    /// 前台用户登录授权认证方案
    /// </summary>
    public class MemberAuthAttribute : AuthorizeAttribute
    {
        public MemberAuthAttribute()
        {
            base.AuthenticationSchemes = ConstParameters.MemberAuthScheme;
        }
    }



    /// <summary>
    /// 授权认证
    /// </summary>
    public static class AuthenticationFactory
    {
        /// <summary>
        /// 用户登录配置
        /// </summary>
        /// <param name="services"></param>
        public static void UserAuthenConfig(this IServiceCollection services)
        {
            //多种登录授权方式，前台/后台 【参考 https://www.cnblogs.com/sky-net/p/8669892.html】
            services.AddAuthentication(ConstParameters.SysManageAuthScheme)
            .AddCookie(ConstParameters.SysManageAuthScheme, o =>
            {//后台
                o.LoginPath = new PathString("/SystemManage/Login");
                o.AccessDeniedPath = new PathString("/SystemManage/Login/Forbidden");
                o.LogoutPath = new PathString("/SystemManage/Login/OutLogin");
            }).AddCookie(ConstParameters.MemberAuthScheme, o => //前台会员
            {
                o.LoginPath = new PathString("/Login");
                o.AccessDeniedPath = new PathString("/Member/Forbidden");
                o.LogoutPath = new PathString("/Member/OutLogin");
                o.ReturnUrlParameter = "ReturnUrl";
            });

        }
    }

}
