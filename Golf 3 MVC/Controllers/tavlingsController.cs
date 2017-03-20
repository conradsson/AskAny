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
    public class tavlingsController : Controller
    {
        private dsu3Entities db = new dsu3Entities();

        // GET: tavlings
        public ActionResult Index()
        {
            return View(db.tavlings.ToList());
        }

        // GET: tavlings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tavling tavling = db.tavlings.Find(id);
            if (tavling == null)
            {
                return HttpNotFound();
            }
            return View(tavling);
        }

        // GET: tavlings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tavlings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,tavlingsNamn,tavlingsDatum,forstaStarttid,sistaAnmalning,tavlingsForm,publicerad")] tavling tavling)
        {
            if (ModelState.IsValid)
            {
                db.tavlings.Add(tavling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tavling);
        }

        // GET: tavlings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tavling tavling = db.tavlings.Find(id);
            if (tavling == null)
            {
                return HttpNotFound();
            }
            return View(tavling);
        }

        // POST: tavlings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,tavlingsNamn,tavlingsDatum,forstaStarttid,sistaAnmalning,tavlingsForm,publicerad")] tavling tavling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tavling).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tavling);
        }

        // GET: tavlings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tavling tavling = db.tavlings.Find(id);
            if (tavling == null)
            {
                return HttpNotFound();
            }
            return View(tavling);
        }

        // POST: tavlings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tavling tavling = db.tavlings.Find(id);
            db.tavlings.Remove(tavling);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Anmälning till tävling
        public ActionResult Anmälan(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error");
            }
            else
            {
                // Hämta den inloggades uppgifter
                //Lägg till i relationstabellen mellan medlem och tävling
                // Räkna in +1 i den aktuella tävlingen



            }


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
