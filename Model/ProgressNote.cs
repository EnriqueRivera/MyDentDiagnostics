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
    
    public partial class ProgressNote
    {
        public ProgressNote()
        {
            this.ProgressNoteDetails = new HashSet<ProgressNoteDetail>();
        }
    
        public int ProgressNoteId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int PatientId { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ProgressNoteDetail> ProgressNoteDetails { get; set; }
    }
}
