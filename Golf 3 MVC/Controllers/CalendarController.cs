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

        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Flat;

            sched.Config.first_hour = 8;
            sched.Config.last_hour = 21;
            sched.InitialView = "day";

            sched.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);

            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            return View(sched);
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
                        bokning EV = new bokning();
                        EV.id = changedEvent.id;
                        EV.start_date = changedEvent.start_date;
                        EV.end_date = changedEvent.end_date;
                        EV.text = changedEvent.text;
                        //EV.golf_id = "12319_182";
                        EV.golf_id = User.Identity.GetUserName();
                        ds.boknings.Add(EV);
                        ds.SaveChanges();
                        break;
                    case DataActionTypes.Delete:
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