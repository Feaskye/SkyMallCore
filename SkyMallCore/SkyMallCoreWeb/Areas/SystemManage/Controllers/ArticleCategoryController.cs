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
        public ActionResult GetGridJson(string keyword)
        {
            var data = _ArticleCategoryService.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _ArticleCategoryService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ArticleCategory articleCategory, string permissionIds, string keyValue)
        {
            _ArticleCategoryService.SubmitForm(articleCategory, permissionIds.Split(','), keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _ArticleCategoryService.DeleteForm(keyValue);
            return Success("删除成功。");
        }


    }
}
