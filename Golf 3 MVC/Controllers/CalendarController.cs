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

        public ActionResult GetAutoCompleteDataBokning(string term)
        {
            var result = ds.boknings.Where(x => x.text.Contains(term))
                .Select(s => new BokningarAutoComplete { value = s.text, text = s.start_date + " " + s.text + " ID: " + s.id })
                .Union(ds.boknings.Where(x => x.start_date.ToString().Contains(term))
                .Select(s => new BokningarAutoComplete { value = s.start_date.ToString(), text = s.start_date + " " + s.text + " ID: " + s.id })
                .Union(ds.boknings.Where(x => x.id.ToString().Contains(term))
                .Select(s => new BokningarAutoComplete { value = s.id.ToString(), text = s.start_date + " " + s.text + " ID: " + s.id }))).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(FormCollection actionValues, string medlemsId, string sokBokning)
        {

            string bokningsID = sokBokning.Split(' ').Last();
            string golfID = medlemsId.Split(' ').Last();
            medbokare medbokare = new medbokare();
            List<medbokare> aktuellaMedbokare = new List<medbokare>();
            CalendarBookings model = new CalendarBookings();
            medlemmar aktuellMedlem = new medlemmar();
            medlemmar huvudbokare = new medlemmar();
            string id = actionValues["Bokningar"];
            aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsID).ToList();
            model.aktuellaMedbokare = aktuellaMedbokare;
            List<medlemmar> allaMedlemmar = new List<medlemmar>();
            allaMedlemmar = ds.medlemmars.ToList();
            

            
            if (Request.Form["laggtill"] != null)
            {
                if (aktuellaMedbokare.Count >= 3) // KONTROLL OM BOKNINGEN INNEHÅLLER 4 (inkl. huvudbokare) PERSONER ELLER FLER
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

                        bokning hej;
                        hej = ds.boknings.Where(x => x.id.ToString() == bokningsID).FirstOrDefault();

            medbokare.Id = 33;
            medbokare.BokningsId = Convert.ToInt32(bokningsID);
            medbokare.Huvudbokare = hej.golf_id;
            medbokare.Medbokare1 = golfID;
            ds.medbokares.Add(medbokare);
            ds.SaveChanges();
            }
                }
            }
            else if (Request.Form["tabort"] != null) // TAR BORT EN MEDBOKARE FRÅN EN BOKNING
            {

                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsID).ToList();

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

        public ActionResult SkrivUtScoreKort(string medlemsId, string sokBokning)
        {
            string bokningsID = sokBokning.Split(' ').Last();
            string golfID = medlemsId.Split(' ').Last();
            return RedirectToAction("scorekort", "scorekortsController", new { bokningsID, golfID });
        }


        public ActionResult CreateMedlem(FormCollection actionValues, string golfidstring, IEnumerable<bool> checkbox)
        {
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

            if (checkbox != null && checkbox.Count() == 2)
            {
                TempData["msg"] = "<script>alert('HEJ');</script>";
            }

            if (Request.Form["laggtill"] != null)
            {
                if (id == null)
                {
                    TempData["msg"] = "<script>alert('Du måste välja en bokning');</script>";
                    
                }
                
                if (aktuellaMedbokare.Count >= 3) // KONTROLL OM BOKNINGEN INNEHÅLLER 4 (inkl. huvudbokare) PERSONER ELLER FLER
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
                        aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == golfidstring).FirstOrDefault();

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
                        TempData["msg"] = "<script>alert('Bokningen går ej att göra då det totala handikappet är över 100');</script>";
                    }
                    else // OM ALLT OK; LÄGGER TILL PERSON
                    {


                        medbokare.Id = 33;
                        medbokare.BokningsId = Convert.ToInt32(id);
                        medbokare.Huvudbokare = User.Identity.GetUserName();
                        medbokare.Medbokare1 = golfidstring;
                        ds.medbokares.Add(medbokare);
                        ds.SaveChanges();


                        foreach (medbokare mb in aktuellaMedbokare)
                        {
                            medlemmar m;

                            m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1.Trim()).FirstOrDefault();
                            string epost = m.epost;
                            SendEmail(epost, "Bokning", "En spelare har bokat sig på samma tid som dig!");

                        }

                        TempData["msg"] = "<script>alert('Spelaren är nu tillagd');</script>";

                    }
                }
            }
            else if (Request.Form["tabort"] != null) // TAR BORT EN MEDBOKARE FRÅN EN BOKNING
            {
                if (id == null)
                {
                    TempData["msg"] = "<script>alert('Du måste välja en bokning');</script>";
                    goto Foo;
                }

                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();

                foreach (medbokare mb in aktuellaMedbokare)
                {
                    if (mb.Medbokare1.Trim() == golfidstring)
                    {
                        ds.medbokares.Remove(mb);

                    }
                }
                TempData["msg"] = "<script>alert('Spelaren är nu borttagen');</script>";
                ds.SaveChanges();
                foreach (medbokare mb in aktuellaMedbokare)
                {
                    medlemmar m;

                    m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1.Trim()).FirstOrDefault();
                    string epost = m.epost;
                    SendEmail(epost, "Avbokning", "En spelare har avbokat sig från samma tid som du är inbokad på!");

                }
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
                                bokning EV = new bokning();
                                EV.id = changedEvent.id;
                                EV.start_date = changedEvent.start_date;
                                EV.end_date = changedEvent.end_date;
                                EV.text = changedEvent.text;
                                EV.golf_id = User.Identity.GetUserName();
                                EV.blocktime = true;
                                ds.boknings.Add(EV);
                                ds.SaveChanges();
                                BlockTimeDelete(EV.id,EV.start_date, EV.end_date);

                                SendEmail("conradsson1993@hotmail.com", "Din tid har avbokats!", "På grund av yttre omständigheter måste banan vara stängd under denna tid!");
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
                            SendEmail("conradsson1993@hotmail.com", "Bokning", "En spelare har bokat sig på samma tid som dig!");
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
                        SendEmail("conradsson1993@hotmail.com", "Avbokning", "En spelare har avbokat sig på samma tid som dig!");
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
        public void BlockTimeDelete(int id,DateTime start, DateTime stop)
        {
            dsu3Entities ds3 = new dsu3Entities();
            string golf_id = User.Identity.GetUserName();

            foreach (var i in ds.boknings)
            {
                if (i.start_date > start && i.end_date < stop)
                {
                    //foreach (var x in ds3.medbokares)
                    //{
                    //    if (i.id == x.BokningsId)
                    //    {
                    //        ds3.medbokares.Remove(x);
                    //        ds3.SaveChanges();
                    //    }
                    //}
                    foreach (var x in ds3.medbokares)
                    {
                        if (x.BokningsId == id && x.Huvudbokare == golf_id)
                        {
                            ds3.medbokares.Remove(x);
                        }
                        else if (x.BokningsId == id && x.Medbokare1 == golf_id)
                        {
                            ds3.medbokares.Remove(x);
                        }
                    }

                }
                ds3.SaveChanges();
                var details = ds.boknings.Where(x => x.id == id && x.golf_id == golf_id).FirstOrDefault();

                ds3.boknings.Remove(details);
                ds3.SaveChanges();

                //ds3.boknings.Remove(i);
                //ds3.SaveChanges();               
            }
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
        
        public static void SendEmail(string toAddress, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            
            var smtpClient = new SmtpClient { EnableSsl = true };
            smtpClient.Send(mailMessage);
        }
    }


}