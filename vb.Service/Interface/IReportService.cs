using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service
{
    public interface IReportService
    {
        List<DayWiseSalesList> GetDayWiseSalesList(DateTime? CreatedOn);

        List<DayWiseCreditMemoList> GetDayWiseCreditMemoList(DateTime? CreatedOn);

        List<DayWiseTaxList> GetDayWiseTaxList(DateTime? CreatedOn);

        List<DayWiseSalesManList> GetDayWiseSalesManList(DateTime? CreatedOn);

        List<DayWiseSalesList> GetDayWiseSalesListByUserID(long UserID, string Date);

        List<DeliverySheetList> GetDeliverySheetList(DateTime? AssignedDate, string VehicleNo, string TempoNo, long GodownID, int BySign);

        GetEvent GetEventDateForSameYear(long EventID, long StartYear);

        GetEvent GetEventDateForDiffYear(long EventID, long StartYear, long EndYear);

        List<CustomerListResponse> GetAllCustomerMasterList();

        List<ProductWiseSalesList> GetProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID);

        List<ProductWiseSalesList> GetProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID);

        List<SalesManWiseSalesList> GetSalesManWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);

        List<SalesManWiseSalesList> GetSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);

        List<WholesalePouchListResponse> GetPouchWiseReportList(DateTime? StartDate, DateTime? EndDate, string PouchNameID);

        List<CashCounterListResponse> GetCashCounterReportList(DateTime? AssignedDate, long GodownID, int BySign);

        List<BillHistoryListResponse> GetBillHistoryList(string InvoiceNumber, DateTime? FromDate, DateTime? ToDate, long CustomerID);

        List<CashCounterDayWiseSalesManList> GetCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID, int BySign);

        bool UpdatePayment(Payment data, long SessionUserID);

        VehicleNoListResponse GetAllVehicleNoForDeliverysheetReport(DateTime AssignedDate);

        string GetCustomerIDForSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek);

        List<SalesManWiseSalesList> GetSalesManWiseSalesList2(string CustomerIDs, long UserID, long AreaID, long DaysofWeek);

        string GetProductIDForProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID);

        List<ProductWiseSalesList> GetProductWiseSalesList2(DateTime StartDate1, DateTime EndDate1, string ProductIDs, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        // new 30-04-2019
        decimal GetReturnQuantityMonthWise(int MonthName, int YearName, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetReturnQuantityDayWise(int DayName, int MonthName, int YearName, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        decimal GetReturnOrderAmountDayWise(int DayName, int MonthName, int YearName, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        // Purchase Report 
        List<DayWisePurchaseList> GetDayWisePurchaseList(DateTime? CreatedOn);

        List<PurchasePaidPaymentList> GetPurchasePaidPaymentList(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        List<PurchasePaidPaymentList> GetPurchasePaidPaymentListForExport(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        // 02/08/2019
        List<PurchasePaidPaymentList> GetPurchasePaidPaymentListByCreatedOn(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        List<PurchasePaidPaymentList> GetPurchasePaidPaymentListByCreatedOnForExport(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        List<UnVerifyPendingPurchaseAavakList> GetAllUnVerifyPendingPurchaseAavakReport();

        bool UpdateVerifyPurcahseOrderStatus(List<UpdateVerifyPurcahseOrderStatus> data, long SessionUserID);

        // GST Report
        List<ProductWiseGSTReportList> GetProductWiseSalesForGSTReport(DateTime? StartDate, DateTime? EndDate, string Tax, long TaxID, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID);

        decimal GetReturnQuantityMonthWiseForGSTReport(int MonthName, int YearName, long TaxID, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID);

        bool UpdatePurchasePayment(PurchasePayment data, long SessionUserID);

        // Expense Report
        List<DayWiseExpenseList> GetDayWiseExpenseList(DateTime? CreatedOn);

        List<ExpensePaidPaymentList> GetExpensePaidPaymentList(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        List<ExpensePaidPaymentList> GetExpensePaidPaymentListByCreatedOn(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        bool UpdateExpensePayment(ExpensePayment data, long SessionUserID);

        List<ExpensePaidPaymentList> GetExpensePaidPaymentListForExport(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        List<ExpensePaidPaymentList> GetExpensePaidPaymentListByCreatedOnForExport(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline);

        // 24 Aug 2020 Piyush Limbani
        List<ExpenseItemWiseReportList> GetExpenseItemWiseReportList(DateTime? From, DateTime? To, long SupplierID, long ProductID);


        // 12 Sep 2020 Piyush Limbani
        List<VoucherCashCounterListResponse> GetWholesaleExpenseVoucherCashCounterReportList(DateTime? AssignedDate, long GodownID);

        bool UpdateVoucherPayment(UpdateVoucherPayment data, long UserID);

        List<VoucherCashCounterDayWiseSalesManList> GetVoucherCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID);

        // 14 June,2021 Sonal Gandhi
        List<OnlineOrder> GetOnlineOrderProductList(DateTime? CreatedOn, bool IsConfirm = false);

        List<OnlineOrderQty> ViewBillWiseOnlineOrderDetails(long OnlineOrderID);

        OnlineOrderViewModel GetOnlineOrderDetailsByOnlineOrderID(long OnlineOrderId);

        bool UpdateOnlineOrderIsConfirm(long OnlineOrderId);


       //Add By Dhruvik
        List<CashCounterListResponse> GetCheckRetrunEntryList(DateTime? AssignedDate, long GodownID);

        List<CashCounterListResponse> GetRetrunChargeList(DateTime? AssignedDate, long GodownID);
    }
}
