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
    
    public partial class GroundStockTransfer_Inward_Mst
    {
        public long TransferInwardID { get; set; }
        public Nullable<long> ChallanQtyID { get; set; }
        public Nullable<long> ChallanID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> GodownID { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public Nullable<decimal> OpeningQty { get; set; }
        public Nullable<decimal> PurchaseQty { get; set; }
        public Nullable<decimal> ClosingQty { get; set; }
        public Nullable<decimal> LoadingQty { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
