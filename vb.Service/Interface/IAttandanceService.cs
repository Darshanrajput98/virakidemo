using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.Model;

namespace vb.Service
{
    public interface IAttandanceService
    {
        EmployeeDetail GetEmployeeDetailByEmployeeCode(string StartDate, string EndDate, long EmployeeCode);

        //Attandance Syatem
        bool AddMachineRawPunch(MachineRawPunch Obj);

        Int32 InsertDailyAttendance(DateTime ADate, string EmployeeID, string AEmployeeName);

        bool AddAllowance(Allowance_Master Obj);

        EmployeeDetail GetOpeningAllowanceDetail(long EmployeeCode);

        //bool AddAllowanceDetail(AddAllowance Obj);

        List<AllowanceListResponse> GetAllowanceList(string StartDate, string EndDate);

        bool DeleteAllowanceDetail(long AllowanceDetailID, bool IsDelete);

        //GetEmployeeAttandanceDetail GetEmployeeAttandanceDetail(int MonthID, int YearID, long EmployeeCode, string StartDate, string EndDate, string MonthStartDate);
        GetEmployeeAttandanceDetail GetEmployeeAttandanceDetail(int MonthID, int YearID, long EmployeeCode, string MonthStartDate);

        // 07 Aug 2020 Piyush Limbani
        GetEmployeeAttandanceDetail GetEmployeeAttandanceDetailBySalarySheetID(int MonthID, int YearID, long EmployeeCode, long SalarySheetID);




        // long CheckSalaryExist(int MonthID, int YearID, long EmployeeCode);

        //20-04-2020
        SalaryExistModel CheckSalaryExist(int MonthID, int YearID, long EmployeeCode);

        long AddSalarySheet(SalarySheet_Master Obj);

        List<SalarySheetListResponse> GetSalarySheetList(SearchSalarySheet model);

        List<SalarySheetListResponse> GetExportExcelSalarySheet(long MonthID, long YearID, long GodownID, long EmployeeCode);

        //14-04-2020
        GetTotalDatsIfTheMonth GetTotalDaysMonthInTheMonth(int MonthID, int YearID);

        //16-04-2020 Attendance Changes
        bool AddNewDailyAttendance(NewDailyAttendanceModel Obj);

        List<AttandanceListResponse> GetAttandanceList(DateTime? FromDate, DateTime? ToDate, long GodownID, string EmployeeCode);

        //17-04-2020
        long AddFestival(Festival_Mst obj);

        List<FestivalListResponse> GetAllFestivalList();

        bool DeleteFestival(long FestivalID, bool IsDelete);

        //20-04-2020
        List<AllowanceStatusNameResponse> GetAllAllowanceStatusName();

        long AddEarnedLeaves(EarnedLeaves_Mst obj);

        List<EarnedLeavesListResponse> GetAllEarnedLeavesList();

        bool DeleteEarnedLeaves(long EarnedLeavesID, bool IsDelete);

        //22-04-2020
        List<BonusListResponse> GetEmployeeWiseBonusList(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode);

        //23-04-2020
        List<LeaveEncashmentListResponse> GetLeaveEncashmentList(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode);

        List<GratuityListResponse> GetGratuityList(int LeavingYear, int GodownID, long EmployeeCode, DateTime DateOfLeaving);

        //29-04-2020
        long AddPF(PF_Mst obj);

        List<PFListResponse> GetAllPFList();

        bool DeletePF(long PFID, bool IsDelete);

        long AddESIC(ESIC_Mst obj);

        List<ESICListResponse> GetAllESICList();

        bool DeleteESIC(long ESICID, bool IsDelete);

        long AddPT(PT_Mst obj);

        List<PTListResponse> GetAllPTList();

        bool DeletePT(long PTID, bool IsDelete);

        long AddMLWF(MLWF_Mst obj);

        List<MLWFListResponse> GetAllMLWFList();

        bool DeleteMLWF(long MLWFID, bool IsDelete);

        long AddLeaveEncashment(LeaveEncashment_Mst obj);

        List<LeaveEncashmentMstListResponse> GetAllLeaveEncashmentList();

        bool DeleteLeaveEncashment(long LeaveEncashmentID, bool IsDelete);

        long AddGratuity(Gratuity_Mst obj);

        List<GratuityMstListResponse> GetAllGratuityList();

        bool DeleteGratuity(long GratuityID, bool IsDelete);

        ClosingLeavesandClosingAdvance GetClosingLeavesandClosingAdvance(long EmployeeCode);

        long AddLeaveAndAdvanceApplication(LeaveApplication_Mst obj);

        List<LeaveApplicationListResponse> GetLeaveAndAdvanceApplicationList(DateTime? FromDate, DateTime? ToDate, long EmployeeCode);

        LeaveApplicationListResponse GetDataForLeaveApplicationPrint(long LeaveApplicationID);

        MonthList GetLastTwelveMonthDataForLeavePrint(int Month, int Year, long EmployeeCode);

        //ClosingAdvanceMonthList GetLastTwelveMonthClosingAdvanceDataForLeavePrint(int Month, int Year, long EmployeeCode);
        ClosingAdvanceMonthList GetLastTwelveMonthClosingAdvanceDataForLeavePrint(DateTime? ToDate, long EmployeeCode);

        bool UpdateApprovalLeave(ApprovalLeave data, long UserID);

        List<ActiveEmployeeCode> GetActiveEmployeeCodeFromUserMaster(long GodownID);

        // 11-05-2020
        List<CashCounterListResponse> GetCashCounterReportList(DateTime? AssignedDate, long GodownID);

        List<VirakiEmployeeAsCustomer> GetAllVirakiEmployeeAsCustomerName();

        List<LeaveApprovalListResponse> GetLeaveApprovalList(DateTime? FromDate, DateTime? ToDate, long EmployeeCode, long GodownID, long RoleID);

        // 15 July 2020 Piyush Limbani
        List<VehicleListForAllowanceResponse> GetVehicleListForAllowance(long MonthID, long YearID, long EmployeeCode);

        // 24 July 2020 Piyush Limbani
        CalculateEmployeeSalary GetCalculateEmployeeSalary(long SalarySheetID, long EmployeeCode, int MonthID, int YearID, int TotalDays, int TotalMonthDay, int TotalPresent, int TotalAvailedLeaves);

        // 28 July 2020 Piyush Limbani
        List<AddPayment> GetEmployeeSalaryDetailForPayment(int MonthID, int YearID, long GodownID, long EmployeeCode);

        // 28 July 2020 Piyush Limbani
        SalaryPaymentExistModel CheckPaymentSalaryExist(int MonthID, int YearID, long EmployeeCode);

        // 28 July 2020 Piyush Limbani
        long AddSalaryPayment(SalaryPayment_Mst Obj);

        // 29 July 2020 Piyush Limbani
        List<PaidPaymentList> GetPaidPaymentList(DateTime PaymentDate, long? GodownID, long? BankID, long? EmployeeCode);

        List<SalaryPaymentListResponse> GetSalaryPaymentListByMonthAndYear(int MonthID, int YearID);

        // 31 July 2020 Piyush Limbani
        bool UpdateSalaryPayment(SalaryPayment data, long UserID);

        // 11 Aug 2020 Piyush Limbani
        AllowanceDetailCount GetAllowanceDetailCount(string StartDate, string EndDate);

        List<EmployeeCodeList> GetAllEmployeeCodeListFromAllowanceMaster();

        ForwardAllownceDetailList GetLastAllowanceDetailForForwardNextYear(long EmployeeCode);

        // 13 Aug 2020 Piyush Limbani      
        List<ModelSalarySlip> GetDataForSalarySlipPrint(int MonthID, int YearID, long? EmployeeCode, int? GodownID);

        // 04 Sep. 2020 Piyush Limbani
        List<MonthlyAttendanceList> GetMonthlyAttendanceList(int MonthID, int YearID, int? GodownID, long? EmployeeCode);

        // 23 Dec 2020 Piyush Limbani
        List<EmployeeNameResponse> GetEmployeeByGodownID(long GodownID);

        GetDateofLeaving GetEmployeeDateofLeavingByEmployeeCode(long GodownID, long EmployeeCode);

        GratuityListResponse GetCalculateGratuity(int LeavingYear, int GodownID, long EmployeeCode, DateTime DateOfLeaving);

        ModelCalculateBonus GetCalculateBonus(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode);

        ModelCalculateLeaveEncashment GetCalculateLeaveEncashment(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode);

        long AddGratuityandHisab(Gratuity_Hisab_Mst obj);

        // bool DeActiveUserByEmployeeCode(long EmployeeCode);

        List<Gratuity_HisabListResponse> GetAllGratuity_HisabList();

        ModelEmployeeDetail GetEmployeeDetailByEmployeeCodeForResignation(long EmployeeCode);

        // 25 Dec 2020 Piyush Limbani
        long AddResignationLetter(Resignation_Mst obj);

        PrintResignationLetter GetDataForResignationLetterPrint(long ResignationID);

        List<ResignationLetterResponse> GetAllResignationLetterList();

        List<ResignationAcceptanceLetterListResponse> GetResignationAcceptanceLetterList(DateTime? FromDate, DateTime? ToDate);

        bool UpdateResignationApprovalStatus(long ResignationID, int Status, long UserID);

        PrintResignationAcceptanceLetter GetDataForResignationAcceptanceLetterPrint(long ResignationID);

        // 26 Dec 2020 Piyush Limbani
        List<PavatiListResponse> GetAllPavatiList(DateTime FromDate, DateTime ToDate, long EmployeeCode);

        PavatiListResponse GetDataForPavatiPrint(long Gratuity_Hisab_ID, long WitnessOneID, long WitnessTwoID);

        // 24 Jan 2022 Piyush Limbani
        List<YearlySalarySheetList> GetYearlySalarySheetList(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, long EmployeeCode,DateTime FromDate,DateTime ToDate);

        // 18 Feb 2022 Piyush Limbani
        FormSixteenDetailListResponse GetFormSixteenDetailFromMaster();

        // 31 Jan 2022 Piyush Limbani
        //bool AddFormSixteen(DateTime FromDate, DateTime ToDate, long EmployeeCode, decimal StandardDeductions4_a, decimal HousingLoanPrincipal, decimal ELSS, decimal PPF, decimal LifeInsurance, decimal Others, decimal HealthInsurancePremiaUnderSection80D, decimal HealthInsurancePremiaUnderSection80D_Actual, decimal InterestOn80E, decimal InterestOn80E_Actual, decimal UnderSection80G, decimal UnderSection80TTA, decimal UnderSection80TTA_Actual, bool IsFiftyPer, decimal RebateUnderSection87A, decimal UnderSection80C, decimal TaxSlabPer, decimal SurchargeSlabPer, decimal EducationSlabPer,
        //   decimal PensionUnderSection80CCD_1, decimal PensionUnderSection80CCD_1_Actual, long CreatedBy, DateTime CreatedOn, bool IsDelete);

        //bool UpdateFormSixteenDetail(long FormSixteenID, decimal StandardDeduction, decimal HousingLoanPrincipal, decimal ELSS, decimal PPF, decimal LifeInsurance, decimal Others,
        //   decimal HealthInsurancePremiaUnderSection80D_Actual, decimal InterestOn80E_Actual, decimal UnderSection80G, decimal UnderSection80TTA_Actual, bool IsFiftyPer, decimal RebateUnderSection87A, decimal UnderSection80C, decimal TaxSlabPer, decimal SurchargeSlabPer, decimal EducationSlabPer,
        //    decimal PensionUnderSection80CCD_1, decimal PensionUnderSection80CCD_1_Actual, long UpdatedBy, DateTime UpdatedOn);

        //bool AddFormSixteen(PrintYearlySalarySheet obj, EmployeeFormSixteenDataReq data, long CreatedBy, DateTime CreatedOn, bool IsDelete);
        //20 - 05 - 2022 

        bool AddFormSixteen(PrintYearlySalarySheet obj, EmployeeFormSixteenDataReq data, long CreatedBy, DateTime CreatedOn, bool IsDelete);
        //20 - 05 - 2022 


        bool UpdateFormSixteenDetail(PrintYearlySalarySheet obj, EmployeeFormSixteenDataReq data, long UpdatedBy, DateTime UpdatedOn);

        FormSixteenDetails CheckIsExistsFormSixteenDetails(DateTime FromDate, DateTime ToDate, long EmployeeCode);
        // 31 Jan 2022 Piyush Limbani

        // 14 Feb 2022 Piyush Limbani


       
        //Form16TexableIncome

        //long AddFormSixteenDetailMaster(FormSixteenDetail_Mst obj);

        long AddFormSixteenDetailMaster(AddFormSixteenDetail data);

        List<AddFormSixteenTexableIncome> GetAllTexableIncomeListByForm16TexableIncomeID(long FormSixteenDetailID);

        AddFormSixteenTexableIncome GetTexableIncome(decimal TotalTaxableIncome);
        
        //Form16TexableIncome
       

        List<FormSixteenDetailListResponse> GetAllFormSixteenDetailList();

        bool DeleteFormSixteenDetail(long FormSixteenDetailID, bool IsDelete);

        // 16 Feb 2022 Piyush Limbani
        decimal GetTaxSlabPer(decimal TotalTaxableIncome);

        decimal GetSurchargeSlabPer(decimal TotalTaxableIncome);

        decimal GetEducationSlabPer(decimal TotalTaxableIncome);

        List<FormSixteenValueModel> GetFormSixteenByDate(DateTime FromDate, DateTime ToDate, long EmployeeCode);




    }
}
