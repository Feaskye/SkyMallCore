using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class SysMemScoreController : SysBaseController
    {
        private IMemberScoreService _MemberScoreService;
        private IMemberService _IMemberService;

        public SysMemScoreController(IMemberScoreService MemberScoreService, IMemberService memberService)
        {
            _MemberScoreService = MemberScoreService;
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
        public ActionResult GetGridJson([FromServices]ISysItemsDetailService sysItemsDetailService, MemScoreSearchView search, int page = 1)
        {
            var data = _MemberScoreService.GetList(search, page, PageSize);

            if (data.ScoreList != null && data.ScoreList.Any())
            {
                var scoreTypes = data.ScoreList.Select(w => w.ScoreType + "").Distinct().ToArray();
                var itemDatas = sysItemsDetailService.GetListItem("ScoreType", scoreTypes).ToDictionary(k => k.Code, v => v.Text);
                data.ScoreList.ForEach(score=> {
                    score.ScoreTypeName = itemDatas.TryGetValue(score.ScoreType + "");//ExtList.TryGetValue();
                });
            }


            return Content(new
            {
                rows = data.ScoreList,
                total = data.ScoreList.PageCount,
                page = data.ScoreList.PageIndex,
                records = data.ScoreList.TotalCount
            }.ToJson());
        }

        [HttpGet]
        public override ActionResult Form()
        {
            if (Request.Query.Any(w=>w.Key == "act"))
            {
                ViewData["act"] = 1;
            }
            return base.Form();
        }

        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _MemberScoreService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(MemberScore member, string keyValue)
        {
            member.Id = keyValue;
            var result = _MemberScoreService.SubmitForm(member);
            if (!result.Success)
            {
                return Error(result.Message);
            }
            return Success("操作成功。");
        }

        



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _MemberScoreService.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
