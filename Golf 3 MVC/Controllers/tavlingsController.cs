using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Golf_3_MVC.Models;
using Microsoft.AspNet.Identity;

namespace Golf_3_MVC.Controllers
{
    public class tavlingsController : Controller
    {
        private dsu3Entities db = new dsu3Entities();
        private static Random random;
        private string aktuellTavling;

        public ActionResult LaggTillTavlare(int id)
        {
            string golfID = User.Identity.GetUserName();
            tavlare nyTavlare = new tavlare();
            List<tavlare> allaTavlare = db.tavlares.ToList();

            foreach (tavlare tavlare in allaTavlare)
            {
                if (tavlare.TävlareGolf_ID == golfID && tavlare.TävlingsId == id)
                {
                    TempData["msg"] = "<script>alert('Du är redan anmäld till denna tävling');</script>";
                    goto Foo;
                }
            }

            nyTavlare.TävlingsId = id;
            nyTavlare.TävlareGolf_ID = golfID;
            db.tavlares.Add(nyTavlare);
            db.SaveChanges();

            TempData["msg"] = "<script>alert('Du är nu anmäld till tävlingen');</script>";

            Foo:
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Aktuell användares tävlingar!
        /// </summary>
        /// <returns></returns>
        public PartialViewResult MinaTavlingar()
        {
            string golfID = User.Identity.GetUserName();
            tavlare nyTavlare = new tavlare();
            List<tavlare> allaTavlare = db.tavlares.ToList();
            List<tavling> allaTavlingar = db.tavlings.ToList();
            List<tavling> minaTavlingar = new List<tavling>();

            foreach (tavlare tavlare in allaTavlare)
            {
                if (tavlare.TävlareGolf_ID == golfID)
                {
                    tavling nyTavling = allaTavlingar.Where(x => x.Id == tavlare.TävlingsId).FirstOrDefault();
                    minaTavlingar.Add(nyTavling);
                }
            }
            //return RedirectToAction("Index");
            return PartialView("_minaanmalningar",minaTavlingar);
        }

        public ActionResult Avanmalan(int? id)
        {
            int? aktuelltavling = id;
            string golfID = User.Identity.GetUserName();

            if (aktuelltavling == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            tavlare anmalningen = db.tavlares.Where(x => x.TävlingsId == aktuelltavling && x.TävlareGolf_ID == golfID).FirstOrDefault();
            if (anmalningen == null)
            {
                return HttpNotFound();
            }


            db.tavlares.Remove(anmalningen);
            db.SaveChanges();

            TempData["msg"] = "<script>alert('Avanmälningen lyckades');</script>";

            return RedirectToAction("Index");
        }

        public PartialViewResult Aktuelltavling(string id)
        {
            tavling tavling = db.tavlings.Find(Convert.ToInt32(id));
            this.aktuellTavling = id;

            return PartialView("_aktuelltavling", tavling);
        }


        public ActionResult LäggTillMedlemPersonal(FormCollection actionValues, string sokmedlem)
        {
            string golfID = sokmedlem.Split(' ').Last();
            tavlare nyTavalre = new tavlare();
            string id = actionValues["Tavlingar"];


            if (true) // KONTROLL OM MEDLEMMEN REDAN ÄR MED I TÄVLINGEN
            {

            }

            nyTavalre.TävlareGolf_ID = golfID;
            nyTavalre.TävlingsId = Convert.ToInt32(id);
            db.tavlares.Add(nyTavalre);
            db.SaveChanges();


            TempData["msg"] = "<script>alert('Medlemmen är nu tillagd i tävlingen');</script>";

            return RedirectToAction("Index");
        }

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

        /// <summary>
        /// Metod för lottning.
        /// </summary>
        public void Randomizer()
        {
            random = new Random();

             // Använd senare för att slumpa ur 3 personer bland alla deltagare. Gör om till lista
            // Random r = new Random();
           //  IEnumerable<int> threeRandom = myValues.OrderBy(x => r.Next()).Take(3);

        }
    }
}
