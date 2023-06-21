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

namespace vb.Service
{
    public class RetDeliveryServices : IRetDeliveryService
    {
        public List<RetPendingDeliveryListResponse> GetAllPendingDeliveryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetPendingDeliveryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetPendingDeliveryListResponse> objlst = new List<RetPendingDeliveryListResponse>();
                while (dr.Read())
                {
                    RetPendingDeliveryListResponse objDelivery = new RetPendingDeliveryListResponse();
                    objDelivery.DeliveryID = objBaseSqlManager.GetInt64(dr, "DeliveryID");
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objDelivery.FinancialInvoiceNumber = objDelivery.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objDelivery.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objDelivery.InvoiceDate).ToString("yy");
                    objDelivery.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objDelivery.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objDelivery.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDeliveryStatusListResponse> GetPendingInvoiceListForPrint(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetPendingInvoiceListForPrint";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDeliveryStatusListResponse> objlst = new List<RetDeliveryStatusListResponse>();
                while (dr.Read())
                {
                    RetDeliveryStatusListResponse objDelivery = new RetDeliveryStatusListResponse();
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    objDelivery.PaymentAmount = objBaseSqlManager.GetDecimal(dr, "PaymentAmount");
                    objDelivery.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");
                    objDelivery.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objDelivery.AreaID = objBaseSqlManager.GetTextValue(dr, "AreaID");
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDeliveryStatusListResponse> GetDeliveryStatusList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetDeliveryStatusList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDeliveryStatusListResponse> objlst = new List<RetDeliveryStatusListResponse>();
                long oldorderid = 0;
                while (dr.Read())
                {
                    RetDeliveryStatusListResponse objDelivery = new RetDeliveryStatusListResponse();
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objDelivery.FinancialInvoiceNumber = objDelivery.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objDelivery.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objDelivery.InvoiceDate).ToString("yy");
                    objDelivery.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    try
                    {
                        objDelivery.PaymentAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "PaymentAmount")));
                        objDelivery.PaymentAmount = Convert.ToDecimal(objDelivery.PaymentAmount.ToString() + ".00");
                    }
                    catch { objDelivery.PaymentAmount = Convert.ToDecimal("0.00"); }
                    objDelivery.ByCash = objBaseSqlManager.GetDecimal(dr, "ByCash");
                    objDelivery.ByCheque = objBaseSqlManager.GetBoolean(dr, "ByCheque");
                    objDelivery.BySign = objBaseSqlManager.GetBoolean(dr, "BySign");
                    objDelivery.ByCard = objBaseSqlManager.GetBoolean(dr, "ByCard");
                    objDelivery.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");
                    objDelivery.Remark = objBaseSqlManager.GetTextValue(dr, "Remark");
                    objDelivery.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    // objDelivery.Container = objBaseSqlManager.GetTextValue(dr, "Container");
                    if (objBaseSqlManager.GetTextValue(dr, "Container") == "")
                    {
                        if (oldorderid == 0 || objDelivery.OrderID != oldorderid)
                        {
                            string TotalContainer = GetTotalContainerbyOrderID(objDelivery.OrderID);
                            objDelivery.Container = TotalContainer.TrimEnd(',');
                            oldorderid = objDelivery.OrderID;
                        }
                    }
                    else
                    {
                        objDelivery.Container = objBaseSqlManager.GetTextValue(dr, "Container");
                    }
                    objDelivery.AreaID = objBaseSqlManager.GetTextValue(dr, "AreaID");
                    objDelivery.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objDelivery.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objDelivery.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    objDelivery.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    objDelivery.AdjustAmount = objBaseSqlManager.GetDecimal(dr, "AdjustAmount");
                    objDelivery.DeliveryID = objBaseSqlManager.GetInt64(dr, "DeliveryID");
                    objDelivery.DisableRow = objBaseSqlManager.GetInt32(dr, "DisableRow");
                    objDelivery.CustBankName = objBaseSqlManager.GetTextValue(dr, "CustBankName");
                    objDelivery.CustBranch = objBaseSqlManager.GetTextValue(dr, "CustBranch");
                    objDelivery.CustIFCCode = objBaseSqlManager.GetTextValue(dr, "CustIFCCode");
                    objDelivery.AssignedDate = objBaseSqlManager.GetDateTime(dr, "AssignedDate");
                    objDelivery.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objDelivery.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDeliveryStatusListResponse> GetTempoSheetList(long VehicleNo, DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetTempoSheetList";
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDeliveryStatusListResponse> objlst = new List<RetDeliveryStatusListResponse>();
                long oldorderid = 0;
                while (dr.Read())
                {
                    RetDeliveryStatusListResponse objDelivery = new RetDeliveryStatusListResponse();
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objDelivery.FinancialInvoiceNumber = objDelivery.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objDelivery.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objDelivery.InvoiceDate).ToString("yy");
                    objDelivery.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    try
                    {
                        objDelivery.PaymentAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "PaymentAmount")));
                        objDelivery.PaymentAmount = Convert.ToDecimal(objDelivery.PaymentAmount.ToString() + ".00");
                    }
                    catch { objDelivery.PaymentAmount = Convert.ToDecimal("0.00"); }
                    objDelivery.ByCash = objBaseSqlManager.GetDecimal(dr, "ByCash");
                    objDelivery.AdjustAmount = objBaseSqlManager.GetDecimal(dr, "AdjustAmount");
                    objDelivery.ByCheque = objBaseSqlManager.GetBoolean(dr, "ByCheque");
                    objDelivery.BySign = objBaseSqlManager.GetBoolean(dr, "BySign");
                    objDelivery.ByCard = objBaseSqlManager.GetBoolean(dr, "ByCard");
                    objDelivery.ByOnline = objBaseSqlManager.GetBoolean(dr, "ByOnline");
                    objDelivery.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");
                    objDelivery.Remark = objBaseSqlManager.GetTextValue(dr, "Remark");
                    objDelivery.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    if (objBaseSqlManager.GetTextValue(dr, "Container") == "")
                    {
                        if (oldorderid == 0 || objDelivery.OrderID != oldorderid)
                        {
                            string TotalContainer = GetTotalContainerbyOrderID(objDelivery.OrderID);
                            objDelivery.Container = TotalContainer.TrimEnd(',');
                            oldorderid = objDelivery.OrderID;
                        }
                    }
                    else
                    {
                        objDelivery.Container = objBaseSqlManager.GetTextValue(dr, "Container");
                    }
                    objDelivery.AreaID = objBaseSqlManager.GetTextValue(dr, "AreaID");
                    objDelivery.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objDelivery.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objDelivery.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    objDelivery.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    objDelivery.DisableRow = objBaseSqlManager.GetInt32(dr, "DisableRow");
                    objDelivery.CustBankName = objBaseSqlManager.GetTextValue(dr, "CustBankName");
                    objDelivery.CustBranch = objBaseSqlManager.GetTextValue(dr, "CustBranch");
                    objDelivery.CustIFCCode = objBaseSqlManager.GetTextValue(dr, "CustIFCCode");
                    objDelivery.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objDelivery.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    // 04 Aug 2020 Piyush Limbani
                    objDelivery.ActualCreatedDate = objBaseSqlManager.GetDateTime(dr, "ActualCreatedDate");
                    if (objDelivery.ActualCreatedDate == Convert.ToDateTime("10/10/2014") || objDelivery.ActualCreatedDate == null)
                    {
                        objDelivery.ActualCreatedDatestr = "";
                    }
                    else
                    {
                        objDelivery.ActualCreatedDatestr = objDelivery.ActualCreatedDate.ToString("MM/dd/yyyy");
                    }
                    // 04 Aug 2020 Piyush Limbani

                    // 07 Sep. 2020 Piyush Limbani
                    objDelivery.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objDelivery.PDFName = objBaseSqlManager.GetTextValue(dr, "PDFName");
                    objDelivery.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    // 07 Sep. 2020 Piyush Limbani

                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        private string GetTotalContainerbyOrderID(long OrderID)
        {
            string TotalContainers = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalContainerbyOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TotalContainer objCustomer = new TotalContainer();
                    objCustomer.Container = objBaseSqlManager.GetTextValue(dr, "Container");
                    TotalContainers += objCustomer.Container + ",";
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TotalContainers;
            }
        }

        public Int64 AddDeliveryAllocation(RetDeliveryStatusListResponse ObjDel)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                RetDeliveryAllocationMst delm = new RetDeliveryAllocationMst();
                if (ObjDel.DeliveryAllocationID == 0)
                {
                    delm.VehicleNo = ObjDel.VehicleNo;
                    delm.TempoNo = ObjDel.TempoNo;
                    delm.AssignedDate = ObjDel.AssignedDate.Value;
                    delm.CreatedOn = ObjDel.CreatedOn.Value;
                    delm.CreatedBy = ObjDel.CreatedBy;
                    delm.DeliveryPerson1 = ObjDel.DeliveryPerson1;
                    delm.DeliveryPerson2 = ObjDel.DeliveryPerson2;
                    delm.DeliveryPerson3 = ObjDel.DeliveryPerson3;
                    delm.DeliveryPerson4 = ObjDel.DeliveryPerson4;
                    delm.AreaID = ObjDel.AreaID;
                    context.RetDeliveryAllocationMsts.Add(delm);
                    context.SaveChanges();
                }
                else
                {
                    delm.DeliveryAllocationID = ObjDel.DeliveryAllocationID;
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateRetTempoSheetDeliveryAllocationDetails";
                        cmdGet.Parameters.AddWithValue("@VehicleNo", ObjDel.VehicleNo);
                        cmdGet.Parameters.AddWithValue("@TempoNo", ObjDel.TempoNo);
                        cmdGet.Parameters.AddWithValue("@UpdatedOn", ObjDel.UpdatedOn);
                        cmdGet.Parameters.AddWithValue("@UpdatedBy", ObjDel.UpdatedBy);
                        cmdGet.Parameters.AddWithValue("@DeliveryPerson1", ObjDel.DeliveryPerson1);
                        cmdGet.Parameters.AddWithValue("@DeliveryPerson2", ObjDel.DeliveryPerson2);
                        cmdGet.Parameters.AddWithValue("@DeliveryPerson3", ObjDel.DeliveryPerson3);
                        cmdGet.Parameters.AddWithValue("@DeliveryPerson4", ObjDel.DeliveryPerson4);
                        cmdGet.Parameters.AddWithValue("@AreaID", ObjDel.AreaID);
                        cmdGet.Parameters.AddWithValue("@DeliveryAllocationID", ObjDel.DeliveryAllocationID);
                        try
                        {
                            objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                if (delm.DeliveryAllocationID > 0)
                {
                    return delm.DeliveryAllocationID;
                }
                else
                {
                    return delm.DeliveryAllocationID;
                }
            }
        }




        // 12-01-2019 Tempo Summary

        public List<RetTempoSummary> GetPackDetailVehicleList(DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPackDetailVehicleList";
                cmdGet.Parameters.AddWithValue("@DeliveryAssignDate", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetTempoSummary> objlst = new List<RetTempoSummary>();
                while (dr.Read())
                {
                    RetTempoSummary objDelivery = new RetTempoSummary();
                    objDelivery.VehicleNo = objBaseSqlManager.GetInt32(dr, "VehicleNo");
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public RetTempoSummary GetTempoSummaryDetailList(DateTime CreatedOn, int VehicleNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoSummaryDetailList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetTempoSummary objDelivery = new RetTempoSummary();
                while (dr.Read())
                {
                    objDelivery.VehicleNo = objBaseSqlManager.GetInt32(dr, "VehicleNo");
                    objDelivery.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    objDelivery.AreaNme = objBaseSqlManager.GetTextValue(dr, "AreaNme");
                    string DeliveryPersons = objBaseSqlManager.GetTextValue(dr, "DeliveryPersons").TrimEnd(' ');
                    objDelivery.DeliveryPersons = DeliveryPersons.Remove(DeliveryPersons.Length - 1);
                    objDelivery.Tray = objBaseSqlManager.GetTextValue(dr, "Tray");
                    objDelivery.Other = objBaseSqlManager.GetTextValue(dr, "Other");
                    if (objDelivery.Tray == "" && objDelivery.Tray == "")
                    {
                        string TotalContainer = GetTotalContainerbyVahicleNoandDate(CreatedOn, VehicleNo);
                        objDelivery.Container = TotalContainer.TrimEnd(',');
                    }
                    else
                    {
                        objDelivery.Container = "";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objDelivery;
            }
        }

        private string GetTotalContainerbyVahicleNoandDate(DateTime CreatedOn, int VehicleNo)
        {
            string TotalContainers = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalContainerbyVahicleNoandDate";
                cmdGet.Parameters.AddWithValue("@DeliveryAssignDate", CreatedOn);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TotalContainer objCustomer = new TotalContainer();
                    objCustomer.Container = objBaseSqlManager.GetTextValue(dr, "Container");
                    TotalContainers += objCustomer.Container + ",";
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TotalContainers;
            }
        }







        //public List<RetTempoSummary> GetTempoSummaryList(DateTime CreatedOn)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetRetTempoSummaryList";
        //    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<RetTempoSummary> objlst = new List<RetTempoSummary>();
        //    while (dr.Read())
        //    {
        //        RetTempoSummary objDelivery = new RetTempoSummary();
        //        objDelivery.VehicleSummary = objBaseSqlManager.GetTextValue(dr, "VehicleSummary");
        //        objlst.Add(objDelivery);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return objlst;
        //}

        public RetDeliveryStatusListResponse GetDeliveryInfoList(long VehicleNo, DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetDeliveryInfoList";
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetDeliveryStatusListResponse objDelivery = new RetDeliveryStatusListResponse();
                while (dr.Read())
                {
                    objDelivery.VehicleNo = objBaseSqlManager.GetInt64(dr, "VehicleNo");
                    objDelivery.AssignedDate = objBaseSqlManager.GetDateTime(dr, "AssignedDate");
                    objDelivery.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    objDelivery.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    objDelivery.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    objDelivery.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    objDelivery.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    objDelivery.AreaID = objBaseSqlManager.GetTextValue(dr, "AreaID");
                    objDelivery.DeliveryAllocationID = objBaseSqlManager.GetInt64(dr, "DeliveryAllocationID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objDelivery;
            }
        }

        public RetDeliveryStatusListResponse GetTempoInfoList(long VehicleNo, DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetTempoInfoList";
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetDeliveryStatusListResponse objDelivery = new RetDeliveryStatusListResponse();
                while (dr.Read())
                {
                    objDelivery.VehicleNo = objBaseSqlManager.GetInt64(dr, "VehicleNo");
                    objDelivery.AssignedDate = objBaseSqlManager.GetDateTime(dr, "AssignedDate");
                    objDelivery.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    objDelivery.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    objDelivery.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    objDelivery.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    objDelivery.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    objDelivery.AreaID = objBaseSqlManager.GetTextValue(dr, "AreaID");
                    objDelivery.DeliveryAllocationID = objBaseSqlManager.GetInt64(dr, "DeliveryAllocationID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objDelivery;
            }
        }

        //public bool UpdatePendingDelivery(List<RetOrderPendingRequest> data, long SessionUserID)
        //{
        //    string id = "";
        //    string OrderID = "";
        //    using (VirakiEntities context = new VirakiEntities())
        //    {
        //        foreach (var item in data)
        //        {
        //            RetDeliveryMst dm = new RetDeliveryMst();
        //            dm.VehicleNo = item.VehicleNo;
        //            dm.Tray = item.Tray;
        //            dm.Other = item.Other;
        //            dm.InvoiceNumber = item.InvoiceNumber;
        //            dm.InvoiceDate = Convert.ToDateTime(DateTime.Now);
        //            dm.OrderID = item.OrderID;
        //            dm.CreatedBy = SessionUserID;
        //            dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
        //            context.RetDeliveryMsts.Add(dm);
        //            context.SaveChanges();
        //            if (!string.IsNullOrEmpty(dm.InvoiceNumber))
        //            {
        //                id += dm.InvoiceNumber + ",";
        //            }
        //        }
        //        SqlCommand cmdGet = new SqlCommand();
        //        BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //        cmdGet.CommandType = CommandType.StoredProcedure;
        //        cmdGet.CommandText = "UpdatePendingDeliveryOfRetOrder";
        //        cmdGet.Parameters.AddWithValue("@InvoiceNumber", id.TrimEnd(','));
        //        objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //        objBaseSqlManager.ForceCloseConnection();
        //    }
        //    return true;
        //}

        public bool UpdatePendingDelivery(List<RetOrderPendingRequest> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    RetDeliveryMst dm = new RetDeliveryMst();
                    dm.VehicleNo = item.VehicleNo;
                    dm.Tray = item.Tray;
                    dm.Other = item.Other;
                    dm.InvoiceNumber = item.InvoiceNumber;
                    // 05 Aug 2020 Piyush Limbani
                    if (item.AssignedDate == null || item.AssignedDate == Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                    {
                        dm.InvoiceDate = Convert.ToDateTime(DateTime.Now);
                    }
                    else
                    {
                        dm.InvoiceDate = item.AssignedDate;
                    }
                    // 05 Aug 2020 Piyush Limbani
                    //dm.InvoiceDate = Convert.ToDateTime(DateTime.Now);
                    dm.OrderID = item.OrderID;
                    dm.CreatedBy = SessionUserID;
                    // 05 Aug 2020 Piyush Limbani
                    if (item.AssignedDate == null || item.AssignedDate == Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                    {
                        dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    }
                    else
                    {
                        dm.CreatedOn = item.AssignedDate;
                    }
                    // 05 Aug 2020 Piyush Limbani
                    //dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    dm.IsVehicleCosting = false;
                    // 05 Aug 2020 Piyush Limbani Add New Field
                    dm.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                    // 05 Aug 2020 Piyush Limbani Add New Field
                    context.RetDeliveryMsts.Add(dm);
                    context.SaveChanges();
                    if (!string.IsNullOrEmpty(dm.InvoiceNumber))
                    {
                        SqlCommand cmdGet = new SqlCommand();
                        using (var objBaseSqlManager = new BaseSqlManager())
                        {
                            cmdGet.CommandType = CommandType.StoredProcedure;
                            cmdGet.CommandText = "UpdatePendingDeliveryOfRetOrder";
                            cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                            cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                            objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                    }
                    if (dm.OrderID != 0)
                    {
                        SqlCommand cmdGet = new SqlCommand();
                        using (var objBaseSqlManager = new BaseSqlManager())
                        {
                            cmdGet.CommandType = CommandType.StoredProcedure;
                            cmdGet.CommandText = "UpdateVehicleNoforCheckList";
                            cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                            cmdGet.Parameters.AddWithValue("@DeliveryAssignDate", dm.InvoiceDate);
                            cmdGet.Parameters.AddWithValue("@VehicleNo", dm.VehicleNo);
                            objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                    }
                    // 04-04-2019 Update Vehicle Assign Date in RetOrderQtyMst (Pending Delivery CretedOn = VehicleAssignedDate)
                    if (!string.IsNullOrEmpty(dm.InvoiceNumber))
                    {
                        SqlCommand cmdGet = new SqlCommand();
                        using (var objBaseSqlManager = new BaseSqlManager())
                        {
                            cmdGet.CommandType = CommandType.StoredProcedure;
                            cmdGet.CommandText = "UpdateVehicleAssignDateOfRetOrder";
                            cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                            cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                            cmdGet.Parameters.AddWithValue("@VehicleAssignedDate", dm.CreatedOn);
                            objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                    }
                }
            }
            return true;
        }

        //public bool RemovePendingDeliveryOfOrder(string data)
        //{
        //    try
        //    {
        //        SqlCommand cmdGet = new SqlCommand();
        //        BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //        cmdGet.CommandType = CommandType.StoredProcedure;
        //        cmdGet.CommandText = "RemoveRetDeliveryOfOrder";
        //        cmdGet.Parameters.AddWithValue("@InvoiceNumber", data.TrimEnd(','));
        //        objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //        objBaseSqlManager.ForceCloseConnection();

        //        cmdGet = new SqlCommand();
        //        objBaseSqlManager = new BaseSqlManager();
        //        cmdGet.CommandType = CommandType.StoredProcedure;
        //        cmdGet.CommandText = "RemoveRetPendingDeliveryOfOrder";
        //        cmdGet.Parameters.AddWithValue("@InvoiceNumber", data.TrimEnd(','));
        //        objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //        objBaseSqlManager.ForceCloseConnection();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;

        //    }
        //}

        public bool RemovePendingDeliveryOfOrder(List<RetOrderRemoveFromTempo> data)
        {
            try
            {
                foreach (var item in data)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "RemoveRetDeliveryOfOrder";
                        cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                        cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                    cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "RemoveRetPendingDeliveryOfOrder";
                        cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                        cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                return true;
            }
            catch
            {
                return false;

            }
        }

        public bool UpdateChequeDetailsForPayment(RetDeliveryStatusListResponse data)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (data.OrderID != 0)
                {
                    Payment_Mst dm = new Payment_Mst();
                    dm.OrderID = data.OrderID;
                    dm.ByCheque = true;
                    dm.BankName = data.BankName;
                    dm.BankBranch = data.BankBranch;
                    dm.ChequeNo = data.ChequeNo;
                    dm.UpdatedBy = Convert.ToInt64(1);
                    dm.UpdatedOn = Convert.ToDateTime(DateTime.Now);

                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateRetChequeDetailsForPayment";
                        cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                        cmdGet.Parameters.AddWithValue("@ByCheque", dm.ByCheque);
                        cmdGet.Parameters.AddWithValue("@BankName", dm.BankName);
                        cmdGet.Parameters.AddWithValue("@BankBranch", dm.BankBranch);
                        cmdGet.Parameters.AddWithValue("@ChequeNo", dm.ChequeNo);
                        cmdGet.Parameters.AddWithValue("@UpdatedBy", dm.UpdatedBy);
                        cmdGet.Parameters.AddWithValue("@UpdatedOn", dm.UpdatedOn);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            return true;
        }

        public bool UpdatePayment(List<RetDeliveryStatusListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    //NewCode
                    var sign = context.RetPaymentMsts.Where(i => i.OrderID == item.OrderID && i.InvoiceNumber == item.InvoiceNumber && i.BySign == true).FirstOrDefault();
                    //END NEW CODE

                    //bool existpmnt0vehicle = CheckExistPaymnetOf0Vehicle(item.InvoiceNumber);
                    bool existpmnt0vehicle = CheckExistPaymnetOf0Vehicle(item.InvoiceNumber, item.OrderID);

                    #region NewCode
                    if (sign != null && Convert.ToInt64(sign.ByCash) == 0)
                    {
                        if (item.PaymentID == 0)
                        {
                            RetPaymentMst dm = new RetPaymentMst();
                            dm.OrderID = item.OrderID;
                            dm.InvoiceNumber = item.InvoiceNumber;
                            dm.VehicleNo = Convert.ToInt32(item.VehicleNo);

                            if (item.AssignedDate == null)
                            {
                                dm.AssignedDate = DateTime.Now;
                            }
                            else
                            {
                                dm.AssignedDate = item.AssignedDate;
                            }
                            dm.CustomerID = item.CustomerID;
                            dm.AdjustAmount = item.AdjustAmount;
                            dm.PaymentAmount = item.PaymentAmount;
                            dm.Containers = item.Container;
                            dm.ByCash = item.ByCash;
                            dm.ByCheque = item.ByCheque;
                            dm.BankName = item.BankName;
                            dm.BankBranch = item.BankBranch;
                            dm.ChequeNo = item.ChequeNo;
                            dm.ChequeDate = item.ChequeDate;
                            dm.IFCCode = item.IFCCode;
                            dm.BySign = item.BySign;
                            dm.ByCard = item.ByCard;
                            dm.BankNameForCard = item.BankNameForCard;
                            dm.TypeOfCard = item.TypeOfCard;
                            dm.ByOnline = item.ByOnline;
                            dm.BankNameForOnline = item.BankNameForOnline;
                            dm.UTRNumber = item.UTRNumber;
                            dm.OnlinePaymentDate = item.OnlinePaymentDate;
                            dm.IsDelivered = item.IsDelivered;
                            dm.IsDelete = item.IsDelete;
                            dm.Remark = item.Remark;

                            dm.GodownID = item.GodownID;
                            if (dm.VehicleNo != 0)
                            {
                                dm.IsInwardTempo = true;
                            }
                            else
                            {
                                dm.IsInwardTempo = false;
                            }

                            dm.CreatedBy = SessionUserID;
                            dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                            context.RetPaymentMsts.Add(dm);
                            context.SaveChanges();
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateRetDeliveredOrder";
                                    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }
                            // New add

                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateRetDeliveredPayment";
                                    cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }


                            // 05-04-2019 For delivery sheet
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateVehicleAssignDateOfRetOrder";
                                    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                    cmdGet.Parameters.AddWithValue("@VehicleAssignedDate", dm.AssignedDate);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }

                            // 10-01-2019
                            //if (dm.Containers != null)
                            //{
                            //    SqlCommand cmdGet = new SqlCommand();
                            //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
                            //    cmdGet.CommandType = CommandType.StoredProcedure;
                            //    cmdGet.CommandText = "UpdateRetDeliveredMstContainer";
                            //    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                            //    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                            //    cmdGet.Parameters.AddWithValue("@Containers", dm.Containers);
                            //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            //    objBaseSqlManager.ForceCloseConnection();
                            //}


                            bool exist = CheckExistDeliveryOfInvoice(dm.InvoiceNumber, dm.VehicleNo, dm.AssignedDate);
                            if (exist == false)
                            {
                                RetDeliveryMst dms = new RetDeliveryMst();
                                dms.VehicleNo = dm.VehicleNo.Value;
                                dms.Tray = "";
                                dms.Other = "";
                                dms.InvoiceNumber = dm.InvoiceNumber;
                                dms.OrderID = item.OrderID;
                                dms.InvoiceDate = dm.AssignedDate;
                                dms.CreatedBy = SessionUserID;
                                dms.CreatedOn = dm.AssignedDate.Value;
                                //dms.IsVehicleCosting = false;
                                dms.IsVehicleCosting = true;
                                // 05 Aug 2020 Piyush Limbani Add New Field
                                dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                                // 05 Aug 2020 Piyush Limbani Add New Field
                                context.RetDeliveryMsts.Add(dms);
                                context.SaveChanges();
                            }
                        }
                    }
                    //END NEW CODE REmove Else
                    #endregion

                    else if (existpmnt0vehicle == true && item.DeliveryType == 1)
                    {
                        if (item.IsDelivered == true)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateRetDeliveredOrder";
                                cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }

                        // New add
                        if (item.IsDelivered == true)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateRetDeliveredPayment";
                                cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }

                        if (item.AssignedDate == null)
                        {
                            item.AssignedDate = DateTime.Now;
                        }
                        else
                        {
                            item.AssignedDate = item.AssignedDate;
                        }
                        bool exist = CheckExistDeliveryOfInvoice(item.InvoiceNumber, Convert.ToInt32(item.VehicleNo), item.AssignedDate);

                        // && item.IsDelivered==true
                        if (exist == false && item.IsDelivered == true)
                        {
                            RetDeliveryMst dms = new RetDeliveryMst();
                            dms.VehicleNo = Convert.ToInt32(item.VehicleNo);
                            dms.Tray = "";
                            dms.Other = "";
                            dms.InvoiceNumber = item.InvoiceNumber;
                            dms.OrderID = item.OrderID;
                            dms.InvoiceDate = item.AssignedDate;
                            dms.CreatedBy = SessionUserID;
                            dms.CreatedOn = item.AssignedDate.Value;
                            //dms.IsVehicleCosting = false;
                            dms.IsVehicleCosting = true;
                            // 05 Aug 2020 Piyush Limbani Add New Field
                            dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                            // 05 Aug 2020 Piyush Limbani Add New Field
                            context.RetDeliveryMsts.Add(dms);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        if (item.PaymentID == 0)
                        {
                            RetPaymentMst dm = new RetPaymentMst();
                            dm.OrderID = item.OrderID;
                            dm.InvoiceNumber = item.InvoiceNumber;
                            dm.VehicleNo = Convert.ToInt32(item.VehicleNo);

                            if (item.AssignedDate == null)
                            {
                                dm.AssignedDate = DateTime.Now;
                            }
                            else
                            {
                                dm.AssignedDate = item.AssignedDate;
                            }
                            dm.CustomerID = item.CustomerID;
                            dm.AdjustAmount = item.AdjustAmount;
                            dm.PaymentAmount = item.PaymentAmount;
                            dm.Containers = item.Container;
                            dm.ByCash = item.ByCash;
                            dm.ByCheque = item.ByCheque;
                            dm.BankName = item.BankName;
                            dm.BankBranch = item.BankBranch;
                            dm.ChequeNo = item.ChequeNo;
                            dm.ChequeDate = item.ChequeDate;
                            dm.IFCCode = item.IFCCode;
                            dm.BySign = item.BySign;
                            dm.ByCard = item.ByCard;
                            dm.BankNameForCard = item.BankNameForCard;
                            dm.TypeOfCard = item.TypeOfCard;
                            dm.ByOnline = item.ByOnline;
                            dm.BankNameForOnline = item.BankNameForOnline;
                            dm.UTRNumber = item.UTRNumber;
                            dm.OnlinePaymentDate = item.OnlinePaymentDate;
                            dm.IsDelivered = item.IsDelivered;
                            dm.IsDelete = item.IsDelete;
                            dm.Remark = item.Remark;
                            dm.GodownID = item.GodownID;
                            if (dm.VehicleNo != 0)
                            {
                                dm.IsInwardTempo = true;
                            }
                            else
                            {
                                dm.IsInwardTempo = false;
                            }
                            dm.CreatedBy = SessionUserID;
                            dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                            context.RetPaymentMsts.Add(dm);
                            context.SaveChanges();
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateRetDeliveredOrder";
                                    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }
                            // New add
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateRetDeliveredPayment";
                                    cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }

                            // 05-04-2019 For delivery sheet
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateVehicleAssignDateOfRetOrder";
                                    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                    cmdGet.Parameters.AddWithValue("@VehicleAssignedDate", dm.AssignedDate);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }


                            // 10-01-2019
                            //if (dm.Containers != null)
                            //{
                            //    SqlCommand cmdGet = new SqlCommand();
                            //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
                            //    cmdGet.CommandType = CommandType.StoredProcedure;
                            //    cmdGet.CommandText = "UpdateRetDeliveredMstContainer";
                            //    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                            //    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                            //    cmdGet.Parameters.AddWithValue("@Containers", dm.Containers);
                            //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            //    objBaseSqlManager.ForceCloseConnection();
                            //}

                            bool exist = CheckExistDeliveryOfInvoice(dm.InvoiceNumber, dm.VehicleNo, dm.AssignedDate);
                            if (exist == false)
                            {
                                RetDeliveryMst dms = new RetDeliveryMst();
                                dms.VehicleNo = dm.VehicleNo.Value;
                                dms.Tray = "";
                                dms.Other = "";
                                dms.InvoiceNumber = dm.InvoiceNumber;
                                dms.OrderID = dm.OrderID;
                                dms.InvoiceDate = dm.AssignedDate;
                                dms.CreatedBy = SessionUserID;
                                dms.CreatedOn = dm.AssignedDate.Value;
                                //dms.IsVehicleCosting = false;
                                dms.IsVehicleCosting = true;
                                // 05 Aug 2020 Piyush Limbani Add New Field
                                dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                                // 05 Aug 2020 Piyush Limbani Add New Field
                                context.RetDeliveryMsts.Add(dms);
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            RetPaymentMst dm = new RetPaymentMst();
                            dm.PaymentID = item.PaymentID;
                            dm.OrderID = item.OrderID;
                            dm.InvoiceNumber = item.InvoiceNumber;
                            dm.VehicleNo = Convert.ToInt32(item.VehicleNo);
                            if (item.AssignedDate == null)
                            {
                                dm.AssignedDate = DateTime.Now;
                            }
                            else
                            {
                                dm.AssignedDate = item.AssignedDate;
                            }
                            dm.CustomerID = item.CustomerID;
                            dm.AdjustAmount = item.AdjustAmount;
                            dm.PaymentAmount = item.PaymentAmount;
                            dm.Containers = item.Container;
                            dm.ByCash = item.ByCash;
                            dm.ByCheque = item.ByCheque;
                            dm.BankName = item.BankName;
                            dm.BankBranch = item.BankBranch;
                            dm.ChequeNo = item.ChequeNo;
                            dm.ChequeDate = item.ChequeDate;
                            dm.IFCCode = item.IFCCode;
                            dm.BySign = item.BySign;
                            dm.ByCard = item.ByCard;
                            dm.BankNameForCard = item.BankNameForCard;
                            dm.TypeOfCard = item.TypeOfCard;
                            dm.ByOnline = item.ByOnline;
                            dm.BankNameForOnline = item.BankNameForOnline;
                            dm.UTRNumber = item.UTRNumber;
                            dm.OnlinePaymentDate = item.OnlinePaymentDate;
                            dm.IsDelivered = item.IsDelivered;
                            dm.IsDelete = item.IsDelete;
                            dm.Remark = item.Remark;
                            dm.GodownID = item.GodownID;
                            bool IsInwardTempoStatus = GetIsInwardTempoStatus(dm.PaymentID);
                            dm.IsInwardTempo = IsInwardTempoStatus;

                            if (dm.CreatedBy == 0)
                            {
                                dm.CreatedBy = SessionUserID;
                                dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                            }
                            else
                            {
                                dm.CreatedBy = item.CreatedBy;
                                dm.CreatedOn = item.CreatedOn;
                            }
                            dm.UpdatedBy = SessionUserID;
                            dm.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                            context.Entry(dm).State = EntityState.Modified;
                            context.SaveChanges();
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateRetDeliveredOrder";
                                    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }
                            // New add
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateRetDeliveredPayment";
                                    cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }

                            // 05-04-2019 For delivery sheet
                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateVehicleAssignDateOfRetOrder";
                                    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                    cmdGet.Parameters.AddWithValue("@VehicleAssignedDate", dm.AssignedDate);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }

                            // 10-01-2019
                            //if (dm.Containers != null)
                            //{
                            //    SqlCommand cmdGet = new SqlCommand();
                            //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
                            //    cmdGet.CommandType = CommandType.StoredProcedure;
                            //    cmdGet.CommandText = "UpdateRetDeliveredMstContainer";
                            //    cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                            //    cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                            //    cmdGet.Parameters.AddWithValue("@Containers", dm.Containers);
                            //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            //    objBaseSqlManager.ForceCloseConnection();
                            //}

                        }
                    }
                }

            }
            return true;
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

        public bool DeleteOrderQty(string InvoiceNumber, long OrderID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetOrderQty";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();

                // 21 Aug 2020 Piyush Limbani
                bool exist = CheckExistPaymentOfInvoice(InvoiceNumber, OrderID);
                if (exist == true)
                {
                    SqlCommand cmdGet1 = new SqlCommand();
                    using (var objBaseSqlManager1 = new BaseSqlManager())
                    {
                        cmdGet1.CommandType = CommandType.StoredProcedure;
                        cmdGet1.CommandText = "DeleteRetInvoiceFromPayment";
                        cmdGet1.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                        cmdGet1.Parameters.AddWithValue("@OrderID", OrderID);
                        cmdGet1.Parameters.AddWithValue("@IsDelete", IsDelete);
                        object dr1 = objBaseSqlManager.ExecuteNonQuery(cmdGet1);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                // 21 Aug 2020 Piyush Limbani
            }
            return true;
        }

        // 21 Aug 2020 Piyush Limbani
        private bool CheckExistPaymentOfInvoice(string InvoiceNumber, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetExistPaymentOfInvoice";
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
        // 21 Aug 2020 Piyush Limbani

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

        private bool CheckExistPaymnetOf0Vehicle(string InvoiceNumber, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistRetPaymnetOf0Vehicle";
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


        public bool GetIsInwardTempoStatus(long PaymentID)
        {
            bool IsInwardTempo = false;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailIsInwardTempoStatus";
                cmdGet.Parameters.AddWithValue("@PaymentID", PaymentID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    IsInwardTempo = objBaseSqlManager.GetBoolean(dr, "IsInwardTempo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return IsInwardTempo;
            }
        }





        // 13 Dec 2021 Piyush Limbani
        public List<RetDayWiseSalesApproveList> DeleteSalesApprovalList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "RetDayWiseSalesApproveList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDayWiseSalesApproveList> objlst = new List<RetDayWiseSalesApproveList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                decimal sumTCSTaxAmount = 0;
                decimal GrandNetAmount = 0;
                while (dr.Read())
                {
                    RetDayWiseSalesApproveList obj = new RetDayWiseSalesApproveList();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.InvoiceNumber = obj.InvCode + "/" + DateTimeExtensions.FromFinancialYear(obj.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.CreatedOn).ToString("yy");
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrossAmt"), 2);
                    obj.GrandGrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandGrossAmt"), 2);
                    //obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    if (objBaseSqlManager.GetTextValue(dr, "Tax") == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                        obj.TaxAmtIGST = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmt"), 2).ToString();
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0).ToString();
                        obj.TaxAmtSGST = Convert.ToDecimal(0).ToString();
                        sumIGST = sumIGST + Convert.ToDecimal(obj.TaxAmtIGST);
                    }
                    else
                    {
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0).ToString();
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtCGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtSGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        sumCGST = sumCGST + Convert.ToDecimal(obj.TaxAmtCGST);
                        sumSGST = sumSGST + Convert.ToDecimal(obj.TaxAmtSGST);
                    }
                    obj.TCSTaxAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount")), 2);
                    sumTCSTaxAmount = sumTCSTaxAmount + obj.TCSTaxAmount;
                    obj.NetAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmount")), 2);
                    obj.RoundOff = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOff")), 2);
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.IsApprove = objBaseSqlManager.GetBoolean(dr, "IsApprove");
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandCGSTAmt = sumCGST;
                    objlst[i].GrandIGSTAmt = sumIGST;
                    objlst[i].GrandSGSTAmt = sumSGST;
                    objlst[i].GrandTCSAmt = sumTCSTaxAmount;
                    objlst[i].GrandNetAmount = sumTCSTaxAmount + GrandNetAmount;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateDayWiseSalesApprove(long OrderID, string InvoiceNumber, bool IsApprove)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateRetDayWiseSalesApprove";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    cmdGet.Parameters.AddWithValue("@IsApprove", IsApprove);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }
        // 13 Dec 2021 Piyush Limbani


    }
}

