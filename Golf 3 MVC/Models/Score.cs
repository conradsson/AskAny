using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf_3_MVC.Models
{
    public class Score
    {
        public List<scorekort> scoreKort { get; set; }
        public List<slope> slope { get; set; }
        public List<medlemmar> medlems { get; set; }
        public List<bokning> bokning { get; set; }
        public List<medbokare> medbokare { get; set; }
        public int bokningsID { get; set; }
        public string golfID { get; set; }
        public string golfID2 { get; set; }
        public string golfID3 { get; set; }
        public string golfID4 { get; set; }



    }
}