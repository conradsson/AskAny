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


        public medbokare LäggTillMedbokare(medbokare medbokare, FormCollection actionValues)
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


        public ActionResult Create(FormCollection actionValues, string searchString, bokning Bokningar)
        {
            medbokare medbokare = new medbokare();
            CalendarBookings model = new CalendarBookings();
            bokning bokning = new bokning();



            var valtVarde = actionValues.GetValue("Bokningar");

            medbokare.Id = 33;
            medbokare.BokningsId = Bokningar.id;
            medbokare.Huvudbokare = User.Identity.GetUserName();
            medbokare.Medbokare1 = searchString;
            ds.medbokares.Add(medbokare);
            ds.SaveChanges();

            //return View();
            return RedirectToAction("index");
            //return (ContentResult)new AjaxSaveResponse(action);
        }


    public ActionResult Index()
        {
            season season = new season();

            var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();
            


            if (data.seasontoggle == false)
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

                allaMedlemmar = ds.medlemmars.ToList();
                aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();
                allaBokningar = ds.boknings.ToList();
                model.minaBokningar = (IEnumerable<bokning>)allaBokningar.Where(x => x.golf_id == User.Identity.GetUserName()).ToList();

                var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Flat;


            sched.Config.first_hour = 8;
            sched.Config.last_hour = 21;
            sched.Config.time_step = 10;

            sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR ALLT INNAN NU
            {
                StartDate = new DateTime(2000, 1, 1),
                EndDate = DateTime.Now
            });

            var check = new LightboxText("Highlighting", "Lägg till person");
            sched.Lightbox.Add(check);

           // sched.Config.buttons_left =["dhx_save_btn", "dhx_cancel_btn", "locate_button"];

            //sched.Config.buttons_right.Add(new EventButton
            //{
                
            //    Label = "Lägg till medlem",
            //    OnClick = "some_function",
            //    Name = "location"
                
            //});

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

        
        public ActionResult Blockinterval(string blockfrom, string blockto)
        {
            // Hämta värdena från blockfrom och blockto
            // Kontrollera att värdena har rätt format. (år(xxxx),månad(x),dag(x))
            // Skicka det nya värdena till DB och boknings tabellen
            // 
            // Uppdatera vyn med att returnera till index


            //var sched = new DHXScheduler(this);

            //sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR TIDER IFRÅN TEXTBOXARNA
            //{
            //    StartDate = DateTime.Parse(blockfrom),
            //    EndDate = DateTime.Parse(blockto)
            //});

            return RedirectToAction("index");

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
                    case DataActionTypes.Insert: // "insert new data"
                        bokning EV = new bokning();
                        EV.id = changedEvent.id;
                        EV.start_date = changedEvent.start_date;
                        EV.end_date = changedEvent.end_date;
                        EV.text = changedEvent.text;
                        EV.golf_id = User.Identity.GetUserName();
                        ds.boknings.Add(EV);
                        ds.SaveChanges();


                        break;
                    case DataActionTypes.Delete: // "delete chosen data"

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