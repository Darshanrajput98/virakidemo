

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    //using System;

    public class ProductWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
        public decimal OrderQuantity { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public decimal ReturnOrderAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public string SrNo { get; set; }
        public string CategoryName { get; set; }
    }

    public class RetProductWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
        public decimal TotalQuantity { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal OrderTotalKg { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnTotalKg { get; set; }
        public decimal ReturnOrderAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalKg { get; set; }
        public decimal TotalAmount { get; set; }
        public string ArticleCode { get; set; }
        public string SrNo { get; set; }
        public string CategoryName { get; set; }

        //public DateTime UpdatedOn { get; set; }
        //public int RerurnMonthName { get; set; }
        //public int RerurnYearName { get; set; }

    }

    public class ProductMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetProductMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class ProductMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class RetProductMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetProductWiseSalesList> ListMainProduct { get; set; }
    }

    public enum MonthOFYear
    {
        Jan = 1,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Sep,
        Oct,
        Nov,
        Dec
    }

    public class ProductListForZeroSalesExport
    {
        public string SrNo { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
    }

    public class RetProductForZeroSalesExport
    {
        public string SrNo { get; set; }
        public string CategoryName { get; set; }
        public string ArticleCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
    }


    // GST Report
    public class ProductMainListByMonthGST
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ProductWiseGSTReportList> ListMainProduct { get; set; }
    }

    public class ProductWiseGSTReportList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public long ProductCategoryID { get; set; }
        public long AreaID { get; set; }
        public long TaxID { get; set; }
        public string Tax { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; } 
        public decimal TotalTaxableAmount { get; set; }
        public decimal ReturnTaxableAmount { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public decimal ReturnOrderAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal FinalTaxableAmount { get; set; }
        public decimal FinalOrderTotalAmount { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public string SrNo { get; set; }
        public string CategoryName { get; set; }
    }

    public class RetProductMainListByMonthGST
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<RetProductWiseGSTReportList> ListMainProduct { get; set; }
    }

    public class RetProductWiseGSTReportList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public long ProductCategoryID { get; set; }
        public long AreaID { get; set; }
        public long TaxID { get; set; }
        public string Tax { get; set; }
       // public long ProductID { get; set; }
        public long ProductQtyID { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal ReturnTaxableAmount { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        //public decimal OrderQuantity { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public decimal ReturnOrderAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal FinalTaxableAmount { get; set; }
        public decimal FinalOrderTotalAmount { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public string SrNo { get; set; }
        public string CategoryName { get; set; }
    }

}
