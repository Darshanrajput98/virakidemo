using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.ViewModel;
using vb.Service;

namespace vbwebsite.Areas.export.Controllers
{
    public class OrderController : Controller
    {
        private IRetOrderService _orderservice;
        private ICommonService _commonservice;
        private IRetProductService _productservice;

        public long OrderIdvalue;
        List<RetOrderQtyInvoiceList> Lstdata;
        List<RetOrderQtyInvoiceList> Lstdatainvoice;

        public OrderController(IRetOrderService orderservice, ICommonService commonservice, IRetProductService productservice)
        {
            _orderservice = orderservice;
            _commonservice = commonservice;
            _productservice = productservice;
        }

        //
        // GET: /export/Order/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageOrder(Int64? id)
        {
            ViewBag.Product = _orderservice.GetAllRetProductName();
            try
            {
                ExportOrderViewModel objModel = _orderservice.GetExportOrderDetailsByOrderID(Convert.ToInt64(id));
                if (objModel.DeActiveCustomer == true)
                {
                    ViewBag.Customer = _commonservice.GetActiveRetCustomerName(objModel.CustomerID);
                }
                else
                {
                    ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
                }
                return View(objModel);
            }
            catch
            {
                ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
                return View();
            }
        }

        public JsonResult txtCustomerName_TextChanged(string obj)
        {
            int quntity = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["drugQuntityshow"].ToString());
            List<RetCustomerName1> data = _orderservice.GetTaxForCustomerByTextChange(obj).Select(x => new RetCustomerName1() { CustomerName = x }).Take(quntity).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDetailForExportOrder(string obj)
        {
            try
            {
                string[] cityarr = obj.Split(',');
                string ID = string.Empty;
                if (cityarr.Length > 1)
                {
                    var Tax = _orderservice.GetCustomerDetailForExportOrder(Convert.ToInt64(cityarr[0]));
                    return Json(Tax, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetExportProductDetails(long ProductQtyID)
        {
            var SellPrice = _orderservice.GetExportProductDetails(ProductQtyID);
            return Json(SellPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddExportOrder(ExportOrderViewModel data)
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
                    if (data.OrderID == 0)
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
                    long respose = _orderservice.AddExportOrder(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchViewBillWiseOrderList()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseOrderList(string OrderDate)
        {
            if (string.IsNullOrEmpty(OrderDate))
            {
                OrderDate = DateTime.Now.ToString();
            }
            List<ExportOrderListResponse> objModel = _orderservice.GetExportOrderListByOrderDate(Convert.ToDateTime(OrderDate));
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult PrintInvoice(long OrderID, string InvoiceNumber)
        {
            OrderIdvalue = OrderID;
            LocalReport lr = new LocalReport();
            string path = "";
            List<ExportOrderQtyInvoiceList> orderdata = _orderservice.GetExportInvoiceOrderDetailForPrint(OrderID);
            List<ExportOrderQtyInvoiceList> Lstdata = _orderservice.GetInvoiceForExportOrderItemPrint(OrderID, InvoiceNumber);
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    path = "Report/PrintExportInvoice.rdlc";
            //}
            //else
            //{
            //    path = Server.MapPath("~/Report/PrintExportInvoice.rdlc");
            //}



            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                path = "Report/PrintExportInvoice.rdlc";
            }
            else
            {
                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintExportInvoice.rdlc");
            }

            lr.ReportPath = path;
            DataTable header = Common.ToDataTable(orderdata);
            DataTable FoodDT = Common.ToDataTable(Lstdata);
            ReportDataSource InvoiceHeader = new ReportDataSource("DataSet1", header);
            ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
            lr.DataSources.Add(InvoiceHeader);
            lr.DataSources.Add(DataRecord);
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

            //    "<DeviceInfo>" +
            //"  <OutputFormat>" + reportType + "</OutputFormat>" +
            //"  <PageWidth>8.5in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>1cm</MarginTop>" +
            //"  <MarginLeft>0.34cm</MarginLeft>" +
            //"  <MarginRight>0.1cm</MarginRight>" +
            //"  <MarginBottom>1cm</MarginBottom>" +
            //"</DeviceInfo>";

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
            string Pdfpathcreate = Server.MapPath("~/bill/" + name);
            BinaryWriter Writer = null;
            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
            Writer.Write(renderedBytes);
            Writer.Flush();
            Writer.Close();


            //string url = "";
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
            //}
            //else
            //{
            //    url = "http://" + Request.Url.Host + "/bill/" + name;
            //}


            string url = "";
            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/bill/" + name;
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            }
            else
            {
                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            }


            //string url = "";
            //if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
            //{
            //    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/bill/" + name;

            //}
            //else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
            //{              
            //    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
                

            //}
            //else
            //{
            //    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            //}


            return Json(url, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PrintInvoiceforRupees(long OrderID, string InvoiceNumber, decimal Rupees)
        {
            OrderIdvalue = OrderID;
            LocalReport lr = new LocalReport();
            string path = "";
            if (Rupees != null && Rupees > 0)
            {
                bool response = _orderservice.UpdateExportDollarPrice(OrderID, InvoiceNumber, Rupees);
            }
            List<ExportOrderQtyInvoiceList> orderdata = _orderservice.GetExportInvoiceOrderDetailForPrintRupees(OrderID, Rupees);
            List<ExportOrderQtyInvoiceList> Lstdata = _orderservice.GetInvoiceForExportOrderItemPrintRupees(OrderID, InvoiceNumber, Rupees);
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    path = "Report/PrintExportInvoiceRupees.rdlc";
            //}
            //else
            //{
            //    path = Server.MapPath("~/Report/PrintExportInvoiceRupees.rdlc");
            //}


            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                path = "Report/PrintExportInvoiceRupees.rdlc";
            }
            else
            {
                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintExportInvoiceRupees.rdlc");
            }

            lr.ReportPath = path;
            DataTable header = Common.ToDataTable(orderdata);
            DataTable FoodDT = Common.ToDataTable(Lstdata);
            ReportDataSource InvoiceHeader = new ReportDataSource("DataSet1", header);
            ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
            lr.DataSources.Add(InvoiceHeader);
            lr.DataSources.Add(DataRecord);
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

            //    "<DeviceInfo>" +
            //"  <OutputFormat>" + reportType + "</OutputFormat>" +
            //"  <PageWidth>8.5in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>1cm</MarginTop>" +
            //"  <MarginLeft>0.34cm</MarginLeft>" +
            //"  <MarginRight>0.1cm</MarginRight>" +
            //"  <MarginBottom>1cm</MarginBottom>" +
            //"</DeviceInfo>";

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
            string Pdfpathcreate = Server.MapPath("~/bill/" + name);
            BinaryWriter Writer = null;
            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
            Writer.Write(renderedBytes);
            Writer.Flush();
            Writer.Close();
            //string url = "";
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
            //}
            //else
            //{
            //    url = "http://" + Request.Url.Host + "/bill/" + name;
            //}


            string url = "";
            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/bill/" + name;
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            }
            else
            {
                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PrintInvoiceforRupeesExp(long OrderID, string InvoiceNumber, decimal Rupees)
        {
            OrderIdvalue = OrderID;
            LocalReport lr = new LocalReport();
            string path = "";
            //if (Rupees != null || Rupees > 0)
            //{
            // bool response = _orderservice.UpdateExportDollarPrice(OrderID, InvoiceNumber, Rupees);
            //}
            List<ExportOrderQtyInvoiceList> orderdata = _orderservice.GetExportInvoiceOrderDetailForPrintRupees(OrderID, Rupees);
            List<ExportOrderQtyInvoiceList> Lstdata = _orderservice.GetInvoiceForExportOrderItemPrintRupees(OrderID, InvoiceNumber, Rupees);
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    path = "Report/PrintExportInvoiceRupees.rdlc";
            //}
            //else
            //{
            //    path = Server.MapPath("~/Report/PrintExportInvoiceRupees.rdlc");
            //}

            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                path = "Report/PrintExportInvoiceRupees.rdlc";
            }
            else
            {
                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintExportInvoiceRupees.rdlc");
            }
            lr.ReportPath = path;
            DataTable header = Common.ToDataTable(orderdata);
            DataTable FoodDT = Common.ToDataTable(Lstdata);
            ReportDataSource InvoiceHeader = new ReportDataSource("DataSet1", header);
            ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
            lr.DataSources.Add(InvoiceHeader);
            lr.DataSources.Add(DataRecord);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

            "<DeviceInfo>" +
            " <OutputFormat>" + reportType + "</OutputFormat>" +
            " <PageWidth>8.5in</PageWidth>" +
            " <PageHeight>11in</PageHeight>" +
            " <MarginTop>1cm</MarginTop>" +
            " <MarginLeft>1cm</MarginLeft>" +
            " <MarginRight>1cm</MarginRight>" +
            " <MarginBottom>1cm</MarginBottom>" +
            "</DeviceInfo>";

            // "<DeviceInfo>" +
            //" <OutputFormat>" + reportType + "</OutputFormat>" +
            //" <PageWidth>8.5in</PageWidth>" +
            //" <PageHeight>11in</PageHeight>" +
            //" <MarginTop>1cm</MarginTop>" +
            //" <MarginLeft>0.34cm</MarginLeft>" +
            //" <MarginRight>0.1cm</MarginRight>" +
            //" <MarginBottom>1cm</MarginBottom>" +
            //"</DeviceInfo>";

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
            string Pdfpathcreate = Server.MapPath("~/bill/" + name);
            BinaryWriter Writer = null;
            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
            Writer.Write(renderedBytes);
            Writer.Flush();
            Writer.Close();
            //string url = "";
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
            //}
            //else
            //{
            //    url = "http://" + Request.Url.Host + "/bill/" + name;
            //}




            string url = "";
            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/bill/" + name;
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            }
            else
            {
                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }



    }
}