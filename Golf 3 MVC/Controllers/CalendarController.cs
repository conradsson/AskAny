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

            sched.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);

            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            return View(sched);
        }

        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                new  dsu3Entities().boknings
                    .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.golf_id })
                )
            );
        }
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            bokning changedEvent = DHXEventsHelper.Bind<bokning>(actionValues);
            changedEvent.id = 131;
            changedEvent.golf_id = "12319_182";

           using (dsu3Entities entities = new dsu3Entities()) 
                {

            try
            {
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

            }
            return (new AjaxSaveResponse(action));


        }


    }
}