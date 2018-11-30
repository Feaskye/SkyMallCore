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
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 文章分类
    /// </summary>
    public class ArticleCategoryController : SysBaseController
    {
        private IArticleCategoryService _ArticleCategoryService;

        public ArticleCategoryController(IArticleCategoryService articleCategoryService)
        {
            _ArticleCategoryService = articleCategoryService;
        }


        [HttpGet]
        public ActionResult GetGridJson(ArticleCateSearchView searchView,int page = 1)
        {
            var data = _ArticleCategoryService.GetCateList(searchView, page, PageSize);
            
            //取出分类
            var parentIds = data.Where(w => w.ParentId != null).Select(u => u.ParentId).Distinct().ToArray();
            Dictionary<string, string> cateDictory = new Dictionary<string, string>();
            if (parentIds.Any())
            {
                cateDictory = _ArticleCategoryService.GetCates(parentIds,true).ToDictionary(k => k.Code, v => v.Text);
            }

            if (data.Any())
            {
                data.ForEach(cate =>
                {
                    if (cateDictory.Any() && !cate.ParentId.IsEmpty())
                    {
                        cate.Category = cateDictory.TryGetValue(cate.ParentId);
                    }
                });
            }

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
            var data = _ArticleCategoryService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ArticleCategory cate, string keyValue)
        {
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _ArticleCategoryService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该分组不存在，请刷新重试！");
                }
                data.Title = cate.Title;
                data.EnabledMark = cate.EnabledMark;
                data.Description = cate.Description;
                data.SortCode = cate.SortCode;
                data.ShortTitle = cate.ShortTitle;
                data.ParentId = cate.ParentId;
                data.CoverUrl = cate.CoverUrl;
                cate = data;
            }
            _ArticleCategoryService.SubmitForm(cate);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            //todo 先删除文件：
            _ArticleCategoryService.DeleteForm(keyValue);
            return Success("删除成功。");
        }


    }
}
