

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System;

    public class RetCustGroupSalesManWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalQuantity { get; set; }
    }

    public class RetCustGroupSalesMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetCustGroupSalesManWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetCustGroupSalesMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetCustGroupSalesManWiseSalesList> ListMainProduct { get; set; }
    }
}
