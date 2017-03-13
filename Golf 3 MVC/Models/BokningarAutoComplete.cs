using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf_3_MVC.Models
{
    public class BokningarAutoComplete
    {
        public DateTime start_date { get; set; }

        public int id { get; set; }
        public string text { get; set; }
        public string value { get; set; }
    }
}