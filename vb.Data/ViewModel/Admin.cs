

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AreaViewModel
    {
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public int DaysofWeek { get; set; }
        public bool IsOnline { get; set; }

        //21 June,2021 Sonal Gandhi
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public string DeleteItems { get; set; }
        public List<AreaPincodeModel> lstAreaPincode { get; set; }
    }

    public class AreaListResponse
    {
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public int DaysofWeek { get; set; }
        public string DaysofWeekstr { get; set; }
        public Boolean IsDelete { get; set; }
        // 12 June,2021 Sonal Gandhi
        public Boolean IsOnline { get; set; }

        //22 June, 2021 Sonal Gandhi
        public List<AreaPincodeModel> lstAreaPincode { get; set; }
    }

    public class EventViewModel
    {
        public long EventID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
    }

    public class EventListResponse
    {
        public long EventID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class EventDateViewModel
    {
        public long EventDateID { get; set; }
        public long EventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
    }

    public class EventDateListResponse
    {
        public long EventDateID { get; set; }
        public long EventID { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public enum Weekdays
    {
        [Description("Monday")]
        Monday = 1,
        [Description("Tuesday")]
        Tuesday = 2,
        [Description("Wednesday")]
        Wednesday = 3,
        [Description("Thursday")]
        Thursday = 4,
        [Description("Friday")]
        Friday = 5,
        [Description("Saturday")]
        Saturday = 6,
        [Description("Sunday")]
        Sunday = 7
    }

    public class GodownViewModel
    {
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string GodownPhone { get; set; }
        public string GodownAddress1 { get; set; }
        public string GodownAddress2 { get; set; }
        public string GodownFSSAINumber { get; set; }
        public string GodownCode { get; set; }
        public string GodownNote { get; set; }
        public decimal GSTNumber { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string CashOption { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal ChillarAmount { get; set; }
    }

    public class GodownListResponse
    {
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string GodownPhone { get; set; }
        public string GodownAddress1 { get; set; }
        public string GodownAddress2 { get; set; }
        public string GodownFSSAINumber { get; set; }
        public string GodownCode { get; set; }
        public string GodownNote { get; set; }
        public decimal GSTNumber { get; set; }
        public Boolean IsDelete { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string CashOption { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal ChillarAmount { get; set; }
    }

    public class TaxViewModel
    {
        public long TaxID { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxDescription { get; set; }
    }

    public class TaxListResponse
    {
        public long TaxID { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxDescription { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class UnitViewModel
    {
        public long UnitID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }
    }

    public class UnitListResponse
    {
        public long UnitID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RoleListResponse
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class UsersViewModel
    {
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long RoleID { get; set; }
        public string UserCode { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string UserPhone { get; set; }
        public string UserPhoneExtn { get; set; }
        public string UserDesignation { get; set; }
        public string UserDepartment { get; set; }
        public long GodownID { get; set; }
        public string UserLocation { get; set; }
        public string UserRemark { get; set; }

    }

    public class UserListResponse
    {
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserCode { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string UserPhone { get; set; }
        public string UserPhoneExtn { get; set; }
        public string UserDesignation { get; set; }
        public string UserDepartment { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string UserLocation { get; set; }
        public string UserRemark { get; set; }
        public Boolean IsDelete { get; set; }
        public List<GodownListResponse> lstGodown { get; set; }

    }

    public class RegistrationListResponse
    {
        public long UserID { get; set; }
        public string FullName { get; set; }
        public long EmployeeCode { get; set; }
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDatestr { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public bool IsDelete { get; set; }
        public long PrimaryArea { get; set; }
        public string PrimaryAreaName { get; set; }
        public string PrimaryAddress { get; set; }
        public long PrimaryPin { get; set; }
        public long SecondaryArea { get; set; }
        public string SecondaryAreaName { get; set; }
        public string SecondaryAddress { get; set; }
        public long SecondaryPin { get; set; }
        public string PanNo { get; set; }
        public string PassportNo { get; set; }
        public DateTime PassportValiddate1 { get; set; }
        public string PassportValiddate { get; set; }
        public long UIDAI { get; set; }
        public long UAN { get; set; }
        public string PF { get; set; }
        public string ESIC { get; set; }
        public string Drivinglicence { get; set; }
        public DateTime DrivingValidup1 { get; set; }
        public string DrivingValidup { get; set; }
        public string ReferenceName { get; set; }
        public string FName { get; set; }
        public string Fdob { get; set; }
        public long FUIDAI { get; set; }
        public string FRelation { get; set; }
        public string Flivingtogether { get; set; }
        public string MName { get; set; }
        public string Mdob { get; set; }
        public long MUIDAI { get; set; }
        public string MRelation { get; set; }
        public string Mlivingtogether { get; set; }
        public string WName { get; set; }
        public string Wdob { get; set; }
        public long WUIDAI { get; set; }
        public string WRelation { get; set; }
        public string Wlivingtogether { get; set; }
        public string C1Name { get; set; }
        public string C1dob { get; set; }
        public long C1UIDAI { get; set; }
        public string C1Relation { get; set; }
        public string C1livingtogether { get; set; }
        public string C2Name { get; set; }
        public string C2dob { get; set; }
        public long C2UIDAI { get; set; }
        public string C2Relation { get; set; }
        public string C2livingtogether { get; set; }
        public string C3Name { get; set; }
        public string C3dob { get; set; }
        public long C3UIDAI { get; set; }
        public string C3Relation { get; set; }
        public string C3livingtogether { get; set; }
        public long Godown { get; set; }
        public string GodownName { get; set; }
        public string ServiceTime { get; set; }
        public string Maritalstatus { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfilePicturePath { get; set; }

        public string FSSAIDoctorCertificate { get; set; }
        public string FSSAIDoctorCertificatepath { get; set; }
        public DateTime FSSAIDoctorCertificateValidity { get; set; }
        public string FSSAIDoctorCertificateValiditystr { get; set; }

        public DateTime DateofLeaving { get; set; }
        public string DateofLeavingstr { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class GetDocumentsResponse
    {
        public long DocumentID { get; set; }
        public long EmployeeCode { get; set; }
        public string AadharCard { get; set; }
        public string BankPassBook { get; set; }
        public string BioData { get; set; }
        public string DrivingLicence { get; set; }
        public string ElectionCard { get; set; }
        public string ElectricityBill { get; set; }
        public string IDCard { get; set; }
        public string LeavingCertificate { get; set; }
        public string PanCard { get; set; }
        public string RationCard { get; set; }
        public string Rentagreement { get; set; }
        public string Photo { get; set; }
        public string Signechar { get; set; }
        public string FamilyPhoto { get; set; }
        public string Passport { get; set; }
        public string Other1 { get; set; }
        public string Other2 { get; set; }
        public string Other3 { get; set; }
        public string ESICCard { get; set; }
        public string ESIPehchanCard { get; set; }
        public string MedicalFitnessCirtificate { get; set; }
    }

    public class UserListExport
    {
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Designation { get; set; }
        public string UIDAINo { get; set; }
        public string BirthDate { get; set; }
        public string DateOfJoining { get; set; }
        public string PassportNo { get; set; }
        public string PassportValidDate { get; set; }
        public string Drivinglicence { get; set; }
        public string DrivingValidup { get; set; }
        public string GodownName { get; set; }
        public string PanNo { get; set; }
        public string ESIC { get; set; }
        public string MobileNumber { get; set; }
        public string PF { get; set; }
        public string Email { get; set; }
        public string UAN { get; set; }
        public string PrimaryAddress { get; set; }
        public string SecondaryAddress { get; set; }
        public string MaritalStatus { get; set; }
        public string ReferenceName { get; set; }
        public string FSSAIValidity { get; set; }
        //public string Address { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
    }

    public class DriverViewModel
    {
        public long DriverID { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNumber { get; set; }
        public string Licence { get; set; }
        public string TempoNumber { get; set; }
    }

    public class DriverListResponse
    {
        public long DriverID { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNumber { get; set; }
        public string Licence { get; set; }
        public string TempoNumber { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class VehicleNoListResponse
    {
        public string VehicleNo { get; set; }
    }

    public class RetVehicleNoListResponse
    {
        public string VehicleNo { get; set; }
    }

    public class TransportViewModel
    {
        public long TransportID { get; set; }
        public string TransID { get; set; }
        public string TransportName { get; set; }
        public string TransportGSTNumber { get; set; }
        public string ContactNumber { get; set; }
    }

    public class TransportListResponse
    {
        public long TransportID { get; set; }
        public string TransID { get; set; }
        public string TransportName { get; set; }
        public string TransportGSTNumber { get; set; }
        public string ContactNumber { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RoleWiseActiveAuthorizedManuList
    {
        public long MenuID { get; set; }
        public string SystemName { get; set; }
        public Boolean IsActive { get; set; }
    }

    public class VehicleListResponse
    {
        public long VehicleDetailID { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string DateOfPurchasestr { get; set; }



        public DateTime RCNoValidity { get; set; }
        public string RCNoValiditystr { get; set; }
        public string RCCertificate { get; set; }
        public string RCCertificatepath { get; set; }
        public long RCNumberRemaining { get; set; }



        public DateTime FitnessValidity { get; set; }
        public string FitnessValiditystr { get; set; }
        public string FitnessCertificate { get; set; }
        public string FitnessCertificatepath { get; set; }
        public long FitnessRemaining { get; set; }


        public DateTime PermitValidity { get; set; }
        public string PermitValiditystr { get; set; }
        public string PermitCertificate { get; set; }
        public string PermitCertificatepath { get; set; }
        public long PermitRemaining { get; set; }



        public DateTime PUCValidity { get; set; }
        public string PUCValiditystr { get; set; }
        public string PUCCertificate { get; set; }
        public string PUCCertificatepath { get; set; }
        public long PUCRemaining { get; set; }


        public DateTime InsuranceValidity { get; set; }
        public string InsuranceValiditystr { get; set; }
        public string InsuranceCertificate { get; set; }
        public string InsuranceCertificatepath { get; set; }
        public long InsuranceRemaining { get; set; }


        public string AdvertisementCertificate { get; set; }
        public string AdvertisementCertificatepath { get; set; }
        public DateTime AdvertisementValidity { get; set; }
        public string AdvertisementValiditystr { get; set; }
        public long AdvertisementRemaining { get; set; }



        public DateTime SpeedGovernorCertificateValidity { get; set; }
        public string SpeedGovernorCertificateValiditystr { get; set; }
        public string SpeedGovernorCertificate { get; set; }
        public string SpeedGovernorCertificatepath { get; set; }
        public long SpeedGovernorCertiRemaining { get; set; }



        public DateTime FSSAICertificateValidity { get; set; }
        public string FSSAICertificateValiditystr { get; set; }
        public string FSSAICertificate { get; set; }
        public string FSSAICertificatepath { get; set; }
        public long FSSAICertiRemaining { get; set; }


        public DateTime GreenTaxCertificateValidity { get; set; }
        public string GreenTaxCertificateValiditystr { get; set; }
        public string GreenTaxCertificate { get; set; }
        public string GreenTaxCertificatepath { get; set; }
        public long GreenTaxCertiRemaining { get; set; }




        public decimal InstallmentAmount { get; set; }
        public DateTime InstallmentAmountDate { get; set; }
        public string InstallmentAmountDatestr { get; set; }

        public decimal OneTimeTax { get; set; }
        public DateTime OneTimeTaxDate { get; set; }
        public string OneTimeTaxDatestr { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GetVehicleCertificate
    {
        public long VehicleDetailID { get; set; }
        public string RCCertificate { get; set; }
        public string FitnessCertificate { get; set; }
        public string PermitCertificate { get; set; }
        public string PUCCertificate { get; set; }
        public string InsuranceCertificate { get; set; }
        public string AdvertisementCertificate { get; set; }
        public string SpeedGovernorCertificate { get; set; }
        public string FSSAICertificate { get; set; }
        public string GreenTaxCertificate { get; set; }
    }

    public class LicenceListResponse
    {
        public long LicenceID { get; set; }
        public string LicenceType { get; set; }
        public string WhereFrom { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDatestr { get; set; }
        public DateTime ToDate { get; set; }
        public string ToDatestr { get; set; }
        public string Remark { get; set; }
        public string Documents { get; set; }
        public string Documentspath { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public long DaysRemaining { get; set; }
    }

    public class GetAllTempNumber
    {
        public long VehicleDetailID { get; set; }
        public string TempoNumber { get; set; }
        public string VehicleNumber { get; set; }
    }

    public class BankNameListResponse
    {
        public long BankID { get; set; }
        public string BankName { get; set; }
    }

    public class UpdateVehicleAssignedDate
    {
        public string InvoiceNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public long OrderQtyID { get; set; }
        public DateTime OrderDate { get; set; }
        public long ProductQtyID { get; set; }
    }


    // purchase module
    public class AddPurchaseType
    {
        public long PurchaseTypeID { get; set; }
        public string PurchaseType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PurchaseTypeListResponse
    {
        public long PurchaseTypeID { get; set; }
        public string PurchaseType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddPurchaseDebitAccountType
    {
        public long PurchaseDebitAccountTypeID { get; set; }
        public string PurchaseDebitAccountType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PurchaseDebitAccountTypeListResponse
    {
        public long PurchaseDebitAccountTypeID { get; set; }
        public string PurchaseDebitAccountType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddBroker
    {
        public long BrokerID { get; set; }
        public string BrokerName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class BrokerListResponse
    {
        public long BrokerID { get; set; }
        public string BrokerName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddBank
    {
        public long BankID { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class BankListResponse
    {
        public long BankID { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    // Expense Module

    public class AddExpenseType
    {
        public long ExpenseTypeID { get; set; }
        public string ExpenseType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ExpenseTypeListResponse
    {
        public long ExpenseTypeID { get; set; }
        public string ExpenseType { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddExpenseDebitAccountType
    {
        public long ExpenseDebitAccountTypeID { get; set; }
        public string ExpenseDebitAccountType { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ExpenseDebitAccountTypeListResponse
    {
        public long ExpenseDebitAccountTypeID { get; set; }
        public string ExpenseDebitAccountType { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class UtilityModel
    {
        public long UtilityID { get; set; }
        //public string UtilityName { get; set; }
        public long UtilityNameID { get; set; }
        public string UtilityDescription { get; set; }
        public int UtilityQuantity { get; set; }
        public long GodownID { get; set; }
        public long MinUtilityQuantity { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class UtilityListResponse
    {
        public long UtilityID { get; set; }
        public long UtilityNameID { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public string UtilityDescription { get; set; }
        public int UtilityQuantity { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long MinUtilityQuantity { get; set; }
        public Boolean IsDelete { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class EmployeeName
    {
        public long EmployeeID { get; set; }
        public string FullName { get; set; }

    }

    public class BarcodeHistoryListResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long EmployeeID { get; set; }
        public long ProductQtyID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedDate { get; set; }
        public string ProductName { get; set; }
        public long NoOfBarcodes { get; set; }
        public long TotalNoOfBarcode { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
    }

    public class MinPouchQuantityListResponse
    {
        public long PouchID { get; set; }
        public string PouchName { get; set; }
        public string PouchDescription { get; set; }
        public long MinPouchQuantity { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long OpeningPouch { get; set; }
    }

    public class MinUtilityQuantityListResponse
    {
        public long UtilityID { get; set; }
        public string UtilityName { get; set; }
        public string UtilityDescription { get; set; }
        public long MinUtilityQuantity { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long OpeningUtility { get; set; }
    }

    // 17 June 2020
    public class AddTDSCategory
    {
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class TDSCategoryListResponse
    {
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class TDSCategoryName
    {
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
    }

    // For Update Customer Mobile no         // 19 Aug 2020 Piyush Limbani
    public class GetCustomerID
    {
        public long CustomerID { get; set; }
        public string CellNo { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
    }

    public class AddTCS
    {
        public long TCSID { get; set; }
        public decimal TaxWithOutGST { get; set; }
        public decimal TaxWithGST { get; set; }
        public string Note { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class TCSListResponse
    {
        public long TCSID { get; set; }
        public decimal TaxWithOutGST { get; set; }
        public decimal TaxWithGST { get; set; }
        public string Note { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class UtilityNameList
    {
        public long UtilityNameID { get; set; }
        public string UtilityName { get; set; }
    }

    public class UtilityNameModel
    {
        public long UtilityNameID { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class UtilityNameListResponse
    {
        public long UtilityNameID { get; set; }
        public string UtilityName { get; set; }
        public string HSNNumber { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class PouchNameList
    {
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
    }


    public class Authentication
    {
        public string AuthToken { get; set; }
        public string Sek { get; set; }
        public DateTime? ExpiredOn { get; set; }
    }


    public class AuthTokenDetail
    {
        public long AuthTokenID { get; set; }
        public string AuthToken { get; set; }
        public string Sek { get; set; }
        public DateTime ExpiredOn { get; set; }
    }


    // 17 June 2020
    public class AddPurchaseTDSCategory
    {
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PurchaseTDSCategoryListResponse
    {
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PurchaseTDSCategoryName
    {
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
    }

    //21 June,2021 Sonal Gandhi
    public class AreaPincodeModel
    {
        public long AreaPincodeID { get; set; }
        public long AreaID { get; set; }
        public string Pincode { get; set; }
    }

}
