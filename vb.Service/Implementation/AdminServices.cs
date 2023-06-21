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
//using MapYourMeds.Model;


namespace vb.Service
{
    public class AdminServices : IAdminService
    {
        //public bool AddArea(Area_Mst ObjArea)
        //{
        //    using (VirakiEntities context = new VirakiEntities())
        //    {
        //        if (ObjArea.AreaID == 0)
        //        {
        //            context.Area_Mst.Add(ObjArea);
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            context.Entry(ObjArea).State = EntityState.Modified;
        //            context.SaveChanges();
        //        }
        //        if (ObjArea.AreaID > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //21 June,2021 Sonal Gandhi
        public bool AddArea(AreaViewModel data)
        {
            if (!string.IsNullOrEmpty(data.DeleteItems))
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteAreaPincodeItems";
                    cmdGet.Parameters.AddWithValue("@AreaPincodeID", data.DeleteItems);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }

            Area_Mst obj = new Area_Mst();
            obj.AreaID = data.AreaID;
            obj.AreaName = data.AreaName;
            obj.City = data.City;
            obj.Country = data.Country;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            obj.DaysofWeek = data.DaysofWeek;
            obj.IsDelete = false;
            obj.PinCode = data.PinCode;
            obj.State = data.State;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = DateTime.Now;
            obj.IsOnline = data.IsOnline;

            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.AreaID == 0)
                {
                    context.Area_Mst.Add(obj);
                    context.SaveChanges();

                    AreaPincode_SubMst objPincode = new AreaPincode_SubMst();
                    objPincode.AreaPincodeID = 0;
                    objPincode.AreaID = obj.AreaID;
                    objPincode.PinCode = obj.PinCode;
                    objPincode.CreatedBy = obj.CreatedBy;
                    objPincode.CreatedOn = obj.CreatedOn;
                    objPincode.UpdatedBy = obj.UpdatedBy;
                    objPincode.UpdatedOn = obj.UpdatedOn;
                    objPincode.IsDelete = false;
                    if (objPincode.AreaPincodeID == 0)
                    {
                        context.AreaPincode_SubMst.Add(objPincode);
                        context.SaveChanges();
                    }
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }

                if (obj.AreaID > 0)
                {
                    foreach (var item in data.lstAreaPincode)
                    {
                        AreaPincode_SubMst objPincode = new AreaPincode_SubMst();
                        objPincode.AreaPincodeID = item.AreaPincodeID;
                        objPincode.AreaID = obj.AreaID;
                        objPincode.PinCode = item.Pincode;
                        objPincode.CreatedBy = obj.CreatedBy;
                        objPincode.CreatedOn = obj.CreatedOn;
                        objPincode.UpdatedBy = obj.UpdatedBy;
                        objPincode.UpdatedOn = obj.UpdatedOn;
                        objPincode.IsDelete = false;
                        if (objPincode.AreaPincodeID == 0)
                        {
                            context.AreaPincode_SubMst.Add(objPincode);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objPincode).State = EntityState.Modified;
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


        public List<AreaListResponse> GetAllAreaList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllAreaList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AreaListResponse> objlst = new List<AreaListResponse>();
                List<AreaPincodeModel> objlstPincode = new List<AreaPincodeModel>();
                while (dr.Read())
                {
                    AreaListResponse objAreaList = new AreaListResponse();
                    objAreaList.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objAreaList.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objAreaList.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objAreaList.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objAreaList.Country = objBaseSqlManager.GetTextValue(dr, "Country");
                    objAreaList.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objAreaList.DaysofWeek = objBaseSqlManager.GetInt32(dr, "DaysofWeek");
                    objAreaList.DaysofWeekstr = new Utility().GetTextEnum(objAreaList.DaysofWeek);
                    objAreaList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");

                    // 12 June,2021 Sonal Gandhi
                    objAreaList.IsOnline = objBaseSqlManager.GetBoolean(dr, "IsOnline");

                    objlstPincode = GetAreaPincodeList(objAreaList.AreaID);
                    objAreaList.lstAreaPincode = objlstPincode;

                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteArea(long AreaID, bool IsDelete)
        {
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                SqlCommand cmdGet = new SqlCommand();
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteArea";

                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddEvent(Event_Mst ObjEvent)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjEvent.EventID == 0)
                {
                    context.Event_Mst.Add(ObjEvent);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjEvent).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjEvent.EventID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<EventListResponse> GetAllEventList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEventList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EventListResponse> objlst = new List<EventListResponse>();
                while (dr.Read())
                {
                    EventListResponse objEventList = new EventListResponse();
                    objEventList.EventID = objBaseSqlManager.GetInt64(dr, "EventID");
                    objEventList.EventName = objBaseSqlManager.GetTextValue(dr, "EventName");
                    objEventList.EventDescription = objBaseSqlManager.GetTextValue(dr, "EventDescription");
                    objEventList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objEventList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteEvent(long EventID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteEvent";
                cmdGet.Parameters.AddWithValue("@EventID", EventID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddEventDate(EventDate_Mst ObjEvent)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjEvent.EventDateID == 0)
                {
                    context.EventDate_Mst.Add(ObjEvent);
                    context.SaveChanges();
                }
                else
                {

                    context.Entry(ObjEvent).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjEvent.EventDateID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<EventDateListResponse> GetAllEventDateList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEventDateList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EventDateListResponse> objlst = new List<EventDateListResponse>();
                while (dr.Read())
                {
                    EventDateListResponse objEventList = new EventDateListResponse();
                    objEventList.EventDateID = objBaseSqlManager.GetInt64(dr, "EventDateID");
                    objEventList.EventID = objBaseSqlManager.GetInt64(dr, "EventID");
                    objEventList.EventName = objBaseSqlManager.GetTextValue(dr, "EventName");
                    objEventList.EventDate = objBaseSqlManager.GetDateTime(dr, "EventDate");
                    objEventList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objEventList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteEventDate(long EventDateID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteEventDate";
                cmdGet.Parameters.AddWithValue("@EventDateID", EventDateID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddGodown(Godown_Mst ObjGodown)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjGodown.GodownID == 0)
                {
                    context.Godown_Mst.Add(ObjGodown);
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

        public List<GodownListResponse> GetAllGodownList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGodownList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GodownListResponse> objlst = new List<GodownListResponse>();
                while (dr.Read())
                {
                    GodownListResponse objGodown = new GodownListResponse();
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
                    objGodown.GSTNumber = objBaseSqlManager.GetDecimal(dr, "GSTNumber");
                    objGodown.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objGodown.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    objGodown.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "OpeningAmount");
                    objGodown.ChillarAmount = objBaseSqlManager.GetDecimal(dr, "ChillarAmount");
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
                cmdGet.CommandText = "DeleteGodown";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddTax(Tax_Mst ObjTax)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjTax.TaxID == 0)
                {
                    context.Tax_Mst.Add(ObjTax);
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

        public List<TaxListResponse> GetAllTaxList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTaxList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TaxListResponse> objlst = new List<TaxListResponse>();
                while (dr.Read())
                {
                    TaxListResponse objTax = new TaxListResponse();
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
                cmdGet.CommandText = "DeleteTax";
                cmdGet.Parameters.AddWithValue("@TaxID", TaxID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool AddUnit(Unit_Mst ObjUnit)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjUnit.UnitID == 0)
                {
                    context.Unit_Mst.Add(ObjUnit);
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

        public List<UnitListResponse> GetAllUnitList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUnitList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UnitListResponse> objlst = new List<UnitListResponse>();
                while (dr.Read())
                {
                    UnitListResponse objUnit = new UnitListResponse();
                    objUnit.UnitID = objBaseSqlManager.GetInt64(dr, "UnitID");
                    objUnit.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objUnit.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objUnit.UnitDescription = objBaseSqlManager.GetTextValue(dr, "UnitDescription");
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
                cmdGet.CommandText = "DeleteUnit";
                cmdGet.Parameters.AddWithValue("@UnitID", UnitID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //public bool AddRole(Role_Mst ObjRole)
        //{
        //    using (VirakiEntities context = new VirakiEntities())
        //    {
        //        if (ObjRole.RoleID == 0)
        //        {
        //            context.Role_Mst.Add(ObjRole);
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            context.Entry(ObjRole).State = EntityState.Modified;
        //            context.SaveChanges();
        //        }
        //        if (ObjRole.RoleID > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}


        public int AddRole(webpages_Roles ObjRole)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjRole.RoleId == 0)
                {
                    context.webpages_Roles.Add(ObjRole);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjRole).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjRole.RoleId > 0)
                {
                    //return true;
                    return ObjRole.RoleId;
                }
                else
                {
                    return 0;
                    //return false;
                }
            }
        }


        public List<RoleListResponse> GetAllRoleList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRoleList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RoleListResponse> objlst = new List<RoleListResponse>();
                while (dr.Read())
                {
                    RoleListResponse objRole = new RoleListResponse();
                    objRole.RoleID = objBaseSqlManager.GetInt64(dr, "RoleID");
                    objRole.RoleName = objBaseSqlManager.GetTextValue(dr, "RoleName");
                    objRole.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objRole.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objRole.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objRole);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteRole(long RoleID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRole";
                cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
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
                    // objProductresponse.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    lstProduct.Add(objProductresponse);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstProduct;
            }
        }



        // old
        //public List<UserListResponse> GetAllUserList()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetAllUserList";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<UserListResponse> objlst = new List<UserListResponse>();
        //    while (dr.Read())
        //    {
        //        UserListResponse objUser = new UserListResponse();
        //        objUser.UserID = objBaseSqlManager.GetInt64(dr, "Id");
        //        objUser.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
        //        objUser.UserFullName = objBaseSqlManager.GetTextValue(dr, "FullName");
        //        objUser.RoleID = objBaseSqlManager.GetInt64(dr, "RoleID");
        //        objUser.RoleName = objBaseSqlManager.GetTextValue(dr, "RoleName");
        //        objUser.UserCode = objBaseSqlManager.GetTextValue(dr, "UserCode");
        //        objUser.UserEmail = objBaseSqlManager.GetTextValue(dr, "UserEmail");
        //        objUser.UserMobile = objBaseSqlManager.GetTextValue(dr, "UserMobile");
        //        objUser.UserPhone = objBaseSqlManager.GetTextValue(dr, "UserPhone");
        //        objUser.UserPhoneExtn = objBaseSqlManager.GetTextValue(dr, "UserPhoneExtn");
        //        objUser.UserDesignation = objBaseSqlManager.GetTextValue(dr, "UserDesignation");
        //        objUser.UserDepartment = objBaseSqlManager.GetTextValue(dr, "UserDepartment");
        //        objUser.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
        //        objUser.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
        //        objUser.UserLocation = objBaseSqlManager.GetTextValue(dr, "UserLocation");
        //        objUser.UserRemark = objBaseSqlManager.GetTextValue(dr, "UserRemark");
        //        objUser.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
        //        objlst.Add(objUser);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return objlst;
        //}


        public bool UpdateUser(User obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.Id == 0)
                {
                    context.Users.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<RegistrationListResponse> GetAllUserList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUserList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RegistrationListResponse> objlst = new List<RegistrationListResponse>();

                string path = ConfigurationManager.AppSettings["Document"];

                while (dr.Read())
                {
                    RegistrationListResponse obj = new RegistrationListResponse();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    obj.RoleID = objBaseSqlManager.GetInt64(dr, "RoleID");
                    obj.RoleName = objBaseSqlManager.GetTextValue(dr, "RoleName");
                    obj.Address = objBaseSqlManager.GetTextValue(dr, "Address");
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    if (obj.BirthDate < DateTime.Now)
                    {
                        obj.Age = GetAge(obj.BirthDate);
                    }
                    else
                    {
                        obj.Age = null;
                    }
                    obj.Gender = objBaseSqlManager.GetTextValue(dr, "Gender");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    obj.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    obj.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    obj.PrimaryArea = objBaseSqlManager.GetInt64(dr, "PrimaryArea");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.PrimaryPin = objBaseSqlManager.GetInt64(dr, "PrimaryPin");
                    obj.SecondaryArea = objBaseSqlManager.GetInt64(dr, "SecondaryArea");
                    obj.SecondaryAddress = objBaseSqlManager.GetTextValue(dr, "SecondaryAddress");
                    obj.SecondaryPin = objBaseSqlManager.GetInt64(dr, "SecondaryPin");
                    obj.PanNo = objBaseSqlManager.GetTextValue(dr, "PanNo");
                    obj.PassportNo = objBaseSqlManager.GetTextValue(dr, "PassportNo");
                    obj.PassportValiddate = (objBaseSqlManager.GetTextValue(dr, "PassportValiddate"));
                    obj.UIDAI = objBaseSqlManager.GetInt64(dr, "UIDAI");
                    obj.UAN = objBaseSqlManager.GetInt64(dr, "UAN");
                    obj.PF = objBaseSqlManager.GetTextValue(dr, "PF");
                    obj.ESIC = objBaseSqlManager.GetTextValue(dr, "ESIC");
                    obj.Drivinglicence = objBaseSqlManager.GetTextValue(dr, "Drivinglicence");
                    obj.ReferenceName = objBaseSqlManager.GetTextValue(dr, "ReferenceName");
                    obj.FName = objBaseSqlManager.GetTextValue(dr, "FName");
                    obj.Fdob = (objBaseSqlManager.GetTextValue(dr, "Fdob"));
                    obj.FUIDAI = objBaseSqlManager.GetInt64(dr, "FUIDAI");
                    obj.FRelation = objBaseSqlManager.GetTextValue(dr, "FRelation");
                    obj.Flivingtogether = objBaseSqlManager.GetTextValue(dr, "Flivingtogether");
                    obj.MName = objBaseSqlManager.GetTextValue(dr, "MName");
                    obj.Mdob = (objBaseSqlManager.GetTextValue(dr, "Mdob"));
                    obj.MUIDAI = objBaseSqlManager.GetInt64(dr, "MUIDAI");
                    obj.MRelation = objBaseSqlManager.GetTextValue(dr, "MRelation");
                    obj.Mlivingtogether = objBaseSqlManager.GetTextValue(dr, "Mlivingtogether");
                    obj.WName = objBaseSqlManager.GetTextValue(dr, "WName");
                    obj.Wdob = (objBaseSqlManager.GetTextValue(dr, "Wdob"));
                    obj.WUIDAI = objBaseSqlManager.GetInt64(dr, "WUIDAI");
                    obj.WRelation = objBaseSqlManager.GetTextValue(dr, "WRelation");
                    obj.Wlivingtogether = objBaseSqlManager.GetTextValue(dr, "Wlivingtogether");
                    obj.C1Name = objBaseSqlManager.GetTextValue(dr, "C1Name");
                    obj.C1dob = (objBaseSqlManager.GetTextValue(dr, "C1dob"));
                    obj.C1UIDAI = objBaseSqlManager.GetInt64(dr, "C1UIDAI");
                    obj.C1Relation = objBaseSqlManager.GetTextValue(dr, "C1Relation");
                    obj.C1livingtogether = objBaseSqlManager.GetTextValue(dr, "C1livingtogether");
                    obj.C2Name = objBaseSqlManager.GetTextValue(dr, "C2Name");
                    obj.C2dob = (objBaseSqlManager.GetTextValue(dr, "C2dob"));
                    obj.C2UIDAI = objBaseSqlManager.GetInt64(dr, "C2UIDAI");
                    obj.C2Relation = objBaseSqlManager.GetTextValue(dr, "C2Relation");
                    obj.C2livingtogether = objBaseSqlManager.GetTextValue(dr, "C2livingtogether");
                    obj.C3Name = objBaseSqlManager.GetTextValue(dr, "C3Name");
                    obj.C3dob = (objBaseSqlManager.GetTextValue(dr, "C3dob"));
                    obj.C3UIDAI = objBaseSqlManager.GetInt64(dr, "C3UIDAI");
                    obj.C3Relation = objBaseSqlManager.GetTextValue(dr, "C3Relation");
                    obj.C3livingtogether = objBaseSqlManager.GetTextValue(dr, "C3livingtogether");
                    obj.DrivingValidup = objBaseSqlManager.GetTextValue(dr, "DrivingValidup");
                    obj.Godown = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.Maritalstatus = objBaseSqlManager.GetTextValue(dr, "Maritalstatus");
                    if (obj.DateOfJoining < DateTime.Now)
                    {
                        obj.ServiceTime = GetAge(obj.DateOfJoining);
                    }
                    else
                    {
                        obj.ServiceTime = null;
                    }
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.ProfilePicture = objBaseSqlManager.GetTextValue(dr, "ProfilePicture");
                    obj.ProfilePicturePath = "../../../ProfilePicture/" + obj.ProfilePicture;

                    obj.FSSAIDoctorCertificate = objBaseSqlManager.GetTextValue(dr, "FSSAIDoctorCertificate");
                    if (obj.FSSAIDoctorCertificate != "")
                    {
                        obj.FSSAIDoctorCertificatepath = path + obj.FSSAIDoctorCertificate;
                    }
                    else
                    {
                        obj.FSSAIDoctorCertificatepath = "";
                    }

                    obj.FSSAIDoctorCertificateValidity = objBaseSqlManager.GetDateTime(dr, "FSSAIDoctorCertificateValidity");
                    if (obj.FSSAIDoctorCertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FSSAIDoctorCertificateValiditystr = obj.FSSAIDoctorCertificateValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FSSAIDoctorCertificateValiditystr = "";
                    }

                    obj.DateofLeaving = objBaseSqlManager.GetDateTime(dr, "DateofLeaving");
                    if (obj.DateofLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateofLeavingstr = obj.DateofLeaving.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateofLeavingstr = "";
                    }


                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.Password = objBaseSqlManager.GetTextValue(dr, "Password");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        private string GetAge(DateTime birthday)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(birthday).Ticks).Year - 1;
            DateTime dtPastYearDate = birthday.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (dtPastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (dtPastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(dtPastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(dtPastYearDate).Hours;
            int Minutes = Now.Subtract(dtPastYearDate).Minutes;
            int Seconds = Now.Subtract(dtPastYearDate).Seconds;
            return String.Format("{0}Year {1}Month {2}Day",
                                Years, Months, Days);
        }

        public List<RegistrationListResponse> GetAllUserListForExcel()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUserListForExcel";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RegistrationListResponse> objlst = new List<RegistrationListResponse>();
                while (dr.Read())
                {
                    RegistrationListResponse obj = new RegistrationListResponse();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    if (obj.BirthDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.BirthDatestr = obj.BirthDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.BirthDatestr = "";
                    }
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    obj.RoleID = objBaseSqlManager.GetInt64(dr, "RoleID");
                    obj.RoleName = objBaseSqlManager.GetTextValue(dr, "RoleName");
                    obj.Address = objBaseSqlManager.GetTextValue(dr, "Address");
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    if (obj.BirthDate < DateTime.Now)
                    {
                        obj.Age = GetAge(obj.BirthDate);
                    }
                    else
                    {
                        obj.Age = null;
                    }
                    obj.Gender = objBaseSqlManager.GetTextValue(dr, "Gender");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    obj.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    obj.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    obj.PrimaryArea = objBaseSqlManager.GetInt64(dr, "PrimaryArea");
                    obj.PrimaryAreaName = objBaseSqlManager.GetTextValue(dr, "PrimaryAreaName");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.PrimaryPin = objBaseSqlManager.GetInt64(dr, "PrimaryPin");
                    obj.SecondaryArea = objBaseSqlManager.GetInt64(dr, "SecondaryArea");
                    obj.SecondaryAreaName = objBaseSqlManager.GetTextValue(dr, "SecondaryAreaName");
                    obj.SecondaryAddress = objBaseSqlManager.GetTextValue(dr, "SecondaryAddress");
                    obj.SecondaryPin = objBaseSqlManager.GetInt64(dr, "SecondaryPin");
                    obj.PanNo = objBaseSqlManager.GetTextValue(dr, "PanNo");
                    obj.PassportNo = objBaseSqlManager.GetTextValue(dr, "PassportNo");
                    //obj.PassportValiddate = (objBaseSqlManager.GetTextValue(dr, "PassportValiddate"));
                    obj.PassportValiddate1 = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.PassportValiddate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PassportValiddate = obj.PassportValiddate1.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.PassportValiddate = "";
                    }

                    //obj.DrivingValidup = objBaseSqlManager.GetTextValue(dr, "DrivingValidup");
                    obj.DrivingValidup1 = objBaseSqlManager.GetDateTime(dr, "DrivingValidup");
                    if (obj.DrivingValidup1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DrivingValidup = obj.DrivingValidup1.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DrivingValidup = "";
                    }
                    obj.UIDAI = objBaseSqlManager.GetInt64(dr, "UIDAI");
                    obj.UAN = objBaseSqlManager.GetInt64(dr, "UAN");
                    obj.PF = objBaseSqlManager.GetTextValue(dr, "PF");
                    obj.ESIC = objBaseSqlManager.GetTextValue(dr, "ESIC");
                    obj.Drivinglicence = objBaseSqlManager.GetTextValue(dr, "Drivinglicence");
                    obj.ReferenceName = objBaseSqlManager.GetTextValue(dr, "ReferenceName");
                    obj.FName = objBaseSqlManager.GetTextValue(dr, "FName");
                    obj.Fdob = (objBaseSqlManager.GetTextValue(dr, "Fdob"));
                    obj.FUIDAI = objBaseSqlManager.GetInt64(dr, "FUIDAI");
                    obj.FRelation = objBaseSqlManager.GetTextValue(dr, "FRelation");
                    obj.Flivingtogether = objBaseSqlManager.GetTextValue(dr, "Flivingtogether");
                    obj.MName = objBaseSqlManager.GetTextValue(dr, "MName");
                    obj.Mdob = (objBaseSqlManager.GetTextValue(dr, "Mdob"));
                    obj.MUIDAI = objBaseSqlManager.GetInt64(dr, "MUIDAI");
                    obj.MRelation = objBaseSqlManager.GetTextValue(dr, "MRelation");
                    obj.Mlivingtogether = objBaseSqlManager.GetTextValue(dr, "Mlivingtogether");
                    obj.WName = objBaseSqlManager.GetTextValue(dr, "WName");
                    obj.Wdob = (objBaseSqlManager.GetTextValue(dr, "Wdob"));
                    obj.WUIDAI = objBaseSqlManager.GetInt64(dr, "WUIDAI");
                    obj.WRelation = objBaseSqlManager.GetTextValue(dr, "WRelation");
                    obj.Wlivingtogether = objBaseSqlManager.GetTextValue(dr, "Wlivingtogether");
                    obj.C1Name = objBaseSqlManager.GetTextValue(dr, "C1Name");
                    obj.C1dob = (objBaseSqlManager.GetTextValue(dr, "C1dob"));
                    obj.C1UIDAI = objBaseSqlManager.GetInt64(dr, "C1UIDAI");
                    obj.C1Relation = objBaseSqlManager.GetTextValue(dr, "C1Relation");
                    obj.C1livingtogether = objBaseSqlManager.GetTextValue(dr, "C1livingtogether");
                    obj.C2Name = objBaseSqlManager.GetTextValue(dr, "C2Name");
                    obj.C2dob = (objBaseSqlManager.GetTextValue(dr, "C2dob"));
                    obj.C2UIDAI = objBaseSqlManager.GetInt64(dr, "C2UIDAI");
                    obj.C2Relation = objBaseSqlManager.GetTextValue(dr, "C2Relation");
                    obj.C2livingtogether = objBaseSqlManager.GetTextValue(dr, "C2livingtogether");
                    obj.C3Name = objBaseSqlManager.GetTextValue(dr, "C3Name");
                    obj.C3dob = (objBaseSqlManager.GetTextValue(dr, "C3dob"));
                    obj.C3UIDAI = objBaseSqlManager.GetInt64(dr, "C3UIDAI");
                    obj.C3Relation = objBaseSqlManager.GetTextValue(dr, "C3Relation");
                    obj.C3livingtogether = objBaseSqlManager.GetTextValue(dr, "C3livingtogether");
                    obj.Godown = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.Maritalstatus = objBaseSqlManager.GetTextValue(dr, "Maritalstatus");
                    if (obj.DateOfJoining < DateTime.Now)
                    {
                        obj.ServiceTime = GetAge(obj.DateOfJoining);
                    }
                    else
                    {
                        obj.ServiceTime = null;
                    }


                    obj.FSSAIDoctorCertificateValidity = objBaseSqlManager.GetDateTime(dr, "FSSAIDoctorCertificateValidity");
                    if (obj.FSSAIDoctorCertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FSSAIDoctorCertificateValiditystr = obj.FSSAIDoctorCertificateValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FSSAIDoctorCertificateValiditystr = "";
                    }


                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.ProfilePicture = objBaseSqlManager.GetTextValue(dr, "ProfilePicture");
                    obj.ProfilePicturePath = "../../../ProfilePicture/" + obj.ProfilePicture;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        public string GetUserOldImage(long UserID)
        {
            User ImageName = new User();
            string image = string.Empty;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    ImageName = context.Users.Where(i => i.Id == UserID).FirstOrDefault();
                }
                if (ImageName != null)
                {
                    image = ImageName.ProfilePicture;
                }
            }
            catch (Exception ex) { }
            return image;
        }




        public string GetUserOldFSSAIDoctorCertificate(long UserID)
        {
            User FSSAIDoctorCertificate = new User();
            string FSSAICerti = string.Empty;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    FSSAIDoctorCertificate = context.Users.Where(i => i.Id == UserID).FirstOrDefault();
                }
                if (FSSAIDoctorCertificate != null)
                {
                    FSSAICerti = FSSAIDoctorCertificate.FSSAIDoctorCertificate;
                }
            }
            catch (Exception ex) { }
            return FSSAICerti;
        }

        public bool AddDocuments(Document_Master Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.DocumentID == 0)
                {
                    context.Document_Master.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.DocumentID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public GetDocumentsResponse GetDocumentsByEmployeeID(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDocumentsByEmployeeID";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetDocumentsResponse obj = new GetDocumentsResponse();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                        obj.DocumentID = objBaseSqlManager.GetInt64(dr, "DocumentID");
                        obj.AadharCard = objBaseSqlManager.GetTextValue(dr, "AadharCard");
                        obj.BankPassBook = objBaseSqlManager.GetTextValue(dr, "BankPassBook");
                        obj.BioData = objBaseSqlManager.GetTextValue(dr, "BioData");
                        obj.ElectionCard = objBaseSqlManager.GetTextValue(dr, "ElectionCard");
                        obj.DrivingLicence = objBaseSqlManager.GetTextValue(dr, "DrivingLicence");
                        obj.ElectricityBill = objBaseSqlManager.GetTextValue(dr, "ElectricityBill");
                        obj.IDCard = objBaseSqlManager.GetTextValue(dr, "IDCard");
                        obj.LeavingCertificate = objBaseSqlManager.GetTextValue(dr, "LeavingCertificate");
                        obj.PanCard = objBaseSqlManager.GetTextValue(dr, "PanCard");
                        obj.RationCard = objBaseSqlManager.GetTextValue(dr, "RationCard");
                        obj.Rentagreement = objBaseSqlManager.GetTextValue(dr, "Rentagreement");
                        obj.Photo = objBaseSqlManager.GetTextValue(dr, "Photo");
                        obj.Signechar = objBaseSqlManager.GetTextValue(dr, "Signechar");
                        obj.FamilyPhoto = objBaseSqlManager.GetTextValue(dr, "FamilyPhoto");
                        obj.Passport = objBaseSqlManager.GetTextValue(dr, "Passport");
                        obj.Other1 = objBaseSqlManager.GetTextValue(dr, "Other1");
                        obj.Other2 = objBaseSqlManager.GetTextValue(dr, "Other2");
                        obj.Other3 = objBaseSqlManager.GetTextValue(dr, "Other3");
                        obj.ESICCard = objBaseSqlManager.GetTextValue(dr, "ESICCard");
                        obj.ESIPehchanCard = objBaseSqlManager.GetTextValue(dr, "ESIPehchanCard");
                        obj.MedicalFitnessCirtificate = objBaseSqlManager.GetTextValue(dr, "MedicalFitnessCirtificate");
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetDocumentsResponse GetUploadedDocumentsFullPathListByEmployeeID(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUploadedDocumentsFullPathListByEmployeeID";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetDocumentsResponse obj = new GetDocumentsResponse();
                string path = ConfigurationManager.AppSettings["Document"];
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        obj.DocumentID = objBaseSqlManager.GetInt64(dr, "DocumentID");
                        obj.AadharCard = path + objBaseSqlManager.GetTextValue(dr, "AadharCard");
                        obj.BankPassBook = path + objBaseSqlManager.GetTextValue(dr, "BankPassBook");
                        obj.BioData = path + objBaseSqlManager.GetTextValue(dr, "BioData");
                        obj.ElectionCard = path + objBaseSqlManager.GetTextValue(dr, "ElectionCard");
                        obj.DrivingLicence = path + objBaseSqlManager.GetTextValue(dr, "DrivingLicence");
                        obj.ElectricityBill = path + objBaseSqlManager.GetTextValue(dr, "ElectricityBill");
                        obj.IDCard = path + objBaseSqlManager.GetTextValue(dr, "IDCard");
                        obj.LeavingCertificate = path + objBaseSqlManager.GetTextValue(dr, "LeavingCertificate");
                        obj.PanCard = path + objBaseSqlManager.GetTextValue(dr, "PanCard");
                        obj.RationCard = path + objBaseSqlManager.GetTextValue(dr, "RationCard");
                        obj.Rentagreement = path + objBaseSqlManager.GetTextValue(dr, "Rentagreement");
                        obj.Photo = path + objBaseSqlManager.GetTextValue(dr, "Photo");
                        obj.Signechar = path + objBaseSqlManager.GetTextValue(dr, "Signechar");
                        obj.FamilyPhoto = path + objBaseSqlManager.GetTextValue(dr, "FamilyPhoto");
                        obj.Passport = path + objBaseSqlManager.GetTextValue(dr, "Passport");
                        obj.Other1 = path + objBaseSqlManager.GetTextValue(dr, "Other1");
                        obj.Other2 = path + objBaseSqlManager.GetTextValue(dr, "Other2");
                        obj.Other3 = path + objBaseSqlManager.GetTextValue(dr, "Other3");
                        obj.ESICCard = path + objBaseSqlManager.GetTextValue(dr, "ESICCard");
                        obj.ESIPehchanCard = path + objBaseSqlManager.GetTextValue(dr, "ESIPehchanCard");
                        obj.MedicalFitnessCirtificate = path + objBaseSqlManager.GetTextValue(dr, "MedicalFitnessCirtificate");

                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public bool DeleteUser(long UserID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteUser";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
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
                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        public bool AddDriver(Driver_Mst ObjDriver)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjDriver.DriverID == 0)
                {
                    context.Driver_Mst.Add(ObjDriver);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjDriver).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjDriver.DriverID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<DriverListResponse> GetAllDriverList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllDriverList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DriverListResponse> objlst = new List<DriverListResponse>();
                while (dr.Read())
                {
                    DriverListResponse objDriverList = new DriverListResponse();
                    objDriverList.DriverID = objBaseSqlManager.GetInt64(dr, "DriverID");
                    objDriverList.DriverName = objBaseSqlManager.GetTextValue(dr, "DriverName");
                    objDriverList.TempoNumber = objBaseSqlManager.GetTextValue(dr, "TempoNumber");
                    objDriverList.DriverMobileNumber = objBaseSqlManager.GetTextValue(dr, "DriverMobileNumber");
                    objDriverList.Licence = objBaseSqlManager.GetTextValue(dr, "Licence");
                    objDriverList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteDriver(long DriverID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteDriver";
                cmdGet.Parameters.AddWithValue("@DriverID", DriverID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<EventListResponse> GetAllEventName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEventName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EventListResponse> lstGodown = new List<EventListResponse>();
                while (dr.Read())
                {
                    EventListResponse objGodown = new EventListResponse();
                    objGodown.EventID = objBaseSqlManager.GetInt64(dr, "EventID");
                    objGodown.EventName = objBaseSqlManager.GetTextValue(dr, "EventName");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        public List<MainMenu> GetMenuList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetMenuList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MainMenu> objlst = new List<MainMenu>();
                while (dr.Read())
                {
                    MainMenu objAreaList = new MainMenu();
                    objAreaList.Id = objBaseSqlManager.GetInt32(dr, "MenuID");
                    objAreaList.Name = objBaseSqlManager.GetTextValue(dr, "DisplayName");
                    objAreaList.MainTier = objBaseSqlManager.GetInt32(dr, "MainTier");
                    objAreaList.SubTier = objBaseSqlManager.GetInt32(dr, "SubTier");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool AddAuthority(AuthorizeMaster ObjArea)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjArea.AuthorizeID == 0)
                {
                    context.AuthorizeMasters.Add(ObjArea);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjArea).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjArea.AuthorizeID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public AuthorizeMaster GetExistAuthorityDetail(int RoleID, long MenuID)
        {
            AuthorizeMaster obj = new AuthorizeMaster();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExistAuthorityDetail";
                cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                cmdGet.Parameters.AddWithValue("@MenuID", MenuID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    obj.AuthorizeID = objBaseSqlManager.GetInt64(dr, "AuthorizeID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public AuthorizeMaster GetLastAuthority()
        {
            AuthorizeMaster obj = new AuthorizeMaster();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastAuthority";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    obj.AuthorizeID = objBaseSqlManager.GetInt64(dr, "AuthorizeID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public bool UpdateAuthorityMaster(long AuthorizeID, bool IsActive)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                try
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateAuthorityMaster";
                        cmdGet.Parameters.AddWithValue("@AuthorizeID", AuthorizeID);
                        cmdGet.Parameters.AddWithValue("@IsActive", IsActive);
                        object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                catch
                {

                }
                return true;
            }
        }

        //public List<Register> Guser()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "Guser";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<Register> objlst = new List<Register>();
        //    while (dr.Read())
        //    {
        //        Register objAreaList = new Register();
        //        objAreaList.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
        //        objAreaList.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
        //        objAreaList.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
        //        objAreaList.Password = objBaseSqlManager.GetTextValue(dr, "Password");
        //        objAreaList.RoleID = objBaseSqlManager.GetInt32(dr, "RoleID");
        //        objAreaList.UserCode = objBaseSqlManager.GetTextValue(dr, "UserCode");
        //        objAreaList.UserEmail = objBaseSqlManager.GetTextValue(dr, "UserEmail");
        //        objAreaList.UserMobile = objBaseSqlManager.GetTextValue(dr, "UserMobile");
        //        objAreaList.UserPhone = objBaseSqlManager.GetTextValue(dr, "UserPhone");
        //        objAreaList.UserPhoneExtn = objBaseSqlManager.GetTextValue(dr, "UserPhoneExtn");
        //        objAreaList.UserDesignation = objBaseSqlManager.GetTextValue(dr, "UserDesignation");
        //        objAreaList.UserDepartment = objBaseSqlManager.GetTextValue(dr, "UserDepartment");
        //        objAreaList.GodownID = objBaseSqlManager.GetInt32(dr, "GodownID");
        //        objAreaList.UserLocation = objBaseSqlManager.GetTextValue(dr, "UserLocation");
        //        objAreaList.UserRemark = objBaseSqlManager.GetTextValue(dr, "UserRemark");
        //        objAreaList.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
        //        objAreaList.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
        //        objAreaList.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
        //        objAreaList.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
        //        objAreaList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
        //        objlst.Add(objAreaList);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return objlst;
        //}

        public List<Register> Guser()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "Guser";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<Register> objlst = new List<Register>();
                while (dr.Read())
                {
                    Register objAreaList = new Register();
                    objAreaList.UserID = objBaseSqlManager.GetInt32(dr, "UserID");
                    //  objAreaList.EmployeeCode = objBaseSqlManager.GetTextValue(dr, "EmployeeCode");
                    objAreaList.EmployeeCode = objBaseSqlManager.GetInt32(dr, "EmployeeCode");
                    objAreaList.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    objAreaList.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objAreaList.Password = objBaseSqlManager.GetTextValue(dr, "Password");
                    objAreaList.RoleID = objBaseSqlManager.GetInt64(dr, "RoleID");
                    objAreaList.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    objAreaList.Age = objBaseSqlManager.GetTextValue(dr, "Age");
                    objAreaList.Gender = objBaseSqlManager.GetTextValue(dr, "Gender");
                    objAreaList.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    objAreaList.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objAreaList.Address = objBaseSqlManager.GetTextValue(dr, "Address");
                    objAreaList.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objAreaList.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    objAreaList.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objAreaList.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    objAreaList.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objAreaList.PrimaryArea = objBaseSqlManager.GetInt64(dr, "PrimaryArea");
                    objAreaList.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    objAreaList.PrimaryPin = objBaseSqlManager.GetInt64(dr, "PrimaryPin");
                    objAreaList.SecondaryArea = objBaseSqlManager.GetInt32(dr, "SecondaryArea");
                    objAreaList.SecondaryAddress = objBaseSqlManager.GetTextValue(dr, "SecondaryAddress");
                    objAreaList.SecondaryPin = objBaseSqlManager.GetInt64(dr, "SecondaryPin");
                    objAreaList.PanNo = objBaseSqlManager.GetTextValue(dr, "PanNo");
                    objAreaList.PassportNo = objBaseSqlManager.GetTextValue(dr, "PassportNo");
                    objAreaList.PassportValiddate = objBaseSqlManager.GetDateTime(dr, "PassportValiddate");
                    objAreaList.UIDAI = objBaseSqlManager.GetInt64(dr, "UIDAI");
                    objAreaList.UAN = objBaseSqlManager.GetInt64(dr, "UAN");
                    objAreaList.PF = objBaseSqlManager.GetTextValue(dr, "PF");
                    objAreaList.ESIC = objBaseSqlManager.GetTextValue(dr, "ESIC");
                    objAreaList.Drivinglicence = objBaseSqlManager.GetTextValue(dr, "Drivinglicence");
                    objAreaList.DrivingValidup = objBaseSqlManager.GetDateTime(dr, "DrivingValidup");
                    objAreaList.ReferenceName = objBaseSqlManager.GetTextValue(dr, "ReferenceName");
                    objAreaList.FName = objBaseSqlManager.GetTextValue(dr, "FName");
                    objAreaList.Fdob = objBaseSqlManager.GetDateTime(dr, "Fdob");
                    objAreaList.FUIDAI = objBaseSqlManager.GetInt64(dr, "FUIDAI");
                    objAreaList.FRelation = objBaseSqlManager.GetTextValue(dr, "FRelation");
                    objAreaList.Flivingtogether = objBaseSqlManager.GetTextValue(dr, "Flivingtogether");
                    objAreaList.MName = objBaseSqlManager.GetTextValue(dr, "MName");
                    objAreaList.Mdob = objBaseSqlManager.GetDateTime(dr, "Mdob");
                    objAreaList.MUIDAI = objBaseSqlManager.GetInt64(dr, "MUIDAI");
                    objAreaList.MRelation = objBaseSqlManager.GetTextValue(dr, "MRelation");
                    objAreaList.Mlivingtogether = objBaseSqlManager.GetTextValue(dr, "Mlivingtogether");
                    objAreaList.WName = objBaseSqlManager.GetTextValue(dr, "WName");
                    objAreaList.Wdob = objBaseSqlManager.GetDateTime(dr, "Wdob");
                    objAreaList.WUIDAI = objBaseSqlManager.GetInt64(dr, "WUIDAI");
                    objAreaList.WRelation = objBaseSqlManager.GetTextValue(dr, "WRelation");
                    objAreaList.Wlivingtogether = objBaseSqlManager.GetTextValue(dr, "Wlivingtogether");
                    objAreaList.C1Name = objBaseSqlManager.GetTextValue(dr, "C1Name");
                    objAreaList.C1dob = objBaseSqlManager.GetDateTime(dr, "C1dob");
                    objAreaList.C1UIDAI = objBaseSqlManager.GetInt64(dr, "C1UIDAI");
                    objAreaList.C1Relation = objBaseSqlManager.GetTextValue(dr, "C1Relation");
                    objAreaList.C1livingtogether = objBaseSqlManager.GetTextValue(dr, "C1livingtogether");
                    objAreaList.C2Name = objBaseSqlManager.GetTextValue(dr, "C2Name");
                    objAreaList.C2dob = objBaseSqlManager.GetDateTime(dr, "C2dob");
                    objAreaList.C2UIDAI = objBaseSqlManager.GetInt64(dr, "C2UIDAI");
                    objAreaList.C2Relation = objBaseSqlManager.GetTextValue(dr, "C2Relation");
                    objAreaList.C2livingtogether = objBaseSqlManager.GetTextValue(dr, "C2livingtogether");
                    objAreaList.C3Name = objBaseSqlManager.GetTextValue(dr, "C3Name");
                    objAreaList.C3dob = objBaseSqlManager.GetDateTime(dr, "C3dob");
                    objAreaList.C3UIDAI = objBaseSqlManager.GetInt64(dr, "C3UIDAI");
                    objAreaList.C3Relation = objBaseSqlManager.GetTextValue(dr, "C3Relation");
                    objAreaList.C3livingtogether = objBaseSqlManager.GetTextValue(dr, "C3livingtogether");
                    objAreaList.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objAreaList.ServiceTime = objBaseSqlManager.GetTextValue(dr, "ServiceTime");
                    objAreaList.Maritalstatus = objBaseSqlManager.GetTextValue(dr, "Maritalstatus");
                    objAreaList.ProfilePicture = objBaseSqlManager.GetTextValue(dr, "ProfilePicture");
                    objAreaList.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objAreaList.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objAreaList.UpdatedBy = objBaseSqlManager.GetInt64(dr, "UpdatedBy");
                    objAreaList.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    objAreaList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public LoginResponse GetUserDetails(string username)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUserDetails";
                cmdGet.Parameters.AddWithValue("@username", username);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                LoginResponse obj = new LoginResponse();
                string path = ConfigurationManager.AppSettings["ProfilePicture"];
                while (dr.Read())
                {
                    obj.UserID = objBaseSqlManager.GetInt32(dr, "Id");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    // obj.UserEmail = objBaseSqlManager.GetTextValue(dr, "UserEmail");
                    obj.UserEmail = objBaseSqlManager.GetTextValue(dr, "Email");
                    obj.RoleID = objBaseSqlManager.GetInt32(dr, "RoleID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.ProfilePicture = objBaseSqlManager.GetTextValue(dr, "ProfilePicture");
                    obj.ProfilePicturePath = path + objBaseSqlManager.GetTextValue(dr, "ProfilePicture");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<InvoiceTotal> GetInvoice()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoice";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<InvoiceTotal> objlst = new List<InvoiceTotal>();
                while (dr.Read())
                {
                    InvoiceTotal objAreaList = new InvoiceTotal();
                    objAreaList.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objAreaList.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objAreaList.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetInvoiceTotal> GetRetInvoice()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetInvoice";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetInvoiceTotal> objlst = new List<RetInvoiceTotal>();
                while (dr.Read())
                {
                    RetInvoiceTotal objAreaList = new RetInvoiceTotal();
                    objAreaList.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objAreaList.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objAreaList.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool AddTransport(Transport_Mst ObjTransport)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjTransport.TransportID == 0)
                {
                    context.Transport_Mst.Add(ObjTransport);
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

        public List<TransportListResponse> GetAllTransportList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTransportList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TransportListResponse> objlst = new List<TransportListResponse>();
                while (dr.Read())
                {
                    TransportListResponse objTransportList = new TransportListResponse();
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
                cmdGet.CommandText = "DeleteTransport";
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);

                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public Register Users_SelectByUserIDProfile(long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUsers_SelectByUserIDProfile";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                Register objEntity = new Register();
                while (dr.Read())
                {
                    objEntity.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objEntity.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objEntity;
            }
        }

        public bool UpdatePassword(long UserID, string NewPassword)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdatePassword";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@NewPassword", NewPassword);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public bool AddVehicleDetail(VehicleDetail_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.VehicleDetailID == 0)
                {
                    context.VehicleDetail_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.VehicleDetailID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<VehicleListResponse> GetAllVehicleList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleListResponse> objlst = new List<VehicleListResponse>();
                string path = ConfigurationManager.AppSettings["VehicleDoc"];
                while (dr.Read())
                {
                    VehicleListResponse obj = new VehicleListResponse();
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.DateOfPurchase = objBaseSqlManager.GetDateTime(dr, "DateOfPurchase");
                    if (obj.DateOfPurchase != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfPurchasestr = obj.DateOfPurchase.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfPurchasestr = "";
                    }
                    obj.RCCertificate = objBaseSqlManager.GetTextValue(dr, "RCCertificate");
                    if (obj.RCCertificate != "")
                    {
                        obj.RCCertificatepath = path + obj.RCCertificate;
                    }
                    else
                    {
                        obj.RCCertificatepath = "";
                    }
                    obj.RCNoValidity = objBaseSqlManager.GetDateTime(dr, "RCNoValidity");
                    if (obj.RCNoValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RCNoValiditystr = obj.RCNoValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RCNoValiditystr = "";
                    }
                    obj.FitnessCertificate = objBaseSqlManager.GetTextValue(dr, "FitnessCertificate");
                    if (obj.FitnessCertificate != "")
                    {
                        obj.FitnessCertificatepath = path + obj.FitnessCertificate;
                    }
                    else
                    {
                        obj.FitnessCertificatepath = "";
                    }
                    obj.FitnessValidity = objBaseSqlManager.GetDateTime(dr, "FitnessValidity");
                    if (obj.FitnessValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FitnessValiditystr = obj.FitnessValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FitnessValiditystr = "";
                    }
                    obj.PermitCertificate = objBaseSqlManager.GetTextValue(dr, "PermitCertificate");
                    if (obj.PermitCertificate != "")
                    {
                        obj.PermitCertificatepath = path + obj.PermitCertificate;
                    }
                    else
                    {
                        obj.PermitCertificatepath = "";
                    }
                    obj.PermitValidity = objBaseSqlManager.GetDateTime(dr, "PermitValidity");
                    if (obj.PermitValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PermitValiditystr = obj.PermitValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.PermitValiditystr = "";
                    }
                    obj.PUCCertificate = objBaseSqlManager.GetTextValue(dr, "PUCCertificate");
                    if (obj.PUCCertificate != "")
                    {
                        obj.PUCCertificatepath = path + obj.PUCCertificate;
                    }
                    else
                    {
                        obj.PUCCertificatepath = "";
                    }
                    obj.PUCValidity = objBaseSqlManager.GetDateTime(dr, "PUCValidity");
                    if (obj.PUCValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PUCValiditystr = obj.PUCValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.PUCValiditystr = "";
                    }
                    obj.InsuranceCertificate = objBaseSqlManager.GetTextValue(dr, "InsuranceCertificate");
                    if (obj.InsuranceCertificate != "")
                    {
                        obj.InsuranceCertificatepath = path + obj.InsuranceCertificate;
                    }
                    else
                    {
                        obj.InsuranceCertificatepath = "";
                    }
                    obj.InsuranceValidity = objBaseSqlManager.GetDateTime(dr, "InsuranceValidity");
                    if (obj.InsuranceValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.InsuranceValiditystr = obj.InsuranceValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.InsuranceValiditystr = "";
                    }
                    obj.AdvertisementCertificate = objBaseSqlManager.GetTextValue(dr, "AdvertisementCertificate");
                    if (obj.AdvertisementCertificate != "")
                    {
                        obj.AdvertisementCertificatepath = path + obj.AdvertisementCertificate;
                    }
                    else
                    {
                        obj.AdvertisementCertificatepath = "";
                    }

                    obj.AdvertisementValidity = objBaseSqlManager.GetDateTime(dr, "AdvertisementValidity");
                    if (obj.AdvertisementValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.AdvertisementValiditystr = obj.AdvertisementValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.AdvertisementValiditystr = "";
                    }
                    obj.SpeedGovernorCertificate = objBaseSqlManager.GetTextValue(dr, "SpeedGovernorCertificate");
                    if (obj.SpeedGovernorCertificate != "")
                    {
                        obj.SpeedGovernorCertificatepath = path + obj.SpeedGovernorCertificate;
                    }
                    else
                    {
                        obj.SpeedGovernorCertificatepath = "";
                    }
                    obj.SpeedGovernorCertificateValidity = objBaseSqlManager.GetDateTime(dr, "SpeedGovernorCertificateValidity");
                    if (obj.SpeedGovernorCertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.SpeedGovernorCertificateValiditystr = obj.SpeedGovernorCertificateValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.SpeedGovernorCertificateValiditystr = "";
                    }
                    obj.FSSAICertificate = objBaseSqlManager.GetTextValue(dr, "FSSAICertificate");
                    if (obj.FSSAICertificate != "")
                    {
                        obj.FSSAICertificatepath = path + obj.FSSAICertificate;
                    }
                    else
                    {
                        obj.FSSAICertificatepath = "";
                    }
                    obj.FSSAICertificateValidity = objBaseSqlManager.GetDateTime(dr, "FSSAICertificateValidity");
                    if (obj.FSSAICertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FSSAICertificateValiditystr = obj.FSSAICertificateValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FSSAICertificateValiditystr = "";
                    }



                    obj.GreenTaxCertificate = objBaseSqlManager.GetTextValue(dr, "GreenTaxCertificate");
                    if (obj.GreenTaxCertificate != "")
                    {
                        obj.GreenTaxCertificatepath = path + obj.GreenTaxCertificate;
                    }
                    else
                    {
                        obj.GreenTaxCertificatepath = "";
                    }
                    obj.GreenTaxCertificateValidity = objBaseSqlManager.GetDateTime(dr, "GreenTaxCertificateValidity");
                    if (obj.GreenTaxCertificateValidity != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.GreenTaxCertificateValiditystr = obj.GreenTaxCertificateValidity.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.GreenTaxCertificateValiditystr = "";
                    }




                    obj.InstallmentAmount = objBaseSqlManager.GetDecimal(dr, "InstallmentAmount");
                    obj.InstallmentAmountDate = objBaseSqlManager.GetDateTime(dr, "InstallmentAmountDate");
                    if (obj.InstallmentAmountDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.InstallmentAmountDatestr = obj.InstallmentAmountDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.InstallmentAmountDatestr = "";

                    }
                    obj.OneTimeTax = objBaseSqlManager.GetDecimal(dr, "OneTimeTax");
                    obj.OneTimeTaxDate = objBaseSqlManager.GetDateTime(dr, "OneTimeTaxDate");
                    if (obj.OneTimeTaxDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OneTimeTaxDatestr = obj.OneTimeTaxDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.OneTimeTaxDatestr = "";

                    }
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteVehicle(long VehicleDetailID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteVehicle";
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public GetVehicleCertificate GetVehicleCertificateByVehicleDetailID(long VehicleDetailID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleCertificateByVehicleDetailID";
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleCertificate obj = new GetVehicleCertificate();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                        obj.RCCertificate = objBaseSqlManager.GetTextValue(dr, "RCCertificate");
                        obj.FitnessCertificate = objBaseSqlManager.GetTextValue(dr, "FitnessCertificate");
                        obj.PermitCertificate = objBaseSqlManager.GetTextValue(dr, "PermitCertificate");
                        obj.PUCCertificate = objBaseSqlManager.GetTextValue(dr, "PUCCertificate");
                        obj.InsuranceCertificate = objBaseSqlManager.GetTextValue(dr, "InsuranceCertificate");
                        obj.AdvertisementCertificate = objBaseSqlManager.GetTextValue(dr, "AdvertisementCertificate");
                        obj.SpeedGovernorCertificate = objBaseSqlManager.GetTextValue(dr, "SpeedGovernorCertificate");
                        obj.FSSAICertificate = objBaseSqlManager.GetTextValue(dr, "FSSAICertificate");
                        obj.GreenTaxCertificate = objBaseSqlManager.GetTextValue(dr, "GreenTaxCertificate");
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public bool AddLicenceDetails(Licence_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.LicenceID == 0)
                {
                    context.Licence_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.LicenceID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<LicenceListResponse> GetAllLicenceList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllLicenceList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LicenceListResponse> objlst = new List<LicenceListResponse>();
                string path = ConfigurationManager.AppSettings["VehicleDoc"];
                while (dr.Read())
                {
                    LicenceListResponse obj = new LicenceListResponse();
                    obj.LicenceID = objBaseSqlManager.GetInt64(dr, "LicenceID");
                    obj.LicenceType = objBaseSqlManager.GetTextValue(dr, "LicenceType");
                    obj.WhereFrom = objBaseSqlManager.GetTextValue(dr, "WhereFrom");
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    if (obj.FromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FromDatestr = obj.FromDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FromDatestr = "";
                    }

                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");

                    if (obj.ToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ToDatestr = obj.ToDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ToDatestr = "";
                    }


                    obj.Remark = objBaseSqlManager.GetTextValue(dr, "Remark");

                    obj.Documents = objBaseSqlManager.GetTextValue(dr, "Documents");
                    if (obj.Documents != "")
                    {
                        obj.Documentspath = path + obj.Documents;
                    }
                    else
                    {
                        obj.Documentspath = "";
                    }
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public LicenceListResponse GetLicenceDocByLicenceID(long LicenceID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLicenceDocByLicenceID";
                cmdGet.Parameters.AddWithValue("@LicenceID", LicenceID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                LicenceListResponse obj = new LicenceListResponse();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        obj.LicenceID = objBaseSqlManager.GetInt64(dr, "LicenceID");
                        obj.Documents = objBaseSqlManager.GetTextValue(dr, "Documents");
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public bool DeleteLicence(long LicenceID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteLicence";
                cmdGet.Parameters.AddWithValue("@LicenceID", LicenceID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //public List<UpdateVehicleAssignedDate> GetRetVehicleAssignedDate()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetRetVehicleAssignedDate";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<UpdateVehicleAssignedDate> objlst = new List<UpdateVehicleAssignedDate>();
        //    while (dr.Read())
        //    {
        //        UpdateVehicleAssignedDate objAreaList = new UpdateVehicleAssignedDate();
        //        objAreaList.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
        //        objAreaList.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
        //        objlst.Add(objAreaList);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return objlst;
        //}
        //public void UpdateRetVehicleAssignedDate(string InvoiceNumber, DateTime CreatedOn)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "UpdateRetVehicleAssignedDate";
        //    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
        //    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
        //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //    objBaseSqlManager.ForceCloseConnection();
        //}

        public List<UpdateVehicleAssignedDate> GetRetOrderQtyID()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetOrderQtyID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UpdateVehicleAssignedDate> objlst = new List<UpdateVehicleAssignedDate>();
                while (dr.Read())
                {
                    UpdateVehicleAssignedDate objAreaList = new UpdateVehicleAssignedDate();
                    objAreaList.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public UpdateVehicleAssignedDate GetOrderDareandProductQTYID(long OrderQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOrderDareandProductQTYID";
                cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                UpdateVehicleAssignedDate objExpenses = new UpdateVehicleAssignedDate();
                while (dr.Read())
                {
                    objExpenses.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    // objExpenses.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objExpenses;
            }
        }

        //public void UpdateRetOrderDareandProductQTYID(long OrderQtyID, long ProductQtyID, DateTime OrderDate)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "UpdateRetOrderDareandProductQTYID";
        //    cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
        //    cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
        //    cmdGet.Parameters.AddWithValue("@OrderDate", OrderDate);
        //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //    objBaseSqlManager.ForceCloseConnection();
        //}

        public void UpdateRetOrderDareandProductQTYID(long OrderQtyID, DateTime OrderDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetOrderDareandProductQTYID";
                cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
                cmdGet.Parameters.AddWithValue("@OrderDate", OrderDate);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        //  Purchase Admin
        public long AddPurchaseType(PurchaseType_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.PurchaseTypeID == 0)
                {
                    context.PurchaseType_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.PurchaseTypeID > 0)
                {
                    return Obj.PurchaseTypeID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PurchaseTypeListResponse> GetAllPurchaseTypeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseTypeList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseTypeListResponse> objlst = new List<PurchaseTypeListResponse>();
                while (dr.Read())
                {
                    PurchaseTypeListResponse obj = new PurchaseTypeListResponse();
                    obj.PurchaseTypeID = objBaseSqlManager.GetInt64(dr, "PurchaseTypeID");
                    obj.PurchaseType = objBaseSqlManager.GetTextValue(dr, "PurchaseType");
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

        public bool DeletePurchaseType(long PurchaseTypeID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePurchaseType";
                cmdGet.Parameters.AddWithValue("@PurchaseTypeID", PurchaseTypeID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddPurchaseDebitAccountType(PurchaseDebitAccountType_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.PurchaseDebitAccountTypeID == 0)
                {
                    context.PurchaseDebitAccountType_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.PurchaseDebitAccountTypeID > 0)
                {
                    return Obj.PurchaseDebitAccountTypeID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PurchaseDebitAccountTypeListResponse> GetAllPurchaseDebitAccountTypeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseDebitAccountTypeList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseDebitAccountTypeListResponse> objlst = new List<PurchaseDebitAccountTypeListResponse>();
                while (dr.Read())
                {
                    PurchaseDebitAccountTypeListResponse obj = new PurchaseDebitAccountTypeListResponse();
                    obj.PurchaseDebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "PurchaseDebitAccountTypeID");
                    obj.PurchaseDebitAccountType = objBaseSqlManager.GetTextValue(dr, "PurchaseDebitAccountType");
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

        public bool DeletePurchaseDebitAccountType(long PurchaseDebitAccountTypeID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePurchaseDebitAccountType";
                cmdGet.Parameters.AddWithValue("@PurchaseDebitAccountTypeID", PurchaseDebitAccountTypeID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddBroker(Broker_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.BrokerID == 0)
                {
                    context.Broker_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.BrokerID > 0)
                {
                    return Obj.BrokerID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<BrokerListResponse> GetAllBrokerList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllBrokerList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BrokerListResponse> objlst = new List<BrokerListResponse>();
                while (dr.Read())
                {
                    BrokerListResponse obj = new BrokerListResponse();
                    obj.BrokerID = objBaseSqlManager.GetInt64(dr, "BrokerID");
                    obj.BrokerName = objBaseSqlManager.GetTextValue(dr, "BrokerName");
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

        public bool DeleteBroker(long BrokerID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteBroker";
                cmdGet.Parameters.AddWithValue("@BrokerID", BrokerID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<PurchaseTypeListResponse> GetAllPurchaseTypeName(long PurchaseTypeID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseTypeName";
                cmdGet.Parameters.AddWithValue("@PurchaseTypeID", PurchaseTypeID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseTypeListResponse> lstGodown = new List<PurchaseTypeListResponse>();
                while (dr.Read())
                {
                    PurchaseTypeListResponse objGodown = new PurchaseTypeListResponse();
                    objGodown.PurchaseTypeID = objBaseSqlManager.GetInt64(dr, "PurchaseTypeID");
                    objGodown.PurchaseType = objBaseSqlManager.GetTextValue(dr, "PurchaseType");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        public List<PurchaseDebitAccountTypeListResponse> GetAllPurchaseDebitAccountTypeName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseDebitAccountTypeName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseDebitAccountTypeListResponse> lstGodown = new List<PurchaseDebitAccountTypeListResponse>();
                while (dr.Read())
                {
                    PurchaseDebitAccountTypeListResponse objGodown = new PurchaseDebitAccountTypeListResponse();
                    objGodown.PurchaseDebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "PurchaseDebitAccountTypeID");
                    objGodown.PurchaseDebitAccountType = objBaseSqlManager.GetTextValue(dr, "PurchaseDebitAccountType");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        public List<BrokerListResponse> GetAllBrokerName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllBrokerName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BrokerListResponse> lstGodown = new List<BrokerListResponse>();
                while (dr.Read())
                {
                    BrokerListResponse objGodown = new BrokerListResponse();
                    objGodown.BrokerID = objBaseSqlManager.GetInt64(dr, "BrokerID");
                    objGodown.BrokerName = objBaseSqlManager.GetTextValue(dr, "BrokerName");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        public long AddBank(Bank_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.BankID == 0)
                {
                    context.Bank_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.BankID > 0)
                {
                    return Obj.BankID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<BankListResponse> GetAllBankList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllBankList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BankListResponse> objlst = new List<BankListResponse>();
                while (dr.Read())
                {
                    BankListResponse obj = new BankListResponse();
                    obj.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.Branch = objBaseSqlManager.GetTextValue(dr, "Branch");
                    obj.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    obj.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
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

        public bool DeleteBank(long BankID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteBank";
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //  Expense Admin 17-12-2019
        public long AddExpenseType(ExpenseType_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ExpenseTypeID == 0)
                {
                    context.ExpenseType_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ExpenseTypeID > 0)
                {
                    return Obj.ExpenseTypeID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ExpenseTypeListResponse> GetAllExpenseTypeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseTypeList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseTypeListResponse> objlst = new List<ExpenseTypeListResponse>();
                while (dr.Read())
                {
                    ExpenseTypeListResponse obj = new ExpenseTypeListResponse();
                    obj.ExpenseTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseTypeID");
                    obj.ExpenseType = objBaseSqlManager.GetTextValue(dr, "ExpenseType");
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

        public bool DeleteExpenseType(long ExpenseTypeID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteExpenseType";
                cmdGet.Parameters.AddWithValue("@ExpenseTypeID", ExpenseTypeID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddExpenseDebitAccountType(ExpenseDebitAccountType_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ExpenseDebitAccountTypeID == 0)
                {
                    context.ExpenseDebitAccountType_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ExpenseDebitAccountTypeID > 0)
                {
                    return Obj.ExpenseDebitAccountTypeID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ExpenseDebitAccountTypeListResponse> GetAllExpenseDebitAccountTypeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseDebitAccountTypeList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseDebitAccountTypeListResponse> objlst = new List<ExpenseDebitAccountTypeListResponse>();
                while (dr.Read())
                {
                    ExpenseDebitAccountTypeListResponse obj = new ExpenseDebitAccountTypeListResponse();
                    obj.ExpenseDebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseDebitAccountTypeID");
                    obj.ExpenseDebitAccountType = objBaseSqlManager.GetTextValue(dr, "ExpenseDebitAccountType");
                    obj.SGST = objBaseSqlManager.GetDecimal(dr, "SGST");
                    obj.CGST = objBaseSqlManager.GetDecimal(dr, "CGST");
                    obj.IGST = objBaseSqlManager.GetDecimal(dr, "IGST");
                    obj.HFor = objBaseSqlManager.GetDecimal(dr, "HFor");
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

        public bool DeleteExpenseDebitAccountType(long ExpenseDebitAccountTypeID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteExpenseDebitAccountType";
                cmdGet.Parameters.AddWithValue("@ExpenseDebitAccountTypeID", ExpenseDebitAccountTypeID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<ExpenseTypeListResponse> GetAllExpenseTypeName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseTypeName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseTypeListResponse> lstGodown = new List<ExpenseTypeListResponse>();
                while (dr.Read())
                {
                    ExpenseTypeListResponse objGodown = new ExpenseTypeListResponse();
                    objGodown.ExpenseTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseTypeID");
                    objGodown.ExpenseType = objBaseSqlManager.GetTextValue(dr, "ExpenseType");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        public List<ExpenseDebitAccountTypeListResponse> GetAllExpenseDebitAccountTypeName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseDebitAccountTypeName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseDebitAccountTypeListResponse> lstGodown = new List<ExpenseDebitAccountTypeListResponse>();
                while (dr.Read())
                {
                    ExpenseDebitAccountTypeListResponse objGodown = new ExpenseDebitAccountTypeListResponse();
                    objGodown.ExpenseDebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseDebitAccountTypeID");
                    objGodown.ExpenseDebitAccountType = objBaseSqlManager.GetTextValue(dr, "ExpenseDebitAccountType");
                    lstGodown.Add(objGodown);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstGodown;
            }
        }

        // Utility_Master 02-03-2020
        public bool AddUtility(Utility_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.UtilityID == 0)
                {
                    context.Utility_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.UtilityID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<UtilityListResponse> GetAllUtilityList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUtilityList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityListResponse> objlst = new List<UtilityListResponse>();
                while (dr.Read())
                {
                    UtilityListResponse obj = new UtilityListResponse();
                    obj.UtilityID = objBaseSqlManager.GetInt64(dr, "UtilityID");
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.UtilityDescription = objBaseSqlManager.GetTextValue(dr, "UtilityDescription");
                    obj.UtilityQuantity = objBaseSqlManager.GetInt32(dr, "UtilityQuantity");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.MinUtilityQuantity = objBaseSqlManager.GetInt64(dr, "MinUtilityQuantity");
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

        public bool DeleteUtility(long UtilityID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteUtility";
                cmdGet.Parameters.AddWithValue("@UtilityID", UtilityID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //public List<UtilityListResponse> GetAllUtilityName()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetAllUtilityName";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<UtilityListResponse> lstCategory = new List<UtilityListResponse>();
        //    while (dr.Read())
        //    {
        //        UtilityListResponse obj = new UtilityListResponse();
        //        obj.UtilityID = objBaseSqlManager.GetInt64(dr, "UtilityID");
        //        obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
        //        lstCategory.Add(obj);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return lstCategory;
        //}

        // 03-04-2020 - barcode history
        public List<EmployeeName> GetAllEmployeeName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEmployeeNameForBarcode";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EmployeeName> lstProduct = new List<EmployeeName>();
                while (dr.Read())
                {
                    EmployeeName objProductresponse = new EmployeeName();
                    objProductresponse.EmployeeID = objBaseSqlManager.GetInt64(dr, "Id");
                    objProductresponse.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    lstProduct.Add(objProductresponse);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstProduct;
            }
        }

        // 17 June 2020
        public long AddTDSCategory(TDSCategory_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.TDSCategoryID == 0)
                {
                    context.TDSCategory_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.TDSCategoryID > 0)
                {
                    return Obj.TDSCategoryID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<TDSCategoryListResponse> GetAllTDSCategoryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTDSCategoryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TDSCategoryListResponse> objlst = new List<TDSCategoryListResponse>();
                while (dr.Read())
                {
                    TDSCategoryListResponse obj = new TDSCategoryListResponse();
                    obj.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    obj.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
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

        public bool DeleteTDSCategory(long TDSCategoryID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteTDSCategory";
                cmdGet.Parameters.AddWithValue("@TDSCategoryID", TDSCategoryID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<TDSCategoryName> GetAllTDSCategoryName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTDSCategoryName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TDSCategoryName> lst = new List<TDSCategoryName>();
                while (dr.Read())
                {
                    TDSCategoryName obj = new TDSCategoryName();
                    obj.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    obj.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

        // 08 Aug 2020 Piyush Limbani
        public List<MenuList> GetAllMenuList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllMenuList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MenuList> objlst = new List<MenuList>();
                while (dr.Read())
                {
                    MenuList obj = new MenuList();
                    obj.MenuID = objBaseSqlManager.GetInt32(dr, "MenuID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 08 Aug 2020 Piyush Limbani
        public long CheckMenuForRoleIsExist(int RoleID, long MenuID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckMenuForRoleIsExist";
                cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                cmdGet.Parameters.AddWithValue("@MenuID", MenuID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long AuthorizeID = 0;
                while (dr.Read())
                {
                    AuthorizeID = objBaseSqlManager.GetInt64(dr, "AuthorizeID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return AuthorizeID;
            }
        }

        // For Update Customer Mobile no  Wholeasle       // 19 Aug 2020 Piyush Limbani
        public List<GetCustomerID> GetAllCustomerID()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetCustomerID> objlst = new List<GetCustomerID>();
                while (dr.Read())
                {
                    GetCustomerID objAreaList = new GetCustomerID();
                    objAreaList.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetCustomerID GetCellNoDetail(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCellNoDetail";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetCustomerID objExpenses = new GetCustomerID();
                while (dr.Read())
                {
                    objExpenses.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo");
                    objExpenses.TelNo = objBaseSqlManager.GetTextValue(dr, "TelNo");
                    objExpenses.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objExpenses;
            }
        }

        public bool UpdateCellNoDetaiLByCystomerID(long CustomerID, string CellNo, string TelNo, string Email)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateCellNoDetaiLByCystomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@CellNo", CellNo);
                cmdGet.Parameters.AddWithValue("@TelNo", TelNo);
                cmdGet.Parameters.AddWithValue("@Email", Email);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        // For Update Customer Mobile no  Retail       // 19 Aug 2020 Piyush Limbani
        public List<GetCustomerID> GetAllRetCustomerID()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetCustomerID> objlst = new List<GetCustomerID>();
                while (dr.Read())
                {
                    GetCustomerID objAreaList = new GetCustomerID();
                    objAreaList.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetCustomerID GetRetCellNoDetail(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCellNoDetail";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetCustomerID objExpenses = new GetCustomerID();
                while (dr.Read())
                {
                    objExpenses.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo");
                    objExpenses.TelNo = objBaseSqlManager.GetTextValue(dr, "TelNo");
                    objExpenses.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objExpenses;
            }
        }

        public bool UpdateRetCellNoDetaiLByCystomerID(long CustomerID, string CellNo, string TelNo, string Email)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetCellNoDetaiLByCystomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@CellNo", CellNo);
                cmdGet.Parameters.AddWithValue("@TelNo", TelNo);
                cmdGet.Parameters.AddWithValue("@Email", Email);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        // 21 Sep 2020 Piyush Limbani
        public long AddTCS(TCS_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.TCSID == 0)
                {
                    context.TCS_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.TCSID > 0)
                {
                    return Obj.TCSID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<TCSListResponse> GetAllTCSList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTCSList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TCSListResponse> objlst = new List<TCSListResponse>();
                while (dr.Read())
                {
                    TCSListResponse obj = new TCSListResponse();
                    obj.TCSID = objBaseSqlManager.GetInt64(dr, "TCSID");
                    obj.TaxWithGST = objBaseSqlManager.GetDecimal(dr, "TaxWithGST");
                    obj.TaxWithOutGST = objBaseSqlManager.GetDecimal(dr, "TaxWithOutGST");
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
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

        public bool DeleteTCS(long TCSID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteTCS";
                cmdGet.Parameters.AddWithValue("@TCSID", TCSID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 07 Oct 2020 Piyush Limbani
        public bool AddUtilityName(UtilityName_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.UtilityNameID == 0)
                {
                    context.UtilityName_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.UtilityNameID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<UtilityNameListResponse> GetAllUtilityNameList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUtilityNameList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityNameListResponse> objlst = new List<UtilityNameListResponse>();
                while (dr.Read())
                {
                    UtilityNameListResponse obj = new UtilityNameListResponse();
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
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

        public bool DeleteUtilityName(long UtilityNameID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteUtilityName";
                cmdGet.Parameters.AddWithValue("@UtilityNameID", UtilityNameID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 08 Oct 2020 Piyush Limbani
        public bool AddPouchName(PouchName_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.PouchNameID == 0)
                {
                    context.PouchName_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.PouchNameID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<PouchNameListResponse> GetAllPouchNameList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPouchNameList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchNameListResponse> objlst = new List<PouchNameListResponse>();
                while (dr.Read())
                {
                    PouchNameListResponse obj = new PouchNameListResponse();
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");

                    obj.FontSize = objBaseSqlManager.GetInt64(dr, "FontSize");
                    obj.DelayTime = objBaseSqlManager.GetTextValue(dr, "DelayTime");
                    obj.PouchSize = objBaseSqlManager.GetInt64(dr, "PouchSize");

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

        public bool DeletePouchName(long PouchNameID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePouchName";
                cmdGet.Parameters.AddWithValue("@PouchNameID", PouchNameID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 08 Feb 2021 Piyush Limbani
        public bool Updatewebpages_UsersInRoles(int UserID, long RoleID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "Updatewebpages_UsersInRoles";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }
        // 08 Feb 2021 Piyush Limbani


        // 12 June,2021 Sonal Gandhi
        public bool AddOnline(long AreaID, bool IsOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "AddOnline";
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@IsOnline", IsOnline);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }



        // 16 June 2021 Piyush Limbani
        public long AddPurchaseTDSCategory(PurchaseTDSCategory_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.TDSCategoryID == 0)
                {
                    context.PurchaseTDSCategory_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.TDSCategoryID > 0)
                {
                    return Obj.TDSCategoryID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PurchaseTDSCategoryListResponse> GetAllPurchaseTDSCategoryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseTDSCategoryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseTDSCategoryListResponse> objlst = new List<PurchaseTDSCategoryListResponse>();
                while (dr.Read())
                {
                    PurchaseTDSCategoryListResponse obj = new PurchaseTDSCategoryListResponse();
                    obj.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    obj.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
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

        public bool DeletePurchaseTDSCategory(long TDSCategoryID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePurchaseTDSCategory";
                cmdGet.Parameters.AddWithValue("@TDSCategoryID", TDSCategoryID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<PurchaseTDSCategoryName> GetAllPurchaseTDSCategoryName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseTDSCategoryName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchaseTDSCategoryName> lst = new List<PurchaseTDSCategoryName>();
                while (dr.Read())
                {
                    PurchaseTDSCategoryName obj = new PurchaseTDSCategoryName();
                    obj.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    obj.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }


        //21 June,2021 Sonal Gandhi
        public List<AreaPincodeModel> GetAreaPincodeList(long AreaID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAreaPincodeList";
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AreaPincodeModel> objlst = new List<AreaPincodeModel>();
                while (dr.Read())
                {
                    AreaPincodeModel objAreaList = new AreaPincodeModel();

                    objAreaList.AreaPincodeID = objBaseSqlManager.GetInt64(dr, "AreaPincodeID");
                    objAreaList.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objAreaList.Pincode = objBaseSqlManager.GetTextValue(dr, "PinCode");

                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 07 Feb,2022 Piyush Limbani
        public bool ResetUserPassword(long UserID, string NewPassword, long UpdatedBy)
        {
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdatePassword";
                    cmdGet.Parameters.AddWithValue("@UserID", UserID);
                    cmdGet.Parameters.AddWithValue("@NewPassword", NewPassword);
                    //cmdGet.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    //cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch (Exception)
            {
            }
            return true;
        }


        //public long CheckEmployeeCodeIsExists(int EmployeeCode)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "CheckEmployeeCodeIsExists";
        //    cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);        
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    long EmployeeCodeExist = 0;
        //    while (dr.Read())
        //    {
        //        EmployeeCodeExist = objBaseSqlManager.GetInt64(dr, "EmployeeCode");

        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return EmployeeCodeExist;
        //}

    }
}
