using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Models;
using SkyMallCore.Services;
using SkyMallCore.Core;
using SkyMallCoreWeb.AppCode;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class CrawlerController : SysBaseController
    {
        public static string WebDomain = "http://www.oilwenku.com/";
        public static IArticleCategoryService _ArticleCategoryService;
        public static IArticleService _ArticleService;


        public CrawlerController(IArticleCategoryService articleCategoryService, IArticleService articleService)
        {
            _ArticleCategoryService = articleCategoryService;
            _ArticleService = articleService;
        }


        [HttpGet]
        //[AllowAnonymous]
        public override ActionResult Index()
        {
            return base.Index();
        }


        
        /// <summary>
        /// 请求的时间间隔
        /// </summary>
        /// <param name="interval">请求停顿多久 毫秒</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult StartCraw(int? interval)
        {
           
            return JsonResult(true);
        }
















        






    }
}