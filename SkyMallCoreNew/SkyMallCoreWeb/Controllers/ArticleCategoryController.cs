using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using SkyMallCore.Core;
using SkyMallCore.ViewModel.Business;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// 文章分类
    /// </summary>
   [AllowAnonymous]
    public class ArticleCategoryController : FBaseController
    {
        IArticleService _ArticleService;
        public ArticleCategoryController(
            IArticleService articleService)
        {
            _ArticleService = articleService;
            
        }

        [Route("/cate")]
        [Route("/cate/Index")]
        public IActionResult Index([FromServices]IArticleTopicService topicService,[FromServices]IArticleCategoryService articleCategoryService)
        {
            AddPageCrumbs("资源分类");
            var cateList = articleCategoryService.GetCateList(null, null, true);

            //var topicService = SkyCore.GlobalProvider.CoreContextProvider.GetService<IArticleTopicService>();
            ViewBag.TopicCates = topicService.GetTopicCateList(null, null);
            ViewBag.HotTopics = topicService.GetTopicInfoList(new ArticleTopicSearchView { HotTopic = true,TopicStatus = TopicStatus.Audited,IgnoreCate = true }, 1, 3);
            return View(cateList);
        }
        

        /// <summary>
        /// 其他子分类   如：文件类型等
        /// </summary>
        /// <returns></returns>
        public IActionResult Brand()
        {
            return View();
        }












    }
    
}
