

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;


    public interface IGroundStockService
    {
        long ManageGroundStock(GroundStock_Mst Obj);

        List<GroundStockListResponse> GetAllGroundStockList();

        bool DeleteGroundStock(long GroundStockID, bool IsDelete);

        long AddInward(GroundStock_Inward_Mst obj);

        List<GroundStockInwardListResponse> GetAllGroundStockInwardList();


        // piyush
        bool UpdateIsInwardStatus(long PurchaseQtyID, long PurchaseID, long ProductID);
        bool UpdateGroundStockQty(long ProductID, decimal ClosingQty);


        //Ground Stock Transfer

        List<ProductNameDDL> GetProductNameForDDL();

        long ManageGroundStockTransfer(GroundStockTransfer_Mst Obj);

        List<GroundStockTransferListResponse> GetAllGroundStockTransferList();

        bool DeleteGroundStockTransfer(long GroundStockTransferID, bool IsDelete);

        //Ground Stock Transfer Inward

        List<GroundStockTransferInwardResponse> GetAllGroundStockTransferInwardList();

        long AddStockTransferInward(GroundStockTransfer_Inward_Mst obj);

        bool UpdateStockTransferIsInwardStatus(long ChallanQtyID, long ChallanID, long ProductID);

        // 26-07-2022
        bool UpdateGroundStockTransferQty(long ProductID, long GodownIDTo, decimal ClosingQty, long GodownIDFrom, decimal DeductQty);

        List<GroundStockTransferInwardListResponse> GetGroundStockTransferInwardData(long ChallanID);

        // 21-07-2022
        long GetExistProductGroundStock(long ProductID);

        long GetExistProductGroundStockTransfer(long ProductID, long GodownID);

        
        //Outward
        List<GroundStockOutwardListResponse> GetAllGroundStockOutwardList();

        long AddOutward(GroundStock_Outward_Mst obj);

        bool UpdateGroundStockOutwardQty(long ProductID, decimal ClosingQty);
    }
}
