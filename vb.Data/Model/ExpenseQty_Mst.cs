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
    
    public partial class ExpenseQty_Mst
    {
        public long ExpenseQtyID { get; set; }
        public Nullable<long> ExpenseID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public string HSNNumber { get; set; }
        public Nullable<long> ExpenseDebitAccountTypeID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> TotalTaxableAmount { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> CGSTTax { get; set; }
        public Nullable<decimal> CGSTTaxAmount { get; set; }
        public Nullable<decimal> SGSTTax { get; set; }
        public Nullable<decimal> SGSTTaxAmount { get; set; }
        public Nullable<decimal> IGSTTax { get; set; }
        public Nullable<decimal> IGSTTaxAmount { get; set; }
        public Nullable<decimal> TotalTaxAmount { get; set; }
        public Nullable<decimal> TCSAmount { get; set; }
        public Nullable<decimal> RoundOff { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> TDSTax { get; set; }
        public Nullable<decimal> TDSTaxAmount { get; set; }
        public Nullable<decimal> GrandTotalAmount { get; set; }
        public Nullable<decimal> TotalExpenseBillAmount { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
