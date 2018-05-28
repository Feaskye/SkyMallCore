

using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Services;
using System.Collections.Generic;
using System.Linq;


namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    public class ManagerController : SysControllerBase
    {
        private ISysUserService UserApp;
        private ISysUserLogOnService UserLogOnApp;

        public ManagerController(ISysUserService userApp, ISysUserLogOnService userLogOnApp)
        {
            UserApp = userApp;
            UserLogOnApp = userLogOnApp;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = UserApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = UserApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysUser SysUser, SysUserLogOn SysUserLogOn, string keyValue)
        {
            UserApp.SubmitForm(SysUser, SysUserLogOn, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            UserApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            UserLogOnApp.RevisePassword(userPassword, keyValue);
            return Success("重置密码成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            SysUser SysUser = new SysUser();
            SysUser.Id = keyValue;
            SysUser.EnabledMark = false;
            UserApp.UpdateForm(SysUser);
            return Success("账户禁用成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            SysUser SysUser = new SysUser();
            SysUser.Id = keyValue;
            SysUser.EnabledMark = true;
            UserApp.UpdateForm(SysUser);
            return Success("账户启用成功。");
        }

        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }
    }
}
