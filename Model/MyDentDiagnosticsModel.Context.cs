﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyDentDiagnosticsDBEntities : DbContext
    {
        public MyDentDiagnosticsDBEntities()
            : base("name=MyDentDiagnosticsDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Diagnostic> Diagnostics { get; set; }
        public virtual DbSet<InitialDentalNote> InitialDentalNotes { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<ProgressNote> ProgressNotes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ProgressNoteDetail> ProgressNoteDetails { get; set; }
    }
}
