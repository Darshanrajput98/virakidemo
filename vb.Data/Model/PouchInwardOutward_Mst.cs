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
    
    public partial class PouchInwardOutward_Mst
    {
        public long PouchInwardID { get; set; }
        public Nullable<long> GodownID { get; set; }
        public Nullable<long> PouchNameID { get; set; }
        public Nullable<long> PouchID { get; set; }
        public Nullable<long> OpeningPouch { get; set; }
        public Nullable<long> NoofPcs { get; set; }
        public Nullable<long> TotalPouch { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public Nullable<long> SupplierID { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<decimal> TotalInwardCost { get; set; }
        public Nullable<int> CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
