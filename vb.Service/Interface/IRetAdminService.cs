

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data.Model;
    using vb.Data;

    public interface IRetAdminService
    {
        bool AddArea(RetAreaMst ObjArea);

        List<RetAreaListResponse> GetAllAreaList();

        bool DeleteArea(long AreaID, bool IsDelete);

        bool AddGodown(RetGodownMst ObjGodown);

        List<RetGodownListResponse> GetAllGodownList();

        bool DeleteGodown(long GodownID, bool IsDelete);

        bool AddTax(RetTaxMst ObjTax);

        List<RetTaxListResponse> GetAllTaxList();

        bool DeleteTax(long TaxID, bool IsDelete);

        bool AddUnit(RetUnitMst ObjUnit);

        List<RetUnitListResponse> GetAllUnitList();

        bool DeleteUnit(long UnitID, bool IsDelete);

        bool AddRole(Role_Mst ObjRole);

        List<RoleListResponse> GetAllRoleList();

        bool DeleteRole(long RoleID);

        List<RoleListResponse> GetAllRoleName();

        bool AddTransport(RetTransportMst ObjTransport);

        List<RetTransportListResponse> GetAllTransportList();

        bool DeleteTransport(long TransportID, bool IsDelete);

    }
}
