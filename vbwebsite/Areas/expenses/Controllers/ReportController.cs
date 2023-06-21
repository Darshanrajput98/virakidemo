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
using vb.Service;

namespace vbwebsite.Areas.expenses.Controllers
{
    public class ReportController : Controller
    {

        private IReportService _IReportService;
        private ICommonService _ICommonService;
        private IAdminService _IAdminService;
        private ISupplierService _ISupplierService;
        private IPaymentService _IPaymentService;
        private IPurchaseService _IPurchaseService;

        public ReportController(IReportService reportservice, ICommonService commonservice, IAdminService adminservice, ISupplierService supplierservice, IPaymentService paymentservice, IPurchaseService purchaseservice)
        {
            _IReportService = reportservice;
            _ICommonService = commonservice;
            _IAdminService = adminservice;
            _ISupplierService = supplierservice;
            _IPaymentService = paymentservice;
            _IPurchaseService = purchaseservice;
        }

        //
        // GET: /expense/Report/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DayWiseExpense()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult DayWiseExpenseList(DayWiseExpenseList model)
        {
            List<DayWiseExpenseList> objlst = _IReportService.GetDayWiseExpenseList(model.ExpenseDate);
            for (int i = 0; i < objlst.Count; i++)
            {
                if (objlst[i].IsDelete == true)
                {
                    objlst[i].Party = "Cancelled";
                    objlst[i].Area = "";
                    objlst[i].TotalTaxableAmount = 0;
                    objlst[i].CGSTTax = 0;
                    objlst[i].CGSTTaxAmount = 0;
                    objlst[i].SGSTTax = 0;
                    objlst[i].SGSTTaxAmount = 0;
                    objlst[i].IGSTTax = 0;
                    objlst[i].IGSTTaxAmount = 0;
                    objlst[i].TotalTaxAmount = 0;
                    objlst[i].TCSAmount = 0;
                    objlst[i].RoundOff = 0;
                    objlst[i].TotalAmount = 0;
                    objlst[i].TDSTax = 0;
                    objlst[i].TDSTaxAmount = 0;
                    objlst[i].GrandTotalAmount = 0;
                    objlst[i].NetAmount = 0;
                    objlst[i].GrandRoundOff = 0;
                }
            }
            return PartialView(objlst);
        }

        public ActionResult ExportExcelDayWiseExpense(string ExpenseDate)
        {
            if (string.IsNullOrEmpty(ExpenseDate))
            {
                ExpenseDate = DateTime.Now.ToString();
            }
            List<DayWiseExpenseList> dataInventory = _IReportService.GetDayWiseExpenseList(Convert.ToDateTime(ExpenseDate));
            for (int i = 0; i < dataInventory.Count; i++)
            {
                if (dataInventory[i].IsDelete == true)
                {
                    dataInventory[i].Party = "Cancelled";
                    dataInventory[i].Area = "";
                    dataInventory[i].TotalTaxableAmount = 0;
                    dataInventory[i].CGSTTax = 0;
                    dataInventory[i].CGSTTaxAmount = 0;
                    dataInventory[i].SGSTTax = 0;
                    dataInventory[i].SGSTTaxAmount = 0;
                    dataInventory[i].IGSTTax = 0;
                    dataInventory[i].IGSTTaxAmount = 0;
                    dataInventory[i].TotalTaxAmount = 0;
                    dataInventory[i].TCSAmount = 0;
                    dataInventory[i].RoundOff = 0;
                    dataInventory[i].TotalAmount = 0;
                    dataInventory[i].TDSTax = 0;
                    dataInventory[i].TDSTaxAmount = 0;
                    dataInventory[i].GrandTotalAmount = 0;
                    dataInventory[i].NetAmount = 0;
                    dataInventory[i].GrandRoundOff = 0;
                }
            }
            List<DayWiseExpenseListForExp> lstdaywiseexpense = dataInventory.Select(x => new DayWiseExpenseListForExp()
            {
                AavakDate = x.ExpenseDatestr,
                //AvakNumber = x.AvakNumber,
                ExpenseType = x.ExpenseType,
                GodownName = x.GodownName,
                BillNumber = x.BillNumber,
                BillDate = x.BillDatestr,
                Party = x.Party,
                Area = x.Area,
                DebitAccountType = x.DebitAccountType,
                ProductName = x.ProductName,
                HSNNumber = x.HSNNumber,
                TotalTaxableAmount = x.TotalTaxableAmount,
                TotalTax = x.TotalTax,
                CGST = x.CGSTTax,
                CGSTAmount = x.CGSTTaxAmount,
                SGST = x.SGSTTax,
                SGSTAmount = x.SGSTTaxAmount,
                IGST = x.IGSTTax,
                IGSTAmount = x.IGSTTaxAmount,
                TotalTaxAmount = x.TotalTaxAmount,
                TaxCollectedAtSourceReceiver = x.TCSAmount,
                RoundOff = x.RoundOff,
                TotalAmount = x.TotalAmount,
                TDSCategory = x.TDSCategory,
                TDSTax = x.TDSTax,
                TDSTaxAmount = x.TDSTaxAmount,
                NetAmount = x.NetAmount,
                EWayNumber = x.EWayNumber
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdaywiseexpense));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "DayWiseExpense.xls");
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

        public ActionResult ExpensePaidPayment()
        {
            ViewBag.BankName = _ICommonService.GetBankNameList();
            ViewBag.Area = _IAdminService.GetAllAreaList();
            ViewBag.Supplier = _ISupplierService.GetAllExpenseSupplierName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ExpensePaidPaymentList(DateTime PaymentDate, long? PaymentMode, long? BankID, long? AreaID, long? SupplierID, bool IsCheckForCreatedDate)
        {
            bool ByCheque = false;
            bool ByCard = false;
            bool ByOnline = false;

            if (PaymentMode == 2)
            {
                ByCheque = true;
                ByCard = false;
                ByOnline = false;
            }
            else if (PaymentMode == 3)
            {
                ByCheque = false;
                ByCard = true;
                ByOnline = false;
            }
            else if (PaymentMode == 4)
            {
                ByCheque = false;
                ByCard = false;
                ByOnline = true;
            }
            else
            {
                ByCheque = false;
                ByCard = false;
                ByOnline = false;
            }

            decimal sumAmount = 0;
            decimal sumBillAmount = 0;
            decimal sumNetAmount = 0;

            List<ExpensePaidPaymentList> objlst = null;
            List<ExpensePaidPaymentList> objFinalList = new List<ExpensePaidPaymentList>();
            ViewBag.BankName = _ICommonService.GetBankNameList();
            ViewBag.CashOption = _ICommonService.GetAllCashOption();
            if (IsCheckForCreatedDate == false)
            {
                objlst = _IReportService.GetExpensePaidPaymentList(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
                foreach (var record in objlst)
                {
                    sumAmount += record.Amount;
                    sumBillAmount += record.BillAmount;
                    sumNetAmount += record.NetAmount;
                    objFinalList.Add(record);
                }
                if (sumAmount != 0)
                {
                    objFinalList[0].sumAmount = sumAmount;
                }
                if (sumBillAmount != 0)
                {
                    objFinalList[0].sumBillAmount = sumBillAmount;
                }
                if (sumNetAmount != 0)
                {
                    objFinalList[0].sumNetAmount = sumNetAmount;
                }
            }
            else
            {
                objlst = _IReportService.GetExpensePaidPaymentListByCreatedOn(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
                foreach (var record in objlst)
                {
                    sumAmount += record.Amount;
                    sumBillAmount += record.BillAmount;
                    sumNetAmount += record.NetAmount;
                    objFinalList.Add(record);
                }
                if (sumAmount != 0)
                {
                    objFinalList[0].sumAmount = sumAmount;
                }
                if (sumBillAmount != 0)
                {
                    objFinalList[0].sumBillAmount = sumBillAmount;
                }
                if (sumNetAmount != 0)
                {
                    objFinalList[0].sumNetAmount = sumNetAmount;
                }
            }
            return PartialView(objFinalList);

            //objlst = _IReportService.GetExpensePaidPaymentListByCreatedOn(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
            // return PartialView(objlst);
        }

        public ActionResult ExportExcelPaidPayment(DateTime PaymentDate, long? PaymentMode, long? BankID, long? AreaID, long? SupplierID, bool IsCheckForCreatedDate)
        {
            bool ByCheque = false;
            bool ByCard = false;
            bool ByOnline = false;

            if (PaymentMode == 2)
            {
                ByCheque = true;
                ByCard = false;
                ByOnline = false;
            }
            else if (PaymentMode == 3)
            {
                ByCheque = false;
                ByCard = true;
                ByOnline = false;
            }
            else if (PaymentMode == 4)
            {
                ByCheque = false;
                ByCard = false;
                ByOnline = true;
            }
            else
            {
                ByCheque = false;
                ByCard = false;
                ByOnline = false;
            }

            decimal sumAmount = 0;
            decimal sumBillAmount = 0;
            decimal sumNetAmount = 0;

            List<ExpensePaidPaymentList> objlst = null;
            //List<ExpensePaidPaymentList> objFinalList = new List<ExpensePaidPaymentList>();
            if (IsCheckForCreatedDate == false)
            {
                objlst = _IReportService.GetExpensePaidPaymentListForExport(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
                //foreach (var record in objlst)
                //{
                //    sumAmount += record.Amount;
                //    sumBillAmount += record.BillAmount;
                //    sumNetAmount += record.NetAmount;
                //    objFinalList.Add(record);
                //}
                //if (sumAmount != 0)
                //{
                //    objFinalList[0].sumAmount = sumAmount;
                //}
                //if (sumBillAmount != 0)
                //{
                //    objFinalList[0].sumBillAmount = sumBillAmount;
                //}
                //if (sumNetAmount != 0)
                //{
                //    objFinalList[0].sumNetAmount = sumNetAmount;
                //}
            }
            else
            {
                objlst = _IReportService.GetExpensePaidPaymentListByCreatedOnForExport(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
                //foreach (var record in objlst)
                //{
                //    sumAmount += record.Amount;
                //    sumBillAmount += record.BillAmount;
                //    sumNetAmount += record.NetAmount;
                //    objFinalList.Add(record);
                //}
                //if (sumAmount != 0)
                //{
                //    objFinalList[0].sumAmount = sumAmount;
                //}
                //if (sumBillAmount != 0)
                //{
                //    objFinalList[0].sumBillAmount = sumBillAmount;
                //}
                //if (sumNetAmount != 0)
                //{
                //    objFinalList[0].sumNetAmount = sumNetAmount;
                //}
            }
            List<ExpensePaidPaymentListExport> lstexpense = objlst.Select(x => new ExpensePaidPaymentListExport()
            {
                Bank_Name = x.Bank_Name,
                Client_Code = x.Client_Code,
                Product_Code = x.Product_Code,
                Payment_Type = x.Payment_Type,
                Payment_Ref_No = x.Payment_Ref_No,
                Payment_Date = x.Payment_Date,
                Instrument_Date = x.Instrument_Date,
                Dr_Ac_No = x.Dr_Ac_No,
                Amount = x.Amount,
                Bank_Code_Indicator = x.Bank_Code_Indicator,
                Beneficiary_Code = x.Beneficiary_Code,
                EmployeeName = x.SupplierName,
                Beneficiary_Name = x.NameAsBankAccount,
                Beneficiary_Bank = x.Beneficiary_Bank,
                Beneficiary_Branch_IFSC_Code = x.IFSCCode,
                Beneficiary_Acc_No = x.AccountNumber,
                Location = x.Location,
                Print_Location = x.Print_Location,
                Instrument_Number = x.Instrument_Number,
                Ben_Add1 = x.Ben_Add1,
                Ben_Add2 = x.Ben_Add2,
                Ben_Add3 = x.Ben_Add3,
                Ben_Add4 = x.Ben_Add4,
                Beneficiary_Email = x.Email,
                Beneficiary_Mobile = x.MobileNumber,
                Debit_Narration = x.Debit_Narration,
                Credit_Narration = x.Credit_Narration,
                Payment_Details_1 = x.Payment_Details_1,
                Payment_Details_2 = x.Payment_Details_2,
                Payment_Details_3 = x.Payment_Details_3,
                Payment_Details_4 = x.Payment_Details_4,
                Bill_No = x.BillNumber,
                Bill_Date = x.BillDatestr,
                Bill_Amt = x.BillAmount,
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
            ds.Tables.Add(ToDataTable(lstexpense));

            //DataRow dRow = ds.Tables[0].NewRow();
            //dRow["Amount"] = sumAmount;
            //dRow["Bill_Amt"] = sumBillAmount;
            //dRow["Net"] = sumNetAmount;
            //ds.Tables[0].Rows.Add(dRow);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "ExpensePaymentList.xls");
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

        public JsonResult GetBankDetailByBankID(int BankID)
        {
            try
            {
                var detail = _IPaymentService.GetBankDetailByBankID(BankID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateExpensePayment(ExpensePayment data)
        {
            bool respose = false;
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    ExpensePayment obj = new ExpensePayment();
                    long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    if (data.Cash == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.ExpenseID = data.ExpenseID;
                        obj.BillNumber = data.BillNumber;
                        obj.Amount = data.Amount;
                        obj.GodownID = data.GodownID;
                        obj.PaymentDate = data.CashPaymentDate;
                        obj.BankID = 0;
                        obj.Cheque = data.Cheque;
                        obj.ChequeNo = null;
                        obj.Card = data.Card;
                        obj.TypeOfCard = null;
                        obj.Online = data.Online;
                        obj.UTRNumber = null;
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                        respose = _IReportService.UpdateExpensePayment(obj, UserID);
                    }
                    else if (data.Cheque == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.ExpenseID = data.ExpenseID;
                        obj.BillNumber = data.BillNumber;
                        obj.Amount = data.Amount;
                        obj.GodownID = 0;
                        obj.PaymentDate = data.ChequeDate;
                        obj.BankID = data.BankIDCheck;
                        obj.Cheque = data.Cheque;
                        obj.ChequeNo = data.ChequeNo;
                        obj.Card = data.Card;
                        obj.TypeOfCard = null;
                        obj.Online = data.Online;
                        obj.UTRNumber = null;
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                        respose = _IReportService.UpdateExpensePayment(obj, UserID);
                    }
                    else if (data.Card == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.ExpenseID = data.ExpenseID;
                        obj.BillNumber = data.BillNumber;
                        obj.Amount = data.Amount;
                        obj.GodownID = 0;
                        obj.PaymentDate = data.CardPaymentDate;
                        obj.BankID = data.BankIDCard;
                        obj.Cheque = data.Cheque;
                        obj.ChequeNo = null;
                        obj.Card = data.Card;
                        obj.TypeOfCard = data.TypeOfCard;
                        obj.Online = data.Online;
                        obj.UTRNumber = null;
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                        respose = _IReportService.UpdateExpensePayment(obj, UserID);
                    }
                    else if (data.Online == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.ExpenseID = data.ExpenseID;
                        obj.BillNumber = data.BillNumber;
                        obj.Amount = data.Amount;
                        obj.GodownID = 0;
                        obj.PaymentDate = data.OnlinePaymentDate;
                        obj.BankID = data.BankIDOnline;
                        obj.Cheque = data.Cheque;
                        obj.ChequeNo = null;
                        obj.Card = data.Card;
                        obj.TypeOfCard = null;
                        obj.Online = data.Online;
                        obj.UTRNumber = data.UTRNumber;
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                        respose = _IReportService.UpdateExpensePayment(obj, UserID);
                    }
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        // 24 Aug 2020 Piyush Limbani
        public ActionResult ExpenseItemWiseReport()
        {
            ViewBag.Supplier = _ISupplierService.GetAllExpenseSupplierName();
            ViewBag.Product = _IPurchaseService.GetAllExpenseProductName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ExpenseItemWiseReportList(ExpenseItemWiseReportList model)
        {
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ExpenseItemWiseReportList> objModel = _IReportService.GetExpenseItemWiseReportList(model.From, model.To, model.SupplierID, model.ProductID);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelExpenseItemWiseReportList(DateTime? From, DateTime? To, long? SupplierID, long? ProductID)
        {
            if (SupplierID == null)
            {
                SupplierID = 0;
            }
            if (ProductID == null)
            {
                ProductID = 0;
            }
            var ItemWiseReportList = _IReportService.GetExpenseItemWiseReportList(From.Value, To.Value, SupplierID.Value, ProductID.Value);
            List<ItemWiseReportListForExp> lstbrokeranditemwisereportlist = ItemWiseReportList.Select(x => new ItemWiseReportListForExp()
            {
                GodownName = x.GodownName,
                BillNumber = x.BillNumber,
                BillDate = x.BillDatestr,
                SupplierName = x.SupplierName,
                AreaName = x.AreaName,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                Rate = x.Rate,
                TotalAmount = x.GrandTotalAmount
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstbrokeranditemwisereportlist));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "ItemWiseReportList.xls");
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