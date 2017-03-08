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

namespace Golf_3_MVC.Controllers
{
    public class CalendarController : Controller
    {
        dsu3Entities ds = new dsu3Entities();


        public ActionResult Create(FormCollection actionValues, string searchString)
        {
            //var action = new DataAction(actionValues);
            //var changedEvent = (bokning)DHXEventsHelper.Bind(typeof(bokning), actionValues);
            //bokningstid bokningstid = new bokningstid();
            ////bokning bokning = ds.boknings.Where(x => x.id == 1).FirstOrDefault();
            medbokare medbokare = new medbokare();
            //bokning bokning = new bokning();

            medbokare.Id = 33;
            medbokare.BokningsId = 87;
            medbokare.Huvudbokare = User.Identity.GetUserName();
            medbokare.Medbokare1 = searchString;
            //medbokare.bokning = ds.boknings.Where(x => x.id == 87).FirstOrDefault();
            ds.medbokares.Add(medbokare);
            ds.SaveChanges();

            //return View();
            return RedirectToAction("index");
            //return (ContentResult)new AjaxSaveResponse(action);
        }


    public ActionResult Index()
        {

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

            var check = new LightboxText("Highlighting", "L�gg till person");
            sched.Lightbox.Add(check);

           // sched.Config.buttons_left =["dhx_save_btn", "dhx_cancel_btn", "locate_button"];

            //sched.Config.buttons_right.Add(new EventButton
            //{
                
            //    Label = "L�gg till medlem",
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


            return View(sched);

        }

        [HttpPost]
        public ActionResult Blockinterval(string blockfrom, string blockto)
        {

            // H�mta v�rdena fr�n blockfrom och blockto
            // Kontrollera att v�rdena har r�tt format. (�r(xxxx),m�nad(x),dag(x))
            // Skicka det nya v�rdena till DB och boknings tabellen
            // 
            // Uppdatera vyn med att returnera till index

            //sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR TIDER IFR�N TEXTBOXARNA
            //{
            //    StartDate = DateTime.Parse(blockfrom),
            //    EndDate = DateTime.Parse(blockto)
            //});

            return RedirectToAction("index");

        }

        public ActionResult Seasontoggle()
        {
            // H�mta v�rdet fr�n seasontoggle
            // Kontrollera att v�rdet �r antingen True eller False
            // Skicka det nya v�rdet till DB och season tabellen i seasontoggle kolumnen p� knapptrycket seasontogglebtn
            // Uppdatera vyn med att returnera till index




            //if(seasonon == "true" || seasonon == "false" || seasonoff == "true" || seasonoff == "false")
            //{

            //}
            //else
            //{

            //}
            



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