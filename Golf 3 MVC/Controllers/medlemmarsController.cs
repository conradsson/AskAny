using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using Golf_3_MVC.Models;


namespace Golf_3_MVC.Controllers
{
    public class medlemmarsController : Controller
    {
        private dsu3Entities db = new dsu3Entities();

        public ActionResult Index(int? page, string sortOrder, string searchString)
        {
            ViewBag.EfternamnSortParm = String.IsNullOrEmpty(sortOrder) ? "efternamn_desc" : "";

            var medlemmars = db.medlemmars.Include(m => m.medlemskategori);

            if (!String.IsNullOrEmpty(searchString))
            {
                medlemmars = medlemmars.Where(m => m.fornamn.Contains(searchString) ||
                m.efternamn.Contains(searchString) || m.golf_id.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "efternamn_desc":
                    medlemmars = medlemmars.OrderByDescending(m => m.efternamn);
                    break;
                default:
                    medlemmars = medlemmars.OrderBy(m => m.efternamn);
                    break;
            }
                        
            return View(medlemmars.ToList().ToPagedList(page ?? 1, 16));
        }

        // GET: medlemmars
        //public ActionResult Index()
        //{
        //    var medlemmars = db.medlemmars.Include(m => m.medlemskategori);
        //    return View(medlemmars.ToList());
        //}


        // GET: medlemmars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medlemmar medlemmar = db.medlemmars.Find(id);
            if (medlemmar == null)
            {
                return HttpNotFound();
            }
            return View(medlemmar);
        }

        // GET: medlemmars/Create
        public ActionResult Create()
        {
            ViewBag.kategori = new SelectList(db.medlemskategoris, "namn", "namn");
            return View();
        }

        // POST: medlemmars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fornamn,efternamn,adress,postnummer,ort,epost,kon,hcp,golf_id,kategori,telefonnr,betalstatus")] medlemmar medlemmar)
        {
            if (ModelState.IsValid)
            {
                db.medlemmars.Add(medlemmar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kategori = new SelectList(db.medlemskategoris, "namn", "namn", medlemmar.kategori);
            return View(medlemmar);
        }

        // GET: medlemmars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medlemmar medlemmar = db.medlemmars.Find(id);
            if (medlemmar == null)
            {
                return HttpNotFound();
            }
            ViewBag.kategori = new SelectList(db.medlemskategoris, "namn", "namn", medlemmar.kategori);
            return View(medlemmar);
        }

        // POST: medlemmars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fornamn,efternamn,adress,postnummer,ort,epost,kon,hcp,golf_id,kategori,telefonnr,betalstatus")] medlemmar medlemmar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medlemmar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kategori = new SelectList(db.medlemskategoris, "namn", "namn", medlemmar.kategori);
            return View(medlemmar);
        }

        // GET: medlemmars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medlemmar medlemmar = db.medlemmars.Find(id);
            if (medlemmar == null)
            {
                return HttpNotFound();
            }
            return View(medlemmar);
        }

        // POST: medlemmars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            medlemmar medlemmar = db.medlemmars.Find(id);
            db.medlemmars.Remove(medlemmar);
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
