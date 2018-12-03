using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using SkyCore.GlobalProvider;
using System.IO;

namespace SkyMallCoreWeb.Areas
{

    /// <summary>
    /// 后台管理基类
    /// </summary>
    [Area("SystemManage")]
    public class SysBaseController : SysControllerBase
    {
        public ISysLogService _logService;
        public SysBaseController()
        {
            _logService = CoreContextProvider.GetService<ISysLogService>();
        }


        public virtual void WriteDbLog(string msg, bool b = true)
        {
            var sysLog = new SysLog();
            sysLog.Id = Common.GuId();
            sysLog.Date = DateTime.Now;
            sysLog.Account = CoreContextProvider.CurrentSysUser == null ? "" : CoreContextProvider.CurrentSysUser.Account;
            sysLog.NickName = CoreContextProvider.CurrentSysUser == null ? "" : CoreContextProvider.CurrentSysUser.RealName;
            sysLog.IPAddress = HttpContext.GetIP();
            sysLog.IPAddressName = NetClient.GetLocation(sysLog.IPAddress);
            sysLog.Result = b;
            sysLog.Description = msg;
            Task.Factory.StartNew(()=> {
                try
                {
                    var logService = CoreContextProvider.GetService<SkyMallCore.Respository.ISysLogRespository>();
                    logService.OperatLog(sysLog);
                }
                catch
                { }
            });
        }

    }

    /// <summary>
    /// 后台安全管理基类
    /// </summary>
    [Area("SystemSecurity")]
    public class SysSecBaseController : SysControllerBase
    {  }




    /// <summary>
    /// 后台全局管理控制器基类
    /// </summary>
    [SysManageAuth]
    public class SysControllerBase : BaseController
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult Details()
        {
            return View();
        }






    }

    /// <summary>
    /// 全局控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        public ILogger _Logger;
        protected int PageSize = CoreContextProvider.PageSize;
        protected int PageIndex = 1;
        


        public BaseController()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _Logger = CoreContextProvider.GetLogger(this.ControllerContext.ActionDescriptor.ControllerTypeInfo.Name);
            
            base.OnActionExecuting(context);
        }


        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="specilName">指定文件名</param>
        /// <returns></returns>
        public InvokeResult<List<UploadedFileModel>> InvokeUploadFiles(UpLoadAction action, string specilName = null)
        {
            var uploadedFiles = new List<UploadedFileModel>();
            var files = Request.Form.Files;
            var uploadFolder = ConfigManager.UploadFolder;
            if (!FileHelper.IsExistMapFolder(uploadFolder))
            {
                FileHelper.CreateDir(uploadFolder);
            }
            var dateFolder = DateTime.Now.ToString("yyyyMMdd");
            if (!FileHelper.IsExistMapFolder(uploadFolder + "\\" + dateFolder))
            {
                FileHelper.CreateDir(uploadFolder + "\\" + dateFolder);
            }

            var uploadAllowExtension = new string[] { };
            switch (action)
            {
                case UpLoadAction.cover:
                    uploadAllowExtension = ConfigManager.UploadAllowImgExtension;
                    break;
                case UpLoadAction.package:
                    uploadAllowExtension = ConfigManager.UploadAllowPackExtension;
                    break;
                case UpLoadAction.attichfile:
                    var extenList = ConfigManager.UploadAllowPackExtension.ToList();
                    extenList.AddRange(ConfigManager.UploadAllowOfficeExtension);
                    extenList.AddRange(ConfigManager.UploadAllowImgExtension);
                    extenList.AddRange(ConfigManager.UploadAllowVideoExtension);
                    extenList.AddRange(ConfigManager.UploadAllowVoiceExtension);
                    uploadAllowExtension = extenList.ToArray();
                    break;
            }

            if (!uploadAllowExtension.Any())
            {
                if (UpLoadAction.attichfile == action)
                {
                    return RequestResult.Failed<List<UploadedFileModel>>("只能上传文档、图片、视/音频类格式文件");
                }
                return RequestResult.Failed<List<UploadedFileModel>>("只能上传文件格式：" + string.Join(",", uploadAllowExtension));
            }
            
            var allowExten = files.Select(u => FileHelper.GetExtension(u.FileName).Replace(".", ""));
            var leaveExten = allowExten.Where(w => !uploadAllowExtension.Contains(w));
            if (leaveExten.Any())
            {
                return RequestResult.Failed<List<UploadedFileModel>>("不支持的文件格式：" + string.Join(',', leaveExten));
            }
            if (files.Any(w=>w.Length > ConfigManager.MaxFileLength))
            {
                return RequestResult.Failed<List<UploadedFileModel>>("文件最大不能超过200M，请重新选择文件");
            }
            try
            {
                int i = 0;
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var extension = FileHelper.GetExtension(formFile.FileName);
                        if (string.IsNullOrWhiteSpace(specilName))
                        {
                            specilName = Guid.NewGuid().ToString().Replace("-", "");
                        }
                        else
                        {
                            if (specilName == "headicon")
                            {
                                dateFolder = "headicon";
                                specilName = (CoreContextProvider.CurrentMember?.Account)??Request.Form["ownid"];
                                if (specilName.IsEmpty())
                                {
                                    return RequestResult.Failed<List<UploadedFileModel>>("上传参数有误！");
                                }
                                extension = ".jpg";
                                if (!Directory.Exists(FileHelper.MapFilePath($"/{uploadFolder}/{dateFolder}")))
                                {
                                    Directory.CreateDirectory(FileHelper.MapFilePath($"/{uploadFolder}/{dateFolder}"));
                                }
                            }
                            else
                            {
                                specilName = specilName + i;
                            }
                        }
                        var fileName = $"/{uploadFolder}/{dateFolder}/{specilName}" + extension;
                        var filePath = FileHelper.MapFilePath(fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                            uploadedFiles.Add(new UploadedFileModel { FileName = fileName,Name = formFile.FileName, FileType = extension.Trim('.'), FileSize = stream.Length });
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "文件上传失败！");
                return RequestResult.Failed<List<UploadedFileModel>>("文件上传失败！");
            }
            return RequestResult.Success(uploadedFiles);
        }


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTitile"></param>
        /// <param name="mailTo"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public InvokeResult<bool> SendSysEmail(string mailTitile,string mailTo,string content)
        {
            MailHelper mailHelper = new MailHelper(ConfigManager.MailServer
                                                                                    , ConfigManager.SysConfiguration.SysMailUser, 
                                                                                    ConfigManager.SysConfiguration.SysMailPassword, 
                                                                                    ConfigManager.SysConfiguration.SiteName, 
                                                                                    ConfigManager.MailServerPort);
            try
            {
                var b = mailHelper.Send(mailTo, mailTitile, content);
                return RequestResult.Result(b, "发送失败");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.ToString());
                return RequestResult.Failed<bool>(ex.Message + "，发送失败！");
            }
        }




        protected virtual ActionResult JsonResult(string message)
        {
            return Content(new AjaxResult { state = ResultType.error, message = message }.ToJson());
        }

        protected virtual ActionResult JsonResult(object data)
        {
            return Content(new AjaxResult { state = ResultType.success, data = data }.ToJson());
        }


        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = null, data = data }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }

        protected virtual ActionResult JsonResult<TResult>(InvokeResult<TResult> result)
        {
            if (!result.Success)
            {
                return Error(result.Message);
            }
            return Success(result.Data);
        }

        protected virtual ActionResult Error(string message,object data = null)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message, data = data }.ToJson());
        }

        [AllowAnonymous]
        public virtual ActionResult ErrorPage(string message)
        {
            return View("Error", new ErrorViewModel() { Message = message });
        }









    }
}