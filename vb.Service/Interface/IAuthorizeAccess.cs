

namespace vb.Service
{
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;
    public interface IAuthorizeAccess
    {
        int AuthorizeMaster_CheckAuthorizeAccess(string RoleName, string controllerName, string actionName, string areaName);

        List<RoleWiseActiveAuthorizedManuList> GetAllRoleWiseActiveManuList(long RoleID);
    }
}
