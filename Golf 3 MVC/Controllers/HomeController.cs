using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf_3_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Medlem()
        {
            ViewBag.Message = "Medlem";

            return View();
        }
        public ActionResult Banorna()
        {
            ViewBag.Message = "Banorna";

            return View();
        }

        public ActionResult Klubben()
        {
            ViewBag.Message = "Klubben";

            return View();
        }

        public ActionResult Kontakt()
        {
            ViewBag.Message = "Kontakt";

            return View();
        }
    }
}