using Microsoft.Extensions.Logging;
using SkyMallCore.Core;
using SkyMallCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using SkyCore.GlobalProvider;
using System.Threading.Tasks;
using SkyMallCore.ViewModel;

namespace SkyMallCoreWeb
{

    /// <summary>
    /// 业务相关辅助
    /// </summary>
    public class BusinessHelper
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public static void LoadSysConfiguration()
        {

            try
            {
                var configService = CoreContextProvider.GetService<ISysConfigurationService>();
                var configs = configService.GetList().Select(u => new { u.ConfigCode, u.ConfigValue }).ToDictionary(k => k.ConfigCode, v => v.ConfigValue);
                var _sysConfig = new SysConfigurationModel()
                {
                    SiteName = configs.TryGetValue("SiteName"),
                    UploadFolder = configs.TryGetValue("UploadFolderCode"),
                    SiteKeywords = configs.TryGetValue("SiteKeywords"),
                    SiteDescription = configs.TryGetValue("SiteDescription"),
                    SiteLogo = configs.TryGetValue("SiteLogo"),
                    SiteQQ = configs.TryGetValue("SiteQQ"),
                    SysMailPassword = configs.TryGetValue("SysMailPassword"),
                    SysMailUser = configs.TryGetValue("SysMailUser"),
                    //SiteQQ = configs.TryGetValue("SiteQQ"),
                    //VoiceRootFolder = configlist.Where(w => w.ConfigCode == ConstParameters.VoiceRootFolderCode).Select(u => u.ConfigValue).FirstOrDefault() //,
                    //ExecuteHour = Convert.ToInt32(CoreContextProvider.Configuration.GetSection("SysConfiguration")["JobExecuteHour"]),
                    //ExecuteMunite = Convert.ToInt32(CoreContextProvider.Configuration.GetSection("SysConfiguration")["JobExecuteMunite"])
                };
                ConfigManager.SysConfiguration = _sysConfig;
            }
            catch (Exception ex)
            {
                ILogger logger = CoreContextProvider.GetLogger("LoadSysConfiguration");
                logger.LogError(ex, ex.Message);
            }
        }


        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <returns></returns>
        public static string GetArticleFileType(string type)
        {
            return "doc";
        }


        /// <summary>
        /// 获取系统字典
        /// </summary>
        /// <param name="enCode"></param>
        /// <returns></returns>
        public static List<ListItem> GetItemDictionary(string enCode)
        {
            var sysItemsDetailService = CoreContextProvider.GetService<ISysItemsDetailService>();
            return sysItemsDetailService.GetListItem(enCode);
        }


        public static string GetErrorImage(string type)
        {
            var errorImage = "d_pdf.png";
            if (ConfigManager.UploadAllowVoiceExtension.Contains(type))
            {
                errorImage = "d_mp3.jpg";
            }
            else if (ConfigManager.UploadAllowVideoExtension.Contains(type))
            {
                errorImage = "d_vedio.png";
            }
            return errorImage;
        }

        //public static string GetAllowExtens(UpLoadAction? actionType)
        //{
        //    var upload
        //    switch (actionType)
        //    {
        //        case UpLoadAction.attichfile:
        //            return 
        //            break;
        //            case UpLoadAction.cover,
                    
        //    }
        //}











    }
}
