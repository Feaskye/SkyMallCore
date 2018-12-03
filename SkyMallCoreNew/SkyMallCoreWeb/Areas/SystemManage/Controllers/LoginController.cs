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

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 后台登录页
    /// </summary>
    public class LoginController : SysBaseController
    {
        ISysUserService _SysUserService;
        ISysLogService _ISysLogService;
        public LoginController(ISysUserService sysUserService
            , ISysLogService sysLogService)
        {
            _SysUserService = sysUserService;
            _ISysLogService = sysLogService;
        }

        [HttpGet]
        [AllowAnonymous]
        public override ActionResult Index()
        {
            _Logger.LogInformation("log testing.........................................");
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAuthCode()
        {
            return File(CoreContextProvider.NewVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public async Task<IActionResult> OutLogin()
        {
            _ISysLogService.WriteSysLog(new SysLog
            {
                ModuleName = "系统登录",
                Type = DbLogType.Exit.ToString(),
                Account = CoreContextProvider.CurrentSysUser==null?"": CoreContextProvider.CurrentSysUser.Account,
                NickName = CoreContextProvider.CurrentSysUser == null?"": CoreContextProvider.CurrentSysUser.RealName,
                Result = true,
                Description = "安全退出系统",
            });

            this.Request.HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(ConstParameters.SysManageAuthScheme);
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
            if (CoreContextProvider.CurrentSysUser != null)
            {//若有用户信息 则清除
                await HttpContext.SignOutAsync(ConstParameters.SysManageAuthScheme);
            }
            SysLog logEntity = new SysLog();
            logEntity.ModuleName = "系统登录";
            logEntity.Type = DbLogType.Login.ToString();
            try
            {
                if (HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).IsEmpty() 
                    || Md5Hash.Md5(code.ToLower(), 16) != HttpContext.GetSession(ConstParameters.VerifyCodeKeyName).ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }

                var userEntity = _SysUserService.CheckLogin(username, password);
                if (userEntity != null)
                {
                    //登录已重写
                    var identity = new ClaimsIdentity(ConstParameters.SysManageAuthScheme);  // 指定身份认证类型
                    List<Claim> claims = new List<Claim>(){
                        new Claim(ClaimTypes.Sid, userEntity.Id),// 用户Id
                        new Claim(ClaimTypes.Name, userEntity.Account),// 用户账号
                        new Claim(ClaimTypes.GivenName, userEntity.RealName),
                        new Claim(ClaimTypes.PrimarySid, userEntity.OrganizeId),
                        new Claim(ClaimTypes.PrimaryGroupSid, userEntity.DepartmentId),
                        new Claim(ClaimTypes.Role, userEntity.RoleId??""),
                        new Claim(ClaimTypes.Dns, HttpContext.GetIP()??"")
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
                    await HttpContext.SignInAsync(ConstParameters.SysManageAuthScheme,
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
                CoreContextProvider.GetLogger("LoginController").LogError(ex, logEntity.ToJson());
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }


    }
}
