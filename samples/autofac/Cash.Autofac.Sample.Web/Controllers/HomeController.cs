using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Cash.Autofac.Sample.Web.Services;

namespace Cash.Autofac.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRandomDataService _randomDataService;

        public HomeController(IRandomDataService randomDataService)
        {
            _randomDataService = randomDataService;
        }

        public ActionResult Index()
        {
            ViewBag.CachedRandomNumber = _randomDataService.GetCachedRandomNumber();
            ViewBag.NonCachedRandomNumber = _randomDataService.GetNonCachedRandomNumber();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}