using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkyMallCore.ViewModel;
using SkyMallCore.Core;
using SkyMallCore.Services;
using SkyMallCore.Models;
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class SysConfigurationController : SysBaseController
    {
        private ISysConfigurationService _Service;

        public SysConfigurationController(ISysConfigurationService service)
        {
            _Service = service;
        }


        [HttpGet]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _Service.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _Service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysConfiguration config, string permissionIds, string keyValue)
        {
            if (config.ConfigCode == ConstParameters.VoiceRootFolderCode)
            {
                if (string.IsNullOrWhiteSpace(config.ConfigValue))
                {
                    return Error("配置值不能为空");
                }
                if (FileHelper.IsExistMapFolder(config.ConfigValue))
                {
                    return Error("该文件夹已存在不能为空");
                }
                FileHelper.CreateDir(config.ConfigValue);
            }
            //_Service.SubmitForm(link, permissionIds.Split(','), keyValue);
            config.Id = keyValue;
            _Service.SubmitForm(config);
            ConfigManager.SysConfiguration = null;
            BusinessHelper.LoadSysConfiguration();
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _Service.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}