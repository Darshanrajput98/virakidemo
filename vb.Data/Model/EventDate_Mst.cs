//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vb.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventDate_Mst
    {
        public long EventDateID { get; set; }
        public Nullable<long> EventID { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public bool IsDelete { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
    }
}