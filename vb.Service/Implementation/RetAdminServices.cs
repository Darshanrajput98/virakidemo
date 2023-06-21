
namespace vb.Service
{
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

    public class RetAdminServices : IRetAdminService
    {
        public bool AddArea(RetAreaMst ObjArea)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjArea.AreaID == 0)
                {
                    context.RetAreaMsts.Add(ObjArea);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjArea).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjArea.AreaID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<RetAreaListResponse> GetAllAreaList()
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
                    objAreaList.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objAreaList.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objAreaList.Country = objBaseSqlManager.GetTextValue(dr, "Country");
                    objAreaList.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objAreaList.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objAreaList.DaysofWeekstr = new Utility().GetTextEnum(objAreaList.DaysofWeek);
                    objAreaList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteArea(long AreaID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetArea";
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddGodown(RetGodownMst ObjGodown)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjGodown.GodownID == 0)
                {
                    context.RetGodownMsts.Add(ObjGodown);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjGodown).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjGodown.GodownID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<RetGodownListResponse> GetAllGodownList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetGodownList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetGodownListResponse> objlst = new List<RetGodownListResponse>();
                while (dr.Read())
                {
                    RetGodownListResponse objGodown = new RetGodownListResponse();
                    objGodown.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objGodown.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objGodown.GodownPhone = objBaseSqlManager.GetTextValue(dr, "GodownPhone");
                    objGodown.GodownAddress1 = objBaseSqlManager.GetTextValue(dr, "GodownAddress1");
                    objGodown.GodownAddress2 = objBaseSqlManager.GetTextValue(dr, "GodownAddress2");
                    objGodown.GodownFSSAINumber = objBaseSqlManager.GetTextValue(dr, "GodownFSSAINumber");
                    objGodown.GodownCode = objBaseSqlManager.GetTextValue(dr, "GodownCode");
                    objGodown.GodownNote = objBaseSqlManager.GetTextValue(dr, "GodownNote");
                    objGodown.Place = objBaseSqlManager.GetTextValue(dr, "Place");
                    objGodown.Pincode = objBaseSqlManager.GetTextValue(dr, "Pincode");
                    objGodown.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objGodown.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    objGodown.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteGodown(long GodownID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetGodown";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddTax(RetTaxMst ObjTax)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjTax.TaxID == 0)
                {
                    context.RetTaxMsts.Add(ObjTax);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjTax).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjTax.TaxID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<RetTaxListResponse> GetAllTaxList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetTaxList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetTaxListResponse> objlst = new List<RetTaxListResponse>();
                while (dr.Read())
                {
                    RetTaxListResponse objTax = new RetTaxListResponse();
                    objTax.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objTax.TaxCode = objBaseSqlManager.GetTextValue(dr, "TaxCode");
                    objTax.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objTax.TaxDescription = objBaseSqlManager.GetTextValue(dr, "TaxDescription");
                    objTax.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objTax);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteTax(long TaxID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetTax";
                cmdGet.Parameters.AddWithValue("@TaxID", TaxID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddUnit(RetUnitMst ObjUnit)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjUnit.UnitID == 0)
                {
                    context.RetUnitMsts.Add(ObjUnit);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjUnit).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjUnit.UnitID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<RetUnitListResponse> GetAllUnitList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetUnitList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetUnitListResponse> objlst = new List<RetUnitListResponse>();
                while (dr.Read())
                {
                    RetUnitListResponse objUnit = new RetUnitListResponse();
                    objUnit.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objUnit.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objUnit.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objUnit.UnitDescription = objBaseSqlManager.GetTextValue(dr, "UnitDescription");
                    objUnit.GuiID = objBaseSqlManager.GetInt64(dr, "GuiID");
                    objUnit.LanguageName = objBaseSqlManager.GetTextValue(dr, "LanguageName");
                    objUnit.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objUnit);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteUnit(long UnitID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetUnit";
                cmdGet.Parameters.AddWithValue("@UnitID", UnitID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddRole(Role_Mst ObjRole)
        {
            throw new NotImplementedException();
        }

        public List<RoleListResponse> GetAllRoleList()
        {
            throw new NotImplementedException();
        }

        public bool DeleteRole(long RoleID)
        {
            throw new NotImplementedException();
        }

        public List<RoleListResponse> GetAllRoleName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRoleName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RoleListResponse> lstProduct = new List<RoleListResponse>();
                while (dr.Read())
                {
                    RoleListResponse objProductresponse = new RoleListResponse();
                    objProductresponse.RoleID = objBaseSqlManager.GetInt64(dr, "RoleID");
                    objProductresponse.RoleName = objBaseSqlManager.GetTextValue(dr, "RoleName");
                    lstProduct.Add(objProductresponse);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstProduct;
            }
        }

        public bool AddTransport(RetTransportMst ObjTransport)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjTransport.TransportID == 0)
                {
                    context.RetTransportMsts.Add(ObjTransport);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjTransport).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjTransport.TransportID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<RetTransportListResponse> GetAllTransportList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetTransportList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetTransportListResponse> objlst = new List<RetTransportListResponse>();
                while (dr.Read())
                {
                    RetTransportListResponse objTransportList = new RetTransportListResponse();
                    objTransportList.TransportID = objBaseSqlManager.GetInt64(dr, "TransportID");
                    objTransportList.TransportName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                    objTransportList.TransID = objBaseSqlManager.GetTextValue(dr, "TransID");
                    objTransportList.TransportGSTNumber = objBaseSqlManager.GetTextValue(dr, "TransportGSTNumber");
                    objTransportList.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objTransportList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objTransportList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteTransport(long TransportID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetTransport";
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

    }
}
