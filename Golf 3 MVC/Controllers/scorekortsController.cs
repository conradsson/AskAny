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
    public class scorekortsController : Controller
    {
        private dsu3Entities ds = new dsu3Entities();
        private dsu3Entities3 db = new dsu3Entities3();

        // GET: scorekorts
        public ActionResult Index()
        {
            return View(db.scorekort.ToList());
        }

        public ActionResult scorekort(int bokningsID, string golfID) //Lägg till int bokningsID, string golfID
        {
            Score model = new Score();

            List<scorekort> scorkort1 = new List<scorekort>();           

            scorkort1 = db.scorekort.ToList();

            List<slope> slope1 = new List<slope>();

            slope1 = db.slope.ToList();

            List<medlemmar> medlem1 = new List<medlemmar>();
            medlem1 = ds.medlemmars.ToList();

            List<bokning> bokning1 = new List<bokning>();
            bokning1 = ds.boknings.ToList();

            List<medbokare> medbokare1 = new List<medbokare>();
            medbokare1 = ds.medbokares.ToList();

            model.bokning = bokning1;
            model.medlems = medlem1;
            model.scoreKort = scorkort1;
            model.slope = slope1;
            model.medbokare = medbokare1;
            model.bokningsID = bokningsID; //ViewBag.bokningsID;
            model.golfID = golfID; //ViewBag.golfID;



            return View(model);
        }

        // GET: scorekorts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scorekort scorekort = db.scorekort.Find(id);
            if (scorekort == null)
            {
                return HttpNotFound();
            }
            return View(scorekort);
        }

        // GET: scorekorts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: scorekorts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Hole_nr,Lenght_Tee1,Lenght_Tee2,Par,Index")] scorekort scorekort)
        {
            if (ModelState.IsValid)
            {
                db.scorekort.Add(scorekort);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scorekort);
        }

        // GET: scorekorts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scorekort scorekort = db.scorekort.Find(id);
            if (scorekort == null)
            {
                return HttpNotFound();
            }
            return View(scorekort);
        }

        // POST: scorekorts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Hole_nr,Lenght_Tee1,Lenght_Tee2,Par,Index")] scorekort scorekort)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scorekort).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scorekort);
        }

        // GET: scorekorts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scorekort scorekort = db.scorekort.Find(id);
            if (scorekort == null)
            {
                return HttpNotFound();
            }
            return View(scorekort);
        }

        // POST: scorekorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            scorekort scorekort = db.scorekort.Find(id);
            db.scorekort.Remove(scorekort);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
