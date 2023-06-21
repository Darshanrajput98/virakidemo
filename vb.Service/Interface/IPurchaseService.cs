using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service
{
    public interface IPurchaseService
    {
        List<string> GetTaxForSupplierByTextChange(string SupplierName);

        GetSupplierTax GetTaxForSupplierBySupplierID(long SupplierID);

        List<ProductListResponse> GetAllProductName();

        List<GetProductDetaiForPurchase> GetAutoCompleteProductDetaiForPurchase(long Prefix, string Tax);

        string CheckSupplierCurrentYearBillNumber(string StartDate, string EndDate, long SupplierID, string BillNumber);

        string AddPurchaseBill(AddPurchaseDetail ObjPurchase, string StartDate, string EndDate);

        List<PurcahseListResponse> GetAllPurchaseList(PurcahseListResponse model);

        AddPurchaseDetail GetPurchaseOrderDetailsByPurchaseID(long PurchaseID);

        bool DeletePurchaseOrder(long PurchaseID, bool IsDelete);

        // Expense Module 18-12-2019
        List<string> GetTaxForExpenseSupplierByTextChange(string SupplierName);

        GetSupplierTax GetTaxForExpenseSupplierBySupplierID(long SupplierID);

        List<ProductListResponse> GetAllExpenseProductName();

        List<GetProductDetaiForExpense> GetAutoCompleteProductDetaiForExpense(long ProductID);

        List<GetTaxDetailForExpense> GetAutoCompleteTaxDetailForExpense(long ExpenseDebitAccountTypeID, string Tax);

        long AddExpenseBill(AddExpenseDetail ObjPurchase);

        List<ExpenseListResponse> GetAllExpenseList(ExpenseListResponse model);

        bool DeleteExpenseOrder(long ExpenseID, bool IsDelete);

        AddExpenseDetail GetExpenseOrderDetailsByExpenseID(long ExpenseID);

        // 30-03-2020
        List<GetLastPurchaseProductDetail> GetLastPurchaseProductRatePerKG(long ProductID);

        // 29-05-2020
        List<BrokerAndItemWiseReportList> GetBrokerAndItemWiseReportList(DateTime? From, DateTime? To, long SupplierID, long ProductID, long BrokerID);

        // 10-06-2020
        List<GetLastExpenseProductDetail> GetLastExpenseProductRate(long ProductID);

        // 25 Aug 2020 Piyush Limbani
        string CheckExpenseSupplierCurrentYearBillNumber(string StartDate, string EndDate, long SupplierID, string BillNumber);

    }
}
