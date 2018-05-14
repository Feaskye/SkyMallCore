using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Core
{
    /// <summary>
    /// 常量参数集合
    /// </summary>
    public class ConstParameters
    {
        public static string VerifyCodeKeyName = "netcore_session_verifycode";
        //CoreProviderContext.Configuration.GetSection() 

        public static string SysLoginUserKey = "netcore_loginuserkey_manage";

        public static string MemLoginUserKey = "netcore_loginuserkey_mem";

        public static string SysLoginProvider = CoreProviderContext.Configuration.GetSection("SystemProvider")["LoginProvider"];
    }
}
