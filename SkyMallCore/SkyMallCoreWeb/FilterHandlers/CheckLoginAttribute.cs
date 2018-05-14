using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb
{
    //-----------------仅登录权限判断
    public class CheckLoginAttribute: AuthorizeAttribute
    {
        private static AuthorizationPolicy _policy_ = new AuthorizationPolicy(new[] {
            new DenyAnonymousAuthorizationRequirement()
        }, new string[] { });

        public bool Ignore = true;
        public CheckLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
            //base.Policy = _policy_.Requirements;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (Ignore == false)
        //    {
        //        return;
        //    }
        //    if (OperatorProvider.Provider.GetCurrent() == null)
        //    {
        //        WebHelper.WriteCookie("nfine_login_error", "overdue");
        //        filterContext.HttpContext.Response.Write("<script>top.location.href = '/SystemManage/Login/Index';</script>");
        //        return;
        //    }
        //}
    }

    //---------------------------------------详细权限
    public class ClaimRequirementAttribute : Microsoft.AspNetCore.Mvc.TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            if (!hasClaim)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
            }
        }
    }

}
