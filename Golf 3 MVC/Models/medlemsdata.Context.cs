﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dsu3Entities : DbContext
    {
        public dsu3Entities()
            : base("name=dsu3Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<medlemskategori> medlemskategoris { get; set; }
        public virtual DbSet<starttid> starttids { get; set; }
        public virtual DbSet<medlemmar> medlemmars { get; set; }
        public virtual DbSet<season> seasons { get; set; }
        public virtual DbSet<bokning> boknings { get; set; }
        public virtual DbSet<scorekort> scorekorts { get; set; }
        public virtual DbSet<slope> slopes { get; set; }
        public virtual DbSet<medbokare> medbokares { get; set; }
        public virtual DbSet<tavling> tavlings { get; set; }
    }
}
