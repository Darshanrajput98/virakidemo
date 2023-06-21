using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;

namespace vbwebsite.Areas.purchase.Controllers
{
    public class PaymentController : Controller
    {
        private IPurchaseService _IPurchaseService;
        private IAdminService _IAdminService;
        private ISupplierService _ISupplierService;
        private IPaymentService _IPaymentService;
        private ICommonService _ICommonService;

        public PaymentController(IPurchaseService purchaseservice, IAdminService adminservice, ISupplierService supplierservice, IPaymentService paymentservice, ICommonService commonservice)
        {
            _IPurchaseService = purchaseservice;
            _IAdminService = adminservice;
            _ISupplierService = supplierservice;
            _IPaymentService = paymentservice;
            _ICommonService = commonservice;
        }

        //
        // GET: /purchase/Payment/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PurchasePayment()
        {
            //ViewBag.BankName = _ICommonService.GetBankNameList();
            ViewBag.Area = _IAdminService.GetAllAreaList();
            ViewBag.Supplier = _ISupplierService.GetAllSupplierName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewPurchasePaymentList(PurchasePaymentListResponse model)
        {
            ViewBag.CashOption = _ICommonService.GetAllCashOption();
            ViewBag.BankName = _ICommonService.GetBankNameList();
            List<PurchasePaymentListResponse> objModel = _IPaymentService.GetAllPurcahsePaymentList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult SavePayment(List<PurchasePaymentListResponse> data)
        {
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                bool respose = _IPaymentService.SavePurchasePayment(data, UserID);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
        }



        //public ActionResult ExportExcelPaymentPending(PaymentListResponse model)
        //{
        //    if (model.CustomerID == null)
        //    {
        //        model.CustomerID = 0;
        //    }
        //    if (model.AreaID == null)
        //    {
        //        model.AreaID = 0;
        //    }
        //    if (model.DaysofWeek == null)
        //    {
        //        model.DaysofWeek = 0;
        //    }
        //    if (model.UserID == null)
        //    {
        //        model.UserID = 0;
        //    }
        //    var objlst = _paymentservice.GetAllPaymentList(model);
        //    List<PaymentListForExp> lstproduct = objlst.Select(x => new PaymentListForExp() { CustomerNumber = x.CustomerNumber, CustomerName = x.CustomerName, AreaName = x.AreaName, InvoiceDate = x.InvoiceDate, InvoiceNumber = x.InvoiceNumber, InvoiceAmount = x.FinalTotal, OutstandingAmount = x.OutstandingAmount, Remark = x.Remark }).ToList();
        //    DataTable ds = new DataTable();
        //    ds = ToDataTable(lstproduct);
        //    DataRow row = ds.NewRow();
        //    row["CustomerNumber"] = 0;
        //    row["CustomerName"] = "";
        //    row["AreaName"] = "";
        //    row["InvoiceDate"] = "";
        //    row["InvoiceNumber"] = "";
        //    row["InvoiceAmount"] = 0;
        //    row["OutstandingAmount"] = 0;
        //    row["Remark"] = "";
        //    ds.Rows.InsertAt(row, 0);
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        var ws = wb.Worksheets.Add(ds);
        //        ws.Tables.FirstOrDefault().ShowAutoFilter = false;
        //        ws.Cell("A1").Value = "";
        //        ws.Cell("B1").Value = "SALES MAN:" + model.UserName;
        //        ws.Range("B1:E1").Row(1).Merge();
        //        ws.Cell("F1").Value = "Date :" + DateTime.Now;
        //        ws.Range("F1:H1").Row(1).Merge();
        //        ws.Cell("A2").Value = "CustomerNumber";
        //        ws.Cell("B2").Value = "CustomerName";
        //        ws.Cell("C2").Value = "AreaName";
        //        ws.Cell("D2").Value = "InvoiceDate";
        //        ws.Cell("E2").Value = "InvoiceNumber";
        //        ws.Cell("F2").Value = "InvoiceAmount";
        //        ws.Cell("G2").Value = "OutstandingAmount";
        //        ws.Cell("H2").Value = "Remark";
        //        ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        ws.Style.Font.Bold = true;
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename= " + "PaymentList.xlsx");
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //    return View();
        //}

        //public static DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);
        //    //Get all the properties
        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo prop in Props)
        //    {
        //        //Defining type of data column gives proper data table 
        //        var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
        //        //Setting column names as Property names
        //        dataTable.Columns.Add(prop.Name, type);
        //    }
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }
        //    //put a breakpoint here and check datatable
        //    return dataTable;
        //}

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


    }
}