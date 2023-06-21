namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data;
    using vb.Data.Model;
    using vb.Data.ViewModel;
    using vb.Service.Common;

    public class RetCustomerServices : IRetCustomerService
    {
        public List<RetAreaListResponse> GetAllAreaName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetAreaName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetAreaListResponse> lstArea = new List<RetAreaListResponse>();
                while (dr.Read())
                {
                    RetAreaListResponse objArea = new RetAreaListResponse();
                    objArea.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objArea.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    lstArea.Add(objArea);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public List<RetCustomerGroupListResponse> GetAllCustomerGroupName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerGroupName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerGroupListResponse> lstArea = new List<RetCustomerGroupListResponse>();
                while (dr.Read())
                {
                    RetCustomerGroupListResponse objArea = new RetCustomerGroupListResponse();
                    objArea.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objArea.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    lstArea.Add(objArea);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public List<RetUserListResponse> GetAllSalesPersonName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetSalesPersonName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetUserListResponse> lstArea = new List<RetUserListResponse>();
                while (dr.Read())
                {
                    RetUserListResponse objSales = new RetUserListResponse();
                    objSales.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    objSales.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    lstArea.Add(objSales);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public List<RetTaxListResponse> GetAllTaxName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetTaxName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetTaxListResponse> lstArea = new List<RetTaxListResponse>();
                while (dr.Read())
                {
                    RetTaxListResponse objSales = new RetTaxListResponse();
                    objSales.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objSales.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    lstArea.Add(objSales);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public bool AddCustomerGroup(RetCustomerGroupMst ObjCustomerGroup)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjCustomerGroup.CustomerGroupID == 0)
                {
                    context.RetCustomerGroupMsts.Add(ObjCustomerGroup);
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

        public List<RetCustomerGroupListResponse> GetAllCustomerGroupList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerGroupList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerGroupListResponse> objlst = new List<RetCustomerGroupListResponse>();
                while (dr.Read())
                {
                    RetCustomerGroupListResponse objCustomerGroup = new RetCustomerGroupListResponse();
                    objCustomerGroup.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objCustomerGroup.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomerGroup.CustomerGroupAddress1 = objBaseSqlManager.GetTextValue(dr, "CustomerGroupAddress1");
                    objCustomerGroup.CustomerGroupAddress2 = objBaseSqlManager.GetTextValue(dr, "CustomerGroupAddress2");
                    objCustomerGroup.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objCustomerGroup.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomerGroup.CustomerGroupDescription = objBaseSqlManager.GetTextValue(dr, "CustomerGroupDescription");
                    objCustomerGroup.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");

                    objCustomerGroup.IsShow = objBaseSqlManager.GetBoolean(dr, "IsShow");

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
                cmdGet.CommandText = "DeleteRetCustomerGroup";
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddCustomer(RetCustomerViewModel data)
        {
            try
            {
                RetCustomerMst obj = new RetCustomerMst();
                obj.CustomerID = data.CustomerID;
                obj.CustomerNumber = data.CustomerNumber;
                obj.CustomerName = data.CustomerName;
                obj.CustomerGroupID = data.CustomerGroupID;
                obj.AreaID = data.AreaID;
                obj.UserID = data.UserID;
                obj.TaxID = data.TaxID;
                obj.ClosingTime = data.ClosingTime;
                obj.OpeningTime = data.OpeningTime;
                obj.NoofInvoice = data.NoofInvoice;
                obj.CustomerTypeID = data.CustomerTypeID;
                obj.LBTNo = data.LBTNo;
                obj.CallWeek1 = data.CallWeek1;
                obj.CallWeek2 = data.CallWeek2;
                obj.CallWeek3 = data.CallWeek3;
                obj.CallWeek4 = data.CallWeek4;
                obj.CustomerNote = data.CustomerNote;
                obj.BankName = data.BankName;
                obj.Branch = data.Branch;
                obj.IFCCode = data.IFCCode;
                obj.DeliveryAreaID = data.DeliveryAreaID;
                obj.DeliveryAddressLine1 = data.DeliveryAddressLine1;
                obj.DeliveryAddressLine2 = data.DeliveryAddressLine2;
                obj.DeliveryAddressPincode = data.DeliveryAddressPincode;
                obj.DeliveryAddressDistance = data.DeliveryAddressDistance;
                obj.TaxNo = data.TaxNo;
                obj.FSSAI = data.FSSAI;
                obj.BillingAreaID = data.BillingAreaID;
                obj.BillingAddressLine1 = data.BillingAddressLine1;
                obj.BillingAddressLine2 = data.BillingAddressLine2;
                obj.TaxNo2 = data.TaxNo2;
                obj.FSSAI2 = data.FSSAI2;
                obj.FSSAIValidUpTo = data.FSSAIValidUpTo;
                obj.FSSAIValidUpTo2 = data.FSSAIValidUpTo2;
                obj.CountryID = data.CountryID;
                obj.IsReflectInVoucher = data.IsReflectInVoucher;
                obj.FSSAICertificate = data.FSSAICertificate;
                obj.FSSAICertificate2 = data.FSSAICertificate2;
                obj.CellNo1 = data.CellNo1;
                obj.CellNo2 = data.CellNo2;
                obj.TelNo1 = data.TelNo1;
                obj.TelNo2 = data.TelNo2;
                obj.Email1 = data.Email1;
                obj.Email2 = data.Email2;

                obj.IsTCSApplicable = data.IsTCSApplicable;
                obj.PanCard = data.PanCard;

                obj.CreatedBy = data.CreatedBy;
                obj.CreatedOn = data.CreatedOn;
                obj.UpdatedBy = data.UpdatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                obj.IsDelete = false;
                using (VirakiEntities context = new VirakiEntities())
                {
                    if (obj.CustomerID == 0)
                    {
                        context.RetCustomerMsts.Add(obj);
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
                            cmdGet.CommandText = "UpdateRetEInvoiceErrorByCustomerID";
                            cmdGet.Parameters.AddWithValue("@CustomerID", obj.CustomerID);
                            object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                    }
                    if (obj.CustomerID > 0)
                    {
                        if (data.lstAddress != null)
                        {
                            foreach (var item in data.lstAddress)
                            {
                                RetCustomerAddress objProductQty = new RetCustomerAddress();
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
                                    context.RetCustomerAddresses.Add(objProductQty);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    context.Entry(objProductQty).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
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
            catch (Exception)
            {
                throw;
            }
        }

        public List<RetCustomerListResponse> GetAllCustomerList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> objlst = new List<RetCustomerListResponse>();
                string path = ConfigurationManager.AppSettings["CustomerDocument"];
                while (dr.Read())
                {
                    RetCustomerListResponse objCustomer = new RetCustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objCustomer.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomer.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objCustomer.TaxNo2 = objBaseSqlManager.GetTextValue(dr, "TaxNo2");
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
                    objCustomer.FSSAI2 = objBaseSqlManager.GetTextValue(dr, "FSSAI2");
                    objCustomer.FSSAIValidUpTo = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo");
                    if (objCustomer.FSSAIValidUpTo != Convert.ToDateTime("10/10/2014"))
                    {
                        objCustomer.FSSAIValidUpTostr = objCustomer.FSSAIValidUpTo.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objCustomer.FSSAIValidUpTostr = "";
                    }

                    objCustomer.FSSAIValidUpTo2 = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo2");
                    if (objCustomer.FSSAIValidUpTo2 != Convert.ToDateTime("10/10/2014"))
                    {
                        objCustomer.FSSAIValidUpTostr2 = objCustomer.FSSAIValidUpTo2.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objCustomer.FSSAIValidUpTostr2 = "";
                    }
                    objCustomer.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objCustomer.CallWeek1 = objBaseSqlManager.GetBoolean(dr, "CallWeek1");
                    objCustomer.CallWeek2 = objBaseSqlManager.GetBoolean(dr, "CallWeek2");
                    objCustomer.CallWeek3 = objBaseSqlManager.GetBoolean(dr, "CallWeek3");
                    objCustomer.CallWeek4 = objBaseSqlManager.GetBoolean(dr, "CallWeek4");
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
                    objCustomer.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
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
                    objCustomer.FSSAICertificate2 = objBaseSqlManager.GetTextValue(dr, "FSSAICertificate2");
                    if (objCustomer.FSSAICertificate2 != "")
                    {
                        objCustomer.FSSAICertificatepath2 = path + objCustomer.FSSAICertificate2;
                    }
                    else
                    {
                        objCustomer.FSSAICertificatepath2 = "";
                    }
                    objCustomer.IsReflectInVoucher = objBaseSqlManager.GetBoolean(dr, "IsReflectInVoucher");
                    objCustomer.CellNo1 = objBaseSqlManager.GetTextValue(dr, "CellNo1");
                    objCustomer.CellNo2 = objBaseSqlManager.GetTextValue(dr, "CellNo2");
                    objCustomer.TelNo1 = objBaseSqlManager.GetTextValue(dr, "TelNo1");
                    objCustomer.TelNo2 = objBaseSqlManager.GetTextValue(dr, "TelNo2");
                    objCustomer.Email1 = objBaseSqlManager.GetTextValue(dr, "Email1");
                    objCustomer.Email2 = objBaseSqlManager.GetTextValue(dr, "Email2");
                    objCustomer.IsTCSApplicable = objBaseSqlManager.GetBoolean(dr, "IsTCSApplicable");
                    objCustomer.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objCustomer.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
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
                cmdGet.CommandText = "DeleteRetCustomer";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<RetCustomerAddressViewModel> GetCustomerAddressListByCustomerID(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustomerAddressListByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerAddressViewModel> objlst = new List<RetCustomerAddressViewModel>();
                while (dr.Read())
                {
                    RetCustomerAddressViewModel objCategory = new RetCustomerAddressViewModel();
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

        public List<RetCustomerListResponse> GetAllCustomerCallList(RetCustomerListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerCallList";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", model.DaysofWeek);
                cmdGet.Parameters.AddWithValue("@CallWeek1", model.CallWeek1);
                cmdGet.Parameters.AddWithValue("@CallWeek2", model.CallWeek2);
                cmdGet.Parameters.AddWithValue("@CallWeek3", model.CallWeek3);
                cmdGet.Parameters.AddWithValue("@CallWeek4", model.CallWeek4);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> objlst = new List<RetCustomerListResponse>();
                while (dr.Read())
                {
                    RetCustomerListResponse objCallList = new RetCustomerListResponse();
                    objCallList.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCallList.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCallList.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCallList.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objCallList.DaysofWeekstr = new Utility().GetTextEnum(objCallList.DaysofWeek);
                    objCallList.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objCallList.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objlst.Add(objCallList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public RetCustomerListResponse GetLastCustomerNumber()
        {
            RetCustomerListResponse obj = new RetCustomerListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastRetCustomerNumber";
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

        public RetCustomerDiscountListResponse GetRetailDiscountForCustomer(long CustomerID)
        {
            RetCustomerDiscountListResponse objlst = new RetCustomerDiscountListResponse();
            objlst.lstProductDiscount = GetAllProductListByCustomerID(CustomerID);
            objlst.lstCategoryDiscount = GetAllProductCategoryListByCustomerID(CustomerID);
            if (objlst.lstCategoryDiscount.Count > 0)
            {
                var lst = objlst.lstCategoryDiscount.ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    if (!string.IsNullOrEmpty(lst[i].Contract))
                    {
                        objlst.Contract = lst[i].Contract;
                        objlst.ValidFrom = lst[i].ValidFrom;
                        objlst.ValidTo = lst[i].ValidTo;
                        break;
                    }
                }
            }
            else if (objlst.lstProductDiscount.Count > 0)
            {
                var lst = objlst.lstProductDiscount.ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    if (!string.IsNullOrEmpty(lst[i].Contract))
                    {
                        objlst.Contract = lst[i].Contract;
                        objlst.ValidFrom = lst[i].ValidFrom;
                        objlst.ValidTo = lst[i].ValidTo;
                        break;
                    }
                }
            }
            return objlst;
        }

        private List<ProductDiscount> GetAllProductListByCustomerID(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProductListByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductDiscount> objlst = new List<ProductDiscount>();
                while (dr.Read())
                {
                    ProductDiscount objProduct = new ProductDiscount();
                    objProduct.ProductDiscountID = objBaseSqlManager.GetInt64(dr, "ProductDiscountID");
                    objProduct.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objProduct.Contract = objBaseSqlManager.GetTextValue(dr, "Contract");
                    objProduct.ValidFrom = objBaseSqlManager.GetDateTime(dr, "ValidFrom");
                    objProduct.ValidTo = objBaseSqlManager.GetDateTime(dr, "ValidTo");
                    objProduct.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objProduct.Discount = objBaseSqlManager.GetDecimal(dr, "Discount");
                    objProduct.Margin = objBaseSqlManager.GetDecimal(dr, "Margin");
                    objProduct.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        private List<CategoryDiscount> GetAllProductCategoryListByCustomerID(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProductCategoryListByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CategoryDiscount> objlst = new List<CategoryDiscount>();
                while (dr.Read())
                {
                    CategoryDiscount objCategory = new CategoryDiscount();
                    objCategory.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objCategory.CategoryCode = objBaseSqlManager.GetTextValue(dr, "CategoryCode");
                    objCategory.CategoryTypeID = objBaseSqlManager.GetInt32(dr, "CategoryTypeID");
                    objCategory.CategoryTypestr = new Utility1().GetTextEnum(objCategory.CategoryTypeID);
                    objCategory.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objCategory.CategoryDescription = objBaseSqlManager.GetTextValue(dr, "CategoryDescription");
                    objCategory.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCategory.CategoryDiscountID = objBaseSqlManager.GetInt64(dr, "CategoryDiscountID");
                    objCategory.Discount = objBaseSqlManager.GetDecimal(dr, "Discount");
                    objCategory.Margin = objBaseSqlManager.GetDecimal(dr, "Margin");
                    objCategory.Contract = objBaseSqlManager.GetTextValue(dr, "Contract");
                    objCategory.ValidFrom = objBaseSqlManager.GetDateTime(dr, "ValidFrom");
                    objCategory.ValidTo = objBaseSqlManager.GetDateTime(dr, "ValidTo");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool AddCustomerDiscount(RetCustomerDiscountListResponse data)
        {
            if (data.lstCategoryDiscount != null)
            {
                foreach (var item in data.lstCategoryDiscount)
                {
                    RetCustomerCategoryDiscountMst objProductQty = new RetCustomerCategoryDiscountMst();
                    objProductQty.CategoryDiscountID = item.CategoryDiscountID;
                    objProductQty.CustomerID = data.CustomerID;
                    objProductQty.Contract = item.Contract;
                    objProductQty.ValidFrom = item.ValidFrom;
                    objProductQty.ValidTo = item.ValidTo;
                    objProductQty.CategoryID = item.CategoryID;
                    objProductQty.Discount = item.Discount;
                    objProductQty.Margin = item.Margin;
                    objProductQty.IsDelete = false;
                    long exist = GetRetCustomerCategoryDiscountByCategoryID(objProductQty.CategoryID, objProductQty.CustomerID);
                    objProductQty.CategoryDiscountID = exist;
                    using (VirakiEntities context = new VirakiEntities())
                    {
                        if (objProductQty.CategoryDiscountID == 0)
                        {
                            context.RetCustomerCategoryDiscountMsts.Add(objProductQty);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objProductQty).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }

            }



            if (data.lstProductDiscount != null)
            {
                foreach (var item in data.lstProductDiscount)
                {
                    RetCustomerProductDiscountMst objProductQty = new RetCustomerProductDiscountMst();
                    objProductQty.ProductDiscountID = item.ProductDiscountID;
                    objProductQty.CustomerID = data.CustomerID;
                    objProductQty.Contract = item.Contract;
                    objProductQty.ValidFrom = item.ValidFrom;
                    objProductQty.ValidTo = item.ValidTo;
                    objProductQty.ProductQtyID = item.ProductQtyID;
                    objProductQty.Discount = item.Discount;
                    objProductQty.Margin = item.Margin;
                    objProductQty.IsDelete = item.IsDelete;


                    long exist = GetRetCustomerProductDiscountByCategoryID(objProductQty.ProductQtyID, objProductQty.CustomerID);
                    objProductQty.ProductDiscountID = exist;

                    if (objProductQty.IsDelete == true)
                    {
                        if (objProductQty.ProductDiscountID != 0)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "DeleteCustomerProductDiscount";
                                cmdGet.Parameters.AddWithValue("@ProductDiscountID", objProductQty.ProductDiscountID);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }
                    }
                    else
                    {
                        using (VirakiEntities context = new VirakiEntities())
                        {
                            if (objProductQty.ProductDiscountID == 0)
                            {
                                context.RetCustomerProductDiscountMsts.Add(objProductQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objProductQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }

            }

            return true;
        }

        private long GetRetCustomerCategoryDiscountByCategoryID(long CategoryID, long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustomerCategoryDiscountByCategoryID";
                cmdGet.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long CategoryDiscountID = 0;
                while (dr.Read())
                {
                    CategoryDiscountID = objBaseSqlManager.GetInt64(dr, "CategoryDiscountID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CategoryDiscountID;
            }
        }

        private long GetRetCustomerProductDiscountByCategoryID(long ProductQtyID, long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustomerProductDiscountByProductQtyID";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long ProductDiscountID = 0;
                while (dr.Read())
                {
                    ProductDiscountID = objBaseSqlManager.GetInt64(dr, "ProductDiscountID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();

                return ProductDiscountID;
            }
        }

        public List<RetProductQtyListResponse> GetAllRetProductQtyList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProductQtyList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductQtyListResponse> lstArea = new List<RetProductQtyListResponse>();
                while (dr.Read())
                {
                    RetProductQtyListResponse objArea = new RetProductQtyListResponse();
                    objArea.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objArea.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    lstArea.Add(objArea);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstArea;
            }
        }

        public string GetCustomerNameByCustomerID(long CustomerID)
        {
            string CustomerName = "";
            CustomerListResponse obj = new CustomerListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustomerNameByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CustomerName;
            }
        }

        public RetCustomerListResponse GetExistCustomerDetials(string CustomerName, long AreaID)
        {
            RetCustomerListResponse obj = new RetCustomerListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExistRetCustomerDetials";
                cmdGet.Parameters.AddWithValue("@CustomerName", CustomerName);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return obj;
        }

        public List<RetCustomerListResponse> GetAllCustomerListForExel()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerListForExcel";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> objlst = new List<RetCustomerListResponse>();
                while (dr.Read())
                {
                    RetCustomerListResponse objCustomer = new RetCustomerListResponse();
                    objCustomer.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objCustomer.DaysofWeekstr = new Utility().GetTextEnum(objCustomer.DaysofWeek);
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objCustomer.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomer.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objCustomer.TaxNo2 = objBaseSqlManager.GetTextValue(dr, "TaxNo2");
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

        public RetCustomerArticleCodeListResponse GetRetailArticleCodeForCustomerGroup(long CustomerGroupID)
        {
            RetCustomerArticleCodeListResponse objlst = new RetCustomerArticleCodeListResponse();
            objlst.lstProductArticleCode = GetAllProductArticleCodeListByCustomerGroupID(CustomerGroupID);
            if (objlst.lstProductArticleCode.Count > 0)
            {
                var lst = objlst.lstProductArticleCode.ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    if (!string.IsNullOrEmpty(lst[i].ArticleCode))
                    {
                        objlst.ArticleCode = lst[i].ArticleCode;
                        break;
                    }
                }
            }
            return objlst;
        }

        private List<ProductArticleCode> GetAllProductArticleCodeListByCustomerGroupID(long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllProductArticleCodeListByCustomerGroupID";
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductArticleCode> objlst = new List<ProductArticleCode>();
                while (dr.Read())
                {
                    ProductArticleCode objProduct = new ProductArticleCode();
                    objProduct.ProductArticleCodeID = objBaseSqlManager.GetInt64(dr, "ProductArticleCodeID");
                    objProduct.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objProduct.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objProduct.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                    objProduct.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool AddProductArticleCode(RetCustomerArticleCodeListResponse data)
        {
            if (data.lstProductArticleCode != null)
            {
                foreach (var item in data.lstProductArticleCode)
                {
                    RetProductArticleCodeMst objProductQty = new RetProductArticleCodeMst();
                    objProductQty.ProductArticleCodeID = item.ProductArticleCodeID;
                    objProductQty.CustomerGroupID = data.CustomerGroupID;
                    objProductQty.ProductQtyID = item.ProductQtyID;
                    objProductQty.ArticleCode = item.ArticleCode;
                    objProductQty.IsDelete = item.IsDelete;
                    objProductQty.CreatedBy = data.CreatedBy;
                    objProductQty.CreatedOn = data.CreatedOn;
                    objProductQty.UpdatedBy = data.UpdatedBy;
                    objProductQty.UpdatedOn = data.UpdatedOn;
                    long exist = GetRetCustomerGroupArticleCodeByProductQtyID(objProductQty.ProductQtyID, objProductQty.CustomerGroupID);
                    objProductQty.ProductArticleCodeID = exist;
                    if (objProductQty.IsDelete == true)
                    {
                        if (objProductQty.ProductArticleCodeID != 0)
                        {
                            SqlCommand cmdGet = new SqlCommand();
                            using (var objBaseSqlManager = new BaseSqlManager())
                            {
                                cmdGet.CommandType = CommandType.StoredProcedure;
                                cmdGet.CommandText = "DeleteCustomerGroupProductArticleCode";
                                cmdGet.Parameters.AddWithValue("@ProductArticleCodeID", objProductQty.ProductArticleCodeID);
                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                objBaseSqlManager.ForceCloseConnection();
                            }
                        }
                    }
                    else
                    {
                        using (VirakiEntities context = new VirakiEntities())
                        {
                            if (objProductQty.ProductArticleCodeID == 0)
                            {
                                context.RetProductArticleCodeMsts.Add(objProductQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objProductQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }

            }
            return true;
        }

        private long GetRetCustomerGroupArticleCodeByProductQtyID(long ProductQtyID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustomerGroupArticleCodeByProductQtyID";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long ProductArticleCodeID = 0;
                while (dr.Read())
                {
                    ProductArticleCodeID = objBaseSqlManager.GetInt64(dr, "ProductArticleCodeID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();

                return ProductArticleCodeID;
            }
        }

        public List<RetCustomerListResponse> GetAllCustomerFSSAIExpireList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerFSSAIExpireList";
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

        public bool UpdateFSSAIDate(long CustomerID, DateTime? FSSAIValidUpTo, DateTime? FSSAIValidUpTo2)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateRetFSSAIDate";
                    cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmdGet.Parameters.AddWithValue("@FSSAIValidUpTo", FSSAIValidUpTo);
                    cmdGet.Parameters.AddWithValue("@FSSAIValidUpTo2", FSSAIValidUpTo2);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }


        public List<CountryNameModel> GetAllCountryName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCountryName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CountryNameModel> lstCountry = new List<CountryNameModel>();
                while (dr.Read())
                {
                    CountryNameModel obj = new CountryNameModel();
                    obj.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
                    obj.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    lstCountry.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCountry;
            }
        }


        public string GetOldFSSAICertificateByCustomerID(long CustomerID)
        {
            RetCustomerMst FSSAICertificate = new RetCustomerMst();
            string FSSAICerti = string.Empty;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    FSSAICertificate = context.RetCustomerMsts.Where(i => i.CustomerID == CustomerID).FirstOrDefault();
                }
                if (FSSAICertificate != null)
                {
                    FSSAICerti = FSSAICertificate.FSSAICertificate;
                }
            }
            catch (Exception ex) { }
            return FSSAICerti;
        }

        public string GetOldFSSAICertificate2ByCustomerID(long CustomerID)
        {
            RetCustomerMst FSSAICertificate2 = new RetCustomerMst();
            string FSSAICerti = string.Empty;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    FSSAICertificate2 = context.RetCustomerMsts.Where(i => i.CustomerID == CustomerID).FirstOrDefault();
                }
                if (FSSAICertificate2 != null)
                {
                    FSSAICerti = FSSAICertificate2.FSSAICertificate2;
                }
            }
            catch (Exception ex) { }
            return FSSAICerti;
        }

    }
}
