using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;
using vb.Service;
using vb.Service.Common;
using WAProAPI;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class DeliveryController : Controller
    {
        private IDeliveryService _deliveryservice;
        private ICommonService _commonservice;
        private IOrderService _orderservice;
        private ICustomerService _customerservice;


        public long OrderIdvalue;
        List<OrderQtyInvoiceList> Lstdata;
        List<OrderQtyInvoiceList> Lstdatainvoice;

        WAAPI wa = new WAAPI();

        ECredentials eCred = new ECredentials();

        public DeliveryController(IDeliveryService deliveryservice, ICommonService commonservice, IOrderService orderservice, ICustomerService customerservice)
        {
            _deliveryservice = deliveryservice;
            _commonservice = commonservice;
            _orderservice = orderservice;
            _customerservice = customerservice;
        }

        // GET: /wholesale/Delivery/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendingDelivery()
        {
            List<PendingDeliveryListResponse> objModel = _deliveryservice.GetAllPendingDeliveryList();
            return View(objModel);
        }

        [HttpPost]
        public ActionResult UpdatePendingDelivery(List<OrderPendingRequest> data)
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
                    bool respose = _deliveryservice.UpdatePendingDelivery(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemoveDelivery(string data)
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
                    bool respose = _deliveryservice.RemovePendingDeliveryOfOrder(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateChequeDetailsForPayment(DeliveryStatusListResponse data)
        {
            try
            {
                bool respose = _deliveryservice.UpdateChequeDetailsForPayment(data);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdatePayment(List<DeliveryStatusListResponse> data)
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
                    if (data[0].DeliveryType == 0)
                    {
                        List<OrderPendingRequest> lst = new List<OrderPendingRequest>();
                        foreach (var item in data)
                        {
                            OrderPendingRequest obj = new OrderPendingRequest();
                            if (item.IsDelivered == true)
                            {
                                obj.VehicleNo = 0;
                                obj.InvoiceNumber = item.InvoiceNumber;
                                obj.DeliveryID = item.DeliveryID;
                                lst.Add(obj);
                            }
                        }
                        UpdatePendingDelivery(lst);
                        bool exist = _deliveryservice.CheckDeliveryAllocationVehicle0Exist(Convert.ToDateTime(DateTime.Now));
                        if (exist == false)
                        {
                            int suc = _deliveryservice.InsertDeliveryAllocationVehicle(Convert.ToDateTime(DateTime.Now));
                        }
                    }
                    long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    bool respose = _deliveryservice.UpdatePayment(data, UserID);

                    foreach (var item in data)
                    {
                        if (item.ByCash > 0)
                        {
                            SendMessageData sendmessageData = new SendMessageData();

                            sendmessageData.ByCash = item.ByCash;
                            sendmessageData.ByCheque = item.ByCheque;
                            sendmessageData.BankName = item.BankName;
                            sendmessageData.BankBranch = item.BankBranch;
                            sendmessageData.ChequeNo = item.ChequeNo;
                            sendmessageData.ChequeDate = item.ChequeDate;
                            sendmessageData.IFCCode = item.IFCCode;
                            sendmessageData.ByCard = item.ByCard;
                            sendmessageData.BankNameForCard = item.BankNameForCard;
                            sendmessageData.TypeOfCard = item.TypeOfCard;
                            sendmessageData.ByOnline = item.ByOnline;
                            sendmessageData.BankNameForOnline = item.BankNameForOnline;
                            sendmessageData.UTRNumber = item.UTRNumber;
                            sendmessageData.OnlinePaymentDate = item.OnlinePaymentDate;
                            sendmessageData.InvoiceNumber = item.InvoiceNumber;

                            List<CustomerAddressViewModel> customerDetails = _customerservice.GetCustomerAddressListByCustomerID(item.CustomerID);
                            if (customerDetails.Count > 0)
                            {
                                CustomerDetailModel customerDetail = new CustomerDetailModel();
                                customerDetail.Name = customerDetails[0].Name;
                                customerDetail.Email = customerDetails[0].Email;
                                customerDetail.CellNo = customerDetails[0].CellNo;

                                try
                                {
                                    string res = wa.SendEmailToCustomer(customerDetail, sendmessageData);
                                    string msg = "From Viraki Brothers...";
                                    wa.SendWAMessage(msg, customerDetails[0].CellNo);
                                }
                                catch (Exception)
                                {
                                }

                            }
                        }
                    }

                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeliveryStatus()
        {
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            ViewBag.DeliveryPerson = _commonservice.GetAllDeliveryPersonList();
            ViewBag.VehicleNo = _commonservice.GetAllVehicleNoList();
            return View();
        }

        public ActionResult TempoSheet()
        {
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            ViewBag.DeliveryPerson = _commonservice.GetAllDeliveryPersonList();
            ViewBag.VehicleNo = _commonservice.GetAllVehicleNoList();
            ViewBag.TempoNumber = _commonservice.GetAllTempoNumberList();
            return View();
        }

        [HttpPost]
        public PartialViewResult DeliveryStatusList(DeliveryStatusListResponse model)
        {
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            DeliverAllocation obj = new DeliverAllocation();
            obj.lstDelAllocation = _deliveryservice.GetDeliveryStatusList(model.CreatedOn);
            DeliveryStatusListResponse objModel2 = _deliveryservice.GetDeliveryInfoList(model.VehicleNo, model.CreatedOn);
            obj.VehicleNo = objModel2.VehicleNo;
            obj.AreaID = objModel2.AreaID;
            obj.DeliveryAllocationID = objModel2.DeliveryAllocationID;
            obj.TempoNo = objModel2.TempoNo;
            obj.DeliveryPerson1 = objModel2.DeliveryPerson1;
            obj.DeliveryPerson2 = objModel2.DeliveryPerson2;
            obj.DeliveryPerson3 = objModel2.DeliveryPerson3;
            obj.DeliveryPerson4 = objModel2.DeliveryPerson4;
            return PartialView(obj);
        }

        [HttpGet]
        public ActionResult TempoSummaryList()
        {
            List<TempoSummary> obj = _deliveryservice.GetTempoSummaryList(Convert.ToDateTime(DateTime.Now));
            return View(obj);
        }

        public ActionResult ExportExcelTempoSummary()
        {
            List<TempoSummary> obj = _deliveryservice.GetTempoSummaryList(Convert.ToDateTime(DateTime.Now));
            List<TempoSummary> lst = new List<TempoSummary>();
            for (int i = 0; i < obj.Count; i++)
            {
                TempoSummary ts = new TempoSummary();
                string newstring = obj[i].VehicleSummary.Replace("<br/>", "\r\n");
                ts.VehicleSummary = newstring;
                lst.Add(ts);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lst));
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
                        workSheet.Columns().Width = 45;
                    }
                }
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "TempoSummary" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
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
            //foreach (PropertyInfo prop in Props)
            //{
            //    //Defining type of data column gives proper data table 
            //    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
            //    //Setting column names as Property names
            //    dataTable.Columns.Add(prop.Name, type);
            //}
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
        public PartialViewResult TempoSheetList(DeliveryStatusListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            DeliverAllocation obj = new DeliverAllocation();
            obj.lstDelAllocation = _deliveryservice.GetTempoSheetList(model.VehicleNo, model.CreatedOn);
            DeliveryStatusListResponse objModel2 = _deliveryservice.GetTempoInfoList(model.VehicleNo, model.CreatedOn);
            obj.VehicleNo = objModel2.VehicleNo;
            obj.AreaID = objModel2.AreaID;
            obj.DeliveryAllocationID = objModel2.DeliveryAllocationID;
            obj.TempoNo = objModel2.TempoNo;
            obj.DeliveryPerson1 = objModel2.DeliveryPerson1;
            obj.DeliveryPerson2 = objModel2.DeliveryPerson2;
            obj.DeliveryPerson3 = objModel2.DeliveryPerson3;
            obj.DeliveryPerson4 = objModel2.DeliveryPerson4;
            return PartialView(obj);
        }

        [HttpPost]
        public ActionResult AddDeliveryAllocation(DeliveryStatusListResponse data)
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
                    Int64 respose = _deliveryservice.AddDeliveryAllocation(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InvoiceList(long CustomerID)
        {
            try
            {
                DeliverAllocation obj = new DeliverAllocation();
                obj.lstDelAllocation = _deliveryservice.GetPendingInvoiceListForPrint(CustomerID);
                return Json(obj.lstDelAllocation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintTempoSheet(DeliverAllocationPrint data)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/TempoSheet.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/TempoSheet.rdlc");
                }
                lr.ReportPath = path;
                List<DeliverAllocationPrintList> orderdata = new List<DeliverAllocationPrintList>();
                DeliverAllocationPrintList lstitem = new DeliverAllocationPrintList();
                List<DeliveryStatusListPrint> lst = new List<DeliveryStatusListPrint>();
                lstitem.Area = data.AreaID;
                lstitem.VehicleNo = data.VehicleNo;
                lstitem.TempoNo = data.TempoNo;
                lstitem.DeliveryPerson1 = data.DeliveryPerson1;
                lstitem.DeliveryPerson2 = data.DeliveryPerson2;
                lstitem.DeliveryPerson3 = data.DeliveryPerson3;
                lstitem.DeliveryPerson4 = data.DeliveryPerson4;
                orderdata.Add(lstitem);

                var logFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/InvoiceNumberWiseBill/log.txt"));
                using (StreamWriter w = System.IO.File.AppendText(logFilePath))
                {
                    eCred.Log(string.Format("Print Tempo Sheet Method"), w);
                }

                //Send Email To Customer 
                //if (data.SendEmail == true)
                //{
                //    List<customergroupbycustomer> groupedCustomerList = new List<customergroupbycustomer>();
                //    foreach (var item in data.lstDelAllocation)
                //    {                      
                //        groupedCustomerList = data.lstDelAllocation.GroupBy(x => new { x.Customer, x.Email, x.MobileNumber }).Select(x => new customergroupbycustomer() { Lstdelivery = x.ToList(), Customer = x.Key.Customer, Email = x.Key.Email, MobileNumber = x.Key.MobileNumber }).ToList();
                //    }
                //    foreach (var item1 in groupedCustomerList)
                //    {                       
                //        List<string> LstInvoiceNo = item1.Lstdelivery.Select(x => x.InvoiceNumber).ToList();
                //        List<decimal> Total = item1.Lstdelivery.Select(x => x.PaymentAmount).ToList();
                //        decimal OrderTotal = Total.Sum();                     
                //        List<string> LstPDF = item1.Lstdelivery.Select(x => x.PDFName).Distinct().ToList();
                //        string PdfPath = Server.MapPath("~/bill/");
                //        if (item1.Email != null)
                //        {
                //            SendEmailToCustomer(item1.Customer, item1.Email, string.Join(",", LstInvoiceNo), OrderTotal, LstPDF, PdfPath);
                //        }
                //        //if (item1.MobileNumber != null)
                //        //{
                //        //     SendSMSToCustomer(item1.Customer, item1.Email, string.Join(",", LstInvoiceNo), OrderTotal, LstPDF, PdfPath);
                //        //}                       
                //    }
                //}
                //Send Email To Customer 

                //Send Email To Customer 02 Sep. 2020 Piyush Limbani
                if (data.SendEmail == true)
                {
                    List<customergroupbycustomer> groupedCustomerList = new List<customergroupbycustomer>();
                    foreach (var item in data.lstDelAllocation)
                    {
                        groupedCustomerList = data.lstDelAllocation.GroupBy(x => new { x.Customer, x.Email, x.MobileNumber }).Select(x => new customergroupbycustomer() { Lstdelivery = x.ToList(), Customer = x.Key.Customer, Email = x.Key.Email, MobileNumber = x.Key.MobileNumber }).ToList();
                    }
                    foreach (var item1 in groupedCustomerList)
                    {
                        List<string> LstInvoiceNo = item1.Lstdelivery.Select(x => x.InvoiceNumber).ToList();
                        List<decimal> Total = item1.Lstdelivery.Select(x => x.PaymentAmount).ToList();
                        decimal OrderTotal = Total.Sum();
                        List<string> LstPDF = item1.Lstdelivery.Select(x => x.PDFName).Distinct().ToList();
                        string PdfPath = Server.MapPath("~/InvoiceNumberWiseBill/");
                        if (item1.Email != null)
                        {

                            var logFilePath1 = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/InvoiceNumberWiseBill/log.txt"));
                            using (StreamWriter w = System.IO.File.AppendText(logFilePath1))
                            {
                                eCred.Log(string.Format("email not null"), w);
                            }

                            //SendEmailToCustomerOld(item1.Customer, item1.Email, string.Join(",", LstInvoiceNo), OrderTotal, LstPDF, PdfPath);

                            try
                            {
                                SendEmailToCustomer(item1.Customer, item1.Email, LstInvoiceNo, Total, LstPDF, PdfPath, item1.MobileNumber);
                            }
                            catch (Exception ex)
                            {

                                var logFilePath2 = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/InvoiceNumberWiseBill/log.txt"));
                                using (StreamWriter w = System.IO.File.AppendText(logFilePath2))
                                {
                                    eCred.Log(string.Format("email not null" + ex.Message.ToString()), w);
                                }
                            }

                        }
                        //if (item1.MobileNumber != null)
                        //{
                        //     SendSMSToCustomer(item1.Customer, item1.Email, string.Join(",", LstInvoiceNo), OrderTotal, LstPDF, PdfPath);
                        //}                       
                    }
                }
                //Send Email To Customer 02 Sep. 2020 Piyush Limbani

                DataTable header = Common.ToDataTable(orderdata);
                ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                DataTable FoodDT = Common.ToDataTable(data.lstDelAllocation);
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
                string Pdfpathcreate = Server.MapPath("~/TempoSheet/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/TempoSheet/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/TempoSheet/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/TempoSheet/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public static string SendEmailToCustomer(string Customer, string Email, List<string> InvoiceNumber, List<decimal> OrderTotal, List<string> PDF, string PdfPath, string MobileNumber)
        {
            try
            {
                try
                {

                    WriteToFile("strat send email");

                    string logo = "<img src = 'https://system.virakibrothers.com/dist/img/viraki-logo1.png' /><br /><br /><div style = 'border-top:3px solid #41a69a'>&nbsp;</div>";
                    string FromMail = ConfigurationManager.AppSettings["FromMail"];
                    string Tomail = Email;
                    MailMessage mailmessage = new MailMessage();
                    mailmessage.From = new MailAddress(FromMail);
                    mailmessage.Subject = "Order Delivery From Viraki Brothers";
                    // mailmessage.CC.Add(System.Configuration.ConfigurationManager.AppSettings["CCMail"]);                 
                    //mailmessage.Body = logo + "Hello  " + "" + ",<br><br>Your order  Dispatch shortly<br><br>Invoice Number is " + InvoiceNumber + "<br><br>Order Total is " + OrderTotal + "  <br><br>feel free to contact us on virakibrothers@gmail.com if any query.<br><br>"
                    //   + "<br><br>Thank you,<br>Viraki Brothers team";

                    //With Table             
                    string msg = string.Empty;

                    msg += "Hello Patron" + "" +
                        ",<br> We are dispatching goods of the following invoices shortly. <br><br>"
                        + "<table border=1><tr>" +
                 "<th width = 150px>Invoice Number</th>" +
                 "<th width = 150px>Amount</th>" +
              "<tr/>";
                    for (int i = 0; i < InvoiceNumber.Count; i++)
                    {
                        msg += "<tr>" +
                           "<td style=text-align: center;>" + InvoiceNumber[i] + "</td>" +
                           "<td style=text-align: center;>" + OrderTotal[i] + "</td>" +
                           "</tr>";
                    }
                    msg += "<tr><td style=text-align: center;> Total </td>" + "<td style=text-align: center;>" + OrderTotal.Sum() + "</td></tr>";
                    msg += "</table>"
                  + "<br>Invoices are attached here with."
                     + "<br>In case of discrepancy, please contact us on 9769642799 or purchase@virakibrothers.com"
                    + "<br>Looking forward to serve you further. <br>"
                        + "<br>Thank you,<br>Team Viraki Brothers <br>"
                        + logo;
                    //With Table

                    mailmessage.Body = msg;
                    foreach (var item in PDF)
                    {
                        if (item != "" && item != null)
                        {
                            string name = PdfPath + item;
                            mailmessage.Attachments.Add(new Attachment(name));

                            if (!string.IsNullOrEmpty(MobileNumber))
                            {
                                try
                                {
                                    SendMediaMsgJson waMediaMsgBody = new SendMediaMsgJson();
                                    string Attachment = Convert.ToBase64String(System.IO.File.ReadAllBytes(PdfPath + item));
                                    string AttachmentFileName = Path.GetFileName(item);
                                    waMediaMsgBody.base64data = Attachment;
                                    waMediaMsgBody.mimeType = MimeMapping.GetMimeMapping(AttachmentFileName);
                                    waMediaMsgBody.caption = "PFA";
                                    waMediaMsgBody.filename = AttachmentFileName;
                                    TxnRespWithSendMessageDtls txnResp = Task.Run(() => APIMethods.SendMediaMessageAsync("91" + MobileNumber, waMediaMsgBody)).Result;
                                    //TxnRespWithSendMessageDtls txnResp = Task.Run(() => APIMethods.SendMediaMessageAsync("919820435663", waMediaMsgBody)).Result;

                                    string message = "From Viraki Brothers...";
                                    WAAPI wa1 = new WAAPI();
                                    wa1.SendWAMessage(message, MobileNumber);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    mailmessage.To.Add(Tomail);
                    // mailmessage.CC.Add(System.Configuration.ConfigurationManager.AppSettings["CCMail"]);
                    mailmessage.IsBodyHtml = true;
                    var smtp = new System.Net.Mail.SmtpClient();
                    {
                        smtp = GetSMPTP(smtp);
                    }
                    try
                    {
                        smtp.Send(mailmessage);
                        WriteToFile("email sent succ");
                    }
                    catch (Exception ex)
                    {
                        WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                    }
                }
                catch (Exception ex)
                {
                    WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                }
                return "Send Email Sent succesfully..";
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                return "Sending Email fail due to " + ex.Message.ToString() + " & " + ex.Message.ToString();
            }
        }

        //public static string SendEmailToCustomerOld(string Customer, string Email, string InvoiceNumber, decimal OrderTotal, List<string> PDF, string PdfPath)
        //{
        //    try
        //    {
        //        try
        //        {
        //            string logo = "<img src = 'https://system.virakibrothers.com/dist/img/viraki-logo1.png' /><br /><br /><div style = 'border-top:3px solid #41a69a'>&nbsp;</div>";
        //            string FromMail = ConfigurationManager.AppSettings["FromMail"];
        //            string Tomail = Email;
        //            MailMessage mailmessage = new MailMessage();
        //            mailmessage.From = new MailAddress(FromMail);
        //            mailmessage.Subject = "Order Delivery From Viraki Brothers";
        //            // mailmessage.CC.Add(System.Configuration.ConfigurationManager.AppSettings["CCMail"]);                 
        //            //mailmessage.Body = logo + "Hello  " + "" + ",<br><br>Your order  Dispatch shortly<br><br>Invoice Number is " + InvoiceNumber + "<br><br>Order Total is " + OrderTotal + "  <br><br>feel free to contact us on virakibrothers@gmail.com if any query.<br><br>"
        //            //   + "<br><br>Thank you,<br>Viraki Brothers team";

        //            //With Table
        //            mailmessage.Body = logo + "Hello Patron" + "" +
        //                ",<br> We are dispatching goods of the following invoices shortly. <br><br>"
        //                + "<table border=1><tr>" +
        //         "<th>Invoice Number</th>" +
        //         "<th>Amount</th>" +
        //      "<tr/><tr>" +
        //         "<td>" + InvoiceNumber + "</td>" +
        //         "<td>" + OrderTotal + "</td>" +
        //         "</tr></table>"
        //      + "<br>Invoices are attached here with."
        //         + "<br>In case of discrepancy, please contact us on 9769642799 or purchase@virakibrothers.com"
        //        + "<br>Looking forward to serve you further. <br>"
        //                //+ "<br><br>feel free to contact us on virakibrothers@gmail.com if any query.<br><br>"
        //            + "<br>Thank you,<br>Team Viraki Brothers";
        //            //With Table
        //            foreach (var item in PDF)
        //            {
        //                if (item != "" && item != null)
        //                {
        //                    string name = PdfPath + item;
        //                    mailmessage.Attachments.Add(new Attachment(name));
        //                }
        //            }
        //            mailmessage.To.Add(Tomail);
        //            // mailmessage.CC.Add(System.Configuration.ConfigurationManager.AppSettings["CCMail"]);
        //            mailmessage.IsBodyHtml = true;
        //            var smtp = new System.Net.Mail.SmtpClient();
        //            {
        //                smtp = GetSMPTP(smtp);
        //            }
        //            try
        //            {
        //                smtp.Send(mailmessage);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        catch
        //        {
        //        }
        //        return "Send Email Sent succesfully..";
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
        //        return "Sending Email fail due to " + ex.Message.ToString() + " & " + ex.Message.ToString();
        //    }
        //}

        #region Email SMTP configuration
        public static System.Net.Mail.SmtpClient GetSMPTP(System.Net.Mail.SmtpClient OLDSMTP)
        {
            string FromMail = ConfigurationManager.AppSettings["FromMail"];
            string FromPassword = ConfigurationManager.AppSettings["FromPassword"];
            //OLDSMTP.Host = "smtp.gmail.com";

            if (FromMail == "purchase@virakibrothers.com")
            //if (FromMail == "sales@virakibrothers.com")
            {
                OLDSMTP.Host = "smtp.office365.com";
            }
            else
            {
                OLDSMTP.Host = "smtp.gmail.com";
            }
            OLDSMTP.Port = 587;
            OLDSMTP.EnableSsl = true;
            OLDSMTP.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            OLDSMTP.Credentials = new NetworkCredential(FromMail, FromPassword);
            OLDSMTP.Timeout = 20000;
            return OLDSMTP;
        }
        #endregion

        private static void WriteToFile(string text)
        {
            var logFilePath1 = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/InvoiceNumberWiseBill/log.txt"));
            using (StreamWriter writer = new StreamWriter(logFilePath1, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }








        [HttpPost]
        public ActionResult DeleteOrderQty(string InvoiceNumber, bool IsDelete)
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
                    bool respose = _deliveryservice.DeleteOrderQty(InvoiceNumber, IsDelete);

                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SavePackedBy(PackedByViewModel data)
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
                bool respose = _deliveryservice.SavePackedBy(data);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintPackDetail(string Tray, string Bag, string Jabla, string PackBag)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/PackDetails.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/PackDetails.rdlc");
                //}



                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/PackDetails.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/PackDetails.rdlc");
                }

                lr.ReportPath = path;
                List<PackedBy_Mst> LabelData = new List<PackedBy_Mst>();
                PackedBy_Mst obj = new PackedBy_Mst();
                if (Tray != "" && Bag == "" && Jabla == "" && PackBag == "")
                {
                    obj.Tray = "Tray" + "-" + Tray;
                }
                else if (Tray != "")
                {
                    obj.Tray = "Tray" + "-" + Tray + ",";
                }
                else
                {
                    //obj.Tray = "Tray" + "-" + "0";
                    obj.Tray = "";
                }
                if (Bag != "" && Jabla == "" && PackBag == "")
                {
                    obj.Bag = "Bag" + "-" + Bag;
                }
                else if (Bag != "")
                {
                    obj.Bag = "Bag" + "-" + Bag + ",";
                }
                else
                {
                    // obj.Bag = "Bag" + "-" + "0";
                    obj.Bag = "";
                }
                if (Jabla != "" && PackBag == "")
                {
                    obj.Jabla = "Zabla" + "-" + Jabla;
                }
                else if (Jabla != "")
                {
                    obj.Jabla = "Zabla" + "-" + Jabla + ",";
                }
                else
                {
                    // obj.Jabla = "Zabla" + "-" + "0";
                    obj.Jabla = "";
                }
                if (PackBag != "")
                {
                    obj.PackBag = "Pack Bag" + "-" + PackBag;
                }
                else
                {
                    // obj.PackBag = "Pack Bag" + "-" + "0";
                    obj.PackBag = "";
                }
                LabelData.Add(obj);
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
                    //     "  <PageWidth>11in</PageWidth>" +
                    //"  <PageHeight>8.7in</PageHeight>" +
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
                string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                string Pdfpathcreate = Server.MapPath("~/PackDetails/" + name1);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                //string url = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    url = "http://" + Request.Url.Host + ":6551/PackDetails/" + name1;
                //}
                //else
                //{
                //    url = "http://" + Request.Url.Host + "/PackDetails/" + name1;
                //}



                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/PackDetails/" + name1;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/PackDetails/" + name1;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/PackDetails/" + name1;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintCustomerName(string CustomerName)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    path = "Report/CustomerName.rdlc";
                //}
                //else
                //{
                //    path = Server.MapPath("~/Report/CustomerName.rdlc");
                //}


                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/CustomerName.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/CustomerName.rdlc");
                }
                lr.ReportPath = path;
                List<Customer_Mst> LabelData = new List<Customer_Mst>();
                Customer_Mst obj = new Customer_Mst();
                obj.CustomerName = CustomerName;
                LabelData.Add(obj);
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
                    //     "  <PageWidth>11in</PageWidth>" +
                    //"  <PageHeight>8.7in</PageHeight>" +
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
                string name1 = DateTime.Now.Ticks.ToString() + ".pdf";
                string Pdfpathcreate = Server.MapPath("~/CustomerName/" + name1);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();
                //string url = "";
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    url = "http://" + Request.Url.Host + ":6551/CustomerName/" + name1;
                //}
                //else
                //{
                //    url = "http://" + Request.Url.Host + "/CustomerName/" + name1;
                //}




                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/CustomerName/" + name1;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/CustomerName/" + name1;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/CustomerName/" + name1;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }







        // 01 Sep. 2020 Piyush Limbani
        [HttpPost]
        public ActionResult GenerateOriginalInvoice(GenerateOriginalInvoice data)
        {
            try
            {
                {
                    foreach (var item2 in data.lstInvoice)
                    {
                        long OrderID = item2.OrderID;
                        string InvoiceNumber = item2.InvoiceNumber;

                        OrderIdvalue = OrderID;
                        LocalReport lr = new LocalReport();
                        string path = "";
                        decimal InvoiceTotal = 0;
                        long ProductCount = 0;
                        var orderdata = _orderservice.GetInvoiceForOrderPrint(OrderID);
                        DataTable header = Common.ToDataTable(orderdata);
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
                        ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                        Lstdatainvoice = new List<OrderQtyInvoiceList>();
                        int dividerdata = 0;
                        int ordergroup = 0;
                        List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);
                        //for (int i = 1; i <= orderdata[0].NoofInvoiceint; i++)
                        for (int i = 1; i <= 1; i++)
                        {
                            Lstdata = _orderservice.GetInvoiceForOrderItemPrint(OrderIdvalue, 1, InvoiceNumber);
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
                                    if (Foodzero.Count < 15)
                                    {
                                        for (int s = Foodzero.Count; s < 15; s++)
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
                                            Lstdatainvoice.Add(item);
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
                                            item.GrandTotal = item.InvTotal;
                                            ProductCount++;
                                            if (Count.Count == ProductCount && item.InvoiceTitleHeader == "ORIGINAL FOR RECIPIENT")
                                            {
                                                ProductCount = 0;
                                                InvoiceTotal += item.InvTotal;
                                            }
                                            item.InvoiceTotal = InvoiceTotal;
                                            item.OrderTotal = item.OrderTotal;
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
                                            Lstdatainvoice.Add(item);
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
                        Lstdatainvoice[0].InvoiceTotal = InvoiceTotal;
                        DataTable FoodDT = Common.ToDataTable(Lstdatainvoice);
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
                        string Pdfpathcreate = Server.MapPath("~/InvoiceNumberWiseBill/" + name);
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                        Writer.Write(renderedBytes);
                        Writer.Flush();
                        Writer.Close();

                        // save pdf
                        string PDFName = _orderservice.CheckPDFIsExistForInvoiceNumber(OrderID, InvoiceNumber);
                        if (PDFName == "")
                        {
                            bool respose = _orderservice.UpdateInvoiceNameByOrderIDAndInvoiceNumber(name, OrderID, InvoiceNumber);
                        }
                        // save pdf          
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
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
        // 01 Sep. 2020 Piyush Limbani

        // 02 Dec 2021 Sonal Gandhi

        public ActionResult DayWiseSalesApproveList()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult DeleteSalesApprovalList(DayWiseSalesApproveList model)
        {
            List<DayWiseSalesApproveList> objlst = _deliveryservice.DeleteSalesApprovalList(model.InvDate);
            return PartialView(objlst);
        }

        public JsonResult UpdateDayWiseSalesApprove(long OrderID, string InvoiceNumber, bool IsApprove)
        {
            bool res = _deliveryservice.UpdateDayWiseSalesApprove(OrderID,InvoiceNumber, IsApprove);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        // 02 Dec 2021 Sonal Gandhi

    }
    public class customergroupbycustomer
    {
        public string Customer { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public List<DeliveryStatusListPrint> Lstdelivery { get; set; }
    }
}