﻿using System;
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
        public IEnumerable<bokning> allaBokningar { get; set; }
        public IEnumerable<bokning> allaBlocktimeBokningar { get; set; }
        public IEnumerable<medbokare> aktuellaMedbokare { get; set; }
        public DHXScheduler sched { get; set; }
        public string säsongsknapp { get; set; }
        public List<medbokare> medbokareLista { get; set; }
        public virtual List<medlemmar> medlemsLista { get; set; }
    }


}