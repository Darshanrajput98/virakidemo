

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IOrderService
    {
        List<string> GetListOfInvoice(long OrderID);

        long AddOrder(OrderViewModel ObjOrder);

        List<OrderListResponse> GetAllBillWiseOrderList(OrderListResponse model);

        List<OrderListResponse> GetAllOrderList(OrderListResponse model);

        List<OrderListResponse> GetAllReturnedOrderList(OrderListResponse model);

        List<OrderListResponse> GetSearchInvoiceList(OrderListResponse model);

        List<OrderQtyList> GetCreditMemoInvoiceForOrder(long OrderID);

        List<OrderQtyList> GetBillWiseCreditMemoInvoiceForOrder(string InvoiceNumber);

        List<OrderQtyList> GetBillWiseInvoiceForOrder(string InvoiceNumber);

        List<OrderQtyList> GetInvoiceForOrder(long OrderID);

        List<OrderQtyInvoiceList> GetInvoiceForOrderPrint(long OrderID);

        List<OrderQtyInvoiceList> GetCreditMemoInvoiceForOrderPrint(long OrderID);

        List<OrderQtyInvoiceList> GetCreditMemoInvoiceForOrderItemPrint(string CreditMemoNumber);

        List<OrderQtyInvoiceList> GetInvoiceForOrderItemPrint(long OrderID, long CategoryID, string InvoiceNumber);

        List<ClsReturnOrderListResponse> GetBillWiseCreditMemoForOrder(ClsReturnOrderListResponse model);

        List<OrderQtyList> GetCreditMemoForOrder(long OrderID);

        List<CustomerListResponse> GetActiveCustomerName(long CustomerID);

        List<ProductListResponse> GetAllProductName();

        List<OrderListResponse> GetAllOrderQtyList();

        string CreateCreditMemo(List<OrderQtyList> ObjOrder, long SessionUserID);

        List<GetUnitRate> GetAutoCompleteProduct(long Prefix);

        OrderListResponse GetLastOrderID();

        GetSellPrice GetAutoCompleteSellPrice(long ProductID, decimal Quantity, string Tax);

        GetTax GetTaxForCustomer(long CustomerID);

        List<string> GetTaxForCustomerByTextChange(string CustomerName);

        GetTax GetTaxForCustomerNumber(long CustomerNumber);

        //List<InvoiceForExcelList> GetInvoiceForExcel(long OrderID, long GodownID, long TransportID);

        // 07 May 2021 Sonal Gandhi
        List<InvoiceForExcelList> GetInvoiceForExcel(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0);


        long SaveChallan(ChallanViewModel ObjOrder);

        List<ChallanQtyInvoiceList> GetInvoiceForChallanPrint(long OrderID);

        List<string> GetListOfChallan(long ChallanID);

        List<ChallanQtyInvoiceList> GetInvoiceForChallanItemPrint(long ChallanID, long CategoryID, string ChallanNumber);

        List<ChallanListResponse> GetAllChallanList(ChallanListResponse model);

        List<ChallanQtyList> GetInvoiceForChallan(long ChallanID);

        List<ChallanForExcelList> GetChallanForExcel(long ChallanID);

        bool DeleteCreditMemo(string CreditMemoNumber, bool IsDelete);

        List<ProductList> GetProductNameByCustomerID(long CustomerID);

        bool UpdateOrderpdf(string PDFName, long OrderID);

        string CheckPDFNameExist(long OrderID);

        List<OrderListResponse> GetAllMobileOrderList();

        // 06 Jan,2022 Piyush Limbani
        List<OrderListResponse> GetAllMobileOrderListByOrderID(string OrderID);
        // 06 Jan,2022 Piyush Limbani

        bool UpdateRemotePrintStatus(long OrderID);

        bool UpdateEWayNumberByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, string EWayNumber);

        bool UpdateDocateDetailByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, long TransportID, string DocketNo, DateTime DocketDate);

        DecateNumberResponse GetTransportDetailByInvoiceNnumberandOrderID(long OrderID, string InvoiceNumber);


        List<ChallanListResponse> GetAllChallanNoWiseChallanList(ChallanListResponse model);

        List<ChallanQtyList> GetChallanNoWiseChallanForChallan(string ChallanNumber);

        bool UpdateEWayNumberForChallanByChallanNumber(long ChallanID, string ChallanNumber, string EWayNumber);


        // 01 Sep. 2020 Piyush Limbani
        string CheckPDFIsExistForInvoiceNumber(long OrderID, string InvoiceNumber);

        bool UpdateInvoiceNameByOrderIDAndInvoiceNumber(string PDFName, long OrderID, string InvoiceNumber);


        // 02 April 2021 Piyush Limbani
        long CheckEInvoiceExist(long OrderId, string InvoiceNumber);

        // 04 April 2021 Piyush Limbani
        EInvoiceIRN GetIRNNumberByInvoiceNumber(long OrderID, string InvoiceNumber);


        // 07 April 2021 Piyush Limbani
        long CheckECreditMemoExist(string CreditMemoNumber);


        // 08 April 2021 Piyush Limbani
        EInvoiceCreditMemoIRN GetIRNNumberByCreditMemoNumber(string CreditMemoNumber);

        // 14 April 2021 Piyush Limbani  E-Invoice Error Report
        List<EInvoiceErrorListResponse> GetEInvoiceErrorList(DateTime Date);


        // 28 April 2021 Sonal Gandhi
        long CheckEWayBillExist(long OrderId, string InvoiceNumber);


        // 24 May, 2021 Sonal Gandhi
        List<DetailsForEWB> GetDetailsForEWB(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0);


        // 24 May, 2021 Sonal Gandhi
        long CheckEWayBillChallanExist(long ChallanID, string ChallanNumber);


        // 25 May, 2021 Sonal Gandhi
        List<ChallanItemForEWB> GetChallanItemForEWB(long ChallanID, string ChallanNumber);


        // 25 May, 2021 Sonal Gandhi
        List<ChallanDetailForEWB> GetChallanDetailForEWB(long ChallanID, long TransportID, long VehicleDetailID);
    }
}
