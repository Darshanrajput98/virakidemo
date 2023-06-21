using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vb.Data
{
    public class SearchAttandance
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long GodownID { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class AttandanceListResponse
    {
        public long DAttendanceID { get; set; }
        public DateTime ADate { get; set; }
        public string ADatestr { get; set; }
        public string AEmployeeName { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal OT { get; set; }
        public string HalfdayStatus { get; set; }
        public string Status { get; set; }
        public string DayName { get; set; }

        public decimal sumTotalHoursWorked { get; set; }
    }

    public class SearchLeaveApplication
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long EmployeeCode { get; set; }
    }

    public class LeaveApplicationListResponse
    {
        public long LeaveApplicationID { get; set; }
        public long EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PrimaryAddress { get; set; }
        public string AreaName { get; set; }
        public long PrimaryPin { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public string ESIC { get; set; }
        public string GodownAddress1 { get; set; }
        public string GodownAddress2 { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicationDatestr { get; set; }
        public string Subject { get; set; }
        public string Sir { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDateLable { get; set; }
        public string FromDatestr { get; set; }
        public string lblFromDate { get; set; }
        public string ToLable { get; set; }
        public DateTime ToDate { get; set; }
        public string ToDatestr { get; set; }
        public string lblToDate { get; set; }
        public string ToDateLable { get; set; }
        public decimal NoofDays { get; set; }
        public string Reason { get; set; }
        public string ReasonLable { get; set; }
        public string GoingToLable { get; set; }
        public string GoingTo { get; set; }
        public string AdvanceLable { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string MalaLabel { get; set; }
        public string AdvanceAmountInWordsLabel { get; set; }
        public string AdvanceAmountInWords { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
        public string Line7 { get; set; }
        public string AdvanceReason { get; set; }
        public string NoteLabel { get; set; }
        public string NoteLine1 { get; set; }
        public string NoteLine2 { get; set; }
        public string NoteLine3 { get; set; }
        public string NoteLine4 { get; set; }
        public string NoteLine5 { get; set; }
        public string VirakiMarathi { get; set; }
        public string YoursFaithfully { get; set; }
        public decimal DeductionPerMonthAmount { get; set; }
        public long LeaveStatusID { get; set; }
        public string LeaveStatus { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal TotalClosingAdvance { get; set; }
        public DateTime ApprovalFromDate { get; set; }
        public string ApprovalFromDatestr { get; set; }
        public string lblApprovalFromDate { get; set; }
        public DateTime ApprovalToDate { get; set; }
        public string ApprovalToDatestr { get; set; }
        public string lblApprovalToDate { get; set; }
        public decimal ApprovalAdvanceAmount { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDatestr { get; set; }
    }

    public class LeaveStatusName
    {
        public long LeaveStatusID { get; set; }
        public string LeaveStatus { get; set; }
    }

    public class EmployeeDetail
    {
        public long UserID { get; set; }
        public DateTime BirthDate { get; set; }
        public string Age { get; set; }
        public long AllowanceDetailID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public DateTime OpeningDate { get; set; }
        public string OpeningDatestr { get; set; }
        public DateTime IncrementDate { get; set; }
        public string IncrementDatestr { get; set; }
        public DateTime DA1Date { get; set; }
        public string DA1Datestr { get; set; }
        public DateTime DA2Date { get; set; }
        public string DA2Datestr { get; set; }
        public DateTime OthersDate { get; set; }
        public string OthersDatestr { get; set; }
        public decimal BasicAllowance1 { get; set; }
        public decimal BasicAllowance2 { get; set; }
        public decimal BasicAllowance3 { get; set; }
        public decimal BasicAllowance4 { get; set; }
        public decimal BasicAllowance5 { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal HRAPercentage1 { get; set; }
        public decimal HRAPercentage2 { get; set; }
        public decimal HRAPercentage3 { get; set; }
        public decimal HRAPercentage4 { get; set; }
        public decimal HRAPercentage5 { get; set; }
        public decimal HouseRentAllowance1 { get; set; }
        public decimal HouseRentAllowance2 { get; set; }
        public decimal HouseRentAllowance3 { get; set; }
        public decimal HouseRentAllowance4 { get; set; }
        public decimal HouseRentAllowance5 { get; set; }
        public decimal TotalHouseRentAllowance { get; set; }
        public decimal TotalWages1 { get; set; }
        public decimal TotalWages2 { get; set; }
        public decimal TotalWages3 { get; set; }
        public decimal TotalWages4 { get; set; }
        public decimal TotalWages5 { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal Conveyance { get; set; }
        public decimal ConveyancePerDay { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal BonusAmount { get; set; }
        public long BonusStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public long LeaveEnhancementStatusID { get; set; }
        public decimal GratuityPercentage { get; set; }
        public decimal GratuityAmount { get; set; }
        public long GratuityStatusID { get; set; }
        public long CustomerID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnstr { get; set; }
    }



    // Attandance System
    public class EmployeeNameResponse
    {
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
    }

    public class AddAllowance
    {
        public long AllowanceDetailID { get; set; }
        public long EmployeeCode { get; set; }
        public DateTime? OpeningDate { get; set; }
        public DateTime? IncrementDate { get; set; }
        public DateTime? DA1Date { get; set; }
        public DateTime? DA2Date { get; set; }
        public DateTime? OthersDate { get; set; }
        public decimal BasicAllowance1 { get; set; }
        public decimal BasicAllowance2 { get; set; }
        public decimal BasicAllowance3 { get; set; }
        public decimal BasicAllowance4 { get; set; }
        public decimal BasicAllowance5 { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal HouseRentAllowancePercentage1 { get; set; }
        public decimal HouseRentAllowancePercentage2 { get; set; }
        public decimal HouseRentAllowancePercentage3 { get; set; }
        public decimal HouseRentAllowancePercentage4 { get; set; }
        public decimal HouseRentAllowancePercentage5 { get; set; }
        public decimal HouseRentAllowance1 { get; set; }
        public decimal HouseRentAllowance2 { get; set; }
        public decimal HouseRentAllowance3 { get; set; }
        public decimal HouseRentAllowance4 { get; set; }
        public decimal HouseRentAllowance5 { get; set; }
        public decimal TotalHouseRentAllowance { get; set; }
        public decimal TotalWages1 { get; set; }
        public decimal TotalWages2 { get; set; }
        public decimal TotalWages3 { get; set; }
        public decimal TotalWages4 { get; set; }
        public decimal TotalWages5 { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal Conveyance { get; set; }
        public decimal ConveyancePerDay { get; set; }
        public bool IsOldPF { get; set; }
        public bool IsOldESIC { get; set; }
        public bool IsOldCityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal PerformanceAllowance { get; set; }



        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal BonusAmount { get; set; }
        public long BonusStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public long LeaveEnhancementStatusID { get; set; }
        public decimal GratuityPercentage { get; set; }
        public decimal GratuityAmount { get; set; }
        public long GratuityStatusID { get; set; }
        public long CustomerID { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AllowanceListResponse
    {
        public long AllowanceDetailID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDatestr { get; set; }
        public string Age { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public DateTime OpeningDate { get; set; }
        public string OpeningDatestr { get; set; }
        public DateTime IncrementDate { get; set; }
        public string IncrementDatestr { get; set; }
        public DateTime DA1Date { get; set; }
        public string DA1Datestr { get; set; }
        public DateTime DA2Date { get; set; }
        public string DA2Datestr { get; set; }
        public DateTime OthersDate { get; set; }
        public string OthersDatestr { get; set; }
        public decimal BasicAllowance1 { get; set; }
        public decimal BasicAllowance2 { get; set; }
        public decimal BasicAllowance3 { get; set; }
        public decimal BasicAllowance4 { get; set; }
        public decimal BasicAllowance5 { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal HRAPercentage1 { get; set; }
        public decimal HRAPercentage2 { get; set; }
        public decimal HRAPercentage3 { get; set; }
        public decimal HRAPercentage4 { get; set; }
        public decimal HRAPercentage5 { get; set; }
        public decimal HouseRentAllowance1 { get; set; }
        public decimal HouseRentAllowance2 { get; set; }
        public decimal HouseRentAllowance3 { get; set; }
        public decimal HouseRentAllowance4 { get; set; }
        public decimal HouseRentAllowance5 { get; set; }
        public decimal TotalHouseRentAllowance { get; set; }

        public decimal TotalWages1 { get; set; }
        public decimal TotalWages2 { get; set; }
        public decimal TotalWages3 { get; set; }
        public decimal TotalWages4 { get; set; }
        public decimal TotalWages5 { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal Conveyance { get; set; }
        public decimal ConveyancePerDay { get; set; }
        //public bool IsOldPF { get; set; }
        //public bool IsOldESIC { get; set; }
        //public bool IsOldCityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal PerformanceAllowance { get; set; }

        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal BonusAmount { get; set; }
        public long BonusStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public long LeaveEnhancementStatusID { get; set; }
        public decimal GratuityPercentage { get; set; }
        public decimal GratuityAmount { get; set; }
        public long GratuityStatusID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long CustomerNumber { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class GetEmployeeAttandanceDetail
    {
        public int PresentAct { get; set; }
        public int Present { get; set; }
        public int AdditionalPresent { get; set; }
        public int TotalPresent { get; set; }
        public int TotalMonthSundayAct { get; set; }
        public int TotalMonthSunday { get; set; }
        public int AdditionalSunday { get; set; }
        public int TotalSunday { get; set; }
        public int Holiday { get; set; }
        public int AdditionalHoliday { get; set; }
        public int TotalHoliday { get; set; }
        public int Absent { get; set; }
        public int AdditionalAbsent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalDays { get; set; }
        public int TotalMonthDay { get; set; }
        public decimal BasicAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal ConveyancePerDay { get; set; }
        //public bool IsOldPF { get; set; }
        //public bool IsOldESIC { get; set; }
        //public bool IsOldCityAllowance { get; set; }

        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }

        public decimal CityAllowance { get; set; }
        public decimal AdditionalCityAllowance { get; set; }
        public decimal TotalCityAllowance { get; set; }

        public decimal VehicleAllowance { get; set; }
        public decimal AdditionalVehicleAllowance { get; set; }
        public decimal TotalVehicleAllowance { get; set; }

        public decimal GrossWagesPayable { get; set; }
        public decimal PF { get; set; }
        public decimal ESIC { get; set; }
        public decimal PT { get; set; }
        public decimal MLWF { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetWagesPaid { get; set; }
        public decimal ConveyancePerMonth { get; set; }

        public decimal Conveyance { get; set; }
        public decimal AdditionalConveyance { get; set; }
        public decimal TotalConveyance { get; set; }

        public decimal PerformanceAllowanceAct { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal AdditionalPerformanceAllowance { get; set; }
        public decimal TotalPerformanceAllowance { get; set; }


        //   public decimal Overtime { get; set; }
        public decimal CityAllowanceMinutes { get; set; }
        public decimal CityAllowanceHours { get; set; }
        public decimal OpeningLeaves { get; set; }
        public decimal EarnedLeaves { get; set; }
        public decimal TotalLeaves { get; set; }
        public decimal AvailedLeaves { get; set; }
        public decimal AdditionalAvailedLeaves { get; set; }
        public decimal TotalAvailedLeaves { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal AdditionalClosingLeaves { get; set; }
        public decimal TotalClosingLeaves { get; set; }

        public decimal HighestSlab { get; set; }
        public decimal HighestPF { get; set; }
        public decimal PFPercentage { get; set; }
        public decimal EmployeeSlab { get; set; }
        public decimal PTHighestSlab { get; set; }
        public decimal PTHighestAmount { get; set; }
        public decimal PTLowestSlab { get; set; }
        public decimal PTLowestAmount { get; set; }
        public decimal MLWFHighestSlab { get; set; }
        public decimal MLWFHighestAmount { get; set; }
        public int MLWFMonthID { get; set; }

        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal AnyOtherDeductions1 { get; set; }
        public decimal AnyOtherDeductions2 { get; set; }

        public long CustomerID { get; set; }
        public int AMonth { get; set; }
        public int AYear { get; set; }
        public long SalarySheetID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnstr { get; set; }

        // 31 May 2023 Dhruvik
        public decimal AdditionalCityAllowanceMinutes { get; set; }
        // 31 May 2023 Dhruvik
    }

    public class GetAllowanceDetail
    {
        public decimal BasicAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal ConveyancePerDay { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal PerformanceAllowance { get; set; }

        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal OpeningLeaves { get; set; }
        public long CustomerID { get; set; }

        //public bool IsOldPF { get; set; }
        //public bool IsOldESIC { get; set; }
        //public bool IsOldCityAllowance { get; set; }
    }

    public class GetOTDetails
    {
        public decimal PluseTotalMinutes { get; set; }
        public decimal MinuseTotalMinutes { get; set; }
        public decimal TotalMinutes { get; set; }
        public decimal TotalHrs { get; set; }
    }


    // 16 July 2020 Piyush Limbani
    public class AdditionalAmountDetails
    {
        public int AdditionalPresent { get; set; }
        public int TotalPresent { get; set; }
        public int AdditionalSunday { get; set; }
        public int TotalSunday { get; set; }
        public int AdditionalHoliday { get; set; }
        public int TotalHoliday { get; set; }
        public int AdditionalAbsent { get; set; }
        public int TotalAbsent { get; set; }

        public decimal AdditionalCityAllowance { get; set; }
        public decimal TotalCityAllowance { get; set; }
        public decimal AdditionalVehicleAllowance { get; set; }
        public decimal TotalVehicleAllowance { get; set; }
        public decimal AdditionalConveyance { get; set; }
        public decimal TotalConveyance { get; set; }
        public decimal AdditionalPerformanceAllowance { get; set; }
        public decimal TotalPerformanceAllowance { get; set; }

        public decimal AdditionalAvailedLeaves { get; set; }
        public decimal TotalAvailedLeaves { get; set; }
        public decimal AdditionalClosingLeaves { get; set; }
        public decimal TotalClosingLeaves { get; set; }

    }



    public class SalaryExistModel
    {
        public long SalarySheetID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class AddSalarySheet
    {
        public long SalarySheetID { get; set; }
        public long EmployeeCode { get; set; }
        public int MonthID { get; set; }
        public int YearID { get; set; }
        public int Present { get; set; }
        public int AdditionalPresent { get; set; }
        public int TotalPresent { get; set; }
        public int Absent { get; set; }
        public int AdditionalAbsent { get; set; }
        public int TotalAbsent { get; set; }
        public int Holiday { get; set; }
        public int AdditionalHoliday { get; set; }
        public int TotalHoliday { get; set; }
        public int Sunday { get; set; }
        public int AdditionalSunday { get; set; }
        public int TotalSunday { get; set; }
        public int TotalDays { get; set; }
        public int TotalDaysIntheMonth { get; set; }
        public decimal BasicAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal CityAllowanceMinutes { get; set; }
        public decimal CityAllowanceHours { get; set; }
        public decimal CityAllowance { get; set; }
        public decimal AdditionalCityAllowance { get; set; }
        public decimal TotalCityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal AdditionalVehicleAllowance { get; set; }
        public decimal TotalVehicleAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal AdditionalConveyance { get; set; }
        public decimal TotalConveyance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal AdditionalPerformanceAllowance { get; set; }
        public decimal TotalPerformanceAllowance { get; set; }
        public decimal GrossWagesPayable { get; set; }
        public decimal PF { get; set; }
        public decimal PT { get; set; }
        public decimal ESIC { get; set; }
        public decimal MLWF { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetWagesPaid { get; set; }
        public decimal OpeningLeaves { get; set; }
        public decimal EarnedLeaves { get; set; }
        public decimal AvailedLeaves { get; set; }
        public decimal AdditionalAvailedLeaves { get; set; }
        public decimal TotalAvailedLeaves { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal AdditionalClosingLeaves { get; set; }
        public decimal TotalClosingLeaves { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal AnyOtherDeductions1 { get; set; }
        public decimal AnyOtherDeductions2 { get; set; }

        public int GodownID { get; set; }

        // 31 May 2023 Dhruvik
        public decimal AdditionalCityAllowanceMinutes { get; set; }
        // 31 May 2023 Dhruvik
    }

    public class SearchSalarySheet
    {
        public long MonthID { get; set; }
        public long YearID { get; set; }
        public long GodownID { get; set; }
        public long EmployeeCode { get; set; }
    }

    public class SalarySheetListResponse
    {
        public int MonthID { get; set; }
        public int YearID { get; set; }
        public long SalarySheetID { get; set; }
        public int RowNumber { get; set; }
        // public string RowNumberstr { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int Age { get; set; }
        public string Agestr { get; set; }
        public string Sex { get; set; }
        public string Designation { get; set; }
        public DateTime DateofJoining { get; set; }
        public string DateofJoiningstr { get; set; }
        public DateTime BirthDate { get; set; }
        public string WorkingHours { get; set; }
        public string IntervalForRest { get; set; }
        public int Present { get; set; }
        public int AdditionalPresent { get; set; }
        public int Absent { get; set; }
        public int AdditionalAbsent { get; set; }
        public int Holiday { get; set; }
        public int AdditionalHoliday { get; set; }
        public int Sunday { get; set; }
        public int AdditionalSunday { get; set; }
        public int TotalDays { get; set; }
        public decimal BasicAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal TotalEarnedWages { get; set; }
        // public decimal CityAllowanceHours { get; set; }
        public decimal CityAllowanceMinutes { get; set; }

        public decimal CityAllowance { get; set; }
        public decimal AdditionalCityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal AdditionalVehicleAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal AdditionalConveyance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal AdditionalPerformanceAllowance { get; set; }

        public decimal GrossWagesPayable { get; set; }
        public decimal PF { get; set; }
        public decimal PT { get; set; }
        public decimal ESIC { get; set; }
        public decimal MLWF { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetWagesPaid { get; set; }

        public decimal OpeningLeaves { get; set; }
        public decimal EarnedLeaves { get; set; }
        public decimal AvailedLeaves { get; set; }
        public decimal AdditionalAvailedLeaves { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal AdditionalClosingLeaves { get; set; }
        public string Sign { get; set; }
        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal FinalTotalDeductions { get; set; }
        public decimal NetWagesToPay { get; set; }
        public decimal AnyOtherDeductions1 { get; set; }
        public decimal AnyOtherDeductions2 { get; set; }
        public long CustomerID { get; set; }
        public long EmployeeCode { get; set; }
        public string CustomerName { get; set; }

        public decimal sumBasicAllowance { get; set; }
        public decimal sumHouseRentAllowance { get; set; }
        public decimal sumEarnedBasicWages { get; set; }
        public decimal sumEarnedHouseRentAllowance { get; set; }
        public decimal sumTotalEarnedWages { get; set; }
        public decimal sumCityAllowance { get; set; }
        public decimal sumVehicleAllowance { get; set; }
        public decimal sumConveyance { get; set; }
        public decimal sumPerformanceAllowance { get; set; }
        public decimal sumGrossWagesPayable { get; set; }
        public decimal sumPF { get; set; }
        public decimal sumPT { get; set; }
        public decimal sumESIC { get; set; }
        public decimal sumMLWF { get; set; }
        public decimal sumNetWagesPaid { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class SalarySheetListExport
    {
        public int SrNo { get; set; }
        public string Name { get; set; }
        //public int Age { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Designation { get; set; }
        public string DateofJoining { get; set; }
        public string WorkingHours { get; set; }
        public string IntervalForRest { get; set; }
        public int Present { get; set; }
        public int Sunday { get; set; }
        public int Holiday { get; set; }
        public int Absent { get; set; }
        public int TotalDays { get; set; }
        public decimal BasicAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal TotalEarnedWages { get; set; }
        //public decimal CityAllowanceHours { get; set; }
        public decimal CityAllowanceMinutes { get; set; }
        public decimal CityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal GrossWagesPayable { get; set; }
        public decimal PF { get; set; }
        public decimal ESIC { get; set; }
        public decimal PT { get; set; }
        public decimal MLWF { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetWagesPaid { get; set; }
        public decimal OpeningLeaves { get; set; }
        public decimal EarnedLeaves { get; set; }
        public decimal AvailedLeaves { get; set; }
        public decimal ClosingLeaves { get; set; }
        public string Sign { get; set; }
        public decimal NetWagesPaid2 { get; set; }
        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal NetWagesToPay { get; set; }
        public string CustomerName { get; set; }
    }

    public class LeaveTypeResponse
    {
        public long LeaveMasterID { get; set; }
        public string LeaveCode { get; set; }
    }

    public class GetTotalDatsIfTheMonth
    {
        public long TotalMonthDay { get; set; }
        public long TotalSunday { get; set; }
    }

    //16-04-2020 Attendance Changes
    public partial class NewDailyAttendanceModel
    {
        public long DAttendanceID { get; set; }
        public long EmployeeCode { get; set; }
        public string AEmployeeName { get; set; }
        public DateTime ADate { get; set; }
        public int AYear { get; set; }
        public int AMonth { get; set; }
        public string ShiftStartTime { get; set; }
        public DateTime ShiftStartDateTime { get; set; }
        public DateTime? InDateTime { get; set; }
        public string TimeIn { get; set; }
        public DateTime? OutDateTime { get; set; }
        public string TimeOut { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal OT { get; set; }
        public string ShiftEndTime { get; set; }
        public string Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }

        public DateTime OutDate { get; set; }
    }

    public class GetSundayDate
    {
        public DateTime Date { get; set; }
    }

    public class GetHolidayDate
    {
        public DateTime HolidayDate { get; set; }
        public int Holiday { get; set; }
    }

    public class GetAbsentHolidayDate
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

    //17-04-2020
    public class AddFestival
    {
        public long FestivalID { get; set; }
        public long EventID { get; set; }
        public DateTime FestivalDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class FestivalListResponse
    {
        public long FestivalID { get; set; }
        public long EventID { get; set; }
        public string EventName { get; set; }
        public DateTime FestivalDate { get; set; }
        public string FestivalDatestr { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    //20-04-2020
    public class AllowanceStatusNameResponse
    {
        public long AllowanceStatusID { get; set; }
        public string Status { get; set; }
    }

    public class AddEarnedLeaves
    {
        public long EarnedLeavesID { get; set; }
        public long MonthID { get; set; }
        public decimal NoOfEarnedLeaves { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class EarnedLeavesListResponse
    {
        public long EarnedLeavesID { get; set; }
        public long MonthID { get; set; }
        public decimal NoOfEarnedLeaves { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class BonusListResponse
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromMonthID { get; set; }
        public int FromYearID { get; set; }
        public int ToMonthID { get; set; }
        public int ToYearID { get; set; }
        public int GodownID { get; set; }
        public string FullName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public long EmployeeCode { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal BonusPercentage { get; set; }
        public int BonusStatusID { get; set; }
    }

    public class BonusListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<BonusListResponse> ListMainBonus { get; set; }
    }

    public class LeaveEncashmentListResponse
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromMonthID { get; set; }
        public int FromYearID { get; set; }
        public int ToMonthID { get; set; }
        public int ToYearID { get; set; }
        public int GodownID { get; set; }
        public long SalarySheetID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal TotalLeaveEnhancement { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal LeaveEncashment { get; set; }
        public int LeaveEnhancementStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public int RowNumber { get; set; }
    }

    public class GratuityListResponse
    {
        public DateTime DateOfLeaving { get; set; }
        public string DateOfLeavingstr { get; set; }
        public int GodownID { get; set; }
        public long EmployeeCode { get; set; }
        public int RowNumber { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public decimal TotalMonth { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal GrandTotalWages { get; set; }
        public int GratuityStatusID { get; set; }
        public decimal GratuityAmount { get; set; }
        public decimal GratuityPercentage { get; set; }
        public decimal TotalGratuity { get; set; }
        public decimal Gratuity { get; set; }
    }

    //29-04-2020
    public class AddPF
    {
        public long PFID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestPF { get; set; }
        public decimal PFPercentage { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PFListResponse
    {
        public long PFID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestPF { get; set; }
        public decimal PFPercentage { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddESIC
    {
        public long ESICID { get; set; }
        public decimal EmployeeSlab { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ESICListResponse
    {
        public long ESICID { get; set; }
        public decimal EmployeeSlab { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddPT
    {
        public long PTID { get; set; }
        public long MonthID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestAmount { get; set; }
        public decimal LowestSlab { get; set; }
        public decimal LowestAmount { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PTListResponse
    {
        public long PTID { get; set; }
        public long MonthID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestAmount { get; set; }
        public decimal LowestSlab { get; set; }
        public decimal LowestAmount { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddMLWF
    {
        public long MLWFID { get; set; }
        public long MonthID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestAmount { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class MLWFListResponse
    {
        public long MLWFID { get; set; }
        public long MonthID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestAmount { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddLeaveEncashment
    {
        public long LeaveEncashmentID { get; set; }
        public decimal NoOfDaysLeaveEncashment { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class LeaveEncashmentMstListResponse
    {
        public long LeaveEncashmentID { get; set; }
        public decimal NoOfDaysLeaveEncashment { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddGratuity
    {
        public long GratuityID { get; set; }
        public decimal NoOfDaysInMonth { get; set; }
        public decimal GratuityNoOfDaysInYear { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

    public class GratuityMstListResponse
    {
        public long GratuityID { get; set; }
        public decimal NoOfDaysInMonth { get; set; }
        public decimal GratuityNoOfDaysInYear { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }


    //30-04-2020
    public class GetLastPFInformation
    {
        public long PFID { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestPF { get; set; }
        public decimal PFPercentage { get; set; }
    }

    public class GetLastESICInformation
    {
        public long ESICID { get; set; }
        public decimal EmployeeSlab { get; set; }
    }

    public class GetLastPTInformation
    {
        public long PTID { get; set; }
        public int PTMonthID { get; set; }
        public decimal PTHighestSlab { get; set; }
        public decimal PTHighestAmount { get; set; }
        public decimal PTLowestSlab { get; set; }
        public decimal PTLowestAmount { get; set; }
    }

    public class GetLastMLWFInformation
    {
        public long MLWFID { get; set; }
        public int MLWFMonthID { get; set; }
        public decimal MLWFHighestSlab { get; set; }
        public decimal MLWFHighestAmount { get; set; }
    }

    public class GetLastGratuityInformation
    {
        public long GratuityID { get; set; }
        public decimal NoOfDaysInMonth { get; set; }
        public decimal GratuityNoOfDaysInYear { get; set; }
    }

    // 04/05/2020
    public class AdvanceAndOtherDeductions
    {
        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal AnyOtherDeductions1 { get; set; }
        public decimal AnyOtherDeductions2 { get; set; }
    }

    public class ClosingLeavesandClosingAdvance
    {
        public string GodownName { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal ClosingAdvance { get; set; }
    }

    public class AddLeaveAndAdvanceApplication
    {
        public long LeaveApplicationID { get; set; }
        public long EmployeeCode { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal ClosingAdvance { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Reason { get; set; }
        public string GoingTo { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string AdvanceReason { get; set; }
        public decimal DeductionPerMonthAmount { get; set; }
        public int LeaveStatus { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }

        public Boolean IsAdvanceAmount { get; set; }
    }

    public class LastTwelveMonthDataForLeavePrint
    {
        public long MonthID { get; set; }
        public long YearID { get; set; }
        public decimal Leaves { get; set; }
    }

    public class MonthList
    {
        public decimal JAN { get; set; }
        public decimal FEB { get; set; }
        public decimal MAR { get; set; }
        public decimal APR { get; set; }
        public decimal MAY { get; set; }
        public decimal JUN { get; set; }
        public decimal JULY { get; set; }
        public decimal AUG { get; set; }
        public decimal SEP { get; set; }
        public decimal OCT { get; set; }
        public decimal NOV { get; set; }
        public decimal DEC { get; set; }
        public decimal Total { get; set; }
    }

    public class LastTwelveMonthClosingAdvanceDataForLeavePrint
    {
        public long MonthID { get; set; }
        public long YearID { get; set; }
        //public decimal ClosingAdvance { get; set; }
        public decimal ApprovalAdvanceAmount { get; set; }
    }

    public class ClosingAdvanceMonthList
    {
        public decimal JAN { get; set; }
        public decimal FEB { get; set; }
        public decimal MAR { get; set; }
        public decimal APR { get; set; }
        public decimal MAY { get; set; }
        public decimal JUN { get; set; }
        public decimal JULY { get; set; }
        public decimal AUG { get; set; }
        public decimal SEP { get; set; }
        public decimal OCT { get; set; }
        public decimal NOV { get; set; }
        public decimal DEC { get; set; }
        public decimal TotalClosingAdvance { get; set; }
    }

    public class CheckDailyAttendanceResponse
    {
        public long DAttendanceID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ApprovalLeave
    {
        public long LeaveApplicationID { get; set; }
        public DateTime? ApprovalFromDate { get; set; }
        public DateTime? ApprovalToDate { get; set; }
        public decimal ApprovalAdvanceAmount { get; set; }
        public long LeaveStatusID { get; set; }
        public long ApprovalBy { get; set; }
        public DateTime ApprovalDate { get; set; }
    }

    public class ActiveEmployeeCode
    {
        public long EmployeeCode { get; set; }
    }



    public class AllowanceListForExp
    {
        public long EmployeeCode { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Age { get; set; }
        public string DateOfJoining { get; set; }
        public string OpeningDate { get; set; }
        public string IncrementDate { get; set; }
        public string DA1Date { get; set; }
        public string DA2Date { get; set; }
        public string OthersDate { get; set; }
        public decimal BasicAllowance1 { get; set; }
        public decimal BasicAllowance2 { get; set; }
        public decimal BasicAllowance3 { get; set; }
        public decimal BasicAllowance4 { get; set; }
        public decimal BasicAllowance5 { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal HRAPercentage1 { get; set; }
        public decimal HRAPercentage2 { get; set; }
        public decimal HRAPercentage3 { get; set; }
        public decimal HRAPercentage4 { get; set; }
        public decimal HRAPercentage5 { get; set; }
        public decimal HouseRentAllowance1 { get; set; }
        public decimal HouseRentAllowance2 { get; set; }
        public decimal HouseRentAllowance3 { get; set; }
        public decimal HouseRentAllowance4 { get; set; }
        public decimal HouseRentAllowance5 { get; set; }
        public decimal TotalHouseRentAllowance { get; set; }
        public decimal TotalWages1 { get; set; }
        public decimal TotalWages2 { get; set; }
        public decimal TotalWages3 { get; set; }
        public decimal TotalWages4 { get; set; }
        public decimal TotalWages5 { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal Conveyance { get; set; }
        public decimal ConveyancePerDay { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal BonusAmount { get; set; }
        public long BonusStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public long LeaveEnhancementStatusID { get; set; }
        public decimal GratuityPercentage { get; set; }
        public decimal GratuityAmount { get; set; }
        public long GratuityStatusID { get; set; }
    }


    public class AttandanceListForExp
    {
        public string Date { get; set; }
        public string EmployeeName { get; set; }
        public string Day { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal OT { get; set; }
        public string Status { get; set; }
    }

    public class LeaveEncashmentListForExp
    {
        public int SrNo { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal LeaveEncashment { get; set; }
    }

    public class GratuityListForExp
    {
        public int SrNo { get; set; }
        public string Name { get; set; }
        public string DOJ { get; set; }
        public string DOL { get; set; }
        public decimal Total { get; set; }
        public decimal TotalMonth { get; set; }
        public decimal Gratuity { get; set; }
    }

    public class VirakiEmployeeAsCustomer
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
    }


    public class SearchLeaveApproval
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long EmployeeCode { get; set; }
        public long GodownID { get; set; }
        public long RoleID { get; set; }
    }

    public class LeaveApprovalListResponse
    {
        public long LeaveApplicationID { get; set; }
        public long EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string PrimaryAddress { get; set; }
        public string AreaName { get; set; }
        public long PrimaryPin { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public string ESIC { get; set; }
        public string GodownAddress1 { get; set; }
        public string GodownAddress2 { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicationDatestr { get; set; }
        public string Subject { get; set; }
        public string Sir { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDateLable { get; set; }
        public string FromDatestr { get; set; }
        public string ToLable { get; set; }
        public DateTime ToDate { get; set; }
        public string ToDatestr { get; set; }
        public string ToDateLable { get; set; }
        public decimal NoofDays { get; set; }
        public string Reason { get; set; }
        public string ReasonLable { get; set; }
        public string GoingToLable { get; set; }
        public string GoingTo { get; set; }
        public string AdvanceLable { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string MalaLabel { get; set; }
        public string AdvanceAmountInWordsLabel { get; set; }
        public string AdvanceAmountInWords { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
        public string Line7 { get; set; }
        public string AdvanceReason { get; set; }
        public string NoteLabel { get; set; }
        public string NoteLine1 { get; set; }
        public string NoteLine2 { get; set; }
        public string NoteLine3 { get; set; }
        public string NoteLine4 { get; set; }
        public string NoteLine5 { get; set; }
        public string VirakiMarathi { get; set; }
        public string YoursFaithfully { get; set; }
        public decimal DeductionPerMonthAmount { get; set; }
        public long LeaveStatusID { get; set; }
        public string LeaveStatus { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal TotalClosingAdvance { get; set; }
        public DateTime ApprovalFromDate { get; set; }
        public string ApprovalFromDatestr { get; set; }
        public DateTime ApprovalToDate { get; set; }
        public string ApprovalToDatestr { get; set; }
        public decimal ApprovalAdvanceAmount { get; set; }
    }

    public class LeaveApprovalListForExp
    {
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string ApplicationDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Reason { get; set; }
        public string GoingTo { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string AdvanceReason { get; set; }
        public decimal DeductionPerMonthAmount { get; set; }
        public string ApprovalFromDate { get; set; }
        public string ApprovalToDate { get; set; }
        public decimal ApprovalAdvanceAmount { get; set; }
    }

    public class LeaveApplicationListForExp
    {
        public string EmployeeName { get; set; }
        public string ApplicationDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Reason { get; set; }
        public string GoingTo { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string AdvanceReason { get; set; }
        public decimal DeductionPerMonthAmount { get; set; }
        public string ApprovalFromDate { get; set; }
        public string ApprovalToDate { get; set; }
        public decimal ApprovalAdvanceAmount { get; set; }
    }

    public class VehicleListForAllowanceResponse
    {
        public long MonthID { get; set; }
        public long YearID { get; set; }
        public long EmployeeCode { get; set; }
        public DateTime AssignedDate { get; set; }
        public string TempoNo { get; set; }
        public string AssignedDatestr { get; set; }
        public string VehicleNo { get; set; }
        public string DeliveryPerson1 { get; set; }
        public string DeliveryPerson2 { get; set; }
        public string DeliveryPerson3 { get; set; }
        public string DeliveryPerson4 { get; set; }
        public string AreaName { get; set; }
    }

    public class VehicleListForAllowanceForExp
    {
        public string Date { get; set; }
        public string TempoNo { get; set; }
        public string VehicleNo { get; set; }
        public string DeliveryPerson1 { get; set; }
        public string DeliveryPerson2 { get; set; }
        public string DeliveryPerson3 { get; set; }
        public string DeliveryPerson4 { get; set; }
        public string Area { get; set; }
    }

    public class VehiclePersonDetails
    {
        public string VehicleNo { get; set; }
        public string DeliveryPerson1 { get; set; }
        public string DeliveryPerson2 { get; set; }
        public string DeliveryPerson3 { get; set; }
        public string DeliveryPerson4 { get; set; }
        public string AreaName { get; set; }
        public string TempoNo { get; set; }
    }

    public class CalculateEmployeeSalary
    {
        public int TotalDays { get; set; }
        public int TotalMonthDay { get; set; }
        public decimal OpeningLeaves { get; set; }
        public decimal EarnedLeaves { get; set; }
        public decimal TotalLeaves { get; set; }
        public decimal BasicAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal ConveyancePerDay { get; set; }
        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal CityAllowance { get; set; }
        public decimal AdditionalCityAllowance { get; set; }
        public decimal TotalCityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal AdditionalVehicleAllowance { get; set; }
        public decimal TotalVehicleAllowance { get; set; }
        public decimal GrossWagesPayable { get; set; }
        public decimal PF { get; set; }
        public decimal ESIC { get; set; }
        public decimal PT { get; set; }
        public decimal MLWF { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetWagesPaid { get; set; }
        public decimal ConveyancePerMonth { get; set; }
        public decimal Conveyance { get; set; }
        public decimal AdditionalConveyance { get; set; }
        public decimal TotalConveyance { get; set; }
        public decimal PerformanceAllowanceAct { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal AdditionalPerformanceAllowance { get; set; }
        public decimal TotalPerformanceAllowance { get; set; }
        public decimal CityAllowanceMinutes { get; set; }
        public decimal CityAllowanceHours { get; set; }
        public decimal HighestSlab { get; set; }
        public decimal HighestPF { get; set; }
        public decimal PFPercentage { get; set; }
        public decimal EmployeeSlab { get; set; }
        public decimal PTHighestSlab { get; set; }
        public decimal PTHighestAmount { get; set; }
        public decimal PTLowestSlab { get; set; }
        public decimal PTLowestAmount { get; set; }
        public decimal MLWFHighestSlab { get; set; }
        public decimal MLWFHighestAmount { get; set; }
        public int MLWFMonthID { get; set; }
        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal AnyOtherDeductions1 { get; set; }
        public decimal AnyOtherDeductions2 { get; set; }
        public long CustomerID { get; set; }
        public int AMonth { get; set; }
        public int AYear { get; set; }

        // 31 May 2023 Dhruvik
        public decimal AdditionalCityAllowanceMinutes { get; set; }
        // 31 May 2023 Dhruvik
    }

    public class AddPayment
    {
        public int MonthID { get; set; }
        public int YearID { get; set; }
        public long GodownID { get; set; }
        public long EmployeeCode { get; set; }
        public long BankID { get; set; }
        public DateTime PaymentDate { get; set; }
        public long SalarySheetID { get; set; }
        public decimal NetWagesPaid { get; set; }
        public decimal Deductions { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal AnyOtherDeductions1 { get; set; }
        public decimal AnyOtherDeductions2 { get; set; }
        public decimal FinalTotalDeductions { get; set; }
        public decimal NetWagesToPay { get; set; }

    }

    public class SalaryPaymentExistModel
    {
        public long PaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PaidPaymentList
    {
        public DateTime PaymentDate { get; set; }
        public long GodownID { get; set; }
        public long BankID { get; set; }
        public long EmployeeCode { get; set; }

        public long PaymentID { get; set; }
        public string Bank_Name { get; set; }
        public string Client_Code { get; set; }
        public string Product_Code { get; set; }
        public string Payment_Type { get; set; }
        public string Payment_Ref_No { get; set; }
        public string Payment_Date { get; set; }
        public string Instrument_Date { get; set; }
        public string Dr_Ac_No { get; set; }
        public decimal Amount { get; set; }
        public string Bank_Code_Indicator { get; set; }
        public string Beneficiary_Code { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Beneficiary_Branch_IFSC_Code { get; set; }
        public string Beneficiary_Acc_No { get; set; }
        public string Location { get; set; }
        public string Print_Location { get; set; }
        public string Instrument_Number { get; set; }
        public string Ben_Add1 { get; set; }
        public string Ben_Add2 { get; set; }
        public string Ben_Add3 { get; set; }
        public string Ben_Add4 { get; set; }
        public string Beneficiary_Email { get; set; }
        public string Beneficiary_Mobile { get; set; }
        public string Debit_Narration { get; set; }
        public string Credit_Narration { get; set; }
        public string Payment_Details_1 { get; set; }
        public string Payment_Details_2 { get; set; }
        public string Payment_Details_3 { get; set; }
        public string Payment_Details_4 { get; set; }

        public string Bill_No { get; set; }
        public string Bill_Date { get; set; }
        public decimal Net_Wages_Paid { get; set; }
        public decimal Deductions { get; set; }

        public decimal NetAmount { get; set; }
        public string Enrichment_6 { get; set; }
        public string Enrichment_7 { get; set; }
        public string Enrichment_8 { get; set; }
        public string Enrichment_9 { get; set; }
        public string Enrichment_10 { get; set; }
        public string Enrichment_11 { get; set; }
        public string Enrichment_12 { get; set; }
        public string Enrichment_13 { get; set; }
        public string Enrichment_14 { get; set; }
        public string Enrichment_15 { get; set; }
        public string Enrichment_16 { get; set; }
        public string Enrichment_17 { get; set; }
        public string Enrichment_18 { get; set; }
        public string Enrichment_19 { get; set; }
        public string Enrichment_20 { get; set; }
        public decimal sumAmount { get; set; }
        public decimal sumNetAmount { get; set; }
        public string FullName { get; set; }
    }

    public class PaidSalaryListExport
    {
        public string Bank_Name { get; set; }
        public string Client_Code { get; set; }
        public string Product_Code { get; set; }
        public string Payment_Type { get; set; }
        public string Payment_Ref_No { get; set; }
        public string Payment_Date { get; set; }
        public string Instrument_Date { get; set; }
        public string Dr_Ac_No { get; set; }
        public decimal Amount { get; set; }
        public string Bank_Code_Indicator { get; set; }
        public string Beneficiary_Code { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Beneficiary_Branch_IFSC_Code { get; set; }
        public string Beneficiary_Acc_No { get; set; }
        public string Location { get; set; }
        public string Print_Location { get; set; }
        public string Instrument_Number { get; set; }
        public string Ben_Add1 { get; set; }
        public string Ben_Add2 { get; set; }
        public string Ben_Add3 { get; set; }
        public string Ben_Add4 { get; set; }
        public string Beneficiary_Email { get; set; }
        public string Beneficiary_Mobile { get; set; }
        public string Debit_Narration { get; set; }
        public string Credit_Narration { get; set; }
        public string Payment_Details_1 { get; set; }
        public string Payment_Details_2 { get; set; }
        public string Payment_Details_3 { get; set; }
        public string Payment_Details_4 { get; set; }

        public string Bill_No { get; set; }
        public string Bill_Date { get; set; }
        public decimal Net_Wages_Paid { get; set; }
        public decimal Deductions { get; set; }

        public decimal Net { get; set; }
        public string Enrichment_6 { get; set; }
        public string Enrichment_7 { get; set; }
        public string Enrichment_8 { get; set; }
        public string Enrichment_9 { get; set; }
        public string Enrichment_10 { get; set; }
        public string Enrichment_11 { get; set; }
        public string Enrichment_12 { get; set; }
        public string Enrichment_13 { get; set; }
        public string Enrichment_14 { get; set; }
        public string Enrichment_15 { get; set; }
        public string Enrichment_16 { get; set; }
        public string Enrichment_17 { get; set; }
        public string Enrichment_18 { get; set; }
        public string Enrichment_19 { get; set; }
        public string Enrichment_20 { get; set; }
    }

    public class SalaryPaymentListResponse
    {
        public long PaymentID { get; set; }
        public int RowNumber { get; set; }
        public string PaymentDate { get; set; }
        public long EmployeeCode { get; set; }
        public string GodownName { get; set; }
        public string Beneficiary_Name { get; set; }
        public decimal Amount { get; set; }
        public string Beneficiary_Bank { get; set; }
        public string Beneficiary_Acc_No { get; set; }
        public string Beneficiary_Branch_IFSC_Code { get; set; }
        public string Beneficiary_Mobile { get; set; }
        public string Beneficiary_Email { get; set; }
    }

    public class SalaryPayment
    {
        public long PaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public long BankID { get; set; }
    }


    public class AllowanceDetailCount
    {
        public long RecordCount { get; set; }
    }

    public class EmployeeCodeList
    {
        public long EmployeeCode { get; set; }
    }

    public class ForwardAllownceDetailList
    {
        public long EmployeeCode { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal TotalHouseRentAllowance { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal Conveyance { get; set; }
        public decimal ConveyancePerDay { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public long PerformanceAllowanceStatusID { get; set; }
        public long CityAllowanceStatusID { get; set; }
        public long PFStatusID { get; set; }
        public long ESICStatusID { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal BonusAmount { get; set; }
        public long BonusStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public long LeaveEnhancementStatusID { get; set; }
        public decimal GratuityPercentage { get; set; }
        public decimal GratuityAmount { get; set; }
        public long GratuityStatusID { get; set; }
        public long CustomerID { get; set; }
    }

    public class ModelSalarySlip
    {
        public long EmployeeCode { get; set; }
        public string BankName { get; set; }
        public string EmployeeName { get; set; }
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PanNo { get; set; }
        public string Email { get; set; }
        public long AadharNumber { get; set; }
        public string Designation { get; set; }
        public string ESICNumber { get; set; }
        public string GodownName { get; set; }
        public string PFNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public string UANNumber { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal AdditionalPerformanceAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal AdditionalConveyance { get; set; }
        public decimal CityAllowance { get; set; }
        public decimal AdditionalCityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal AdditionalVehicleAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal ESIC { get; set; }
        public decimal PT { get; set; }
        public decimal MLWF { get; set; }
        public decimal TDS { get; set; }
        public decimal Goods { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDecuctions { get; set; }
        public decimal NetPay { get; set; }
        public string AmountInWords { get; set; }

        public decimal OpeningLeaves { get; set; }
        public decimal EarnedLeaves { get; set; }
        public decimal AvailedLeaves { get; set; }
        public decimal AdditionalAvailedLeaves { get; set; }
        public decimal TotalAvailedLeaves { get; set; }

        public decimal ClosingLeaves { get; set; }
        public decimal AdditionalClosingLeaves { get; set; }
        public decimal TotalClosingLeaves { get; set; }

        public decimal OpeningAdvance { get; set; }
        public decimal Addition { get; set; }
        public decimal Deductions { get; set; }
        public decimal ClosingAdvance { get; set; }

        public string Month { get; set; }
        public int Year { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDatestr { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDatestr { get; set; }
    }

    public class MonthlyAttendanceList
    {
        public int RowNumber { get; set; }
        public long EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Sunday { get; set; }
        public int Holiday { get; set; }
        public decimal OTMinutes { get; set; }
        public string OTHours { get; set; }
        public long VehicleNoOfDaysCount { get; set; }

        public int sumPresent { get; set; }
        public int sumAbsent { get; set; }
        public int sumSunday { get; set; }
        public int sumHoliday { get; set; }
        public string sumTotalOTHrs { get; set; }
        public long sumVehicleNoOfDaysCount { get; set; }
    }

    public class MonthlyAttendanceForExp
    {
        public int RowNumber { get; set; }
        public string EmployeeName { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Sunday { get; set; }
        public int Holiday { get; set; }
        public string OTHours { get; set; }
        public long VehicleNoOfDaysCount { get; set; }
    }

    public class GetDateofLeaving
    {
        public DateTime DateofLeaving { get; set; }
        public string DateofLeavingstr { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
    }

    public class ModelCalculateBonus
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromMonthID { get; set; }
        public int FromYearID { get; set; }
        public int ToMonthID { get; set; }
        public int ToYearID { get; set; }
        public int GodownID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal TotalBonus { get; set; }
        public int BonusStatusID { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal ActBonusAmount { get; set; }
        public decimal BonusPercentage { get; set; }
    }

    public class ModelCalculateLeaveEncashment
    {
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        //public int FromMonthID { get; set; }
        //public int FromYearID { get; set; }
        //public int ToMonthID { get; set; }
        //public int ToYearID { get; set; }
        //public int GodownID { get; set; }
        public long SalarySheetID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal TotalLeaveEnhancement { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal LeaveEncashment { get; set; }
        public int LeaveEnhancementStatusID { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
    }

    public class AddGratuityandHisab
    {
        public long Gratuity_Hisab_ID { get; set; }
        public long GodownID { get; set; }
        public long EmployeeCode { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public decimal LastDrawnSalary { get; set; }
        public int TotalMonth { get; set; }
        public decimal Gratuity { get; set; }
        public decimal AdditionalGratuity { get; set; }
        public decimal TotalGratuity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalEarnedSalary { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal AdditionalBonusAmount { get; set; }
        public decimal TotalBonusAmount { get; set; }
        public decimal LastDrawnSalaryLeaveEnhancement { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal LeaveEncashment { get; set; }
        public decimal AdditionalLeaveEncashment { get; set; }
        public decimal TotalLeaveEncashment { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public long WitnessOneID { get; set; }
        public long WitnessTwoID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }


    public class Gratuity_HisabListResponse
    {
        public long Gratuity_Hisab_ID { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string DateOfLeavingstr { get; set; }
        public decimal LastDrawnSalary { get; set; }
        public int TotalMonth { get; set; }
        public decimal Gratuity { get; set; }
        public decimal AdditionalGratuity { get; set; }
        public decimal TotalGratuity { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDatestr { get; set; }
        public DateTime ToDate { get; set; }
        public string ToDatestr { get; set; }
        public decimal TotalEarnedSalary { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal AdditionalBonusAmount { get; set; }
        public decimal TotalBonusAmount { get; set; }
        public decimal LastDrawnSalaryLeaveEnhancement { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal LeaveEncashment { get; set; }
        public decimal AdditionalLeaveEncashment { get; set; }
        public decimal TotalLeaveEncashment { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public long WitnessOneID { get; set; }
        public long WitnessTwoID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
    }


    public class ModelEmployeeDetail
    {
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string PrimaryAddress { get; set; }
        public string AreaName { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateofLeaving { get; set; }
        public string DateofLeavingstr { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
    }
    public class AddResignationLetter
    {
        public long ResignationID { get; set; }
        public long EmployeeCode { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public DateTime DateOfApplication { get; set; }
        public int Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ResignationLetterResponse
    {
        public long ResignationID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string PrimaryAddress { get; set; }
        public string AreaName { get; set; }
        public string PinCode { get; set; }
        public string FullAddress { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string DateOfLeavingstr { get; set; }
        public DateTime DateOfApplication { get; set; }
        public string DateOfApplicationstr { get; set; }
        public int Status { get; set; }
        public string Statusstr { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PrintResignationLetter
    {
        public long ResignationID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string PrimaryAddress { get; set; }
        public string AreaName { get; set; }
        public string PinCode { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string DateOfLeavingstr { get; set; }
        public DateTime DateOfApplication { get; set; }
        public string DateOfApplicationstr { get; set; }
        public int Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ResignationAcceptanceLetterListResponse
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long ResignationID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfApplication { get; set; }
        public string DateOfApplicationstr { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DateOfJoiningstr { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string DateOfLeavingstr { get; set; }
        public int Status { get; set; }
        public string Statusstr { get; set; }
    }

    public class PrintResignationAcceptanceLetter
    {
        public long ResignationID { get; set; }
        public long EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDatestr { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string DateOfLeavingstr { get; set; }
    }

    public class PavatiListResponse
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long EmployeeCode { get; set; }
        public long Gratuity_Hisab_ID { get; set; }
        public string FullName { get; set; }
        public decimal LastDrawnSalary { get; set; }
        public int TotalMonth { get; set; }
        public string TotalService { get; set; }
        public string TotalServiceEng { get; set; }
        public decimal Gratuity { get; set; }
        public decimal AdditionalGratuity { get; set; }
        public decimal TotalGratuity { get; set; }
        public decimal TotalEarnedSalary { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal AdditionalBonusAmount { get; set; }
        public decimal TotalBonusAmount { get; set; }
        public decimal LastDrawnSalaryLeaveEnhancement { get; set; }
        public decimal ClosingLeaves { get; set; }
        public decimal LeaveEncashment { get; set; }
        public decimal AdditionalLeaveEncashment { get; set; }
        public decimal TotalLeaveEncashment { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public decimal Advance { get; set; }
        public decimal NetAmount { get; set; }
        public string AmountInWords { get; set; }
        public string WitnessOne { get; set; }
        public string WitnessTwo { get; set; }
        public string Year { get; set; }
        public int Status { get; set; }
        public string Statusstr { get; set; }
        public long WitnessOneID { get; set; }
        public long WitnessTwoID { get; set; }
        public DateTime DateOfApplication { get; set; }
        public string DateOfApplicationstr { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }
    }




    public class YearlySalarySheetReq
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long EmployeeCode { get; set; }
    }


    public class YearlySalarySheetList
    {
        public long SalarySheetID { get; set; }
        public int RowNumber { get; set; }
        public long EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PrimaryAddress { get; set; }
        public long PrimaryPin { get; set; }
        public string AreaName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string PanNo { get; set; }
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public int YearID { get; set; }
        public decimal GrossWagesPayable { get; set; }
        //  public decimal BasicAllowance { get; set; }
        public decimal EarnedBasicWages { get; set; }
        public decimal EarnedHouseRentAllowance { get; set; }
        public decimal CityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal PT { get; set; }
        public decimal ESIC { get; set; }
        public decimal MLWF { get; set; }
        public decimal TDS { get; set; }
        public long BonusStatusID { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal TotalEarnedWages { get; set; }
        public decimal TotalBonus { get; set; }
        public long LeaveEnhancementStatusID { get; set; }
        public decimal LeaveEnhancementAmount { get; set; }
        public decimal LeaveEnhancementPercentage { get; set; }
        public decimal LeaveEncashment { get; set; }
        public decimal GrandTotalWages { get; set; }
        public decimal TotalLeaveEnhancement { get; set; }
        public long MaxSalarySheetID { get; set; }
        public decimal TotalBasicAllowance { get; set; }
        public decimal sumGrossWagesPayable { get; set; }
        public decimal sumEarnedBasicWages { get; set; }
        public decimal sumEarnedHouseRentAllowance { get; set; }
        public decimal sumCityAllowance { get; set; }
        public decimal sumVehicleAllowance { get; set; }
        public decimal sumConveyance { get; set; }
        public decimal sumPerformanceAllowance { get; set; }
        public decimal sumPF { get; set; }
        public decimal sumPT { get; set; }
        public decimal sumESIC { get; set; }
        public decimal sumMLWF { get; set; }
        public decimal sumTDS { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Gratuity { get; set; }
        public decimal TDSSalary { get; set; }

        //public decimal StandardDeductions4_a { get; set; }
        //public decimal UnderSection80C { get; set; }
        //public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        //public decimal InterestOn80E { get; set; }
        //public decimal UnderSection80TTA { get; set; }
        //public decimal RebateUnderSection87A { get; set; }
    }


    public class YearlySalarySheetExport
    {
        public int SrNo { get; set; }
        public string MonthofSalary { get; set; }
        public decimal GrossWages { get; set; }
        public decimal EBA { get; set; }
        public decimal EHRA { get; set; }
        public decimal CityAllowance { get; set; }
        public decimal VehicleAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal PerformanceAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal PT { get; set; }
        public decimal ESIC { get; set; }
        public decimal MLWF { get; set; }
        public decimal TDS { get; set; }
    }



    public class PrintYearlySalarySheet
    {
        public long EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        public string PrimaryAddress { get; set; }
        public long PrimaryPin { get; set; }
        public string AreaName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public string PanNo { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PerquisitesUnderSection_17_2 { get; set; }
        public decimal SalaryUnderSection_17_3 { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal SalaryReceivedFromOtherEmployer1_e { get; set; }


        public decimal TravelConcession2_a { get; set; }
        public decimal Gratuity { get; set; }
        public decimal CommutedValueofPension2_c { get; set; }
        public decimal LeaveSalaryEncashment2_d { get; set; }
        public decimal HouseRentAllowance2_e { get; set; }
        public decimal AmountofAnyOtherExemption2_f { get; set; }
        public decimal TotalAmountofAnyOtherExemption2_g { get; set; }
        public decimal TotalAmountofExemptionClaimed2_h { get; set; }

        public decimal TotalAmountofSalary_3 { get; set; }

        public decimal StandardDeductions4_a { get; set; }
        public decimal EntertainmentAllowance4_b { get; set; }
        public decimal TaxOnEmployment4_c { get; set; }
        public decimal TotalAmountOfDeductions_5 { get; set; }

        public decimal lncomeChargeable_6 { get; set; }


        public decimal IncomeFromHouseProperty7_a { get; set; }
        public decimal IncomeUnderTheHead_OtherSources7_b { get; set; }
        public decimal TotalAmountOfOtherIncome_8 { get; set; }


        public decimal GrossTotalIncome { get; set; }

        public decimal UnderSection80C { get; set; }
        public decimal GrossAmount10_a { get; set; }
        public decimal DeductibleAmount10_a { get; set; }
        public decimal GrossAmount10_b { get; set; }
        public decimal DeductibleAmount10_b { get; set; }
        public decimal GrossAmount10_c { get; set; }
        public decimal DeductibleAmount10_c { get; set; }
        public decimal TotalGrossAmount10_d { get; set; }
        public decimal TotalDeductibleAmount10_d { get; set; }
        public decimal GrossAmount10_e { get; set; }
        public decimal DeductibleAmount10_e { get; set; }
        public decimal GrossAmount10_f { get; set; }
        public decimal DeductibleAmount10_f { get; set; }

        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D_Actual { get; set; }
        public decimal GrossAmount10_g { get; set; }
        public decimal DeductibleAmount10_g { get; set; }

        public decimal InterestOn80E { get; set; }
        public decimal InterestOn80E_Actual { get; set; }
        public decimal GrossAmount10_h { get; set; }
        public decimal DeductibleAmount10_h { get; set; }


        public decimal GrossAmount10_i { get; set; }
        public decimal QualifyingAmount10_i { get; set; }
        public decimal DeductibleAmount10_i { get; set; }


        public decimal GrossAmount10_j { get; set; }
        public decimal QualifyingAmount10_j { get; set; }
        public decimal DeductibleAmount10_j { get; set; }

        public decimal AggregateOfDeductibleAmount_11 { get; set; }
        public decimal TotalTaxableIncome_12 { get; set; }
        public decimal TaxOnTotalIncome_13 { get; set; }
        public decimal RebateUnderSection87A_14 { get; set; }
        public decimal Surcharge_15 { get; set; }
        public decimal Education_16 { get; set; }
        public decimal TaxPayable_17 { get; set; }
        public decimal ReliefUnderSection89_18 { get; set; }
        public decimal NetTaxPayable_19 { get; set; }
        public decimal TDSDeducted_20 { get; set; }
        public decimal TotalTaxPayble_21 { get; set; }

        public string Period { get; set; }
        public string CertificateNo { get; set; }
        public string LastUpdatedOn { get; set; }
        public string FinancialYear { get; set; }
        public string AssessmentYear { get; set; }
        public string FromEmployer { get; set; }
        public string ToEmployer { get; set; }
        public decimal StandardDeduction { get; set; }
        public decimal Balance { get; set; }
        public decimal EntertainmentAllowance { get; set; }
        public decimal TaxOnEmployment { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal IncomeChargeable { get; set; }
        public decimal AnyOtherIncome { get; set; }

        // public decimal Section80C { get; set; }
        public decimal PF { get; set; }
        public decimal Investment { get; set; }
        public decimal ESIC { get; set; }
        public decimal Mediclaim { get; set; }
        public decimal AggregateOfDeductibleAmount { get; set; }
        public decimal TotalIncome { get; set; }

        public decimal UnderSection80TTA { get; set; }
        public decimal UnderSection80TTA_Actual { get; set; }
        public decimal RebateUnderSection87A { get; set; }
        public decimal TaxSlabPer { get; set; }
        public decimal SurchargeSlabPer { get; set; }
        public decimal EducationSlabPer { get; set; }

        public decimal PensionUnderSection80CCD_1 { get; set; }
        public decimal PensionUnderSection80CCD_1_Actual { get; set; }

        //20 - 05 - 2022
        public decimal IncomeFrom { get; set; }
        public decimal IncomeTo { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal TaxOnTotalIncome_one { get; set; }
        public decimal RebateUnderSection87A_Income { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }
        //20 - 05 - 2022

        public string Date { get; set; }
    }

    public class FormSixteenDetails
    {
        public long FormSixteenID { get; set; }
        public decimal StandardDeductions { get; set; }
        public decimal HousingLoanPrincipal { get; set; }
        public decimal ELSS { get; set; }
        public decimal PPF { get; set; }
        public decimal LifeInsurance { get; set; }
        public decimal Others { get; set; }
        public decimal TDSSalary { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D_Actual { get; set; }
        public decimal InterestOn80E { get; set; }
        public decimal InterestOn80E_Actual { get; set; }
        public decimal UnderSection80G { get; set; }
        public decimal UnderSection80TTA { get; set; }
        public decimal UnderSection80TTA_Actual { get; set; }
        public decimal RebateUnderSection87A { get; set; }

        public decimal UnderSection80C { get; set; }

        public decimal TaxSlabPer { get; set; }
        public decimal SurchargeSlabPer { get; set; }
        public decimal EducationSlabPer { get; set; }

        public decimal PensionUnderSection80CCD_1 { get; set; }
        public decimal PensionUnderSection80CCD_1_Actual { get; set; }

        public decimal IncomeFrom { get; set; }
        public decimal IncomeTo { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal TaxOnTotalIncome_one { get; set; }
        public decimal RebateUnderSection87A_Income { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }

        public bool IsFiftyPer { get; set; }

        //Add By Dhruvik
        public decimal FormSixteenEHRA { get; set; }
        //Add By Dhruvik
    }

    //Form16TexableIncome
    public class AddFormSixteenDetail
    {
        public string DeleteItems { get; set; }
        public long FormSixteenDetailID { get; set; }
        public decimal StandardDeduction { get; set; }
        public decimal UnderSection80C { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal InterestOn80E { get; set; }
        public decimal UnderSection80TTA { get; set; }
        public decimal RebateUnderSection87A { get; set; }
        public decimal PensionUnderSection80CCD_1 { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }

        public List<AddFormSixteenTexableIncome> lstForm16TexableIncomeQty { get; set; }
        
    }

    //Form16texableIncome
    public class AddFormSixteenTexableIncome
    {
        public long FormSixteenTexableID { get; set; }
        public long FormSixteenDetailID { get; set; }
        public decimal IncomeFrom { get; set; }
        public decimal IncomeTo { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal TaxOnTotalIncome_one { get; set; }
        public decimal RebateUnderSection87A_Income { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }
    }



    public class FormSixteenDetailListResponse
    {
        public long FormSixteenDetailID { get; set; }
        public decimal StandardDeduction { get; set; }
        public decimal UnderSection80C { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal InterestOn80E { get; set; }
        public decimal UnderSection80TTA { get; set; }
        public decimal RebateUnderSection87A { get; set; }
        public decimal PensionUnderSection80CCD_1 { get; set; }

        //Form16TexableIncome
        public long FormSixteenTexableID { get; set; }
        public decimal IncomeFrom { get; set; }
        public decimal IncomeTo { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal TaxOnTotalIncome_one { get; set; }
        public decimal RebateUnderSection87A_Income { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }
        //Form16TexableIncome

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
    }




    public class FormSixteenData
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long EmployeeCode { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public decimal SalarySection17_1 { get; set; }
        public decimal ValueOfPerquisitesSection17_2 { get; set; }
        public decimal ProfitsInlieuSection17_3 { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal SalaryReceivedFromOther { get; set; }
        public decimal TravelConcessionSection10_5 { get; set; }
        public decimal GratuitySection10_10 { get; set; }
        public decimal CommutedValueOfPensionSection10_10A { get; set; }
        public decimal LeaveSalaryEncashmentSection10_10AA { get; set; }
        public decimal HRASection10_13A { get; set; }
        public decimal AmountOfAnyOtherExemptionSection10 { get; set; }
        public decimal TotalAmountOfAnyOtherExemptionSection10 { get; set; }
        public decimal TotalAmountOfAnyOtherExemptionClaimedSection10 { get; set; }
        public decimal TotalAmountofSalary { get; set; }
        public decimal StandardDeductionsSection16_ia { get; set; }
        public decimal EntertainmentAllowanceSection16_ii { get; set; }
        public decimal TaxOnEmploymentSection16_iii { get; set; }
        public decimal TotalAmountOfDeductionsSection16 { get; set; }
        public decimal lncomeChargeable { get; set; }
        public decimal IncomeFromHouseProperty { get; set; }
        public decimal IncomeUnderTheHeadOtherSources { get; set; }
        public decimal TotalAmountOfOtherIncome { get; set; }
        public decimal GrossTotalIncome { get; set; }
        public decimal HousingLoanPrincipal { get; set; }
        public decimal ELSS { get; set; }
        public decimal PPF { get; set; }
        public decimal Others { get; set; }
        public decimal LifeInsurance { get; set; }
        public decimal UnderSection80C { get; set; }
        public decimal GrossAmountSection80C { get; set; }
        public decimal DeductibleAmountSection80C { get; set; }
        public decimal GrossAmountSection80CCC { get; set; }
        public decimal DeductibleAmountSection80CCC { get; set; }
        public decimal GrossAmountSection80CCD_1 { get; set; }
        public decimal DeductibleAmountSection80CCD_1 { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal TotalDeductibleAmount { get; set; }
        public decimal GrossAmountSection80CCD_1B { get; set; }
        public decimal DeductibleAmountSection80CCD_1B { get; set; }
        public decimal GrossAmountSection80CCD_2 { get; set; }
        public decimal DeductibleAmountSection80CCD_2 { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal GrossAmountSection80D { get; set; }
        public decimal DeductibleAmountSection80D { get; set; }
        public decimal InterestOn80E { get; set; }
        public decimal GrossAmountSection80E { get; set; }
        public decimal DeductibleAmountSection80E { get; set; }
        public decimal GrossAmountSection80G { get; set; }
        public decimal QualifyingAmountSection80G { get; set; }
        public decimal DeductibleAmountSection80G { get; set; }
        public decimal GrossAmountSection80TTA { get; set; }
        public decimal QualifyingAmountSection80TTA { get; set; }
        public decimal DeductibleAmountSection80TTA { get; set; }
        public decimal AggregateOfDeductibleAmount { get; set; }
        public decimal TotalTaxableIncome { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal RebateSection87A { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }
        public decimal TaxPayable { get; set; }
        public decimal ReliefSection89 { get; set; }
        public decimal NetTaxPayable { get; set; }
        public decimal TDSDeducted { get; set; }
        public decimal TotalTaxPayble { get; set; }
        public bool IsFiftyPer { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
    }




    public class FormSixteenValueModel
    {
        public long FormSixteenID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FullName { get; set; }
        public decimal StandardDeductions { get; set; }
        public decimal HousingLoanPrincipal { get; set; }
        public decimal ELSS { get; set; }
        public decimal PPF { get; set; }
        public decimal LifeInsurance { get; set; }
        public decimal Others { get; set; }
        public decimal TDSSalary { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D_Actual { get; set; }
        public decimal InterestOn80E { get; set; }
        public decimal InterestOn80E_Actual { get; set; }
        public decimal UnderSection80G { get; set; }
        public decimal UnderSection80TTA { get; set; }
        public decimal UnderSection80TTA_Actual { get; set; }
        public decimal RebateUnderSection87A { get; set; }
        public decimal UnderSection80C { get; set; }
        public decimal TaxSlabPer { get; set; }
        public decimal SurchargeSlabPer { get; set; }
        public decimal EducationSlabPer { get; set; }
        public decimal PensionUnderSection80CCD_1 { get; set; }
        public decimal PensionUnderSection80CCD_1_Actual { get; set; }
        public bool IsFiftyPer { get; set; }
        public long EmployeeCode { get; set; }

        //20 - 05 - 2022 from16TexableIncome
        public decimal IncomeFrom { get; set; }
        public decimal IncomeTo { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal TaxOnTotalIncome_one { get; set; }
        public decimal RebateUnderSection87A_Income { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }
        //20 - 05 - 2022 from16TexableIncome
    }

    public class EmployeeFormSixteenDataReq
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long EmployeeCode { get; set; }
        public decimal StandardDeduction { get; set; }
        public decimal HousingLoanPrincipal { get; set; }
        public decimal ELSS { get; set; }
        public decimal PPF { get; set; }
        public decimal LifeInsurance { get; set; }
        public decimal Others { get; set; }
        public decimal TDSSalary { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D { get; set; }
        public decimal HealthInsurancePremiaUnderSection80D_Actual { get; set; }
        public decimal InterestOn80E { get; set; }
        public decimal InterestOn80E_Actual { get; set; }
        public decimal UnderSection80G { get; set; }
        public decimal UnderSection80TTA { get; set; }
        public decimal UnderSection80TTA_Actual { get; set; }
        public bool IsFiftyPer { get; set; }
        public decimal RebateUnderSection87A { get; set; }
        public decimal UnderSection80C { get; set; }
        public decimal TaxSlabPer { get; set; }
        public decimal SurchargeSlabPer { get; set; }
        public decimal EducationSlabPer { get; set; }
        public decimal PensionUnderSection80CCD_1 { get; set; }
        public decimal PensionUnderSection80CCD_1_Actual { get; set; }

        //20 - 05 - 2022 from16TexableIncome
        public decimal IncomeFrom { get; set; }
        public decimal IncomeTo { get; set; }
        public decimal TaxOnTotalIncome { get; set; }
        public decimal TaxOnTotalIncome_one { get; set; }
        public decimal RebateUnderSection87A_Income { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Education { get; set; }
        //20 - 05 - 2022 from16TexableIncome

        public long FormSixteenID { get; set; }

        //Add By Dhruvik
        public decimal FormSixteenEHRA { get; set; }
        //Add By Dhruvik
    }

}
