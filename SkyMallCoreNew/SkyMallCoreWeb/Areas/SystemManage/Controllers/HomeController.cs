using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
using SkyMallCore.ViewModel;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : SysBaseController
    {
        
        public IActionResult About()
        {
            var descrition = new StringBuilder();
            //descrition.Append("1：该项目是由NFine开源项目转化而来    \r\n");
            descrition.Append("1：该项目基本框架开发平台是在Asp.net Core 2.1基础上编写\r\n");
            //descrition.Append("3：旨在促进.Net Core跨平台学习交流，提高开发效率");
            descrition.Append("2：Asp.Net Core Mvc + EFCore 等技术，该项目仍会继续完善！");
            return Content(descrition.ToString());
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        public IActionResult FileUpload(FileUploadModel fileUpload)
        {
            if (fileUpload == null)
            {
                fileUpload = new FileUploadModel();
            }
            return View(fileUpload);
        }



        //[HttpPost]
        //public async Task<IActionResult> FileUpload(List<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);
        //    var uploadFolder = "files";
        //    // full path to file in temp location
        //    var filePath = "";
        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var extension =FileHelper.GetExtension(formFile.FileName);
        //            filePath = FileHelper.MapFilePath(uploadFolder + $"\\{DateTime.Now.ToString("yyyyMMdd")}\\{Guid.NewGuid().ToString().Replace("-", "")}."+ extension);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    return Success(filePath);
        //}
        




    }
}
