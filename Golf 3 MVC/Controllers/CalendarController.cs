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

namespace Golf_3_MVC.Controllers
{
    public class CalendarController : Controller
    {
        dsu3Entities ds = new dsu3Entities();
        //bokning bok = new bokning()
        //{
        //    id = 33,
        //    text = "Nu fungerar det",
        //    start_date = DateTime.Now.AddHours(+1),
        //    end_date = DateTime.Now.AddHours(+2),
        //    golf_id = "12319_182"
        //};


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
            return (new SchedulerAjaxData(
                new dsu3Entities().boknings
                    .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.golf_id })
                )
            );


        }
        //////public ContentResult Data()
        //////{
        //////    try
        //////    {
        //////        var details = ds.boknings.ToList();
        //////        return new SchedulerAjaxData(details);

        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }

        //////}

        //////public ContentResult Save(int? id, FormCollection actionValues)

        //////{

        //////    var action = new DataAction(actionValues);

        //////    try
        //////    {
        //////        var changedEvent = (bokning)DHXEventsHelper.Bind(typeof(bokning),actionValues);

        //////        switch (action.Type)
        //////        {
        //////            case DataActionTypes.Insert:
        //////                bokning EV = new bokning();
        //////                EV.id = 110;
        //////                EV.start_date = changedEvent.start_date;
        //////                EV.end_date = changedEvent.end_date;
        //////                EV.text = changedEvent.text;
        //////                EV.golf_id = "12319_182";
        //////                ds.boknings.Add(EV);
        //////                ds.SaveChanges();
        //////                break;
        //////            case DataActionTypes.Delete:
        //////                var details = ds.boknings.Where(x => x.id == id).FirstOrDefault();
        //////                ds.boknings.Remove(details);
        //////                ds.SaveChanges();
        //////                break;
        //////            default:// "update"    
        //////                var data = ds.boknings.Where(x => x.id == id).FirstOrDefault();
        //////                data.start_date = changedEvent.start_date;
        //////                data.end_date = changedEvent.end_date;
        //////                data.text = changedEvent.text;
        //////                ds.SaveChanges();
        //////                break;
        //////        }
        //////    }
        //////    catch
        //////    {
        //////        action.Type = DataActionTypes.Error;
        //////    }
        //////    return (ContentResult)new AjaxSaveResponse(action);
        //////}
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<bokning>(actionValues);
            var entities = new dsu3Entities();

            try
            {
                //var changedEvent = DHXEventsHelper.Bind<bokning>(actionValues);
                //var entities = new dsu3Entities();

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        entities.boknings.Add(changedEvent);
                        //entities.boknings.Add(bok);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = entities.boknings.FirstOrDefault(ev => ev.id == action.SourceId);
                        entities.boknings.Remove(changedEvent);
                        break;
                    default:// "update"   
                        var target = entities.boknings.Single(e => e.id == changedEvent.id);
                        DHXEventsHelper.Update(target, changedEvent, new List<string> { "id" });
                        break;
                }

                entities.SaveChanges();
                action.TargetId = changedEvent.id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }


    }
}