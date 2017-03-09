using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;

namespace Golf_3_MVC.Models
{
    public class CalendarBookings
    {
        public IEnumerable<bokning> minaBokningar { get; set; }

        public DHXScheduler sched { get; set; }

        public List<medlemmar> medlems { get; set; }

    }


}