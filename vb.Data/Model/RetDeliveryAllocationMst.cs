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
    
    public partial class RetDeliveryAllocationMst
    {
        public long DeliveryAllocationID { get; set; }
        public long VehicleNo { get; set; }
        public System.DateTime AssignedDate { get; set; }
        public string TempoNo { get; set; }
        public Nullable<long> DeliveryPerson1 { get; set; }
        public Nullable<long> DeliveryPerson2 { get; set; }
        public Nullable<long> DeliveryPerson3 { get; set; }
        public Nullable<long> DeliveryPerson4 { get; set; }
        public string AreaID { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
