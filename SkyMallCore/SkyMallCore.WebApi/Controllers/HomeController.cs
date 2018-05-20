using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using SkyMallCore.WebApi.Models;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        /// 获取首页内容  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Index()
        {
            return new { data=100};
        }
        
    }
}
