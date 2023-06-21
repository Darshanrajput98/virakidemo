using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;
using vb.Service;

namespace vbwebsite.Areas.attendance.Controllers
{
    public class ResignationController : Controller
    {
        private IAdminService _adminservice;
        private ICommonService _ICommonService;
        private IAttandanceService _IAttandanceService;

        public ResignationController(IAdminService adminservice, ICommonService commonservice, IAttandanceService attandanceservice)
        {
            _adminservice = adminservice;
            _ICommonService = commonservice;
            _IAttandanceService = attandanceservice;
        }

        //
        // GET: /attendance/Resignation/
        public ActionResult Index()
        {
            return View();
        }

        // 25 Dec 2020 Piyush Limbani
        public ActionResult ResignationLetter()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        public JsonResult GetEmployeeDetailByEmployeeCodeForResignation(long EmployeeCode)
        {
            try
            {
                var detail = _IAttandanceService.GetEmployeeDetailByEmployeeCodeForResignation(EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddResignationLetter(AddResignationLetter data)
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
                    Resignation_Mst obj = new Resignation_Mst();
                    obj.ResignationID = data.ResignationID;
                    obj.EmployeeCode = data.EmployeeCode;
                    obj.DateOfJoining = data.DateOfJoining;
                    obj.DateOfLeaving = data.DateOfLeaving;
                    obj.DateOfApplication = data.DateOfApplication;
                    obj.Status = data.Status;
                    if (obj.ResignationID == 0)
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
                    obj.IsDelete = false;
                    long respose = _IAttandanceService.AddResignationLetter(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ResignationLetterList()
        {
            List<ResignationLetterResponse> objModel = _IAttandanceService.GetAllResignationLetterList();
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult PrintResignationLetter(long ResignationID)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/ResignationLetter.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ResignationLetter.rdlc");
                }
                lr.ReportPath = path;
                var Resignation = _IAttandanceService.GetDataForResignationLetterPrint(ResignationID);
                List<PrintResignationLetter> LabelData = new List<PrintResignationLetter>();
                PrintResignationLetter obj = new PrintResignationLetter();
                obj.FullName = Resignation.FullName;
                obj.PrimaryAddress = Resignation.PrimaryAddress;
                obj.AreaName = Resignation.AreaName;
                obj.PinCode = Resignation.PinCode;
                obj.MobileNumber = Resignation.MobileNumber;
                obj.EmployeeCode = Resignation.EmployeeCode;
                obj.DateOfJoiningstr = Resignation.DateOfJoiningstr;
                obj.DateOfLeavingstr = Resignation.DateOfLeavingstr;
                obj.DateOfApplicationstr = Resignation.DateOfApplicationstr;
                LabelData.Add(obj);

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
                string Pdfpathcreate = Server.MapPath("~/ResignationLetter/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/ResignationLetter/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/ResignationLetter/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/ResignationLetter/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        // 25 Dec 2020 Piyush Limbani
        public ActionResult ResignationAcceptanceLetter()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ResignationAcceptanceLetterList(ResignationAcceptanceLetterListResponse model)
        {
            if (model.FromDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.FromDate = Convert.ToDateTime(DateTime.Now);
            }
            if (model.ToDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.ToDate = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ResignationAcceptanceLetterListResponse> objModel = _IAttandanceService.GetResignationAcceptanceLetterList(model.FromDate, model.ToDate);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult UpdateResignationApprovalStatus(long ResignationID, int Status)
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
                    bool respose = _IAttandanceService.UpdateResignationApprovalStatus(ResignationID, Status, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintResignationAcceptanceLetter(long ResignationID)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/ResignationAcceptanceLetter.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ResignationAcceptanceLetter.rdlc");
                }
                lr.ReportPath = path;
                var Resignation = _IAttandanceService.GetDataForResignationAcceptanceLetterPrint(ResignationID);
                List<PrintResignationAcceptanceLetter> LabelData = new List<PrintResignationAcceptanceLetter>();
                PrintResignationAcceptanceLetter obj = new PrintResignationAcceptanceLetter();
                obj.EmployeeCode = Resignation.EmployeeCode;
                obj.FullName = Resignation.FullName;
                obj.ApprovalDatestr = Resignation.ApprovalDatestr;
                obj.DateOfLeavingstr = Resignation.DateOfLeavingstr;
                LabelData.Add(obj);

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
                string Pdfpathcreate = Server.MapPath("~/ResignationLetter/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/ResignationLetter/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/ResignationLetter/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/ResignationLetter/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        // 23 Dec 2020 Piyush Limbani
        public ActionResult GratuityandHisab()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        public JsonResult GetEmployeeByGodownID(long GodownID)
        {
            try
            {
                List<EmployeeNameResponse> detail = _IAttandanceService.GetEmployeeByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeDateofLeavingByEmployeeCode(long GodownID, long EmployeeCode)
        {
            try
            {
                var detail = _IAttandanceService.GetEmployeeDateofLeavingByEmployeeCode(GodownID, EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        // 23 Dec 2020 Piyush Limbani
        public JsonResult GetCalculateGratuity(int GodownID, long EmployeeCode, DateTime DateOfJoining, DateTime DateOfLeaving)
        {
            try
            {
                GratuityListResponse detail = null;
                if (DateOfLeaving.Month == 1 || DateOfLeaving.Month == 2 || DateOfLeaving.Month == 3)
                {
                    DateTime DateOfLeavingAct = DateOfLeaving.AddYears(-1);
                    // detail = _IAttandanceService.GetCalculateGratuity(DateOfLeavingAct.Year, GodownID, EmployeeCode, DateOfLeavingAct);
                    detail = _IAttandanceService.GetCalculateGratuity(DateOfLeavingAct.Year, GodownID, EmployeeCode, DateOfLeaving);
                }
                else
                {
                    DateTime DateOfLeavingAct = DateOfLeaving;
                    detail = _IAttandanceService.GetCalculateGratuity(DateOfLeavingAct.Year, GodownID, EmployeeCode, DateOfLeavingAct);
                }
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCalculateBonus(int GodownID, long EmployeeCode, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                int FromMonthID = FromDate.Month;
                int FromYearID = FromDate.Year;
                int ToMonthID = ToDate.Month;
                int ToYearID = ToDate.Year;
                ModelCalculateBonus detail = _IAttandanceService.GetCalculateBonus(FromMonthID, FromYearID, ToMonthID, ToYearID, GodownID, EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCalculateLeaveEncashment(int GodownID, long EmployeeCode, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                int FromMonthID = FromDate.Month;
                int FromYearID = FromDate.Year;
                int ToMonthID = ToDate.Month;
                int ToYearID = ToDate.Year;
                ModelCalculateLeaveEncashment detail = _IAttandanceService.GetCalculateLeaveEncashment(FromMonthID, FromYearID, ToMonthID, ToYearID, GodownID, EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddGratuityandHisab(AddGratuityandHisab data)
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
                    Gratuity_Hisab_Mst obj = new Gratuity_Hisab_Mst();
                    obj.Gratuity_Hisab_ID = data.Gratuity_Hisab_ID;
                    obj.GodownID = data.GodownID;
                    obj.EmployeeCode = data.EmployeeCode;
                    obj.DateOfJoining = data.DateOfJoining;
                    obj.DateOfLeaving = data.DateOfLeaving;
                    obj.LastDrawnSalary = data.LastDrawnSalary;
                    obj.TotalMonth = data.TotalMonth;
                    obj.Gratuity = data.Gratuity;
                    obj.AdditionalGratuity = data.AdditionalGratuity;
                    obj.TotalGratuity = data.TotalGratuity;
                    obj.FromDate = data.FromDate;
                    obj.ToDate = data.ToDate;
                    obj.TotalEarnedSalary = data.TotalEarnedSalary;
                    obj.BonusAmount = data.BonusAmount;
                    obj.AdditionalBonusAmount = data.AdditionalBonusAmount;
                    obj.TotalBonusAmount = data.TotalBonusAmount;
                    obj.LastDrawnSalaryLeaveEnhancement = data.LastDrawnSalaryLeaveEnhancement;
                    obj.ClosingLeaves = data.ClosingLeaves;
                    obj.LeaveEncashment = data.LeaveEncashment;
                    obj.AdditionalLeaveEncashment = data.AdditionalLeaveEncashment;
                    obj.TotalLeaveEncashment = data.TotalLeaveEncashment;
                    obj.GrandTotalAmount = data.GrandTotalAmount;

                    obj.WitnessOneID = data.WitnessOneID;
                    obj.WitnessTwoID = data.WitnessTwoID;


                    if (obj.Gratuity_Hisab_ID == 0)
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
                    obj.IsDelete = false;
                    long respose = _IAttandanceService.AddGratuityandHisab(obj);

                    if (respose != 0)
                    {
                        // bool respose2 = _IAttandanceService.DeActiveUserByEmployeeCode(data.EmployeeCode);
                    }

                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GratuityandHisabList()
        {
            List<Gratuity_HisabListResponse> objModel = _IAttandanceService.GetAllGratuity_HisabList();
            return PartialView(objModel);
        }

        // 26 Dec 2020 Piyush Limbani
        public ActionResult Pavati()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        public PartialViewResult PavatiList(PavatiListResponse model)
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            List<PavatiListResponse> objModel = _IAttandanceService.GetAllPavatiList(model.FromDate, model.ToDate, model.EmployeeCode);
            return PartialView(objModel);
        }


        [HttpPost]
        public ActionResult PrintPavati(long Gratuity_Hisab_ID, long WitnessOneID = 0, long WitnessTwoID = 0)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/Pavati.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/Pavati.rdlc");
                }
                lr.ReportPath = path;
                var Pavati = _IAttandanceService.GetDataForPavatiPrint(Gratuity_Hisab_ID, WitnessOneID, WitnessTwoID);
                List<PavatiListResponse> LabelData = new List<PavatiListResponse>();
                PavatiListResponse obj = new PavatiListResponse();
                obj.EmployeeCode = Pavati.EmployeeCode;
                obj.FullName = Pavati.FullName;
                obj.LastDrawnSalary = Pavati.LastDrawnSalary; // Basic Salary
                obj.TotalService = Pavati.TotalService;
                obj.TotalServiceEng = Pavati.TotalServiceEng;
                obj.TotalMonth = Pavati.TotalMonth;
                obj.TotalGratuity = Pavati.TotalGratuity;
                obj.TotalLeaveEncashment = Pavati.TotalLeaveEncashment;
                obj.TotalBonusAmount = Pavati.TotalBonusAmount;
                obj.GrandTotalAmount = Pavati.GrandTotalAmount;
                obj.Advance = Pavati.Advance;
                obj.NetAmount = Pavati.NetAmount;
                obj.AmountInWords = Pavati.AmountInWords;
                obj.DateOfApplicationstr = Pavati.DateOfApplicationstr;
                obj.WitnessOne = Pavati.WitnessOne;
                obj.WitnessTwo = Pavati.WitnessTwo;

                obj.DateOfJoining = Pavati.DateOfJoining;
                obj.DateOfLeaving = Pavati.DateOfLeaving;
                obj.ClosingLeaves = Pavati.ClosingLeaves;
                //obj.Year = Pavati.Year;
                obj.FromDate = Pavati.FromDate;
                obj.ToDate = Pavati.ToDate;

                LabelData.Add(obj);

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
                string Pdfpathcreate = Server.MapPath("~/ResignationLetter/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/ResignationLetter/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/ResignationLetter/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/ResignationLetter/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



    }
}