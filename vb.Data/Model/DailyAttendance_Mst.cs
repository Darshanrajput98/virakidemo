//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vb.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DailyAttendance_Mst
    {
        public long DAttendanceID { get; set; }
        public Nullable<long> EmployeeCode { get; set; }
        public Nullable<System.DateTime> ADate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public Nullable<decimal> HoursWorked { get; set; }
        public Nullable<int> EShiftID { get; set; }
        public Nullable<bool> NonWorking { get; set; }
        public string NonWorkingType { get; set; }
        public string AttendanceType { get; set; }
        public Nullable<decimal> WeeklyOff { get; set; }
        public Nullable<decimal> Holidays { get; set; }
        public Nullable<int> AYear { get; set; }
        public Nullable<int> AMonth { get; set; }
        public string AEmployeeName { get; set; }
        public Nullable<decimal> Present { get; set; }
        public Nullable<decimal> Absent { get; set; }
        public string ActualTimeIn { get; set; }
        public string ActualTimeOut { get; set; }
        public Nullable<decimal> AOtHrs { get; set; }
        public Nullable<decimal> AOtApprovedHrs { get; set; }
        public Nullable<bool> AOtApproved { get; set; }
        public string AOtSupervisior { get; set; }
        public Nullable<decimal> ActualHrs { get; set; }
        public Nullable<decimal> BreakDuration { get; set; }
        public string PayCaderName { get; set; }
        public Nullable<decimal> HrsWorked { get; set; }
        public Nullable<int> AttendanceMonth { get; set; }
        public Nullable<bool> Authorised { get; set; }
        public string HalfdayStatus { get; set; }
        public string LeaveCode { get; set; }
        public string LeaveAppNo { get; set; }
        public Nullable<decimal> Leave { get; set; }
        public Nullable<bool> TempWork { get; set; }
        public string TempWorkType { get; set; }
        public string TempWorkName { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string LogTime { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public string EarlyComingTime { get; set; }
        public string LateComingTime { get; set; }
        public string EarlyGoingTime { get; set; }
        public string LateGoingTime { get; set; }
        public string StartRecessTime { get; set; }
        public string EndRecessTime { get; set; }
        public string EmpRecessStartTime { get; set; }
        public string EmpRecessEndTime { get; set; }
        public string Status { get; set; }
        public string ShiftName { get; set; }
        public Nullable<decimal> NotTotalEmpWorkedHrs { get; set; }
        public Nullable<bool> ShiftNextDay { get; set; }
        public Nullable<System.DateTime> LastLogDateTime { get; set; }
        public Nullable<int> PunchCount { get; set; }
        public Nullable<decimal> TotHrs { get; set; }
        public Nullable<decimal> BreakTimeHrs { get; set; }
        public Nullable<bool> HDGraceDay { get; set; }
        public string InComment { get; set; }
        public string OutComment { get; set; }
        public bool IsEmailSent { get; set; }
        public Nullable<System.DateTime> ActualTimeInDate { get; set; }
        public Nullable<System.DateTime> ActualTimeOutDate { get; set; }
        public Nullable<bool> IsNightShift { get; set; }
    }
}