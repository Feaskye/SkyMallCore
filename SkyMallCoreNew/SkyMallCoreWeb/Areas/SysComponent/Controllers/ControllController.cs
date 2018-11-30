using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyCore.GlobalProvider;
using SkyMallCore.Services;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SkyMallCoreWeb.Areas.SysComponent.Controllers
{

    /// <summary>
    /// 小控件公用数据显示
    /// </summary>
    [Area("SysComponent")]
    [SysManageAuth]
    [AllowAnonymous]
    public class ControllController : SkyMallCoreWeb.Controllers.FBaseController
    {
        ISysAreaService _SysAreaService;
        public ControllController(ISysAreaService sysAreaService)
        {
            _SysAreaService = sysAreaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAuthCode()
        {
            //AuthCodeEnum id
            //Enum.IsDefined(typeof(AuthCodeEnum), id);
            //var key = id;
            return File(CoreContextProvider.NewVerifyCode(ConstParameters.VerifyCodeKeyName), @"image/Gif");
        }


        /// <summary>
        /// 省市区联动，逐级获取
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetArea(string parentId)
        {
            var areaList = _SysAreaService.GetAreaList(parentId);
            return Content(areaList.ToJson());
        }

        /// <summary>
        /// 获取某项字典列表
        /// </summary>
        /// <param name="enCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetItemDictionary(string enCode)
        {
            return Content(BusinessHelper.GetItemDictionary(enCode).ToJson());
        }

        /// <summary>
        /// 文件上传待处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadFiles([FromQuery]UpLoadAction action,string specilName = null)
        {
            //string action = Request.Query["action"];
            //EnumCommon.GetEnum<UpLoadAction>(action)
            var result = InvokeUploadFiles(action,specilName);
            string data = null;
            if (result.Data != null && result.Data.Any())
            {
                data = string.Join(",", result.Data.Select(u => u.FileName));
            }
            return JsonResult(new InvokeResult<string> { Success = result.Success, Message = result.Message, Data = data });
        }


        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetArticleCates(string currentId, string parentId)
        {
            return Content(GetArticleCateList(currentId, parentId).ToJson());
        }

        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetArticleTopics([FromServices]IArticleTopicService articleTopicService, string currentId, string parentId)
        {
            //var articleTopicService = CoreContextProvider.GetService<IArticleTopicService>();
            return Content(articleTopicService.GetTopicCateList(currentId, parentId).ToJson());
        }

        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNewsCates([FromServices]INewsCategoryService newsCategoryService,string currentId, string parentId)
        {
            //var newsCateServices = CoreContextProvider.GetService<INewsCategoryService>();
            return Content(newsCategoryService.GetCateList(currentId, parentId).ToJson());
        }


    }




    /// <summary>
    /// ViewComponent 用法
    ///  //https://stackoverflow.com/questions/41353874/equivalent-of-html-renderaction-in-asp-net-core
    /// </summary>
    public class PagerViewComponent : ViewComponent
    {
        //private DbContextOptions<MyContext> db = new DbContextOptions<MyContext>();
        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    MyContext context = new MyContext(db);
        //    IEnumerable<tableRowClass> mc = await context.tableRows.ToListAsync();
        //    return View(mc);
        //}
    }



}
