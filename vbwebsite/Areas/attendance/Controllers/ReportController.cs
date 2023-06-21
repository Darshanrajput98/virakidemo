
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.ViewModel;
using vb.Service;

namespace vbwebsite.Areas.attendance.Controllers
{
    public class ReportController : Controller
    {
        private IAdminService _adminservice;
        private ICommonService _ICommonService;
        private IAttandanceService _IAttandanceService;

        public ReportController(IAdminService adminservice, ICommonService commonservice, IAttandanceService attandanceservice)
        {
            _adminservice = adminservice;
            _ICommonService = commonservice;
            _IAttandanceService = attandanceservice;
        }

        //
        // GET: /attendance/Report/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bonus()
        {
            //decimal s = 463.44M;
            //decimal a = Math.Round(s, 1, MidpointRounding.AwayFromZero);
            //// ans 463.4
            //decimal s1 = 463.46M;
            //decimal a1 = Math.Round(s1, 1, MidpointRounding.AwayFromZero);
            //// ans 463.50

            //decimal s3 = 463.99M;
            //decimal a3 = Math.Round(s3, 1, MidpointRounding.AwayFromZero);
            //// ans 464

            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        public ActionResult BonusList(BonusListResponse model)
        {
            int FromMonthID = model.FromDate.Month;
            int FromYearID = model.FromDate.Year;
            int ToMonthID = model.ToDate.Month;
            int ToYearID = model.ToDate.Year;
            List<BonusListResponse> objlst = _IAttandanceService.GetEmployeeWiseBonusList(FromMonthID, FromYearID, ToMonthID, ToYearID, model.GodownID, model.EmployeeCode);
            List<BonusListByMonth> lst = new List<BonusListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new BonusListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainBonus = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeCode");
            dt.Columns.Add("FullName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("Bonus");
            var result = (from item in objlst
                          select new
                          {
                              EmployeeCode = item.EmployeeCode,
                              FullName = item.FullName,
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].EmployeeCode;
                dr[1] = result.ToList()[i].FullName;
                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            int BonusStatusID = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainBonus.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["EmployeeCode"].ToString() == lst[i].ListMainBonus[j].EmployeeCode.ToString())
                        {
                            BonusStatusID = lst[i].ListMainBonus[j].BonusStatusID;
                            if (BonusStatusID == 1)
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainBonus[j].TotalEarnedWages, 2);
                                cnt++;
                                break;
                            }
                            else if (BonusStatusID == 2)
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainBonus[j].EarnedBasicWages, 2);
                                cnt++;
                                break;
                            }
                            else
                            {
                                dt.Rows[k][i + 3] = 0;
                                cnt++;
                                break;
                            }
                        }
                    }
                }
            }
            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 3; col < dt.Columns.Count - 1; col++)
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                dt.Rows[t][lst.Count + 3] = sum;
                decimal BonusAmount = 0;
                decimal BonusPercentage = 0;
                for (int col = 4; col < dt.Columns.Count - 1; col++)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        for (int j = 0; j < lst[i].ListMainBonus.Count; j++)
                        {
                            BonusAmount = lst[i].ListMainBonus[j].BonusAmount;
                            BonusPercentage = lst[i].ListMainBonus[j].BonusPercentage;
                        }
                    }
                    //if (BonusPercentage != 0)
                    //{
                    //    dt.Rows[t][lst.Count + 4] = (sum * BonusPercentage) / 100;
                    //}
                    //else
                    //{
                    //    dt.Rows[t][lst.Count + 4] = BonusAmount;
                    //}
                    if (BonusAmount != 0)
                    {
                        dt.Rows[t][lst.Count + 4] = BonusAmount;
                    }
                    else
                    {
                        dt.Rows[t][lst.Count + 4] = Math.Round(((sum * BonusPercentage) / 100));
                    }
                }
            }
            DataRow drtot = dt.NewRow();
            for (int col = 3; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 3)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }
            return View(dt);
        }

        //23-04-2020
        public ActionResult LeaveEncashment()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        [HttpPost]
        public PartialViewResult LeaveEncashmentList(LeaveEncashmentListResponse model)
        {
            int FromMonthID = model.FromDate.Month;
            int FromYearID = model.FromDate.Year;
            int ToMonthID = model.ToDate.Month;
            int ToYearID = model.ToDate.Year;
            List<LeaveEncashmentListResponse> objModel = _IAttandanceService.GetLeaveEncashmentList(FromMonthID, FromYearID, ToMonthID, ToYearID, model.GodownID, model.EmployeeCode);
            return PartialView(objModel);
        }

        public ActionResult Gratuity()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        [HttpPost]
        public PartialViewResult GratuityList(GratuityListResponse model)
        {
            List<GratuityListResponse> objModel = null;
            if (model.DateOfLeaving.Month == 1 || model.DateOfLeaving.Month == 2 || model.DateOfLeaving.Month == 3)
            {
                DateTime DateOfLeaving = model.DateOfLeaving.AddYears(-1);
                objModel = _IAttandanceService.GetGratuityList(DateOfLeaving.Year, model.GodownID, model.EmployeeCode, model.DateOfLeaving);
            }
            else
            {
                DateTime DateOfLeaving = model.DateOfLeaving;
                objModel = _IAttandanceService.GetGratuityList(DateOfLeaving.Year, model.GodownID, model.EmployeeCode, model.DateOfLeaving);
            }
            //List<GratuityListResponse> objModel = _IAttandanceService.GetGratuityList(DateOfLeaving, model.GodownID, model.EmployeeCode);
            return PartialView(objModel);
        }

        // 11-05-2020
        public ActionResult ExportExcelBonusList(BonusListResponse model)
        {
            int FromMonthID = model.FromDate.Month;
            int FromYearID = model.FromDate.Year;
            int ToMonthID = model.ToDate.Month;
            int ToYearID = model.ToDate.Year;
            List<BonusListResponse> objlst = _IAttandanceService.GetEmployeeWiseBonusList(FromMonthID, FromYearID, ToMonthID, ToYearID, model.GodownID, model.EmployeeCode);
            List<BonusListByMonth> lst = new List<BonusListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new BonusListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainBonus = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeCode");
            dt.Columns.Add("FullName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("Bonus");
            var result = (from item in objlst
                          select new
                          {
                              EmployeeCode = item.EmployeeCode,
                              FullName = item.FullName,
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].EmployeeCode;
                dr[1] = result.ToList()[i].FullName;
                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            int BonusStatusID = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainBonus.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["EmployeeCode"].ToString() == lst[i].ListMainBonus[j].EmployeeCode.ToString())
                        {
                            BonusStatusID = lst[i].ListMainBonus[j].BonusStatusID;
                            if (BonusStatusID == 1)
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainBonus[j].TotalEarnedWages, 2);
                                cnt++;
                                break;
                            }
                            else if (BonusStatusID == 2)
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainBonus[j].EarnedBasicWages, 2);
                                cnt++;
                                break;
                            }
                            else
                            {
                                dt.Rows[k][i + 3] = 0;
                                cnt++;
                                break;
                            }
                        }
                    }
                }
            }
            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 3; col < dt.Columns.Count - 1; col++)
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                dt.Rows[t][lst.Count + 3] = sum;
                decimal BonusAmount = 0;
                decimal BonusPercentage = 0;
                for (int col = 4; col < dt.Columns.Count - 1; col++)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        for (int j = 0; j < lst[i].ListMainBonus.Count; j++)
                        {
                            BonusAmount = lst[i].ListMainBonus[j].BonusAmount;
                            BonusPercentage = lst[i].ListMainBonus[j].BonusPercentage;
                        }
                    }
                    //if (BonusPercentage != 0)
                    //{
                    //    dt.Rows[t][lst.Count + 4] = (sum * BonusPercentage) / 100;
                    //}
                    //else
                    //{
                    //    dt.Rows[t][lst.Count + 4] = BonusAmount;
                    //}
                    if (BonusAmount != 0)
                    {
                        dt.Rows[t][lst.Count + 4] = BonusAmount;
                    }
                    else
                    {
                        dt.Rows[t][lst.Count + 4] = Math.Round(((sum * BonusPercentage) / 100), 2);
                    }
                }
            }
            DataRow drtot = dt.NewRow();
            for (int col = 3; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 3)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add((dt));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "BonusList.xls");
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

        // 11-05-2020
        public ActionResult ExportExcelLeaveEncashmentList(LeaveEncashmentListResponse model)
        {
            int FromMonthID = model.FromDate.Month;
            int FromYearID = model.FromDate.Year;
            int ToMonthID = model.ToDate.Month;
            int ToYearID = model.ToDate.Year;
            var LeaveEncashmentList = _IAttandanceService.GetLeaveEncashmentList(FromMonthID, FromYearID, ToMonthID, ToYearID, model.GodownID, model.EmployeeCode);
            List<LeaveEncashmentListForExp> lstallowancelist = LeaveEncashmentList.Select(x => new LeaveEncashmentListForExp()
            {
                SrNo = x.RowNumber,
                Name = x.FullName,
                Total = x.TotalLeaveEnhancement,
                ClosingLeaves = x.ClosingLeaves,
                LeaveEncashment = x.LeaveEncashment
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstallowancelist));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "LeaveEncashmentList.xls");
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

        // 11-05-2020
        public ActionResult ExportExcelGratuityList(GratuityListResponse model)
        {
            List<GratuityListResponse> GratuityList = null;
            if (model.DateOfLeaving.Month == 1 || model.DateOfLeaving.Month == 2 || model.DateOfLeaving.Month == 3)
            {
                DateTime DateOfLeaving = model.DateOfLeaving.AddYears(-1);
                GratuityList = _IAttandanceService.GetGratuityList(DateOfLeaving.Year, model.GodownID, model.EmployeeCode, model.DateOfLeaving);
            }
            else
            {
                DateTime DateOfLeaving = model.DateOfLeaving;
                GratuityList = _IAttandanceService.GetGratuityList(DateOfLeaving.Year, model.GodownID, model.EmployeeCode, model.DateOfLeaving);
            }
            List<GratuityListForExp> lstallowancelist = GratuityList.Select(x => new GratuityListForExp()
            {
                SrNo = x.RowNumber,
                Name = x.FullName,
                DOJ = x.DateOfJoiningstr,
                DOL = x.DateOfLeavingstr,
                Total = x.TotalGratuity,
                TotalMonth = x.TotalMonth,
                Gratuity = x.Gratuity
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstallowancelist));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "GratuityList.xls");
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

        public ActionResult VehicleAllowance()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        // 15 July 2020 Piyush Limbani
        [HttpPost]
        public PartialViewResult VehicleAllowanceList(VehicleListForAllowanceResponse model)
        {
            List<VehicleListForAllowanceResponse> objModel = _IAttandanceService.GetVehicleListForAllowance(model.MonthID, model.YearID, model.EmployeeCode);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelVehicleAllowanceList(VehicleListForAllowanceResponse model)
        {
            var LeaveEncashmentList = _IAttandanceService.GetVehicleListForAllowance(model.MonthID, model.YearID, model.EmployeeCode);
            List<VehicleListForAllowanceForExp> lstallowancelist = LeaveEncashmentList.Select(x => new VehicleListForAllowanceForExp()
            {
                Date = x.AssignedDatestr,
                TempoNo = x.TempoNo,
                VehicleNo = x.VehicleNo,
                DeliveryPerson1 = x.DeliveryPerson1,
                DeliveryPerson2 = x.DeliveryPerson2,
                DeliveryPerson3 = x.DeliveryPerson3,
                DeliveryPerson4 = x.DeliveryPerson4,
                Area = x.AreaName,
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstallowancelist));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "VehicleAllowanceList.xls");
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

        // 29 July 2020 Piyush Limbani
        public ActionResult PaidPayment()
        {
            ViewBag.BankName = _ICommonService.GetBankNameList();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        // 29 July 2020 Piyush Limbani
        [HttpPost]
        public PartialViewResult PaidPaymentList(DateTime PaymentDate, long? GodownID, long? BankID, long? EmployeeCode)
        {
            ViewBag.BankName = _ICommonService.GetBankNameList();
            decimal sumAmount = 0;
            decimal sumNetAmount = 0;
            List<PaidPaymentList> objlst = null;
            List<PaidPaymentList> objFinalList = new List<PaidPaymentList>();
            objlst = _IAttandanceService.GetPaidPaymentList(PaymentDate, GodownID, BankID, EmployeeCode);
            foreach (var record in objlst)
            {
                sumAmount += record.Amount;
                sumNetAmount += record.NetAmount;
                objFinalList.Add(record);
            }
            if (sumAmount != 0)
            {
                objFinalList[0].sumAmount = sumAmount;
            }
            if (sumNetAmount != 0)
            {
                objFinalList[0].sumNetAmount = sumNetAmount;
            }
            return PartialView(objFinalList);

            //List<PaidPaymentList> objlst = _IAttandanceService.GetPaidPaymentList(PaymentDate, GodownID, BankID, EmployeeCode);
            //return PartialView(objlst);
        }

        // 29 July 2020 Piyush Limbani
        public ActionResult ExportExcelPaidPayment(DateTime PaymentDate, long? GodownID, long? BankID, long? EmployeeCode)
        {
            List<PaidPaymentList> objlst = _IAttandanceService.GetPaidPaymentList(PaymentDate, GodownID, BankID, EmployeeCode);
            List<PaidSalaryListExport> lstproduct = objlst.Select(x => new PaidSalaryListExport()
            {
                Bank_Name = x.Bank_Name,
                Client_Code = x.Client_Code,
                Product_Code = x.Product_Code,
                Payment_Type = x.Payment_Type,
                Payment_Ref_No = x.Payment_Ref_No,
                Payment_Date = x.Payment_Date,
                Instrument_Date = x.Instrument_Date,
                Dr_Ac_No = x.Dr_Ac_No,
                Amount = x.NetAmount,
                Bank_Code_Indicator = x.Bank_Code_Indicator,
                Beneficiary_Code = x.Beneficiary_Code,
                //Beneficiary_Name = x.Beneficiary_Name,
                Beneficiary_Name = x.FullName,

                // 10 Feb 2021 Piyush Limbani
                //Beneficiary_Bank = x.Beneficiary_Bank,
                Beneficiary_Bank = "",
                // 10 Feb 2021 Piyush Limbani

                Beneficiary_Branch_IFSC_Code = x.Beneficiary_Branch_IFSC_Code,
                Beneficiary_Acc_No = x.Beneficiary_Acc_No,
                Location = x.Location,
                Print_Location = x.Print_Location,
                Instrument_Number = x.Instrument_Number,
                Ben_Add1 = x.Ben_Add1,
                Ben_Add2 = x.Ben_Add2,
                Ben_Add3 = x.Ben_Add3,
                Ben_Add4 = x.Ben_Add4,
                Beneficiary_Email = x.Beneficiary_Email,
                Beneficiary_Mobile = x.Beneficiary_Mobile,
                Debit_Narration = x.Debit_Narration,
                Credit_Narration = x.Credit_Narration,
                Payment_Details_1 = x.Payment_Details_1,
                Payment_Details_2 = x.Payment_Details_2,
                Payment_Details_3 = x.Payment_Details_3,
                Payment_Details_4 = x.Payment_Details_4,

                Bill_No = x.Bill_No,
                Bill_Date = x.Bill_Date,
                Net_Wages_Paid = x.Net_Wages_Paid,
                Deductions = x.Deductions,

                Net = x.NetAmount,
                Enrichment_6 = x.Enrichment_6,
                Enrichment_7 = x.Enrichment_7,
                Enrichment_8 = x.Enrichment_8,
                Enrichment_9 = x.Enrichment_9,
                Enrichment_10 = x.Enrichment_10,
                Enrichment_11 = x.Enrichment_11,
                Enrichment_12 = x.Enrichment_12,
                Enrichment_13 = x.Enrichment_13,
                Enrichment_14 = x.Enrichment_14,
                Enrichment_15 = x.Enrichment_15,
                Enrichment_16 = x.Enrichment_16,
                Enrichment_17 = x.Enrichment_17,
                Enrichment_18 = x.Enrichment_18,
                Enrichment_19 = x.Enrichment_19,
                Enrichment_20 = x.Enrichment_20
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstproduct));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "PaidSalaryList.xls");
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


        // 31 July 2020 Piyush Limbani
        [HttpPost]
        public ActionResult UpdateSalaryPayment(SalaryPayment data)
        {
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    bool respose = _IAttandanceService.UpdateSalaryPayment(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        // 13 Aug 2020 Piyush Limbani
        public ActionResult SalarySlip()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        [HttpPost]
        public ActionResult PrintSalarySlip(int MonthID, int YearID, long? EmployeeCode = 0, int? GodownID = 0)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/SalarySlip.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SalarySlip.rdlc");
                }
                lr.ReportPath = path;

                List<ModelSalarySlip> LabelData = _IAttandanceService.GetDataForSalarySlipPrint(MonthID, YearID, EmployeeCode, GodownID);

                DataTable header = Common.ToDataTable(LabelData);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                lr.DataSources.Add(MedsheetHeader);

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =


                     "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>7.5in</PageHeight>" +
            "  <MarginTop>0.5cm</MarginTop>" +
            "  <MarginLeft>1cm</MarginLeft>" +
            "  <MarginRight>1cm</MarginRight>" +
            "  <MarginBottom>0.4cm</MarginBottom>" +
            "</DeviceInfo>";

                //           "<DeviceInfo>" +
                //"  <OutputFormat>" + reportType + "</OutputFormat>" +
                //"  <PageWidth>11in</PageWidth>" +
                //"  <PageHeight>7.5in</PageHeight>" +
                //"  <MarginTop>0.7cm</MarginTop>" +
                //"  <MarginLeft>1cm</MarginLeft>" +
                //"  <MarginRight>1cm</MarginRight>" +
                //"  <MarginBottom>0.5cm</MarginBottom>" +
                //"</DeviceInfo>";



                //           "<DeviceInfo>" +
                //"  <OutputFormat>" + reportType + "</OutputFormat>" +
                //"  <PageWidth>11in</PageWidth>" +
                //"  <PageHeight>7.5in</PageHeight>" +
                //"  <MarginTop>1cm</MarginTop>" +
                //"  <MarginLeft>1cm</MarginLeft>" +
                //"  <MarginRight>1cm</MarginRight>" +
                //"  <MarginBottom>1cm</MarginBottom>" +
                //"</DeviceInfo>";


                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                string name = DateTime.Now.Ticks.ToString() + ".pdf";
                string Pdfpathcreate = Server.MapPath("~/SalarySlip/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();

                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/SalarySlip/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/SalarySlip/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/SalarySlip/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        // 24 Jan 2022 Piyush Limbani
        public ActionResult YearlySalarySheet()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            //ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }


        [HttpPost]
        public PartialViewResult YearlySalarySheetList(YearlySalarySheetReq model)
        {
            int FromMonthID = model.FromDate.Month;
            int FromYearID = model.FromDate.Year;
            int ToMonthID = model.ToDate.Month;
            int ToYearID = model.ToDate.Year;
            List<YearlySalarySheetList> objlst = _IAttandanceService.GetYearlySalarySheetList(FromMonthID, FromYearID, ToMonthID, ToYearID, model.EmployeeCode, model.FromDate, model.ToDate);
            return PartialView(objlst);
        }
        // 24 Jan 2022 Piyush Limbani


        // 25 Jan 2022 Piyush Limbani
        public ActionResult ExportExcelYearlySalarySheet(DateTime FromDate, DateTime ToDate, long EmployeeCode)
        {
            string sdate = FromDate.ToString("dd-MMM-yy");
            string edate = ToDate.ToString("dd-MMM-yy");
            string syear = FromDate.ToString("yy");
            string eyear = ToDate.ToString("yyyy");
            int FromMonthID = FromDate.Month;
            int FromYearID = FromDate.Year;
            int ToMonthID = ToDate.Month;
            int ToYearID = ToDate.Year;
            var objlst = _IAttandanceService.GetYearlySalarySheetList(FromMonthID, FromYearID, ToMonthID, ToYearID, EmployeeCode, FromDate, ToDate);
            List<YearlySalarySheetList> objFinalList = new List<YearlySalarySheetList>();
            List<YearlySalarySheetExport> lstsalary = objlst.Select(x => new YearlySalarySheetExport()
            {
                SrNo = x.RowNumber,
                MonthofSalary = x.MonthName,
                GrossWages = x.GrossWagesPayable,
                EBA = x.EarnedBasicWages,
                EHRA = x.EarnedHouseRentAllowance,
                CityAllowance = x.CityAllowance,
                VehicleAllowance = x.VehicleAllowance,
                Conveyance = x.Conveyance,
                PerformanceAllowance = x.PerformanceAllowance,
                PF = x.PF,
                PT = x.PT,
                ESIC = x.ESIC,
                MLWF = x.MLWF,
                TDS = x.TDS
            }).ToList();

            DataTable ds = new DataTable();
            ds = ToDataTable(lstsalary);
            DataRow row = ds.NewRow();
            row["SrNo"] = 0;
            row["MonthofSalary"] = "";
            row["GrossWages"] = 0;
            row["EBA"] = 0;
            row["EHRA"] = 0;
            row["CityAllowance"] = 0;
            row["VehicleAllowance"] = 0;
            row["Conveyance"] = 0;
            row["PerformanceAllowance"] = 0;
            row["PF"] = 0;
            row["PT"] = 0;
            row["ESIC"] = 0;
            row["MLWF"] = 0;
            row["TDS"] = 0;
            ds.Rows.InsertAt(row, 0);

            DataRow row1 = ds.NewRow();
            row1["SrNo"] = 0;
            row1["MonthofSalary"] = "";
            row1["GrossWages"] = 0;
            row1["EBA"] = 0;
            row1["EHRA"] = 0;
            row1["CityAllowance"] = 0;
            row1["VehicleAllowance"] = 0;
            row1["Conveyance"] = 0;
            row1["PerformanceAllowance"] = 0;
            row1["PF"] = 0;
            row1["PT"] = 0;
            row1["ESIC"] = 0;
            row1["MLWF"] = 0;
            row1["TDS"] = 0;
            ds.Rows.InsertAt(row1, 0);

            DataRow row2 = ds.NewRow();
            row2["SrNo"] = 0;
            row2["MonthofSalary"] = "";
            row2["GrossWages"] = 0;
            row2["EBA"] = 0;
            row2["EHRA"] = 0;
            row2["CityAllowance"] = 0;
            row2["VehicleAllowance"] = 0;
            row2["Conveyance"] = 0;
            row2["PerformanceAllowance"] = 0;
            row2["PF"] = 0;
            row2["PT"] = 0;
            row2["ESIC"] = 0;
            row2["MLWF"] = 0;
            row2["TDS"] = 0;
            ds.Rows.InsertAt(row2, 0);

            DataRow row3 = ds.NewRow();
            row3["SrNo"] = 0;
            row3["MonthofSalary"] = "";
            row3["GrossWages"] = 0;
            row3["EBA"] = 0;
            row3["EHRA"] = 0;
            row3["CityAllowance"] = 0;
            row3["VehicleAllowance"] = 0;
            row3["Conveyance"] = 0;
            row3["PerformanceAllowance"] = 0;
            row3["PF"] = 0;
            row3["PT"] = 0;
            row3["ESIC"] = 0;
            row3["MLWF"] = 0;
            row3["TDS"] = 0;
            ds.Rows.InsertAt(row3, 0);

            DataRow row4 = ds.NewRow();
            row4["SrNo"] = 0;
            row4["MonthofSalary"] = "";
            row4["GrossWages"] = 0;
            row4["EBA"] = 0;
            row4["EHRA"] = 0;
            row4["CityAllowance"] = 0;
            row4["VehicleAllowance"] = 0;
            row4["Conveyance"] = 0;
            row4["PerformanceAllowance"] = 0;
            row4["PF"] = 0;
            row4["PT"] = 0;
            row4["ESIC"] = 0;
            row4["MLWF"] = 0;
            row4["TDS"] = 0;
            ds.Rows.InsertAt(row4, 0);

            DataRow row5 = ds.NewRow();
            row5["SrNo"] = 0;
            row5["MonthofSalary"] = "";
            row5["GrossWages"] = 0;
            row5["EBA"] = 0;
            row5["EHRA"] = 0;
            row5["CityAllowance"] = 0;
            row5["VehicleAllowance"] = 0;
            row5["Conveyance"] = 0;
            row5["PerformanceAllowance"] = 0;
            row5["PF"] = 0;
            row5["PT"] = 0;
            row5["ESIC"] = 0;
            row5["MLWF"] = 0;
            row5["TDS"] = 0;
            ds.Rows.InsertAt(row5, 0);

            DataRow row6 = ds.NewRow();
            row6["SrNo"] = objlst.Count + 1;
            row6["MonthofSalary"] = "Additional TDS :";
            row6["GrossWages"] = 0;
            row6["EBA"] = 0;
            row6["EHRA"] = 0;
            row6["CityAllowance"] = 0;
            row6["VehicleAllowance"] = 0;
            row6["Conveyance"] = 0;
            row6["PerformanceAllowance"] = 0;
            row6["PF"] = 0;
            row6["PT"] = 0;
            row6["ESIC"] = 0;
            row6["MLWF"] = 0;
            row6["TDS"] = objlst[0].TDSSalary;
            ds.Rows.InsertAt(row6, 0);

            DataRow row7 = ds.NewRow();
            row7["SrNo"] = objlst.Count + 2;
            row7["MonthofSalary"] = "Bonus :";
            row7["GrossWages"] = objlst[0].TotalBonus;
            row7["EBA"] = 0;
            row7["EHRA"] = 0;
            row7["CityAllowance"] = 0;
            row7["VehicleAllowance"] = 0;
            row7["Conveyance"] = 0;
            row7["PerformanceAllowance"] = 0;
            row7["PF"] = 0;
            row7["PT"] = 0;
            row7["ESIC"] = 0;
            row7["MLWF"] = 0;
            row7["TDS"] = 0;
            ds.Rows.InsertAt(row7, 0);

            DataRow row8 = ds.NewRow();
            row8["SrNo"] = objlst.Count + 3;
            row8["MonthofSalary"] = "Leave Encashment :";
            row8["GrossWages"] = objlst[0].LeaveEncashment;
            row8["EBA"] = 0;
            row8["EHRA"] = 0;
            row8["CityAllowance"] = 0;
            row8["VehicleAllowance"] = 0;
            row8["Conveyance"] = 0;
            row8["PerformanceAllowance"] = 0;
            row8["PF"] = 0;
            row8["PT"] = 0;
            row8["ESIC"] = 0;
            row8["MLWF"] = 0;
            row8["TDS"] = 0;
            ds.Rows.InsertAt(row8, 0);

            DataRow row9 = ds.NewRow();
            row9["SrNo"] = objlst.Count + 4;
            row9["MonthofSalary"] = "Grand Total :";
            row9["GrossWages"] = objlst[0].GrandTotal;
            row9["EBA"] = objlst[0].sumEarnedBasicWages;
            row9["EHRA"] = objlst[0].sumEarnedHouseRentAllowance;
            row9["CityAllowance"] = objlst[0].sumCityAllowance;
            row9["VehicleAllowance"] = objlst[0].sumVehicleAllowance;
            row9["Conveyance"] = objlst[0].sumConveyance;
            row9["PerformanceAllowance"] = objlst[0].sumPerformanceAllowance;
            row9["PF"] = objlst[0].sumPF;
            row9["PT"] = objlst[0].sumPT;
            row9["ESIC"] = objlst[0].sumESIC;
            row9["MLWF"] = objlst[0].sumMLWF;
            row9["TDS"] = objlst[0].sumTDS + objlst[0].TDSSalary;
            ds.Rows.InsertAt(row9, 0);

            DataView dv = ds.DefaultView;
            dv.Sort = "SrNo asc";
            DataTable sortedDT = dv.ToTable();
            ds = sortedDT;

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(ds);
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws.Cell("A1").Value = "";
                ws.Cell("B1").Value = "Name : " + objlst[0].EmployeeName;
                ws.Range("B1:M1").Row(1).Merge();
                ws.Range("N1").Value = "";
                ws.Cell("A2").Value = "";
                ws.Cell("B2").Value = "Employee Code : " + objlst[0].EmployeeCode;
                ws.Range("B2:M2").Row(1).Merge();
                ws.Range("N2").Value = "";
                ws.Cell("A3").Value = "";
                ws.Cell("B3").Value = "Address : " + objlst[0].PrimaryAddress;
                ws.Range("B3:M3").Row(1).Merge();
                ws.Range("N3").Value = "";
                ws.Cell("A4").Value = "";
                ws.Cell("B4").Value = objlst[0].AreaName + '-' + objlst[0].PrimaryPin;
                ws.Range("B4:M4").Row(1).Merge();
                ws.Range("N4").Value = "";
                ws.Cell("A5").Value = "";
                ws.Cell("B5").Value = "Mobile Number : " + objlst[0].MobileNumber;
                ws.Range("B5:M5").Row(1).Merge();
                ws.Range("N5").Value = "";
                ws.Cell("A6").Value = "";
                ws.Cell("B6").Value = "Financial Year : " + sdate + " To " + edate;
                ws.Range("B6:M6").Row(1).Merge();
                ws.Range("N6").Value = "";
                ws.Cell("A7").Value = "Sr No";
                ws.Cell("B7").Value = "Month of Salary";
                ws.Cell("C7").Value = "Gross Wages";
                ws.Cell("D7").Value = "EBA";
                ws.Cell("E7").Value = "EHRA";
                ws.Cell("F7").Value = "City";
                ws.Cell("G7").Value = "Vehicle";
                ws.Cell("H7").Value = "Conveyance";
                ws.Cell("I7").Value = "Performance Allowance";
                ws.Cell("J7").Value = "PF";
                ws.Cell("K7").Value = "PT";
                ws.Cell("L7").Value = "ESIC";
                ws.Cell("M7").Value = "MLWF";
                ws.Cell("N7").Value = "TDS";
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Style.Font.Bold = true;
                ws.Name = "Yearly Salary Sheet";
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + objlst[0].EmployeeName + "_" + "YEARLY_SALARY_SHEET" + "_" + syear + "-" + eyear + ".xlsx");
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
        // 25 Jan 2022 Piyush Limbani



        // 27 Jan 2022 Piyush Limbani
        //[HttpPost]
        //public ActionResult PrintEmployeeForm16(DateTime FromDate, DateTime ToDate, long EmployeeCode, decimal StandardDeduction, decimal HousingLoanPrincipal, decimal ELSS, decimal PPF, decimal LifeInsurance, decimal Others, decimal HealthInsurancePremiaUnderSection80D, decimal HealthInsurancePremiaUnderSection80D_Actual,
        //    decimal InterestOn80E, decimal InterestOn80E_Actual, decimal UnderSection80G, decimal UnderSection80TTA, decimal UnderSection80TTA_Actual, bool IsFiftyPer, decimal RebateUnderSection87A, decimal UnderSection80C, decimal TaxSlabPer, decimal SurchargeSlabPer, decimal EducationSlabPer, decimal PensionUnderSection80CCD_1, decimal PensionUnderSection80CCD_1_Actual, long FormSixteenID)

        [HttpPost]
        public ActionResult PrintEmployeeForm16(EmployeeFormSixteenDataReq data)
        {
            int FromMonthID = data.FromDate.Month;
            int FromYearID = data.FromDate.Year;
            int ToMonthID = data.ToDate.Month;
            int ToYearID = data.ToDate.Year;


            string FromEmployer = data.FromDate.ToString("dd-MMM-yyyy");
            string ToEmployer = data.ToDate.ToString("dd-MMM-yyyy");


            string FromFinancialYear = data.FromDate.ToString("yyyy");
            string ToFinancialYear = data.ToDate.ToString("yy");


            string FromAssessment = data.FromDate.AddYears(1).ToString("yyyy");
            string ToAssessment = data.ToDate.AddYears(1).ToString("yy");

            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/EmployeeForm16.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/EmployeeForm16.rdlc");
                }
                lr.ReportPath = path;
                var objlst = _IAttandanceService.GetYearlySalarySheetList(FromMonthID, FromYearID, ToMonthID, ToYearID, data.EmployeeCode, data.FromDate, data.ToDate);
                List<PrintYearlySalarySheet> LabelData = new List<PrintYearlySalarySheet>();
                PrintYearlySalarySheet obj = new PrintYearlySalarySheet();

                if (data.FormSixteenID == 0)
                {
                    var objlst2 = _IAttandanceService.GetFormSixteenDetailFromMaster();
                    obj.StandardDeductions4_a = Math.Round(objlst2.StandardDeduction, 2);
                    obj.UnderSection80C = Math.Round(objlst2.UnderSection80C, 2);
                    obj.HealthInsurancePremiaUnderSection80D = Math.Round(objlst2.HealthInsurancePremiaUnderSection80D, 2);
                    obj.HealthInsurancePremiaUnderSection80D_Actual = data.HealthInsurancePremiaUnderSection80D_Actual;
                    obj.InterestOn80E = Math.Round(objlst2.InterestOn80E, 2);
                    obj.InterestOn80E_Actual = data.InterestOn80E_Actual;
                    obj.UnderSection80TTA = objlst2.UnderSection80TTA;
                    obj.UnderSection80TTA_Actual = data.UnderSection80TTA_Actual;
                    obj.RebateUnderSection87A = objlst2.RebateUnderSection87A;
                    obj.PensionUnderSection80CCD_1 = objlst2.PensionUnderSection80CCD_1;
                    obj.PensionUnderSection80CCD_1_Actual = data.PensionUnderSection80CCD_1_Actual;
                }
                else
                {
                    obj.StandardDeductions4_a = data.StandardDeduction;
                    obj.UnderSection80C = data.UnderSection80C;
                    obj.HealthInsurancePremiaUnderSection80D = data.HealthInsurancePremiaUnderSection80D;
                    obj.HealthInsurancePremiaUnderSection80D_Actual = data.HealthInsurancePremiaUnderSection80D_Actual;
                    obj.InterestOn80E = data.InterestOn80E;
                    obj.InterestOn80E_Actual = data.InterestOn80E_Actual;
                    obj.UnderSection80TTA = data.UnderSection80TTA;
                    obj.UnderSection80TTA_Actual = data.UnderSection80TTA_Actual;
                    obj.RebateUnderSection87A = data.RebateUnderSection87A;
                    obj.PensionUnderSection80CCD_1 = data.PensionUnderSection80CCD_1;
                    obj.PensionUnderSection80CCD_1_Actual = data.PensionUnderSection80CCD_1_Actual;
                }


                obj.EmployeeCode = objlst[0].EmployeeCode;
                obj.EmployeeName = objlst[0].EmployeeName;
                obj.PrimaryAddress = objlst[0].PrimaryAddress;
                obj.PrimaryPin = objlst[0].PrimaryPin;
                obj.AreaName = objlst[0].AreaName;
                obj.MobileNumber = objlst[0].MobileNumber;
                obj.Email = objlst[0].Email;
                obj.PanNo = objlst[0].PanNo;
                string FromDatestr = data.FromDate.ToString("dd/MM/yyyy");
                string ToDatestr = data.ToDate.ToString("dd/MM/yyyy");
                obj.Period = FromDatestr + " To " + ToDatestr;

                obj.FinancialYear = FromFinancialYear + "-" + ToFinancialYear;
                obj.CertificateNo = obj.EmployeeCode + "/" + obj.FinancialYear;
                obj.LastUpdatedOn = DateTime.Now.ToString("dd-MMM-yyyy");

                //  obj.AssessmentYear = FromYearID.ToString() + "-" + ToYearID.ToString();
                obj.AssessmentYear = FromAssessment + "-" + ToAssessment;
                obj.FromEmployer = FromEmployer;
                obj.ToEmployer = ToEmployer;

                // Section 1
                obj.GrandTotal = Math.Round(objlst[0].GrandTotal, 2);
                obj.PerquisitesUnderSection_17_2 = 0;
                obj.SalaryUnderSection_17_3 = 0;
                //obj.TotalGrossSalary = Math.Round((obj.GrandTotal + obj.PerquisitesUnderSection_17_2 + obj.SalaryUnderSection_17_3), 2);
                obj.TotalGrossSalary = Math.Round((obj.GrandTotal + obj.PerquisitesUnderSection_17_2 + obj.SalaryUnderSection_17_3), 0, MidpointRounding.AwayFromZero);
                obj.SalaryReceivedFromOtherEmployer1_e = 0;

                // Section 2
                obj.TravelConcession2_a = 0;
                obj.Gratuity = Math.Round(objlst[0].Gratuity, 2);

                // 23 Feb 2022 Piyush Limbani
                //if (PensionUnderSection10_10A_Actual > obj.PensionUnderSection10_10A)
                //{
                //    obj.CommutedValueofPension2_c = obj.PensionUnderSection10_10A;
                //}
                //else
                //{
                //    obj.CommutedValueofPension2_c = PensionUnderSection10_10A_Actual;
                //}
                // 23 Feb 2022 Piyush Limbani

                obj.CommutedValueofPension2_c = 0;
                obj.LeaveSalaryEncashment2_d = 0;

                // 28 Feb 2022 Piyush Limbani
                obj.HouseRentAllowance2_e = Math.Round(objlst[0].sumEarnedHouseRentAllowance, 2);
                //obj.HouseRentAllowance2_e = 7200.50M;
                //obj.HouseRentAllowance2_e = Math.Round(obj.HouseRentAllowance2_e, 0, MidpointRounding.AwayFromZero);

                //Add By Dhruvik
                obj.HouseRentAllowance2_e = Math.Round(data.FormSixteenEHRA, 0, MidpointRounding.AwayFromZero);
                //Add By Dhruvik

                // 28 Feb 2022 Piyush Limbani

                obj.AmountofAnyOtherExemption2_f = 0;
                obj.TotalAmountofAnyOtherExemption2_g = 0;
                obj.TotalAmountofExemptionClaimed2_h = Math.Round((obj.TravelConcession2_a + obj.Gratuity + obj.CommutedValueofPension2_c + obj.LeaveSalaryEncashment2_d + obj.HouseRentAllowance2_e + obj.AmountofAnyOtherExemption2_f + obj.TotalAmountofAnyOtherExemption2_g), 2);
                obj.TotalAmountofSalary_3 = (obj.TotalGrossSalary - obj.TotalAmountofExemptionClaimed2_h);

                // Section 4
                // obj.StandardDeductions4_a = Math.Round(objlst[0].StandardDeductions4_a, 2);
                obj.EntertainmentAllowance4_b = 0;
                obj.TaxOnEmployment4_c = Math.Round(objlst[0].sumPT, 2);
                obj.TotalAmountOfDeductions_5 = obj.StandardDeductions4_a + obj.EntertainmentAllowance4_b + obj.TaxOnEmployment4_c;
                obj.lncomeChargeable_6 = (obj.TotalAmountofSalary_3 + obj.SalaryReceivedFromOtherEmployer1_e - obj.TotalAmountOfDeductions_5);

                // Section 7
                obj.IncomeFromHouseProperty7_a = 0;
                obj.IncomeUnderTheHead_OtherSources7_b = 0;
                obj.TotalAmountOfOtherIncome_8 = obj.IncomeFromHouseProperty7_a + obj.IncomeUnderTheHead_OtherSources7_b;
                obj.GrossTotalIncome = Math.Round(obj.lncomeChargeable_6 + obj.TotalAmountOfOtherIncome_8, 2);
                // obj.UnderSection80C = Math.Round(objlst[0].UnderSection80C, 2);

                // Section 10
                obj.GrossAmount10_a = Math.Round((data.HousingLoanPrincipal + data.ELSS + data.PPF + data.LifeInsurance + data.Others), 2);
                if (obj.UnderSection80C > obj.GrossAmount10_a)
                {
                    obj.DeductibleAmount10_a = obj.GrossAmount10_a;
                }
                else
                {
                    obj.DeductibleAmount10_a = obj.UnderSection80C;
                }

                obj.GrossAmount10_b = 0;
                obj.DeductibleAmount10_b = 0;


                // 24 Feb 2022 Piyush Limbani
                obj.GrossAmount10_c = data.PensionUnderSection80CCD_1_Actual;
                if (data.PensionUnderSection80CCD_1_Actual > obj.PensionUnderSection80CCD_1)
                {
                    obj.DeductibleAmount10_c = obj.PensionUnderSection80CCD_1;
                }
                else
                {
                    obj.DeductibleAmount10_c = data.PensionUnderSection80CCD_1_Actual;
                }
                // 24 Feb 2022 Piyush Limbani


                //obj.GrossAmount10_c = 0;
                //obj.DeductibleAmount10_c = 0;

                obj.TotalGrossAmount10_d = Math.Round((obj.GrossAmount10_a + obj.GrossAmount10_b + obj.GrossAmount10_c), 2);
                obj.TotalDeductibleAmount10_d = Math.Round((obj.DeductibleAmount10_a + obj.DeductibleAmount10_b + obj.DeductibleAmount10_c), 2);
                obj.GrossAmount10_e = 0;
                obj.DeductibleAmount10_e = 0;
                obj.GrossAmount10_f = 0;
                obj.DeductibleAmount10_f = 0;
                //  obj.HealthInsurancePremiaUnderSection80D = Math.Round(objlst[0].HealthInsurancePremiaUnderSection80D, 2);
                obj.GrossAmount10_g = data.HealthInsurancePremiaUnderSection80D_Actual;
                if (data.HealthInsurancePremiaUnderSection80D_Actual > obj.HealthInsurancePremiaUnderSection80D)
                {
                    obj.DeductibleAmount10_g = obj.HealthInsurancePremiaUnderSection80D;
                }
                else
                {
                    obj.DeductibleAmount10_g = obj.GrossAmount10_g;
                }
                //  obj.InterestOn80E = Math.Round(objlst[0].InterestOn80E, 2);
                obj.GrossAmount10_h = data.InterestOn80E_Actual;
                if (data.InterestOn80E_Actual > obj.InterestOn80E)
                {
                    obj.DeductibleAmount10_h = obj.InterestOn80E;
                }
                else
                {
                    obj.DeductibleAmount10_h = data.InterestOn80E_Actual;
                }
                decimal ICalculate80G = obj.GrossTotalIncome - (obj.TotalDeductibleAmount10_d + obj.DeductibleAmount10_e + obj.DeductibleAmount10_f + obj.DeductibleAmount10_g + obj.DeductibleAmount10_h);
                decimal amount = (ICalculate80G * 10 / 100);
                obj.GrossAmount10_i = data.UnderSection80G;
                if (data.IsFiftyPer == true)
                {
                    obj.QualifyingAmount10_i = data.UnderSection80G * Convert.ToDecimal(0.5);
                }
                else
                {
                    obj.QualifyingAmount10_i = data.UnderSection80G;
                }
                if (obj.QualifyingAmount10_i > amount)
                {
                    obj.DeductibleAmount10_i = amount;
                }
                else
                {
                    obj.DeductibleAmount10_i = obj.QualifyingAmount10_i;
                }
                obj.GrossAmount10_j = data.UnderSection80TTA_Actual;
                if (data.UnderSection80TTA_Actual > obj.UnderSection80TTA)
                {
                    obj.QualifyingAmount10_j = obj.UnderSection80TTA;
                    obj.DeductibleAmount10_j = obj.UnderSection80TTA;
                }
                else
                {
                    obj.QualifyingAmount10_j = data.UnderSection80TTA_Actual;
                    obj.DeductibleAmount10_j = data.UnderSection80TTA_Actual;
                }
                obj.AggregateOfDeductibleAmount_11 = Math.Round((obj.TotalDeductibleAmount10_d + obj.DeductibleAmount10_e + obj.DeductibleAmount10_f + obj.DeductibleAmount10_g + obj.DeductibleAmount10_h + obj.DeductibleAmount10_i + obj.DeductibleAmount10_j), 2);

                obj.TotalTaxableIncome_12 = (obj.GrossTotalIncome - obj.AggregateOfDeductibleAmount_11);  // Remove comment after calculation
                // obj.TotalTaxableIncome_12 = 10100000;
                obj.TotalTaxableIncome_12 = Math.Round(obj.TotalTaxableIncome_12, 0, MidpointRounding.AwayFromZero);


                // sp call for get tax setting   obj.TotalTaxableIncome_12


                var Form16TexIncomeData = _IAttandanceService.GetTexableIncome(obj.TotalTaxableIncome_12);
                if (data.FormSixteenID == 0)
                {
                    obj.IncomeFrom = Form16TexIncomeData.IncomeFrom;
                    obj.IncomeTo = Form16TexIncomeData.IncomeTo;
                    obj.TaxOnTotalIncome = Form16TexIncomeData.TaxOnTotalIncome;
                    obj.TaxOnTotalIncome_one = Form16TexIncomeData.TaxOnTotalIncome_one;
                    obj.RebateUnderSection87A_Income = Form16TexIncomeData.RebateUnderSection87A_Income;
                    obj.Surcharge = Form16TexIncomeData.Surcharge;
                    obj.Education = Form16TexIncomeData.Education;
                }
                else
                {
                    obj.IncomeFrom = data.IncomeFrom;
                    obj.IncomeTo = data.IncomeTo;
                    obj.TaxOnTotalIncome = data.TaxOnTotalIncome;
                    obj.TaxOnTotalIncome_one = data.TaxOnTotalIncome_one;
                    obj.RebateUnderSection87A_Income = data.RebateUnderSection87A_Income;
                    obj.Surcharge = data.Surcharge;
                    obj.Education = data.Education;
                }

                decimal Tax_12 = obj.TotalTaxableIncome_12 - Form16TexIncomeData.IncomeFrom;


                // 16 Feb 2022 Piyush Limbani
                // decimal slabper = 0;
                if (data.FormSixteenID == 0)
                {
                    obj.TaxSlabPer = _IAttandanceService.GetTaxSlabPer(obj.TotalTaxableIncome_12);
                }
                else
                {
                    obj.TaxSlabPer = data.TaxSlabPer;
                }
                // obj.TaxOnTotalIncome_13 = ((obj.TotalTaxableIncome_12 * obj.TaxSlabPer) / 100);
                obj.TaxOnTotalIncome_13 = Math.Round((((Tax_12 * Form16TexIncomeData.TaxOnTotalIncome_one) / 100) + Form16TexIncomeData.TaxOnTotalIncome), 0, MidpointRounding.AwayFromZero);


                if (obj.TotalTaxableIncome_12 < obj.RebateUnderSection87A)
                {
                    obj.RebateUnderSection87A_14 = obj.TaxOnTotalIncome_13;
                }
                else
                {
                    obj.RebateUnderSection87A_14 = 0;
                }



                // decimal surchargeslabpper = 0;
                if (data.FormSixteenID == 0)
                {
                    obj.SurchargeSlabPer = _IAttandanceService.GetSurchargeSlabPer(obj.TotalTaxableIncome_12);
                }
                else
                {
                    obj.SurchargeSlabPer = data.SurchargeSlabPer;
                }
                //obj.Surcharge_15 = ((obj.TaxOnTotalIncome_13 * obj.SurchargeSlabPer) / 100);
                obj.Surcharge_15 = Math.Round(((obj.TaxOnTotalIncome_13 * Form16TexIncomeData.Surcharge) / 100), 0, MidpointRounding.AwayFromZero);

                //  decimal educationslabpper = 0;
                if (data.FormSixteenID == 0)
                {
                    obj.EducationSlabPer = _IAttandanceService.GetEducationSlabPer(obj.TotalTaxableIncome_12);
                }
                else
                {
                    obj.EducationSlabPer = data.EducationSlabPer;
                }
                //obj.Education_16 = (((obj.TaxOnTotalIncome_13 + obj.Surcharge_15) * obj.EducationSlabPer) / 100);

                obj.Education_16 = Math.Round((((obj.TaxOnTotalIncome_13 + obj.Surcharge_15) * Form16TexIncomeData.Education) / 100), 0, MidpointRounding.AwayFromZero);

                obj.TaxPayable_17 = (obj.TaxOnTotalIncome_13 + obj.Surcharge_15 + obj.Education_16) - obj.RebateUnderSection87A_14;
                obj.ReliefUnderSection89_18 = 0;
                obj.NetTaxPayable_19 = (obj.TaxPayable_17 - obj.ReliefUnderSection89_18);


                if (data.FormSixteenID == 0)
                {
                    //  obj.TDSDeducted_20 = objlst[0].sumTDS + objlst[0].TDSSalary;
                    obj.TDSDeducted_20 = objlst[0].sumTDS + data.TDSSalary;
                }
                else
                {
                    obj.TDSDeducted_20 = objlst[0].sumTDS + data.TDSSalary;
                }



                // obj.TotalTaxPayble_21 = obj.NetTaxPayable_19 - obj.TDSDeducted_20;
                obj.TotalTaxPayble_21 = Math.Round((obj.NetTaxPayable_19 - obj.TDSDeducted_20), 0, MidpointRounding.AwayFromZero);
                // 31 Jan 2022 Piyush Limbani


                // 28 Feb 2022 Piyush Limbani
                obj.Date = DateTime.Now.ToString("dd/MM/yyyy");
                // 28 Feb 2022 Piyush Limbani


                //if (data.FormSixteenID == 0)
                //{
                //    bool resposeforadd = _IAttandanceService.AddFormSixteen(data.FromDate, data.ToDate, data.EmployeeCode, obj.StandardDeductions4_a,
                //        data.HousingLoanPrincipal, data.ELSS, data.PPF, data.Others, data.LifeInsurance, obj.HealthInsurancePremiaUnderSection80D, obj.HealthInsurancePremiaUnderSection80D_Actual,
                //        obj.InterestOn80E, obj.InterestOn80E_Actual, data.UnderSection80G, obj.UnderSection80TTA, obj.UnderSection80TTA_Actual, data.IsFiftyPer, obj.RebateUnderSection87A, obj.UnderSection80C,
                //    obj.TaxSlabPer, obj.SurchargeSlabPer, obj.EducationSlabPer, obj.PensionUnderSection80CCD_1, obj.PensionUnderSection80CCD_1_Actual, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                //}
                //else
                //{
                //    // 21 Feb 2022
                //    bool resposeforupdate = _IAttandanceService.UpdateFormSixteenDetail(data.FormSixteenID, data.StandardDeduction, data.HousingLoanPrincipal, data.ELSS, data.PPF, data.LifeInsurance, data.Others,
                //        data.HealthInsurancePremiaUnderSection80D_Actual, data.InterestOn80E_Actual, data.UnderSection80G, data.UnderSection80TTA_Actual, data.IsFiftyPer,
                //        obj.RebateUnderSection87A, obj.UnderSection80C, obj.TaxSlabPer, obj.SurchargeSlabPer, obj.EducationSlabPer,
                //       obj.PensionUnderSection80CCD_1, obj.PensionUnderSection80CCD_1_Actual, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now);
                //}

                if (data.FormSixteenID == 0)
                {
                    bool resposeforadd = _IAttandanceService.AddFormSixteen(obj, data, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                }
                else
                {
                    // 21 Feb 2022
                    bool resposeforupdate = _IAttandanceService.UpdateFormSixteenDetail(obj, data, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now);
                }
                // 31 Jan 2022 Piyush Limbani

                LabelData.Add(obj);

                DataTable header = Common.ToDataTable(LabelData);
                ReportDataSource Form16 = new ReportDataSource("DataSet1", header);
                lr.DataSources.Add(Form16);

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

             "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>1.5cm</MarginTop>" +
                "  <MarginLeft>1cm</MarginLeft>" +
                "  <MarginRight>1cm</MarginRight>" +
                "  <MarginBottom>1.5cm</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                string name = DateTime.Now.Ticks.ToString() + ".pdf";
                // string name = obj.EmployeeCode + "_" + obj.FinancialYear + ".pdf";
                string Pdfpathcreate = Server.MapPath("~/EmployeeForm16/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/EmployeeForm16/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/EmployeeForm16/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/EmployeeForm16/" + name;
                }

                // Send Email               
                // SendEmailToEmployee(name, obj.Email, obj.EmployeeName, obj.EmployeeCode, obj.FinancialYear, obj.AssessmentYear, obj.FromEmployer, obj.ToEmployer);

                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public static string SendEmailToEmployee(string name, string Email, string EmployeeName, long EmployeeCode, string FinancialYear, string AssessmentYear, string FromEmployer, string ToEmployer)
        {
            try
            {
                try
                {
                    string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/EmployeeForm16/") + name);

                    //  WriteToFile("strat send email");

                    string logo = "<img src = 'https://system.virakibrothers.com/dist/img/viraki-logo1.png' /><br /><br /><div style = 'border-top:3px solid #41a69a'>&nbsp;</div>";
                    string FromMail = ConfigurationManager.AppSettings["EMailForForm16"];
                    string Tomail = Email;
                    MailMessage mailmessage = new MailMessage();
                    mailmessage.From = new MailAddress(FromMail);
                    mailmessage.Subject = "Form 16 From Viraki Brothers";
                    // mailmessage.CC.Add(System.Configuration.ConfigurationManager.AppSettings["CCMail"]);      
                    mailmessage.Body = logo + "Hello  " + " " + EmployeeName + " (" + Convert.ToString(EmployeeCode) + ")" + " ,"
                        + "<br><br>Please download your Form No. 16 - Part B :"
                       + "<br><br>Financial Year : " + FinancialYear
                         + "<br><br>Assessment Year : " + AssessmentYear
                           + "<br><br>Period with the Employer : " + FromEmployer + " To " + ToEmployer
                        + "  <br><br>For any query, feel free to contact us on accounts@virakibrothers.com <br><br>"
                       + "<br><br>Thank you,<br>Viraki Brothers";
                    mailmessage.Attachments.Add(new Attachment(path));
                    mailmessage.To.Add(Tomail);
                    mailmessage.IsBodyHtml = true;
                    var smtp = new System.Net.Mail.SmtpClient();
                    {
                        smtp = GetSMPTP(smtp);
                    }
                    try
                    {
                        smtp.Send(mailmessage);
                        //  WriteToFile("email sent succ");
                    }
                    catch (Exception ex)
                    {
                        // WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                    }
                }
                catch (Exception ex)
                {
                    // WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                }
                return "Send Email Sent succesfully..";
            }
            catch (Exception ex)
            {
                // WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                return "Sending Email fail due to " + ex.Message.ToString() + " & " + ex.Message.ToString();
            }
        }

        #region Email SMTP configuration
        public static System.Net.Mail.SmtpClient GetSMPTP(System.Net.Mail.SmtpClient OLDSMTP)
        {
            string FromMail = ConfigurationManager.AppSettings["EMailForForm16"];
            string FromPassword = ConfigurationManager.AppSettings["PasswordForForm16"];
            if (FromMail == "accounts@virakibrothers.com")
            {
                OLDSMTP.Host = "smtp.office365.com";
            }
            else
            {
                OLDSMTP.Host = "smtp.gmail.com";
            }
            OLDSMTP.Port = 587;
            OLDSMTP.EnableSsl = true;
            OLDSMTP.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            OLDSMTP.Credentials = new NetworkCredential(FromMail, FromPassword);
            OLDSMTP.Timeout = 20000;
            return OLDSMTP;
        }
        #endregion


        // 31 Jan 2022 Piyush Limbani
        public JsonResult CheckIsExistsFormSixteenDetails(DateTime FromDate, DateTime ToDate, long EmployeeCode)
        {
            try
            {
                var detail = _IAttandanceService.CheckIsExistsFormSixteenDetails(FromDate, ToDate, EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        // 31 Jan 2022 Piyush Limbani

        [HttpPost]
        public PartialViewResult FormSixteenList(YearlySalarySheetReq model)
        {
            List<FormSixteenValueModel> objlst = _IAttandanceService.GetFormSixteenByDate(model.FromDate, model.ToDate, model.EmployeeCode);
            return PartialView(objlst);
        }





        [HttpPost]
        public ActionResult SendFormSixteenEmail(EmployeeFormSixteenDataReq data)
        {
            int FromMonthID = data.FromDate.Month;
            int FromYearID = data.FromDate.Year;
            int ToMonthID = data.ToDate.Month;
            int ToYearID = data.ToDate.Year;

            string FromEmployer = data.FromDate.ToString("dd-MMM-yyyy");
            string ToEmployer = data.ToDate.ToString("dd-MMM-yyyy");

            string FromFinancialYear = data.FromDate.ToString("yyyy");
            string ToFinancialYear = data.ToDate.ToString("yy");

            string FromAssessment = data.FromDate.AddYears(1).ToString("yyyy");
            string ToAssessment = data.ToDate.AddYears(1).ToString("yy");

            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/EmployeeForm16.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/EmployeeForm16.rdlc");
                }
                lr.ReportPath = path;
                var objlst = _IAttandanceService.GetYearlySalarySheetList(FromMonthID, FromYearID, ToMonthID, ToYearID, data.EmployeeCode, data.FromDate, data.ToDate);
                List<PrintYearlySalarySheet> LabelData = new List<PrintYearlySalarySheet>();
                PrintYearlySalarySheet obj = new PrintYearlySalarySheet();

                if (data.FormSixteenID == 0)
                {
                    var objlst2 = _IAttandanceService.GetFormSixteenDetailFromMaster();
                    obj.StandardDeductions4_a = Math.Round(objlst2.StandardDeduction, 2);
                    obj.UnderSection80C = Math.Round(objlst2.UnderSection80C, 2);
                    obj.HealthInsurancePremiaUnderSection80D = Math.Round(objlst2.HealthInsurancePremiaUnderSection80D, 2);
                    obj.HealthInsurancePremiaUnderSection80D_Actual = data.HealthInsurancePremiaUnderSection80D_Actual;
                    obj.InterestOn80E = Math.Round(objlst2.InterestOn80E, 2);
                    obj.InterestOn80E_Actual = data.InterestOn80E_Actual;
                    obj.UnderSection80TTA = objlst2.UnderSection80TTA;
                    obj.UnderSection80TTA_Actual = data.UnderSection80TTA_Actual;
                    obj.RebateUnderSection87A = objlst2.RebateUnderSection87A;
                    obj.PensionUnderSection80CCD_1 = objlst2.PensionUnderSection80CCD_1;
                    obj.PensionUnderSection80CCD_1_Actual = data.PensionUnderSection80CCD_1_Actual;
                }
                else
                {
                    obj.StandardDeductions4_a = data.StandardDeduction;
                    obj.UnderSection80C = data.UnderSection80C;
                    obj.HealthInsurancePremiaUnderSection80D = data.HealthInsurancePremiaUnderSection80D;
                    obj.HealthInsurancePremiaUnderSection80D_Actual = data.HealthInsurancePremiaUnderSection80D_Actual;
                    obj.InterestOn80E = data.InterestOn80E;
                    obj.InterestOn80E_Actual = data.InterestOn80E_Actual;
                    obj.UnderSection80TTA = data.UnderSection80TTA;
                    obj.UnderSection80TTA_Actual = data.UnderSection80TTA_Actual;
                    obj.RebateUnderSection87A = data.RebateUnderSection87A;
                    obj.PensionUnderSection80CCD_1 = data.PensionUnderSection80CCD_1;
                    obj.PensionUnderSection80CCD_1_Actual = data.PensionUnderSection80CCD_1_Actual;
                }
                obj.EmployeeCode = objlst[0].EmployeeCode;
                obj.EmployeeName = objlst[0].EmployeeName;
                obj.PrimaryAddress = objlst[0].PrimaryAddress;
                obj.PrimaryPin = objlst[0].PrimaryPin;
                obj.AreaName = objlst[0].AreaName;
                obj.MobileNumber = objlst[0].MobileNumber;
                obj.Email = objlst[0].Email;
                obj.PanNo = objlst[0].PanNo;
                string FromDatestr = data.FromDate.ToString("dd/MM/yyyy");
                string ToDatestr = data.ToDate.ToString("dd/MM/yyyy");
                obj.Period = FromDatestr + " To " + ToDatestr;
                obj.FinancialYear = FromFinancialYear + "-" + ToFinancialYear;
                obj.CertificateNo = obj.EmployeeCode + "/" + obj.FinancialYear;
                obj.LastUpdatedOn = DateTime.Now.ToString("dd-MMM-yyyy");

                //  obj.AssessmentYear = FromYearID.ToString() + "-" + ToYearID.ToString();
                obj.AssessmentYear = FromAssessment + "-" + ToAssessment;
                obj.FromEmployer = FromEmployer;
                obj.ToEmployer = ToEmployer;

                // Section 1
                obj.GrandTotal = Math.Round(objlst[0].GrandTotal, 2);
                obj.PerquisitesUnderSection_17_2 = 0;
                obj.SalaryUnderSection_17_3 = 0;
                obj.TotalGrossSalary = Math.Round((obj.GrandTotal + obj.PerquisitesUnderSection_17_2 + obj.SalaryUnderSection_17_3), 0, MidpointRounding.AwayFromZero);
                obj.SalaryReceivedFromOtherEmployer1_e = 0;

                // Section 2
                obj.TravelConcession2_a = 0;
                obj.Gratuity = Math.Round(objlst[0].Gratuity, 2);

                obj.CommutedValueofPension2_c = 0;
                obj.LeaveSalaryEncashment2_d = 0;

                // 28 Feb 2022 Piyush Limbani
                obj.HouseRentAllowance2_e = Math.Round(objlst[0].sumEarnedHouseRentAllowance, 2);
                obj.HouseRentAllowance2_e = Math.Round(obj.HouseRentAllowance2_e, 0, MidpointRounding.AwayFromZero);
                // 28 Feb 2022 Piyush Limbani

                obj.AmountofAnyOtherExemption2_f = 0;
                obj.TotalAmountofAnyOtherExemption2_g = 0;
                obj.TotalAmountofExemptionClaimed2_h = Math.Round((obj.TravelConcession2_a + obj.Gratuity + obj.CommutedValueofPension2_c + obj.LeaveSalaryEncashment2_d + obj.HouseRentAllowance2_e + obj.AmountofAnyOtherExemption2_f + obj.TotalAmountofAnyOtherExemption2_g), 2);
                obj.TotalAmountofSalary_3 = (obj.TotalGrossSalary - obj.TotalAmountofExemptionClaimed2_h);

                // Section 4
                // obj.StandardDeductions4_a = Math.Round(objlst[0].StandardDeductions4_a, 2);
                obj.EntertainmentAllowance4_b = 0;
                obj.TaxOnEmployment4_c = Math.Round(objlst[0].sumPT, 2);
                obj.TotalAmountOfDeductions_5 = obj.StandardDeductions4_a + obj.EntertainmentAllowance4_b + obj.TaxOnEmployment4_c;
                obj.lncomeChargeable_6 = (obj.TotalAmountofSalary_3 + obj.SalaryReceivedFromOtherEmployer1_e - obj.TotalAmountOfDeductions_5);

                // Section 7
                obj.IncomeFromHouseProperty7_a = 0;
                obj.IncomeUnderTheHead_OtherSources7_b = 0;
                obj.TotalAmountOfOtherIncome_8 = obj.IncomeFromHouseProperty7_a + obj.IncomeUnderTheHead_OtherSources7_b;
                obj.GrossTotalIncome = Math.Round(obj.lncomeChargeable_6 + obj.TotalAmountOfOtherIncome_8, 2);
                // obj.UnderSection80C = Math.Round(objlst[0].UnderSection80C, 2);

                // Section 10
                obj.GrossAmount10_a = Math.Round((data.HousingLoanPrincipal + data.ELSS + data.PPF + data.LifeInsurance + data.Others), 2);
                if (obj.UnderSection80C > obj.GrossAmount10_a)
                {
                    obj.DeductibleAmount10_a = obj.GrossAmount10_a;
                }
                else
                {
                    obj.DeductibleAmount10_a = obj.UnderSection80C;
                }

                obj.GrossAmount10_b = 0;
                obj.DeductibleAmount10_b = 0;

                // 24 Feb 2022 Piyush Limbani
                obj.GrossAmount10_c = data.PensionUnderSection80CCD_1_Actual;
                if (data.PensionUnderSection80CCD_1_Actual > obj.PensionUnderSection80CCD_1)
                {
                    obj.DeductibleAmount10_c = obj.PensionUnderSection80CCD_1;
                }
                else
                {
                    obj.DeductibleAmount10_c = data.PensionUnderSection80CCD_1_Actual;
                }
                // 24 Feb 2022 Piyush Limbani

                obj.TotalGrossAmount10_d = Math.Round((obj.GrossAmount10_a + obj.GrossAmount10_b + obj.GrossAmount10_c), 2);
                obj.TotalDeductibleAmount10_d = Math.Round((obj.DeductibleAmount10_a + obj.DeductibleAmount10_b + obj.DeductibleAmount10_c), 2);
                obj.GrossAmount10_e = 0;
                obj.DeductibleAmount10_e = 0;
                obj.GrossAmount10_f = 0;
                obj.DeductibleAmount10_f = 0;
                //  obj.HealthInsurancePremiaUnderSection80D = Math.Round(objlst[0].HealthInsurancePremiaUnderSection80D, 2);
                obj.GrossAmount10_g = data.HealthInsurancePremiaUnderSection80D_Actual;
                if (data.HealthInsurancePremiaUnderSection80D_Actual > obj.HealthInsurancePremiaUnderSection80D)
                {
                    obj.DeductibleAmount10_g = obj.HealthInsurancePremiaUnderSection80D;
                }
                else
                {
                    obj.DeductibleAmount10_g = obj.GrossAmount10_g;
                }
                //  obj.InterestOn80E = Math.Round(objlst[0].InterestOn80E, 2);
                obj.GrossAmount10_h = data.InterestOn80E_Actual;
                if (data.InterestOn80E_Actual > obj.InterestOn80E)
                {
                    obj.DeductibleAmount10_h = obj.InterestOn80E;
                }
                else
                {
                    obj.DeductibleAmount10_h = data.InterestOn80E_Actual;
                }
                decimal ICalculate80G = obj.GrossTotalIncome - (obj.TotalDeductibleAmount10_d + obj.DeductibleAmount10_e + obj.DeductibleAmount10_f + obj.DeductibleAmount10_g + obj.DeductibleAmount10_h);
                decimal amount = (ICalculate80G * 10 / 100);
                obj.GrossAmount10_i = data.UnderSection80G;
                if (data.IsFiftyPer == true)
                {
                    obj.QualifyingAmount10_i = data.UnderSection80G * Convert.ToDecimal(0.5);
                }
                else
                {
                    obj.QualifyingAmount10_i = data.UnderSection80G;
                }
                if (obj.QualifyingAmount10_i > amount)
                {
                    obj.DeductibleAmount10_i = amount;
                }
                else
                {
                    obj.DeductibleAmount10_i = obj.QualifyingAmount10_i;
                }
                obj.GrossAmount10_j = data.UnderSection80TTA_Actual;
                if (data.UnderSection80TTA_Actual > obj.UnderSection80TTA)
                {
                    obj.QualifyingAmount10_j = obj.UnderSection80TTA;
                    obj.DeductibleAmount10_j = obj.UnderSection80TTA;
                }
                else
                {
                    obj.QualifyingAmount10_j = data.UnderSection80TTA_Actual;
                    obj.DeductibleAmount10_j = data.UnderSection80TTA_Actual;
                }
                obj.AggregateOfDeductibleAmount_11 = Math.Round((obj.TotalDeductibleAmount10_d + obj.DeductibleAmount10_e + obj.DeductibleAmount10_f + obj.DeductibleAmount10_g + obj.DeductibleAmount10_h + obj.DeductibleAmount10_i + obj.DeductibleAmount10_j), 2);

                obj.TotalTaxableIncome_12 = (obj.GrossTotalIncome - obj.AggregateOfDeductibleAmount_11);  // Remove comment after calculation
                // obj.TotalTaxableIncome_12 = 10100000;
                obj.TotalTaxableIncome_12 = Math.Round(obj.TotalTaxableIncome_12, 0, MidpointRounding.AwayFromZero);

                var Form16TexIncomeData = _IAttandanceService.GetTexableIncome(obj.TotalTaxableIncome_12);

                if (data.FormSixteenID == 0)
                {
                    obj.IncomeFrom = Form16TexIncomeData.IncomeFrom;
                    obj.IncomeTo = Form16TexIncomeData.IncomeTo;
                    obj.TaxOnTotalIncome = Form16TexIncomeData.TaxOnTotalIncome;
                    obj.TaxOnTotalIncome_one = Form16TexIncomeData.TaxOnTotalIncome_one;
                    obj.RebateUnderSection87A_Income = Form16TexIncomeData.RebateUnderSection87A_Income;
                    obj.Surcharge = Form16TexIncomeData.Surcharge;
                    obj.Education = Form16TexIncomeData.Education;
                }
                else
                {
                    obj.IncomeFrom = data.IncomeFrom;
                    obj.IncomeTo = data.IncomeTo;
                    obj.TaxOnTotalIncome = data.TaxOnTotalIncome;
                    obj.TaxOnTotalIncome_one = data.TaxOnTotalIncome_one;
                    obj.RebateUnderSection87A_Income = data.RebateUnderSection87A_Income;
                    obj.Surcharge = data.Surcharge;
                    obj.Education = data.Education;
                }

                decimal Tax_12 = obj.TotalTaxableIncome_12 - Form16TexIncomeData.IncomeFrom;

                // 16 Feb 2022 Piyush Limbani
                // decimal slabper = 0;
                if (data.FormSixteenID == 0)
                {
                    obj.TaxSlabPer = _IAttandanceService.GetTaxSlabPer(obj.TotalTaxableIncome_12);
                }
                else
                {
                    obj.TaxSlabPer = data.TaxSlabPer;
                }

                //obj.TaxOnTotalIncome_13 = Math.Round(((obj.TotalTaxableIncome_12 * obj.TaxSlabPer) / 100), 0, MidpointRounding.AwayFromZero);
                obj.TaxOnTotalIncome_13 = Math.Round((((Tax_12 * Form16TexIncomeData.TaxOnTotalIncome_one) / 100) + Form16TexIncomeData.TaxOnTotalIncome), 0, MidpointRounding.AwayFromZero);
                
                if (obj.TotalTaxableIncome_12 < obj.RebateUnderSection87A)
                {
                    obj.RebateUnderSection87A_14 = obj.TaxOnTotalIncome_13;
                }
                else
                {
                    obj.RebateUnderSection87A_14 = 0;
                }

                // decimal surchargeslabpper = 0;
                if (data.FormSixteenID == 0)
                {
                    obj.SurchargeSlabPer = _IAttandanceService.GetSurchargeSlabPer(obj.TotalTaxableIncome_12);
                }
                else
                {
                    obj.SurchargeSlabPer = data.SurchargeSlabPer;
                }
                obj.Surcharge_15 = Math.Round(((obj.TaxOnTotalIncome_13 * Form16TexIncomeData.Surcharge) / 100), 0, MidpointRounding.AwayFromZero);

                //  decimal educationslabpper = 0;
                if (data.FormSixteenID == 0)
                {
                    obj.EducationSlabPer = _IAttandanceService.GetEducationSlabPer(obj.TotalTaxableIncome_12);
                }
                else
                {
                    obj.EducationSlabPer = data.EducationSlabPer;
                }
                obj.Education_16 = Math.Round((((obj.TaxOnTotalIncome_13 + obj.Surcharge_15) * Form16TexIncomeData.Education) / 100), 0, MidpointRounding.AwayFromZero);
                obj.TaxPayable_17 = (obj.TaxOnTotalIncome_13 + obj.Surcharge_15 + obj.Education_16) - obj.RebateUnderSection87A_14;
                obj.ReliefUnderSection89_18 = 0;
                obj.NetTaxPayable_19 = (obj.TaxPayable_17 - obj.ReliefUnderSection89_18);

                if (data.FormSixteenID == 0)
                {
                    //  obj.TDSDeducted_20 = objlst[0].sumTDS + objlst[0].TDSSalary;
                    obj.TDSDeducted_20 = objlst[0].sumTDS + data.TDSSalary;
                }
                else
                {
                    obj.TDSDeducted_20 = objlst[0].sumTDS + data.TDSSalary;
                }

                obj.TotalTaxPayble_21 = Math.Round((obj.NetTaxPayable_19 - obj.TDSDeducted_20), 0, MidpointRounding.AwayFromZero);
                // 31 Jan 2022 Piyush Limbani

                // 28 Feb 2022 Piyush Limbani
                obj.Date = DateTime.Now.ToString("dd/MM/yyyy");
                // 28 Feb 2022 Piyush Limbani


                LabelData.Add(obj);

                DataTable header = Common.ToDataTable(LabelData);
                ReportDataSource Form16 = new ReportDataSource("DataSet1", header);
                lr.DataSources.Add(Form16);

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

             "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>1.5cm</MarginTop>" +
                "  <MarginLeft>1cm</MarginLeft>" +
                "  <MarginRight>1cm</MarginRight>" +
                "  <MarginBottom>1.5cm</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                string name = DateTime.Now.Ticks.ToString() + ".pdf";
                string Pdfpathcreate = Server.MapPath("~/EmployeeForm16/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/EmployeeForm16/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/EmployeeForm16/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/EmployeeForm16/" + name;
                }

                // Send Email
                // SendEmailToEmployee(url, obj.Email, obj.EmployeeName, obj.FinancialYear, obj.AssessmentYear, obj.FromEmployer, obj.ToEmployer);
                string email = SendEmailToEmployee(name, obj.Email, obj.EmployeeName, obj.EmployeeCode, obj.FinancialYear, obj.AssessmentYear, obj.FromEmployer, obj.ToEmployer);
                // SendEmailToEmployee(item1.Email, LstInvoiceNo, Total, LstPDF, PdfPath, item1.MobileNumber);


                return Json(email, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }




    }
}