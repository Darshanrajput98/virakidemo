

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
    using System;
    using System.Configuration;

    public class CustomerServices : ICustomerService
    {
        public List<AreaListResponse> GetAllAreaName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllAreaName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AreaListResponse> lstArea = new List<AreaListResponse>();
                while (dr.Read())
                {
                    AreaListResponse objArea = new AreaListResponse();
                    objArea.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objArea.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objArea.DaysofWeekstr = new Utility().GetTextEnum(objArea.DaysofWeek);
                    objArea.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objArea.AreaName = objArea.AreaName + " " + "(" + objArea.DaysofWeekstr + ")";
                    lstArea.Add(objArea);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public List<CustomerGroupListResponse> GetAllCustomerGroupName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerGroupName";

                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerGroupListResponse> lstArea = new List<CustomerGroupListResponse>();
                while (dr.Read())
                {
                    CustomerGroupListResponse objArea = new CustomerGroupListResponse();
                    objArea.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objArea.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    // objProductresponse.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    lstArea.Add(objArea);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public List<UserListResponse> GetAllSalesPersonName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllSalesPersonName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UserListResponse> lstArea = new List<UserListResponse>();
                while (dr.Read())
                {
                    UserListResponse objSales = new UserListResponse();
                    objSales.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    objSales.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    lstArea.Add(objSales);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
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

        public bool AddCustomerGroup(CustomerGroup_Mst ObjCustomerGroup)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjCustomerGroup.CustomerGroupID == 0)
                {
                    context.CustomerGroup_Mst.Add(ObjCustomerGroup);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjCustomerGroup).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjCustomerGroup.CustomerGroupID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<CustomerGroupListResponse> GetAllCustomerGroupList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerGroupList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerGroupListResponse> objlst = new List<CustomerGroupListResponse>();
                while (dr.Read())
                {
                    CustomerGroupListResponse objCustomerGroup = new CustomerGroupListResponse();
                    objCustomerGroup.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objCustomerGroup.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomerGroup.CustomerGroupAddress1 = objBaseSqlManager.GetTextValue(dr, "CustomerGroupAddress1");
                    objCustomerGroup.CustomerGroupAddress2 = objBaseSqlManager.GetTextValue(dr, "CustomerGroupAddress2");
                    objCustomerGroup.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objCustomerGroup.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomerGroup.CustomerGroupDescription = objBaseSqlManager.GetTextValue(dr, "CustomerGroupDescription");
                    objCustomerGroup.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objCustomerGroup);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteCustomerGroup(long CustomerGroupID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteCustomerGroup";
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddCustomer(CustomerViewModel data)
        {
            Customer_Mst obj = new Customer_Mst();
            obj.CustomerID = data.CustomerID;
            obj.CustomerNumber = data.CustomerNumber;
            obj.CustomerName = data.CustomerName;
            obj.CustomerGroupID = data.CustomerGroupID;
            obj.AreaID = data.AreaID;
            obj.TaxNo = data.TaxNo;
            obj.UserID = data.UserID;
            obj.TaxID = data.TaxID;
            obj.ClosingTime = data.ClosingTime;
            obj.OpeningTime = data.OpeningTime;
            obj.NoofInvoice = data.NoofInvoice;
            obj.FSSAI = data.FSSAI;
            obj.FSSAIValidUpTo = data.FSSAIValidUpTo;
            obj.CustomerTypeID = data.CustomerTypeID;
            obj.LBTNo = data.LBTNo;
            obj.CallWeek1 = data.CallWeek1;
            obj.CallWeek2 = data.CallWeek2;
            obj.CallWeek3 = data.CallWeek3;
            obj.CallWeek4 = data.CallWeek4;
            obj.DoNotDisturb = data.DoNotDisturb;
            obj.CustomerNote = data.CustomerNote;
            obj.BankName = data.BankName;
            obj.Branch = data.Branch;
            obj.IFCCode = data.IFCCode;
            obj.DeliveryAreaID = data.DeliveryAreaID;
            obj.DeliveryAddressLine1 = data.DeliveryAddressLine1;
            obj.DeliveryAddressLine2 = data.DeliveryAddressLine2;
            obj.DeliveryAddressPincode = data.DeliveryAddressPincode;
            obj.DeliveryAddressDistance = data.DeliveryAddressDistance;
            obj.BillingAreaID = data.BillingAreaID;
            obj.BillingAddressLine1 = data.BillingAddressLine1;
            obj.BillingAddressLine2 = data.BillingAddressLine2;
            obj.FSSAICertificate = data.FSSAICertificate;
            obj.IsVirakiEmployee = data.IsVirakiEmployee;
            obj.IsReflectInVoucher = data.IsReflectInVoucher;
            obj.CellNo1 = data.CellNo1;
            obj.CellNo2 = data.CellNo2;
            obj.TelNo1 = data.TelNo1;
            obj.TelNo2 = data.TelNo2;
            obj.Email1 = data.Email1;
            obj.Email2 = data.Email2;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = false;

            obj.IsTCSApplicable = data.IsTCSApplicable;
            obj.PanCard = data.PanCard;

            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.CustomerID == 0)
                {
                    context.Customer_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();

                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateEInvoiceErrorByCustomerID";
                        cmdGet.Parameters.AddWithValue("@CustomerID", obj.CustomerID);
                        object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                if (obj.CustomerID > 0)
                {
                    foreach (var item in data.lstAddress)
                    {
                        CustomerAddress objProductQty = new CustomerAddress();
                        objProductQty.CustomerID = obj.CustomerID;
                        objProductQty.AddressID = item.AddressID;
                        objProductQty.CustomerAddressID = item.CustomerAddressID;
                        objProductQty.Name = item.Name;
                        objProductQty.RoleDescription = item.RoleDescription;
                        objProductQty.CellNo = item.CellNo;
                        objProductQty.TelNo = item.TelNo;
                        objProductQty.Email = item.Email;
                        objProductQty.Note = item.Note;
                        objProductQty.IsDelete = false;
                        objProductQty.CreatedBy = data.CreatedBy;
                        objProductQty.CreatedOn = data.CreatedOn;
                        objProductQty.UpdatedBy = data.UpdatedBy;
                        objProductQty.UpdatedOn = data.UpdatedOn;

                        if (objProductQty.CustomerAddressID == 0)
                        {
                            context.CustomerAddresses.Add(objProductQty);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objProductQty).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
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
                string path = ConfigurationManager.AppSettings["CustomerDocument"];
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objCustomer.Country = objBaseSqlManager.GetTextValue(dr, "Country");
                    objCustomer.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objCustomer.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomer.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objCustomer.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    objCustomer.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objCustomer.OpeningTime = objBaseSqlManager.GetDateTime(dr, "OpeningTime");
                    objCustomer.ClosingTime = objBaseSqlManager.GetDateTime(dr, "ClosingTime");
                    objCustomer.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objCustomer.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objCustomer.CustomerTypeID = objBaseSqlManager.GetInt32(dr, "CustomerTypeID");
                    objCustomer.CustomerTypeName = new Utility2().GetTextEnum(objCustomer.CustomerTypeID);
                    objCustomer.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    objCustomer.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objCustomer.FSSAIValidUpTo = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo");
                    if (objCustomer.FSSAIValidUpTo != Convert.ToDateTime("10/10/2014"))
                    {
                        objCustomer.FSSAIValidUpTostr = objCustomer.FSSAIValidUpTo.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objCustomer.FSSAIValidUpTostr = "";
                    }
                    objCustomer.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objCustomer.CallWeek1 = objBaseSqlManager.GetBoolean(dr, "CallWeek1");
                    objCustomer.CallWeek2 = objBaseSqlManager.GetBoolean(dr, "CallWeek2");
                    objCustomer.CallWeek3 = objBaseSqlManager.GetBoolean(dr, "CallWeek3");
                    objCustomer.CallWeek4 = objBaseSqlManager.GetBoolean(dr, "CallWeek4");
                    objCustomer.DoNotDisturb = objBaseSqlManager.GetBoolean(dr, "DoNotDisturb");
                    objCustomer.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objCustomer.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objCustomer.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    objCustomer.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFCCode");
                    objCustomer.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCustomer.DeliveryAreaID = objBaseSqlManager.GetInt64(dr, "DeliveryAreaID");
                    objCustomer.DeliveryAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    objCustomer.DeliveryAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    objCustomer.DeliveryAddressPincode = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressPincode");
                    objCustomer.DeliveryAddressDistance = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressDistance");
                    objCustomer.BillingAreaID = objBaseSqlManager.GetInt64(dr, "BillingAreaID");
                    objCustomer.BillingAddressLine1 = objBaseSqlManager.GetTextValue(dr, "BillingAddressLine1");
                    objCustomer.BillingAddressLine2 = objBaseSqlManager.GetTextValue(dr, "BillingAddressLine2");
                    objCustomer.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objCustomer.ContactPerson = objBaseSqlManager.GetTextValue(dr, "ContactPerson");

                    objCustomer.FSSAICertificate = objBaseSqlManager.GetTextValue(dr, "FSSAICertificate");
                    if (objCustomer.FSSAICertificate != "")
                    {
                        objCustomer.FSSAICertificatepath = path + objCustomer.FSSAICertificate;
                    }
                    else
                    {
                        objCustomer.FSSAICertificatepath = "";
                    }
                    objCustomer.IsVirakiEmployee = objBaseSqlManager.GetBoolean(dr, "IsVirakiEmployee");
                    objCustomer.IsReflectInVoucher = objBaseSqlManager.GetBoolean(dr, "IsReflectInVoucher");
                    objCustomer.CellNo1 = objBaseSqlManager.GetTextValue(dr, "CellNo1");
                    objCustomer.CellNo2 = objBaseSqlManager.GetTextValue(dr, "CellNo2");
                    objCustomer.TelNo1 = objBaseSqlManager.GetTextValue(dr, "TelNo1");
                    objCustomer.TelNo2 = objBaseSqlManager.GetTextValue(dr, "TelNo2");
                    objCustomer.Email1 = objBaseSqlManager.GetTextValue(dr, "Email1");
                    objCustomer.Email2 = objBaseSqlManager.GetTextValue(dr, "Email2");
                    objCustomer.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objCustomer.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objCustomer.IsTCSApplicable = objBaseSqlManager.GetBoolean(dr, "IsTCSApplicable");
                    objCustomer.PanCard = objBaseSqlManager.GetTextValue(dr, "PanCard");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteCustomer(long CustomerID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteCustomer";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<CustomerAddressViewModel> GetCustomerAddressListByCustomerID(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCustomerAddressListByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerAddressViewModel> objlst = new List<CustomerAddressViewModel>();
                while (dr.Read())
                {
                    CustomerAddressViewModel objCategory = new CustomerAddressViewModel();
                    objCategory.CustomerAddressID = objBaseSqlManager.GetInt64(dr, "CustomerAddressID");
                    objCategory.AddressID = objBaseSqlManager.GetInt64(dr, "AddressID");
                    objCategory.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    objCategory.RoleDescription = objBaseSqlManager.GetTextValue(dr, "RoleDescription");
                    objCategory.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo");
                    objCategory.TelNo = objBaseSqlManager.GetTextValue(dr, "TelNo");
                    objCategory.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objCategory.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<CustomerListResponse> GetAllCustomerCallList(CustomerListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerCallList";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", model.DaysofWeek);
                cmdGet.Parameters.AddWithValue("@CallWeek1", model.CallWeek1);
                cmdGet.Parameters.AddWithValue("@CallWeek2", model.CallWeek2);
                cmdGet.Parameters.AddWithValue("@CallWeek3", model.CallWeek3);
                cmdGet.Parameters.AddWithValue("@CallWeek4", model.CallWeek4);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCallList = new CustomerListResponse();
                    objCallList.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCallList.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCallList.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCallList.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objCallList.DaysofWeekstr = new Utility().GetTextEnum(objCallList.DaysofWeek);
                    objCallList.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objCallList.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objCallList.CallWeek1 = objBaseSqlManager.GetBoolean(dr, "CallWeek1");
                    objCallList.CallWeek2 = objBaseSqlManager.GetBoolean(dr, "CallWeek2");
                    objCallList.CallWeek3 = objBaseSqlManager.GetBoolean(dr, "CallWeek3");
                    objCallList.CallWeek4 = objBaseSqlManager.GetBoolean(dr, "CallWeek4");
                    objCallList.DoNotDisturb = objBaseSqlManager.GetBoolean(dr, "DoNotDisturb");
                    if (objCallList.DoNotDisturb == true)
                    {
                        objCallList.DoNotDisturbstr = "Do Not Disturb";
                    }
                    objlst.Add(objCallList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public CustomerListResponse GetExistCustomerDetials(string CustomerName, long AreaID)
        {
            CustomerListResponse obj = new CustomerListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExistCustomerDetials";
                cmdGet.Parameters.AddWithValue("@CustomerName", CustomerName);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public CustomerListResponse GetLastCustomerNumber()
        {
            CustomerListResponse obj = new CustomerListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastCustomerNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return obj;
        }

        public string GetCustomerNameByCustomerID(long CustomerID)
        {
            string CustomerName = "";
            CustomerListResponse obj = new CustomerListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCustomerNameByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return CustomerName;
        }

        public List<CustomerListResponse> GetCustomerCallListForExcel(long AreaID, long UserID, long DaysofWeek, bool CallWeek1, bool CallWeek2, bool CallWeek3, bool CallWeek4)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerCallList";
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                cmdGet.Parameters.AddWithValue("@CallWeek1", CallWeek1);
                cmdGet.Parameters.AddWithValue("@CallWeek2", CallWeek2);
                cmdGet.Parameters.AddWithValue("@CallWeek3", CallWeek3);
                cmdGet.Parameters.AddWithValue("@CallWeek4", CallWeek4);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse obj = new CustomerListResponse();
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    obj.DaysofWeekstr = new Utility().GetTextEnum(obj.DaysofWeek);
                    obj.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.CallWeek1 = objBaseSqlManager.GetBoolean(dr, "CallWeek1");
                    obj.CallWeek2 = objBaseSqlManager.GetBoolean(dr, "CallWeek2");
                    obj.CallWeek3 = objBaseSqlManager.GetBoolean(dr, "CallWeek3");
                    obj.CallWeek4 = objBaseSqlManager.GetBoolean(dr, "CallWeek4");
                    obj.DoNotDisturb = objBaseSqlManager.GetBoolean(dr, "DoNotDisturb");
                    if (obj.CallWeek1 == true)
                    {
                        obj.CallWeek1str = "Yes";
                    }
                    else
                    {
                        obj.CallWeek1str = "";
                    }

                    if (obj.CallWeek2 == true)
                    {
                        obj.CallWeek2str = "Yes";
                    }
                    else
                    {
                        obj.CallWeek2str = "";
                    }

                    if (obj.CallWeek3 == true)
                    {
                        obj.CallWeek3str = "Yes";
                    }
                    else
                    {
                        obj.CallWeek3str = "";
                    }

                    if (obj.CallWeek4 == true)
                    {
                        obj.CallWeek4str = "Yes";
                    }
                    else
                    {
                        obj.CallWeek4str = "";
                    }

                    if (obj.DoNotDisturb == true)
                    {
                        obj.DoNotDisturbstr = "Do Not Disturb";
                    }
                    else
                    {
                        obj.DoNotDisturbstr = "";
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<CustomerListResponse> GetAllCustomerListForExel()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerListForExel";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
                    objCustomer.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objCustomer.DaysofWeekstr = new Utility().GetTextEnum(objCustomer.DaysofWeek);
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objCustomer.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomer.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objCustomer.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
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
                    objCustomer.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objCustomer.DeliveryAreaName = objBaseSqlManager.GetTextValue(dr, "DeliveryAreaName");
                    objCustomer.DeliveryAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    objCustomer.DeliveryAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    objCustomer.DeliveryAddressPincode = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressPincode");
                    objCustomer.BillingAreaName = objBaseSqlManager.GetTextValue(dr, "BillingAreaName");
                    objCustomer.BillingAddressLine1 = objBaseSqlManager.GetTextValue(dr, "BillingAddressLine1");
                    objCustomer.BillingAddressLine2 = objBaseSqlManager.GetTextValue(dr, "BillingAddressLine2");
                    objCustomer.ContactName = objBaseSqlManager.GetTextValue(dr, "ContactName");
                    objCustomer.ContactEmail = objBaseSqlManager.GetTextValue(dr, "ContactEmail");
                    objCustomer.ContactMobNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objCustomer.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objCustomer.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    objCustomer.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFCCode");
                    objCustomer.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");

                    objCustomer.CellNo1 = objBaseSqlManager.GetTextValue(dr, "CellNo1");
                    objCustomer.CellNo2 = objBaseSqlManager.GetTextValue(dr, "CellNo2");
                    objCustomer.TelNo1 = objBaseSqlManager.GetTextValue(dr, "TelNo1");
                    objCustomer.TelNo2 = objBaseSqlManager.GetTextValue(dr, "TelNo2");
                    objCustomer.Email1 = objBaseSqlManager.GetTextValue(dr, "Email1");
                    objCustomer.Email2 = objBaseSqlManager.GetTextValue(dr, "Email2");

                    //objCustomer.State = objBaseSqlManager.GetTextValue(dr, "State");
                    //objCustomer.Country = objBaseSqlManager.GetTextValue(dr, "Country");  
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<CustomerListResponse> GetAllCustomerFSSAIExpireList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerFSSAIExpireList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
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

        public bool UpdateFSSAIDate(long CustomerID, DateTime FSSAIValidUpTo)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateFSSAIDate";
                    cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmdGet.Parameters.AddWithValue("@FSSAIValidUpTo", FSSAIValidUpTo);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }



        public string GetOldFSSAICertificateByCustomerID(long CustomerID)
        {
            Customer_Mst FSSAICertificate = new Customer_Mst();
            string FSSAICerti = string.Empty;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    FSSAICertificate = context.Customer_Mst.Where(i => i.CustomerID == CustomerID).FirstOrDefault();
                }
                if (FSSAICertificate != null)
                {
                    FSSAICerti = FSSAICertificate.FSSAICertificate;
                }
            }
            catch (Exception ex) { }
            return FSSAICerti;
        }


    }
}
