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
    
    public partial class ProgressNoteDetail
    {
        public int ProgressNoteDetailId { get; set; }
        public int ProgressNoteId { get; set; }
        public string DetailName { get; set; }
        public string DetailDescription { get; set; }
    
        public virtual ProgressNote ProgressNote { get; set; }
    }
}