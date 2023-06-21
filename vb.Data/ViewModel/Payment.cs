

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PaymentListResponse
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public int DaysofWeek { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal ByCash { get; set; }
        public bool ByCheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool ByOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public string Remark { get; set; }
        public string IsSelect { get; set; }
        public int Quantity { get; set; }
        public string ShipTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal AdjustAmount { get; set; }
        public int VehicleNo { get; set; }
        public DateTime AssignedDate { get; set; }
        public long CustomerNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string CustBankName { get; set; }
        public string CustBranch { get; set; }
        public string CustIFCCode { get; set; }

        public long GodownID { get; set; }
    }

    public class PaymentListForExp
    {
        public long CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Remark { get; set; }
    }

    public class BillHistoryListResponse
    {
        public string Customer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string AreaName { get; set; }
        public decimal Cash { get; set; }
        public decimal Cheque { get; set; }
        public decimal Card { get; set; }
        public decimal Online { get; set; }
        public decimal AdjustAmount { get; set; }
        public DateTime CreditMemoDate { get; set; }
        public string CreditMemoDatestr { get; set; }
        public string CreditMemoNumber { get; set; }
        public decimal CreditMemoAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryPerson1 { get; set; }
        public string DeliveryPerson2 { get; set; }
        public string DeliveryPerson3 { get; set; }
        public string DeliveryPerson4 { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime OnlinePaymentDate { get; set; }
        public long VehicleNo { get; set; }
        public string TempoNo { get; set; }
        public bool BySign { get; set; }
        public long PaymentID { get; set; }
        public bool IsCreditNote { get; set; }
        public long OrderID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long CustomerID { get; set; }
        public bool IsDelivered { get; set; }
        public string DeliveryStatus { get; set; }
        public string PaymentDatestr { get; set; }
        public string DeliveryDatestr { get; set; }
    }

    public class Payment
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentID { get; set; }
        public bool Cash { get; set; }
        public decimal Amount { get; set; }
        public long GodownID { get; set; }
        public bool Cheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool Card { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool Online { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public bool Sign { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal ByCash { get; set; }
    }

    public class PurchasePaymentListResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long AreaID { get; set; }
        public long SupplierID { get; set; }
        public long PurchaseID { get; set; }
        public string BillNumber { get; set; }
        public string SupplierName { get; set; }
        public DateTime BillDate { get; set; }
        public string AreaName { get; set; }
        public string PurchaseDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool ByCheque { get; set; }
        public decimal OutstandingAmount { get; set; }

        public long BankID { get; set; }

        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string IFSCCode { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }

        public long BankIDForCard { get; set; }

        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime? CardPaymentDate { get; set; }

        public bool ByOnline { get; set; }

        public long BankIDForOnline { get; set; }

        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public string Remark { get; set; }
        public decimal ByCash { get; set; }

        public long GodownID { get; set; }
        public DateTime? PaymentDateForCash { get; set; }

        public decimal AdjustAmount { get; set; }
    }

    // Expense Payment 27-12-2019
    public class ExpensePaymentListResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long AreaID { get; set; }
        public long SupplierID { get; set; }
        public long ExpenseID { get; set; }
        public string BillNumber { get; set; }
        public string SupplierName { get; set; }
        public DateTime BillDate { get; set; }
        public string AreaName { get; set; }
        public string ExpenseDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool ByCheque { get; set; }
        public decimal OutstandingAmount { get; set; }
        public long BankID { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string IFSCCode { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }
        public long BankIDForCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public DateTime? CardPaymentDate { get; set; }
        public bool ByOnline { get; set; }
        public long BankIDForOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public string Remark { get; set; }
        public decimal ByCash { get; set; }
        public long GodownID { get; set; }
        public DateTime? PaymentDateForCash { get; set; }
        public decimal AdjustAmount { get; set; }
    }

    public class VoucherPaymentListResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long AreaID { get; set; }
        public long UserID { get; set; }
        public long CustomerID { get; set; }
        public int DaysofWeek { get; set; }
        public long ExpensesVoucherID { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string UserName { get; set; }
        public string BillNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DateofVoucher { get; set; }
        public string DateofVoucherstr { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public long CustomerNumber { get; set; }
        public string CustBankName { get; set; }
        public string CustBranch { get; set; }
        public string CustIFCCode { get; set; }
        public decimal AdjustAmount { get; set; }
        public decimal ByCash { get; set; }
        public bool ByCheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool ByOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public string Remark { get; set; }
        public long GodownID { get; set; }

        //public string IsSelect { get; set; }
        //public int Quantity { get; set; }
        //public string ShipTo { get; set; }
        //public DateTime DeliveryDate { get; set; }
        //public string Tax { get; set; }
        //public string OrderRef { get; set; }          
        //public int VehicleNo { get; set; }
        //public DateTime AssignedDate { get; set; }        
        //public string InvoiceDate { get; set; }     
    }

    public class VoucherPaymentListForExp
    {
        public long CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string DateofVoucher { get; set; }
        public string BillNumber { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Remark { get; set; }
    }


    public class UpdateVoucherPayment
    {
        public long ExpensesVoucherID { get; set; }
        public string BillNumber { get; set; }
        public string PaymentID { get; set; }
        public bool Cash { get; set; }
        public decimal Amount { get; set; }
        public long GodownID { get; set; }
        public bool Cheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool Card { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool Online { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }       
        public decimal AdjustAmount { get; set; }
    }


    //Add By Dhruvik
    public class CheckReturnEntryListResponse
    {
        public long PaymentID { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public int DaysofWeek { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal ByCash { get; set; }
        public bool ByCheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool ByOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public string Remark { get; set; }
        public string IsSelect { get; set; }
        public int Quantity { get; set; }
        public string ShipTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal AdjustAmount { get; set; }
        public int VehicleNo { get; set; }
        public DateTime AssignedDate { get; set; }
        public long CustomerNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string CustBankName { get; set; }
        public string CustBranch { get; set; }
        public string CustIFCCode { get; set; }
        public string CustChequeNo { get; set; }

        public long GodownID { get; set; }
        public decimal CheckBounce { get; set; }

    }

    //Add By Dhruvik
    public class PaymentForCheckReturn
    {
        public long CheckBounceID { get; set; }
        public long PaymentID { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public string InvoiceNumber { get; set; }
        public string BankName { get; set; }
        public string SalesPerson { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public decimal ChequeAmount { get; set; }
        public string IFSCCode { get; set; }
        public decimal BounceAmount { get; set; }
        public string Remark { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal ChequeReturnCharges { get; set; }
    }

    //Add By Dhruvik
    public class PaymentChequeListResponse
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public int DaysofWeek { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal ByCash { get; set; }
        public bool ByCheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool ByOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public string Remark { get; set; }
        public string IsSelect { get; set; }
        public int Quantity { get; set; }
        public string ShipTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal AdjustAmount { get; set; }
        public int VehicleNo { get; set; }
        public DateTime AssignedDate { get; set; }
        public long CustomerNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string CustBankName { get; set; }
        public string CustBranch { get; set; }
        public string CustIFCCode { get; set; }

        public long GodownID { get; set; }

        public decimal BounceAmount { get; set; }
        public decimal ChequeReturnCharges { get; set; }
    }
    //Add By Dhruvik

}
