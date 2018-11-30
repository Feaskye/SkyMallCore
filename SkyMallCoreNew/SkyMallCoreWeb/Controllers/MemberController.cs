using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.DrawingCore.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyCore.GlobalProvider;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Member;

namespace SkyMallCoreWeb.Controllers
{
    [MemberAuth]
    public class MemberController : MemberBaseController
    {
        IMemberService _MemberService;
        IArticleService _ArticleService;

        public MemberController(IMemberService memberService, IArticleService articleService)
        {
            _MemberService = memberService;
            _ArticleService = articleService;
        }

        //首页
        public IActionResult Index()
        {
            AddPageCrumbs("个人中心");

            //统计信息
            ViewBag.CateBookStatics = _ArticleService.GetBookStatics(CoreContextProvider.CurrentMember.UserId);
            ViewBag.AuditStatics = _ArticleService.GetAuditStatics(CoreContextProvider.CurrentMember.UserId);
            return View(GetMember(CoreContextProvider.CurrentMember.UserId));
        }

        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExistName(string name)
        {
            var result = _MemberService.ExistUserName(name);
            return JsonResult(result);
        }


        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Modify()
        {
            AddPageCrumbs("个人资料");
            var data = _MemberService.GetMember(CoreContextProvider.CurrentMember.UserId);
            var member = AutoMapper.Mapper.Map<MemberDetailView>(data);
            return View(member);
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Modify(MemberDetailView mem)
        {
            var member = _MemberService.GetMember(CoreContextProvider.CurrentMember.UserId);
            member.NickName = mem.NickName;
            member.MobilePhone = mem.MobilePhone;
            member.Description = mem.Description;
            member.Gender = mem.Gender;
            member.Province = mem.Province;
            member.Company = mem.Company;
            member.City= mem.City;
            member.Position = mem.Position;
            member.QQ = mem.QQ;
            var result = _MemberService.SubmitForm(member);
            if (result.Success)
            {
                await WriteUserIdentity(member);
            }
            return JsonResult(result);
        }

        
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangeEmail()
        {
            AddPageCrumbs("修改邮箱");
            return View();
        }
        [HttpPost]
        public IActionResult ChangeEmail(ChangeEmailView emailView)
        {
            if (CoreContextProvider.HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).IsEmpty()
                || Md5Hash.Md5(emailView.VerifyCode.ToLower(), 16) !=
                CoreContextProvider.HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).ToString())
            {
                return Error("验证码错误，请重新输入");
            }

            if (!CoreContextProvider.CurrentMember.UserPwd.Equals(emailView.Password))
            {
                return Error("密码错误，请重试！");
            }
            //todo 发送邮件激活邮箱
            var result = _MemberService.ChangeEmail(CoreContextProvider.CurrentMember.UserId,emailView.Email);
            return JsonResult(result);
        }



        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePwd()
        {
            AddPageCrumbs("修改密码");
            return View();
        }
        [HttpPost]
        public IActionResult ChangePwd(string oldpassword,string password)
        {
            if (!CoreContextProvider.CurrentMember.UserPwd.Equals(oldpassword))
            {
                return Error("原始密码错误，请重试！");
            }
            var result = _MemberService.ChangePwd(CoreContextProvider.CurrentMember.UserId, password);
            return JsonResult(result);
        }


        /// <summary>
        /// 修改头像
        /// </summary>
        /// <returns></returns>
        public IActionResult ZoomImage()
        {
            AddPageCrumbs("上传头像");
             var data = _MemberService.GetMember(CoreContextProvider.CurrentMember.UserId);
             var member =new ZoomImageView {
                 HeadIcon = data.HeadIcon
             };
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> ZoomImage(string imageData = null)
        {
            var data = _MemberService.GetMember(CoreContextProvider.CurrentMember.UserId);
            var member = new ZoomImageView
            {
                HeadIcon = data.HeadIcon
            };
            var uploader = InvokeUploadFiles(UpLoadAction.cover, "headicon");

            if (!uploader.Success || uploader.Data.Count < 1)
            {
                member.ErrorMessage = uploader.Message;
                return View(member);
            }
            var imgFile = uploader.Data.FirstOrDefault().FileName;
            //统一为：/UploadFIles/headicon/{UserName}.jpg 需要时加载
            var b= FileHelper.Base64StringToImage(imageData,FileHelper.MapFilePath(imgFile), ImageFormat.Jpeg);
            if (!b)
            {
                member.ErrorMessage = "保存失败，请重试！";
                return View(member);
            }
            
            //imgFile = imageData ?? imgFile;
            var result = _MemberService.ChangeImage(CoreContextProvider.CurrentMember.UserId, imgFile);
            if (!result.Success)
            {
                member.ErrorMessage = result.Message;
                return View(member);
            }
            member.HeadIcon = imgFile;
           await WriteUserIdentity(data);
            return View(member);
        }


        /// <summary>
        /// 我的积分
        /// </summary>
        /// <returns></returns>
        public IActionResult UserScore([FromServices]IMemberScoreService memberScoreService, MemScoreSearchView searchView, int p = 1)
        {
            AddPageCrumbs("我的积分");
            searchView.MemberId = CoreContextProvider.CurrentMember.UserId;
            return View(memberScoreService.GetList(searchView, p, PageSize));
        }


        











    }
}
