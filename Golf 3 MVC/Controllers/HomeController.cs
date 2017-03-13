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
            //dynamic email = new Email("Example");
            //email.To = "webninja@example.com";
            //email.FunnyLink = DB.GetRandomLolcatLink();
            //email.Send();
            return View();
        }

        public ActionResult Medlem()
        {
            return View();
        }

        public ActionResult Banorna()
        {
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

        public ActionResult Tavling()
        {
            ViewBag.Message = "Tävling";

            return View();
        }

    }
}