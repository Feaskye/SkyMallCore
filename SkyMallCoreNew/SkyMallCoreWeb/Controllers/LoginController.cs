using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;
using SkyMallCore.ViewModel;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// 后台登录页
    /// </summary>
    [AllowAnonymous]
    public class LoginController : FBaseController
    {
        IMemberService _MemberService;
        public LoginController(IMemberService memberService)
        {
            _MemberService = memberService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            AddPageCrumbs("登录");
            if (CoreContextProvider.CurrentMember != null)
            {
                return RedirectToAction("Index","Member");
            }
            return View();
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

        [HttpGet]
        public async Task<IActionResult> OutLogin()
        {
      
            this.Request.HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(ConstParameters.MemberAuthScheme);
            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// 检测登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CheckLogin(LoginView loginView)
        {
            try
            {
                if (CoreContextProvider.HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).IsEmpty()
                    || Md5Hash.Md5(loginView.Code.ToLower(), 16) != 
                    CoreContextProvider.HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).ToString())
                {
                    return JsonResult("验证码错误，请重新输入");
                }

                CoreContextProvider.HttpContext.RemoveSession(ConstParameters.VerifyCodeKeyName);
                var result = _MemberService.CheckLogin(loginView.UserName, loginView.Password);
                if (!result.Success)
                {
                    return Error(result.Message);
                }
                var userEntity = result.Data;

                //登录已重写
                await WriteUserIdentity(userEntity,loginView.RemDay);

                var returnUrl = "/Member";
                if (!string.IsNullOrWhiteSpace(loginView.ReturnUrl) && !returnUrl.Equals(loginView.ReturnUrl))
                {
                    returnUrl = loginView.ReturnUrl;
                    if (loginView.ReturnUrl.ToLower().Contains("download"))
                    {
                        returnUrl = loginView.ReturnUrl.ToLower().Replace("download", "down");
                    }
                }
                return JsonResult(data: returnUrl);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, ex.Message);
                return JsonResult(ex.Message);
            }

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [Route("/Reg")]
        public IActionResult Register([FromServices]ISysItemsDetailService sysItemsDetailService)
        {
            AddPageCrumbs("注册");

            var score = 1;
            var sysitem = sysItemsDetailService.GetItem("ScoreType", ((int)ScoreType.reg).ToString());
            if (sysitem == null)
            {
                _Logger.LogError("注册积分为空");
            }
            score = sysitem.Description.ToInt();
            ViewBag.Score = score;


            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (viewModel.Code.IsEmpty())
            {
                return Error("验证码不能为空 请重试");
            }
            if (CoreContextProvider.HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).IsEmpty()
                 || Md5Hash.Md5(viewModel.Code.ToLower(), 16) !=
                 CoreContextProvider.HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).ToString())
            {
                return JsonResult("验证码错误，请重新输入");
            }

            CoreContextProvider.HttpContext.RemoveSession(ConstParameters.VerifyCodeKeyName);
            var result = _MemberService.Register(viewModel);
            return JsonResult(result);
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/forgetpass")]
        public IActionResult ForgetPass()
        {
            AddPageCrumbs("找回密码");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgetPass(string userName,string email)
        {
            if (email.IsEmpty())
            {
                return JsonResult("邮箱不能为空！");
            }

            var existUser = _MemberService.ExistUserName(userName);
            if (!existUser.Success)
            {
                return JsonResult(existUser);
            }
            var userId = _MemberService.GetUserIdByName(userName);
            var code = Common.GetRndNum(5);
            var sendReslt = SendSysEmail("找回密码验证码-" + ConfigManager.SysConfiguration.SiteName, email, $"你好{userName}，您的找回密码验证码为：{code}，请输入此验证码进行修改密码！" );
            if (!sendReslt.Success)
            {
                return JsonResult(sendReslt.Message??"发送失败！");
            }
            HttpContext.WriteSession(ConstParameters.MemForgetKey, code);
            HttpContext.WriteSession(ConstParameters.MemForgetUserKey, userId);
            return Success(data:"/changeconfirm");
        }


        /// <summary>
        /// 确认修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/changeconfirm")]
        public IActionResult ChangeConfirm()
        {
            if (HttpContext.GetSession(ConstParameters.MemForgetKey).IsEmpty())
            {
                return Redirect("/forgetpass");
            }
            AddPageCrumbs("确认修改密码");
            return View();
        }

        /// <summary>
        /// todo 502 - Web 服务器在作为网关或代理服务器时收到了无效响应。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeConfirm(string code,string newpassword)
        {
            if (code.IsEmpty())
            {
                return JsonResult("验证码不能为空！");
            }
            if (newpassword.IsEmpty())
            {
                return JsonResult("新密码不能为空！");
            }

            if (code != HttpContext.GetSession(ConstParameters.MemForgetKey))
            {
                return JsonResult("验证码有误或已过期，请返回重试！");
            }
            var userId = HttpContext.GetSession(ConstParameters.MemForgetUserKey);
            if (userId.IsEmpty())
            {
                return JsonResult("验证信息有误或已过期，请返回重试！");
            }

            var result = _MemberService.ChangePwd(userId, newpassword);
            if (!result.Success)
            {
                return Error(result.Message??"修改密码失败！");
            }
            HttpContext.RemoveSession(ConstParameters.MemForgetKey);
            HttpContext.RemoveSession(ConstParameters.MemForgetUserKey);
            return Success(data:"/login");
        }

    }
}
