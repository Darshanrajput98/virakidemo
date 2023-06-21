using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Reflection;
using vb.Data.ViewModel;
using System.Configuration;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class OrdersController : Controller
    {
        private static object Lock = new object();
        private IOrderService _orderservice;
        private ICommonService _commonservice;
        private IProductService _productservice;
        public long OrderIdvalue;
        public long ChallanIdvalue;

        List<OrderQtyInvoiceList> Lstdata;
        List<OrderQtyInvoiceList> Lstdatainvoice;
        List<ChallanQtyInvoiceList> Lstdata2;
        List<ChallanQtyInvoiceList> Lstdatainvoice2;

        public OrdersController(IOrderService orderservice, ICommonService commonservice, IProductService productservice)
        {
            _orderservice = orderservice;
            _commonservice = commonservice;
            _productservice = productservice;
        }


        //
        // GET: /wholesale/Orders/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageOrders()
        {
            //Session["UserID"] = "1";
            ViewBag.Customer = _orderservice.GetActiveCustomerName(0);
            ViewBag.Product = _orderservice.GetAllProductName();
            return View();
        }

        public JsonResult txtCustomerName_TextChanged(string obj)
        {
            int quntity = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["drugQuntityshow"].ToString());
            List<CustomerName1> data = _orderservice.GetTaxForCustomerByTextChange(obj).Select(x => new CustomerName1() { CustomerName = x }).Take(quntity).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult txtCustomerName_AfterCustomerSelect(string obj)
        {
            try
            {
                string[] cityarr = obj.Split(',');
                string ID = string.Empty;
                if (cityarr.Length > 1)
                {
                    var Tax = _orderservice.GetTaxForCustomerNumber(Convert.ToInt64(cityarr[0]));
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
        public ActionResult AddOrder(OrderViewModel data)
        {
            lock (Lock)
            {
                var lstdata = _orderservice.GetLastOrderID();
                string OrderID = "";
                if (lstdata != null)
                {
                    long incr = Convert.ToInt64(lstdata.OrderID + 1);
                    OrderID = "VB" + Convert.ToString(incr);
                }
                else
                {
                    OrderID = "VB1";
                }
                try
                {
                    if (Request.Cookies["UserID"] == null)
                    {
                        Request.Cookies["UserID"].Value = null;
                        return JavaScript("location.reload(true)");
                    }
                    else
                    {
                        data.InvoiceNumber = OrderID;
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                        data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.UpdatedOn = DateTime.Now;
                        long respose = _orderservice.AddOrder(data);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ViewOrderList()
        {
            //Session["UserID"] = "1";
            return View();
        }

        public PartialViewResult ViewOrderList1()
        {
            List<OrderListResponse> objModel = _orderservice.GetAllOrderQtyList();
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult GetUnit(long ProductID)
        {
            var lstUnit = _orderservice.GetAutoCompleteProduct(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSellPrice(long ProductName, decimal Quantity, string Tax)
        {
            if (Tax != "")
            {
                var SellPrice = _orderservice.GetAutoCompleteSellPrice(ProductName, Quantity, Tax);
                return Json(SellPrice, JsonRequestBehavior.AllowGet);
            }
            else
            {
                GetSellPrice objdata = new GetSellPrice();
                objdata.SellPrice = 0;
                objdata.Tax = 0;
                return Json(objdata, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTaxForCustomer(long CustomerID)
        {
            var Tax = _orderservice.GetTaxForCustomer(CustomerID);
            return Json(Tax, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchViewBillWiseOrderList(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto)
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseOrderList(OrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["custid"] = "";
                Session["uid"] = "";
                Session["txtfrom"] = "";
                Session["txtto"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            ViewBag.TransportName = _commonservice.GetAllTransportName();
            List<OrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        public ActionResult SearchViewBillWiseCreditMemoList(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto, Int64? pid, string invoicenumber)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(custid)) && !string.IsNullOrEmpty(Convert.ToString(txtfrom)) && !string.IsNullOrEmpty(Convert.ToString(txtto)) && !string.IsNullOrEmpty(invoicenumber))
            {
                Session["ccustid"] = Convert.ToInt64(custid);
                string from = Convert.ToString(txtfrom);
                from = from.Remove(from.Length - 12);
                string to = Convert.ToString(txtto);
                to = to.Remove(to.Length - 12);
                Session["ctxtfrom"] = from;
                Session["ctxtto"] = to;
                Session["cinvoicenumber"] = invoicenumber;
            }
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            ViewBag.Product = _orderservice.GetAllProductName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseCreditMemoList(ClsReturnOrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["ccustid"] = "";
                Session["cuid"] = "";
                Session["ctxtfrom"] = "";
                Session["ctxtto"] = "";
                Session["cpid"] = "";
                Session["cinvoicenumber"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ClsReturnOrderListResponse> objModel = _orderservice.GetBillWiseCreditMemoForOrder(model);
            var results = from p in objModel
                          group p.CustomerName by p.InvoiceNumber into g
                          select new { InvoiceNumber = g.Key, Cars = g.ToList() };
            List<ReturnOrderListResponse> objdata = new List<ReturnOrderListResponse>();
            foreach (var item in results)
            {
                ReturnOrderListResponse obj = new ReturnOrderListResponse();
                var data = objModel.Where(x => x.InvoiceNumber == item.InvoiceNumber).FirstOrDefault();
                obj.AreaName = data.AreaName;
                obj.CustomerName = data.CustomerName;
                obj.CreatedOn = data.CreatedOn;
                obj.GrandFinalTotal = data.GrandFinalTotal;
                obj.GrandQuantity = data.GrandQuantity;
                obj.InvoiceNumber = data.InvoiceNumber;
                obj.OrderID = data.OrderID;
                obj.UserName = data.UserName;
                List<OrderQtyList> lstdatanew = new List<OrderQtyList>();
                lstdatanew = objModel.Where(x => x.InvoiceNumber == obj.InvoiceNumber).Select(x => new OrderQtyList() { ProductID = x.ProductID, OrderQtyID = x.OrderQtyID, OrderID = x.OrderID, CategoryTypeID = x.CategoryTypeID, ReturnedQuantity = x.ReturnedQuantity, SerialNumber = x.SerialNumber, ProductName = x.ProductName, Quantity = x.Quantity, UnitName = x.UnitName, ProductPrice = x.ProductPrice, LessAmount = x.LessAmount, SaleRate = x.SaleRate, BillDiscount = x.BillDiscount, Total = x.Total, Tax = x.Tax, TaxAmt = x.TaxAmt, FinalTotal = x.FinalTotal, CustomerID = x.CustomerID, CreatedOn = x.CreatedOn }).ToList();
                obj.lstOrderQty = new List<OrderQtyList>();
                obj.lstOrderQty = lstdatanew;
                objdata.Add(obj);
            }
            return PartialView(objdata);
        }

        public ActionResult SearchViewOrderList(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto)
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewOrderList(OrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["custid"] = "";
                Session["uid"] = "";
                Session["txtfrom"] = "";
                Session["txtto"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            ViewBag.Godown = _productservice.GetAllGodownName();
            ViewBag.Transport = _commonservice.GetAllTransportName();

            // 29th April, 2021 Sonal Gandhi
            ViewBag.VehicleNo = _commonservice.GetAllTempoNumberList();

            List<OrderListResponse> objModel = _orderservice.GetAllOrderList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public PartialViewResult ViewReturnedOrderList(OrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["custid"] = "";
                Session["txtfrom"] = "";
                Session["txtto"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<OrderListResponse> objModel = _orderservice.GetAllReturnedOrderList(model);
            return PartialView(objModel);
        }

        public ActionResult SearchInvoice()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewSearchInvoiceList(OrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["custid"] = "";
                Session["uid"] = "";
                Session["txtfrom"] = "";
                Session["txtto"] = "";
            }
            List<OrderListResponse> objModel = _orderservice.GetSearchInvoiceList(model);
            return PartialView(objModel);
        }




        //[HttpPost]
        //public ActionResult PrintCreditMemo(string CreditMemoNumber)
        //{
        //    LocalReport lr = new LocalReport();
        //    string path = "";
        //    Lstdata = _orderservice.GetCreditMemoInvoiceForOrderItemPrint(CreditMemoNumber);
        //    if (Lstdata[0].TaxName == "IGST")
        //    {
        //        //if (Request.Url.Host.Contains("localhost"))
        //        //{
        //        //    path = "Report/IGSTCreditMemoInvoiceTest.rdlc";
        //        //}
        //        //else
        //        //{
        //        //    path = Server.MapPath("~/Report/IGSTCreditMemoInvoiceTest.rdlc");
        //        //}

        //        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
        //        {
        //            path = "Report/IGSTCreditMemoInvoiceTest.rdlc";
        //        }
        //        else
        //        {
        //            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/IGSTCreditMemoInvoiceTest.rdlc");
        //        }


        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        //if (Request.Url.Host.Contains("localhost"))
        //        //{
        //        //    path = "Report/CreditMemoInvoiceTest.rdlc";
        //        //}
        //        //else
        //        //{
        //        //    path = Server.MapPath("~/Report/CreditMemoInvoiceTest.rdlc");
        //        //}

        //        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
        //        {
        //            path = "Report/CreditMemoInvoiceTest.rdlc";
        //        }
        //        else
        //        {
        //            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/CreditMemoInvoiceTest.rdlc");
        //        }

        //        lr.ReportPath = path;
        //    }

        //    Lstdatainvoice = new List<OrderQtyInvoiceList>();
        //    List<string> incoiveNolist = new List<string>();

        //    string[] strcmn = CreditMemoNumber.Split(',');
        //    foreach (var itm in strcmn)
        //    {
        //        incoiveNolist.Add(itm);
        //    }

        //    foreach (var invoicelstitem in incoiveNolist)
        //    {
        //        int rownumber = 1;
        //        int totalnumber = 1; decimal total = 0;
        //        decimal taxtotal = 0;
        //        decimal grandtotal = 0;
        //        string NumberToWord = "";

        //        var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
        //        if (Foodzero.Count > 0)
        //        {
        //            int Foodzero2 = Foodzero.Count;

        //            int FTotalcount = 0;
        //            int FTotalrecord = 0;
        //            int FTotalQuantity = 0;
        //            decimal FATotalAmount = 0;
        //            decimal FTotalTax = 0;
        //            decimal FAGrandTotal = 0;
        //            decimal FGrandTotal = 0;
        //            string FTaxName = "";
        //            string FGrandAmtWord = "";
        //            string InvoiceNumber = "";

        //            if (Foodzero.Count < 15)
        //            {
        //                for (int s = Foodzero.Count; s < 15; s++)
        //                {
        //                    Foodzero.Add(new OrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = 0, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", City = "", State = "", StateCode = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
        //                }
        //            }
        //            foreach (var item in Foodzero)
        //            {
        //                if (item.ProductName == "")
        //                {
        //                    item.RowNumber = rownumber;
        //                    item.InvoiceNumber = InvoiceNumber;
        //                    item.Totalcount = FTotalcount;
        //                    item.Totalrecord = Convert.ToInt32(FTotalrecord);
        //                    item.ATotalAmount = Math.Round(FATotalAmount, 2);
        //                    item.TaxName = FTaxName;
        //                    item.TotalTax = FTotalTax;
        //                    item.AGrandTotal = FAGrandTotal;
        //                    item.GrandTotal = FGrandTotal;
        //                    item.GrandAmtWord = FGrandAmtWord;
        //                    Lstdatainvoice.Add(item);
        //                    rownumber = rownumber + 1;
        //                }
        //                else
        //                {
        //                    if (rownumber > 15)
        //                    {
        //                        rownumber = 1;
        //                        totalnumber = totalnumber + 1;
        //                        total = 0;
        //                        taxtotal = 0;
        //                        grandtotal = 0;
        //                        NumberToWord = "";
        //                    }
        //                    total += item.Total;
        //                    taxtotal += item.TaxAmt;
        //                    grandtotal += item.FinalTotal;
        //                    item.ATotalAmount = total;
        //                    item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
        //                    item.Totalrecord = Lstdata.Count;
        //                    if (item.TaxName == "IGST")
        //                    {
        //                        item.TotalTax = taxtotal;
        //                    }
        //                    else
        //                    {
        //                        item.TotalTax = taxtotal / 2;
        //                    }

        //                    if (item.Tax == 0)
        //                    {
        //                        item.InvoiceTitle = "BILL OF SUPPLY";
        //                    }
        //                    else
        //                    {
        //                        item.InvoiceTitle = "TAX";
        //                    }
        //                    item.TotalTax = Math.Round(item.TotalTax, 2);
        //                    item.TotalAmount = item.Total + item.TaxAmt;
        //                    item.AGrandTotal += grandtotal;
        //                    item.AGrandTotal = Math.Round(item.AGrandTotal, 2);
        //                    int number = Convert.ToInt32(item.AGrandTotal);
        //                    NumberToWord = NumberToWords(number);
        //                    item.GrandAmtWord = NumberToWord;
        //                    var Gtotal = Math.Round(grandtotal);
        //                    item.GrandTotal = Gtotal;
        //                    if (item.TaxName == "IGST")
        //                    {
        //                        item.TaxAmt = item.TaxAmt;
        //                    }
        //                    else
        //                    {
        //                        item.TaxAmt = item.TaxAmt / 2;
        //                    }
        //                    if (item.TaxName == "IGST")
        //                    {
        //                        item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
        //                    }
        //                    else
        //                    {
        //                        item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
        //                    }
        //                    item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
        //                    item.RowNumber = rownumber;
        //                    item.OrdRowNumber = rownumber;
        //                    item.Totalcount = Foodzero2;
        //                    if (Foodzero2 > 15 * totalnumber)
        //                    {
        //                        item.Totalcount = Foodzero2 > 15 * totalnumber ? 15 * totalnumber : 15 * totalnumber - Foodzero2;
        //                    }
        //                    else
        //                    {
        //                        if (totalnumber == 1)
        //                        {
        //                            item.Totalcount = Foodzero2;
        //                        }
        //                        else
        //                        {
        //                            item.Totalcount = Foodzero2 - (15 * (totalnumber - 1));
        //                        }
        //                    }
        //                    Lstdatainvoice.Add(item);
        //                    if (rownumber == Foodzero2)
        //                    {
        //                        FTotalcount = item.Totalcount;
        //                        FTotalrecord = Convert.ToInt32(item.Totalrecord);
        //                        FATotalAmount = item.ATotalAmount;
        //                        FTaxName = item.TaxName;
        //                        FTotalTax = item.TotalTax;
        //                        FAGrandTotal = item.AGrandTotal;
        //                        FGrandTotal = item.GrandTotal;
        //                        FGrandAmtWord = item.GrandAmtWord;
        //                        InvoiceNumber = item.InvoiceNumber;
        //                    }
        //                    rownumber = rownumber + 1;
        //                }
        //            }
        //        }
        //    }
        //    DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);
        //    ReportDataSource DataRecord = new ReportDataSource("DataSet1", FoodDT);
        //    lr.DataSources.Add(DataRecord);
        //    string reportType = "pdf";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;
        //    string deviceInfo =

        //        "<DeviceInfo>" +
        //    "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //    "  <PageWidth>11in</PageWidth>" +
        //    "  <PageHeight>8.5in</PageHeight>" +
        //    "  <MarginTop>1cm</MarginTop>" +
        //    "  <MarginLeft>1cm</MarginLeft>" +
        //    "  <MarginRight>1cm</MarginRight>" +
        //    "  <MarginBottom>1cm</MarginBottom>" +
        //    "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);

        //    string name = DateTime.Now.Ticks.ToString() + ".pdf";
        //    string Pdfpathcreate = Server.MapPath("~/creditmemo/" + name);

        //    BinaryWriter Writer = null;
        //    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
        //    Writer.Write(renderedBytes);
        //    Writer.Flush();
        //    Writer.Close();

        //    //string url = "";
        //    //if (Request.Url.Host.Contains("localhost"))
        //    //{
        //    //    url = "http://" + Request.Url.Host + ":6551/creditmemo/" + name;
        //    //}
        //    //else
        //    //{
        //    //    url = "http://" + Request.Url.Host + "/creditmemo/" + name;
        //    //}


        //    string url = "";
        //    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
        //    {
        //        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/creditmemo/" + name;
        //    }
        //    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
        //    {
        //        url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/creditmemo/" + name;
        //    }
        //    else
        //    {
        //        url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/creditmemo/" + name;
        //    }


        //    return Json(url, JsonRequestBehavior.AllowGet);
        //}




        // 6 April,2021 Sonal Gandhi
        [HttpPost]
        public ActionResult PrintCreditMemo(string CreditMemoNumber, bool isNewCreditMemo = false)
        {
            LocalReport lr = new LocalReport();
            string path = "";

            // 6 April,2021 Sonal Gandhi
            List<string> chkInvoiceNumber = new List<string>();
            string IRN = "";
            long ActNo = 0;
            string ActDate = "";
            string QRCode = "";

            // 27 April,2021 Sonal Gandhi
            bool isGenIrn = false;
            // 27 April,2021 Sonal Gandhi

            int creditMemoRowNumber = 0;
            Lstdata = _orderservice.GetCreditMemoInvoiceForOrderItemPrint(CreditMemoNumber);
            if (string.IsNullOrEmpty(Lstdata[0].TaxNo) || Convert.ToDateTime(Lstdata[0].CreatedOn) < Convert.ToDateTime("03/31/2021"))
            {
                creditMemoRowNumber = 15;
                if (Lstdata[0].TaxName == "IGST")
                {
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/IGSTCreditMemoInvoiceTest.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/IGSTCreditMemoInvoiceTest.rdlc");
                    //}

                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/IGSTCreditMemoInvoiceTest.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/IGSTCreditMemoInvoiceTest.rdlc");
                    }
                    lr.ReportPath = path;
                }
                else
                {
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/CreditMemoInvoiceTest.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/CreditMemoInvoiceTest.rdlc");
                    //}

                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/CreditMemoInvoiceTest.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/CreditMemoInvoiceTest.rdlc");
                    }
                    lr.ReportPath = path;
                }
            }
            else
            {
                creditMemoRowNumber = 10;
                if (Lstdata[0].TaxName == "IGST")
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_IGSTCreditMemoInvoiceTest.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_IGSTCreditMemoInvoiceTest.rdlc");
                    }
                    lr.ReportPath = path;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_CreditMemoInvoiceTest.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_CreditMemoInvoiceTest.rdlc");
                    }
                    lr.ReportPath = path;
                }
            }

            Lstdatainvoice = new List<OrderQtyInvoiceList>();
            List<string> incoiveNolist = new List<string>();

            //6 April,2021 Sonal Gandhi
            EInvoiceCreditMemo einvoice = new EInvoiceCreditMemo();
            List<EInvoiceCreditMemo> eInvoiceLst = new List<EInvoiceCreditMemo>();
            List<OrderQtyInvoiceList> creditMemoToEInvoiceLst = new List<OrderQtyInvoiceList>();

            string[] strcmn = CreditMemoNumber.Split(',');
            foreach (var itm in strcmn)
            {
                incoiveNolist.Add(itm);
            }

            EInvoiceCreditMemoController eInvoice = new EInvoiceCreditMemoController();

            foreach (var invoicelstitem in incoiveNolist)
            {
                int rownumber = 1;
                int totalnumber = 1; decimal total = 0;
                decimal taxtotal = 0;
                decimal grandtotal = 0;
                string NumberToWord = "";

                var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
                if (Foodzero.Count > 0)
                {
                    int Foodzero2 = Foodzero.Count;

                    int FTotalcount = 0;
                    int FTotalrecord = 0;
                    int FTotalQuantity = 0;
                    decimal FATotalAmount = 0;
                    decimal FTotalTax = 0;
                    decimal FAGrandTotal = 0;
                    decimal FGrandTotal = 0;
                    string FTaxName = "";
                    string FGrandAmtWord = "";
                    string InvoiceNumber = "";

                    // 09 April 2021 Piyush Limbani
                    decimal TCSTaxAmount = 0;
                    // 09 April 2021 Piyush Limbani

                    //6 April,2021 Sonal Gandhi
                    string creditMemoNumber = invoicelstitem;
                    QRCode = "";
                    creditMemoToEInvoiceLst = new List<OrderQtyInvoiceList>();

                    if (Foodzero.Count < creditMemoRowNumber)
                    {
                        for (int s = Foodzero.Count; s < creditMemoRowNumber; s++)
                        {
                            Foodzero.Add(new OrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = 0, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", City = "", State = "", StateCode = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
                        }
                    }
                    foreach (var item in Foodzero)
                    {
                        if (item.ProductName == "")
                        {
                            item.RowNumber = rownumber;
                            item.InvoiceNumber = InvoiceNumber;
                            item.Totalcount = FTotalcount;
                            item.Totalrecord = Convert.ToInt32(FTotalrecord);
                            item.ATotalAmount = Math.Round(FATotalAmount, 2);
                            item.TaxName = FTaxName;
                            item.TotalTax = FTotalTax;
                            item.AGrandTotal = FAGrandTotal;
                            item.GrandTotal = FGrandTotal;

                            // 09 April 2021 Piyush Limbani
                            item.TCSTaxAmount = TCSTaxAmount;
                            // 09 April 2021 Piyush Limbani

                            item.GrandAmtWord = FGrandAmtWord;

                            // 6 April, 2021 Sonal Gandhi
                            if (!isNewCreditMemo)
                            {
                                if (IRN != null || IRN != "")
                                    item.IRN = IRN;
                                if (ActNo > 0)
                                    item.AckNo = ActNo;
                                if (ActDate != null || ActDate != "")
                                    item.AckDt = Convert.ToDateTime(ActDate);
                                if (QRCode != null || QRCode != "")
                                {
                                    item.QRCode = QRCode;
                                }
                            }

                            Lstdatainvoice.Add(item);
                            rownumber = rownumber + 1;
                        }
                        else
                        {
                            if (rownumber > creditMemoRowNumber)
                            {
                                rownumber = 1;
                                totalnumber = totalnumber + 1;
                                total = 0;
                                taxtotal = 0;
                                grandtotal = 0;
                                NumberToWord = "";
                            }
                            total += item.Total;
                            taxtotal += item.TaxAmt;
                            grandtotal += item.FinalTotal;
                            item.ATotalAmount = total;
                            item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                            item.Totalrecord = Lstdata.Count;
                            if (item.TaxName == "IGST")
                            {
                                item.TotalTax = taxtotal;
                            }
                            else
                            {
                                item.TotalTax = taxtotal / 2;
                            }

                            if (item.Tax == 0)
                            {
                                item.InvoiceTitle = "BILL OF SUPPLY";
                            }
                            else
                            {
                                item.InvoiceTitle = "TAX";
                            }
                            item.TotalTax = Math.Round(item.TotalTax, 2);
                            item.TotalAmount = item.Total + item.TaxAmt;
                            item.AGrandTotal += grandtotal;
                            item.AGrandTotal = Math.Round(item.AGrandTotal, 2);

                            // 09 April 2021 Piyush Limbani              
                            var Gtotal = Math.Round(grandtotal);
                            if (item.InvTotal == 0)
                            {
                                int number = Convert.ToInt32(item.AGrandTotal);
                                NumberToWord = NumberToWords(number);
                                item.GrandTotal = Gtotal;
                                item.GrandAmtWord = NumberToWord;
                            }
                            else
                            {
                                int number = Convert.ToInt32(item.InvTotal);
                                NumberToWord = NumberToWords(number);
                                item.GrandTotal = item.InvTotal;
                                item.GrandAmtWord = NumberToWord;
                            }
                            item.TCSTaxAmount = item.TCSTaxAmount;
                            // 09 April 2021 Piyush Limbani

                            if (item.TaxName == "IGST")
                            {
                                item.TaxAmt = item.TaxAmt;
                                item.IGSTAmount = Math.Round(item.TaxAmt, 2);
                            }
                            else
                            {
                                item.TaxAmt = item.TaxAmt / 2;
                                item.SGSTAmount = Math.Round(item.TaxAmt, 2);
                                item.CGSTAmount = Math.Round(item.TaxAmt, 2);
                            }
                            if (item.TaxName == "IGST")
                            {
                                item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                item.IGSTTaxRate = item.Tax;
                            }
                            else
                            {
                                item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                item.SGSTTaxRate = Math.Round(item.Tax, 2);
                                item.CGSTTaxRate = Math.Round(item.Tax, 2);
                            }
                            item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
                            item.RowNumber = rownumber;
                            item.OrdRowNumber = rownumber;
                            item.Totalcount = Foodzero2;
                            if (Foodzero2 > creditMemoRowNumber * totalnumber)
                            {
                                item.Totalcount = Foodzero2 > creditMemoRowNumber * totalnumber ? creditMemoRowNumber * totalnumber : creditMemoRowNumber * totalnumber - Foodzero2;
                            }
                            else
                            {
                                if (totalnumber == 1)
                                {
                                    item.Totalcount = Foodzero2;
                                }
                                else
                                {
                                    item.Totalcount = Foodzero2 - (creditMemoRowNumber * (totalnumber - 1));
                                }
                            }

                            //6 April,2021 Sonal Gandhi
                            if (!isNewCreditMemo)
                            {
                                IRN = item.IRN;
                                ActNo = item.AckNo;
                                ActDate = Convert.ToString(item.AckDt);
                                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                {
                                    item.QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + item.QRCode;
                                }
                                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                {
                                    item.QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                }
                                else
                                {
                                    item.QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                }
                            }


                            Lstdatainvoice.Add(item);
                            creditMemoToEInvoiceLst.Add(item);

                            if (rownumber == Foodzero2)
                            {
                                FTotalcount = item.Totalcount;
                                FTotalrecord = Convert.ToInt32(item.Totalrecord);
                                FATotalAmount = item.ATotalAmount;
                                FTaxName = item.TaxName;
                                FTotalTax = item.TotalTax;
                                FAGrandTotal = item.AGrandTotal;
                                FGrandTotal = item.GrandTotal;

                                // 09 April 2021 Piyush Limbani
                                TCSTaxAmount = item.TCSTaxAmount;
                                // 09 April 2021 Piyush Limbani

                                FGrandAmtWord = item.GrandAmtWord;
                                InvoiceNumber = item.InvoiceNumber;

                                //6 April,2021 Sonal Gandhi
                                creditMemoNumber = invoicelstitem;
                                IRN = item.IRN;
                                ActNo = item.AckNo;
                                ActDate = Convert.ToString(item.AckDt);
                                QRCode = item.QRCode;
                            }
                            rownumber = rownumber + 1;
                        }
                    }



                    // 07 April, 2021 Piyush Limbani (Check IRN is avaliable or not)
                    long EInvoiceCreditMemoId = 0;
                    if (isNewCreditMemo == false && !string.IsNullOrEmpty(CreditMemoNumber))
                    {
                        EInvoiceCreditMemoId = _orderservice.CheckECreditMemoExist(CreditMemoNumber);
                    }
                    // 07 April, 2021 Piyush Limbani (Check IRN is avaliable or not)




                    // 6 April, 2021 Sonal Gandhi Call EInvoice API
                    if (isNewCreditMemo)
                    {
                        //if (Lstdata[0].TaxNo != null || Lstdata[0].TaxNo != "")
                        if (!string.IsNullOrEmpty(Lstdata[0].TaxNo))
                        {
                            if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                            {
                                chkInvoiceNumber.Add(invoicelstitem);
                                einvoice = new EInvoiceCreditMemo();
                                einvoice = eInvoice.GenIRN(Lstdata[0], creditMemoToEInvoiceLst);
                                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                {
                                    QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + einvoice.QRCode;
                                }
                                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                {
                                    QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                }
                                else
                                {
                                    QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                }
                                einvoice.QRCode = QRCode;
                                einvoice.CreditMemoNumber = invoicelstitem;
                                eInvoiceLst.Add(einvoice);
                            }
                        }
                    }

                    // 07 April, 2021 Piyush Limbani (Regenerate Credit Memo)
                    else if (EInvoiceCreditMemoId == 0 && isNewCreditMemo == false && !string.IsNullOrEmpty(CreditMemoNumber) && Convert.ToDateTime(Lstdata[0].CreatedOn) > Convert.ToDateTime("03/31/2021"))
                    {
                        //if (Lstdata[0].TaxNo != null || Lstdata[0].TaxNo != "")
                        if (!string.IsNullOrEmpty(Lstdata[0].TaxNo))
                        {
                            if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                            {
                                isGenIrn = true;  // 27 April 2021 Sonal Gandhi
                                chkInvoiceNumber.Add(invoicelstitem);
                                einvoice = new EInvoiceCreditMemo();
                                einvoice = eInvoice.GenIRN(Lstdata[0], creditMemoToEInvoiceLst);
                                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                {
                                    QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + einvoice.QRCode;
                                }
                                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                {
                                    QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                }
                                else
                                {
                                    QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                }
                                einvoice.QRCode = QRCode;
                                einvoice.CreditMemoNumber = invoicelstitem;
                                eInvoiceLst.Add(einvoice);
                            }
                        }
                    }
                    // 07 April, 2021 Piyush Limbani (Regenerate Credit Memo)

                }
            }

            if (isNewCreditMemo || isGenIrn)
            {
                foreach (var item in eInvoiceLst)
                {
                    foreach (var itm in Lstdatainvoice)
                    {
                        if (itm.InvoiceNumber.Contains(item.CreditMemoNumber))
                        {
                            itm.IRN = item.IRN;
                            itm.AckDt = item.AckDt;
                            itm.AckNo = item.AckNo;
                            itm.QRCode = item.QRCode;
                        }
                    }
                }
            }

            DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);
            ReportDataSource DataRecord = new ReportDataSource("DataSet1", FoodDT);
            if (!string.IsNullOrEmpty(Lstdata[0].TaxNo))
            {
                lr.EnableExternalImages = true;
            }
            lr.DataSources.Add(DataRecord);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

                "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
            string Pdfpathcreate = Server.MapPath("~/creditmemo/" + name);

            BinaryWriter Writer = null;
            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
            Writer.Write(renderedBytes);
            Writer.Flush();
            Writer.Close();

            //string url = "";
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    url = "http://" + Request.Url.Host + ":6551/creditmemo/" + name;
            //}
            //else
            //{
            //    url = "http://" + Request.Url.Host + "/creditmemo/" + name;
            //}


            string url = "";
            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/creditmemo/" + name;
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/creditmemo/" + name;
            }
            else
            {
                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/creditmemo/" + name;
            }


            return Json(url, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public ActionResult PrintInvoice(long InvoiceID, string InvoiceNumber, bool isNewOrder = false)
        {
            OrderIdvalue = InvoiceID;
            LocalReport lr = new LocalReport();
            string path = "";

            // 27 March,2021 Sonal Gandhi
            List<string> chkInvoiceNumber = new List<string>();
            //24 March,2021 Sonal Gandhi
            string IRN = "";
            long ActNo = 0;
            string ActDate = "";
            string QRCode = "";

            // 27 April,2021 Sonal Gandhi
            bool isGenIrn = false;
            // 27 April,2021 Sonal Gandhi

            int invoiceRowNumber = 0;

            decimal InvoiceTotal = 0;
            long ProductCount = 0;
            var orderdata = _orderservice.GetInvoiceForOrderPrint(InvoiceID);

            DataTable header = Common.ToDataTable(orderdata);

            /// check gst no for EInvoice.rdlc
            //if (orderdata[0].TaxNo == null || orderdata[0].TaxNo == "")
            if (string.IsNullOrEmpty(orderdata[0].TaxNo) || Convert.ToDateTime(orderdata[0].CreatedOn) < Convert.ToDateTime("03/31/2021"))
            {
                invoiceRowNumber = 15;
                if (orderdata[0].TaxName == "IGST")
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/IGSTInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/IGSTInvoice.rdlc");
                    }
                    lr.ReportPath = path;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/Invoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/Invoice.rdlc");
                    }
                    lr.ReportPath = path;
                }
            }
            else
            {
                invoiceRowNumber = 10;
                if (orderdata[0].TaxName == "IGST")
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_IGSTInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_IGSTInvoice.rdlc");
                    }
                    lr.ReportPath = path;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_Invoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_Invoice.rdlc");
                    }
                    lr.ReportPath = path;
                }
            }


            ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
            Lstdatainvoice = new List<OrderQtyInvoiceList>();


            int dividerdata = 0;
            int ordergroup = 0;


            //27 March,2021 Sonal Gandhi
            EInvoice einvoice = new EInvoice();
            List<EInvoice> eInvoiceLst = new List<EInvoice>();
            List<OrderQtyInvoiceList> invoiceToEInvoiceLst = new List<OrderQtyInvoiceList>();

            List<string> incoiveNolist = _orderservice.GetListOfInvoice(InvoiceID);
            for (int i = 1; i <= orderdata[0].NoofInvoiceint; i++)
            {
                Lstdata = _orderservice.GetInvoiceForOrderItemPrint(OrderIdvalue, 1, InvoiceNumber);
                EInvoiceController eInvoice = new EInvoiceController();
                foreach (var invoicelstitem in incoiveNolist)
                {

                    int rownumber = 1;
                    int totalnumber = 1;
                    var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
                    var Count = Foodzero.Where(x => x.InvoiceNumber != "").ToList();
                    if (Foodzero.Count > 0)
                    {
                        int Foodzero2 = Foodzero.Count;
                        ordergroup++;
                        decimal total = 0;
                        decimal taxtotal = 0;
                        decimal grandtotal = 0;
                        string NumberToWord = "";
                        dividerdata = dividerdata + 1;
                        int FTotalcount = 0;
                        int FTotalrecord = 0;
                        // int FTotalQuantity = 0;
                        decimal FATotalAmount = 0;
                        decimal FTotalTax = 0;
                        decimal FAGrandTotal = 0;
                        decimal FGrandTotal = 0;
                        // 21 Sep 2020
                        decimal TCSTaxAmount = 0;
                        string FTaxName = "";
                        string FGrandAmtWord = "";

                        //27 March,2021 Sonal Gandhi
                        string invoiceNumber = invoicelstitem;
                        QRCode = "";
                        invoiceToEInvoiceLst = new List<OrderQtyInvoiceList>();



                        if (Foodzero.Count < invoiceRowNumber)
                        {
                            for (int s = Foodzero.Count; s < invoiceRowNumber; s++)
                            {
                                Foodzero.Add(new OrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", City = "", State = "", StateCode = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
                            }
                        }



                        foreach (var item in Foodzero)
                        {
                            if (item.ProductName == "")
                            {
                                item.RowNumber = rownumber;
                                item.ordergroup = ordergroup;
                                item.Totalcount = FTotalcount;
                                item.Totalrecord = Convert.ToInt32(FTotalrecord);
                                item.ATotalAmount = Math.Round(FATotalAmount, 2);
                                item.TaxName = FTaxName;
                                item.TotalTax = FTotalTax;
                                item.AGrandTotal = FAGrandTotal;
                                item.GrandTotal = FGrandTotal;
                                item.TCSTaxAmount = TCSTaxAmount;
                                item.GrandAmtWord = FGrandAmtWord;

                                // 24 March, 2021 Sonal Gandhi
                                item.InvoiceNumber = invoiceNumber;
                                if (!isNewOrder)
                                {
                                    if (IRN != null || IRN != "")
                                        item.IRN = IRN;
                                    if (ActNo > 0)
                                        item.AckNo = ActNo;
                                    if (ActDate != null || ActDate != "")
                                        item.AckDt = Convert.ToDateTime(ActDate);
                                    if (QRCode != null || QRCode != "")
                                    {
                                        item.QRCode = QRCode;
                                    }
                                }

                                Lstdatainvoice.Add(item);
                                rownumber = rownumber + 1;
                            }
                            else
                            {
                                if (rownumber > invoiceRowNumber)
                                {
                                    rownumber = 1;
                                    totalnumber = totalnumber + 1;
                                    total = 0;
                                    taxtotal = 0;
                                    grandtotal = 0;
                                    NumberToWord = "";
                                    ordergroup++;
                                }
                                item.ordergroup = ordergroup;
                                total += item.Total;
                                taxtotal += item.TaxAmt;
                                grandtotal += item.FinalTotal;
                                item.ATotalAmount = total;
                                item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                                //old
                                //item.Totalrecord = Lstdata.Count;
                                //23/01/2019_New
                                item.Totalrecord = item.Totalrecord;
                                if (item.TaxName == "IGST")
                                {
                                    item.TotalTax = taxtotal;
                                }
                                else
                                {
                                    item.TotalTax = taxtotal / 2;
                                }
                                if (i == 1)
                                {
                                    item.InvoiceTitleHeader = "ORIGINAL FOR RECIPIENT";
                                }
                                else if (i == 2)
                                {
                                    item.InvoiceTitleHeader = "DUPLICATE FOR TRANSPORTER";
                                }
                                else if (i == 3)
                                {
                                    item.InvoiceTitleHeader = "TRIPLICATE FOR SUPPLIER";
                                }
                                else
                                {
                                    item.InvoiceTitleHeader = "";
                                }
                                if (item.Tax == 0)
                                {
                                    item.InvoiceTitle = "BILL OF SUPPLY";
                                }
                                else
                                {
                                    item.InvoiceTitle = "TAX INVOICE";
                                }
                                item.TotalTax = Math.Round(item.TotalTax, 2);
                                item.TotalAmount = item.Total + item.TaxAmt;
                                item.AGrandTotal += grandtotal;
                                item.AGrandTotal = Math.Round(item.AGrandTotal, 2);

                                //int number = Convert.ToInt32(item.AGrandTotal);
                                //NumberToWord = NumberToWords(number);
                                int number = Convert.ToInt32(item.InvTotal);
                                NumberToWord = NumberToWords(number);

                                item.GrandAmtWord = NumberToWord;
                                var Gtotal = Math.Round(grandtotal);
                                //Old
                                // item.GrandTotal = Gtotal;
                                // New
                                item.GrandTotal = item.InvTotal;
                                item.TCSTaxAmount = item.TCSTaxAmount;
                                ProductCount++;
                                if (Count.Count == ProductCount && item.InvoiceTitleHeader == "ORIGINAL FOR RECIPIENT")
                                {
                                    ProductCount = 0;
                                    //Old
                                    //  InvoiceTotal += Gtotal;
                                    // New
                                    InvoiceTotal += item.InvTotal;
                                }
                                item.InvoiceTotal = InvoiceTotal;
                                //23-01-2019_Updatedon
                                item.OrderTotal = item.OrderTotal;
                                if (item.TaxName == "IGST")
                                {
                                    item.TaxAmt = item.TaxAmt;
                                    item.IGSTAmount = Math.Round(item.TaxAmt, 2);
                                }
                                else
                                {
                                    item.TaxAmt = item.TaxAmt / 2;
                                    item.SGSTAmount = Math.Round(item.TaxAmt, 2);
                                    item.CGSTAmount = Math.Round(item.TaxAmt, 2);
                                }
                                if (item.TaxName == "IGST")
                                {
                                    item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                    item.IGSTTaxRate = item.Tax;
                                }
                                else
                                {
                                    item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                    item.SGSTTaxRate = Math.Round(item.Tax, 2);
                                    item.CGSTTaxRate = Math.Round(item.Tax, 2);
                                }
                                item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
                                item.divider = dividerdata;
                                item.RowNumber = rownumber;
                                item.OrdRowNumber = rownumber;
                                item.Totalcount = Foodzero2;
                                if (Foodzero2 > invoiceRowNumber * totalnumber)
                                {
                                    item.Totalcount = Foodzero2 > invoiceRowNumber * totalnumber ? invoiceRowNumber * totalnumber : invoiceRowNumber * totalnumber - Foodzero2;
                                }
                                else
                                {
                                    if (totalnumber == 1)
                                    {
                                        item.Totalcount = Foodzero2;
                                    }
                                    else
                                    {
                                        item.Totalcount = Foodzero2 - (invoiceRowNumber * (totalnumber - 1));
                                    }
                                }

                                //25 March,2021 Sonal Gandhi

                                if (!isNewOrder)
                                {
                                    IRN = item.IRN;
                                    ActNo = item.AckNo;
                                    ActDate = Convert.ToString(item.AckDt);
                                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                    {
                                        item.QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + item.QRCode;
                                    }
                                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                    {
                                        item.QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                    }
                                    else
                                    {
                                        item.QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                    }
                                }
                                Lstdatainvoice.Add(item);
                                invoiceToEInvoiceLst.Add(item);
                                if (rownumber == Foodzero2)
                                {
                                    FTotalcount = item.Totalcount;
                                    FTotalrecord = Convert.ToInt32(item.Totalrecord);
                                    FATotalAmount = item.ATotalAmount;
                                    FTaxName = item.TaxName;
                                    FTotalTax = item.TotalTax;
                                    FAGrandTotal = item.AGrandTotal;
                                    FGrandTotal = item.GrandTotal;
                                    TCSTaxAmount = item.TCSTaxAmount;
                                    FGrandAmtWord = item.GrandAmtWord;

                                    //24 March,2021 Sonal Gandhi
                                    invoiceNumber = invoicelstitem;
                                    IRN = item.IRN;
                                    ActNo = item.AckNo;
                                    ActDate = Convert.ToString(item.AckDt);
                                    QRCode = item.QRCode;
                                    //if (!isNewOrder)
                                    //{
                                    //    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                    //    {
                                    //        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + item.QRCode;
                                    //    }
                                    //    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                    //    {
                                    //        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                    //    }
                                    //    else
                                    //    {
                                    //        QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                    //    }
                                    //}
                                }
                                rownumber = rownumber + 1;
                            }
                        }



                        // 02 April, 2021 Piyush Limbani (Check IRN is avaliable or not)
                        long EInvoiceId = 0;
                        if (isNewOrder == false && !string.IsNullOrEmpty(InvoiceNumber))
                        {
                            EInvoiceId = _orderservice.CheckEInvoiceExist(OrderIdvalue, InvoiceNumber);
                        }
                        // 02 April, 2021 Piyush Limbani (Check IRN is avaliable or not)


                        // 21 April, 2021 Piyush Limbani (Check IRN is avaliable or not)
                        else if (isNewOrder == false && !string.IsNullOrEmpty(invoicelstitem))
                        {
                            EInvoiceId = _orderservice.CheckEInvoiceExist(OrderIdvalue, invoicelstitem);
                        }
                        // 21 April, 2021 Piyush Limbani (Check IRN is avaliable or not)



                        // 24 March 2021 Sonal Gandhi Call EInvoice API
                        if (isNewOrder)
                        {
                            //if (orderdata[0].TaxNo != null || orderdata[0].TaxNo != "")
                            if (!string.IsNullOrEmpty(orderdata[0].TaxNo))
                            {
                                if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                                {
                                    chkInvoiceNumber.Add(invoicelstitem);
                                    einvoice = new EInvoice();
                                    einvoice = eInvoice.GenIRN(orderdata[0], invoiceToEInvoiceLst);
                                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                    {
                                        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + einvoice.QRCode;
                                    }
                                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                    {
                                        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                    }
                                    else
                                    {
                                        QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                    }
                                    einvoice.QRCode = QRCode;
                                    einvoice.InvoiceNumber = invoicelstitem;
                                    eInvoiceLst.Add(einvoice);
                                }
                            }
                        }

                         // 02 April, 2021 Piyush Limbani (Regenerate Invoice)
                        else if (EInvoiceId == 0 && isNewOrder == false && !string.IsNullOrEmpty(InvoiceNumber) && Convert.ToDateTime(orderdata[0].CreatedOn) > Convert.ToDateTime("03/31/2021"))
                        {
                            //if (orderdata[0].TaxNo != null || orderdata[0].TaxNo != "")
                            if (!string.IsNullOrEmpty(orderdata[0].TaxNo))
                            {
                                if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                                {
                                    isGenIrn = true;  // 27 April 2021 Sonal Gandhi
                                    chkInvoiceNumber.Add(invoicelstitem);
                                    einvoice = new EInvoice();
                                    einvoice = eInvoice.GenIRN(orderdata[0], invoiceToEInvoiceLst);
                                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                    {
                                        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + einvoice.QRCode;
                                    }
                                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                    {
                                        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                    }
                                    else
                                    {
                                        QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                    }
                                    einvoice.QRCode = QRCode;
                                    einvoice.InvoiceNumber = invoicelstitem;
                                    eInvoiceLst.Add(einvoice);
                                }
                            }
                        }
                        // 02 April, 2021 Piyush Limbani (Regenerate Invoice)




                        // 21 April, 2021 Piyush Limbani (Generate Invoice For Mobile Order)
                        else if (EInvoiceId == 0 && isNewOrder == false && !string.IsNullOrEmpty(invoicelstitem) && Convert.ToDateTime(orderdata[0].CreatedOn) > Convert.ToDateTime("03/31/2021"))
                        {
                            if (!string.IsNullOrEmpty(orderdata[0].TaxNo))
                            {
                                if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                                {
                                    isGenIrn = true; // 27 April 2021 Sonal Gandhi
                                    chkInvoiceNumber.Add(invoicelstitem);
                                    einvoice = new EInvoice();
                                    einvoice = eInvoice.GenIRN(orderdata[0], invoiceToEInvoiceLst);
                                    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                    {
                                        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + einvoice.QRCode;
                                    }
                                    else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                    {
                                        QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                    }
                                    else
                                    {
                                        QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                    }
                                    einvoice.QRCode = QRCode;
                                    einvoice.InvoiceNumber = invoicelstitem;
                                    eInvoiceLst.Add(einvoice);
                                }
                            }
                        }
                        // 21 April, 2021 Piyush Limbani (Generate Invoice For Mobile Order)




                    }
                }
            }

            if (isNewOrder || isGenIrn)
            {
                foreach (var item in eInvoiceLst)
                {
                    foreach (var itm in Lstdatainvoice)
                    {
                        if (itm.InvoiceNumber.Contains(item.InvoiceNumber))
                        {
                            itm.IRN = item.IRN;
                            itm.AckDt = item.AckDt;
                            itm.AckNo = item.AckNo;
                            itm.QRCode = item.QRCode;
                        }
                    }
                }
            }

            Lstdatainvoice[0].InvoiceTotal = InvoiceTotal;
            DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);


            // 26 March, 2021 Sonal Gandhi
            //if (!string.IsNullOrEmpty(orderdata[0].TaxNo))
            if (!string.IsNullOrEmpty(orderdata[0].TaxNo) && Convert.ToDateTime(orderdata[0].CreatedOn) > Convert.ToDateTime("03/31/2021"))
            {
                lr.EnableExternalImages = true;
                if (orderdata[0].TaxName == "IGST")
                {
                    ReportParameter[] param = new ReportParameter[1];
                    param[0] = new ReportParameter("QRCode", "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/");
                    lr.SetParameters(param);
                }
                else
                {
                    ReportParameter[] param = new ReportParameter[2];
                    param[0] = new ReportParameter("OrderID", "0");
                    param[1] = new ReportParameter("QRCode", "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/");
                    lr.SetParameters(param);
                }
            }

            ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
            lr.DataSources.Add(MedsheetHeader);
            lr.DataSources.Add(DataRecord);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
            string Pdfpathcreate = Server.MapPath("~/bill/" + name);
            BinaryWriter Writer = null;
            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
            Writer.Write(renderedBytes);
            Writer.Flush();
            Writer.Close();
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
            //For Email Sending
            string PDFName = _orderservice.CheckPDFNameExist(InvoiceID);
            if (PDFName == "" && InvoiceNumber == null)
            {
                bool respose = _orderservice.UpdateOrderpdf(name, InvoiceID);
            }
            //For Email Sending
            return Json(url, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult ViewBillWiseInvoice(string invid, Int64? custid, Int64? uid, string txtfrom, string txtto)
        {
            Session["custid"] = custid;
            Session["uid"] = uid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            if (!string.IsNullOrEmpty(invid))
            {
                List<OrderQtyList> objModel = _orderservice.GetBillWiseInvoiceForOrder(invid);
                return View(objModel);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ViewInvoice(Int64? id, Int64? custid, Int64? uid, string txtfrom, string txtto)
        {
            Session["custid"] = custid;
            Session["uid"] = uid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<OrderQtyList> objModel = _orderservice.GetInvoiceForOrder(Convert.ToInt64(id));
            return View(objModel);
        }

        [HttpGet]
        public ActionResult ViewCreditMemoInvoice(Int64? id, Int64? custid, string txtfrom, string txtto)
        {
            Session["custid"] = custid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<OrderQtyList> objModel = _orderservice.GetCreditMemoInvoiceForOrder(Convert.ToInt64(id));
            return View(objModel);
        }

        [HttpGet]
        public ActionResult ViewCreditMemo(Int64? id, Int64? custid, Int64? uid, string txtfrom, string txtto)
        {
            Session["custid"] = custid;
            Session["uid"] = uid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<OrderQtyList> objModel = _orderservice.GetCreditMemoForOrder(Convert.ToInt64(id));
            return View(objModel);
        }

        [HttpPost]
        public ActionResult CreateMemo(List<OrderQtyList> data)
        {
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                string objModel = _orderservice.CreateCreditMemo(data, UserID);
                return Json(objModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreditMemoPrint(Int64? id, Int64? custid, DateTime? txtfrom, DateTime? txtto)
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            return View();
        }

        public ActionResult ExportExcelInvoice(long OrderID, long? GodownID, long? TransportID)
        {
            if (GodownID == null)
            {
                GodownID = 0;
            }
            if (TransportID == null)
            {
                TransportID = 0;
            }
            var lstInvoice = _orderservice.GetInvoiceForExcel(OrderID, GodownID.Value, TransportID.Value);
            List<ExportToExcelInvoiceWholesale> lst = lstInvoice.Select(x => new ExportToExcelInvoiceWholesale()
            {
                SupplyType = x.SupplyType,
                SubType = x.SubType,
                DocType = x.DocType,
                DocNo = x.DocNo,
                DocDate = x.DocDate,
                Transaction_Type = x.Transaction_Type,
                From_OtherPartyName = x.From_OtherPartyName,
                From_GSTIN = x.From_GSTIN,
                From_Address1 = x.From_Address1,
                From_Address2 = x.From_Address2,
                From_Place = x.From_Place,
                From_PinCode = x.From_PinCode,
                From_State = x.From_State,
                DispatchState = x.DispatchState,
                To_OtherPartyName = x.To_OtherPartyName,
                To_GSTIN = x.To_GSTIN,
                To_Address1 = x.To_Address1,
                To_Address2 = x.To_Address2,
                To_Place = x.To_Place,
                To_PinCode = x.To_PinCode,
                To_State = x.To_State,
                ShipToState = x.ShipToState,
                Product = x.Product,
                Description = x.Description,
                HSN = x.HSN,
                Unit = x.Unit,
                Qty = x.Qty,
                AssessableValue = x.AssessableValue,
                TaxRate = x.TaxRate,
                CGSTAmount = x.CGSTAmount,
                SGSTAmount = x.SGSTAmount,
                IGSTAmount = x.IGSTAmount,
                CESSAmount = x.CESSAmount,
                Cess_Non_Advol_Amount = x.Cess_Non_Advol_Amount,
                //Others = x.Others,
                TCS = x.TCSTaxAmount,
                TotalInvoiceValue = x.TotalInvoiceValue,
                TransMode = x.TransMode,
                DistanceKM = x.DistanceKM,
                TransName = x.TransName,
                TransID = x.TransID,
                TransDocNo = x.TransDocNo,
                TransDate = x.TransDate,
                VehicleNo = x.VehicleNo,
                VehicleType = x.VehicleType
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable1(lst));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "Invoice.xls");
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

        public ActionResult ManageDeliveryChallan()
        {
            ViewBag.Godown = _productservice.GetAllGodownName();
            ViewBag.Tax = _commonservice.GetAllTaxName();
            ViewBag.Product = _orderservice.GetAllProductName();
            return View();
        }

        [HttpPost]
        public ActionResult SaveChallan(ChallanViewModel data)
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
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                        data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.UpdatedOn = DateTime.Now;

                        long respose = _orderservice.SaveChallan(data);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult PrintChallan(long ChallanID, string ChallanNumber)
        {
            ChallanIdvalue = ChallanID;
            LocalReport lr = new LocalReport();
            string path = "";
            var challandata = _orderservice.GetInvoiceForChallanPrint(ChallanID);
            DataTable header = Common.ToDataTable(challandata);
            if (challandata[0].TaxName == "IGST")
            {
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/IGSTInvoice.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/IGSTInvoice.rdlc");
                //}

                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/IGSTInvoice.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/IGSTInvoice.rdlc");
                }


                lr.ReportPath = path;
            }
            else
            {
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/Challan.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/Challan.rdlc");
                //}

                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/Challan.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/Challan.rdlc");
                }


                lr.ReportPath = path;
            }
            ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
            Lstdatainvoice2 = new List<ChallanQtyInvoiceList>();
            int dividerdata = 0;
            int ordergroup = 0;
            List<string> challanNolist = _orderservice.GetListOfChallan(ChallanID);
            for (int i = 1; i <= challandata[0].NoofInvoiceint; i++)
            {
                Lstdata2 = _orderservice.GetInvoiceForChallanItemPrint(ChallanIdvalue, 1, ChallanNumber);
                foreach (var challanlstitem in challanNolist)
                {
                    int rownumber = 1;
                    int totalnumber = 1;
                    var Foodzero = Lstdata2.Where(x => x.ChallanNumber == challanlstitem).OrderBy(y => y.ProductName).ToList();
                    if (Foodzero.Count > 0)
                    {
                        int Foodzero2 = Foodzero.Count;
                        ordergroup++;
                        decimal total = 0;
                        decimal taxtotal = 0;
                        decimal grandtotal = 0;
                        string NumberToWord = "";
                        dividerdata = dividerdata + 1;
                        int FTotalcount = 0;
                        int FTotalrecord = 0;
                        decimal FATotalAmount = 0;
                        decimal FTotalTax = 0;
                        decimal FAGrandTotal = 0;
                        decimal FGrandTotal = 0;
                        string FTaxName = "";
                        string FGrandAmtWord = "";
                        if (Foodzero.Count < 15)
                        {
                            for (int s = Foodzero.Count; s < 15; s++)
                            {
                                Foodzero.Add(new ChallanQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, ChallanNumber = "", TaxName = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, ChallanID = 0, CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
                            }
                        }
                        foreach (var item in Foodzero)
                        {
                            if (item.ProductName == "")
                            {
                                item.RowNumber = rownumber;
                                item.ordergroup = ordergroup;
                                item.Totalcount = FTotalcount;
                                item.Totalrecord = Convert.ToInt32(FTotalrecord);
                                item.ATotalAmount = Math.Round(FATotalAmount, 2);
                                item.TaxName = FTaxName;
                                item.TotalTax = FTotalTax;
                                item.AGrandTotal = FAGrandTotal;
                                item.GrandTotal = FGrandTotal;
                                item.GrandAmtWord = FGrandAmtWord;
                                Lstdatainvoice2.Add(item);
                                rownumber = rownumber + 1;
                            }
                            else
                            {
                                if (rownumber > 15)
                                {
                                    rownumber = 1;
                                    totalnumber = totalnumber + 1;
                                    total = 0;
                                    taxtotal = 0;
                                    grandtotal = 0;
                                    NumberToWord = "";
                                    ordergroup++;
                                }
                                item.ordergroup = ordergroup;
                                total += item.Total;
                                taxtotal += item.TaxAmt;
                                grandtotal += item.FinalTotal;
                                item.ATotalAmount = total;
                                item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                                item.Totalrecord = Lstdata2.Count;

                                if (item.TaxName == "IGST")
                                {
                                    item.TotalTax = taxtotal;
                                }
                                else
                                {
                                    item.TotalTax = taxtotal / 2;
                                }

                                if (i == 1)
                                {
                                    item.InvoiceTitleHeader = "DELIVERY CHALLAN";
                                }
                                else
                                {
                                    item.InvoiceTitleHeader = "";
                                }
                                item.TotalTax = Math.Round(item.TotalTax, 2);
                                item.TotalAmount = item.Total + item.TaxAmt;
                                item.AGrandTotal += grandtotal;
                                item.AGrandTotal = Math.Round(item.AGrandTotal, 2);
                                int number = Convert.ToInt32(item.AGrandTotal);
                                NumberToWord = NumberToWords(number);
                                item.GrandAmtWord = NumberToWord;
                                var Gtotal = Math.Round(grandtotal);
                                item.GrandTotal = Gtotal;
                                item.SaleRateAmount = Math.Round(item.SaleRateAmount, 2);
                                if (item.TaxName == "IGST")
                                {
                                    item.TaxAmt = item.TaxAmt;
                                }
                                else
                                {
                                    item.TaxAmt = item.TaxAmt / 2;
                                }
                                if (item.TaxName == "IGST")
                                {
                                    item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                }
                                item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
                                item.divider = dividerdata;
                                item.RowNumber = rownumber;
                                item.OrdRowNumber = rownumber;
                                item.Totalcount = Foodzero2;
                                if (Foodzero2 > 15 * totalnumber)
                                {
                                    item.Totalcount = Foodzero2 > 15 * totalnumber ? 15 * totalnumber : 15 * totalnumber - Foodzero2;
                                }
                                else
                                {
                                    if (totalnumber == 1)
                                    {
                                        item.Totalcount = Foodzero2;
                                    }
                                    else
                                    {
                                        item.Totalcount = Foodzero2 - (15 * (totalnumber - 1));
                                    }
                                }
                                Lstdatainvoice2.Add(item);
                                if (rownumber == Foodzero2)
                                {
                                    FTotalcount = item.Totalcount;
                                    FTotalrecord = Convert.ToInt32(item.Totalrecord);
                                    FATotalAmount = item.ATotalAmount;
                                    FTaxName = item.TaxName;
                                    FTotalTax = item.TotalTax;
                                    FAGrandTotal = item.AGrandTotal;
                                    FGrandTotal = item.GrandTotal;
                                    FGrandAmtWord = item.GrandAmtWord;
                                }
                                rownumber = rownumber + 1;
                            }
                        }
                    }
                }
            }
            DataTable FoodDT = Common.ToDataTable(Lstdatainvoice2);
            ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
            lr.DataSources.Add(MedsheetHeader);
            lr.DataSources.Add(DataRecord);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

                "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
            string Pdfpathcreate = Server.MapPath("~/Challan/" + name);

            BinaryWriter Writer = null;
            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
            Writer.Write(renderedBytes);
            Writer.Flush();
            Writer.Close();

            //string url = "";
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    url = "http://" + Request.Url.Host + ":6551/Challan/" + name;
            //}
            //else
            //{
            //    url = "http://" + Request.Url.Host + "/Challan/" + name;
            //}


            string url = "";
            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/Challan/" + name;
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
            {
                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/Challan/" + name;
            }
            else
            {
                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/Challan/" + name;
            }

            return Json(url, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchDeliveryChallan(DateTime? txtfrom, DateTime? txtto)
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewChallanList(ChallanListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["txtfrom"] = "";
                Session["txtto"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ChallanListResponse> objModel = _orderservice.GetAllChallanList(model);

            // 24 May, 2021 Sonal Gandhi
            ViewBag.Transport = _commonservice.GetAllTransportName();
            ViewBag.VehicleNo = _commonservice.GetAllTempoNumberList();

            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewChallan(Int64? id, string txtfrom, string txtto)
        {
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<ChallanQtyList> objModel = _orderservice.GetInvoiceForChallan(Convert.ToInt64(id));
            return View(objModel);
        }

        public ActionResult ExportExcelChallan(long ChallanID, string FromAddress1, string FromAddress2, string FromPlace, string FromPinCode, string FromState, string DispatchState, string ToAddress1, string ToAddress2, string ToPlace, string ToPinCode, string ToState, string ShipToState)
        {
            var lstInvoice = _orderservice.GetChallanForExcel(ChallanID);
            List<ExportToExcelChallanWholesale> lst = lstInvoice.Select(x => new ExportToExcelChallanWholesale() { SupplyType = x.SupplyType, SubType = x.SubType, DocType = x.DocType, DocNo = x.DocNo, DocDate = x.DocDate, Transaction_Type = x.Transaction_Type, DeliveryFrom = x.DeliveryFrom, From_GSTIN = x.From_GSTIN, From_Address1 = FromAddress1, From_Address2 = FromAddress2, From_Place = FromPlace, From_PinCode = FromPinCode, From_State = FromState, DispatchState = DispatchState, DeliveryTo = x.DeliveryTo, To_GSTIN = x.To_GSTIN, To_Address1 = ToAddress1, To_Address2 = ToAddress2, To_Place = ToPlace, To_PinCode = ToPinCode, To_State = ToState, ShipToState = ShipToState, Product = x.Product, Description = x.Description, HSN = x.HSN, Unit = x.Unit, Qty = x.Qty, AssessableValue = x.AssessableValue, TaxRate = x.TaxRate, CGSTAmount = x.CGSTAmount, SGSTAmount = x.SGSTAmount, IGSTAmount = x.IGSTAmount, CESSAmount = x.CESSAmount, Cess_Non_Advol_Amount = x.Cess_Non_Advol_Amount, Others = x.Others, ChallanTotal = x.ChallanTotal, TransMode = x.TransMode, DistanceKM = x.DistanceKM, TransName = x.TransName, TransID = x.TransID, TransDocNo = x.TransDocNo, TransDate = x.TransDate, VehicleNo = x.VehicleNo }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable1(lst));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "DeliveryChallan.xls");
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
        public ActionResult DeleteCreditMemo(string CreditMemoNumber, bool IsDelete)
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
                    // 08 April 2021 Piyush Limbani
                    bool respose = false;
                    var detail = _orderservice.GetIRNNumberByCreditMemoNumber(CreditMemoNumber);

                    if (string.IsNullOrEmpty(detail.IRN))
                    {
                        respose = _orderservice.DeleteCreditMemo(CreditMemoNumber, IsDelete);
                    }
                    else
                    {
                        EInvoiceCreditMemoController eInvoice = new EInvoiceCreditMemoController();
                        respose = eInvoice.DeleteEInvoiceCreditMemo(detail.IRN, CreditMemoNumber, IsDelete, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now);
                    }
                    // 08 April 2021 Piyush Limbani

                    // bool respose = _orderservice.DeleteCreditMemo(CreditMemoNumber, IsDelete);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        //public ActionResult GetProductNameByCustomerID(long CustomerID)
        //{
        //    List<ProductList> ProductList = new List<ProductList>();
        //    ProductList = _orderservice.GetProductNameByCustomerID(CustomerID);
        //    return Json(new { lstArea = ProductList }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public PartialViewResult ProductList(long CustomerID)
        {
            List<ProductList> ProductList = new List<ProductList>();
            ProductList = _orderservice.GetProductNameByCustomerID(CustomerID);
            return PartialView(ProductList);
        }

        public ActionResult MobileOrderPrint()
        {
            List<OrderListResponse> objModel = _orderservice.GetAllMobileOrderList();
            return View(objModel);
        }


        //[HttpPost]
        //public ActionResult PrintMobileInvoice(List<OrderIDList> data)
        //{
        //    string url = "";
        //    try
        //    {
        //        if (Request.Cookies["UserID"] == null)
        //        {
        //            Request.Cookies["UserID"].Value = null;
        //            return JavaScript("location.reload(true)");
        //        }
        //        else
        //        {
        //            LocalReport lr = new LocalReport();
        //            string path = "";
        //            //var orderdata = _orderservice.GetAllMobileOrderList();
        //            DataTable header = Common.ToDataTable(data);
        //            //if (Request.Url.Host.Contains("localhost"))
        //            //{
        //            //    path = "Report/SubReportInvoice.rdlc";
        //            //}
        //            //else
        //            //{
        //            //    path = Server.MapPath("~/Report/SubReportInvoice.rdlc");
        //            //}


        //            if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
        //            {
        //                path = "Report/SubReportInvoice.rdlc";
        //            }
        //            else
        //            {
        //                path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SubReportInvoice.rdlc");
        //            }



        //            lr.ReportPath = path;
        //            ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
        //            lr.DataSources.Add(MedsheetHeader);
        //            lr.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessing);
        //            string reportType = "pdf";
        //            string mimeType;
        //            string encoding;
        //            string fileNameExtension;
        //            string deviceInfo =
        //                "<DeviceInfo>" +
        //            "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //            "  <PageWidth>11in</PageWidth>" +
        //            "  <PageHeight>8.5in</PageHeight>" +
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
        //            string name = DateTime.Now.Ticks.ToString() + ".pdf";
        //            string Pdfpathcreate = Server.MapPath("~/bill/" + name);
        //            BinaryWriter Writer = null;
        //            Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
        //            Writer.Write(renderedBytes);
        //            Writer.Flush();
        //            Writer.Close();

        //            //if (Request.Url.Host.Contains("localhost"))
        //            //{
        //            //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
        //            //}
        //            //else
        //            //{
        //            //    url = "http://" + Request.Url.Host + "/bill/" + name;
        //            //}



        //            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
        //            {
        //                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/bill/" + name;
        //            }
        //            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
        //            {
        //                url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
        //            }
        //            else
        //            {
        //                url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/bill/" + name;
        //            }



        //            return Json(url, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}


        [HttpPost]
        public ActionResult PrintMobileInvoice(List<OrderIDList> data, string orderids)
        {
            string url = "";
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    LocalReport lr = new LocalReport();
                    string path = "";

                    //var orderdata = _orderservice.GetAllMobileOrderList();
                    // 06 Jan,2022 Piyush Limbani
                    var orderdata = _orderservice.GetAllMobileOrderListByOrderID(orderids);
                    // 06 Jan,2022 Piyush Limbani
                    var dataorder = orderdata.Where(i => (i.TaxNo == "") && ((i.TaxName == "SGST") || (i.TaxName == "CGST"))).ToList();

                    DataTable header = Common.ToDataTable(dataorder);

                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/SubReportInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/SubReportInvoice.rdlc");
                    }



                    lr.ReportPath = path;
                    ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                    lr.DataSources.Add(MedsheetHeader);
                    lr.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessing);
                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =
                        "<DeviceInfo>" +
                    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>11in</PageWidth>" +
                    "  <PageHeight>8.5in</PageHeight>" +
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
                    string Pdfpathcreate = Server.MapPath("~/bill/" + name);
                    BinaryWriter Writer = null;
                    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                    Writer.Write(renderedBytes);
                    Writer.Flush();
                    Writer.Close();

                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
                    //}
                    //else
                    //{
                    //    url = "http://" + Request.Url.Host + "/bill/" + name;
                    //}



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
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintMobileGSTInvoice(List<OrderIDList> data, string orderids)
        {
            string url = "";
            try
            {
                int invoiceRowNumber = 0;
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    LocalReport lr1 = new LocalReport();
                    string path = "";

                    // var orderdata = _orderservice.GetAllMobileOrderList();
                    // 06 Jan,2022 Piyush Limbani
                    var orderdata = _orderservice.GetAllMobileOrderListByOrderID(orderids);
                    // 06 Jan,2022 Piyush Limbani

                    var dataorder = orderdata.Where(i => (i.TaxNo != "") && ((i.TaxName == "SGST") || (i.TaxName == "CGST"))).ToList();

                    DataTable header = Common.ToDataTable(dataorder);

                    if (Request.Url.Host.Contains("localhost"))
                    {

                        path = "Report/SubReportEInvoice.rdlc";
                    }
                    else
                    {
                        path = Server.MapPath("~/Report/SubReportEInvoice.rdlc");
                    }

                    lr1.ReportPath = path;
                    lr1.EnableExternalImages = true;
                    ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                    lr1.DataSources.Add(MedsheetHeader);

                    lr1.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessing); // Calculating invoice details
                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =
                        "<DeviceInfo>" +
                    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>11in</PageWidth>" +
                    "  <PageHeight>8.5in</PageHeight>" +
                    "  <MarginTop>1cm</MarginTop>" +
                    "  <MarginLeft>1cm</MarginLeft>" +
                    "  <MarginRight>1cm</MarginRight>" +
                    "  <MarginBottom>1cm</MarginBottom>" +
                    "</DeviceInfo>";
                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;
                    renderedBytes = lr1.Render(
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

                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
                    //}
                    //else
                    //{
                    //    url = "http://" + Request.Url.Host + "/bill/" + name;
                    //}



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
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintMobileIGSTInvoice(List<OrderIDList> data, string orderids)
        {
            string url = "";
            try
            {
                int invoiceRowNumber = 0;
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    LocalReport lr = new LocalReport();
                    string path = "";

                    // var orderdata = _orderservice.GetAllMobileOrderList();
                    // 06 Jan,2022 Piyush Limbani
                    var orderdata = _orderservice.GetAllMobileOrderListByOrderID(orderids);
                    // 06 Jan,2022 Piyush Limbani

                    var dataorder = orderdata.Where(i => (i.TaxNo == "") && (i.TaxName == "IGST")).ToList();


                    DataTable header = Common.ToDataTable(dataorder);

                    if (Request.Url.Host.Contains("localhost"))
                    {
                        //path = "Report/SubReportIGSTnvoice_Test.rdlc";
                        path = "Report/SubIGSTInvoice.rdlc";

                    }
                    else
                    {
                        path = Server.MapPath("~/Report/SubReportIGSTInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                    lr.EnableExternalImages = true;
                    ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                    lr.DataSources.Add(MedsheetHeader);
                    lr.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessing); // Calculating invoice details
                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>11in</PageWidth>" +
                    "  <PageHeight>8.5in</PageHeight>" +
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
                    string Pdfpathcreate = Server.MapPath("~/bill/" + name);
                    BinaryWriter Writer = null;
                    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                    Writer.Write(renderedBytes);
                    Writer.Flush();
                    Writer.Close();


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
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintMobileIGSTEInvoice(List<OrderIDList> data, string orderids)
        {
            string url = "";
            try
            {
                int invoiceRowNumber = 0;
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    LocalReport lr = new LocalReport();
                    string path = "";


                    // var orderdata = _orderservice.GetAllMobileOrderList();
                    // 06 Jan,2022 Piyush Limbani
                    var orderdata = _orderservice.GetAllMobileOrderListByOrderID(orderids);
                    // 06 Jan,2022 Piyush Limbani


                    var dataorder = orderdata.Where(i => (i.TaxNo != "") && (i.TaxName == "IGST")).ToList();


                    DataTable header = Common.ToDataTable(dataorder);

                    if (Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/SubReportIGSTEInvoice.rdlc";

                    }
                    else
                    {
                        path = Server.MapPath("~/Report/SubReportIGSTEInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                    lr.EnableExternalImages = true;
                    ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                    lr.DataSources.Add(MedsheetHeader);
                    lr.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessing); // Calculating invoice details
                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =
                        "<DeviceInfo>" +
                    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                    "  <PageWidth>11in</PageWidth>" +
                    "  <PageHeight>8.5in</PageHeight>" +
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
                    string Pdfpathcreate = Server.MapPath("~/bill/" + name);
                    BinaryWriter Writer = null;
                    Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                    Writer.Write(renderedBytes);
                    Writer.Flush();
                    Writer.Close();

                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    url = "http://" + Request.Url.Host + ":6551/bill/" + name;
                    //}
                    //else
                    //{
                    //    url = "http://" + Request.Url.Host + "/bill/" + name;
                    //}



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
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public void SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            // 27 Nov,2021 Sonal Gandhi
            List<string> chkInvoiceNumber = new List<string>();
            string IRN = "";
            long ActNo = 0;
            string ActDate = "";
            string QRCode = "";
            bool isGenIrn = false;
            int invoiceRowNumber = 0;
            // 27 Nov,2021 Sonal Gandhi

            decimal InvoiceTotal = 0;
            long ProductCount = 0;
            string InvoiceNumber = null;
            int OrderID = Convert.ToInt32((e.Parameters["OrderID"].Values[0]));
            var orderdata = _orderservice.GetInvoiceForOrderPrint(OrderID);

            if (string.IsNullOrEmpty(orderdata[0].TaxNo))
            {
                invoiceRowNumber = 15;
            }
            else
            {
                invoiceRowNumber = 10;
            }

            DataTable header = Common.ToDataTable(orderdata);
            ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);

            e.DataSources.Add(MedsheetHeader);
            Lstdatainvoice = new List<OrderQtyInvoiceList>();
            int dividerdata = 0;
            int ordergroup = 0;

            //27 Nov,2021 Sonal Gandhi
            EInvoice einvoice = new EInvoice();
            List<EInvoice> eInvoiceLst = new List<EInvoice>();
            List<OrderQtyInvoiceList> invoiceToEInvoiceLst = new List<OrderQtyInvoiceList>();
            long EInvoiceId = 0;
            //27 Nov,2021 Sonal Gandhi

            List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);
            for (int i = 1; i <= orderdata[0].NoofInvoiceint; i++)
            {
                Lstdata = _orderservice.GetInvoiceForOrderItemPrint(OrderID, 1, InvoiceNumber);

                //27 Nov,2021 Sonal Gandhi
                EInvoiceController eInvoice = new EInvoiceController();
                //27 Nov,2021 Sonal Gandhi

                foreach (var invoicelstitem in incoiveNolist)
                {
                    int rownumber = 1;
                    int totalnumber = 1;
                    var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
                    var Count = Foodzero.Where(x => x.InvoiceNumber != "").ToList();
                    if (Foodzero.Count > 0)
                    {
                        int Foodzero2 = Foodzero.Count;
                        ordergroup++;
                        decimal total = 0;
                        decimal taxtotal = 0;
                        decimal grandtotal = 0;
                        string NumberToWord = "";
                        dividerdata = dividerdata + 1;
                        int FTotalcount = 0;
                        int FTotalrecord = 0;
                        decimal FATotalAmount = 0;
                        decimal FTotalTax = 0;
                        decimal FAGrandTotal = 0;
                        decimal FGrandTotal = 0;
                        string FTaxName = "";
                        string FGrandAmtWord = "";

                        //27 Nov,2021 Sonal Gandhi
                        string invoiceNumber = invoicelstitem;
                        QRCode = "";
                        invoiceToEInvoiceLst = new List<OrderQtyInvoiceList>();
                        //27 Nov,2021 Sonal Gandhi

                        if (Foodzero.Count < invoiceRowNumber)
                        {
                            for (int s = Foodzero.Count; s < invoiceRowNumber; s++)
                            {
                                Foodzero.Add(new OrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", City = "", State = "", StateCode = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
                            }
                        }

                        foreach (var item in Foodzero)
                        {
                            if (item.ProductName == "")
                            {
                                item.RowNumber = rownumber;
                                item.ordergroup = ordergroup;
                                item.Totalcount = FTotalcount;
                                item.Totalrecord = Convert.ToInt32(FTotalrecord);
                                item.ATotalAmount = Math.Round(FATotalAmount, 2);
                                item.TaxName = FTaxName;
                                item.TotalTax = FTotalTax;
                                item.AGrandTotal = FAGrandTotal;
                                item.GrandTotal = FGrandTotal;
                                item.GrandAmtWord = FGrandAmtWord;

                                // 27 Nov, 2021 Sonal Gandhi
                                item.InvoiceNumber = invoiceNumber;
                                //if (!isNewOrder)
                                //{
                                if (IRN != null || IRN != "")
                                    item.IRN = IRN;
                                if (ActNo > 0)
                                    item.AckNo = ActNo;
                                if (ActDate != null || ActDate != "")
                                    item.AckDt = Convert.ToDateTime(ActDate);
                                if (QRCode != null || QRCode != "")
                                {
                                    item.QRCode = QRCode;
                                }
                                //}
                                // 27 Nov, 2021 Sonal Gandhi

                                Lstdatainvoice.Add(item);
                                rownumber = rownumber + 1;
                            }
                            else
                            {
                                if (rownumber > invoiceRowNumber)
                                {
                                    rownumber = 1;
                                    totalnumber = totalnumber + 1;
                                    total = 0;
                                    taxtotal = 0;
                                    grandtotal = 0;
                                    NumberToWord = "";
                                    ordergroup++;
                                }
                                item.ordergroup = ordergroup;
                                total += item.Total;
                                taxtotal += item.TaxAmt;
                                grandtotal += item.FinalTotal;
                                item.ATotalAmount = total;
                                item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                                item.Totalrecord = Lstdata.Count;
                                if (item.TaxName == "IGST")
                                {
                                    item.TotalTax = taxtotal;
                                }
                                else
                                {
                                    item.TotalTax = taxtotal / 2;
                                }
                                if (i == 1)
                                {
                                    item.InvoiceTitleHeader = "ORIGINAL FOR RECIPIENT";
                                }
                                else if (i == 2)
                                {
                                    item.InvoiceTitleHeader = "DUPLICATE FOR TRANSPORTER";
                                }
                                else if (i == 3)
                                {
                                    item.InvoiceTitleHeader = "TRIPLICATE FOR SUPPLIER";
                                }
                                else
                                {
                                    item.InvoiceTitleHeader = "";
                                }
                                if (item.Tax == 0)
                                {
                                    item.InvoiceTitle = "BILL OF SUPPLY";
                                }
                                else
                                {
                                    item.InvoiceTitle = "TAX INVOICE";
                                }
                                item.TotalTax = Math.Round(item.TotalTax, 2);
                                item.TotalAmount = item.Total + item.TaxAmt;
                                item.AGrandTotal += grandtotal;
                                item.AGrandTotal = Math.Round(item.AGrandTotal, 2);
                                int number = Convert.ToInt32(item.AGrandTotal);
                                NumberToWord = NumberToWords(number);
                                item.GrandAmtWord = NumberToWord;
                                var Gtotal = Math.Round(grandtotal);

                                //Old
                                // item.GrandTotal = Gtotal;

                                // New
                                item.GrandTotal = item.InvTotal;

                                ProductCount++;
                                if (Count.Count == ProductCount && item.InvoiceTitleHeader == "ORIGINAL FOR RECIPIENT")
                                {
                                    ProductCount = 0;

                                    //Old
                                    //  InvoiceTotal += Gtotal;

                                    // New
                                    InvoiceTotal += item.InvTotal;
                                }
                                item.InvoiceTotal = InvoiceTotal;
                                if (item.TaxName == "IGST")
                                {
                                    item.TaxAmt = item.TaxAmt;
                                    // 27 Nov, 2021 Sonal Gandhi
                                    item.IGSTAmount = Math.Round(item.TaxAmt, 2);
                                    // 27 Nov, 2021 Sonal Gandhi
                                }
                                else
                                {
                                    item.TaxAmt = item.TaxAmt / 2;
                                    // 27 Nov, 2021 Sonal Gandhi
                                    item.SGSTAmount = Math.Round(item.TaxAmt, 2);
                                    item.CGSTAmount = Math.Round(item.TaxAmt, 2);
                                    // 27 Nov, 2021 Sonal Gandhi
                                }
                                if (item.TaxName == "IGST")
                                {
                                    item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                    // 27 Nov, 2021 Sonal Gandhi
                                    item.IGSTTaxRate = item.Tax;
                                    // 27 Nov, 2021 Sonal Gandhi
                                }
                                else
                                {
                                    item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                    // 27 Nov, 2021 Sonal Gandhi
                                    item.SGSTTaxRate = Math.Round(item.Tax, 2);
                                    item.CGSTTaxRate = Math.Round(item.Tax, 2);
                                    // 27 Nov, 2021 Sonal Gandhi
                                }
                                item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
                                item.divider = dividerdata;
                                item.RowNumber = rownumber;
                                item.OrdRowNumber = rownumber;
                                item.Totalcount = Foodzero2;
                                if (Foodzero2 > 15 * totalnumber)
                                {
                                    item.Totalcount = Foodzero2 > invoiceRowNumber * totalnumber ? invoiceRowNumber * totalnumber : invoiceRowNumber * totalnumber - Foodzero2;
                                }
                                else
                                {
                                    if (totalnumber == 1)
                                    {
                                        item.Totalcount = Foodzero2;
                                    }
                                    else
                                    {
                                        item.Totalcount = Foodzero2 - (invoiceRowNumber * (totalnumber - 1));
                                    }
                                }
                                // 27 Nov, 2021 Sonal Gandhi
                                IRN = item.IRN;
                                ActNo = item.AckNo;
                                ActDate = Convert.ToString(item.AckDt);
                                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                {
                                    item.QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + item.QRCode;
                                }
                                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                {
                                    item.QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                }
                                else
                                {
                                    item.QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + item.QRCode;
                                }
                                // 27 Nov, 2021 Sonal Gandhi

                                Lstdatainvoice.Add(item);

                                // 27 Nov, 2021 Sonal Gandhi
                                invoiceToEInvoiceLst.Add(item);
                                // 27 Nov, 2021 Sonal Gandhi

                                if (rownumber == Foodzero2)
                                {
                                    FTotalcount = item.Totalcount;
                                    FTotalrecord = Convert.ToInt32(item.Totalrecord);
                                    FATotalAmount = item.ATotalAmount;
                                    FTaxName = item.TaxName;
                                    FTotalTax = item.TotalTax;
                                    FAGrandTotal = item.AGrandTotal;
                                    FGrandTotal = item.GrandTotal;
                                    FGrandAmtWord = item.GrandAmtWord;

                                    // 27 Nov, 2021 Sonal Gandhi
                                    invoiceNumber = invoicelstitem;
                                    IRN = item.IRN;
                                    ActNo = item.AckNo;
                                    ActDate = Convert.ToString(item.AckDt);
                                    QRCode = item.QRCode;
                                    // 27 Nov, 2021 Sonal Gandhi
                                }
                                rownumber = rownumber + 1;
                            }
                        }

                        // 29 Nov, 2021 Sonal Gandhi
                        if (!string.IsNullOrEmpty(orderdata[0].TaxNo) && string.IsNullOrEmpty(Lstdata.Select(x => x.IRN).FirstOrDefault()))
                        {
                            if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                            {
                                isGenIrn = true;
                                chkInvoiceNumber.Add(invoicelstitem);
                                einvoice = new EInvoice();
                                einvoice = eInvoice.GenIRN(orderdata[0], invoiceToEInvoiceLst);
                                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                                {
                                    QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/QRCode/" + einvoice.QRCode;
                                }
                                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                                {
                                    QRCode = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                }
                                else
                                {
                                    QRCode = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/QRCode/" + einvoice.QRCode;
                                }
                                einvoice.QRCode = QRCode;
                                einvoice.InvoiceNumber = invoicelstitem;
                                eInvoiceLst.Add(einvoice);
                            }
                        }
                        // 29 Nov, 2021 Sonal Gandhi

                    }
                }
            }

            // 29 Nov, 2021 Sonal Gandhi
            foreach (var item in eInvoiceLst)
            {
                foreach (var itm in Lstdatainvoice)
                {
                    if (itm.InvoiceNumber.Contains(item.InvoiceNumber))
                    {
                        itm.IRN = item.IRN;
                        itm.AckDt = item.AckDt;
                        itm.AckNo = item.AckNo;
                        itm.QRCode = item.QRCode;
                    }
                }
            }
            // 29 Nov, 2021 Sonal Gandhi

            Lstdatainvoice[0].InvoiceTotal = InvoiceTotal;
            DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);
            ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
            e.DataSources.Add(DataRecord);

            bool respose = _orderservice.UpdateRemotePrintStatus(OrderID);

        }


        //public void SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        //{
        //    decimal InvoiceTotal = 0;
        //    long ProductCount = 0;
        //    string InvoiceNumber = null;
        //    int OrderID = Convert.ToInt32((e.Parameters["OrderID"].Values[0]));
        //    var orderdata = _orderservice.GetInvoiceForOrderPrint(OrderID);
        //    DataTable header = Common.ToDataTable(orderdata);

        //    ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
        //    e.DataSources.Add(MedsheetHeader);
        //    Lstdatainvoice = new List<OrderQtyInvoiceList>();
        //    int dividerdata = 0;
        //    int ordergroup = 0;

        //    List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);
        //    for (int i = 1; i <= orderdata[0].NoofInvoiceint; i++)
        //    {
        //        Lstdata = _orderservice.GetInvoiceForOrderItemPrint(OrderID, 1, InvoiceNumber);
        //        foreach (var invoicelstitem in incoiveNolist)
        //        {
        //            int rownumber = 1;
        //            int totalnumber = 1;
        //            var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
        //            var Count = Foodzero.Where(x => x.InvoiceNumber != "").ToList();
        //            if (Foodzero.Count > 0)
        //            {
        //                int Foodzero2 = Foodzero.Count;
        //                ordergroup++;
        //                decimal total = 0;
        //                decimal taxtotal = 0;
        //                decimal grandtotal = 0;
        //                string NumberToWord = "";
        //                dividerdata = dividerdata + 1;
        //                int FTotalcount = 0;
        //                int FTotalrecord = 0;
        //                decimal FATotalAmount = 0;
        //                decimal FTotalTax = 0;
        //                decimal FAGrandTotal = 0;
        //                decimal FGrandTotal = 0;
        //                string FTaxName = "";
        //                string FGrandAmtWord = "";
        //                if (Foodzero.Count < 15)
        //                {
        //                    for (int s = Foodzero.Count; s < 15; s++)
        //                    {
        //                        Foodzero.Add(new OrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", City = "", State = "", StateCode = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
        //                    }
        //                }

        //                foreach (var item in Foodzero)
        //                {
        //                    if (item.ProductName == "")
        //                    {
        //                        item.RowNumber = rownumber;
        //                        item.ordergroup = ordergroup;
        //                        item.Totalcount = FTotalcount;
        //                        item.Totalrecord = Convert.ToInt32(FTotalrecord);
        //                        item.ATotalAmount = Math.Round(FATotalAmount, 2);
        //                        item.TaxName = FTaxName;
        //                        item.TotalTax = FTotalTax;
        //                        item.AGrandTotal = FAGrandTotal;
        //                        item.GrandTotal = FGrandTotal;
        //                        item.GrandAmtWord = FGrandAmtWord;
        //                        Lstdatainvoice.Add(item);
        //                        rownumber = rownumber + 1;
        //                    }
        //                    else
        //                    {
        //                        if (rownumber > 15)
        //                        {
        //                            rownumber = 1;
        //                            totalnumber = totalnumber + 1;
        //                            total = 0;
        //                            taxtotal = 0;
        //                            grandtotal = 0;
        //                            NumberToWord = "";
        //                            ordergroup++;
        //                        }
        //                        item.ordergroup = ordergroup;
        //                        total += item.Total;
        //                        taxtotal += item.TaxAmt;
        //                        grandtotal += item.FinalTotal;
        //                        item.ATotalAmount = total;
        //                        item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
        //                        item.Totalrecord = Lstdata.Count;
        //                        if (item.TaxName == "IGST")
        //                        {
        //                            item.TotalTax = taxtotal;
        //                        }
        //                        else
        //                        {
        //                            item.TotalTax = taxtotal / 2;
        //                        }
        //                        if (i == 1)
        //                        {
        //                            item.InvoiceTitleHeader = "ORIGINAL FOR RECIPIENT";
        //                        }
        //                        else if (i == 2)
        //                        {
        //                            item.InvoiceTitleHeader = "DUPLICATE FOR TRANSPORTER";
        //                        }
        //                        else if (i == 3)
        //                        {
        //                            item.InvoiceTitleHeader = "TRIPLICATE FOR SUPPLIER";
        //                        }
        //                        else
        //                        {
        //                            item.InvoiceTitleHeader = "";
        //                        }
        //                        if (item.Tax == 0)
        //                        {
        //                            item.InvoiceTitle = "BILL OF SUPPLY";
        //                        }
        //                        else
        //                        {
        //                            item.InvoiceTitle = "TAX INVOICE";
        //                        }
        //                        item.TotalTax = Math.Round(item.TotalTax, 2);
        //                        item.TotalAmount = item.Total + item.TaxAmt;
        //                        item.AGrandTotal += grandtotal;
        //                        item.AGrandTotal = Math.Round(item.AGrandTotal, 2);
        //                        int number = Convert.ToInt32(item.AGrandTotal);
        //                        NumberToWord = NumberToWords(number);
        //                        item.GrandAmtWord = NumberToWord;
        //                        var Gtotal = Math.Round(grandtotal);

        //                        //Old
        //                        // item.GrandTotal = Gtotal;

        //                        // New
        //                        item.GrandTotal = item.InvTotal;

        //                        ProductCount++;
        //                        if (Count.Count == ProductCount && item.InvoiceTitleHeader == "ORIGINAL FOR RECIPIENT")
        //                        {
        //                            ProductCount = 0;

        //                            //Old
        //                            //  InvoiceTotal += Gtotal;

        //                            // New
        //                            InvoiceTotal += item.InvTotal;
        //                        }
        //                        item.InvoiceTotal = InvoiceTotal;
        //                        if (item.TaxName == "IGST")
        //                        {
        //                            item.TaxAmt = item.TaxAmt;
        //                        }
        //                        else
        //                        {
        //                            item.TaxAmt = item.TaxAmt / 2;
        //                        }
        //                        if (item.TaxName == "IGST")
        //                        {
        //                            item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
        //                        }
        //                        else
        //                        {
        //                            item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
        //                        }
        //                        item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
        //                        item.divider = dividerdata;
        //                        item.RowNumber = rownumber;
        //                        item.OrdRowNumber = rownumber;
        //                        item.Totalcount = Foodzero2;
        //                        if (Foodzero2 > 15 * totalnumber)
        //                        {
        //                            item.Totalcount = Foodzero2 > 15 * totalnumber ? 15 * totalnumber : 15 * totalnumber - Foodzero2;
        //                        }
        //                        else
        //                        {
        //                            if (totalnumber == 1)
        //                            {
        //                                item.Totalcount = Foodzero2;
        //                            }
        //                            else
        //                            {
        //                                item.Totalcount = Foodzero2 - (15 * (totalnumber - 1));
        //                            }
        //                        }
        //                        Lstdatainvoice.Add(item);
        //                        if (rownumber == Foodzero2)
        //                        {
        //                            FTotalcount = item.Totalcount;
        //                            FTotalrecord = Convert.ToInt32(item.Totalrecord);
        //                            FATotalAmount = item.ATotalAmount;
        //                            FTaxName = item.TaxName;
        //                            FTotalTax = item.TotalTax;
        //                            FAGrandTotal = item.AGrandTotal;
        //                            FGrandTotal = item.GrandTotal;
        //                            FGrandAmtWord = item.GrandAmtWord;
        //                        }
        //                        rownumber = rownumber + 1;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    Lstdatainvoice[0].InvoiceTotal = InvoiceTotal;
        //    DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);
        //    ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);
        //    e.DataSources.Add(DataRecord);

        //    bool respose = _orderservice.UpdateRemotePrintStatus(OrderID);

        //}


        [HttpPost]
        public ActionResult UpdateEWayNumber(long OrderID, string InvoiceNumber, string EWayNumber)
        {
            try
            {
                bool respose = _orderservice.UpdateEWayNumberByOrderIDandInvoiceNumber(OrderID, InvoiceNumber, EWayNumber);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTransportDetailByTransportID(long TransportID)
        {
            var detail = _commonservice.GetTransportDetailByTransportID(TransportID);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateDocateDetail(long OrderID, string InvoiceNumber, long TransportID, string DocketNo, DateTime DocketDate)
        {
            try
            {
                bool respose = _orderservice.UpdateDocateDetailByOrderIDandInvoiceNumber(OrderID, InvoiceNumber, TransportID, DocketNo, DocketDate);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTransportDetailByInvoiceNnumberandOrderID(long OrderID, string InvoiceNumber)
        {
            var detail = _orderservice.GetTransportDetailByInvoiceNnumberandOrderID(OrderID, InvoiceNumber);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }





        public ActionResult SearchChallanNoWiseChallan(DateTime? txtfrom, DateTime? txtto)
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewChallanNoWiseChallanList(ChallanListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["txtfrom"] = "";
                Session["txtto"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ChallanListResponse> objModel = _orderservice.GetAllChallanNoWiseChallanList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewChallanNoWiseChallan(string ChallanNumber, string txtfrom, string txtto)
        {
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            if (!string.IsNullOrEmpty(ChallanNumber))
            {
                List<ChallanQtyList> objModel = _orderservice.GetChallanNoWiseChallanForChallan(ChallanNumber);
                return View(objModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateEWayNumberForChallan(long ChallanID, string ChallanNumber, string EWayNumber)
        {
            try
            {
                bool respose = _orderservice.UpdateEWayNumberForChallanByChallanNumber(ChallanID, ChallanNumber, EWayNumber);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        // 04 April 2021 Piyush Limbani Deleted E-Invoice
        public ActionResult DeleteEInvoice(long OrderID, string InvoiceNumber)
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
                    var detail = _orderservice.GetIRNNumberByInvoiceNumber(OrderID, InvoiceNumber);
                    EInvoiceController eInvoice = new EInvoiceController();
                    bool respose = eInvoice.DeleteEInvoice(detail.IRN, OrderID, InvoiceNumber, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        // 14 April 2021 Piyush Limbani  E-Invoice Error Report
        public ActionResult EInvoiceError()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult EInvoiceErrorList(DateTime Date)
        {
            List<EInvoiceErrorListResponse> obj = _orderservice.GetEInvoiceErrorList(Date);
            return PartialView(obj);
        }

        public ActionResult ExportExcelEInvoiceErrorList(DateTime Date)
        {
            var lstEInvoiceError = _orderservice.GetEInvoiceErrorList(Date);
            List<ExportEInvoiceErrorList> lst = lstEInvoiceError.Select(x => new ExportEInvoiceErrorList()
            {
                CustomerName = x.CustomerName,
                InvoiceNumber = x.InvoiceNumber,
                ErrorCodes = x.ErrorCodes,
                ErrorDesc = x.ErrorDesc
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable1(lst));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "EInvoiceErrorList.xls");
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



        // 07 May 2021 Sonal Gandhi

        public ActionResult GenerateEWB(long OrderID, long? GodownID, long? TransportID, long? VehicleDetailID)
        {
            string result = string.Empty;
            long userId = Convert.ToInt64(Request.Cookies["UserID"].Value);
            if (GodownID == null)
            {
                GodownID = 0;
            }
            if (TransportID == null)
            {
                TransportID = 0;
            }
            if (VehicleDetailID == null)
            {
                VehicleDetailID = 0;
            }
            long EWBId = 0;

            List<DetailsForEWB> lstInvoice = _orderservice.GetDetailsForEWB(OrderID, GodownID.Value, TransportID.Value, VehicleDetailID.Value);
            EWayBillController eWayBill = new EWayBillController();
            List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);

            foreach (var invoiceLstItem in incoiveNolist)
            {
                EWBId = _orderservice.CheckEWayBillExist(OrderID, invoiceLstItem);
                if (EWBId == 0)
                {
                    DetailsForEWB data = lstInvoice.Where(x => x.InvoiceNumber == invoiceLstItem).FirstOrDefault();
                    EWayBill response = eWayBill.GenerateEWB(data, OrderID, invoiceLstItem, userId);
                    if (!string.IsNullOrEmpty(response.EWayBillNumber.ToString()))
                        result = response.InvoiceNumber + ", " + result;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GenerateEWBChallan(long ChallanID, long? TransportID, long? VehicleDetailID)
        {
            string result = string.Empty;
            long userId = Convert.ToInt64(Request.Cookies["UserID"].Value);
            if (TransportID == null)
            {
                TransportID = 0;
            }
            if (VehicleDetailID == null)
            {
                VehicleDetailID = 0;
            }
            long EWBId = 0;

            List<ChallanDetailForEWB> lstChallanDetails = _orderservice.GetChallanDetailForEWB(ChallanID, TransportID.Value, VehicleDetailID.Value);

            EWayBillChallanController eWayBill = new EWayBillChallanController();
            List<string> challanNolist = _orderservice.GetListOfChallan(ChallanID);

            foreach (var challanNumber in challanNolist)
            {
                EWBId = _orderservice.CheckEWayBillChallanExist(ChallanID, challanNumber);
                if (EWBId == 0)
                {
                    List<ChallanItemForEWB> lstChallanItems = _orderservice.GetChallanItemForEWB(ChallanID, challanNumber);
                    EWayBillChallan response = eWayBill.GenerateEWB(lstChallanDetails[0], lstChallanItems, userId);
                    if (!string.IsNullOrEmpty(response.EWayBillNumber.ToString()))
                        result = result + "  " + response.ChallanNumber;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // 2 July,2021 Sonal Gandhi
        [HttpGet]
        public ActionResult OnlineViewInvoice(Int64? id, Int64? custid, Int64? uid, string txtfrom, string txtto)
        {
            Session["custid"] = custid;
            Session["uid"] = uid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<OrderQtyList> objModel = _orderservice.GetInvoiceForOrder(Convert.ToInt64(id));
            return View(objModel);
        }




        //public ActionResult GenerateEWB(long OrderID, long? GodownID, long? TransportID, long? VehicleDetailID)
        //{
        //    long userId = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //    if (GodownID == null)
        //    {
        //        GodownID = 0;
        //    }
        //    if (TransportID == null)
        //    {
        //        TransportID = 0;
        //    }
        //    if (VehicleDetailID == null)
        //    {
        //        VehicleDetailID = 0;
        //    }
        //    long EWBId = 0;

        //    var lstInvoice = _orderservice.GetInvoiceForExcel(OrderID, GodownID.Value, TransportID.Value, VehicleDetailID.Value);

        //    EWayBillController eWayBill = new EWayBillController();
        //    List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);
        //    List<ExportToExcelInvoiceWholesale> lst = lstInvoice.Select(x => new ExportToExcelInvoiceWholesale()
        //    {
        //        SupplyType = x.SupplyType,
        //        SubType = x.SubType,
        //        DocType = x.DocType,
        //        DocNo = x.DocNo,
        //        DocDate = x.DocDate,
        //        Transaction_Type = x.Transaction_Type,
        //        From_OtherPartyName = x.From_OtherPartyName,
        //        From_GSTIN = x.From_GSTIN,
        //        From_Address1 = x.From_Address1,
        //        From_Address2 = x.From_Address2,
        //        From_Place = x.From_Place,
        //        From_PinCode = x.From_PinCode,
        //        From_State = x.From_State,
        //        DispatchState = x.DispatchState,
        //        To_OtherPartyName = x.To_OtherPartyName,
        //        To_GSTIN = x.To_GSTIN,
        //        To_Address1 = x.To_Address1,
        //        To_Address2 = x.To_Address2,
        //        To_Place = x.To_Place,
        //        To_PinCode = x.To_PinCode,
        //        To_State = x.To_State,
        //        ShipToState = x.ShipToState,
        //        Product = x.Product,
        //        Description = x.Description,
        //        HSN = x.HSN,
        //        Unit = x.Unit,
        //        Qty = x.Qty,
        //        AssessableValue = x.AssessableValue,
        //        TaxRate = x.TaxRate,
        //        CGSTAmount = x.CGSTAmount,
        //        SGSTAmount = x.SGSTAmount,
        //        IGSTAmount = x.IGSTAmount,
        //        CESSAmount = x.CESSAmount,
        //        Cess_Non_Advol_Amount = x.Cess_Non_Advol_Amount,
        //        //Others = x.Others,
        //        TCS = x.TCSTaxAmount,
        //        TotalInvoiceValue = x.TotalInvoiceValue,
        //        TransMode = x.TransMode,
        //        DistanceKM = x.DistanceKM,
        //        TransName = x.TransName,
        //        TransID = x.TransID,
        //        TransDocNo = x.TransDocNo,
        //        TransDate = x.TransDate,
        //        VehicleNo = x.VehicleNo,
        //        VehicleType = x.VehicleType,

        //        //28th April,2021 Sonal Gandhi
        //        IRN = x.IRN,
        //        InvoiceNumber = x.InvoiceNumber
        //    }).ToList();
        //    foreach (var invoiceLstItem in incoiveNolist)
        //    {
        //        EWBId = _orderservice.CheckEWayBillExist(OrderID, invoiceLstItem);
        //        if (EWBId == 0)
        //        {
        //            var data = lst.Where(x => x.InvoiceNumber == invoiceLstItem).ToList();
        //            EWayBill response = eWayBill.GenerateEWB(data, OrderID, invoiceLstItem, userId);
        //        }
        //    }
        //    return View();
        //}




    }

    public class GetSellPrice
    {
        public decimal SellPrice { get; set; }
        public decimal Tax { get; set; }
    }


}