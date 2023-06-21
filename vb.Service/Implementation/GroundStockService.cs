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
    public class GroundStockService : IGroundStockService
    {

        public long ManageGroundStock(GroundStock_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.GroundStockID == 0)
                {
                    context.GroundStock_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.GroundStockID > 0)
                {
                    return Obj.GroundStockID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<GroundStockListResponse> GetAllGroundStockList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGroundStockList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GroundStockListResponse> objlst = new List<GroundStockListResponse>();
                while (dr.Read())
                {
                    GroundStockListResponse obj = new GroundStockListResponse();
                    obj.GroundStockID = objBaseSqlManager.GetInt64(dr, "GroundStockID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.GroundStockQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "GroundStockQuantity"), 2);
                    obj.MinGroundStockQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "MinGroundStockQuantity"), 2);
                    obj.GroundStockDescription = objBaseSqlManager.GetTextValue(dr, "GroundStockDescription");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
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

        public bool DeleteGroundStock(long GroundStockID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteGroundStock";
                cmdGet.Parameters.AddWithValue("@GroundStockID", GroundStockID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddInward(GroundStock_Inward_Mst obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.InwardID == 0)
                {
                    context.GroundStock_Inward_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.InwardID > 0)
                {
                    return obj.InwardID;
                }
                else
                {
                    return 0;
                }
            }
        }


        //change in GetAllGroundStockInwardList
        public List<GroundStockInwardListResponse> GetAllGroundStockInwardList()
        {
            decimal StockQuantity = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGroundStockInwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GroundStockInwardListResponse> objlst = new List<GroundStockInwardListResponse>();
                while (dr.Read())
                {
                    GroundStockInwardListResponse obj = new GroundStockInwardListResponse();
                    obj.PurchaseQtyID = objBaseSqlManager.GetInt64(dr, "PurchaseQtyID");
                    obj.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.OpeningQty = Math.Round(objBaseSqlManager.GetDecimal(dr, "OpeningQty"), 2);
                    StockQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantity"), 2);

                    if (obj.OpeningQty == 0)
                    {
                        obj.OpeningQty = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantity"), 2);
                    }
                    else if (StockQuantity == 0 || StockQuantity != 0)
                    {
                        obj.OpeningQty = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantity"), 2);
                    }

                    obj.NetWeight = Math.Round(objBaseSqlManager.GetDecimal(dr, "PurchaseQty"), 2);
                    obj.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.IsInward = objBaseSqlManager.GetBoolean(dr, "IsInward");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        public bool UpdateIsInwardStatus(long PurchaseQtyID, long PurchaseID, long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateIsInwardStatus";
                cmdGet.Parameters.AddWithValue("@PurchaseQtyID", PurchaseQtyID);
                cmdGet.Parameters.AddWithValue("@PurchaseID", PurchaseID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdateGroundStockQty(long ProductID, decimal ClosingQty)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateGroundStockQty";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@ClosingQty", ClosingQty);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // Ground Stock Transfer

        public List<ProductNameDDL> GetProductNameForDDL()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductNameDDL> objlst = new List<ProductNameDDL>();
                while (dr.Read())
                {
                    ProductNameDDL obj = new ProductNameDDL();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long ManageGroundStockTransfer(GroundStockTransfer_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.GroundStockTransferID == 0)
                {
                    context.GroundStockTransfer_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.GroundStockTransferID > 0)
                {
                    return Obj.GroundStockTransferID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<GroundStockTransferListResponse> GetAllGroundStockTransferList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGroundStockTransferList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GroundStockTransferListResponse> objlst = new List<GroundStockTransferListResponse>();
                while (dr.Read())
                {
                    GroundStockTransferListResponse obj = new GroundStockTransferListResponse();
                    obj.GroundStockTransferID = objBaseSqlManager.GetInt64(dr, "GroundStockTransferID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.StockTransferQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockTransferQuantity"), 2);
                    obj.MinStockTransferQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "MinStockTransferQuantity"), 2);
                    obj.StockTransferDescription = objBaseSqlManager.GetTextValue(dr, "StockTransferDescription");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
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

        public bool DeleteGroundStockTransfer(long GroundStockTransferID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteGroundStockTransfer";
                cmdGet.Parameters.AddWithValue("@GroundStockTransferID", GroundStockTransferID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // Ground Stock Transfer Inward

        public List<GroundStockTransferInwardResponse> GetAllGroundStockTransferInwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGroundStockTransferInwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GroundStockTransferInwardResponse> objlst = new List<GroundStockTransferInwardResponse>();
                while (dr.Read())
                {
                    GroundStockTransferInwardResponse obj = new GroundStockTransferInwardResponse();
                    obj.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    obj.ChallanDate = objBaseSqlManager.GetDateTime(dr, "ChallanDate");
                    if (obj.ChallanDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ChallanDatestr = "";
                    }
                    else
                    {
                        obj.ChallanDatestr = objBaseSqlManager.GetDateTime(dr, "ChallanDate").ToString("dd-MM-yyyy");
                    }
                    obj.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    obj.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");
                    obj.FinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "FinalTotal"), 2);
                    obj.TotalItem = objBaseSqlManager.GetInt32(dr, "TotalItem");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.IsInward = objBaseSqlManager.GetBoolean(dr, "IsInward");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddStockTransferInward(GroundStockTransfer_Inward_Mst obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.TransferInwardID == 0)
                {
                    context.GroundStockTransfer_Inward_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.TransferInwardID > 0)
                {
                    return obj.TransferInwardID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool UpdateStockTransferIsInwardStatus(long ChallanQtyID, long ChallanID, long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateStockTransferIsInwardStatus";
                cmdGet.Parameters.AddWithValue("@ChallanQtyID", ChallanQtyID);
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 26-07-2022
        public bool UpdateGroundStockTransferQty(long ProductID, long GodownIDTo, decimal ClosingQty, long GodownIDFrom, decimal DeductQty)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateGroundStockTransferQty";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@GodownIDTo", GodownIDTo);
                cmdGet.Parameters.AddWithValue("@ClosingQty", ClosingQty);
                cmdGet.Parameters.AddWithValue("@GodownIDFrom", GodownIDFrom);
                cmdGet.Parameters.AddWithValue("@DeductQty", DeductQty);

                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 26-07-2022
        public List<GroundStockTransferInwardListResponse> GetGroundStockTransferInwardData(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGroundStockTransferInwardData";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GroundStockTransferInwardListResponse> objlst = new List<GroundStockTransferInwardListResponse>();
                while (dr.Read())
                {
                    GroundStockTransferInwardListResponse obj = new GroundStockTransferInwardListResponse();
                    obj.ChallanQtyID = objBaseSqlManager.GetInt64(dr, "ChallanQtyID");
                    obj.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");

                    obj.GodownIDFrom = objBaseSqlManager.GetInt64(dr, "GodownIDFrom");
                    obj.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");

                    obj.GodownIDTo = objBaseSqlManager.GetInt64(dr, "GodownIDTo");
                    obj.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");

                    obj.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "PurchaseQty"), 2);

                    obj.OpeningQty = Math.Round(objBaseSqlManager.GetDecimal(dr, "OpeningQty"), 2);
                    if (obj.OpeningQty == 0)
                    {
                        obj.OpeningQty = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantity"), 2);
                    }

                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.IsInward = objBaseSqlManager.GetBoolean(dr, "IsInward");
                    obj.StockQuantityFrom = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantityFrom"), 2);

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 26-07-2022
        public long GetExistProductGroundStock(long ProductID)
        {
            AddGroundStock obj = new AddGroundStock();
            SqlCommand cmdGet = new SqlCommand();

            long GroundStockID = 0;


            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExistProductGroundStock";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    GroundStockID = objBaseSqlManager.GetInt64(dr, "ProductID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return GroundStockID;
            }
        }

        // 26-07-2022
        public long GetExistProductGroundStockTransfer(long ProductID, long GodownID)
        {
            AddGroundStockTransfer obj = new AddGroundStockTransfer();
            SqlCommand cmdGet = new SqlCommand();
            long GroundStockTransferID = 0;
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExistProductGroundStockTransfer";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    GroundStockTransferID = objBaseSqlManager.GetInt64(dr, "GroundStockTransferID");

                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return GroundStockTransferID;
            }
        }





        //Outward
        public List<GroundStockOutwardListResponse> GetAllGroundStockOutwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGroundStockOutwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GroundStockOutwardListResponse> objlst = new List<GroundStockOutwardListResponse>();
                while (dr.Read())
                {

                    GroundStockOutwardListResponse obj = new GroundStockOutwardListResponse();
                    obj.OpeningQty = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantity"), 2);
                    if (obj.OpeningQty != 0)
                    {
                        obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                        obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                        obj.NetWeight = Math.Round(objBaseSqlManager.GetDecimal(dr, "StockQuantity"), 2);
                        obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                        objlst.Add(obj);
                    }

                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddOutward(GroundStock_Outward_Mst obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.OutwardID == 0)
                {
                    context.GroundStock_Outward_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.OutwardID > 0)
                {
                    return obj.OutwardID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool UpdateGroundStockOutwardQty(long ProductID, decimal ClosingQty)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateGroundStockOutwardQty";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@ClosingQty", ClosingQty);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

    }
}
