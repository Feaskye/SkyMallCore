using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SkyMallCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyMallCoreWeb
{
    /// <summary>
    /// 系统管理权限验证
    /// </summary>
    public class SysRoleAuthAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
        public SysRoleAuthAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CoreContextProvider.CurrentSysUser.IsSystem)
            {
                return;
            }
            if (Ignore == false)
            {
                return;
            }
            if (!this.ActionAuthorize(filterContext))
            {
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');</script>");
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var operatorProvider = CoreContextProvider.CurrentSysUser;
            var roleId = operatorProvider.RoleId;
            var moduleId = WebHelper.GetCookie("nfine_currentmoduleid");
            var action = CoreContextProvider.HttpContext.Request.Headers["SCRIPT_NAME"].ToString();
            return CoreContextProvider.GetService<SkyMallCore.Services.ISysRoleAuthorizeService>()
                .ActionValidate(roleId, moduleId, action);
        }
    }
}
