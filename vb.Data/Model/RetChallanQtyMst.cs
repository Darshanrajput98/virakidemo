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
    
    public partial class RetChallanQtyMst
    {
        public long ChallanQtyID { get; set; }
        public string ChallanNumber { get; set; }
        public Nullable<long> ChallanID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> ProductQtyID { get; set; }
        public Nullable<long> CategoryTypeID { get; set; }
        public string TaxName { get; set; }
        public decimal Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public Nullable<decimal> BillDiscount { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> FinalTotal { get; set; }
        public Nullable<decimal> ChallanTotal { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string EWayNumber { get; set; }
    }
}