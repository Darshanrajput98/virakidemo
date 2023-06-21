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

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class ExpensesController : Controller
    {
        private IAdminService _areaservice;
        private ICommonService _ICommonService;
        private IExpensesService _IExpensesService;

        public ExpensesController(IAdminService areaservice, ICommonService commonservice, IExpensesService expensesservice)
        {
            _areaservice = areaservice;
            _ICommonService = commonservice;
            _IExpensesService = expensesservice;
        }

        // GET: /wholesale/Expenses/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpensesVoucher()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.DebitAccountType = _IExpensesService.GetAllDebitAccountTypeName();

            ViewBag.WholesaleCustomer = _IExpensesService.GetAllWholesaleCustomerNameForVoucher();
            ViewBag.RetailCustomer = _IExpensesService.GetAllRetailCustomerNameForVoucher();
            return View();
        }

        [HttpPost]
        public ActionResult ExpensesVoucher(AddExpensesVoucher data)
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
                    ExpensesVoucher_Mst obj = new ExpensesVoucher_Mst();
                    obj.ExpensesVoucherID = data.ExpensesVoucherID;
                    obj.GodownID = data.GodownID;
                    obj.DateofVoucher = data.DateofVoucher;
                    obj.VoucherNumber = data.VoucherNumber;
                    obj.Pay = data.Pay;
                    obj.RemarksL1 = data.RemarksL1;
                    obj.RemarksL2 = data.RemarksL2;
                    obj.RemarksL3 = data.RemarksL3;
                    obj.DebitAccountTypeID = data.DebitAccountTypeID;
                    obj.Amount = data.Amount;
                    obj.BillNumber = data.BillNumber;
                    obj.CustomerID = data.CustomerID;
                    if (obj.ExpensesVoucherID == 0)
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
                    obj.IsDactive = false;
                    obj.Identification = data.Identification;
                    long respose = _IExpensesService.AddExpensesVoucher(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ExpensesVoucherList()
        {
            List<ExpensesVoucherListResponse> objModel = _IExpensesService.GetAllExpensesVoucherList();
            return PartialView(objModel);
        }

        public ActionResult DeleteExpensesVoucher(long? ExpensesVoucherID, bool IsDelete)
        {
            try
            {
                _IExpensesService.DeleteExpensesVoucher(ExpensesVoucherID.Value, IsDelete);
                return RedirectToAction("ExpensesVoucher");
            }
            catch (Exception)
            {
                return RedirectToAction("ExpensesVoucher");
            }
        }

        [HttpPost]
        public ActionResult PrintExpensesVoucher(long ExpensesVoucherID)
        {
            try
            {
                string NumberToWord = "";
                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/ExpensesVoucher.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/ExpensesVoucher.rdlc");
                //}

                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/ExpensesVoucher.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExpensesVoucher.rdlc");
                }
                lr.ReportPath = path;

                var ExpensesVoucherDetail = _IExpensesService.GetDataForExpensesVoucherPrint(ExpensesVoucherID);
                List<ExpensesVoucherListResponse> LabelData = new List<ExpensesVoucherListResponse>();
                ExpensesVoucherListResponse obj = new ExpensesVoucherListResponse();
                obj.DateofVoucher = ExpensesVoucherDetail.DateofVoucher;
                obj.GodownName = ExpensesVoucherDetail.GodownName;
                obj.VoucherNumber = ExpensesVoucherDetail.VoucherNumber;
                obj.Pay = ExpensesVoucherDetail.Pay;
                obj.RemarksL1 = ExpensesVoucherDetail.RemarksL1;
                obj.RemarksL2 = ExpensesVoucherDetail.RemarksL2;
                obj.RemarksL3 = ExpensesVoucherDetail.RemarksL3;
                obj.DebitAccountType = ExpensesVoucherDetail.DebitAccountType;
                obj.Amount = ExpensesVoucherDetail.Amount;
                int number = Convert.ToInt32(obj.Amount);
                NumberToWord = NumberToWords(number);
                obj.AmountInWords = NumberToWord + "  " + "Only/-";
                obj.PreparedBy = ExpensesVoucherDetail.PreparedBy;
                if (ExpensesVoucherDetail.BillNumber != "" && ExpensesVoucherDetail.BillNumber != null)
                {
                    obj.BillNumber = "(" + " " + ExpensesVoucherDetail.BillNumber + " " + ")";
                }
                else
                {
                    obj.BillNumber = "";
                }
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
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>7.5in</PageHeight>" +
            "  <MarginTop>1cm</MarginTop>" +
            "  <MarginLeft>1cm</MarginLeft>" +
            "  <MarginRight>1cm</MarginRight>" +
            "  <MarginBottom>1cm</MarginBottom>" +
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
                string Pdfpathcreate = Server.MapPath("~/ExpensesVoucher/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                //string url = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    url = "http://" + Request.Url.Host + ":6551/ExpensesVoucher/" + name;
                //}
                //else
                //{
                //    url = "http://" + Request.Url.Host + "/ExpensesVoucher/" + name;
                //}


                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/ExpensesVoucher/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/ExpensesVoucher/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/ExpensesVoucher/" + name;
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

        public ActionResult DebitAccountType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDebitAccountType(AddDebitAccountType data)
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
                    DebitAccountType_Mst obj = new DebitAccountType_Mst();
                    obj.DebitAccountTypeID = data.DebitAccountTypeID;
                    obj.DebitAccountType = data.DebitAccountType;
                    if (obj.DebitAccountTypeID == 0)
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
                    long respose = _IExpensesService.AddDebitAccountType(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult DebitAccountTypeList()
        {
            List<DebitAccountTypeListResponse> objModel = _IExpensesService.GetAllDebitAccountTypeList();
            return PartialView(objModel);
        }

        public ActionResult DeleteDebitAccountType(long? DebitAccountTypeID, bool IsDelete)
        {
            try
            {
                _IExpensesService.DeleteDebitAccountType(DebitAccountTypeID.Value, IsDelete);
                return RedirectToAction("DebitAccountType");
            }
            catch (Exception)
            {
                return RedirectToAction("DebitAccountType");
            }
        }

        public ActionResult SearchExpensesVoucher()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchExpensesVoucherList(DateTime? FromDate, DateTime? ToDate, long GodownID)
        {
            List<ExpensesVoucherListResponse> objModel = _IExpensesService.GetExpensesVoucherListByDate(FromDate, ToDate, GodownID);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelExpensesVoucher(DateTime? FromDate, DateTime? ToDate, long GodownID)
        {
            var VoucherList = _IExpensesService.GetExpensesVoucherListByDate(FromDate, ToDate, GodownID);
            List<ExpensesVoucherListExport> lstVoucher = VoucherList.Select(x => new ExpensesVoucherListExport() { Cash = x.GodownNamestr, DateofVoucher = x.DateofVoucherstr, VoucherNumber = x.VoucherNumber, PayTo = x.Pay, DebitAccount = x.DebitAccountType, BillNumber = x.BillNumber, Amount = x.Amount, PreparedBy = x.PreparedBy, Remarks = x.Remarks }).ToList();

            var Total1 = _IExpensesService.GetExpensesVoucherTotal(FromDate, ToDate);
            List<ExpensesVoucherTotalEXP> lstTotal = Total1.Select(x => new ExpensesVoucherTotalEXP() { TotalAmount = x.TotalAmount }).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstVoucher));
            ds.Tables.Add(ToDataTable(lstTotal));

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "ExpensesVoucher.xls");
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

        public ActionResult Inward()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.VehicleNo = _ICommonService.GetVehicleNoList();
            ViewBag.RetVehicleNo = _ICommonService.GetRetVehicleNoList();
            ViewBag.BankName = _ICommonService.GetBankNameList();

            // 27 Aug 2020 Piyush Limbani
            //ViewBag.TempoNumber = _ICommonService.GetAllTempoNumberListCurrentDateWise();
            //ViewBag.InwardTempoNumber = _ICommonService.GetAllTempoNumberListForInward();
            // 27 Aug 2020 Piyush Limbani

            ViewBag.TempoNumber = _ICommonService.GetAllTempoNumberList2();

            //Add By Dhruvik
            //ViewBag.RoleId = Utility.GetUserRoleId().ToString();
            ViewBag.RoleId = HttpContext.Session["RoleId"].ToString();
            //Add By Dhruvik
            return View();
        }

        public JsonResult CheckTodaysGodownIsExists(long GodownID)
        {
            try
            {
                var detail = _IExpensesService.CheckTodaysGodownIsExists(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOpeningAmountByGodownID(long GodownID)
        {
            try
            {
                var detail = _IExpensesService.GetOpeningAmountByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOpeningChillarByGodownID(long GodownID)
        {
            try
            {
                var detail = _IExpensesService.GetOpeningChillarByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetWholesaleAmountByCustID(DateTime WholesaleAssignedDate, long GodownID)
        {
            try
            {
                var detail = _IExpensesService.GetWholesaleAmountByCustID(WholesaleAssignedDate, GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult SalesManWiseCashList(DateTime WholesaleAssignedDate, long GodownID)
        {
            List<SalesManWiseCash> CashList = new List<SalesManWiseCash>();
            CashList = _IExpensesService.GetSalesManWiseCashList(WholesaleAssignedDate, GodownID);
            return PartialView(CashList);
        }

        public JsonResult GetRetailAmountTotalByAssignedDate(DateTime RetailAssignedDate, long GodownID)
        {
            try
            {
                var detail = _IExpensesService.GetRetailAmountTotalByAssignedDate(RetailAssignedDate, GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult RetSalesManWiseCashList(DateTime RetailAssignedDate, long GodownID)
        {
            List<SalesManWiseCash> CashList = new List<SalesManWiseCash>();
            CashList = _IExpensesService.GetRetSalesManWiseCashList(RetailAssignedDate, GodownID);
            return PartialView(CashList);
        }

        // 18 June 2020
        public JsonResult GetAllVehicleNoForInwardByAssignedDate(DateTime AssignedDate)
        {
            try
            {
                var detail = _IExpensesService.GetAllVehicleNoForInwardByAssignedDate(AssignedDate);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllRetailVehicleNoForInwardByAssignedDate(DateTime AssignedDate)
        {
            try
            {
                var detail = _IExpensesService.GetAllRetailVehicleNoForInwardByAssignedDate(AssignedDate);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTempoCashAmount(TempoCashAmount model)
        {
            try
            {
                decimal TotalAmount = 0;
                foreach (var item in model.VehicleNo)
                {
                    decimal Amount = _IExpensesService.GetTempoCashAmountTotal(model.TempoDateWholesale, item, model.GodownID);
                    TotalAmount += Amount;
                }
                return Json(TotalAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTempoCashAmountTotalRetail(TempoCashAmount model)
        {
            try
            {
                decimal TotalAmount = 0;
                foreach (var item in model.VehicleNo)
                {
                    decimal Amount = _IExpensesService.GetTempoCashAmountTotalRetail(model.TempoDateRetail, item, model.GodownID);
                    TotalAmount += Amount;
                }
                return Json(TotalAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTotalExpenseByGodownwise(DateTime ExpensesDate, long GodownID)
        {
            try
            {
                var detail = _IExpensesService.GetTotalExpenseByGodownwise(ExpensesDate, GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddInwardOutWard(AddInwardOutWard data)
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
                    if (data.InwardID == 0)
                    {
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        data.CreatedBy = data.CreatedBy;
                        data.CreatedOn = data.CreatedOn;
                    }
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    data.IsDelete = false;
                    data.IsDactive = false;
                    long respose = _IExpensesService.AddInwardOutWard(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult InwardList()
        {
            List<InwardOutWardListResponse> objModel = _IExpensesService.GetAllInwardList();
            return PartialView(objModel);
        }

        public ActionResult GetVehicleInwardCostList(long InwardID)
        {
            List<VehicleInwardCost> lstVehicleInwardCost = _IExpensesService.GetVehicleCostListByInwardID(InwardID);
            lstVehicleInwardCost = lstVehicleInwardCost.Where(c => c.ID == 1).ToList();
            return Json(lstVehicleInwardCost, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVehicleOutwardCostList(long InwardID)
        {
            List<VehicleInwardCost> lstVehicleInwardCost = _IExpensesService.GetVehicleCostListByInwardID(InwardID);
            lstVehicleInwardCost = lstVehicleInwardCost.Where(c => c.ID == 2).ToList();
            return Json(lstVehicleInwardCost, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchInward()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchInwardList(DateTime? FromDate)
        {
            List<InwardOutWardListResponse> objModel = _IExpensesService.GetInwardListByDate(FromDate);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelnward(DateTime? FromDate)
        {
            var InwardList = _IExpensesService.GetInwardListByDate(FromDate);
            List<lnwardExport> lstInward = InwardList.Select(x => new lnwardExport()
            {
                GodownName = x.GodownName,
                Opening = x.OpeningAmount,
                Chillar = x.ChillarInward,
                Wholesale = x.WholesaleCash,
                Retail = x.RetailCash,
                WholesaleTempoDate1 = x.WholesaleTempoDate1str,
                VehicleNo1 = x.WholesaleVehicleNo1,
                TempoAmount1 = x.WholesaleTempoAmount1,
                WholesaleTempoDate2 = x.WholesaleTempoDate2str,
                VehicleNo2 = x.WholesaleVehicleNo2,
                TempoAmount2 = x.WholesaleTempoAmount2,
                WholesaleTempoDate3 = x.WholesaleTempoDate3str,
                VehicleNo3 = x.WholesaleVehicleNo3,
                TempoAmount3 = x.WholesaleTempoAmount3,
                RetailTempoDate1 = x.RetailTempoDate1str,
                RetailVehicleNo1 = x.RetailVehicleNo1,
                RetailTempoAmount1 = x.RetailTempoAmount1,
                RetailTempoDate2 = x.RetailTempoDate2str,
                RetailVehicleNo2 = x.RetailVehicleNo2,
                RetailTempoAmount2 = x.RetailTempoAmount2,
                RetailTempoDate3 = x.RetailTempoDate3str,
                RetailVehicleNo3 = x.RetailVehicleNo3,
                RetailTempoAmount3 = x.RetailTempoAmount3,
                TransferAmountInward = x.TransferAmountInward,
                TotalInwardVehicleExpenses = x.TotalInwardVehicleExpenses,
                TotalInward = x.TotalInward,
                //TotalInward = x.FinalTotalInward,
                ChillarOutward = x.ChillarOutward,
                Expenses = x.TotalExpenses,
                BankName1 = x.BankName1,
                BankDepositeAmount1 = x.BankDepositeAmount1,
                BankName2 = x.BankName2,
                BankDepositeAmount2 = x.BankDepositeAmount2,
                BankName3 = x.BankName3,
                BankDepositeAmount3 = x.BankDepositeAmount3,
                TransferAmount = x.TransferAmount,
                TotalOutwardVehicleExpenses = x.TotalOutwardVehicleExpenses,
                TotalOutward = x.TotalOutward,
                // TotalOutward = x.FinalTotalOutward,
                GrandTotal = x.GrandTotal
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstInward));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "Inward.xls");
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

        [HttpPost]
        public ActionResult PrintInward(long InwardID, DateTime ExpensesDate, long GodownID)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/PrintInward.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/PrintInward.rdlc");
                //}


                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/PrintInward.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintInward.rdlc");
                }
                lr.ReportPath = path;

                List<ExpensesVoucherListResponse> ExpensesVoucherList = _IExpensesService.GetExpensesVoucherListByGodownID(ExpensesDate, GodownID);

                List<InwardOutWardListResponse> IODetalis = _IExpensesService.GetInwardOutwardDetailForPrint(InwardID, GodownID);

                List<TransferAmountListResponse> TransferAmountList = _IExpensesService.GetTransferAmountListForPrint(IODetalis[0].CreatedOn, GodownID);

                List<TransferAmountListResponse> TransferAmountListInward = _IExpensesService.GetTransferAmountListForPrintInward(IODetalis[0].CreatedOn, GodownID);

                List<VehicleExpensesList> InwardVehicleExpensesList = _IExpensesService.GetInwardVehicleExpensesList(InwardID);

                List<VehicleExpensesList> OutwardVehicleExpensesList = _IExpensesService.GetOutwardVehicleExpensesList(InwardID);

                DataTable VoucherList = Common.ToDataTable(ExpensesVoucherList);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", VoucherList);

                DataTable InwardOutward = Common.ToDataTable(IODetalis);
                ReportDataSource DataRecord = new ReportDataSource("DataSet2", InwardOutward);

                DataTable TransferAmount = Common.ToDataTable(TransferAmountList);
                ReportDataSource TransferAmt = new ReportDataSource("DataSet3", TransferAmount);

                DataTable TransferAmountInward = Common.ToDataTable(TransferAmountListInward);
                ReportDataSource TransferAmtInward = new ReportDataSource("DataSet4", TransferAmountInward);

                DataTable InwardVehicleExpenses = Common.ToDataTable(InwardVehicleExpensesList);
                ReportDataSource TotalInwardVehicleExpenses = new ReportDataSource("DataSet5", InwardVehicleExpenses);

                DataTable OutwardVehicleExpenses = Common.ToDataTable(OutwardVehicleExpensesList);
                ReportDataSource TotalOutwardVehicleExpenses = new ReportDataSource("DataSet6", OutwardVehicleExpenses);

                lr.DataSources.Add(MedsheetHeader);
                lr.DataSources.Add(DataRecord);
                lr.DataSources.Add(TransferAmt);
                lr.DataSources.Add(TransferAmtInward);
                lr.DataSources.Add(TotalInwardVehicleExpenses);
                lr.DataSources.Add(TotalOutwardVehicleExpenses);

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                            "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>1cm</MarginTop>" +
                "  <MarginLeft>1cm</MarginLeft>" +
                "  <MarginRight>1cm</MarginRight>" +
                "  <MarginBottom>1cm</MarginBottom>" +
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
                string Pdfpathcreate = Server.MapPath("~/Inward/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                //string url = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    url = "http://" + Request.Url.Host + ":6551/Inward/" + name;
                //}
                //else
                //{
                //    url = "http://" + Request.Url.Host + "/Inward/" + name;
                //}


                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Inward/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Inward/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Inward/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TrasferAmount()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        public JsonResult GetGodownForToGodown(long FromGodownID)
        {
            try
            {
                List<GodownListResponse> detail = _IExpensesService.GetGodownForToGodown(FromGodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddTransferAmount(AddTransferAmount data)
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
                    TransferAmount_Mst obj = new TransferAmount_Mst();
                    obj.TransferID = data.TransferID;
                    obj.FromGodownID = data.FromGodownID;
                    obj.ToGodownID = data.ToGodownID;
                    obj.Amount = data.Amount;
                    if (obj.TransferID == 0)
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
                    long respose = _IExpensesService.AddTransferAmount(obj);
                    if (respose > 0)
                    {
                        // minus from
                        var OutwardDetail = _IExpensesService.GetTopGrandTotalAmountByGodownIDandCreatedDateForOutward(obj.FromGodownID, obj.CreatedOn);
                        decimal GrandTotalOutward = OutwardDetail.OpeningAmount - data.Amount;
                        decimal TotalOutward = OutwardDetail.TotalOutward + data.Amount;
                        bool respose2 = _IExpensesService.UpdateGrandTotalForOutwardGodown(OutwardDetail.InwardID, obj.FromGodownID, GrandTotalOutward, TotalOutward);
                        ////plus to
                        var InwardDetail = _IExpensesService.GetTopGrandTotalAmountByGodownIDandCreatedDateForInward(obj.ToGodownID, obj.CreatedOn);
                        decimal GrandTotalInward = InwardDetail.OpeningAmount + data.Amount;
                        decimal TotalInward = InwardDetail.TotalInward + data.Amount;
                        bool respose3 = _IExpensesService.UpdateGrandTotalForInwardGodown(InwardDetail.InwardID, obj.ToGodownID, GrandTotalInward, TotalInward);
                    }
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult TrasferAmountList()
        {
            //  Jan 21, 2021 Piyush Limbani
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            //  Jan 21, 2021 Piyush Limbani
            List<TransferAmountListResponse> objModel = _IExpensesService.GetAllTrasferAmountList();
            return PartialView(objModel);
        }

        [HttpGet]
        public PartialViewResult PopupTransferAmount(long GodownID, DateTime CreatedOn)
        {
            List<TransferAmountListResponse> TransferAmountList = _IExpensesService.GetPopupTransferAmountList(GodownID, CreatedOn);
            return PartialView(TransferAmountList);
        }

        [HttpGet]
        public PartialViewResult PopupInwardVehicleExpenses(long InwardID)
        {
            List<VehicleExpensesList> InwardVehicleExpensesList = _IExpensesService.GetInwardVehicleExpensesList(InwardID);
            return PartialView(InwardVehicleExpensesList);
        }

        [HttpGet]
        public PartialViewResult PopupOutwardVehicleExpenses(long InwardID)
        {
            List<VehicleExpensesList> OutwardVehicleExpensesList = _IExpensesService.GetOutwardVehicleExpensesList(InwardID);
            return PartialView(OutwardVehicleExpensesList);
        }

        public ActionResult VehicleCosting()
        {
            ViewBag.WholesaleVehicleNo = _IExpensesService.GetAllVehicleNoListForVehicleCosting();
            ViewBag.RetailVehicleNo = _IExpensesService.GetAllRetVehicleNoListForVehicleCosting();
            ViewBag.DeliveryPerson = _ICommonService.GetAllDeliveryPersonList();
            ViewBag.TempoNumber = _ICommonService.GetAllTempoNumberList2();
            return View();
        }

        public JsonResult GetWholesaleVehicleDetailsForVehicleCosting(long WholesaleVehicleNo1, DateTime CreatedOn)
        {
            try
            {
                var detail = _IExpensesService.GetWholesaleVehicleDetailsForVehicleCosting(WholesaleVehicleNo1, CreatedOn);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetWholesaleTotalInvoiceAmountVehicle2(long WholesaleVehicleNo2, DateTime CreatedOn)
        {
            try
            {
                var detail = _IExpensesService.GetWholesaleTotalInvoiceAmountVehicle2(WholesaleVehicleNo2, CreatedOn);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRetailVehicleDetailsForVehicleCosting(long RetailVehicleNo1, DateTime CreatedOn)
        {
            try
            {
                var detail = _IExpensesService.GetRetailVehicleDetailsForVehicleCosting(RetailVehicleNo1, CreatedOn);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRetailTotalInvoiceAmountVehicle1(long RetailVehicleNo1, DateTime CreatedOn)
        {
            try
            {
                var detail = _IExpensesService.GetRetailTotalInvoiceAmountVehicle1(RetailVehicleNo1, CreatedOn);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRetailTotalInvoiceAmountVehicle2(long RetailVehicleNo2, DateTime CreatedOn)
        {
            try
            {
                var detail = _IExpensesService.GetRetailTotalInvoiceAmountVehicle2(RetailVehicleNo2, CreatedOn);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddVehicleCosting(AddVehicleCosting data)
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
                    VehicleCosting_Mst obj = new VehicleCosting_Mst();
                    obj.VehicleCostingID = data.VehicleCostingID;
                    //1
                    if (data.WholesaleVehicleNo1 != 0 && data.WholesaleVehicleNo1Date.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        obj.WholesaleVehicleNo1 = data.WholesaleVehicleNo1;
                        obj.WholesaleVehicleNo1Date = data.WholesaleVehicleNo1Date;
                    }
                    else
                    {
                        obj.WholesaleVehicleNo1 = 0;
                        obj.WholesaleVehicleNo1Date = null;
                    }
                    obj.WholesaleInvioceAmount1 = data.WholesaleInvioceAmount1;
                    obj.WholesaleTotalKG1 = data.WholesaleTotalKG1;
                    //2
                    if (data.WholesaleVehicleNo2 != 0 && data.WholesaleVehicleNo2Date.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        obj.WholesaleVehicleNo2 = data.WholesaleVehicleNo2;
                        obj.WholesaleVehicleNo2Date = data.WholesaleVehicleNo2Date;
                    }
                    else
                    {
                        obj.WholesaleVehicleNo2 = 0;
                        obj.WholesaleVehicleNo2Date = null;
                    }
                    obj.WholesaleInvioceAmount2 = data.WholesaleInvioceAmount2;
                    obj.WholesaleTotalKG2 = data.WholesaleTotalKG2;
                    //3
                    if (data.RetailVehicleNo1 != 0 && data.RetailVehicleNo1Date.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        obj.RetailVehicleNo1 = data.RetailVehicleNo1;
                        obj.RetailVehicleNo1Date = data.RetailVehicleNo1Date;
                    }
                    else
                    {
                        obj.RetailVehicleNo1 = 0;
                        obj.RetailVehicleNo1Date = null;
                    }
                    obj.RetailInvioceAmount1 = data.RetailInvioceAmount1;
                    obj.RetailTotalKG1 = data.RetailTotalKG1;
                    //4
                    if (data.RetailVehicleNo2 != 0 && data.RetailVehicleNo2Date.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        obj.RetailVehicleNo2 = data.RetailVehicleNo2;
                        obj.RetailVehicleNo2Date = data.RetailVehicleNo2Date;
                    }
                    else
                    {
                        obj.RetailVehicleNo2 = 0;
                        obj.RetailVehicleNo2Date = null;
                    }
                    obj.RetailInvioceAmount2 = data.RetailInvioceAmount2;
                    obj.RetailTotalKG2 = data.RetailTotalKG2;
                    obj.GrandInvoiceAmount = data.GrandInvoiceAmount;
                    obj.GrandTotalKG = data.GrandTotalKG;
                    obj.VehicleDetailID = data.VehicleDetailID;
                    obj.AreaName = data.AreaName;
                    obj.DeliveryPerson1 = data.DeliveryPerson1;
                    obj.DeliveryPerson2 = data.DeliveryPerson2;
                    obj.DeliveryPerson3 = data.DeliveryPerson3;
                    obj.DeliveryPerson4 = data.DeliveryPerson4;
                    obj.OpeningKM = data.OpeningKM;
                    obj.ClosingKM = data.ClosingKM;
                    string VehicleOutDate = data.VehicleOutDate.ToString("MM/dd/yyyy");
                    string OutTime = data.OutTime.ToString("hh:mm tt");
                    string OutDateTime = VehicleOutDate + " " + OutTime;
                    obj.OutDateTime = Convert.ToDateTime(OutDateTime);
                    string VehicleInDate = data.VehicleInDate.ToString("MM/dd/yyyy");
                    string InTime = data.InTime.ToString("hh:mm tt");
                    string InDateTime = VehicleInDate + " " + InTime;
                    if (VehicleInDate != "01/01/0001")
                    {
                        obj.InDateTime = Convert.ToDateTime(InDateTime);
                    }
                    else
                    {
                        obj.InDateTime = null;
                    }
                    obj.DieselOpeningKM = data.DieselOpeningKM;
                    obj.DieselKM = data.DieselKM;
                    obj.DieselAmount = data.DieselAmount;
                    obj.DieselLiter = data.DieselLiter;
                    obj.RepairingAmount = data.RepairingAmount;
                    obj.RepairingDetail = data.RepairingDetail;
                    obj.RepairingAmount2 = data.RepairingAmount2;
                    obj.RepairingDetail2 = data.RepairingDetail2;
                    obj.RepairingAmount3 = data.RepairingAmount3;
                    obj.RepairingDetail3 = data.RepairingDetail3;
                    obj.TollAmount = data.TollAmount;
                    obj.TollDetail = data.TollDetail;
                    obj.BharaiAmount = data.BharaiAmount;
                    obj.BharaiDetail = data.BharaiDetail;
                    obj.MiscellaneousAmount1 = data.MiscellaneousAmount1;
                    obj.MiscellaneousAmount2 = data.MiscellaneousAmount2;
                    obj.MiscellaneousAmount3 = data.MiscellaneousAmount3;
                    obj.MiscellaneousDetail1 = data.MiscellaneousDetail1;
                    obj.MiscellaneousDetail2 = data.MiscellaneousDetail2;
                    obj.MiscellaneousDetail3 = data.MiscellaneousDetail3;
                    if (obj.VehicleCostingID == 0)
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
                    long respose = _IExpensesService.AddVehicleCosting(obj);
                    if (respose > 0)
                    {
                        if (data.WholesaleVehicleNo1 != 0)
                        {
                            bool UpdateWholesaleVehicleNo1 = _IExpensesService.UpdateWholesaleVehicleNo1(data.WholesaleVehicleNo1, data.WholesaleVehicleNo1Date);
                        }
                        if (data.WholesaleVehicleNo2 != 0)
                        {
                            bool UpdateWholesaleVehicleNo2 = _IExpensesService.UpdateWholesaleVehicleNo2(data.WholesaleVehicleNo2, data.WholesaleVehicleNo2Date);
                        }
                        if (data.RetailVehicleNo1 != 0)
                        {
                            bool UpdateRetailVehicleNo1 = _IExpensesService.UpdateRetailVehicleNo1(data.RetailVehicleNo1, data.RetailVehicleNo1Date);
                        }
                        if (data.RetailVehicleNo2 != 0)
                        {
                            bool UpdateRetailVehicleNo2 = _IExpensesService.UpdateRetailVehicleNo2(data.RetailVehicleNo2, data.RetailVehicleNo2Date);
                        }
                    }
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult VehicleCostingList()
        {
            List<VehicleCostingListResponse> objModel = _IExpensesService.GetAllVehicleCostingList();
            return PartialView(objModel);
        }

        public ActionResult SearchVehicleCosting()
        {
            ViewBag.TempoNumber = _ICommonService.GetAllTempoNumberList2();
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchVehicleCostingList(DateTime FromDate, DateTime ToDate, string VehicleDetailIDs)
        {
            List<VehicleCostingListReport> objModel = _IExpensesService.GetVehicleCostingListByDate(FromDate, ToDate, VehicleDetailIDs);
            return PartialView(objModel);
        }

        public ActionResult ExportExceVehicleCosting(DateTime FromDate, DateTime ToDate, string VehicleDetailIDs)
        {
            var VehicleCostingList = _IExpensesService.GetVehicleCostingListByDate(FromDate, ToDate, VehicleDetailIDs);
            List<VehicleCostingListExport> lstInward = VehicleCostingList.Select(x => new VehicleCostingListExport()
            {
                VehicleNumber = x.VehicleNumber,
                DieselOpeningKM = x.DieselOpeningKM,
                DieselClosingKM = x.DieselClosingKM,
                DieselDiffKM = x.DieselDiffKM,
                TotalDieselAmount = x.TotalDieselAmount,
                AvgDieselCost = x.AvgDieselCost,
                TotalDieselLiter = x.TotalDieselLiter,
                AvgKMPerLtr = x.AvgKMPerLtr,
                OpeningKM = x.OpeningKM,
                ClosingKM = x.ClosingKM,
                DiffKM = x.DiffKM,
                TotalRepairingAmount = x.TotalRepairingAmount,
                RepairingCostPerKM = x.RepairingCostPerKM,
                TotalTollAmount = x.TotalTollAmount,
                TollCostPerKM = x.TollCostPerKM,
                TotalBharaiAmount = x.TotalBharaiAmount,
                BharaiCostPerKM = x.BharaiCostPerKM,
                MiscellaneousCostPerKM = x.MiscellaneousCostPerKM,
                TotalCostPerKM = x.TotalCostPerKM,
                ActualTotalCost = x.ActualTotalCost,
                TotalInvoiceAmt = x.GrandInvoiceAmount,
                PerOfInvoiceAmount = x.PerOfInvoiceAmount,
                TotalKG = x.GrandTotalKG
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstInward));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "VehicleCosting.xls");
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

        // 10/12/2019 & 11/12/2019
        public ActionResult DeActiveInwardVoucher()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeActiveInwardANDVoucherByDate(DateTime? FromDate, DateTime? ToDate, bool IsDactive)
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
                    bool respose = _IExpensesService.DeActiveInwardANDVoucherByDate(FromDate, ToDate, IsDactive);

                    if (respose == true)
                    {
                        DeActiveHistory_Mst obj = new DeActiveHistory_Mst();
                        obj.DeActiveHistoryID = 0;
                        obj.FromDate = FromDate;
                        obj.ToDate = ToDate;
                        obj.Status = IsDactive;
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                        long respose2 = _IExpensesService.AddDeActiveInwardANDVoucherHistory(obj);
                    }
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult DeActiveInwardVoucherList()
        {
            List<DeActiveInwardVoucherListResponse> objModel = _IExpensesService.GetAllDeActiveInwardVoucherList();
            return PartialView(objModel);
        }

        //27 Aug 2020 Piyush Limbani
        //public JsonResult GetVehicleExpensesInwardDetail(long VehicleDetailID, string VehicleNumber)
        //{
        //    try
        //    {
        //        string[] VehicleNumber1 = VehicleNumber.Split('/');
        //        string VehicleNo = VehicleNumber1[0];
        //        DateTime Date = Convert.ToDateTime(VehicleNumber1[1]);
        //        var detail = _IExpensesService.GetVehicleExpensesInwardDetail(VehicleDetailID, Date);
        //        return Json(detail, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult GetVehicleExpensesInwardDetail(long VehicleDetailID, DateTime AssignedDate, long GodownID)
        {
            try
            {
                var detail = _IExpensesService.GetVehicleExpensesInwardDetail(VehicleDetailID, AssignedDate, GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        // 27 Aug 2020 Piyush Limbani


        //  Jan 21, 2021 Piyush Limbani
        [HttpPost]
        public ActionResult UpdateTransferDetail(TransferPaymentModel data)
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
                    bool respose = _IExpensesService.UpdateTransferDetail(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        // 22 Oct 2020 Piyush Limbani For Delete Vehicle Cost Uncommented on 01 Feb 2022
        //[HttpPost]
        //public ActionResult DeleteVehicleInwardCost(long? VehicleInwardCostID, long? VehicleDetailID, DateTime? AssignedDate)
        //{
        //    try
        //    {
        //        if (Request.Cookies["UserID"] == null)
        //        {
        //            Request.Cookies["UserID"].Value = null;
        //            return JavaScript("location.reload(true)");
        //        }
        //        else
        //        {
        //            long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //            bool respose = _IExpensesService.DeleteVehicleInwardCost(VehicleInwardCostID, VehicleDetailID, AssignedDate, UserID);
        //            return Json(respose, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}
        // 22 Oct 2020 Piyush Limbani For Delete Vehicle Cost  Uncommented on 01 Feb 2022




        //[HttpPost]
        //public ActionResult AddInwardOutWard(AddInwardOutWard data)
        //{
        //    try
        //    {
        //        if (Request.Cookies["UserID"] == null)
        //        {
        //            Request.Cookies["UserID"].Value = null;
        //            return JavaScript("location.reload(true)");
        //        }
        //        else
        //        {
        //            InwardOutward_Mst obj = new InwardOutward_Mst();
        //            obj.InwardID = data.InwardID;
        //            obj.GodownID = data.GodownID;
        //            obj.OpeningAmount = data.OpeningAmount;
        //            obj.ChillarInward = data.ChillarInward;
        //            obj.WholesaleAssignedDate = data.WholesaleAssignedDate;
        //            obj.WholesaleCash = data.WholesaleCash;
        //            obj.RetailAssignedDate = data.RetailAssignedDate;
        //            obj.RetailCash = data.RetailCash;
        //            obj.WholesaleTempoDate1 = data.WholesaleTempoDate1;
        //            obj.WholesaleVehicleNo1 = data.WholesaleVehicleNo1;
        //            obj.WholesaleTempoAmount1 = data.WholesaleTempoAmount1;
        //            obj.WholesaleTempoDate2 = data.WholesaleTempoDate2;
        //            obj.WholesaleVehicleNo2 = data.WholesaleVehicleNo2;
        //            obj.WholesaleTempoAmount2 = data.WholesaleTempoAmount2;
        //            obj.WholesaleTempoDate3 = data.WholesaleTempoDate3;
        //            obj.WholesaleVehicleNo3 = data.WholesaleVehicleNo3;
        //            obj.WholesaleTempoAmount3 = data.WholesaleTempoAmount3;
        //            obj.RetailTempoDate1 = data.RetailTempoDate1;
        //            obj.RetailVehicleNo1 = data.RetailVehicleNo1;
        //            obj.RetailTempoAmount1 = data.RetailTempoAmount1;
        //            obj.RetailTempoDate2 = data.RetailTempoDate2;
        //            obj.RetailVehicleNo2 = data.RetailVehicleNo2;
        //            obj.RetailTempoAmount2 = data.RetailTempoAmount2;
        //            obj.RetailTempoDate3 = data.RetailTempoDate3;
        //            obj.RetailVehicleNo3 = data.RetailVehicleNo3;
        //            obj.RetailTempoAmount3 = data.RetailTempoAmount3;
        //            obj.TotalInward = data.TotalInward;
        //            obj.ChillarOutward = data.ChillarOutward;
        //            obj.ExpensesDate = data.ExpensesDate;
        //            obj.TotalExpenses = data.TotalExpenses;
        //            obj.TransferVB2 = data.TransferVB2;
        //            obj.TransferVB3 = data.TransferVB3;
        //            obj.BankID1 = data.BankID1;
        //            obj.BankDepositeAmount1 = data.BankDepositeAmount1;
        //            obj.BankID2 = data.BankID2;
        //            obj.BankDepositeAmount2 = data.BankDepositeAmount2;
        //            obj.BankID3 = data.BankID3;
        //            obj.BankDepositeAmount3 = data.BankDepositeAmount3;
        //            obj.TotalOutward = data.TotalOutward;
        //            obj.GrandTotal = data.GrandTotal;
        //            obj.OpeningChillar = data.OpeningChillar;
        //            obj.ClosingChillar = data.ClosingChillar;
        //            if (obj.InwardID == 0)
        //            {
        //                obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //                obj.CreatedOn = DateTime.Now;
        //            }
        //            else
        //            {
        //                obj.CreatedBy = data.CreatedBy;
        //                obj.CreatedOn = data.CreatedOn;
        //            }
        //            obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //            obj.UpdatedOn = DateTime.Now;
        //            obj.IsDelete = false;
        //            long respose = _IExpensesService.AddInwardOutWard(obj);

        //            //if (respose > 0)
        //            //{
        //            //    if (data.TransferVB3 != 0)
        //            //    {
        //            //        var detail = _IExpensesService.GetOpeningAmountByGodownID(2);
        //            //        decimal ClosingAmount = detail.OpeningAmount;
        //            //        decimal GrandTotal = ClosingAmount + data.TransferVB2;
        //            //        bool respose2 = _IExpensesService.UpdateGrandTotalForTransferVB2(2, GrandTotal);
        //            //    }
        //            //    if (data.TransferVB2 != 0)
        //            //    {
        //            //        var detail = _IExpensesService.GetOpeningAmountByGodownID(3);
        //            //       decimal ClosingAmount =  detail.OpeningAmount;
        //            //       decimal GrandTotal = ClosingAmount + data.TransferVB2;
        //            //       bool respose2 = _IExpensesService.UpdateGrandTotalForTransferVB2(3,GrandTotal);
        //            //    }
        //            //}

        //            return Json(respose, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult GetAssignedDateWholesaleCashCounterAmount(DateTime AssignedDateCashCounter)
        //{
        //    try
        //    {
        //        var detail = _IExpensesService.GetAssignedDateWholesaleCashCounterAmount(AssignedDateCashCounter);
        //        return Json(detail, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult GetVehicleDetailsForVehicleCosting(long VehicleDetailID, string TempoNumber)
        //{
        //    try
        //    {
        //        var detail = _IExpensesService.GetVehicleDetailsForVehicleCosting(VehicleDetailID, TempoNumber);
        //        return Json(detail, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult GetVehicleOpeningKM(long VehicleDetailID)
        //{
        //    try
        //    {
        //        var detail = _IExpensesService.GetVehicleOpeningKM(VehicleDetailID);
        //        return Json(detail, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckTodaysVehicleCostIsExists(long VehicleDetailID)
        //{
        //    try
        //    {
        //        var detail = _IExpensesService.CheckTodaysVehicleCostIsExists(VehicleDetailID);
        //        return Json(detail, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}