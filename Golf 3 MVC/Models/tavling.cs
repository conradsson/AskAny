using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Golf_3_MVC.Models
{
    public class tavling
    {
        public int Id { get; set; }
        public string tavlingsNamn { get; set; }
        //[DataType(DataType.Date)]
        public DateTime tavlingsDatum { get; set; }
        public TimeSpan forstaStarttid { get; set; }
        //[DataType(DataType.Date)]
        public DateTime sistaAnmalning { get; set; }
        public string tavlingsForm { get; set; }
        public bool publicerad { get; set; }
	}
}