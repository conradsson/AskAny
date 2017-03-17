using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Golf_3_MVC.Models;

namespace Golf_3_MVC.Controllers
{
    public class TavlingController : Controller
    {
        private dsu3Entities db = new dsu3Entities();

        // GET: Tavling
        public ActionResult Index()
        {
            return View();
        }
    }
}