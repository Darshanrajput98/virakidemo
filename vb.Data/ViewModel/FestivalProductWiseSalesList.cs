

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System;

    public class FestivalProductWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long BeforeDays { get; set; }
        public long AfterDays { get; set; }
        public long StartYear { get; set; }
        public long EndYear { get; set; }
        public long EventID { get; set; }
        public string EventName { get; set; }
        public long EventDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public long ProductID { get; set; }
        public long ProductCategoryID { get; set; }
        public long AreaID { get; set; }
        public string ProductName { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalQuantity { get; set; }
    }

    public class GetEvent
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class RetFestivalProductWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long BeforeDays { get; set; }
        public long AfterDays { get; set; }
        public long StartYear { get; set; }
        public long EndYear { get; set; }
        public long EventID { get; set; }
        public string EventName { get; set; }
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
        public decimal TotalQuantity { get; set; }
    }

    public class FestivalProductMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class FestivalRetProductMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class FestivalProductMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetFestivalProductMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetProductWiseSalesList> ListMainProduct { get; set; }
    }

}
