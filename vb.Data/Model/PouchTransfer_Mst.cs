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
    
    public partial class PouchTransfer_Mst
    {
        public long PouchTransferID { get; set; }
        public Nullable<long> FromGodownID { get; set; }
        public Nullable<long> PouchNameID { get; set; }
        public Nullable<long> OpeningPouch { get; set; }
        public Nullable<long> TransferNoofPcs { get; set; }
        public Nullable<long> TotalPouch { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<long> ToGodownID { get; set; }
        public Nullable<long> PouchInwardID { get; set; }
        public Nullable<bool> IsAccept { get; set; }
        public Nullable<long> AcceptBy { get; set; }
        public Nullable<System.DateTime> AcceptDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
