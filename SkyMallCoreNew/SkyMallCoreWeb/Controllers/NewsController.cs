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
using Microsoft.AspNetCore.Mvc.Filters;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// 新闻资讯列表
    /// </summary>
    [AllowAnonymous]
    public class NewsController : FBaseController
    {
        INewsService _NewsService;
        INewsCategoryService _NewsCategoryService;
        public NewsController(INewsService NewsService,
            INewsCategoryService NewsCategoryService)
        {
            _NewsService = NewsService;
            _NewsCategoryService = NewsCategoryService;
            AddPageCrumbs("新闻", "/News");
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IActionResult Index([FromQuery]NewsSearchView search,int pageIndex = 1)
        {
            if (search == null)
            {
                search = new NewsSearchView();
            }
            search.CategoryId = Request.Query["nid"];
            var cateInfo = _NewsCategoryService.GetCate(search.CategoryId);
            string parentId = null;
            if (cateInfo != null)
            {
                parentId = cateInfo.ParentId ?? cateInfo.Code;
            }

            var books = _NewsService.GetNewsList(search, pageIndex, PageSize);
            ViewBag.CateList = _NewsCategoryService.GetCateList(search.CategoryId, null, true);
            ViewBag.HotNews = _NewsService.GetTopNewss(NewsTopEnum.HotNews, PageSize);
            ViewBag.CateInfo = cateInfo;
            if (cateInfo != null)
            {
                AddPageCrumbs(cateInfo.Text);
            }
            return View(books);
        }


        /// <summary>
        /// 新闻详细
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IActionResult Detail(string nid)
        {
            if (nid.IsEmpty())
            {
                return ErrorPage("该新闻资源不存在");
            }
            var NewsPage = new NewsPageView();
            var NewsDetail = _NewsService.GetForm(nid);
            NewsPage.NewsDetail = AutoMapper.Mapper.Map<NewsDetailView>(NewsDetail);
            ViewBag.AboutNews = _NewsService.GetNewsList(new NewsSearchView { CategoryId= NewsDetail.CategoryId },1, PageSize).ToList();
            ViewBag.CateList = _NewsCategoryService.GetCateList(NewsPage.NewsDetail.CategoryId, null, true);
            AddPageCrumbs("详情");
            UserRead(ReadType.News, nid);
            return View(NewsPage);
        }
        










    }
    
}
