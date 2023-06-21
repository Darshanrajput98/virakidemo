

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DebitAccountTypeNameList
    {
        public long DebitAccountTypeID { get; set; }
        public string DebitAccountType { get; set; }
    }

    public class AddExpensesVoucher
    {
        public long ExpensesVoucherID { get; set; }
        public long GodownID { get; set; }
        public DateTime DateofVoucher { get; set; }
        public string VoucherNumber { get; set; }
        public string Pay { get; set; }
        public string RemarksL1 { get; set; }
        public string RemarksL2 { get; set; }
        public string RemarksL3 { get; set; }
        public long DebitAccountTypeID { get; set; }
        public decimal Amount { get; set; }
        public string BillNumber { get; set; }
        public long CustomerID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public string Identification { get; set; }
    }

    public class ExpensesVoucherListResponse
    {
        public long ExpensesVoucherID { get; set; }
        public DateTime DateofVoucher { get; set; }
        public string DateofVoucherstr { get; set; }
        public string VoucherNumber { get; set; }
        public string Pay { get; set; }
        public string RemarksL1 { get; set; }
        public string RemarksL2 { get; set; }
        public string RemarksL3 { get; set; }
        public string Remarks { get; set; }
        public long DebitAccountTypeID { get; set; }
        public string DebitAccountType { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public long CreatedBy { get; set; }
        public string PreparedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
        public string AmountInWords { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string GodownNamestr { get; set; }
        public string BillNumber { get; set; }
        public long CustomerID { get; set; }
        public string Identification { get; set; }
    }

    public class AddDebitAccountType
    {
        public long DebitAccountTypeID { get; set; }
        public string DebitAccountType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class DebitAccountTypeListResponse
    {
        public long DebitAccountTypeID { get; set; }
        public string DebitAccountType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ExpensesVoucherListExport
    {
        public string Cash { get; set; }
        public string DateofVoucher { get; set; }
        public string VoucherNumber { get; set; }
        public string PayTo { get; set; }
        public string DebitAccount { get; set; }
        public string BillNumber { get; set; }
        public decimal Amount { get; set; }
        //public string AmountInWords { get; set; }
        public string PreparedBy { get; set; }
        public string Remarks { get; set; }
    }

    public class ExpensesVoucherTotal
    {
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ExpensesVoucherTotalEXP
    {
        public decimal TotalAmount { get; set; }
    }

    public class GetCustomerNameByCash
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

    public class GetWholesaleAmount1
    {
        public decimal CashTotal { get; set; }
    }

    public class GetWholesaleAmount
    {
        public decimal ByCash { get; set; }
        public decimal CashTotal { get; set; }
        public decimal CashTotalRetail { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal ChillarAmount { get; set; }
        public decimal OpeningChillar { get; set; }
        public long InwardID { get; set; }
        public decimal TotalOutward { get; set; }
        public decimal TotalInward { get; set; }
    }

    public class TempoCashAmount
    {
        public long GodownID { get; set; }
        public DateTime TempoDateWholesale { get; set; }
        public DateTime TempoDateRetail { get; set; }
        public List<string> VehicleNo { get; set; }
    }

    public class SalesManWiseCash
    {
        public string SalesPerson { get; set; }
        public decimal Cash { get; set; }
        public decimal CashTotal { get; set; }
    }

    public class AddInwardOutWard
    {
        public long InwardID { get; set; }
        public long GodownID { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal ChillarInward { get; set; }
        public DateTime WholesaleAssignedDate { get; set; }
        public decimal WholesaleCash { get; set; }
        public DateTime RetailAssignedDate { get; set; }
        public decimal RetailCash { get; set; }
        public DateTime? WholesaleTempoDate1 { get; set; }
        public string WholesaleVehicleNo1 { get; set; }
        public decimal WholesaleTempoAmount1 { get; set; }
        public DateTime? WholesaleTempoDate2 { get; set; }
        public string WholesaleVehicleNo2 { get; set; }
        public decimal WholesaleTempoAmount2 { get; set; }
        public DateTime? WholesaleTempoDate3 { get; set; }
        public string WholesaleVehicleNo3 { get; set; }
        public decimal WholesaleTempoAmount3 { get; set; }
        public DateTime? RetailTempoDate1 { get; set; }
        public string RetailVehicleNo1 { get; set; }
        public decimal RetailTempoAmount1 { get; set; }
        public DateTime? RetailTempoDate2 { get; set; }
        public string RetailVehicleNo2 { get; set; }
        public decimal RetailTempoAmount2 { get; set; }
        public DateTime? RetailTempoDate3 { get; set; }
        public string RetailVehicleNo3 { get; set; }
        public decimal RetailTempoAmount3 { get; set; }
        public decimal TotalInward { get; set; }
        public decimal ChillarOutward { get; set; }
        public DateTime? ExpensesDate { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TransferVB2 { get; set; }
        public decimal TransferVB3 { get; set; }
        public long BankID1 { get; set; }
        public decimal BankDepositeAmount1 { get; set; }
        public long BankID2 { get; set; }
        public decimal BankDepositeAmount2 { get; set; }
        public long BankID3 { get; set; }
        public decimal BankDepositeAmount3 { get; set; }
        public decimal TotalOutward { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal OpeningChillar { get; set; }
        public decimal ClosingChillar { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public bool IsDactive { get; set; }
        public List<VehicleInwardCost> lstVehicleInwardCost { get; set; }
        public List<VehicleOutwardCost> lstVehicleOutwardCost { get; set; }
    }

    public class VehicleInwardCost
    {
        public long VehicleInwardCostID { get; set; }
        public long ID { get; set; }
        public long InwardID { get; set; }
        public long VehicleDetailID { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AssignedDatestr { get; set; }
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class VehicleOutwardCost
    {
        public long VehicleInwardCostID { get; set; }
        public long ID { get; set; }
        public long InwardID { get; set; }
        public long VehicleDetailID { get; set; }
        public DateTime AssignedDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class VehicleExpensesList
    {
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class InwardOutWardListResponse
    {
        public long InwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal ChillarInward { get; set; }
        public DateTime WholesaleAssignedDate { get; set; }
        public string WholesaleAssignedDatestr { get; set; }
        public decimal WholesaleCash { get; set; }
        public DateTime RetailAssignedDate { get; set; }
        public string RetailAssignedDatestr { get; set; }
        public decimal RetailCash { get; set; }
        public DateTime WholesaleTempoDate1 { get; set; }
        public string WholesaleTempoDate1str { get; set; }
        public string WholesaleVehicleNo1 { get; set; }
        public decimal WholesaleTempoAmount1 { get; set; }
        public DateTime WholesaleTempoDate2 { get; set; }
        public string WholesaleTempoDate2str { get; set; }
        public string WholesaleVehicleNo2 { get; set; }
        public decimal WholesaleTempoAmount2 { get; set; }
        public DateTime WholesaleTempoDate3 { get; set; }
        public string WholesaleTempoDate3str { get; set; }
        public string WholesaleVehicleNo3 { get; set; }
        public decimal WholesaleTempoAmount3 { get; set; }
        public decimal WholesaleTempoAmount { get; set; }
        public DateTime RetailTempoDate1 { get; set; }
        public string RetailTempoDate1str { get; set; }
        public string RetailVehicleNo1 { get; set; }
        public decimal RetailTempoAmount1 { get; set; }
        public DateTime RetailTempoDate2 { get; set; }
        public string RetailTempoDate2str { get; set; }
        public string RetailVehicleNo2 { get; set; }
        public decimal RetailTempoAmount2 { get; set; }
        public DateTime RetailTempoDate3 { get; set; }
        public string RetailTempoDate3str { get; set; }
        public string RetailVehicleNo3 { get; set; }
        public decimal RetailTempoAmount3 { get; set; }
        public decimal RetailTempoAmount { get; set; }
        public decimal TotalInward { get; set; }
        public decimal ChillarOutward { get; set; }
        public DateTime ExpensesDate { get; set; }
        public string ExpensesDatestr { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TransferVB2 { get; set; }
        public decimal TransferVB3 { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal TransferAmountInward { get; set; }
        public decimal TotalTransferAmount { get; set; }
        public long BankID1 { get; set; }
        public string BankName1 { get; set; }
        public decimal BankDepositeAmount1 { get; set; }
        public long BankID2 { get; set; }
        public string BankName2 { get; set; }
        public decimal BankDepositeAmount2 { get; set; }
        public long BankID3 { get; set; }
        public string BankName3 { get; set; }
        public decimal BankDepositeAmount3 { get; set; }
        public decimal BankDepositeAmount { get; set; }
        public decimal TotalOutward { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal OpeningChillar { get; set; }
        public decimal ClosingChillar { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public decimal Total { get; set; }
        public decimal InwardTotal { get; set; }
        public decimal BankAmount { get; set; }
        public decimal OutwardTotal { get; set; }
        public decimal FinalTotalOutward { get; set; }
        public decimal FinalTotalInward { get; set; }
        public string CreatedOnstr { get; set; }
        public decimal TotalInwardVehicleExpenses { get; set; }
        public decimal TotalOutwardVehicleExpenses { get; set; }
    }

    public class lnwardExport
    {
        public string GodownName { get; set; }
        public decimal Opening { get; set; }
        public decimal Chillar { get; set; }
        public decimal Wholesale { get; set; }
        public decimal Retail { get; set; }
        public string WholesaleTempoDate1 { get; set; }
        public string VehicleNo1 { get; set; }
        public decimal TempoAmount1 { get; set; }
        public string WholesaleTempoDate2 { get; set; }
        public string VehicleNo2 { get; set; }
        public decimal TempoAmount2 { get; set; }
        public string WholesaleTempoDate3 { get; set; }
        public string VehicleNo3 { get; set; }
        public decimal TempoAmount3 { get; set; }
        public string RetailTempoDate1 { get; set; }
        public string RetailVehicleNo1 { get; set; }
        public decimal RetailTempoAmount1 { get; set; }
        public string RetailTempoDate2 { get; set; }
        public string RetailVehicleNo2 { get; set; }
        public decimal RetailTempoAmount2 { get; set; }
        public string RetailTempoDate3 { get; set; }
        public string RetailVehicleNo3 { get; set; }
        public decimal RetailTempoAmount3 { get; set; }
        public decimal TransferAmountInward { get; set; }
        public decimal TotalInwardVehicleExpenses { get; set; }
        public decimal TotalInward { get; set; }
        public decimal ChillarOutward { get; set; }
        public decimal Expenses { get; set; }
        public string BankName1 { get; set; }
        public decimal BankDepositeAmount1 { get; set; }
        public string BankName2 { get; set; }
        public decimal BankDepositeAmount2 { get; set; }
        public string BankName3 { get; set; }
        public decimal BankDepositeAmount3 { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal TotalOutwardVehicleExpenses { get; set; }
        public decimal TotalOutward { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public partial class AddTransferAmount
    {
        public long TransferID { get; set; }
        public long FromGodownID { get; set; }
        public long ToGodownID { get; set; }
        public decimal Amount { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public partial class TransferAmountListResponse
    {
        public long TransferID { get; set; }
        public long FromGodownID { get; set; }
        public string FromGodownName { get; set; }
        public long ToGodownID { get; set; }
        public string ToGodownName { get; set; }
        public decimal Amount { get; set; }
        public string CreatedName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        //public long UpdatedBy { get; set; }
        //public DateTime UpdatedOn { get; set; }
        //public bool IsDelete { get; set; }
    }

    public class GetVehicleDetails
    {
        public string AreaName { get; set; }
        public long DeliveryPerson1 { get; set; }
        public long DeliveryPerson2 { get; set; }
        public long DeliveryPerson3 { get; set; }
        public long DeliveryPerson4 { get; set; }
        public long VehicleDetailID { get; set; }
        public string TempoNo { get; set; }
        public decimal WholesaleInvioceAmount1 { get; set; }
        public decimal WholesaleTotalKG1 { get; set; }
        public decimal WholesaleInvioceAmount2 { get; set; }
        public decimal WholesaleTotalKG2 { get; set; }
        public decimal RetailInvioceAmount1 { get; set; }
        public decimal RetailTotalKG1 { get; set; }
        public decimal RetailInvioceAmount2 { get; set; }
        public decimal RetailTotalKG2 { get; set; }
        public decimal OpeningKM { get; set; }
        public decimal DieselOpeningKM { get; set; }
    }

    public class AddVehicleCosting
    {
        public long VehicleCostingID { get; set; }
        public int WholesaleVehicleNo1 { get; set; }
        public DateTime WholesaleVehicleNo1Date { get; set; }
        public decimal WholesaleInvioceAmount1 { get; set; }
        public decimal WholesaleTotalKG1 { get; set; }
        public int WholesaleVehicleNo2 { get; set; }
        public DateTime WholesaleVehicleNo2Date { get; set; }
        public decimal WholesaleInvioceAmount2 { get; set; }
        public decimal WholesaleTotalKG2 { get; set; }
        public int RetailVehicleNo1 { get; set; }
        public DateTime RetailVehicleNo1Date { get; set; }
        public decimal RetailInvioceAmount1 { get; set; }
        public decimal RetailTotalKG1 { get; set; }
        public int RetailVehicleNo2 { get; set; }
        public DateTime RetailVehicleNo2Date { get; set; }
        public decimal RetailInvioceAmount2 { get; set; }
        public decimal RetailTotalKG2 { get; set; }
        public decimal GrandInvoiceAmount { get; set; }
        public decimal GrandTotalKG { get; set; }
        public long VehicleDetailID { get; set; }
        public string AreaName { get; set; }
        public long DeliveryPerson1 { get; set; }
        public long DeliveryPerson2 { get; set; }
        public long DeliveryPerson3 { get; set; }
        public long DeliveryPerson4 { get; set; }
        public decimal OpeningKM { get; set; }
        public decimal ClosingKM { get; set; }
        public decimal DieselOpeningKM { get; set; }
        public decimal DieselKM { get; set; }
        public decimal DieselAmount { get; set; }
        public decimal DieselLiter { get; set; }
        public decimal RepairingAmount { get; set; }
        public string RepairingDetail { get; set; }
        public decimal RepairingAmount2 { get; set; }
        public string RepairingDetail2 { get; set; }
        public decimal RepairingAmount3 { get; set; }
        public string RepairingDetail3 { get; set; }
        public decimal TollAmount { get; set; }
        public string TollDetail { get; set; }
        public decimal BharaiAmount { get; set; }
        public string BharaiDetail { get; set; }
        public decimal MiscellaneousAmount1 { get; set; }
        public decimal MiscellaneousAmount2 { get; set; }
        public decimal MiscellaneousAmount3 { get; set; }
        public string MiscellaneousDetail1 { get; set; }
        public string MiscellaneousDetail2 { get; set; }
        public string MiscellaneousDetail3 { get; set; }
        public DateTime VehicleOutDate { get; set; }
        public DateTime VehicleInDate { get; set; }
        public DateTime OutTime { get; set; }
        public DateTime InTime { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class VehicleCostingListResponse
    {
        public long VehicleCostingID { get; set; }
        public string WholesaleVehicleNo1 { get; set; }
        public decimal WholesaleInvioceAmount1 { get; set; }
        public decimal WholesaleTotalKG1 { get; set; }
        public string WholesaleVehicleNo2 { get; set; }
        public decimal WholesaleInvioceAmount2 { get; set; }
        public decimal WholesaleTotalKG2 { get; set; }
        public string RetailVehicleNo1 { get; set; }
        public decimal RetailInvioceAmount1 { get; set; }
        public decimal RetailTotalKG1 { get; set; }
        public string RetailVehicleNo2 { get; set; }
        public decimal RetailInvioceAmount2 { get; set; }
        public decimal RetailTotalKG2 { get; set; }
        public decimal GrandInvoiceAmount { get; set; }
        public decimal GrandTotalKG { get; set; }
        public long VehicleDetailID { get; set; }
        public string VehicleNumber { get; set; }
        public string AreaName { get; set; }
        public long DeliveryPerson1 { get; set; }
        public string DeliveryPersonName1 { get; set; }
        public long DeliveryPerson2 { get; set; }
        public string DeliveryPersonName2 { get; set; }
        public long DeliveryPerson3 { get; set; }
        public string DeliveryPersonName3 { get; set; }
        public long DeliveryPerson4 { get; set; }
        public string DeliveryPersonName4 { get; set; }
        public string DeliveryPersons { get; set; }
        public decimal OpeningKM { get; set; }
        public decimal ClosingKM { get; set; }
        public DateTime OutDateTime { get; set; }
        public string OutDatestr { get; set; }
        public string OutTimestr { get; set; }
        public DateTime InDateTime { get; set; }
        public string InDatestr { get; set; }
        public string InTimestr { get; set; }
        public string InDateTimestr { get; set; }
        public string TotalHrs { get; set; }
        public decimal DieselOpeningKM { get; set; }
        public decimal DieselKM { get; set; }
        public decimal DieselAmount { get; set; }
        public decimal DieselLiter { get; set; }
        public decimal RepairingAmount { get; set; }
        public string RepairingDetail { get; set; }
        public decimal RepairingAmount2 { get; set; }
        public string RepairingDetail2 { get; set; }
        public decimal RepairingAmount3 { get; set; }
        public string RepairingDetail3 { get; set; }
        public decimal TollAmount { get; set; }
        public string TollDetail { get; set; }
        public decimal BharaiAmount { get; set; }
        public string BharaiDetail { get; set; }
        public decimal MiscellaneousAmount1 { get; set; }
        public decimal MiscellaneousAmount2 { get; set; }
        public decimal MiscellaneousAmount3 { get; set; }
        public string MiscellaneousDetail1 { get; set; }
        public string MiscellaneousDetail2 { get; set; }
        public string MiscellaneousDetail3 { get; set; }
        public DateTime VehicleOutDate { get; set; }
        public DateTime VehicleInDate { get; set; }
        public DateTime OutTime { get; set; }
        public DateTime InTime { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnstr { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class VehicleCostingListReport
    {
        public long VehicleCostingID { get; set; }
        public long VehicleDetailID { get; set; }
        public string VehicleNumber { get; set; }
        public decimal DieselOpeningKM { get; set; }
        public decimal DieselClosingKM { get; set; }
        public decimal TotalDieselAmount { get; set; }
        public decimal TotalDieselLiter { get; set; }
        public decimal DieselDiffKM { get; set; }
        public decimal AvgDieselCost { get; set; }
        public decimal AvgKMPerLtr { get; set; }
        public decimal OpeningKM { get; set; }
        public decimal ClosingKM { get; set; }
        public decimal DiffKM { get; set; }
        public decimal TotalRepairingAmount { get; set; }
        public decimal RepairingCostPerKM { get; set; }
        public decimal TotalTollAmount { get; set; }
        public decimal TollCostPerKM { get; set; }
        public decimal TotalBharaiAmount { get; set; }
        public decimal BharaiCostPerKM { get; set; }
        public decimal TotalMiscellaneousAmount { get; set; }
        public decimal MiscellaneousCostPerKM { get; set; }
        public decimal TotalCostPerKM { get; set; }
        public decimal ActualTotalCost { get; set; }
        public decimal GrandInvoiceAmount { get; set; }
        public decimal PerOfInvoiceAmount { get; set; }
        public decimal GrandTotalKG { get; set; }
    }

    public class VehicleCostingListExport
    {
        public string VehicleNumber { get; set; }
        public decimal DieselOpeningKM { get; set; }
        public decimal DieselClosingKM { get; set; }
        public decimal DieselDiffKM { get; set; }
        public decimal TotalDieselAmount { get; set; }
        public decimal AvgDieselCost { get; set; }
        public decimal TotalDieselLiter { get; set; }
        public decimal AvgKMPerLtr { get; set; }
        public decimal OpeningKM { get; set; }
        public decimal ClosingKM { get; set; }
        public decimal DiffKM { get; set; }
        public decimal TotalRepairingAmount { get; set; }
        public decimal RepairingCostPerKM { get; set; }
        public decimal TotalTollAmount { get; set; }
        public decimal TollCostPerKM { get; set; }
        public decimal TotalBharaiAmount { get; set; }
        public decimal BharaiCostPerKM { get; set; }
        public decimal TotalMiscellaneousAmount { get; set; }
        public decimal MiscellaneousCostPerKM { get; set; }
        public decimal TotalCostPerKM { get; set; }
        public decimal ActualTotalCost { get; set; }
        public decimal TotalInvoiceAmt { get; set; }
        public decimal PerOfInvoiceAmount { get; set; }
        public decimal TotalKG { get; set; }
    }

    public class DeActiveInwardVoucherListResponse
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Status { get; set; }
        public string StatusStr { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class PouchInwardOutward
    {
        public long PouchInwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long PouchNameID { get; set; }
        public long PouchID { get; set; }
        public long OpeningPouch { get; set; }
        public long NoofPcs { get; set; }
        public long TotalPouch { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
        public long SupplierID { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalInwardCost { get; set; }
        public int CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public string CreatedOnstr { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<PouchInwardCost> lstPouchInwardCost { get; set; }
    }

    public class PouchInwardCost
    {
        public long PouchCostID { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class PouchInwardListResponse
    {
        public long PouchInwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long PouchID { get; set; }
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }
        public long OpeningPouch { get; set; }
        public long NoofPcs { get; set; }
        public long TotalPouch { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalInwardCost { get; set; }
        public int CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public string CreatedOnstr { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<PouchInwardCost> lstPouchInwardCost { get; set; }
    }

    public class PouchOutwardListResponse
    {
        public long PouchInwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long PouchNameID { get; set; }
        public long PouchID { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }
        public long OpeningPouch { get; set; }
        public long NoofPcs { get; set; }
        public long TotalPouch { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
        public int CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public string CreatedOnstr { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class GetOpeningPouch
    {
        public long OpeningPouch { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long PouchNameID { get; set; }
        public long PouchTransferID { get; set; }
        public long TransferNoofPcs { get; set; }
        public string PouchName { get; set; }
    }

    public class AddNoOfPouchTransfer
    {
        public long PouchTransferID { get; set; }
        public long FromGodownID { get; set; }
        public long PouchID { get; set; }
        public long PouchNameID { get; set; }
        public long OpeningPouch { get; set; }
        public long TransferNoofPcs { get; set; }
        public long TotalPouch { get; set; }
        public DateTime TransferDate { get; set; }
        public long ToGodownID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public long CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
    }

    public class PouchTransferListResponse
    {
        public long PouchTransferID { get; set; }
        public long FromGodownID { get; set; }
        public string FromGodownName { get; set; }
        public long PouchID { get; set; }
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }
        public long OpeningPouch { get; set; }
        public long TransferNoofPcs { get; set; }
        public long TotalPouch { get; set; }
        public DateTime TransferDate { get; set; }
        public string TransferDatestr { get; set; }
        public long ToGodownID { get; set; }
        public string ToGodownName { get; set; }
        public bool IsAccept { get; set; }
        public string Status { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
        public long AcceptBy { get; set; }
        public DateTime AcceptDate { get; set; }
        public string AcceptDatestr { get; set; }
        public string AcceptedName { get; set; }
        public long CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
    }




    // Utility Stock 02-03-2020     
    public class UtilityInwardOutward
    {
        public long UtilityInwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long UtilityNameID { get; set; }
        public long UtilityID { get; set; }
        public long OpeningUtility { get; set; }
        public long NoofPcs { get; set; }
        public long TotalUtility { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
        public long SupplierID { get; set; }
        public string Identification { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalInwardCost { get; set; }
        public int CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public string CreatedOnstr { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<UtilityInwardCost> lstUtilityInwardCost { get; set; }
    }

    public class UtilityInwardCost
    {
        public long UtilityCostID { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class GetOpeningUtility
    {
        public long OpeningUtility { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        //public long UtilityID { get; set; }
        public decimal UtilityNameID { get; set; }
        public long UtilityTransferID { get; set; }
        public long TransferNoofPcs { get; set; }
        public string UtilityName { get; set; }
    }

    public class UtilityInwardListResponse
    {
        public long UtilityInwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long UtilityID { get; set; }
        public long UtilityNameID { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public long OpeningUtility { get; set; }
        public long NoofPcs { get; set; }
        public long TotalUtility { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalInwardCost { get; set; }
        public int CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public string CreatedOnstr { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<UtilityInwardCost> lstUtilityInwardCost { get; set; }
    }

    public class UtilityOutwardListResponse
    {
        public long UtilityInwardID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long UtilityNameID { get; set; }
        public long UtilityID { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public long OpeningUtility { get; set; }
        public long NoofPcs { get; set; }
        public long TotalUtility { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
        public int CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
        public string CreatedOnstr { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddNoOfUtilityTransfer
    {
        public long UtilityTransferID { get; set; }
        public long FromGodownID { get; set; }
        public long UtilityNameID { get; set; }
        public long UtilityID { get; set; }
        public long OpeningUtility { get; set; }
        public long TransferNoofPcs { get; set; }
        public long TotalUtility { get; set; }
        public DateTime TransferDate { get; set; }
        public long ToGodownID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public long CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
    }

    public class UtilityTransferListResponse
    {
        public long UtilityTransferID { get; set; }
        public long FromGodownID { get; set; }
        public string FromGodownName { get; set; }
        public long UtilityNameID { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public long OpeningUtility { get; set; }
        public long TransferNoofPcs { get; set; }
        public long TotalUtility { get; set; }
        public DateTime TransferDate { get; set; }
        public string TransferDatestr { get; set; }
        public long ToGodownID { get; set; }
        public string ToGodownName { get; set; }
        public bool IsAccept { get; set; }
        public string Status { get; set; }
        public long CreatedBy { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
        public long AcceptBy { get; set; }
        public DateTime AcceptDate { get; set; }
        public string AcceptDatestr { get; set; }
        public string AcceptedName { get; set; }
        public long CreditDebitStatusID { get; set; }
        public string CreditDebitStatus { get; set; }
    }


    public class VehicleNoListInwardResponse
    {
        public string VehicleNo { get; set; }
    }

    public class RetailVehicleNoListInwardResponse
    {
        public string VehicleNo { get; set; }
    }

    public class GetVehicleExpensesInwardAmount
    {
        public decimal Amount { get; set; }
    }

    public class WCustomerNameList
    {
        public long WCustomerID { get; set; }
        public string WCustomerName { get; set; }
    }

    public class RCustomerNameList
    {
        public long RCustomerID { get; set; }
        public string RCustomerName { get; set; }
    }

    public class UtilityStockResponse
    {
        public string GodownName { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public long MinUtility { get; set; }
        public long TotalUtility { get; set; }
        public long UtilityInwardID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
    }

    public class UtilityStockExp
    {
        public string Godown { get; set; }
        public string Utility { get; set; }
        public long MinUtility { get; set; }
        public long TotalUtility { get; set; }
        public string PurchaseDate { get; set; }
    }

    public class PouchStockResponse
    {
        public string GodownName { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }
        public long MinPouchQuantity { get; set; }
        public long TotalPouch { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDatestr { get; set; }
    }

    public class PouchStockExp
    {
        public string Godown { get; set; }
        public string Pouch { get; set; }
        public string HSNNo { get; set; }
        public long MinPouch { get; set; }
        public long TotalPouch { get; set; }
        public string PurchaseDate { get; set; }
    }

    public class UtilityExport
    {
        public string Utility { get; set; }
        public string HSNNo { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Godown { get; set; }
        public long MinQuantity { get; set; }
    }

    public class UtilityInwardExport
    {
        public string Godown { get; set; }
        public string Utility { get; set; }
        public string HSNNo { get; set; }
        public long Opening { get; set; }
        public long NoofPcs { get; set; }
        public long Total { get; set; }
        public string PurchaseDate { get; set; }
        public string Supplier { get; set; }
        public string InvoiceNumber { get; set; }
        public string PreparedBy { get; set; }
    }

    public class UtilityOutwardExport
    {
        public string Godown { get; set; }
        public string Utility { get; set; }
        public string HSNNo { get; set; }
        public long Opening { get; set; }
        public long NoofPcs { get; set; }
        public long Total { get; set; }
        public string PurchaseDate { get; set; }
        public string PreparedBy { get; set; }
    }

    public class StockTransferExp
    {
        public string FromGodown { get; set; }
        public string ToGodown { get; set; }
        public string Utility { get; set; }
        public string HSNNo { get; set; }
        public long NoofPcs { get; set; }
        public string TransferDate { get; set; }
        public string PreparedBy { get; set; }
        public string AcceptDate { get; set; }
        public string AcceptedBy { get; set; }
        public string Status { get; set; }
    }

    public class PouchInwardExport
    {
        public string Godown { get; set; }
        public string Pouch { get; set; }
        public string HSNNo { get; set; }
        public long Opening { get; set; }
        public long NoofPcs { get; set; }
        public long Total { get; set; }
        public string PurchaseDate { get; set; }
        public string Supplier { get; set; }
        public string InvoiceNumber { get; set; }
        public string PreparedBy { get; set; }
    }

    public class PouchOutwardExport
    {
        public string Godown { get; set; }
        public string Pouch { get; set; }
        public string HSNNo { get; set; }
        public long Opening { get; set; }
        public long NoofPcs { get; set; }
        public long Total { get; set; }
        public string PurchaseDate { get; set; }
        public string PreparedBy { get; set; }
    }

    public class PouchStockTransferExp
    {
        public string FromGodown { get; set; }
        public string ToGodown { get; set; }
        public string Pouch { get; set; }
        public string HSNNo { get; set; }
        public long NoofPcs { get; set; }
        public string TransferDate { get; set; }
        public string PreparedBy { get; set; }
        public string AcceptDate { get; set; }
        public string AcceptedBy { get; set; }
        public string Status { get; set; }
    }

    public class TransferPaymentModel
    {
        public long TransferID { get; set; }
        public long FromGodownID { get; set; }
        public long ToGodownID { get; set; }
        public decimal Amount { get; set; }
    }
}
