

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IRetDeliveryService
    {
        List<RetPendingDeliveryListResponse> GetAllPendingDeliveryList();

        bool UpdatePendingDelivery(List<RetOrderPendingRequest> data, long SessionUserID);

        //    bool RemovePendingDeliveryOfOrder(string data);

        bool RemovePendingDeliveryOfOrder(List<RetOrderRemoveFromTempo> data);



        bool UpdatePayment(List<RetDeliveryStatusListResponse> data, long SessionUserID);

        bool UpdateChequeDetailsForPayment(RetDeliveryStatusListResponse data);

        List<RetDeliveryStatusListResponse> GetDeliveryStatusList(DateTime? CreatedOn);

        List<RetDeliveryStatusListResponse> GetTempoSheetList(long VehicleNo, DateTime? CreatedOn);

        RetDeliveryStatusListResponse GetDeliveryInfoList(long VehicleNo, DateTime? CreatedOn);

        RetDeliveryStatusListResponse GetTempoInfoList(long VehicleNo, DateTime? CreatedOn);

        Int64 AddDeliveryAllocation(RetDeliveryStatusListResponse data);

        List<RetDeliveryStatusListResponse> GetPendingInvoiceListForPrint(long CustomerID);

        //  List<RetTempoSummary> GetTempoSummaryList(DateTime CreatedOn);

        bool DeleteOrderQty(string InvoiceNumber, long OrderID, bool IsDelete);

        bool CheckDeliveryAllocationVehicle0Exist(DateTime date);

        Int32 InsertDeliveryAllocationVehicle(DateTime date);



        List<RetTempoSummary> GetPackDetailVehicleList(DateTime CreatedOn);

        RetTempoSummary GetTempoSummaryDetailList(DateTime CreatedOn, int VehicleNo);



        // 13 Dec 2021 Piyush Limbani
        List<RetDayWiseSalesApproveList> DeleteSalesApprovalList(DateTime? CreatedOn);

        bool UpdateDayWiseSalesApprove(long OrderID, string InvoiceNumber, bool IsApprove);
        // 13 Dec 2021 Piyush Limbani


    }
}
