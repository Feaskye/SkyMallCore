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

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 后台登录页
    /// </summary>
    public class LoginController : Controller
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
        [Area("SystemManage")]
        [HttpGet]
        public virtual IActionResult Index()
        {
            var test = string.Format("{0:E2}", 1);
            return View();
        }
        [Area("SystemManage")]
        [HttpGet]
        public IActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        [SysManageAuth]
        public async Task<IActionResult> OutLogin()
        {
            _ISysLogService.WriteSysLog(new SysLog
            {
                ModuleName = "系统登录",
                Type = DbLogType.Exit.ToString(),
                Account = CoreProviderContext.Provider.GetCurrent().UserCode,
                NickName = CoreProviderContext.Provider.GetCurrent().UserName,
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
        /// 登录方式需要改造 https://www.codeproject.com/Articles/1205161/ASP-NET-Core-Cookie-Authentication
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        // [HandlerAjaxOnly]
        [Area("SystemManage")]
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
                    OperatorModel operatorModel = new OperatorModel();
                    operatorModel.UserId = userEntity.Id;
                    operatorModel.UserCode = userEntity.Account;
                    operatorModel.UserName = userEntity.RealName;
                    operatorModel.CompanyId = userEntity.OrganizeId;
                    operatorModel.DepartmentId = userEntity.DepartmentId;
                    operatorModel.RoleId = userEntity.RoleId;
                    operatorModel.LoginIPAddress = Net.Ip;
                    operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
                    operatorModel.LoginTime = DateTime.Now;
                    operatorModel.LoginToken = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                    if (userEntity.Account == "admin")
                    {
                        operatorModel.IsSystem = true;
                    }
                    else
                    {
                        operatorModel.IsSystem = false;
                    }

                    //登录已重写
                    //CoreProviderContext.Provider.AddCurrent(operatorModel);
                    var identity = new ClaimsIdentity(SysManageAuthAttribute.SysManageAuthScheme);  // 指定身份认证类型
                    identity.AddClaim(new Claim(ClaimTypes.Sid, userEntity.Id)); // 用户Id
                    identity.AddClaim(new Claim(ClaimTypes.Name, userEntity.Account));// 用户账号
                    var principal = new ClaimsPrincipal(identity);
                    //过期时间20分钟
                    var authProperty = new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(20) };

                    await HttpContext.SignInAsync(SysManageAuthAttribute.SysManageAuthScheme,
                                                                            principal, authProperty);

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

            /*
             todo 登录异常
             {"state":"error","message":"The property 'Id' on entity type 'SysUserLogOn' 
                is part of a key and so cannot be modified or marked as modified. To change the principal of an 
                existing entity with an identifying foreign key first delete the dependent 
                and invoke 'SaveChanges' then associate the dependent with the new principal.","data":null}
             */


        }


    }
}
