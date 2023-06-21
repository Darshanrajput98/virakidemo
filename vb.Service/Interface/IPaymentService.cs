

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IPaymentService
    {
        List<PaymentListResponse> GetAllPaymentList(PaymentListResponse model);

        bool SavePayment(List<PaymentListResponse> data, long SessionUserID);

        List<PurchasePaymentListResponse> GetAllPurcahsePaymentList(PurchasePaymentListResponse model);

        bool SavePurchasePayment(List<PurchasePaymentListResponse> data, long SessionUserID);

        BankListResponse GetBankDetailByBankID(int BankID);

        // Expense Payment 27-12-2019
        List<ExpensePaymentListResponse> GetAllExpensePaymentList(ExpensePaymentListResponse model);

        bool SaveExpensePayment(List<ExpensePaymentListResponse> data, long SessionUserID);

        // 10 Sep 2020 Piyush Limbani
        //List<VoucherPaymentListResponse> GetAllWholesaleExpensesVoucherPaymentList(VoucherPaymentListResponse model);

        List<VoucherPaymentListResponse> GetAllWholesaleExpensesVoucherPaymentList(DateTime From, DateTime To, long AreaID, long UserID, long CustomerID, long DaysofWeek);
        

        bool SaveExpenseVoucherPayment(List<VoucherPaymentListResponse> data, long UserID);

        //Add By Dhruvik
        List<CheckReturnEntryListResponse> GetAllCheckReturnList(CheckReturnEntryListResponse model);

        bool SaveReturnCheck(List<PaymentForCheckReturn> data, long SessionUserID);

        bool IsCheckBounceOnPayment(long PaymentID, long OrderID, string InvoiceNumber, decimal OutAmount, decimal ChequeReturnAmount);
        //Add By Dhruvik

    }
}
