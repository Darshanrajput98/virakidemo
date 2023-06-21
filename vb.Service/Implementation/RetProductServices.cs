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
using vb.Service.Common;

namespace vb.Service
{
    public class RetProductServices : IRetProductService
    {
        public bool AddProductCategory(RetProductCategoryMst ObjCategory)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjCategory.CategoryID == 0)
                {
                    context.RetProductCategoryMsts.Add(ObjCategory);
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

        public List<RetProductCategoryListResponse> GetAllProductCategoryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProductCategoryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductCategoryListResponse> objlst = new List<RetProductCategoryListResponse>();
                while (dr.Read())
                {
                    RetProductCategoryListResponse objCategory = new RetProductCategoryListResponse();
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
                cmdGet.CommandText = "DeleteRetProductCategory";
                cmdGet.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdateProductDetails(List<RetProductQtyViewModel> data)
        {
            try
            {
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        SqlCommand cmdGet = new SqlCommand();
                        using (var objBaseSqlManager = new BaseSqlManager())
                        {
                            cmdGet.CommandType = CommandType.StoredProcedure;
                            cmdGet.CommandText = "UpdateProductDetails";
                            cmdGet.Parameters.AddWithValue("@ProductQtyID", item.ProductQtyID);
                            cmdGet.Parameters.AddWithValue("@ProductPrice", Math.Round(item.ProductPrice, 2));
                            cmdGet.Parameters.AddWithValue("@ProductMRP", Math.Round(item.ProductMRP, 2));
                            cmdGet.Parameters.AddWithValue("@ProductBarcode", item.ProductBarcode);
                            object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                            objBaseSqlManager.ForceCloseConnection();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<RetProductCategoryListResponse> GetAllRetCategoryName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCategoryName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductCategoryListResponse> lstCategory = new List<RetProductCategoryListResponse>();
                while (dr.Read())
                {
                    RetProductCategoryListResponse objCategory = new RetProductCategoryListResponse();
                    objCategory.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objCategory.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    lstCategory.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCategory;
            }
        }

        public List<RetUnitListResponse> GetAllRetUnitName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetUnitName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetUnitListResponse> lstUnit = new List<RetUnitListResponse>();
                while (dr.Read())
                {
                    RetUnitListResponse objUnit = new RetUnitListResponse();
                    objUnit.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objUnit.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    lstUnit.Add(objUnit);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstUnit;
            }
        }

        public bool AddProduct(RetProductViewModel data)
        {
            RetProductMst obj = new RetProductMst();
            obj.ProductID = data.ProductID;
            obj.ProductName = data.ProductName;
            obj.CategoryID = data.CategoryID;
            obj.GodownID = data.GodownID;
            obj.BestBeforeMonthID = data.BestBeforeMonthID;
            obj.ProductDescription = data.ProductDescription;
            obj.HSNNumber = data.HSNNumber;
            obj.IGST = data.IGST;
            obj.CGST = data.CGST;
            obj.SGST = data.SGST;
            obj.HFor = data.HFor;
            obj.ContentValue = data.ContentValue;
            obj.NutritionValue = data.NutritionValue;
            obj.PlaceOfOrigin = data.PlaceOfOrigin;

            obj.Protein = data.Protein;
            obj.Fat = data.Fat;
            obj.Carbohydrate = data.Carbohydrate;
            obj.TotalEnergy = data.TotalEnergy;
            obj.Information = data.Information;

            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = false;

            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.ProductID == 0)
                {
                    context.RetProductMsts.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.ProductID > 0)
                {
                    foreach (var item in data.lstProductQty)
                    {
                        RetProductQtyMst objProductQty = new RetProductQtyMst();
                        objProductQty.ProductQtyID = item.ProductQtyID;
                        objProductQty.ProductID = obj.ProductID;
                        objProductQty.IsSelect = item.IsSelect;
                        objProductQty.ProductQuantity = item.ProductQuantity;
                        objProductQty.UnitID = item.UnitID;
                        objProductQty.ProductPrice = item.ProductPrice;
                        objProductQty.ProductMRP = item.ProductMRP;

                        objProductQty.GramPerKG = item.GramPerKG;

                        objProductQty.ProductBarcode = item.ProductBarcode;
                        objProductQty.PouchNameID = item.PouchNameID;
                        objProductQty.CreatedBy = data.CreatedBy;
                        objProductQty.CreatedOn = data.CreatedOn;
                        objProductQty.UpdatedBy = data.UpdatedBy;
                        objProductQty.UpdatedOn = data.UpdatedOn;
                        if (objProductQty.ProductQtyID == 0)
                        {
                            context.RetProductQtyMsts.Add(objProductQty);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objProductQty).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }

                    foreach (var item in data.lstCountryWiseProduct)
                    {
                        CountryWiseProductName_Mst objCountryWiseProdName = new CountryWiseProductName_Mst();
                        objCountryWiseProdName.CountryWiseProductID = item.CountryWiseProductID;
                        objCountryWiseProdName.CountryID = item.CountryID;
                        objCountryWiseProdName.ProductID = obj.ProductID;
                        objCountryWiseProdName.ProductName = item.CountryWiseProductName;
                        objCountryWiseProdName.CreatedBy = data.CreatedBy;
                        objCountryWiseProdName.CreatedOn = data.CreatedOn;
                        objCountryWiseProdName.UpdatedBy = data.UpdatedBy;
                        objCountryWiseProdName.UpdatedOn = data.UpdatedOn;
                        objCountryWiseProdName.IsDelete = false;
                        if (objCountryWiseProdName.CountryWiseProductID == 0)
                        {
                            context.CountryWiseProductName_Mst.Add(objCountryWiseProdName);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objCountryWiseProdName).State = EntityState.Modified;
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

        public List<RetProductListResponse> GetAllProductList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProductList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductListResponse> objlst = new List<RetProductListResponse>();
                while (dr.Read())
                {
                    RetProductListResponse objProduct = new RetProductListResponse();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objProduct.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objProduct.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objProduct.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objProduct.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objProduct.BestBeforeMonthID = objBaseSqlManager.GetInt64(dr, "BestBeforeMonthID");
                    objProduct.MonthNumber = objBaseSqlManager.GetTextValue(dr, "MonthNumber");
                    objProduct.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objProduct.SGST = objBaseSqlManager.GetDecimal(dr, "SGST");
                    objProduct.CGST = objBaseSqlManager.GetDecimal(dr, "CGST");
                    objProduct.IGST = objBaseSqlManager.GetDecimal(dr, "IGST");
                    objProduct.HFor = objBaseSqlManager.GetDecimal(dr, "HFor");
                    objProduct.ContentValue = objBaseSqlManager.GetTextValue(dr, "ContentValue");
                    objProduct.NutritionValue = objBaseSqlManager.GetTextValue(dr, "NutritionValue");
                    objProduct.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objProduct.ProductDescription = objBaseSqlManager.GetTextValue(dr, "ProductDescription");
                    objProduct.PlaceOfOrigin = objBaseSqlManager.GetTextValue(dr, "PlaceOfOrigin");

                    objProduct.Protein = objBaseSqlManager.GetTextValue(dr, "Protein");
                    objProduct.Fat = objBaseSqlManager.GetTextValue(dr, "Fat");
                    objProduct.Carbohydrate = objBaseSqlManager.GetTextValue(dr, "Carbohydrate");
                    objProduct.TotalEnergy = objBaseSqlManager.GetTextValue(dr, "TotalEnergy");
                    objProduct.Information = objBaseSqlManager.GetTextValue(dr, "Information");

                    objProduct.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objProduct.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
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
                cmdGet.CommandText = "DeleteRetProduct";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddPouch(Pouch_Mst ObjPouch)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjPouch.PouchID == 0)
                {
                    context.Pouch_Mst.Add(ObjPouch);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjPouch).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjPouch.PouchID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<PouchListResponse> GetAllPouchList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPouchList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchListResponse> objlst = new List<PouchListResponse>();
                while (dr.Read())
                {
                    PouchListResponse objPouchList = new PouchListResponse();
                    objPouchList.PouchID = objBaseSqlManager.GetInt64(dr, "PouchID");
                    objPouchList.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    objPouchList.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    objPouchList.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPouchList.PouchDescription = objBaseSqlManager.GetTextValue(dr, "PouchDescription");
                    objPouchList.PouchQuantity = objBaseSqlManager.GetInt32(dr, "PouchQuantity");
                    objPouchList.Material = objBaseSqlManager.GetTextValue(dr, "Material");
                    objPouchList.Weight = objBaseSqlManager.GetDecimal(dr, "Weight");
                    objPouchList.KG = objBaseSqlManager.GetDecimal(dr, "KG");
                    objPouchList.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objPouchList.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objPouchList.MinPouchQuantity = objBaseSqlManager.GetInt64(dr, "MinPouchQuantity");
                    objPouchList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objPouchList.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objPouchList.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(objPouchList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeletePouch(long PouchID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePouch";
                cmdGet.Parameters.AddWithValue("@PouchID", PouchID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<GodownListResponse> GetAllRetGodownName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetGodownName";
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

        public bool AddPackageStation(PackageStation_Mst ObjPackageStation)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjPackageStation.PackageStationID == 0)
                {
                    context.PackageStation_Mst.Add(ObjPackageStation);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjPackageStation).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjPackageStation.PackageStationID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }

        public List<PackageStationListResponse> GetAllPackageStationList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPackageStationList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PackageStationListResponse> objlst = new List<PackageStationListResponse>();
                while (dr.Read())
                {
                    PackageStationListResponse objPouchList = new PackageStationListResponse();
                    objPouchList.PackageStationID = objBaseSqlManager.GetInt64(dr, "PackageStationID");
                    objPouchList.PackageStationName = objBaseSqlManager.GetTextValue(dr, "PackageStationName");
                    objPouchList.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objPouchList.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objPouchList.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    objPouchList.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPouchList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objPouchList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeletePackageStation(long PackageStationID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePackageStation";
                cmdGet.Parameters.AddWithValue("@PackageStationID", PackageStationID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
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

        public List<RetMonthListResponse> GetAllMonth()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllMonth";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetMonthListResponse> lstCategory = new List<RetMonthListResponse>();
                while (dr.Read())
                {
                    RetMonthListResponse objCategory = new RetMonthListResponse();
                    objCategory.BestBeforeMonthID = objBaseSqlManager.GetInt64(dr, "BestBeforeMonthID");
                    objCategory.MonthNumber = objBaseSqlManager.GetTextValue(dr, "MonthNumber");
                    lstCategory.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCategory;
            }
        }

        public List<RetProductQtyViewModel> GetAllRetailProductQtyListByProductID(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetailProductQtyListByProductID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductQtyViewModel> objlst = new List<RetProductQtyViewModel>();
                while (dr.Read())
                {
                    RetProductQtyViewModel objCategory = new RetProductQtyViewModel();
                    objCategory.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objCategory.IsSelect = objBaseSqlManager.GetBoolean(dr, "IsSelect");
                    objCategory.ProductQuantity = objBaseSqlManager.GetInt64(dr, "ProductQuantity");
                    objCategory.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objCategory.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objCategory.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");

                    objCategory.GramPerKG = objBaseSqlManager.GetDecimal(dr, "GramPerKG");

                    objCategory.ProductBarcode = objBaseSqlManager.GetTextValue(dr, "ProductBarcode");
                    objCategory.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    objCategory.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }




        public List<CountryWiseProductViewModel> GetCountryWiseProductNameByProductID(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCountryWiseProductNameByProductID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CountryWiseProductViewModel> objlst = new List<CountryWiseProductViewModel>();
                while (dr.Read())
                {
                    CountryWiseProductViewModel objCategory = new CountryWiseProductViewModel();
                    objCategory.CountryWiseProductID = objBaseSqlManager.GetInt64(dr, "CountryWiseProductID");
                    objCategory.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
                    objCategory.CountryWiseProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");

                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        public List<RetProductQtyViewModel> GetRetProductCategoryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetProductCategoryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductQtyViewModel> objlst = new List<RetProductQtyViewModel>();
                while (dr.Read())
                {
                    RetProductQtyViewModel objCategory = new RetProductQtyViewModel();
                    objCategory.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objCategory.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objCategory.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                    objCategory.ProductMRP = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductMRP"), 2);
                    objCategory.ProductBarcode = objBaseSqlManager.GetTextValue(dr, "ProductBarcode");
                    objCategory.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objCategory.NutritionValue = objBaseSqlManager.GetTextValue(dr, "NutritionValue");
                    objCategory.ContentValue = objBaseSqlManager.GetTextValue(dr, "ContentValue");
                    objCategory.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");
                    objCategory.ProductQuantity = objBaseSqlManager.GetInt64(dr, "ProductQuantity");
                    objCategory.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objCategory.PlaceOfOrigin = objBaseSqlManager.GetTextValue(dr, "PlaceOfOrigin");
                    objCategory.PouchSize = objBaseSqlManager.GetInt64(dr, "PouchSize");
                    objCategory.Protein = objBaseSqlManager.GetTextValue(dr, "Protein");
                    objCategory.Fat = objBaseSqlManager.GetTextValue(dr, "Fat");
                    objCategory.Carbohydrate = objBaseSqlManager.GetTextValue(dr, "Carbohydrate");
                    objCategory.TotalEnergy = objBaseSqlManager.GetTextValue(dr, "TotalEnergy");
                    objCategory.Information = objBaseSqlManager.GetTextValue(dr, "Information");
                    objCategory.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    objCategory.GramPerKG = objBaseSqlManager.GetDecimal(dr, "GramPerKG"); 
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public Godown_Mst GetGodownDetailsByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGodownDetailsByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                Godown_Mst objCategory = new Godown_Mst();
                while (dr.Read())
                {
                    objCategory.GodownAddress1 = objBaseSqlManager.GetTextValue(dr, "GodownAddress1");
                    objCategory.GodownAddress2 = objBaseSqlManager.GetTextValue(dr, "GodownAddress2");
                    objCategory.GodownFSSAINumber = objBaseSqlManager.GetTextValue(dr, "GodownFSSAINumber");
                    objCategory.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objCategory.GodownPhone = objBaseSqlManager.GetTextValue(dr, "GodownPhone");
                    objCategory.GodownCode = objBaseSqlManager.GetTextValue(dr, "GodownCode");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objCategory;
            }
        }

        public BestBeforeMonth GetMonthByProductID(long ID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetMonthByProductID";
                cmdGet.Parameters.AddWithValue("@ProductID", ID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                BestBeforeMonth objCategory = new BestBeforeMonth();
                while (dr.Read())
                {
                    objCategory.MonthNumber = objBaseSqlManager.GetTextValue(dr, "MonthNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objCategory;
            }
        }

        public List<RetProductQtyViewModelForExp> GetRetProductListForExp(decimal CurrencyRate = 0)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetProductListForExp";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductQtyViewModelForExp> objlst = new List<RetProductQtyViewModelForExp>();
                while (dr.Read())
                {
                    RetProductQtyViewModelForExp objCategory = new RetProductQtyViewModelForExp();
                    objCategory.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objCategory.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objCategory.ProductQuantity = objBaseSqlManager.GetInt64(dr, "ProductQuantity");
                    objCategory.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objCategory.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objCategory.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                    objCategory.ProductMRP = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductMRP"), 2);
                    objCategory.ProductBarcode = objBaseSqlManager.GetTextValue(dr, "ProductBarcode");
                    objCategory.BestBefore = objBaseSqlManager.GetTextValue(dr, "MonthNumber");
                    objCategory.SGST = objBaseSqlManager.GetDecimal(dr, "SGST");
                    objCategory.CGST = objBaseSqlManager.GetDecimal(dr, "CGST");
                    objCategory.IGST = objBaseSqlManager.GetDecimal(dr, "IGST");
                    objCategory.HFor = objBaseSqlManager.GetDecimal(dr, "HFor");
                    objCategory.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");

                    // 31 May, 2021 Sonal Gandhi
                    decimal productPrice = objCategory.ProductPrice;
                    decimal productMRP = objCategory.ProductMRP;

                    objCategory.ConvertedProductPrice = (CurrencyRate == 0) ? productPrice : Math.Round((productPrice / CurrencyRate), 2);
                    objCategory.ConvertedProductMRP = (CurrencyRate == 0) ? productMRP : Math.Round((productMRP / CurrencyRate), 2);

                    objCategory.PouchSize = objBaseSqlManager.GetInt64(dr, "PouchSize");

                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // CHIRAG 05-02-2019

        public List<GuiLanguageViewModel> GetAllLanguage()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGuiLanguage";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GuiLanguageViewModel> lstCategory = new List<GuiLanguageViewModel>();
                while (dr.Read())
                {
                    GuiLanguageViewModel objCategory = new GuiLanguageViewModel();
                    objCategory.GuiID = objBaseSqlManager.GetInt32(dr, "GuiID");
                    objCategory.LanguageName = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                    lstCategory.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCategory;
            }
        }

        public List<RetProdGuiViewModel> GetAllRetailProductListByProductID(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProdGuiByID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProdGuiViewModel> objlst = new List<RetProdGuiViewModel>();
                while (dr.Read())
                {
                    RetProdGuiViewModel objCategory = new RetProdGuiViewModel();
                    objCategory.RetProdGuiID = objBaseSqlManager.GetInt64(dr, "RetProdGuiID");
                    objCategory.GuiID = objBaseSqlManager.GetInt32(dr, "GuiID");
                    objCategory.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objCategory.ProductNameGui = objBaseSqlManager.GetTextValue(dr, "ProductNameGui");
                    objCategory.ContentGui = objBaseSqlManager.GetTextValue(dr, "ContentGui");
                    objCategory.NutritionGui = objBaseSqlManager.GetTextValue(dr, "NutritionGui");
                    objCategory.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objCategory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool AddProductGuiMaster(RetProdGuiViewModel data)
        {
            vb.Data.Model.RetProdGui obj = new vb.Data.Model.RetProdGui();
            obj.ProductID = data.ProductID;
            obj.RetProdGuiID = data.RetProdGuiID;
            obj.GuiID = data.GuiID;
            obj.ProductNameGui = data.ProductNameGui;
            obj.ContentGui = data.ContentGui;
            obj.NutritionGui = data.NutritionGui;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.ModifiedBy = data.ModifiedBy;
            obj.ModifiedOn = data.ModifiedOn;
            obj.IsDelete = false;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.RetProdGuiID == 0)
                {
                    context.RetProdGuis.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.ProductID > 0)
                {
                    if (data.lstRetProdQtyGui.Count() > 0)
                    {
                        foreach (var item in data.lstRetProdQtyGui)
                        {
                            if (item.UnitGuiID > 0 && item.ProductQtyGui.Trim() != "")
                            {
                                vb.Data.Model.RetProdQtyGui objProductQtyGui = new vb.Data.Model.RetProdQtyGui();
                                objProductQtyGui.RetProdGuiID = obj.RetProdGuiID;
                                objProductQtyGui.ProductQtyID = item.ProductQtyID;
                                objProductQtyGui.RetProdGuiQtyID = item.RetProdGuiQtyID;
                                objProductQtyGui.ProductQtyGui = item.ProductQtyGui;
                                objProductQtyGui.UnitGuiID = item.UnitGuiID;
                                objProductQtyGui.CreatedBy = data.CreatedBy;
                                objProductQtyGui.CreatedOn = data.CreatedOn;
                                objProductQtyGui.ModifiedBy = data.ModifiedBy;
                                objProductQtyGui.ModifiedOn = data.ModifiedOn;
                                objProductQtyGui.IsDelete = false;
                                if (objProductQtyGui.RetProdGuiQtyID == 0)
                                {
                                    context.RetProdQtyGuis.Add(objProductQtyGui);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    context.Entry(objProductQtyGui).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
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

        public List<RetUnitListResponse> GetAllRetUnitNameByGuiID(long GuiID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGuiLaguageByID";
                cmdGet.Parameters.AddWithValue("@GuiID", GuiID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetUnitListResponse> lstUnit = new List<RetUnitListResponse>();
                while (dr.Read())
                {
                    RetUnitListResponse objUnit = new RetUnitListResponse();
                    objUnit.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objUnit.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objUnit.UnitDescription = objBaseSqlManager.GetTextValue(dr, "UnitDescription");
                    lstUnit.Add(objUnit);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstUnit;
            }
        }

        public RetProdGuiViewModel GetRetailProductGuiByID(long ProductID, long GuiID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailProductGuiByID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@GuiID", GuiID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetProdGuiViewModel objRetProd = new RetProdGuiViewModel();
                while (dr.Read())
                {
                    objRetProd.RetProdGuiID = objBaseSqlManager.GetInt64(dr, "RetProdGuiID");
                    objRetProd.GuiID = objBaseSqlManager.GetInt32(dr, "GuiID");
                    objRetProd.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objRetProd.ProductNameGui = objBaseSqlManager.GetTextValue(dr, "ProductNameGui");
                    objRetProd.ContentGui = objBaseSqlManager.GetTextValue(dr, "ContentGui");
                    objRetProd.NutritionGui = objBaseSqlManager.GetTextValue(dr, "NutritionGui");
                    objRetProd.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objRetProd.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objRetProd.ModifiedBy = objBaseSqlManager.GetInt64(dr, "ModifiedBy");
                    objRetProd.ModifiedOn = objBaseSqlManager.GetDateTime(dr, "ModifiedOn");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objRetProd;
            }
        }

        public List<RetProdQtyGuiViewModel> GetRetailProductQtyGuiByID(long RetProdGuiID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailProductQtyGuiByID";
                cmdGet.Parameters.AddWithValue("@RetProdGuiID", RetProdGuiID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProdQtyGuiViewModel> ListobjRetProd = new List<RetProdQtyGuiViewModel>();
                while (dr.Read())
                {
                    RetProdQtyGuiViewModel objRetProd = new RetProdQtyGuiViewModel();
                    objRetProd.RetProdGuiQtyID = objBaseSqlManager.GetInt64(dr, "RetProdGuiQtyID");
                    objRetProd.RetProdGuiID = objBaseSqlManager.GetInt32(dr, "RetProdGuiID");
                    objRetProd.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objRetProd.ProductQtyGui = objBaseSqlManager.GetTextValue(dr, "ProductQtyGui");
                    objRetProd.UnitGuiID = objBaseSqlManager.GetInt32(dr, "UnitGuiID");
                    objRetProd.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objRetProd.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    ListobjRetProd.Add(objRetProd);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ListobjRetProd;
            }
        }

        public bool AddLanguage(GuiLanguageViewModel data)
        {
            try
            {
                vb.Data.Model.GuiLanguage obj = new vb.Data.Model.GuiLanguage();
                obj.GuiID = data.GuiID;
                obj.LanguageName = data.LanguageName.Trim();
                obj.Description = data.Description;
                obj.UpdatedBy = data.CreatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                obj.CreatedOn = data.CreatedOn;
                obj.CreatedBy = data.CreatedBy;
                obj.IsDelete = false;
                using (VirakiEntities context = new VirakiEntities())
                {
                    var exists = context.GuiLanguages.Where(o => o.GuiID != data.GuiID && o.LanguageName == obj.LanguageName).ToList();
                    if (exists.Count() > 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (obj.GuiID == 0)
                        {
                            context.GuiLanguages.Add(obj);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(obj).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GuiLanguageViewModel> GetAllLanguageList(long GuiID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllLabguage";
                cmdGet.Parameters.AddWithValue("@GuiID", GuiID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GuiLanguageViewModel> ListOfLanguage = new List<GuiLanguageViewModel>();
                while (dr.Read())
                {
                    GuiLanguageViewModel objLanguage = new GuiLanguageViewModel();
                    objLanguage.GuiID = objBaseSqlManager.GetInt32(dr, "GuiID");
                    objLanguage.LanguageName = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                    objLanguage.Description = objBaseSqlManager.GetTextValue(dr, "Description");
                    objLanguage.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objLanguage.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objLanguage.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
                    objLanguage.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objLanguage.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    ListOfLanguage.Add(objLanguage);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ListOfLanguage;
            }
        }

        public bool DeleteLanguage(long? GuiID, bool IsDelete)
        {
            bool result = false;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteLanguage";
                    cmdGet.Parameters.AddWithValue("@GuiID", GuiID);
                    cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                    object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool AddLabel(GuiLabelViewModel data)
        {
            try
            {
                vb.Data.Model.GuiLabel obj = new vb.Data.Model.GuiLabel();
                obj.GuiLabelID = data.GuiLabelID;
                obj.GuiID = data.GuiID;
                obj.LabelCode = data.LabelCode.Trim();
                obj.LabelValue = data.LabelValue.Trim();
                obj.UpdatedBy = data.CreatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                obj.CreatedOn = data.CreatedOn;
                obj.CreatedBy = data.CreatedBy;
                obj.IsDelete = false;
                using (VirakiEntities context = new VirakiEntities())
                {
                    var exists = context.GuiLabels.Where(o => o.GuiID == data.GuiID && o.LabelCode == data.LabelCode.Trim() && o.GuiLabelID != data.GuiLabelID).ToList();

                    if (exists.Count() > 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (obj.GuiLabelID == 0)
                        {
                            context.GuiLabels.Add(obj);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(obj).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GuiLabelViewModel> GetAllLabelList(long GuiLabelID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllLabel";
                cmdGet.Parameters.AddWithValue("@GuiLabelID", GuiLabelID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GuiLabelViewModel> ListOfGuiLable = new List<GuiLabelViewModel>();
                while (dr.Read())
                {
                    GuiLabelViewModel objLanguage = new GuiLabelViewModel();
                    objLanguage.GuiLabelID = objBaseSqlManager.GetInt64(dr, "GuiLabelID");
                    objLanguage.GuiID = objBaseSqlManager.GetInt32(dr, "GuiID");
                    objLanguage.LabelCode = objBaseSqlManager.GetTextValue(dr, "LabelCode");
                    objLanguage.LabelValue = objBaseSqlManager.GetTextValue(dr, "LabelValue");
                    objLanguage.LanguageName = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                    objLanguage.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objLanguage.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objLanguage.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
                    objLanguage.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objLanguage.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    ListOfGuiLable.Add(objLanguage);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ListOfGuiLable;
            }
        }

        public bool DeleteLabel(long? GuiLabelID, bool IsDelete)
        {
            bool result = false;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteLabel";
                    cmdGet.Parameters.AddWithValue("@GuiLabelID", GuiLabelID);
                    cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                    object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public List<Resource> GetAllLabelByGuiID(long GuiID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllLabelByGuiID";
                cmdGet.Parameters.AddWithValue("@GuiID", GuiID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<Resource> ListOfResource = new List<Resource>();
                while (dr.Read())
                {
                    Resource objResource = new Resource();
                    objResource.ResourceKey = objBaseSqlManager.GetTextValue(dr, "LabelCode");
                    objResource.ResourceValue = objBaseSqlManager.GetTextValue(dr, "LabelValue");
                    ListOfResource.Add(objResource);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ListOfResource;
            }
        }

        public List<RetProdWithQtyGuiList> GetProductGuiByProductID(long ProductID, long GuiID, long ProductQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductGuiByProductID";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@GuiID", GuiID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProdWithQtyGuiList> ListOfProdQtyGui = new List<RetProdWithQtyGuiList>();
                while (dr.Read())
                {
                    RetProdWithQtyGuiList objResource = new RetProdWithQtyGuiList();
                    objResource.RetProdGuiID = objBaseSqlManager.GetInt64(dr, "RetProdGuiID");
                    objResource.GuiID = objBaseSqlManager.GetInt32(dr, "GuiID");
                    objResource.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objResource.RetProdGuiQtyID = objBaseSqlManager.GetInt64(dr, "RetProdGuiQtyID");
                    objResource.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objResource.ProductNameGui = objBaseSqlManager.GetTextValue(dr, "ProductNameGui");
                    objResource.ContentGui = objBaseSqlManager.GetTextValue(dr, "ContentGui");
                    objResource.NutritionGui = objBaseSqlManager.GetTextValue(dr, "NutritionGui");
                    objResource.ProductQtyGui = objBaseSqlManager.GetTextValue(dr, "ProductQtyGui");
                    objResource.LanguageName = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                    objResource.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objResource.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objResource.UnitDescription = objBaseSqlManager.GetTextValue(dr, "UnitDescription");
                    ListOfProdQtyGui.Add(objResource);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ListOfProdQtyGui;
            }
        }

        public List<RetUnitListResponse> GetAllRetGuiUnitName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetUnitName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetUnitListResponse> lstUnit = new List<RetUnitListResponse>();
                while (dr.Read())
                {
                    RetUnitListResponse objUnit = new RetUnitListResponse();
                    objUnit.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objUnit.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    lstUnit.Add(objUnit);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                RetUnitListResponse defaultUnit = new RetUnitListResponse();
                defaultUnit.UnitID = 0;
                defaultUnit.UnitCode = "Select Unit";
                lstUnit.Insert(0, defaultUnit);
                return lstUnit;
            }
        }


        public ProductNameByLanguageID GetProductNameByLanguageID(long Language1, long ProductIDlabel)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductNameByLanguageID";
                cmdGet.Parameters.AddWithValue("@Language", Language1);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductIDlabel);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ProductNameByLanguageID objCategory = new ProductNameByLanguageID();
                while (dr.Read())
                {
                    objCategory.ProductName1 = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objCategory;
            }
        }

        public ProductNameByLanguageID GetProductNameByLanguageID2(long Language2, long ProductIDlabel)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductNameByLanguageID";
                cmdGet.Parameters.AddWithValue("@Language", Language2);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductIDlabel);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ProductNameByLanguageID objCategory = new ProductNameByLanguageID();
                while (dr.Read())
                {
                    objCategory.ProductName2 = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objCategory;
            }
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


        // Add Barcode QTY Details 30-03-2020      
        public Int64 AddBarcodeQuantityDetails(string ProductID, long ProductQtyID, string ProductName, DateTime DateOfPackaging, long NoOfBarcodes, long GodownID, long CreatedBy, DateTime CreatedOn, long UpdatedBy, DateTime UpdatedOn, bool IsDelete)
        {
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "InsertBarcodeQuantityDetails";
                    cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                    cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                    cmdGet.Parameters.AddWithValue("@ProductName", ProductName);
                    cmdGet.Parameters.AddWithValue("@DateOfPackaging", DateOfPackaging);
                    cmdGet.Parameters.AddWithValue("@NoOfBarcodes", NoOfBarcodes);
                    cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                    cmdGet.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);
                    cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
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


        // 31 May, 2021 Sonal Gandhi
        public List<CurrencyViewModel> GetAllCurrency()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCurrency";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CurrencyViewModel> lstCurrency = new List<CurrencyViewModel>();
                while (dr.Read())
                {
                    CurrencyViewModel objCurrency = new CurrencyViewModel();
                    objCurrency.CurrencyID = objBaseSqlManager.GetInt32(dr, "CurrencyID");
                    objCurrency.CurrencyName = objBaseSqlManager.GetTextValue(dr, "CurrencyName");
                    objCurrency.CurrencyCode = objBaseSqlManager.GetTextValue(dr, "CurrencyCode");
                    objCurrency.CurrencyName = objCurrency.CurrencyCode + " - " + objCurrency.CurrencyName;
                    lstCurrency.Add(objCurrency);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCurrency;
            }
        }
        public List<CurrencyViewModel> GetAllCurrencyList(long CurrencyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCurrency";
                cmdGet.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CurrencyViewModel> ListOfCurrency = new List<CurrencyViewModel>();
                while (dr.Read())
                {
                    CurrencyViewModel objCurrency = new CurrencyViewModel();
                    objCurrency.CurrencyID = objBaseSqlManager.GetInt32(dr, "CurrencyID");
                    objCurrency.CurrencyName = objBaseSqlManager.GetTextValue(dr, "CurrencyName");
                    objCurrency.CurrencyCode = objBaseSqlManager.GetTextValue(dr, "CurrencyCode");
                    objCurrency.CurrencySign = objBaseSqlManager.GetTextValue(dr, "CurrencySign");
                    objCurrency.CurrencyRate = objBaseSqlManager.GetDecimal(dr, "CurrencyRate");
                    objCurrency.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCurrency.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objCurrency.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
                    objCurrency.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objCurrency.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    ListOfCurrency.Add(objCurrency);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ListOfCurrency;
            }
        }

        public bool AddCurrency(CurrencyViewModel data)
        {
            try
            {
                vb.Data.Model.Currency_Mst obj = new vb.Data.Model.Currency_Mst();
                obj.CurrencyID = data.CurrencyID;
                obj.CurrencyName = data.CurrencyName;
                obj.CurrencyCode = data.CurrencyCode;
                obj.CurrencySign = data.CurrencySign;
                obj.UpdatedBy = data.CreatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                obj.CreatedOn = data.CreatedOn;
                obj.CreatedBy = data.CreatedBy;
                obj.IsDelete = false;
                using (VirakiEntities context = new VirakiEntities())
                {
                    var exists = context.Currency_Mst.Where(o => o.CurrencyID != data.CurrencyID && o.CurrencyName == obj.CurrencyName).ToList();
                    if (exists.Count() > 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (obj.CurrencyID == 0)
                        {
                            context.Currency_Mst.Add(obj);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(obj).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool DeleteCurrency(long? CurrencyID, bool IsDelete)
        {
            bool result = false;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteCurrency";
                    cmdGet.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                    cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                    object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public string GetCurrencySignByCurrencyID(long currencyID)
        {
            string CurrencySign = string.Empty;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCurrencySignByCurrencyID";
                cmdGet.Parameters.AddWithValue("@CurrencyID", currencyID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    CurrencySign = objBaseSqlManager.GetTextValue(dr, "CurrencySign");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CurrencySign;
            }
        }

    }
}
