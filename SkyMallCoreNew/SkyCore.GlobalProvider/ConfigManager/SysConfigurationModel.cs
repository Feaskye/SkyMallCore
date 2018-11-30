using System;
using System.Collections.Generic;
using System.Text;

namespace SkyCore.GlobalProvider
{
    //http://www.cnblogs.com/hahaxi/p/6931690.html
    /// <summary>
    /// 站点配置
    /// </summary>
    public class SysConfigurationModel
    {


        public string SiteDomain { get; set; }

        public string SiteName { get; set; }

        public string SiteKeywords { get; set; }


        public string SiteDescription { get; set; }

        public string UploadFolder { get; set; }

        public string SiteQQ { get; set; }


        public string SiteLogo { get; set; }


        public int ExecuteHour { get; set; } = 2;

        public int ExecuteMunite { get; set; } = 30;




        public string SysMailUser { get; set; }

        public string SysMailPassword { get; set; }
        
    }
}
