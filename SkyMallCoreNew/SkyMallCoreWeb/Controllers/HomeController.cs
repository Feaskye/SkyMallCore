using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
namespace SkyMallCoreWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : FBaseController
    {
        ISysUserService _SysUserService;
        IArticleService _ArticleService;
        IMemberScoreService _IMemberScoreService;
        IMemberService _IMemberService;
        public HomeController(ISysUserService sysUserService, 
             IArticleService articleService, 
             IMemberScoreService memberScoreService,
             IMemberService memberService)
        {
            _SysUserService = sysUserService;
            _ArticleService = articleService;
            _IMemberScoreService = memberScoreService;
            _IMemberService = memberService;
        }

        //首页
        public IActionResult Index([FromServices]IArticleTopicService articleTopicService,
            [FromServices]IHelpService helpService,[FromServices]INewsService newsService
            ,[FromServices]ILinkService linkService, [FromServices]IArticleCategoryService articleCategoryService)
        {
            ViewBag.TodayArticles = _ArticleService.GetTopArticles(ArticleTopEnum.HotArticle, 10);
            ViewBag.NewArticles = _ArticleService.GetTopArticles(ArticleTopEnum.NewArticle, 10);

            ViewBag.HotTopics = articleTopicService.GetHotTopics(8);

            ViewBag.BestCates = articleCategoryService.GetCateList(new ArticleCateSearchView { IsRemmand = true },1,6);

            var tradeScores = _IMemberScoreService.GetList(true, 5);
            ViewBag.TradeScores = tradeScores;
            //PPT
            ViewBag.HotPPT = _ArticleService.GetTopArticles(ArticleTopEnum.HotPPT,18, null);
            ViewBag.NewPPT = _ArticleService.GetTopArticles(ArticleTopEnum.NewPPT, 18, null);
            
            //轮播图
            ViewBag.HomeCarousels = helpService.GetHelps(new HelpSearchView() { HelpCode = HelpCode.HomeCarousel }, 1, 10).ToList();
            //公告
            ViewBag.Announcements = newsService
                                                               .GetTopNewss(NewsTopEnum.Announcement, 5).ToList();
            //资源总数量
            var totalData = _ArticleService.GetTotalBooks();
            ViewBag.TotalBooks = totalData;

           
            //友情链接
            ViewBag.FriendLinks = linkService.GetList();


            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            AddPageCrumbs("关于我们");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("/contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
