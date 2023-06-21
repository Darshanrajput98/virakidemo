

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RetPouchListResponse
    {
        public long ProductID { get; set; }
        public long ProductQtyID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       // public List<string> PouchID { get; set; }

        public List<string> PouchNameID { get; set; }  

        //public int PouchName { get; set; }
        public string PouchName { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPouch { get; set; }
    }

    public class RetPouchListForExport
    {
        //public int Pouch { get; set; }
        public string Pouch { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
    }

    public class WholesalePouchListResponse
    {
        public long ProductID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> PouchNameID { get; set; }
        //public int PouchName { get; set; }
        public string PouchName { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPouch { get; set; }
    }

    public class WholesalePouchListForExport
    {
        //public int Pouch { get; set; }
        public string Pouch { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }

}
