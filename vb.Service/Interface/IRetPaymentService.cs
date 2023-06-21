

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IRetPaymentService
    {
        List<RetPaymentListResponse> GetAllPaymentList(RetPaymentListResponse model);

        bool SavePayment(List<RetPaymentListResponse> data, long SessionUserID);


        // 11 Sep 2020 Piyush Limbani
        List<RetVoucherPaymentListResponse> GetAllRetailExpensesVoucherPaymentList(DateTime From, DateTime To, long AreaID, long UserID, long CustomerID, long DaysofWeek);

        bool SaveExpenseVoucherPayment(List<RetVoucherPaymentListResponse> data, long UserID);

        //Add by Dhruvik
        List<RetCheckReturnEntryListResponse> GetAllRetCheckReturnList(RetCheckReturnEntryListResponse model);

        bool SaveRetReturnCheque(List<RetPaymentForCheckReturn> data, long SessionUserID);

        bool IsCheckBounceOnRetPayment(long PaymentID, long OrderID, string InvoiceNumber, decimal OutAmount, decimal ChequeReturnAmount);
        //Add by Dhruvik
    }
}
