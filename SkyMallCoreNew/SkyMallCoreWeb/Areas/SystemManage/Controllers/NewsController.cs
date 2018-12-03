using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    public class NewsController : SysBaseController
    {
        private INewsService _NewsService;

        public NewsController(INewsService NewsService)
        {
            _NewsService = NewsService;
        }


        [HttpGet]
        public ActionResult GetGridJson(NewsSearchView searchView,int page = 1)
        {
            var data = _NewsService.GetNewsList(searchView, page, PageSize);
            data.ForEach(da => {
                da.Description = RegexRegular.CheckMathLength(RegexRegular.NoHTML(da.Description), 20);
            });
            return Content(new
            {
                rows = data,
                total = data.PageCount,
                page = data.PageIndex,
                records = data.TotalCount
            }.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _NewsService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(News news, string keyValue)
        {
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _NewsService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该分组不存在，请刷新重试！");
                }
                data.Title = news.Title;
                data.EnabledMark = news.EnabledMark;
                data.Description = news.Description;
                data.SortCode = news.SortCode;
                data.ShortTitle= news.ShortTitle;
                data.CategoryId= news.CategoryId;
                news = data;
            }
            var result = _NewsService.SubmitForm(news);
            return JsonResult(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            //todo 先删除文件：
            _NewsService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
