

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data;
    using vb.Data.Model;

    public interface IExpensesService
    {
        List<DebitAccountTypeNameList> GetAllDebitAccountTypeName();

        long AddExpensesVoucher(ExpensesVoucher_Mst obj);

        List<ExpensesVoucherListResponse> GetAllExpensesVoucherList();

        bool DeleteExpensesVoucher(long ExpensesVoucherID, bool IsDelete);

        ExpensesVoucherListResponse GetDataForExpensesVoucherPrint(long ExpensesVoucherID);

        long AddDebitAccountType(DebitAccountType_Mst obj);

        List<DebitAccountTypeListResponse> GetAllDebitAccountTypeList();

        bool DeleteDebitAccountType(long DebitAccountTypeID, bool IsDelete);

        List<ExpensesVoucherListResponse> GetExpensesVoucherListByDate(DateTime? FromDate, DateTime? ToDate, long GodownID);

        List<ExpensesVoucherTotal> GetExpensesVoucherTotal(DateTime? FromDate, DateTime? ToDate);

        GetWholesaleAmount GetOpeningAmountByGodownID(long FromGodownID);

        GetWholesaleAmount GetOpeningChillarByGodownID(long GodownID);

        GetWholesaleAmount GetWholesaleAmountByCustID(DateTime WholesaleAssignedDate, long GodownID);

        List<SalesManWiseCash> GetSalesManWiseCashList(DateTime WholesaleAssignedDate, long GodownID);

        GetWholesaleAmount GetRetailAmountTotalByAssignedDate(DateTime RetailAssignedDate, long GodownID);

        List<SalesManWiseCash> GetRetSalesManWiseCashList(DateTime RetailAssignedDate, long GodownID);

        decimal GetTempoCashAmountTotal(DateTime TempoDateWholesale, string VehicleNo, long GodownID);

        decimal GetTempoCashAmountTotalRetail(DateTime TempoDateRetail, string VehicleNo, long GodownID);

        GetWholesaleAmount GetTotalExpenseByGodownwise(DateTime ExpensesDate, long GodownID);

        long AddInwardOutWard(AddInwardOutWard data);

        List<InwardOutWardListResponse> GetAllInwardList();

        List<VehicleInwardCost> GetVehicleCostListByInwardID(long InwardID);

        List<InwardOutWardListResponse> GetInwardListByDate(DateTime? FromDate);

        List<ExpensesVoucherListResponse> GetExpensesVoucherListByGodownID(DateTime ExpensesDate, long GodownID);

        List<InwardOutWardListResponse> GetInwardOutwardDetailForPrint(long InwardID, long GodownID);

        List<GodownListResponse> GetGodownForToGodown(long FromGodownID);

        long AddTransferAmount(TransferAmount_Mst obj);

        GetWholesaleAmount GetTopGrandTotalAmountByGodownIDandCreatedDateForOutward(long FromGodownID, DateTime CreatedOn);

        bool UpdateGrandTotalForOutwardGodown(long InwardID, long GodownID, decimal GrandTotal, decimal TotalOutward);

        GetWholesaleAmount GetTopGrandTotalAmountByGodownIDandCreatedDateForInward(long ToGodownID, DateTime CreatedOn);

        bool UpdateGrandTotalForInwardGodown(long InwardID, long GodownID, decimal GrandTotal, decimal TotalInward);

        List<TransferAmountListResponse> GetAllTrasferAmountList();

        List<TransferAmountListResponse> GetPopupTransferAmountList(long GodownID, DateTime CreatedOn);

        List<TransferAmountListResponse> GetTransferAmountListForPrint(DateTime CreatedOn, long GodownID);

        List<TransferAmountListResponse> GetTransferAmountListForPrintInward(DateTime CreatedOn, long GodownID);

        List<VehicleExpensesList> GetInwardVehicleExpensesList(long InwardID);

        List<VehicleExpensesList> GetOutwardVehicleExpensesList(long InwardID);

        InwardOutWardListResponse CheckTodaysGodownIsExists(long GodownID);

        List<VehicleNoListResponse> GetAllVehicleNoListForVehicleCosting();

        List<RetVehicleNoListResponse> GetAllRetVehicleNoListForVehicleCosting();

        GetVehicleDetails GetWholesaleVehicleDetailsForVehicleCosting(long WholesaleVehicleNo1, DateTime CreatedOn);

        GetVehicleDetails GetWholesaleTotalInvoiceAmountVehicle2(long WholesaleVehicleNo2, DateTime CreatedOn);

        GetVehicleDetails GetRetailVehicleDetailsForVehicleCosting(long RetailVehicleNo1, DateTime CreatedOn);

        GetVehicleDetails GetRetailTotalInvoiceAmountVehicle1(long RetailVehicleNo1, DateTime CreatedOn);

        GetVehicleDetails GetRetailTotalInvoiceAmountVehicle2(long RetailVehicleNo2, DateTime CreatedOn);

        long AddVehicleCosting(VehicleCosting_Mst obj);

        bool UpdateWholesaleVehicleNo1(int WholesaleVehicleNo1, DateTime WholesaleVehicleNo1Date);

        bool UpdateWholesaleVehicleNo2(int WholesaleVehicleNo2, DateTime WholesaleVehicleNo2Date);

        bool UpdateRetailVehicleNo1(int RetailVehicleNo1, DateTime RetailVehicleNo1Date);

        bool UpdateRetailVehicleNo2(int RetailVehicleNo2, DateTime RetailVehicleNo2Date);

        List<VehicleCostingListResponse> GetAllVehicleCostingList();

        List<VehicleCostingListReport> GetVehicleCostingListByDate(DateTime FromDate, DateTime ToDate, string VehicleDetailIDs);

        bool DeActiveInwardANDVoucherByDate(DateTime? FromDate, DateTime? ToDate, bool IsDactive);

        long AddDeActiveInwardANDVoucherHistory(DeActiveHistory_Mst obj);

        List<DeActiveInwardVoucherListResponse> GetAllDeActiveInwardVoucherList();

        // Pouch Stock 06-02-2020           
        string AddPouchInward(PouchInwardOutward data);

        List<PouchInwardListResponse> GetAllPouchInwardList();

        GetOpeningPouch GetOpeningPouchByPouchID(long GodownID, long PouchNameID);

        long AddPouchOutward(PouchInwardOutward data);

        List<PouchInwardCost> GetLastPouchCostByPouchID(long PouchNameID);

        List<PouchInwardCost> GetPouchCostByPouchInwardID(long PouchInwardID);

        List<PouchOutwardListResponse> GetAllPouchOutwardList();

        long AddNoOfPouchTransfer(AddNoOfPouchTransfer data);

        List<GetOpeningPouch> GetPouchForTransferByGodownID(long GodownID);

        bool UpdatePouchTransferAcceptStatusByPouchTransferID(List<GetOpeningPouch> data, long SessionUserID);

        List<PouchTransferListResponse> GetAllPouchStockTransferList();

        // Utility Stock 02-03-2020     
        List<GetOpeningUtility> GetUtilityForTransferByGodownID(long GodownID);

        GetOpeningUtility GetOpeningUtilityByUtilityID(long GodownID, long UtilityNameID);

        List<UtilityInwardCost> GetLastUtilityCostByUtilityID(long UtilityNameID);

        List<UtilityInwardCost> GetUtilityCostByUtilityInwardID(long UtilityInwardID);

        string AddUtilityInward(UtilityInwardOutward data);

        List<UtilityInwardListResponse> GetAllUtilityInwardList();

        long AddUtilityOutward(UtilityInwardOutward data);

        List<UtilityOutwardListResponse> GetAllUtilityOutwardList();

        long AddNoOfUtilityTransfer(AddNoOfUtilityTransfer data);

        bool UpdateUtilityTransferAcceptStatusByUtilityTransferID(List<GetOpeningUtility> data, long SessionUserID);

        List<UtilityTransferListResponse> GetAllUtilityStockTransferList();

        // 18 June 2020
        VehicleNoListInwardResponse GetAllVehicleNoForInwardByAssignedDate(DateTime AssignedDate);

        RetailVehicleNoListInwardResponse GetAllRetailVehicleNoForInwardByAssignedDate(DateTime AssignedDate);

        // 27 Aug 2020 Piyush Limbani
        GetVehicleExpensesInwardAmount GetVehicleExpensesInwardDetail(long VehicleDetailID, DateTime AssignedDate, long GodownID);

        // 09 Sep 2020 Piyush Limbani
        List<WCustomerNameList> GetAllWholesaleCustomerNameForVoucher();

        List<RCustomerNameList> GetAllRetailCustomerNameForVoucher();

        // 16 Sep 2020 Piyush Limbani
        List<UtilityStockResponse> GetUtilityStockReport(long? GodownID, long? UtilityNameID);

        List<PouchStockResponse> GetPouchStockReport(long? GodownID, long? PouchNameID);

        // 05 Oct 2020 Piyush Limbani
        List<UtilityListResponse> GetUtilityByGodownID(long GodownID);

        // 08 Oct 2020 Piyush Limbani
        List<PouchListResponse> GetPouchByGodownID(long GodownID);

        //  Jan 21, 2021 Piyush Limbani
        bool UpdateTransferDetail(TransferPaymentModel data, long UserID);

        // 22 Oct 2020 Piyush Limbani For Delete Vehicle Cost Uncommented on 01 Feb 2022
        // bool DeleteVehicleInwardCost(long? VehicleInwardCostID, long? VehicleDetailID, DateTime? AssignedDate, long UserID);
    }
}
