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
    
    public partial class OnlineOrderQty_Mst
    {
        public long OnlineOrderQtyID { get; set; }
        public Nullable<long> OnlineOrderID { get; set; }
        public long ProductID { get; set; }
        public Nullable<long> OnlineProductQtyID { get; set; }
        public Nullable<decimal> OnlineProductPrice { get; set; }
        public Nullable<long> OnlineOrderQty { get; set; }
        public Nullable<decimal> OnlineTotalAmount { get; set; }
        public Nullable<decimal> OnlineGrandAmount { get; set; }
        public Nullable<bool> IsConfirm { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
