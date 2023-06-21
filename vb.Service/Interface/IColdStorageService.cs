

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;


    public interface IColdStorageService
    {


        long AddColdStorage(ColdStorage_Mst obj);

        List<ColdStorageListResponse> GetAllColdStorageList();

        bool DeleteColdStorage(long PFID, bool IsDelete);


        List<string> GetColdstorageByTextChange(string Name);

        List<GetProductDetaiForPurchase> GetAutoCompleteProductDetaiForInward(long Prefix);

        AddInwardDetails GetPurchaseOrderDetailsByInwardID(long InwardID);

        ColdStorageName1 GetColdStorageByColdStorageID(long ColdStorageID);

        List<ProductListResponse> GetAllProductName();

        string AddInward(AddInwardDetails data);

        List<InwardListResponse> GetAllColdStorage_InwardList(DateTime? ChallanDate, long ColdStorageID, string LotNo, long ProductID);

        //08-07-2022
        string GetExistInwardDetails(long ColdStorageID, string LotNo);


        bool DeleteColdstorageInward(long InwardID, bool IsDelete);

        //Outward Details

        InwardListResponse GetAllColdStorage_OutwardID(long OutwardID);

        //12-28-2022 
        long AddOutward(List<InwardListResponse> data, long SessionUserID);

        List<OutwardListResponse> GetAllColdStorage_OutwardList(long ColdStorageID, DateTime? FromDate, DateTime? ToDate);

        bool DeleteColdstorageOutward(long OutwardID, bool IsDelete);

        //--------ColdStorage DDl ---------------

        List<ColdStorageNameDDL> GetAllColdStorageName();

        ///Stock Report List
        //08-07-2022
        List<StockReportResponseList> GetAllColdStorage_StockReportList(long ProductID, long ColdStorageID, DateTime? ToDate);


        //01-02-2023
        long UpdateOutward(InwardListResponse data);



        List<StockReportResponseList> MonthAgoExpiryDateWiseGetColdStorage_StockReportList();
    }
}
