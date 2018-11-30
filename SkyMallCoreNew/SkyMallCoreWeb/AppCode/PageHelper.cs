using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb
{
    public class PageHelper
    {

        /// <summary>
        /// 获取分页连接
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="curr"></param>
        /// <returns></returns>
        public static string GetPagerUrl(ViewContext viewContext, int curr)
        {
            var url = viewContext.HttpContext.Request.Path.Value;
            var queryCellection = viewContext.HttpContext.Request.Query.
                Select(u => new { u.Key, Value = u.Value.FirstOrDefault() }).ToDictionary(u => u.Key, v => v.Value);
            if (!queryCellection.ContainsKey("pageIndex"))
            {
                queryCellection.Add("pageIndex","");
            }
            queryCellection["pageIndex"] = curr.ToString();

            return $"{url}?{string.Join("&", queryCellection.Select(u => u.Key + "=" + u.Value))}";
        }


    }
}
