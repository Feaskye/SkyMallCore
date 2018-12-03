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
    public class SysHelpController : SysBaseController
    {
        private IHelpService _HelpService;

        public SysHelpController(IHelpService HelpService)
        {
            _HelpService = HelpService;
        }


        [HttpGet]
        public ActionResult GetGridJson(HelpSearchView searchView,int page=1)
        {
            var data = _HelpService.GetHelps(searchView, page, PageSize);
            data.ForEach(da=> {
                da.Description = RegexRegular.CheckMathLength(RegexRegular.NoHTML(da.Description),20);
                //da.HelpCode = EnumCommon.GetDescription((HelpCode)da.HelpCode);
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
            var data = _HelpService.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetTreeSelectJson(HelpSearchView searchView)
        {
            var data = _HelpService.GetList(searchView)
                .Select(u=>new TreeSelectModel{ id= u.Id, text= u.Title}).ToList();
            //默认显示未分组
            data.Insert(0,new TreeSelectModel() { id ="-1",text="未分组" });
            return Content(data.ToJson());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Help help, string keyValue)
        {
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _HelpService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该分组不存在，请刷新重试！");
                }
                data.Title = help.Title;
                data.EnabledMark = help.EnabledMark;
                data.Description = help.Description;
                data.SortCode = help.SortCode;
                data.HelpCode = help.HelpCode;
                data.CoverUrl = help.CoverUrl;
                data.Attachment = help.Attachment;
                data.ReadCount = help.ReadCount;
                data.ResourceUrl= help.ResourceUrl;
                help = data;
            }
            _HelpService.SubmitForm(help);
            //_HelpService.SubmitForm(Help, permissionIds.Split(','), keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var result = _HelpService.DeleteForm(keyValue);
            if (!result.Success)
            {
                return Error("删除失败");
            }
            return Success("删除成功。");
        }
    }
}
