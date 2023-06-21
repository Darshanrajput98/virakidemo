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
    public class UtilityController : Controller
    {
        //
        // GET: /wholesale/Utility/
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private IAdminService _areaservice;
        private ICommonService _ICommonService;
        private ISupplierService _ISupplierService;
        private IExpensesService _IExpensesService;

        public UtilityController(IAdminService areaservice, IProductService productservice, ICommonService commonservice, ISupplierService supplierservice, IExpensesService expensesservice)
        {
            _areaservice = areaservice;
            _ICommonService = commonservice;
            _ISupplierService = supplierservice;
            _IExpensesService = expensesservice;
        }

        public ActionResult ManageUtility()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Utility = _ICommonService.GetAllUtilityName();
            return View();
        }

        [HttpPost]
        public ActionResult ManageUtility(UtilityModel data)
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
                    Utility_Mst obj = new Utility_Mst();
                    obj.UtilityID = data.UtilityID;

                    //obj.UtilityName = data.UtilityName;
                    obj.UtilityNameID = data.UtilityNameID;

                    obj.UtilityDescription = data.UtilityDescription;
                    obj.UtilityQuantity = data.UtilityQuantity;
                    obj.GodownID = data.GodownID;
                    obj.MinUtilityQuantity = data.MinUtilityQuantity;
                    if (obj.UtilityID == 0)
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
                    bool respose = _areaservice.AddUtility(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageUtilityList()
        {
            List<UtilityListResponse> objModel = _areaservice.GetAllUtilityList();
            return PartialView(objModel);
        }

        public ActionResult DeleteUtility(long? UtilityID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteUtility(UtilityID.Value, IsDelete);
                return RedirectToAction("ManageUtility");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageUtility");
            }
        }

        // Utility Stock 02-03-2020 
        public ActionResult UtilityInward()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            //ViewBag.Utility = _areaservice.GetAllUtilityName();
            ViewBag.Utility = _ICommonService.GetAllUtilityName();
            ViewBag.Supplier = _ISupplierService.GetAllPurchaseAndExpenseSupplierName();
            return View();
        }

        public JsonResult GetUtilityForTransferByGodownID()
        {
            try
            {
                long GodownID = Convert.ToInt64(Request.Cookies["GodownID"].Value);
                List<GetOpeningUtility> detail = _IExpensesService.GetUtilityForTransferByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOpeningUtilityByUtilityID(long GodownID, long UtilityNameID)
        {
            try
            {
                var detail = _IExpensesService.GetOpeningUtilityByUtilityID(GodownID, UtilityNameID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetLastUtilityCostByUtilityID(long UtilityNameID)
        {
            List<UtilityInwardCost> lstUtilityCost = _IExpensesService.GetLastUtilityCostByUtilityID(UtilityNameID);
            return Json(lstUtilityCost, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUtilityInward(UtilityInwardOutward data)
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
                    if (data.UtilityInwardID == 0)
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
                    string respose = _IExpensesService.AddUtilityInward(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult UtilityInwardList()
        {
            List<UtilityInwardListResponse> objModel = _IExpensesService.GetAllUtilityInwardList();
            return PartialView(objModel);
        }

        public ActionResult GetUtilityCostByUtilityInwardID(long UtilityInwardID)
        {
            List<UtilityInwardCost> lstUtilityCost = _IExpensesService.GetUtilityCostByUtilityInwardID(UtilityInwardID);
            return Json(lstUtilityCost, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UtilityOutward()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            //ViewBag.Utility = _areaservice.GetAllUtilityName();
            ViewBag.Utility = _ICommonService.GetAllUtilityName();
            return View();
        }

        [HttpPost]
        public ActionResult AddUtilityOutward(UtilityInwardOutward data)
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
                    if (data.UtilityInwardID == 0)
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
                    long respose = _IExpensesService.AddUtilityOutward(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult UtilityOutwardList()
        {
            List<UtilityOutwardListResponse> objModel = _IExpensesService.GetAllUtilityOutwardList();
            return PartialView(objModel);
        }

        // Utility Stock Transfer 02-03-2020
        public ActionResult UtilityStockTransfer()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            //ViewBag.Utility = _areaservice.GetAllUtilityName();
            ViewBag.Utility = _ICommonService.GetAllUtilityName();
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
        public ActionResult AddNoOfUtilityTransfer(AddNoOfUtilityTransfer data)
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
                    if (data.UtilityTransferID == 0)
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
                    long respose = _IExpensesService.AddNoOfUtilityTransfer(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateUtilityTransferAcceptStatusByUtilityTransferID(List<GetOpeningUtility> data)
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
                    bool respose = _IExpensesService.UpdateUtilityTransferAcceptStatusByUtilityTransferID(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult UtilityStockTransferList()
        {
            List<UtilityTransferListResponse> objModel = _IExpensesService.GetAllUtilityStockTransferList();
            return PartialView(objModel);
        }

        // 16 Sep 2020 Piyush Limbani
        public ActionResult UtilityStockReport()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            //ViewBag.Utility = _areaservice.GetAllUtilityName();
            ViewBag.Utility = _ICommonService.GetAllUtilityName();
            return View();
        }

        [HttpPost]
        public PartialViewResult UtilityStockReportList(long? GodownID = 0, long? UtilityNameID = 0)
        {
            List<UtilityStockResponse> objModel = _IExpensesService.GetUtilityStockReport(GodownID, UtilityNameID);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelUtilityStockReportList(long? GodownID = 0, long? UtilityNameID = 0)
        {
            var UtilityStockList = _IExpensesService.GetUtilityStockReport(GodownID, UtilityNameID);
            List<UtilityStockExp> lstStock = UtilityStockList.Select(x => new UtilityStockExp()
            {
                Godown = x.GodownName,
                Utility = x.UtilityName,
                MinUtility = x.MinUtility,
                TotalUtility = x.TotalUtility,
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "UtilityStockList.xls");
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

        // 05 Oct 2020 Piyush Limbani
        public JsonResult GetUtilityByGodownID(long GodownID)
        {
            try
            {
                List<UtilityListResponse> detail = _IExpensesService.GetUtilityByGodownID(GodownID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        // 07 Oct 2020 Piyush Limbani
        public ActionResult ManageUtilityName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUtilityName(UtilityNameModel data)
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
                    UtilityName_Mst obj = new UtilityName_Mst();
                    obj.UtilityNameID = data.UtilityNameID;
                    obj.UtilityName = data.UtilityName;
                    obj.HSNNumber = data.HSNNumber;
                    if (obj.UtilityNameID == 0)
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
                    bool respose = _areaservice.AddUtilityName(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult UtilityNameList()
        {
            List<UtilityNameListResponse> objModel = _areaservice.GetAllUtilityNameList();
            return PartialView(objModel);
        }

        public ActionResult DeleteUtilityName(long? UtilityNameID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteUtilityName(UtilityNameID.Value, IsDelete);
                return RedirectToAction("ManageUtilityName");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageUtilityName");
            }
        }



        // 08 Oct 2020 Piyush Limbani
        public ActionResult ExportExcelUtility()
        {
            var UtilityList = _areaservice.GetAllUtilityList();
            List<UtilityExport> lstutility = UtilityList.Select(x => new UtilityExport()
            {
                Utility = x.UtilityName,
                HSNNo = x.HSNNumber,
                Description = x.UtilityDescription,
                Quantity = x.UtilityQuantity,
                Godown = x.GodownName,
                MinQuantity = x.MinUtilityQuantity
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstutility));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "UtilityList.xls");
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

        public ActionResult ExportExcelUtilityInward()
        {
            var InwardList = _IExpensesService.GetAllUtilityInwardList();
            List<UtilityInwardExport> lstinward = InwardList.Select(x => new UtilityInwardExport()
            {
                Godown = x.GodownName,
                Utility = x.UtilityName,
                HSNNo = x.HSNNumber,
                Opening = x.OpeningUtility,
                NoofPcs = x.NoofPcs,
                Total = x.TotalUtility,
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "UtilityInwardList.xls");
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

        public ActionResult ExportExcelUtilityOutward()
        {
            var OutwardList = _IExpensesService.GetAllUtilityOutwardList();
            List<UtilityOutwardExport> lstoutward = OutwardList.Select(x => new UtilityOutwardExport()
            {
                Godown = x.GodownName,
                Utility = x.UtilityName,
                HSNNo = x.HSNNumber,
                Opening = x.OpeningUtility,
                NoofPcs = x.NoofPcs,
                Total = x.TotalUtility,
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "UtilityOutwardList.xls");
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

        public ActionResult ExportExcelUtilityStockTransfer()
        {
            var StockTransferList = _IExpensesService.GetAllUtilityStockTransferList();
            List<StockTransferExp> lstoutward = StockTransferList.Select(x => new StockTransferExp()
            {
                FromGodown = x.FromGodownName,
                ToGodown = x.ToGodownName,
                Utility = x.UtilityName,
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