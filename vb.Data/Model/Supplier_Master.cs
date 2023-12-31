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
    
    public partial class Supplier_Master
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public Nullable<long> TaxID { get; set; }
        public string PanCardNumber { get; set; }
        public string FSSAI { get; set; }
        public Nullable<System.DateTime> FSSAIValidUpTo { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public Nullable<long> TDSCategoryID { get; set; }
        public Nullable<decimal> TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public Nullable<long> AreaIDOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public Nullable<long> AreaIDTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Identification { get; set; }
        public string NameAsBankAccount { get; set; }
    }
}
