using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SkyCore.GlobalProvider;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using SkyMallCore.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SkyMallCore.Models;

namespace SkyMallCoreWeb.Controllers
{
    /// <summary>
    /// Controller基类
    /// </summary>
    [MemberAuth]
    public class FBaseController : Areas.BaseController
    {
        
        public FBaseController()
        {
        }


        /// <summary>
        /// 面包屑导航
        /// </summary>
        public List<ListItem> PageNavCrumbs = new List<ListItem>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.IsAjax())
            {
                //查询出顶级分类
                var layCateList = CoreContextProvider.MemCache.GetCache<List<ListItem>>("LayCateList");
                if (layCateList == null || !layCateList.Any())
                {
                    layCateList = GetArticleCateList(null,null, null);
                    CoreContextProvider.MemCache.SetCache(layCateList, "LayCateList");
                }

                ViewData["LayCateList"] = layCateList;
            }
            base.OnActionExecuting(context);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.HttpContext.Request.IsAjax())
            {
                ViewBag.PageCrumbs = PageNavCrumbs;
            }
            base.OnActionExecuted(context);
        }


        public void AddPageCrumbs(string name,string url = null)
        {
            PageNavCrumbs.Add(new ListItem { Text = name, ParentId = url });
        }
        
        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<ListItem> GetArticleCateList(IArticleCategoryService articleCategoryService, string currentId, string parentId,bool containsChild = false)
        {
            if (articleCategoryService == null)
            {
                articleCategoryService = CoreContextProvider.GetService<IArticleCategoryService>();
            }
            return articleCategoryService.GetCateList(currentId, parentId, containsChild);
        }


        public MemberDetailView GetMember(string memberId)
        {
            //if (memberId.IsEmpty())
            //{
            //    memberId = CoreContextProvider.CurrentMember.UserId;
            //}
            var data = CoreContextProvider.GetService<IMemberService>().GetMember(memberId);
            var result = AutoMapper.Mapper.Map<MemberDetailView>(data);
            if (result == null)
            {
                result = new MemberDetailView { UserName = "管理员"};
            }
            return result;
        }



        /// <summary>
        /// 写入阅读数量
        /// </summary>
        /// <param name="readType"></param>
        /// <param name="id"></param>
        public void UserRead(ReadType readType, string id)
        {
            var hasRead = CoreContextProvider.HttpContext.GetCookie($"{readType}_{id}").IsEmpty();
            if (hasRead)
            {
                CoreContextProvider.HttpContext.WriteCookie($"{readType}_{id}", id);
                Task.Run(() =>
                {
                    switch (readType)
                    {
                        case ReadType.News:
                            CoreContextProvider.GetService<INewsService>().ClientRead(id);
                            break;
                        case ReadType.Topic:
                            CoreContextProvider.GetService<IArticleTopicService>().ClientRead(id);
                            break;
                        case ReadType.Article:
                            CoreContextProvider.GetService<IArticleService>().ClientRead(id);
                            break;
                    }
                });
            }
        }




        /// <summary>
        /// 用户登陆信息
        /// </summary>
        /// <param name="member"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        public  async Task WriteUserIdentity(Member member, bool remember = false)
        {
            //清除
            await HttpContext.SignOutAsync(ConstParameters.MemberAuthScheme);

            var identity = new ClaimsIdentity(ConstParameters.MemberAuthScheme);  // 指定身份认证类型
            List<Claim> claims = new List<Claim>(){
                        new Claim(ClaimTypes.Sid, member.Id),// 用户Id
                        new Claim(ClaimTypes.NameIdentifier, member.UserName),// 用户账号
                        new Claim(ClaimTypes.Name, member.NickName??""),
                        new Claim(ClaimTypes.Spn, member.Password),
                        new Claim(ClaimTypes.Dns, HttpContext.GetIP()??""),
                        new Claim(ClaimTypes.Actor, member.HeadIcon??"")
                    };
            identity.AddClaims(claims);
            var principal = new ClaimsPrincipal(identity);

            var remMinutes = 60;
            if (remember)
            {
                remMinutes = 24 * 60;
            }
            var authProperty = new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(remMinutes) };
            await HttpContext.SignInAsync(ConstParameters.MemberAuthScheme,
                                                                    principal, authProperty);

        }







    }
}
