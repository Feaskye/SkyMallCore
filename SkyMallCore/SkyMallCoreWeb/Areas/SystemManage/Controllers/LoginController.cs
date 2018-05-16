using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 后台登录页
    /// </summary>
    public class LoginController : BaseSysController
    {
        ISysUserService _SysUserService;
        ISysLogService _ISysLogService;
        public LoginController(ISysUserService sysUserService
            , ISysLogService sysLogService)
        {
            _SysUserService = sysUserService;
            _ISysLogService = sysLogService;
        }

        //todo area路由访问问题待解决
        //[Area("SystemManage")]
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult Index()
        {
            var test = string.Format("{0:E2}", 1);
            return View();
        }
        //[Area("SystemManage")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public async Task<IActionResult> OutLogin()
        {
            _ISysLogService.WriteSysLog(new SysLog
            {
                ModuleName = "系统登录",
                Type = DbLogType.Exit.ToString(),
                Account = CoreProviderContext.Provider.CurrentSysUser.Account,
                NickName = CoreProviderContext.Provider.CurrentSysUser.RealName,
                Result = true,
                Description = "安全退出系统",
            });

            this.Request.HttpContext.Session.Clear();
            //CoreProviderContext.Provider.RemoveCurrent();
            await HttpContext.SignOutAsync(SysManageAuthAttribute.SysManageAuthScheme);
            return RedirectToAction("Index", "Login", new { area = "SystemManage" });
        }

        /// <summary>
        /// 检测登录
        /// 登录方式需要改造 
        /// https://www.codeproject.com/Articles/1205161/ASP-NET-Core-Cookie-Authentication
        /// https://www.cnblogs.com/sky-net/p/8669892.html
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CheckLogin(string username, string password, string code)
        {
            SysLog logEntity = new SysLog();
            logEntity.ModuleName = "系统登录";
            logEntity.Type = DbLogType.Login.ToString();
            try
            {
                if (WebHelper.GetSession(ConstParameters.VerifyCodeKeyName).IsEmpty() 
                    || Md5Hash.Md5(code.ToLower(), 16) != WebHelper.GetSession(ConstParameters.VerifyCodeKeyName).ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }

                var userEntity = _SysUserService.CheckLogin(username, password);
                if (userEntity != null)
                {
                 
                    //登录已重写
                    var identity = new ClaimsIdentity(SysManageAuthAttribute.SysManageAuthScheme);  // 指定身份认证类型
                    List<Claim> claims = new List<Claim>(){
                        new Claim(ClaimTypes.Sid, userEntity.Id),// 用户Id
                        new Claim(ClaimTypes.Name, userEntity.Account),// 用户账号
                        new Claim(ClaimTypes.GivenName, userEntity.RealName),
                        new Claim(ClaimTypes.PrimarySid, userEntity.OrganizeId),
                        new Claim(ClaimTypes.PrimaryGroupSid, userEntity.DepartmentId),
                        new Claim(ClaimTypes.Role, userEntity.RoleId??""),
                        new Claim(ClaimTypes.Dns, Net.Ip??"")
                    };
                    var isSystem = false;
                    if (userEntity.Account == "admin")
                    {
                        isSystem = true;
                    }
                    identity.AddClaims(claims);
                    identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isSystem.ToString()));
                    var principal = new ClaimsPrincipal(identity);
                    //过期时间20分钟
                    //var authProperty = new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(20) };
                    await HttpContext.SignInAsync(SysManageAuthAttribute.SysManageAuthScheme,
                                                                            principal);

                    logEntity.Account = userEntity.Account;
                    logEntity.NickName = userEntity.RealName;
                    logEntity.Result = true;
                    logEntity.Description = "登录成功";
                    _ISysLogService.WriteSysLog(logEntity);
                }
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。" }.ToJson());
            }
            catch (Exception ex)
            {
                logEntity.Account = username;
                logEntity.NickName = username;
                logEntity.Result = false;
                logEntity.Description = "登录失败，" + ex.Message;
                _ISysLogService.WriteSysLog(logEntity);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }


    }
}
