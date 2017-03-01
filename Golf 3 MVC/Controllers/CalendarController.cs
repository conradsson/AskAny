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
        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Glossy;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = new DateTime(2013, 5, 5);
            return View(sched);
        }

        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                new  dsu3Entities().boknings
                    .Select(e => new { /*e.id, */e.text, e.start_date, e.end_date/*, e.golf_id*/ })
                )
            );
        }
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<bokning>(actionValues);
            var entities = new dsu3Entities();
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        entities.boknings.Add(changedEvent);
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