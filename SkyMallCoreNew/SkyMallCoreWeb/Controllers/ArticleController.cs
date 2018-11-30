using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCore.Core;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using SkyCore.GlobalProvider;
using System.IO;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// 文章列表
    /// </summary>
    public class ArticleController : FBaseController
    {
        IArticleService _ArticleService;
        IMemberService _MemberService;
        public ArticleController(IArticleService articleService,IMemberService memberService)
        {
            _ArticleService = articleService;
            _MemberService = memberService;
            AddPageCrumbs("资源分类", "/cate");
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index([FromQuery]ArticleSearchView search,int pageIndex = 1)
        {
            if (search == null)
            {
                search = new ArticleSearchView();
            }
            search.BookStatus = (int)BookStatus.审核通过;

            ListItem cateInfo = new ListItem { Text = "全部" };
            //所有分类
            var allCateList = GetArticleCateList(null, null, true);
            ViewBag.CateList = allCateList;
            //cid参数
            string cid = Request.Query["cid"];
            if (!cid.IsEmpty())
            {
                if (allCateList.Any(w=>w.Code == cid))
                {
                    search.ParentCategoryId = cid;
                }
                else
                {
                    search.CategoryId = cid;
                }
            }

            if (!search.ParentCategoryId.IsEmpty())
            {
                cateInfo = allCateList.FirstOrDefault(w => w.Code == search.ParentCategoryId);
            }
            if (!search.CategoryId.IsEmpty())
            {
                allCateList.ForEach(cate =>
                {
                    if (cate.Childs != null && cate.Childs.Any(w => w.Code == search.CategoryId))
                    {
                        cateInfo = cate.Childs.FirstOrDefault(w => w.Code == search.CategoryId);
                    }
                });
            }
            //Keyword
            search.Keyword = Request.Query["q"];
            //上架时间范围选择
            if (search.OnShelfDays.HasValue)
            {
                switch (search.OnShelfDays.Value)
                {
                    case 3:
                        search.StartDate = DateTime.Now.AddDays(-3);
                        break;
                    case 7:
                        search.StartDate = DateTime.Now.AddDays(-7);
                        break;
                    case 30:
                        search.StartDate = DateTime.Now.AddDays(-30);
                        break;
                    case 365:
                        search.StartDate = DateTime.Now.AddDays(-365);
                        break;
                }
            }
            //key
            ViewBag.SearchKey = search.Keyword;
            //版权
            ViewBag.CopyrightList = BusinessHelper.GetItemDictionary("Copyright");
            //资源类型
            ViewBag.ResourceTypeList = BusinessHelper.GetItemDictionary("ResourceType");

            //CateBooksStaticModel //string categoryId, string parentCategoryId
            
            search.Articles = _ArticleService.GetBooks(search, pageIndex, PageSize);

            ViewBag.HotArticle = _ArticleService.GetTopArticles(ArticleTopEnum.HotArticle, PageSize);
            ViewBag.CateInfo = cateInfo;
            AddPageCrumbs(cateInfo == null?"搜索":cateInfo.Text);
            return View(search);
        }


        /// <summary>
        /// 文库详细
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Detail(string aid)
        {
            if (aid.IsEmpty())
            {
                return ErrorPage("该文库资源不存在");
            }
            var articlePage = new ArticlePageView();
            var articleDetail = _ArticleService.GetForm(aid);
            if (articleDetail == null)
            {
                return ErrorPage("该文库资源不存在");
            }
            articlePage.ArticleDetail = AutoMapper.Mapper.Map<ArticleDetailView>(articleDetail);
            articlePage.ArticleDetail.Member = "admin";
            if (articleDetail.Member != null)
            {
                articlePage.ArticleDetail.Member = (articleDetail.Member.NickName ?? articleDetail.Member.RealName) ?? articleDetail.Member.UserName;
            }
            articlePage.AboutArticles = _ArticleService.GetBooks(new ArticleSearchView { MemberId = articlePage.ArticleDetail.MemberId },1,20);
            articlePage.LikeArticles = _ArticleService.GetBooks(new ArticleSearchView { CategoryId = articlePage.ArticleDetail.CategoryId }, 1, 20);

            var attch = articlePage.ArticleDetail.Attachment;
            if (!attch.IsEmpty())
            {
                var filhe = new FileInfo(FileHelper.MapFilePath(articlePage.ArticleDetail.Attachment));
                articlePage.ArticleDetail.AttachmentImage = articlePage.ArticleDetail.Attachment.Split('.')[0] + "/" + filhe.Name.Split('.')[0];
                if (articlePage.ArticleDetail.IsOnline && articlePage.ArticleDetail.OnlinePageCount == 0)
                {
                    articlePage.ArticleDetail.OnlinePageCount = articlePage.ArticleDetail.PageCount;
                }
            }
            else//图为空则不显示
            {
                articlePage.ArticleDetail.OnlinePageCount = 0;
                articlePage.ArticleDetail.PageCount = 0;
            }
            //其他方式？video，voice，image，txt等
            
            var member = GetMember(articlePage.ArticleDetail.MemberId);
            articlePage.Member = member;
            AddPageCrumbs(articleDetail.Title);

            var viewName = "Detail";

            if (ConfigManager.UploadAllowVideoExtension.Contains(articleDetail.ResourceType) ||
                ConfigManager.UploadAllowFlashExtension.Contains(articleDetail.ResourceType) ||
                ConfigManager.UploadAllowImgExtension.Contains(articleDetail.ResourceType) ||
                ConfigManager.UploadAllowVoiceExtension.Contains(articleDetail.ResourceType))
            {
                viewName = "imgDetail";
            }
            UserRead(ReadType.Article, aid);
            return View(viewName, articlePage);
        }



        [HttpGet]
        [Route("/article/down/{aid}")]
        [Route("/article/down")]
        [AllowAnonymous]
        public IActionResult Down([FromServices]IArticleTopicService articleTopicService, string aid)
        {
            AddPageCrumbs("资源下载");
            var articlePage = new ArticleDetailView();
            var articleDetail = _ArticleService.GetForm(aid);
            if (articleDetail == null)
            {
                return ErrorPage("该文库资源不存在");
            }
            if (!articleDetail.AllowDownload)
            {
                return ErrorPage("该文库资源设置为不允许下载");
            }


            if (!articleDetail.SpecialTopicId.IsEmpty() && articleDetail.CategoryId.IsEmpty())
            {
                //var topicServie = CoreContextProvider.GetService<IArticleTopicService>();
                var topicName = articleTopicService.GetCate(articleDetail.SpecialTopicId)?.Text;
                if (!topicName.IsEmpty())
                {
                    return ErrorPage($"该文库已绑定资源专题，无法单独下载，请到专题【{topicName}】购买并下载！");
                }
            }

            articlePage = AutoMapper.Mapper.Map<ArticleDetailView>(articleDetail);
            articlePage.Member = "admin";
            if (articleDetail.Member != null)
            {
                articlePage.Member = (articleDetail.Member.NickName ?? articleDetail.Member.RealName) ?? articleDetail.Member.UserName;
            }
            return View(articlePage);
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult BuyBooks([FromServices]IArticleTopicService articleTopicService,[FromServices]IMemberScoreService memberScoreService,
            string id)
        {
            if (CoreContextProvider.CurrentMember == null)
            {
                return Error("购买失败，请先登录！");
            }
            var article = _ArticleService.GetForm(id);
            if (article == null)
            {
                return Error("该文库不存在，购买失败！");
            }

            if (article.BookStatus != (int)BookStatus.审核通过)
            {
                return Error("该文库未审核成功，购买失败！");
            }

            //如果选择专题，无分类，则无法下载
            if (!article.SpecialTopicId.IsEmpty()&& article.CategoryId.IsEmpty())
            {
                //var topicServie = CoreContextProvider.GetService<IArticleTopicService>();
                var topicName = articleTopicService.GetCate(article.SpecialTopicId)?.Text;
                if (!topicName.IsEmpty())
                {
                    return Error($"该文库已绑定资源专题，无法单独购买，请到专题【{topicName}】购买！");
                }
            }

            var buyResult = memberScoreService.MarketBuy(CoreContextProvider.CurrentMember.UserId,
                                ScoreType.buybook, article.Id, article.RequireAmount, article.CreatorUserId);
            return JsonResult(buyResult);
        }


        [HttpGet]
        public IActionResult Download(string aid)
        {
            var downloadResult = _ArticleService.ClientDownLoad(aid);
            if (downloadResult.Success && downloadResult.Data!=null && !downloadResult.Data.Attachment.IsEmpty())
            {
                byte[] fileBytes;
                var files = downloadResult.Data.Attachment.Split(',');
                var contentType = FileHelper.GetContentType("zip");
                var resourceType = "zip";

                if (files.Length == 1)
                {
                    resourceType = downloadResult.Data.ResourceType ?? FileHelper.GetExtension(FileHelper.MapFilePath(files[0]));
                    contentType = FileHelper.GetContentType(resourceType);
                    fileBytes = FileDownHelper.DownLoad(files[0]);
                }
                else
                {
                    fileBytes = FileDownHelper.DownloadZip(files.ToDictionary(k=>k,v=>v));
                }
                return File(fileBytes, contentType, $"{downloadResult.Data.Title}.{resourceType}");
            }
            return ErrorPage(downloadResult.Message??"下载失败！");
        }








    }
    
}
