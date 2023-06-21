using vb.Data;
using vb.Data.Model;
using System.Data.Entity;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using vb.Data.ViewModel;
using System.Linq;
using vb.Service.Common;

namespace vb.Service
{
    public class ProductService : IProductService
    {
        public bool AddProductCategory(Category_Mst ObjCategory)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjCategory.CategoryID == 0)
                {
                    context.Category_Mst.Add(ObjCategory);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjCategory).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjCategory.CategoryID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<ProductQtyViewModel> GetAllProductQtyListByProductID(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllProductQtyListByProductID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductQtyViewModel> objlst = new List<ProductQtyViewModel>();
                while (dr.Read())
                {
                    ProductQtyViewModel objCategory = new ProductQtyViewModel();
                    objCategory.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objCategory.LowerQty = objBaseSqlManager.GetInt64(dr, "LowerQty");
                    objCategory.UpperQty = objBaseSqlManager.GetInt64(dr, "UpperQty");
                    objCategory.SellPrice = objBaseSqlManager.GetDecimal(dr, "SellPrice");
                    objCategory.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ProductCategoryListResponse> GetAllProductCategoryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllProductCategoryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductCategoryListResponse> objlst = new List<ProductCategoryListResponse>();
                while (dr.Read())
                {
                    ProductCategoryListResponse objCategory = new ProductCategoryListResponse();
                    objCategory.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objCategory.CategoryCode = objBaseSqlManager.GetTextValue(dr, "CategoryCode");
                    objCategory.CategoryTypeID = objBaseSqlManager.GetInt32(dr, "CategoryTypeID");
                    objCategory.CategoryTypestr = new Utility1().GetTextEnum(objCategory.CategoryTypeID);
                    objCategory.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objCategory.CategoryDescription = objBaseSqlManager.GetTextValue(dr, "CategoryDescription");
                    objCategory.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteProductCategory(long CategoryID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteProductCategory";
                cmdGet.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<ProductCategoryListResponse> GetAllCategoryName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCategoryName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductCategoryListResponse> lstCategory = new List<ProductCategoryListResponse>();
                while (dr.Read())
                {
                    ProductCategoryListResponse objCategory = new ProductCategoryListResponse();
                    objCategory.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objCategory.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    lstCategory.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCategory;
            }
        }

        public List<GodownListResponse> GetAllGodownName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGodownName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GodownListResponse> lstGodown = new List<GodownListResponse>();
                while (dr.Read())
                {
                    GodownListResponse objGodown = new GodownListResponse();
                    objGodown.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objGodown.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        public List<UnitListResponse> GetAllUnitName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUnitName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UnitListResponse> lstUnit = new List<UnitListResponse>();
                while (dr.Read())
                {
                    UnitListResponse objUnit = new UnitListResponse();
                    objUnit.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objUnit.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    lstUnit.Add(objUnit);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstUnit;
            }
        }

        public bool AddProduct(ProductViewModel data)
        {
            if (!string.IsNullOrEmpty(data.DeleteItems))
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteProductQtyItems";
                    cmdGet.Parameters.AddWithValue("@ProductQtyID", data.DeleteItems);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }

            if (!string.IsNullOrEmpty(data.DeleteOnlineItem))
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteOnlineProductQtyItems";
                    cmdGet.Parameters.AddWithValue("@OnlineProductQtyID", data.DeleteOnlineItem);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }

            Product_Mst obj = new Product_Mst();
            obj.ProductID = data.ProductID;
            obj.ProductName = data.ProductName;
            obj.ProductAlternateName = data.ProductAlternateName;
            obj.CategoryID = data.CategoryID;
            obj.GodownID = data.GodownID;
            obj.ProductPrice = data.ProductPrice;
            obj.UnitID = data.UnitID;
            obj.PouchNameID = data.PouchNameID;
            obj.ProductDescription = data.ProductDescription;
            obj.HSNNumber = data.HSNNumber;
            obj.SGST = data.SGST;
            obj.CGST = data.CGST;
            obj.IGST = data.IGST;
            obj.HFor = data.HFor;
            obj.IsDelete = false;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            
            //19-07-2022
            obj.SlabForGST = data.SlabForGST;
            
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.ProductID == 0)
                {
                    context.Product_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.ProductID > 0)
                {
                    if (data.lstProductQty != null)
                    {
                        foreach (var item in data.lstProductQty)
                        {
                            ProductQty_Mst objProductQty = new ProductQty_Mst();
                            objProductQty.ProductID = obj.ProductID;
                            objProductQty.ProductQtyID = item.ProductQtyID;
                            objProductQty.LowerQty = item.LowerQty;
                            objProductQty.UpperQty = item.UpperQty;
                            objProductQty.LessAmount = item.LessAmount;
                            objProductQty.SellPrice = item.SellPrice;
                            objProductQty.CreatedBy = data.CreatedBy;
                            objProductQty.CreatedOn = data.CreatedOn;
                            objProductQty.UpdatedBy = data.UpdatedBy;
                            objProductQty.UpdatedOn = data.UpdatedOn;
                            if (objProductQty.ProductQtyID == 0)
                            {
                                context.ProductQty_Mst.Add(objProductQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objProductQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }

                    if (data.lstOnlineProductQty != null)
                    {
                        foreach (var item in data.lstOnlineProductQty)
                        {
                            OnlineProductQty_Mst objOnlineProductQty = new OnlineProductQty_Mst();
                            objOnlineProductQty.ProductID = obj.ProductID;
                            objOnlineProductQty.OnlineProductQtyID = item.OnlineProductQtyID;
                            objOnlineProductQty.OnlineProductPrice = item.OnlineProductPrice;
                            objOnlineProductQty.OnlineQty = item.OnlineQty;
                            objOnlineProductQty.OnlineUnitID = item.UnitID;
                            objOnlineProductQty.Factoring = item.Factoring;
                            objOnlineProductQty.FactoringAmount = item.FactoringAmount;
                            objOnlineProductQty.PremiumPercentage = item.PremiumPercentage;
                            objOnlineProductQty.PremiumAmount = item.PremiumPercentageAmt;
                            objOnlineProductQty.OnlineTotalAmount = item.TotalOnlineAmount;
                            objOnlineProductQty.IsOnline = item.IsOnline;
                            objOnlineProductQty.CreatedBy = data.CreatedBy;
                            objOnlineProductQty.CreatedOn = data.CreatedOn;
                            objOnlineProductQty.UpdatedBy = data.UpdatedBy;
                            objOnlineProductQty.UpdatedOn = data.UpdatedOn;
                            objOnlineProductQty.IsDelete = false;
                            if (objOnlineProductQty.OnlineProductQtyID == 0)
                            {
                                context.OnlineProductQty_Mst.Add(objOnlineProductQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objOnlineProductQty).State = EntityState.Modified;
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

        public List<ProductListResponse> GetAllProductList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllProductList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductListResponse> objlst = new List<ProductListResponse>();
                while (dr.Read())
                {
                    ProductListResponse objProduct = new ProductListResponse();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objProduct.ProductAlternateName = objBaseSqlManager.GetTextValue(dr, "ProductAlternateName");
                    objProduct.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objProduct.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objProduct.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objProduct.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objProduct.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objProduct.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objProduct.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    objProduct.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objProduct.ProductDescription = objBaseSqlManager.GetTextValue(dr, "ProductDescription");
                    objProduct.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objProduct.SGST = objBaseSqlManager.GetDecimal(dr, "SGST");
                    objProduct.CGST = objBaseSqlManager.GetDecimal(dr, "CGST");
                    objProduct.IGST = objBaseSqlManager.GetDecimal(dr, "IGST");
                    objProduct.HFor = objBaseSqlManager.GetDecimal(dr, "HFor");
                    objProduct.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");

                    //19-07-2022
                    objProduct.SlabForGST = objBaseSqlManager.GetDecimal(dr, "SlabForGST");

                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteProduct(long ProductID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteProduct";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdateProduct(List<ProductViewModel> data)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    try
                    {
                        SqlCommand cmdGet = new SqlCommand();
                        using (var objBaseSqlManager = new BaseSqlManager())
                        {
                            cmdGet.CommandType = CommandType.StoredProcedure;
                            cmdGet.CommandText = "UpdateProduct";
                            cmdGet.Parameters.AddWithValue("@ProductID", item.ProductID);
                            cmdGet.Parameters.AddWithValue("@ProductPrice", item.ProductPrice);
                            cmdGet.Parameters.AddWithValue("@SGST", item.SGST);
                            cmdGet.Parameters.AddWithValue("@CGST", item.CGST);
                            cmdGet.Parameters.AddWithValue("@IGST", item.IGST);
                            cmdGet.Parameters.AddWithValue("@HFor", item.HFor);
                            object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                        SqlCommand cmd = new SqlCommand();
                        using (var objBaseSqlManager2 = new BaseSqlManager())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "UpdateProductQtySalesPrice";
                            cmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                            cmd.Parameters.AddWithValue("@ProductPrice", item.ProductPrice);
                            object dr2 = objBaseSqlManager2.ExecuteNonQuery(cmd);
                            objBaseSqlManager2.ForceCloseConnection();
                        }

                        SqlCommand cmdUpdate = new SqlCommand();
                        using (var objBaseSqlManager3 = new BaseSqlManager())
                        {
                            cmdUpdate.CommandType = CommandType.StoredProcedure;
                            cmdUpdate.CommandText = "UpdateOnlineProductQtyByProductID";
                            cmdUpdate.Parameters.AddWithValue("@ProductID", item.ProductID);
                            cmdUpdate.Parameters.AddWithValue("@ProductPrice", item.ProductPrice);
                            object dr3 = objBaseSqlManager3.ExecuteNonQuery(cmdUpdate);
                            objBaseSqlManager3.ForceCloseConnection();
                        }
                    }
                    catch
                    {

                    }
                }
                return true;
            }
        }

        public List<ProductListResponse> GetAllProductListSearchByPrice(int ProductPrice)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllProductListSearchByPrice";
                cmdGet.Parameters.AddWithValue("@ProductPrice", ProductPrice);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductListResponse> objlst = new List<ProductListResponse>();
                while (dr.Read())
                {
                    ProductListResponse objProduct = new ProductListResponse();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objProduct.ProductAlternateName = objBaseSqlManager.GetTextValue(dr, "ProductAlternateName");
                    objProduct.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objProduct.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objProduct.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objProduct.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objProduct.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objProduct.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objProduct.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objProduct.ProductDescription = objBaseSqlManager.GetTextValue(dr, "ProductDescription");
                    objProduct.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objProduct.SGST = objBaseSqlManager.GetDecimal(dr, "SGST");
                    objProduct.CGST = objBaseSqlManager.GetDecimal(dr, "CGST");
                    objProduct.IGST = objBaseSqlManager.GetDecimal(dr, "IGST");
                    objProduct.HFor = objBaseSqlManager.GetDecimal(dr, "HFor");
                    objProduct.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");

                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //public List<PouchListResponse> GetAllPouchName()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetAllPouchName";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<PouchListResponse> lstCategory = new List<PouchListResponse>();
        //    while (dr.Read())
        //    {
        //        PouchListResponse objCategory = new PouchListResponse();
        //        objCategory.PouchID = objBaseSqlManager.GetInt64(dr, "PouchID");
        //        objCategory.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
        //        lstCategory.Add(objCategory);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return lstCategory;
        //}


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

        public long AddPurchaseProduct(AddPurchaseProduct data)
        {
            PurchaseProduct_Mst obj = new PurchaseProduct_Mst();
            obj.ProductID = data.ProductID;
            obj.ProductName = data.ProductName;
            obj.ProductAlternateName = data.ProductAlternateName;
            obj.CategoryID = data.CategoryID;
            obj.ProductDescription = data.ProductDescription;
            obj.HSNNumber = data.HSNNumber;
            obj.SGST = data.SGST;
            obj.CGST = data.CGST;
            obj.IGST = data.IGST;
            obj.HFor = data.HFor;
            obj.IsDelete = false;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.ProductID == 0)
                {
                    context.PurchaseProduct_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.ProductID > 0)
                {
                    return obj.ProductID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PurchaseProductListResponse> GetAllPurchaseProductList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseProductList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseProductListResponse> objlst = new List<PurchaseProductListResponse>();
                while (dr.Read())
                {
                    PurchaseProductListResponse objProduct = new PurchaseProductListResponse();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objProduct.ProductAlternateName = objBaseSqlManager.GetTextValue(dr, "ProductAlternateName");
                    objProduct.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objProduct.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objProduct.ProductDescription = objBaseSqlManager.GetTextValue(dr, "ProductDescription");
                    objProduct.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objProduct.SGST = objBaseSqlManager.GetDecimal(dr, "SGST");
                    objProduct.CGST = objBaseSqlManager.GetDecimal(dr, "CGST");
                    objProduct.IGST = objBaseSqlManager.GetDecimal(dr, "IGST");
                    objProduct.HFor = objBaseSqlManager.GetDecimal(dr, "HFor");
                    objProduct.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objProduct.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objProduct.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeletePurchaseProduct(long ProductID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePurchaseProduct";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdatePurchaseProduct(List<AddPurchaseProduct> data)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    try
                    {
                        SqlCommand cmdGet = new SqlCommand();
                        using (var objBaseSqlManager = new BaseSqlManager())
                        {
                            cmdGet.CommandType = CommandType.StoredProcedure;
                            cmdGet.CommandText = "UpdatePurchaseProduct";
                            cmdGet.Parameters.AddWithValue("@ProductID", item.ProductID);
                            cmdGet.Parameters.AddWithValue("@SGST", item.SGST);
                            cmdGet.Parameters.AddWithValue("@CGST", item.CGST);
                            cmdGet.Parameters.AddWithValue("@IGST", item.IGST);
                            cmdGet.Parameters.AddWithValue("@HFor", item.HFor);
                            object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                    }
                    catch
                    {

                    }
                }
                return true;
            }
        }




        // expense product 18/12/2019
        public long AddExpenseProduct(ExpenseProduct_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ProductID == 0)
                {
                    context.ExpenseProduct_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ProductID > 0)
                {
                    return Obj.ProductID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ExpenseProductListResponse> GetAllExpenseProductList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseProductList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseProductListResponse> objlst = new List<ExpenseProductListResponse>();
                while (dr.Read())
                {
                    ExpenseProductListResponse obj = new ExpenseProductListResponse();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
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

        public bool DeleteExpenseProduct(long ProductID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteExpenseProduct";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //8 June,2021 Sonal Gandhi
        public List<OnlineProductQty> GetAllOnlineProductQtyListByProductID(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllOnlineProductQtyListByProductID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OnlineProductQty> objlst = new List<OnlineProductQty>();
                while (dr.Read())
                {
                    OnlineProductQty objCategory = new OnlineProductQty();
                    objCategory.OnlineProductQtyID = objBaseSqlManager.GetInt64(dr, "OnlineProductQtyID");
                    objCategory.OnlineProductPrice = objBaseSqlManager.GetInt64(dr, "OnlineProductPrice");
                    objCategory.OnlineQty = objBaseSqlManager.GetInt64(dr, "OnlineQty");
                    objCategory.UnitID = objBaseSqlManager.GetInt64(dr, "OnlineUnitID");
                    objCategory.Factoring = objBaseSqlManager.GetDecimal(dr, "Factoring");
                    objCategory.FactoringAmount = objBaseSqlManager.GetDecimal(dr, "FactoringAmount");
                    objCategory.PremiumPercentage = objBaseSqlManager.GetDecimal(dr, "PremiumPercentage");
                    objCategory.PremiumPercentageAmt = objBaseSqlManager.GetDecimal(dr, "PremiumAmount");
                    objCategory.TotalOnlineAmount = objBaseSqlManager.GetDecimal(dr, "OnlineTotalAmount");
                    objCategory.IsOnline = objBaseSqlManager.GetBoolean(dr, "IsOnline");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }




    }
}
