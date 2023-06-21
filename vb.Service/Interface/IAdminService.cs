

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IAdminService
    {
        List<EventListResponse> GetAllEventName();

        //bool AddArea(Area_Mst ObjArea);

        //21 June,2021 Sonal Gandhi
        bool AddArea(AreaViewModel ObjArea);

        List<AreaListResponse> GetAllAreaList();

        bool DeleteArea(long AreaID, bool IsDelete);

        bool AddEvent(Event_Mst ObjArea);

        List<EventListResponse> GetAllEventList();

        bool DeleteEvent(long EventID, bool IsDelete);

        bool AddEventDate(EventDate_Mst ObjArea);

        List<EventDateListResponse> GetAllEventDateList();

        bool DeleteEventDate(long EventDateID, bool IsDelete);

        bool AddGodown(Godown_Mst ObjGodown);

        List<GodownListResponse> GetAllGodownList();

        bool DeleteGodown(long GodownID, bool IsDelete);

        bool AddTax(Tax_Mst ObjTax);

        List<TaxListResponse> GetAllTaxList();

        bool DeleteTax(long TaxID, bool IsDelete);

        bool AddUnit(Unit_Mst ObjUnit);

        List<UnitListResponse> GetAllUnitList();

        bool DeleteUnit(long UnitID, bool IsDelete);

        // bool AddRole(Role_Mst ObjRole);
        int AddRole(webpages_Roles ObjRole);


        List<RoleListResponse> GetAllRoleList();

        bool DeleteRole(long RoleID, bool IsDelete);

        List<RoleListResponse> GetAllRoleName();

        //  List<UserListResponse> GetAllUserList();

        List<RegistrationListResponse> GetAllUserList();

        List<RegistrationListResponse> GetAllUserListForExcel();

        Register Users_SelectByUserIDProfile(long UserID);

        bool UpdatePassword(long UserID, string NewPassword);

        string GetUserOldImage(long UserID);

        string GetUserOldFSSAIDoctorCertificate(long UserID);

        //long CheckEmployeeCodeIsExists(int EmployeeCode);

        bool UpdateUser(User obj);

        GetDocumentsResponse GetUploadedDocumentsFullPathListByEmployeeID(long EmployeeCode);

        GetDocumentsResponse GetDocumentsByEmployeeID(long EmployeeCode);

        bool AddDocuments(Document_Master Obj);

        bool DeleteUser(long UserID, bool IsDelete);

        bool AddDriver(Driver_Mst ObjDriver);

        List<DriverListResponse> GetAllDriverList();

        bool DeleteDriver(long DriverID, bool IsDelete);

        List<MainMenu> GetMenuList();

        bool AddAuthority(AuthorizeMaster ObjArea);

        AuthorizeMaster GetExistAuthorityDetail(int RoleID, long MenuID);

        AuthorizeMaster GetLastAuthority();

        bool UpdateAuthorityMaster(long AuthorizeID, bool IsActive);

        List<Register> Guser();

        LoginResponse GetUserDetails(string username);

        List<InvoiceTotal> GetInvoice();

        List<RetInvoiceTotal> GetRetInvoice();

        bool AddTransport(Transport_Mst ObjTransport);

        List<TransportListResponse> GetAllTransportList();

        bool DeleteTransport(long TransportID, bool IsDelete);

        bool AddVehicleDetail(VehicleDetail_Mst Obj);

        List<VehicleListResponse> GetAllVehicleList();

        bool DeleteLicence(long LicenceID, bool IsDelete);

        GetVehicleCertificate GetVehicleCertificateByVehicleDetailID(long VehicleDetailID);

        bool AddLicenceDetails(Licence_Mst Obj);

        List<LicenceListResponse> GetAllLicenceList();

        LicenceListResponse GetLicenceDocByLicenceID(long LicenceID);

        bool DeleteVehicle(long VehicleDetailID, bool IsDelete);

        //List<UpdateVehicleAssignedDate> GetRetVehicleAssignedDate();
        //void UpdateRetVehicleAssignedDate(string InvoiceNumber, DateTime CreatedOn);

        List<UpdateVehicleAssignedDate> GetRetOrderQtyID();

        UpdateVehicleAssignedDate GetOrderDareandProductQTYID(long OrderQtyID);

        // void UpdateRetOrderDareandProductQTYID(long OrderQtyID, long ProductQtyID, DateTime OrderDate);

        void UpdateRetOrderDareandProductQTYID(long OrderQtyID, DateTime OrderDate);

        //  Purchase Admin
        long AddPurchaseType(PurchaseType_Mst obj);

        List<PurchaseTypeListResponse> GetAllPurchaseTypeList();

        bool DeletePurchaseType(long PurchaseTypeID, bool IsDelete);

        long AddPurchaseDebitAccountType(PurchaseDebitAccountType_Mst obj);

        List<PurchaseDebitAccountTypeListResponse> GetAllPurchaseDebitAccountTypeList();

        bool DeletePurchaseDebitAccountType(long PurchaseDebitAccountTypeID, bool IsDelete);

        long AddBroker(Broker_Mst obj);

        List<BrokerListResponse> GetAllBrokerList();

        bool DeleteBroker(long BrokerID, bool IsDelete);

        List<PurchaseTypeListResponse> GetAllPurchaseTypeName(long PurchaseTypeID);

        List<PurchaseDebitAccountTypeListResponse> GetAllPurchaseDebitAccountTypeName();

        List<BrokerListResponse> GetAllBrokerName();

        long AddBank(Bank_Mst obj);

        List<BankListResponse> GetAllBankList();

        bool DeleteBank(long BankID, bool IsDelete);

        // expense module 17-12-2019
        long AddExpenseType(ExpenseType_Mst obj);

        List<ExpenseTypeListResponse> GetAllExpenseTypeList();

        bool DeleteExpenseType(long ExpenseTypeID, bool IsDelete);

        long AddExpenseDebitAccountType(ExpenseDebitAccountType_Mst obj);

        List<ExpenseDebitAccountTypeListResponse> GetAllExpenseDebitAccountTypeList();

        bool DeleteExpenseDebitAccountType(long ExpenseDebitAccountTypeID, bool IsDelete);

        List<ExpenseTypeListResponse> GetAllExpenseTypeName();

        List<ExpenseDebitAccountTypeListResponse> GetAllExpenseDebitAccountTypeName();

        // Utility_Master 02-03-2020
        bool AddUtility(Utility_Mst Obj);

        List<UtilityListResponse> GetAllUtilityList();

        bool DeleteUtility(long UtilityID, bool IsDelete);

        // List<UtilityListResponse> GetAllUtilityName();

        // 03-04-2020 - barcode history
        List<EmployeeName> GetAllEmployeeName();

        // 17 June 2020
        long AddTDSCategory(TDSCategory_Mst obj);

        List<TDSCategoryListResponse> GetAllTDSCategoryList();

        bool DeleteTDSCategory(long TDSCategoryID, bool IsDelete);

        List<TDSCategoryName> GetAllTDSCategoryName();

        // 08 Aug 2020 Piyush Limbani
        List<MenuList> GetAllMenuList();

        // 08 Aug 2020 Piyush Limbani
        long CheckMenuForRoleIsExist(int RoleID, long MenuID);

        // For Update Customer Mobile no  Wholeasle       // 19 Aug 2020 Piyush Limbani
        List<GetCustomerID> GetAllCustomerID();

        GetCustomerID GetCellNoDetail(long CustomerID);

        bool UpdateCellNoDetaiLByCystomerID(long CustomerID, string CellNo, string TelNo, string Email);

        // For Update Customer Mobile no  Retail       // 19 Aug 2020 Piyush Limbani
        List<GetCustomerID> GetAllRetCustomerID();

        GetCustomerID GetRetCellNoDetail(long CustomerID);

        bool UpdateRetCellNoDetaiLByCystomerID(long CustomerID, string CellNo, string TelNo, string Email);

        // 21 Sep 2020 Piyush Limbani
        long AddTCS(TCS_Mst obj);

        List<TCSListResponse> GetAllTCSList();

        bool DeleteTCS(long TCSID, bool IsDelete);

        // 07 Oct 2020 Piyush Limbani
        bool AddUtilityName(UtilityName_Mst Obj);

        List<UtilityNameListResponse> GetAllUtilityNameList();

        bool DeleteUtilityName(long UtilityNameID, bool IsDelete);

        // 08 Oct 2020 Piyush Limbani
        bool AddPouchName(PouchName_Mst Obj);

        List<PouchNameListResponse> GetAllPouchNameList();

        bool DeletePouchName(long PouchNameID, bool IsDelete);

        // 08 Feb 2021 Piyush Limbani
        bool Updatewebpages_UsersInRoles(int UserID, long RoleID);

        // 12 June,2021 Sonal Gandhi
        bool AddOnline(long AreaID, bool IsOnline);

        // 16 June 2021 Piyush Limbani
        long AddPurchaseTDSCategory(PurchaseTDSCategory_Mst obj);

        List<PurchaseTDSCategoryListResponse> GetAllPurchaseTDSCategoryList();

        bool DeletePurchaseTDSCategory(long TDSCategoryID, bool IsDelete);

        List<PurchaseTDSCategoryName> GetAllPurchaseTDSCategoryName();

        //21 June,2021 Sonal Gandhi
        List<AreaPincodeModel> GetAreaPincodeList(long AreaID);

        // 07 Feb,2022 Piyush Limbani
        bool ResetUserPassword(long UserID, string NewPassword, long UpdatedBy);
    }

}
