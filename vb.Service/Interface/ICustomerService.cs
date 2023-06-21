

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

   public interface ICustomerService
    {
       List<AreaListResponse> GetAllAreaName();

       List<CustomerGroupListResponse> GetAllCustomerGroupName();

       List<UserListResponse> GetAllSalesPersonName();

       List<TaxListResponse> GetAllTaxName();

       bool AddCustomerGroup(CustomerGroup_Mst ObjCustomerGroup);

       List<CustomerGroupListResponse> GetAllCustomerGroupList();

       bool DeleteCustomerGroup(long CustomerGroupID, bool IsDelete);

       bool AddCustomer(CustomerViewModel ObjCustomer);

       List<CustomerListResponse> GetAllCustomerList();

       List<CustomerAddressViewModel> GetCustomerAddressListByCustomerID(long CustomerID);

       bool DeleteCustomer(long CustomerID,bool IsDelete);

       List<CustomerListResponse> GetAllCustomerCallList(CustomerListResponse model);

       CustomerListResponse GetExistCustomerDetials(string CustomerName, long AreaID);

       CustomerListResponse GetLastCustomerNumber();

       string GetCustomerNameByCustomerID(long CustomerID);

       List<CustomerListResponse> GetCustomerCallListForExcel(long AreaID, long UserID, long DaysofWeek,bool CallWeek1,bool CallWeek2,bool CallWeek3,bool CallWeek4);

       List<CustomerListResponse> GetAllCustomerListForExel();

       List<CustomerListResponse> GetAllCustomerFSSAIExpireList();

       bool UpdateFSSAIDate(long CustomerID, DateTime FSSAIValidUpTo);

       string GetOldFSSAICertificateByCustomerID(long CustomerID);

    }
}
