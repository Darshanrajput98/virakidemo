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
    
    public partial class Resignation_Mst
    {
        public long ResignationID { get; set; }
        public Nullable<long> EmployeeCode { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public Nullable<System.DateTime> DateOfLeaving { get; set; }
        public Nullable<System.DateTime> DateOfApplication { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> ApprovalBy { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
