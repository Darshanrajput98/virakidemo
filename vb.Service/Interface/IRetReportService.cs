using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service
{
    public interface IRetReportService
    {
        List<RetDayWiseSalesList> GetDayWiseSalesList(DateTime? CreatedOn);

        List<RetDayWiseCreditMemoList> GetDayWiseCreditMemoList(DateTime? CreatedOn);

        List<RetDayWiseTaxList> GetDayWiseTaxList(DateTime? CreatedOn);

        List<RetDayWiseSalesManList> GetDayWiseSalesManList(DateTime? CreatedOn);

        List<RetCustomerListResponse> GetAllCustomerMasterList();

        List<RetDeliverySheetList> GetDeliverySheetList(DateTime? AssignedDate, string VehicleNo, string TempoNo, long GodownID, int BySign);

        List<RetProductWiseSalesList> GetProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID);

        List<RetProductWiseSalesList> GetProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID);

        List<RetSalesManWiseSalesList> GetSalesManWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);

        // 03 Oct 2020 Piyush Limbani
        List<RetSalesManWiseSalesList> GetRetSalesManWiseSalesListForFinalisedOrder(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);
        // 03 Oct 2020 Piyush Limbani

        List<RetSalesManWiseSalesList> GetSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);

        // 03 Oct 2020 Piyush Limbani
        List<RetSalesManWiseSalesList> GetRetSalesManWiseDailySalesListForFinalisedOrder(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);
        // 03 Oct 2020 Piyush Limbani 

        List<RetCustGroupSalesManWiseSalesList> GetCustGroupSalesManWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long CustomerGroupID);

        List<RetCustGroupSalesManWiseSalesList> GetCustGroupSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long CustomerGroupID);

        List<RetCustGroupProductWiseSalesList> GetRetCustGroupTotalProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID, long CustomerGroupID);

        List<RetCustGroupProductWiseSalesList> GetCustGroupProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID, long CustomerGroupID);

        List<RetCustGroupProductWiseSalesList> GetCustGroupProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID, long CustomerGroupID);

        List<RetDayWiseSalesList> GetDayWiseSalesListByUserID(long UserID, string Date);

        List<RetPouchListResponse> GetPouchWiseReportList(DateTime? StartDate, DateTime? EndDate, string PouchNameID);

        List<RetCashCounterListResponse> GetCashCounterReportList(DateTime? AssignedDate, long GodownID, int BySign);

        List<RetBillHistoryListResponse> GetBillHistoryList(string InvoiceNumber, long OrderID, string UpdatedOn, DateTime? FromDate, DateTime? ToDate, long CustomerID);

        List<RetCashCounterDayWiseSalesManList> GetCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID, int BySign);

        bool UpdatePayment(RetPayment data, long SessionUserID);

        //int GetOrderIDForBillHistory(string InvoiceNumber, string Year);
        List<GetRetOrderIDResponse> GetOrderIDForBillHistory(string InvoiceNumber, string Year);

        RetVehicleNoListResponse GetAllVehicleNoForDeliverysheetReport(DateTime AssignedDate);

        string GetRetCustomerIDForSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);

        List<RetSalesManWiseSalesList> GetRetSalesManWiseSalesList2(string CustomerIDs, long UserID, long AreaID, long DaysofWeek);

        string GetProductIDForRetProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID);

        List<RetProductWiseSalesList> GetProductWiseSalesList2(DateTime StartDate1, DateTime EndDate1, string ProductQtyIDs, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        // new 29-04-2019
        decimal GetRetReturnQuantityMonthWise(int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetRetReturnReturnTotalKgMonthWise(int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetRetReturnTotalAmountMonthWise(int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetRetReturnQuantityDayWise(int DayName, int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetRetReturnTotalKgDayWise(int DayName, int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetRetReturnOrderAmountDayWise(int DayName, int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        // GST Report
        List<RetProductWiseGSTReportList> GetProductWiseSalesForGSTReport(DateTime? StartDate, DateTime? EndDate, string Tax, long TaxID, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID);

        decimal GetRetReturnQuantityMonthWiseForGSTReport(int MonthName, int YearName, long TaxID, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        List<DayWiseSalesExportListForExp> GetDayWiseExportSalesList(DateTime? CreatedOn);

        List<ExpProductWiseSalesList> GetExProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long ProductCategoryID, long ProductID, long UserID);

        List<ExpProductWiseSalesList> GetExpProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long ProductCategoryID, long ProductID, long UserID);

        List<ExpProductWiseSalesList> GetExpProductWiseSalesList2(DateTime StartDate1, DateTime EndDate1, string ProductIDs, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        string GetExpProductIDForProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID);

        List<SalesManWiseExpSalesList> GetSalesManWiseExpSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID);

        List<SalesManWiseExpSalesList> GetExSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID);

        // 03-04-2020 - barcode history
        List<BarcodeHistoryListResponse> GetBarcodeHistoryList(BarcodeHistoryListResponse model);



        // 12 Sep 2020 Piyush Limbani
        List<RetVoucherCashCounterListResponse> GetRetailExpenseVoucherCashCounterReportList(DateTime? AssignedDate, long GodownID);

        bool UpdateVoucherPayment(RetUpdateVoucherPayment data, long UserID);

        List<RetVoucherCashCounterDayWiseSalesManList> GetRetVoucherCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID);

        //Add By Dhruvik 
        List<RetCashCounterListResponse> GetRetChequeRetrunList(DateTime? AssignedDate, long GodownID);

        List<RetCashCounterListResponse> GetRetChequeRetrunChargeList(DateTime? AssignedDate, long GodownID);
        //Add By Dhruvik

    }
}
