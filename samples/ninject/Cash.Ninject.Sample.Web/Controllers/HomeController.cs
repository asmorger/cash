using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Cash.Sample.Shared.Services;

namespace Cash.Ninject.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRandomDataService _randomDataService;

        private readonly IUserService _userService;

        public HomeController(IRandomDataService randomDataService, IUserService userService)
        {
            _randomDataService = randomDataService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            ViewBag.CachedRandomNumber = _randomDataService.GetCachedRandomNumber();
            ViewBag.NonCachedRandomNumber = _randomDataService.GetNonCachedRandomNumber();

            var user = _userService.GetUserById(1);
            var address = _userService.GetByUser(user);

            ViewBag.UserName = string.Concat(user.FirstName, " ", user.LastName);
            ViewBag.Address = string.Concat(address.PrimaryAddress, " ", address.SecondaryAddress);

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