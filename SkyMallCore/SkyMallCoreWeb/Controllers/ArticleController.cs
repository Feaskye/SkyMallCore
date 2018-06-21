using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCoreWeb.Models;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// 文章列表
    /// </summary>
    public class ArticleController : FBaseController
    {
        IArticleService _ArticleService;
        IArticleCategoryService _ArticleCategoryService;
        public ArticleController(IArticleService articleService,
            IArticleCategoryService articleCategoryService)
        {
            _ArticleService = articleService;
            _ArticleCategoryService = articleCategoryService;
        }


        //首页
        public IActionResult Index()
        {
            var users = _ArticleService.GetList();
            return Content("博客中心");
        }







    }
    
}
