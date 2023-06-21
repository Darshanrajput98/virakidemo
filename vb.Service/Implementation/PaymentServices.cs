
namespace vb.Service
{
    using vb.Data;
    using vb.Data.Model;
    using System.Data.Entity;
    using System.Data;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using vb.Data.ViewModel;
    using System.Linq;
    using vb.Service.Common;
    using System;

    public class PaymentServices : IPaymentService
    {
        public List<PaymentListResponse> GetAllPaymentList(PaymentListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPaymentList";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", model.DaysofWeek);
                if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@From", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@From", model.From);
                }
                if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@To", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@To", model.To);
                }
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PaymentListResponse> objlst = new List<PaymentListResponse>();
                while (dr.Read())
                {
                    string OustandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount").ToString();

                    if (OustandingAmount != "0.00")
                    {
                        PaymentListResponse objPayment = new PaymentListResponse();
                        objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                        objPayment.InvoiceDate = objPayment.CreatedOn.ToString("dd/MM/yyyy");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                        objPayment.PaymentTotal = objBaseSqlManager.GetDecimal(dr, "PaymentTotal");
                        objPayment.OutstandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount");
                        objPayment.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                        objPayment.CustBankName = objBaseSqlManager.GetTextValue(dr, "CustBankName");
                        objPayment.CustBranch = objBaseSqlManager.GetTextValue(dr, "CustBranch");
                        objPayment.CustIFCCode = objBaseSqlManager.GetTextValue(dr, "CustIFCCode");
                        objlst.Add(objPayment);
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        private bool CheckExistDeliveryOfInvoice(string InvoiceNumber, int? VehicleNo, DateTime? AssignedDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistDeliveryOfInvoice";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                bool exist = false;
                while (dr.Read())
                {
                    exist = true;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return exist;
            }
        }

        private bool CheckExistInvoice(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistInvoice";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                bool exist = false;
                while (dr.Read())
                {
                    exist = true;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return exist;
            }
        }

        private bool CheckExistInvoiceForCreditNote(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistInvoiceForCreditNote";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                bool exist = false;
                while (dr.Read())
                {
                    exist = true;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return exist;
            }
        }

        public bool SavePayment(List<PaymentListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        Payment_Mst obj = new Payment_Mst();
                        obj.CustomerID = item.CustomerID;
                        obj.OrderID = item.OrderID;
                        obj.InvoiceNumber = item.InvoiceNumber;
                        obj.VehicleNo = 0;
                        obj.AssignedDate = DateTime.Now;
                        obj.Containers = "";
                        obj.ByCash = item.ByCash;
                        obj.AdjustAmount = item.AdjustAmount;
                        obj.PaymentAmount = item.FinalTotal;
                        obj.ByCheque = item.ByCheque;
                        obj.BankName = item.BankName;
                        obj.BankBranch = item.BankBranch;
                        obj.ChequeNo = item.ChequeNo;
                        obj.ChequeDate = item.ChequeDate;
                        obj.IFCCode = item.IFCCode;
                        obj.ByCard = item.ByCard;
                        obj.BankNameForCard = item.BankNameForCard;
                        obj.TypeOfCard = item.TypeOfCard;
                        obj.ByOnline = item.ByOnline;
                        obj.BankNameForOnline = item.BankNameForOnline;
                        obj.UTRNumber = item.UTRNumber;
                        obj.OnlinePaymentDate = item.OnlinePaymentDate;
                        obj.IsDelivered = true;
                        // obj.IsDelivered = false;
                        obj.Remark = item.Remark;

                        obj.GodownID = item.GodownID;
                        obj.IsInwardTempo = false;

                        obj.CreatedBy = SessionUserID;
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = SessionUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsDelete = false;
                        bool ExistInvoice = CheckExistInvoice(obj.InvoiceNumber);
                        if (ExistInvoice == true)
                        {
                            obj.IsInvoiceExist = true;
                        }
                        bool ExistInvForCreditNote = CheckExistInvoiceForCreditNote(obj.InvoiceNumber);
                        if (ExistInvForCreditNote == true)
                        {
                            obj.IsCreditNote = true;
                        }
                        context.Payment_Mst.Add(obj);
                        context.SaveChanges();
                        if (obj.IsDelivered == false)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateDeliveredOrder";
                                cmdGet.Parameters.AddWithValue("@OrderID", obj.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", obj.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }
                        if (obj.InvoiceNumber != null)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateDeliveredPayment";
                                cmdGet.Parameters.AddWithValue("@OrderID", obj.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", obj.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }
                        bool exist = CheckExistDeliveryOfInvoice(obj.InvoiceNumber, obj.VehicleNo, obj.AssignedDate);
                        if (exist == false)
                        {
                            Delivery_Mst dms = new Delivery_Mst();
                            dms.VehicleNo = obj.VehicleNo.Value;
                            dms.Tray = "";
                            dms.Other = "";
                            dms.InvoiceNumber = obj.InvoiceNumber;
                            dms.InvoiceDate = obj.AssignedDate;
                            dms.CreatedBy = SessionUserID;
                            dms.CreatedOn = obj.AssignedDate.Value;

                            dms.IsVehicleCosting = true;

                            // 04 Aug 2020 Piyush Limbani Add New Field
                            dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                            // 04 Aug 2020 Piyush Limbani Add New Field

                            context.Delivery_Mst.Add(dms);
                            context.SaveChanges();
                        }

                        bool exist2 = CheckDeliveryAllocationVehicle0Exist(Convert.ToDateTime(DateTime.Now));
                        if (exist2 == false)
                        {
                            int suc = InsertDeliveryAllocationVehicle(Convert.ToDateTime(DateTime.Now));
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public bool CheckDeliveryAllocationVehicle0Exist(DateTime date)
        {
            bool flag = false;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "CheckDeliveryAllocationVehicle0Exist";
                    cmdGet.Parameters.AddWithValue("@date", date);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        flag = true;
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public Int32 InsertDeliveryAllocationVehicle(DateTime date)
        {
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "InsertDeliveryAllocationVehicle";
                    cmdGet.Parameters.AddWithValue("@date", date);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        // Purchase Payment
        public List<PurchasePaymentListResponse> GetAllPurcahsePaymentList(PurchasePaymentListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurcahsePaymentList";
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@SupplierID", model.SupplierID);
                if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@From", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@From", model.From);
                }
                if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@To", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@To", model.To);
                }
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchasePaymentListResponse> objlst = new List<PurchasePaymentListResponse>();
                while (dr.Read())
                {
                    string OustandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount").ToString();
                    if (OustandingAmount != "0.0000")
                    {
                        PurchasePaymentListResponse objPayment = new PurchasePaymentListResponse();
                        objPayment.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                        objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                        objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                        objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                        objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                        objPayment.PurchaseDate = objPayment.BillDate.ToString("dd/MM/yyyy");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.PaymentAmount = objBaseSqlManager.GetDecimal(dr, "PaymentAmount");
                        objPayment.OutstandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount");
                        objPayment.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                        objPayment.BankBranch = objBaseSqlManager.GetTextValue(dr, "Branch");
                        objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                        //objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objlst.Add(objPayment);
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool SavePurchasePayment(List<PurchasePaymentListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        PurcahsePayment_Mst obj = new PurcahsePayment_Mst();
                        obj.SupplierID = item.SupplierID;
                        obj.PurchaseID = item.PurchaseID;

                        obj.BillNumber = item.BillNumber;
                        obj.PaymentAmount = item.PaymentAmount;
                        obj.ByCash = item.ByCash;
                        obj.ByCheque = item.ByCheque;
                        obj.ByCard = item.ByCard;
                        obj.ByOnline = item.ByOnline;
                        if (item.ChequeDate != null && item.ByCheque == true)
                        {
                            obj.PaymentDate = item.ChequeDate;
                            obj.BankID = item.BankID;
                            obj.ChequeNo = item.ChequeNo;
                        }
                        else if (item.CardPaymentDate != null && item.ByCard == true)
                        {
                            obj.PaymentDate = item.CardPaymentDate;
                            obj.BankID = item.BankIDForCard;
                            obj.TypeOfCard = item.TypeOfCard;
                        }
                        else if (item.OnlinePaymentDate != null && item.ByOnline == true)
                        {
                            obj.PaymentDate = item.OnlinePaymentDate;
                            obj.BankID = item.BankIDForOnline;
                            obj.UTRNumber = item.UTRNumber;
                        }
                        else
                        {
                            obj.PaymentDate = item.PaymentDateForCash;
                            obj.BankID = 0;
                        }
                        obj.GodownID = item.GodownID;
                        obj.AdjustAmount = item.AdjustAmount;

                        // // cheque                     
                        // obj.BankName = item.BankName;
                        // obj.BankBranch = item.BankBranch;                     
                        // obj.ChequeDate = item.ChequeDate;
                        // obj.IFCCode = item.IFCCode;
                        //// card
                        // obj.BankIDForCard = item.BankIDForCard;
                        // obj.BankNameForCard = item.BankNameForCard;                     
                        // obj.CardPaymentDate = item.CardPaymentDate;
                        // // online                     
                        // obj.BankIDForOnline = item.BankIDForOnline;
                        // obj.BankNameForOnline = item.BankNameForOnline;                      
                        // obj.OnlinePaymentDate = item.OnlinePaymentDate;                    
                        // obj.PaymentDateForCash = item.PaymentDateForCash;

                        obj.Remark = item.Remark;
                        obj.CreatedBy = SessionUserID;
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = SessionUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsDelete = false;
                        context.PurcahsePayment_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public BankListResponse GetBankDetailByBankID(int BankID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBankDetailByBankID";
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                BankListResponse obj = new BankListResponse();
                while (dr.Read())
                {
                    obj.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    obj.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // Expense Payment 27-12-2019
        public List<ExpensePaymentListResponse> GetAllExpensePaymentList(ExpensePaymentListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpensePaymentList";
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@SupplierID", model.SupplierID);
                if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@From", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@From", model.From);
                }
                if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@To", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@To", model.To);
                }
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensePaymentListResponse> objlst = new List<ExpensePaymentListResponse>();
                while (dr.Read())
                {
                    string OustandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount").ToString();
                    if (OustandingAmount != "0.0000")
                    {
                        ExpensePaymentListResponse objPayment = new ExpensePaymentListResponse();
                        objPayment.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                        objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                        objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                        objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                        objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                        objPayment.ExpenseDate = objPayment.BillDate.ToString("dd/MM/yyyy");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.PaymentAmount = objBaseSqlManager.GetDecimal(dr, "PaymentAmount");
                        objPayment.OutstandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount");
                        objPayment.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                        objPayment.BankBranch = objBaseSqlManager.GetTextValue(dr, "Branch");
                        objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                        //objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objlst.Add(objPayment);
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool SaveExpensePayment(List<ExpensePaymentListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        ExpensePayment_Mst obj = new ExpensePayment_Mst();
                        obj.SupplierID = item.SupplierID;
                        obj.ExpenseID = item.ExpenseID;
                        obj.BillNumber = item.BillNumber;
                        obj.PaymentAmount = item.PaymentAmount;
                        obj.ByCash = item.ByCash;
                        obj.ByCheque = item.ByCheque;
                        obj.ByCard = item.ByCard;
                        obj.ByOnline = item.ByOnline;
                        if (item.ChequeDate != null && item.ByCheque == true)
                        {
                            obj.PaymentDate = item.ChequeDate;
                            obj.BankID = item.BankID;
                            obj.ChequeNo = item.ChequeNo;
                        }
                        else if (item.CardPaymentDate != null && item.ByCard == true)
                        {
                            obj.PaymentDate = item.CardPaymentDate;
                            obj.BankID = item.BankIDForCard;
                            obj.TypeOfCard = item.TypeOfCard;
                        }
                        else if (item.OnlinePaymentDate != null && item.ByOnline == true)
                        {
                            obj.PaymentDate = item.OnlinePaymentDate;
                            obj.BankID = item.BankIDForOnline;
                            obj.UTRNumber = item.UTRNumber;
                        }
                        else
                        {
                            obj.PaymentDate = item.PaymentDateForCash;
                            obj.BankID = 0;
                        }
                        obj.GodownID = item.GodownID;
                        obj.AdjustAmount = item.AdjustAmount;
                        obj.Remark = item.Remark;
                        obj.CreatedBy = SessionUserID;
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = SessionUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsDelete = false;
                        context.ExpensePayment_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        // 11 Sep 2020 Piyush Limbani
        public List<VoucherPaymentListResponse> GetAllWholesaleExpensesVoucherPaymentList(DateTime From, DateTime To, long AreaID, long UserID, long CustomerID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllWholesaleExpensesVoucherPaymentList";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                if (From.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@From", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@From", From);
                }
                if (To.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@To", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@To", To);
                }
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VoucherPaymentListResponse> objlst = new List<VoucherPaymentListResponse>();
                while (dr.Read())
                {
                    string OustandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount").ToString();
                    if (OustandingAmount != "0.00")
                    {
                        VoucherPaymentListResponse objPayment = new VoucherPaymentListResponse();
                        objPayment.ExpensesVoucherID = objBaseSqlManager.GetInt64(dr, "ExpensesVoucherID");
                        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                        objPayment.DateofVoucher = objBaseSqlManager.GetDateTime(dr, "DateofVoucher");
                        //objPayment.DateofVoucherstr = objPayment.DateofVoucher.ToString("dd/MM/yyyy");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objPayment.PaymentAmount = objBaseSqlManager.GetDecimal(dr, "PaymentAmount");
                        objPayment.OutstandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount");
                        // objPayment.PaymentTotal = objBaseSqlManager.GetDecimal(dr, "PaymentTotal");
                        // objPayment.OutstandingAmount = objBaseSqlManager.GetDecimal(dr, "PaymentAmount");
                        objPayment.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                        objPayment.CustBankName = objBaseSqlManager.GetTextValue(dr, "CustBankName");
                        objPayment.CustBranch = objBaseSqlManager.GetTextValue(dr, "CustBranch");
                        objPayment.CustIFCCode = objBaseSqlManager.GetTextValue(dr, "CustIFCCode");
                        objlst.Add(objPayment);
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool SaveExpenseVoucherPayment(List<VoucherPaymentListResponse> data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        WholesaleExpenseVoucherPayment_Mst obj = new WholesaleExpenseVoucherPayment_Mst();
                        obj.ExpensesVoucherID = item.ExpensesVoucherID;
                        obj.CustomerID = item.CustomerID;
                        obj.BillNumber = item.BillNumber;
                        obj.PaymentAmount = item.PaymentAmount;
                        obj.ByCash = item.ByCash;
                        obj.ByCheque = item.ByCheque;
                        obj.BankName = item.BankName;
                        obj.BankBranch = item.BankBranch;
                        obj.ChequeNo = item.ChequeNo;
                        obj.ChequeDate = item.ChequeDate;
                        obj.IFCCode = item.IFCCode;
                        obj.ByCard = item.ByCard;
                        obj.BankNameForCard = item.BankNameForCard;
                        obj.TypeOfCard = item.TypeOfCard;
                        obj.ByOnline = item.ByOnline;
                        obj.BankNameForOnline = item.BankNameForOnline;
                        obj.UTRNumber = item.UTRNumber;
                        obj.OnlinePaymentDate = item.OnlinePaymentDate;
                        obj.AdjustAmount = item.AdjustAmount;
                        obj.Remark = item.Remark;
                        obj.CreatedBy = UserID;
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = UserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.GodownID = item.GodownID;
                        obj.IsDelete = false;
                        context.WholesaleExpenseVoucherPayment_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        //Add By Dhruvik
        public List<CheckReturnEntryListResponse> GetAllCheckReturnList(CheckReturnEntryListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCheckReturnList";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", model.DaysofWeek);
                cmdGet.Parameters.AddWithValue("@ChequeNo", model.ChequeNo);
                if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@From", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@From", model.From);
                }
                if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@To", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@To", model.To);
                }
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CheckReturnEntryListResponse> objlst = new List<CheckReturnEntryListResponse>();
                while (dr.Read())
                {
                    string IsCheckBounce = objBaseSqlManager.GetBoolean(dr, "IsCheckBounce").ToString();
                    if (IsCheckBounce == "False")
                    {
                        CheckReturnEntryListResponse objPayment = new CheckReturnEntryListResponse();
                        objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                        objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                        objPayment.InvoiceDate = objPayment.CreatedOn.ToString("dd/MM/yyyy");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                        objPayment.CheckBounce = objBaseSqlManager.GetDecimal(dr, "CheckBounce");
                        objPayment.PaymentTotal = objBaseSqlManager.GetDecimal(dr, "PaymentTotal");
                        objPayment.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                        objPayment.CustBankName = objBaseSqlManager.GetTextValue(dr, "CustBankName");
                        objPayment.CustBranch = objBaseSqlManager.GetTextValue(dr, "CustBranch");
                        objPayment.CustIFCCode = objBaseSqlManager.GetTextValue(dr, "CustIFCCode");
                        objPayment.CustChequeNo = objBaseSqlManager.GetTextValue(dr, "CustChequeNo");
                        objPayment.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                        objlst.Add(objPayment);
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool SaveReturnCheck(List<PaymentForCheckReturn> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        CheckReturn_Mst obj = new CheckReturn_Mst();
                        obj.CheckBounceID = item.CheckBounceID;
                        obj.PaymentID = item.PaymentID;
                        obj.OrderID = item.OrderID;
                        obj.CustomerID = item.CustomerID;
                        obj.InvoiceNumber = item.InvoiceNumber;
                        obj.BankName = item.BankName;
                        obj.SalesPerson = item.SalesPerson;
                        obj.ChequeNo = item.ChequeNo;
                        obj.ChequeDate = item.ChequeDate;
                        obj.ChequeAmount = item.ChequeAmount;
                        obj.IFSCCode = item.IFSCCode;
                        obj.BounceAmount = item.BounceAmount;
                        obj.ChequeReturnCharges = item.ChequeReturnCharges;
                        obj.Remark = item.Remark;
                        obj.CreatedBy = SessionUserID;
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = SessionUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsDelete = false;
                        context.CheckReturn_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public bool IsCheckBounceOnPayment(long PaymentID, long OrderID, string InvoiceNumber, decimal OutAmount, decimal ChequeReturnAmount)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateIsCheckBounceOnPayment";
                cmdGet.Parameters.AddWithValue("@PaymentID", PaymentID);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OutAmount", OutAmount);
                cmdGet.Parameters.AddWithValue("@ChequeReturnAmount", ChequeReturnAmount);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }
        //Add By Dhruvik

    }
}
