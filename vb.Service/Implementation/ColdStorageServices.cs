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

namespace vb.Service
{
    public class ColdStorageServices : IColdStorageService
    {

        //Coldstorage Details 

        public long AddColdStorage(ColdStorage_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ColdStorageID == 0)
                {
                    context.ColdStorage_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ColdStorageID > 0)
                {
                    return Obj.ColdStorageID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ColdStorageListResponse> GetAllColdStorageList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllColdStorageList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ColdStorageListResponse> objlst = new List<ColdStorageListResponse>();
                while (dr.Read())
                {
                    ColdStorageListResponse obj = new ColdStorageListResponse();
                    obj.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    obj.ShortName = objBaseSqlManager.GetTextValue(dr, "ShortName");
                    obj.Address = objBaseSqlManager.GetTextValue(dr, "Address");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    obj.GSTNo = objBaseSqlManager.GetTextValue(dr, "GSTNo");
                    obj.PANNo = objBaseSqlManager.GetTextValue(dr, "PANNo");
                    obj.FssaiLicenseNo = objBaseSqlManager.GetTextValue(dr, "FssaiLicenseNo");

                    //08-07-2022
                    obj.ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate");
                    if (obj.ExpiryDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ExpiryDatestr = obj.ExpiryDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ExpiryDatestr = "";
                    }

                    obj.ContactPersonName = objBaseSqlManager.GetTextValue(dr, "ContactPersonName");
                    obj.ContactPersonName1 = objBaseSqlManager.GetTextValue(dr, "ContactPersonName1");
                    obj.ContactPersonName2 = objBaseSqlManager.GetTextValue(dr, "ContactPersonName2");
                    obj.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    obj.ContactNumber1 = objBaseSqlManager.GetTextValue(dr, "ContactNumber1");
                    obj.ContactNumber2 = objBaseSqlManager.GetTextValue(dr, "ContactNumber2");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteColdStorage(long ColdStorageID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteColdStorage";
                cmdGet.Parameters.AddWithValue("@ColdStorageID", ColdStorageID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //Inward Details

        public List<string> GetColdstorageByTextChange(string Name)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetColdstorageName";
                cmdGet.Parameters.AddWithValue("@Name", Name);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<string> coldstoragename = new List<string>();
                while (dr.Read())
                {
                    coldstoragename.Add(objBaseSqlManager.GetTextValue(dr, "Name"));
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return coldstoragename;
            }
        }

        public List<GetProductDetaiForPurchase> GetAutoCompleteProductDetaiForInward(long Prefix)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteProductDetaiForInward";
                cmdGet.Parameters.AddWithValue("@ProductID", Prefix.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetProductDetaiForPurchase> objlst = new List<GetProductDetaiForPurchase>();
                while (dr.Read())
                {
                    GetProductDetaiForPurchase obj = new GetProductDetaiForPurchase();
                    //obj.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public ColdStorageName1 GetColdStorageByColdStorageID(long ColdStorageID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetColdstorageByColdstorageID";
                cmdGet.Parameters.AddWithValue("@ColdStorageID", ColdStorageID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ColdStorageName1 objOrder = new ColdStorageName1();
                while (dr.Read())
                {
                    objOrder.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    objOrder.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public AddInwardDetails GetPurchaseOrderDetailsByInwardID(long InwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                cmdGet.CommandText = "GetPurchaseOrderDetailsByInwardID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AddInwardDetails objM = new AddInwardDetails();
                List<AddInwardQtyDetail> lstInwardQty = new List<AddInwardQtyDetail>();
                while (dr.Read())
                {
                    objM.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    objM.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    objM.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    objM.Date = objBaseSqlManager.GetDateTime(dr, "Date");

                    if (objM.Date != Convert.ToDateTime("10/10/2014"))
                    {
                        objM.Datestr = objM.Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objM.Datestr = "";
                    }

                    objM.LotNo = objBaseSqlManager.GetTextValue(dr, "LotNo");
                    objM.DeliveryChallanNumber = objBaseSqlManager.GetTextValue(dr, "DeliveryChallanNumber");

                    // 28-06-2022
                    objM.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");

                    // 29-06-2022
                    objM.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");

                    objM.DeliveryChallanDate = objBaseSqlManager.GetDateTime(dr, "DeliveryChallanDate");

                    if (objM.DeliveryChallanDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objM.DeliveryChallanDatestr = objM.DeliveryChallanDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objM.DeliveryChallanDatestr = "";
                    }

                    objM.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objM.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");

                    // 21-12-2022
                    objM.ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate");

                    if (objM.ExpiryDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objM.ExpiryDatestr = objM.ExpiryDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objM.ExpiryDatestr = "";
                    }


                    AddInwardQtyDetail obj = new AddInwardQtyDetail();
                    obj.InwardQtyID = objBaseSqlManager.GetInt64(dr, "InwardQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.Notes = objBaseSqlManager.GetTextValue(dr, "Notes");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.WeightPerBag = objBaseSqlManager.GetDecimal(dr, "WeightPerBag");
                    obj.TotalWeight = objBaseSqlManager.GetDecimal(dr, "TotalWeight");
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");

                    // 28-06-2022
                    obj.RentPerBags = objBaseSqlManager.GetDecimal(dr, "RentPerBags");

                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    //obj.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");

                    lstInwardQty.Add(obj);
                }
                objM.lstInwardQty = lstInwardQty;
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        public List<ProductListResponse> GetAllProductName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseProductName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductListResponse> lstPeoduct = new List<ProductListResponse>();
                while (dr.Read())
                {
                    ProductListResponse objProduct = new ProductListResponse();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    lstPeoduct.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public string AddInward(AddInwardDetails data)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                long InwardID = 0;
                string Message = "";
                ColdStorage_Inward_Mst obj = new ColdStorage_Inward_Mst();
                obj.InwardID = data.InwardID;
                obj.ColdStorageID = data.ColdStorageID;
                obj.Name = data.Name;

                if (data.Date == Convert.ToDateTime("01/01/0001"))
                {
                    obj.Date = null;
                }
                else
                {
                    obj.Date = data.Date;
                }

                // 21-12-2022 
                if (data.ExpiryDate == Convert.ToDateTime("01/01/0001"))
                {
                    obj.ExpiryDate = null;
                }
                else
                {
                    obj.ExpiryDate = data.ExpiryDate;
                }

                obj.LotNo = data.LotNo;
                obj.DeliveryChallanNumber = data.DeliveryChallanNumber;

                // 28-06-2022
                obj.SupplierID = data.SupplierID;

                // 29-06-2022
                obj.BillNumber = data.BillNumber;

                if (data.DeliveryChallanDate == Convert.ToDateTime("01/01/0001"))
                {
                    obj.DeliveryChallanDate = null;
                }
                else
                {
                    obj.DeliveryChallanDate = data.DeliveryChallanDate;
                }

                obj.CreatedBy = data.CreatedBy;
                obj.CreatedOn = data.CreatedOn;
                obj.UpdatedBy = data.UpdatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                obj.IsDelete = false;

                if (obj.InwardID == 0)
                {
                    if (InwardID == 0)
                    {
                        context.ColdStorage_Inward_Mst.Add(obj);
                        context.SaveChanges();
                        Message = "Insert Sucessfully";
                    }
                    else
                    {
                        //context.Entry(obj).State = EntityState.Modified;
                        //context.SaveChanges();
                        Message = "Bill Exist";
                    }
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    Message = "Updated Sucessfully";
                }

                if (obj.InwardID > 0)
                {
                    try
                    {
                        foreach (var item in data.lstInwardQty)
                        {
                            ColdStorage_InwardQty_Mst objInwardQty = new ColdStorage_InwardQty_Mst();
                            objInwardQty.InwardQtyID = item.InwardQtyID;
                            objInwardQty.ProductID = item.ProductID;
                            objInwardQty.InwardID = obj.InwardID;
                            objInwardQty.Notes = item.Notes;
                            objInwardQty.HSNNumber = item.HSNNumber;
                            objInwardQty.NoofBags = item.NoofBags;
                            objInwardQty.WeightPerBag = item.WeightPerBag;
                            objInwardQty.TotalWeight = item.TotalWeight;
                            objInwardQty.RatePerKG = item.RatePerKG;

                            // 28-06-2022
                            objInwardQty.RentPerBags = item.RentPerBags;

                            objInwardQty.TotalAmount = item.TotalAmount;
                            objInwardQty.GrandTotal = data.GrandTotal;
                            objInwardQty.CreatedBy = data.CreatedBy;
                            objInwardQty.CreatedOn = data.CreatedOn;
                            objInwardQty.UpdatedBy = data.UpdatedBy;
                            objInwardQty.UpdatedOn = data.UpdatedOn;

                            objInwardQty.IsDelete = false;
                            if (objInwardQty.InwardQtyID == 0)
                            {
                                context.ColdStorage_InwardQty_Mst.Add(objInwardQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objInwardQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }

                        }
                        return Message;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            ColdStorage_Inward_Mst data1 = context.ColdStorage_Inward_Mst.Where(x => x.InwardID == obj.InwardID).FirstOrDefault();
                            if (data != null)
                            {
                                context.ColdStorage_Inward_Mst.Remove(data1);
                                context.SaveChanges();
                                Message = "Error";
                                return Message;
                            }
                            else
                            {
                                Message = "Error";
                                return Message;
                            }
                        }
                    }
                }
                else
                {
                    if (InwardID > 0)
                    {
                        Message = "Bill Exist";
                    }
                    else
                    {
                        Message = "Error";
                    }
                    return Message;
                }

            }
        }

        public List<InwardListResponse> GetAllColdStorage_InwardList(DateTime? ChallanDate, long ColdStorageID, string LotNo, long ProductID)
        {

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllColdStorage_InwardList";
                cmdGet.Parameters.AddWithValue("@ChallanDate", ChallanDate);
                cmdGet.Parameters.AddWithValue("@ColdStorageID", ColdStorageID);
                cmdGet.Parameters.AddWithValue("@LotNo", LotNo);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<InwardListResponse> objlst = new List<InwardListResponse>();
                while (dr.Read())
                {
                    InwardListResponse obj = new InwardListResponse();
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    //obj.Date = objBaseSqlManager.GetTextValue(dr, "Date");

                    //08-07-2022
                    DateTime Date = objBaseSqlManager.GetDateTime(dr, "Date");
                    if (Date == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.Date = "";
                    }
                    else
                    {
                        obj.Date = objBaseSqlManager.GetDateTime(dr, "Date").ToString("dd/MM/yyyy");
                    }
                    //21-12-2022
                    DateTime ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate");
                    if (ExpiryDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ExpiryDate = "";
                    }
                    else
                    {
                        obj.ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate").ToString("dd/MM/yyyy");
                    }

                    obj.LotNo = objBaseSqlManager.GetTextValue(dr, "LotNo");
                    obj.DeliveryChallanNumber = objBaseSqlManager.GetTextValue(dr, "DeliveryChallanNumber");
                    //obj.DeliveryChallanDate = objBaseSqlManager.GetTextValue(dr, "DeliveryChallanDate");

                    //08-07-2022
                    DateTime DeliveryChallanDate = objBaseSqlManager.GetDateTime(dr, "DeliveryChallanDate");
                    if (DeliveryChallanDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DeliveryChallanDate = "";
                    }
                    else
                    {
                        obj.DeliveryChallanDate = objBaseSqlManager.GetDateTime(dr, "DeliveryChallanDate").ToString("dd/MM/yyyy");
                    }
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.Notes = objBaseSqlManager.GetTextValue(dr, "Notes");
                    obj.NoofBags = objBaseSqlManager.GetInt64(dr, "NoofBags");
                    obj.UsedQty = objBaseSqlManager.GetDecimal(dr, "UsedQty");

                    //08-07-2022
                    //obj.RemQty = (obj.NoofBags - obj.UsedQty);
                    obj.RemQty = objBaseSqlManager.GetDecimal(dr, "RemQty");

                    obj.WeightPerBag = objBaseSqlManager.GetDecimal(dr, "WeightPerBag");
                    obj.TotalWeight = objBaseSqlManager.GetDecimal(dr, "TotalWeight");
                    obj.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    obj.RatePerKG = Math.Round(objBaseSqlManager.GetDecimal(dr, "RatePerKG"), 2);

                    // 28-06-2022
                    obj.RentPerBags = Math.Round(objBaseSqlManager.GetDecimal(dr, "RentPerBags"), 2);
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");

                    obj.lblTotal = objBaseSqlManager.GetTextValue(dr, "lblTotal");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //08-07-2022
        public string GetExistInwardDetails(long ColdStorageID, string LotNo)
        {
            AddInwardDetails obj = new AddInwardDetails();
            SqlCommand cmdGet = new SqlCommand();
            string ELotNo = "";
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExistInwardDetails";
                cmdGet.Parameters.AddWithValue("@ColdStorageID", ColdStorageID);
                cmdGet.Parameters.AddWithValue("@LotNo", LotNo);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    ELotNo = objBaseSqlManager.GetTextValue(dr, "LotNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ELotNo;
            }
        }

        public bool DeleteColdstorageInward(long InwardID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteColdstorageInward";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }


        //Outward Details

        public InwardListResponse GetAllColdStorage_OutwardID(long OutwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OutwardID", OutwardID);
                cmdGet.CommandText = "GetAllColdStorage_OutwardID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                InwardListResponse objM = new InwardListResponse();
                while (dr.Read())
                {
                    //objM.DeActiveColdStorage = objBaseSqlManager.GetBoolean(dr, "DeActiveColdStorage");
                    objM.OutwardID = objBaseSqlManager.GetInt64(dr, "OutwardID");
                    objM.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    objM.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objM.TotalQuantity = objBaseSqlManager.GetInt32(dr, "TotalQuantity");
                    objM.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objM.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    objM.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    objM.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objM.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        //12-28-2022 
        public long AddOutward(List<InwardListResponse> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {

                foreach (var item in data)
                {
                    string Message = "";

                    ColdStorage_Outward_Mst Obj = new ColdStorage_Outward_Mst();
                    Obj.OutwardID = item.OutwardID;
                    Obj.InwardID = item.InwardID;
                    Obj.TotalQuantity = item.TotalQuantity;
                    Obj.Quantity = item.Quantity;

                    if (item.Outward_date == Convert.ToDateTime("01/01/0001"))
                    {
                        Obj.Outward_date = null;
                    }
                    else
                    {
                        Obj.Outward_date = item.Outward_date;
                    }

                    Obj.ColdStorageID = item.ColdStorageID;
                    Obj.GodownIDTo = item.GodownIDTo;
                    Obj.CreatedBy = SessionUserID;
                    Obj.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    Obj.UpdatedBy = SessionUserID;
                    Obj.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                    Obj.IsDelete = false;

                    if (Obj.OutwardID == 0)
                    {
                        context.ColdStorage_Outward_Mst.Add(Obj);
                        context.SaveChanges();
                        Message = "Insert Sucessfully";
                    }
                    else
                    {
                        context.Entry(Obj).State = EntityState.Modified;
                        context.SaveChanges();
                        Message = "Updated Sucessfully";
                    }

                }
                return 0;
            }
        }

        // 01-03-2023
        public long UpdateOutward(InwardListResponse data)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateOutward";
                cmdGet.Parameters.AddWithValue("@OutwardID", data.OutwardID);
                cmdGet.Parameters.AddWithValue("@Quantity", data.Quantity);
                cmdGet.Parameters.AddWithValue("@Outward_date", data.Outward_date);
                cmdGet.Parameters.AddWithValue("@GodownIDTo", data.GodownIDTo);
                cmdGet.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                cmdGet.Parameters.AddWithValue("@UpdatedOn", data.UpdatedOn);

                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return data.OutwardID;
        }


        // 30-12-2022
        public List<OutwardListResponse> GetAllColdStorage_OutwardList(long ColdStorageID, DateTime? FromDate, DateTime? ToDate)
        {

            decimal sumNoofBags = 0;

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllColdStorage_OutwardList";
                cmdGet.Parameters.AddWithValue("@ColdStorageID", ColdStorageID);
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OutwardListResponse> objlst = new List<OutwardListResponse>();
                while (dr.Read())
                {
                    OutwardListResponse obj = new OutwardListResponse();
                    obj.OutwardID = objBaseSqlManager.GetInt64(dr, "OutwardID");
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.TotalQuantity = objBaseSqlManager.GetInt32(dr, "TotalQuantity");

                    obj.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");

                    sumNoofBags = sumNoofBags + Convert.ToDecimal(obj.Quantity);

                    obj.DeliveryChallanNumber = objBaseSqlManager.GetTextValue(dr, "DeliveryChallanNumber");
                    obj.RemQty = objBaseSqlManager.GetDecimal(dr, "RemQty");
                    //obj.Date = objBaseSqlManager.GetTextValue(dr, "Date");

                    //08-07-2022
                    DateTime Date = objBaseSqlManager.GetDateTime(dr, "Date");
                    if (Date == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.Date = "";
                    }
                    else
                    {
                        obj.Date = objBaseSqlManager.GetDateTime(dr, "Date").ToString("dd/MM/yyyy");
                    }
                    obj.LotNo = objBaseSqlManager.GetTextValue(dr, "LotNo");

                    //08-07-2022
                    DateTime Outward_date = objBaseSqlManager.GetDateTime(dr, "Outward_date");
                    if (Outward_date == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.Outward_date = "";
                    }
                    else
                    {
                        obj.Outward_date = objBaseSqlManager.GetDateTime(dr, "Outward_date").ToString("MM/dd/yyyy");
                    }

                    obj.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");

                    //30-12-2022
                    obj.GodownIDTo = objBaseSqlManager.GetInt64(dr, "GodownIDTo");

                    obj.GodownNameTo = objBaseSqlManager.GetTextValue(dr, "GodownNameTo");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }


                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].UsedNoofbags = sumNoofBags;
                }

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteColdstorageOutward(long OutwardID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteColdstorageOutward";
                cmdGet.Parameters.AddWithValue("@OutwardID", OutwardID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<ColdStorageNameDDL> GetAllColdStorageName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllColdStorageName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ColdStorageNameDDL> lstColdStorage = new List<ColdStorageNameDDL>();
                while (dr.Read())
                {
                    ColdStorageNameDDL objColdStorage = new ColdStorageNameDDL();
                    objColdStorage.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                    objColdStorage.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                    lstColdStorage.Add(objColdStorage);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstColdStorage;
            }
        }

        //StockReport list 01-03-2023
        public List<StockReportResponseList> GetAllColdStorage_StockReportList(long ProductID, long ColdStorageID, DateTime? ToDate)
        {
            int sumNoofBags = 0;
            decimal sumTotalWeight = 0;
            decimal sumTotalAmount = 0;

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllColdStorage_StockReportList_New";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@ColdStorageID", ColdStorageID);
                //cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<StockReportResponseList> objlst = new List<StockReportResponseList>();
                while (dr.Read())
                {
                    StockReportResponseList obj = new StockReportResponseList();
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.UsedQuantity = objBaseSqlManager.GetInt32(dr, "UsedQuantity");
                    obj.RemNoofBags = (obj.NoofBags - obj.UsedQuantity);

                    if (obj.RemNoofBags != 0)
                    {
                        obj.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                        obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                        obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                        obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");

                        //08-07-2022
                        DateTime Date = objBaseSqlManager.GetDateTime(dr, "Date");
                        if (Date == Convert.ToDateTime("10/10/2014"))
                        {
                            obj.Date = "";
                        }
                        else
                        {
                            obj.Date = objBaseSqlManager.GetDateTime(dr, "Date").ToString("dd/MM/yyyy");
                        }


                        DateTime ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate");
                        if (ExpiryDate == Convert.ToDateTime("10/10/2014"))
                        {
                            obj.ExpiryDate = "";
                        }
                        else
                        {
                            obj.ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate").ToString("dd/MM/yyyy");
                        }

                        obj.LotNo = objBaseSqlManager.GetTextValue(dr, "LotNo");


                        obj.WeightPerBag = Math.Round(objBaseSqlManager.GetDecimal(dr, "WeightPerBag"), 2);
                        obj.TotalWeight = (obj.RemNoofBags * obj.WeightPerBag);
                        obj.RatePerKG = Math.Round(objBaseSqlManager.GetDecimal(dr, "RatePerKG"), 2);
                        obj.TotalAmount = Math.Round((obj.TotalWeight * obj.RatePerKG), 2);
                        sumNoofBags = sumNoofBags + Convert.ToInt32(obj.RemNoofBags);
                        sumTotalWeight = sumTotalWeight + Convert.ToDecimal(obj.TotalWeight);
                        sumTotalAmount = sumTotalAmount + Convert.ToDecimal(obj.TotalAmount);
                        objlst.Add(obj);
                    }

                }

                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].sumNoofBags = sumNoofBags;
                    objlst[i].sumTotalWeight = sumTotalWeight;
                    objlst[i].sumTotalAmount = sumTotalAmount;
                }

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        //StockReport list 01-03-2023
        public List<StockReportResponseList> MonthAgoExpiryDateWiseGetColdStorage_StockReportList()
        {
            int sumNoofBags = 0;
            decimal sumTotalWeight = 0;
            decimal sumTotalAmount = 0;

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "MonthAgoExpiryDateWiseGetColdStorage_StockReportList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<StockReportResponseList> objlst = new List<StockReportResponseList>();
                while (dr.Read())
                {
                    StockReportResponseList obj = new StockReportResponseList();
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.UsedQuantity = objBaseSqlManager.GetInt32(dr, "UsedQuantity");
                    obj.RemNoofBags = (obj.NoofBags - obj.UsedQuantity);

                    if (obj.RemNoofBags != 0)
                    {
                        obj.ColdStorageID = objBaseSqlManager.GetInt64(dr, "ColdStorageID");
                        obj.Name = objBaseSqlManager.GetTextValue(dr, "Name");
                        obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                        obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");

                        //08-07-2022
                        DateTime Date = objBaseSqlManager.GetDateTime(dr, "Date");
                        if (Date == Convert.ToDateTime("10/10/2014"))
                        {
                            obj.Date = "";
                        }
                        else
                        {
                            obj.Date = objBaseSqlManager.GetDateTime(dr, "Date").ToString("dd/MM/yyyy");
                        }


                        DateTime ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate");
                        if (ExpiryDate == Convert.ToDateTime("10/10/2014"))
                        {
                            obj.ExpiryDate = "";
                        }
                        else
                        {
                            obj.ExpiryDate = objBaseSqlManager.GetDateTime(dr, "ExpiryDate").ToString("dd/MM/yyyy");
                        }

                        obj.LotNo = objBaseSqlManager.GetTextValue(dr, "LotNo");


                        obj.WeightPerBag = Math.Round(objBaseSqlManager.GetDecimal(dr, "WeightPerBag"), 2);
                        obj.TotalWeight = (obj.RemNoofBags * obj.WeightPerBag);
                        obj.RatePerKG = Math.Round(objBaseSqlManager.GetDecimal(dr, "RatePerKG"), 2);
                        obj.TotalAmount = Math.Round((obj.TotalWeight * obj.RatePerKG), 2);
                        sumNoofBags = sumNoofBags + Convert.ToInt32(obj.RemNoofBags);
                        sumTotalWeight = sumTotalWeight + Convert.ToDecimal(obj.TotalWeight);
                        sumTotalAmount = sumTotalAmount + Convert.ToDecimal(obj.TotalAmount);
                        objlst.Add(obj);
                    }

                }

                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].sumNoofBags = sumNoofBags;
                    objlst[i].sumTotalWeight = sumTotalWeight;
                    objlst[i].sumTotalAmount = sumTotalAmount;
                }

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

    }
}
