using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.attendance.Controllers
{
    public class AttendanceController : Controller
    {
        private IAdminService _adminservice;
        private ICommonService _ICommonService;
        private IAttandanceService _IAttandanceservice;

        public AttendanceController(IAdminService adminservice, ICommonService commonservice, IAttandanceService Attandanceservice)
        {
            _adminservice = adminservice;
            _ICommonService = commonservice;
            _IAttandanceservice = Attandanceservice;
        }

        //
        // GET: /attendance/Attendance/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Attendance()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewAttandanceList(SearchAttandance model)
        {
            decimal sumTotalHoursWorked = 0;
            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            
            List<AttandanceListResponse> objModel = _IAttandanceservice.GetAttandanceList(model.FromDate, model.ToDate, model.GodownID, model.EmployeeCode);

            foreach (var record in objModel)
            {
                sumTotalHoursWorked += record.TotalHoursWorked;
            }
            if (sumTotalHoursWorked != 0)
            {
                objModel[0].sumTotalHoursWorked = sumTotalHoursWorked;
            }

            return PartialView(objModel);
        }

        // 11-05-2020
        public ActionResult ExportExcelAttandanceList(SearchAttandance model)
        {
            decimal sumTotalHoursWorked = 0;
            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            var AttandanceList = _IAttandanceservice.GetAttandanceList(model.FromDate, model.ToDate, model.GodownID, model.EmployeeCode);
            List<AttandanceListResponse> objFinalList = new List<AttandanceListResponse>();
            foreach (var record in AttandanceList)
            {
                objFinalList.Add(record);
                sumTotalHoursWorked += record.TotalHoursWorked;
            }

            List<AttandanceListForExp> lstallowancelist = objFinalList.Select(x => new AttandanceListForExp()
            {
                Date = x.ADatestr,
                EmployeeName = x.AEmployeeName,
                Day = x.DayName,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                TotalHoursWorked = x.TotalHoursWorked,
                OT = x.OT,
                Status = x.Status
            }).ToList();

            DataTable ds = new DataTable();
            ds = ToDataTable(lstallowancelist);
            DataRow row = ds.NewRow();           
            row["Date"] = "Total :";
            row["EmployeeName"] = "";
            row["Day"] = "";
            row["TimeIn"] = "";
            row["TimeOut"] = "";
            row["TotalHoursWorked"] = sumTotalHoursWorked;
            row["Status"] = "";
            ds.Rows.InsertAt(row, 0);
            DataView dv = ds.DefaultView;
            dv.Sort = "Date asc";
            DataTable sortedDT = dv.ToTable();
            ds = sortedDT;

            //DataSet ds = new DataSet();
            //ds.Tables.Add(ToDataTable(lstallowancelist));

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "AttandanceList.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }



        // 04 Sep. 2020 Piyush Limbani
        public ActionResult MonthlyAttendance()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        [HttpPost]
        public PartialViewResult MonthlyAttendanceList(int MonthID, int YearID, int? GodownID = 0, long? EmployeeCode = 0)
        {
            List<MonthlyAttendanceList> objlst = _IAttandanceservice.GetMonthlyAttendanceList(MonthID, YearID, GodownID, EmployeeCode);
            return PartialView(objlst);
        }

        public ActionResult ExportExcelMonthlyAttendance(int MonthID, int YearID, int? GodownID = 0, long? EmployeeCode = 0)
        {
            string sumTotalOTHrs = "";
            int sumPresent = 0;
            int sumAbsent = 0;
            int sumSunday = 0;
            int sumHoliday = 0;
            long sumVehicleNoOfDaysCount = 0;

            var MonthlyAttendanceList = _IAttandanceservice.GetMonthlyAttendanceList(MonthID, YearID, GodownID, EmployeeCode);
            List<MonthlyAttendanceList> objFinalList = new List<MonthlyAttendanceList>();
            foreach (var record in MonthlyAttendanceList)
            {
                objFinalList.Add(record);
                sumPresent += record.Present;
                sumAbsent += record.Absent;
                sumSunday += record.Sunday;
                sumHoliday += record.Holiday;
                sumTotalOTHrs = record.sumTotalOTHrs;
                sumVehicleNoOfDaysCount += record.VehicleNoOfDaysCount;
            }

            List<MonthlyAttendanceForExp> lstattendancelist = objFinalList.Select(x => new MonthlyAttendanceForExp()
            {
                RowNumber = x.RowNumber,
                EmployeeName = x.EmployeeName,
                Present = x.Present,
                Absent = x.Absent,
                Sunday = x.Sunday,
                Holiday = x.Holiday,
                OTHours = x.OTHours,
                VehicleNoOfDaysCount = x.VehicleNoOfDaysCount
            }).ToList();

            DataTable ds = new DataTable();
            ds = ToDataTable(lstattendancelist);

            DataRow row = ds.NewRow();
            row["RowNumber"] = MonthlyAttendanceList.Count + 1;
            row["EmployeeName"] = "";
            row["Present"] = sumPresent;
            row["Absent"] = sumAbsent;
            row["Sunday"] = sumSunday;
            row["Holiday"] = sumHoliday;
            row["OTHours"] = sumTotalOTHrs;
            row["VehicleNoOfDaysCount"] = sumVehicleNoOfDaysCount;
            ds.Rows.InsertAt(row, 0);

            DataView dv = ds.DefaultView;
            dv.Sort = "RowNumber asc";
            DataTable sortedDT = dv.ToTable();
            ds = sortedDT;

            //DataSet ds = new DataSet();
            //ds.Tables.Add(ToDataTable(lstattendancelist));

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "MonthlyAttendanceList.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
    }
}