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
    
    public partial class CustomerAddress
    {
        public long CustomerAddressID { get; set; }
        public long AddressID { get; set; }
        public long CustomerID { get; set; }
        public string Name { get; set; }
        public string RoleDescription { get; set; }
        public string CellNo { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}