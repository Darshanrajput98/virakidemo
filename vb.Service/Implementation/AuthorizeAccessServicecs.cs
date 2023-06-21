
using vb.Data;
using vb.Data.Model;
using System.Data.Entity;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using vb.Data.ViewModel;
using System.Linq;
namespace vb.Service
{
  
    public class AuthorizeAccessServicecs : IAuthorizeAccess
    {
        public int AuthorizeMaster_CheckAuthorizeAccess(string RoleName, string controllerName, string actionName, string areaName)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "AuthorizeMaster_CheckAuthorizeAccess";
                cmdGet.Parameters.AddWithValue("@RoleName", RoleName);
                cmdGet.Parameters.AddWithValue("@controllerName", controllerName);
                cmdGet.Parameters.AddWithValue("@actionName", actionName);
                cmdGet.Parameters.AddWithValue("@Area", areaName);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                int RoleID = 0;
                while (dr.Read())
                {
                    RoleID = objBaseSqlManager.GetInt32(dr, "RoleId");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return RoleID;
            }
        }



        public List<RoleWiseActiveAuthorizedManuList> GetAllRoleWiseActiveManuList(long RoleID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRoleWiseActiveManuList";
                cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RoleWiseActiveAuthorizedManuList> objlst = new List<RoleWiseActiveAuthorizedManuList>();
                while (dr.Read())
                {
                    RoleWiseActiveAuthorizedManuList objRole = new RoleWiseActiveAuthorizedManuList();
                    objRole.MenuID = objBaseSqlManager.GetInt64(dr, "MenuID");
                    objRole.SystemName = objBaseSqlManager.GetTextValue(dr, "SystemName");
                    objRole.IsActive = objBaseSqlManager.GetBoolean(dr, "IsActive");
                    objlst.Add(objRole);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


    }
}
