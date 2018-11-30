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
