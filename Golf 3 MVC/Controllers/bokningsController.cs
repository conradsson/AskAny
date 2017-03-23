using System.Net;
using System.Data.Entity;
using System.Linq;
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
            return View(db.boknings.ToList());
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
            return View();
        }

        // POST: boknings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,text,start_date,end_date,golf_id")] bokning bokning)
        {
            if (ModelState.IsValid)
            {
                db.boknings.Add(bokning);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            return View(bokning);
        }

        // POST: boknings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,text,start_date,end_date,golf_id")] bokning bokning)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bokning).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
