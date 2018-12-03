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
    /// 文章分类
    /// </summary>
    public class NewsCategoryController : SysBaseController
    {
        private INewsCategoryService _NewsCategoryService;

        public NewsCategoryController(INewsCategoryService NewsCategoryService)
        {
            _NewsCategoryService = NewsCategoryService;
        }


        [HttpGet]
        public ActionResult GetGridJson(NewsCateSearchView searchView,int page =1)
        {
            var data = _NewsCategoryService.GetCateList(searchView, page, PageSize);
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
            var data = _NewsCategoryService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(NewsCategory cate, string keyValue)
        {
             if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _NewsCategoryService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该分组不存在，请刷新重试！");
                }
                data.Title = cate.Title;
                data.EnabledMark = cate.EnabledMark;
                data.Description = cate.Description;
                data.SortCode = cate.SortCode;
                data.ShortTitle= cate.ShortTitle;
                data.ParentId= cate.ParentId;
                cate = data;
            }
            var result = _NewsCategoryService.SubmitForm(cate);
            return Success(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _NewsCategoryService.DeleteForm(keyValue);
            return Success("删除成功。");
        }


    }
}
