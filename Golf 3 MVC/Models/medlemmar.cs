//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Golf_3_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class medlemmar
    {
        public int id { get; set; }
        public string fornamn { get; set; }
        public string efternamn { get; set; }
        public string adress { get; set; }
        public string postnummer { get; set; }
        public string ort { get; set; }
        public string epost { get; set; }
        public string kon { get; set; }
        public Nullable<double> hcp { get; set; }
        public string golf_id { get; set; }
        public string kategori { get; set; }
        public string telefonnr { get; set; }
        public Nullable<bool> betalstatus { get; set; }
    
        public virtual medlemskategori medlemskategori { get; set; }
    }
}
