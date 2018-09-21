using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkyMallCore.WebApi.Models;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : Controller
    {
        IActionDescriptorCollectionProvider ActionDescriptorCollectionProvider;
        public HomeController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            ActionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }



        /// <summary>
        /// 获取首页内容  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }


        /// <summary>
        /// 获取所有的Actions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetActions()
        {
            return Ok(ActionDescriptorCollectionProvider
            .ActionDescriptors
            .Items
            .OfType<ControllerActionDescriptor>()
            .Select(a => new
            {
                a.DisplayName,
                a.ControllerName,
                a.ActionName,
                AttributeRouteTemplate = a.AttributeRouteInfo?.Template,
                HttpMethods = string.Join(", ", a.ActionConstraints?.OfType<HttpMethodActionConstraint>().SingleOrDefault()?.HttpMethods ?? new string[] { "any" }),
                Parameters = a.Parameters?.Select(p => new
                {
                    Type = p.ParameterType.Name,
                    p.Name
                }),
                ControllerClassName = a.ControllerTypeInfo.FullName,
                ActionMethodName = a.MethodInfo.Name,
                Filters = a.FilterDescriptors?.Select(f => new
                {
                    ClassName = f.Filter.GetType().FullName,
                    f.Scope //10 = Global, 20 = Controller, 30 = Action
                }),
                Constraints = a.ActionConstraints?.Select(c => new
                {
                    Type = c.GetType().Name
                }),
                RouteValues = a.RouteValues.Select(r => new
                {
                    r.Key,
                    r.Value
                }),
            }));
        }

        /// <summary>
        /// Discovering Razor Pages
        /// </summary>
        /// <returns></returns>
        public IActionResult GetPages()
        {
            return Ok(ActionDescriptorCollectionProvider
                .ActionDescriptors
                .Items
                .OfType<PageActionDescriptor>()
                .Select(a => new
                {
                    a.DisplayName,
                    a.ViewEnginePath,
                    a.RelativePath,
                }));
        }


    }











}
