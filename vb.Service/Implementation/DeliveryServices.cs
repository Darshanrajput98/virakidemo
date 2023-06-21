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
    public class DeliveryServices : IDeliveryService
    {
        public List<PendingDeliveryListResponse> GetAllPendingDeliveryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPendingDeliveryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PendingDeliveryListResponse> objlst = new List<PendingDeliveryListResponse>();
                while (dr.Read())
                {
                    PendingDeliveryListResponse objDelivery = new PendingDeliveryListResponse();
                    objDelivery.DeliveryID = objBaseSqlManager.GetInt64(dr, "DeliveryID");
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objDelivery.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objDelivery.Tray = objBaseSqlManager.GetTextValue(dr, "Tray");
                    objDelivery.Bag = objBaseSqlManager.GetTextValue(dr, "Bag") == "" ? " " : "Bag-" + objBaseSqlManager.GetTextValue(dr, "Bag") + " ";
                    objDelivery.Jabla = objBaseSqlManager.GetTextValue(dr, "Jabla") == "" ? " " : "Zabla-" + objBaseSqlManager.GetTextValue(dr, "Jabla") + " ";
                    objDelivery.PackBag = objBaseSqlManager.GetTextValue(dr, "PackBag") == "" ? " " : objBaseSqlManager.GetTextValue(dr, "PackBag");
                    objDelivery.Other = objDelivery.Bag + objDelivery.Jabla + objDelivery.PackBag;
                    objDelivery.Other = objDelivery.Other.Trim();
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<DeliveryStatusListResponse> GetPendingInvoiceListForPrint(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPendingInvoiceListForPrint";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DeliveryStatusListResponse> objlst = new List<DeliveryStatusListResponse>();
                while (dr.Read())
                {
                    DeliveryStatusListResponse objDelivery = new DeliveryStatusListResponse();
                    objDelivery.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
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

        public List<DeliveryStatusListResponse> GetDeliveryStatusList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDeliveryStatusList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DeliveryStatusListResponse> objlst = new List<DeliveryStatusListResponse>();
                while (dr.Read())
                {
                    DeliveryStatusListResponse objDelivery = new DeliveryStatusListResponse();
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    objDelivery.Container = objBaseSqlManager.GetTextValue(dr, "Container");
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
                    objDelivery.ByOnline = objBaseSqlManager.GetBoolean(dr, "ByOnline");
                    objDelivery.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");
                    objDelivery.Remark = objBaseSqlManager.GetTextValue(dr, "Remark");
                    objDelivery.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
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
                    objDelivery.PackedByID = objBaseSqlManager.GetInt64(dr, "PackedByID");
                    objDelivery.Tray = objBaseSqlManager.GetTextValue(dr, "Tray");
                    objDelivery.Jabla = objBaseSqlManager.GetTextValue(dr, "Jabla");
                    objDelivery.Bag = objBaseSqlManager.GetTextValue(dr, "Bag");
                    objDelivery.PackBag = objBaseSqlManager.GetTextValue(dr, "PackBag");
                    objDelivery.PackedBy = objBaseSqlManager.GetBoolean(dr, "PackedBy");
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

        public List<DeliveryStatusListResponse> GetTempoSheetList(long VehicleNo, DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoSheetList";
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DeliveryStatusListResponse> objlst = new List<DeliveryStatusListResponse>();
                while (dr.Read())
                {
                    DeliveryStatusListResponse objDelivery = new DeliveryStatusListResponse();
                    objDelivery.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objDelivery.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    objDelivery.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objDelivery.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    objDelivery.Container = objBaseSqlManager.GetTextValue(dr, "Container");
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
                    objDelivery.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objDelivery.PDFName = objBaseSqlManager.GetTextValue(dr, "PDFName");
                    objDelivery.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
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
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public Int64 AddDeliveryAllocation(DeliveryStatusListResponse ObjDel)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                DeliveryAllocation_Mst delm = new DeliveryAllocation_Mst();
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
                    context.DeliveryAllocation_Mst.Add(delm);
                    context.SaveChanges();
                }
                else
                {
                    delm.DeliveryAllocationID = ObjDel.DeliveryAllocationID;
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateTempoSheetDeliveryAllocationDetails";
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

        public List<TempoSummary> GetTempoSummaryList(DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoSummaryList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TempoSummary> objlst = new List<TempoSummary>();
                while (dr.Read())
                {
                    TempoSummary objDelivery = new TempoSummary();
                    objDelivery.VehicleSummary = objBaseSqlManager.GetTextValue(dr, "VehicleSummary");
                    objlst.Add(objDelivery);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public DeliveryStatusListResponse GetDeliveryInfoList(long VehicleNo, DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDeliveryInfoList";
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                DeliveryStatusListResponse objDelivery = new DeliveryStatusListResponse();
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

        public DeliveryStatusListResponse GetTempoInfoList(long VehicleNo, DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoInfoList";
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                DeliveryStatusListResponse objDelivery = new DeliveryStatusListResponse();
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

        public bool UpdatePendingDelivery(List<OrderPendingRequest> data, long SessionUserID)
        {
            string id = "";
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    Delivery_Mst dm = new Delivery_Mst();
                    dm.VehicleNo = item.VehicleNo;
                    dm.Tray = item.Tray;
                    dm.Other = item.Other;
                    dm.InvoiceNumber = item.InvoiceNumber;

                    // 04 Aug 2020 Piyush Limbani
                    if (item.AssignedDate == null || item.AssignedDate == Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                    {
                        dm.InvoiceDate = Convert.ToDateTime(DateTime.Now);
                    }
                    else
                    {
                        dm.InvoiceDate = item.AssignedDate;
                    }
                    // 04 Aug 2020 Piyush Limbani

                    //dm.InvoiceDate = Convert.ToDateTime(DateTime.Now);
                    dm.CreatedBy = SessionUserID;

                    // 04 Aug 2020 Piyush Limbani
                    if (item.AssignedDate == null || item.AssignedDate == Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                    {
                        dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    }
                    else
                    {
                        dm.CreatedOn = item.AssignedDate;
                    }
                    // 04 Aug 2020 Piyush Limbani

                    //dm.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    dm.IsVehicleCosting = false;

                    // 04 Aug 2020 Piyush Limbani Add New Field
                    dm.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                    // 04 Aug 2020 Piyush Limbani Add New Field

                    context.Delivery_Mst.Add(dm);
                    context.SaveChanges();
                    if (!string.IsNullOrEmpty(dm.InvoiceNumber))
                    {
                        id += dm.InvoiceNumber + ",";
                    }
                }
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdatePendingDeliveryOfOrder";
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", id.TrimEnd(','));
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
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

        public bool RemovePendingDeliveryOfOrder(string data)
        {
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "RemoveDeliveryOfOrder";
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", data.TrimEnd(','));
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "RemovePendingDeliveryOfOrder";
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", data.TrimEnd(','));
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateChequeDetailsForPayment(DeliveryStatusListResponse data)
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
                        cmdGet.CommandText = "UpdateChequeDetailsForPayment";
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

        public bool UpdatePayment(List<DeliveryStatusListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    //NewCode
                    var sign = context.Payment_Mst.Where(i => i.InvoiceNumber == item.InvoiceNumber && i.BySign == true).FirstOrDefault();
                    //END NEW CODE

                    bool existpmnt0vehicle = CheckExistPaymnetOf0Vehicle(item.InvoiceNumber);

                    #region NewCode
                    if (sign != null && Convert.ToInt64(sign.ByCash) == 0)
                    {
                        if (item.PaymentID == 0)
                        {
                            Payment_Mst dm = new Payment_Mst();
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
                            context.Payment_Mst.Add(dm);
                            context.SaveChanges();

                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                BaseSqlManager objBaseSqlManager = new BaseSqlManager();
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateDeliveredOrder";
                                cmdGet.Parameters.AddWithValue("@OrderID", dm.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", dm.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }

                            // New add

                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                BaseSqlManager objBaseSqlManager = new BaseSqlManager();
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "UpdateDeliveredPayment";
                                cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }

                            bool exist = CheckExistDeliveryOfInvoice(dm.InvoiceNumber, dm.VehicleNo, dm.AssignedDate);

                            //  && item.IsDelivered==true
                            if (exist == false)
                            {
                                Delivery_Mst dms = new Delivery_Mst();
                                dms.VehicleNo = dm.VehicleNo.Value;
                                dms.Tray = "";
                                dms.Other = "";
                                dms.InvoiceNumber = dm.InvoiceNumber;
                                dms.InvoiceDate = dm.AssignedDate;
                                dms.CreatedBy = SessionUserID;
                                dms.CreatedOn = dm.AssignedDate.Value;

                                //dms.IsVehicleCosting = false;
                                dms.IsVehicleCosting = true;

                                // 04 Aug 2020 Piyush Limbani Add New Field
                                dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                                // 04 Aug 2020 Piyush Limbani Add New Field

                                context.Delivery_Mst.Add(dms);
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
                                cmdGet.CommandText = "UpdateDeliveredOrder";
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
                                cmdGet.CommandText = "UpdateDeliveredPayment";
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
                            Delivery_Mst dms = new Delivery_Mst();
                            dms.VehicleNo = Convert.ToInt32(item.VehicleNo);
                            dms.Tray = "";
                            dms.Other = "";
                            dms.InvoiceNumber = item.InvoiceNumber;
                            dms.InvoiceDate = item.AssignedDate;
                            dms.CreatedBy = SessionUserID;
                            dms.CreatedOn = item.AssignedDate.Value;

                            //dms.IsVehicleCosting = false;
                            dms.IsVehicleCosting = true;

                            // 04 Aug 2020 Piyush Limbani Add New Field
                            dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                            // 04 Aug 2020 Piyush Limbani Add New Field

                            context.Delivery_Mst.Add(dms);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        if (item.PaymentID == 0)
                        {
                            Payment_Mst dm = new Payment_Mst();
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
                            context.Payment_Mst.Add(dm);
                            context.SaveChanges();

                            if (dm.IsDelivered == true)
                            {
                                SqlCommand cmdGet = new SqlCommand();
                                using (var objBaseSqlManager = new BaseSqlManager())
                                {
                                    cmdGet.CommandType = CommandType.StoredProcedure;
                                    cmdGet.CommandText = "UpdateDeliveredOrder";
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
                                    cmdGet.CommandText = "UpdateDeliveredPayment";
                                    cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }

                            bool exist = CheckExistDeliveryOfInvoice(dm.InvoiceNumber, dm.VehicleNo, dm.AssignedDate);

                            //  && item.IsDelivered==true
                            if (exist == false)
                            {
                                Delivery_Mst dms = new Delivery_Mst();
                                dms.VehicleNo = dm.VehicleNo.Value;
                                dms.Tray = "";
                                dms.Other = "";
                                dms.InvoiceNumber = dm.InvoiceNumber;
                                dms.InvoiceDate = dm.AssignedDate;
                                dms.CreatedBy = SessionUserID;
                                dms.CreatedOn = dm.AssignedDate.Value;

                                //dms.IsVehicleCosting = false;
                                dms.IsVehicleCosting = true;

                                // 04 Aug 2020 Piyush Limbani Add New Field
                                dms.ActualCreatedDate = Convert.ToDateTime(DateTime.Now);
                                // 04 Aug 2020 Piyush Limbani Add New Field

                                context.Delivery_Mst.Add(dms);
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            Payment_Mst dm = new Payment_Mst();
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
                                    cmdGet.CommandText = "UpdateDeliveredOrder";
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
                                    cmdGet.CommandText = "UpdateDeliveredPayment";
                                    cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", item.InvoiceNumber);
                                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                    objBaseSqlManager.ForceCloseConnection();
                                }
                            }

                        }
                    }
                }

            }
            return true;
        }

        private bool CheckExistPaymnetOf0Vehicle(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistPaymnetOf0Vehicle";
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

        public bool DeleteOrderQty(string InvoiceNumber, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteOrderQty";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            // 21 Aug 2020 Piyush Limbani
            bool exist = CheckExistPaymentOfInvoice(InvoiceNumber);
            if (exist == true)
            {
                SqlCommand cmdGet1 = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet1.CommandType = CommandType.StoredProcedure;
                    cmdGet1.CommandText = "DeleteInvoiceFromPayment";
                    cmdGet1.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    cmdGet1.Parameters.AddWithValue("@IsDelete", IsDelete);
                    object dr1 = objBaseSqlManager.ExecuteNonQuery(cmdGet1);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            // 21 Aug 2020 Piyush Limbani

            return true;
        }

        // 21 Aug 2020 Piyush Limbani
        private bool CheckExistPaymentOfInvoice(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistPaymentOfInvoice";
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
        // 21 Aug 2020 Piyush Limbani

        public bool SavePackedBy(PackedByViewModel SavePackedBy)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    if (SavePackedBy.PackedByID == 0)
                    {
                        PackedBy_Mst obj = new PackedBy_Mst();
                        obj.OrderID = SavePackedBy.OrderID;
                        obj.InvoiceNumber = SavePackedBy.InvoiceNumber;
                        obj.Tray = SavePackedBy.Tray;
                        obj.Bag = SavePackedBy.Bag;
                        obj.Jabla = SavePackedBy.Jabla;
                        obj.PackBag = SavePackedBy.PackBag;
                        obj.CreatedBy = SavePackedBy.UpdatedBy;
                        obj.CreatedOn = SavePackedBy.CreatedOn;
                        obj.UpdatedBy = SavePackedBy.UpdatedBy;
                        obj.UpdatedOn = SavePackedBy.UpdatedOn;
                        obj.IsDelete = true;
                        context.PackedBy_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    else
                    {
                        PackedBy_Mst obj = new PackedBy_Mst();
                        obj.PackedByID = SavePackedBy.PackedByID;
                        obj.OrderID = SavePackedBy.OrderID;
                        obj.InvoiceNumber = SavePackedBy.InvoiceNumber;
                        obj.Tray = SavePackedBy.Tray;
                        obj.Bag = SavePackedBy.Bag;
                        obj.Jabla = SavePackedBy.Jabla;
                        obj.PackBag = SavePackedBy.PackBag;
                        obj.CreatedBy = SavePackedBy.UpdatedBy;
                        obj.CreatedOn = SavePackedBy.CreatedOn;
                        obj.UpdatedBy = SavePackedBy.UpdatedBy;
                        obj.UpdatedOn = SavePackedBy.UpdatedOn;
                        obj.IsDelete = true;
                        context.Entry(obj).State = EntityState.Modified;
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

        private bool CheckExistInvoiceForPackedBy(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExistInvoiceForPackedBy";
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




        public bool GetIsInwardTempoStatus(long PaymentID)
        {
            bool IsInwardTempo = false;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetIsInwardTempoStatus";
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


        // 02 Dec 2021 Sonal Gandhi
        public List<DayWiseSalesApproveList> DeleteSalesApprovalList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DayWiseSalesApproveList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseSalesApproveList> objlst = new List<DayWiseSalesApproveList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                decimal sumTCSTaxAmount = 0;
                decimal GrandNetAmount = 0;
                while (dr.Read())
                {
                    DayWiseSalesApproveList obj = new DayWiseSalesApproveList();
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrossAmt"), 2);
                    obj.GrandGrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandGrossAmt"), 2);
                    //obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    obj.Granddiff = Math.Round(objBaseSqlManager.GetDecimal(dr, "Granddiff"), 2);
                    obj.Discount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Discount"), 2);
                    obj.CreditedTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "CreditedTotal"), 2);
                    obj.CreditedFinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal"), 2);
                    obj.diff = Math.Round(objBaseSqlManager.GetDecimal(dr, "diff"), 2);
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
                    cmdGet.CommandText = "UpdateDayWiseSalesApprove";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    cmdGet.Parameters.AddWithValue("@IsApprove", IsApprove);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }
        // 02 Dec 2021 Sonal Gandhi



        //public static List<DeliveryStatusListPrint> tblfaclilityEmail_LastActive()
        //{
        //    List<DeliveryStatusListPrint> objemail = new List<DeliveryStatusListPrint>();

        //    SqlCommand cmdGets = new SqlCommand();
        //    BaseSqlManager objBaseSqlManagers = new BaseSqlManager();
        //    cmdGets.CommandType = CommandType.StoredProcedure;
        //    cmdGets.CommandText = "GetFacilitybyAdminEmail";
        //    SqlDataReader drs = objBaseSqlManagers.ExecuteDataReader(cmdGets);
        //    List<DeliveryStatusListPrint> lstfacility1 = new List<DeliveryStatusListPrint>();
        //    while (drs.Read())
        //    {
        //        var facidata = objBaseSqlManagers.GetTextValue(drs, "AdminEmail");
        //        string[] arrINID = facidata.Split(',');

        //        foreach (var item in arrINID)
        //        {
        //            ListOfEmail singleemail = new ListOfEmail();
        //            singleemail.FacilityEmail = item;
        //            singleemail.FacilityID = objBaseSqlManagers.GetInt64(drs, "FacilityId");
        //            singleemail.FacilityName = objBaseSqlManagers.GetTextValue(drs, "FacilityName");
        //            lstfacility1.Add(singleemail);

        //            var disfacity = lstfacility1.Distinct();

        //            if (singleemail.FacilityID != 0)
        //            {
        //                SqlCommand cmdGetforuser = new SqlCommand();
        //                BaseSqlManager objBaseSqlManagerforuser = new BaseSqlManager();
        //                cmdGetforuser.CommandType = CommandType.StoredProcedure;
        //                cmdGetforuser.CommandText = "tblLastActiveUser";
        //                cmdGetforuser.Parameters.AddWithValue("@FacilityID", singleemail.FacilityID);
        //                SqlDataReader drforuser = objBaseSqlManagerforuser.ExecuteDataReader(cmdGetforuser);
        //                List<UserList> lstUserperFacility = new List<UserList>();
        //                while (drforuser.Read())
        //                {
        //                    UserList SingleObjtUSer = new UserList();
        //                    SingleObjtUSer.UserName = objBaseSqlManagerforuser.GetTextValue(drforuser, "UserLastName") + " " + objBaseSqlManagerforuser.GetTextValue(drforuser, "UserFirstName");
        //                    string dt = objBaseSqlManagerforuser.GetDateTime(drforuser, "LastActivity").ToString("MM/dd/yy hh:mm tt");
        //                    if (dt == "01/01/01 12:00 AM")
        //                    {
        //                        dt = "";
        //                    }
        //                    else
        //                    {
        //                        dt = " (" + dt + ")";
        //                    }
        //                    SingleObjtUSer.UserName += dt;
        //                    SingleObjtUSer.UserID = objBaseSqlManagerforuser.GetInt64(drforuser, "UserID");
        //                    lstUserperFacility.Add(SingleObjtUSer);
        //                }
        //                if (lstUserperFacility.Count > 0)
        //                {
        //                    singleemail.UserList = lstUserperFacility;
        //                    objemail.Add(singleemail);
        //                }
        //            }
        //        }
        //        //}
        //    }
        //    drs.Close();
        //    objBaseSqlManagers.ForceCloseConnection();
        //    return objemail;
        //}


    }
}
