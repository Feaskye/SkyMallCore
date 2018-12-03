using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SkyMallCore.Services;
using SkyCoreLib.Utils;
using SkyMallCore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using SimpleMvcSitemap;

namespace SkyMallCoreWeb.Controllers
{
    [AllowAnonymous]
    public class HelpController : FBaseController
    {
        IHelpService _HelpService;
        ILinkService _ILinkService;
        private List<HelpDetailView> helpDetails;
        public HelpController(IHelpService helpService, ILinkService linkService)
        {
            _HelpService = helpService;
            _ILinkService = linkService;
            helpDetails = _HelpService.GetHelps(new HelpSearchView() { HelpCode = HelpCode.HelpCenter }, 1, 20).ToList();
        }

        [Route("/help")]
        [Route("/help/{hid=hid}")]
        public IActionResult Index(string hid)
        {
            HelpDetailView helpDetail= helpDetails.FirstOrDefault();
            if (!hid.IsEmpty())
            {
                helpDetail = helpDetails.Where(w=>w.Id == hid).FirstOrDefault();
            }
            AddPageCrumbs(helpDetail.Title);
            return View(helpDetail);
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        [Route("/pub/{hid=hid}")]
        public IActionResult Announcement(string hid)
        {
            HelpDetailView helpDetail =new HelpDetailView();
            if (!hid.IsEmpty())
            {
                helpDetail = AutoMapper.Mapper.Map<HelpDetailView>(_HelpService.GetForm(hid));
            }
            AddPageCrumbs("网站公告");
            return View(helpDetail);
        }

        [Route("/Friend")]
        public IActionResult Friend()
        {
            var links = _ILinkService.GetList();
            AddPageCrumbs("友情链接");
            ViewBag.Links = links;
            return View("Index");
        }

        /// <summary>
        /// 网站地图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
       [Route("/Sitemap")]
        public IActionResult Sitemap()
        {
            return View();
        }

        /// <summary>
        /// 网站地图xml
        /// https://github.com/uhaciogullari/SimpleMvcSitemap#installation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/sitemap.xml")]
        public IActionResult sitemapxml()
        {
            List<SitemapNode> nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index","Home")),
                new SitemapNode(Url.Action("Index","Help")),
                new SitemapNode(Url.Action("Index","Article")),
                new SitemapNode(Url.Action("Index","ArticleCategory")),
                new SitemapNode(Url.Action("Index","ArticleTopic")),
                new SitemapNode(Url.Action("Index","News"))
                //other nodes
            };

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

        /// <summary>
        /// 用户协议
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/memprotocol")]
        public IActionResult MemProtocol()
        {
            var detail = _HelpService.GetHelpByCode(HelpCode.MemProtocol);
            AddPageCrumbs("用户协议");
            return View(detail);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.HelpList = helpDetails;
            base.OnActionExecuted(context);
        }






















    }
}