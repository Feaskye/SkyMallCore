using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SkyNetCore.Web.Models;

namespace SkyNetCore.Web.Controllers
{
    public class HomeController : Controller
    {
        //private IHubContext<MessageHub> HubContext;
        //public HomeController(IHubContext<MessageHub> hubContext)
        //{
        //    HubContext = hubContext;
        //}


        public IActionResult Index()
        {

            Task.Factory.StartNew(()=> {
                
            });


            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult PostData()
        {
            return Content(".................................................................................................");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
