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

namespace vbwebsite.Areas.purchase.Controllers
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
        // GET: /purchase/Report/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DayWisePurchase()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult DayWisePurchaseList(DayWisePurchaseList model)
        {
            List<DayWisePurchaseList> objlst = _IReportService.GetDayWisePurchaseList(model.PurchaseDate);
            for (int i = 0; i < objlst.Count; i++)
            {
                if (objlst[i].IsDelete == true)
                {
                    objlst[i].Party = "Cancelled";
                    objlst[i].Area = "";
                    objlst[i].Amount = 0;
                    objlst[i].TotalPackingCharge = 0;
                    objlst[i].Hamali = 0;
                    objlst[i].DiscountPer = 0;
                    objlst[i].DiscountAmount = 0;
                    objlst[i].APMCPer = 0;
                    objlst[i].APMCAmount = 0;
                    objlst[i].CGSTTax = 0;
                    objlst[i].CGSTTaxAmount = 0;
                    objlst[i].SGSTTax = 0;
                    objlst[i].SGSTTaxAmount = 0;
                    objlst[i].IGSTTax = 0;
                    objlst[i].IGSTTaxAmount = 0;
                    objlst[i].TotalTaxAmount = 0;
                    objlst[i].TotalAmount = 0;
                    objlst[i].Insurance = 0;
                    objlst[i].TransportInward = 0;
                    objlst[i].TCSAmount = 0;
                    objlst[i].GrandTotalAmount = 0;
                    objlst[i].RoundOff = 0;
                    objlst[i].NetAmount = 0;
                    objlst[i].GrandRoundOff = 0;

                    objlst[i].TDSTax = 0;
                    objlst[i].TDSTaxAmount = 0;
                    objlst[i].AmountAfterTDS = 0;
                }
            }
            return PartialView(objlst);
        }

        public ActionResult ExportExcelDayWisePurchase(string PurchaseDate)
        {
            //DateTime expextedDate = Convert.ToDateTime(PurchaseDate);
            //string CreditMemoDate = expextedDate.ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(PurchaseDate))
            {
                PurchaseDate = DateTime.Now.ToString();
            }
            List<DayWisePurchaseList> dataInventory = _IReportService.GetDayWisePurchaseList(Convert.ToDateTime(PurchaseDate));
            for (int i = 0; i < dataInventory.Count; i++)
            {
                if (dataInventory[i].IsDelete == true)
                {
                    dataInventory[i].Party = "Cancelled";
                    dataInventory[i].Area = "";
                    dataInventory[i].Amount = 0;
                    dataInventory[i].TotalPackingCharge = 0;
                    dataInventory[i].Hamali = 0;
                    dataInventory[i].DiscountPer = 0;
                    dataInventory[i].DiscountAmount = 0;
                    dataInventory[i].APMCPer = 0;
                    dataInventory[i].APMCAmount = 0;
                    dataInventory[i].CGSTTax = 0;
                    dataInventory[i].CGSTTaxAmount = 0;
                    dataInventory[i].SGSTTax = 0;
                    dataInventory[i].SGSTTaxAmount = 0;
                    dataInventory[i].IGSTTax = 0;
                    dataInventory[i].IGSTTaxAmount = 0;
                    dataInventory[i].TotalTaxAmount = 0;
                    dataInventory[i].TotalAmount = 0;
                    dataInventory[i].Insurance = 0;
                    dataInventory[i].TransportInward = 0;
                    dataInventory[i].TCSAmount = 0;
                    dataInventory[i].GrandTotalAmount = 0;
                    dataInventory[i].RoundOff = 0;
                    dataInventory[i].NetAmount = 0;
                    dataInventory[i].GrandRoundOff = 0;

                    dataInventory[i].TDSTax = 0;
                    dataInventory[i].TDSTaxAmount = 0;
                    dataInventory[i].AmountAfterTDS = 0;
                }
            }
            List<DayWisePurchaseListForExp> lstdaywisepurchase = dataInventory.Select(x => new DayWisePurchaseListForExp()
            {
                AavakDate = x.PurchaseDatestr,
                AvakNumber = x.AvakNumber,
                PurchaseType = x.PurchaseType,
                GodownName = x.GodownName,
                BillNumber = x.BillNumber,
                BillDate = x.BillDatestr,
                Party = x.Party,
                Area = x.Area,
                DebitAccountType = x.DebitAccountType,

                ProductName = x.ProductName,
                HSNNumber = x.HSNNumber,
                VakalNumber = x.VakalNumber,
                NoofBags = x.NoofBags,
                GrossWeight = x.GrossWeight,
                TareWeight = x.TareWeight,
                NetWeight = x.NetWeight,
                RatePerKG = x.RatePerKG,
                Amount = x.Amount,
                WeightPerBag = x.WeightPerBag,
                RatePerBags = x.RatePerBags,
                PackingChargesBag = x.PackingChargesBag,
                PackingCharge = x.TotalPackingCharge,
                Rebate = x.DiscountPer,
                DiscountAmount = x.DiscountAmount,
                APMC = x.APMCPer,
                APMCAmount = x.APMCAmount,
                TotalTaxableAmount = x.TotalTaxableAmount,
                TotalTax = x.TotalTax,
                CGST = x.CGSTTax,
                CGSTAmount = x.CGSTTaxAmount,
                SGST = x.SGSTTax,
                SGSTAmount = x.SGSTTaxAmount,
                IGST = x.IGSTTax,
                IGSTAmount = x.IGSTTaxAmount,
                TotalTaxAmount = x.TotalTaxAmount,
                TotalAmount = x.TotalAmount,
                Hamali = x.Hamali,
                TransportInward = x.TransportInward,
                Insurance = x.Insurance,
                TaxCollectedAtSourceReceiver = x.TCSAmount,
                RoundOff = x.RoundOff,
                NetAmount = x.NetAmount,
                TDSTax = x.TDSTax,
                TDSTaxAmount = x.TDSTaxAmount,
                AmountAfterTDS = x.AmountAfterTDS,
                BrokerName = x.BrokerName,
                EWayNumber = x.EWayNumber
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdaywisepurchase));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "DayWisePurchase.xls");
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

        public ActionResult PurchasePaidPayment()
        {
            ViewBag.BankName = _ICommonService.GetBankNameList();
            ViewBag.Area = _IAdminService.GetAllAreaList();
            ViewBag.Supplier = _ISupplierService.GetAllSupplierName();
            //ViewBag.CashOption = _ICommonService.GetAllCashOption();
            return View();
        }

        [HttpPost]
        public PartialViewResult PurchasePaidPaymentList(DateTime PaymentDate, long? PaymentMode, long? BankID, long? AreaID, long? SupplierID, bool IsCheckForCreatedDate)
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

            List<PurchasePaidPaymentList> objlst = null;
            List<PurchasePaidPaymentList> objFinalList = new List<PurchasePaidPaymentList>();
            ViewBag.BankName = _ICommonService.GetBankNameList();
            ViewBag.CashOption = _ICommonService.GetAllCashOption();
            if (IsCheckForCreatedDate == false)
            {
                objlst = _IReportService.GetPurchasePaidPaymentList(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
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
                objlst = _IReportService.GetPurchasePaidPaymentListByCreatedOn(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
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
            // List<PurchasePaidPaymentList> objlst = _IReportService.GetPurchasePaidPaymentList(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
            //  return PartialView(objlst);
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

            List<PurchasePaidPaymentList> objlst = null;
            // List<PurchasePaidPaymentList> objFinalList = new List<PurchasePaidPaymentList>();
            if (IsCheckForCreatedDate == false)
            {
                objlst = _IReportService.GetPurchasePaidPaymentListForExport(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
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
                objlst = _IReportService.GetPurchasePaidPaymentListByCreatedOnForExport(PaymentDate, BankID, AreaID, SupplierID, ByCheque, ByCard, ByOnline);
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
            List<PurchasePaidPaymentListExport> lstpurchase = objlst.Select(x => new PurchasePaidPaymentListExport()
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
            ds.Tables.Add(ToDataTable(lstpurchase));

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
                Response.AddHeader("content-disposition", "attachment;filename= " + "PurchasePaymentList.xls");
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

        public ActionResult AavakReport()
        {
            List<UnVerifyPendingPurchaseAavakList> objModel = _IReportService.GetAllUnVerifyPendingPurchaseAavakReport();
            return View(objModel);
        }

        [HttpPost]
        public ActionResult UpdateVerifyPurcahseOrderStatus(List<UpdateVerifyPurcahseOrderStatus> data)
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
                    bool respose = _IReportService.UpdateVerifyPurcahseOrderStatus(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBankDetailByBankID(int BankID)
        {
            // string DATE = YearID.ToString() + "-" + MonthID.ToString()+"-" + "01";
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
        public ActionResult UpdatePurchasePayment(PurchasePayment data)
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
                    PurchasePayment obj = new PurchasePayment();
                    long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    if (data.Cash == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.PurchaseID = data.PurchaseID;
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
                        respose = _IReportService.UpdatePurchasePayment(obj, UserID);
                    }
                    else if (data.Cheque == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.PurchaseID = data.PurchaseID;
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
                        respose = _IReportService.UpdatePurchasePayment(obj, UserID);
                    }
                    else if (data.Card == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.PurchaseID = data.PurchaseID;
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
                        respose = _IReportService.UpdatePurchasePayment(obj, UserID);
                    }
                    else if (data.Online == true)
                    {
                        obj.PaymentID = data.PaymentID;
                        obj.PurchaseID = data.PurchaseID;
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
                        respose = _IReportService.UpdatePurchasePayment(obj, UserID);
                    }
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        // 29-05-2020
        public ActionResult BrokerAndItemWiseReport()
        {
            ViewBag.Supplier = _ISupplierService.GetAllSupplierName();
            ViewBag.Product = _IPurchaseService.GetAllProductName();
            ViewBag.Broker = _IAdminService.GetAllBrokerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult BrokerAndItemWiseReportList(BrokerAndItemWiseReportList model)
        {
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<BrokerAndItemWiseReportList> objModel = _IPurchaseService.GetBrokerAndItemWiseReportList(model.From, model.To, model.SupplierID, model.ProductID, model.BrokerID);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelBrokerAndItemWiseReportList(DateTime? From, DateTime? To, long? SupplierID, long? ProductID, long? BrokerID)
        {
            if (SupplierID == null)
            {
                SupplierID = 0;
            }
            if (ProductID == null)
            {
                ProductID = 0;
            }
            if (BrokerID == null)
            {
                BrokerID = 0;
            }
            var BrokerAndItemWiseReportList = _IPurchaseService.GetBrokerAndItemWiseReportList(From.Value, To.Value, SupplierID.Value, ProductID.Value, BrokerID.Value);
            List<PurchaseReportListForExp> lstbrokeranditemwisereportlist = BrokerAndItemWiseReportList.Select(x => new PurchaseReportListForExp()
            {
                GodownName = x.GodownName,
                BillNumber = x.BillNumber,
                BillDate = x.BillDatestr,
                SupplierName = x.SupplierName,
                AreaName = x.AreaName,
                ProductName = x.ProductName,
                NoofBags = x.NoofBags,
                RatePerKG = x.RatePerKG,
                TotalAmount = x.GrandTotalAmount,
                BrokerName = x.BrokerName
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "PurchaseReportList.xls");
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