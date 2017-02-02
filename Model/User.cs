//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Areas = new HashSet<Area>();
            this.Diagnostics = new HashSet<Diagnostic>();
            this.Patients = new HashSet<Patient>();
            this.ProgressNotes = new HashSet<ProgressNote>();
        }
    
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Diagnostic> Diagnostics { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<ProgressNote> ProgressNotes { get; set; }
    }
}