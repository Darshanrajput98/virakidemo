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
    
    public partial class ColdStorage_InwardQty_Mst
    {
        public long InwardQtyID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> InwardID { get; set; }
        public string Notes { get; set; }
        public string HSNNumber { get; set; }
        public Nullable<int> NoofBags { get; set; }
        public Nullable<decimal> WeightPerBag { get; set; }
        public Nullable<decimal> TotalWeight { get; set; }
        public Nullable<decimal> RatePerKG { get; set; }
        public Nullable<decimal> RentPerBags { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}