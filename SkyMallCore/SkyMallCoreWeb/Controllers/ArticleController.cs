using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
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


        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IActionResult Index([FromQuery]ArticleSearchView search,int pageIndex = 1)
        {
            if (search == null)
            {
                search = new ArticleSearchView();
            }
            var articles = _ArticleService.GetList(search, pageIndex,1);
            var articlesView = articles.MapTo<PagedList<ArticleDetailView>>();
            return View(articlesView);
        }







    }
    
}
