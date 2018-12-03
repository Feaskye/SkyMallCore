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
    public class SysMemberController : SysBaseController
    {
        private IMemberService _MemberService;

        public SysMemberController(IMemberService MemberService)
        {
            _MemberService = MemberService;
        }


        [HttpGet]
        public ActionResult GetGridJson(MemberSearchView search, int page = 1)
        {
            var data = _MemberService.GetList(search, page, PageSize);
            return Content(new
            {
                rows = data,
                total = data.PageCount,
                page = data.PageIndex,
                records = data.TotalCount
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
        public ActionResult GetFormJson(string keyValue,string mobile)
        {
            var member = _MemberService.GetForm(keyValue, mobile);
            var data = AutoMapper.Mapper.Map<MemberDetailView>(member);
            data.Password = null;//默认为空不做操作
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(MemberDetailView member, string keyValue)
        {
            Member data;
            member.Id = keyValue;
            if (member.Id.IsEmpty())
            {
                data = AutoMapper.Mapper.Map<Member>(member);
            }
            else
            {
                data = _MemberService.GetMember(keyValue);
                var oldpassword = data.Password;
                data = AutoMapper.Mapper.Map(member, data);
                //修改时 有密码则加密更改
                if (!data.Password.IsEmpty())
                {
                    data.Password = EncodePassword(data.Password);
                }
                else
                {
                    data.Password = oldpassword;
                }
            }

            if (!Enum.IsDefined(typeof(UserLevel), data.UserLevel))
            {
                data.UserLevel = (int)UserLevel.Common;
            }
            var result = _MemberService.SubmitForm(data);
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
            _MemberService.DeleteForm(keyValue);
            return Success("删除成功。");
        }



        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncodePassword(string password)
        {
            return Md5Hash.Md5(DESEncrypt.Encrypt(password.ToLower(),
                        ConstParameters.MemLoginUserKey).ToLower(), 32).ToLower();
        }


    }
}
