
namespace vb.Service
{
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;
    public interface ICommonService
    {
        List<UsersViewModel> GetRoleWiseUserList();

        List<CustomerListResponse> GetAllCustomerName();

        List<AreaListResponse> GetAllAreaList();

        List<CustomerListResponse> GetAllCustomerList();

        List<DeliveryStatus> GetAllDeliveryPersonList();

        List<VehicleNoListResponse> GetAllVehicleNoList();

        List<VehicleNoListResponse> GetVehicleNoList();

        List<RetAreaListResponse> GetAllRetAreaList();

        List<RetVehicleNoListResponse> GetAllRetVehicleNoList();

        List<RetVehicleNoListResponse> GetRetVehicleNoList();

        List<RetCustomerListResponse> GetActiveRetCustomerName(long CustomerID);
  
        DynamicMenuModel DynamicMenuMaster_RoleWiseMenuList(int RoleId, string Area);

        DynamicMenuModel GetAllDynamicMenuList(int RoleID);

        void UpdateInvoiceTotal(decimal InvoiceTotal, string InvoiceNo, long OrderID);

        void UpdateRetInvoiceTotal(decimal InvoiceTotal, string InvoiceNo, long OrderID);

        List<RetTransportListResponse> GetAllRetTransportName();

        List<TransportListResponse> GetAllTransportName();

        List<TaxListResponse> GetAllTaxName();

        List<GodownListResponse> GetAllCashOption();

        List<RetGodownListResponse> GetAllRetCashOption();

        TransportListResponse GetTransportDetailByTransportID(long TransportID);

        RetTransportListResponse GetRetTransportDetailByTransportID(long TransportID);

        List<EmployeeNameResponse> GetAllEmployeeName();       

        List<LicenceListResponse> GetAllLicenceExpireList();

        List<VehicleListResponse> GetAllVehicleDocExpireList();

        List<GetAllTempNumber> GetAllTempoNumberList();

        List<GodownListResponse> GetGodownNameForExpense();

        List<BankNameListResponse> GetBankNameList();

        List<GetAllTempNumber> GetAllTempoNumberList2();


        List<CustomerListResponse> GetWholesaleFSSAIExpireListByUserID(long UserID);

        List<RetCustomerListResponse> GetRetailFSSAIExpireListByUserID(long UserID);

        // 03-04-2020 Display Pouch Qty Stock in Dashboard
        List<MinPouchQuantityListResponse> GetStockPouchListForDashboard(long GodownID);

      
        // 17 June 2022 Display Pouch Qty Stock in Dashboard
        List<MinPouchQuantityListResponse> GetStockPouchListForDashboardForAdmin(long GodownID);
        

        // 07 Oct 2020 Piyush Limbani
        List<UtilityNameList> GetAllUtilityName();

        // 08 Oct 2020 Piyush Limbani
        List<PouchNameList> GetAllPouchName();

        // 13 Oct 2020 Piyush Limbani
        List<MinUtilityQuantityListResponse> GetStockUtilityListForDashboard(long GodownID);

        
        // 17 June 2022 Display Utility Qty Stock in Dashboard
        List<MinUtilityQuantityListResponse> GetStockUtilityListForDashboardForAdmin(long GodownID);

        // 27 Aug 2020 Piyush Limbani
        //List<GetAllTempNumber> GetAllTempoNumberListCurrentDateWise();
        //List<GetAllTempNumber> GetAllTempoNumberListForInward();

    }
}
