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

            var check = new LightboxText("Highlighting", "Lägg till person");
            sched.Lightbox.Add(check);

            

            //sched.Config.buttons_left =["dhx_save_btn", "dhx_cancel_btn", "locate_button"];

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


            return View(sched);

        }

        [HttpPost]
        public ActionResult Blockinterval(string blockfrom, string blockto)
        {


            //sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR TIDER IFRÅN TEXTBOXARNA
            //{
            //    StartDate = DateTime.Parse(blockfrom),
            //    EndDate = DateTime.Parse(blockto)
            //});

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
                        var details = ds.boknings.Where(x => x.id == id).FirstOrDefault();
                        ds.boknings.Remove(details);
                        ds.SaveChanges();
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