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
    public class bokningsController : Controller
    {
        private dsu3Entities db = new dsu3Entities();

        // GET: boknings
        public ActionResult Index()
        {
            var boknings = db.boknings.Include(b => b.medlemmar);
            return View(boknings.ToList());
        }

        // GET: boknings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bokning bokning = db.boknings.Find(id);
            if (bokning == null)
            {
                return HttpNotFound();
            }
            return View(bokning);
        }

        // GET: boknings/Create
        public ActionResult Create()
        {
            ViewBag.medlems_id = new SelectList(db.medlemmars, "id", "fornamn");
            return View();
        }

        // POST: boknings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,start_date,medlems_id,end_date,text")] bokning bokning)
        {
            if (ModelState.IsValid)
            {
                db.boknings.Add(bokning);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.medlems_id = new SelectList(db.medlemmars, "id", "fornamn", bokning.medlems_id);
            return View(bokning);
        }

        // GET: boknings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bokning bokning = db.boknings.Find(id);
            if (bokning == null)
            {
                return HttpNotFound();
            }
            ViewBag.medlems_id = new SelectList(db.medlemmars, "id", "fornamn", bokning.medlems_id);
            return View(bokning);
        }

        // POST: boknings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,start_date,medlems_id,end_date,text")] bokning bokning)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bokning).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.medlems_id = new SelectList(db.medlemmars, "id", "fornamn", bokning.medlems_id);
            return View(bokning);
        }

        // GET: boknings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bokning bokning = db.boknings.Find(id);
            if (bokning == null)
            {
                return HttpNotFound();
            }
            return View(bokning);
        }

        // POST: boknings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bokning bokning = db.boknings.Find(id);
            db.boknings.Remove(bokning);
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
