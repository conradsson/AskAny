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
    public class starttidsController : Controller
    {
        private dsu3Entities db = new dsu3Entities();

        // GET: starttids
        public ActionResult Index()
        {
            return View(db.starttids.ToList());
        }

        // GET: starttids/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            starttid starttid = db.starttids.Find(id);
            if (starttid == null)
            {
                return HttpNotFound();
            }
            return View(starttid);
        }

        // GET: starttids/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: starttids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "starttid1")] starttid starttid)
        {
            if (ModelState.IsValid)
            {
                db.starttids.Add(starttid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(starttid);
        }

        // GET: starttids/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            starttid starttid = db.starttids.Find(id);
            if (starttid == null)
            {
                return HttpNotFound();
            }
            return View(starttid);
        }

        // POST: starttids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "starttid1")] starttid starttid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(starttid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(starttid);
        }

        // GET: starttids/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            starttid starttid = db.starttids.Find(id);
            if (starttid == null)
            {
                return HttpNotFound();
            }
            return View(starttid);
        }

        // POST: starttids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            starttid starttid = db.starttids.Find(id);
            db.starttids.Remove(starttid);
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
