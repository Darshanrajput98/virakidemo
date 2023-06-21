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

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class PouchStockController : Controller
    {
        private IAdminService _areaservice;
        private IProductService _productservice;
        private ICommonService _ICommonService;
        private ISupplierService _ISupplierService;
        private IExpensesService _IExpensesService;

        public PouchStockController(IAdminService areaservice, IProductService productservice, ICommonService commonservice, ISupplierService supplierservice, IExpensesService expensesservice)
        {
            _areaservice = areaservice;
            _productservice = productservice;
            _ICommonService = commonservice;
            _ISupplierService = supplierservice;
            _IExpensesService = expensesservice;
        }

        //
        // GET: /wholesale/PouchStock/
        public ActionResult Index()
        {
            return View();
        }

        // Pouch Stock 05-02-2020
        public ActionResult PouchInward()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Pouch = _productservice.GetAllPouchName();
            ViewBag.Supplier = _ISupplierService.GetAllSupplierName();
            return View();
        }

        // 08 Oct 2020 Piyush Limbani
        public JsonResult GetPouchByGodownID(long GodownID)
        {
            try
            {
                List<PouchListResponse> detail = _IExpensesService.GetPouchByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetLastPouchCostByPouchID(long PouchNameID)
        {
            List<PouchInwardCost> lstPouchCost = _IExpensesService.GetLastPouchCostByPouchID(PouchNameID);
            return Json(lstPouchCost, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOpeningPouchByPouchID(long GodownID, long PouchNameID)
        {
            try
            {
                var detail = _IExpensesService.GetOpeningPouchByPouchID(GodownID, PouchNameID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        // Pouch Stock 06-02-2020     
        [HttpPost]
        public ActionResult AddPouchInward(PouchInwardOutward data)
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
                    if (data.PouchInwardID == 0)
                    {
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        data.CreatedBy = data.CreatedBy;
                        data.CreatedOn = data.CreatedOn;
                    }
                    data.CreditDebitStatusID = 1;
                    data.CreditDebitStatus = "Credit";
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    data.IsDelete = false;
                    // long respose = _IExpensesService.AddPouchInward(data);
                    string respose = _IExpensesService.AddPouchInward(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PouchInwardList()
        {
            List<PouchInwardListResponse> objModel = _IExpensesService.GetAllPouchInwardList();
            return PartialView(objModel);
        }

        public ActionResult GetPouchCostByPouchInwardID(long PouchInwardID)
        {
            List<PouchInwardCost> lstPouchCost = _IExpensesService.GetPouchCostByPouchInwardID(PouchInwardID);
            return Json(lstPouchCost, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PouchOutward()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Pouch = _productservice.GetAllPouchName();
            return View();
        }

        [HttpPost]
        public ActionResult AddPouchOutward(PouchInwardOutward data)
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
                    if (data.PouchInwardID == 0)
                    {
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        data.CreatedBy = data.CreatedBy;
                        data.CreatedOn = data.CreatedOn;
                    }
                    data.CreditDebitStatusID = 2;
                    data.CreditDebitStatus = "Debit";
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    data.IsDelete = false;
                    long respose = _IExpensesService.AddPouchOutward(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PouchOutwardList()
        {
            List<PouchOutwardListResponse> objModel = _IExpensesService.GetAllPouchOutwardList();
            return PartialView(objModel);
        }

        // Pouch Stock Transfer 14-02-2020
        public ActionResult PouchStockTransfer()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Pouch = _productservice.GetAllPouchName();
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
        public ActionResult AddNoOfPouchTransfer(AddNoOfPouchTransfer data)
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
                    if (data.PouchTransferID == 0)
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
                    long respose = _IExpensesService.AddNoOfPouchTransfer(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPouchForTransferByGodownID()
        {
            try
            {
                long GodownID = Convert.ToInt64(Request.Cookies["GodownID"].Value);
                List<GetOpeningPouch> detail = _IExpensesService.GetPouchForTransferByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PouchStockTransferList()
        {
            List<PouchTransferListResponse> objModel = _IExpensesService.GetAllPouchStockTransferList();
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult UpdatePouchTransferAcceptStatusByPouchTransferID(List<GetOpeningPouch> data)
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
                    bool respose = _IExpensesService.UpdatePouchTransferAcceptStatusByPouchTransferID(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        // 16 Sep 2020 Piyush Limbani
        public ActionResult PouchStockReport()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Pouch = _productservice.GetAllPouchName();
            return View();
        }

        [HttpPost]
        public PartialViewResult PouchStockReportList(long? GodownID = 0, long? PouchNameID = 0)
        {
            List<PouchStockResponse> objModel = _IExpensesService.GetPouchStockReport(GodownID, PouchNameID);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelPouchStockReportList(long? GodownID = 0, long? PouchNameID = 0)
        {
            var PouchStockList = _IExpensesService.GetPouchStockReport(GodownID, PouchNameID);
            List<PouchStockExp> lstStock = PouchStockList.Select(x => new PouchStockExp()
            {
                Godown = x.GodownName,
                Pouch = x.PouchName,
                HSNNo = x.HSNNumber,
                MinPouch = x.MinPouchQuantity,
                TotalPouch = x.TotalPouch,
                PurchaseDate = x.PurchaseDatestr
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstStock));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "PouchStockList.xls");
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

        // 09 Oct 2020 Piyush Limbani
        public ActionResult ExportExcelPouchInward()
        {
            var InwardList = _IExpensesService.GetAllPouchInwardList();
            List<PouchInwardExport> lstinward = InwardList.Select(x => new PouchInwardExport()
            {
                Godown = x.GodownName,
                Pouch = x.PouchName,
                HSNNo = x.HSNNumber,
                Opening = x.OpeningPouch,
                NoofPcs = x.NoofPcs,
                Total = x.TotalPouch,
                PurchaseDate = x.PurchaseDatestr,
                Supplier = x.SupplierName,
                InvoiceNumber = x.InvoiceNumber,
                PreparedBy = x.FullName
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstinward));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "PouchInwardList.xls");
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

        public ActionResult ExportExcelPouchOutward()
        {
            var OutwardList = _IExpensesService.GetAllPouchOutwardList();
            List<PouchOutwardExport> lstoutward = OutwardList.Select(x => new PouchOutwardExport()
            {
                Godown = x.GodownName,
                Pouch = x.PouchName,
                HSNNo = x.HSNNumber,
                Opening = x.OpeningPouch,
                NoofPcs = x.NoofPcs,
                Total = x.TotalPouch,
                PurchaseDate = x.PurchaseDatestr,
                PreparedBy = x.FullName
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstoutward));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "PouchOutwardList.xls");
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

        public ActionResult ExportExcelPouchStockTransfer()
        {
            var StockTransferList = _IExpensesService.GetAllPouchStockTransferList();
            List<PouchStockTransferExp> lstoutward = StockTransferList.Select(x => new PouchStockTransferExp()
            {
                FromGodown = x.FromGodownName,
                ToGodown = x.ToGodownName,
                Pouch = x.PouchName,
                HSNNo = x.HSNNumber,
                NoofPcs = x.TransferNoofPcs,
                TransferDate = x.TransferDatestr,
                PreparedBy = x.FullName,
                AcceptDate = x.AcceptDatestr,
                AcceptedBy = x.AcceptedName,
                Status = x.Status
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstoutward));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "StockTransferList.xls");
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