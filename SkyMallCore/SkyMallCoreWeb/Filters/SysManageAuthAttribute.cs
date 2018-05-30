﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
        public const string SysManageAuthScheme = "SysManageAuthScheme";
        public SysManageAuthAttribute()
        {
            base.AuthenticationSchemes = SysManageAuthScheme;
        }
    }


    /// <summary>
    /// 前台用户登录授权认证方案
    /// </summary>
    public class MemberAuthAttribute : AuthorizeAttribute
    {
        public const string MemberAuthScheme = "MemberAuthScheme";
        public MemberAuthAttribute()
        {
            base.AuthenticationSchemes = MemberAuthScheme;
        }
    }



    /// <summary>
    /// 授权认证
    /// </summary>
    public class AuthenticationFactory
    {
        public static void Initialize(IServiceCollection services)
        {
            //多种登录授权方式，前台/后台 【参考 https://www.cnblogs.com/sky-net/p/8669892.html】
            services.AddAuthentication(SysManageAuthAttribute.SysManageAuthScheme)
            .AddCookie(SysManageAuthAttribute.SysManageAuthScheme, o =>
            {
                o.LoginPath = new PathString("/SystemManage/Login");
                o.AccessDeniedPath = new PathString("/SystemManage/Login/Forbidden");
            }).AddCookie(MemberAuthAttribute.MemberAuthScheme, o =>
            {
                o.LoginPath = new PathString("/Member/Signin");
                o.AccessDeniedPath = new PathString("/Member/Forbidden");
            });

        }
    }

}
