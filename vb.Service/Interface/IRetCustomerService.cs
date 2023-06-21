

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data;
    using vb.Data.Model;

    public interface IRetCustomerService
    {
        List<RetProductQtyListResponse> GetAllRetProductQtyList();

        List<RetAreaListResponse> GetAllAreaName();

        List<RetCustomerGroupListResponse> GetAllCustomerGroupName();

        List<RetUserListResponse> GetAllSalesPersonName();

        List<RetTaxListResponse> GetAllTaxName();

        List<CountryNameModel> GetAllCountryName();

        bool AddCustomerGroup(RetCustomerGroupMst ObjCustomerGroup);

        List<RetCustomerGroupListResponse> GetAllCustomerGroupList();

        bool DeleteCustomerGroup(long CustomerGroupID, bool IsDelete);

        bool AddCustomer(RetCustomerViewModel ObjCustomer);

        List<RetCustomerListResponse> GetAllCustomerList();

        List<RetCustomerAddressViewModel> GetCustomerAddressListByCustomerID(long CustomerID);

        bool DeleteCustomer(long CustomerID, bool IsDelete);

        List<RetCustomerListResponse> GetAllCustomerCallList(RetCustomerListResponse model);

        RetCustomerListResponse GetLastCustomerNumber();

        bool AddCustomerDiscount(RetCustomerDiscountListResponse ObjCustomer);

        RetCustomerDiscountListResponse GetRetailDiscountForCustomer(long CustomerID);

        string GetCustomerNameByCustomerID(long CustomerID);

        RetCustomerListResponse GetExistCustomerDetials(string CustomerName, long AreaID);

        List<RetCustomerListResponse> GetAllCustomerListForExel();

        RetCustomerArticleCodeListResponse GetRetailArticleCodeForCustomerGroup(long CustomerGroupID);

        bool AddProductArticleCode(RetCustomerArticleCodeListResponse ObjCustomer);

        List<RetCustomerListResponse> GetAllCustomerFSSAIExpireList();

        bool UpdateFSSAIDate(long CustomerID, DateTime? FSSAIValidUpTo, DateTime? FSSAIValidUpTo2);

        string GetOldFSSAICertificateByCustomerID(long CustomerID);

        string GetOldFSSAICertificate2ByCustomerID(long CustomerID);

    }
}
