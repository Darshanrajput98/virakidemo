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
    
    public partial class Customer_Mst
    {
        public long CustomerID { get; set; }
        public Nullable<long> CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public Nullable<long> CustomerGroupID { get; set; }
        public long AreaID { get; set; }
        public string TaxNo { get; set; }
        public long UserID { get; set; }
        public long TaxID { get; set; }
        public Nullable<System.DateTime> ClosingTime { get; set; }
        public Nullable<System.DateTime> OpeningTime { get; set; }
        public string NoofInvoice { get; set; }
        public string FSSAI { get; set; }
        public Nullable<System.DateTime> FSSAIValidUpTo { get; set; }
        public int CustomerTypeID { get; set; }
        public string LBTNo { get; set; }
        public Nullable<bool> CallWeek1 { get; set; }
        public Nullable<bool> CallWeek2 { get; set; }
        public Nullable<bool> CallWeek3 { get; set; }
        public Nullable<bool> CallWeek4 { get; set; }
        public Nullable<bool> DoNotDisturb { get; set; }
        public string CustomerNote { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFCCode { get; set; }
        public Nullable<long> DeliveryAreaID { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliveryAddressLine2 { get; set; }
        public string DeliveryAddressPincode { get; set; }
        public Nullable<decimal> DeliveryAddressDistance { get; set; }
        public Nullable<long> BillingAreaID { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string FSSAICertificate { get; set; }
        public Nullable<bool> IsVirakiEmployee { get; set; }
        public Nullable<bool> IsReflectInVoucher { get; set; }
        public string CellNo1 { get; set; }
        public string CellNo2 { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public Nullable<bool> IsTCSApplicable { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string PanCard { get; set; }
    }
}
