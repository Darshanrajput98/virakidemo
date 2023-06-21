using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;

namespace vb.Service
{
    public class AttandanceServices : IAttandanceService
    {
        public bool AddMachineRawPunch(MachineRawPunch Obj)
        {
            SqlCommand cmdAdd = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdAdd.CommandType = CommandType.StoredProcedure;
                cmdAdd.CommandText = "MachineRawPunch_Insert";
                cmdAdd.Parameters.AddWithValue("@ADate", Obj.ADate);
                cmdAdd.Parameters.AddWithValue("@EmployeeID", Obj.EmployeeID);
                cmdAdd.Parameters.AddWithValue("@Name", Obj.Name);
                cmdAdd.Parameters.AddWithValue("@Shift", Obj.Shift);
                cmdAdd.Parameters.AddWithValue("@TimeIn", Obj.TimeIn);
                cmdAdd.Parameters.AddWithValue("@TimeOut", Obj.TimeOut);
                cmdAdd.Parameters.AddWithValue("@WorkDuration", Obj.WorkDuration);
                cmdAdd.Parameters.AddWithValue("@OT", Obj.OT);
                cmdAdd.Parameters.AddWithValue("@TotalDuration", Obj.TotalDuration);
                cmdAdd.Parameters.AddWithValue("@Status", Obj.Status);
                cmdAdd.Parameters.AddWithValue("@Remarks", Obj.Remarks);
                objBaseSqlManager.ExecuteNonQuery(cmdAdd);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public Int32 InsertDailyAttendance(DateTime ADate, string EmployeeID, string AEmployeeName)
        {
            SqlCommand cmdAdd = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdAdd.CommandType = CommandType.StoredProcedure;
                cmdAdd.CommandText = "DailyAttendance_Insert";
                cmdAdd.Parameters.AddWithValue("@ADate", ADate);
                cmdAdd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                cmdAdd.Parameters.AddWithValue("@TimeIn", null);
                cmdAdd.Parameters.AddWithValue("@TimeOut", null);
                cmdAdd.Parameters.AddWithValue("@HoursWorked", "0.00");
                cmdAdd.Parameters.AddWithValue("@EShiftID", 1);
                cmdAdd.Parameters.AddWithValue("@NonWorking", 1);
                cmdAdd.Parameters.AddWithValue("@NonWorkingType", "OffDay");
                cmdAdd.Parameters.AddWithValue("@AttendanceType", "OffDay");
                cmdAdd.Parameters.AddWithValue("@WeeklyOff", "1.00");
                cmdAdd.Parameters.AddWithValue("@Holidays", "0.00");
                cmdAdd.Parameters.AddWithValue("@AMonth", ADate.Month);
                cmdAdd.Parameters.AddWithValue("@AYear", ADate.Year);
                cmdAdd.Parameters.AddWithValue("@AEmployeeName", AEmployeeName);
                cmdAdd.Parameters.AddWithValue("@Present", "0.00");
                cmdAdd.Parameters.AddWithValue("@Absent", "0.00");
                cmdAdd.Parameters.AddWithValue("@ActualTimeIn", "10:00:29 AM");
                cmdAdd.Parameters.AddWithValue("@ActualTimeOut", "7:30:29 PM");
                cmdAdd.Parameters.AddWithValue("@AOtHrs", "0.00");
                cmdAdd.Parameters.AddWithValue("@AOtApprovedHrs", "0.00");
                cmdAdd.Parameters.AddWithValue("@AOtApproved", null);
                cmdAdd.Parameters.AddWithValue("@AOtSupervisior", null);
                cmdAdd.Parameters.AddWithValue("@ActualHrs", "9.00");
                cmdAdd.Parameters.AddWithValue("@BreakDuration", "0.00");
                cmdAdd.Parameters.AddWithValue("@PayCaderName", "Staff");
                cmdAdd.Parameters.AddWithValue("@HrsWorked", "0.00");
                cmdAdd.Parameters.AddWithValue("@AttendanceMonth", DateTime.Now.Month.ToString());
                cmdAdd.Parameters.AddWithValue("@Authorised", 0);
                cmdAdd.Parameters.AddWithValue("@HalfdayStatus", 0);
                cmdAdd.Parameters.AddWithValue("@LeaveCode", null);
                cmdAdd.Parameters.AddWithValue("@LeaveAppNo", null);
                cmdAdd.Parameters.AddWithValue("@Leave", "0.00");
                cmdAdd.Parameters.AddWithValue("@TempWork", null);
                cmdAdd.Parameters.AddWithValue("@TempWorkType", null);
                cmdAdd.Parameters.AddWithValue("@TempWorkName", null);
                cmdAdd.Parameters.AddWithValue("@BranchID", 1);
                cmdAdd.Parameters.AddWithValue("@DepartmentID", 1);
                cmdAdd.Parameters.AddWithValue("@LogTime", null);
                cmdAdd.Parameters.AddWithValue("@ShiftStartTime", null);
                cmdAdd.Parameters.AddWithValue("@ShiftEndTime", null);
                cmdAdd.Parameters.AddWithValue("@EarlyComingTime", null);
                cmdAdd.Parameters.AddWithValue("@LateComingTime", null);
                cmdAdd.Parameters.AddWithValue("@EarlyGoingTime", null);
                cmdAdd.Parameters.AddWithValue("@LateGoingTime", null);
                cmdAdd.Parameters.AddWithValue("@StartRecessTime", "1:00:29 PM");
                cmdAdd.Parameters.AddWithValue("@EndRecessTime", "2:00:29 PM");
                cmdAdd.Parameters.AddWithValue("@EmpRecessStartTime", null);
                cmdAdd.Parameters.AddWithValue("@EmpRecessEndTime", null);
                cmdAdd.Parameters.AddWithValue("@Status", "WO");
                cmdAdd.Parameters.AddWithValue("@ShiftName", "General");
                cmdAdd.Parameters.AddWithValue("@NotTotalEmpWorkedHrs", "0.00");
                cmdAdd.Parameters.AddWithValue("@ShiftNextDay", 0);
                cmdAdd.Parameters.AddWithValue("@LastLogDateTime", "2018-09-04 01:22:10.760");
                cmdAdd.Parameters.AddWithValue("@PunchCount", 0);
                cmdAdd.Parameters.AddWithValue("@TotHrs", "0.00");
                cmdAdd.Parameters.AddWithValue("@BreakTimeHrs", "0.00");
                cmdAdd.Parameters.AddWithValue("@HDGraceDay", 0);
                cmdAdd.Parameters.AddWithValue("@InComment", null);
                cmdAdd.Parameters.AddWithValue("@OutComment", null);
                cmdAdd.Parameters.AddWithValue("@IsEmailSent", 0);
                cmdAdd.Parameters.AddWithValue("@ActualTimeInDate", null);
                cmdAdd.Parameters.AddWithValue("@ActualTimeOutDate", null);
                cmdAdd.Parameters.AddWithValue("@IsNightShift", 0);
                objBaseSqlManager.ExecuteNonQuery(cmdAdd);
                objBaseSqlManager.ForceCloseConnection();
                return 1;
            }
        }

        // Attandance System
        public bool AddAllowance(Allowance_Master Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.AllowanceDetailID == 0)
                {
                    context.Allowance_Master.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.AllowanceDetailID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //public bool AddAllowanceDetail(AddAllowance Obj)
        //{
        //    using (VirakiEntities context = new VirakiEntities())
        //    {
        //        if (Obj.BasicAllowance != 0)
        //        {
        //            AllowanceDetail_Master Allowance = new AllowanceDetail_Master();
        //            Allowance.EmployeeCode = Obj.EmployeeCode;
        //            Allowance.BasicAllowance = Obj.BasicAllowance;
        //            Allowance.BasicAllowancedate = Obj.BasicAllowancedate;
        //            Allowance.HouseAllowance = Obj.HouseRentAllowance;
        //            Allowance.HouseAllowancedate = Obj.HouseRentAllowancedate;
        //            Allowance.VehicleAllowance = Obj.VehicleAllowance;
        //            Allowance.VehicleAllowancedate = Obj.VehicleAllowancedate;
        //            Allowance.PerformanceAllowance = Obj.PerformanceAllowance;
        //            Allowance.PerformanceAllowancedate = Obj.PerformanceAllowancedate;
        //            Allowance.Conveyance = Obj.Conveyance;
        //            Allowance.Conveyancedate = Obj.Conveyancedate;
        //            Allowance.CreatedBy = Obj.CreatedBy;
        //            Allowance.CreatedOn = DateTime.Now;
        //            Allowance.UpdatedBy = Obj.CreatedBy;
        //            Allowance.UpdatedOn = DateTime.Now;
        //            Allowance.IsActive = false;
        //            context.AllowanceDetail_Master.Add(Allowance);
        //            context.SaveChanges();
        //        }
        //        if (Obj.AllowanceDetailID > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        public List<AllowanceListResponse> GetAllowanceList(string StartDate, string EndDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllowanceList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AllowanceListResponse> objlst = new List<AllowanceListResponse>();
                while (dr.Read())
                {
                    AllowanceListResponse obj = new AllowanceListResponse();
                    obj.AllowanceDetailID = objBaseSqlManager.GetInt64(dr, "AllowanceDetailID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");

                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    if (obj.BirthDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.BirthDatestr = obj.BirthDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.BirthDatestr = "";
                    }
                    if (obj.BirthDate < DateTime.Now)
                    {
                        obj.Age = GetAge(obj.BirthDate);
                    }
                    else
                    {
                        obj.Age = null;
                    }

                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.OpeningDate = objBaseSqlManager.GetDateTime(dr, "OpeningDate");
                    if (obj.OpeningDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OpeningDatestr = obj.OpeningDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.OpeningDatestr = "";
                    }
                    obj.IncrementDate = objBaseSqlManager.GetDateTime(dr, "IncrementDate");
                    if (obj.IncrementDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.IncrementDatestr = obj.IncrementDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.IncrementDatestr = "";
                    }
                    obj.DA1Date = objBaseSqlManager.GetDateTime(dr, "DA1Date");
                    if (obj.DA1Date != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DA1Datestr = obj.DA1Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DA1Datestr = "";
                    }
                    obj.DA2Date = objBaseSqlManager.GetDateTime(dr, "DA2Date");
                    if (obj.DA2Date != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DA2Datestr = obj.DA2Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DA2Datestr = "";
                    }
                    obj.OthersDate = objBaseSqlManager.GetDateTime(dr, "OthersDate");
                    if (obj.OthersDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OthersDatestr = obj.OthersDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.OthersDatestr = "";
                    }
                    obj.BasicAllowance1 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance1");
                    obj.BasicAllowance2 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance2");
                    obj.BasicAllowance3 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance3");
                    obj.BasicAllowance4 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance4");
                    obj.BasicAllowance5 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance5");
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.HRAPercentage1 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage1");
                    obj.HRAPercentage2 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage2");
                    obj.HRAPercentage3 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage3");
                    obj.HRAPercentage4 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage4");
                    obj.HRAPercentage5 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage5");
                    obj.HouseRentAllowance1 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance1");
                    obj.HouseRentAllowance2 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance2");
                    obj.HouseRentAllowance3 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance3");
                    obj.HouseRentAllowance4 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance4");
                    obj.HouseRentAllowance5 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance5");
                    obj.TotalHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "TotalHouseRentAllowance");
                    obj.TotalWages1 = objBaseSqlManager.GetDecimal(dr, "TotalWages1");
                    obj.TotalWages2 = objBaseSqlManager.GetDecimal(dr, "TotalWages2");
                    obj.TotalWages3 = objBaseSqlManager.GetDecimal(dr, "TotalWages3");
                    obj.TotalWages4 = objBaseSqlManager.GetDecimal(dr, "TotalWages4");
                    obj.TotalWages5 = objBaseSqlManager.GetDecimal(dr, "TotalWages5");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.ConveyancePerDay = objBaseSqlManager.GetDecimal(dr, "ConveyancePerDay");
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.PerformanceAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "PerformanceAllowanceStatusID");
                    obj.CityAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "CityAllowanceStatusID");
                    obj.PFStatusID = objBaseSqlManager.GetInt64(dr, "PFStatusID");
                    obj.ESICStatusID = objBaseSqlManager.GetInt64(dr, "ESICStatusID");
                    obj.BonusPercentage = objBaseSqlManager.GetDecimal(dr, "BonusPercentage");
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.BonusStatusID = objBaseSqlManager.GetInt64(dr, "BonusStatusID");
                    obj.LeaveEnhancementPercentage = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementPercentage");
                    obj.LeaveEnhancementAmount = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementAmount");
                    obj.LeaveEnhancementStatusID = objBaseSqlManager.GetInt64(dr, "LeaveEnhancementStatusID");
                    obj.GratuityPercentage = objBaseSqlManager.GetDecimal(dr, "GratuityPercentage");
                    obj.GratuityAmount = objBaseSqlManager.GetDecimal(dr, "GratuityAmount");
                    obj.GratuityStatusID = objBaseSqlManager.GetInt64(dr, "GratuityStatusID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteAllowanceDetail(long AllowanceDetailID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteAllowanceDetail";
                cmdGet.Parameters.AddWithValue("@AllowanceDetailID", AllowanceDetailID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public GetEmployeeAttandanceDetail GetEmployeeAttandanceDetail(int MonthID, int YearID, long EmployeeCode, string MonthStartDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeAttandanceDetail";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);               
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetEmployeeAttandanceDetail obj = new GetEmployeeAttandanceDetail();
                while (dr.Read())
                {
                    int TotalDeductedSunday = 0;
                    int TotalDeductedHoliday = 0;
                    //GetAllowanceDetail AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(StartDate, EndDate, EmployeeCode);
                    GetAllowanceDetail AllowanceDetail = null;
                    long YearForAlloawance = 0;
                    if (MonthID == 1 || MonthID == 2 || MonthID == 3)
                    {
                        YearForAlloawance = YearID - 1;
                        AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, EmployeeCode);
                    }
                    else
                    {
                        YearForAlloawance = YearID;
                        AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, EmployeeCode);
                    }
                    SalaryExistModel existsalary = CheckSalaryExist(MonthID, YearID, EmployeeCode);
                    if (MonthID == 4)
                    {
                        obj.OpeningLeaves = 0;
                    }
                    else
                    {
                        decimal ClosingLeaves = 0;
                        if (existsalary.SalarySheetID == 0)
                        {
                            // 25 July 2020 Piyush Limbani For May 2020 Change in Function Get AdditionalClosingLeaves
                            ClosingLeaves = GetLastOpeningLeavesByEmployeeCode(EmployeeCode);
                            obj.OpeningLeaves = ClosingLeaves;
                        }
                        else
                        {
                            ClosingLeaves = GetLastOpeningLeavesBySalarySheetID(existsalary.SalarySheetID);
                            obj.OpeningLeaves = ClosingLeaves;
                        }
                        //decimal ClosingLeaves = GetLastOpeningLeavesByEmployeeCode(EmployeeCode);
                        //obj.OpeningLeaves = ClosingLeaves;
                    }
                    decimal EarnedLeaves = GetEarnedLeavesByMonth(MonthID);

                    if (objBaseSqlManager.GetInt32(dr, "Present") == 0)
                    {
                        obj.EarnedLeaves = 0;
                    }
                    else if (objBaseSqlManager.GetInt32(dr, "Present") <= 8)
                    {
                        obj.EarnedLeaves = 1;
                    }
                    else if (objBaseSqlManager.GetInt32(dr, "Present") <= 16)
                    {
                        obj.EarnedLeaves = 2;
                    }
                    else
                    {
                        obj.EarnedLeaves = EarnedLeaves;
                    }

                    obj.TotalMonthDay = objBaseSqlManager.GetInt32(dr, "TotalMonthDay");
                    obj.TotalMonthSundayAct = objBaseSqlManager.GetInt32(dr, "TotalSunday");
                    List<GetSundayDate> datelist = GetSundayDateList(MonthStartDate);
                    foreach (var item in datelist)
                    {
                        int month = item.Date.Month;
                        int Day = item.Date.Day;
                        int Year = item.Date.Year;
                        if (Day > 1)
                        {
                            string Sdate = Convert.ToString(item.Date.AddDays(-1));
                            string Edate = Convert.ToString(item.Date.AddDays(1));
                            int absentcount = GetAbsentCount(Sdate, Edate, EmployeeCode);
                            if (absentcount == 2)
                            {
                                TotalDeductedSunday = TotalDeductedSunday + 1;
                            }
                        }
                    }
                    //obj.TotalMonthSunday = obj.TotalMonthSundayAct - TotalDeductedSunday;

                    // 7 July 2020 Piyush Limbani For May 2020
                    if (MonthID == 5 && YearID == 2020)
                    {
                        obj.TotalMonthSunday = 1;
                    }
                    else
                    {
                        if (objBaseSqlManager.GetInt32(dr, "Present") == 0)
                        {
                            obj.TotalMonthSunday = 0;
                        }
                        else
                        {
                            obj.TotalMonthSunday = obj.TotalMonthSundayAct - TotalDeductedSunday;
                        }
                    }
                    // 7 July 2020 Piyush Limbani For May 2020

                    List<GetHolidayDate> holidaylist = GetHolidayDateList(MonthID, YearID);
                    // 28/04/2020 (Holiday deduction)
                    List<GetAbsentHolidayDate> absentholidaylist = GetAbsentHolidayDate(MonthID, YearID, EmployeeCode);
                    int count = 0;
                    if (absentholidaylist.Count > 0)
                    {
                        foreach (var item in absentholidaylist)
                        {
                            if (item.Status == "A")
                            {
                                var dlist = absentholidaylist.Where(a => a.Date >= item.Date).ToList();
                                int cntday = 1;
                                DateTime? getdate = null;
                                int currcount = 0;
                                foreach (var item1 in dlist)
                                {
                                    if (getdate == null)
                                    {
                                        getdate = item1.Date.AddDays(1);
                                    }
                                    else
                                    {
                                        getdate = Convert.ToDateTime(getdate).AddDays(cntday);
                                    }
                                    var GetH = absentholidaylist.Where(a => a.Date == getdate).FirstOrDefault();
                                    if (GetH != null)
                                    {
                                        if (GetH.Status == "H")
                                        {
                                            currcount++;
                                        }
                                        else
                                        {
                                            count = count + currcount;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // 28/04/2020 (Holiday deduction)
                    // var getcnt = count;
                    TotalDeductedHoliday = count;
                    if (objBaseSqlManager.GetInt32(dr, "Present") == 0)
                    {
                        obj.Holiday = 0;
                    }
                    else
                    {
                        obj.Holiday = holidaylist.Count - TotalDeductedHoliday;
                    }

                    obj.PresentAct = objBaseSqlManager.GetInt32(dr, "Present");


                    int NotPresent = 0;

                    //if (obj.EarnedLeaves == 1)
                    //{
                    //    int total = obj.TotalMonthSunday + obj.Holiday + obj.PresentAct;
                    //    if (obj.TotalMonthDay == (obj.TotalMonthSunday + obj.Holiday + obj.PresentAct))
                    //    {
                    //        NotPresent = 0;
                    //    }
                    //    else
                    //    {
                    //        NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct - 2;
                    //    }
                    //}
                    //else if (obj.EarnedLeaves == 2)
                    //{
                    //    int total = obj.TotalMonthSunday + obj.Holiday + obj.PresentAct;
                    //    if (obj.TotalMonthDay == (obj.TotalMonthSunday + obj.Holiday + obj.PresentAct))
                    //    {
                    //        NotPresent = 0;
                    //    }
                    //    else
                    //    {
                    //        NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct - 1;
                    //    }
                    //}
                    //else
                    //{
                    //    NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct;
                    //}



                    // 05/05/2021 comented date
                    if (obj.EarnedLeaves == 1)
                    {
                        //NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct - 2;
                        NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct;
                    }
                    else if (obj.EarnedLeaves == 2)
                    {
                        NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct;
                        //NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct - 1;
                    }
                    else
                    {
                        NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct;
                    }
                    // 05/05/2021 comented date
                    //int NotPresent = obj.TotalMonthDay - obj.TotalMonthSunday - obj.Holiday - obj.PresentAct;


                    obj.TotalLeaves = obj.OpeningLeaves + obj.EarnedLeaves;
                    if (obj.TotalLeaves >= NotPresent)
                    {
                        // 03 Nov.2020 Piyush Limbani
                        //obj.ClosingLeaves = (obj.OpeningLeaves + obj.EarnedLeaves) - NotPresent;
                        obj.ClosingLeaves = (obj.OpeningLeaves + obj.EarnedLeaves + TotalDeductedSunday) - NotPresent;
                        // 03 Nov.2020 Piyush Limbani

                        // 03 Nov.2020 Piyush Limbani
                        //obj.Absent = 0;
                        obj.Absent = 0 + TotalDeductedSunday;
                        // 03 Nov.2020 Piyush Limbani


                        // 03 Nov.2020 Piyush Limbani
                        //obj.AvailedLeaves = NotPresent;
                        obj.AvailedLeaves = NotPresent - TotalDeductedSunday;
                        // 03 Nov.2020 Piyush Limbani


                        // 03 Nov.2020 Piyush Limbani
                        //obj.Present = obj.PresentAct + NotPresent;
                        obj.Present = obj.PresentAct + NotPresent - TotalDeductedSunday;
                        // 03 Nov.2020 Piyush Limbani

                        //obj.Absent = 8;
                        //obj.Present = 18;
                    }
                    else if (obj.TotalLeaves < NotPresent)
                    {
                        obj.ClosingLeaves = 0;
                        obj.Absent = NotPresent - Convert.ToInt32(obj.TotalLeaves);
                        obj.AvailedLeaves = obj.TotalLeaves;
                        obj.Present = (obj.PresentAct + NotPresent) - obj.Absent;
                    }
                    obj.TotalDays = obj.Present + obj.TotalMonthSunday + obj.Holiday;
                    obj.BasicAllowance = AllowanceDetail.BasicAllowance;
                    obj.HouseRentAllowance = AllowanceDetail.HouseRentAllowance;
                    obj.TotalBasic = AllowanceDetail.TotalBasic;

                    // 8 July 2020 Piyush Limbani
                    if (obj.TotalDays == 0)
                    {
                        obj.EarnedBasicWages = 0;
                    }
                    else
                    {
                        decimal EarnedBasicWagesAct = Math.Round(((obj.BasicAllowance / obj.TotalMonthDay) * obj.TotalDays), 2);
                        string EarnedBasicWages = GetRoundoffValue(EarnedBasicWagesAct);
                        obj.EarnedBasicWages = Convert.ToDecimal(EarnedBasicWages);
                    }


                    if (obj.TotalDays == 0)
                    {
                        obj.EarnedHouseRentAllowance = 0;
                    }
                    else
                    {
                        decimal EarnedHouseRentAllowanceAct = Math.Round(((obj.HouseRentAllowance / obj.TotalMonthDay) * obj.TotalDays), 2);
                        string EarnedHouseRentAllowance = GetRoundoffValue(EarnedHouseRentAllowanceAct);
                        obj.EarnedHouseRentAllowance = Convert.ToDecimal(EarnedHouseRentAllowance);
                    }
                    // 8 July 2020 Piyush Limbani

                    //obj.EarnedBasicWages = Math.Round(((obj.BasicAllowance / obj.TotalMonthDay) * obj.TotalDays), 2);
                    //obj.EarnedHouseRentAllowance = Math.Round(((obj.HouseRentAllowance / obj.TotalMonthDay) * obj.TotalDays), 2);

                    obj.TotalEarnedWages = Math.Round((obj.EarnedBasicWages + obj.EarnedHouseRentAllowance), 2);

                    obj.ConveyancePerMonth = AllowanceDetail.Conveyance;
                    obj.ConveyancePerDay = AllowanceDetail.ConveyancePerDay;

                    if (obj.TotalDays == 0)
                    {
                        obj.Conveyance = 0;
                    }
                    else
                    {
                        decimal TotalConveyancePerDay = (obj.ConveyancePerDay) * (Convert.ToDecimal(obj.Present) - obj.AvailedLeaves);
                        decimal TotalConveyancePerMonth = Math.Round(((obj.ConveyancePerMonth / obj.TotalMonthDay) * obj.TotalDays));
                        decimal Conveyance = TotalConveyancePerMonth + TotalConveyancePerDay;
                        obj.Conveyance = Conveyance;
                    }



                    if (existsalary.SalarySheetID == 0)
                    {
                        obj.AdditionalCityAllowance = 0;
                        obj.TotalCityAllowance = 0;
                        obj.AdditionalVehicleAllowance = 0;
                        obj.TotalVehicleAllowance = 0;
                        obj.AdditionalConveyance = 0;
                        obj.TotalConveyance = 0;
                        obj.AdditionalPerformanceAllowance = 0;
                        obj.TotalPerformanceAllowance = 0;
                    }
                    else
                    {
                        AdditionalAmountDetails AdditionalAmount = GetAdditionalAmountBySalarySheetID(existsalary.SalarySheetID);
                        // 24 July 2020 Piyush Limbani
                        obj.AdditionalPresent = AdditionalAmount.AdditionalPresent;
                        obj.TotalPresent = AdditionalAmount.TotalPresent;
                        obj.AdditionalSunday = AdditionalAmount.AdditionalSunday;
                        obj.TotalSunday = AdditionalAmount.TotalSunday;
                        obj.AdditionalHoliday = AdditionalAmount.AdditionalHoliday;
                        obj.TotalHoliday = AdditionalAmount.TotalHoliday;
                        obj.AdditionalAbsent = AdditionalAmount.AdditionalAbsent;
                        obj.TotalAbsent = AdditionalAmount.TotalAbsent;
                        // 24 July 2020 Piyush Limbani

                        obj.AdditionalCityAllowance = AdditionalAmount.AdditionalCityAllowance;
                        obj.TotalCityAllowance = AdditionalAmount.TotalCityAllowance;
                        obj.AdditionalVehicleAllowance = AdditionalAmount.AdditionalVehicleAllowance;
                        obj.TotalVehicleAllowance = AdditionalAmount.TotalVehicleAllowance;
                        obj.AdditionalConveyance = AdditionalAmount.AdditionalConveyance;
                        obj.TotalConveyance = AdditionalAmount.TotalConveyance;
                        obj.AdditionalPerformanceAllowance = AdditionalAmount.AdditionalPerformanceAllowance;
                        obj.TotalPerformanceAllowance = AdditionalAmount.TotalPerformanceAllowance;

                        // 24 July 2020 Piyush Limbani
                        obj.AdditionalAvailedLeaves = AdditionalAmount.AdditionalAvailedLeaves;
                        obj.TotalAvailedLeaves = AdditionalAmount.TotalAvailedLeaves;
                        obj.AdditionalClosingLeaves = AdditionalAmount.AdditionalClosingLeaves;
                        obj.TotalClosingLeaves = AdditionalAmount.TotalClosingLeaves;
                        // 24 July 2020 Piyush Limbani
                    }

                    obj.PerformanceAllowanceStatusID = AllowanceDetail.PerformanceAllowanceStatusID;
                    obj.CityAllowanceStatusID = AllowanceDetail.CityAllowanceStatusID;
                    obj.PFStatusID = AllowanceDetail.PFStatusID;
                    obj.ESICStatusID = AllowanceDetail.ESICStatusID;
                    GetOTDetails Minutes = GetTotalOTMinutes(MonthID, YearID, EmployeeCode);
                    obj.CityAllowanceMinutes = Minutes.TotalMinutes;
                    obj.CityAllowanceHours = Minutes.TotalHrs;

                    if (obj.CityAllowanceMinutes > 0)
                    {
                        if (obj.CityAllowanceStatusID == 1)  //1 =  applicable old
                        {
                            decimal CityAllowancePerHr = Math.Round(((((obj.TotalBasic / obj.TotalMonthDay) / 8) / 60) * 2), 2);
                            decimal CityAllowance = CityAllowancePerHr * obj.CityAllowanceMinutes;

                            // 9 July 2020 Piyush Limbani
                            decimal CityAllowanceAct = Math.Round(CityAllowance, 2);
                            string CityAllowanceNew = GetRoundoffValue(CityAllowanceAct);
                            obj.CityAllowance = Convert.ToDecimal(CityAllowanceNew);
                            // 9 July 2020 Piyush Limbani

                            //obj.CityAllowance = Math.Round(CityAllowance, 2);
                        }
                        else if (obj.CityAllowanceStatusID == 2)  //2 = applicable new
                        {
                            decimal CityAllowancePerHr = Math.Round(((((obj.BasicAllowance / obj.TotalMonthDay) / 8) / 60) * 2), 2);
                            // decimal CityAllowancePerHr = Math.Round((((obj.TotalBasic / obj.TotalMonthDay) / 8) / 60), 2);
                            decimal CityAllowance = CityAllowancePerHr * obj.CityAllowanceMinutes;

                            // 9 July 2020 Piyush Limbani
                            decimal CityAllowanceAct = Math.Round(CityAllowance, 2);
                            string CityAllowanceNew = GetRoundoffValue(CityAllowanceAct);
                            obj.CityAllowance = Convert.ToDecimal(CityAllowanceNew);
                            // 9 July 2020 Piyush Limbani


                            //obj.CityAllowance = Math.Round(CityAllowance, 2);
                        }
                        else //3 = NA
                        {
                            obj.CityAllowance = 0;
                        }
                    }
                    else
                    {
                        obj.CityAllowance = 0;
                    }

                    if (obj.PerformanceAllowanceStatusID == 1) //1 =  applicable old
                    {
                        obj.PerformanceAllowanceAct = AllowanceDetail.PerformanceAllowance;
                        obj.PerformanceAllowance = Math.Round(obj.PerformanceAllowanceAct);
                    }
                    else if (obj.PerformanceAllowanceStatusID == 2)  //2 = applicable new
                    {
                        if (obj.TotalDays == 0)
                        {
                            obj.PerformanceAllowance = 0;
                            obj.PerformanceAllowanceAct = AllowanceDetail.PerformanceAllowance;
                        }
                        else
                        {
                            obj.PerformanceAllowanceAct = AllowanceDetail.PerformanceAllowance;
                            decimal PerformanceAllowance = ((obj.PerformanceAllowanceAct) / (obj.TotalMonthDay)) * ((obj.TotalDays) - (obj.AvailedLeaves));
                            obj.PerformanceAllowance = Math.Round(PerformanceAllowance);
                        }
                    }
                    else
                    {
                        obj.PerformanceAllowance = 0;
                    }
                    long VehicleNoOfDaysCount = GetVehicleNoOfDaysCount(MonthID, YearID, EmployeeCode);
                    obj.VehicleAllowance = AllowanceDetail.VehicleAllowance * VehicleNoOfDaysCount;
                    //obj.VehicleAllowance = 200;

                    decimal GrossWagesPayable = obj.TotalEarnedWages + obj.CityAllowance + obj.AdditionalCityAllowance + obj.VehicleAllowance + obj.AdditionalVehicleAllowance + obj.Conveyance + obj.AdditionalConveyance + obj.PerformanceAllowance + obj.AdditionalPerformanceAllowance;
                    // decimal GrossWagesPayable = Convert.ToDecimal(95766.90);
                    obj.GrossWagesPayable = Math.Round(GrossWagesPayable, 2);

                    //30-04-2020
                    GetLastPFInformation PFInfo = GetLastPFInformation();
                    obj.HighestSlab = PFInfo.HighestSlab;
                    obj.HighestPF = PFInfo.HighestPF;
                    obj.PFPercentage = PFInfo.PFPercentage;
                    if (obj.PFStatusID == 1) //1 =  applicable old
                    {
                        if (obj.TotalEarnedWages > obj.HighestSlab) // 15000
                        {
                            obj.PF = obj.HighestPF; // 1800
                        }
                        else
                        {
                            //obj.PF = Math.Round((obj.TotalEarnedWages * obj.PFPercentage) / 100); // 12%
                            decimal PF = Math.Round(((obj.TotalEarnedWages * obj.PFPercentage) / 100), 2);// 12%
                            obj.PF = Math.Round(PF, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    else if (obj.PFStatusID == 2)  //2 = applicable new
                    {
                        if (obj.EarnedBasicWages > obj.HighestSlab)
                        {
                            obj.PF = obj.HighestPF;
                        }
                        else
                        {
                            // obj.PF = Math.Round((obj.EarnedBasicWages * obj.PFPercentage) / 100);
                            decimal PF = Math.Round(((obj.EarnedBasicWages * obj.PFPercentage) / 100), 2);
                            obj.PF = Math.Round(PF, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    else
                    {
                        obj.PF = 0;
                    }
                    //30-04-2020
                    GetLastESICInformation ESICInfo = GetLastESICInformation();
                    obj.EmployeeSlab = ESICInfo.EmployeeSlab;
                    if (obj.ESICStatusID == 1)  //1 =  applicable old
                    {
                        obj.ESIC = 0;
                    }
                    else if (obj.ESICStatusID == 2)//2 = applicable new
                    {
                        obj.ESIC = Math.Ceiling(((obj.GrossWagesPayable) * Convert.ToDecimal(obj.EmployeeSlab))); // 0.0075
                    }
                    else
                    {
                        obj.ESIC = 0;
                    }
                    //30-04-2020
                    GetLastPTInformation PTInfo = GetLastPTInformationByMonth(MonthID);
                    obj.PTHighestSlab = PTInfo.PTHighestSlab;
                    obj.PTHighestAmount = PTInfo.PTHighestAmount;
                    obj.PTLowestSlab = PTInfo.PTLowestSlab;
                    obj.PTLowestAmount = PTInfo.PTLowestAmount;
                    if (obj.GrossWagesPayable > obj.PTHighestSlab) // HighestSlab = 10000
                    {
                        obj.PT = obj.PTHighestAmount;
                    }
                    else if (obj.GrossWagesPayable > obj.PTLowestSlab) // LowestSlab = 5000
                    {
                        obj.PT = obj.PTLowestAmount;
                    }
                    else
                    {
                        obj.PT = 0;
                    }
                    //30-04-2020
                    GetLastMLWFInformation MLWFInfo = GetLastMLWFInformationByMonth(MonthID);
                    obj.MLWFHighestSlab = MLWFInfo.MLWFHighestSlab;
                    obj.MLWFHighestAmount = MLWFInfo.MLWFHighestAmount;
                    obj.MLWFMonthID = MLWFInfo.MLWFMonthID;
                    if (MonthID == obj.MLWFMonthID)
                    {
                        if (obj.GrossWagesPayable > obj.MLWFHighestSlab) // HighestSlab = 5000
                        {
                            obj.MLWF = obj.MLWFHighestAmount;
                        }
                        else
                        {
                            obj.MLWF = 0;
                        }
                    }
                    else
                    {
                        obj.MLWF = 0;
                    }
                    decimal TotalDeductions = obj.PF + obj.PT + obj.ESIC + obj.MLWF;
                    obj.TotalDeductions = Math.Round(TotalDeductions, 2);

                    // 10 July 2020 Piyush Limbani
                    decimal NetWagesPaid = Math.Round((obj.GrossWagesPayable - obj.TotalDeductions), 2);
                    obj.NetWagesPaid = Math.Round(NetWagesPaid, 0, MidpointRounding.AwayFromZero);
                    // 10 July 2020 Piyush Limbani
                    //obj.NetWagesPaid = Math.Round((obj.GrossWagesPayable - obj.TotalDeductions));

                    // 04/05/2020 GetLastAdvanceAndOtherDeductions 
                    //SalaryExistModel exist = CheckSalaryExist(MonthID, YearID, EmployeeCode);
                    if (existsalary.SalarySheetID == 0)
                    {
                        AdvanceAndOtherDeductions AdvanceAndOtherDeductions = GetLastAdvanceAndOtherDeductionsByEmployeeCode(EmployeeCode);
                        obj.OpeningAdvance = AdvanceAndOtherDeductions.ClosingAdvance;
                        obj.ClosingAdvance = AdvanceAndOtherDeductions.ClosingAdvance;
                        obj.TDS = AdvanceAndOtherDeductions.TDS;

                        // 10 July 2020 Piyush Limbani
                        if (AllowanceDetail.CustomerID != 0)
                        {
                            decimal OutStandingAmount = GetOutStandingPaymentAmountByCustomerIDForEmployee(AllowanceDetail.CustomerID);
                            obj.Goods = OutStandingAmount;
                        }
                        // 10 July 2020 Piyush Limbani
                    }
                    else
                    {
                        AdvanceAndOtherDeductions AdvanceAndOtherDeductions = GetAdvanceAndOtherDeductionsDetailBySalarySheetID(existsalary.SalarySheetID);
                        obj.OpeningAdvance = AdvanceAndOtherDeductions.OpeningAdvance;
                        obj.Addition = AdvanceAndOtherDeductions.Addition;
                        obj.Deductions = AdvanceAndOtherDeductions.Deductions;
                        obj.ClosingAdvance = AdvanceAndOtherDeductions.ClosingAdvance;
                        obj.TDS = AdvanceAndOtherDeductions.TDS;
                        obj.Goods = AdvanceAndOtherDeductions.Goods;
                        obj.AnyOtherDeductions1 = AdvanceAndOtherDeductions.AnyOtherDeductions1;
                        obj.AnyOtherDeductions2 = AdvanceAndOtherDeductions.AnyOtherDeductions2;
                    }

                    // 14 July 2020 Piyush Limbani
                    obj.CustomerID = AllowanceDetail.CustomerID;
                    // 14 July 2020 Piyush Limbani

                    obj.AMonth = objBaseSqlManager.GetInt32(dr, "AMonth");
                    obj.AYear = objBaseSqlManager.GetInt32(dr, "AYear");
                    obj.SalarySheetID = existsalary.SalarySheetID;
                    obj.CreatedBy = existsalary.CreatedBy;
                    obj.CreatedOn = existsalary.CreatedOn;
                    obj.CreatedOnstr = string.Format("{0:G}", obj.CreatedOn);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        private string GetRoundoffValue(decimal ActAmount)
        {
            string[] ActAmountstr = ActAmount.ToString().Split('.');
            string ActAmountDecimalPlaces = (ActAmountstr[1]);
            string ActAmountDecimalPlacesStr = "0." + (ActAmountDecimalPlaces.ToString());
            decimal ActAmountDecimalPlaces1 = Convert.ToDecimal(ActAmountDecimalPlacesStr);
            double roundedTimes20 = Convert.ToDouble(Math.Round(ActAmountDecimalPlaces1 * 20));
            double rounded = roundedTimes20 / 20;
            string ActAmountDecimalPlacesStr1 = rounded.ToString();
            string FinalActAmount = (Convert.ToDecimal(ActAmountstr[0]) + Convert.ToDecimal(ActAmountDecimalPlacesStr1)).ToString();
            return String.Format(FinalActAmount);
        }

        public decimal GetOutStandingPaymentAmountByCustomerIDForEmployee(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOutStandingPaymentAmountByCustomerIDForEmployee";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal OutstandingAmount = 0;
                decimal TotalOutstandingAmount = 0;
                while (dr.Read())
                {
                    OutstandingAmount = objBaseSqlManager.GetDecimal(dr, "OutstandingAmount");
                    TotalOutstandingAmount += OutstandingAmount;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TotalOutstandingAmount;
            }
        }

        public GetAllowanceDetail GetTopOneAllowanceDetailByEmployeeCode(long YearID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTopOneAllowanceDetailByEmployeeCode";
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetAllowanceDetail obj = new GetAllowanceDetail();
                while (dr.Read())
                {
                    obj.BasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.HouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "TotalHouseRentAllowance");
                    obj.TotalBasic = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.ConveyancePerDay = objBaseSqlManager.GetDecimal(dr, "ConveyancePerDay");
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.PerformanceAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "PerformanceAllowanceStatusID");
                    obj.CityAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "CityAllowanceStatusID");
                    obj.PFStatusID = objBaseSqlManager.GetInt64(dr, "PFStatusID");
                    obj.ESICStatusID = objBaseSqlManager.GetInt64(dr, "ESICStatusID");
                    obj.OpeningLeaves = objBaseSqlManager.GetInt64(dr, "OpeningLeaves");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 16 July 2020 Piyush Limbani
        public AdditionalAmountDetails GetAdditionalAmountBySalarySheetID(long SalarySheetID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAdditionalAmountBySalarySheetID";
                cmdGet.Parameters.AddWithValue("@SalarySheetID", SalarySheetID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AdditionalAmountDetails obj = new AdditionalAmountDetails();
                while (dr.Read())
                {
                    obj.AdditionalPresent = objBaseSqlManager.GetInt32(dr, "AdditionalPresent");
                    obj.TotalPresent = objBaseSqlManager.GetInt32(dr, "TotalPresent");
                    obj.AdditionalSunday = objBaseSqlManager.GetInt32(dr, "AdditionalSunday");
                    obj.TotalSunday = objBaseSqlManager.GetInt32(dr, "TotalSunday");
                    obj.AdditionalHoliday = objBaseSqlManager.GetInt32(dr, "AdditionalHoliday");
                    obj.TotalHoliday = objBaseSqlManager.GetInt32(dr, "TotalHoliday");
                    obj.AdditionalAbsent = objBaseSqlManager.GetInt32(dr, "AdditionalAbsent");
                    obj.TotalAbsent = objBaseSqlManager.GetInt32(dr, "TotalAbsent");

                    obj.AdditionalCityAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowance");
                    obj.TotalCityAllowance = objBaseSqlManager.GetDecimal(dr, "TotalCityAllowance");
                    obj.AdditionalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalVehicleAllowance");
                    obj.TotalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "TotalVehicleAllowance");
                    obj.AdditionalConveyance = objBaseSqlManager.GetDecimal(dr, "AdditionalConveyance");
                    obj.TotalConveyance = objBaseSqlManager.GetDecimal(dr, "TotalConveyance");
                    obj.AdditionalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalPerformanceAllowance");
                    obj.TotalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "TotalPerformanceAllowance");

                    obj.AdditionalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalAvailedLeaves");
                    obj.TotalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "TotalAvailedLeaves");
                    obj.AdditionalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalClosingLeaves");
                    obj.TotalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "TotalClosingLeaves");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public EmployeeDetail GetEmployeeDetailByEmployeeCode(string StartDate, string EndDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeDetailByEmployeeCode";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                EmployeeDetail obj = new EmployeeDetail();
                while (dr.Read())
                {
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "Id");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    obj.AllowanceDetailID = objBaseSqlManager.GetInt64(dr, "AllowanceDetailID");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }

                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    if (obj.BirthDate < DateTime.Now)
                    {
                        obj.Age = GetAge(obj.BirthDate);
                    }
                    else
                    {
                        obj.Age = null;
                    }
                    obj.OpeningDate = objBaseSqlManager.GetDateTime(dr, "OpeningDate");
                    if (obj.OpeningDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OpeningDatestr = obj.OpeningDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.OpeningDatestr = "";
                    }
                    obj.IncrementDate = objBaseSqlManager.GetDateTime(dr, "IncrementDate");
                    if (obj.IncrementDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.IncrementDatestr = obj.IncrementDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.IncrementDatestr = "";
                    }
                    obj.DA1Date = objBaseSqlManager.GetDateTime(dr, "DA1Date");
                    if (obj.DA1Date != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DA1Datestr = obj.DA1Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DA1Datestr = "";
                    }
                    obj.DA2Date = objBaseSqlManager.GetDateTime(dr, "DA2Date");
                    if (obj.DA2Date != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DA2Datestr = obj.DA2Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DA2Datestr = "";
                    }
                    obj.OthersDate = objBaseSqlManager.GetDateTime(dr, "OthersDate");
                    if (obj.OthersDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OthersDatestr = obj.OthersDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.OthersDatestr = "";
                    }
                    obj.BasicAllowance1 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance1");
                    obj.BasicAllowance2 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance2");
                    obj.BasicAllowance3 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance3");
                    obj.BasicAllowance4 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance4");
                    obj.BasicAllowance5 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance5");
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.HRAPercentage1 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage1");
                    obj.HRAPercentage2 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage2");
                    obj.HRAPercentage3 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage3");
                    obj.HRAPercentage4 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage4");
                    obj.HRAPercentage5 = objBaseSqlManager.GetDecimal(dr, "HRAPercentage5");
                    obj.HouseRentAllowance1 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance1");
                    obj.HouseRentAllowance2 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance2");
                    obj.HouseRentAllowance3 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance3");
                    obj.HouseRentAllowance4 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance4");
                    obj.HouseRentAllowance5 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance5");
                    obj.TotalHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "TotalHouseRentAllowance");

                    obj.TotalWages1 = objBaseSqlManager.GetDecimal(dr, "TotalWages1");
                    obj.TotalWages2 = objBaseSqlManager.GetDecimal(dr, "TotalWages2");
                    obj.TotalWages3 = objBaseSqlManager.GetDecimal(dr, "TotalWages3");
                    obj.TotalWages4 = objBaseSqlManager.GetDecimal(dr, "TotalWages4");
                    obj.TotalWages5 = objBaseSqlManager.GetDecimal(dr, "TotalWages5");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");


                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.ConveyancePerDay = objBaseSqlManager.GetDecimal(dr, "ConveyancePerDay");
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");


                    obj.PerformanceAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "PerformanceAllowanceStatusID");
                    obj.CityAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "CityAllowanceStatusID");
                    obj.PFStatusID = objBaseSqlManager.GetInt64(dr, "PFStatusID");
                    obj.ESICStatusID = objBaseSqlManager.GetInt64(dr, "ESICStatusID");
                    obj.BonusPercentage = objBaseSqlManager.GetDecimal(dr, "BonusPercentage");
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.BonusStatusID = objBaseSqlManager.GetInt64(dr, "BonusStatusID");
                    obj.LeaveEnhancementPercentage = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementPercentage");
                    obj.LeaveEnhancementAmount = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementAmount");
                    obj.LeaveEnhancementStatusID = objBaseSqlManager.GetInt64(dr, "LeaveEnhancementStatusID");
                    obj.GratuityPercentage = objBaseSqlManager.GetDecimal(dr, "GratuityPercentage");
                    obj.GratuityAmount = objBaseSqlManager.GetDecimal(dr, "GratuityAmount");
                    obj.GratuityStatusID = objBaseSqlManager.GetInt64(dr, "GratuityStatusID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");

                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.CreatedOnstr = string.Format("{0:G}", obj.CreatedOn);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        //18/04/2020
        public GetOTDetails GetTotalOTMinutes(int MonthID, int YearID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalOTMinutes";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetOTDetails obj = new GetOTDetails();
                while (dr.Read())
                {
                    obj.PluseTotalMinutes = objBaseSqlManager.GetDecimal(dr, "PluseTotalMinutes");
                    obj.MinuseTotalMinutes = objBaseSqlManager.GetDecimal(dr, "MinuseTotalMinutes");
                    obj.TotalMinutes = objBaseSqlManager.GetDecimal(dr, "TotalMinutes");
                    obj.TotalHrs = 0;
                    // obj.TotalHrs = objBaseSqlManager.GetDecimal(dr, "TotalHrs");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public int GetVehicleNoOfDaysCount(int MonthID, int YearID, long EmployeeCode)
        {
            int VehicleNoOfDaysCount = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleNoOfDaysCount";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    VehicleNoOfDaysCount = objBaseSqlManager.GetInt32(dr, "VehicleNoOfDaysCount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return VehicleNoOfDaysCount;
            }
        }

        //21/04/2020
        public decimal GetEarnedLeavesByMonth(int MonthID)
        {
            decimal EarnedLeaves = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEarnedLeavesByMonth";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    EarnedLeaves = objBaseSqlManager.GetDecimal(dr, "EarnedLeaves");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return EarnedLeaves;
            }
        }

        public List<GetSundayDate> GetSundayDateList(string MonthStartDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetMonthSundayDate";
                cmdGet.Parameters.AddWithValue("@MonthStartDate", MonthStartDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetSundayDate> objlst = new List<GetSundayDate>();
                while (dr.Read())
                {
                    GetSundayDate obj = new GetSundayDate();
                    obj.Date = objBaseSqlManager.GetDateTime(dr, "Date");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<GetHolidayDate> GetHolidayDateList(int MonthID, int YearID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetHolidayDateList";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetHolidayDate> objlst = new List<GetHolidayDate>();
                int HolidayCount = 0;
                while (dr.Read())
                {
                    GetHolidayDate obj = new GetHolidayDate();
                    obj.HolidayDate = objBaseSqlManager.GetDateTime(dr, "FestivalDate");
                    HolidayCount = HolidayCount + 1;
                    obj.Holiday = HolidayCount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public int GetAbsentCount(string Sdate, string Edate, long EmployeeCode)
        {
            int absentcount = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAbsentCount";
                cmdGet.Parameters.AddWithValue("@Sdate", Convert.ToDateTime(Sdate));
                cmdGet.Parameters.AddWithValue("@Edate", Convert.ToDateTime(Edate));
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    absentcount = objBaseSqlManager.GetInt32(dr, "absentcount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return absentcount;
            }
        }

        // 27-04-2020
        public List<GetAbsentHolidayDate> GetAbsentHolidayDate(int MonthID, int YearID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAbsentHolidayDate";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetAbsentHolidayDate> objlst = new List<GetAbsentHolidayDate>();
                while (dr.Read())
                {
                    GetAbsentHolidayDate obj = new GetAbsentHolidayDate();
                    obj.Date = objBaseSqlManager.GetDateTime(dr, "ADate");
                    obj.Status = objBaseSqlManager.GetTextValue(dr, "Status");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public decimal GetLastOpeningLeavesByEmployeeCode(long EmployeeCode)
        {
            decimal ClosingLeaves = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastOpeningLeavesByEmployeeCode";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    decimal ClosingLeavesAct = objBaseSqlManager.GetInt32(dr, "ClosingLeaves");
                    decimal AdditionalClosingLeaves = objBaseSqlManager.GetInt32(dr, "AdditionalClosingLeaves");
                    ClosingLeaves = ClosingLeavesAct + AdditionalClosingLeaves;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ClosingLeaves;
            }
        }

        // 14-05-2020
        public decimal GetLastOpeningLeavesBySalarySheetID(long SalarySheetID)
        {
            decimal ClosingLeaves = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastOpeningLeavesBySalarySheetID";
                cmdGet.Parameters.AddWithValue("@SalarySheetID", SalarySheetID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AdvanceAndOtherDeductions obj = new AdvanceAndOtherDeductions();
                while (dr.Read())
                {
                    ClosingLeaves = objBaseSqlManager.GetInt32(dr, "ClosingLeaves");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ClosingLeaves;
            }
        }

        // 04/05/2020
        public AdvanceAndOtherDeductions GetLastAdvanceAndOtherDeductionsByEmployeeCode(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastAdvanceAndOtherDeductionsByEmployeeCode";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AdvanceAndOtherDeductions obj = new AdvanceAndOtherDeductions();
                while (dr.Read())
                {
                    obj.ClosingAdvance = objBaseSqlManager.GetDecimal(dr, "ClosingAdvance");
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public AdvanceAndOtherDeductions GetAdvanceAndOtherDeductionsDetailBySalarySheetID(long SalarySheetID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAdvanceAndOtherDeductionsDetailBySalarySheetID";
                cmdGet.Parameters.AddWithValue("@SalarySheetID", SalarySheetID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AdvanceAndOtherDeductions obj = new AdvanceAndOtherDeductions();
                while (dr.Read())
                {
                    obj.OpeningAdvance = objBaseSqlManager.GetDecimal(dr, "OpeningAdvance");
                    obj.Addition = objBaseSqlManager.GetDecimal(dr, "Addition");
                    obj.Deductions = objBaseSqlManager.GetDecimal(dr, "Deductions");
                    obj.ClosingAdvance = objBaseSqlManager.GetDecimal(dr, "ClosingAdvance");
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                    obj.Goods = objBaseSqlManager.GetDecimal(dr, "Goods");
                    obj.AnyOtherDeductions1 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions1");
                    obj.AnyOtherDeductions2 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions2");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        //30/04/2020
        public GetLastPFInformation GetLastPFInformation()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastPFInformation";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetLastPFInformation obj = new GetLastPFInformation();
                while (dr.Read())
                {
                    obj.PFID = objBaseSqlManager.GetInt64(dr, "PFID");
                    obj.HighestSlab = objBaseSqlManager.GetDecimal(dr, "HighestSlab");
                    obj.HighestPF = objBaseSqlManager.GetDecimal(dr, "HighestPF");
                    obj.PFPercentage = objBaseSqlManager.GetDecimal(dr, "PFPercentage");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetLastESICInformation GetLastESICInformation()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastESICInformation";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetLastESICInformation obj = new GetLastESICInformation();
                while (dr.Read())
                {
                    obj.ESICID = objBaseSqlManager.GetInt64(dr, "ESICID");
                    obj.EmployeeSlab = objBaseSqlManager.GetDecimal(dr, "EmployeeSlab");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetLastPTInformation GetLastPTInformationByMonth(int MonthID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastPTInformationByMonth";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetLastPTInformation obj = new GetLastPTInformation();
                while (dr.Read())
                {
                    obj.PTID = objBaseSqlManager.GetInt64(dr, "PTID");
                    obj.PTMonthID = objBaseSqlManager.GetInt32(dr, "MonthID");
                    obj.PTHighestSlab = objBaseSqlManager.GetDecimal(dr, "HighestSlab");
                    obj.PTHighestAmount = objBaseSqlManager.GetDecimal(dr, "HighestAmount");
                    obj.PTLowestSlab = objBaseSqlManager.GetDecimal(dr, "LowestSlab");
                    obj.PTLowestAmount = objBaseSqlManager.GetDecimal(dr, "LowestAmount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetLastMLWFInformation GetLastMLWFInformationByMonth(int MonthID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastMLWFInformationByMonth";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetLastMLWFInformation obj = new GetLastMLWFInformation();
                while (dr.Read())
                {
                    obj.MLWFID = objBaseSqlManager.GetInt64(dr, "MLWFID");
                    obj.MLWFMonthID = objBaseSqlManager.GetInt32(dr, "MonthID");
                    obj.MLWFHighestSlab = objBaseSqlManager.GetDecimal(dr, "HighestSlab");
                    obj.MLWFHighestAmount = objBaseSqlManager.GetDecimal(dr, "HighestAmount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        //public long CheckSalaryExist(int MonthID, int YearID, long EmployeeCode)
        //{
        //    long SalarySheetID = 0;
        //    try
        //    {
        //        SqlCommand cmdGet = new SqlCommand();
        //        BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //        cmdGet.CommandType = CommandType.StoredProcedure;
        //        cmdGet.CommandText = "CheckSalaryExist";
        //        cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
        //        cmdGet.Parameters.AddWithValue("@YearID", YearID);
        //        cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
        //        SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //        while (dr.Read())
        //        {
        //            SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
        //        }
        //        dr.Close();
        //        objBaseSqlManager.ForceCloseConnection();
        //    }
        //    catch
        //    {
        //        SalarySheetID = 0;
        //    }
        //    return SalarySheetID;
        //}

        public SalaryExistModel CheckSalaryExist(int MonthID, int YearID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckSalaryExist";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);             
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                SalaryExistModel obj = new SalaryExistModel();
                while (dr.Read())
                {
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public long AddSalarySheet(SalarySheet_Master Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.SalarySheetID == 0)
                {
                    context.SalarySheet_Master.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.SalarySheetID > 0)
                {
                    return Obj.SalarySheetID;
                    // return true;
                }
                else
                {
                    return 0;
                    // return false;
                }
            }
        }

        public List<SalarySheetListResponse> GetSalarySheetList(SearchSalarySheet model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAttandanceListByMonthAndYear";
                cmdGet.Parameters.AddWithValue("@MonthID", model.MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", model.YearID);
                cmdGet.Parameters.AddWithValue("@GodownID", model.GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", model.EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalarySheetListResponse> objlst = new List<SalarySheetListResponse>();
                while (dr.Read())
                {
                    SalarySheetListResponse obj = new SalarySheetListResponse();
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");

                    // 27 July 2020 Piyush Limbani
                    int Present = objBaseSqlManager.GetInt32(dr, "Present");
                    obj.AdditionalPresent = objBaseSqlManager.GetInt32(dr, "AdditionalPresent");
                    obj.Present = Present + obj.AdditionalPresent;
                    int Absent = objBaseSqlManager.GetInt32(dr, "Absent");
                    obj.AdditionalAbsent = objBaseSqlManager.GetInt32(dr, "AdditionalAbsent");
                    obj.Absent = Absent + obj.AdditionalAbsent;
                    int Holiday = objBaseSqlManager.GetInt32(dr, "Holiday");
                    obj.AdditionalHoliday = objBaseSqlManager.GetInt32(dr, "AdditionalHoliday");
                    obj.Holiday = Holiday + obj.AdditionalHoliday;
                    int Sunday = objBaseSqlManager.GetInt32(dr, "Sunday");
                    obj.AdditionalSunday = objBaseSqlManager.GetInt32(dr, "AdditionalSunday");
                    obj.Sunday = Sunday + obj.AdditionalSunday;
                    // 27 July 2020 Piyush Limbani

                    //obj.Present = objBaseSqlManager.GetInt32(dr, "Present");
                    //obj.Absent = objBaseSqlManager.GetInt32(dr, "Absent");
                    //obj.Holiday = objBaseSqlManager.GetInt32(dr, "Holiday");
                    //obj.Sunday = objBaseSqlManager.GetInt32(dr, "Sunday");
                    obj.TotalDays = objBaseSqlManager.GetInt32(dr, "TotalDays");
                    obj.BasicAllowance = objBaseSqlManager.GetDecimal(dr, "BasicAllowance");
                    obj.HouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance");
                    obj.EarnedBasicWages = objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages");
                    obj.EarnedHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "EarnedHouseRentAllowance");
                    obj.TotalEarnedWages = objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages");

                    // 17 July 2020 Piyush Limbani
                    decimal CityAllowance = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    obj.AdditionalCityAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowance");
                    obj.CityAllowance = CityAllowance + obj.AdditionalCityAllowance;

                    decimal VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.AdditionalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalVehicleAllowance");
                    obj.VehicleAllowance = VehicleAllowance + obj.AdditionalVehicleAllowance;

                    decimal Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.AdditionalConveyance = objBaseSqlManager.GetDecimal(dr, "AdditionalConveyance");
                    obj.Conveyance = Conveyance + obj.AdditionalConveyance;

                    decimal PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.AdditionalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalPerformanceAllowance");
                    obj.PerformanceAllowance = PerformanceAllowance + obj.AdditionalPerformanceAllowance;
                    // 17 July 2020 Piyush Limbani


                    //obj.CityAllowance = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    //obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    //obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    //obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.GrossWagesPayable = objBaseSqlManager.GetDecimal(dr, "GrossWagesPayable");
                    obj.PF = objBaseSqlManager.GetDecimal(dr, "PF");
                    obj.PT = objBaseSqlManager.GetDecimal(dr, "PT");
                    obj.ESIC = objBaseSqlManager.GetDecimal(dr, "ESIC");
                    obj.MLWF = objBaseSqlManager.GetDecimal(dr, "MLWF");
                    obj.TotalDeductions = objBaseSqlManager.GetDecimal(dr, "TotalDeductions");
                    obj.NetWagesPaid = objBaseSqlManager.GetDecimal(dr, "NetWagesPaid");

                    obj.OpeningLeaves = objBaseSqlManager.GetDecimal(dr, "OpeningLeaves");
                    obj.EarnedLeaves = objBaseSqlManager.GetDecimal(dr, "EarnedLeaves");

                    // 27 July 2020 Piyush Limbani
                    decimal AvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AvailedLeaves");
                    obj.AdditionalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalAvailedLeaves");
                    obj.AvailedLeaves = AvailedLeaves + obj.AdditionalAvailedLeaves;
                    decimal ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.AdditionalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalClosingLeaves");
                    obj.ClosingLeaves = ClosingLeaves + obj.AdditionalClosingLeaves;
                    // 27 July 2020 Piyush Limbani

                    //obj.AvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AvailedLeaves");
                    //obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.OpeningAdvance = objBaseSqlManager.GetDecimal(dr, "OpeningAdvance");
                    obj.Addition = objBaseSqlManager.GetDecimal(dr, "Addition");
                    obj.Deductions = objBaseSqlManager.GetDecimal(dr, "Deductions");
                    obj.ClosingAdvance = objBaseSqlManager.GetDecimal(dr, "ClosingAdvance");
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                    obj.Goods = objBaseSqlManager.GetDecimal(dr, "Goods");
                    obj.AnyOtherDeductions1 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions1");
                    obj.AnyOtherDeductions2 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions2");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<SalarySheetListResponse> GetExportExcelSalarySheet(long MonthID, long YearID, long GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExportExcelSalarySheet";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalarySheetListResponse> objlst = new List<SalarySheetListResponse>();
                while (dr.Read())
                {
                    SalarySheetListResponse obj = new SalarySheetListResponse();
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    // obj.RowNumberstr = obj.RowNumber.ToString();
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.Sex = objBaseSqlManager.GetTextValue(dr, "Sex");
                    obj.Designation = objBaseSqlManager.GetTextValue(dr, "Designation");
                    obj.DateofJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateofJoining == Convert.ToDateTime("10/10/2014") || obj.DateofJoining == null)
                    {
                        obj.DateofJoiningstr = "";
                    }
                    else
                    {
                        obj.DateofJoiningstr = obj.DateofJoining.ToString("dd/MM/yyyy");
                    }
                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    obj.Age = DateTime.Now.Year - obj.BirthDate.Year;
                    obj.Agestr = obj.Age.ToString();
                    obj.WorkingHours = "9.00 AM - 6.00 PM";
                    obj.IntervalForRest = "1.00 PM - 2.00 PM";

                    // 27 July 2020 Piyush Limbani
                    int Present = objBaseSqlManager.GetInt32(dr, "Present");
                    obj.AdditionalPresent = objBaseSqlManager.GetInt32(dr, "AdditionalPresent");
                    obj.Present = Present + obj.AdditionalPresent;
                    int Absent = objBaseSqlManager.GetInt32(dr, "Absent");
                    obj.AdditionalAbsent = objBaseSqlManager.GetInt32(dr, "AdditionalAbsent");
                    obj.Absent = Absent + obj.AdditionalAbsent;
                    int Holiday = objBaseSqlManager.GetInt32(dr, "Holiday");
                    obj.AdditionalHoliday = objBaseSqlManager.GetInt32(dr, "AdditionalHoliday");
                    obj.Holiday = Holiday + obj.AdditionalHoliday;
                    int Sunday = objBaseSqlManager.GetInt32(dr, "Sunday");
                    obj.AdditionalSunday = objBaseSqlManager.GetInt32(dr, "AdditionalSunday");
                    obj.Sunday = Sunday + obj.AdditionalSunday;
                    // 27 July 2020 Piyush Limbani

                    //obj.Present = objBaseSqlManager.GetInt32(dr, "Present");
                    //obj.Absent = objBaseSqlManager.GetInt32(dr, "Absent");
                    //obj.Holiday = objBaseSqlManager.GetInt32(dr, "Holiday");
                    //obj.Sunday = objBaseSqlManager.GetInt32(dr, "Sunday");
                    obj.TotalDays = objBaseSqlManager.GetInt32(dr, "TotalDays");
                    obj.BasicAllowance = objBaseSqlManager.GetDecimal(dr, "BasicAllowance");
                    obj.HouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance");
                    obj.EarnedBasicWages = objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages");
                    obj.EarnedHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "EarnedHouseRentAllowance");
                    obj.TotalEarnedWages = objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages");
                    // obj.CityAllowanceHours = objBaseSqlManager.GetDecimal(dr, "CityAllowanceHours");
                    obj.CityAllowanceMinutes = objBaseSqlManager.GetDecimal(dr, "CityAllowanceMinutes");

                    // 17 July 2020 Piyush Limbani
                    decimal CityAllowance = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    obj.AdditionalCityAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowance");
                    obj.CityAllowance = CityAllowance + obj.AdditionalCityAllowance;

                    decimal VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.AdditionalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalVehicleAllowance");
                    obj.VehicleAllowance = VehicleAllowance + obj.AdditionalVehicleAllowance;

                    decimal Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.AdditionalConveyance = objBaseSqlManager.GetDecimal(dr, "AdditionalConveyance");
                    obj.Conveyance = Conveyance + obj.AdditionalConveyance;

                    decimal PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.AdditionalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalPerformanceAllowance");
                    obj.PerformanceAllowance = PerformanceAllowance + obj.AdditionalPerformanceAllowance;
                    // 17 July 2020 Piyush Limbani

                    //obj.CityAllowance = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    //obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    //obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    //obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.GrossWagesPayable = objBaseSqlManager.GetDecimal(dr, "GrossWagesPayable");
                    obj.PF = objBaseSqlManager.GetDecimal(dr, "PF");
                    obj.ESIC = objBaseSqlManager.GetDecimal(dr, "ESIC");
                    obj.PT = objBaseSqlManager.GetDecimal(dr, "PT");
                    obj.MLWF = objBaseSqlManager.GetDecimal(dr, "MLWF");
                    obj.TotalDeductions = objBaseSqlManager.GetDecimal(dr, "TotalDeductions");
                    obj.NetWagesPaid = objBaseSqlManager.GetDecimal(dr, "NetWagesPaid");
                    obj.OpeningLeaves = objBaseSqlManager.GetDecimal(dr, "OpeningLeaves");
                    obj.EarnedLeaves = objBaseSqlManager.GetDecimal(dr, "EarnedLeaves");

                    // 27 July 2020 Piyush Limbani
                    decimal AvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AvailedLeaves");
                    obj.AdditionalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalAvailedLeaves");
                    obj.AvailedLeaves = AvailedLeaves + obj.AdditionalAvailedLeaves;
                    decimal ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.AdditionalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalClosingLeaves");
                    obj.ClosingLeaves = ClosingLeaves + obj.AdditionalClosingLeaves;
                    // 27 July 2020 Piyush Limbani

                    //obj.AvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AvailedLeaves");
                    //obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.Sign = "";
                    obj.OpeningAdvance = objBaseSqlManager.GetDecimal(dr, "OpeningAdvance");
                    obj.Addition = objBaseSqlManager.GetDecimal(dr, "Addition");
                    obj.Deductions = objBaseSqlManager.GetDecimal(dr, "Deductions");
                    obj.ClosingAdvance = objBaseSqlManager.GetDecimal(dr, "ClosingAdvance");
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                    obj.Goods = objBaseSqlManager.GetDecimal(dr, "Goods");
                    obj.FinalTotalDeductions = obj.Deductions + obj.TDS + obj.Goods;
                    obj.NetWagesToPay = obj.NetWagesPaid - obj.FinalTotalDeductions;
                    obj.AnyOtherDeductions1 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions1");
                    obj.AnyOtherDeductions2 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions2");

                    // 14 July 2020 Piyush Limbani
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    if (obj.CustomerID != 0)
                    {
                        obj.CustomerName = GetEmployeeCustomerNameByCustomerID(obj.CustomerID);
                    }
                    else
                    {
                        GetAllowanceDetail AllowanceDetail = null;
                        long YearForAlloawance = 0;
                        if (MonthID == 1 || MonthID == 2 || MonthID == 3)
                        {
                            YearForAlloawance = YearID - 1;
                            AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, obj.EmployeeCode);
                            if (AllowanceDetail.CustomerID != 0)
                            {
                                obj.CustomerName = GetEmployeeCustomerNameByCustomerID(AllowanceDetail.CustomerID);
                            }
                        }
                        else
                        {
                            YearForAlloawance = YearID;
                            AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, obj.EmployeeCode);
                            if (AllowanceDetail.CustomerID != 0)
                            {
                                obj.CustomerName = GetEmployeeCustomerNameByCustomerID(AllowanceDetail.CustomerID);
                            }
                        }
                    }
                    // 14 July 2020 Piyush Limbani
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string GetEmployeeCustomerNameByCustomerID(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeCustomerNameByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string CustomerName = "";
                while (dr.Read())
                {
                    CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CustomerName;
            }
        }

        public EmployeeDetail GetOpeningAllowanceDetail(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOpeningAllowanceDetail";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                EmployeeDetail obj = new EmployeeDetail();
                while (dr.Read())
                {
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "Id");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    if (obj.BirthDate < DateTime.Now)
                    {
                        obj.Age = GetAge(obj.BirthDate);
                    }
                    else
                    {
                        obj.Age = null;
                    }
                    obj.BasicAllowance1 = objBaseSqlManager.GetDecimal(dr, "BasicAllowance1");
                    obj.HouseRentAllowance1 = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance1");
                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        private string GetAge(DateTime birthday)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(birthday).Ticks).Year - 1;
            DateTime dtPastYearDate = birthday.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (dtPastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (dtPastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(dtPastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(dtPastYearDate).Hours;
            int Minutes = Now.Subtract(dtPastYearDate).Minutes;
            int Seconds = Now.Subtract(dtPastYearDate).Seconds;
            return String.Format("{0}Year {1}Month {2}Day",
                                Years, Months, Days);
        }

        //14-04-2020
        public GetTotalDatsIfTheMonth GetTotalDaysMonthInTheMonth(int MonthID, int YearID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalDaysMonthInTheMonth";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetTotalDatsIfTheMonth obj = new GetTotalDatsIfTheMonth();
                while (dr.Read())
                {
                    obj.TotalMonthDay = objBaseSqlManager.GetInt32(dr, "TotalMonthDay");
                    obj.TotalSunday = objBaseSqlManager.GetInt32(dr, "TotalSunday");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        //16-04-2020 Attendance Changes
        public bool AddNewDailyAttendance(NewDailyAttendanceModel Obj)
        {
            //CheckDailyAttendanceaDateIsExistByEmployeeCode -- 08-05-2020
            long DAttendanceID = CheckDailyAttendanceADateIsExistByEmployeeCode(Obj.ADate, Obj.EmployeeCode);

            if (DAttendanceID == 0)
            {
                SqlCommand cmdAdd = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdAdd.CommandType = CommandType.StoredProcedure;
                    cmdAdd.CommandText = "InsertNewDailyAttendance";
                    cmdAdd.Parameters.AddWithValue("@EmployeeCode", Obj.EmployeeCode);
                    cmdAdd.Parameters.AddWithValue("@AEmployeeName", Obj.AEmployeeName);
                    cmdAdd.Parameters.AddWithValue("@ADate", Obj.ADate);
                    cmdAdd.Parameters.AddWithValue("@AMonth", Obj.ADate.Month);
                    cmdAdd.Parameters.AddWithValue("@AYear", Obj.ADate.Year);
                    cmdAdd.Parameters.AddWithValue("@ShiftStartTime", Obj.ShiftStartTime);
                    cmdAdd.Parameters.AddWithValue("@ShiftStartDateTime", Obj.ShiftStartDateTime);
                    cmdAdd.Parameters.AddWithValue("@InDateTime", Obj.InDateTime);
                    cmdAdd.Parameters.AddWithValue("@TimeIn", Obj.TimeIn);
                    cmdAdd.Parameters.AddWithValue("@OutDateTime", Obj.OutDateTime);
                    cmdAdd.Parameters.AddWithValue("@TimeOut", Obj.TimeOut);
                    cmdAdd.Parameters.AddWithValue("@TotalHoursWorked", Obj.TotalHoursWorked);
                    cmdAdd.Parameters.AddWithValue("@OT", Obj.OT);
                    cmdAdd.Parameters.AddWithValue("@ShiftEndTime", "18.00");
                    cmdAdd.Parameters.AddWithValue("@Status", Obj.Status);
                    cmdAdd.Parameters.AddWithValue("@CreatedBy", Obj.CreatedBy);
                    cmdAdd.Parameters.AddWithValue("@CreatedOn", Obj.CreatedOn);
                    cmdAdd.Parameters.AddWithValue("@IsDelete", Obj.IsDelete);
                    objBaseSqlManager.ExecuteNonQuery(cmdAdd);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            else
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateNewDailyAttendanceByDAttendanceID";
                    cmdGet.Parameters.AddWithValue("@DAttendanceID", DAttendanceID);
                    cmdGet.Parameters.AddWithValue("@EmployeeCode", Obj.EmployeeCode);
                    cmdGet.Parameters.AddWithValue("@AEmployeeName", Obj.AEmployeeName);
                    cmdGet.Parameters.AddWithValue("@ADate", Obj.ADate);
                    cmdGet.Parameters.AddWithValue("@AMonth", Obj.ADate.Month);
                    cmdGet.Parameters.AddWithValue("@AYear", Obj.ADate.Year);
                    cmdGet.Parameters.AddWithValue("@ShiftStartTime", Obj.ShiftStartTime);
                    cmdGet.Parameters.AddWithValue("@ShiftStartDateTime", Obj.ShiftStartDateTime);
                    cmdGet.Parameters.AddWithValue("@InDateTime", Obj.InDateTime);
                    cmdGet.Parameters.AddWithValue("@TimeIn", Obj.TimeIn);
                    cmdGet.Parameters.AddWithValue("@OutDateTime", Obj.OutDateTime);
                    cmdGet.Parameters.AddWithValue("@TimeOut", Obj.TimeOut);
                    cmdGet.Parameters.AddWithValue("@TotalHoursWorked", Obj.TotalHoursWorked);
                    cmdGet.Parameters.AddWithValue("@OT", Obj.OT);
                    cmdGet.Parameters.AddWithValue("@ShiftEndTime", "18.00");
                    cmdGet.Parameters.AddWithValue("@Status", Obj.Status);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", Obj.UpdatedBy);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", Obj.UpdatedOn);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public long CheckDailyAttendanceADateIsExistByEmployeeCode(DateTime ADate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckDailyAttendanceADateIsExistByEmployeeCode";
                cmdGet.Parameters.AddWithValue("@ADate", ADate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long DAttendanceID = 0;
                while (dr.Read())
                {
                    DAttendanceID = objBaseSqlManager.GetInt64(dr, "DAttendanceID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return DAttendanceID;
            }
        }

        public List<AttandanceListResponse> GetAttandanceList(DateTime? FromDate, DateTime? ToDate, long GodownID, string EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAttandanceListByYear";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AttandanceListResponse> objlst = new List<AttandanceListResponse>();
                while (dr.Read())
                {
                    AttandanceListResponse obj = new AttandanceListResponse();
                    obj.DAttendanceID = objBaseSqlManager.GetInt64(dr, "DAttendanceID");
                    obj.ADate = objBaseSqlManager.GetDateTime(dr, "ADate");
                    if (obj.ADate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ADatestr = "";
                    }
                    else
                    {
                        obj.ADatestr = obj.ADate.ToString("dd/MM/yyyy");
                    }
                    obj.AEmployeeName = objBaseSqlManager.GetTextValue(dr, "AEmployeeName");
                    obj.DayName = objBaseSqlManager.GetTextValue(dr, "DayName");
                    obj.TimeIn = objBaseSqlManager.GetTextValue(dr, "TimeIn");
                    obj.TimeOut = objBaseSqlManager.GetTextValue(dr, "TimeOut");
                    obj.TotalHoursWorked = objBaseSqlManager.GetDecimal(dr, "TotalHoursWorked");
                    obj.OT = objBaseSqlManager.GetDecimal(dr, "OT");
                    obj.Status = objBaseSqlManager.GetTextValue(dr, "Status");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //17-04-2020
        public long AddFestival(Festival_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.FestivalID == 0)
                {
                    context.Festival_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.FestivalID > 0)
                {
                    return Obj.FestivalID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<FestivalListResponse> GetAllFestivalList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllFestivalList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<FestivalListResponse> objlst = new List<FestivalListResponse>();
                while (dr.Read())
                {
                    FestivalListResponse obj = new FestivalListResponse();
                    obj.FestivalID = objBaseSqlManager.GetInt64(dr, "FestivalID");
                    obj.EventID = objBaseSqlManager.GetInt64(dr, "EventID");
                    obj.EventName = objBaseSqlManager.GetTextValue(dr, "EventName");
                    obj.FestivalDate = objBaseSqlManager.GetDateTime(dr, "FestivalDate");
                    if (obj.FestivalDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FestivalDatestr = obj.FestivalDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FestivalDatestr = "";
                    }
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteFestival(long FestivalID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteFestival";
                cmdGet.Parameters.AddWithValue("@FestivalID", FestivalID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //20-04-2020
        public List<AllowanceStatusNameResponse> GetAllAllowanceStatusName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllAllowanceStatusName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AllowanceStatusNameResponse> lstProduct = new List<AllowanceStatusNameResponse>();
                while (dr.Read())
                {
                    AllowanceStatusNameResponse obj = new AllowanceStatusNameResponse();
                    obj.AllowanceStatusID = objBaseSqlManager.GetInt64(dr, "AllowanceStatusID");
                    obj.Status = objBaseSqlManager.GetTextValue(dr, "Status");
                    lstProduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstProduct;
            }
        }

        public long AddEarnedLeaves(EarnedLeaves_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.EarnedLeavesID == 0)
                {
                    context.EarnedLeaves_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.EarnedLeavesID > 0)
                {
                    return Obj.EarnedLeavesID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<EarnedLeavesListResponse> GetAllEarnedLeavesList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEarnedLeavesList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EarnedLeavesListResponse> objlst = new List<EarnedLeavesListResponse>();
                while (dr.Read())
                {
                    EarnedLeavesListResponse obj = new EarnedLeavesListResponse();
                    obj.EarnedLeavesID = objBaseSqlManager.GetInt64(dr, "EarnedLeavesID");
                    obj.MonthID = objBaseSqlManager.GetInt64(dr, "MonthID");
                    obj.NoOfEarnedLeaves = objBaseSqlManager.GetDecimal(dr, "NoOfEarnedLeaves");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteEarnedLeaves(long EarnedLeavesID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteEarnedLeaves";
                cmdGet.Parameters.AddWithValue("@EarnedLeavesID", EarnedLeavesID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //22-04-2020
        public List<BonusListResponse> GetEmployeeWiseBonusList(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeWiseBonus";
                cmdGet.Parameters.AddWithValue("@FromMonthID", FromMonthID);
                cmdGet.Parameters.AddWithValue("@FromYearID", FromYearID);
                cmdGet.Parameters.AddWithValue("@ToMonthID", ToMonthID);
                cmdGet.Parameters.AddWithValue("@ToYearID", ToYearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BonusListResponse> objlst = new List<BonusListResponse>();
                while (dr.Read())
                {
                    BonusListResponse obj = new BonusListResponse();
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.TotalEarnedWages = objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.BonusPercentage = objBaseSqlManager.GetDecimal(dr, "BonusPercentage");
                    obj.EarnedBasicWages = objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages");
                    obj.BonusStatusID = objBaseSqlManager.GetInt32(dr, "BonusStatusID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //23-04-2020
        public List<LeaveEncashmentListResponse> GetLeaveEncashmentList(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLeaveEncashmentList";
                cmdGet.Parameters.AddWithValue("@FromMonthID", FromMonthID);
                cmdGet.Parameters.AddWithValue("@FromYearID", FromYearID);
                cmdGet.Parameters.AddWithValue("@ToMonthID", ToMonthID);
                cmdGet.Parameters.AddWithValue("@ToYearID", ToYearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LeaveEncashmentListResponse> objlst = new List<LeaveEncashmentListResponse>();
                while (dr.Read())
                {
                    LeaveEncashmentListResponse obj = new LeaveEncashmentListResponse();
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.LeaveEnhancementStatusID = objBaseSqlManager.GetInt32(dr, "LeaveEnhancementStatusID");
                    obj.LeaveEnhancementAmount = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementAmount");
                    obj.LeaveEnhancementPercentage = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementPercentage");
                    decimal ClosingLeaves = GetClosingLeavesBySalarySheetID(obj.SalarySheetID);

                    decimal NoOfDaysLeaveEncashment = GetLastLeaveEncashmentNoOfDays();

                    obj.ClosingLeaves = ClosingLeaves;
                    if (obj.LeaveEnhancementStatusID == 1)
                    {
                        if (obj.LeaveEnhancementAmount == 0)
                        {
                            obj.TotalLeaveEnhancement = Math.Round(obj.GrandTotalWages, 2);
                            obj.LeaveEncashment = Math.Round(((obj.TotalLeaveEnhancement / NoOfDaysLeaveEncashment) * obj.ClosingLeaves)); //NoOfDaysLeaveEncashment = 30
                        }
                        else
                        {
                            obj.TotalLeaveEnhancement = 0;
                            obj.ClosingLeaves = 0;
                            obj.LeaveEncashment = obj.LeaveEnhancementAmount;
                        }
                    }
                    else if (obj.LeaveEnhancementStatusID == 2)
                    {
                        if (obj.LeaveEnhancementAmount == 0)
                        {
                            obj.TotalLeaveEnhancement = Math.Round(obj.TotalBasicAllowance, 2);
                            obj.LeaveEncashment = Math.Round(((obj.TotalLeaveEnhancement / NoOfDaysLeaveEncashment) * obj.ClosingLeaves));  //NoOfDaysLeaveEncashment = 30
                        }
                        else
                        {
                            obj.TotalLeaveEnhancement = 0;
                            obj.ClosingLeaves = 0;
                            obj.LeaveEncashment = obj.LeaveEnhancementAmount;
                        }
                    }
                    else
                    {
                        obj.TotalLeaveEnhancement = 0;
                        obj.LeaveEncashment = 0;
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public decimal GetClosingLeavesBySalarySheetID(long SalarySheetID)
        {
            decimal ClosingLeaves = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetClosingLeavesBySalarySheetID";
                cmdGet.Parameters.AddWithValue("@SalarySheetID", SalarySheetID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ClosingLeaves;
            }
        }

        //30-04-2020
        public decimal GetLastLeaveEncashmentNoOfDays()
        {
            decimal NoOfDaysLeaveEncashment = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastLeaveEncashmentNoOfDays";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    NoOfDaysLeaveEncashment = objBaseSqlManager.GetDecimal(dr, "NoOfDaysLeaveEncashment");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return NoOfDaysLeaveEncashment;
            }
        }

        public List<GratuityListResponse> GetGratuityList(int LeavingYear, int GodownID, long EmployeeCode, DateTime DateOfLeaving)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGratuityList";
                cmdGet.Parameters.AddWithValue("@DateOfLeaving", DateOfLeaving);
                cmdGet.Parameters.AddWithValue("@LeavingYear", LeavingYear);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GratuityListResponse> objlst = new List<GratuityListResponse>();
                while (dr.Read())
                {
                    GratuityListResponse obj = new GratuityListResponse();
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    else
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                    }
                    obj.DateOfLeaving = DateOfLeaving;
                    if (obj.DateOfLeaving == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = "";
                    }
                    else
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("dd/MM/yyyy");
                    }
                    if (obj.DateOfJoining == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.TotalMonth = 0;
                    }
                    else
                    {
                        obj.TotalMonth = objBaseSqlManager.GetInt64(dr, "TotalMonth");
                    }
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.GratuityStatusID = objBaseSqlManager.GetInt32(dr, "GratuityStatusID");
                    obj.GratuityAmount = objBaseSqlManager.GetDecimal(dr, "GratuityAmount");
                    obj.GratuityPercentage = objBaseSqlManager.GetDecimal(dr, "GratuityPercentage");

                    //30-04-2020
                    GetLastGratuityInformation GratuityInfo = GetLastGratuityInformation();


                    if (obj.GratuityStatusID == 1)
                    {
                        if (obj.GratuityAmount == 0)
                        {
                            obj.TotalGratuity = Math.Round(obj.GrandTotalWages, 2);
                            decimal a = Math.Round(((obj.TotalGratuity / GratuityInfo.NoOfDaysInMonth) * GratuityInfo.GratuityNoOfDaysInYear), 2); // NoOfDaysInMonth = 26,GratuityNoOfDaysInYear = 15
                            decimal b = Math.Round((obj.TotalMonth / 12), 2);
                            obj.Gratuity = Math.Round(a * b);
                        }
                        else
                        {
                            obj.TotalGratuity = 0;
                            obj.TotalMonth = 0;
                            obj.Gratuity = obj.GratuityAmount;
                        }
                    }
                    else if (obj.GratuityStatusID == 2)
                    {
                        if (obj.GratuityAmount == 0)
                        {
                            obj.TotalGratuity = Math.Round(obj.TotalBasicAllowance, 2);
                            decimal a = Math.Round(((obj.TotalGratuity / GratuityInfo.NoOfDaysInMonth) * GratuityInfo.GratuityNoOfDaysInYear), 2);
                            decimal b = Math.Round((obj.TotalMonth / 12), 2);
                            obj.Gratuity = Math.Round(a * b);
                        }
                        else
                        {
                            obj.TotalGratuity = 0;
                            obj.TotalMonth = 0;
                            obj.Gratuity = obj.GratuityAmount;
                        }
                    }
                    else
                    {
                        obj.TotalGratuity = 0;
                        obj.Gratuity = 0;
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //30-04-2020
        public GetLastGratuityInformation GetLastGratuityInformation()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastGratuityInformation";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetLastGratuityInformation obj = new GetLastGratuityInformation();
                while (dr.Read())
                {
                    obj.GratuityID = objBaseSqlManager.GetInt64(dr, "GratuityID");
                    obj.NoOfDaysInMonth = objBaseSqlManager.GetDecimal(dr, "NoOfDaysInMonth");
                    obj.GratuityNoOfDaysInYear = objBaseSqlManager.GetDecimal(dr, "GratuityNoOfDaysInYear");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        //29-04-2020
        public long AddPF(PF_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.PFID == 0)
                {
                    context.PF_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.PFID > 0)
                {
                    return Obj.PFID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PFListResponse> GetAllPFList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPFList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PFListResponse> objlst = new List<PFListResponse>();
                while (dr.Read())
                {
                    PFListResponse obj = new PFListResponse();
                    obj.PFID = objBaseSqlManager.GetInt64(dr, "PFID");
                    obj.HighestSlab = Math.Round((objBaseSqlManager.GetDecimal(dr, "HighestSlab")), 2);
                    obj.HighestPF = Math.Round((objBaseSqlManager.GetDecimal(dr, "HighestPF")), 2);
                    obj.PFPercentage = Math.Round((objBaseSqlManager.GetDecimal(dr, "PFPercentage")), 2);
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeletePF(long PFID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePF";
                cmdGet.Parameters.AddWithValue("@PFID", PFID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddESIC(ESIC_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ESICID == 0)
                {
                    context.ESIC_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ESICID > 0)
                {
                    return Obj.ESICID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ESICListResponse> GetAllESICList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllESICList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ESICListResponse> objlst = new List<ESICListResponse>();
                while (dr.Read())
                {
                    ESICListResponse obj = new ESICListResponse();
                    obj.ESICID = objBaseSqlManager.GetInt64(dr, "ESICID");
                    obj.EmployeeSlab = objBaseSqlManager.GetDecimal(dr, "EmployeeSlab");
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteESIC(long ESICID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteESIC";
                cmdGet.Parameters.AddWithValue("@ESICID", ESICID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddPT(PT_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.PTID == 0)
                {
                    context.PT_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.PTID > 0)
                {
                    return Obj.PTID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PTListResponse> GetAllPTList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPTList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PTListResponse> objlst = new List<PTListResponse>();
                while (dr.Read())
                {
                    PTListResponse obj = new PTListResponse();
                    obj.PTID = objBaseSqlManager.GetInt64(dr, "PTID");
                    obj.MonthID = objBaseSqlManager.GetInt64(dr, "MonthID");
                    obj.HighestSlab = Math.Round((objBaseSqlManager.GetDecimal(dr, "HighestSlab")), 2);
                    obj.HighestAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "HighestAmount")), 2);
                    obj.LowestSlab = Math.Round((objBaseSqlManager.GetDecimal(dr, "LowestSlab")), 2);
                    obj.LowestAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "LowestAmount")), 2);
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeletePT(long PTID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePT";
                cmdGet.Parameters.AddWithValue("@PTID", PTID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddMLWF(MLWF_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.MLWFID == 0)
                {
                    context.MLWF_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.MLWFID > 0)
                {
                    return Obj.MLWFID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<MLWFListResponse> GetAllMLWFList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllMLWFList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MLWFListResponse> objlst = new List<MLWFListResponse>();
                while (dr.Read())
                {
                    MLWFListResponse obj = new MLWFListResponse();
                    obj.MLWFID = objBaseSqlManager.GetInt64(dr, "MLWFID");
                    obj.MonthID = objBaseSqlManager.GetInt64(dr, "MonthID");
                    obj.HighestSlab = Math.Round((objBaseSqlManager.GetDecimal(dr, "HighestSlab")), 2);
                    obj.HighestAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "HighestAmount")), 2);
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteMLWF(long MLWFID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteMLWF";
                cmdGet.Parameters.AddWithValue("@MLWFID", MLWFID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddLeaveEncashment(LeaveEncashment_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.LeaveEncashmentID == 0)
                {
                    context.LeaveEncashment_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.LeaveEncashmentID > 0)
                {
                    return Obj.LeaveEncashmentID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<LeaveEncashmentMstListResponse> GetAllLeaveEncashmentList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllLeaveEncashmentList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LeaveEncashmentMstListResponse> objlst = new List<LeaveEncashmentMstListResponse>();
                while (dr.Read())
                {
                    LeaveEncashmentMstListResponse obj = new LeaveEncashmentMstListResponse();
                    obj.LeaveEncashmentID = objBaseSqlManager.GetInt64(dr, "LeaveEncashmentID");
                    obj.NoOfDaysLeaveEncashment = Math.Round((objBaseSqlManager.GetDecimal(dr, "NoOfDaysLeaveEncashment")), 2);
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteLeaveEncashment(long LeaveEncashmentID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteLeaveEncashment";
                cmdGet.Parameters.AddWithValue("@LeaveEncashmentID", LeaveEncashmentID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public long AddGratuity(Gratuity_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.GratuityID == 0)
                {
                    context.Gratuity_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.GratuityID > 0)
                {
                    return Obj.GratuityID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<GratuityMstListResponse> GetAllGratuityList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGratuityList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GratuityMstListResponse> objlst = new List<GratuityMstListResponse>();
                while (dr.Read())
                {
                    GratuityMstListResponse obj = new GratuityMstListResponse();
                    obj.GratuityID = objBaseSqlManager.GetInt64(dr, "GratuityID");
                    obj.NoOfDaysInMonth = Math.Round((objBaseSqlManager.GetDecimal(dr, "NoOfDaysInMonth")), 2);
                    obj.GratuityNoOfDaysInYear = Math.Round((objBaseSqlManager.GetDecimal(dr, "GratuityNoOfDaysInYear")), 2);
                    obj.Note = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteGratuity(long GratuityID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteGratuity";
                cmdGet.Parameters.AddWithValue("@GratuityID", GratuityID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public ClosingLeavesandClosingAdvance GetClosingLeavesandClosingAdvance(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetClosingLeavesandClosingAdvance";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ClosingLeavesandClosingAdvance obj = new ClosingLeavesandClosingAdvance();
                while (dr.Read())
                {
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.ClosingAdvance = objBaseSqlManager.GetDecimal(dr, "ClosingAdvance");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public long AddLeaveAndAdvanceApplication(LeaveApplication_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.LeaveApplicationID == 0)
                {
                    context.LeaveApplication_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.LeaveApplicationID > 0)
                {
                    return Obj.LeaveApplicationID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<LeaveApplicationListResponse> GetLeaveAndAdvanceApplicationList(DateTime? FromDate, DateTime? ToDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLeaveAndAdvanceApplicationList";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LeaveApplicationListResponse> objlst = new List<LeaveApplicationListResponse>();
                while (dr.Read())
                {
                    LeaveApplicationListResponse obj = new LeaveApplicationListResponse();
                    obj.LeaveApplicationID = objBaseSqlManager.GetInt64(dr, "LeaveApplicationID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.ApplicationDate = objBaseSqlManager.GetDateTime(dr, "ApplicationDate");
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");

                    if (obj.FromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FromDatestr = obj.FromDate.ToString("MM/dd/yyyy");
                        obj.lblFromDate = obj.FromDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.FromDatestr = "";
                        obj.lblFromDate = "";
                    }


                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");

                    if (obj.ToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ToDatestr = obj.ToDate.ToString("MM/dd/yyyy");
                        obj.lblToDate = obj.ToDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ToDatestr = "";
                        obj.lblToDate = "";
                    }


                    obj.NoofDays = objBaseSqlManager.GetDecimal(dr, "NoofDays");
                    obj.Reason = objBaseSqlManager.GetTextValue(dr, "Reason");
                    obj.GoingTo = objBaseSqlManager.GetTextValue(dr, "GoingTo");
                    obj.AdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "AdvanceAmount"), 2);
                    obj.AdvanceReason = objBaseSqlManager.GetTextValue(dr, "AdvanceReason");
                    obj.DeductionPerMonthAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "DeductionPerMonthAmount"), 2);
                    obj.LeaveStatusID = objBaseSqlManager.GetInt64(dr, "LeaveStatusID");
                    if (obj.LeaveStatusID == 1)
                    {
                        obj.LeaveStatus = "Pending";
                    }
                    else if (obj.LeaveStatusID == 2)
                    {
                        obj.LeaveStatus = "Approved";
                    }
                    else if (obj.LeaveStatusID == 3)
                    {
                        obj.LeaveStatus = "Reject";
                    }
                    obj.ApprovalFromDate = objBaseSqlManager.GetDateTime(dr, "ApprovalFromDate");
                    if (obj.ApprovalFromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalFromDatestr = obj.ApprovalFromDate.ToString("MM/dd/yyyy");
                        obj.lblApprovalFromDate = obj.ApprovalFromDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalFromDatestr = "";
                        obj.lblApprovalFromDate = "";
                    }
                    obj.ApprovalToDate = objBaseSqlManager.GetDateTime(dr, "ApprovalToDate");
                    if (obj.ApprovalToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalToDatestr = obj.ApprovalToDate.ToString("MM/dd/yyyy");
                        obj.lblApprovalToDate = obj.ApprovalToDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalToDatestr = "";
                        obj.lblApprovalToDate = "";
                    }
                    obj.ApprovalAdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ApprovalAdvanceAmount"), 2);
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public LeaveApplicationListResponse GetDataForLeaveApplicationPrint(long LeaveApplicationID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDataForLeaveApplicationPrint";
                cmdGet.Parameters.AddWithValue("@LeaveApplicationID", LeaveApplicationID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                LeaveApplicationListResponse obj = new LeaveApplicationListResponse();
                while (dr.Read())
                {
                    obj.LeaveApplicationID = objBaseSqlManager.GetInt64(dr, "LeaveApplicationID");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.PrimaryPin = objBaseSqlManager.GetInt64(dr, "PrimaryPin");
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.ESIC = objBaseSqlManager.GetTextValue(dr, "ESIC");
                    obj.GodownAddress1 = objBaseSqlManager.GetTextValue(dr, "GodownAddress1");
                    obj.GodownAddress2 = objBaseSqlManager.GetTextValue(dr, "GodownAddress2");
                    obj.Place = objBaseSqlManager.GetTextValue(dr, "Place");
                    obj.Pincode = objBaseSqlManager.GetTextValue(dr, "Pincode");
                    obj.State = objBaseSqlManager.GetTextValue(dr, "State");
                    obj.ApplicationDate = objBaseSqlManager.GetDateTime(dr, "ApplicationDate");
                    if (obj.ApplicationDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApplicationDatestr = obj.ApplicationDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApplicationDatestr = "";
                    }
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    if (obj.FromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FromDatestr = obj.FromDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.FromDatestr = "";
                    }
                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");
                    if (obj.ToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ToDatestr = obj.ToDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ToDatestr = "";
                    }
                    obj.Reason = objBaseSqlManager.GetTextValue(dr, "Reason");
                    obj.GoingTo = objBaseSqlManager.GetTextValue(dr, "GoingTo");
                    obj.AdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "AdvanceAmount"), 2);
                    obj.AdvanceReason = objBaseSqlManager.GetTextValue(dr, "AdvanceReason");
                    obj.DeductionPerMonthAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "DeductionPerMonthAmount"), 2);

                    obj.ApprovalFromDate = objBaseSqlManager.GetDateTime(dr, "ApprovalFromDate");
                    if (obj.ApprovalFromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalFromDatestr = obj.ApprovalFromDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalFromDatestr = "";
                    }
                    obj.ApprovalToDate = objBaseSqlManager.GetDateTime(dr, "ApprovalToDate");
                    if (obj.ApprovalToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalToDatestr = obj.ApprovalToDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalToDatestr = "";
                    }
                    obj.ApprovalAdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ApprovalAdvanceAmount"), 2);

                    obj.LeaveStatusID = objBaseSqlManager.GetInt64(dr, "LeaveStatusID");
                    if (obj.LeaveStatusID == 1)
                    {
                        obj.LeaveStatus = "Pending";
                    }
                    else if (obj.LeaveStatusID == 2)
                    {
                        obj.LeaveStatus = "Approved";
                    }
                    else if (obj.LeaveStatusID == 3)
                    {
                        obj.LeaveStatus = "Rejected";
                    }
                    obj.ApprovedBy = objBaseSqlManager.GetTextValue(dr, "ApprovedBy");
                    obj.ApprovalDate = objBaseSqlManager.GetDateTime(dr, "ApprovalDate");
                    if (obj.ApprovalDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalDatestr = obj.ApprovalDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalDatestr = "";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public MonthList GetLastTwelveMonthDataForLeavePrint(int Month, int Year, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastTwelveMonthDataForLeavePrint";
                cmdGet.Parameters.AddWithValue("@MonthID", Month);
                cmdGet.Parameters.AddWithValue("@YearID", Year);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LastTwelveMonthDataForLeavePrint> objlst = new List<LastTwelveMonthDataForLeavePrint>();
                while (dr.Read())
                {
                    LastTwelveMonthDataForLeavePrint obj = new LastTwelveMonthDataForLeavePrint();
                    obj.MonthID = objBaseSqlManager.GetInt64(dr, "MonthID");
                    obj.YearID = objBaseSqlManager.GetInt64(dr, "YearID");
                    obj.Leaves = objBaseSqlManager.GetDecimal(dr, "Leaves");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();

                MonthList objmonth = new MonthList();
                decimal GrandTotal = 0;
                foreach (var item in objlst)
                {
                    switch (item.MonthID)
                    {

                        case 1:
                            objmonth.JAN = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.JAN;
                            objmonth.Total = GrandTotal;
                            break;

                        case 2:
                            objmonth.FEB = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.FEB;
                            objmonth.Total = GrandTotal;
                            break;

                        case 3:
                            objmonth.MAR = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.MAR;
                            objmonth.Total = GrandTotal;
                            break;
                        case 4:
                            objmonth.APR = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.APR;
                            objmonth.Total = GrandTotal;
                            break;
                        case 5:
                            objmonth.MAY = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.MAY;
                            objmonth.Total = GrandTotal;
                            break;
                        case 6:
                            objmonth.JUN = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.JUN;
                            objmonth.Total = GrandTotal;
                            break;
                        case 7:
                            objmonth.JULY = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.JULY;
                            objmonth.Total = GrandTotal;
                            break;
                        case 8:
                            objmonth.AUG = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.AUG;
                            objmonth.Total = GrandTotal;
                            break;
                        case 9:
                            objmonth.SEP = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.SEP;
                            objmonth.Total = GrandTotal;
                            break;
                        case 10:
                            objmonth.OCT = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.OCT;
                            objmonth.Total = GrandTotal;
                            break;
                        case 11:
                            objmonth.NOV = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.NOV;
                            objmonth.Total = GrandTotal;
                            break;
                        case 12:
                            objmonth.DEC = item.Leaves;
                            GrandTotal = GrandTotal + objmonth.DEC;
                            objmonth.Total = GrandTotal;
                            break;

                        default:
                            Console.WriteLine("No match found");
                            break;
                    }
                }
                return objmonth;
            }
        }

        public ClosingAdvanceMonthList GetLastTwelveMonthClosingAdvanceDataForLeavePrint(DateTime? ToDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastTwelveMonthClosingAdvanceDataForLeavePrint";
                //cmdGet.Parameters.AddWithValue("@MonthID", Month);
                //cmdGet.Parameters.AddWithValue("@YearID", Year);
                cmdGet.Parameters.AddWithValue("@Date", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LastTwelveMonthClosingAdvanceDataForLeavePrint> objlst = new List<LastTwelveMonthClosingAdvanceDataForLeavePrint>();
                while (dr.Read())
                {
                    LastTwelveMonthClosingAdvanceDataForLeavePrint obj = new LastTwelveMonthClosingAdvanceDataForLeavePrint();
                    obj.MonthID = objBaseSqlManager.GetInt64(dr, "MonthID");
                    //obj.YearID = objBaseSqlManager.GetInt64(dr, "YearID");
                    //obj.ClosingAdvance = Math.Round(objBaseSqlManager.GetDecimal(dr, "ClosingAdvance"), 2);
                    obj.ApprovalAdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ApprovalAdvanceAmount"), 2);
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();

                ClosingAdvanceMonthList objmonth = new ClosingAdvanceMonthList();
                decimal GrandTotal = 0;
                foreach (var item in objlst)
                {
                    switch (item.MonthID)
                    {

                        case 1:
                            objmonth.JAN = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.JAN;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;

                        case 2:
                            objmonth.FEB = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.FEB;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;

                        case 3:
                            objmonth.MAR = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.MAR;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 4:
                            objmonth.APR = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.APR;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 5:
                            objmonth.MAY = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.MAY;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 6:
                            objmonth.JUN = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.JUN;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 7:
                            objmonth.JULY = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.JULY;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 8:
                            objmonth.AUG = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.AUG;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 9:
                            objmonth.SEP = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.SEP;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 10:
                            objmonth.OCT = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.OCT;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 11:
                            objmonth.NOV = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.NOV;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;
                        case 12:
                            objmonth.DEC = item.ApprovalAdvanceAmount;
                            GrandTotal = GrandTotal + objmonth.DEC;
                            objmonth.TotalClosingAdvance = GrandTotal;
                            break;

                        default:
                            Console.WriteLine("No match found");
                            break;
                    }
                }
                return objmonth;
            }
        }

        public bool UpdateApprovalLeave(ApprovalLeave data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateApprovalLeave";
                    cmdGet.Parameters.AddWithValue("@LeaveApplicationID", data.LeaveApplicationID);
                    cmdGet.Parameters.AddWithValue("@ApprovalFromDate", data.ApprovalFromDate);
                    cmdGet.Parameters.AddWithValue("@ApprovalToDate", data.ApprovalToDate);
                    cmdGet.Parameters.AddWithValue("@ApprovalAdvanceAmount", data.ApprovalAdvanceAmount);
                    cmdGet.Parameters.AddWithValue("@LeaveStatusID", data.LeaveStatusID);
                    cmdGet.Parameters.AddWithValue("@ApprovalBy", UserID);
                    cmdGet.Parameters.AddWithValue("@ApprovalDate", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public List<ActiveEmployeeCode> GetActiveEmployeeCodeFromUserMaster(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetActiveEmployeeCodeFromUserMaster";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ActiveEmployeeCode> objlst = new List<ActiveEmployeeCode>();
                while (dr.Read())
                {
                    ActiveEmployeeCode obj = new ActiveEmployeeCode();
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 11-05-2020
        public List<CashCounterListResponse> GetCashCounterReportList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCashCounterReportList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CashCounterListResponse> objlst = new List<CashCounterListResponse>();
                decimal CashTotal = 0;
                decimal ChequeTotal = 0;
                decimal CardTotal = 0;
                decimal SignTotal = 0;
                decimal OnlineTotal = 0;
                decimal AdjustAmountTotal = 0;
                while (dr.Read())
                {
                    CashCounterListResponse obj = new CashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    CashTotal += obj.Cash;
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    ChequeTotal += obj.Cheque;
                    obj.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    CardTotal += obj.Card;
                    obj.Sign = Math.Round((objBaseSqlManager.GetDecimal(dr, "Sign")), 2);
                    SignTotal += obj.Sign;
                    obj.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    OnlineTotal += obj.Online;
                    obj.AdjustAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmount")), 2);
                    AdjustAmountTotal += obj.AdjustAmount;
                    obj.CashTotal = CashTotal;
                    obj.ChequeTotal = ChequeTotal;
                    obj.CardTotal = CardTotal;
                    obj.SignTotal = SignTotal;
                    obj.OnlineTotal = OnlineTotal;
                    obj.AdjustAmountTotal = AdjustAmountTotal;
                    obj.Remarks = objBaseSqlManager.GetTextValue(dr, "Remarks");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    obj.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    obj.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                    if (obj.ChequeDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ChequeDate1 = "";
                    }
                    else
                    {
                        obj.ChequeDate1 = obj.ChequeDate.ToString("dd/MM/yyyy");
                    }
                    obj.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFCCode");
                    obj.BankNameForCard = objBaseSqlManager.GetTextValue(dr, "BankNameForCard");
                    obj.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    obj.BankNameForOnline = objBaseSqlManager.GetTextValue(dr, "BankNameForOnline");
                    obj.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    obj.OnlinePaymentDate = objBaseSqlManager.GetDateTime(dr, "OnlinePaymentDate");
                    if (obj.OnlinePaymentDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OnlinePaymentDate1 = "";
                    }
                    else
                    {
                        obj.OnlinePaymentDate1 = obj.OnlinePaymentDate.ToString("dd/MM/yyyy");
                    }

                    obj.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");

                    if (obj.IsDelivered == true)
                    {
                        obj.IsDeliveredstr = "Delivered";
                    }
                    else
                    {
                        obj.IsDeliveredstr = "Not Delivered";
                    }
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 7 July 2020 Piyush Limbani For May 2020
        public List<VirakiEmployeeAsCustomer> GetAllVirakiEmployeeAsCustomerName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVirakiEmployeeAsCustomerName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VirakiEmployeeAsCustomer> lstProduct = new List<VirakiEmployeeAsCustomer>();
                while (dr.Read())
                {
                    VirakiEmployeeAsCustomer obj = new VirakiEmployeeAsCustomer();
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    lstProduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstProduct;
            }
        }

        public List<LeaveApprovalListResponse> GetLeaveApprovalList(DateTime? FromDate, DateTime? ToDate, long EmployeeCode, long GodownID, long RoleID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLeaveApprovalList";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@RoleID", RoleID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<LeaveApprovalListResponse> objlst = new List<LeaveApprovalListResponse>();
                while (dr.Read())
                {
                    LeaveApprovalListResponse obj = new LeaveApprovalListResponse();
                    obj.LeaveApplicationID = objBaseSqlManager.GetInt64(dr, "LeaveApplicationID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.Designation = objBaseSqlManager.GetTextValue(dr, "Designation");
                    obj.ApplicationDate = objBaseSqlManager.GetDateTime(dr, "ApplicationDate");
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    if (obj.FromDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FromDatestr = "";
                    }
                    else
                    {
                        obj.FromDatestr = obj.FromDate.ToString("dd/MM/yyyy");
                    }
                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");
                    if (obj.ToDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ToDatestr = "";
                    }
                    else
                    {
                        obj.ToDatestr = obj.ToDate.ToString("dd/MM/yyyy");
                    }
                    obj.NoofDays = objBaseSqlManager.GetDecimal(dr, "NoofDays");
                    obj.Reason = objBaseSqlManager.GetTextValue(dr, "Reason");
                    obj.GoingTo = objBaseSqlManager.GetTextValue(dr, "GoingTo");
                    obj.AdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "AdvanceAmount"), 2);
                    obj.AdvanceReason = objBaseSqlManager.GetTextValue(dr, "AdvanceReason");
                    obj.DeductionPerMonthAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "DeductionPerMonthAmount"), 2);
                    obj.LeaveStatusID = objBaseSqlManager.GetInt64(dr, "LeaveStatusID");
                    if (obj.LeaveStatusID == 1)
                    {
                        obj.LeaveStatus = "Pending";
                    }
                    else if (obj.LeaveStatusID == 2)
                    {
                        obj.LeaveStatus = "Approved";
                    }
                    else if (obj.LeaveStatusID == 3)
                    {
                        obj.LeaveStatus = "Reject";
                    }
                    obj.ApprovalFromDate = objBaseSqlManager.GetDateTime(dr, "ApprovalFromDate");
                    if (obj.ApprovalFromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalFromDatestr = obj.ApprovalFromDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalFromDatestr = "";
                    }
                    obj.ApprovalToDate = objBaseSqlManager.GetDateTime(dr, "ApprovalToDate");
                    if (obj.ApprovalToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalToDatestr = obj.ApprovalToDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalToDatestr = "";
                    }
                    obj.ApprovalAdvanceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ApprovalAdvanceAmount"), 2);
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 15 July 2020 Piyush Limbani
        public List<VehicleListForAllowanceResponse> GetVehicleListForAllowance(long MonthID, long YearID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleListForAllowance";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleListForAllowanceResponse> objlst = new List<VehicleListForAllowanceResponse>();
                while (dr.Read())
                {
                    VehicleListForAllowanceResponse obj = new VehicleListForAllowanceResponse();
                    obj.AssignedDate = objBaseSqlManager.GetDateTime(dr, "AssignedDate");
                    //obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    if (obj.AssignedDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.AssignedDatestr = obj.AssignedDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.AssignedDatestr = "";
                    }

                    VehiclePersonDetails VehiclePersonDetails = null;
                    VehiclePersonDetails = GetVehiclePersonDetails(obj.AssignedDate, EmployeeCode);
                    if (VehiclePersonDetails.VehicleNo != null && VehiclePersonDetails.TempoNo != null)
                    {
                        obj.VehicleNo = VehiclePersonDetails.VehicleNo;
                        obj.DeliveryPerson1 = VehiclePersonDetails.DeliveryPerson1;
                        obj.DeliveryPerson2 = VehiclePersonDetails.DeliveryPerson2;
                        obj.DeliveryPerson3 = VehiclePersonDetails.DeliveryPerson3;
                        obj.DeliveryPerson4 = VehiclePersonDetails.DeliveryPerson4;
                        obj.AreaName = VehiclePersonDetails.AreaName;
                        obj.TempoNo = VehiclePersonDetails.TempoNo;
                    }
                    else
                    {
                        VehiclePersonDetails VehiclePersonDetailsRet = null;
                        VehiclePersonDetailsRet = GetVehiclePersonDetailsForRetail(obj.AssignedDate, EmployeeCode);
                        obj.VehicleNo = VehiclePersonDetailsRet.VehicleNo;
                        obj.DeliveryPerson1 = VehiclePersonDetailsRet.DeliveryPerson1;
                        obj.DeliveryPerson2 = VehiclePersonDetailsRet.DeliveryPerson2;
                        obj.DeliveryPerson3 = VehiclePersonDetailsRet.DeliveryPerson3;
                        obj.DeliveryPerson4 = VehiclePersonDetailsRet.DeliveryPerson4;
                        obj.AreaName = VehiclePersonDetailsRet.AreaName;
                        obj.TempoNo = VehiclePersonDetailsRet.TempoNo;
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public VehiclePersonDetails GetVehiclePersonDetails(DateTime AssignedDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehiclePersonDetails";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                VehiclePersonDetails obj = new VehiclePersonDetails();
                while (dr.Read())
                {
                    obj.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson4");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public VehiclePersonDetails GetVehiclePersonDetailsForRetail(DateTime AssignedDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehiclePersonDetailsForRetail";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                VehiclePersonDetails obj = new VehiclePersonDetails();
                while (dr.Read())
                {
                    obj.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson4");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 24 July 2020 Piyush Limbani
        public CalculateEmployeeSalary GetCalculateEmployeeSalary(long SalarySheetID, long EmployeeCode, int MonthID, int YearID, int TotalDays, int TotalMonthDay, int TotalPresent, int TotalAvailedLeaves)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCalculateEmployeeSalaryBySalarySheetID";
                cmdGet.Parameters.AddWithValue("@SalarySheetID", SalarySheetID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                CalculateEmployeeSalary obj = new CalculateEmployeeSalary();
                while (dr.Read())
                {
                    GetAllowanceDetail AllowanceDetail = null;
                    long YearForAlloawance = 0;
                    if (MonthID == 1 || MonthID == 2 || MonthID == 3)
                    {
                        YearForAlloawance = YearID - 1;
                        AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, EmployeeCode);
                    }
                    else
                    {
                        YearForAlloawance = YearID;
                        AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, EmployeeCode);
                    }

                    //SalaryExistModel existsalary = CheckSalaryExist(MonthID, YearID, EmployeeCode);

                    obj.TotalDays = TotalDays;
                    obj.TotalMonthDay = TotalMonthDay;

                    obj.OpeningLeaves = objBaseSqlManager.GetDecimal(dr, "OpeningLeaves");
                    obj.EarnedLeaves = objBaseSqlManager.GetDecimal(dr, "EarnedLeaves");
                    obj.TotalLeaves = obj.OpeningLeaves + obj.EarnedLeaves;


                    obj.BasicAllowance = AllowanceDetail.BasicAllowance;
                    obj.HouseRentAllowance = AllowanceDetail.HouseRentAllowance;
                    obj.TotalBasic = AllowanceDetail.TotalBasic;
                    // 8 July 2020 Piyush Limbani
                    decimal EarnedBasicWagesAct = Math.Round(((obj.BasicAllowance / obj.TotalMonthDay) * obj.TotalDays), 2);
                    string EarnedBasicWages = GetRoundoffValue(EarnedBasicWagesAct);
                    obj.EarnedBasicWages = Convert.ToDecimal(EarnedBasicWages);
                    decimal EarnedHouseRentAllowanceAct = Math.Round(((obj.HouseRentAllowance / obj.TotalMonthDay) * obj.TotalDays), 2);
                    string EarnedHouseRentAllowance = GetRoundoffValue(EarnedHouseRentAllowanceAct);
                    obj.EarnedHouseRentAllowance = Convert.ToDecimal(EarnedHouseRentAllowance);
                    // 8 July 2020 Piyush Limbani
                    obj.TotalEarnedWages = Math.Round((obj.EarnedBasicWages + obj.EarnedHouseRentAllowance), 2);
                    obj.ConveyancePerMonth = AllowanceDetail.Conveyance;
                    obj.ConveyancePerDay = AllowanceDetail.ConveyancePerDay;

                    //decimal TotalConveyancePerDay = (obj.ConveyancePerDay) * (Convert.ToDecimal(obj.Present) - obj.AvailedLeaves);
                    decimal TotalConveyancePerDay = (obj.ConveyancePerDay) * (Convert.ToDecimal(TotalPresent) - TotalAvailedLeaves);
                    //TotalPresent,TotalAvaild

                    decimal TotalConveyancePerMonth = Math.Round(((obj.ConveyancePerMonth / obj.TotalMonthDay) * obj.TotalDays));
                    decimal Conveyance = TotalConveyancePerMonth + TotalConveyancePerDay;
                    obj.Conveyance = Conveyance;
                    if (SalarySheetID == 0)
                    {
                        obj.AdditionalCityAllowance = 0;
                        obj.TotalCityAllowance = 0;
                        obj.AdditionalVehicleAllowance = 0;
                        obj.TotalVehicleAllowance = 0;
                        obj.AdditionalConveyance = 0;
                        obj.TotalConveyance = 0;
                        obj.AdditionalPerformanceAllowance = 0;
                        obj.TotalPerformanceAllowance = 0;

                        // 31 May 2023 Dhruvik
                        obj.AdditionalCityAllowanceMinutes = 0;
                        // 31 May 2023 Dhruvik
                    }
                    else
                    {
                        obj.AdditionalCityAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowance");
                        obj.TotalCityAllowance = objBaseSqlManager.GetDecimal(dr, "TotalCityAllowance");
                        obj.AdditionalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalVehicleAllowance");
                        obj.TotalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "TotalVehicleAllowance");
                        obj.AdditionalConveyance = objBaseSqlManager.GetDecimal(dr, "AdditionalConveyance");
                        obj.TotalConveyance = objBaseSqlManager.GetDecimal(dr, "TotalConveyance");
                        obj.AdditionalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalPerformanceAllowance");
                        obj.TotalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "TotalPerformanceAllowance");

                        // 31 May 2023 Dhruvik
                        obj.AdditionalCityAllowanceMinutes = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowanceMinutes");
                        // 31 May 2023 Dhruvik
                    }
                    obj.PerformanceAllowanceStatusID = AllowanceDetail.PerformanceAllowanceStatusID;
                    obj.CityAllowanceStatusID = AllowanceDetail.CityAllowanceStatusID;
                    obj.PFStatusID = AllowanceDetail.PFStatusID;
                    obj.ESICStatusID = AllowanceDetail.ESICStatusID;
                    GetOTDetails Minutes = GetTotalOTMinutes(MonthID, YearID, EmployeeCode);
                    obj.CityAllowanceMinutes = Minutes.TotalMinutes;
                    obj.CityAllowanceHours = Minutes.TotalHrs;

                    if (obj.CityAllowanceMinutes > 0)
                    {
                        if (obj.CityAllowanceStatusID == 1)  //1 =  applicable old
                        {
                            decimal CityAllowancePerHr = Math.Round(((((obj.TotalBasic / obj.TotalMonthDay) / 8) / 60) * 2), 2);
                            //decimal CityAllowance = CityAllowancePerHr * obj.CityAllowanceMinutes;
                            
                            // 31 May 2023 Dhruvik
                            decimal CityAllowance = CityAllowancePerHr * (obj.CityAllowanceMinutes + obj.AdditionalCityAllowanceMinutes);
                            // 31 May 2023 Dhruvik

                            // 9 July 2020 Piyush Limbani
                            decimal CityAllowanceAct = Math.Round(CityAllowance, 2);
                            string CityAllowanceNew = GetRoundoffValue(CityAllowanceAct);
                            obj.CityAllowance = Convert.ToDecimal(CityAllowanceNew);
                            // 9 July 2020 Piyush Limbani
                        }
                        else if (obj.CityAllowanceStatusID == 2)  //2 = applicable new
                        {
                            decimal CityAllowancePerHr = Math.Round(((((obj.BasicAllowance / obj.TotalMonthDay) / 8) / 60) * 2), 2);
                            //decimal CityAllowance = CityAllowancePerHr * obj.CityAllowanceMinutes;

                            // 31 May 2023 Dhruvik
                            decimal CityAllowance = CityAllowancePerHr * (obj.CityAllowanceMinutes + obj.AdditionalCityAllowanceMinutes);
                            // 31 May 2023 Dhruvik

                            // 9 July 2020 Piyush Limbani
                            decimal CityAllowanceAct = Math.Round(CityAllowance, 2);
                            string CityAllowanceNew = GetRoundoffValue(CityAllowanceAct);
                            obj.CityAllowance = Convert.ToDecimal(CityAllowanceNew);
                            // 9 July 2020 Piyush Limbani
                        }
                        else //3 = NA
                        {
                            obj.CityAllowance = 0;
                        }
                    }
                    else
                    {
                        obj.CityAllowance = 0;
                    }

                    if (obj.PerformanceAllowanceStatusID == 1) //1 =  applicable old
                    {
                        obj.PerformanceAllowanceAct = AllowanceDetail.PerformanceAllowance;
                        obj.PerformanceAllowance = Math.Round(obj.PerformanceAllowanceAct);
                    }
                    else if (obj.PerformanceAllowanceStatusID == 2)  //2 = applicable new
                    {
                        obj.PerformanceAllowanceAct = AllowanceDetail.PerformanceAllowance;

                        //decimal PerformanceAllowance = ((obj.PerformanceAllowanceAct) / (obj.TotalMonthDay)) * ((obj.TotalDays) - (obj.AvailedLeaves));
                        decimal PerformanceAllowance = ((obj.PerformanceAllowanceAct) / (obj.TotalMonthDay)) * ((obj.TotalDays) - (TotalAvailedLeaves));
                        // TotalAvaild        

                        obj.PerformanceAllowance = Math.Round(PerformanceAllowance);
                    }
                    else
                    {
                        obj.PerformanceAllowance = 0;
                    }
                    long VehicleNoOfDaysCount = GetVehicleNoOfDaysCount(MonthID, YearID, EmployeeCode);
                    obj.VehicleAllowance = AllowanceDetail.VehicleAllowance * VehicleNoOfDaysCount;
                    //obj.VehicleAllowance = 200;
                    decimal GrossWagesPayable = obj.TotalEarnedWages + obj.CityAllowance + obj.AdditionalCityAllowance + obj.VehicleAllowance + obj.AdditionalVehicleAllowance + obj.Conveyance + obj.AdditionalConveyance + obj.PerformanceAllowance + obj.AdditionalPerformanceAllowance;
                    // decimal GrossWagesPayable = Convert.ToDecimal(95766.90);
                    obj.GrossWagesPayable = Math.Round(GrossWagesPayable, 2);

                    //30-04-2020
                    GetLastPFInformation PFInfo = GetLastPFInformation();
                    obj.HighestSlab = PFInfo.HighestSlab;
                    obj.HighestPF = PFInfo.HighestPF;
                    obj.PFPercentage = PFInfo.PFPercentage;
                    if (obj.PFStatusID == 1) //1 =  applicable old
                    {
                        if (obj.TotalEarnedWages > obj.HighestSlab) // 15000
                        {
                            obj.PF = obj.HighestPF; // 1800
                        }
                        else
                        {
                            decimal PF = Math.Round(((obj.TotalEarnedWages * obj.PFPercentage) / 100), 2);// 12%
                            obj.PF = Math.Round(PF, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    else if (obj.PFStatusID == 2)  //2 = applicable new
                    {
                        if (obj.EarnedBasicWages > obj.HighestSlab)
                        {
                            obj.PF = obj.HighestPF;
                        }
                        else
                        {
                            decimal PF = Math.Round(((obj.EarnedBasicWages * obj.PFPercentage) / 100), 2);
                            obj.PF = Math.Round(PF, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    else
                    {
                        obj.PF = 0;
                    }
                    //30-04-2020
                    GetLastESICInformation ESICInfo = GetLastESICInformation();
                    obj.EmployeeSlab = ESICInfo.EmployeeSlab;
                    if (obj.ESICStatusID == 1)  //1 =  applicable old
                    {
                        obj.ESIC = 0;
                    }
                    else if (obj.ESICStatusID == 2)//2 = applicable new
                    {
                        obj.ESIC = Math.Ceiling(((obj.GrossWagesPayable) * Convert.ToDecimal(obj.EmployeeSlab))); // 0.0075
                    }
                    else
                    {
                        obj.ESIC = 0;
                    }
                    //30-04-2020
                    GetLastPTInformation PTInfo = GetLastPTInformationByMonth(MonthID);
                    obj.PTHighestSlab = PTInfo.PTHighestSlab;
                    obj.PTHighestAmount = PTInfo.PTHighestAmount;
                    obj.PTLowestSlab = PTInfo.PTLowestSlab;
                    obj.PTLowestAmount = PTInfo.PTLowestAmount;
                    if (obj.GrossWagesPayable > obj.PTHighestSlab) // HighestSlab = 10000
                    {
                        obj.PT = obj.PTHighestAmount;
                    }
                    else if (obj.GrossWagesPayable > obj.PTLowestSlab) // LowestSlab = 5000
                    {
                        obj.PT = obj.PTLowestAmount;
                    }
                    else
                    {
                        obj.PT = 0;
                    }
                    //30-04-2020
                    GetLastMLWFInformation MLWFInfo = GetLastMLWFInformationByMonth(MonthID);
                    obj.MLWFHighestSlab = MLWFInfo.MLWFHighestSlab;
                    obj.MLWFHighestAmount = MLWFInfo.MLWFHighestAmount;
                    obj.MLWFMonthID = MLWFInfo.MLWFMonthID;
                    if (MonthID == obj.MLWFMonthID)
                    {
                        if (obj.GrossWagesPayable > obj.MLWFHighestSlab) // HighestSlab = 5000
                        {
                            obj.MLWF = obj.MLWFHighestAmount;
                        }
                        else
                        {
                            obj.MLWF = 0;
                        }
                    }
                    else
                    {
                        obj.MLWF = 0;
                    }
                    decimal TotalDeductions = obj.PF + obj.PT + obj.ESIC + obj.MLWF;
                    obj.TotalDeductions = Math.Round(TotalDeductions, 2);

                    // 10 July 2020 Piyush Limbani
                    decimal NetWagesPaid = Math.Round((obj.GrossWagesPayable - obj.TotalDeductions), 2);
                    obj.NetWagesPaid = Math.Round(NetWagesPaid, 0, MidpointRounding.AwayFromZero);
                    // 10 July 2020 Piyush Limbani
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 28 July 2020 Piyush Limbani
        public List<AddPayment> GetEmployeeSalaryDetailForPayment(int MonthID, int YearID, long GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeSalaryDetailForPayment";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AddPayment> objlst = new List<AddPayment>();
                while (dr.Read())
                {
                    AddPayment obj = new AddPayment();
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.NetWagesPaid = objBaseSqlManager.GetDecimal(dr, "NetWagesPaid");
                    obj.Deductions = objBaseSqlManager.GetDecimal(dr, "Deductions");
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                    obj.Goods = objBaseSqlManager.GetDecimal(dr, "Goods");
                    obj.AnyOtherDeductions1 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions1");
                    obj.AnyOtherDeductions2 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions2");
                    obj.FinalTotalDeductions = obj.Deductions + obj.TDS + obj.Goods + obj.AnyOtherDeductions1 + obj.AnyOtherDeductions2;
                    obj.NetWagesToPay = obj.NetWagesPaid - obj.FinalTotalDeductions;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 28 July 2020 Piyush Limbani
        public SalaryPaymentExistModel CheckPaymentSalaryExist(int MonthID, int YearID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckPaymentSalaryExist";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                SalaryPaymentExistModel obj = new SalaryPaymentExistModel();
                while (dr.Read())
                {
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.PaymentDate = objBaseSqlManager.GetDateTime(dr, "PaymentDate");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 28 July 2020 Piyush Limbani
        public long AddSalaryPayment(SalaryPayment_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.PaymentID == 0)
                {
                    context.SalaryPayment_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.SalarySheetID > 0)
                {
                    return Obj.PaymentID;
                }
                else
                {
                    return 0;
                }
            }
        }

        // 29 July 2020 Piyush Limbani
        public List<PaidPaymentList> GetPaidPaymentList(DateTime PaymentDate, long? GodownID, long? BankID, long? EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPaidPaymentList";
                if (PaymentDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", PaymentDate);
                }
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PaidPaymentList> objlst = new List<PaidPaymentList>();
                while (dr.Read())
                {
                    PaidPaymentList objPayment = new PaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.PaymentDate = objBaseSqlManager.GetDateTime(dr, "PaymentDate");
                    objPayment.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    objPayment.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objPayment.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    objPayment.Bank_Name = objBaseSqlManager.GetTextValue(dr, "BankNameViraki");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    //objPayment.Payment_Type = "Online";
                    objPayment.Payment_Ref_No = "";
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "NetWagesToPay");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.Beneficiary_Name = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Name");
                    objPayment.Beneficiary_Bank = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Bank");

                    // 10 Feb 2021 Piyush Limbani
                    if (objPayment.Beneficiary_Bank != "")
                    {
                        string strbank = objPayment.Beneficiary_Bank.Substring(0, 5);
                        //if (objPayment.ByOnline == true)
                        //{
                        if (strbank == "Kotak" || strbank == "KOTAK")
                        {
                            objPayment.Payment_Type = "IFT";
                        }
                        else
                        {
                            objPayment.Payment_Type = "NEFT";
                        }
                        //}
                        //else if (objPayment.ByCheque == true)
                        //{
                        //    objPayment.Payment_Type = "CHEQUE";
                        //}
                        //else if (objPayment.ByCard == true)
                        //{
                        //    objPayment.Payment_Type = "CARD";
                        //}
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }
                    // 10 Feb 2021 Piyush Limbani

                    objPayment.Beneficiary_Branch_IFSC_Code = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Branch_IFSC_Code");
                    objPayment.Beneficiary_Acc_No = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Acc_No");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Beneficiary_Email = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Email");
                    objPayment.Beneficiary_Mobile = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Mobile");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";

                    objPayment.Bill_No = "";
                    objPayment.Bill_Date = "";
                    objPayment.Net_Wages_Paid = objBaseSqlManager.GetDecimal(dr, "NetWagesPaid");
                    objPayment.Deductions = objBaseSqlManager.GetDecimal(dr, "FinalTotalDeductions");

                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetWagesToPay");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 29 July 2020 Piyush Limbani
        public List<SalaryPaymentListResponse> GetSalaryPaymentListByMonthAndYear(int MonthID, int YearID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSalaryPaymentListByMonthAndYear";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalaryPaymentListResponse> objlst = new List<SalaryPaymentListResponse>();
                while (dr.Read())
                {
                    SalaryPaymentListResponse obj = new SalaryPaymentListResponse();
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    obj.PaymentDate = PaymentDate1;
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.Beneficiary_Name = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Name");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    obj.Beneficiary_Bank = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Bank");
                    obj.Beneficiary_Acc_No = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Acc_No");
                    obj.Beneficiary_Branch_IFSC_Code = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Branch_IFSC_Code");
                    obj.Beneficiary_Mobile = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Mobile");
                    obj.Beneficiary_Email = objBaseSqlManager.GetTextValue(dr, "Beneficiary_Email");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 31 July 2020 Piyush Limbani
        public bool UpdateSalaryPayment(SalaryPayment data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateSalaryPayment";
                    cmdGet.Parameters.AddWithValue("@PaymentID", data.PaymentID);
                    cmdGet.Parameters.AddWithValue("@PaymentDate", data.PaymentDate);
                    cmdGet.Parameters.AddWithValue("@BankID", data.BankID);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", UserID);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }


        // 07 Aug 2020 Piyush Limbani
        public GetEmployeeAttandanceDetail GetEmployeeAttandanceDetailBySalarySheetID(int MonthID, int YearID, long EmployeeCode, long SalarySheetID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeAttandanceDetailBySalarySheetID";
                cmdGet.Parameters.AddWithValue("@SalarySheetID", SalarySheetID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetEmployeeAttandanceDetail obj = new GetEmployeeAttandanceDetail();
                while (dr.Read())
                {
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.Present = objBaseSqlManager.GetInt32(dr, "Present");
                    obj.AdditionalPresent = objBaseSqlManager.GetInt32(dr, "AdditionalPresent");
                    obj.TotalPresent = objBaseSqlManager.GetInt32(dr, "TotalPresent");
                    obj.TotalMonthSunday = objBaseSqlManager.GetInt32(dr, "Sunday");
                    obj.AdditionalSunday = objBaseSqlManager.GetInt32(dr, "AdditionalSunday");
                    obj.TotalSunday = objBaseSqlManager.GetInt32(dr, "TotalSunday");
                    obj.Holiday = objBaseSqlManager.GetInt32(dr, "Holiday");
                    obj.AdditionalHoliday = objBaseSqlManager.GetInt32(dr, "AdditionalHoliday");
                    obj.TotalHoliday = objBaseSqlManager.GetInt32(dr, "TotalHoliday");
                    obj.Absent = objBaseSqlManager.GetInt32(dr, "Absent");
                    obj.AdditionalAbsent = objBaseSqlManager.GetInt32(dr, "AdditionalAbsent");
                    obj.TotalAbsent = objBaseSqlManager.GetInt32(dr, "TotalAbsent");
                    obj.TotalDays = objBaseSqlManager.GetInt32(dr, "TotalDays");
                    obj.TotalMonthDay = objBaseSqlManager.GetInt32(dr, "TotalDaysIntheMonth");
                    obj.BasicAllowance = objBaseSqlManager.GetDecimal(dr, "BasicAllowance");
                    obj.HouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "HouseRentAllowance");
                    obj.TotalBasic = objBaseSqlManager.GetDecimal(dr, "TotalBasic");
                    obj.EarnedBasicWages = objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages");
                    obj.EarnedHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "EarnedHouseRentAllowance");
                    obj.TotalEarnedWages = objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages");
                    obj.CityAllowanceMinutes = objBaseSqlManager.GetDecimal(dr, "CityAllowanceMinutes");

                    // 31 May 2023 Dhruvik
                    obj.AdditionalCityAllowanceMinutes = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowanceMinutes");
                    // 31 May 2023 Dhruvik

                    obj.CityAllowanceHours = objBaseSqlManager.GetDecimal(dr, "CityAllowanceHours");
                    obj.CityAllowance = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    obj.AdditionalCityAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowance");
                    obj.TotalCityAllowance = objBaseSqlManager.GetDecimal(dr, "TotalCityAllowance");
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.AdditionalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalVehicleAllowance");
                    obj.TotalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "TotalVehicleAllowance");
                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.AdditionalConveyance = objBaseSqlManager.GetDecimal(dr, "AdditionalConveyance");
                    obj.TotalConveyance = objBaseSqlManager.GetDecimal(dr, "TotalConveyance");
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.AdditionalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalPerformanceAllowance");
                    obj.TotalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "TotalPerformanceAllowance");
                    obj.GrossWagesPayable = objBaseSqlManager.GetDecimal(dr, "GrossWagesPayable");

                    // 05 Jan 2021 Piyush Limbani     
                    GetLastPTInformation PTInfo = GetLastPTInformationByMonth(MonthID);
                    obj.PTHighestSlab = PTInfo.PTHighestSlab;
                    obj.PTHighestAmount = PTInfo.PTHighestAmount;
                    obj.PTLowestSlab = PTInfo.PTLowestSlab;
                    obj.PTLowestAmount = PTInfo.PTLowestAmount;
                    // 05 Jan 2021 Piyush Limbani       

                    //GetAllowanceDetail AllowanceDetail = null;
                    //long YearForAlloawance = 0;
                    //if (MonthID == 1 || MonthID == 2 || MonthID == 3)
                    //{
                    //    YearForAlloawance = YearID - 1;
                    //    AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, EmployeeCode);
                    //}
                    //else
                    //{
                    //    YearForAlloawance = YearID;
                    //    AllowanceDetail = GetTopOneAllowanceDetailByEmployeeCode(YearForAlloawance, EmployeeCode);
                    //}
                    //obj.PerformanceAllowanceStatusID = AllowanceDetail.PerformanceAllowanceStatusID;
                    //obj.CityAllowanceStatusID = AllowanceDetail.CityAllowanceStatusID;
                    //obj.PFStatusID = AllowanceDetail.PFStatusID;
                    //obj.ESICStatusID = AllowanceDetail.ESICStatusID;


                    obj.PF = objBaseSqlManager.GetDecimal(dr, "PF");
                    obj.ESIC = objBaseSqlManager.GetDecimal(dr, "ESIC");
                    obj.PT = objBaseSqlManager.GetDecimal(dr, "PT");
                    obj.MLWF = objBaseSqlManager.GetDecimal(dr, "MLWF");
                    obj.TotalDeductions = objBaseSqlManager.GetDecimal(dr, "TotalDeductions");
                    obj.NetWagesPaid = objBaseSqlManager.GetDecimal(dr, "NetWagesPaid");
                    obj.OpeningLeaves = objBaseSqlManager.GetDecimal(dr, "OpeningLeaves");
                    obj.EarnedLeaves = objBaseSqlManager.GetDecimal(dr, "EarnedLeaves");
                    obj.TotalLeaves = obj.OpeningLeaves + obj.EarnedLeaves;
                    obj.AvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AvailedLeaves");
                    obj.AdditionalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalAvailedLeaves");
                    obj.TotalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "TotalAvailedLeaves");
                    obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.AdditionalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalClosingLeaves");
                    obj.TotalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "TotalClosingLeaves");
                    obj.OpeningAdvance = objBaseSqlManager.GetDecimal(dr, "OpeningAdvance");
                    obj.Addition = objBaseSqlManager.GetDecimal(dr, "Addition");
                    obj.Deductions = objBaseSqlManager.GetDecimal(dr, "Deductions");
                    obj.ClosingAdvance = objBaseSqlManager.GetDecimal(dr, "ClosingAdvance");
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                    obj.Goods = objBaseSqlManager.GetDecimal(dr, "Goods");
                    obj.AnyOtherDeductions1 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions1");
                    obj.AnyOtherDeductions2 = objBaseSqlManager.GetDecimal(dr, "AnyOtherDeductions2");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.CreatedOnstr = string.Format("{0:G}", obj.CreatedOn);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 11 Aug 2020 Piyush Limbani
        public AllowanceDetailCount GetAllowanceDetailCount(string StartDate, string EndDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllowanceDetailCount";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AllowanceDetailCount obj = new AllowanceDetailCount();
                while (dr.Read())
                {
                    obj.RecordCount = objBaseSqlManager.GetInt64(dr, "RecordCount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<EmployeeCodeList> GetAllEmployeeCodeListFromAllowanceMaster()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllEmployeeCodeListFromAllowanceMaster";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EmployeeCodeList> objlst = new List<EmployeeCodeList>();
                while (dr.Read())
                {
                    EmployeeCodeList obj = new EmployeeCodeList();
                    obj.EmployeeCode = objBaseSqlManager.GetInt32(dr, "EmployeeCode");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public ForwardAllownceDetailList GetLastAllowanceDetailForForwardNextYear(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastAllowanceDetailForForwardNextYear";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ForwardAllownceDetailList obj = new ForwardAllownceDetailList();
                while (dr.Read())
                {
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.TotalHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "TotalHouseRentAllowance");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.ConveyancePerDay = objBaseSqlManager.GetDecimal(dr, "ConveyancePerDay");
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.PerformanceAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "PerformanceAllowanceStatusID");
                    obj.CityAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "CityAllowanceStatusID");
                    obj.PFStatusID = objBaseSqlManager.GetInt64(dr, "PFStatusID");
                    obj.ESICStatusID = objBaseSqlManager.GetInt64(dr, "ESICStatusID");
                    obj.BonusPercentage = objBaseSqlManager.GetDecimal(dr, "BonusPercentage");
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.BonusStatusID = objBaseSqlManager.GetInt64(dr, "BonusStatusID");
                    obj.LeaveEnhancementPercentage = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementPercentage");
                    obj.LeaveEnhancementAmount = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementAmount");
                    obj.LeaveEnhancementStatusID = objBaseSqlManager.GetInt64(dr, "LeaveEnhancementStatusID");
                    obj.GratuityPercentage = objBaseSqlManager.GetDecimal(dr, "GratuityPercentage");
                    obj.GratuityAmount = objBaseSqlManager.GetDecimal(dr, "GratuityAmount");
                    obj.GratuityStatusID = objBaseSqlManager.GetInt64(dr, "GratuityStatusID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }
        // 11 Aug 2020 Piyush Limbani

        // 13 Aug 2020 Piyush Limbani
        public List<ModelSalarySlip> GetDataForSalarySlipPrint(int MonthID, int YearID, long? EmployeeCode, int? GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDataForSalarySlipPrint";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ModelSalarySlip> objlst = new List<ModelSalarySlip>();
                while (dr.Read())
                {
                    string NumberToWord = "";
                    ModelSalarySlip obj = new ModelSalarySlip();
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    obj.PanNo = objBaseSqlManager.GetTextValue(dr, "PanNo");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    obj.AadharNumber = objBaseSqlManager.GetInt64(dr, "AadharNumber");
                    obj.Designation = objBaseSqlManager.GetTextValue(dr, "Designation");
                    obj.ESICNumber = objBaseSqlManager.GetTextValue(dr, "ESICNumber");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.PFNumber = objBaseSqlManager.GetTextValue(dr, "PFNumber");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    if (objBaseSqlManager.GetTextValue(dr, "UANNumber") == "0")
                    {
                        obj.UANNumber = "";
                    }
                    else
                    {
                        obj.UANNumber = objBaseSqlManager.GetTextValue(dr, "UANNumber");
                    }
                    obj.EarnedBasicWages = Math.Round(objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages"), 2);
                    obj.EarnedHouseRentAllowance = Math.Round(objBaseSqlManager.GetDecimal(dr, "EarnedHouseRentAllowance"), 2);
                    obj.TotalEarnedWages = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages"), 2);
                    decimal PerformanceAllowanceAct = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    obj.AdditionalPerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalPerformanceAllowance");
                    obj.PerformanceAllowance = Math.Round((PerformanceAllowanceAct + obj.AdditionalPerformanceAllowance), 2);
                    decimal ConveyanceAct = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    obj.AdditionalConveyance = objBaseSqlManager.GetDecimal(dr, "AdditionalConveyance");
                    obj.Conveyance = Math.Round((ConveyanceAct + obj.AdditionalConveyance), 2);
                    decimal CityAllowanceAct = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    obj.AdditionalCityAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalCityAllowance");
                    obj.CityAllowance = Math.Round((CityAllowanceAct + obj.AdditionalCityAllowance), 2);
                    decimal VehicleAllowanceAct = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    obj.AdditionalVehicleAllowance = objBaseSqlManager.GetDecimal(dr, "AdditionalVehicleAllowance");
                    obj.VehicleAllowance = Math.Round((VehicleAllowanceAct + obj.AdditionalVehicleAllowance), 2);
                    obj.PF = Math.Round(objBaseSqlManager.GetDecimal(dr, "PF"), 2);
                    obj.ESIC = Math.Round(objBaseSqlManager.GetDecimal(dr, "ESIC"), 2);
                    obj.PT = Math.Round(objBaseSqlManager.GetDecimal(dr, "PT"), 2);
                    obj.MLWF = Math.Round(objBaseSqlManager.GetDecimal(dr, "MLWF"), 2);
                    obj.TDS = Math.Round(objBaseSqlManager.GetDecimal(dr, "TDS"), 2);
                    obj.Goods = Math.Round(objBaseSqlManager.GetDecimal(dr, "Goods"), 2);
                    obj.TotalEarnings = Math.Round((obj.TotalEarnedWages + obj.PerformanceAllowance + obj.Conveyance + obj.CityAllowance + obj.VehicleAllowance), 2);
                    obj.TotalDecuctions = Math.Round((obj.PF + obj.ESIC + obj.PT + obj.MLWF + obj.TDS + obj.Goods), 2);
                    decimal NetPay = Math.Round((obj.TotalEarnings - obj.TotalDecuctions), 2);
                    obj.NetPay = Math.Round(NetPay, 0, MidpointRounding.AwayFromZero);
                    int number = Convert.ToInt32(obj.NetPay);
                    NumberToWord = NumberToWords(number);
                    obj.AmountInWords = NumberToWord + "  " + "Only/-";
                    obj.OpeningLeaves = objBaseSqlManager.GetDecimal(dr, "OpeningLeaves");
                    obj.EarnedLeaves = objBaseSqlManager.GetDecimal(dr, "EarnedLeaves");
                    obj.AvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AvailedLeaves");
                    obj.AdditionalAvailedLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalAvailedLeaves");
                    obj.TotalAvailedLeaves = obj.AvailedLeaves + obj.AdditionalAvailedLeaves;
                    obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.AdditionalClosingLeaves = objBaseSqlManager.GetDecimal(dr, "AdditionalClosingLeaves");
                    obj.TotalClosingLeaves = obj.ClosingLeaves + obj.AdditionalClosingLeaves;
                    obj.OpeningAdvance = Math.Round(objBaseSqlManager.GetDecimal(dr, "OpeningAdvance"), 2);
                    obj.Addition = Math.Round(objBaseSqlManager.GetDecimal(dr, "Addition"), 2);
                    obj.Deductions = Math.Round(objBaseSqlManager.GetDecimal(dr, "Deductions"), 2);
                    obj.ClosingAdvance = Math.Round(objBaseSqlManager.GetDecimal(dr, "ClosingAdvance"), 2);
                    if (MonthID == 1)
                    {
                        obj.Month = "January";
                    }
                    else if (MonthID == 2)
                    {
                        obj.Month = "February";
                    }
                    else if (MonthID == 3)
                    {
                        obj.Month = "March";
                    }
                    else if (MonthID == 4)
                    {
                        obj.Month = "April";
                    }
                    else if (MonthID == 5)
                    {
                        obj.Month = "May";
                    }
                    else if (MonthID == 6)
                    {
                        obj.Month = "June";
                    }
                    else if (MonthID == 7)
                    {
                        obj.Month = "July";
                    }
                    else if (MonthID == 8)
                    {
                        obj.Month = "August";
                    }
                    else if (MonthID == 9)
                    {
                        obj.Month = "September";
                    }
                    else if (MonthID == 10)
                    {
                        obj.Month = "October";
                    }
                    else if (MonthID == 11)
                    {
                        obj.Month = "November";
                    }
                    else if (MonthID == 12)
                    {
                        obj.Month = "December";
                    }
                    obj.Year = objBaseSqlManager.GetInt32(dr, "Year");
                    obj.PaymentDate = objBaseSqlManager.GetDateTime(dr, "PaymentDate");
                    if (obj.PaymentDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PaymentDatestr = obj.PaymentDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.PaymentDatestr = "";
                    }
                    obj.BirthDate = objBaseSqlManager.GetDateTime(dr, "BirthDate");
                    if (obj.BirthDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.BirthDatestr = obj.BirthDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.BirthDatestr = "";
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";
            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));
            string words = "";

            //if ((number / 1000000000) > 0)
            //{
            //    words += NumberToWords(number / 1000000000) + " billion  ";
            //    number %= 1000000000;
            //}

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " Lakhs ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }
            return words;
        }





        // 04 Sep. 2020 Piyush Limbani
        public List<MonthlyAttendanceList> GetMonthlyAttendanceList(int MonthID, int YearID, int? GodownID, long? EmployeeCode)
        {
            decimal sumOTMinutes = 0;
            string sumTotalOTHrs = "";
            int sumPresent = 0;
            int sumAbsent = 0;
            int sumSunday = 0;
            int sumHoliday = 0;
            long sumVehicleNoOfDaysCount = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetMonthlyAttendanceList";
                cmdGet.Parameters.AddWithValue("@MonthID", MonthID);
                cmdGet.Parameters.AddWithValue("@YearID", YearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<MonthlyAttendanceList> objlst = new List<MonthlyAttendanceList>();
                while (dr.Read())
                {
                    MonthlyAttendanceList obj = new MonthlyAttendanceList();
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.Present = objBaseSqlManager.GetInt32(dr, "Present");
                    obj.Absent = objBaseSqlManager.GetInt32(dr, "Absent");
                    obj.Sunday = objBaseSqlManager.GetInt32(dr, "Sunday");
                    obj.Holiday = objBaseSqlManager.GetInt32(dr, "Holiday");
                    GetOTDetails Minutes = GetTotalOTMinutes(MonthID, YearID, obj.EmployeeCode);
                    obj.OTMinutes = Minutes.TotalMinutes;
                    obj.OTHours = GetMinutesToHours(Convert.ToInt64(obj.OTMinutes));
                    obj.VehicleNoOfDaysCount = GetVehicleNoOfDaysCount(MonthID, YearID, obj.EmployeeCode);

                    sumPresent = sumPresent + Convert.ToInt32(obj.Present);
                    sumAbsent = sumAbsent + Convert.ToInt32(obj.Absent);
                    sumSunday = sumSunday + Convert.ToInt32(obj.Sunday);
                    sumHoliday = sumHoliday + Convert.ToInt32(obj.Holiday);

                    sumOTMinutes = sumOTMinutes + obj.OTMinutes;
                    obj.sumTotalOTHrs = GetMinutesToHours(Convert.ToInt64(sumOTMinutes));
                    sumTotalOTHrs = obj.sumTotalOTHrs;
                    sumVehicleNoOfDaysCount = sumVehicleNoOfDaysCount + Convert.ToInt32(obj.VehicleNoOfDaysCount);
                    
                    objlst.Add(obj);
                }

                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].sumPresent = sumPresent;
                    objlst[i].sumAbsent = sumAbsent;
                    objlst[i].sumSunday = sumSunday;
                    objlst[i].sumHoliday = sumHoliday;
                    objlst[i].sumTotalOTHrs = sumTotalOTHrs;
                    objlst[i].sumVehicleNoOfDaysCount = sumVehicleNoOfDaysCount;
                }

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string GetMinutesToHours(long Minutes)
        {
            string OTHours = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetMinutesToHours";
                cmdGet.Parameters.AddWithValue("@Minutes", Minutes);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    OTHours = objBaseSqlManager.GetTextValue(dr, "OTHours");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return OTHours;
            }
        }



        // 23 Dec 2020 Piyush Limbani
        public List<EmployeeNameResponse> GetEmployeeByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EmployeeNameResponse> objlst = new List<EmployeeNameResponse>();
                while (dr.Read())
                {
                    EmployeeNameResponse obj = new EmployeeNameResponse();
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetDateofLeaving GetEmployeeDateofLeavingByEmployeeCode(long GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeDateofLeavingByEmployeeCode";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetDateofLeaving obj = new GetDateofLeaving();
                while (dr.Read())
                {
                    obj.DateofLeaving = objBaseSqlManager.GetDateTime(dr, "DateofLeaving");
                    if (obj.DateofLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateofLeavingstr = obj.DateofLeaving.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateofLeavingstr = "";
                    }
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GratuityListResponse GetCalculateGratuity(int LeavingYear, int GodownID, long EmployeeCode, DateTime DateOfLeaving)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCalculateGratuity";
                cmdGet.Parameters.AddWithValue("@DateOfLeaving", DateOfLeaving);
                cmdGet.Parameters.AddWithValue("@LeavingYear", LeavingYear);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GratuityListResponse obj = new GratuityListResponse();
                while (dr.Read())
                {
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    else
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                    }
                    obj.DateOfLeaving = DateOfLeaving;
                    if (obj.DateOfLeaving == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = "";
                    }
                    else
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("dd/MM/yyyy");
                    }
                    if (obj.DateOfJoining == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.TotalMonth = 0;
                    }
                    else
                    {
                        obj.TotalMonth = objBaseSqlManager.GetInt64(dr, "TotalMonth");
                    }
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.GratuityStatusID = objBaseSqlManager.GetInt32(dr, "GratuityStatusID");
                    obj.GratuityAmount = objBaseSqlManager.GetDecimal(dr, "GratuityAmount");
                    obj.GratuityPercentage = objBaseSqlManager.GetDecimal(dr, "GratuityPercentage");

                    //30-04-2020
                    GetLastGratuityInformation GratuityInfo = GetLastGratuityInformation();

                    if (obj.GratuityStatusID == 1)
                    {
                        if (obj.GratuityAmount == 0)
                        {
                            obj.TotalGratuity = Math.Round(obj.GrandTotalWages, 2);
                            decimal a = Math.Round(((obj.TotalGratuity / GratuityInfo.NoOfDaysInMonth) * GratuityInfo.GratuityNoOfDaysInYear), 2); // NoOfDaysInMonth = 26,GratuityNoOfDaysInYear = 15
                            decimal b = Math.Round((obj.TotalMonth / 12), 2);
                            obj.Gratuity = Math.Round(a * b);
                        }
                        else
                        {
                            obj.TotalGratuity = 0;
                            obj.TotalMonth = 0;
                            obj.Gratuity = obj.GratuityAmount;
                        }
                    }
                    else if (obj.GratuityStatusID == 2)
                    {
                        if (obj.GratuityAmount == 0)
                        {
                            obj.TotalGratuity = Math.Round(obj.TotalBasicAllowance, 2);
                            decimal a = Math.Round(((obj.TotalGratuity / GratuityInfo.NoOfDaysInMonth) * GratuityInfo.GratuityNoOfDaysInYear), 2);
                            decimal b = Math.Round((obj.TotalMonth / 12), 2);
                            obj.Gratuity = Math.Round(a * b);
                        }
                        else
                        {
                            obj.TotalGratuity = 0;
                            obj.TotalMonth = 0;
                            obj.Gratuity = obj.GratuityAmount;
                        }
                    }
                    else
                    {
                        obj.TotalGratuity = 0;
                        obj.Gratuity = 0;
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public ModelCalculateBonus GetCalculateBonus(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCalculateBonus";
                cmdGet.Parameters.AddWithValue("@FromMonthID", FromMonthID);
                cmdGet.Parameters.AddWithValue("@FromYearID", FromYearID);
                cmdGet.Parameters.AddWithValue("@ToMonthID", ToMonthID);
                cmdGet.Parameters.AddWithValue("@ToYearID", ToYearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ModelCalculateBonus obj = new ModelCalculateBonus();
                while (dr.Read())
                {
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.TotalEarnedWages = objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages");
                    obj.EarnedBasicWages = objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages");
                    obj.BonusStatusID = objBaseSqlManager.GetInt32(dr, "BonusStatusID");
                    if (obj.BonusStatusID == 1)
                    {
                        obj.TotalBonus = obj.TotalEarnedWages;
                    }
                    else if (obj.BonusStatusID == 2)
                    {
                        obj.TotalBonus = obj.EarnedBasicWages;
                    }
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.BonusPercentage = objBaseSqlManager.GetDecimal(dr, "BonusPercentage");
                    if (obj.BonusAmount != 0)
                    {
                        obj.ActBonusAmount = obj.BonusAmount;
                    }
                    else
                    {
                        obj.ActBonusAmount = Math.Round(((obj.TotalBonus * obj.BonusPercentage) / 100));
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public ModelCalculateLeaveEncashment GetCalculateLeaveEncashment(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, int GodownID, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCalculateLeaveEncashment";
                cmdGet.Parameters.AddWithValue("@FromMonthID", FromMonthID);
                cmdGet.Parameters.AddWithValue("@FromYearID", FromYearID);
                cmdGet.Parameters.AddWithValue("@ToMonthID", ToMonthID);
                cmdGet.Parameters.AddWithValue("@ToYearID", ToYearID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ModelCalculateLeaveEncashment obj = new ModelCalculateLeaveEncashment();
                while (dr.Read())
                {
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.LeaveEnhancementStatusID = objBaseSqlManager.GetInt32(dr, "LeaveEnhancementStatusID");
                    obj.LeaveEnhancementAmount = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementAmount");
                    obj.LeaveEnhancementPercentage = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementPercentage");
                    decimal ClosingLeaves = GetClosingLeavesBySalarySheetID(obj.SalarySheetID);
                    decimal NoOfDaysLeaveEncashment = GetLastLeaveEncashmentNoOfDays();
                    obj.ClosingLeaves = ClosingLeaves;
                    if (obj.LeaveEnhancementStatusID == 1)
                    {
                        if (obj.LeaveEnhancementAmount == 0)
                        {
                            obj.TotalLeaveEnhancement = Math.Round(obj.GrandTotalWages, 2);
                            obj.LeaveEncashment = Math.Round(((obj.TotalLeaveEnhancement / NoOfDaysLeaveEncashment) * obj.ClosingLeaves)); //NoOfDaysLeaveEncashment = 30
                        }
                        else
                        {
                            obj.TotalLeaveEnhancement = 0;
                            obj.ClosingLeaves = 0;
                            obj.LeaveEncashment = obj.LeaveEnhancementAmount;
                        }
                    }
                    else if (obj.LeaveEnhancementStatusID == 2)
                    {
                        if (obj.LeaveEnhancementAmount == 0)
                        {
                            obj.TotalLeaveEnhancement = Math.Round(obj.TotalBasicAllowance, 2);
                            obj.LeaveEncashment = Math.Round(((obj.TotalLeaveEnhancement / NoOfDaysLeaveEncashment) * obj.ClosingLeaves));  //NoOfDaysLeaveEncashment = 30
                        }
                        else
                        {
                            obj.TotalLeaveEnhancement = 0;
                            obj.ClosingLeaves = 0;
                            obj.LeaveEncashment = obj.LeaveEnhancementAmount;
                        }
                    }
                    else
                    {
                        obj.TotalLeaveEnhancement = 0;
                        obj.LeaveEncashment = 0;
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }


        public long AddGratuityandHisab(Gratuity_Hisab_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.Gratuity_Hisab_ID == 0)
                {
                    context.Gratuity_Hisab_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.Gratuity_Hisab_ID > 0)
                {
                    return Obj.Gratuity_Hisab_ID;
                }
                else
                {
                    return 0;
                }
            }
        }

        //public bool DeActiveUserByEmployeeCode(long EmployeeCode)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "DeActiveUserByEmployeeCode";
        //    cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
        //    cmdGet.Parameters.AddWithValue("@IsDelete", true);
        //    object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //    objBaseSqlManager.ForceCloseConnection();
        //    return true;
        //}

        public List<Gratuity_HisabListResponse> GetAllGratuity_HisabList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllGratuity_HisabList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<Gratuity_HisabListResponse> objlst = new List<Gratuity_HisabListResponse>();
                while (dr.Read())
                {
                    Gratuity_HisabListResponse obj = new Gratuity_HisabListResponse();
                    obj.Gratuity_Hisab_ID = objBaseSqlManager.GetInt64(dr, "Gratuity_Hisab_ID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfLeavingstr = "";
                    }
                    obj.LastDrawnSalary = objBaseSqlManager.GetDecimal(dr, "LastDrawnSalary");
                    obj.TotalMonth = objBaseSqlManager.GetInt32(dr, "TotalMonth");
                    obj.Gratuity = objBaseSqlManager.GetDecimal(dr, "Gratuity");
                    obj.AdditionalGratuity = objBaseSqlManager.GetDecimal(dr, "AdditionalGratuity");
                    obj.TotalGratuity = objBaseSqlManager.GetDecimal(dr, "TotalGratuity");
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    if (obj.FromDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.FromDatestr = obj.FromDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.FromDatestr = "";
                    }
                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");
                    if (obj.ToDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ToDatestr = obj.ToDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ToDatestr = "";
                    }
                    obj.TotalEarnedSalary = objBaseSqlManager.GetDecimal(dr, "TotalEarnedSalary");

                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.AdditionalBonusAmount = objBaseSqlManager.GetDecimal(dr, "AdditionalBonusAmount");
                    obj.TotalBonusAmount = objBaseSqlManager.GetDecimal(dr, "TotalBonusAmount");
                    obj.LastDrawnSalaryLeaveEnhancement = objBaseSqlManager.GetDecimal(dr, "LastDrawnSalaryLeaveEnhancement");
                    obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.LeaveEncashment = objBaseSqlManager.GetDecimal(dr, "LeaveEncashment");
                    obj.AdditionalLeaveEncashment = objBaseSqlManager.GetDecimal(dr, "AdditionalLeaveEncashment");
                    obj.TotalLeaveEncashment = objBaseSqlManager.GetDecimal(dr, "TotalLeaveEncashment");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.WitnessOneID = objBaseSqlManager.GetInt64(dr, "WitnessOneID");
                    obj.WitnessTwoID = objBaseSqlManager.GetInt64(dr, "WitnessTwoID");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public ModelEmployeeDetail GetEmployeeDetailByEmployeeCodeForResignation(long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEmployeeDetailByEmployeeCodeForResignation";
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ModelEmployeeDetail obj = new ModelEmployeeDetail();
                while (dr.Read())
                {
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    obj.Address = obj.PrimaryAddress + ',' + obj.AreaName + ',' + obj.PinCode;
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.DateofLeaving = objBaseSqlManager.GetDateTime(dr, "DateofLeaving");
                    if (obj.DateofLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateofLeavingstr = obj.DateofLeaving.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateofLeavingstr = "";
                    }

                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 25 Dec 2020 Piyush Limbani
        public long AddResignationLetter(Resignation_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ResignationID == 0)
                {
                    context.Resignation_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ResignationID > 0)
                {
                    return Obj.ResignationID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public PrintResignationLetter GetDataForResignationLetterPrint(long ResignationID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDataForResignationLetterPrint";
                cmdGet.Parameters.AddWithValue("@ResignationID", ResignationID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                PrintResignationLetter obj = new PrintResignationLetter();
                while (dr.Read())
                {
                    obj.ResignationID = objBaseSqlManager.GetInt64(dr, "ResignationID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfLeavingstr = "";
                    }
                    obj.DateOfApplication = objBaseSqlManager.GetDateTime(dr, "DateOfApplication");
                    if (obj.DateOfApplication != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfApplicationstr = obj.DateOfApplication.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfApplicationstr = "";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<ResignationLetterResponse> GetAllResignationLetterList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllResignationLetterList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ResignationLetterResponse> objlst = new List<ResignationLetterResponse>();
                while (dr.Read())
                {
                    ResignationLetterResponse obj = new ResignationLetterResponse();
                    obj.ResignationID = objBaseSqlManager.GetInt64(dr, "ResignationID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    obj.FullAddress = obj.PrimaryAddress + "," + obj.AreaName + "," + obj.PinCode;
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfLeavingstr = "";
                    }
                    obj.DateOfApplication = objBaseSqlManager.GetDateTime(dr, "DateOfApplication");
                    if (obj.DateOfApplication != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfApplicationstr = obj.DateOfApplication.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfApplicationstr = "";
                    }
                    obj.Status = objBaseSqlManager.GetInt32(dr, "Status");
                    if (obj.Status == 1)
                    {
                        obj.Statusstr = "Pending";
                    }
                    else if (obj.Status == 2)
                    {
                        obj.Statusstr = "Approved";
                    }
                    else if (obj.Status == 3)
                    {
                        obj.Statusstr = "Rejected";
                    }
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ResignationAcceptanceLetterListResponse> GetResignationAcceptanceLetterList(DateTime? FromDate, DateTime? ToDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetResignationAcceptanceLetterList";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ResignationAcceptanceLetterListResponse> objlst = new List<ResignationAcceptanceLetterListResponse>();
                while (dr.Read())
                {
                    ResignationAcceptanceLetterListResponse obj = new ResignationAcceptanceLetterListResponse();
                    obj.ResignationID = objBaseSqlManager.GetInt64(dr, "ResignationID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.DateOfApplication = objBaseSqlManager.GetDateTime(dr, "DateOfApplication");
                    if (obj.DateOfApplication != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfApplicationstr = obj.DateOfApplication.ToString("dd/MM/yyyy");
                        // obj.DateOfApplicationstr = obj.DateOfApplication.ToString("MM/dd/yyyy");
                        //obj.lblFromDate = obj.FromDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfApplicationstr = "";
                        //obj.lblFromDate = "";
                    }
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfJoiningstr = obj.DateOfJoining.ToString("dd/MM/yyyy");
                        //obj.DateOfJoiningstr = obj.DateOfJoining.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfJoiningstr = "";
                    }
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("dd/MM/yyyy");
                        // obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.DateOfLeavingstr = "";
                    }
                    obj.Status = objBaseSqlManager.GetInt32(dr, "Status");
                    if (obj.Status == 1)
                    {
                        obj.Statusstr = "Pending";
                    }
                    else if (obj.Status == 2)
                    {
                        obj.Statusstr = "Approved";
                    }
                    else if (obj.Status == 3)
                    {
                        obj.Statusstr = "Reject";
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateResignationApprovalStatus(long ResignationID, int Status, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateResignationApprovalStatus";
                    cmdGet.Parameters.AddWithValue("@ResignationID", ResignationID);
                    cmdGet.Parameters.AddWithValue("@Status", Status);
                    cmdGet.Parameters.AddWithValue("@ApprovalBy", UserID);
                    cmdGet.Parameters.AddWithValue("@ApprovalDate", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public PrintResignationAcceptanceLetter GetDataForResignationAcceptanceLetterPrint(long ResignationID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDataForResignationAcceptanceLetterPrint";
                cmdGet.Parameters.AddWithValue("@ResignationID", ResignationID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                PrintResignationAcceptanceLetter obj = new PrintResignationAcceptanceLetter();
                while (dr.Read())
                {
                    obj.ResignationID = objBaseSqlManager.GetInt64(dr, "ResignationID");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.ApprovalDate = objBaseSqlManager.GetDateTime(dr, "ApprovalDate");
                    if (obj.ApprovalDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ApprovalDatestr = obj.ApprovalDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.ApprovalDatestr = "";
                    }
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfLeavingstr = obj.DateOfLeaving.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfLeavingstr = "";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 26 Dec 2020 Piyush Limbani
        public List<PavatiListResponse> GetAllPavatiList(DateTime FromDate, DateTime ToDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPavatiList";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PavatiListResponse> objlst = new List<PavatiListResponse>();
                while (dr.Read())
                {
                    PavatiListResponse obj = new PavatiListResponse();
                    obj.Gratuity_Hisab_ID = objBaseSqlManager.GetInt64(dr, "Gratuity_Hisab_ID");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.LastDrawnSalary = objBaseSqlManager.GetDecimal(dr, "LastDrawnSalary");
                    obj.TotalMonth = objBaseSqlManager.GetInt32(dr, "TotalMonth");

                    //obj.TotalService = objBaseSqlManager.GetTextValue(dr, "TotalService");

                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014") && obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        string ServiceTime = GetServiceTimeEng(obj.DateOfJoining, obj.DateOfLeaving);
                        obj.TotalService = ServiceTime;
                    }
                    else
                    {
                        obj.TotalService = "";
                    }

                    obj.Gratuity = objBaseSqlManager.GetDecimal(dr, "Gratuity");
                    obj.AdditionalGratuity = objBaseSqlManager.GetDecimal(dr, "AdditionalGratuity");
                    obj.TotalGratuity = objBaseSqlManager.GetDecimal(dr, "TotalGratuity");
                    obj.TotalEarnedSalary = objBaseSqlManager.GetDecimal(dr, "TotalEarnedSalary");
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.AdditionalBonusAmount = objBaseSqlManager.GetDecimal(dr, "AdditionalBonusAmount");
                    obj.TotalBonusAmount = objBaseSqlManager.GetDecimal(dr, "TotalBonusAmount");
                    obj.LastDrawnSalaryLeaveEnhancement = objBaseSqlManager.GetDecimal(dr, "LastDrawnSalaryLeaveEnhancement");
                    obj.ClosingLeaves = objBaseSqlManager.GetDecimal(dr, "ClosingLeaves");
                    obj.LeaveEncashment = objBaseSqlManager.GetDecimal(dr, "LeaveEncashment");
                    obj.AdditionalLeaveEncashment = objBaseSqlManager.GetDecimal(dr, "AdditionalLeaveEncashment");
                    obj.TotalLeaveEncashment = objBaseSqlManager.GetDecimal(dr, "TotalLeaveEncashment");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.Advance = objBaseSqlManager.GetDecimal(dr, "Advance");
                    obj.NetAmount = (obj.GrandTotalAmount - obj.Advance);
                    obj.Status = objBaseSqlManager.GetInt32(dr, "Status");

                    obj.WitnessOneID = objBaseSqlManager.GetInt64(dr, "WitnessOneID");
                    obj.WitnessTwoID = objBaseSqlManager.GetInt64(dr, "WitnessTwoID");


                    if (obj.Status == 1)
                    {
                        obj.Statusstr = "Pending";
                    }
                    else if (obj.Status == 2)
                    {
                        obj.Statusstr = "Approved";
                    }
                    else if (obj.Status == 3)
                    {
                        obj.Statusstr = "Reject";
                    }

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        public PavatiListResponse GetDataForPavatiPrint(long Gratuity_Hisab_ID, long WitnessOneID, long WitnessTwoID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDataForPavatiPrint";
                cmdGet.Parameters.AddWithValue("@Gratuity_Hisab_ID", Gratuity_Hisab_ID);
                cmdGet.Parameters.AddWithValue("@WitnessOneID", WitnessOneID);
                cmdGet.Parameters.AddWithValue("@WitnessTwoID", WitnessTwoID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                PavatiListResponse obj = new PavatiListResponse();
                while (dr.Read())
                {
                    obj.Gratuity_Hisab_ID = objBaseSqlManager.GetInt64(dr, "Gratuity_Hisab_ID");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.LastDrawnSalary = Math.Round(objBaseSqlManager.GetDecimal(dr, "LastDrawnSalary"), 2);
                    obj.TotalMonth = objBaseSqlManager.GetInt32(dr, "TotalMonth");
                    // obj.TotalService = objBaseSqlManager.GetTextValue(dr, "TotalService");
                    obj.DateOfJoining = objBaseSqlManager.GetDateTime(dr, "DateOfJoining");
                    obj.DateOfLeaving = objBaseSqlManager.GetDateTime(dr, "DateOfLeaving");
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014") && obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        string ServiceTime = GetServiceTime(obj.DateOfJoining, obj.DateOfLeaving);
                        obj.TotalService = ServiceTime;
                    }
                    else
                    {
                        obj.TotalService = "";
                    }
                    if (obj.DateOfJoining != Convert.ToDateTime("10/10/2014") && obj.DateOfLeaving != Convert.ToDateTime("10/10/2014"))
                    {
                        string ServiceTime = GetServiceTimeEng(obj.DateOfJoining, obj.DateOfLeaving);
                        obj.TotalServiceEng = ServiceTime;
                    }
                    else
                    {
                        obj.TotalServiceEng = "";
                    }
                    obj.Gratuity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Gratuity"), 2);
                    obj.AdditionalGratuity = Math.Round(objBaseSqlManager.GetDecimal(dr, "AdditionalGratuity"), 2);
                    obj.TotalGratuity = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalGratuity"), 2);
                    obj.TotalEarnedSalary = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalEarnedSalary"), 2);
                    obj.BonusAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "BonusAmount"), 2);
                    obj.AdditionalBonusAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "AdditionalBonusAmount"), 2);
                    obj.TotalBonusAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalBonusAmount"), 2);
                    obj.LastDrawnSalaryLeaveEnhancement = Math.Round(objBaseSqlManager.GetDecimal(dr, "LastDrawnSalaryLeaveEnhancement"), 2);
                    obj.ClosingLeaves = Math.Round(objBaseSqlManager.GetDecimal(dr, "ClosingLeaves"), 2);
                    obj.LeaveEncashment = Math.Round(objBaseSqlManager.GetDecimal(dr, "LeaveEncashment"), 2);
                    obj.AdditionalLeaveEncashment = Math.Round(objBaseSqlManager.GetDecimal(dr, "AdditionalLeaveEncashment"), 2);
                    obj.TotalLeaveEncashment = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalLeaveEncashment"), 2);
                    obj.GrandTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount"), 2);
                    obj.Advance = Math.Round(objBaseSqlManager.GetDecimal(dr, "Advance"), 2);
                    decimal NetAmount = Math.Round((obj.GrandTotalAmount - obj.Advance), 2);
                    obj.NetAmount = Math.Round(NetAmount, 0, MidpointRounding.AwayFromZero);
                    obj.DateOfApplication = objBaseSqlManager.GetDateTime(dr, "DateOfApplication");
                    if (obj.DateOfApplication != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateOfApplicationstr = obj.DateOfApplication.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateOfApplicationstr = "";
                    }
                    int number = Convert.ToInt32(obj.NetAmount);
                    string NumberToWord = NumberToWords(number);
                    obj.AmountInWords = NumberToWord + "  " + "Only/-";
                    obj.WitnessOne = objBaseSqlManager.GetTextValue(dr, "WitnessOne");
                    obj.WitnessTwo = objBaseSqlManager.GetTextValue(dr, "WitnessTwo");
                    //obj.Year = objBaseSqlManager.GetTextValue(dr, "Year");

                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");

                    obj.Status = objBaseSqlManager.GetInt32(dr, "Status");
                    if (obj.Status == 1)
                    {
                        obj.Statusstr = "Pending";
                    }
                    else if (obj.Status == 2)
                    {
                        obj.Statusstr = "Approved";
                    }
                    else if (obj.Status == 3)
                    {
                        obj.Statusstr = "Reject";
                    }
                    bool respose = UpdateWitnessID(Gratuity_Hisab_ID, WitnessOneID, WitnessTwoID);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        private string GetServiceTimeEng(DateTime DateOfJoining, DateTime DateOfLeaving)
        {
            //DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(DateOfJoining).Ticks).Year - 1;
            DateTime dtPastYearDate = DateOfJoining.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (dtPastYearDate.AddMonths(i) == DateOfLeaving)
                {
                    Months = i;
                    break;
                }
                else if (dtPastYearDate.AddMonths(i) >= DateOfLeaving)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = DateOfLeaving.Subtract(dtPastYearDate.AddMonths(Months)).Days;
            int Hours = DateOfLeaving.Subtract(dtPastYearDate).Hours;
            int Minutes = DateOfLeaving.Subtract(dtPastYearDate).Minutes;
            int Seconds = DateOfLeaving.Subtract(dtPastYearDate).Seconds;
            return String.Format("{0} Year {1} Month {2} Day",
                                Years, Months, Days);
        }

        private string GetServiceTime(DateTime DateOfJoining, DateTime DateOfLeaving)
        {
            //DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(DateOfJoining).Ticks).Year - 1;
            DateTime dtPastYearDate = DateOfJoining.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (dtPastYearDate.AddMonths(i) == DateOfLeaving)
                {
                    Months = i;
                    break;
                }
                else if (dtPastYearDate.AddMonths(i) >= DateOfLeaving)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = DateOfLeaving.Subtract(dtPastYearDate.AddMonths(Months)).Days;
            int Hours = DateOfLeaving.Subtract(dtPastYearDate).Hours;
            int Minutes = DateOfLeaving.Subtract(dtPastYearDate).Minutes;
            int Seconds = DateOfLeaving.Subtract(dtPastYearDate).Seconds;
            return String.Format("{0} वर्ष {1} महिने",
                                Years, Months);
        }

        public bool UpdateWitnessID(long Gratuity_Hisab_ID, long WitnessOneID, long WitnessTwoID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateWitnessID";
                    cmdGet.Parameters.AddWithValue("@WitnessOneID", WitnessOneID);
                    cmdGet.Parameters.AddWithValue("@WitnessTwoID", WitnessTwoID);
                    cmdGet.Parameters.AddWithValue("@Gratuity_Hisab_ID", Gratuity_Hisab_ID);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        // 24 Jan 2022 Piyush Limbani
        public List<YearlySalarySheetList> GetYearlySalarySheetList(int FromMonthID, int FromYearID, int ToMonthID, int ToYearID, long EmployeeCode, DateTime FromDate, DateTime ToDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetYearlySalarySheetList";
                cmdGet.Parameters.AddWithValue("@FromMonthID", FromMonthID);
                cmdGet.Parameters.AddWithValue("@FromYearID", FromYearID);
                cmdGet.Parameters.AddWithValue("@ToMonthID", ToMonthID);
                cmdGet.Parameters.AddWithValue("@ToYearID", ToYearID);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<YearlySalarySheetList> objlst = new List<YearlySalarySheetList>();
                decimal sumTotalBonus = 0;
                decimal sumGrossWagesPayable = 0;
                decimal sumEarnedBasicWages = 0;
                decimal sumEarnedHouseRentAllowance = 0;
                decimal sumCityAllowance = 0;
                decimal sumVehicleAllowance = 0;
                decimal sumConveyance = 0;
                decimal sumPerformanceAllowance = 0;
                decimal sumPF = 0;
                decimal sumPT = 0;
                decimal sumESIC = 0;
                decimal sumMLWF = 0;
                decimal sumTDS = 0;
                decimal TDSSalary = 0;
                while (dr.Read())
                {
                    YearlySalarySheetList obj = new YearlySalarySheetList();
                    obj.SalarySheetID = objBaseSqlManager.GetInt64(dr, "SalarySheetID");
                    obj.RowNumber = objBaseSqlManager.GetInt32(dr, "RowNumber");
                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.EmployeeName = objBaseSqlManager.GetTextValue(dr, "EmployeeName");
                    obj.PrimaryAddress = objBaseSqlManager.GetTextValue(dr, "PrimaryAddress");
                    obj.PrimaryPin = objBaseSqlManager.GetInt64(dr, "PrimaryPin");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    obj.PanNo = objBaseSqlManager.GetTextValue(dr, "PanNo");
                    obj.MonthID = objBaseSqlManager.GetInt32(dr, "MonthID");
                    obj.YearID = objBaseSqlManager.GetInt32(dr, "YearID");
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string MonthName = mfi.GetMonthName(obj.MonthID).ToString();
                    MonthName = MonthName.Substring(0, 3);
                    string YearID = obj.YearID.ToString();
                    YearID = YearID.Substring(2, 2);
                    obj.MonthName = MonthName + '-' + YearID.ToString();
                    obj.GrossWagesPayable = objBaseSqlManager.GetDecimal(dr, "GrossWagesPayable");
                    sumGrossWagesPayable = sumGrossWagesPayable + obj.GrossWagesPayable;
                    obj.EarnedBasicWages = objBaseSqlManager.GetDecimal(dr, "EarnedBasicWages");
                    sumEarnedBasicWages = Math.Round((sumEarnedBasicWages + obj.EarnedBasicWages), 2);
                    obj.EarnedHouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "EarnedHouseRentAllowance");
                    sumEarnedHouseRentAllowance = Math.Round((sumEarnedHouseRentAllowance + obj.EarnedHouseRentAllowance), 2);
                    obj.CityAllowance = objBaseSqlManager.GetDecimal(dr, "CityAllowance");
                    sumCityAllowance = sumCityAllowance + obj.CityAllowance;
                    obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
                    sumVehicleAllowance = sumVehicleAllowance + obj.VehicleAllowance;
                    obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
                    sumConveyance = sumConveyance + obj.Conveyance;
                    obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");
                    sumPerformanceAllowance = sumPerformanceAllowance + obj.PerformanceAllowance;
                    obj.PF = objBaseSqlManager.GetDecimal(dr, "PF");
                    sumPF = sumPF + obj.PF;
                    obj.PT = objBaseSqlManager.GetDecimal(dr, "PT");
                    sumPT = sumPT + obj.PT;
                    obj.ESIC = objBaseSqlManager.GetDecimal(dr, "ESIC");
                    sumESIC = sumESIC + obj.ESIC;
                    obj.MLWF = objBaseSqlManager.GetDecimal(dr, "MLWF");
                    sumMLWF = sumMLWF + obj.MLWF;
                    obj.TDS = objBaseSqlManager.GetDecimal(dr, "TDS");
                    sumTDS = sumTDS + obj.TDS;
                    obj.BonusStatusID = objBaseSqlManager.GetInt64(dr, "BonusStatusID");
                    obj.BonusAmount = objBaseSqlManager.GetDecimal(dr, "BonusAmount");
                    obj.BonusPercentage = objBaseSqlManager.GetDecimal(dr, "BonusPercentage");
                    obj.TotalEarnedWages = objBaseSqlManager.GetDecimal(dr, "TotalEarnedWages");
                    if (obj.BonusStatusID == 1)
                    {
                        sumTotalBonus = sumTotalBonus + Convert.ToDecimal(obj.TotalEarnedWages);
                    }
                    else if (obj.BonusStatusID == 2)
                    {
                        sumTotalBonus = sumTotalBonus + Convert.ToDecimal(obj.EarnedBasicWages);
                    }
                    else
                    {
                        sumTotalBonus = 0;
                    }
                    obj.LeaveEnhancementStatusID = objBaseSqlManager.GetInt32(dr, "LeaveEnhancementStatusID");
                    obj.LeaveEnhancementAmount = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementAmount");
                    obj.LeaveEnhancementPercentage = objBaseSqlManager.GetDecimal(dr, "LeaveEnhancementPercentage");
                    obj.GrandTotalWages = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
                    obj.TotalBasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
                    obj.MaxSalarySheetID = objBaseSqlManager.GetInt64(dr, "MaxSalarySheetID");
                    obj.Gratuity = objBaseSqlManager.GetDecimal(dr, "Gratuity");


                    TDSSalary = GetTDSSalary(FromDate, ToDate, EmployeeCode);


                    objlst.Add(obj);
                    for (int i = 0; i < objlst.Count; i++)
                    {
                        decimal TotalBonus = 0;
                        decimal LeaveEncashment = 0;
                        if (obj.BonusAmount != 0)
                        {
                            TotalBonus = obj.BonusAmount;
                            objlst[i].TotalBonus = TotalBonus;
                        }
                        else
                        {
                            TotalBonus = Math.Round(((sumTotalBonus * obj.BonusPercentage) / 100));
                            objlst[i].TotalBonus = TotalBonus;
                        }
                        decimal ClosingLeaves = GetClosingLeavesBySalarySheetID(obj.MaxSalarySheetID);
                        decimal NoOfDaysLeaveEncashment = GetLastLeaveEncashmentNoOfDays();
                        if (obj.LeaveEnhancementStatusID == 1)
                        {
                            if (obj.LeaveEnhancementAmount == 0)
                            {
                                obj.TotalLeaveEnhancement = Math.Round(obj.GrandTotalWages, 2);
                                LeaveEncashment = Math.Round(((obj.TotalLeaveEnhancement / NoOfDaysLeaveEncashment) * ClosingLeaves)); //NoOfDaysLeaveEncashment = 30
                                objlst[i].LeaveEncashment = LeaveEncashment;
                            }
                            else
                            {
                                obj.TotalLeaveEnhancement = 0;
                                LeaveEncashment = obj.LeaveEnhancementAmount;
                                objlst[i].LeaveEncashment = LeaveEncashment;
                            }
                        }
                        else if (obj.LeaveEnhancementStatusID == 2)
                        {
                            if (obj.LeaveEnhancementAmount == 0)
                            {
                                obj.TotalLeaveEnhancement = Math.Round(obj.TotalBasicAllowance, 2);
                                LeaveEncashment = Math.Round(((obj.TotalLeaveEnhancement / NoOfDaysLeaveEncashment) * ClosingLeaves));  //NoOfDaysLeaveEncashment = 30
                                objlst[i].LeaveEncashment = LeaveEncashment;
                            }
                            else
                            {
                                obj.TotalLeaveEnhancement = 0;
                                LeaveEncashment = obj.LeaveEnhancementAmount;
                                objlst[i].LeaveEncashment = LeaveEncashment;
                            }
                        }
                        else
                        {
                            obj.TotalLeaveEnhancement = 0;
                            LeaveEncashment = 0;
                            objlst[i].LeaveEncashment = LeaveEncashment;
                        }
                        objlst[i].sumGrossWagesPayable = sumGrossWagesPayable;
                        objlst[i].sumEarnedBasicWages = sumEarnedBasicWages;
                        objlst[i].sumEarnedHouseRentAllowance = Math.Round(sumEarnedHouseRentAllowance, 0, MidpointRounding.AwayFromZero);
                        objlst[i].sumCityAllowance = sumCityAllowance;
                        objlst[i].sumVehicleAllowance = sumVehicleAllowance;
                        objlst[i].sumConveyance = sumConveyance;
                        objlst[i].sumPerformanceAllowance = sumPerformanceAllowance;
                        objlst[i].sumPF = sumPF;
                        objlst[i].sumPT = sumPT;
                        objlst[i].sumESIC = sumESIC;
                        objlst[i].sumMLWF = sumMLWF;
                        objlst[i].sumTDS = sumTDS;

                        objlst[i].TDSSalary = TDSSalary;


                        objlst[i].GrandTotal = sumGrossWagesPayable + TotalBonus + LeaveEncashment;
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        public decimal GetTDSSalary(DateTime FromDate, DateTime ToDate, long EmployeeCode)
        {
            decimal TDSSalary = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTDSSalary";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TDSSalary = objBaseSqlManager.GetDecimal(dr, "TDSSalary");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return TDSSalary;
        }

        public FormSixteenDetailListResponse GetFormSixteenDetailFromMaster()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetFormSixteenDetailFromMaster";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                FormSixteenDetailListResponse obj = new FormSixteenDetailListResponse();
                while (dr.Read())
                {
                    obj.StandardDeduction = objBaseSqlManager.GetDecimal(dr, "StandardDeduction");
                    obj.UnderSection80C = objBaseSqlManager.GetDecimal(dr, "UnderSection80C");
                    obj.HealthInsurancePremiaUnderSection80D = objBaseSqlManager.GetDecimal(dr, "HealthInsurancePremiaUnderSection80D");
                    obj.InterestOn80E = objBaseSqlManager.GetDecimal(dr, "InterestOn80E");
                    obj.UnderSection80TTA = objBaseSqlManager.GetDecimal(dr, "UnderSection80TTA");
                    obj.RebateUnderSection87A = objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A");
                    obj.PensionUnderSection80CCD_1 = objBaseSqlManager.GetDecimal(dr, "PensionUnderSection80CCD_1");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 31 Jan 2022 Piyush Limbani
        //public bool AddFormSixteen(DateTime FromDate, DateTime ToDate, long EmployeeCode, decimal StandardDeductions4_a, decimal HousingLoanPrincipal, decimal ELSS, decimal PPF, decimal LifeInsurance, decimal Others, decimal HealthInsurancePremiaUnderSection80D, decimal HealthInsurancePremiaUnderSection80D_Actual, decimal InterestOn80E, decimal InterestOn80E_Actual, decimal UnderSection80G, decimal UnderSection80TTA, decimal UnderSection80TTA_Actual, bool IsFiftyPer, decimal RebateUnderSection87A, decimal UnderSection80C, decimal TaxSlabPer, decimal SurchargeSlabPer, decimal EducationSlabPer,
        //  decimal PensionUnderSection80CCD_1, decimal PensionUnderSection80CCD_1_Actual, long CreatedBy, DateTime CreatedOn, bool IsDelete)

        public bool AddFormSixteen(PrintYearlySalarySheet obj, EmployeeFormSixteenDataReq data, long CreatedBy, DateTime CreatedOn, bool IsDelete)
        {
            SqlCommand cmdAdd = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdAdd.CommandType = CommandType.StoredProcedure;
                cmdAdd.CommandText = "AddFormSixteenDetail";
                cmdAdd.Parameters.AddWithValue("@FromDate", data.FromDate);
                cmdAdd.Parameters.AddWithValue("@ToDate", data.ToDate);
                cmdAdd.Parameters.AddWithValue("@EmployeeCode", data.EmployeeCode);
                cmdAdd.Parameters.AddWithValue("@StandardDeduction", obj.StandardDeductions4_a);
                cmdAdd.Parameters.AddWithValue("@HousingLoanPrincipal", data.HousingLoanPrincipal);
                cmdAdd.Parameters.AddWithValue("@ELSS", data.ELSS);
                cmdAdd.Parameters.AddWithValue("@PPF", data.PPF);
                cmdAdd.Parameters.AddWithValue("@LifeInsurance", data.LifeInsurance);
                cmdAdd.Parameters.AddWithValue("@Others", data.Others);
                cmdAdd.Parameters.AddWithValue("@TDSSalary", data.TDSSalary);
                cmdAdd.Parameters.AddWithValue("@HealthInsurancePremiaUnderSection80D", obj.HealthInsurancePremiaUnderSection80D);
                cmdAdd.Parameters.AddWithValue("@HealthInsurancePremiaUnderSection80D_Actual", obj.HealthInsurancePremiaUnderSection80D_Actual);
                cmdAdd.Parameters.AddWithValue("@InterestOn80E", obj.InterestOn80E);
                cmdAdd.Parameters.AddWithValue("@InterestOn80E_Actual", obj.InterestOn80E_Actual);
                cmdAdd.Parameters.AddWithValue("@UnderSection80G", data.UnderSection80G);
                cmdAdd.Parameters.AddWithValue("@UnderSection80TTA", obj.UnderSection80TTA);
                cmdAdd.Parameters.AddWithValue("@UnderSection80TTA_Actual", obj.UnderSection80TTA_Actual);
                cmdAdd.Parameters.AddWithValue("@IsFiftyPer", data.IsFiftyPer);
                cmdAdd.Parameters.AddWithValue("@RebateUnderSection87A", obj.RebateUnderSection87A);
                cmdAdd.Parameters.AddWithValue("@UnderSection80C", obj.UnderSection80C);
                cmdAdd.Parameters.AddWithValue("@TaxSlabPer", obj.TaxSlabPer);
                cmdAdd.Parameters.AddWithValue("@SurchargeSlabPer", obj.SurchargeSlabPer);
                cmdAdd.Parameters.AddWithValue("@EducationSlabPer", obj.EducationSlabPer);
                cmdAdd.Parameters.AddWithValue("@PensionUnderSection80CCD_1", obj.PensionUnderSection80CCD_1);
                cmdAdd.Parameters.AddWithValue("@PensionUnderSection80CCD_1_Actual", obj.PensionUnderSection80CCD_1_Actual);

                //FormTexableIncome
                cmdAdd.Parameters.AddWithValue("@IncomeFrom", obj.IncomeFrom);
                cmdAdd.Parameters.AddWithValue("@IncomeTo", obj.IncomeTo);
                cmdAdd.Parameters.AddWithValue("@TaxOnTotalIncome", obj.TaxOnTotalIncome);
                cmdAdd.Parameters.AddWithValue("@TaxOnTotalIncome_one", obj.TaxOnTotalIncome_one);
                cmdAdd.Parameters.AddWithValue("@RebateUnderSection87A_Income", obj.RebateUnderSection87A_Income);
                cmdAdd.Parameters.AddWithValue("@Surcharge", obj.Surcharge);
                cmdAdd.Parameters.AddWithValue("@Education", obj.Education);
                //FormTexableIncome

                cmdAdd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                cmdAdd.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                cmdAdd.Parameters.AddWithValue("@IsDelete", IsDelete);

                //Add By Dhruvik
                cmdAdd.Parameters.AddWithValue("@FormSixteenEHRA", data.FormSixteenEHRA);
                //Add By Dhruvik

                objBaseSqlManager.ExecuteNonQuery(cmdAdd);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public FormSixteenDetails CheckIsExistsFormSixteenDetails(DateTime FromDate, DateTime ToDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckIsExistsFormSixteenDetails";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                FormSixteenDetails obj = new FormSixteenDetails();
                while (dr.Read())
                {
                    obj.FormSixteenID = objBaseSqlManager.GetInt64(dr, "FormSixteenID");
                    obj.StandardDeductions = Math.Round(objBaseSqlManager.GetDecimal(dr, "StandardDeductions"), 2);
                    obj.HousingLoanPrincipal = Math.Round(objBaseSqlManager.GetDecimal(dr, "HousingLoanPrincipal"), 2);
                    obj.ELSS = Math.Round(objBaseSqlManager.GetDecimal(dr, "ELSS"), 2);
                    obj.PPF = Math.Round(objBaseSqlManager.GetDecimal(dr, "PPF"), 2);
                    obj.LifeInsurance = Math.Round(objBaseSqlManager.GetDecimal(dr, "LifeInsurance"), 2);
                    obj.Others = Math.Round(objBaseSqlManager.GetDecimal(dr, "Others"), 2);
                    obj.TDSSalary = Math.Round(objBaseSqlManager.GetDecimal(dr, "TDSSalary"), 2);
                    obj.TotalGrossAmount = obj.HousingLoanPrincipal + obj.ELSS + obj.PPF + obj.LifeInsurance + obj.Others;
                    obj.HealthInsurancePremiaUnderSection80D = Math.Round(objBaseSqlManager.GetDecimal(dr, "HealthInsurancePremiaUnderSection80D"), 2);
                    obj.HealthInsurancePremiaUnderSection80D_Actual = Math.Round(objBaseSqlManager.GetDecimal(dr, "HealthInsurancePremiaUnderSection80D_Actual"), 2);
                    obj.InterestOn80E = Math.Round(objBaseSqlManager.GetDecimal(dr, "InterestOn80E"), 2);
                    obj.InterestOn80E_Actual = Math.Round(objBaseSqlManager.GetDecimal(dr, "InterestOn80E_Actual"), 2);
                    obj.UnderSection80G = Math.Round(objBaseSqlManager.GetDecimal(dr, "UnderSection80G"), 2);
                    obj.UnderSection80TTA = Math.Round(objBaseSqlManager.GetDecimal(dr, "UnderSection80TTA"), 2);
                    obj.UnderSection80TTA_Actual = Math.Round(objBaseSqlManager.GetDecimal(dr, "UnderSection80TTA_Actual"), 2);
                    obj.RebateUnderSection87A = Math.Round(objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A"), 2);
                    obj.UnderSection80C = Math.Round(objBaseSqlManager.GetDecimal(dr, "UnderSection80C"), 2);
                    obj.TaxSlabPer = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxSlabPer"), 2);
                    obj.SurchargeSlabPer = Math.Round(objBaseSqlManager.GetDecimal(dr, "SurchargeSlabPer"), 2);
                    obj.EducationSlabPer = Math.Round(objBaseSqlManager.GetDecimal(dr, "EducationSlabPer"), 2);
                    obj.PensionUnderSection80CCD_1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "PensionUnderSection80CCD_1"), 2);
                    obj.PensionUnderSection80CCD_1_Actual = Math.Round(objBaseSqlManager.GetDecimal(dr, "PensionUnderSection80CCD_1_Actual"), 2);

                    //FormTexableIncome
                    obj.IncomeFrom = Math.Round(objBaseSqlManager.GetDecimal(dr, "IncomeFrom"), 2);
                    obj.IncomeTo = Math.Round(objBaseSqlManager.GetDecimal(dr, "IncomeTo"), 2);
                    obj.TaxOnTotalIncome = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome"), 2);
                    obj.TaxOnTotalIncome_one = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome_one"), 2);
                    obj.RebateUnderSection87A_Income = Math.Round(objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A_Income"), 2);
                    obj.Surcharge = Math.Round(objBaseSqlManager.GetDecimal(dr, "Surcharge"), 2);
                    obj.Education = Math.Round(objBaseSqlManager.GetDecimal(dr, "Education"), 2);
                    //FormTexableIncome

                    obj.IsFiftyPer = objBaseSqlManager.GetBoolean(dr, "IsFiftyPer");

                    //Add By Dhruvik
                    obj.FormSixteenEHRA = Math.Round(objBaseSqlManager.GetDecimal(dr, "FormSixteenEHRA"), 2);
                    //Add By Dhruvik
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        //public bool UpdateFormSixteenDetail(long FormSixteenID, decimal StandardDeduction, decimal HousingLoanPrincipal, decimal ELSS, decimal PPF, decimal LifeInsurance, decimal Others,
        //    decimal HealthInsurancePremiaUnderSection80D_Actual, decimal InterestOn80E_Actual, decimal UnderSection80G, decimal UnderSection80TTA_Actual, bool IsFiftyPer, decimal RebateUnderSection87A, decimal UnderSection80C, decimal TaxSlabPer, decimal SurchargeSlabPer, decimal EducationSlabPer,
        //     decimal PensionUnderSection80CCD_1, decimal PensionUnderSection80CCD_1_Actual, long UpdatedBy, DateTime UpdatedOn)

        public bool UpdateFormSixteenDetail(PrintYearlySalarySheet obj, EmployeeFormSixteenDataReq data, long UpdatedBy, DateTime UpdatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateFormSixteenDetail";
                cmdGet.Parameters.AddWithValue("@FormSixteenID", data.FormSixteenID);
                cmdGet.Parameters.AddWithValue("@StandardDeduction", data.StandardDeduction);
                cmdGet.Parameters.AddWithValue("@HousingLoanPrincipal", data.HousingLoanPrincipal);
                cmdGet.Parameters.AddWithValue("@ELSS", data.ELSS);
                cmdGet.Parameters.AddWithValue("@PPF", data.PPF);
                cmdGet.Parameters.AddWithValue("@LifeInsurance", data.LifeInsurance);
                cmdGet.Parameters.AddWithValue("@Others", data.Others);
                cmdGet.Parameters.AddWithValue("@TDSSalary", data.TDSSalary);
                cmdGet.Parameters.AddWithValue("@HealthInsurancePremiaUnderSection80D_Actual", data.HealthInsurancePremiaUnderSection80D_Actual);
                cmdGet.Parameters.AddWithValue("@InterestOn80E_Actual", data.InterestOn80E_Actual);
                cmdGet.Parameters.AddWithValue("@UnderSection80G", data.UnderSection80G);
                cmdGet.Parameters.AddWithValue("@UnderSection80TTA_Actual", data.UnderSection80TTA_Actual);
                cmdGet.Parameters.AddWithValue("@RebateUnderSection87A", obj.RebateUnderSection87A);
                cmdGet.Parameters.AddWithValue("@UnderSection80C", obj.UnderSection80C);
                cmdGet.Parameters.AddWithValue("@IsFiftyPer", data.IsFiftyPer);
                cmdGet.Parameters.AddWithValue("@TaxSlabPer", obj.TaxSlabPer);
                cmdGet.Parameters.AddWithValue("@SurchargeSlabPer", obj.SurchargeSlabPer);
                cmdGet.Parameters.AddWithValue("@EducationSlabPer", obj.EducationSlabPer);
                cmdGet.Parameters.AddWithValue("@PensionUnderSection80CCD_1", obj.PensionUnderSection80CCD_1);
                cmdGet.Parameters.AddWithValue("@PensionUnderSection80CCD_1_Actual", obj.PensionUnderSection80CCD_1_Actual);

                //FormTexableIncome
                cmdGet.Parameters.AddWithValue("@IncomeFrom", data.IncomeFrom);
                cmdGet.Parameters.AddWithValue("@IncomeTo", data.IncomeTo);
                cmdGet.Parameters.AddWithValue("@TaxOnTotalIncome", data.TaxOnTotalIncome);
                cmdGet.Parameters.AddWithValue("@TaxOnTotalIncome_one", data.TaxOnTotalIncome_one);
                cmdGet.Parameters.AddWithValue("@RebateUnderSection87A_Income", data.RebateUnderSection87A_Income);
                cmdGet.Parameters.AddWithValue("@Surcharge", data.Surcharge);
                cmdGet.Parameters.AddWithValue("@Education", data.Education);
                //FormTexableIncome

                cmdGet.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmdGet.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);

                //Add By Dhruvik
                cmdGet.Parameters.AddWithValue("@FormSixteenEHRA", data.FormSixteenEHRA);
                //Add By Dhruvik

                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }
        // 31 Jan 2022 Piyush Limbani

        // 14 Feb 2022 Piyush Limbani

        //FormTexableIncome
        public long AddFormSixteenDetailMaster(AddFormSixteenDetail data)
        {
            if (!string.IsNullOrEmpty(data.DeleteItems))
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteTexableIncomeQtyItems";
                    cmdGet.Parameters.AddWithValue("@FormSixteenTexableID", data.DeleteItems);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }

            FormSixteenDetail_Mst obj = new FormSixteenDetail_Mst();
            obj.FormSixteenDetailID = data.FormSixteenDetailID;
            obj.StandardDeduction = data.StandardDeduction;
            obj.UnderSection80C = data.UnderSection80C;
            obj.HealthInsurancePremiaUnderSection80D = data.HealthInsurancePremiaUnderSection80D;
            obj.InterestOn80E = data.InterestOn80E;
            obj.UnderSection80TTA = data.UnderSection80TTA;
            obj.RebateUnderSection87A = data.RebateUnderSection87A;
            obj.PensionUnderSection80CCD_1 = data.PensionUnderSection80CCD_1;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = false;

            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.FormSixteenDetailID == 0)
                {
                    context.FormSixteenDetail_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }

                if (obj.FormSixteenDetailID > 0)
                {
                    foreach (var item in data.lstForm16TexableIncomeQty)
                    {
                        FormSixteenTexableIncome objTexableIncomeQty = new FormSixteenTexableIncome();
                        objTexableIncomeQty.FormSixteenTexableID = item.FormSixteenTexableID;
                        objTexableIncomeQty.FormSixteenDetailID = obj.FormSixteenDetailID;
                        objTexableIncomeQty.IncomeFrom = item.IncomeFrom;
                        objTexableIncomeQty.IncomeTo = item.IncomeTo;
                        objTexableIncomeQty.TaxOnTotalIncome = item.TaxOnTotalIncome;
                        objTexableIncomeQty.TaxOnTotalIncome_one = item.TaxOnTotalIncome_one;
                        objTexableIncomeQty.RebateUnderSection87A_Income = item.RebateUnderSection87A_Income;
                        objTexableIncomeQty.Surcharge = item.Surcharge;
                        objTexableIncomeQty.Education = item.Education;
                        objTexableIncomeQty.CreatedBy = data.CreatedBy;
                        objTexableIncomeQty.CreatedOn = data.CreatedOn;
                        objTexableIncomeQty.UpdatedBy = data.UpdatedBy;
                        objTexableIncomeQty.UpdatedOn = data.UpdatedOn;
                        objTexableIncomeQty.IsDelete = false;

                        if (objTexableIncomeQty.FormSixteenTexableID == 0)
                        {
                            context.FormSixteenTexableIncomes.Add(objTexableIncomeQty);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(objTexableIncomeQty).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    return 0;
                }
                else
                {
                    if (obj.FormSixteenDetailID > 0)
                    {
                        return obj.FormSixteenDetailID;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        //FormTexableIncome
        public List<AddFormSixteenTexableIncome> GetAllTexableIncomeListByForm16TexableIncomeID(long FormSixteenDetailID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTexableIncomeListByForm16TexableIncomeID";
                cmdGet.Parameters.AddWithValue("@FormSixteenDetailID", FormSixteenDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<AddFormSixteenTexableIncome> objlst = new List<AddFormSixteenTexableIncome>();
                while (dr.Read())
                {
                    AddFormSixteenTexableIncome objTexableIncome = new AddFormSixteenTexableIncome();
                    objTexableIncome.FormSixteenTexableID = objBaseSqlManager.GetInt64(dr, "FormSixteenTexableID");
                    objTexableIncome.IncomeFrom = objBaseSqlManager.GetInt64(dr, "IncomeFrom");
                    objTexableIncome.IncomeTo = objBaseSqlManager.GetInt64(dr, "IncomeTo");
                    objTexableIncome.TaxOnTotalIncome = objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome");
                    objTexableIncome.TaxOnTotalIncome_one = objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome_one");
                    objTexableIncome.RebateUnderSection87A_Income = objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A_Income");
                    objTexableIncome.Surcharge = objBaseSqlManager.GetDecimal(dr, "Surcharge");
                    objTexableIncome.Education = objBaseSqlManager.GetDecimal(dr, "Education");
                    objlst.Add(objTexableIncome);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //FormTexableIncome
        public AddFormSixteenTexableIncome GetTexableIncome(decimal TotalTaxableIncome)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTexableIncome";
                cmdGet.Parameters.AddWithValue("@TotalTaxableIncome", TotalTaxableIncome);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AddFormSixteenTexableIncome objIncome = new AddFormSixteenTexableIncome();
                while (dr.Read())
                {
                    objIncome.IncomeFrom = objBaseSqlManager.GetDecimal(dr, "IncomeFrom");
                    objIncome.IncomeTo = objBaseSqlManager.GetDecimal(dr, "IncomeTo");
                    objIncome.TaxOnTotalIncome = objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome");
                    objIncome.TaxOnTotalIncome_one = objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome_one");
                    objIncome.RebateUnderSection87A_Income = objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A_Income");
                    objIncome.Surcharge = objBaseSqlManager.GetDecimal(dr, "Surcharge");
                    objIncome.Education = objBaseSqlManager.GetDecimal(dr, "Education");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objIncome;
            }
        }


        public List<FormSixteenDetailListResponse> GetAllFormSixteenDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllFormSixteenDetailList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<FormSixteenDetailListResponse> objlst = new List<FormSixteenDetailListResponse>();
                while (dr.Read())
                {
                    FormSixteenDetailListResponse obj = new FormSixteenDetailListResponse();
                    obj.FormSixteenDetailID = objBaseSqlManager.GetInt64(dr, "FormSixteenDetailID");
                    obj.StandardDeduction = Math.Round((objBaseSqlManager.GetDecimal(dr, "StandardDeduction")), 2);
                    obj.UnderSection80C = Math.Round((objBaseSqlManager.GetDecimal(dr, "UnderSection80C")), 2);
                    obj.HealthInsurancePremiaUnderSection80D = Math.Round((objBaseSqlManager.GetDecimal(dr, "HealthInsurancePremiaUnderSection80D")), 2);
                    obj.InterestOn80E = Math.Round((objBaseSqlManager.GetDecimal(dr, "InterestOn80E")), 2);
                    obj.UnderSection80TTA = Math.Round((objBaseSqlManager.GetDecimal(dr, "UnderSection80TTA")), 2);
                    obj.RebateUnderSection87A = Math.Round((objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A")), 2);
                    obj.PensionUnderSection80CCD_1 = Math.Round((objBaseSqlManager.GetDecimal(dr, "PensionUnderSection80CCD_1")), 2);
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteFormSixteenDetail(long FormSixteenDetailID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteFormSixteenDetail";
                cmdGet.Parameters.AddWithValue("@FormSixteenDetailID", FormSixteenDetailID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 16 Feb 2022 Piyush Limbani
        public decimal GetTaxSlabPer(decimal TotalTaxableIncome)
        {
            decimal Percentage = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxSlabPer";
                cmdGet.Parameters.AddWithValue("@TotalTaxableIncome", TotalTaxableIncome);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    Percentage = objBaseSqlManager.GetDecimal(dr, "Percentage");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return Percentage;
        }

        public decimal GetSurchargeSlabPer(decimal TotalTaxableIncome)
        {
            decimal Percentage = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSurchargeSlabPer";
                cmdGet.Parameters.AddWithValue("@TotalTaxableIncome", TotalTaxableIncome);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    Percentage = objBaseSqlManager.GetDecimal(dr, "Percentage");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return Percentage;
        }

        public decimal GetEducationSlabPer(decimal TotalTaxableIncome)
        {
            decimal Percentage = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEducationSlabPer";
                cmdGet.Parameters.AddWithValue("@TotalTaxableIncome", TotalTaxableIncome);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    Percentage = objBaseSqlManager.GetDecimal(dr, "Percentage");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return Percentage;
        }

        public List<FormSixteenValueModel> GetFormSixteenByDate(DateTime FromDate, DateTime ToDate, long EmployeeCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetFormSixteenByDate";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<FormSixteenValueModel> objlst = new List<FormSixteenValueModel>();
                while (dr.Read())
                {
                    FormSixteenValueModel obj = new FormSixteenValueModel();
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.StandardDeductions = objBaseSqlManager.GetDecimal(dr, "StandardDeductions");
                    obj.HousingLoanPrincipal = objBaseSqlManager.GetDecimal(dr, "HousingLoanPrincipal");
                    obj.ELSS = objBaseSqlManager.GetDecimal(dr, "ELSS");
                    obj.PPF = objBaseSqlManager.GetDecimal(dr, "PPF");
                    obj.LifeInsurance = objBaseSqlManager.GetDecimal(dr, "LifeInsurance");
                    obj.Others = objBaseSqlManager.GetDecimal(dr, "Others");
                    obj.TDSSalary = objBaseSqlManager.GetDecimal(dr, "TDSSalary");
                    obj.HealthInsurancePremiaUnderSection80D = objBaseSqlManager.GetDecimal(dr, "HealthInsurancePremiaUnderSection80D");
                    obj.HealthInsurancePremiaUnderSection80D_Actual = objBaseSqlManager.GetDecimal(dr, "HealthInsurancePremiaUnderSection80D_Actual");
                    obj.InterestOn80E = objBaseSqlManager.GetDecimal(dr, "InterestOn80E");
                    obj.InterestOn80E_Actual = objBaseSqlManager.GetDecimal(dr, "InterestOn80E_Actual");
                    obj.UnderSection80G = objBaseSqlManager.GetDecimal(dr, "UnderSection80G");
                    obj.UnderSection80TTA = objBaseSqlManager.GetDecimal(dr, "UnderSection80TTA");
                    obj.UnderSection80TTA_Actual = objBaseSqlManager.GetDecimal(dr, "UnderSection80TTA_Actual");
                    obj.RebateUnderSection87A = objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A");
                    obj.UnderSection80C = objBaseSqlManager.GetDecimal(dr, "UnderSection80C");
                    obj.TaxSlabPer = objBaseSqlManager.GetDecimal(dr, "TaxSlabPer");
                    obj.SurchargeSlabPer = objBaseSqlManager.GetDecimal(dr, "SurchargeSlabPer");
                    obj.EducationSlabPer = objBaseSqlManager.GetDecimal(dr, "EducationSlabPer");
                    obj.PensionUnderSection80CCD_1 = objBaseSqlManager.GetDecimal(dr, "PensionUnderSection80CCD_1");
                    obj.PensionUnderSection80CCD_1_Actual = objBaseSqlManager.GetDecimal(dr, "PensionUnderSection80CCD_1_Actual");

                    obj.IncomeFrom = objBaseSqlManager.GetDecimal(dr, "IncomeFrom");
                    obj.IncomeTo = objBaseSqlManager.GetDecimal(dr, "IncomeTo");
                    obj.TaxOnTotalIncome = objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome");
                    obj.TaxOnTotalIncome_one = objBaseSqlManager.GetDecimal(dr, "TaxOnTotalIncome_one");
                    obj.RebateUnderSection87A_Income = objBaseSqlManager.GetDecimal(dr, "RebateUnderSection87A_Income");
                    obj.Surcharge = objBaseSqlManager.GetDecimal(dr, "Surcharge");
                    obj.Education = objBaseSqlManager.GetDecimal(dr, "Education");

                    obj.EmployeeCode = objBaseSqlManager.GetInt64(dr, "EmployeeCode");
                    obj.FormSixteenID = objBaseSqlManager.GetInt64(dr, "FormSixteenID");
                    obj.IsFiftyPer = objBaseSqlManager.GetBoolean(dr, "IsFiftyPer");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        



        //public GetAllowanceDetail GetTopOneAllowanceDetailByEmployeeCode(string StartDate, string EndDate, long EmployeeCode)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetTopOneAllowanceDetailByEmployeeCode";
        //    cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
        //    cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
        //    cmdGet.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    GetAllowanceDetail obj = new GetAllowanceDetail();
        //    while (dr.Read())
        //    {
        //        obj.BasicAllowance = objBaseSqlManager.GetDecimal(dr, "TotalBasicAllowance");
        //        obj.HouseRentAllowance = objBaseSqlManager.GetDecimal(dr, "TotalHouseRentAllowance");
        //        obj.TotalBasic = objBaseSqlManager.GetDecimal(dr, "GrandTotalWages");
        //        obj.Conveyance = objBaseSqlManager.GetDecimal(dr, "Conveyance");
        //        obj.ConveyancePerDay = objBaseSqlManager.GetDecimal(dr, "ConveyancePerDay");
        //        obj.VehicleAllowance = objBaseSqlManager.GetDecimal(dr, "VehicleAllowance");
        //        obj.PerformanceAllowance = objBaseSqlManager.GetDecimal(dr, "PerformanceAllowance");

        //        obj.PerformanceAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "PerformanceAllowanceStatusID");
        //        obj.CityAllowanceStatusID = objBaseSqlManager.GetInt64(dr, "CityAllowanceStatusID");
        //        obj.PFStatusID = objBaseSqlManager.GetInt64(dr, "PFStatusID");
        //        obj.ESICStatusID = objBaseSqlManager.GetInt64(dr, "ESICStatusID");
        //        obj.OpeningLeaves = objBaseSqlManager.GetInt64(dr, "OpeningLeaves");
        //        //obj.IsOldPF = objBaseSqlManager.GetBoolean(dr, "IsOldPF");
        //        //obj.IsOldESIC = objBaseSqlManager.GetBoolean(dr, "IsOldESIC");
        //        //obj.IsOldCityAllowance = objBaseSqlManager.GetBoolean(dr, "IsOldCityAllowance");
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return obj;
        //}

        //private string GetRoundoffValue(decimal ActAmount)
        //{
        //    string[] ActAmountstr = ActAmount.ToString().Split('.');
        //    decimal ActAmountDecimalPlaces = Convert.ToDecimal(ActAmountstr[1]);
        //    string ActAmountDecimalPlacesStr = "0." + (ActAmountDecimalPlaces.ToString());
        //    ActAmountDecimalPlaces = Convert.ToDecimal(ActAmountDecimalPlacesStr);
        //    double roundedTimes20 = Convert.ToDouble(Math.Round(ActAmountDecimalPlaces * 20));
        //    double rounded = roundedTimes20 / 20;
        //    ActAmountDecimalPlacesStr = rounded.ToString();
        //    string FinalActAmount = (Convert.ToDecimal(ActAmountstr[0]) + Convert.ToDecimal(ActAmountDecimalPlacesStr)).ToString();
        //    return String.Format(FinalActAmount);
        //}
    }
}
