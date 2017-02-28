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
    public class medlemskategorisController : Controller
    {
        private dsu3Entities db = new dsu3Entities();

        // GET: medlemskategoris
        public ActionResult Index()
        {
            return View(db.medlemskategoris.ToList());
        }

        // GET: medlemskategoris/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medlemskategori medlemskategori = db.medlemskategoris.Find(id);
            if (medlemskategori == null)
            {
                return HttpNotFound();
            }
            return View(medlemskategori);
        }

        // GET: medlemskategoris/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: medlemskategoris/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "avgift,namn,kategori_id")] medlemskategori medlemskategori)
        {
            if (ModelState.IsValid)
            {
                db.medlemskategoris.Add(medlemskategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medlemskategori);
        }

        // GET: medlemskategoris/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medlemskategori medlemskategori = db.medlemskategoris.Find(id);
            if (medlemskategori == null)
            {
                return HttpNotFound();
            }
            return View(medlemskategori);
        }

        // POST: medlemskategoris/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "avgift,namn,kategori_id")] medlemskategori medlemskategori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medlemskategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medlemskategori);
        }

        // GET: medlemskategoris/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medlemskategori medlemskategori = db.medlemskategoris.Find(id);
            if (medlemskategori == null)
            {
                return HttpNotFound();
            }
            return View(medlemskategori);
        }

        // POST: medlemskategoris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            medlemskategori medlemskategori = db.medlemskategoris.Find(id);
            db.medlemskategoris.Remove(medlemskategori);
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
