
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

    public class RetPaymentServices : IRetPaymentService
    {
        public List<RetPaymentListResponse> GetAllPaymentList(RetPaymentListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetPaymentList";
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
                List<RetPaymentListResponse> objlst = new List<RetPaymentListResponse>();
                while (dr.Read())
                {
                    string OustandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount").ToString();

                    if (OustandingAmount != "0.00")
                    {
                        RetPaymentListResponse objPayment = new RetPaymentListResponse();
                        objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                        objPayment.FinancialInvoiceNumber = objPayment.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objPayment.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objPayment.CreatedOn).ToString("yy");
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

        public bool SavePayment(List<RetPaymentListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        RetPaymentMst obj = new RetPaymentMst();
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
                        bool ExistInvoice = CheckExistInvoice(obj.InvoiceNumber, item.OrderID);
                        if (ExistInvoice == true)
                        {
                            obj.IsInvoiceExist = true;
                        }
                        bool ExistInvForCreditNote = CheckExistInvoiceForCreditNote(obj.InvoiceNumber, item.OrderID);
                        if (ExistInvForCreditNote == true)
                        {
                            obj.IsCreditNote = true;
                        }
                        context.RetPaymentMsts.Add(obj);
                        context.SaveChanges();
                        if (obj.IsDelivered == false)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateRetDeliveredOrder";
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
                                cmdGet.CommandText = "UpdateRetDeliveredPayment";
                                cmdGet.Parameters.AddWithValue("@OrderID", obj.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", obj.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }
                        bool exist = CheckExistDeliveryOfInvoice(obj.InvoiceNumber, obj.VehicleNo, obj.AssignedDate);
                        if (exist == false)
                        {
                            RetDeliveryMst dms = new RetDeliveryMst();
                            dms.VehicleNo = obj.VehicleNo.Value;
                            dms.Tray = "";
                            dms.Other = "";
                            dms.InvoiceNumber = obj.InvoiceNumber;
                            dms.OrderID = obj.OrderID;
                            dms.InvoiceDate = obj.AssignedDate;
                            dms.CreatedBy = SessionUserID;
                            dms.CreatedOn = obj.AssignedDate.Value;
                            dms.IsVehicleCosting = true;
                            // 05 Aug 2020 Piyush Limbani Add New Field
                            dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                            // 05 Aug 2020 Piyush Limbani Add New Field
                            context.RetDeliveryMsts.Add(dms);
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

        private bool CheckExistDeliveryOfInvoice(string InvoiceNumber, int? VehicleNo, DateTime? AssignedDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistRetDeliveryOfInvoice";
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

        public bool CheckDeliveryAllocationVehicle0Exist(DateTime date)
        {
            bool flag = false;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "CheckRetDeliveryAllocationVehicle0Exist";
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
                    cmdGet.CommandText = "InsertRetDeliveryAllocationVehicle";
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

        private bool CheckExistInvoice(string InvoiceNumber, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetExistInvoice";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
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

        private bool CheckExistInvoiceForCreditNote(string InvoiceNumber, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetExistInvoiceForCreditNote";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
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


        // 11 Sep 2020 Piyush Limbani
        public List<RetVoucherPaymentListResponse> GetAllRetailExpensesVoucherPaymentList(DateTime From, DateTime To, long AreaID, long UserID, long CustomerID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetailExpensesVoucherPaymentList";
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
                List<RetVoucherPaymentListResponse> objlst = new List<RetVoucherPaymentListResponse>();
                while (dr.Read())
                {
                    string OustandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount").ToString();
                    if (OustandingAmount != "0.00")
                    {
                        RetVoucherPaymentListResponse objPayment = new RetVoucherPaymentListResponse();
                        objPayment.ExpensesVoucherID = objBaseSqlManager.GetInt64(dr, "ExpensesVoucherID");
                        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                        objPayment.DateofVoucher = objBaseSqlManager.GetDateTime(dr, "DateofVoucher");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objPayment.PaymentAmount = objBaseSqlManager.GetDecimal(dr, "PaymentAmount");
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

        public bool SaveExpenseVoucherPayment(List<RetVoucherPaymentListResponse> data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        RetailExpenseVoucherPayment_Mst obj = new RetailExpenseVoucherPayment_Mst();
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
                        context.RetailExpenseVoucherPayment_Mst.Add(obj);
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
        public List<RetCheckReturnEntryListResponse> GetAllRetCheckReturnList(RetCheckReturnEntryListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCheckReturnList";
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
                List<RetCheckReturnEntryListResponse> objlst = new List<RetCheckReturnEntryListResponse>();
                while (dr.Read())
                {

                    string IsCheckBounce = objBaseSqlManager.GetBoolean(dr, "IsCheckBounce").ToString();
                    if (IsCheckBounce == "False")
                    {
                        RetCheckReturnEntryListResponse objPayment = new RetCheckReturnEntryListResponse();
                        objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                        objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                        objPayment.FinancialInvoiceNumber = objPayment.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objPayment.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objPayment.CreatedOn).ToString("yy");
                        objPayment.InvoiceDate = objPayment.CreatedOn.ToString("dd/MM/yyyy");
                        objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                        objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                        objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                        objPayment.PaymentTotal = objBaseSqlManager.GetDecimal(dr, "PaymentTotal");
                        objPayment.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                        objPayment.BankName = objBaseSqlManager.GetTextValue(dr, "CustBankName");
                        objPayment.CustBranch = objBaseSqlManager.GetTextValue(dr, "CustBranch");
                        objPayment.IFCCode = objBaseSqlManager.GetTextValue(dr, "CustIFCCode");
                        objPayment.ChequeNo = objBaseSqlManager.GetTextValue(dr, "CustChequeNo");
                        objPayment.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                        objlst.Add(objPayment);
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool SaveRetReturnCheque(List<RetPaymentForCheckReturn> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    foreach (var item in data)
                    {
                        RetCheckReturn_Mst obj = new RetCheckReturn_Mst();
                        obj.RetCheckBounceID = item.RetCheckBounceID;
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
                        context.RetCheckReturn_Mst.Add(obj);
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

        public bool IsCheckBounceOnRetPayment(long PaymentID, long OrderID, string InvoiceNumber, decimal OutAmount, decimal ChequeReturnAmount)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateIsCheckBounceOnRetPayment";
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
