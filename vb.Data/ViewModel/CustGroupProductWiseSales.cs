

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System;


    public class RetCustGroupProductWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerGroupID { get; set; }
        public long CustomerGroupName { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public long ProductQtyID { get; set; }
        public long ProductCategoryID { get; set; }
        public long AreaID { get; set; }
        public string ProductName { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }

        public decimal TotalKg { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal TotalQuantity { get; set; }
    }

    public class RetCustGroupProductMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetCustGroupProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetCustGroupProductMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetCustGroupProductWiseSalesList> ListMainProduct { get; set; }
    }
}
