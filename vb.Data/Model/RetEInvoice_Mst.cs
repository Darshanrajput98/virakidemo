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
    
    public partial class RetEInvoice_Mst
    {
        public long EInvoiceId { get; set; }
        public Nullable<long> OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public string IRN { get; set; }
        public string QRCode { get; set; }
        public Nullable<long> AckNo { get; set; }
        public Nullable<System.DateTime> AckDt { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsCancel { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
    }
}
