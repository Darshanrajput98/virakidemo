﻿using ClosedXML.Excel;
using Newtonsoft.Json;
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


namespace vbwebsite.Areas.coldstorage.Controllers
{
    public class StockController : Controller
    {
        private static object Lock = new object();
        private ICommonService _ICommonService;
        private IProductService _ProductService;
        private IColdStorageService _ColdStorageService;
        private IPurchaseService _PurchaseService;
        private IAdminService _AdminService;
        private ISupplierService _ISupplierService;

        public StockController(ICommonService ICommonService, IAdminService AdminService, IPurchaseService PurchaseService, IProductService ProductService, IColdStorageService ColdStorageService, ISupplierService SupplierService)
        {
            _ICommonService = ICommonService;
            _ProductService = ProductService;
            _ColdStorageService = ColdStorageService;
            _PurchaseService = PurchaseService;
            _AdminService = AdminService;
            _ISupplierService = SupplierService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddInwardDetails(Int64? InwardID)
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            ViewBag.ColdStorage = _ColdStorageService.GetAllColdStorageName();
            ViewBag.Supplier = _ISupplierService.GetAllSupplierName();
            try
            {
                AddInwardDetails objModel = _ColdStorageService.GetPurchaseOrderDetailsByInwardID(Convert.ToInt64(InwardID));
                return View(objModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult AddInwardBill(AddInwardDetails data)
        {
            string respose = "";
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
                    respose = _ColdStorageService.AddInward(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetUnit(long ProductID)
        {
            var lstUnit = _ColdStorageService.GetAutoCompleteProductDetaiForInward(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        public JsonResult txtColdStorageName_TextChanged(string obj)
        {
            int quntity = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["drugQuntityshow"].ToString());
            List<ColdStorageName1> data = _ColdStorageService.GetColdstorageByTextChange(obj).Select(x => new ColdStorageName1() { Name = x }).Take(quntity).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult txtColdStorageName_AfterColdStorageSelect(string obj)
        {
            try
            {
                string[] namearr = obj.Split(',');
                string ID = string.Empty;
                if (namearr.Length > 1)
                {
                    var response = _ColdStorageService.GetColdStorageByColdStorageID(Convert.ToInt64(namearr[0]));
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetLastPurchaseProductRatePerKG(long ProductID)
        {
            var lstUnit = _PurchaseService.GetLastPurchaseProductRatePerKG(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        // 22-12-2022 
        public ActionResult Inward()
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            ViewBag.ColdStorage = _ColdStorageService.GetAllColdStorageName();

            return View();
        }

        // 22-12-2022
        public PartialViewResult InwardList(InwardSearchRequest model)
        {
            ViewBag.Godown = _ProductService.GetAllGodownName();
            ViewBag.ColdStorage = _ColdStorageService.GetAllColdStorageName();

            List<InwardListResponse> objModel = _ColdStorageService.GetAllColdStorage_InwardList(model.challanDate, model.ColdStorageID, model.LotNo, model.ProductID);
            //objModel = objModel.OrderBy(x => x.ProductName).ToList();
            return PartialView(objModel);
        }

        public ActionResult Delete(long? InwardID, bool IsDelete)
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
                    _ColdStorageService.DeleteColdstorageInward(InwardID.Value, IsDelete);
                    return RedirectToAction("InwardList");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("InwardList");
            }
        }

        //Outward Details
        public ActionResult AddOutwardDetails(Int64? OutwardID)
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            try
            {
                InwardListResponse objModel = _ColdStorageService.GetAllColdStorage_OutwardID(Convert.ToInt64(OutwardID));
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 28-12-2022
        [HttpPost]
        public ActionResult AddOutwardBill(List<InwardListResponse> data)
        {
            lock (Lock)
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
                        //if (data[0].OutwardID == 0)
                        //{
                        //    data[0].CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        //    data[0].CreatedOn = DateTime.Now;
                        //}
                        //else
                        //{
                        //    data[0].CreatedBy = data[0].CreatedBy;
                        //    data[0].CreatedOn = data[0].CreatedOn;

                        //}
                        //data[0].UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        //data[0].UpdatedOn = DateTime.Now;

                        long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        long respose = _ColdStorageService.AddOutward(data, UserID);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
        }


        // 01-02-2023
        [HttpPost]
        public ActionResult UpdateOutwardBill(InwardListResponse data)
        {
            long respose = 0;
            lock (Lock)
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
                        if (data.OutwardID != 0)
                        {
                            data.CreatedBy = data.CreatedBy;
                            data.CreatedOn = data.CreatedOn;

                            data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            data.UpdatedOn = DateTime.Now;

                            respose = _ColdStorageService.UpdateOutward(data);
                        }
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
        }


        public ActionResult Outward()
        {
            ViewBag.ColdStorage = _ColdStorageService.GetAllColdStorageName();
            return View();
        }

        // 01-02-2023
        public PartialViewResult OutwardList(OutwardSearchRequest model)
        {
            ViewBag.Godown = _ProductService.GetAllGodownName();
            ViewBag.ColdStorage = _ColdStorageService.GetAllColdStorageName();

            List<OutwardListResponse> objModel = _ColdStorageService.GetAllColdStorage_OutwardList(model.ColdStorageID, model.FromDate, model.ToDate);
            return PartialView(objModel);
        }

        public ActionResult DeleteOutward(long? OutwardID, bool IsDelete)
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
                    _ColdStorageService.DeleteColdstorageOutward(OutwardID.Value, IsDelete);
                    return RedirectToAction("Outward");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Outward");
            }
        }

        //08-07-2022
        public JsonResult CheckLotNumberIsExist(long ColdStorageID, string LotNo)
        {
            try
            {
                string LotNumber = _ColdStorageService.GetExistInwardDetails(ColdStorageID, LotNo);
                return Json(LotNumber, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }


        //12-06-2023
        public ActionResult ExportExcelInwardListReport(InwardSearchRequest model)
        {
            decimal lblTotalRemQty = 0;
            decimal lblTotalWeight = 0;
            decimal lblTotalAmount = 0;

            List<InwardListResponseExp> InwardTotal = new List<InwardListResponseExp>();
            List<InwardListResponseExp> objModel = _ColdStorageService.GetAllColdStorage_InwardListExport(model.challanDate, model.ColdStorageID, model.LotNo, model.ProductID);

            if (objModel != null)
            {
                foreach (var record in objModel)
                {
                    InwardTotal.Add(record);
                    if (record.LotNo != "")
                    {
                        lblTotalRemQty += Convert.ToDecimal(record.RemQty);
                        lblTotalWeight += Convert.ToDecimal(record.TotalWeight);
                        lblTotalAmount += Convert.ToDecimal(record.TotalAmount);
                    }
                    else
                    {
                        record.Name = "Total:";
                        record.WeightPerBag = "";
                        record.RatePerKG = "";
                        record.RentPerBags = "";
                    }
                }
            }

            List<InwardListResponseForExport> lstdelsheet = InwardTotal.Select(x => new InwardListResponseForExport()
            {
                ColdStorageName = x.Name,
                DeliveryChallanDate = x.DeliveryChallanDate,
                LotNo = x.LotNo,
                ItemName = x.ProductName,
                Notes = x.Notes,
                Balance = x.RemQty,
                WeightPerBags = x.WeightPerBag,
                TotalWeight = x.TotalWeight,
                RatePerKG = x.RatePerKG,
                TotalAmount = x.TotalAmount,
                RentPerBags = x.RentPerBags
            }).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdelsheet));

            DataRow dRow = ds.Tables[0].NewRow();
            dRow["ColdStorageName"] = "Grand Total:";
            dRow["Balance"] = lblTotalRemQty;
            dRow["TotalWeight"] = lblTotalWeight;
            dRow["TotalAmount"] = lblTotalAmount;
            ds.Tables[0].Rows.Add(dRow);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "InwardListReport.xlsx");

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
        //12-06-2023
    }
}