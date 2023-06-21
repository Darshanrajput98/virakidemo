﻿

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PendingDeliveryListResponse
    {
        public long DeliveryID { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public decimal FinalTotal { get; set; }
        public int Quantity { get; set; }
        public string ShipTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Vehicle { get; set; }
        public string Tray { get; set; }
        public string Other { get; set; }
        public string Bag { get; set; }
        public string Jabla { get; set; }
        public string PackBag { get; set; }
    }

    public class OrderPendingRequest
    {
        public long DeliveryID { get; set; }
        public long OrderID { get; set; }
        public int VehicleNo { get; set; }
        public string Tray { get; set; }
        public string Other { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool PendingDelivery { get; set; }
        public bool DeliveryCompletedDate { get; set; }
        public DateTime AssignedDate { get; set; }
    }

    public class DeliveryStatus
    {
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public long DeliveryPerson1 { get; set; }
        public long DeliveryPerson2 { get; set; }
        public long DeliveryPerson3 { get; set; }
        public long DeliveryPerson4 { get; set; }
        public string DeliveryPersonName1 { get; set; }
        public string DeliveryPersonName2 { get; set; }
        public string DeliveryPersonName3 { get; set; }
        public string DeliveryPersonName4 { get; set; }
        public string VehicleNo { get; set; }
    }

    public class DeliveryStatusListResponse
    {
        public long DeliveryAllocationID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long VehicleNo { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string TempoNo { get; set; }
        public long DeliveryPerson1 { get; set; }
        public long DeliveryPerson2 { get; set; }
        public long DeliveryPerson3 { get; set; }
        public long DeliveryPerson4 { get; set; }
        public string AreaID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string Customer { get; set; }
        public string Area { get; set; }
        public long PaymentID { get; set; }
        public string Container { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal ByCash { get; set; }
        public bool ByCheque { get; set; }
        public bool BySign { get; set; }
        public bool ByCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool ByOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public bool PackedBy { get; set; }
        public long PackedByID { get; set; }
        public string Tray { get; set; }
        public string Jabla { get; set; }
        public string Bag { get; set; }
        public string PackBag { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsDelete { get; set; }
        public string Remark { get; set; }
        public long OrderID { get; set; }
        public long DeliveryType { get; set; }
        public decimal AdjustAmount { get; set; }
        public int DisableRow { get; set; }
        public long OrderQtyID { get; set; }
        public long DeliveryID { get; set; }
        public string CustBankName { get; set; }
        public string CustBranch { get; set; }
        public string CustIFCCode { get; set; }
        public DateTime PaymentCreateOn { get; set; }
        public string PaymentCreateOn1 { get; set; }

        public string Email { get; set; }
        public string PDFName { get; set; }
        public string MobileNumber { get; set; }
        public long GodownID { get; set; }
        public DateTime ActualCreatedDate { get; set; }
        public string ActualCreatedDatestr { get; set; }

    }

    public class DeliverAllocation
    {
        public long DeliveryAllocationID { get; set; }
        public long VehicleNo { get; set; }
        public string TempoNo { get; set; }
        public long DeliveryPerson1 { get; set; }
        public long DeliveryPerson2 { get; set; }
        public long DeliveryPerson3 { get; set; }
        public long DeliveryPerson4 { get; set; }
        public string AreaID { get; set; }
        public List<DeliveryStatusListResponse> lstDelAllocation { get; set; }
    }

    public class TempoSummary
    {
        public string VehicleSummary { get; set; }
    }

    public class DeliveryStatusListPrint
    {
        public string InvoiceNumber { get; set; }
        public string Customer { get; set; }
        public string Area { get; set; }
        public string Container { get; set; }
        public decimal PaymentAmount { get; set; }
        public long OrderID { get; set; }
        public string Email { get; set; }
        public string PDFName { get; set; }
        public string MobileNumber { get; set; }
    }

    public class DeliverAllocationPrint
    {
        public string VehicleNo { get; set; }
        public string TempoNo { get; set; }
        public string DeliveryPerson1 { get; set; }
        public string DeliveryPerson2 { get; set; }
        public string DeliveryPerson3 { get; set; }
        public string DeliveryPerson4 { get; set; }
        public string AreaID { get; set; }
        public bool SendEmail { get; set; }
        public List<DeliveryStatusListPrint> lstDelAllocation { get; set; }
    }

    public class DeliverAllocationPrintList
    {
        public string VehicleNo { get; set; }
        public string TempoNo { get; set; }
        public string DeliveryPerson1 { get; set; }
        public string DeliveryPerson2 { get; set; }
        public string DeliveryPerson3 { get; set; }
        public string DeliveryPerson4 { get; set; }
        public string Area { get; set; }
    }

    public class DeliverySheetList
    {
        public DateTime AssignedDate { get; set; }
        public List<string> VehicleNo { get; set; }
        public string VehicleNo1 { get; set; }
        public string TempoNo { get; set; }
        public string Customer { get; set; }
        public string Area { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ChequeTotal { get; set; }
        public decimal CardTotal { get; set; }
        public decimal SignTotal { get; set; }
        public decimal OnlineTotal { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal AdjustAmountTotal { get; set; }
        public string Remarks { get; set; }
        public long OrderID { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string ChequeDate1 { get; set; }
        public string IFCCode { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime OnlinePaymentDate { get; set; }
        public string OnlinePaymentDate1 { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public long PaymentID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string CashOption { get; set; }
        public bool IsCheckBySign { get; set; }
    }

    public class DeliverySheetListForExport
    {
        public string Customer { get; set; }
        public string Area { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        public string Remarks { get; set; }
        public string Godown { get; set; }
        public string VehicleNo { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public string OnlinePaymentDate { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
    }

    public class CashCounterListResponse
    {
        public DateTime AssignedDate { get; set; }
        public List<string> VehicleNo { get; set; }
        public string VehicleNo1 { get; set; }
        public string TempoNo { get; set; }
        public string Customer { get; set; }
        public string Area { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ChequeTotal { get; set; }
        public decimal CardTotal { get; set; }
        public decimal SignTotal { get; set; }
        public decimal OnlineTotal { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal AdjustAmountTotal { get; set; }
        public string Remarks { get; set; }
        public long OrderID { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string ChequeDate1 { get; set; }
        public string IFCCode { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime OnlinePaymentDate { get; set; }
        public string OnlinePaymentDate1 { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public bool IsDelivered { get; set; }
        public string IsDeliveredstr { get; set; }
        public long PaymentID { get; set; }

        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string CashOption { get; set; }
        public bool IsCheckBySign { get; set; }

        //Add By Dhruvik
        public decimal TotalCheque { get; set; }

        public decimal ChequeReturnCharges { get; set; }
        public decimal ByCash { get; set; }
        public decimal ChequeReturnAmount { get; set; }
    }

    public class CashCounterListForExport
    {
        public string Customer { get; set; }
        public string Area { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        public string Remarks { get; set; }
        public string Godown { get; set; }
        public string VehicleNo { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public string OnlinePaymentDate { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public string IsDeliveredstr { get; set; }

        
    }

    public class PackedByViewModel
    {
        public long PackedByID { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public string Tray { get; set; }
        public string Bag { get; set; }
        public string Jabla { get; set; }
        public string PackBag { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class CashCounterDayWiseSalesManList
    {
        public DateTime? AssignedDate { get; set; }
        public long UserID { get; set; }
        public string SalesPerson { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ChequeTotal { get; set; }
        public decimal CardTotal { get; set; }
        public decimal SignTotal { get; set; }
        public decimal OnlineTotal { get; set; }
        public decimal AdjustAmountTotal { get; set; }
        public long GodownID { get; set; }

        public bool IsCheckBySign { get; set; }
    }

    public class DayWiseSalesManListForExport
    {
        public string SalesPerson { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        //public decimal CashTotal { get; set; }
        //public decimal ChequeTotal { get; set; }
        //public decimal CardTotal { get; set; }
        //public decimal SignTotal { get; set; }
        //public decimal OnlineTotal { get; set; }
        //public decimal AdjustAmountTotal { get; set; }
    }

    public class GenerateOriginalInvoice
    {
        public List<GenerateOriginalInvoiceListPrint> lstInvoice { get; set; }
    }

    public class GenerateOriginalInvoiceListPrint
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
    }


    public class VoucherCashCounterListResponse
    {
        public DateTime AssignedDate { get; set; }
        public long GodownID { get; set; }

        public long ExpensesVoucherID { get; set; }
        public long PaymentID { get; set; }
        public string Customer { get; set; }
        public string Area { get; set; }
        public string BillNumber { get; set; }
        public decimal Cash { get; set; }
        public decimal CashTotal { get; set; }
        public decimal Cheque { get; set; }
        public decimal ChequeTotal { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal CardTotal { get; set; }
        public decimal SignTotal { get; set; }
        public decimal Online { get; set; }
        public decimal OnlineTotal { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal AdjustAmountTotal { get; set; }
        public string Remarks { get; set; }
        public string GodownName { get; set; }
        public string VehicleNo1 { get; set; }
        public string CashOption { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string ChequeDate1 { get; set; }
        public string IFCCode { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime OnlinePaymentDate { get; set; }
        public string OnlinePaymentDate1 { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public string IsDeliveredstr { get; set; }
    }

    public class VoucherCashCounterDayWiseSalesManList
    {
        public DateTime? AssignedDate { get; set; }
        public long GodownID { get; set; }
        public long UserID { get; set; }
        public string SalesPerson { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ChequeTotal { get; set; }
        public decimal CardTotal { get; set; }
        public decimal SignTotal { get; set; }
        public decimal OnlineTotal { get; set; }
        public decimal AdjustAmountTotal { get; set; }
    }

    public class VoucherCashCounterExp
    {
        public string Customer { get; set; }
        public string Area { get; set; }
        public string BillNumber { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        public string Remarks { get; set; }
        public string Godown { get; set; }
        public string VehicleNo { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public string OnlinePaymentDate { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public string IsDeliveredstr { get; set; }
    }

    public class VoucherDayWiseSalesManListExp
    {
        public string SalesPerson { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Sign { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        //public decimal CashTotal { get; set; }
        //public decimal ChequeTotal { get; set; }
        //public decimal CardTotal { get; set; }
        //public decimal OnlineTotal { get; set; }
        //public decimal AdjustAmountTotal { get; set; }
    }

    public class DayWiseSalesApproveList
    {
        public long UserID { get; set; }
        public string State { get; set; }
        public string TaxNo { get; set; }
        public string InvoiceDate { get; set; }
        public long OrderID { get; set; }
        public string InvCode { get; set; }
        public DateTime? InvDate { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal GrandGrossAmt { get; set; }
        public decimal GrandCGSTAmt { get; set; }
        public decimal GrandSGSTAmt { get; set; }
        public decimal GrandIGSTAmt { get; set; }
        public decimal Discount { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public string UserFullName { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal GrandTCSAmt { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrandNetAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal diff { get; set; }
        public decimal Granddiff { get; set; }
        public long CustomerNumber { get; set; }
        public decimal CreditedTotal { get; set; }
        public decimal CreditedFinalTotal { get; set; }
        public Boolean IsDelete { get; set; }
        public Boolean IsApprove { get; set; }
    }
    
}
