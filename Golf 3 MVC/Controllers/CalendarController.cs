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

namespace Golf_3_MVC.Controllers
{
    public class CalendarController : Controller
    {
        dsu3Entities ds = new dsu3Entities();


        public medbokare L�ggTillMedbokare(medbokare medbokare, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);

                var changedEvent = (bokning)DHXEventsHelper.Bind(typeof(bokning), actionValues);

                medbokare.BokningsId = 33;
                medbokare.Huvudbokare = User.Identity.GetUserName();
                medbokare.Medbokare1 = changedEvent.text;
                medbokare.BokningsId = changedEvent.id;

            return medbokare;

            
        }

        
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


        public ActionResult Create(FormCollection actionValues, string searchString)
        {
            medbokare medbokare = new medbokare();
            List<medbokare> aktuellaMedbokare = new List<medbokare>();
            CalendarBookings model = new CalendarBookings();
            medlemmar medlem = new medlemmar();
            string id = actionValues["Bokningar"];
            aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();
            model.aktuellaMedbokare = aktuellaMedbokare;

            
            if (aktuellaMedbokare.Count >= 4)
            {
                TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";

                foreach (medbokare mb in aktuellaMedbokare)
                {
                    medlemmar m = new medlemmar();
                    medbokare a = new medbokare();
                    List<medlemmar> allaMedlemmar = new List<medlemmar>();
                    allaMedlemmar = ds.medlemmars.ToList();

                    m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1).FirstOrDefault();

                    //int hcp;

                    //hcp = Convert.ToInt32(m.hcp); /*m.hcp.ToString().Where(x => m.golf_id == mb.Medbokare1).FirstOrDefault();*/



                }
            }
            else
            {
            medbokare.Id = 33;
            medbokare.BokningsId = Convert.ToInt32(id);
            medbokare.Huvudbokare = User.Identity.GetUserName();
            medbokare.Medbokare1 = searchString;
            ds.medbokares.Add(medbokare);
            ds.SaveChanges();
            }



            //return View();
            return RedirectToAction("index");
            //return (ContentResult)new AjaxSaveResponse(action);
        }


    public ActionResult Index()
        {
            season season = new season();   
            var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();

            if (data.seasontoggle == false)  // KOLLAR OM S�SONGEN �R AKTIV
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

                foreach (bokning b in allaBokningar) // H�MTAR OCH URSKILJER BLOCKTIME-BOKNINGAR UR BOKNINGAR
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
        public JsonResult S�k(string Prefix)
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


                        if (diff.TotalHours > 0.17) // om det �r mer �n 10min
                        {//BLOCKTIME

                            if (User.IsInRole("Personal") || User.IsInRole("Admin"))
                            {// ENDAST F�R PERSONAL OCH ADMIN

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
                            {// OM MEDLEM BOKAR MER �N 10 MINUTER

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

    }
}