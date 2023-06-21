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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;
using ClosedXML.Excel;
using Ean13Barcode;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using vb.Data.ViewModel;

namespace vbwebsite.Areas.retail.Controllers
{
    public class OrderController : Controller
    {
        private static object Lock = new object();
        private IRetOrderService _orderservice;
        private ICommonService _commonservice;
        private IRetProductService _productservice;
        public long OrderIdvalue;
        public long ChallanIdvalue;


        public string OrderIdstr;


        List<RetOrderQtyInvoiceList> Lstdata;
        List<RetOrderQtyInvoiceList> Lstdatainvoice;
        List<RetChallanQtyInvoiceList> Lstdata2;
        List<RetChallanQtyInvoiceList> Lstdatainvoice2;

        public OrderController(IRetOrderService orderservice, ICommonService commonservice, IRetProductService productservice)
        {
            _orderservice = orderservice;
            _commonservice = commonservice;
            _productservice = productservice;
        }

        // GET: /retail/Order/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageOrders(Int64? id)
        {
            ViewBag.Product = _orderservice.GetAllRetProductName();

            try
            {
                RetOrderViewModel objModel = _orderservice.GetRetailOrderDetailsByOrderID(Convert.ToInt64(id));
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
        public ActionResult GetTaxForRetCustomer(long CustomerID)
        {
            var Tax = _orderservice.GetTaxForRetCustomer(CustomerID);
            return Json(Tax, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSection(List<RetOrderPackList> data)
        {
            try
            {
                bool respose = _orderservice.UpdateSection(data);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateSummarySection(List<RetOrderSummaryListResponse> data)
        {
            try
            {
                bool respose = _orderservice.UpdateSummarySection(data);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddOrder(RetOrderViewModel data)
        {
            if (data.OrderID == 0)
            {
                var lstdata = _orderservice.GetLastRetailOrderID();
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
                data.InvoiceNumber = OrderID;
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
                    data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.CreatedOn = DateTime.Now;
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    string respose = _orderservice.AddOrder(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FinaliseOrder(RetOrderViewModel data)
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
                        if (data.OrderID == 0)
                        {
                            var lstdata = _orderservice.GetLastRetailOrderID();
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

                            data.InvoiceNumber = OrderID;
                        }
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                        data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.UpdatedOn = DateTime.Now;

                        string respose = _orderservice.FinaliseOrder(data);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult GetAutoCompleteProduct(long ProductID)
        {
            var lstUnit = _orderservice.GetAutoCompleteRetProduct(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRetailOrderDetails(long ProductQtyID, string Tax, long CustomerID, long CustomerGroupID)
        {
            if (Tax != "")
            {
                var SellPrice = _orderservice.GetRetailOrderDetails(ProductQtyID, Tax, CustomerID, CustomerGroupID);
                return Json(SellPrice, JsonRequestBehavior.AllowGet);
            }
            else
            {
                GetRetSellPrice objdata = new GetRetSellPrice();
                objdata.SellPrice = 0;
                objdata.Tax = 0;
                objdata.CustomerGroupID = 0;
                return Json(objdata, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchViewOrderList(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto)
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _orderservice.GetAllRetCustomerName();
            return View();
        }

        public ActionResult SearchViewBillWiseOrderList(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto)
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _orderservice.GetAllRetCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewOrderList(RetOrderListResponse model)
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
            ViewBag.Godown = _productservice.GetAllRetGodownName();
            ViewBag.Transport = _commonservice.GetAllRetTransportName();

            // 29th April, 2021 Sonal Gandhi
            ViewBag.VehicleNo = _commonservice.GetAllTempoNumberList();

            List<RetOrderListResponse> objModel = _orderservice.GetAllOrderList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseOrderList(RetOrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["rcustid"] = "";
                Session["ruid"] = "";
                Session["rtxtfrom"] = "";
                Session["rtxtto"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }

            ViewBag.TransportName = _commonservice.GetAllRetTransportName();

            List<RetOrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewInvoice(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto)
        {
            Session["custid"] = custid;
            Session["uid"] = uid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<RetOrderQtyList> objModel = _orderservice.GetInvoiceForOrder(Convert.ToInt64(id));
            return View(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoice(string invoicenumber, Int64? custid, Int64? uid, string txtfrom, string txtto, long orderid = 0)
        {
            Session["rcustid"] = custid;
            Session["ruid"] = uid;
            Session["rtxtfrom"] = txtfrom;
            Session["rtxtto"] = txtto;
            if (!string.IsNullOrEmpty(invoicenumber))
            {
                List<RetOrderQtyList> objModel = _orderservice.GetBillWiseInvoiceForOrder(invoicenumber, orderid);
                return View(objModel);
            }
            else
            {
                return View();
            }
        }

        public ActionResult SearchViewBillWiseCreditMemoList(Int64? id, Int64? custid, Int64? uid, DateTime? txtfrom, DateTime? txtto, Int64? pid)
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _orderservice.GetAllRetCustomerName();
            ViewBag.Product = _orderservice.GetAllRetProductName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseCreditMemoList(ClsRetReturnOrderListResponse model)
        {
            if (model.Isclear == true)
            {
                Session["rccustid"] = "";
                Session["rcuid"] = "";
                Session["rctxtfrom"] = "";
                Session["rctxtto"] = "";
                Session["rcpid"] = "";
            }
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ClsRetReturnOrderListResponse> objModel = _orderservice.GetBillWiseCreditMemoForOrder(model);
            var results = from p in objModel
                          group p.CustomerName by p.InvoiceNumber into g
                          select new { InvoiceNumber = g.Key, Cars = g.ToList() };
            List<RetReturnOrderListResponse> objdata = new List<RetReturnOrderListResponse>();
            foreach (var item in results)
            {
                RetReturnOrderListResponse obj = new RetReturnOrderListResponse();
                var data = objModel.Where(x => x.InvoiceNumber == item.InvoiceNumber).FirstOrDefault();
                obj.AreaName = data.AreaName;
                obj.CustomerName = data.CustomerName;
                obj.OrderDate = data.OrderDate;
                obj.PODate = data.PODate;
                obj.InvoiceNumber = data.InvoiceNumber;
                obj.OrderID = data.OrderID;
                obj.UserName = data.UserName;
                List<RetOrderQtyList> lstdatanew = new List<RetOrderQtyList>();
                lstdatanew = objModel.Where(x => x.InvoiceNumber == obj.InvoiceNumber).Select(x => new RetOrderQtyList() { ProductID = x.ProductID, OrderQtyID = x.OrderQtyID, OrderID = x.OrderID, CategoryTypeID = x.CategoryTypeID, ReturnedQuantity = x.ReturnedQuantity, SerialNumber = x.SerialNumber, ProductName = x.ProductName, Quantity = x.Quantity, ProductPrice = x.ProductPrice, DiscountPrice = x.DiscountPrice, TotalAmount = x.TotalAmount, Tax = x.Tax, TaxAmt = x.TaxAmount, FinalTotal = x.FinalTotal, CustomerID = x.CustomerID, DiscountPer = x.DiscountPer, ProductMRP = x.ProductMRP, OrderDate = x.OrderDate, ProductQtyID = x.ProductQtyID }).ToList();
                obj.lstOrderQty = new List<RetOrderQtyList>();
                obj.lstOrderQty = lstdatanew;
                objdata.Add(obj);
            }
            return PartialView(objdata);
        }

        [HttpGet]
        public ActionResult ViewCreditMemo(Int64? id, Int64? custid, Int64? uid, string txtfrom, string txtto)
        {
            Session["custid"] = custid;
            Session["uid"] = uid;
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<RetOrderQtyList> objModel = _orderservice.GetCreditMemoForOrder(Convert.ToInt64(id));
            return View(objModel);
        }

        [HttpPost]
        public ActionResult PrintInvoice(long InvoiceID, string InvoiceNumber, bool isNewOrder = false)
        {
            //isNewOrder = true;
            OrderIdvalue = InvoiceID;
            LocalReport lr = new LocalReport();
            string path = "";

            //27 March,2021 Sonal Gandhi
            List<string> chkInvoiceNumber = new List<string>();

            //25 March,2021 Sonal Gandhi
            string IRN = "";
            long ActNo = 0;
            string ActDate = "";
            string QRCode = "";

            // 27 April,2021 Sonal Gandhi
            bool isGenIrn = false;
            // 27 April,2021 Sonal Gandhi

            int invoiceRowNumber = 0;

            List<RetOrderQtyInvoiceList> orderdata = _orderservice.GetInvoiceForOrderPrint(InvoiceID);
            DataTable header = Common.ToDataTable(orderdata);
            //if (string.IsNullOrEmpty(orderdata[0].TaxNo))
            if (string.IsNullOrEmpty(orderdata[0].TaxNo) || Convert.ToDateTime(orderdata[0].CreatedOn) < Convert.ToDateTime("03/31/2021"))
            {
                invoiceRowNumber = 30;
                if (orderdata[0].TaxName == "IGST")
                {
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/RetailIGSTInvoice.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/RetailIGSTInvoice.rdlc");
                    //}

                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/RetailIGSTInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailIGSTInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                }
                else
                {
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/RetailInvoice.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/RetailInvoice.rdlc");
                    //}


                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/RetailInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                }
            }
            else
            {
                invoiceRowNumber = 25;
                if (orderdata[0].TaxName == "IGST")
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_RetailIGSTInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_RetailIGSTInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_RetailInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_RetailInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                }
            }

            bool respose = _orderservice.UpdatePrintStatus(InvoiceID);

            ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
            Lstdatainvoice = new List<RetOrderQtyInvoiceList>();

            int dividerdata = 0;

            //27 March,2021 Sonal Gandhi
            RetEInvoice einvoice = new RetEInvoice();
            List<RetEInvoice> eInvoiceLst = new List<RetEInvoice>();
            List<RetOrderQtyInvoiceList> invoiceToEInvoiceLst = new List<RetOrderQtyInvoiceList>();

            int noofinvoice = orderdata[0].NoofInvoiceint;
            List<string> incoiveNolist = _orderservice.GetListOfInvoice(InvoiceID);
            int ordergroup = 0;
            for (int i = 1; i <= orderdata[0].NoofInvoiceint; i++)
            {
                Lstdata = _orderservice.GetInvoiceForOrderItemPrint(OrderIdvalue, InvoiceNumber);

                EInvoiceController eInvoice = new EInvoiceController();

                foreach (var invoicelstitem in incoiveNolist)
                {
                    int rownumber = 1;
                    int totalnumber = 1;
                    var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
                    if (Foodzero.Count > 0)
                    {
                        int Foodzero2 = Foodzero.Count;
                        ordergroup++;
                        decimal total = 0;
                        decimal taxtotal = 0;
                        decimal grandtotal = 0;
                        decimal totalMRP = 0;
                        string NumberToWord = "";
                        decimal totalQty = 0;
                        int FTotalcount = 0;
                        int FTotalrecord = 0;
                        int FTotalQuantity = 0;
                        decimal FATotalAmount = 0;
                        decimal FTotalTax = 0;
                        decimal FAGrandTotal = 0;
                        decimal FGrandTotal = 0;


                        // 28 Sep 2020
                        decimal TCSTaxAmount = 0;

                        string FTaxName = "";
                        string FGrandAmtWord = "";
                        QRCode = "";

                        //27 March,2021 Sonal Gandhi

                        //02 March 2023 Dhruvik  
                        decimal margin = 0;
                        decimal spdiscount = 0;
                        //02 March 2023 Dhruvik

                        string invoiceNumber = invoicelstitem;
                        invoiceToEInvoiceLst = new List<RetOrderQtyInvoiceList>();


                        if (Foodzero.Count < invoiceRowNumber)
                        {
                            for (int s = Foodzero.Count; s < invoiceRowNumber; s++)
                            {
                                Foodzero.Add(new RetOrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", OrderDate = Convert.ToDateTime(DateTime.Now), TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, DiscountPrice = 0, BillDiscount = 0, ProductMRP = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, TaxAmount = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", PONumber = "", City = "", State = "", StateCode = "", TotalMRP = 0, TotalQuantity = 0, InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
                            }
                        }

                        foreach (var item in Foodzero)
                        {
                            //28 March,2021 Sonal Gandhi
                            item.InvoiceNumber = invoiceNumber;
                            if (item.ProductName == "")
                            {
                                item.RowNumber = rownumber;
                                item.ordergroup = ordergroup;
                                item.Totalcount = FTotalcount;
                                item.Totalrecord = Convert.ToInt32(FTotalrecord);
                                item.TotalQuantity = Convert.ToInt32(FTotalQuantity);
                                item.ATotalAmount = FATotalAmount;
                                item.TaxName = FTaxName;
                                item.TotalTax = FTotalTax;
                                item.AGrandTotal = FAGrandTotal;
                                item.GrandTotal = FGrandTotal;
                                // 28 Sep 2020
                                item.TCSTaxAmount = TCSTaxAmount;
                                item.GrandAmtWord = FGrandAmtWord;

                                //25 March,2021 Sonal Gandhi
                                //item.InvoiceNumber = invoiceNumber;
                                if (!isNewOrder)
                                {
                                    if (IRN != null || IRN != "")
                                        item.IRN = IRN;
                                    if (ActNo > 0)
                                        item.AckNo = ActNo;
                                    if (ActDate != null || ActDate != "")
                                        item.AckDt = Convert.ToDateTime(ActDate);
                                    if (QRCode != null || QRCode != "")
                                        item.QRCode = QRCode;
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
                                    totalMRP = 0;
                                    NumberToWord = "";
                                    totalQty = 0;
                                    margin = 0;
                                    spdiscount = 0;
                                    ordergroup++;
                                }
                                totalQty += item.Quantity;
                                item.TotalQuantity = Convert.ToInt32(totalQty);
                                item.ordergroup = ordergroup;
                                totalMRP += item.ProductMRP;
                                item.TotalMRP = totalMRP;
                                item.TotalMRP = Math.Round(item.TotalMRP, 2);
                                item.Quantity = Math.Round(item.Quantity);
                                total += item.TotalAmount;
                                taxtotal += item.TaxAmount;
                                item.TotalAmount = item.ProductPrice * item.Quantity;
                                item.ProductPrice = item.ProductPrice;

                                grandtotal += item.TotalAmount + item.TaxAmount;

                                item.ATotalAmount = total;
                                item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                                item.Totalrecord = Lstdata.Count;
                                item.FinalTotal = item.TotalAmount + item.TaxAmount;
                                item.FinalTotal = Math.Round(item.FinalTotal, 2);
                                item.AGrandTotal += grandtotal;
                                item.AGrandTotal = Math.Round(item.AGrandTotal, 2);

                                margin += Math.Round(item.Margin, 2);
                                spdiscount += Math.Round(item.SPDiscount, 2);

                                if (item.TaxName == "IGST")
                                {
                                    item.TotalTax = taxtotal;
                                    item.TotalTax = Math.Round(item.TotalTax, 2);
                                }
                                else
                                {
                                    item.TotalTax = taxtotal / 2;
                                    item.TotalTax = Math.Round(item.TotalTax, 2);
                                }


                                //int number = Convert.ToInt32(item.AGrandTotal);
                                //NumberToWord = NumberToWords(number);


                                int number = Convert.ToInt32(item.InvTotal);
                                NumberToWord = NumberToWords(number);


                                item.GrandAmtWord = NumberToWord;
                                var Gtotal = Math.Round(grandtotal);

                                // item.GrandTotal = Gtotal;

                                item.GrandTotal = item.InvTotal;
                                //item.GrandTotal = item.InvTotal;

                                // 28 Sep 2020
                                item.TCSTaxAmount = item.TCSTaxAmount;

                                if (item.TaxName == "IGST")
                                {
                                    item.TaxAmount = item.TaxAmount;
                                    item.IGSTAmount = Math.Round(item.TaxAmount, 2);
                                }
                                else
                                {
                                    item.TaxAmount = item.TaxAmount / 2;
                                    item.SGSTAmount = Math.Round(item.TaxAmount, 2);
                                    item.CGSTAmount = Math.Round(item.TaxAmount, 2);
                                }


                                item.TaxAmount = Math.Round(item.TaxAmount, 2);
                                if (item.TaxName == "IGST")
                                {
                                    item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                    item.IGSTTaxRate = Math.Round(item.Tax, 2);
                                }
                                else
                                {
                                    item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                    item.SGSTTaxRate = Math.Round(item.Tax, 2);
                                    item.CGSTTaxRate = Math.Round(item.Tax, 2);
                                }
                                item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
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
                                item.RowNumber = rownumber;
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
                                    FTotalQuantity = Convert.ToInt32(item.TotalQuantity);
                                    FATotalAmount = item.ATotalAmount;
                                    FTaxName = item.TaxName;
                                    FTotalTax = item.TotalTax;
                                    FAGrandTotal = item.AGrandTotal;
                                    FGrandTotal = item.GrandTotal;

                                    // 28 Sep 2020
                                    TCSTaxAmount = item.TCSTaxAmount;

                                    FGrandAmtWord = item.GrandAmtWord;

                                    //25 March,2021 Sonal Gandhi
                                    invoiceNumber = invoicelstitem;
                                    if (!isNewOrder)
                                    {
                                        IRN = item.IRN;
                                        ActNo = item.AckNo;
                                        ActDate = Convert.ToString(item.AckDt);
                                        QRCode = item.QRCode;
                                    }
                                }
                                rownumber = rownumber + 1;
                            }
                        }

                        // 02 April, 2021 Piyush Limbani (Check IRN is avaliable or not)
                        long EInvoiceId = 0;
                        if (isNewOrder == false && !string.IsNullOrEmpty(InvoiceNumber))
                        {
                            EInvoiceId = _orderservice.CheckRetEInvoiceExist(OrderIdvalue, InvoiceNumber);
                        }
                        // 02 April, 2021 Piyush Limbani (Check IRN is avaliable or not)


                        // 25 March, 2021 Sonal Gandhi Call EInvoice API
                        if (isNewOrder)
                        {
                            //if (orderdata[0].To_GSTIN != null || orderdata[0].To_GSTIN != "")
                            if (!string.IsNullOrEmpty(orderdata[0].TaxNo))
                            {
                                if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                                {
                                    chkInvoiceNumber.Add(invoicelstitem);
                                    einvoice = new RetEInvoice();
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
                            //if (orderdata[0].To_GSTIN != null || orderdata[0].To_GSTIN != "")
                            if (!string.IsNullOrEmpty(orderdata[0].TaxNo))
                            {
                                if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                                {
                                    isGenIrn = true;  // 27 April 2021 Sonal Gandhi
                                    chkInvoiceNumber.Add(invoicelstitem);
                                    einvoice = new RetEInvoice();
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

                    }
                }
            }

            //27 March, 2021 Sonal Gandhi
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

            DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);


            // 26 March, 2021 Sonal Gandhi
            if (!string.IsNullOrEmpty(orderdata[0].TaxNo) && Convert.ToDateTime(orderdata[0].CreatedOn) > Convert.ToDateTime("03/31/2021"))
            {
                lr.EnableExternalImages = true;
                ReportParameter[] param = new ReportParameter[1];
                param[0] = new ReportParameter("QRCode", "1");

                lr.SetParameters(param);
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
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.8cm</MarginTop>" +   // 1cm to 0.8cm
            "  <MarginLeft>0.34cm</MarginLeft>" +
            "  <MarginRight>0.1cm</MarginRight>" +
            "  <MarginBottom>0.8cm</MarginBottom>" + // 1cm to 0.8cm
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
        //        //    path = "Report/RetailIGSTCreditMemoInvoiceTest.rdlc";
        //        //}
        //        //else
        //        //{
        //        //    path = Server.MapPath("~/Report/RetailIGSTCreditMemoInvoiceTest.rdlc");
        //        //}


        //        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
        //        {
        //            path = "Report/RetailIGSTCreditMemoInvoiceTest.rdlc";
        //        }
        //        else
        //        {
        //            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailIGSTCreditMemoInvoiceTest.rdlc");
        //        }

        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        //if (Request.Url.Host.Contains("localhost"))
        //        //{
        //        //    path = "Report/RetailCreditMemoInvoiceTest.rdlc";
        //        //}
        //        //else
        //        //{
        //        //    path = Server.MapPath("~/Report/RetailCreditMemoInvoiceTest.rdlc");
        //        //}



        //        if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
        //        {
        //            path = "Report/RetailCreditMemoInvoiceTest.rdlc";
        //        }
        //        else
        //        {
        //            path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailCreditMemoInvoiceTest.rdlc");
        //        }
        //        lr.ReportPath = path;
        //    }
        //    Lstdatainvoice = new List<RetOrderQtyInvoiceList>();
        //    List<string> incoiveNolist = new List<string>();
        //    string[] strcmn = CreditMemoNumber.Split(',');
        //    foreach (var itm in strcmn)
        //    {
        //        incoiveNolist.Add(itm);
        //    }
        //    foreach (var invoicelstitem in incoiveNolist)
        //    {
        //        int rownumber = 1;
        //        int totalnumber = 1;
        //        decimal total = 0;
        //        decimal taxtotal = 0;
        //        decimal grandtotal = 0;
        //        decimal totalMRP = 0;
        //        string NumberToWord = "";
        //        decimal totalQty = 0;
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
        //            if (Foodzero.Count < 30)
        //            {
        //                for (int s = Foodzero.Count; s < 30; s++)
        //                {
        //                    Foodzero.Add(new RetOrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", OrderDate = Convert.ToDateTime(DateTime.Now), TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, DiscountPrice = 0, BillDiscount = 0, ProductMRP = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, TaxAmount = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = 0, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", PONumber = "", City = "", State = "", StateCode = "", TotalMRP = 0, TotalQuantity = 0, InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
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
        //                    item.TotalQuantity = Convert.ToInt32(FTotalQuantity);
        //                    item.ATotalAmount = FATotalAmount;
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
        //                    if (rownumber > 30)
        //                    {
        //                        rownumber = 1;
        //                        totalnumber = totalnumber + 1;
        //                        total = 0;
        //                        taxtotal = 0;
        //                        grandtotal = 0;
        //                        totalMRP = 0;
        //                        NumberToWord = "";
        //                        totalQty = 0;
        //                    }
        //                    totalQty += item.Quantity;
        //                    item.TotalQuantity = Convert.ToInt32(totalQty);
        //                    totalMRP += item.ProductMRP;
        //                    item.TotalMRP = totalMRP;
        //                    item.TotalMRP = Math.Round(item.TotalMRP, 2);
        //                    item.Quantity = Math.Round(item.Quantity);
        //                    total += item.TotalAmount;
        //                    taxtotal += item.TaxAmount;
        //                    item.TotalAmount = item.TotalAmount;
        //                    grandtotal += item.TotalAmount + item.TaxAmount;
        //                    item.ATotalAmount = total;
        //                    item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
        //                    item.Totalrecord = Lstdata.Count;
        //                    item.FinalTotal = item.TotalAmount + item.TaxAmount;
        //                    item.AGrandTotal += grandtotal;
        //                    item.AGrandTotal = Math.Round(item.AGrandTotal, 2);
        //                    if (item.TaxName == "IGST")
        //                    {
        //                        item.TotalTax = taxtotal;
        //                        item.TotalTax = Math.Round(item.TotalTax, 2);
        //                    }
        //                    else
        //                    {
        //                        item.TotalTax = taxtotal / 2;
        //                        item.TotalTax = Math.Round(item.TotalTax, 2);
        //                    }
        //                    int number = Convert.ToInt32(item.AGrandTotal);
        //                    NumberToWord = NumberToWords(number);
        //                    item.GrandAmtWord = NumberToWord;
        //                    var Gtotal = Math.Round(grandtotal);
        //                    item.GrandTotal = Gtotal;
        //                    if (item.TaxName == "IGST")
        //                    {
        //                        item.TaxAmount = item.TaxAmount;
        //                    }
        //                    else
        //                    {
        //                        item.TaxAmount = item.TaxAmount / 2;
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
        //                    if (item.Tax == 0)
        //                    {
        //                        item.InvoiceTitle = "BILL OF SUPPLY";
        //                    }
        //                    else
        //                    {
        //                        item.InvoiceTitle = "TAX";
        //                    }
        //                    item.RowNumber = rownumber;
        //                    if (Foodzero2 > 30 * totalnumber)
        //                    {
        //                        item.Totalcount = Foodzero2 > 30 * totalnumber ? 30 * totalnumber : 30 * totalnumber - Foodzero2;
        //                    }
        //                    else
        //                    {
        //                        if (totalnumber == 1)
        //                        {
        //                            item.Totalcount = Foodzero2;
        //                        }
        //                        else
        //                        {
        //                            item.Totalcount = Foodzero2 - (30 * (totalnumber - 1));
        //                        }
        //                    }
        //                    Lstdatainvoice.Add(item);

        //                    if (rownumber == Foodzero2)
        //                    {
        //                        FTotalcount = item.Totalcount;
        //                        FTotalrecord = Convert.ToInt32(item.Totalrecord);
        //                        FTotalQuantity = Convert.ToInt32(item.TotalQuantity);
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

        //         "<DeviceInfo>" +
        //    "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>1cm</MarginTop>" +
        //    "  <MarginLeft>0.34cm</MarginLeft>" +
        //    "  <MarginRight>0.1cm</MarginRight>" +
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


        [HttpPost]
        public ActionResult PrintCreditMemo(string CreditMemoNumber, bool isNewCreditMemo = false)
        {
            LocalReport lr = new LocalReport();
            string path = "";

            //6 April,2021 Sonal Gandhi
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
                creditMemoRowNumber = 30;
                if (Lstdata[0].TaxName == "IGST")
                {
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/RetailIGSTCreditMemoInvoiceTest.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/RetailIGSTCreditMemoInvoiceTest.rdlc");
                    //}


                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/RetailIGSTCreditMemoInvoiceTest.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailIGSTCreditMemoInvoiceTest.rdlc");
                    }

                    lr.ReportPath = path;
                }
                else
                {
                    //if (Request.Url.Host.Contains("localhost"))
                    //{
                    //    path = "Report/RetailCreditMemoInvoiceTest.rdlc";
                    //}
                    //else
                    //{
                    //    path = Server.MapPath("~/Report/RetailCreditMemoInvoiceTest.rdlc");
                    //}



                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/RetailCreditMemoInvoiceTest.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailCreditMemoInvoiceTest.rdlc");
                    }
                    lr.ReportPath = path;
                }
            }
            else
            {
                creditMemoRowNumber = 25;
                if (Lstdata[0].TaxName == "IGST")
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_RetailIGSTCreditMemoInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_RetailIGSTCreditMemoInvoice.rdlc");
                    }

                    lr.ReportPath = path;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                    {
                        path = "Report/E_RetailCreditMemoInvoice.rdlc";
                    }
                    else
                    {
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Report/E_RetailCreditMemoInvoice.rdlc");
                    }
                    lr.ReportPath = path;
                }
            }
            Lstdatainvoice = new List<RetOrderQtyInvoiceList>();


            //6 April,2021 Sonal Gandhi
            RetEInvoiceCreditMemo einvoice = new RetEInvoiceCreditMemo();
            List<RetEInvoiceCreditMemo> eInvoiceLst = new List<RetEInvoiceCreditMemo>();
            List<RetOrderQtyInvoiceList> creditMemoToEInvoiceLst = new List<RetOrderQtyInvoiceList>();


            List<string> incoiveNolist = new List<string>();
            string[] strcmn = CreditMemoNumber.Split(',');
            foreach (var itm in strcmn)
            {
                incoiveNolist.Add(itm);
            }

            EInvoiceCreditMemoController eInvoice = new EInvoiceCreditMemoController();

            foreach (var invoicelstitem in incoiveNolist)
            {
                int rownumber = 1;
                int totalnumber = 1;
                decimal total = 0;
                decimal taxtotal = 0;
                decimal grandtotal = 0;
                decimal totalMRP = 0;
                string NumberToWord = "";
                decimal totalQty = 0;
                var Foodzero = Lstdata.Where(x => x.InvoiceNumber == invoicelstitem).OrderBy(y => y.ProductName).ToList();
                if (Foodzero.Count > 0)
                {
                    int Foodzero2 = Foodzero.Count;
                    int FTotalcount = 0;
                    int FTotalrecord = 0;
                    // int FTotalQuantity = 0;
                    decimal FTotalQuantity = 0; // convert in to decimal  27th April, 2021 Sonal Gandhi
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
                    QRCode = "";
                    string creditMemoNumber = invoicelstitem;
                    creditMemoToEInvoiceLst = new List<RetOrderQtyInvoiceList>();

                    if (Foodzero.Count < creditMemoRowNumber)
                    {
                        for (int s = Foodzero.Count; s < creditMemoRowNumber; s++)
                        {
                            Foodzero.Add(new RetOrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", OrderDate = Convert.ToDateTime(DateTime.Now), TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, DiscountPrice = 0, BillDiscount = 0, ProductMRP = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, TaxAmount = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = 0, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", PONumber = "", City = "", State = "", StateCode = "", TotalMRP = 0, TotalQuantity = 0, InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
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
                            // item.TotalQuantity = Convert.ToInt32(FTotalQuantity);
                            item.TotalQuantity = Math.Round(FTotalQuantity, 2); // convert in to decimal  27th April, 2021 Sonal Gandhi
                            item.ATotalAmount = FATotalAmount;
                            item.TaxName = FTaxName;
                            item.TotalTax = FTotalTax;
                            item.AGrandTotal = FAGrandTotal;
                            item.GrandTotal = FGrandTotal;

                            // 09 April 2021 Piyush Limbani
                            item.TCSTaxAmount = TCSTaxAmount;
                            // 09 April 2021 Piyush Limbani

                            item.GrandAmtWord = FGrandAmtWord;

                            //6 April,2021 Sonal Gandhi
                            if (!isNewCreditMemo)
                            {
                                if (IRN != null || IRN != "")
                                    item.IRN = IRN;
                                if (ActNo > 0)
                                    item.AckNo = ActNo;
                                if (ActDate != null || ActDate != "")
                                    item.AckDt = Convert.ToDateTime(ActDate);
                                if (QRCode != null || QRCode != "")
                                    item.QRCode = QRCode;
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
                                totalMRP = 0;
                                NumberToWord = "";
                                totalQty = 0;
                            }
                            totalQty += item.Quantity;
                            item.TotalQuantity = Convert.ToInt32(totalQty);
                            totalMRP += item.ProductMRP;
                            item.TotalMRP = totalMRP;
                            item.TotalMRP = Math.Round(item.TotalMRP, 2);
                            //item.Quantity = Math.Round(item.Quantity);
                            item.Quantity = Math.Round(item.Quantity, 2);  // convert in to decimal  27th April, 2021 Sonal Gandhi
                            total += item.TotalAmount;
                            taxtotal += item.TaxAmount;
                            item.TotalAmount = item.TotalAmount;

                            grandtotal += item.TotalAmount + item.TaxAmount;
                            item.ATotalAmount = total;
                            item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                            item.Totalrecord = Lstdata.Count;
                            item.FinalTotal = item.TotalAmount + item.TaxAmount;
                            item.AGrandTotal += grandtotal;
                            item.AGrandTotal = Math.Round(item.AGrandTotal, 2);
                            if (item.TaxName == "IGST")
                            {
                                item.TotalTax = taxtotal;
                                item.TotalTax = Math.Round(item.TotalTax, 2);
                            }
                            else
                            {
                                item.TotalTax = taxtotal / 2;
                                item.TotalTax = Math.Round(item.TotalTax, 2);
                            }

                            //int number = Convert.ToInt32(item.AGrandTotal);
                            //NumberToWord = NumberToWords(number);
                            //item.GrandAmtWord = NumberToWord;
                            //var Gtotal = Math.Round(grandtotal);
                            //item.GrandTotal = Gtotal;

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
                                item.TaxAmount = item.TaxAmount;
                                item.IGSTAmount = Math.Round(item.TaxAmount, 2);
                            }
                            else
                            {
                                item.TaxAmount = item.TaxAmount / 2;
                                item.SGSTAmount = Math.Round(item.TaxAmount, 2);
                                item.CGSTAmount = Math.Round(item.TaxAmount, 2);
                            }
                            item.TaxAmount = Math.Round(item.TaxAmount, 2);
                            if (item.TaxName == "IGST")
                            {
                                item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                item.IGSTTaxRate = Math.Round(item.Tax, 2);
                            }
                            else
                            {
                                item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                item.SGSTTaxRate = Math.Round(item.Tax, 2);
                                item.CGSTTaxRate = Math.Round(item.Tax, 2);
                            }
                            item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
                            if (item.Tax == 0)
                            {
                                item.InvoiceTitle = "BILL OF SUPPLY";
                            }
                            else
                            {
                                item.InvoiceTitle = "TAX";
                            }
                            item.RowNumber = rownumber;
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
                                // FTotalQuantity = Convert.ToInt32(item.TotalQuantity);
                                FTotalQuantity = Math.Round(item.TotalQuantity, 2); // convert in to decimal  27th April, 2021 Sonal Gandhi
                                FATotalAmount = item.ATotalAmount;
                                FTaxName = item.TaxName;
                                FTotalTax = item.TotalTax;
                                FAGrandTotal = item.AGrandTotal;
                                FGrandTotal = item.GrandTotal;
                                FGrandAmtWord = item.GrandAmtWord;
                                InvoiceNumber = item.InvoiceNumber;

                                // 09 April 2021 Piyush Limbani
                                TCSTaxAmount = item.TCSTaxAmount;
                                // 09 April 2021 Piyush Limbani

                                //25 March,2021 Sonal Gandhi
                                creditMemoNumber = invoicelstitem;
                                if (!isNewCreditMemo)
                                {
                                    IRN = item.IRN;
                                    ActNo = item.AckNo;
                                    ActDate = Convert.ToString(item.AckDt);
                                    QRCode = item.QRCode;
                                }
                            }
                            rownumber = rownumber + 1;
                        }
                    }

                    // 07 April, 2021 Piyush Limbani (Check IRN is avaliable or not)
                    long EInvoiceCreditMemoId = 0;
                    if (isNewCreditMemo == false && !string.IsNullOrEmpty(CreditMemoNumber))
                    {
                        EInvoiceCreditMemoId = _orderservice.CheckRetECreditMemoExist(CreditMemoNumber);
                    }
                    // 07 April, 2021 Piyush Limbani (Check IRN is avaliable or not)

                    // 6 April, 2021 Sonal Gandhi Call EInvoice API
                    if (isNewCreditMemo)
                    {
                        //if (Lstdata[0].To_GSTIN != null || Lstdata[0].To_GSTIN != "")
                        if (!string.IsNullOrEmpty(Lstdata[0].TaxNo))
                        {
                            if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                            {
                                chkInvoiceNumber.Add(invoicelstitem);
                                einvoice = new RetEInvoiceCreditMemo();
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
                        //if (Lstdata[0].To_GSTIN != null || Lstdata[0].To_GSTIN != "")
                        if (!string.IsNullOrEmpty(Lstdata[0].TaxNo))
                        {
                            if (!(chkInvoiceNumber.Contains<string>(invoicelstitem)))
                            {
                                isGenIrn = true;  // 27 April 2021 Sonal Gandhi
                                chkInvoiceNumber.Add(invoicelstitem);
                                einvoice = new RetEInvoiceCreditMemo();
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

            //6 April, 2021 Sonal Gandhi
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

            // 6 April, 2021 Sonal Gandhi
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
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>1cm</MarginTop>" +
            "  <MarginLeft>0.34cm</MarginLeft>" +
            "  <MarginRight>0.1cm</MarginRight>" +
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

        [HttpPost]
        public PartialViewResult SearchViewPackList(RetOrderPackListResponse model)
        {
            Session["PLOrderID"] = "";
            Session["PLEOrderID"] = "";
            Session["PLLOrderID"] = "";
            Session["PLBOrderID"] = "";
            if (string.IsNullOrEmpty(model.date))
            {
                model.date = DateTime.Now.ToString();
            }
            List<RetOrderPackListResponse> objModel = _orderservice.GetOrderPackList(Convert.ToDateTime(model.date));
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult PrintPackListSession(string id, string date)
        {
            Session["PLOrderID"] = id;
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            return Json(Session["PLOrderID"], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintPackList(string date)
        {
            try
            {

                string id = Session["PLOrderID"].ToString();

                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString();
                }
                ViewBag.Godown = _productservice.GetAllRetGodownName();
                List<RetOrderPackList> objModel = new List<RetOrderPackList>();
                if (!string.IsNullOrEmpty(id))
                {
                    objModel = _orderservice.GetOrderPackListForPrint(id, date);
                }
                return View(objModel);
            }
            catch
            {
                return View("SearchPackList");
            }
        }

        [HttpPost]
        public ActionResult ExportExcelSession(string id, string date)
        {
            Session["PLEOrderID"] = id;
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            return Json(Session["PLEOrderID"], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PrintLabelSession(string id, string date)
        {
            Session["PLLOrderID"] = id;
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            return Json(Session["PLLOrderID"], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PrintBarcodeSession(string id, string date)//, string sectionid)
        {
            Session["PLBOrderID"] = id;
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            return Json(Session["PLBOrderID"], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintSummaryList(string date)
        {
            try
            {
                string id = Session["SLOrderID"].ToString();
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString();
                }
                ViewBag.Godown = _productservice.GetAllRetGodownName();
                List<RetOrderSummaryList> objModel = new List<RetOrderSummaryList>();
                List<RetOrderSummaryList> objModelProduct = new List<RetOrderSummaryList>();
                if (!string.IsNullOrEmpty(id))
                {
                    objModel = _orderservice.GetOrderSummaryListForPrint(id, date);
                }
                DataTable dt = new DataTable();
                var Customer = objModel.GroupBy(x => x.CustomerID).Select(y => y.First()).ToList();
                var Products = objModel.GroupBy(x => x.ProductQtyID).Select(y => y.First()).ToList();
                var Category = objModel.GroupBy(x => x.CategoryName).Select(y => y.First()).ToList();
                dt.Columns.Add("CategoryName");
                dt.Columns.Add("Qty");
                foreach (var item in Customer)
                {
                    dt.Columns.Add(item.CustomerName);
                }
                dt.Columns.Add("Total");
                int i = 0;
                string ctgrcmpr = "";

                foreach (var item in Products)
                {
                    string CategoryMain = item.CategoryName;

                    if (ctgrcmpr != CategoryMain)
                    {
                        DataRow row = dt.NewRow();
                        ctgrcmpr = CategoryMain;
                        row[0] = CategoryMain;
                        row[1] = "";
                        i = 2;
                        foreach (var itm in Customer)
                        {
                            row[i] = "";
                            i++;
                        }
                        row[i] = "";
                        dt.Rows.Add(row);
                        row = dt.NewRow();
                        row[0] = item.ProductName;
                        row[1] = item.SKU;
                        i = 2;
                        foreach (var itm in Customer)
                        {
                            var lst = objModel.Where(x => x.CustomerName == itm.CustomerName && x.ProductQtyID == item.ProductQtyID).ToList();
                            TableCell cell = new TableCell();
                            if (lst.Count > 0)
                            {
                                row[i] = lst[0].QuantityPackage.ToString();
                            }
                            else
                            {
                                row[i] = "";
                            }
                            i++;
                        }
                        row[i] = item.Total;
                        dt.Rows.Add(row);
                    }
                    else
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.ProductName;
                        row[1] = item.SKU;

                        i = 2;
                        foreach (var itm in Customer)
                        {
                            var lst = objModel.Where(x => x.CustomerName == itm.CustomerName && x.ProductQtyID == item.ProductQtyID).ToList();
                            TableCell cell = new TableCell();
                            if (lst.Count > 0)
                            {
                                row[i] = lst[0].QuantityPackage.ToString();
                            }
                            else
                            {
                                row[i] = "";
                            }
                            i++;
                        }
                        row[i] = item.Total;
                        dt.Rows.Add(row);
                    }

                }
                return View(dt);
            }
            catch
            {
                return View("SearchSummaryList");
            }
        }

        [HttpPost]
        public ActionResult PrintSummaryListSession(string id, string date)
        {
            Session["SLOrderID"] = id;
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            return Json(Session["SLOrderID"], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportExcelSummary(string date)
        {
            try
            {
                string id = Session["SLOrderID"].ToString();
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString();
                }
                if (!string.IsNullOrEmpty(id))
                {
                    DataSet ds = new DataSet();
                    var Sections = _orderservice.GetSummaryListSections();
                    foreach (var itemS in Sections)
                    {
                        DataTable dt = new DataTable("Section" + itemS.ToString());
                        var objModel = _orderservice.GetOrderSummaryListForExport(id, date, itemS);
                        if (objModel.Count > 0)
                        {
                            var Customer = objModel.GroupBy(x => x.CustomerID).Select(y => y.First()).ToList();
                            var Products = objModel.GroupBy(x => x.ProductQtyID).Select(y => y.First()).ToList();
                            var Category = objModel.GroupBy(x => x.CategoryName).Select(y => y.First()).ToList();
                            dt.Columns.Add("CategoryName");
                            dt.Columns.Add("Qty");
                            foreach (var item in Customer)
                            {
                                dt.Columns.Add(item.CustomerName);
                            }
                            dt.Columns.Add("Total");
                            int i = 0;
                            string ctgrcmpr = "";
                            foreach (var item in Products)
                            {
                                string CategoryMain = item.CategoryName;
                                if (ctgrcmpr != CategoryMain)
                                {
                                    DataRow row = dt.NewRow();
                                    ctgrcmpr = CategoryMain;
                                    row[0] = CategoryMain;
                                    row[1] = "";
                                    i = 2;
                                    foreach (var itm in Customer)
                                    {

                                        row[i] = "";
                                        i++;
                                    }
                                    row[i] = "";
                                    dt.Rows.Add(row);

                                    row = dt.NewRow();
                                    row[0] = item.ProductName;
                                    row[1] = item.SKU;
                                    i = 2;
                                    foreach (var itm in Customer)
                                    {
                                        var lst = objModel.Where(x => x.CustomerName == itm.CustomerName && x.ProductQtyID == item.ProductQtyID).ToList();
                                        TableCell cell = new TableCell();
                                        if (lst.Count > 0)
                                        {
                                            row[i] = lst[0].QuantityPackage.ToString();
                                        }
                                        else
                                        {
                                            row[i] = "";
                                        }
                                        i++;
                                    }
                                    row[i] = item.Total;
                                    dt.Rows.Add(row);
                                }
                                else
                                {
                                    DataRow row = dt.NewRow();
                                    row[0] = item.ProductName;
                                    row[1] = item.SKU;

                                    i = 2;
                                    foreach (var itm in Customer)
                                    {
                                        var lst = objModel.Where(x => x.CustomerName == itm.CustomerName && x.ProductQtyID == item.ProductQtyID).ToList();
                                        TableCell cell = new TableCell();
                                        if (lst.Count > 0)
                                        {
                                            row[i] = lst[0].QuantityPackage.ToString();
                                        }
                                        else
                                        {
                                            row[i] = "";
                                        }
                                        i++;
                                    }
                                    row[i] = item.Total;
                                    dt.Rows.Add(row);
                                }

                            }
                            ds.Tables.Add(dt);
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;
                        wb.Worksheets.FirstOrDefault().AutoFilter.Clear();
                        foreach (IXLWorksheet workSheet in wb.Worksheets)
                        {
                            foreach (IXLTable table in workSheet.Tables)
                            {
                                workSheet.Table(table.Name).ShowAutoFilter = false;
                                workSheet.Columns().Width = 8.5;
                            }
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename= " + "SummaryList.xls");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }

                return View();
            }
            catch
            {
                return View("SearchSummaryList");
            }
        }

        public ActionResult SearchPackList()
        {
            Session["PLOrderID"] = "";
            Session["PLEOrderID"] = "";
            return View();
        }

        public ActionResult SearchSummaryList()
        {
            Session["SLOrderID"] = "";
            return View();
        }
        [HttpPost]
        public PartialViewResult SearchViewSummaryList(RetOrderSummaryListResponse model)
        {
            if (string.IsNullOrEmpty(model.date))
            {
                model.date = DateTime.Now.ToString();
            }
            List<RetOrderSummaryListResponse> objModel = _orderservice.GetOrderSummaryList(Convert.ToDateTime(model.date));
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ExportExcel(string date)
        {
            try
            {
                string id = Session["PLEOrderID"].ToString();

                string OrderID = Session["PLOrderID"].ToString();
                bool respose = _orderservice.UpdatePrintPackList(date, OrderID);

                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString();
                }
                if (!string.IsNullOrEmpty(id))
                {
                    DataSet ds = new DataSet();
                    var Sections = _orderservice.GetPackListSections();
                    foreach (var item in Sections)
                    {
                        // var dataInventory = _orderservice.GetOrderPackListForExport(id, date, item);
                        var dataInventory = _orderservice.GetOrderPackListForExport(id, date, item, OrderID);
                        if (dataInventory.Count > 0)
                        {
                            List<RetOrderPackListForExport> objModel = new List<RetOrderPackListForExport>();
                            objModel = dataInventory.Select(x => new RetOrderPackListForExport() { Category = x.CategoryName, Product = x.ProductName, SKU = x.SKU, QtyPackage = x.QuantityPackage, QtyAvailable = x.QtyAvailable, QtyPacked = x.QtyPacked, Total = (x.Total == 0 ? "" : x.Total.ToString() + " KG") }).ToList();
                            objModel.Add(new RetOrderPackListForExport() { Category = "", Product = "", SKU = "", QtyPackage = dataInventory[0].TotalQuantityPackage, QtyAvailable = dataInventory[0].TotalQtyAvailable, QtyPacked = dataInventory[0].TotalQtyPacked, Total = "" });
                            ds.Tables.Add(ToDataTable(objModel, item.ToString()));
                        }
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;
                        wb.ColumnWidth = 0.8;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename= " + "PackList.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch
            {
                return View("SearchPackList");
            }
            return View();
        }

        public static DataTable ToDataTable<T>(List<T> items, string itm)
        {
            //DataTable dataTable = new DataTable(typeof(T).Name + itm.ToString());
            DataTable dataTable = new DataTable("Section" + itm.ToString());
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
        public ActionResult CreateRetailCreditMemo(List<RetOrderQtyList> data)
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
            ViewBag.Customer = _orderservice.GetAllRetCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewReturnedOrderList(RetOrderListResponse model)
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
            List<RetOrderListResponse> objModel = _orderservice.GetAllReturnedOrderList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult PrintBarcode(List<RetOrderPackList> data, long godown, string date)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                string MonthDat = "";
                string yr1 = DateTime.Now.Year.ToString();
                // Changes By Dhruvik 28-01-2023
                string Batchno = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MM") + yr1.Substring(2, yr1.Length - 2);
                // Changes By Dhruvik 28-01-2023

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
                List<ProductBarcode> LabelData = new List<ProductBarcode>();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].CategoryID != 9)
                    {
                        if (data[i].ContentValue == null || data[i].NutritionValue == null)
                        {

                            ProductBarcode obj = new ProductBarcode();
                            for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                            {
                                BestBeforeMonth promonth = new BestBeforeMonth();
                                promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                DateTime theDate = DateTime.Now;
                                DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                const int MaxLength = 22;
                                obj.ProductName = data[i].ProductName;

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                                obj.QTY = data[i].SKU;
                                obj.MRP = data[i].MRP.ToString("0.00");

                                obj.GramPerKG = data[i].GramPerKG.ToString("0.00");

                                //  obj.Productbarcode = data[i].ProductBarcode;
                                obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                obj.MonthDate = MonthDat;
                                obj.NoofBarcode = data[i].QuantityPackage;
                                obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");

                                //   obj.Batch = Batchno;
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                                obj.GodownCode = godowndetails.GodownCode;
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                LabelData.Add(obj);
                            }
                        }
                    }
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
                //return stream.ToArray();
            }

        }

        [HttpPost]
        public ActionResult PrintLabel(List<RetOrderPackList> data, string date)
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

                string id = Session["PLLOrderID"].ToString();
                string orderid = Session["PLOrderID"].ToString();

                List<Product_Mst> LabelData = new List<Product_Mst>();
                if (!string.IsNullOrEmpty(id))
                {
                    var Sections = _orderservice.GetPackListSections();

                    foreach (var item in Sections)
                    {
                        var dataInventory = _orderservice.GetOrderPackListForLabel(id, date.ToString(), item, orderid);
                        for (int i = 0; i < dataInventory.Count; i++)
                        {
                            Product_Mst obj = new Product_Mst();
                            for (int j = 1; j <= dataInventory[i].ProductPrice; j++)
                            {
                                obj.ProductName = dataInventory[i].ProductName;
                                obj.ProductPrice = dataInventory[i].ProductPrice;// qty
                                LabelData.Add(obj);
                            }
                        }
                    }
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
                    //"  <PageWidth>11in</PageWidth>" +
                    //"  <PageHeight>8.7in</PageHeight>" +
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

        [HttpPost]
        public ActionResult DeleteOrder(int OrderID)
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
                    bool respose = _orderservice.DeleteOrder(OrderID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
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
            List<ExportToExcelInvoice> lst = lstInvoice.Select(x => new ExportToExcelInvoice()
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

        [HttpPost]
        public ActionResult PrintContentBarcode(List<RetOrderPackList> data, long godownid, string date)
        {

            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                string MonthDat = "";
                string yr1 = DateTime.Now.Year.ToString();
                // Changes By Dhruvik 28-01-2023
                string Batchno = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MM") + yr1.Substring(2, yr1.Length - 2);
                // Changes By Dhruvik 28-01-2023

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
                godowndetails = _productservice.GetGodownDetailsByGodownID(godownid);
                List<ProductBarcode> LabelData = new List<ProductBarcode>();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].CategoryID != 9)
                    {
                        if (data[i].ContentValue != null || data[i].NutritionValue != null)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                            {
                                BestBeforeMonth promonth = new BestBeforeMonth();
                                promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                DateTime theDate = DateTime.Now;
                                DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                const int MaxLength = 22;
                                obj.ProductName = data[i].ProductName;
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                                obj.QTY = data[i].SKU;
                                obj.MRP = data[i].MRP.ToString("0.00");
                                obj.GramPerKG = data[i].GramPerKG.ToString("0.00");

                                obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                obj.MonthDate = MonthDat;
                                obj.NoofBarcode = data[i].QuantityPackage;
                                obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");

                                // obj.Batch = Batchno;
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                                obj.GodownCode = godowndetails.GodownCode;
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.NutritionValue = data[i].NutritionValue;
                                obj.ContentValue = data[i].ContentValue;
                                LabelData.Add(obj);
                            }
                        }
                    }
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

        [HttpPost]
        public ActionResult PrintNonFoodBarcode(List<RetOrderPackList> data, long godownidnonfood, string date)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                string MonthDat = "";
                string yr1 = DateTime.Now.Year.ToString();
                // Changes By Dhruvik 28-01-2023
                string Batchno = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MM") + yr1.Substring(2, yr1.Length - 2);
                // Changes By Dhruvik 28-01-2023

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
                godowndetails = _productservice.GetGodownDetailsByGodownID(godownidnonfood);
                List<ProductBarcode> LabelData = new List<ProductBarcode>();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].CategoryID == 9)
                    {
                        if (data[i].ContentValue == null || data[i].NutritionValue == null)
                        {

                            ProductBarcode obj = new ProductBarcode();
                            for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                            {
                                BestBeforeMonth promonth = new BestBeforeMonth();
                                promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                DateTime theDate = DateTime.Now;
                                DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                const int MaxLength = 22;
                                obj.ProductName = data[i].ProductName;

                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                                obj.QTY = data[i].SKU;
                                obj.MRP = data[i].MRP.ToString("0.00");
                                obj.GramPerKG = data[i].GramPerKG.ToString("0.00");

                                //  obj.Productbarcode = data[i].ProductBarcode;
                                obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                obj.MonthDate = MonthDat;
                                obj.NoofBarcode = data[i].QuantityPackage;
                                obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");

                                // obj.Batch = Batchno;
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                                obj.GodownCode = godowndetails.GodownCode;
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                LabelData.Add(obj);
                            }
                        }
                    }
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

        [HttpPost]
        public ActionResult PrintNonFoodContentBarcode(List<RetOrderPackList> data, long godownidnonfoodcontent, string date)
        {

            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                string MonthDat = "";
                string yr1 = DateTime.Now.Year.ToString();
                // Changes By Dhruvik 28-01-2023
                string Batchno = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MM") + yr1.Substring(2, yr1.Length - 2);
                // Changes By Dhruvik 28-01-2023

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
                godowndetails = _productservice.GetGodownDetailsByGodownID(godownidnonfoodcontent);
                List<ProductBarcode> LabelData = new List<ProductBarcode>();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].CategoryID == 9)
                    {
                        if (data[i].ContentValue != null || data[i].NutritionValue != null)
                        {
                            ProductBarcode obj = new ProductBarcode();
                            for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                            {
                                BestBeforeMonth promonth = new BestBeforeMonth();
                                promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                DateTime theDate = DateTime.Now;
                                DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                const int MaxLength = 22;
                                obj.ProductName = data[i].ProductName;
                                if (obj.ProductName.Length > MaxLength)
                                {
                                    obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                }
                                obj.QTY = data[i].SKU;
                                obj.MRP = data[i].MRP.ToString("0.00");
                                obj.GramPerKG = data[i].GramPerKG.ToString("0.00");

                                obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                obj.MonthDate = MonthDat;
                                obj.NoofBarcode = data[i].QuantityPackage;
                                obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");

                                // obj.Batch = Batchno;
                                obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();

                                obj.GodownCode = godowndetails.GodownCode;
                                obj.AddressLine1 = godowndetails.GodownAddress1;
                                obj.AddressLine2 = godowndetails.GodownAddress2;
                                obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                obj.PhoneNo = godowndetails.GodownPhone;
                                obj.GodownName = godowndetails.GodownName;
                                obj.NutritionValue = data[i].NutritionValue;
                                obj.ContentValue = data[i].ContentValue;
                                LabelData.Add(obj);
                            }
                        }
                    }
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


        public ActionResult ManageDeliveryChallan()
        {
            ViewBag.Godown = _productservice.GetAllRetGodownName();
            ViewBag.Tax = _commonservice.GetAllTaxName();
            ViewBag.Product = _orderservice.GetAllRetProductName();
            return View();
        }

        [HttpPost]
        public ActionResult GetUnit(long ProductQtyID)
        {
            var lstUnit = _orderservice.GetAutoCompleteProduct(ProductQtyID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSellPrice(long ProductName, string Tax)
        {
            if (Tax != "")
            {
                var SellPrice = _orderservice.GetAutoCompleteSellPrice(ProductName, Tax);
                return Json(SellPrice, JsonRequestBehavior.AllowGet);
            }
            else
            {
                GetRetSellPrice1 objdata = new GetRetSellPrice1();
                objdata.SellPrice = 0;
                objdata.Tax = 0;
                return Json(objdata, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveChallan(RetChallanViewModel data)
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
                //    path = "Report/RetChallan.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/RetChallan.rdlc");
                //}


                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/RetChallan.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetChallan.rdlc");
                }
                lr.ReportPath = path;
            }
            ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
            Lstdatainvoice2 = new List<RetChallanQtyInvoiceList>();
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
                    //var Count = Foodzero.Where(x => x.ChallanNumber != "").ToList();
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
                                Foodzero.Add(new RetChallanQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, ChallanNumber = "", TaxName = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, BillDiscount = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, FinalTotal = 0, GrandTotal = 0, ChallanID = 0, CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
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
                                //else if (i == 2)
                                //{
                                //    item.InvoiceTitleHeader = "DUPLICATE FOR TRANSPORTER";
                                //}
                                //else if (i == 3)
                                //{
                                //    item.InvoiceTitleHeader = "TRIPLICATE FOR SUPPLIER";
                                //}
                                else
                                {
                                    item.InvoiceTitleHeader = "";
                                }
                                //if (item.Tax == 0)
                                //{
                                //    item.InvoiceTitle = "BILL OF SUPPLY";
                                //}
                                //else
                                //{
                                //    item.InvoiceTitle = "TAX INVOICE";
                                //}
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
                                //ProductCount++;
                                //if (Count.Count == ProductCount && item.InvoiceTitleHeader == "DELIVERY CHALLAN")
                                //{
                                //    ProductCount = 0;
                                //    InvoiceTotal += Gtotal;
                                //}
                                //item.InvoiceTotal = InvoiceTotal;
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
        public PartialViewResult ViewChallanList(RetChallanListResponse model)
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
            List<RetChallanListResponse> objModel = _orderservice.GetAllChallanList(model);

            // 25 May, 2021 Sonal Gandhi
            ViewBag.Transport = _commonservice.GetAllTransportName();
            ViewBag.VehicleNo = _commonservice.GetAllTempoNumberList();

            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewChallan(Int64? id, string txtfrom, string txtto)
        {
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            List<RetChallanQtyList> objModel = _orderservice.GetInvoiceForChallan(Convert.ToInt64(id));
            return View(objModel);
        }

        public ActionResult ExportExcelChallan(long ChallanID, string FromAddress1, string FromAddress2, string FromPlace, string FromPinCode, string FromState, string DispatchState, string ToAddress1, string ToAddress2, string ToPlace, string ToPinCode, string ToState, string ShipToState)
        {
            var lstInvoice = _orderservice.GetChallanForExcel(ChallanID);
            List<ExportToExcelChallanRetail> lst = lstInvoice.Select(x => new ExportToExcelChallanRetail() { SupplyType = x.SupplyType, SubType = x.SubType, DocType = x.DocType, DocNo = x.DocNo, DocDate = x.DocDate, Transaction_Type = x.Transaction_Type, DeliveryFrom = x.DeliveryFrom, From_GSTIN = x.From_GSTIN, From_Address1 = FromAddress1, From_Address2 = FromAddress2, From_Place = FromPlace, From_PinCode = FromPinCode, From_State = FromState, DispatchState = DispatchState, DeliveryTo = x.DeliveryTo, To_GSTIN = x.To_GSTIN, To_Address1 = ToAddress1, To_Address2 = ToAddress2, To_Place = ToPlace, To_PinCode = ToPinCode, To_State = ToState, ShipToState = ShipToState, Product = x.Product, Description = x.Description, HSN = x.HSN, Unit = x.Unit, Qty = x.Qty, AssessableValue = x.AssessableValue, TaxRate = x.TaxRate, CGSTAmount = x.CGSTAmount, SGSTAmount = x.SGSTAmount, IGSTAmount = x.IGSTAmount, CESSAmount = x.CESSAmount, Cess_Non_Advol_Amount = x.Cess_Non_Advol_Amount, Others = x.Others, ChallanTotal = x.ChallanTotal, TransMode = x.TransMode, DistanceKM = x.DistanceKM, TransName = x.TransName, TransID = x.TransID, TransDocNo = x.TransDocNo, TransDate = x.TransDate, VehicleNo = x.VehicleNo }).ToList();
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

                    //bool respose = _orderservice.DeleteCreditMemo(CreditMemoNumber, IsDelete);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }




        // Updated on 06/12/2018

        public ActionResult SearchCheckList()
        {
            Session["PLOrderID"] = "";
            Session["PLEOrderID"] = "";
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchViewCheckList(RetOrderPackListResponse model)
        {
            Session["PLOrderID"] = "";
            Session["PLEOrderID"] = "";
            Session["PLLOrderID"] = "";
            Session["PLBOrderID"] = "";
            if (string.IsNullOrEmpty(model.date))
            {
                model.date = DateTime.Now.ToString();
            }
            List<RetOrderPackListResponse> objModel = _orderservice.GetOrderCheckList(Convert.ToDateTime(model.date));
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult PrintCheckListSession(string id, string date, char Tag)
        {
            Session["PLOrderID"] = id;
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            return Json(Session["PLOrderID"], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintCheckList(string date, string orderid, char Tag)
        {
            try
            {
                //string id = Session["PLOrderID"].ToString();
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString();
                }
                ViewBag.Godown = _productservice.GetAllRetGodownName();
                List<RetOrderPackList> objModel = new List<RetOrderPackList>();
                if (!string.IsNullOrEmpty(orderid))
                {
                    objModel = _orderservice.GetOrderCheckListForLabelPrint(orderid, date);
                }
                return View(objModel);
            }
            catch
            {
                return View("SearchCheckList");
            }
        }

        public ActionResult PrintContentLabel(string CustomerName, string AreaName, string PONumber, string ProductName, string OrderQtyID, long OrderID, long CustomerID, long AreaID, bool Tray, bool Zabla, bool Box, decimal TotalKG, DateTime OrderDate, string Tag)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("ProductName", typeof(string));
                string[] values = ProductName.Split(',');
                for (int i = 0; i < values.Length; i++)
                    table.Rows.Add(new object[] { values[i] });

                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/PrintContentLabel.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/PrintContentLabel.rdlc");
                //}

                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/PrintContentLabel.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintContentLabel.rdlc");
                }
                lr.ReportPath = path;

                string[] tokens = OrderQtyID.Split(',');

                long ID = 0;

                foreach (var item in tokens)
                {
                    string Orderotyid = Convert.ToString(item);
                    string[] Qty = Orderotyid.Split('-');

                    ID = Convert.ToInt64(Qty[0].ToString());
                    decimal Quantity = Convert.ToDecimal(Qty[1].ToString());
                    long ProductID = Convert.ToInt64(Qty[2].ToString());
                    decimal ProductQtyID = Convert.ToInt64(Qty[3].ToString());
                    bool respose = _orderservice.UpdateRetOrderQuantityForPrintLabel(ID, Quantity);
                }

                int Bag = 0;
                int TrayNo = 0;
                int ZablaNo = 0;
                int BoxNo = 0;
                string FinalTag = "";

                if (Tray == true)
                {
                    var lstdata = _orderservice.GetLastTrayNumberByOrderID(OrderID);
                    if (lstdata.Tray != 0)
                    {
                        int incr = Convert.ToInt32(lstdata.Tray + 1);
                        TrayNo = incr;
                    }
                    else
                    {
                        TrayNo = 1;
                    }
                    FinalTag = Tag;
                    // FinalTag = Tag + "-" + Convert.ToString(TrayNo);
                }
                else if (Zabla == true)
                {
                    var lstdata = _orderservice.GetLastZablaNumberByOrderID(OrderID);
                    if (lstdata.Zabla != 0)
                    {
                        int incr = Convert.ToInt32(lstdata.Zabla + 1);
                        ZablaNo = incr;
                    }
                    else
                    {
                        ZablaNo = 1;
                    }
                    FinalTag = Tag;
                    //FinalTag = Tag + "-" + Convert.ToString(ZablaNo);
                }
                else if (Box == true)
                {
                    var lstdata = _orderservice.GetLastBoxNumberByOrderID(OrderID);
                    if (lstdata.Box != 0)
                    {
                        int incr = Convert.ToInt32(lstdata.Box + 1);
                        BoxNo = incr;
                    }
                    else
                    {
                        BoxNo = 1;
                    }
                    FinalTag = Tag;
                    //FinalTag = Tag + "-" + Convert.ToString(BoxNo);
                }
                else
                {
                    var lstdata = _orderservice.GetLastBagNumberByOrderID(OrderID);
                    // Bag = 0;
                    if (lstdata.Bag != 0)
                    {
                        int incr = Convert.ToInt32(lstdata.Bag + 1);
                        Bag = incr;
                    }
                    else
                    {
                        Bag = 1;
                    }
                    FinalTag = Tag;
                    // FinalTag = Tag + "-" + Convert.ToString(Bag);
                }

                long PackSummaryID = _orderservice.AddRetPackSummaryDetail(OrderID, ID, CustomerID, AreaID, ProductName, Bag, TrayNo, ZablaNo, BoxNo, TotalKG, Convert.ToInt64(Request.Cookies["UserID"].Value), 0, OrderDate, FinalTag);

                // Get Tag and packsummaryid


                if (PackSummaryID > 0)
                {
                    foreach (var item in tokens)
                    {
                        string Orderotyid = Convert.ToString(item);
                        string[] Qty = Orderotyid.Split('-');
                        long OrderQtyID1 = Convert.ToInt64(Qty[0].ToString());
                        decimal Quantity = Convert.ToDecimal(Qty[1].ToString());
                        long ProductID = Convert.ToInt64(Qty[2].ToString());
                        long ProductQtyID = Convert.ToInt64(Qty[3].ToString());

                        bool result = _orderservice.AddRetPackSummaryQtyDetail(PackSummaryID, OrderID, OrderQtyID1, ProductID, ProductQtyID, Quantity, Convert.ToInt64(Request.Cookies["UserID"].Value));
                    }

                }

                List<PrintLabelContent> LabelData = new List<PrintLabelContent>();
                List<PrintLabelContent1> LabelData1 = new List<PrintLabelContent1>();
                PrintLabelContent obj = new PrintLabelContent();
                obj.CustomerName = CustomerName;
                obj.PONumber = PONumber;
                //obj.ProductName = ProductName;
                obj.AreaName = AreaName;
                obj.OrderDate = OrderDate.ToString("dd-MM-yyyy");
                obj.TotalKG = "Total KG : " + TotalKG.ToString();
                if (BoxNo != 0)
                {
                    obj.BagNo = "Box No : " + BoxNo.ToString();
                }
                else if (TrayNo != 0)
                {
                    obj.BagNo = "Tray No : " + TrayNo.ToString();
                }

                else if (ZablaNo != 0)
                {
                    obj.BagNo = "Zabla No : " + ZablaNo.ToString();
                }
                else
                {
                    obj.BagNo = "Bag No : " + Bag.ToString();
                }

                obj.Tag = FinalTag;


                LabelData.Add(obj);
                DataTable header = Common.ToDataTable(LabelData);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);

                DataTable header1 = Common.ToDataTable(LabelData1);
                ReportDataSource MedsheetHeader1 = new ReportDataSource("DataSet2", table);

                lr.DataSources.Add(MedsheetHeader);
                lr.DataSources.Add(MedsheetHeader1);
                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                      "<DeviceInfo>" +
                   "  <OutputFormat>" + reportType + "</OutputFormat>" +
               "  <PageWidth>1.78in</PageWidth>" +
                   "  <PageHeight>4.4in</PageHeight>" +
                   "  <MarginTop>0.5cm</MarginTop>" +
                   "  <MarginLeft>0.5cm</MarginLeft>" +
                   "  <MarginRight>0.3cm</MarginRight>" +
                   "  <MarginBottom>0.5cm</MarginBottom>" +
                   "</DeviceInfo>";


                //   "<DeviceInfo>" +
                //    "  <OutputFormat>" + reportType + "</OutputFormat>" +
                //"  <PageWidth>1.78in</PageWidth>" +
                //    "  <PageHeight>2.2in</PageHeight>" +
                //    "  <MarginTop>0.5cm</MarginTop>" +
                //    "  <MarginLeft>0.5cm</MarginLeft>" +
                //    "  <MarginRight>0.3cm</MarginRight>" +
                //    "  <MarginBottom>0.5cm</MarginBottom>" +
                //    "</DeviceInfo>";

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

        [HttpPost]
        public ActionResult PrintSummary(long OrderID)
        {
            //long OrderID = Convert.ToInt64(Session["PLOrderID"].ToString());
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/PrintLabelSummary.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/PrintLabelSummary.rdlc");
                //}



                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/PrintLabelSummary.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintLabelSummary.rdlc");
                }
                lr.ReportPath = path;

                List<RetCustomeDetail> orderdata = _orderservice.GetOrderCustomerDetail(OrderID);
                List<RetPackSummary> lstPackSummary = _orderservice.GetRetPackSummaryByOrderID(OrderID);

                DataTable header = Common.ToDataTable(orderdata);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);

                DataTable FoodDT = Common.ToDataTable(lstPackSummary);
                ReportDataSource DataRecord = new ReportDataSource("DataSet2", FoodDT);

                lr.DataSources.Add(MedsheetHeader);
                lr.DataSources.Add(DataRecord);

                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                //    "<DeviceInfo>" +
                    //"  <OutputFormat>" + reportType + "</OutputFormat>" +
                    //"  <PageWidth>8.5in</PageWidth>" +
                    //"  <PageHeight>11in</PageHeight>" +
                    //"  <MarginTop>1cm</MarginTop>" +
                    //"  <MarginLeft>1cm</MarginLeft>" +
                    //"  <MarginRight>1cm</MarginRight>" +
                    //"  <MarginBottom>1cm</MarginBottom>" +
                    //"</DeviceInfo>";

                 "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>11in</PageWidth>" +
                "  <PageHeight>7.5in</PageHeight>" +
                "  <MarginTop>2.1cm</MarginTop>" +
                "  <MarginLeft>7.6cm</MarginLeft>" +
                "  <MarginRight>0.5cm</MarginRight>" +
                "  <MarginBottom>2.1cm</MarginBottom>" +
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
                string Pdfpathcreate = Server.MapPath("~/PackDetails/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                //string url = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    url = "http://" + Request.Url.Host + ":6551/PackDetails/" + name;
                //}
                //else
                //{
                //    url = "http://" + Request.Url.Host + "/PackDetails/" + name;
                //}



                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/PackDetails/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/PackDetails/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/PackDetails/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintTotalBag(long OrderID, string Tag)
        {
            // long OrderID = Convert.ToInt64(Session["PLOrderID"].ToString());

            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/RetTotalPackDetails.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/RetTotalPackDetails.rdlc");
                //}


                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/RetTotalPackDetails.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetTotalPackDetails.rdlc");
                }
                lr.ReportPath = path;

                RetPackTotal lstPackSummary = _orderservice.GetRetPrintTotalBagByOrderID(OrderID);

                List<RetPackTotal> LabelData = new List<RetPackTotal>();
                RetPackTotal obj = new RetPackTotal();

                obj.Tag = Tag;

                if (lstPackSummary.Bag != 0 && lstPackSummary.Zabla == 0 && lstPackSummary.Tray == 0 && lstPackSummary.Box == 0)
                {
                    obj.Bagstr = lstPackSummary.Bagstr;
                }
                else if (lstPackSummary.Bag != 0)
                {
                    obj.Bagstr = lstPackSummary.Bagstr + ",";
                }
                else
                {
                    obj.Bagstr = "";
                }

                if (lstPackSummary.Zabla != 0 && lstPackSummary.Tray == 0 && lstPackSummary.Box == 0)
                {
                    obj.Zablastr = lstPackSummary.Zablastr;
                }
                else if (lstPackSummary.Zabla != 0)
                {
                    obj.Zablastr = lstPackSummary.Zablastr + ",";
                }
                else
                {
                    obj.Zablastr = "";
                }
                if (lstPackSummary.Tray != 0 && lstPackSummary.Box == 0)
                {
                    obj.Traystr = lstPackSummary.Traystr;
                }
                else if (lstPackSummary.Tray != 0)
                {
                    obj.Traystr = lstPackSummary.Traystr + ",";
                }
                else
                {
                    obj.Traystr = "";
                }

                if (lstPackSummary.Box != 0)
                {
                    obj.Boxstr = lstPackSummary.Boxstr;
                }
                else
                {
                    obj.Boxstr = "";
                }
                LabelData.Add(obj);

                DataTable FoodDT = Common.ToDataTable(LabelData);
                ReportDataSource DataRecord = new ReportDataSource("DataSet1", FoodDT);

                lr.DataSources.Add(DataRecord);
                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                   "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                 "  <PageWidth>3in</PageWidth>" +
                "  <PageHeight>1in</PageHeight>" +
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
                string Pdfpathcreate = Server.MapPath("~/PackDetails/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                //string url = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    url = "http://" + Request.Url.Host + ":6551/PackDetails/" + name;
                //}
                //else
                //{
                //    url = "http://" + Request.Url.Host + "/PackDetails/" + name;
                //}



                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/PackDetails/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/PackDetails/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/PackDetails/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateCheckListQuantity(long OrderID, long OrderQtyID, decimal UpdateQuantity)
        {
            try
            {
                bool respose = _orderservice.UpdateCheckListQuantity(OrderID, OrderQtyID, UpdateQuantity);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PackDetailList(string date, string orderid)
        {
            try
            {
                //string id = orderid;              
                //ViewBag.Godown = _productservice.GetAllRetGodownName();
                List<RetPackSummary> objModel = new List<RetPackSummary>();
                if (!string.IsNullOrEmpty(orderid))
                {
                    objModel = _orderservice.GetPackDetailListByOrderID(orderid, date);
                }
                return View(objModel);
            }
            catch
            {
                return View("SearchCheckList");
            }
        }

        [HttpPost]
        public ActionResult DeleteBag(long PackSummaryID, bool IsDelete)
        {
            try
            {
                List<RetPackSummary> lstUpdateProductQTYList = _orderservice.GetUpdateProductQTYList(PackSummaryID);
                foreach (var item in lstUpdateProductQTYList)
                {
                    long OrderID = item.OrderID;
                    long OrderQtyID = item.OrderQtyID;
                    decimal Quantity = item.Quantity;
                    long PackSummaryQtyID = item.PackSummaryQtyID;
                    long UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    DateTime UpdatedOn = DateTime.Now;
                    bool respose1 = _orderservice.UpdateCheckListQuantity(OrderID, OrderQtyID, Quantity);
                    bool respose2 = _orderservice.DeleteProductPackate(PackSummaryQtyID, IsDelete, UpdatedBy, UpdatedOn);
                }
                bool respose = _orderservice.DeleteBag(PackSummaryID, IsDelete);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult PopupProduct(long packsummaryid)
        {
            List<RetPackSummary> ProductList = _orderservice.GetPackProductListByBagWise(packsummaryid);
            return PartialView(ProductList);
        }

        [HttpPost]
        public ActionResult UpdateProductPackList(List<RetPackSummary> data)
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
                    long OrderQtyID = 0;
                    decimal Quantity = 0;
                    long PackSummaryQtyID = 0;
                    bool IsDelete = true;
                    long PackSummaryID = 0;
                    decimal SumTotalKG = 0;
                    foreach (var item in data)
                    {
                        OrderQtyID = item.OrderQtyID;
                        Quantity = item.Quantity;
                        PackSummaryQtyID = item.PackSummaryQtyID;
                        SumTotalKG = item.SumTotalKG;
                        PackSummaryID = item.PackSummaryID;
                        bool respose = _orderservice.UpdateProductPackList(OrderQtyID, Quantity, PackSummaryQtyID, IsDelete, UserID, PackSummaryID, SumTotalKG);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintContentLabelE(string CustomerName, string AreaName, string PONumber, string Bag, string ProductName, decimal TotalKG, DateTime OrderDate, string Tag)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("ProductName", typeof(string));
                string[] values = ProductName.Split(',');
                for (int i = 0; i < values.Length; i++)
                    table.Rows.Add(new object[] { values[i] });

                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/PrintContentLabel.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/PrintContentLabel.rdlc");
                //}


                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/PrintContentLabel.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PrintContentLabel.rdlc");
                }
                lr.ReportPath = path;

                List<PrintLabelContent> LabelData = new List<PrintLabelContent>();
                List<PrintLabelContent1> LabelData1 = new List<PrintLabelContent1>();
                PrintLabelContent obj = new PrintLabelContent();
                obj.CustomerName = CustomerName;
                obj.PONumber = PONumber;
                obj.AreaName = AreaName;
                obj.TotalKG = "Total KG : " + TotalKG.ToString();
                obj.BagNo = Bag;
                obj.OrderDate = OrderDate.ToString("dd-MM-yyyy");
                obj.Tag = Tag;
                LabelData.Add(obj);
                DataTable header = Common.ToDataTable(LabelData);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);

                DataTable header1 = Common.ToDataTable(LabelData1);
                ReportDataSource MedsheetHeader1 = new ReportDataSource("DataSet2", table);

                lr.DataSources.Add(MedsheetHeader);
                lr.DataSources.Add(MedsheetHeader1);
                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                      "<DeviceInfo>" +
                   "  <OutputFormat>" + reportType + "</OutputFormat>" +
               "  <PageWidth>1.78in</PageWidth>" +
                   "  <PageHeight>4.4in</PageHeight>" +
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

        [HttpPost]
        public PartialViewResult ViewOrderWiseCreditMemoForCheckList(ClsRetReturnOrderListResponse model)
        {
            List<ClsRetReturnOrderListResponse> objModel = _orderservice.GetOrderWiseCreditMemoForCheckList(model);
            var results = from p in objModel
                          group p.CustomerName by p.InvoiceNumber into g
                          select new { InvoiceNumber = g.Key, Cars = g.ToList() };
            List<RetReturnOrderListResponse> objdata = new List<RetReturnOrderListResponse>();
            foreach (var item in results)
            {
                RetReturnOrderListResponse obj = new RetReturnOrderListResponse();
                var data = objModel.Where(x => x.InvoiceNumber == item.InvoiceNumber).FirstOrDefault();
                obj.AreaName = data.AreaName;
                obj.CustomerName = data.CustomerName;
                obj.OrderDate = data.OrderDate;
                obj.PODate = data.PODate;
                obj.InvoiceNumber = data.InvoiceNumber;
                obj.OrderID = data.OrderID;
                obj.UserName = data.UserName;
                List<RetOrderQtyList> lstdatanew = new List<RetOrderQtyList>();
                //lstdatanew = objModel.Where(x => x.InvoiceNumber == obj.InvoiceNumber).Select(x => new RetOrderQtyList() { ProductID = x.ProductID, OrderQtyID = x.OrderQtyID, OrderID = x.OrderID, CategoryTypeID = x.CategoryTypeID, ReturnedQuantity = x.ReturnedQuantity, SerialNumber = x.SerialNumber, ProductName = x.ProductName, Quantity = x.Quantity, ProductPrice = x.ProductPrice, DiscountPrice = x.DiscountPrice, TotalAmount = x.TotalAmount, Tax = x.Tax, TaxAmt = x.TaxAmount, FinalTotal = x.FinalTotal, CustomerID = x.CustomerID, DiscountPer = x.DiscountPer, ProductMRP = x.ProductMRP }).ToList();
                lstdatanew = objModel.Where(x => x.InvoiceNumber == obj.InvoiceNumber).Select(x => new RetOrderQtyList() { ProductID = x.ProductID, OrderQtyID = x.OrderQtyID, OrderID = x.OrderID, CategoryTypeID = x.CategoryTypeID, ReturnedQuantity = x.ReturnedQuantity, SerialNumber = x.SerialNumber, ProductName = x.ProductName, UpdateQuantity = x.UpdateQuantity, ProductPrice = x.ProductPrice, DiscountPrice = x.DiscountPrice, TotalAmount = x.TotalAmount, Tax = x.Tax, TaxAmt = x.TaxAmount, FinalTotal = x.FinalTotal, CustomerID = x.CustomerID, DiscountPer = x.DiscountPer, ProductMRP = x.ProductMRP }).ToList();
                obj.lstOrderQty = new List<RetOrderQtyList>();
                obj.lstOrderQty = lstdatanew;
                objdata.Add(obj);
            }
            return PartialView(objdata);
        }




        [HttpPost]
        public ActionResult UpdateEWayNumber(long OrderID, string InvoiceNumber, string EWayNumber)
        {
            try
            {
                bool respose = _orderservice.UpdateRetailEWayNumberByOrderIDandInvoiceNumber(OrderID, InvoiceNumber, EWayNumber);
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
            var detail = _commonservice.GetRetTransportDetailByTransportID(TransportID);
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
        public PartialViewResult ViewChallanNoWiseChallanList(RetChallanListResponse model)
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
            List<RetChallanListResponse> objModel = _orderservice.GetAllChallanNoWiseChallanList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewChallanNoWiseChallan(string ChallanNumber, string txtfrom, string txtto)
        {
            Session["txtfrom"] = txtfrom;
            Session["txtto"] = txtto;
            if (!string.IsNullOrEmpty(ChallanNumber))
            {
                List<RetChallanQtyList> objModel = _orderservice.GetChallanNoWiseChallanForChallan(ChallanNumber);
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
                    var detail = _orderservice.GetRetIRNNumberByInvoiceNumber(OrderID, InvoiceNumber);
                    EInvoiceController eInvoice = new EInvoiceController();
                    bool respose = eInvoice.DeleteRetEInvoice(detail.IRN, OrderID, InvoiceNumber, Convert.ToInt64(Request.Cookies["UserID"].Value), DateTime.Now);
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
            List<RetEInvoiceErrorListResponse> obj = _orderservice.GetEInvoiceErrorList(Date);
            return PartialView(obj);
        }

        public ActionResult ExportExcelEInvoiceErrorList(DateTime Date)
        {
            var lstEInvoiceError = _orderservice.GetEInvoiceErrorList(Date);
            List<ExportRetEInvoiceErrorList> lst = lstEInvoiceError.Select(x => new ExportRetEInvoiceErrorList()
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "RetEInvoiceErrorList.xls");
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


            List<RetDetailsForEWB> lstInvoice = _orderservice.RetGetDetailsForEWB(OrderID, GodownID.Value, TransportID.Value, VehicleDetailID.Value);
            EWayBillController eWayBill = new EWayBillController();
            List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);

            foreach (var invoiceLstItem in incoiveNolist)
            {
                EWBId = _orderservice.CheckRetEWayBillExist(OrderID, invoiceLstItem);
                if (EWBId == 0)
                {
                    RetDetailsForEWB data = lstInvoice.Where(x => x.InvoiceNumberWithDate == invoiceLstItem).FirstOrDefault();
                    RetEWayBill response = eWayBill.GenerateEWB(data, OrderID, data.InvoiceNumber, userId);
                    if (!string.IsNullOrEmpty(response.EWayBillNumber.ToString()))
                    {
                        // result = data.InvoiceNumber + ", " + result;
                        result = invoiceLstItem + ", " + result;
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // 25 May,2021 Sonal Gandhi
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

            List<RetChallanDetailForEWB> lstChallanDetails = _orderservice.GetRetChallanDetailForEWB(ChallanID, TransportID.Value, VehicleDetailID.Value);

            EWayBillChallanController eWayBill = new EWayBillChallanController();
            List<string> challanNolist = _orderservice.GetListOfChallan(ChallanID);

            foreach (var challanNumber in challanNolist)
            {
                EWBId = _orderservice.CheckRetEWayBillChallanExist(ChallanID, challanNumber);
                if (EWBId == 0)
                {
                    List<RetChallanItemForEWB> lstChallanItems = _orderservice.GetRetChallanItemForEWB(ChallanID, challanNumber);
                    RetEWayBillChallan response = eWayBill.GenerateEWB(lstChallanDetails[0], lstChallanItems, userId);
                    if (!string.IsNullOrEmpty(response.EWayBillNumber.ToString()))
                        result = result + "  " + response.ChallanNumber;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        //Print Pouch Barcode
        [HttpPost]
        public ActionResult PrintPouchBarcode(List<RetOrderPackList> data, long godown, string date, long PouchSize)
        {
            if (PouchSize != 0)
            {

                if (PouchSize == 220 || PouchSize == 260 || PouchSize == 290 || PouchSize == 320 || PouchSize == 340)
                {
                    try
                    {
                        LocalReport lr = new LocalReport();
                        string path = "";
                        string MonthDat = "";
                        string yr1 = DateTime.Now.Year.ToString();
                        string Batchno = DateTime.Now.Month.ToString() + yr1.Substring(2, yr1.Length - 2);

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
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();

                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i].CategoryID != 9)
                            {
                                if (data[i].ContentValue == null || data[i].NutritionValue == null)
                                {

                                    ProductBarcode obj = new ProductBarcode();
                                    for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                                    {
                                        BestBeforeMonth promonth = new BestBeforeMonth();
                                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                        DateTime theDate = DateTime.Now;
                                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                        //  const int MaxLength = 22;
                                        const int MaxLength = 40;
                                        obj.ProductName = data[i].ProductName;

                                        if (obj.ProductName.Length > MaxLength)
                                        {
                                            obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                        }
                                        obj.QTY = data[i].SKU;
                                        obj.MRP = data[i].MRP.ToString("0.00");
                                        //  obj.Productbarcode = data[i].ProductBarcode;
                                        obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                        obj.MonthDate = MonthDat;
                                        obj.NoofBarcode = data[i].QuantityPackage;
                                        obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");
                                        // obj.Batch = Batchno;
                                        obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                        obj.GodownCode = godowndetails.GodownCode;
                                        obj.AddressLine1 = godowndetails.GodownAddress1;
                                        obj.AddressLine2 = godowndetails.GodownAddress2;
                                        obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                        obj.PhoneNo = godowndetails.GodownPhone;
                                        obj.GodownName = godowndetails.GodownName;
                                        LabelData.Add(obj);
                                    }
                                }
                            }
                        }

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
                        LocalReport lr = new LocalReport();
                        string path = "";
                        string MonthDat = "";
                        string yr1 = DateTime.Now.Year.ToString();
                        string Batchno = DateTime.Now.Month.ToString() + yr1.Substring(2, yr1.Length - 2);

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
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();

                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i].CategoryID != 9)
                            {
                                if (data[i].ContentValue == null || data[i].NutritionValue == null)
                                {

                                    ProductBarcode obj = new ProductBarcode();
                                    for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                                    {
                                        BestBeforeMonth promonth = new BestBeforeMonth();
                                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                        DateTime theDate = DateTime.Now;
                                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                        //  const int MaxLength = 22;
                                        const int MaxLength = 40;
                                        obj.ProductName = data[i].ProductName;

                                        if (obj.ProductName.Length > MaxLength)
                                        {
                                            obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                        }
                                        obj.QTY = data[i].SKU;
                                        obj.MRP = data[i].MRP.ToString("0.00");
                                        //  obj.Productbarcode = data[i].ProductBarcode;
                                        obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                        obj.MonthDate = MonthDat;
                                        obj.NoofBarcode = data[i].QuantityPackage;
                                        obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");
                                        // obj.Batch = Batchno;
                                        obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                        obj.GodownCode = godowndetails.GodownCode;
                                        obj.AddressLine1 = godowndetails.GodownAddress1;
                                        obj.AddressLine2 = godowndetails.GodownAddress2;
                                        obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                        obj.PhoneNo = godowndetails.GodownPhone;
                                        obj.GodownName = godowndetails.GodownName;
                                        LabelData.Add(obj);
                                    }
                                }
                            }
                        }

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



            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }


        //Print Pouch Content Barcode
        [HttpPost]
        public ActionResult PrintPouchContentBarcode(List<RetOrderPackList> data, long godownid, string date, long PouchSize)
        {
            if (PouchSize != 0)
            {
                if (PouchSize == 220 || PouchSize == 260 || PouchSize == 290 || PouchSize == 320 || PouchSize == 340)
                {
                    try
                    {
                        LocalReport lr = new LocalReport();
                        string path = "";
                        string MonthDat = "";
                        string yr1 = DateTime.Now.Year.ToString();
                        string Batchno = DateTime.Now.Month.ToString() + yr1.Substring(2, yr1.Length - 2);

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
                        godowndetails = _productservice.GetGodownDetailsByGodownID(godownid);
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();

                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i].CategoryID != 9)
                            {
                                if (data[i].ContentValue != null || data[i].NutritionValue != null)
                                {
                                    ProductBarcode obj = new ProductBarcode();
                                    for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                                    {
                                        BestBeforeMonth promonth = new BestBeforeMonth();
                                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                        DateTime theDate = DateTime.Now;
                                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                        // const int MaxLength = 22;
                                        const int MaxLength = 40;
                                        obj.ProductName = data[i].ProductName;
                                        if (obj.ProductName.Length > MaxLength)
                                        {
                                            obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                        }
                                        obj.QTY = data[i].SKU;
                                        obj.MRP = data[i].MRP.ToString("0.00");
                                        obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                        obj.MonthDate = MonthDat;
                                        obj.NoofBarcode = data[i].QuantityPackage;
                                        obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");
                                        //   obj.Batch = Batchno;
                                        obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                        obj.GodownCode = godowndetails.GodownCode;
                                        obj.AddressLine1 = godowndetails.GodownAddress1;
                                        obj.AddressLine2 = godowndetails.GodownAddress2;
                                        obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                        obj.PhoneNo = godowndetails.GodownPhone;
                                        obj.GodownName = godowndetails.GodownName;
                                        obj.NutritionValue = data[i].NutritionValue;
                                        obj.ContentValue = data[i].ContentValue;
                                        LabelData.Add(obj);
                                    }
                                }
                            }
                        }

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
                else
                {
                    try
                    {
                        LocalReport lr = new LocalReport();
                        string path = "";
                        string MonthDat = "";
                        string yr1 = DateTime.Now.Year.ToString();
                        string Batchno = DateTime.Now.Month.ToString() + yr1.Substring(2, yr1.Length - 2);

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
                        godowndetails = _productservice.GetGodownDetailsByGodownID(godownid);
                        List<ProductBarcode> LabelData = new List<ProductBarcode>();

                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i].CategoryID != 9)
                            {
                                if (data[i].ContentValue != null || data[i].NutritionValue != null)
                                {
                                    ProductBarcode obj = new ProductBarcode();
                                    for (int j = 0; j < Convert.ToInt32(data[i].QuantityPackage); j++)
                                    {
                                        BestBeforeMonth promonth = new BestBeforeMonth();
                                        promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                                        long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                                        long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                                        DateTime theDate = DateTime.Now;
                                        DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                                        MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                                        // const int MaxLength = 22;
                                        const int MaxLength = 40;
                                        obj.ProductName = data[i].ProductName;
                                        if (obj.ProductName.Length > MaxLength)
                                        {
                                            obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                                        }
                                        obj.QTY = data[i].SKU;
                                        obj.MRP = data[i].MRP.ToString("0.00");
                                        obj.BarcodeImage = CreateEan13(data[i].ProductBarcode);
                                        obj.MonthDate = MonthDat;
                                        obj.NoofBarcode = data[i].QuantityPackage;
                                        obj.DatePackaging = DateTime.Now.ToString("dd/MM/yyyy");
                                        //   obj.Batch = Batchno;
                                        obj.Batch = godowndetails.GodownCode + "" + Batchno.ToString();
                                        obj.GodownCode = godowndetails.GodownCode;
                                        obj.AddressLine1 = godowndetails.GodownAddress1;
                                        obj.AddressLine2 = godowndetails.GodownAddress2;
                                        obj.FSSAINo = godowndetails.GodownFSSAINumber;
                                        obj.PhoneNo = godowndetails.GodownPhone;
                                        obj.GodownName = godowndetails.GodownName;
                                        obj.NutritionValue = data[i].NutritionValue;
                                        obj.ContentValue = data[i].ContentValue;
                                        LabelData.Add(obj);
                                    }
                                }
                            }
                        }

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
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }


        // 18 Jan, 2022 Piyush Limbani
        public PartialViewResult TotalPouchGodownWiseList(string date)
        {
            string id = Session["PLOrderID"].ToString();
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            //     ViewBag.Godown = _productservice.GetAllRetGodownName();
            List<TotalPouchListGodownWise> objModel = new List<TotalPouchListGodownWise>();
            if (!string.IsNullOrEmpty(id))
            {
                objModel = _orderservice.GetTotalPouchGodownWiseList(id, date);
            }
            return PartialView(objModel);
        }

        // 04 Feb, 2022 Piyush Limbani
        public ActionResult ExportExcelDataForBarcode(long GodownID, string date)
        {
            string id = Session["PLOrderID"].ToString();



            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }
            List<RetOrderPackList> data = new List<RetOrderPackList>();
            if (!string.IsNullOrEmpty(id))
            {
                data = _orderservice.GetOrderPackListForPrint(id, date);
            }
            string MonthDat = "";
            string yr1 = DateTime.Now.Year.ToString();

            // Changes By Dhruvik 28-01-2023
            string Batchno = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MM") + yr1.Substring(2, yr1.Length - 2);
            // Changes By Dhruvik 28-01-2023

            Godown_Mst godowndetails = new Godown_Mst();
            godowndetails = _productservice.GetGodownDetailsByGodownID(GodownID);
            List<ProductBarcodeData> ListData = new List<ProductBarcodeData>();
            for (int i = 0; i < data.Count; i++)
            {
                ProductBarcodeData obj = new ProductBarcodeData();
                if (data[i].PouchSize != 0)
                {
                    BestBeforeMonth promonth = new BestBeforeMonth();
                    promonth = _productservice.GetMonthByProductID(Convert.ToInt64(data[i].ProductID));
                    long yr = Convert.ToInt64(promonth.MonthNumber) / 12;
                    long month = Convert.ToInt64(promonth.MonthNumber) % 12;
                    DateTime theDate = DateTime.Now;
                    DateTime yearInTheFuture = theDate.AddDays(-1).AddMonths(Convert.ToInt32(month)).AddYears(Convert.ToInt32(yr));
                    MonthDat = yearInTheFuture.ToString("dd/MM/yyyy");
                    //obj.PouchSize = data[i].PouchSize;



                    string[] pouchName = data[i].PouchName.Split('-');
                    obj.PouchSizestr = pouchName[0];



                    //obj.PouchName = pname;
                    obj.NoofBarcodes = data[i].QuantityPackage;
                    obj.SKU = data[i].ProductBarcode;
                    obj.ProductName = data[i].ProductName;
                    const int MaxLength = 40;
                    if (obj.ProductName.Length > MaxLength)
                    {
                        obj.ProductName = obj.ProductName.Substring(0, MaxLength);
                    }
                    obj.NetWeight = data[i].SKU;
                    obj.BatchNo = godowndetails.GodownCode + "" + Batchno.ToString();
                    obj.DateOfPacking = DateTime.Now.ToString("dd/MM/yyyy");
                    obj.MRP = data[i].MRP.ToString("0.00");
                    obj.PGM = data[i].GramPerKG.ToString("0.00");
                    obj.BestBefore = MonthDat;
                    obj.Ingredients = data[i].ContentValue;
                    // obj.NutritionalFacts = data[i].NutritionValue;
                    obj.Protein = data[i].Protein;
                    obj.Fat = data[i].Fat;
                    obj.Carbohydrate = data[i].Carbohydrate;
                    obj.TotalEnergy = data[i].TotalEnergy;
                    obj.Information = data[i].Information;
                    ListData.Add(obj);
                }
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
                Information = x.Information

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

        //    List<ExportToExcelInvoice> lst = lstInvoice.Select(x => new ExportToExcelInvoice()
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

        //        //29th April,2021 Sonal Gandhi
        //        IRN = x.IRN,
        //        InvoiceNumber = x.InvoiceNumber,
        //        InvoiceNumberWithDate = x.InvoiceNumberWithDate
        //    }).ToList();

        //    foreach (var invoiceLstItem in incoiveNolist)
        //    {
        //        EWBId = _orderservice.CheckRetEWayBillExist(OrderID, invoiceLstItem);
        //        if (EWBId == 0)
        //        {
        //            var data = lst.Where(x => x.InvoiceNumberWithDate == invoiceLstItem).ToList();
        //            RetEWayBill response = eWayBill.GenerateEWB(data, OrderID, data[0].InvoiceNumber, userId);
        //        }
        //    }
        //    return View();
        //}



        // 21-06-2022

        [HttpPost]
        public ActionResult IsDonePrintPackList(long ProductQtyID, string date)
        {
            bool respose = _orderservice.IsDonePrintPackList(ProductQtyID, date);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        // 27-06-2022
        [HttpPost]
        public ActionResult IsDoneCheckList(long OrderID)
        {
            bool respose = _orderservice.IsDoneCheckList(OrderID);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }
    }
}