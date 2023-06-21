

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data;
    using vb.Data.Model;

    public interface IRetOrderService
    {
        bool UpdateSection(List<RetOrderPackList> ObjOrder);

        bool UpdateSummarySection(List<RetOrderSummaryListResponse> ObjOrder);

        string AddOrder(RetOrderViewModel ObjOrder);

        List<RetOrderListResponse> GetAllOrderList(RetOrderListResponse model);

        List<RetOrderListResponse> GetAllBillWiseOrderList(RetOrderListResponse model);

        List<RetOrderListResponse> GetAllReturnedOrderList(RetOrderListResponse model);

        string FinaliseOrder(RetOrderViewModel ObjOrder);

        RetOrderViewModel GetRetailOrderDetailsByOrderID(long OrderID);

        List<RetCustomerListResponse> GetAllRetCustomerName();

        List<RetProductListResponse> GetAllRetProductName();

        List<RetGetUnitRate> GetAutoCompleteRetProduct(long Prefix);

        List<string> GetListOfInvoice(long OrderID);

        List<RetGetSellPrice> GetAutoCompleteSellPrice(long ProductID, long Quantity);

        List<Product_Mst> GetOrderPackListForLabel(string id, string date, int Section, string orderid);

        List<ProductBarcode> GetOrderPackListForBarcode(string id, string date, int Section, string orderid);

        GetRetTax GetTaxForRetCustomer(long CustomerID);

        RetOrderListResponse GetLastRetailOrderID();

        GetRetSellPrice GetRetailOrderDetails(long ProductQtyID, string Tax, long CustomerID, long CustomerGroupID);

        List<RetOrderQtyList> GetInvoiceForOrder(long OrderID);

        List<RetOrderQtyList> GetBillWiseInvoiceForOrder(string InvoiceNumber, long OrderID);

        List<RetOrderQtyList> GetCreditMemoForOrder(long OrderID);

        List<ClsRetReturnOrderListResponse> GetBillWiseCreditMemoForOrder(ClsRetReturnOrderListResponse model);

        List<RetOrderQtyInvoiceList> GetInvoiceForOrderPrint(long OrderID);

        List<RetOrderQtyInvoiceList> GetInvoiceForOrderItemPrint(long OrderID, string InvoiceNumber);

        List<RetOrderQtyInvoiceList> GetCreditMemoInvoiceForOrderItemPrint(string CreditMemoNumber);

        List<RetOrderPackListResponse> GetOrderPackList(DateTime date);

        List<RetOrderSummaryListResponse> GetOrderSummaryList(DateTime date);

        List<RetOrderPackList> GetOrderPackListForPrint(string id, string date);



        List<RetOrderSummaryList> GetOrderSummaryListForPrint(string id, string date);

        List<RetOrderSummaryList> GetOrderSummaryListProductsForPrint(string id, string date);

        List<RetOrderPackList> GetOrderPackListForExport(string id, string date, int Section, string OrderID);

        List<RetOrderSummaryList> GetOrderSummaryListForExport(string id, string date, int Section);

        string CreateCreditMemo(List<RetOrderQtyList> ObjOrder, long SessionUserID);

        List<int> GetPackListSections();

        List<int> GetSummaryListSections();

        bool DeleteOrder(int OrderID);

        List<string> GetTaxForCustomerByTextChange(string CustomerName);

        // GetTax GetTaxForCustomerNumber(long CustomerNumber);
        GetRetTax GetTaxForCustomerNumber(long CustomerNumber);

        GetCustDetailExport GetCustomerDetailForExportOrder(long CustomerNumber);

        //List<RetInvoiceForExcelList> GetInvoiceForExcel(long OrderID, long GodownID, long TransportID);
        List<RetInvoiceForExcelList> GetInvoiceForExcel(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0);


        List<GetRetUnitRate> GetAutoCompleteProduct(long ProductQtyID);

        GetRetSellPrice1 GetAutoCompleteSellPrice(long ProductQtyID, string Tax);

        long SaveChallan(RetChallanViewModel ObjOrder);

        List<RetChallanQtyInvoiceList> GetInvoiceForChallanPrint(long ChallanID);

        List<string> GetListOfChallan(long ChallanID);

        List<RetChallanQtyInvoiceList> GetInvoiceForChallanItemPrint(long ChallanID, long CategoryID, string ChallanNumber);

        List<RetChallanListResponse> GetAllChallanList(RetChallanListResponse model);

        List<RetChallanQtyList> GetInvoiceForChallan(long ChallanID);

        List<RetChallanForExcelList> GetChallanForExcel(long ChallanID);

        bool DeleteCreditMemo(string CreditMemoNumber, bool IsDelete);

        bool UpdatePrintStatus(long OrderID);

        bool UpdatePrintPackList(string date, string OrderID);

        List<RetOrderPackListResponse> GetOrderCheckList(DateTime date);

        List<RetOrderPackList> GetOrderCheckListForLabelPrint(string id, string date);

        bool UpdateRetOrderQuantityForPrintLabel(long ID, decimal Quantity);

        RetPackSummaryResponse GetLastBagNumberByOrderID(long OrderID);

        RetPackSummaryResponse GetLastTrayNumberByOrderID(long OrderID);

        RetPackSummaryResponse GetLastZablaNumberByOrderID(long OrderID);

        RetPackSummaryResponse GetLastBoxNumberByOrderID(long OrderID);

        long AddRetPackSummaryDetail(long OrderID, long OrderQtyID, long CustomerID, long AreaID, string ProductName, int Bag, int Tray, int Zabla, int Box, decimal TotalKG, long CreatedBy, long PackSummaryID, DateTime OrderDate, string FinalTag);

        bool AddRetPackSummaryQtyDetail(long PackSummaryID, long OrderID, long OrderQtyID, long ProductID, long ProductQtyID, decimal Quantity, long CreatedBy);

        List<RetCustomeDetail> GetOrderCustomerDetail(long OrderID);

        List<RetPackSummary> GetRetPackSummaryByOrderID(long OrderID);

        bool UpdateCheckListQuantity(long OrderID, long OrderQtyID, decimal UpdateQuantity);

        RetPackTotal GetRetPrintTotalBagByOrderID(long OrderID);

        List<RetPackSummary> GetPackDetailListByOrderID(string id, string date);

        bool DeleteBag(long PackSummaryID, bool IsDelete);

        List<RetPackSummary> GetPackProductListByBagWise(long packsummaryid);

        bool UpdateProductPackList(long OrderQtyID, decimal Quantity, long PackSummaryQtyID, bool IsDelete, long SessionUserID, long PackSummaryID, decimal SumTotalKG);

        List<RetPackSummary> GetUpdateProductQTYList(long PackSummaryID);

        bool DeleteProductPackate(long PackSummaryQtyID, bool IsDelete, long UpdatedBy, DateTime UpdatedOn);

        List<ClsRetReturnOrderListResponse> GetOrderWiseCreditMemoForCheckList(ClsRetReturnOrderListResponse model);

        bool UpdateRetailEWayNumberByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, string EWayNumber);

        bool UpdateDocateDetailByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, long TransportID, string DocketNo, DateTime DocketDate);

        RetDecateNumberResponse GetTransportDetailByInvoiceNnumberandOrderID(long OrderID, string InvoiceNumber);

        List<RetChallanListResponse> GetAllChallanNoWiseChallanList(RetChallanListResponse model);

        List<RetChallanQtyList> GetChallanNoWiseChallanForChallan(string ChallanNumber);

        bool UpdateEWayNumberForChallanByChallanNumber(long ChallanID, string ChallanNumber, string EWayNumber);

        ExportOrderViewModel GetExportOrderDetailsByOrderID(long OrderID);

        ExportProductDetails GetExportProductDetails(long ProductQtyID);

        long AddExportOrder(ExportOrderViewModel ObjOrder);

        List<ExportOrderListResponse> GetExportOrderListByOrderDate(DateTime OrderDate);

        List<ExportOrderQtyInvoiceList> GetExportInvoiceOrderDetailForPrint(long OrderID);

        List<ExportOrderQtyInvoiceList> GetInvoiceForExportOrderItemPrint(long OrderID, string InvoiceNumber);

        List<ExportOrderQtyInvoiceList> GetExportInvoiceOrderDetailForPrintRupees(long OrderID, decimal Rupees);

        List<ExportOrderQtyInvoiceList> GetInvoiceForExportOrderItemPrintRupees(long OrderID, string InvoiceNumber, decimal Rupees);

        bool UpdateExportDollarPrice(long OrderID, string InvoiceNumber, decimal Rupees);

        List<ExpOrderListResponse> GetAllBillWiseExpOrderList(ExpOrderListResponse model);

        List<ExpOrderQtyList> GetBillWiseInvoiceForExpOrder(string InvoiceNumber, string currencyname = "");


        // 07 Sep. 2020 Piyush Limbani
        string CheckPDFIsExistForInvoiceNumber(long OrderID, string InvoiceNumber);

        bool UpdateInvoiceNameByOrderIDAndInvoiceNumber(string PDFName, long OrderID, string InvoiceNumber);

        // 02 April 2021 Piyush Limbani
        long CheckRetEInvoiceExist(long OrderId, string InvoiceNumber);

        // 04 April 2021 Piyush Limbani
        RetEInvoiceIRN GetRetIRNNumberByInvoiceNumber(long OrderID, string InvoiceNumber);

        // 07 April 2021 Piyush Limbani
        long CheckRetECreditMemoExist(string CreditMemoNumber);

        // 08 April 2021 Piyush Limbani
        RetEInvoiceCreditMemoIRN GetIRNNumberByCreditMemoNumber(string CreditMemoNumber);

        // 14 April 2021 Piyush Limbani  E-Invoice Error Report
        List<RetEInvoiceErrorListResponse> GetEInvoiceErrorList(DateTime Date);

        // 29 April 2021 Sonal Gandhi
        long CheckRetEWayBillExist(long OrderId, string InvoiceNumber);


        // 24 May, 2021 Sonal Gandhi
        List<RetDetailsForEWB> RetGetDetailsForEWB(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0);


        // 25 May, 2021 Sonal Gandhi
        long CheckRetEWayBillChallanExist(long ChallanID, string ChallanNumber);

        // 25 May, 2021 Sonal Gandhi
        List<RetChallanItemForEWB> GetRetChallanItemForEWB(long ChallanID, string ChallanNumber);

        // 25 May, 2021 Sonal Gandhi
        List<RetChallanDetailForEWB> GetRetChallanDetailForEWB(long ChallanID, long TransportID, long VehicleDetailID);

        // 18 Jan, 2022 Piyush Limbani
        List<TotalPouchListGodownWise> GetTotalPouchGodownWiseList(string id, string date);



        // 21-06-2022
        bool IsDonePrintPackList(long ProductQtyID, string date);

        // 27-06-2022
        bool IsDoneCheckList(long OrderID);
    }
}
