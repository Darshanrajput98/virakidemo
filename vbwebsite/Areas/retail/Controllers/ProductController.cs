using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using vb.Data;
using vb.Data.Model;
using vb.Service;
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Reflection;
using Ean13Barcode;
using System.Dynamic;
using vb.Data.ViewModel;

namespace vbwebsite.Areas.retail.Controllers
{
    public class ProductController : Controller
    {
        private IRetProductService _productservice;
        private ICommonService _ICommonService;
        private IAdminService _areaservice;

        public ProductController(IRetProductService productservice, ICommonService commonservice, IAdminService areaservice)
        {
            _productservice = productservice;
            _ICommonService = commonservice;
            _areaservice = areaservice;
        }

        public ActionResult ProductCategoryList()
        {
            try
            {
                ViewBag.Godown = _productservice.GetAllRetGodownName();
            }
            catch
            {
                return View("SearchPackList");
            }
            ViewBag.GuiLanguage = _productservice.GetAllLanguage();
            List<RetProductQtyViewModel> lstProducts = _productservice.GetRetProductCategoryList();
            return View(lstProducts);
        }

        public ActionResult UpdateProductDetails(List<RetProductQtyViewModel> data)
        {
            bool respose = _productservice.UpdateProductDetails(data);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageProductCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageProductCategory(RetProductCategoryViewModel data)
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
                    RetProductCategoryMst obj = new RetProductCategoryMst();
                    obj.CategoryID = data.CategoryID;
                    obj.CategoryCode = data.CategoryCode;
                    obj.CategoryTypeID = data.CategoryTypeID;
                    obj.CategoryName = data.CategoryName;
                    obj.CategoryDescription = data.CategoryDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    bool respose = _productservice.AddProductCategory(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageProductCategoryList()
        {
            List<RetProductCategoryListResponse> objModel = _productservice.GetAllProductCategoryList();
            return PartialView(objModel);
        }

        public ActionResult DeleteProductCategory(long? CategoryID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteProductCategory(CategoryID.Value, IsDelete);
                return RedirectToAction("ManageProductCategory");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageProductCategory");
            }
        }

        public ActionResult AddProduct()
        {
            //ViewBag.Pouch = _productservice.GetAllPouchName();
            ViewBag.Pouch = _ICommonService.GetAllPouchName();
            ViewBag.Category = _productservice.GetAllRetCategoryName();
            ViewBag.Godown = _productservice.GetAllRetGodownName();
            ViewBag.Unit = _productservice.GetAllRetUnitName();
            ViewBag.Month = _productservice.GetAllMonth();
            ViewBag.UnitGui = _productservice.GetAllRetGuiUnitName();
            ViewBag.GuiLanguage = _productservice.GetAllLanguage();
            ViewBag.Country = _productservice.GetAllCountryName();

            //31 May, 2021 Sonal Gandhi
            ViewBag.Currency = _productservice.GetAllCurrency();

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(RetProductViewModel data)
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
                    if (data.ProductID == 0)
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
                    bool respose = _productservice.AddProduct(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ProductList()
        {
            ViewBag.tblRowGuiLanguage = _productservice.GetAllLanguage();
            List<RetProductListResponse> objModel = _productservice.GetAllProductList();
            return PartialView(objModel);
        }

        public ActionResult DeleteProduct(long? ProductID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteProduct(ProductID.Value, IsDelete);
                return RedirectToAction("AddProduct");
            }
            catch (Exception)
            {
                return RedirectToAction("AddProduct");
            }
        }

        public ActionResult GetProductQtyList(long ProductID)
        {
            List<RetProductQtyViewModel> lstProductQty = _productservice.GetAllRetailProductQtyListByProductID(ProductID);
            List<RetProdGuiViewModel> lstRetProductGui = _productservice.GetAllRetailProductListByProductID(ProductID);
            return Json(new { lstProductQty, lstRetProductGui }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCountryWiseProductName(long ProductID)
        {
            List<CountryWiseProductViewModel> lstCountryWiseProductName = _productservice.GetCountryWiseProductNameByProductID(ProductID);
            return Json(new { lstCountryWiseProductName }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ManagePouch()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Pouch = _ICommonService.GetAllPouchName();
            return View();
        }

        [HttpPost]
        public ActionResult ManagePouch(PouchViewModel data)
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
                    Pouch_Mst obj = new Pouch_Mst();
                    obj.PouchID = data.PouchID;
                    obj.PouchNameID = data.PouchNameID;
                    obj.PouchDescription = data.PouchDescription;
                    obj.PouchQuantity = data.PouchQuantity;
                    obj.Material = data.Material;
                    obj.Weight = data.Weight;
                    obj.KG = data.KG;
                    obj.GodownID = data.GodownID;
                    obj.MinPouchQuantity = data.MinPouchQuantity;

                    if (obj.PouchID == 0)
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
                    bool respose = _productservice.AddPouch(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManagePouchList()
        {
            List<PouchListResponse> objModel = _productservice.GetAllPouchList();
            return PartialView(objModel);
        }

        public ActionResult DeletePouch(long? PouchID, bool IsDelete)
        {
            try
            {
                _productservice.DeletePouch(PouchID.Value, IsDelete);
                return RedirectToAction("ManagePouch");
            }
            catch (Exception)
            {
                return RedirectToAction("ManagePouch");
            }
        }

        public ActionResult ExportExcelPouch()
        {
            var PouchList = _productservice.GetAllPouchList();
            List<PouchExport> lstpouch = PouchList.Select(x => new PouchExport()
            {
                PouchName = x.PouchName,
                HSNNo = x.HSNNumber,
                Description = x.PouchDescription,
                Quantity = x.PouchQuantity,
                Godown = x.GodownName,
                MinQuantity = x.MinPouchQuantity
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable2(lstpouch));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "PouchList.xls");
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


        public static DataTable ToDataTable2<T>(List<T> items)
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


        public ActionResult ManagePackageStation()
        {
            ViewBag.Godown = _productservice.GetAllRetGodownName();
            ViewBag.SalesPerson = _productservice.GetAllSalesPersonName();
            return View();
        }

        [HttpPost]
        public ActionResult ManagePackageStation(PackageStationViewModel data)
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
                    PackageStation_Mst obj = new PackageStation_Mst();
                    obj.PackageStationID = data.PackageStationID;
                    obj.PackageStationName = data.PackageStationName;
                    obj.GodownID = data.GodownID;
                    obj.UserID = data.UserID;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    bool respose = _productservice.AddPackageStation(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManagePackageStationList()
        {
            List<PackageStationListResponse> objModel = _productservice.GetAllPackageStationList();
            return PartialView(objModel);
        }

        public ActionResult DeletePackageStation(long? PackageStationID, bool IsDelete)
        {
            try
            {
                _productservice.DeletePackageStation(PackageStationID.Value, IsDelete);
                return RedirectToAction("ManagePackageStation");
            }
            catch (Exception)
            {
                return RedirectToAction("ManagePackageStation");
            }
        }

        private Ean13 ean13 = null;

        private string CreateEan13(string number)
        {
            ean13 = new Ean13();
            ean13.CountryCode = number.Substring(0, 3);
            ean13.ManufacturerCode = number.Substring(3, 5);
            ean13.ProductCode = number.Substring(8, 4);
            ean13.ChecksumDigit = "0";
            ean13.Scale = 1.5F;
            System.Drawing.Bitmap bmp = ean13.CreateBitmap();
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        [HttpPost]
        public ActionResult PrintBarcode(DateTime date1, string id, string name, string mrp, string Batchno, int noofbarcodes, long godown, string Productbarcode, string NutritionValue, string ContentValue, long CategoryID, long ProductQtyID, string gramperkg)
        {
            if (CategoryID != 9)
            {
                if (NutritionValue == "" && ContentValue == "")
                {
                    try
                    {
                        string dat = date1.ToString("dd/MM/yyyy");
                        string[] arr = name.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/Barcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/Barcode.rdlc");
                        //}



                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/Barcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/Barcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= noofbarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = mrp;
                            obj.GramPerKG = gramperkg;

                            // obj.Batch = Batchno.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                            obj.BarcodeImage = CreateEan13(Productbarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            LabelData.Add(obj);
                        }
                        // long GodownID = Convert.ToInt64(Request.Cookies["GodownID"].Value);
                        // Add Barcode QTY Details 30-03-2020
                        long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>9in</PageWidth>" +
                        "  <PageHeight>13in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}

                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    try
                    {
                        string dat = date1.ToString("dd/MM/yyyy");
                        string[] arr = name.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }

                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/ContentBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/ContentBarcode.rdlc");
                        //}




                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/ContentBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcode.rdlc");
                        }
                        lr.ReportPath = path;

                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= noofbarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;

                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = mrp;
                            obj.GramPerKG = gramperkg;
                            // obj.Batch = Batchno.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                            obj.BarcodeImage = CreateEan13(Productbarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            obj.NutritionValue = NutritionValue;
                            obj.ContentValue = ContentValue;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020
                        long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>13in</PageWidth>" +
                        "  <PageHeight>9in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}

                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            else
            {
                if (NutritionValue == "" && ContentValue == "")
                {
                    try
                    {
                        string dat = date1.ToString("dd/MM/yyyy");
                        string[] arr = name.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/NonFoodBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/NonFoodBarcode.rdlc");
                        //}


                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/NonFoodBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= noofbarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = mrp;
                            obj.GramPerKG = gramperkg;

                            // obj.Batch = Batchno.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                            obj.BarcodeImage = CreateEan13(Productbarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020
                        long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>9in</PageWidth>" +
                        "  <PageHeight>13in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}

                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    try
                    {
                        string dat = date1.ToString("dd/MM/yyyy");
                        string[] arr = name.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }

                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/NonFoodContentBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/NonFoodContentBarcode.rdlc");
                        //}


                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/NonFoodContentBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodContentBarcode.rdlc");
                        }
                        lr.ReportPath = path;

                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= noofbarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;

                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = mrp;
                            obj.GramPerKG = gramperkg;

                            // obj.Batch = Batchno.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                            obj.BarcodeImage = CreateEan13(Productbarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            obj.NutritionValue = NutritionValue;
                            obj.ContentValue = ContentValue;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020
                        long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>13in</PageWidth>" +
                        "  <PageHeight>9in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}



                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
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

        // New Print Label for language wise(Updated on 15-04-2019)
        [HttpPost]
        public ActionResult PrintLabel(string name, int labelno, long Language1, long Language2, long ProductIDlabel)
        {
            if (Language1 == 0 && Language2 == 0)
            {
                try
                {
                    LocalReport lr = new LocalReport();
                    string path = "";
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/Label.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/Label.rdlc");
                    //}

                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/Label.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/Label.rdlc");
                    }
                    lr.ReportPath = path;
                    List<Product_Mst> LabelData = new List<Product_Mst>();
                    for (int f = 1; f <= labelno; f++)
                    {
                        Product_Mst obj = new Product_Mst();
                        obj.ProductName = name;
                        LabelData.Add(obj);
                    }
                    DataTable header = Common.ToDataTable(LabelData);
                    ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                    lr.DataSources.Add(MedsheetHeader);
                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =

                        "<DeviceInfo>" +
                    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                         "  <PageWidth>8.7in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
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
                    string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                    string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
                    BinaryWriter Writer = null;
                    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                    Writer.Write(renderedBytes);
                    Writer.Flush();
                    Writer.Close();
                    //string url = "";
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
                    //}
                    //else
                    //{
                    //    url = "http://" + Request.Url.Host + "/Label/" + name1;
                    //}


                    string url = "";
                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                    {
                        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Label/" + name1;
                    }
                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                    {
                        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name1;
                    }
                    else
                    {
                        url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name1;
                    }
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (Language1 != 0 && Language2 != 0)
                {
                    try
                    {
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/LanguageLabel.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/LanguageLabel.rdlc");
                        //}

                        if (Language1 == 3)
                        {
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ExportLanguageLabelArabic1.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/ExportLanguageLabelArabic1.rdlc");
                            //}


                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/ExportLanguageLabelArabic1.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLanguageLabelArabic1.rdlc");
                            }
                        }
                        else if (Language2 == 3)
                        {
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ExportLanguageLabelArabic2.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/ExportLanguageLabelArabic2.rdlc");
                            //}


                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/ExportLanguageLabelArabic2.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLanguageLabelArabic2.rdlc");
                            }

                        }
                        else
                        {
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ExportLanguageLabel.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/ExportLanguageLabel.rdlc");
                            //}


                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/ExportLanguageLabel.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLanguageLabel.rdlc");
                            }

                        }

                        lr.ReportPath = path;
                        var ProductName1 = _productservice.GetProductNameByLanguageID(Language1, ProductIDlabel);
                        var ProductName2 = _productservice.GetProductNameByLanguageID2(Language2, ProductIDlabel);
                        List<ProductNameByLanguageID> LabelData = new List<ProductNameByLanguageID>();
                        for (int f = 1; f <= labelno; f++)
                        {
                            ProductNameByLanguageID obj = new ProductNameByLanguageID();
                            obj.ProductName1 = ProductName1.ProductName1;
                            obj.ProductName2 = ProductName2.ProductName2;
                            LabelData.Add(obj);
                        }
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                        //    "<DeviceInfo>" +
                            //"  <OutputFormat>" + reportType + "</OutputFormat>" +
                            //     "  <PageWidth>8.7in</PageWidth>" +
                            //"  <PageHeight>11.69in</PageHeight>" +
                            //"  <MarginTop>1cm</MarginTop>" +
                            //"  <MarginLeft>1cm</MarginLeft>" +
                            //"  <MarginRight>1cm</MarginRight>" +
                            //"  <MarginBottom>1cm</MarginBottom>" +
                            //"</DeviceInfo>";

                                "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                                 "  <PageWidth>8.7in</PageWidth>" +
                            "  <PageHeight>11.69in</PageHeight>" +
                            "  <MarginTop>0.3cm</MarginTop>" +
                            "  <MarginLeft>0.58cm</MarginLeft>" +
                            "  <MarginRight>0.05cm</MarginRight>" +
                            "  <MarginBottom>0.05cm</MarginBottom>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Label/" + name1;
                        //}



                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Label/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (Language1 != 0)
                    {
                        try
                        {
                            LocalReport lr = new LocalReport();
                            string path = "";



                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ExportLabel.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/ExportLabel.rdlc");
                            //}

                            if (Language1 == 3)
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ExportLabelArabic1.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ExportLabelArabic1.rdlc");
                                //}

                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ExportLabelArabic1.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLabelArabic1.rdlc");
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ExportLabel.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ExportLabel.rdlc");
                                //}

                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ExportLabel.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLabel.rdlc");
                                }
                            }



                            lr.ReportPath = path;
                            var ProductName = _productservice.GetProductNameByLanguageID(Language1, ProductIDlabel);
                            List<Product_Mst> LabelData = new List<Product_Mst>();
                            for (int f = 1; f <= labelno; f++)
                            {
                                Product_Mst obj = new Product_Mst();
                                obj.ProductName = ProductName.ProductName1;
                                LabelData.Add(obj);
                            }
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =


                             "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                                 "  <PageWidth>8.7in</PageWidth>" +
                            "  <PageHeight>11.69in</PageHeight>" +
                            "  <MarginTop>0.05cm</MarginTop>" +
                            "  <MarginLeft>0.58cm</MarginLeft>" +
                            "  <MarginRight>0.05cm</MarginRight>" +
                            "  <MarginBottom>0.05cm</MarginBottom>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/Label/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Label/" + name;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        try
                        {
                            LocalReport lr = new LocalReport();
                            string path = "";

                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ExportLabel.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/ExportLabel.rdlc");
                            //}

                            if (Language2 == 3)
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ExportLabelArabic2.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ExportLabelArabic2.rdlc");
                                //}

                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ExportLabelArabic2.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLabelArabic2.rdlc");
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ExportLabel.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ExportLabel.rdlc");
                                //}



                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ExportLabel.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExportLabel.rdlc");
                                }
                            }


                            lr.ReportPath = path;
                            var ProductName2 = _productservice.GetProductNameByLanguageID2(Language2, ProductIDlabel);
                            List<Product_Mst> LabelData = new List<Product_Mst>();
                            for (int f = 1; f <= labelno; f++)
                            {
                                Product_Mst obj = new Product_Mst();
                                obj.ProductName = ProductName2.ProductName2;
                                LabelData.Add(obj);
                            }
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                            //    "<DeviceInfo>" +
                                //"  <OutputFormat>" + reportType + "</OutputFormat>" +
                                //     "  <PageWidth>8.7in</PageWidth>" +
                                //"  <PageHeight>11.69in</PageHeight>" +
                                //"  <MarginTop>1cm</MarginTop>" +
                                //"  <MarginLeft>1cm</MarginLeft>" +
                                //"  <MarginRight>1cm</MarginRight>" +
                                //"  <MarginBottom>1cm</MarginBottom>" +
                                //"</DeviceInfo>";

                             "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                                 "  <PageWidth>8.7in</PageWidth>" +
                            "  <PageHeight>11.69in</PageHeight>" +
                            "  <MarginTop>0.05cm</MarginTop>" +
                            "  <MarginLeft>0.58cm</MarginLeft>" +
                            "  <MarginRight>0.05cm</MarginRight>" +
                            "  <MarginBottom>0.05cm</MarginBottom>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/Label/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Label/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Label/" + name1;
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
        }


        //[HttpPost]
        //public ActionResult PrintLabel(string name, int labelno, long Language1, long Language2)
        //{

        //    if (Language1 == 0 && Language2 == 0)
        //    {
        //        try
        //        {
        //            LocalReport lr = new LocalReport();
        //            string path = "";
        //            if (Request.Url.Host.Contains("localhost"))
        //            {
        //                path = "Report/Label.rdlc";
        //            }
        //            else
        //            {
        //                path = Server.MapPath("~/Report/Label.rdlc");
        //            }
        //            lr.ReportPath = path;
        //            List<Product_Mst> LabelData = new List<Product_Mst>();
        //            for (int f = 1; f <= labelno; f++)
        //            {
        //                Product_Mst obj = new Product_Mst();
        //                obj.ProductName = name;
        //                LabelData.Add(obj);
        //            }
        //            DataTable header = Common.ToDataTable(LabelData);
        //            ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
        //            lr.DataSources.Add(MedsheetHeader);
        //            string reportType = "pdf";
        //            string mimeType;
        //            string encoding;
        //            string fileNameExtension;
        //            string deviceInfo =

        //                "<DeviceInfo>" +
        //            "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //                 "  <PageWidth>8.7in</PageWidth>" +
        //            "  <PageHeight>11.69in</PageHeight>" +
        //            "  <MarginTop>1cm</MarginTop>" +
        //            "  <MarginLeft>1cm</MarginLeft>" +
        //            "  <MarginRight>1cm</MarginRight>" +
        //            "  <MarginBottom>1cm</MarginBottom>" +
        //            "</DeviceInfo>";

        //            Warning[] warnings;
        //            string[] streams;
        //            byte[] renderedBytes;
        //            renderedBytes = lr.Render(
        //                reportType,
        //                deviceInfo,
        //                out mimeType,
        //                out encoding,
        //                out fileNameExtension,
        //                out streams,
        //                out warnings);
        //            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
        //            string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
        //            BinaryWriter Writer = null;
        //            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
        //            Writer.Write(renderedBytes);
        //            Writer.Flush();
        //            Writer.Close();
        //            string url = "";
        //            if (Request.Url.Host.Contains("localhost"))
        //            {
        //                url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
        //            }
        //            else
        //            {
        //                url = "http://" + Request.Url.Host + "/Label/" + name1;
        //            }

        //            return Json(url, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(false, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {


        //        if (Language1 != 0 && Language2 != 0)
        //        {
        //            try
        //            {
        //                LocalReport lr = new LocalReport();
        //                string path = "";
        //                if (Request.Url.Host.Contains("localhost"))
        //                {
        //                    path = "Report/Label.rdlc";
        //                }
        //                else
        //                {
        //                    path = Server.MapPath("~/Report/Label.rdlc");
        //                }
        //                lr.ReportPath = path;
        //                List<Product_Mst> LabelData = new List<Product_Mst>();
        //                for (int f = 1; f <= labelno; f++)
        //                {
        //                    Product_Mst obj = new Product_Mst();
        //                    obj.ProductName = name;
        //                    LabelData.Add(obj);
        //                }
        //                DataTable header = Common.ToDataTable(LabelData);
        //                ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
        //                lr.DataSources.Add(MedsheetHeader);
        //                string reportType = "pdf";
        //                string mimeType;
        //                string encoding;
        //                string fileNameExtension;
        //                string deviceInfo =

        //                    "<DeviceInfo>" +
        //                "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //                     "  <PageWidth>8.7in</PageWidth>" +
        //                "  <PageHeight>11.69in</PageHeight>" +
        //                "  <MarginTop>1cm</MarginTop>" +
        //                "  <MarginLeft>1cm</MarginLeft>" +
        //                "  <MarginRight>1cm</MarginRight>" +
        //                "  <MarginBottom>1cm</MarginBottom>" +
        //                "</DeviceInfo>";

        //                Warning[] warnings;
        //                string[] streams;
        //                byte[] renderedBytes;
        //                renderedBytes = lr.Render(
        //                    reportType,
        //                    deviceInfo,
        //                    out mimeType,
        //                    out encoding,
        //                    out fileNameExtension,
        //                    out streams,
        //                    out warnings);
        //                string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
        //                string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
        //                BinaryWriter Writer = null;
        //                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
        //                Writer.Write(renderedBytes);
        //                Writer.Flush();
        //                Writer.Close();
        //                string url = "";
        //                if (Request.Url.Host.Contains("localhost"))
        //                {
        //                    url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
        //                }
        //                else
        //                {
        //                    url = "http://" + Request.Url.Host + "/Label/" + name1;
        //                }

        //                return Json(url, JsonRequestBehavior.AllowGet);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(false, JsonRequestBehavior.AllowGet);
        //            }


        //        }
        //        else
        //        {
        //            try
        //            {
        //                LocalReport lr = new LocalReport();
        //                string path = "";
        //                if (Request.Url.Host.Contains("localhost"))
        //                {
        //                    path = "Report/Label.rdlc";
        //                }
        //                else
        //                {
        //                    path = Server.MapPath("~/Report/Label.rdlc");
        //                }
        //                lr.ReportPath = path;
        //                List<Product_Mst> LabelData = new List<Product_Mst>();
        //                for (int f = 1; f <= labelno; f++)
        //                {
        //                    Product_Mst obj = new Product_Mst();
        //                    obj.ProductName = name;
        //                    LabelData.Add(obj);
        //                }
        //                DataTable header = Common.ToDataTable(LabelData);
        //                ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
        //                lr.DataSources.Add(MedsheetHeader);
        //                string reportType = "pdf";
        //                string mimeType;
        //                string encoding;
        //                string fileNameExtension;
        //                string deviceInfo =

        //                    "<DeviceInfo>" +
        //                "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //                     "  <PageWidth>8.7in</PageWidth>" +
        //                "  <PageHeight>11.69in</PageHeight>" +
        //                "  <MarginTop>1cm</MarginTop>" +
        //                "  <MarginLeft>1cm</MarginLeft>" +
        //                "  <MarginRight>1cm</MarginRight>" +
        //                "  <MarginBottom>1cm</MarginBottom>" +
        //                "</DeviceInfo>";

        //                Warning[] warnings;
        //                string[] streams;
        //                byte[] renderedBytes;
        //                renderedBytes = lr.Render(
        //                    reportType,
        //                    deviceInfo,
        //                    out mimeType,
        //                    out encoding,
        //                    out fileNameExtension,
        //                    out streams,
        //                    out warnings);
        //                string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
        //                string Pdfpathcreate = Server.MapPath("~/Label/" + name1);
        //                BinaryWriter Writer = null;
        //                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
        //                Writer.Write(renderedBytes);
        //                Writer.Flush();
        //                Writer.Close();
        //                string url = "";
        //                if (Request.Url.Host.Contains("localhost"))
        //                {
        //                    url = "http://" + Request.Url.Host + ":6551/Label/" + name1;
        //                }
        //                else
        //                {
        //                    url = "http://" + Request.Url.Host + "/Label/" + name1;
        //                }

        //                return Json(url, JsonRequestBehavior.AllowGet);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(false, JsonRequestBehavior.AllowGet);
        //            }


        //        }
        //        //   return Json(false, JsonRequestBehavior.AllowGet);

        //    }

        //}



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                    dataTable.Columns.Add(values[i].ToString());
                }
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public ActionResult ExportExcelProduct(long CurrencyID = 0, decimal CurrencyRate = 0)
        {
            var ProductList = _productservice.GetRetProductListForExp(CurrencyRate);
            var currencySign = _productservice.GetCurrencySignByCurrencyID(CurrencyID);

            List<RetProductListForExp> lstproduct = ProductList.Select(x => new RetProductListForExp()
            {
                CategoryName = x.CategoryName,
                ProductName = x.ProductName,
                ProductQuantity = x.ProductQuantity,
                UnitName = x.UnitName,
                PouchSize = x.PouchSize,
                HSNNumber = x.HSNNumber,
                ProductPrice = string.IsNullOrEmpty(currencySign) ? x.ConvertedProductPrice.ToString() : currencySign + x.ConvertedProductPrice.ToString(),
                ProductMRP = string.IsNullOrEmpty(currencySign) ? x.ConvertedProductMRP.ToString() : currencySign + x.ConvertedProductMRP.ToString(),
                ProductBarcode = x.ProductBarcode,
                BestBefore = x.BestBefore,
                SGST = x.SGST,
                CGST = x.CGST,
                IGST = x.IGST,
                HFor = x.HFor
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable1(lstproduct));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "ProductList.xls");
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

        public static DataTable ToDataTable1<T>(List<T> items)
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

        [HttpPost]
        public ActionResult PrintBlankBarcode(DateTime BlankBarcodePackageDate, DateTime BestBeforeDate, string ProductID, string ProductName, string MRP, string BlankBarcodeBatchNo, int NoOfBlankBarcodes, long Godown, string ProductBarcode, string NutritionValue, string ContentValue, long CategoryID, long ProductQtyID, string GramPerKG)
        {
            if (CategoryID != 9)
            {
                if (NutritionValue == "" && ContentValue == "")
                {
                    try
                    {
                        string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = ProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/BlankBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/BlankBarcode.rdlc");
                        //}



                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/BlankBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/BlankBarcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= NoOfBlankBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = PackageDate;
                            //   obj.ProductName = arr[0];
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = MRP;
                            obj.GramPerKG = GramPerKG;
                            // obj.Batch = BlankBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + BlankBarcodeBatchNo.ToString();

                            obj.BarcodeImage = CreateEan13(ProductBarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 1
                        long respose = _productservice.AddBarcodeQuantityDetails(ProductID, ProductQtyID, ProductName, BlankBarcodePackageDate, NoOfBlankBarcodes, Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>9in</PageWidth>" +
                        "  <PageHeight>13in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    try
                    {
                        string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = ProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/ContentBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/ContentBarcode.rdlc");
                        //}


                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/ContentBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= NoOfBlankBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = PackageDate;
                            //   obj.ProductName = arr[0];
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = MRP;
                            obj.GramPerKG = GramPerKG;
                            // obj.Batch = BlankBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + BlankBarcodeBatchNo.ToString();

                            // obj.Productbarcode = ProductBarcode;
                            obj.BarcodeImage = CreateEan13(ProductBarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                            obj.NutritionValue = NutritionValue;
                            obj.ContentValue = ContentValue;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 2
                        long respose = _productservice.AddBarcodeQuantityDetails(ProductID, ProductQtyID, ProductName, BlankBarcodePackageDate, NoOfBlankBarcodes, Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>13in</PageWidth>" +
                        "  <PageHeight>9in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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

                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();

                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                        //}

                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            else
            {
                if (NutritionValue == "" && ContentValue == "")
                {
                    try
                    {
                        string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = ProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/NonFoodBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/NonFoodBarcode.rdlc");
                        //}

                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/NonFoodBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcode.rdlc");
                        }
                        lr.ReportPath = path;

                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= NoOfBlankBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = PackageDate;
                            //   obj.ProductName = arr[0];
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = MRP;
                            obj.GramPerKG = GramPerKG;

                            // obj.Batch = BlankBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + BlankBarcodeBatchNo.ToString();

                            obj.BarcodeImage = CreateEan13(ProductBarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 3
                        long respose = _productservice.AddBarcodeQuantityDetails(ProductID, ProductQtyID, ProductName, BlankBarcodePackageDate, NoOfBlankBarcodes, Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>9in</PageWidth>" +
                        "  <PageHeight>13in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }

                else
                {
                    try
                    {
                        string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = ProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/NonFoodContentBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/NonFoodContentBarcode.rdlc");
                        //}



                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/NonFoodContentBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodContentBarcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= NoOfBlankBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = PackageDate;
                            //   obj.ProductName = arr[0];
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = MRP;
                            obj.GramPerKG = GramPerKG;
                            // obj.Batch = BlankBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + BlankBarcodeBatchNo.ToString();

                            // obj.Productbarcode = ProductBarcode;
                            obj.BarcodeImage = CreateEan13(ProductBarcode);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                            obj.NutritionValue = NutritionValue;
                            obj.ContentValue = ContentValue;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 4
                        long respose = _productservice.AddBarcodeQuantityDetails(ProductID, ProductQtyID, ProductName, BlankBarcodePackageDate, NoOfBlankBarcodes, Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                "<DeviceInfo>" +
                    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>13in</PageWidth>" +
                    "  <PageHeight>9in</PageHeight>" +
                    "  <MarginTop>0.5cm</MarginTop>" +
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

                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();

                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
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



        [HttpPost]
        public ActionResult PrintSingleBarcode(DateTime SingleBarcodePackageDate, string SingleBarcodeProductID, string SingleBarcodeProductName, string SingleBarcodeProductMRP, string SingleBarcodeBatchNo, int SingleBarcodes, long SingleBarcodeGodownID, string SingleBarcodeBarcodeNo, string SingleBarcodeNutritionValue, string SingleBarcodeContentValue, long SingleBarcodeCategoryID, long SingleBarcodeProductQtyID, string SingleBarcodeGramPerKG)
        {
            if (SingleBarcodeCategoryID != 9)
            {
                if (SingleBarcodeNutritionValue == "" && SingleBarcodeContentValue == "")
                {
                    try
                    {
                        string dat = SingleBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = SingleBarcodeProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/SingleBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/SingleBarcode.rdlc");
                        //}



                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/SingleBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SingleBarcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(SingleBarcodeGodownID);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(SingleBarcodeProductID));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= SingleBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = SingleBarcodeProductMRP;
                            obj.GramPerKG = SingleBarcodeGramPerKG;

                            //  obj.Batch = SingleBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + SingleBarcodeBatchNo.ToString();

                            obj.BarcodeImage = CreateEan13(SingleBarcodeBarcodeNo);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 1
                        long respose = _productservice.AddBarcodeQuantityDetails(SingleBarcodeProductID, SingleBarcodeProductQtyID, SingleBarcodeProductName, SingleBarcodePackageDate, SingleBarcodes, SingleBarcodeGodownID, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                            "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>1.78in</PageWidth>" +
                        "  <PageHeight>2.2in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
                        "  <MarginLeft>0.5cm</MarginLeft>" +
                        "  <MarginRight>0.3cm</MarginRight>" +
                        "  <MarginBottom>0.5cm</MarginBottom>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    try
                    {
                        string dat = SingleBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = SingleBarcodeProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }

                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/SingleContentBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/SingleContentBarcode.rdlc");
                        //}

                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/SingleContentBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SingleContentBarcode.rdlc");
                        }
                        lr.ReportPath = path;

                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(SingleBarcodeGodownID);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(SingleBarcodeProductID));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= SingleBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;

                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = SingleBarcodeProductMRP;
                            obj.GramPerKG = SingleBarcodeGramPerKG;

                            // obj.Batch = SingleBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + SingleBarcodeBatchNo.ToString();

                            obj.BarcodeImage = CreateEan13(SingleBarcodeBarcodeNo);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            obj.NutritionValue = SingleBarcodeNutritionValue;
                            obj.ContentValue = SingleBarcodeContentValue;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 2
                        long respose = _productservice.AddBarcodeQuantityDetails(SingleBarcodeProductID, SingleBarcodeProductQtyID, SingleBarcodeProductName, SingleBarcodePackageDate, SingleBarcodes, SingleBarcodeGodownID, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                        "<DeviceInfo>" +
                   "  <OutputFormat>" + reportType + "</OutputFormat>" +
               "  <PageWidth>1.78in</PageWidth>" +
                   "  <PageHeight>4.3in</PageHeight>" +
                   "  <MarginTop>0.5cm</MarginTop>" +
                   "  <MarginLeft>0.5cm</MarginLeft>" +
                   "  <MarginRight>0.3cm</MarginRight>" +
                   "  <MarginBottom>0.5cm</MarginBottom>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            else
            {
                if (SingleBarcodeNutritionValue == "" && SingleBarcodeContentValue == "")
                {
                    try
                    {
                        string dat = SingleBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = SingleBarcodeProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }
                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/SingleNonFoodBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/SingleNonFoodBarcode.rdlc");
                        //}



                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/SingleNonFoodBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SingleNonFoodBarcode.rdlc");
                        }
                        lr.ReportPath = path;
                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(SingleBarcodeGodownID);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(SingleBarcodeProductID));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= SingleBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;
                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = SingleBarcodeProductMRP;
                            obj.GramPerKG = SingleBarcodeGramPerKG;
                            //  obj.Batch = SingleBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + SingleBarcodeBatchNo.ToString();

                            obj.BarcodeImage = CreateEan13(SingleBarcodeBarcodeNo);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 3
                        long respose = _productservice.AddBarcodeQuantityDetails(SingleBarcodeProductID, SingleBarcodeProductQtyID, SingleBarcodeProductName, SingleBarcodePackageDate, SingleBarcodes, SingleBarcodeGodownID, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                             "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>1.78in</PageWidth>" +
                        "  <PageHeight>2.2in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
                        "  <MarginLeft>0.5cm</MarginLeft>" +
                        "  <MarginRight>0.3cm</MarginRight>" +
                        "  <MarginBottom>0.5cm</MarginBottom>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        return Json(url, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    try
                    {
                        string dat = SingleBarcodePackageDate.ToString("dd/MM/yyyy");
                        string[] arr = SingleBarcodeProductName.Split('(');
                        string qty = "";
                        if (arr.Length == 2)
                        {
                            qty = arr[1].Remove(arr[1].Length - 1);
                        }
                        else
                        {
                            qty = arr[2].Remove(arr[2].Length - 1);
                        }

                        string MonthDat = "";
                        LocalReport lr = new LocalReport();
                        string path = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    path = "Report/SingleNonFoodContentBarcode.rdlc";
                        //}
                        //else
                        //{
                        //    path = Server.MapPath("~/Report/SingleNonFoodContentBarcode.rdlc");
                        //}



                        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                        {
                            path = "Report/SingleNonFoodContentBarcode.rdlc";
                        }
                        else
                        {
                            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SingleNonFoodContentBarcode.rdlc");
                        }
                        lr.ReportPath = path;

                        Godown_Mst godowndetails = new Godown_Mst();
                        godowndetails = _productservice.GetGodownDetailsByGodownID(SingleBarcodeGodownID);
                        BestBeforeMonth promonth = new BestBeforeMonth();
                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(SingleBarcodeProductID));
                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                        DateTime theDate = DateTime.Now;
                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();
                        for (int f = 1; f <= SingleBarcodes; f++)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            obj.AddressLine1 = godowndetails.GodownAddress1;
                            obj.AddressLine2 = godowndetails.GodownAddress2;
                            obj.FSSAINo = godowndetails.GodownFSSAINumber;
                            obj.PhoneNo = godowndetails.GodownPhone;
                            obj.GodownName = godowndetails.GodownName;
                            obj.DatePackaging = dat;
                            const int MaxLength = 22;

                            if (arr.Length == 2)
                            {
                                obj.ProductName = arr[0];

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            else
                            {
                                obj.ProductName = arr[0] + " (" + arr[1];
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                            }
                            obj.MRP = SingleBarcodeProductMRP;
                            obj.GramPerKG = SingleBarcodeGramPerKG;
                            // obj.Batch = SingleBarcodeBatchNo.ToString();
                            obj.Batch = godowndetails.GodownCode + "" + SingleBarcodeBatchNo.ToString();

                            obj.BarcodeImage = CreateEan13(SingleBarcodeBarcodeNo);
                            obj.GodownCode = godowndetails.GodownCode;
                            obj.QTY = qty;
                            obj.MonthDate = MonthDat;
                            obj.NutritionValue = SingleBarcodeNutritionValue;
                            obj.ContentValue = SingleBarcodeContentValue;
                            LabelData.Add(obj);
                        }
                        // Add Barcode QTY Details 30-03-2020 - 4
                        long respose = _productservice.AddBarcodeQuantityDetails(SingleBarcodeProductID, SingleBarcodeProductQtyID, SingleBarcodeProductName, SingleBarcodePackageDate, SingleBarcodes, SingleBarcodeGodownID, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                        // Add Barcode QTY Details 30-03-2020
                        DataTable header = Common.ToDataTable(LabelData);
                        ReportDataSource MedsheetHeader = new ReportDataSource("data", header);
                        lr.DataSources.Add(MedsheetHeader);
                        string reportType = "pdf";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                              "<DeviceInfo>" +
                   "  <OutputFormat>" + reportType + "</OutputFormat>" +
               "  <PageWidth>1.78in</PageWidth>" +
                   "  <PageHeight>4.3in</PageHeight>" +
                   "  <MarginTop>0.5cm</MarginTop>" +
                   "  <MarginLeft>0.5cm</MarginLeft>" +
                   "  <MarginRight>0.3cm</MarginRight>" +
                   "  <MarginBottom>0.5cm</MarginBottom>" +
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
                        string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                        string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();
                        //string url = "";
                        //if (Request.Url.Host.Contains("localhost"))
                        //{
                        //    url = "http://" + Request.Url.Host + ":6551/Barcode/" + name1;
                        //}
                        //else
                        //{
                        //    url = "http://" + Request.Url.Host + "/Barcode/" + name1;
                        //}


                        string url = "";
                        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                        {
                            url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                        }
                        else
                        {
                            url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
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


        // CHIRAG 05-02-2019

        [HttpPost]
        public ActionResult PrintExternalBarcode(DateTime BlankBarcodePackageDate, DateTime BestBeforeDate, string ProductID, string ProductQtyID, string ProductName, string MRP, string BlankBarcodeBatchNo, int NoOfBlankBarcodes, long Godown, string ProductBarcode, string NutritionValue, string ContentValue, long CategoryID, long GuiID1, long GuiID2, string DatePackaging1, string BestBefore1, string BatchNo1, string DatePackaging2, string BestBefore2, string BatchNo2, bool chkPlaceOfOrigin, string PlaceOfOrigin, string GramPerKG)
        {
            string NewNutritionValue = NutritionValue.Replace("\n", ", ");
            if (MRP == "0")
            {
                if (CategoryID != 9)
                {
                    if (NutritionValue == "" && ContentValue == "")
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";
                            if (GuiID1 > 0 && GuiID2 > 0)
                            {
                                if (chkPlaceOfOrigin == true)
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/ContentBarcodeLanguagePlaceOfOrigin.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/ContentBarcodeLanguagePlaceOfOrigin.rdlc");
                                    //    //path = "Report/ContentBarcodeLanguagePlaceOfOrigin.rdlc";
                                    //}


                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/ContentBarcodeLanguagePlaceOfOrigin.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguagePlaceOfOrigin.rdlc");
                                    }
                                }
                                else
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/ContentBarcodeLanguage.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/ContentBarcodeLanguage.rdlc");
                                    //    //path = "Report/ContentBarcodeLanguage.rdlc";
                                    //}

                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/ContentBarcodeLanguage.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguage.rdlc");
                                    }
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ContentBarcodeSingleLanguage.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ContentBarcodeSingleLanguage.rdlc");
                                //    //path = "Report/ContentBarcodeSingleLanguage.rdlc";
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ContentBarcodeSingleLanguage.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeSingleLanguage.rdlc");
                                }
                            }
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;
                                if (GuiID1 > 0 && Convert.ToInt64(ProductQtyID) > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);
                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }
                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }
                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        // var clsLabel1Type = typeof(LabelCodes);
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                                //clsLabel1Type.GetProperty(_pair.Key).SetValue(clsLabel1Code, _pair.Value);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1;

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;
                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                    }
                                    else
                                    {
                                        //  gui1 have not laguage dependent value then return default value
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                if (GuiID2 > 0)
                                {
                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);
                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }
                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }

                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductNameVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();
                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }
                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 1
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                            "<DeviceInfo>" +
                       "  <OutputFormat>" + reportType + "</OutputFormat>" +
                   "  <PageWidth>13in</PageWidth>" +
                       "  <PageHeight>9in</PageHeight>" +
                       "  <MarginTop>0.5cm</MarginTop>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}



                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";
                            if (GuiID1 > 0 && GuiID2 > 0)
                            {
                                if (chkPlaceOfOrigin == true)
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/ContentBarcodeLanguageContentPlaceOfOrigin.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/ContentBarcodeLanguageContentPlaceOfOrigin.rdlc");
                                    //    //path = "Report/ContentBarcodeLanguageContentPlaceOfOrigin.rdlc";
                                    //}

                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/ContentBarcodeLanguageContentPlaceOfOrigin.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguageContentPlaceOfOrigin.rdlc");
                                    }
                                }
                                else
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/ContentBarcodeLanguageContent.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/ContentBarcodeLanguageContent.rdlc");
                                    //    // path = "Report/ContentBarcodeLanguageContent.rdlc";
                                    //}


                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/ContentBarcodeLanguageContent.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguageContent.rdlc");
                                    }
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ContentBarcodeSingleLanguageContent.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ContentBarcodeSingleLanguageContent.rdlc");
                                //    //path = "Report/ContentBarcodeSingleLanguageContent.rdlc";
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ContentBarcodeSingleLanguageContent.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeSingleLanguageContent.rdlc");
                                }
                            }
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                //   obj.ProductName = arr[0];
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                // obj.Productbarcode = ProductBarcode;
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                                if (NutritionValue == "")
                                {
                                    obj.NutritionValue = ".";
                                }
                                else
                                {
                                    obj.NutritionValue = NutritionValue;
                                }

                                if (ContentValue == "")
                                {
                                    obj.ContentValue = ".";
                                }
                                else
                                {
                                    obj.ContentValue = ContentValue;
                                }
                                //obj.NutritionValue = NutritionValue;
                                //obj.ContentValue = ContentValue;
                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strContentVal1 = obj.ContentValue;
                                string strContentVal2 = obj.ContentValue;
                                //string strNutritionVal1 = obj.NutritionVal1;
                                //string strNutritionVal2 = obj.NutritionVal2;
                                string strNutritionVal1 = obj.NutritionValue;
                                string strNutritionVal2 = obj.NutritionValue;
                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;
                                if (GuiID1 > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);
                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }
                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }
                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ContentVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ContentGui.Trim() : strContentVal1;
                                        obj.NutritionVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui1.FirstOrDefault().NutritionGui.Trim() : strNutritionVal1;
                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;
                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                    }
                                    else
                                    {
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NutritionVal1 = NewNutritionValue;
                                        obj.ContentVal1 = obj.ContentValue;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                if (GuiID2 > 0)
                                {
                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);
                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }
                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }
                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : obj.MonthDate;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;

                                        obj.ContentVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ContentGui.Trim() : strContentVal2.Trim();
                                        obj.NutritionVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui2.FirstOrDefault().NutritionGui.Trim() : strNutritionVal2.Trim();
                                        //if (NutritionValue != "")
                                        //{
                                        //    obj.NutritionVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui2.FirstOrDefault().NutritionGui.Trim() : strNutritionVal2.Trim();
                                        //}
                                        //else {
                                        //    obj.NutritionVal2 = null;
                                        //}
                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        //  obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();
                                        string substrProductName2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        if (substrProductName2.Length > MaxLength)
                                        {
                                            obj.ProductNameVal2 = substrProductName2.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal2 = substrProductName2;
                                        }
                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }
                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 2
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>13in</PageWidth>" +
                            "  <PageHeight>9in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {   //nonfood
                    if (NutritionValue == "" && ContentValue == "")
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";

                            if (GuiID1 > 0 && GuiID2 > 0)
                            {
                                if (chkPlaceOfOrigin == true)
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/NonFoodBarcodeLanguagePlaceOfOrigin.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguagePlaceOfOrigin.rdlc");
                                    //}

                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/NonFoodBarcodeLanguagePlaceOfOrigin.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguagePlaceOfOrigin.rdlc");
                                    }
                                }
                                else
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/NonFoodBarcodeLanguage.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguage.rdlc");
                                    //}


                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/NonFoodBarcodeLanguage.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguage.rdlc");
                                    }
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/NonFoodBarcodeSingleLanguage.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/NonFoodBarcodeSingleLanguage.rdlc");
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/NonFoodBarcodeSingleLanguage.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeSingleLanguage.rdlc");
                                }
                            }
                            lr.ReportPath = path;

                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                //   obj.ProductName = arr[0];
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");

                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;
                                if (GuiID1 > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);
                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;
                                        //if (LanguageName1.ToLower().Contains("arabic") == true)
                                        //{
                                        //    string UnitName = "";
                                        //    if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        //    {
                                        //        UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        //    }
                                        //    strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui + " " + UnitName;
                                        //}
                                        //else
                                        //{
                                        //    strProdQtyWithUnit1 = obj.QTY;
                                        //}
                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;

                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }

                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }
                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        // var clsLabel1Type = typeof(LabelCodes);
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1;

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;
                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                    }
                                    else
                                    {
                                        //  gui1 have not laguage dependent value then return default value
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                if (GuiID2 > 0)
                                {
                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);
                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }
                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }
                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();
                                        string substrProductName2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        if (substrProductName2.Length > MaxLength)
                                        {
                                            obj.ProductNameVal2 = substrProductName2.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal2 = substrProductName2;
                                        }
                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }

                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 3
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>13in</PageWidth>" +
                            "  <PageHeight>9in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";

                            if (GuiID1 > 0 && GuiID2 > 0)
                            {
                                if (chkPlaceOfOrigin == true)
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/NonFoodBarcodeLanguageContentPlaceOfOrigin.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = "Report/NonFoodBarcodeLanguageContentPlaceOfOrigin.rdlc";
                                    //}


                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/NonFoodBarcodeLanguageContentPlaceOfOrigin.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguageContentPlaceOfOrigin.rdlc");
                                    }
                                }
                                else
                                {
                                    //if (Request.Url.Host.Contains("localhost"))
                                    //{
                                    //    path = "Report/NonFoodBarcodeLanguageContent.rdlc";
                                    //}
                                    //else
                                    //{
                                    //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageContent.rdlc");
                                    //}


                                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                    {
                                        path = "Report/NonFoodBarcodeLanguageContent.rdlc";
                                    }
                                    else
                                    {
                                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguageContent.rdlc");
                                    }
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/NonFoodBarcodeSingleLanguageContent.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/NonFoodBarcodeSingleLanguageContent.rdlc");
                                //}

                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/NonFoodBarcodeSingleLanguageContent.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeSingleLanguageContent.rdlc");
                                }
                            }


                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                //   obj.ProductName = arr[0];
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                // obj.Productbarcode = ProductBarcode;
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");

                                if (NutritionValue == "")
                                {
                                    obj.NutritionValue = ".";
                                }
                                else
                                {
                                    obj.NutritionValue = NutritionValue;
                                }

                                if (ContentValue == "")
                                {
                                    obj.ContentValue = ".";
                                }
                                else
                                {
                                    obj.ContentValue = ContentValue;
                                }

                                //obj.NutritionValue = NutritionValue;
                                //obj.ContentValue = ContentValue;

                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strContentVal1 = obj.ContentValue;
                                string strContentVal2 = obj.ContentValue;

                                string strNutritionVal1 = obj.NutritionValue;
                                string strNutritionVal2 = obj.NutritionValue;

                                //string strNutritionVal1 = obj.NutritionVal1;
                                //string strNutritionVal2 = obj.NutritionVal2;

                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;


                                if (GuiID1 > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);

                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;

                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }

                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }

                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }

                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ContentVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ContentGui.Trim() : strContentVal1;
                                        obj.NutritionVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui1.FirstOrDefault().NutritionGui.Trim() : strNutritionVal1;

                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;

                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                    }
                                    else
                                    {
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NutritionVal1 = NewNutritionValue;
                                        obj.ContentVal1 = obj.ContentValue;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                if (GuiID2 > 0)
                                {

                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);

                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }
                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }


                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }

                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : obj.MonthDate;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;

                                        obj.ContentVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ContentGui.Trim() : strContentVal2.Trim();
                                        obj.NutritionVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui2.FirstOrDefault().NutritionGui.Trim() : strNutritionVal2.Trim();

                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();


                                        string substrProductName2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        if (substrProductName2.Length > MaxLength)
                                        {
                                            obj.ProductNameVal2 = substrProductName2.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal2 = substrProductName2;
                                        }


                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }
                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 4
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =


                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                            "  <PageWidth>13in</PageWidth>" +
                            "  <PageHeight>9in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
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

                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
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

            //WITH foodmrp
            else
            {
                if (CategoryID != 9)
                {
                    if (NutritionValue == "" && ContentValue == "")
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";
                            if (chkPlaceOfOrigin == true)
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ContentBarcodeLanguageMRPPlaceOfOrigin.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ContentBarcodeLanguageMRPPlaceOfOrigin.rdlc");
                                //}



                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ContentBarcodeLanguageMRPPlaceOfOrigin.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguageMRPPlaceOfOrigin.rdlc");
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ContentBarcodeLanguageMRP.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ContentBarcodeLanguageMRP.rdlc");
                                //}



                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ContentBarcodeLanguageMRP.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguageMRP.rdlc");
                                }
                            }
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ContentBarcodeLanguageMRP.rdlc";
                            //}
                            //else
                            //{
                            //    path = "Report/ContentBarcodeLanguageMRP.rdlc";
                            //}
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");

                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;

                                if (GuiID1 > 0 && Convert.ToInt64(ProductQtyID) > 0)
                                {
                                    string LanguageName1 = "";

                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);

                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }
                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }

                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        // var clsLabel1Type = typeof(LabelCodes);
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                                //clsLabel1Type.GetProperty(_pair.Key).SetValue(clsLabel1Code, _pair.Value);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1;

                                        // 10 Jan, 2022 Piyush Limbani
                                        //  obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();

                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;

                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                    }
                                    else
                                    {
                                        //  gui1 have not laguage dependent value then return default value
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }

                                }
                                if (GuiID2 > 0)
                                {

                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);

                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;

                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }

                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }


                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }

                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }

                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductNameVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();

                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }
                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 5
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =


                            "<DeviceInfo>" +
                       "  <OutputFormat>" + reportType + "</OutputFormat>" +
                   "  <PageWidth>13in</PageWidth>" +
                       "  <PageHeight>9in</PageHeight>" +
                       "  <MarginTop>0.5cm</MarginTop>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}



                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {

                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";
                            if (chkPlaceOfOrigin == true)
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ContentBarcodeLanguageContentMRPPlaceOfOrigin.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ContentBarcodeLanguageContentMRPPlaceOfOrigin.rdlc");
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ContentBarcodeLanguageContentMRPPlaceOfOrigin.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguageContentMRPPlaceOfOrigin.rdlc");
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/ContentBarcodeLanguageContentMRP.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/ContentBarcodeLanguageContentMRP.rdlc");
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/ContentBarcodeLanguageContentMRP.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ContentBarcodeLanguageContentMRP.rdlc");
                                }
                            }
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/ContentBarcodeLanguageContentMRP.rdlc";
                            //}
                            //else
                            //{
                            //    path = "Report/ContentBarcodeLanguageContentMRP.rdlc";
                            //}
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                //   obj.ProductName = arr[0];
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                // obj.Productbarcode = ProductBarcode;
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");
                                if (NutritionValue == "")
                                {
                                    obj.NutritionValue = ".";
                                }
                                else
                                {
                                    obj.NutritionValue = NutritionValue;
                                }

                                if (ContentValue == "")
                                {
                                    obj.ContentValue = ".";
                                }
                                else
                                {
                                    obj.ContentValue = ContentValue;
                                }
                                //obj.NutritionValue = NutritionValue;
                                //obj.ContentValue = ContentValue;

                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strContentVal1 = obj.ContentValue;
                                string strContentVal2 = obj.ContentValue;

                                string strNutritionVal1 = obj.NutritionValue;
                                string strNutritionVal2 = obj.NutritionValue;

                                //string strNutritionVal1 = obj.NutritionVal1;
                                //string strNutritionVal2 = obj.NutritionVal2;

                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;


                                if (GuiID1 > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);

                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;

                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }

                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }
                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }


                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ContentVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ContentGui.Trim() : strContentVal1;
                                        obj.NutritionVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui1.FirstOrDefault().NutritionGui.Trim() : strNutritionVal1;

                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;

                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        //obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                    }
                                    else
                                    {
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NutritionVal1 = NewNutritionValue;
                                        obj.ContentVal1 = obj.ContentValue;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                if (GuiID2 > 0)
                                {

                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);

                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;

                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }

                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }


                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }


                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }

                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : obj.MonthDate;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;

                                        obj.ContentVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ContentGui.Trim() : strContentVal2.Trim();
                                        obj.NutritionVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui2.FirstOrDefault().NutritionGui.Trim() : strNutritionVal2.Trim();


                                        //if (NutritionValue != "")
                                        //{
                                        //    obj.NutritionVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui2.FirstOrDefault().NutritionGui.Trim() : strNutritionVal2.Trim();
                                        //}
                                        //else {
                                        //    obj.NutritionVal2 = null;
                                        //}


                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();

                                        string substrProductName2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;

                                        if (substrProductName2.Length > MaxLength)
                                        {
                                            obj.ProductNameVal2 = substrProductName2.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal2 = substrProductName2;
                                        }

                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }
                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 6
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>13in</PageWidth>" +
                            "  <PageHeight>9in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
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

                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                else
                {   //nonfoodmrp
                    if (NutritionValue == "" && ContentValue == "")
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";
                            if (chkPlaceOfOrigin == true)
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/NonFoodBarcodeLanguageMRPPlaceOfOrigin.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageMRPPlaceOfOrigin.rdlc");
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/NonFoodBarcodeLanguageMRPPlaceOfOrigin.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguageMRPPlaceOfOrigin.rdlc");
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/NonFoodBarcodeLanguageMRP.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageMRP.rdlc");
                                //}

                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/NonFoodBarcodeLanguageMRP.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguageMRP.rdlc");
                                }
                            }
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/NonFoodBarcodeLanguageMRP.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageMRP.rdlc");
                            //}
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                //   obj.ProductName = arr[0];
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");

                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;

                                if (GuiID1 > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);

                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;
                                        //if (LanguageName1.ToLower().Contains("arabic") == true)
                                        //{
                                        //    string UnitName = "";
                                        //    if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        //    {
                                        //        UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        //    }
                                        //    strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui + " " + UnitName;
                                        //}
                                        //else
                                        //{
                                        //    strProdQtyWithUnit1 = obj.QTY;
                                        //}

                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;

                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }

                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }


                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        // var clsLabel1Type = typeof(LabelCodes);
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1;

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;

                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                    }
                                    else
                                    {
                                        //  gui1 have not laguage dependent value then return default value
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }

                                }
                                if (GuiID2 > 0)
                                {

                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);

                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;

                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }


                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }

                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }

                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();

                                        string substrProductName2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        if (substrProductName2.Length > MaxLength)
                                        {
                                            obj.ProductNameVal2 = substrProductName2.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal2 = substrProductName2;
                                        }

                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }

                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 7
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>13in</PageWidth>" +
                            "  <PageHeight>9in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();
                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}



                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }

                    else
                    {
                        try
                        {
                            string PackageDate = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                            string[] arr = ProductName.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            LocalReport lr = new LocalReport();
                            string path = "";
                            if (chkPlaceOfOrigin == true)
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/NonFoodBarcodeLanguageContentMRPPlaceOfOrigin.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageContentMRPPlaceOfOrigin.rdlc");
                                //}



                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/NonFoodBarcodeLanguageContentMRPPlaceOfOrigin.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguageContentMRPPlaceOfOrigin.rdlc");
                                }
                            }
                            else
                            {
                                //if (Request.Url.Host.Contains("localhost"))
                                //{
                                //    path = "Report/NonFoodBarcodeLanguageContentMRP.rdlc";
                                //}
                                //else
                                //{
                                //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageContentMRP.rdlc");
                                //}


                                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                                {
                                    path = "Report/NonFoodBarcodeLanguageContentMRP.rdlc";
                                }
                                else
                                {
                                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/NonFoodBarcodeLanguageContentMRP.rdlc");
                                }
                            }
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    path = "Report/NonFoodBarcodeLanguageContentMRP.rdlc";
                            //}
                            //else
                            //{
                            //    path = Server.MapPath("~/Report/NonFoodBarcodeLanguageContentMRP.rdlc");
                            //}
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(Godown);
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= NoOfBlankBarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = PackageDate;
                                obj.chkPlaceOfOrigin = chkPlaceOfOrigin;
                                if (chkPlaceOfOrigin == true)
                                {
                                    obj.PlaceOfOrigin = PlaceOfOrigin;
                                }
                                else
                                {
                                    obj.PlaceOfOrigin = "";
                                }
                                //   obj.ProductName = arr[0];
                                const int MaxLength = 22;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = MRP;
                                obj.GramPerKG = GramPerKG;
                                obj.Batch = BlankBarcodeBatchNo.ToString();
                                // obj.Productbarcode = ProductBarcode;
                                obj.BarcodeImage = CreateEan13(ProductBarcode);
                                obj.GodownCode = godowndetails.GodownCode;

                                // 10 Jan, 2022 Piyush Limbani
                                if (GuiID1 == 3)
                                {
                                    if (obj.GodownCode == "A")
                                    {
                                        obj.GodownCode = "أ";
                                    }
                                    else if (obj.GodownCode == "B")
                                    {
                                        obj.GodownCode = "ب";
                                    }
                                    else
                                    {
                                        obj.GodownCode = "ج";
                                    }
                                }
                                // 10 Jan, 2022 Piyush Limbani

                                obj.QTY = qty;
                                obj.MonthDate = BestBeforeDate.ToString("dd/MM/yyyy");

                                if (NutritionValue == "")
                                {
                                    obj.NutritionValue = ".";
                                }
                                else
                                {
                                    obj.NutritionValue = NutritionValue;
                                }

                                if (ContentValue == "")
                                {
                                    obj.ContentValue = ".";
                                }
                                else
                                {
                                    obj.ContentValue = ContentValue;
                                }

                                //obj.NutritionValue = NutritionValue;
                                //obj.ContentValue = ContentValue;

                                //**************************************************************************************// 
                                string strNetWeight = "Net Weight";
                                string strDateOfPacking = "Date of packing";
                                string strBatchLabel = "Batch No";
                                string strProductName1 = obj.ProductName;
                                string strProductName2 = obj.ProductName;
                                string strContentVal1 = obj.ContentValue;
                                string strContentVal2 = obj.ContentValue;

                                string strNutritionVal1 = obj.NutritionValue;
                                string strNutritionVal2 = obj.NutritionValue;


                                //string strNutritionVal1 = obj.NutritionVal1;
                                //string strNutritionVal2 = obj.NutritionVal2;


                                string strProdQtyWithUnit1 = obj.QTY;
                                string strProdQtyWithUnit2 = obj.QTY;


                                if (GuiID1 > 0)
                                {
                                    string LanguageName1 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui1 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID1, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui1.Count() > 0)
                                    {
                                        List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);

                                        LanguageName1 = RetProdWithQtyGui1.FirstOrDefault().LanguageName;

                                        string UnitName = "";
                                        if (RetProdWithQtyGui1.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui1.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit1 = RetProdWithQtyGui1.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }

                                        if (LanguageName1.ToLower().Contains("arabic") == true)
                                        {
                                            string s1 = "\u202B" + strProdQtyWithUnit1;
                                            strProdQtyWithUnit1 = s1;
                                        }

                                        if (strProdQtyWithUnit1 == "")
                                        {
                                            strProdQtyWithUnit1 = obj.QTY;
                                        }

                                        var expandDynamicLabel1 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst.Count > 0)
                                        {
                                            foreach (var item in lst)
                                            {
                                                expandDynamicLabel1.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel1Obj = expandDynamicLabel1.ToList();
                                        LabelCodes clsLabel1Code = new LabelCodes();
                                        if (_lstLabel1Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel1Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel1Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel1Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }
                                        obj.BestBeforeLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BestBeforeLabel) == true ? clsLabel1Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel1 = !string.IsNullOrEmpty(clsLabel1Code.BatchNumberLabel) == true ? clsLabel1Code.BatchNumberLabel.Trim() : strBatchLabel;
                                        obj.NetWeightLabel1 = !string.IsNullOrEmpty(clsLabel1Code.NetWeightLabel) == true ? clsLabel1Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel1 = !string.IsNullOrEmpty(clsLabel1Code.DateOfPackingLabel) == true ? clsLabel1Code.DateOfPackingLabel.Trim() : strDateOfPacking;
                                        obj.ContentVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ContentGui.Trim() : strContentVal1;
                                        obj.NutritionVal1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui1.FirstOrDefault().NutritionGui.Trim() : strNutritionVal1;

                                        string substrProductName1 = !string.IsNullOrEmpty(RetProdWithQtyGui1.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui1.FirstOrDefault().ProductNameGui.Trim() : strProductName1;

                                        if (substrProductName1.Length > MaxLength)
                                        {
                                            obj.ProductNameVal1 = substrProductName1.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal1 = substrProductName1;
                                        }
                                        obj.ProductQtyWithUnitLabel1 = strProdQtyWithUnit1.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo1 = BatchNo1;
                                        obj.BatchNo1 = obj.GodownCode + BatchNo1;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal1 = DatePackaging1.Trim();
                                        obj.BestBeforeVal1 = BestBefore1.Trim();
                                    }
                                    else
                                    {
                                        obj.ProductNameVal1 = obj.ProductName;
                                        obj.NutritionVal1 = NewNutritionValue;
                                        obj.ContentVal1 = obj.ContentValue;
                                        obj.NetWeightLabel1 = "Net Weight";
                                        obj.ProductQtyWithUnitLabel1 = qty;
                                        obj.BatchNumberLabel1 = "Batch No";
                                        obj.BatchNo1 = BlankBarcodeBatchNo.ToString();
                                        obj.DateOfPackingLabel1 = "Date Of Packing";
                                        obj.DateOfPackingVal1 = BlankBarcodePackageDate.ToString("dd/MM/yyyy");
                                        obj.BestBeforeLabel1 = "Best Before";
                                        obj.BestBeforeVal1 = BestBeforeDate.ToString("dd/MM/yyyy");
                                    }
                                }
                                if (GuiID2 > 0)
                                {

                                    string LanguageName2 = "";
                                    List<RetProdWithQtyGuiList> RetProdWithQtyGui2 = _productservice.GetProductGuiByProductID(Convert.ToInt64(ProductID), GuiID2, Convert.ToInt64(ProductQtyID));
                                    if (RetProdWithQtyGui2.Count() > 0)
                                    {
                                        List<Resource> lst2 = _productservice.GetAllLabelByGuiID(GuiID2);

                                        LanguageName2 = RetProdWithQtyGui2.FirstOrDefault().LanguageName;
                                        string UnitName = "";
                                        if (RetProdWithQtyGui2.FirstOrDefault().UnitName != null)
                                        {
                                            UnitName = RetProdWithQtyGui2.FirstOrDefault().UnitName;
                                        }
                                        if (RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui != null)
                                        {
                                            strProdQtyWithUnit2 = RetProdWithQtyGui2.FirstOrDefault().ProductQtyGui.Trim() + " " + UnitName;
                                        }
                                        if (LanguageName2.ToLower().Contains("arabic") == true)
                                        {
                                            string s2 = "\u202B" + strProdQtyWithUnit2;
                                            strProdQtyWithUnit2 = s2;
                                        }
                                        if (strProdQtyWithUnit2 == "")
                                        {
                                            strProdQtyWithUnit2 = obj.QTY;
                                        }


                                        var expandDynamicLabel2 = new ExpandoObject() as IDictionary<string, Object>;
                                        if (lst2.Count() > 0)
                                        {
                                            foreach (var item in lst2)
                                            {
                                                expandDynamicLabel2.Add(item.ResourceKey, item.ResourceValue);
                                            }
                                        }
                                        List<KeyValuePair<string, object>> _lstLabel2Obj = expandDynamicLabel2.ToList();
                                        LabelCodes clsLabel2Code = new LabelCodes();
                                        var clsLabel2Type = typeof(LabelCodes);
                                        if (_lstLabel2Obj.Count() > 0)
                                        {
                                            foreach (KeyValuePair<string, object> _pair in _lstLabel2Obj)
                                            {
                                                PropertyInfo propertyInfo = clsLabel2Code.GetType().GetProperty(_pair.Key);
                                                propertyInfo.SetValue(clsLabel2Code, Convert.ChangeType(_pair.Value, propertyInfo.PropertyType), null);
                                            }
                                        }

                                        obj.BestBeforeLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BestBeforeLabel) == true ? clsLabel2Code.BestBeforeLabel.Trim() : obj.MonthDate;
                                        obj.BatchNumberLabel2 = !string.IsNullOrEmpty(clsLabel2Code.BatchNumberLabel) == true ? clsLabel2Code.BatchNumberLabel.Trim() : obj.MonthDate;
                                        obj.NetWeightLabel2 = !string.IsNullOrEmpty(clsLabel2Code.NetWeightLabel) == true ? clsLabel2Code.NetWeightLabel.Trim() : strNetWeight;
                                        obj.DateOfPackingLabel2 = !string.IsNullOrEmpty(clsLabel2Code.DateOfPackingLabel) == true ? clsLabel2Code.DateOfPackingLabel.Trim() : strDateOfPacking;

                                        obj.ContentVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ContentGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ContentGui.Trim() : strContentVal2.Trim();
                                        obj.NutritionVal2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().NutritionGui) == true ? RetProdWithQtyGui2.FirstOrDefault().NutritionGui.Trim() : strNutritionVal2.Trim();

                                        obj.ProductQtyWithUnitLabel2 = strProdQtyWithUnit2.Trim();

                                        // 10 Jan, 2022 Piyush Limbani
                                        // obj.BatchNo2 = BatchNo2;
                                        obj.BatchNo2 = godowndetails.GodownCode + BatchNo2;
                                        // 10 Jan, 2022 Piyush Limbani

                                        obj.DateOfPackingVal2 = DatePackaging2.Trim();
                                        obj.BestBeforeVal2 = BestBefore2.Trim();


                                        string substrProductName2 = !string.IsNullOrEmpty(RetProdWithQtyGui2.FirstOrDefault().ProductNameGui) == true ? RetProdWithQtyGui2.FirstOrDefault().ProductNameGui.Trim() : strProductName2;
                                        if (substrProductName2.Length > MaxLength)
                                        {
                                            obj.ProductNameVal2 = substrProductName2.Substring(0, MaxLength);
                                        }
                                        else
                                        {
                                            obj.ProductNameVal2 = substrProductName2;
                                        }


                                        if (obj.ProductNameVal2 == null || obj.ProductNameVal2 == "")
                                        {
                                            obj.ProductNameVal2 = obj.ProductName;
                                        }
                                        if (obj.ProductQtyWithUnitLabel2 == null || obj.ProductQtyWithUnitLabel2 == "")
                                        {
                                            obj.ProductQtyWithUnitLabel2 = obj.QTY;
                                        }
                                        if (obj.BatchNumberLabel2 == null || obj.BatchNumberLabel2 == "")
                                        {
                                            obj.BatchNumberLabel2 = "Batch No";
                                        }
                                        if (obj.NetWeightLabel2 == null || obj.NetWeightLabel2 == "")
                                        {
                                            obj.NetWeightLabel2 = "Net Weight";
                                        }
                                        if (obj.BestBeforeLabel2 == null || obj.BestBeforeLabel2 == "")
                                        {
                                            obj.BestBeforeLabel2 = "Best Before";
                                        }
                                        if (obj.DateOfPackingLabel2 == null || obj.DateOfPackingLabel2 == "")
                                        {
                                            obj.DateOfPackingLabel2 = "Date Of Packing";
                                        }
                                    }
                                }
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020 - 8
                            long respose = _productservice.AddBarcodeQuantityDetails(ProductID, Convert.ToInt64(ProductQtyID), ProductName, BlankBarcodePackageDate, Convert.ToInt64(NoOfBlankBarcodes), Godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource MedsheetHeader = new ReportDataSource("DataReport", header);
                            lr.DataSources.Add(MedsheetHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =


                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                            "  <PageWidth>13in</PageWidth>" +
                            "  <PageHeight>9in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
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

                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            //string url = "";
                            //if (Request.Url.Host.Contains("localhost"))
                            //{
                            //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            //}
                            //else
                            //{
                            //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                            //}


                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
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


        }

        [HttpPost]
        public ActionResult AddProductGuiMaster(RetProdGuiViewModel data)
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
                    data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.CreatedOn = DateTime.Now;
                    data.ModifiedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.ModifiedOn = DateTime.Now;
                    bool respose = _productservice.AddProductGuiMaster(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAllRetUnitNameByGuiID(long GuiID)
        {
            ViewBag.UnitGui = _productservice.GetAllRetUnitNameByGuiID(GuiID);
            return Json(new { UnitGui = ViewBag.UnitGui }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetRetailProductQtyGuiByID(long ProductID, long GuiID)
        {
            var data = _productservice.GetRetailProductGuiByID(ProductID, GuiID);
            if (data != null)
            {
                if (data.RetProdGuiID > 0)
                {
                    long RetProdGuiID = data.RetProdGuiID;
                    data.lstRetProdQtyGui = _productservice.GetRetailProductQtyGuiByID(RetProdGuiID);
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Manage Language
        public ActionResult ManageLanguage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddLanguage(GuiLanguageViewModel data)
        {
            bool response = false;
            data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
            if (data.GuiID == 0)
            {
                data.CreatedOn = DateTime.Now;
            }
            data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
            data.UpdatedOn = DateTime.Now;

            if (data.LanguageName != null)
            {
                response = _productservice.AddLanguage(data);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAllLanguageList()
        {
            List<GuiLanguageViewModel> ListOfLanguage = _productservice.GetAllLanguageList(0);
            return PartialView(ListOfLanguage);
        }

        public JsonResult EditLanguage(int GuiID)
        {
            GuiLanguageViewModel LanguageData = _productservice.GetAllLanguageList(GuiID).FirstOrDefault();
            return Json(LanguageData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteLanguage(long? GuiID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteLanguage(GuiID, IsDelete);
                return RedirectToAction("ManageLanguage");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageLanguage");
            }
        }
        #endregion

        #region Manage GuiLabel
        public ActionResult ManageGuiLabel()
        {
            ViewBag.GuiLanguage = _productservice.GetAllLanguage();
            return View();
        }

        [HttpPost]
        public JsonResult AddLabel(GuiLabelViewModel data)
        {
            bool response = false;

            data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
            if (data.GuiLabelID == 0)
            {
                data.CreatedOn = DateTime.Now;
            }
            data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
            data.UpdatedOn = DateTime.Now;

            if (data.LabelCode != null && data.LabelValue != null)
            {
                response = _productservice.AddLabel(data);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAllLabelList()
        {
            List<GuiLabelViewModel> ListOfLabel = _productservice.GetAllLabelList(0);
            return PartialView(ListOfLabel);
        }

        public JsonResult EditLabel(long GuiLabelID)
        {
            GuiLabelViewModel LabelData = _productservice.GetAllLabelList(GuiLabelID).FirstOrDefault();
            return Json(LabelData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteLabel(long? GuiLabelID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteLabel(GuiLabelID, IsDelete);
                return RedirectToAction("ManageGuiLabel");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageGuiLabel");
            }
        }

        public Resource GetResourceValue(long GuiID1)
        {
            Resource objResource = new Resource();
            List<Resource> lst = _productservice.GetAllLabelByGuiID(GuiID1);

            foreach (Resource lstResource in lst)
            {
                PropertyInfo info = objResource.labelCodes.GetType().GetProperties().FirstOrDefault(o => o.Name.ToLower() == lstResource.ResourceKey.ToLower().ToString());
                if (info != null)
                {
                    objResource.ResourceKey = objResource.ResourceValue;
                    // info.SetValue(objResource.labelCodes, objResource.ResourceValue);
                }
            }
            return objResource;
        }
        #endregion

        // 20-03-2019
        public JsonResult GetProductExpiryDate(DateTime WholesaleBarcodePackageDate, long ProductIDWholesalebarcode)
        {
            try
            {
                BestBeforeMonth promonth = new BestBeforeMonth();
                promonth = _productservice.GetMonthByProductID(Convert.ToInt64(ProductIDWholesalebarcode));
                long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                DateTime theDate = WholesaleBarcodePackageDate;
                DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                string MonthDat = yearInTheFuture.ToString("MM/dd/yyyy");
                return Json(MonthDat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintWholesaleBarcode(DateTime WholesaleBarcodePackageDate, DateTime WholesaleBestBeforeDate, string ProductIDWholesalebarcode, string WholesaleProductName, decimal WholesaleProductMRP, int TotalPacket, decimal ProductQuantity, string UnitName, string WholesaleProductBarcode, long WholesaleCategoryID, string WholesaleBarcodeBatchNo, long WholesaleGodownID, int NoofWholesaleBarcodes, string WholesaleGramPerKG)
        {
            if (NoofWholesaleBarcodes == 0)
            {
                try
                {
                    string PackageDate = WholesaleBarcodePackageDate.ToString("dd/MM/yyyy");
                    string[] arr = WholesaleProductName.Split('(');
                    string qty = "";
                    if (arr.Length == 2)
                    {
                        qty = arr[1].Remove(arr[1].Length - 1);
                    }
                    else
                    {
                        qty = arr[2].Remove(arr[2].Length - 1);
                    }
                    LocalReport lr = new LocalReport();
                    string path = "";
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/WholesaleBarcode.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/WholesaleBarcode.rdlc");
                    //}


                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/WholesaleBarcode.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/WholesaleBarcode.rdlc");
                    }

                    lr.ReportPath = path;
                    Godown_Mst godowndetails = new Godown_Mst();
                    godowndetails = _productservice.GetGodownDetailsByGodownID(WholesaleGodownID);
                    List<ProductBarcode> LabelData = new List<ProductBarcode>();
                    //for (int f = 1; f <= NoOfBlankBarcodes; f++)
                    //{
                    ProductBarcode obj = new ProductBarcode();
                    obj.AddressLine1 = godowndetails.GodownAddress1;
                    obj.AddressLine2 = godowndetails.GodownAddress2;
                    obj.FSSAINo = godowndetails.GodownFSSAINumber;
                    obj.PhoneNo = godowndetails.GodownPhone;
                    obj.GodownName = godowndetails.GodownName;
                    obj.DatePackaging = PackageDate;
                    const int MaxLength = 22;
                    if (arr.Length == 2)
                    {
                        obj.ProductName = arr[0];
                        if (obj.ProductName.Length > MaxLength)
                        {
                            obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                        }
                    }
                    else
                    {
                        obj.ProductName = arr[0] + " (" + arr[1];
                        if (obj.ProductName.Length > MaxLength)
                        {
                            obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                        }
                    }
                    obj.TotalPacket = TotalPacket.ToString();
                    decimal TotalWeight = 0;
                    if (UnitName == "g")
                    {
                        TotalWeight = (ProductQuantity * TotalPacket) / 1000;
                    }
                    else
                    {
                        TotalWeight = ProductQuantity * TotalPacket;
                    }
                    obj.TotalWeight = TotalWeight.ToString();
                    obj.TotalWeight = obj.TotalWeight + "kg";
                    // string Weight = ProductQuantity + " " + UnitName;
                    obj.Weight = qty;
                    obj.WeightPieces = "(" + obj.Weight + " X " + obj.TotalPacket + " " + "pieces" + ")";
                    decimal TotalPackMRP = WholesaleProductMRP * TotalPacket;
                    obj.MRP = WholesaleProductMRP.ToString();
                    obj.MRPPieces = "(" + "Rs." + obj.MRP + " X " + obj.TotalPacket + " " + "pieces" + ")";
                    obj.TotalPackMRP = TotalPackMRP.ToString();

                    obj.GramPerKG = WholesaleGramPerKG;
                    // obj.Batch = WholesaleBarcodeBatchNo.ToString();
                    obj.Batch = godowndetails.GodownCode + "" + WholesaleBarcodeBatchNo.ToString();

                    // obj.BarcodeImage = CreateEan13(WholesaleProductBarcode);
                    obj.BarcodeNo = WholesaleProductBarcode;
                    obj.GodownCode = godowndetails.GodownCode;
                    obj.MonthDate = WholesaleBestBeforeDate.ToString("dd/MM/yyyy");
                    LabelData.Add(obj);
                    //}
                    DataTable header = Common.ToDataTable(LabelData);
                    ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                    lr.DataSources.Add(MedsheetHeader);
                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =

                   //         "<DeviceInfo>" +
                        //    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        //"  <PageWidth>1.78in</PageWidth>" +
                        //    "  <PageHeight>4.3in</PageHeight>" +
                        //    "  <MarginTop>0.5cm</MarginTop>" +
                        //    "  <MarginLeft>0.5cm</MarginLeft>" +
                        //    "  <MarginRight>0.3cm</MarginRight>" +
                        //    "  <MarginBottom>0.5cm</MarginBottom>" +
                        //    "</DeviceInfo>";

                    "<DeviceInfo>" +
                      "  <OutputFormat>" + reportType + "</OutputFormat>" +
                  "  <PageWidth>1.90in</PageWidth>" +
                      "  <PageHeight>4.3in</PageHeight>" +
                      "  <MarginTop>0.5cm</MarginTop>" +
                      "  <MarginLeft>0.5cm</MarginLeft>" +
                      "  <MarginRight>0.3cm</MarginRight>" +
                      "  <MarginBottom>0.5cm</MarginBottom>" +
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

                    string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                    string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                    BinaryWriter Writer = null;
                    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                    Writer.Write(renderedBytes);
                    Writer.Flush();
                    Writer.Close();

                    //string url = "";
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                    //}
                    //else
                    //{
                    //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                    //}



                    string url = "";
                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                    {
                        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                    }
                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                    {
                        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                    }
                    else
                    {
                        url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                    }
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                try
                {
                    string PackageDate = WholesaleBarcodePackageDate.ToString("dd/MM/yyyy");
                    string[] arr = WholesaleProductName.Split('(');
                    string qty = "";
                    if (arr.Length == 2)
                    {
                        qty = arr[1].Remove(arr[1].Length - 1);
                    }
                    else
                    {
                        qty = arr[2].Remove(arr[2].Length - 1);
                    }
                    LocalReport lr = new LocalReport();
                    string path = "";
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/MultipleWholesaleBarcode.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/MultipleWholesaleBarcode.rdlc");
                    //}



                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/MultipleWholesaleBarcode.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/MultipleWholesaleBarcode.rdlc");
                    }
                    lr.ReportPath = path;
                    Godown_Mst godowndetails = new Godown_Mst();
                    godowndetails = _productservice.GetGodownDetailsByGodownID(WholesaleGodownID);
                    List<ProductBarcode> LabelData = new List<ProductBarcode>();
                    for (int f = 1; f <= NoofWholesaleBarcodes; f++)
                    {
                        ProductBarcode obj = new ProductBarcode();
                        obj.AddressLine1 = godowndetails.GodownAddress1;
                        obj.AddressLine2 = godowndetails.GodownAddress2;
                        obj.FSSAINo = godowndetails.GodownFSSAINumber;
                        obj.PhoneNo = godowndetails.GodownPhone;
                        obj.GodownName = godowndetails.GodownName;
                        obj.DatePackaging = PackageDate;
                        const int MaxLength = 22;
                        if (arr.Length == 2)
                        {
                            obj.ProductName = arr[0];
                            if (obj.ProductName.Length > MaxLength)
                            {
                                obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                            }
                        }
                        else
                        {
                            obj.ProductName = arr[0] + " (" + arr[1];
                            if (obj.ProductName.Length > MaxLength)
                            {
                                obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                            }
                        }
                        obj.TotalPacket = TotalPacket.ToString();
                        decimal TotalWeight = 0;
                        if (UnitName == "g")
                        {
                            TotalWeight = (ProductQuantity * TotalPacket) / 1000;
                        }
                        else
                        {
                            TotalWeight = ProductQuantity * TotalPacket;
                        }
                        obj.TotalWeight = TotalWeight.ToString();
                        obj.TotalWeight = obj.TotalWeight + "kg";
                        // string Weight = ProductQuantity + " " + UnitName;
                        obj.Weight = qty;
                        obj.WeightPieces = "(" + obj.Weight + " X " + obj.TotalPacket + " " + "pieces" + ")";
                        decimal TotalPackMRP = WholesaleProductMRP * TotalPacket;
                        obj.MRP = WholesaleProductMRP.ToString();
                        obj.MRPPieces = "(" + "Rs." + obj.MRP + " X " + obj.TotalPacket + " " + "pieces" + ")";
                        obj.TotalPackMRP = TotalPackMRP.ToString();

                        obj.GramPerKG = WholesaleGramPerKG;

                        //obj.Batch = WholesaleBarcodeBatchNo.ToString();
                        obj.Batch = godowndetails.GodownCode + "" + WholesaleBarcodeBatchNo.ToString();

                        // obj.BarcodeImage = CreateEan13(WholesaleProductBarcode);
                        obj.BarcodeNo = WholesaleProductBarcode;
                        obj.GodownCode = godowndetails.GodownCode;
                        obj.MonthDate = WholesaleBestBeforeDate.ToString("dd/MM/yyyy");
                        LabelData.Add(obj);
                    }
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
                    "  <PageWidth>13in</PageWidth>" +
                        "  <PageHeight>9in</PageHeight>" +
                        "  <MarginTop>0.5cm</MarginTop>" +
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

                    string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                    string Pdfpathcreate = Server.MapPath("~/BlankBarcode/" + name1);

                    BinaryWriter Writer = null;
                    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                    Writer.Write(renderedBytes);
                    Writer.Flush();
                    Writer.Close();

                    //string url = "";
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    url = "http://" + Request.Url.Host + ":6551/BlankBarcode/" + name1;
                    //}
                    //else
                    //{
                    //    url = "http://" + Request.Url.Host + "/BlankBarcode/" + name1;
                    //}


                    string url = "";
                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                    {
                        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/BlankBarcode/" + name1;
                    }
                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                    {
                        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                    }
                    else
                    {
                        url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/BlankBarcode/" + name1;
                    }
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }




        // 08 Oct 2020 Piyush Limbani
        public ActionResult ManagePouchName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPouchName(PouchNameModel data)
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
                    PouchName_Mst obj = new PouchName_Mst();
                    obj.PouchNameID = data.PouchNameID;
                    obj.PouchName = data.PouchName;
                    obj.HSNNumber = data.HSNNumber;

                    obj.FontSize = data.FontSize;
                    obj.DelayTime = data.DelayTime;
                    obj.PouchSize = data.PouchSize;

                    if (obj.PouchNameID == 0)
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
                    bool respose = _areaservice.AddPouchName(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PouchNameList()
        {
            List<PouchNameListResponse> objModel = _areaservice.GetAllPouchNameList();
            return PartialView(objModel);
        }

        public ActionResult DeletePouchName(long? PouchNameID, bool IsDelete)
        {
            try
            {
                _areaservice.DeletePouchName(PouchNameID.Value, IsDelete);
                return RedirectToAction("ManagePouchName");
            }
            catch (Exception)
            {
                return RedirectToAction("ManagePouchName");
            }
        }


        //31 May,2021 Sonal Gandhi

        public ActionResult ManageCurrency()
        {
            return View();
        }

        public PartialViewResult GetAllCurrencyList()
        {
            List<CurrencyViewModel> ListOfCurrency = _productservice.GetAllCurrencyList(0);
            return PartialView(ListOfCurrency);
        }

        [HttpPost]
        public JsonResult AddCurrency(CurrencyViewModel data)
        {
            bool response = false;
            data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
            if (data.CurrencyID == 0)
            {
                data.CreatedOn = DateTime.Now;
            }
            data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
            data.UpdatedOn = DateTime.Now;

            if (data.CurrencyID != null)
            {
                response = _productservice.AddCurrency(data);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditCurrency(int CurrencyID)
        {
            CurrencyViewModel CurrencyData = _productservice.GetAllCurrencyList(CurrencyID).FirstOrDefault();
            return Json(CurrencyData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCurrency(long? CurrencyID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteCurrency(CurrencyID, IsDelete);
                return RedirectToAction("ManageCurrency");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageCurrency");
            }
        }

        [HttpPost]
        public ActionResult PrintPouchBarcode(DateTime date1, string id, string name, string mrp, string Batchno, int noofbarcodes, long godown, string Productbarcode, string NutritionValue, string ContentValue, long CategoryID, long ProductQtyID, long PouchSize, string PouchGramPerKG)
        {

            if (PouchSize != 0)
            {
                //if (CategoryID != 9)
                if (PouchSize == 220 || PouchSize == 260 || PouchSize == 290 || PouchSize == 320 || PouchSize == 340)
                {
                    if (NutritionValue == "" && ContentValue == "")
                    {
                        try
                        {
                            string dat = date1.ToString("dd/MM/yyyy");
                            string[] arr = name.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            string MonthDat = "";
                            LocalReport lr = new LocalReport();
                            string path = "";

                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/PouchBarcode.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PouchBarcode.rdlc");
                            }
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                            BestBeforeMonth promonth = new BestBeforeMonth();
                            promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                            long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                            long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                            DateTime theDate = DateTime.Now;
                            DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                            MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= noofbarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = dat;
                                //const int MaxLength = 22;
                                const int MaxLength = 40;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = mrp;
                                obj.GramPerKG = PouchGramPerKG;
                                // obj.Batch = Batchno.ToString();
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                obj.BarcodeImage = CreateEan13(Productbarcode);
                                obj.GodownCode = godowndetails.GodownCode;
                                obj.QTY = qty;
                                obj.MonthDate = MonthDat;
                                LabelData.Add(obj);
                            }

                            // Add Barcode QTY Details 30-03-2020
                            long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource BarcodeHeader = new ReportDataSource("data", header);
                            lr.DataSources.Add(BarcodeHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>1.70in</PageWidth>" +
                            "  <PageHeight>2.55in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
                            "  <MarginLeft>0.5cm</MarginLeft>" +
                            "  <MarginRight>0.5cm</MarginRight>" +
                            "  <MarginBottom>0.5cm</MarginBottom>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        try
                        {
                            string dat = date1.ToString("dd/MM/yyyy");
                            string[] arr = name.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }

                            string MonthDat = "";
                            LocalReport lr = new LocalReport();
                            string path = "";

                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/PouchContentBarcode.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PouchContentBarcode.rdlc");
                            }
                            lr.ReportPath = path;

                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                            BestBeforeMonth promonth = new BestBeforeMonth();
                            promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                            long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                            long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                            DateTime theDate = DateTime.Now;
                            DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                            MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= noofbarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = dat;
                                // const int MaxLength = 22;
                                const int MaxLength = 40;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = mrp;
                                obj.GramPerKG = PouchGramPerKG;
                                // obj.Batch = Batchno.ToString();
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                obj.BarcodeImage = CreateEan13(Productbarcode);
                                obj.GodownCode = godowndetails.GodownCode;
                                obj.QTY = qty;
                                obj.MonthDate = MonthDat;
                                obj.NutritionValue = NutritionValue;
                                obj.ContentValue = ContentValue;
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020
                            long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource BarcodeHeader = new ReportDataSource("data", header);
                            lr.DataSources.Add(BarcodeHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                 "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>1.75in</PageWidth>" +
                            "  <PageHeight>3in</PageHeight>" +
                            "  <MarginTop>0.3cm</MarginTop>" +
                            "  <MarginLeft>0.5cm</MarginLeft>" +
                            "  <MarginRight>0.5cm</MarginRight>" +
                            "  <MarginBottom>0.3cm</MarginBottom>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }



                else
                {


                    if (NutritionValue == "" && ContentValue == "")
                    {
                        try
                        {
                            string dat = date1.ToString("dd/MM/yyyy");
                            string[] arr = name.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }
                            string MonthDat = "";
                            LocalReport lr = new LocalReport();
                            string path = "";

                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/PouchBarcode_390.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PouchBarcode_390.rdlc");
                            }
                            lr.ReportPath = path;
                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                            BestBeforeMonth promonth = new BestBeforeMonth();
                            promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                            long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                            long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                            DateTime theDate = DateTime.Now;
                            DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                            MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= noofbarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = dat;
                                // const int MaxLength = 22;
                                const int MaxLength = 40;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = mrp;
                                obj.GramPerKG = PouchGramPerKG;
                                //  obj.Batch = Batchno.ToString();
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                obj.BarcodeImage = CreateEan13(Productbarcode);
                                obj.GodownCode = godowndetails.GodownCode;
                                obj.QTY = qty;
                                obj.MonthDate = MonthDat;
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020
                            long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource BarcodeHeader = new ReportDataSource("data", header);
                            lr.DataSources.Add(BarcodeHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                  "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                                //"  <PageWidth>2.20in</PageWidth>" +
                        "  <PageWidth>2.7in</PageWidth>" +
                            "  <PageHeight>3.30in</PageHeight>" +
                            "  <MarginTop>0.5cm</MarginTop>" +
                            "  <MarginLeft>0.5cm</MarginLeft>" +
                            "  <MarginRight>0.5cm</MarginRight>" +
                            "  <MarginBottom>0.5cm</MarginBottom>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            return Json(url, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        try
                        {
                            string dat = date1.ToString("dd/MM/yyyy");
                            string[] arr = name.Split('(');
                            string qty = "";
                            if (arr.Length == 2)
                            {
                                qty = arr[1].Remove(arr[1].Length - 1);
                            }
                            else
                            {
                                qty = arr[2].Remove(arr[2].Length - 1);
                            }

                            string MonthDat = "";
                            LocalReport lr = new LocalReport();
                            string path = "";

                            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                            {
                                path = "Report/PouchContentBarcode_390.rdlc";
                            }
                            else
                            {
                                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PouchContentBarcode_390.rdlc");
                            }
                            lr.ReportPath = path;

                            Godown_Mst godowndetails = new Godown_Mst();
                            godowndetails = _productservice.GetGodownDetailsByGodownID(godown);
                            BestBeforeMonth promonth = new BestBeforeMonth();
                            promonth = _productservice.GetMonthByProductID(Convert.ToInt64(id));
                            long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                            long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                            DateTime theDate = DateTime.Now;
                            DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                            MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                            List<ProductBarcode> LabelData = new List<ProductBarcode>();
                            for (int f = 1; f <= noofbarcodes; f++)
                            {
                                ProductBarcode obj = new ProductBarcode();
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.DatePackaging = dat;
                                //  const int MaxLength = 22;
                                const int MaxLength = 40;
                                if (arr.Length == 2)
                                {
                                    obj.ProductName = arr[0];

                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                else
                                {
                                    obj.ProductName = arr[0] + " (" + arr[1];
                                    if (obj.ProductName.Length > MaxLength)
                                    {
                                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                    }
                                }
                                obj.MRP = mrp;
                                obj.GramPerKG = PouchGramPerKG;
                                //  obj.Batch = Batchno.ToString();
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                obj.BarcodeImage = CreateEan13(Productbarcode);
                                obj.GodownCode = godowndetails.GodownCode;
                                obj.QTY = qty;
                                obj.MonthDate = MonthDat;
                                obj.NutritionValue = NutritionValue;
                                obj.ContentValue = ContentValue;
                                LabelData.Add(obj);
                            }
                            // Add Barcode QTY Details 30-03-2020
                            long respose = _productservice.AddBarcodeQuantityDetails(id, ProductQtyID, name, date1, noofbarcodes, godown, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now, false);
                            // Add Barcode QTY Details 30-03-2020
                            DataTable header = Common.ToDataTable(LabelData);
                            ReportDataSource BarcodeHeader = new ReportDataSource("data", header);
                            lr.DataSources.Add(BarcodeHeader);
                            string reportType = "pdf";
                            string mimeType;
                            string encoding;
                            string fileNameExtension;
                            string deviceInfo =

                                  "<DeviceInfo>" +
                            "  <OutputFormat>" + reportType + "</OutputFormat>" +
                                //"  <PageWidth>2.20in</PageWidth>" +
                         "  <PageWidth>2.7in</PageWidth>" +
                            "  <PageHeight>5.5in</PageHeight>" +
                            "  <MarginTop>0.3cm</MarginTop>" +
                            "  <MarginLeft>0.5cm</MarginLeft>" +
                            "  <MarginRight>0.5cm</MarginRight>" +
                            "  <MarginBottom>0.3cm</MarginBottom>" +
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
                            string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                            string Pdfpathcreate = Server.MapPath("~/Barcode/" + name1);
                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                            Writer.Write(renderedBytes);
                            Writer.Flush();
                            Writer.Close();

                            string url = "";
                            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Barcode/" + name1;
                            }
                            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                            {
                                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
                            }
                            else
                            {
                                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Barcode/" + name1;
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
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }


        // 04 Feb, 2022 Piyush Limbani
        public ActionResult ExportExcelDataForBarcode(DateTime PackageDate, long ProductID, string ProductName, string ProductMRP, string BatchNo, int NoofBarcodes, long GodownID, string Barcode, string Ingredients, string NutritionalFacts, long CategoryID, long ProductQtyID, long PouchSize, string Protein, string Fat, string Carbohydrate, string TotalEnergy, string Information, string PouchName, string GramPerKG)
        {
            string dat = PackageDate.ToString("dd/MM/yyyy");
            string[] arr = ProductName.Split('(');
            string qty = "";
            if (arr.Length == 2)
            {
                qty = arr[1].Remove(arr[1].Length - 1);
            }
            else
            {
                qty = arr[2].Remove(arr[2].Length - 1);
            }
            string MonthDat = "";
            Godown_Mst godowndetails = new Godown_Mst();
            godowndetails = _productservice.GetGodownDetailsByGodownID(GodownID);
            BestBeforeMonth promonth = new BestBeforeMonth();
            promonth = _productservice.GetMonthByProductID(Convert.ToInt64(ProductID));
            long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
            long month = Convert.ToInt64(promonth.MonthNumber) % 12;
            DateTime theDate = DateTime.Now;
            DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
            MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
            List<ProductBarcodeData> ListData = new List<ProductBarcodeData>();
            ProductBarcodeData obj = new ProductBarcodeData();
            if (PouchSize != 0)
            {
                obj.PouchSize = PouchSize;
                string[] pouchName = PouchName.Split('-');

                obj.PouchSizestr = pouchName[0];
                obj.NoofBarcodes = NoofBarcodes;
                obj.SKU = Barcode;
                const int MaxLength = 40;
                if (arr.Length == 2)
                {
                    obj.ProductName = arr[0];
                    if (obj.ProductName.Length > MaxLength)
                    {
                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                    }
                }
                else
                {
                    obj.ProductName = arr[0] + " (" + arr[1];
                    if (obj.ProductName.Length > MaxLength)
                    {
                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                    }
                }
                obj.NetWeight = qty;
                obj.BatchNo = godowndetails.GodownCode + "" + BatchNo.ToString();
                obj.DateOfPacking = dat;
                obj.MRP = ProductMRP;
                obj.PGM = GramPerKG;
                obj.BestBefore = MonthDat;
                obj.Ingredients = Ingredients;
                obj.Protein = Protein;
                obj.Fat = Fat;
                obj.Carbohydrate = Carbohydrate;
                obj.TotalEnergy = TotalEnergy;
                obj.Information = Information;
               
                //  obj.NutritionalFacts = NutritionalFacts;
                ListData.Add(obj);
            }
            List<ProductBarcodeDataExp> lstproduct = ListData.Select(x => new ProductBarcodeDataExp()
            {
                PouchSize = x.PouchSizestr,
                NoofBarcodes = x.NoofBarcodes,
                SKU = x.SKU,
                ProductName = x.ProductName,
                NetWeight = x.NetWeight,
                BatchNo = x.BatchNo,
                DateOfPacking = x.DateOfPacking,
                MRP = x.MRP,
                PGM = x.PGM,
                BestBefore = x.BestBefore,
                Ingredients = x.Ingredients,
                Protein = x.Protein,
                Fat = x.Fat,
                Carbohydrate = x.Carbohydrate,
                TotalEnergy = x.TotalEnergy,
                Information = x.Information,
                
                // NutritionalFacts = x.NutritionalFacts
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable1(lstproduct));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename= " + "BarcodeDetail.xls");
                Response.AddHeader("content-disposition", "attachment;filename= " + "BarcodeDetail.xlsx");
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