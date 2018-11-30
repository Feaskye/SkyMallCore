using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleController : SysBaseController
    {
        private IArticleService _ArticleService;
        private IMemberService _IMemberService;

        public ArticleController(IArticleService ArticleService, IMemberService memberService)
        {
            _ArticleService = ArticleService;
            _IMemberService = memberService;
        }

    
        public override ActionResult Index()
        {
            if (Request.Query.Any(w => w.Key == "MemberId"))
            {
                ViewData["MemberId"] = Request.Query["MemberId"].ToString();
                ViewData["Member"] = _IMemberService.GetMemName(Request.Query["MemberId"].ToString());
            }
            return base.Index();
        }


        [HttpGet]
        public ActionResult GetGridJson(ArticleSearchView searchView,int page = 1)
        {
            searchView.HasMember = true;
            var data = _ArticleService.GetBooks(searchView, page, PageSize);
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
            var data = _ArticleService.GetForm(keyValue);
            var article = AutoMapper.Mapper.Map<ArticleDetailView>(data);
            article.Member = "管理员";
            if (!data.MemberId.IsEmpty())
            {
                article.Member = _IMemberService.GetMemName(data.MemberId);
            }
            if (data.ArticleCategory != null && data.ArticleCategory.Category != null)
            {
                article.ParentCategoryId = data.ArticleCategory.Category.Id;
            }
            return Content(article.ToJson());
        }


        public override ActionResult Form()
        {
            ViewBag.CopyrightList = BusinessHelper.GetItemDictionary("Copyright");
            ViewBag.ResourceTypeList = BusinessHelper.GetItemDictionary("ResourceType");

            return base.Form();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Article article, string keyValue)
        {
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _ArticleService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该文库存在，请刷新重试！");
                }
                data.Title = article.Title;
                data.EnabledMark = article.EnabledMark;
                data.SortCode = article.SortCode;
                data.ShortTitle = article.ShortTitle;
                data.CategoryId = article.CategoryId;
                data.CoverUrl = article.CoverUrl;
                data.Description = article.Description;
                data.Attachment = article.Attachment;
                data.ReadCount = article.ReadCount;
                data.DownloadCount = article.DownloadCount;
                data.ResourceUrl = article.ResourceUrl;
                data.ResourceSize = article.ResourceSize;
                data.RequireAmount = article.RequireAmount;
                data.BookStatus = article.BookStatus;
                data.ScoreRequire = article.ScoreRequire;
                data.ShareType = article.ShareType;
                data.IsOnline = article.IsOnline;
                data.OnlinePageCount = article.OnlinePageCount;
                data.PageCount = article.PageCount;
                data.AllowDownload = article.AllowDownload;
                data.ResourceType = article.ResourceType;
                data.Copyright = article.Copyright;
                

                article = data;
            }
            _ArticleService.SubmitForm(article);
            return Success("操作成功。");
        }

        ///// <summary>
        ///// 审核状态
        ///// </summary>
        ///// <param name="article"></param>
        ///// <param name="keyValue"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AuditForm(Article article, string keyValue)
        //{
        //    if (string.IsNullOrWhiteSpace(keyValue))
        //    {
        //        return Error("该文库编号不存在，请重试");
        //    }
        //    var data = _ArticleService.GetForm(keyValue);
        //    if (data == null)
        //    {
        //        return Error("该文库不存在，请刷新重试！");
        //    }
        //    data.BookStatus = article.BookStatus;
        //    data.AuditMessage = article.AuditMessage;
        //    _ArticleService.AuditArticle(data);
        //    return Success("审核成功。");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            //todo 先删除文件：
            var result = _ArticleService.DeleteForm(keyValue);
            if (!result.Success)
            {
                return Error(result.Message);
            }
            return Success("删除成功。");
        }


        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auditStatus"></param>
        /// <param name="auditMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuditArticle(string id, int auditStatus,string auditMessage)
        {
            var result = _ArticleService.AuditArticle(id, auditStatus, auditMessage);
            return JsonResult(result);
        }



    }
}
