using vb.Data;
using vb.Data.Model;
using System.Data.Entity;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using vb.Data.ViewModel;
using System.Linq;
using System.Configuration;
using System;

namespace vb.Service
{
    public class CommonService : ICommonService
    {
        public List<UsersViewModel> GetRoleWiseUserList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRoleWiseUserList";
                //  cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UsersViewModel> lstuser = new List<UsersViewModel>();
                while (dr.Read())
                {
                    UsersViewModel objUser = new UsersViewModel();
                    objUser.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    objUser.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    lstuser.Add(objUser);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstuser;
            }
        }

        public List<CustomerListResponse> GetAllCustomerName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> lstCustomer = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    lstCustomer.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCustomer;
            }
        }

        public List<RetCustomerListResponse> GetActiveRetCustomerName(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetActiveRetCustomerName";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> lstCustomer = new List<RetCustomerListResponse>();
                while (dr.Read())
                {
                    RetCustomerListResponse objCustomer = new RetCustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    lstCustomer.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCustomer;
            }
        }

        public List<AreaListResponse> GetAllAreaList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllAreaList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AreaListResponse> objlst = new List<AreaListResponse>();
                while (dr.Read())
                {
                    AreaListResponse objAreaList = new AreaListResponse();
                    objAreaList.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objAreaList.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<CustomerListResponse> GetAllCustomerList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCust = new CustomerListResponse();
                    objCust.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCust.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objlst.Add(objCust);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<VehicleNoListResponse> GetAllVehicleNoList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleNoList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleNoListResponse> objlst = new List<VehicleNoListResponse>();
                while (dr.Read())
                {
                    VehicleNoListResponse objDriverList = new VehicleNoListResponse();
                    objDriverList.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetVehicleNoListResponse> GetAllRetVehicleNoList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetVehicleNoList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetVehicleNoListResponse> objlst = new List<RetVehicleNoListResponse>();
                while (dr.Read())
                {
                    RetVehicleNoListResponse objDriverList = new RetVehicleNoListResponse();
                    objDriverList.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<VehicleNoListResponse> GetVehicleNoList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleNoList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleNoListResponse> objlst = new List<VehicleNoListResponse>();
                while (dr.Read())
                {
                    VehicleNoListResponse objDriverList = new VehicleNoListResponse();
                    objDriverList.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetVehicleNoListResponse> GetRetVehicleNoList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetVehicleNoList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetVehicleNoListResponse> objlst = new List<RetVehicleNoListResponse>();
                while (dr.Read())
                {
                    RetVehicleNoListResponse objDriverList = new RetVehicleNoListResponse();
                    objDriverList.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<DeliveryStatus> GetAllDeliveryPersonList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllDeliveryPersonList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DeliveryStatus> objlst = new List<DeliveryStatus>();
                while (dr.Read())
                {
                    DeliveryStatus objDriver = new DeliveryStatus();
                    objDriver.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "UserID");
                    objDriver.DeliveryPersonName1 = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objDriver.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "UserID");
                    objDriver.DeliveryPersonName2 = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objDriver.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "UserID");
                    objDriver.DeliveryPersonName3 = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objDriver.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "UserID");
                    objDriver.DeliveryPersonName4 = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objlst.Add(objDriver);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetAreaListResponse> GetAllRetAreaList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetAreaList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetAreaListResponse> objlst = new List<RetAreaListResponse>();
                while (dr.Read())
                {
                    RetAreaListResponse objAreaList = new RetAreaListResponse();
                    objAreaList.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objAreaList.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public DynamicMenuModel DynamicMenuMaster_RoleWiseMenuList(int RoleId, string Area)
        {
            DynamicMenuModel objMenu = new DynamicMenuModel();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DynamicMenuMaster_RoleWiseMenuList";
                cmdGet.Parameters.AddWithValue("@RoleId", RoleId);
                cmdGet.Parameters.AddWithValue("@Area", Area);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MainTier> lstMain = new List<MainTier>();
                List<SubTier> lstSub = new List<SubTier>();
                List<ThirdTier> lstThird = new List<ThirdTier>();
                while (dr.Read())
                {
                    if (objBaseSqlManager.GetInt32(dr, "MainTier") == 0)
                    {
                        MainTier objMainMenu = new MainTier();
                        objMainMenu.MainTierID = objBaseSqlManager.GetInt32(dr, "MainTier");
                        objMainMenu.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                        objMainMenu.DisplayName = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                        objMainMenu.Controller = objBaseSqlManager.GetTextValue(dr, "Controller");
                        objMainMenu.Action = objBaseSqlManager.GetTextValue(dr, "Action");
                        objMainMenu.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                        lstMain.Add(objMainMenu);
                    }
                    else if (objBaseSqlManager.GetInt32(dr, "SubTier") == 1)
                    {
                        SubTier objSubTier = new SubTier();
                        objSubTier.MainTierID = objBaseSqlManager.GetInt32(dr, "MainTier");
                        objSubTier.SubTierID = objBaseSqlManager.GetInt32(dr, "SubTier");
                        objSubTier.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                        objSubTier.DisplayName = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                        objSubTier.Controller = objBaseSqlManager.GetTextValue(dr, "Controller");
                        objSubTier.Action = objBaseSqlManager.GetTextValue(dr, "Action");
                        objSubTier.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                        lstSub.Add(objSubTier);

                    }
                    else if (objBaseSqlManager.GetInt32(dr, "SubTier") == 2)
                    {
                        ThirdTier objThirdTier = new ThirdTier();
                        objThirdTier.MainTierID = objBaseSqlManager.GetInt32(dr, "MainTier");
                        objThirdTier.SubTierID = objBaseSqlManager.GetInt32(dr, "SubTier");
                        objThirdTier.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                        objThirdTier.DisplayName = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                        objThirdTier.Controller = objBaseSqlManager.GetTextValue(dr, "Controller");
                        objThirdTier.Action = objBaseSqlManager.GetTextValue(dr, "Action");
                        objThirdTier.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                        lstThird.Add(objThirdTier);
                    }
                }

                foreach (var item in lstSub)
                {
                    List<ThirdTier> lstt = lstThird.Where(i => i.MainTierID == item.MenuID).ToList();
                    item.ThirdTier = lstt;
                }

                foreach (var item in lstMain)
                {
                    List<SubTier> lsts = lstSub.Where(i => i.MainTierID == item.MenuID).ToList();
                    item.SubTier = lsts;
                }
                objMenu.lstMainTier = lstMain;

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objMenu;
            }
        }

        public DynamicMenuModel GetAllDynamicMenuList(int RoleID)
        {
            DynamicMenuModel objMenu = new DynamicMenuModel();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllDynamicMenuList ";
                cmdGet.Parameters.AddWithValue("@RoleId", RoleID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MainTier> lstMain = new List<MainTier>();
                List<SubTier> lstSub = new List<SubTier>();
                List<ThirdTier> lstThird = new List<ThirdTier>();
                while (dr.Read())
                {
                    if (objBaseSqlManager.GetInt32(dr, "MainTier") == 0)
                    {
                        MainTier objMainMenu = new MainTier();
                        objMainMenu.MainTierID = objBaseSqlManager.GetInt32(dr, "MainTier");
                        objMainMenu.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                        objMainMenu.DisplayName = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                        objMainMenu.Controller = objBaseSqlManager.GetTextValue(dr, "Controller");
                        objMainMenu.Action = objBaseSqlManager.GetTextValue(dr, "Action");
                        // objMainMenu.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                        objMainMenu.IsActive = objBaseSqlManager.GetBoolean(dr, "AuthorizedIsActive");
                        lstMain.Add(objMainMenu);
                    }
                    else if (objBaseSqlManager.GetInt32(dr, "SubTier") == 1)
                    {
                        SubTier objSubTier = new SubTier();
                        objSubTier.MainTierID = objBaseSqlManager.GetInt32(dr, "MainTier");
                        objSubTier.SubTierID = objBaseSqlManager.GetInt32(dr, "SubTier");
                        objSubTier.ThirdTierID = objBaseSqlManager.GetInt32(dr, "ThirdTier");
                        objSubTier.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                        objSubTier.DisplayName = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                        objSubTier.Controller = objBaseSqlManager.GetTextValue(dr, "Controller");
                        objSubTier.Action = objBaseSqlManager.GetTextValue(dr, "Action");
                        //  objSubTier.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                        objSubTier.IsActive = objBaseSqlManager.GetBoolean(dr, "AuthorizedIsActive");
                        lstSub.Add(objSubTier);
                    }
                    else if (objBaseSqlManager.GetInt32(dr, "ThirdTier") == 2)
                    {
                        ThirdTier objThirdTier = new ThirdTier();
                        objThirdTier.MainTierID = objBaseSqlManager.GetInt32(dr, "MainTier");
                        objThirdTier.SubTierID = objBaseSqlManager.GetInt32(dr, "SubTier");
                        objThirdTier.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                        objThirdTier.DisplayName = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                        objThirdTier.Controller = objBaseSqlManager.GetTextValue(dr, "Controller");
                        objThirdTier.Action = objBaseSqlManager.GetTextValue(dr, "Action");
                        //  objThirdTier.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                        objThirdTier.IsActive = objBaseSqlManager.GetBoolean(dr, "AuthorizedIsActive");
                        lstThird.Add(objThirdTier);
                    }
                }

                foreach (var item in lstSub)
                {
                    List<ThirdTier> lstt = lstThird.Where(i => i.SubTierID == item.MenuID).ToList();
                    item.ThirdTier = lstt;
                }

                foreach (var item in lstMain)
                {
                    List<SubTier> lsts = lstSub.Where(i => i.MainTierID == item.MenuID).ToList();
                    item.SubTier = lsts;
                }
                objMenu.lstMainTier = lstMain;

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objMenu;
            }
        }

        public void UpdateInvoiceTotal(decimal InvoiceTotal, string InvoiceNo, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateInvoiceTotal";
                cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmdGet.Parameters.AddWithValue("@InvoiceTotal", InvoiceTotal);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        public void UpdateRetInvoiceTotal(decimal InvoiceTotal, string InvoiceNo, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetInvoiceTotal";
                cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmdGet.Parameters.AddWithValue("@InvoiceTotal", InvoiceTotal);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        public List<RetTransportListResponse> GetAllRetTransportName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetTransportName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetTransportListResponse> objlst = new List<RetTransportListResponse>();
                while (dr.Read())
                {
                    RetTransportListResponse objAreaList = new RetTransportListResponse();
                    objAreaList.TransportID = objBaseSqlManager.GetInt64(dr, "TransportID");
                    objAreaList.TransportName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<TransportListResponse> GetAllTransportName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTransportName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TransportListResponse> objlst = new List<TransportListResponse>();
                while (dr.Read())
                {
                    TransportListResponse objAreaList = new TransportListResponse();
                    objAreaList.TransportID = objBaseSqlManager.GetInt64(dr, "TransportID");
                    objAreaList.TransportName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<TaxListResponse> GetAllTaxName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTaxName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TaxListResponse> lstArea = new List<TaxListResponse>();
                while (dr.Read())
                {
                    TaxListResponse objSales = new TaxListResponse();
                    objSales.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objSales.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    lstArea.Add(objSales);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public List<GodownListResponse> GetAllCashOption()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCashOption";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GodownListResponse> lstCustomer = new List<GodownListResponse>();
                while (dr.Read())
                {
                    GodownListResponse objCustomer = new GodownListResponse();
                    objCustomer.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objCustomer.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    lstCustomer.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCustomer;
            }
        }

        public List<RetGodownListResponse> GetAllRetCashOption()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetAllCashOption";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetGodownListResponse> lstCustomer = new List<RetGodownListResponse>();
                while (dr.Read())
                {
                    RetGodownListResponse objCustomer = new RetGodownListResponse();
                    objCustomer.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objCustomer.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    lstCustomer.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCustomer;
            }
        }

        public TransportListResponse GetTransportDetailByTransportID(long TransportID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTransportDetailByTransportID";
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                TransportListResponse obj = new TransportListResponse();
                while (dr.Read())
                {
                    obj.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public RetTransportListResponse GetRetTransportDetailByTransportID(long TransportID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetTransportDetailByTransportID";
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetTransportListResponse obj = new RetTransportListResponse();
                while (dr.Read())
                {
                    obj.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<EmployeeNameResponse> GetAllEmployeeName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEmployeeName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EmployeeNameResponse> lstProduct = new List<EmployeeNameResponse>();
                while (dr.Read())
                {
                    EmployeeNameResponse obj = new EmployeeNameResponse();
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    lstProduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstProduct;
            }
        }

        public List<LicenceListResponse> GetAllLicenceExpireList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllLicenceExpireList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LicenceListResponse> objlst = new List<LicenceListResponse>();
                string path = ConfigurationManager.AppSettings["VehicleDoc"];
                while (dr.Read())
                {
                    LicenceListResponse obj = new LicenceListResponse();
                    obj.LicenceID = objBaseSqlManager.GetInt64(dr, "LicenceID");
                    obj.LicenceType = objBaseSqlManager.GetTextValue(dr, "LicenceType");
                    obj.WhereFrom = objBaseSqlManager.GetTextValue(dr, "WhereFrom");

                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");

                    if (obj.ToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ToDatestr = obj.ToDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ToDatestr = "";
                    }
                    obj.DaysRemaining = objBaseSqlManager.GetInt64(dr, "DaysRemaining");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<VehicleListResponse> GetAllVehicleDocExpireList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleDocExpireList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleListResponse> objlst = new List<VehicleListResponse>();
                string path = ConfigurationManager.AppSettings["VehicleDoc"];
                while (dr.Read())
                {
                    VehicleListResponse obj = new VehicleListResponse();
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");

                    obj.RCNoValidity = objBaseSqlManager.GetDateTime(dr, "RCNoValidity");
                    if (obj.RCNoValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RCNoValiditystr = obj.RCNoValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.RCNoValiditystr = "";
                    }
                    obj.RCNumberRemaining = objBaseSqlManager.GetInt64(dr, "RCNumberRemaining");

                    obj.FitnessValidity = objBaseSqlManager.GetDateTime(dr, "FitnessValidity");
                    if (obj.FitnessValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FitnessValiditystr = obj.FitnessValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.FitnessValiditystr = "";
                    }
                    obj.FitnessRemaining = objBaseSqlManager.GetInt64(dr, "FitnessRemaining");

                    obj.PermitValidity = objBaseSqlManager.GetDateTime(dr, "PermitValidity");
                    if (obj.PermitValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PermitValiditystr = obj.PermitValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.PermitValiditystr = "";
                    }
                    obj.PermitRemaining = objBaseSqlManager.GetInt64(dr, "PermitRemaining");

                    obj.PUCValidity = objBaseSqlManager.GetDateTime(dr, "PUCValidity");
                    if (obj.PUCValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PUCValiditystr = obj.PUCValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.PUCValiditystr = "";
                    }
                    obj.PUCRemaining = objBaseSqlManager.GetInt64(dr, "PUCRemaining");

                    obj.InsuranceValidity = objBaseSqlManager.GetDateTime(dr, "InsuranceValidity");
                    if (obj.InsuranceValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.InsuranceValiditystr = obj.InsuranceValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.InsuranceValiditystr = "";
                    }
                    obj.InsuranceRemaining = objBaseSqlManager.GetInt64(dr, "InsuranceRemaining");

                    obj.AdvertisementValidity = objBaseSqlManager.GetDateTime(dr, "AdvertisementValidity");
                    if (obj.AdvertisementValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.AdvertisementValiditystr = obj.AdvertisementValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.AdvertisementValiditystr = "";
                    }
                    obj.AdvertisementRemaining = objBaseSqlManager.GetInt64(dr, "AdvertisementRemaining");

                    obj.SpeedGovernorCertificateValidity = objBaseSqlManager.GetDateTime(dr, "SpeedGovernorCertificateValidity");
                    if (obj.SpeedGovernorCertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.SpeedGovernorCertificateValiditystr = obj.SpeedGovernorCertificateValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.SpeedGovernorCertificateValiditystr = "";
                    }
                    obj.SpeedGovernorCertiRemaining = objBaseSqlManager.GetInt64(dr, "SpeedGovernorCertiRemaining");

                    obj.FSSAICertificateValidity = objBaseSqlManager.GetDateTime(dr, "FSSAICertificateValidity");
                    if (obj.FSSAICertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FSSAICertificateValiditystr = obj.FSSAICertificateValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.FSSAICertificateValiditystr = "";
                    }
                    obj.FSSAICertiRemaining = objBaseSqlManager.GetInt64(dr, "FSSAICertiRemaining");





                    obj.GreenTaxCertificateValidity = objBaseSqlManager.GetDateTime(dr, "GreenTaxCertificateValidity");
                    if (obj.GreenTaxCertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.GreenTaxCertificateValiditystr = obj.GreenTaxCertificateValidity.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.GreenTaxCertificateValiditystr = "";
                    }
                    obj.GreenTaxCertiRemaining = objBaseSqlManager.GetInt64(dr, "GreenTaxCertiRemaining");




                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<GetAllTempNumber> GetAllTempoNumberList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTempoNumberList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetAllTempNumber> lst = new List<GetAllTempNumber>();
                while (dr.Read())
                {
                    GetAllTempNumber obj = new GetAllTempNumber();
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.TempoNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

        public List<GodownListResponse> GetGodownNameForExpense()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGodownNameForExpense";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GodownListResponse> objlst = new List<GodownListResponse>();
                while (dr.Read())
                {
                    GodownListResponse objAreaList = new GodownListResponse();
                    objAreaList.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objAreaList.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<BankNameListResponse> GetBankNameList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBankNameList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BankNameListResponse> objlst = new List<BankNameListResponse>();
                while (dr.Read())
                {
                    BankNameListResponse obj = new BankNameListResponse();
                    obj.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<GetAllTempNumber> GetAllTempoNumberList2()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTempoNumberList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetAllTempNumber> lst = new List<GetAllTempNumber>();
                while (dr.Read())
                {
                    GetAllTempNumber obj = new GetAllTempNumber();
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

        public List<CustomerListResponse> GetWholesaleFSSAIExpireListByUserID(long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleFSSAIExpireListByUserID";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.DeliveryAreaName = objBaseSqlManager.GetTextValue(dr, "DeliveryArea");
                    objCustomer.BillingAreaName = objBaseSqlManager.GetTextValue(dr, "BillingArea");
                    //objCustomer.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    objCustomer.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objCustomer.FSSAIValidUpTo = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo");
                    if (objCustomer.FSSAIValidUpTo == Convert.ToDateTime("10/10/2014"))
                    {
                        objCustomer.FSSAIValidUpTostr = "";
                    }
                    else
                    {
                        objCustomer.FSSAIValidUpTostr = objCustomer.FSSAIValidUpTo.ToString("dd/MM/yyyy");
                    }
                    objCustomer.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objCustomer.ContactEmail = objBaseSqlManager.GetTextValue(dr, "ContactEmail");
                    objCustomer.DaysRemaining = objBaseSqlManager.GetInt64(dr, "DaysRemaining");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCustomerListResponse> GetRetailFSSAIExpireListByUserID(long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailFSSAIExpireListByUserID";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> objlst = new List<RetCustomerListResponse>();
                while (dr.Read())
                {
                    RetCustomerListResponse objCustomer = new RetCustomerListResponse();
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.DeliveryAreaName = objBaseSqlManager.GetTextValue(dr, "DeliveryArea");
                    objCustomer.BillingAreaName = objBaseSqlManager.GetTextValue(dr, "BillingArea");
                    objCustomer.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    objCustomer.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objCustomer.FSSAIValidUpTo = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo");

                    if (objCustomer.FSSAIValidUpTo == Convert.ToDateTime("10/10/2014"))
                    {
                        objCustomer.FSSAIValidUpTostr = "";
                    }
                    else
                    {
                        objCustomer.FSSAIValidUpTostr = objCustomer.FSSAIValidUpTo.ToString("dd/MM/yyyy");
                    }
                    objCustomer.DaysRemaining = objBaseSqlManager.GetInt64(dr, "DaysRemaining");
                    objCustomer.FSSAI2 = objBaseSqlManager.GetTextValue(dr, "FSSAI2");
                    objCustomer.FSSAIValidUpTo2 = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo2");

                    if (objCustomer.FSSAIValidUpTo2 == Convert.ToDateTime("10/10/2014"))
                    {
                        objCustomer.FSSAIValidUpTostr2 = "";
                    }
                    else
                    {
                        objCustomer.FSSAIValidUpTostr2 = objCustomer.FSSAIValidUpTo2.ToString("dd/MM/yyyy");
                    }
                    objCustomer.DaysRemaining2 = objBaseSqlManager.GetInt64(dr, "DaysRemaining2");
                    objCustomer.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objCustomer.ContactEmail = objBaseSqlManager.GetTextValue(dr, "ContactEmail");
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 03-04-2020 Display Pouch Qty Stock in Dashboard
        public List<MinPouchQuantityListResponse> GetStockPouchListForDashboard(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetStockPouchListForDashboard";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MinPouchQuantityListResponse> objlst = new List<MinPouchQuantityListResponse>();
                while (dr.Read())
                {
                    MinPouchQuantityListResponse obj = new MinPouchQuantityListResponse();
                    obj.PouchID = objBaseSqlManager.GetInt64(dr, "PouchID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.PouchDescription = objBaseSqlManager.GetTextValue(dr, "PouchDescription");
                    obj.MinPouchQuantity = objBaseSqlManager.GetInt64(dr, "MinPouchQuantity");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningPouch = objBaseSqlManager.GetInt64(dr, "OpeningPouch");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        
        // 17 June 2022 Display Pouch Qty Stock in Dashboard
        public List<MinPouchQuantityListResponse> GetStockPouchListForDashboardForAdmin(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetStockPouchListForDashboardForAdmin";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MinPouchQuantityListResponse> objlst = new List<MinPouchQuantityListResponse>();
                while (dr.Read())
                {
                    MinPouchQuantityListResponse obj = new MinPouchQuantityListResponse();
                    obj.PouchID = objBaseSqlManager.GetInt64(dr, "PouchID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.PouchDescription = objBaseSqlManager.GetTextValue(dr, "PouchDescription");
                    obj.MinPouchQuantity = objBaseSqlManager.GetInt64(dr, "MinPouchQuantity");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningPouch = objBaseSqlManager.GetInt64(dr, "OpeningPouch");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 07 Oct 2020 Piyush Limbani
        public List<UtilityNameList> GetAllUtilityName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUtilityName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityNameList> objlst = new List<UtilityNameList>();
                while (dr.Read())
                {
                    UtilityNameList obj = new UtilityNameList();
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 07 Oct 2020 Piyush Limbani
        public List<PouchNameList> GetAllPouchName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPouchName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchNameList> objlst = new List<PouchNameList>();
                while (dr.Read())
                {
                    PouchNameList obj = new PouchNameList();
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 13 Oct 2020 Piyush Limbani
        public List<MinUtilityQuantityListResponse> GetStockUtilityListForDashboard(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetStockUtilityListForDashboard";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MinUtilityQuantityListResponse> objlst = new List<MinUtilityQuantityListResponse>();
                while (dr.Read())
                {
                    MinUtilityQuantityListResponse obj = new MinUtilityQuantityListResponse();
                    obj.UtilityID = objBaseSqlManager.GetInt64(dr, "UtilityID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.UtilityDescription = objBaseSqlManager.GetTextValue(dr, "UtilityDescription");
                    obj.MinUtilityQuantity = objBaseSqlManager.GetInt64(dr, "MinUtilityQuantity");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningUtility = objBaseSqlManager.GetInt64(dr, "OpeningUtility");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

       
        // 17 June 2022 Display Utility Qty Stock in Dashboard
        public List<MinUtilityQuantityListResponse> GetStockUtilityListForDashboardForAdmin(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetStockUtilityListForDashboardForAdmin";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MinUtilityQuantityListResponse> objlst = new List<MinUtilityQuantityListResponse>();
                while (dr.Read())
                {
                    MinUtilityQuantityListResponse obj = new MinUtilityQuantityListResponse();
                    obj.UtilityID = objBaseSqlManager.GetInt64(dr, "UtilityID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.UtilityDescription = objBaseSqlManager.GetTextValue(dr, "UtilityDescription");
                    obj.MinUtilityQuantity = objBaseSqlManager.GetInt64(dr, "MinUtilityQuantity");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningUtility = objBaseSqlManager.GetInt64(dr, "OpeningUtility");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 27 Aug 2020 Piyush Limbani
        //public List<GetAllTempNumber> GetAllTempoNumberListCurrentDateWise()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetAllTempoNumberListCurrentDateWise";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<GetAllTempNumber> lst = new List<GetAllTempNumber>();
        //    while (dr.Read())
        //    {
        //        GetAllTempNumber obj = new GetAllTempNumber();
        //        obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
        //        obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
        //        lst.Add(obj);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return lst;
        //}

        //public List<GetAllTempNumber> GetAllTempoNumberListForInward()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetAllTempoNumberListForInward";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<GetAllTempNumber> lst = new List<GetAllTempNumber>();
        //    while (dr.Read())
        //    {
        //        GetAllTempNumber obj = new GetAllTempNumber();
        //        obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
        //        obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
        //        lst.Add(obj);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return lst;
        //}

    }
}
