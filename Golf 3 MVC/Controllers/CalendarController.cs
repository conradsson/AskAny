using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf_3_MVC.Controllers;
using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;
using Golf_3_MVC.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Postal;
using System.IO;
using System.Net;
using System.Net.Mail;


namespace Golf_3_MVC.Controllers
{
    public class CalendarController : Controller
    {
        dsu3Entities ds = new dsu3Entities();
        double totalHcp = 0;
        double hHcp;
        double mHcp;

        //public medbokare LäggTillMedbokare(medbokare medbokare, FormCollection actionValues)
        //{
        //    var action = new DataAction(actionValues);

        //        var changedEvent = (bokning)DHXEventsHelper.Bind(typeof(bokning), actionValues);

        //        medbokare.BokningsId = 33;
        //        medbokare.Huvudbokare = User.Identity.GetUserName();
        //        medbokare.Medbokare1 = changedEvent.text;
        //        medbokare.BokningsId = changedEvent.id;

        //    return medbokare;            
        //}

        
        //public ViewResult Index1()
        //{
        //    //Create db context object here 
        //    dsu3Entities db = new dsu3Entities();
        //    //Get the value from database and then set it to ViewBag to pass it View
        //    IEnumerable<SelectListItem> items = db.boknings.Select(c => new SelectListItem
        //    {
        //        Value = c.golf_id,
        //        Text = c.text

        //    });
        //    ViewBag.Bokningar = items;
        //    return View();
        //}


        //public ActionResult MinaBokningar()
        //{
        //    dsu3Entities db = new dsu3Entities();
        //    ViewBag.Bokningar = new SelectList(db.boknings, "golf_id", "text");
            

        //    return RedirectToAction("index");
        //}

        public ActionResult AutoComplete()
        {
            return View();
        }

        public ActionResult GetAutoCompleteData(string term)
        {

            var result = ds.medlemmars.Where(x => x.fornamn.Contains(term))
                .Select(s => new GolfareAutoComplete { value = s.fornamn, fornamn = s.fornamn + " " + s.efternamn + " " + s.golf_id})
                .Union(ds.medlemmars.Where(x => x.efternamn.Contains(term))
                .Select(s => new GolfareAutoComplete { value = s.efternamn, fornamn = s.fornamn + " " + s.efternamn + " " + s.golf_id })
                .Union(ds.medlemmars.Where(x => x.golf_id.Contains(term))
                .Select(s => new GolfareAutoComplete { value = s.golf_id, fornamn = s.fornamn + " " + s.efternamn + " " + s.golf_id }))).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(FormCollection actionValues, string medlemsId)
        {
            string golfID = medlemsId.Split(' ').Last();
            medbokare medbokare = new medbokare();
            List<medbokare> aktuellaMedbokare = new List<medbokare>();
            CalendarBookings model = new CalendarBookings();
            medlemmar aktuellMedlem = new medlemmar();
            medlemmar huvudbokare = new medlemmar();
            string id = actionValues["Bokningar"];
            aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();
            model.aktuellaMedbokare = aktuellaMedbokare;
            List<medlemmar> allaMedlemmar = new List<medlemmar>();
            allaMedlemmar = ds.medlemmars.ToList();

            
            if (Request.Form["laggtill"] != null)
            {
                if (aktuellaMedbokare.Count >= 3) // KONTROLL OM BOKNINGEN INNEHÅLLE 4 (inkl. huvudbokare) PERSONER ELLER FLER
                {
                TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";
                }
                else
                {
                    foreach (medbokare mb in aktuellaMedbokare) // LOOPAR IGENOM ALLA I BOKNINGEN O HÄMTAR HCP SAMT KONTROLL FÖR DUBBELBOKNING
                {
                    medlemmar m = new medlemmar();
                        double hcp;

                    m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1.Trim()).FirstOrDefault();
                        huvudbokare = allaMedlemmar.Where(x => x.golf_id == mb.Huvudbokare).FirstOrDefault();
                        aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == golfID).FirstOrDefault();

                        hcp = Convert.ToDouble(m.hcp);
                        mHcp = Convert.ToDouble(aktuellMedlem.hcp);
                        hHcp = Convert.ToDouble(huvudbokare.hcp); 

                        totalHcp += hcp;

                        if (aktuellMedlem == huvudbokare || aktuellMedlem == m)
                        {
                            TempData["msg"] = "<script>alert('Denna person finns redan med i bokningen');</script>";
                            goto Foo;
                        }
                }
                    totalHcp += hHcp;
                    totalHcp += mHcp;

                    if (totalHcp >= 100) // MAX 100 HANDIKAPP
                    {
                        TempData["msg"] = "<script>alert('Bokningen går ej att göra då det totala handikappet är över 120');</script>";
            }
                    else // OM ALLT OK; LÄGGER TILL PERSON
            {
            medbokare.Id = 33;
            medbokare.BokningsId = Convert.ToInt32(id);
            medbokare.Huvudbokare = User.Identity.GetUserName();
            medbokare.Medbokare1 = golfID;
            ds.medbokares.Add(medbokare);
            ds.SaveChanges();
            }
                }
            }
            else if (Request.Form["tabort"] != null) // TAR BORT EN MEDBOKARE FRÅN EN BOKNING
            {

                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();

                foreach (medbokare mb in aktuellaMedbokare)
                {
                    if (mb.Medbokare1.Trim() == golfID)
                    {
                        ds.medbokares.Remove(mb);

                    }
                }

                ds.SaveChanges();
            }

           
             Foo:
            return RedirectToAction("index");
        }

    public ActionResult Index()
        {
            season season = new season();   
            var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();

            if (data.seasontoggle == false)  // KOLLAR OM SÄSONGEN ÄR AKTIV
            {
                return View("offseason");
            }
            else
            {
                List<medlemmar> allaMedlemmar = new List<medlemmar>();
                medlemmar aktuellMedlem = new medlemmar();
                List<bokning> allaBokningar = new List<bokning>();
                List<bokning> minaBokningar = new List<bokning>();

                CalendarBookings model = new CalendarBookings();
                model.medlems = medlemmars;

                allaMedlemmar = ds.medlemmars.ToList();
                aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();
                allaBokningar = ds.boknings.ToList();
                model.minaBokningar = (IEnumerable<bokning>)allaBokningar.Where(x => x.golf_id == User.Identity.GetUserName()).ToList();


                var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Flat;


            sched.Config.first_hour = 8;
            sched.Config.last_hour = 21;
            sched.Config.time_step = 10;

                foreach (bokning b in allaBokningar) // HÄMTAR OCH URSKILJER BLOCKTIME-BOKNINGAR UR BOKNINGAR
                {
                    if (b.blocktime == true)
                    {
                        sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR ALLA BLOCKTIME-BOKNINGAR
                        {
                            StartDate = b.start_date,
                            EndDate = b.end_date
                        });
                    }
                }

                sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR ALLT INNAN NU
                {
                    StartDate = new DateTime(2000, 1, 1),
                    EndDate = DateTime.Now
                });


            sched.Lightbox.AddDefaults();

            sched.Config.start_on_monday = true;
            sched.InitialView = "day";
            sched.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);
            sched.Config.separate_short_events = true;
            sched.Config.hour_size_px = 84;

            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            model.sched = sched;
            return View(model);
            }

        }
            List<medlemmar> medlemmars = new List<medlemmar>();
        public JsonResult Sök(string Prefix)
        {
            medlemmars = ds.medlemmars.ToList();
            return Json(medlemmars, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Seasontoggle(string responsables, bool checkResp = false)
        {
            season season = new season();

            season.seasontoggle = checkResp;

            if (season.seasontoggle == true)
            {
                var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();
                data.seasontoggle = season.seasontoggle;
                ds.SaveChanges();
            }
            else 
            {
                var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();
                data.seasontoggle = season.seasontoggle;
                ds.SaveChanges();
            }

                return RedirectToAction("index");
        }

        public ContentResult Data()
        {
            try
            {
                var details = ds.boknings.ToList();
                return new SchedulerAjaxData(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                var changedEvent = (bokning)DHXEventsHelper.Bind(typeof(bokning), actionValues);

                switch (action.Type)
                {
                    case DataActionTypes.Insert:

                        var diff = changedEvent.end_date.TimeOfDay - changedEvent.start_date.TimeOfDay;


                        if (diff.TotalHours > 0.17) // om det är mer än 10min
                        {//BLOCKTIME

                            if (User.IsInRole("Personal") || User.IsInRole("Admin"))
                            {// ENDAST FÖR PERSONAL OCH ADMIN
                                BlockTimeDelete(changedEvent.start_date, changedEvent.end_date);
                                bokning EV = new bokning();
                                EV.id = changedEvent.id;
                                EV.start_date = changedEvent.start_date;
                                EV.end_date = changedEvent.end_date;
                                EV.text = changedEvent.text;
                                EV.golf_id = User.Identity.GetUserName();
                                EV.blocktime = true;
                                ds.boknings.Add(EV);
                                ds.SaveChanges();
                            }
                            else 
                            {// OM MEDLEM BOKAR MER ÄN 10 MINUTER

                                TempData["msg"] = "<script>alert('Du kan bara boka 10 minuter');</script>";

                            }

                        }
                        else
                        { // VANLIG BOKNING

                            bokning EV = new bokning();
                            EV.id = changedEvent.id;
                            EV.start_date = changedEvent.start_date;
                            EV.end_date = changedEvent.end_date;
                            EV.text = changedEvent.text;
                            EV.golf_id = User.Identity.GetUserName();
                            EV.blocktime = false;
                            ds.boknings.Add(EV);
                            ds.SaveChanges();
                        }

                        break;
                    case DataActionTypes.Delete:

                        if (User.IsInRole("User") == true)
                        {
                            string golf_id = User.Identity.GetUserName();

                            foreach (var x in ds.medbokares)
                            {
                                if (x.BokningsId == id && x.Huvudbokare == golf_id)
                                {
                                    ds.medbokares.Remove(x);
                                }
                                else if (x.BokningsId == id && x.Medbokare1 == golf_id)
                                {
                                    ds.medbokares.Remove(x);
                                }
                            }
                            ds.SaveChanges();
                            var details = ds.boknings.Where(x => x.id == id && x.golf_id == golf_id).FirstOrDefault();

                            ds.boknings.Remove(details);
                            ds.SaveChanges();
                        }

                        else
                        {
                            foreach (var x in ds.medbokares)
                            {
                                if (x.BokningsId == id)
                                {
                                    ds.medbokares.Remove(x);
                                }
                            }
                            ds.SaveChanges();
                            var details = ds.boknings.Where(x => x.id == id).FirstOrDefault();

                            ds.boknings.Remove(details);
                            ds.SaveChanges();
                        }

                        break;
                    default:// "update"
                        var data = ds.boknings.Where(x => x.id == id).FirstOrDefault();
                            data.start_date = changedEvent.start_date;
                            data.end_date = changedEvent.end_date;
                            data.text = changedEvent.text;
                            ds.SaveChanges();
                            break;
                        }

                }
            catch
            {
                action.Type = DataActionTypes.Error;
            }

            return (ContentResult)new AjaxSaveResponse(action);
        }
        public ActionResult BlockTimeDelete(DateTime start, DateTime stop)
        {
            foreach (var i in ds.boknings)
            {
                if (i.start_date > start && i.end_date < stop)
                {
                    ds.boknings.Remove(i);
                    ds.SaveChanges();
                    foreach (var x in ds.medbokares)
                    {
                        if (i.id == x.BokningsId)
                        {
                            ds.medbokares.Remove(x);
                            ds.SaveChanges();
                        }
                    }
                }
            }
            return View();
        }

        //public ActionResult MedbokareDelete(string golfid, int bokningsid)
        //{

        //    foreach (var i in ds.medbokares)
        //    {
        //        if (i.BokningsId == bokningsid && i.Medbokare1 == golfid)
        //        {
        //            ds.medbokares.Remove(i);
        //        }
        //    }
        //    ds.SaveChanges();

        //    return RedirectToAction("index");
        //}
        
            //Used to send email
      public ActionResult Send(string message)
        {
            bokning EV = new bokning();
            dynamic email = new Email("Example");
            email.To = EV.golf_id; // "conradsson1993@hotmail.com"; //Komma åt användarna på bokningens emails.
            email.Message = message;
            email.Send();
            
            return RedirectToAction("index");
        }
    }

}