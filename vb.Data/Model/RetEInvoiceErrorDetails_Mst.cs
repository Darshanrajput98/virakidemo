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
    
    public partial class RetEInvoiceErrorDetails_Mst
    {
        public long EInvoiceErrorDetailsID { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public Nullable<long> OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public string ErrorCodes { get; set; }
        public string ErrorDesc { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
