using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
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
using vb.Data.ViewModel;
using vb.Service;

namespace vbwebsite.Areas.attendance.Controllers
{
    public class LeavesController : Controller
    {
        private IAdminService _adminservice;
        private ICommonService _ICommonService;
        private IAttandanceService _IAttandanceService;

        public LeavesController(IAdminService adminservice, ICommonService commonservice, IAttandanceService attandanceservice)
        {
            _adminservice = adminservice;
            _ICommonService = commonservice;
            _IAttandanceService = attandanceservice;
        }

        //
        // GET: /attendance/Leavs/
        public ActionResult Index()
        {
            return View();
        }

        //04-05-2020
        public ActionResult LeaveAndAdvanceApplication()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        public JsonResult GetClosingLeavesandClosingAdvance(long EmployeeCode)
        {
            try
            {
                var detail = _IAttandanceService.GetClosingLeavesandClosingAdvance(EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddLeaveAndAdvanceApplication(AddLeaveAndAdvanceApplication data)
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
                    LeaveApplication_Mst obj = new LeaveApplication_Mst();
                    obj.LeaveApplicationID = data.LeaveApplicationID;
                    obj.EmployeeCode = data.EmployeeCode;
                    obj.ClosingLeaves = data.ClosingLeaves;
                    obj.ClosingAdvance = data.ClosingAdvance;
                    obj.ApplicationDate = data.ApplicationDate;
                    obj.FromDate = data.FromDate;
                    obj.ToDate = data.ToDate;
                    obj.Reason = data.Reason;
                    obj.GoingTo = data.GoingTo;
                    obj.AdvanceAmount = data.AdvanceAmount;
                    obj.AdvanceReason = data.AdvanceReason;
                    obj.DeductionPerMonthAmount = data.DeductionPerMonthAmount;
                    obj.LeaveStatusID = 1;

                    obj.IsAdvanceAmount = data.IsAdvanceAmount;

                    if (obj.LeaveApplicationID == 0)
                    {
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                    }
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsActive = false;
                    long respose = _IAttandanceService.AddLeaveAndAdvanceApplication(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchLeaveAndAdvanceApplication()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchLeaveAndAdvanceApplicationList(SearchLeaveApplication model)
        {
            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<LeaveApplicationListResponse> objModel = _IAttandanceService.GetLeaveAndAdvanceApplicationList(model.FromDate, model.ToDate, model.EmployeeCode);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult PrintLeaveApplication(long LeaveApplicationID, DateTime? ToDate)
        {
            try
            {
                string NumberToWord = "";
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/LeaveAndAdvanceApplication.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/LeaveAndAdvanceApplication.rdlc");
                }
                lr.ReportPath = path;
                var LeaveApplicationDetail = _IAttandanceService.GetDataForLeaveApplicationPrint(LeaveApplicationID);
                List<LeaveApplicationListResponse> LabelData = new List<LeaveApplicationListResponse>();
                LeaveApplicationListResponse obj = new LeaveApplicationListResponse();
                obj.EmployeeName = LeaveApplicationDetail.EmployeeName;
                obj.PrimaryAddress = LeaveApplicationDetail.PrimaryAddress;
                obj.AreaName = LeaveApplicationDetail.AreaName;
                obj.PrimaryPin = LeaveApplicationDetail.PrimaryPin;
                obj.MobileNumber = LeaveApplicationDetail.MobileNumber;
                obj.EmployeeCode = LeaveApplicationDetail.EmployeeCode;
                obj.DateOfJoiningstr = LeaveApplicationDetail.DateOfJoiningstr;
                obj.ApplicationDate = LeaveApplicationDetail.ApplicationDate;
                string Month = obj.ApplicationDate.Month.ToString();
                string Year = obj.ApplicationDate.Year.ToString();
                MonthList OBJMONTHS = _IAttandanceService.GetLastTwelveMonthDataForLeavePrint(Convert.ToInt32(Month), Convert.ToInt32(Year), obj.EmployeeCode);
                List<MonthList> LeaveMonthwise = new List<MonthList>();
                LeaveMonthwise.Add(OBJMONTHS);
                obj.TotalLeave = OBJMONTHS.Total;

               
                if (ToDate == null)
                {
                    ToDate = Convert.ToDateTime(DateTime.Now);
                }

                //string Month = ToDate.Month.ToString();

                //string syear = DateTime.Now.Year.ToString();
                //string StartDate = syear + "-" + "04" + "-" + "01";
                //string eyear = DateTime.Now.AddYears(1).Year.ToString();
                //string EndDate = eyear + "-" + "03" + "-" + "31";


                //ClosingAdvanceMonthList ClosingAdvanceTwelveMonth = _IAttandanceService.GetLastTwelveMonthClosingAdvanceDataForLeavePrint(Convert.ToInt32(Month), Convert.ToInt32(Year), obj.EmployeeCode);

                ClosingAdvanceMonthList ClosingAdvanceTwelveMonth = _IAttandanceService.GetLastTwelveMonthClosingAdvanceDataForLeavePrint(ToDate, obj.EmployeeCode);    
                List<ClosingAdvanceMonthList> ClosingAdvanceMonthwise = new List<ClosingAdvanceMonthList>();
                ClosingAdvanceMonthwise.Add(ClosingAdvanceTwelveMonth);
                obj.TotalClosingAdvance = ClosingAdvanceTwelveMonth.TotalClosingAdvance;

                obj.ApplicationDatestr = LeaveApplicationDetail.ApplicationDatestr;
                obj.GodownAddress1 = LeaveApplicationDetail.GodownAddress1;
                obj.GodownAddress2 = LeaveApplicationDetail.GodownAddress2;
                obj.Place = LeaveApplicationDetail.Place;
                obj.Pincode = LeaveApplicationDetail.Pincode;
                obj.State = LeaveApplicationDetail.State;

                //obj.Subject = "विषय : रजा मंजुर करण्याबाबत विनंती अर्ज";
                //obj.Sir = "महाशय,";
                //obj.Line1 = "मी खाली सही करणार श्री.";
                //obj.Line2 = "आपणांस विनंती अर्ज करीतो कि,";
                //obj.FromDateLable = "मला दिनांक";
                obj.FromDatestr = LeaveApplicationDetail.FromDatestr;
                //obj.ToLable = "पासून";
                obj.ToDatestr = LeaveApplicationDetail.ToDatestr;
                //obj.ToDateLable = "पर्यंत,";
                obj.Reason = LeaveApplicationDetail.Reason;
                //obj.ReasonLable = "कारणासाठी, रजा मिळावी हि विनंती.";
                obj.ESIC = LeaveApplicationDetail.ESIC;
                //obj.Line7 = "आजारी असल्यास रजेवर रुजू होताना, E.S.I.C. CERTIFICATE आणल्याशिवाय कामावर रुजू होता येणार नाही,";
                //obj.GoingToLable = "मुंबई बाहेर जात असल्यास तेथील पत्ता :";
                obj.GoingTo = LeaveApplicationDetail.GoingTo;
                //obj.AdvanceLable = "विषय : उचल मिळण्याबाबत विनंती अर्ज";
                obj.AdvanceAmount = LeaveApplicationDetail.AdvanceAmount;
                //obj.MalaLabel = "मला रुपये";

                //obj.AdvanceAmountInWordsLabel = "अक्षर";
                int number = Convert.ToInt32(obj.AdvanceAmount);
                NumberToWord = NumberToWords(number);
                obj.AdvanceAmountInWords = NumberToWord + "  " + "Only/-";
                //obj.Line3 = "उचल घर कामासाठी";
                obj.AdvanceReason = LeaveApplicationDetail.AdvanceReason;
                //obj.Line6 = "दयावी हि विनंती,";
                //obj.Line4 = "उचल माझ्या पगारातून दरमहा  रुपये";
                obj.DeductionPerMonthAmount = LeaveApplicationDetail.DeductionPerMonthAmount;
                //obj.Line5 = "कपात करून घ्यावी.";
                //obj.NoteLabel = "नोट.";
                //obj.NoteLine1 = "रजेसाठी १५ दिवस अगोदर अर्ज करावा.";
                //obj.NoteLine2 = "मंजूर झालेल्या रजेनंतर कोणतेही योग्य कारण न देता, कामावर रुजू न झाल्यास,";
                //obj.NoteLine3 = "आपणांस सेवेतून मुक्त करण्यात येईल.";             
                //obj.NoteLine4 = "पूर्व परवानगीशिवाय किंवा न कळवता सलग १० दिवस सुट्टीचे दिवस धरुन  कामावर गैहजर राहिल्यास,";
                //obj.NoteLine5 = " आपणांस सेवेतून मुक्त करण्यात येईल.";
                //obj.VirakiMarathi = "विरकी ब्रदर्स";
                //obj.YoursFaithfully = "आपला विश्वासू";

                obj.ApprovalFromDatestr = LeaveApplicationDetail.ApprovalFromDatestr;
                obj.ApprovalToDatestr = LeaveApplicationDetail.ApprovalToDatestr;
                obj.ApprovalAdvanceAmount = LeaveApplicationDetail.ApprovalAdvanceAmount;
                obj.LeaveStatus = LeaveApplicationDetail.LeaveStatus;
                obj.ApprovedBy = LeaveApplicationDetail.ApprovedBy;
                obj.ApprovalDatestr = LeaveApplicationDetail.ApprovalDatestr;

                LabelData.Add(obj);

                DataTable header = Common.ToDataTable(LabelData);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                lr.DataSources.Add(MedsheetHeader);

                DataTable InwardOutward = Common.ToDataTable(LeaveMonthwise);
                ReportDataSource DataRecord = new ReportDataSource("DataSet2", InwardOutward);
                lr.DataSources.Add(DataRecord);

                DataTable ClosingAdvance = Common.ToDataTable(ClosingAdvanceMonthwise);
                ReportDataSource DataRecord2 = new ReportDataSource("DataSet3", ClosingAdvance);
                lr.DataSources.Add(DataRecord2);

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

             "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.4cm</MarginTop>" +
                "  <MarginLeft>1cm</MarginLeft>" +
                "  <MarginRight>1cm</MarginRight>" +
                "  <MarginBottom>0.4cm</MarginBottom>" +
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
                string Pdfpathcreate = Server.MapPath("~/LeaveAndAdvanceApplication/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/LeaveAndAdvanceApplication/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/LeaveAndAdvanceApplication/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/LeaveAndAdvanceApplication/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult UpdateApprovalLeave(ApprovalLeave data)
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
                    bool respose = _IAttandanceService.UpdateApprovalLeave(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        // 7 July 2020 Piyush Limbani
        public ActionResult LeaveApproval()
        {
            // fileter by Aprroval from date
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Role = _adminservice.GetAllRoleName();
            return View();
        }

        [HttpPost]
        public PartialViewResult LeaveApprovalList(SearchLeaveApproval model)
        {
            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<LeaveApprovalListResponse> objModel = _IAttandanceService.GetLeaveApprovalList(model.FromDate, model.ToDate, model.EmployeeCode, model.GodownID, model.RoleID);
            return PartialView(objModel);
        }




        // 9 July 2020 Piyush Limbani
        public ActionResult ExportExcelLeaveApprovalList(SearchLeaveApproval model)
        {

            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<LeaveApprovalListResponse> LeaveApprovalList = _IAttandanceService.GetLeaveApprovalList(model.FromDate, model.ToDate, model.EmployeeCode, model.GodownID, model.RoleID);
            List<LeaveApprovalListForExp> lstallowancelist = LeaveApprovalList.Select(x => new LeaveApprovalListForExp()
            {
                EmployeeName = x.EmployeeName,
                Designation = x.Designation,
                ApplicationDate = x.ApplicationDate.ToString("dd-MM-yyyy"),
                FromDate = x.FromDate.ToString("dd-MM-yyyy"),
                ToDate = x.ToDate.ToString("dd-MM-yyyy"),
                Reason = x.Reason,
                GoingTo = x.GoingTo,
                AdvanceAmount = x.AdvanceAmount,
                AdvanceReason = x.AdvanceReason,
                DeductionPerMonthAmount = x.DeductionPerMonthAmount,
                ApprovalFromDate = x.ApprovalFromDate.ToString("dd-MM-yyyy"),
                ApprovalToDate = x.ApprovalToDate.ToString("dd-MM-yyyy"),
                ApprovalAdvanceAmount = x.ApprovalAdvanceAmount
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "LeaveApprovalList.xls");
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

        // 11 July 2020 Piyush Limbani
        public ActionResult ExportExcelLeaveAndAdvanceApplicationList(SearchLeaveApplication model)
        {

            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<LeaveApplicationListResponse> LeaveApplicationList = _IAttandanceService.GetLeaveAndAdvanceApplicationList(model.FromDate, model.ToDate, model.EmployeeCode);
            List<LeaveApplicationListForExp> lstallowancelist = LeaveApplicationList.Select(x => new LeaveApplicationListForExp()
            {
                EmployeeName = x.EmployeeName,
                ApplicationDate = x.ApplicationDate.ToString("dd-MM-yyyy"),
                FromDate = x.FromDate.ToString("dd-MM-yyyy"),
                ToDate = x.ToDate.ToString("dd-MM-yyyy"),
                Reason = x.Reason,
                GoingTo = x.GoingTo,
                AdvanceAmount = x.AdvanceAmount,
                AdvanceReason = x.AdvanceReason,
                DeductionPerMonthAmount = x.DeductionPerMonthAmount,
                ApprovalFromDate = x.ApprovalFromDate.ToString("dd-MM-yyyy"),
                ApprovalToDate = x.ApprovalToDate.ToString("dd-MM-yyyy"),
                ApprovalAdvanceAmount = x.ApprovalAdvanceAmount
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "LeaveApplicationList.xls");
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
    }
}