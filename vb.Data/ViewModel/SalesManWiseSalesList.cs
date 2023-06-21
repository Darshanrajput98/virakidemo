

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System;

    public class SalesManWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public long DaysofWeek { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal ReturnOrderAmount { get; set; }
        public decimal TotalQuantity { get; set; }

        public string SrNo { get; set; }
        public string CellNo { get; set; }
    }

    public class RetSalesManWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }       
        public string UserName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public long DaysofWeek { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal ReturnOrderAmount { get; set; }
        public decimal Quantity { get; set; }
        public string SrNo { get; set; }
        public string CellNo { get; set; }
        public bool IsFinalised { get; set; }
    }

    public class SalesMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<SalesManWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetSalesMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetSalesManWiseSalesList> ListMainProduct { get; set; }
    }

    public class SalesMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<SalesManWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetSalesMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetSalesManWiseSalesList> ListMainProduct { get; set; }
    }

    public class CustomerListForZeroSalesExport
    {
        public string SrNo { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string CellNo { get; set; }
        public string SalesPerson { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class SalesManWiseExpSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long CountryID { get; set; }
        public string AreaName { get; set; }
        public string CountryName { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal InvoiceTotalAmount { get; set; }
        //   public decimal ReturnOrderAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string SrNo { get; set; }
    }
    public class SalesExpMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<SalesManWiseExpSalesList> ListMainProduct { get; set; }
    }
    public class SalesExpMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<SalesManWiseExpSalesList> ListMainProduct { get; set; }
    }
}
