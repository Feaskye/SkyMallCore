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
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// 文章分类
    /// </summary>
    [AllowAnonymous]
    public class TopicsController : FBaseController
    {
        IArticleTopicService _ArticleTopicService;
        IArticleService _ArticleService;
        public TopicsController(IArticleTopicService articleCateService,
            IArticleService articleService)
        {
            _ArticleTopicService = articleCateService;
            _ArticleService = articleService;
            
        }



        /// <summary>
        /// 导航专题页面    
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var topicView = new TopicSearchView();
            topicView.HotTopics = _ArticleTopicService.GetTopicInfoList(new ArticleTopicSearchView { HotTopic = true, TopicStatus = TopicStatus.Audited, IgnoreCate = true }, 1, 4);
            topicView.TopicCates = _ArticleTopicService.GetTopicCateList(null, null, true);
            topicView.TopicCount = topicView.TopicCates.Sum(u => u.Childs.Count);

            var cateIds = topicView.TopicCates.Select(u => u.Code).ToList();
            topicView.TopicCount = 0;
            topicView.TopicList = _ArticleTopicService.GetTopicInfoList(new ArticleTopicSearchView() { IgnoreCate = true , TopicStatus = TopicStatus.Audited}, 1, 8);
            return View(topicView);
        }

        /// <summary>
        /// 导航专题分类点击后专题列表页页面
        /// </summary>
        /// <returns></returns>
        public IActionResult List(string id,int pageIndex=1)
        {
            var topicView = new TopicSearchView();
            topicView.TopicCates = _ArticleTopicService.GetTopicCateList(null, null, true);
            topicView.TopicCount = topicView.TopicCates.Sum(u => u.Childs.Count);
            //精品推荐
            ViewBag.HotTopics = _ArticleTopicService.GetHotTopics(8);

            var cateIds = topicView.TopicCates.Select(u => u.Code).ToList();
            topicView.TopicCount = 0;
            if (!id.IsEmpty())
            {
                ViewBag.TopicCateId = id;
                cateIds = new List<string>() { id };
            }
            //todo topic特殊处理，方法重复，但有公用信息待处理
            //topicView.TopicList = _ArticleTopicService.GetCateList(new ArticleTopicSearchView { CateType = 1, ParentId = cateIds.ToArray()}, pageIndex,PageSize);//??
            topicView.TopicList = _ArticleTopicService.GetTopicInfoList(new ArticleTopicSearchView { TopicStatus = TopicStatus.Audited, ParentId = cateIds.ToArray() },pageIndex, PageSize);//??
            return View(topicView);
        }

        /// <summary>
        /// 专题详细页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Detail(string id,int pageIndex = 1)
        {
            if (id.IsEmpty())
            {
                return ErrorPage("该专题不存在！");
            }
            var topicView = _ArticleTopicService.GetTopicInfoList(new ArticleTopicSearchView { TopicStatus = TopicStatus.Audited, TopicId = id, IgnoreCate = true }, 1, 1).FirstOrDefault();
            if (topicView == null)
            {
                return ErrorPage("该专题不存在！");
            }
            //精品推荐
            ViewBag.HotTopics = _ArticleTopicService.GetHotTopics(8);
            //专题详细
            ViewBag.TopicVIew = topicView;
            ////专题作者 弃用
            //ViewBag.TopicAuthor = _ArticleService.GetMemResourceStatics(topicView.MemberId);
            ////相关专题

            var topicList = _ArticleService.GetTopicArticleList(new ArticleSearchView { SpecialTopicId = id }, pageIndex, PageSize);
            UserRead(ReadType.Topic,id);
            return View(topicList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BuyTopics([FromServices]IMemberScoreService memberScoreService,string id)
        {
            if (CoreContextProvider.CurrentMember == null)
            {
                return Error("用户未登录，请登录！", "/login");
            }
            var topic = _ArticleTopicService.GetForm(id);
            if (topic == null)
            {
                return Error("该专题不存在，购买失败！");
            }

            if (topic.TopicStatus != (int)TopicStatus.Audited)
            {
                return Error("该专题未审核成功，购买失败！");
            }
            if (CoreContextProvider.CurrentMember.UserId == topic.CreatorUserId)
            {
                return Error("无法 购买自己的专题，购买失败！");
            }
            var buyResult = memberScoreService
                    .MarketBuy(CoreContextProvider.CurrentMember.UserId, ScoreType.buytopic,topic.Id, topic.PackageAmount, topic.CreatorUserId);
            return JsonResult(buyResult);
        }




        /// <summary>
        /// 其他子分类   如：文件类型等
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Package([FromServices]IMemberScoreService memberScoreService,[FromServices]IArticleService articleService, string id)
        {
            if (CoreContextProvider.CurrentMember == null)
            {
                return ErrorPage("用户未登录，请登录！");
            }

            var topic = _ArticleTopicService.GetForm(id);
            if (topic == null)
            {
                return ErrorPage("该专题不存在，购买失败！");
            }
            var buyResult = memberScoreService
                  .MarketBuy(CoreContextProvider.CurrentMember.UserId, ScoreType.buytopic, id, topic.PackageAmount, topic.CreatorUserId);
            if (buyResult.Success)
            {
                var files = articleService.GetPackageByTopicId(id);
                //文件
                byte[] fileBytes;
                var contentType = FileHelper.GetContentType("zip");
                var resourceType = "zip";
                fileBytes = FileDownHelper.DownloadZip(files.ToDictionary(k=>k.Code,v=>v.Text));
                return File(fileBytes, contentType, $"{topic.Title}.{resourceType}");
            }
            return ErrorPage("购买后下载文件失败！");
        }












    }
    
}
