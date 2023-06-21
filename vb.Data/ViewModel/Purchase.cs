using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vb.Data
{
    public class SupplierName1
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
    }

    public class GetSupplierTax
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public long TaxID { get; set; }
        public string TaxName { get; set; }
        public string DeliveryTo { get; set; }
        public string ShipTo { get; set; }
        public decimal TDSPercentage { get; set; }
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
    }

    public class GetProductDetaiForPurchase
    {
        public string UnitCode { get; set; }
        public string HSNNumber { get; set; }
        public decimal Tax { get; set; }
        public long CategoryTypeID { get; set; }
        // public decimal ProductPrice { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal TaxAmtIGST { get; set; }
        public decimal TaxAmtCGST { get; set; }
        public decimal TaxAmtSGST { get; set; }
    }

    public class AddPurchaseDetail
    {
        public long PurchaseID { get; set; }
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ShipTo { get; set; }
        public string Tax { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }

        public DateTime? BillDate2 { get; set; }


        public string BillDateStr { get; set; }
        public long PurchaseTypeID { get; set; }
        public string DeliveryChallanNumber { get; set; }
        public DateTime? DeliveryChallanDate { get; set; }
        public string DeliveryChallanDateStr { get; set; }
        public long PurchaseDebitAccountTypeID { get; set; }
        public long BrokerID { get; set; }
        public string EWayNumber { get; set; }
        public long GodownID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public bool Verify { get; set; }
        public bool DeActiveSupplier { get; set; }
        public long AvakNumber { get; set; }
        public decimal TDSPercentage { get; set; }
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public List<AddPurchaseQtyDetail> lstPurchaseQty { get; set; }

    }

    public class AddPurchaseQtyDetail
    {
        public long PurchaseID { get; set; }
        public long PurchaseQtyID { get; set; }
        public long CategoryTypeID { get; set; }
        public long ProductID { get; set; }
        public string HSNNumber { get; set; }
        public string VakalNumber { get; set; }
        public int NoofBags { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal RatePerKG { get; set; }
        public decimal Amount { get; set; }
        public decimal RatePerBags { get; set; }
        public decimal PackingChargesBag { get; set; }
        public decimal TotalPackingCharge { get; set; }
        public decimal Hamali { get; set; }
        public decimal LessDiscount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal APMC { get; set; }
        public decimal APMCAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal CGSTTax { get; set; }
        public decimal CGSTTaxAmount { get; set; }
        public decimal SGSTTax { get; set; }
        public decimal SGSTTaxAmount { get; set; }
        public decimal IGSTTax { get; set; }
        public decimal IGSTTaxAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Insurance { get; set; }
        public decimal TransportInward { get; set; }
        public decimal TCSAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandTotalAmount { get; set; }

        public decimal TDSTax { get; set; }
        public decimal TDSTaxAmount { get; set; }
        public decimal AmountAfterTDS { get; set; }


        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PurcahseListResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long SupplierID { get; set; }
        public long PurchaseID { get; set; }
        public string SupplierName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public string AreaName { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string EWayNumber { get; set; }
        public long AvakNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string GodownName { get; set; }
    }

    public class DayWisePurchaseList
    {
        public long PurchaseID { get; set; }
        public long GodownID { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string BillNumber { get; set; }
        public long SupplierID { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalPackingCharge { get; set; }
        public decimal Hamali { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal APMCPer { get; set; }
        public decimal APMCAmount { get; set; }
        public decimal CGSTTax { get; set; }
        public decimal CGSTTaxAmount { get; set; }
        public decimal SGSTTax { get; set; }
        public decimal SGSTTaxAmount { get; set; }
        public decimal IGSTTax { get; set; }
        public decimal IGSTTaxAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Insurance { get; set; }
        public decimal TransportInward { get; set; }
        public decimal TCSAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public decimal SumAmount { get; set; }
        public decimal SumTotalPackingCharge { get; set; }
        public decimal SumHamali { get; set; }
        public decimal SumDiscountAmount { get; set; }
        public decimal SumAPMCAmount { get; set; }
        public decimal SumCGSTTaxAmount { get; set; }
        public decimal SumSGSTTaxAmount { get; set; }
        public decimal SumIGSTTaxAmount { get; set; }
        public decimal SumTotalTaxAmount { get; set; }
        public decimal SumTotalAmount { get; set; }
        public decimal SumInsurance { get; set; }
        public decimal SumTransportInward { get; set; }
        public decimal SumTCSAmount { get; set; }
        public decimal SumGrandTotalAmount { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal GrandNetAmount { get; set; }
        public Boolean IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PurchaseDatestr { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDatestr { get; set; }
        public string DebitAccountType { get; set; }
        public string BrokerName { get; set; }
        public int NoofBags { get; set; }
        public decimal WeightPerBag { get; set; }
        public string PurchaseType { get; set; }
        public string GodownName { get; set; }
        public string EWayNumber { get; set; }
        public long AvakNumber { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public string VakalNumber { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal RatePerKG { get; set; }
        public decimal RatePerBags { get; set; }
        public decimal PackingChargesBag { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }

        public decimal TDSTax { get; set; }
        public decimal TDSTaxAmount { get; set; }
        public decimal SumTDSTaxAmount { get; set; }
        public decimal AmountAfterTDS { get; set; }
        public decimal SumAmountAfterTDS { get; set; }
    }

    public class DayWisePurchaseListForExp
    {
        public string AavakDate { get; set; }
        public long AvakNumber { get; set; }
        // public string PurchaseDate { get; set; }
        public string PurchaseType { get; set; }
        public string GodownName { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public string DebitAccountType { get; set; }


        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public string VakalNumber { get; set; }
        public int NoofBags { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal RatePerKG { get; set; }
        public decimal Amount { get; set; }
        public decimal WeightPerBag { get; set; }
        public decimal RatePerBags { get; set; }
        public decimal PackingChargesBag { get; set; }
        public decimal PackingCharge { get; set; }
        //public decimal Discount { get; set; }
        public decimal Rebate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal APMC { get; set; }
        public decimal APMCAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal CGST { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGST { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Hamali { get; set; }
        public decimal TransportInward { get; set; }
        public decimal Insurance { get; set; }
        public decimal TaxCollectedAtSourceReceiver { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TDSTax { get; set; }
        public decimal TDSTaxAmount { get; set; }
        public decimal AmountAfterTDS { get; set; }
        public string BrokerName { get; set; }
        public string EWayNumber { get; set; }

        //public decimal TotalTaxAmount { get; set; }
        //public decimal TotalAmount { get; set; }         
        //public decimal GrandTotalAmount { get; set; } 
    }

    public class PurchasePaidPaymentList
    {
        public DateTime PaymentDate { get; set; }

        public long PaymentMode { get; set; }
        public long BankID { get; set; }

        //public DateTime To { get; set; }
        public long SupplierID { get; set; }
        public long AreaID { get; set; }
        public long PaymentID { get; set; }
        public long PurchaseID { get; set; }
        public string Bank_Name { get; set; }
        public string SupplierBankName { get; set; }
        public string Payment_Type { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Payment_Date { get; set; }
        public decimal Amount { get; set; }
        public string SupplierName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string BillNumber { get; set; }
        public DateTime? BillDate { get; set; }
        public string BillDatestr { get; set; }
        public decimal BillAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string AreaName { get; set; }

        public bool ByCheque { get; set; }
        public bool ByCard { get; set; }
        public bool ByOnline { get; set; }


        public string Client_Code { get; set; }
        public string Product_Code { get; set; }
        public string Payment_Ref_No { get; set; }
        public string Instrument_Date { get; set; }
        public string Dr_Ac_No { get; set; }
        public string Bank_Code_Indicator { get; set; }
        public string Beneficiary_Code { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Location { get; set; }
        public string Print_Location { get; set; }
        public string Instrument_Number { get; set; }
        public string Ben_Add1 { get; set; }
        public string Ben_Add2 { get; set; }
        public string Ben_Add3 { get; set; }
        public string Ben_Add4 { get; set; }
        public string Debit_Narration { get; set; }
        public string Credit_Narration { get; set; }
        public string Payment_Details_1 { get; set; }
        public string Payment_Details_2 { get; set; }
        public string Payment_Details_3 { get; set; }
        public string Payment_Details_4 { get; set; }
        public string Deductions { get; set; }
        public string Enrichment_6 { get; set; }
        public string Enrichment_7 { get; set; }
        public string Enrichment_8 { get; set; }
        public string Enrichment_9 { get; set; }
        public string Enrichment_10 { get; set; }
        public string Enrichment_11 { get; set; }
        public string Enrichment_12 { get; set; }
        public string Enrichment_13 { get; set; }
        public string Enrichment_14 { get; set; }
        public string Enrichment_15 { get; set; }
        public string Enrichment_16 { get; set; }
        public string Enrichment_17 { get; set; }
        public string Enrichment_18 { get; set; }
        public string Enrichment_19 { get; set; }
        public string Enrichment_20 { get; set; }

        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Online { get; set; }
        public long GodownID { get; set; }
        public string PaymentDatestr { get; set; }
        public string BankBranchViraki { get; set; }
        public string BankIFSCCodeViraki { get; set; }
        public string ChequeNo { get; set; }
        public string TypeOfCard { get; set; }
        public string UTRNumber { get; set; }
        public decimal sumAmount { get; set; }
        public decimal sumBillAmount { get; set; }
        public decimal sumNetAmount { get; set; }
        public string NameAsBankAccount { get; set; }
    }

    public class PurchasePaidPaymentListExport
    {
        public string Bank_Name { get; set; }
        public string Client_Code { get; set; }
        public string Product_Code { get; set; }
        public string Payment_Type { get; set; }
        public string Payment_Ref_No { get; set; }
        public string Payment_Date { get; set; }
        public string Instrument_Date { get; set; }
        public string Dr_Ac_No { get; set; }
        public decimal Amount { get; set; }
        public string Bank_Code_Indicator { get; set; }
        public string Beneficiary_Code { get; set; }
        public string EmployeeName { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Beneficiary_Branch_IFSC_Code { get; set; }
        public string Beneficiary_Acc_No { get; set; }
        public string Location { get; set; }
        public string Print_Location { get; set; }
        public string Instrument_Number { get; set; }
        public string Ben_Add1 { get; set; }
        public string Ben_Add2 { get; set; }
        public string Ben_Add3 { get; set; }
        public string Ben_Add4 { get; set; }
        public string Beneficiary_Email { get; set; }
        public string Beneficiary_Mobile { get; set; }
        public string Debit_Narration { get; set; }
        public string Credit_Narration { get; set; }
        public string Payment_Details_1 { get; set; }
        public string Payment_Details_2 { get; set; }
        public string Payment_Details_3 { get; set; }
        public string Payment_Details_4 { get; set; }
        public string Bill_No { get; set; }
        public string Bill_Date { get; set; }
        public decimal Bill_Amt { get; set; }
        public string Deductions { get; set; }
        public decimal Net { get; set; }
        public string Enrichment_6 { get; set; }
        public string Enrichment_7 { get; set; }
        public string Enrichment_8 { get; set; }
        public string Enrichment_9 { get; set; }
        public string Enrichment_10 { get; set; }
        public string Enrichment_11 { get; set; }
        public string Enrichment_12 { get; set; }
        public string Enrichment_13 { get; set; }
        public string Enrichment_14 { get; set; }
        public string Enrichment_15 { get; set; }
        public string Enrichment_16 { get; set; }
        public string Enrichment_17 { get; set; }
        public string Enrichment_18 { get; set; }
        public string Enrichment_19 { get; set; }
        public string Enrichment_20 { get; set; }
    }

    public class UnVerifyPendingPurchaseAavakList
    {
        public long PurchaseID { get; set; }
        public long GodownID { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string BillNumber { get; set; }
        public long SupplierID { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalPackingCharge { get; set; }
        public decimal Hamali { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal APMCPer { get; set; }
        public decimal APMCAmount { get; set; }
        public decimal CGSTTax { get; set; }
        public decimal CGSTTaxAmount { get; set; }
        public decimal SGSTTax { get; set; }
        public decimal SGSTTaxAmount { get; set; }
        public decimal IGSTTax { get; set; }
        public decimal IGSTTaxAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Insurance { get; set; }
        public decimal TransportInward { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public decimal SumAmount { get; set; }
        public decimal SumTotalPackingCharge { get; set; }
        public decimal SumHamali { get; set; }
        public decimal SumDiscountAmount { get; set; }
        public decimal SumAPMCAmount { get; set; }
        public decimal SumCGSTTaxAmount { get; set; }
        public decimal SumSGSTTaxAmount { get; set; }
        public decimal SumIGSTTaxAmount { get; set; }
        public decimal SumTotalTaxAmount { get; set; }
        public decimal SumTotalAmount { get; set; }
        public decimal SumInsurance { get; set; }
        public decimal SumTransportInward { get; set; }
        public decimal SumGrandTotalAmount { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal GrandNetAmount { get; set; }
        public Boolean IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PurchaseDatestr { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDatestr { get; set; }
        public string DebitAccountType { get; set; }
        public string BrokerName { get; set; }
        public int NoofBags { get; set; }
        public decimal WeightPerBag { get; set; }
        public string PurchaseType { get; set; }
        public string GodownName { get; set; }
        public string EWayNumber { get; set; }
        public long AvakNumber { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public string VakalNumber { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal RatePerKG { get; set; }
        public decimal RatePerBags { get; set; }
        public decimal PackingChargesBag { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }
    }

    public class UpdateVerifyPurcahseOrderStatus
    {
        public long PurchaseID { get; set; }
        public bool Verify { get; set; }
    }

    public class PurchasePayment
    {
        public bool Cash { get; set; }
        public bool Card { get; set; }
        public bool Online { get; set; }
        public bool Cheque { get; set; }
        public long PaymentID { get; set; }
        public long PurchaseID { get; set; }
        public string BillNumber { get; set; }
        public decimal Amount { get; set; }
        public long GodownID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CashPaymentDate { get; set; }
        public long BankID { get; set; }
        public long BankIDCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime? CardPaymentDate { get; set; }
        public long BankIDCheck { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public long BankIDOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }





    public class AddExpenseDetail
    {
        public long ExpenseID { get; set; }
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ShipTo { get; set; }
        public string Tax { get; set; }
        public string BillNumber { get; set; }
        public DateTime? BillDate { get; set; }
        public string BillDateStr { get; set; }
        public long ExpenseTypeID { get; set; }
        public string DeliveryChallanNumber { get; set; }
        public DateTime? DeliveryChallanDate { get; set; }
        public string DeliveryChallanDateStr { get; set; }
        public string EWayNumber { get; set; }
        public long GodownID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal TDSPercentage { get; set; }
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public bool DeActiveSupplier { get; set; }
        //public bool Verify { get; set; }      
        //public long AvakNumber { get; set; }
        public List<AddExpenseQtyDetail> lstExpenseQty { get; set; }

    }

    public class AddExpenseQtyDetail
    {
        public long ExpenseID { get; set; }
        public long ExpenseQtyID { get; set; }
        public long ProductID { get; set; }
        public string HSNNumber { get; set; }
        public long ExpenseDebitAccountTypeID { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal CGSTTax { get; set; }
        public decimal CGSTTaxAmount { get; set; }
        public decimal SGSTTax { get; set; }
        public decimal SGSTTaxAmount { get; set; }
        public decimal IGSTTax { get; set; }
        public decimal IGSTTaxAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }

        public decimal TCSAmount { get; set; }

        public decimal RoundOff { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TDSTax { get; set; }
        public decimal TDSTaxAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class GetProductDetaiForExpense
    {
        public string HSNNumber { get; set; }
    }

    public class GetTaxDetailForExpense
    {
        public decimal Tax { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal TaxAmtIGST { get; set; }
        public decimal TaxAmtCGST { get; set; }
        public decimal TaxAmtSGST { get; set; }
    }

    public class ExpenseListResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long SupplierID { get; set; }
        public long ExpenseID { get; set; }
        public string SupplierName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDateStr { get; set; }
        public string AreaName { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string EWayNumber { get; set; }
        public string GodownName { get; set; }
    }

    public class DayWiseExpenseList
    {
        public long ExpenseID { get; set; }
        public long GodownID { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string BillNumber { get; set; }
        public long SupplierID { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public long ExpenseTypeID { get; set; }
        public string ExpenseType { get; set; }
        public string GodownName { get; set; }
        public string EWayNumber { get; set; }
        // public long AvakNumber { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public Boolean IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ExpenseDatestr { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDatestr { get; set; }
        public string DebitAccountType { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal CGSTTax { get; set; }
        public decimal CGSTTaxAmount { get; set; }
        public decimal SGSTTax { get; set; }
        public decimal SGSTTaxAmount { get; set; }
        public decimal IGSTTax { get; set; }
        public decimal IGSTTaxAmount { get; set; }
        public decimal TCSAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal TotalAmount { get; set; }
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public decimal TDSTax { get; set; }
        public decimal TDSTaxAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal SumTotalTaxAmount { get; set; }
        public decimal SumCGSTTaxAmount { get; set; }
        public decimal SumSGSTTaxAmount { get; set; }
        public decimal SumIGSTTaxAmount { get; set; }
        public decimal SumTCSAmount { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal SumAmount { get; set; }
        public decimal SumTotalAmount { get; set; }
        public decimal SumTDSTaxAmount { get; set; }
        public decimal SumGrandTotalAmount { get; set; }
        public decimal GrandNetAmount { get; set; }



        //public decimal Amount { get; set; }
        //public decimal TotalPackingCharge { get; set; }
        //public decimal Hamali { get; set; }
        //public decimal DiscountPer { get; set; }
        //public decimal DiscountAmount { get; set; }
        //public decimal APMCPer { get; set; }
        //public decimal APMCAmount { get; set; }    
        //public decimal Insurance { get; set; }
        //public decimal TransportInward { get; set; }
        //public decimal SumTotalPackingCharge { get; set; }
        //public decimal SumHamali { get; set; }
        //public decimal SumDiscountAmount { get; set; }
        //public decimal SumAPMCAmount { get; set; }
        //public decimal SumInsurance { get; set; }
        //public decimal SumTransportInward { get; set; }
        // public int NoofBags { get; set; }
        // public decimal WeightPerBag { get; set; }
        // public string PurchaseType { get; set; }
        // public string VakalNumber { get; set; }
        // public decimal GrossWeight { get; set; }
        // public decimal TareWeight { get; set; }
        // public decimal NetWeight { get; set; }
        // public decimal RatePerKG { get; set; }
        // public decimal RatePerBags { get; set; }
        // public decimal PackingChargesBag { get; set; }
    }

    public class DayWiseExpenseListForExp
    {
        public string AavakDate { get; set; }
        //public long AvakNumber { get; set; }    
        public string ExpenseType { get; set; }
        public string GodownName { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public string DebitAccountType { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal CGST { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGST { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TaxCollectedAtSourceReceiver { get; set; }
        public decimal RoundOff { get; set; }
        public decimal TotalAmount { get; set; }
        public string TDSCategory { get; set; }
        public decimal TDSTax { get; set; }
        public decimal TDSTaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string EWayNumber { get; set; }
    }

    public class ExpensePaidPaymentList
    {
        public DateTime PaymentDate { get; set; }
        public long PaymentMode { get; set; }
        public long BankID { get; set; }
        public long SupplierID { get; set; }
        public long AreaID { get; set; }
        public long PaymentID { get; set; }
        public long ExpenseID { get; set; }
        public string Bank_Name { get; set; }
        public string SupplierBankName { get; set; }
        public string Payment_Type { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Payment_Date { get; set; }
        public decimal Amount { get; set; }
        public string SupplierName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string BillNumber { get; set; }
        public DateTime? BillDate { get; set; }
        public string BillDatestr { get; set; }
        public decimal BillAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string AreaName { get; set; }

        public bool ByCheque { get; set; }
        public bool ByCard { get; set; }
        public bool ByOnline { get; set; }


        public string Client_Code { get; set; }
        public string Product_Code { get; set; }
        public string Payment_Ref_No { get; set; }
        public string Instrument_Date { get; set; }
        public string Dr_Ac_No { get; set; }
        public string Bank_Code_Indicator { get; set; }
        public string Beneficiary_Code { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Location { get; set; }
        public string Print_Location { get; set; }
        public string Instrument_Number { get; set; }
        public string Ben_Add1 { get; set; }
        public string Ben_Add2 { get; set; }
        public string Ben_Add3 { get; set; }
        public string Ben_Add4 { get; set; }
        public string Debit_Narration { get; set; }
        public string Credit_Narration { get; set; }
        public string Payment_Details_1 { get; set; }
        public string Payment_Details_2 { get; set; }
        public string Payment_Details_3 { get; set; }
        public string Payment_Details_4 { get; set; }
        public string Deductions { get; set; }
        public string Enrichment_6 { get; set; }
        public string Enrichment_7 { get; set; }
        public string Enrichment_8 { get; set; }
        public string Enrichment_9 { get; set; }
        public string Enrichment_10 { get; set; }
        public string Enrichment_11 { get; set; }
        public string Enrichment_12 { get; set; }
        public string Enrichment_13 { get; set; }
        public string Enrichment_14 { get; set; }
        public string Enrichment_15 { get; set; }
        public string Enrichment_16 { get; set; }
        public string Enrichment_17 { get; set; }
        public string Enrichment_18 { get; set; }
        public string Enrichment_19 { get; set; }
        public string Enrichment_20 { get; set; }

        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Online { get; set; }
        public long GodownID { get; set; }
        public string PaymentDatestr { get; set; }
        public string BankBranchViraki { get; set; }
        public string BankIFSCCodeViraki { get; set; }
        public string ChequeNo { get; set; }
        public string TypeOfCard { get; set; }
        public string UTRNumber { get; set; }

        public decimal sumAmount { get; set; }
        public decimal sumBillAmount { get; set; }
        public decimal sumNetAmount { get; set; }
        public string NameAsBankAccount { get; set; }
    }

    public class ExpensePayment
    {
        public bool Cash { get; set; }
        public bool Card { get; set; }
        public bool Online { get; set; }
        public bool Cheque { get; set; }
        public long PaymentID { get; set; }
        public long ExpenseID { get; set; }
        public string BillNumber { get; set; }
        public decimal Amount { get; set; }
        public long GodownID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CashPaymentDate { get; set; }
        public long BankID { get; set; }
        public long BankIDCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime? CardPaymentDate { get; set; }
        public long BankIDCheck { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public long BankIDOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class ExpensePaidPaymentListExport
    {
        public string Bank_Name { get; set; }
        public string Client_Code { get; set; }
        public string Product_Code { get; set; }
        public string Payment_Type { get; set; }
        public string Payment_Ref_No { get; set; }
        public string Payment_Date { get; set; }
        public string Instrument_Date { get; set; }
        public string Dr_Ac_No { get; set; }
        public decimal Amount { get; set; }
        public string Bank_Code_Indicator { get; set; }
        public string Beneficiary_Code { get; set; }
        public string EmployeeName { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Beneficiary_Branch_IFSC_Code { get; set; }
        public string Beneficiary_Acc_No { get; set; }
        public string Location { get; set; }
        public string Print_Location { get; set; }
        public string Instrument_Number { get; set; }
        public string Ben_Add1 { get; set; }
        public string Ben_Add2 { get; set; }
        public string Ben_Add3 { get; set; }
        public string Ben_Add4 { get; set; }
        public string Beneficiary_Email { get; set; }
        public string Beneficiary_Mobile { get; set; }
        public string Debit_Narration { get; set; }
        public string Credit_Narration { get; set; }
        public string Payment_Details_1 { get; set; }
        public string Payment_Details_2 { get; set; }
        public string Payment_Details_3 { get; set; }
        public string Payment_Details_4 { get; set; }
        public string Bill_No { get; set; }
        public string Bill_Date { get; set; }
        public decimal Bill_Amt { get; set; }
        public string Deductions { get; set; }
        public decimal Net { get; set; }
        public string Enrichment_6 { get; set; }
        public string Enrichment_7 { get; set; }
        public string Enrichment_8 { get; set; }
        public string Enrichment_9 { get; set; }
        public string Enrichment_10 { get; set; }
        public string Enrichment_11 { get; set; }
        public string Enrichment_12 { get; set; }
        public string Enrichment_13 { get; set; }
        public string Enrichment_14 { get; set; }
        public string Enrichment_15 { get; set; }
        public string Enrichment_16 { get; set; }
        public string Enrichment_17 { get; set; }
        public string Enrichment_18 { get; set; }
        public string Enrichment_19 { get; set; }
        public string Enrichment_20 { get; set; }
    }

    public class GetLastPurchaseProductDetail
    {
        public decimal RatePerKG { get; set; }
    }

    // 29-05-2020
    public class BrokerAndItemWiseReportList
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long SupplierID { get; set; }
        public long PurchaseID { get; set; }
        public long ProductID { get; set; }
        public long BrokerID { get; set; }
        public string GodownName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDatestr { get; set; }
        public string SupplierName { get; set; }
        public string AreaName { get; set; }
        public string ProductName { get; set; }
        public long NoofBags { get; set; }
        public decimal RatePerKG { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string BrokerName { get; set; }
        public DateTime CreatedOn { get; set; }

        //public string EWayNumber { get; set; }
        //public long AvakNumber { get; set; }      
    }

    public class PurchaseReportListForExp
    {
        public string GodownName { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string SupplierName { get; set; }
        public string AreaName { get; set; }
        public string ProductName { get; set; }
        public long NoofBags { get; set; }
        public decimal RatePerKG { get; set; }
        public decimal TotalAmount { get; set; }
        public string BrokerName { get; set; }
    }

    public class GetLastExpenseProductDetail
    {
        public decimal Rate { get; set; }
    }



    // 24 Aug 2020 Piyush Limbani
    public class ExpenseItemWiseReportList
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long SupplierID { get; set; }
        public long ExpenseID { get; set; }
        public long ProductID { get; set; }
        public string GodownName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDatestr { get; set; }
        public string SupplierName { get; set; }
        public string AreaName { get; set; }
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string BrokerName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ItemWiseReportListForExp
    {
        public string GodownName { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string SupplierName { get; set; }
        public string AreaName { get; set; }
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
