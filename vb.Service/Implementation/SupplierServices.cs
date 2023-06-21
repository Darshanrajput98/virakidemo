using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;

namespace vb.Service
{
    public class SupplierServices : ISupplierService
    {
        public bool AddSupplier(AddSupplier data)
        {
            Supplier_Master ObjSupplier = new Supplier_Master();
            ObjSupplier.SupplierID = data.SupplierID;
            ObjSupplier.SupplierName = data.SupplierName;
            ObjSupplier.GSTNo = data.GSTNo;
            ObjSupplier.TaxID = data.TaxID;
            ObjSupplier.PanCardNumber = data.PanCardNumber;
            ObjSupplier.FSSAI = data.FSSAI;
            if ((data.FSSAIValidUpTo).ToString() != "" && (data.FSSAIValidUpTo) != null)
            {
                ObjSupplier.FSSAIValidUpTo = Convert.ToDateTime(data.FSSAIValidUpTo);
            }
            else
            {
                ObjSupplier.FSSAIValidUpTo = null;
            }
            //  ObjSupplier.FSSAIValidUpTo = data.FSSAIValidUpTo;
            ObjSupplier.BankName = data.BankName;
            ObjSupplier.Branch = data.Branch;
            ObjSupplier.AccountNumber = data.AccountNumber;
            ObjSupplier.IFSCCode = data.IFSCCode;

            // 16 June 2021 Piyush Limbani
            ObjSupplier.TDSCategoryID = data.TDSCategoryID;
            ObjSupplier.TDSPercentage = data.TDSPercentage;
            // 16 June 2021 Piyush Limbani

            ObjSupplier.AddressOneLine1 = data.AddressOneLine1;
            ObjSupplier.AddressOneLine2 = data.AddressOneLine2;
            ObjSupplier.AreaIDOne = data.AreaIDOne;
            ObjSupplier.StateOne = data.StateOne;
            ObjSupplier.AddressOnePincode = data.AddressOnePincode;
            ObjSupplier.AddressTwoLine1 = data.AddressTwoLine1;
            ObjSupplier.AddressTwoLine2 = data.AddressTwoLine2;
            ObjSupplier.AreaIDTwo = data.AreaIDTwo;
            ObjSupplier.StateTwo = data.StateTwo;
            ObjSupplier.AddressTwoPincode = data.AddressTwoPincode;
            ObjSupplier.CreatedBy = data.CreatedBy;
            ObjSupplier.CreatedOn = data.CreatedOn;
            ObjSupplier.UpdatedBy = data.UpdatedBy;
            ObjSupplier.UpdatedOn = data.UpdatedOn;
            ObjSupplier.IsDelete = false;
            ObjSupplier.Identification = "Purchase";
            ObjSupplier.NameAsBankAccount = data.NameAsBankAccount;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjSupplier.SupplierID == 0)
                {
                    context.Supplier_Master.Add(ObjSupplier);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjSupplier).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjSupplier.SupplierID > 0)
                {
                    foreach (var item in data.lstContactDetail)
                    {
                        SupplierContact_Master objContact = new SupplierContact_Master();
                        objContact.SupplierContactID = item.SupplierContactID;
                        objContact.AddressID = item.AddressID;
                        objContact.SupplierID = ObjSupplier.SupplierID;
                        objContact.Name = item.Name;
                        objContact.PhoneNo = item.PhoneNo;
                        objContact.MobileNo = item.MobileNo;
                        objContact.Email = item.Email;
                        objContact.CreatedBy = data.CreatedBy;
                        objContact.CreatedOn = data.CreatedOn;
                        objContact.UpdatedBy = data.UpdatedBy;
                        objContact.UpdatedOn = data.UpdatedOn;
                        objContact.IsDelete = false;
                        if (objContact.SupplierContactID == 0)
                        {
                            context.SupplierContact_Master.Add(objContact);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objContact).State = EntityState.Modified;
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

        public List<SupplierListResponse> GetAllSupplierList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllSupplierList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SupplierListResponse> objlst = new List<SupplierListResponse>();
                while (dr.Read())
                {
                    SupplierListResponse objCustomer = new SupplierListResponse();
                    objCustomer.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objCustomer.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objCustomer.GSTNo = objBaseSqlManager.GetTextValue(dr, "GSTNo");
                    objCustomer.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objCustomer.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objCustomer.PanCardNumber = objBaseSqlManager.GetTextValue(dr, "PanCardNumber");
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
                    objCustomer.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objCustomer.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    objCustomer.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objCustomer.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objCustomer.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    objCustomer.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    objCustomer.TDSPercentage = objBaseSqlManager.GetDecimal(dr, "TDSPercentage");
                    objCustomer.AddressOneLine1 = objBaseSqlManager.GetTextValue(dr, "AddressOneLine1");
                    objCustomer.AddressOneLine2 = objBaseSqlManager.GetTextValue(dr, "AddressOneLine2");
                    objCustomer.AreaIDOne = objBaseSqlManager.GetInt64(dr, "AreaIDOne");
                    objCustomer.AreaNameOne = objBaseSqlManager.GetTextValue(dr, "AreaNameOne");
                    objCustomer.StateOne = objBaseSqlManager.GetTextValue(dr, "StateOne");
                    objCustomer.AddressOnePincode = objBaseSqlManager.GetTextValue(dr, "AddressOnePincode");
                    objCustomer.AddressTwoLine1 = objBaseSqlManager.GetTextValue(dr, "AddressTwoLine1");
                    objCustomer.AddressTwoLine2 = objBaseSqlManager.GetTextValue(dr, "AddressTwoLine2");
                    objCustomer.AreaIDTwo = objBaseSqlManager.GetInt64(dr, "AreaIDTwo");
                    objCustomer.AreaNameTwo = objBaseSqlManager.GetTextValue(dr, "AreaNameTwo");
                    objCustomer.StateTwo = objBaseSqlManager.GetTextValue(dr, "StateTwo");
                    objCustomer.AddressTwoPincode = objBaseSqlManager.GetTextValue(dr, "AddressTwoPincode");
                    objCustomer.ContactName = objBaseSqlManager.GetTextValue(dr, "ContactName");
                    string ContactPhoneNo = GetContactPhonebySupplierID(objCustomer.SupplierID);
                    objCustomer.ContactPhoneNo = ContactPhoneNo.TrimEnd(',');
                    string ContactMobileNo = GetContactMobilebySupplierID(objCustomer.SupplierID);
                    objCustomer.ContactMobileNo = ContactMobileNo.TrimEnd(',');
                    string ContactEmail = GetContactEmailbySupplierID(objCustomer.SupplierID);
                    objCustomer.ContactEmail = ContactEmail.TrimEnd(',');
                    objCustomer.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objCustomer.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objCustomer.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCustomer.NameAsBankAccount = objBaseSqlManager.GetTextValue(dr, "NameAsBankAccount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<SupplierContactDetail> GetSupplierAddressListBySupplierID(long SupplierID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSupplierAddressListBySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SupplierContactDetail> objlst = new List<SupplierContactDetail>();
                while (dr.Read())
                {
                    SupplierContactDetail obj = new SupplierContactDetail();
                    obj.SupplierContactID = objBaseSqlManager.GetInt64(dr, "SupplierContactID");
                    obj.AddressID = objBaseSqlManager.GetInt64(dr, "AddressID");
                    obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    obj.PhoneNo = objBaseSqlManager.GetTextValue(dr, "PhoneNo");
                    obj.MobileNo = objBaseSqlManager.GetTextValue(dr, "MobileNo");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteSupplier(long SupplierID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteSupplier";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<SupplierListResponse> GetAllSupplierFSSAIExpireList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllSupplierFSSAIExpireList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SupplierListResponse> objlst = new List<SupplierListResponse>();
                while (dr.Read())
                {
                    SupplierListResponse objSupplier = new SupplierListResponse();
                    objSupplier.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objSupplier.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objSupplier.AreaOne = objBaseSqlManager.GetTextValue(dr, "AreaOne");
                    objSupplier.AreaTwo = objBaseSqlManager.GetTextValue(dr, "AreaTwo");
                    objSupplier.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objSupplier.FSSAIValidUpTo = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo");
                    if (objSupplier.FSSAIValidUpTo == Convert.ToDateTime("10/10/2014"))
                    {
                        objSupplier.FSSAIValidUpTostr = "";
                    }
                    else
                    {
                        objSupplier.FSSAIValidUpTostr = objSupplier.FSSAIValidUpTo.ToString("dd/MM/yyyy");
                    }
                    string ContactPhoneNo = GetContactPhonebySupplierID(objSupplier.SupplierID);
                    objSupplier.ContactPhoneNo = ContactPhoneNo.TrimEnd(',');
                    string ContactMobileNo = GetContactMobilebySupplierID(objSupplier.SupplierID);
                    objSupplier.ContactMobileNo = ContactMobileNo.TrimEnd(',');
                    string ContactEmail = GetContactEmailbySupplierID(objSupplier.SupplierID);
                    objSupplier.ContactEmail = ContactEmail.TrimEnd(',');
                    objSupplier.DaysRemaining = objBaseSqlManager.GetInt64(dr, "DaysRemaining");
                    objlst.Add(objSupplier);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        private string GetContactPhonebySupplierID(long SupplierID)
        {
            string ContactPhoneNo = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetContactPhonebySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    SupplierContactDetail obj = new SupplierContactDetail();
                    obj.PhoneNo = objBaseSqlManager.GetTextValue(dr, "PhoneNo");
                    ContactPhoneNo += obj.PhoneNo + ",";
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ContactPhoneNo;
            }
        }

        private string GetContactMobilebySupplierID(long SupplierID)
        {
            string ContactMobileNo = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetContactMobilebySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    SupplierContactDetail obj = new SupplierContactDetail();
                    obj.MobileNo = objBaseSqlManager.GetTextValue(dr, "MobileNo");
                    ContactMobileNo += obj.MobileNo + ",";
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ContactMobileNo;
            }
        }

        private string GetContactEmailbySupplierID(long SupplierID)
        {
            string ContactEmail = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetContactEmailbySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    SupplierContactDetail obj = new SupplierContactDetail();
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    ContactEmail += obj.Email + ",";
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ContactEmail;
            }
        }

        public bool UpdateSupplierFSSAIDate(long SupplierID, DateTime FSSAIValidUpTo)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateSupplierFSSAIDate";
                    cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                    cmdGet.Parameters.AddWithValue("@FSSAIValidUpTo", FSSAIValidUpTo);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public List<SupplierListResponse> GetAllSupplierName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllSupplierName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SupplierListResponse> lstPeoduct = new List<SupplierListResponse>();
                while (dr.Read())
                {
                    SupplierListResponse objProduct = new SupplierListResponse();
                    objProduct.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objProduct.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    lstPeoduct.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        // Expense supplier 18-12-2019
        public bool AddExpenseSupplier(AddExpenseSupplier data)
        {
            ExpenseSupplier_Master ObjSupplier = new ExpenseSupplier_Master();
            ObjSupplier.SupplierID = data.SupplierID;
            ObjSupplier.SupplierName = data.SupplierName;
            ObjSupplier.GSTNo = data.GSTNo;
            ObjSupplier.TaxID = data.TaxID;
            ObjSupplier.PanCardNumber = data.PanCardNumber;
            ObjSupplier.BankName = data.BankName;
            ObjSupplier.Branch = data.Branch;
            ObjSupplier.AccountNumber = data.AccountNumber;
            ObjSupplier.IFSCCode = data.IFSCCode;
            ObjSupplier.TDSCategoryID = data.TDSCategoryID;
            ObjSupplier.TDSPercentage = data.TDSPercentage;
            ObjSupplier.AddressOneLine1 = data.AddressOneLine1;
            ObjSupplier.AddressOneLine2 = data.AddressOneLine2;
            ObjSupplier.AreaIDOne = data.AreaIDOne;
            ObjSupplier.StateOne = data.StateOne;
            ObjSupplier.AddressOnePincode = data.AddressOnePincode;
            ObjSupplier.AddressTwoLine1 = data.AddressTwoLine1;
            ObjSupplier.AddressTwoLine2 = data.AddressTwoLine2;
            ObjSupplier.AreaIDTwo = data.AreaIDTwo;
            ObjSupplier.StateTwo = data.StateTwo;
            ObjSupplier.AddressTwoPincode = data.AddressTwoPincode;
            ObjSupplier.CreatedBy = data.CreatedBy;
            ObjSupplier.CreatedOn = data.CreatedOn;
            ObjSupplier.UpdatedBy = data.UpdatedBy;
            ObjSupplier.UpdatedOn = data.UpdatedOn;
            ObjSupplier.IsDelete = false;
            ObjSupplier.Identification = "Expense";
            ObjSupplier.NameAsBankAccount = data.NameAsBankAccount;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjSupplier.SupplierID == 0)
                {
                    context.ExpenseSupplier_Master.Add(ObjSupplier);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjSupplier).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjSupplier.SupplierID > 0)
                {
                    foreach (var item in data.lstContactDetail)
                    {
                        ExpenseSupplierContact_Master objContact = new ExpenseSupplierContact_Master();
                        objContact.SupplierContactID = item.SupplierContactID;
                        objContact.AddressID = item.AddressID;
                        objContact.SupplierID = ObjSupplier.SupplierID;
                        objContact.Name = item.Name;
                        objContact.PhoneNo = item.PhoneNo;
                        objContact.MobileNo = item.MobileNo;
                        objContact.Email = item.Email;
                        objContact.CreatedBy = data.CreatedBy;
                        objContact.CreatedOn = data.CreatedOn;
                        objContact.UpdatedBy = data.UpdatedBy;
                        objContact.UpdatedOn = data.UpdatedOn;
                        objContact.IsDelete = false;
                        if (objContact.SupplierContactID == 0)
                        {
                            context.ExpenseSupplierContact_Master.Add(objContact);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objContact).State = EntityState.Modified;
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

        public List<ExpenseSupplierListResponse> GetAllExpenseSupplierList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseSupplierList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseSupplierListResponse> objlst = new List<ExpenseSupplierListResponse>();
                while (dr.Read())
                {
                    ExpenseSupplierListResponse objCustomer = new ExpenseSupplierListResponse();
                    objCustomer.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objCustomer.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objCustomer.GSTNo = objBaseSqlManager.GetTextValue(dr, "GSTNo");
                    objCustomer.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objCustomer.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objCustomer.PanCardNumber = objBaseSqlManager.GetTextValue(dr, "PanCardNumber");
                    objCustomer.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objCustomer.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    objCustomer.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objCustomer.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objCustomer.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    objCustomer.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    objCustomer.TDSPercentage = objBaseSqlManager.GetDecimal(dr, "TDSPercentage");
                    objCustomer.AddressOneLine1 = objBaseSqlManager.GetTextValue(dr, "AddressOneLine1");
                    objCustomer.AddressOneLine2 = objBaseSqlManager.GetTextValue(dr, "AddressOneLine2");
                    objCustomer.AreaIDOne = objBaseSqlManager.GetInt64(dr, "AreaIDOne");
                    objCustomer.AreaNameOne = objBaseSqlManager.GetTextValue(dr, "AreaNameOne");
                    objCustomer.StateOne = objBaseSqlManager.GetTextValue(dr, "StateOne");
                    objCustomer.AddressOnePincode = objBaseSqlManager.GetTextValue(dr, "AddressOnePincode");
                    objCustomer.AddressTwoLine1 = objBaseSqlManager.GetTextValue(dr, "AddressTwoLine1");
                    objCustomer.AddressTwoLine2 = objBaseSqlManager.GetTextValue(dr, "AddressTwoLine2");
                    objCustomer.AreaIDTwo = objBaseSqlManager.GetInt64(dr, "AreaIDTwo");
                    objCustomer.AreaNameTwo = objBaseSqlManager.GetTextValue(dr, "AreaNameTwo");
                    objCustomer.StateTwo = objBaseSqlManager.GetTextValue(dr, "StateTwo");
                    objCustomer.AddressTwoPincode = objBaseSqlManager.GetTextValue(dr, "AddressTwoPincode");
                    objCustomer.ContactName = objBaseSqlManager.GetTextValue(dr, "ContactName");
                    string ContactPhoneNo = GetContactPhonebySupplierID(objCustomer.SupplierID);
                    objCustomer.ContactPhoneNo = ContactPhoneNo.TrimEnd(',');
                    string ContactMobileNo = GetContactMobilebySupplierID(objCustomer.SupplierID);
                    objCustomer.ContactMobileNo = ContactMobileNo.TrimEnd(',');
                    string ContactEmail = GetContactEmailbySupplierID(objCustomer.SupplierID);
                    objCustomer.ContactEmail = ContactEmail.TrimEnd(',');
                    objCustomer.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objCustomer.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objCustomer.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCustomer.NameAsBankAccount = objBaseSqlManager.GetTextValue(dr, "NameAsBankAccount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpenseSupplierContactDetail> GetExpenseSupplierAddressListBySupplierID(long SupplierID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpenseSupplierAddressListBySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseSupplierContactDetail> objlst = new List<ExpenseSupplierContactDetail>();
                while (dr.Read())
                {
                    ExpenseSupplierContactDetail obj = new ExpenseSupplierContactDetail();
                    obj.SupplierContactID = objBaseSqlManager.GetInt64(dr, "SupplierContactID");
                    obj.AddressID = objBaseSqlManager.GetInt64(dr, "AddressID");
                    obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    obj.PhoneNo = objBaseSqlManager.GetTextValue(dr, "PhoneNo");
                    obj.MobileNo = objBaseSqlManager.GetTextValue(dr, "MobileNo");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteExpenseSupplier(long SupplierID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteExpenseSupplier";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<SupplierListResponse> GetAllExpenseSupplierName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseSupplierName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SupplierListResponse> lstPeoduct = new List<SupplierListResponse>();
                while (dr.Read())
                {
                    SupplierListResponse objProduct = new SupplierListResponse();
                    objProduct.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objProduct.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    lstPeoduct.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }


        // 18 Aug 2020 Piyush Limbani
        public List<AllSupplierName> GetAllPurchaseAndExpenseSupplierName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseAndExpenseSupplierName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AllSupplierName> lst = new List<AllSupplierName>();
                while (dr.Read())
                {
                    AllSupplierName obj = new AllSupplierName();
                    obj.SupplierID = objBaseSqlManager.GetTextValue(dr, "SupplierID");
                    obj.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

    }
}
