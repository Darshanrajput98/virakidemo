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
    
    public partial class RetProdGui
    {
        public long RetProdGuiID { get; set; }
        public Nullable<int> GuiID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public string ProductNameGui { get; set; }
        public string ContentGui { get; set; }
        public string NutritionGui { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
