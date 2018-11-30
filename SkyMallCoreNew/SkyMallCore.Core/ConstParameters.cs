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
        public static string VerifyCodeKeyName = "netcore_session_verifycode" + DateTime.Now.ToString("yyyyMMdd");
        //CoreContextProvider.Configuration.GetSection() 

        public const string SysLoginUserKey = "netcore_loginuserkey_manage";

        public const string MemLoginUserKey = "netcore_loginuserkey_mem";

        public const string SysManageAuthScheme = "NetCoreSysManageAuthScheme";

        public const string MemberAuthScheme = "NetCoreMemberAuthScheme";


        public const string VoiceRootFolderCode = "VoiceRootFolder";

        /// <summary>
        /// 忘记密码SessionKey
        /// </summary>
        public const string MemForgetKey = "netcore_MemFortgetKey";
        /// <summary>
        /// 用户名
        /// </summary>
        public const string MemForgetUserKey = "netcore_MemUserFortgetKey";
    }
}
