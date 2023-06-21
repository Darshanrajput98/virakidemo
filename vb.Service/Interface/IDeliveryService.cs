

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IDeliveryService
    {
        bool CheckDeliveryAllocationVehicle0Exist(DateTime date);

        Int32 InsertDeliveryAllocationVehicle(DateTime date);

        List<PendingDeliveryListResponse> GetAllPendingDeliveryList();

        bool UpdatePendingDelivery(List<OrderPendingRequest> data, long SessionUserID);

        bool RemovePendingDeliveryOfOrder(string data);

        bool UpdatePayment(List<DeliveryStatusListResponse> data, long SessionUserID);

        bool UpdateChequeDetailsForPayment(DeliveryStatusListResponse data);

        List<DeliveryStatusListResponse> GetDeliveryStatusList(DateTime? CreatedOn);

        List<DeliveryStatusListResponse> GetTempoSheetList(long VehicleNo, DateTime? CreatedOn);

        DeliveryStatusListResponse GetDeliveryInfoList(long VehicleNo, DateTime? CreatedOn);

        DeliveryStatusListResponse GetTempoInfoList(long VehicleNo, DateTime? CreatedOn);

        Int64 AddDeliveryAllocation(DeliveryStatusListResponse data);

        List<DeliveryStatusListResponse> GetPendingInvoiceListForPrint(long CustomerID);

        List<TempoSummary> GetTempoSummaryList(DateTime CreatedOn);

        bool DeleteOrderQty(string InvoiceNumber, bool IsDelete);

        bool SavePackedBy(PackedByViewModel SavePackedBy);


        // 02 Dec 2021 Sonal Gandhi
        List<DayWiseSalesApproveList> DeleteSalesApprovalList(DateTime? CreatedOn);

        bool UpdateDayWiseSalesApprove(long OrderID,string InvoiceNumber, bool IsApprove);
        // 02 Dec 2021 Sonal Gandhi


        //  List<DeliveryStatusListPrint> tblfaclilityEmail_LastActive();
    }
}
