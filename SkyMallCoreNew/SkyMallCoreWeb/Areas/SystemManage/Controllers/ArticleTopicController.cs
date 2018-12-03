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
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 文章专题
    /// </summary>
    public class ArticleTopicController : SysBaseController
    {
        private IArticleTopicService _ArticleTopicService;
        IMemberService _IMemberService;

        public ArticleTopicController(IArticleTopicService ArticleTopicService, IMemberService memberService)
        {
            _ArticleTopicService = ArticleTopicService;
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
        public ActionResult GetGridJson(ArticleTopicSearchView searchView,int page)
        {
            var data = _ArticleTopicService.GetTopicsList(searchView, page, PageSize);

            //用户信息
            var memberIds = data.Where(w=>w.ParentId != null).Select(u => u.MemberId).Distinct().ToArray();
            Dictionary<string, string> memberDIcs = new Dictionary<string, string>();
            if (memberIds.Any())
            {
                memberDIcs = _IMemberService.GetMemNames(memberIds);
            }

            //取出分类
            var parentIds = data.Where(w => w.ParentId != null).Select(u => u.ParentId).Distinct().ToArray();
            Dictionary<string,string> cateDictory = new Dictionary<string, string>();
            if (parentIds.Any())
            {
                cateDictory = _ArticleTopicService.GetCates(parentIds).ToDictionary(k=>k.Code,v=>v.Text);
            }

            if (data.Any())
            {
                data.ForEach(cate=> {
                    cate.MemberName = "系统管理员";
                    if (memberDIcs.Any())
                    {
                        cate.MemberName = memberDIcs.TryGetValue(cate.MemberId);
                    }
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
            var data = _ArticleTopicService.GetForm(keyValue);
            var member = "管理员";
            if (!data.CreatorUserId.IsEmpty())
            {
                member = _IMemberService.GetMemName(data.CreatorUserId);
            }
            return Content(new { member, data }.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ArticleTopic cate, string keyValue)
        {
            if (!(cate.ParentId.IsEmpty() || cate.ParentId == "-1"))
            {
                return Error("只能添加/修改顶级专题（即专题分类）");
            }
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var data = _ArticleTopicService.GetForm(keyValue);
                if (data == null)
                {
                    return Error("该分组不存在，请刷新重试！");
                }
                data.Title = cate.Title;
                data.EnabledMark = cate.EnabledMark;
                data.Description = cate.Description;
                data.SortCode = cate.SortCode;
                data.ShortTitle = cate.ShortTitle;
                data.ParentId = null;
                data.CoverUrl = cate.CoverUrl;
                data.BigCoverUrl= cate.BigCoverUrl;
                cate = data;
            }
            cate.TopicStatus = (int)TopicStatus.Audited;
            _ArticleTopicService.SubmitForm(cate);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _ArticleTopicService.DeleteForm(keyValue);
            return Success("删除成功。");
        }



        [HttpPost]
        public ActionResult AuditTopic(string keyValue, int topicStatus, string auditMessage)
        {
            var result = _ArticleTopicService.AuditTopic(keyValue, topicStatus, auditMessage);
            return JsonResult(result);
        }
       




    }
}
