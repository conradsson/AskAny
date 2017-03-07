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

          //  var timeline = new TimelineView("timeline", "golf_id");//initializes the view
          //  //timeline.RenderMode = TimelineView.RenderModes.Bar;
          //  timeline.FitEvents = false;
          //  timeline.X_Unit = TimelineView.XScaleUnits.Minute;
          //  timeline.X_Step = 10;
          //  timeline.X_Size = 6;  // (8PM - 8AM)/30min
          //  //timeline.X_Start = 50; // 8AM/30min
          //  timeline.X_Length = 48; // 24/30min
          //  sched.Views.Add(timeline);//adds the view to the scheduler
          ////timeline.AddOptions(ds.boknings);//
          //  var banor = new List<object>(){
          //      new { key = "1", label = "08.00"},
          //      new { key = "2", label = "09.00"},
          //      new { key = "3", label = "10.00"},
          //      new { key = "4", label = "11.00"},
          //      new { key = "5", label = "12.00"},
          //      new { key = "6", label = "13.00"},
          //      new { key = "1", label = "14.00"},
          //      new { key = "2", label = "15.00"},
          //      new { key = "3", label = "16.00"},
          //      new { key = "4", label = "17.00"},
          //      new { key = "5", label = "18.00"},
          //      new { key = "6", label = "19.00"},
          //      new { key = "6", label = "20.00"}
          //  };

          //  timeline.AddOptions(banor);

            sched.Config.first_hour = 8;
            sched.Config.last_hour = 21;
            sched.Config.time_step = 10;

            //var nu = Convert.ToInt64(DateTime.Now.ToShortDateString());

            //sched.TimeSpans.Add(new DHXBlockTime()   // BLOCKAR ALLT BAKOM NU
            //{
            //    StartDate = new DateTime(2000, 1, 1),
            //    EndDate = new DateTime(nu)
            //});

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
            //ViewBag.Bokningar = new SelectList(ds.boknings, "id", "golf_id");

            return View();
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