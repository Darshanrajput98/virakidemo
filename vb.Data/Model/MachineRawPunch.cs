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
    
    public partial class MachineRawPunch
    {
        public long MachineRawPunchID { get; set; }
        public Nullable<System.DateTime> ADate { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Shift { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string WorkDuration { get; set; }
        public string OT { get; set; }
        public string TotalDuration { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string PAYCODE { get; set; }
        public Nullable<System.DateTime> OFFICEPUNCH { get; set; }
    }
}
