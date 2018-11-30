using System;
using System.Collections.Generic;
using System.Text;

namespace SkyCore.GlobalProvider
{
    /// <summary>
    /// 配置操作
    /// </summary>
    public class ConfigManager
    {
        public static string SysLoginProvider = CoreContextProvider.Configuration.GetSection("SystemProvider")["LoginProvider"];

        /// <summary>
        /// 站点配置
        /// </summary>
        public static SysConfigurationModel SysConfiguration { get; set; }


        /// <summary>
        /// 文件最大不能超过200M
        /// </summary>
        public static long MaxFileLength = 200 * 1024 * 1024;


        /// <summary>
        /// 文件转换图片路径
        /// </summary>
        public static string ConvertFile46AppPath
        {
            get {
                return CoreContextProvider.Configuration.GetSection("SysConfiguration")["ConvertFile46AppPath"];
            }
        }



        /// <summary>
        /// 文件上传文件夹
        /// </summary>
        public static string UploadFolder
        {
            get
            {
                return SysConfiguration.UploadFolder;
            }
        }


        /// <summary>
        /// 文件上传允许类型
        /// </summary>
        public static string[] UploadAllowImgExtension
        {
            get
            {
                return (CoreContextProvider.Configuration.GetSection("SysConfiguration")["UploadAllowImgExtension"]).Split(',');
            }
        }


        /// <summary>
        /// 文件上传允许类型
        /// </summary>
        public static string[] UploadAllowPackExtension
        {
            get
            {
                return (CoreContextProvider.Configuration.GetSection("SysConfiguration")["UploadAllowPackExtension"]).Split(',');
            }
        }


        /// <summary>
        /// Office
        /// </summary>
        public static string[] UploadAllowOfficeExtension
        {
            get
            {
                return (CoreContextProvider.Configuration.GetSection("SysConfiguration")["UploadAllowOfficeExtension"]).Split(',');
            }
        }


        /// <summary>
        /// video
        /// </summary>
        public static string[] UploadAllowVideoExtension
        {
            get
            {
                return (CoreContextProvider.Configuration.GetSection("SysConfiguration")["UploadAllowVideoExtension"]).Split(',');
            }
        }


        /// <summary>
        /// video
        /// </summary>
        public static string[] UploadAllowFlashExtension
        {
            get
            {
                return (CoreContextProvider.Configuration.GetSection("SysConfiguration")["UploadAllowFlashExtension"]).Split(',');
            }
        }


        /// <summary>
        /// vioce
        /// </summary>
        public static string[] UploadAllowVoiceExtension
        {
            get
            {
                return (CoreContextProvider.Configuration.GetSection("SysConfiguration")["UploadAllowVoiceExtension"]).Split(',');
            }
        }

        public static string MailServer
        {
            get
            {
                return CoreContextProvider.Configuration.GetSection("SysConfiguration")["MailServer"];
            }
        }



        public static int MailServerPort
        {
            get
            {
                return Convert.ToInt32(CoreContextProvider.Configuration.GetSection("SysConfiguration")["MailServerPort"]);
            }
        }
    }
}
