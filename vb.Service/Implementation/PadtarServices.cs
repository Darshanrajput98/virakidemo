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
    public class PadtarServices : IPadtarService
    {

        //Whole Material
        public List<GetProductDetailForWholeMaterial> GetAutoCompleteProductDetailsForWholeMaterial(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteProductDetailsForWholeMaterial";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetProductDetailForWholeMaterial> objlst = new List<GetProductDetailForWholeMaterial>();
                while (dr.Read())
                {
                    GetProductDetailForWholeMaterial obj = new GetProductDetailForWholeMaterial();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.GST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long ManageWholeMaterial(WholeMaterial_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.MaterialID == 0)
                {
                    context.WholeMaterial_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.MaterialID > 0)
                {
                    return Obj.MaterialID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<WholeMaterialListResponse> GetAllWholeMaterialList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllWholeMaterialList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<WholeMaterialListResponse> objlst = new List<WholeMaterialListResponse>();
                while (dr.Read())
                {
                    WholeMaterialListResponse obj = new WholeMaterialListResponse();
                    obj.MaterialID = objBaseSqlManager.GetInt64(dr, "MaterialID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    obj.Category = objBaseSqlManager.GetTextValue(dr, "Category");
                    obj.GST = objBaseSqlManager.GetDecimal(dr, "GST");
                    obj.CurrentRate = objBaseSqlManager.GetDecimal(dr, "CurrentRate");
                    obj.Notes1 = objBaseSqlManager.GetTextValue(dr, "Notes1");
                    obj.Notes2 = objBaseSqlManager.GetTextValue(dr, "Notes2");
                    obj.APMC = objBaseSqlManager.GetDecimal(dr, "APMC");
                    obj.APMCAmount = objBaseSqlManager.GetDecimal(dr, "APMCAmount");
                    obj.MarketFinal = objBaseSqlManager.GetDecimal(dr, "MarketFinal");
                    obj.GrossRate = objBaseSqlManager.GetDecimal(dr, "GrossRate");
                    obj.KG_P_Hour = objBaseSqlManager.GetDecimal(dr, "KG_P_Hour");
                    obj.Labour_P_Hour = objBaseSqlManager.GetDecimal(dr, "Labour_P_Hour");
                    obj.Gasara = objBaseSqlManager.GetDecimal(dr, "Gasara");
                    obj.GasaraAmount = objBaseSqlManager.GetDecimal(dr, "GasaraAmount");
                    obj.SellRateWholesale = objBaseSqlManager.GetDecimal(dr, "SellRateWholesale");
                    obj.SellRateRetail = objBaseSqlManager.GetDecimal(dr, "SellRateRetail");
                    obj.MarginWholesale = objBaseSqlManager.GetDecimal(dr, "MarginWholesale");
                    obj.MarginRetail = objBaseSqlManager.GetDecimal(dr, "MarginRetail");
                    obj.Discount = objBaseSqlManager.GetDecimal(dr, "Discount");
                    obj.DiscountAmount = objBaseSqlManager.GetDecimal(dr, "DiscountAmount");
                    obj.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    obj.PackingCharge = objBaseSqlManager.GetDecimal(dr, "PackingCharge");
                    obj.Freight_P_KG = objBaseSqlManager.GetDecimal(dr, "Freight_P_KG");
                    obj.Commision_P_KG = objBaseSqlManager.GetDecimal(dr, "Commision_P_KG");
                    obj.LabourAmount_P_KG = objBaseSqlManager.GetDecimal(dr, "LabourAmount_P_KG");
                    obj.Padtar = objBaseSqlManager.GetDecimal(dr, "Padtar");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
                    obj.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteWholeMaterial(long MaterialID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteWholeMaterial";
                cmdGet.Parameters.AddWithValue("@MaterialID", MaterialID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdateMaterial(List<WholeMaterialListResponse> data)
        {
            foreach (var item in data)
            {
                try
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateWholeMaterial";
                        cmdGet.Parameters.AddWithValue("@MaterialID", item.MaterialID);
                        cmdGet.Parameters.AddWithValue("@ProductID", item.ProductID);
                        cmdGet.Parameters.AddWithValue("@GST", item.GST);
                        cmdGet.Parameters.AddWithValue("@CurrentRate", item.CurrentRate);
                        cmdGet.Parameters.AddWithValue("@Notes1", item.Notes1);
                        cmdGet.Parameters.AddWithValue("@Notes2", item.Notes2);
                        cmdGet.Parameters.AddWithValue("@PackingCharge", item.PackingCharge);
                        cmdGet.Parameters.AddWithValue("@NetAmount", item.NetAmount);
                        cmdGet.Parameters.AddWithValue("@Discount", item.Discount);
                        cmdGet.Parameters.AddWithValue("@DiscountAmount", item.DiscountAmount);
                        cmdGet.Parameters.AddWithValue("@APMC", item.APMC);
                        cmdGet.Parameters.AddWithValue("@APMCAmount", item.APMCAmount);
                        cmdGet.Parameters.AddWithValue("@MarketFinal", item.MarketFinal);
                        cmdGet.Parameters.AddWithValue("@GrossRate", item.GrossRate);
                        cmdGet.Parameters.AddWithValue("@KG_P_Hour", item.KG_P_Hour);
                        cmdGet.Parameters.AddWithValue("@Labour_P_Hour", item.Labour_P_Hour);
                        cmdGet.Parameters.AddWithValue("@Gasara", item.Gasara);
                        cmdGet.Parameters.AddWithValue("@GasaraAmount", item.GasaraAmount);
                        cmdGet.Parameters.AddWithValue("@SellRateWholesale", item.SellRateWholesale);
                        cmdGet.Parameters.AddWithValue("@MarginWholesale", item.MarginWholesale);
                        cmdGet.Parameters.AddWithValue("@SellRateRetail", item.SellRateRetail);
                        cmdGet.Parameters.AddWithValue("@MarginRetail", item.MarginRetail);
                        cmdGet.Parameters.AddWithValue("@Freight_P_KG", item.Freight_P_KG);
                        cmdGet.Parameters.AddWithValue("@Commision_P_KG", item.Commision_P_KG);
                        cmdGet.Parameters.AddWithValue("@LabourAmount_P_KG", item.LabourAmount_P_KG);
                        cmdGet.Parameters.AddWithValue("@Padtar", item.Padtar);
                        object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

        
        //Powder Spices
        public List<GetDetailForPowderSpices> GetAutoCompleteDetailsForPowderSpices(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteDetailsForPowderSpices";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetDetailForPowderSpices> objlst = new List<GetDetailForPowderSpices>();
                while (dr.Read())
                {
                    GetDetailForPowderSpices obj = new GetDetailForPowderSpices();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.GST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.GrossRate = objBaseSqlManager.GetDecimal(dr, "GrossRate");
                    obj.Padtar = objBaseSqlManager.GetDecimal(dr, "Padtar");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long ManagePowderSpices(PowderSpices_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.SpicesID == 0)
                {
                    context.PowderSpices_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.SpicesID > 0)
                {
                    return Obj.SpicesID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PowderSpicesListResponse> GetAllPowderSpicesList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPowderSpicesList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PowderSpicesListResponse> objlst = new List<PowderSpicesListResponse>();
                while (dr.Read())
                {
                    PowderSpicesListResponse obj = new PowderSpicesListResponse();
                    obj.SpicesID = objBaseSqlManager.GetInt64(dr, "SpicesID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.GST = objBaseSqlManager.GetDecimal(dr, "GST");
                    obj.CurrentRate = objBaseSqlManager.GetDecimal(dr, "CurrentRate");
                    obj.Notes1 = objBaseSqlManager.GetTextValue(dr, "Notes1");
                    obj.Notes2 = objBaseSqlManager.GetTextValue(dr, "Notes2");
                    obj.GrindingCharge = objBaseSqlManager.GetDecimal(dr, "GrindingCharge");
                    obj.Gasara = objBaseSqlManager.GetDecimal(dr, "Gasara");
                    obj.GasaraAmount = objBaseSqlManager.GetDecimal(dr, "GasaraAmount");
                    obj.GrossRate = objBaseSqlManager.GetDecimal(dr, "GrossRate");
                    obj.Padtar = objBaseSqlManager.GetDecimal(dr, "Padtar");
                    obj.SellRateWholesale = objBaseSqlManager.GetDecimal(dr, "SellRateWholesale");
                    obj.SellRateRetail = objBaseSqlManager.GetDecimal(dr, "SellRateRetail");
                    obj.MarginWholesale = objBaseSqlManager.GetDecimal(dr, "MarginWholesale");
                    obj.MarginRetail = objBaseSqlManager.GetDecimal(dr, "MarginRetail");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
                    obj.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateSpices(List<PowderSpicesListResponse> data)
        {
            foreach (var item in data)
            {
                try
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdatePowderSpices";
                        cmdGet.Parameters.AddWithValue("@SpicesID", item.SpicesID);
                        cmdGet.Parameters.AddWithValue("@ProductID", item.ProductID);
                        cmdGet.Parameters.AddWithValue("@GST", item.GST);
                        cmdGet.Parameters.AddWithValue("@CurrentRate", item.CurrentRate);
                        cmdGet.Parameters.AddWithValue("@Notes1", item.Notes1);
                        cmdGet.Parameters.AddWithValue("@Notes2", item.Notes2);
                        cmdGet.Parameters.AddWithValue("@GrossRate", item.GrossRate);
                        cmdGet.Parameters.AddWithValue("@Gasara", item.Gasara);
                        cmdGet.Parameters.AddWithValue("@GrindingCharge", item.GrindingCharge);
                        cmdGet.Parameters.AddWithValue("@GasaraAmount", item.GasaraAmount);
                        cmdGet.Parameters.AddWithValue("@SellRateWholesale", item.SellRateWholesale);
                        cmdGet.Parameters.AddWithValue("@MarginWholesale", item.MarginWholesale);
                        cmdGet.Parameters.AddWithValue("@SellRateRetail", item.SellRateRetail);
                        cmdGet.Parameters.AddWithValue("@MarginRetail", item.MarginRetail);
                        cmdGet.Parameters.AddWithValue("@Padtar", item.Padtar);
                        object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

        public bool DeletePowderSpices(long SpicesID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePowderSpices";
                cmdGet.Parameters.AddWithValue("@SpicesID", SpicesID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }


        //Premix 
        public string AddPremix(PremixRequest data)
        {
            if (!string.IsNullOrEmpty(data.DeleteItems))
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeletePremixQtyItems";
                    cmdGet.Parameters.AddWithValue("@PremixQtyID", data.DeleteItems);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }

            using (VirakiEntities context = new VirakiEntities())
            {
                long PremixID = 0;
                string Message = "";
                PreMix_Mst obj = new PreMix_Mst();
                obj.PremixID = data.PremixID;
                obj.RatePerKG = data.RatePerKG;
                obj.Notes1 = data.Notes1;
                obj.Notes2 = data.Notes2;
                obj.Gasara = data.Gasara;
                obj.GasaraAmount = data.GasaraAmount;
                obj.GrindingCharge = data.GrindingCharge;
                obj.PackingCharge = data.PackingCharge;
                obj.MakingCharge = data.MakingCharge;
                obj.Padtar = data.Padtar;
                obj.SellRateWholesale = data.SellRateWholesale;
                obj.MarginWholesale = data.MarginWholesale;
                obj.SellRateRetail = data.SellRateRetail;
                obj.MarginRetail = data.MarginRetail;
                obj.CreatedBy = data.CreatedBy;
                obj.CreatedOn = data.CreatedOn;
                obj.UpdatedBy = data.UpdatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                obj.IsDelete = false;

                if (obj.PremixID == 0)
                {
                    if (PremixID == 0)
                    {
                        context.PreMix_Mst.Add(obj);
                        context.SaveChanges();
                        Message = "Insert Sucessfully";
                    }
                    else
                    {
                        Message = "Already Exist";
                    }
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    Message = "Updated Sucessfully";
                }

                if (obj.PremixID > 0)
                {
                    try
                    {
                        foreach (var item in data.lstPremix)
                        {
                            PreMixQty_Mst objPreMixQty = new PreMixQty_Mst();
                            objPreMixQty.PremixQtyID = item.PremixQtyID;
                            objPreMixQty.PremixID = obj.PremixID;
                            objPreMixQty.ProductID = item.ProductID;
                            objPreMixQty.CurrentRate = item.CurrentRate;
                            objPreMixQty.Quantity = item.Quantity;
                            objPreMixQty.Amount = item.Amount;
                            objPreMixQty.CreatedBy = data.CreatedBy;
                            objPreMixQty.CreatedOn = data.CreatedOn;
                            objPreMixQty.UpdatedBy = data.UpdatedBy;
                            objPreMixQty.UpdatedOn = data.UpdatedOn;
                            objPreMixQty.IsDelete = false;

                            if (objPreMixQty.PremixQtyID == 0)
                            {
                                context.PreMixQty_Mst.Add(objPreMixQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objPreMixQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }

                        }
                        return Message;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            PreMix_Mst data1 = context.PreMix_Mst.Where(x => x.PremixID == obj.PremixID).FirstOrDefault();
                            if (data != null)
                            {
                                context.PreMix_Mst.Remove(data1);
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
                    if (PremixID > 0)
                    {
                        Message = "Premix Already Exist";
                    }
                    else
                    {
                        Message = "Error";
                    }
                    return Message;
                }

            }
        }

        public List<PremixListResponse> GetAllPremixList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPremixList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PremixListResponse> objlst = new List<PremixListResponse>();
                while (dr.Read())
                {
                    PremixListResponse obj = new PremixListResponse();
                    obj.PremixID = objBaseSqlManager.GetInt64(dr, "PremixID");
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");
                    obj.Notes1 = objBaseSqlManager.GetTextValue(dr, "Notes1");
                    obj.Notes2 = objBaseSqlManager.GetTextValue(dr, "Notes2");
                    obj.Gasara = objBaseSqlManager.GetDecimal(dr, "Gasara");
                    obj.GasaraAmount = objBaseSqlManager.GetDecimal(dr, "GasaraAmount");
                    obj.GrindingCharge = objBaseSqlManager.GetDecimal(dr, "GrindingCharge");
                    obj.PackingCharge = objBaseSqlManager.GetDecimal(dr, "PackingCharge");
                    obj.MakingCharge = objBaseSqlManager.GetDecimal(dr, "MakingCharge");
                    obj.Padtar = Math.Round(objBaseSqlManager.GetDecimal(dr, "Padtar"), 2);
                    obj.SellRateWholesale = Math.Round(objBaseSqlManager.GetDecimal(dr, "SellRateWholesale"), 2);
                    obj.MarginWholesale = Math.Round(objBaseSqlManager.GetDecimal(dr, "MarginWholesale"), 2);
                    obj.SellRateRetail = Math.Round(objBaseSqlManager.GetDecimal(dr, "SellRateRetail"), 2);
                    obj.MarginRetail = Math.Round(objBaseSqlManager.GetDecimal(dr, "MarginRetail"), 2);
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

        public List<PremixItemRequest> GetPremixDetailsByPremixID(long PremixID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPremixDetailsByPremixID";
                cmdGet.Parameters.AddWithValue("@PremixID", PremixID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PremixItemRequest> objlst = new List<PremixItemRequest>();
                while (dr.Read())
                {
                    PremixItemRequest objPremixQty = new PremixItemRequest();
                    objPremixQty.PremixQtyID = objBaseSqlManager.GetInt64(dr, "PremixQtyID");
                    objPremixQty.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objPremixQty.CurrentRate = objBaseSqlManager.GetDecimal(dr, "CurrentRate");
                    objPremixQty.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPremixQty.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(objPremixQty);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeletePremix(long PremixID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePremix";
                cmdGet.Parameters.AddWithValue("@PremixID", PremixID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdatePremix(List<PremixListResponse> data)
        {
            foreach (var item in data)
            {
                try
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdatePremix";
                        cmdGet.Parameters.AddWithValue("@PremixID", item.PremixID);
                        cmdGet.Parameters.AddWithValue("@RatePerKG", item.RatePerKG);
                        cmdGet.Parameters.AddWithValue("@Notes1", item.Notes1);
                        cmdGet.Parameters.AddWithValue("@Notes2", item.Notes2);
                        cmdGet.Parameters.AddWithValue("@GrindingCharge", item.GrindingCharge);
                        cmdGet.Parameters.AddWithValue("@Gasara", item.Gasara);
                        cmdGet.Parameters.AddWithValue("@GasaraAmount", item.GasaraAmount);
                        cmdGet.Parameters.AddWithValue("@PackingCharge", item.PackingCharge);
                        cmdGet.Parameters.AddWithValue("@MakingCharge", item.MakingCharge);
                        cmdGet.Parameters.AddWithValue("@Padtar", item.Padtar);
                        cmdGet.Parameters.AddWithValue("@SellRateWholesale", item.SellRateWholesale);
                        cmdGet.Parameters.AddWithValue("@MarginWholesale", item.MarginWholesale);
                        cmdGet.Parameters.AddWithValue("@SellRateRetail", item.SellRateRetail);
                        cmdGet.Parameters.AddWithValue("@MarginRetail", item.MarginRetail);
                        object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

    }
}
