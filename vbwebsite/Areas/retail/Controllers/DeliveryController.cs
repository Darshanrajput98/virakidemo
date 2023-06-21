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
using vb.Data.ViewModel;
using vb.Service;
using vb.Service.Common;
using WAProAPI;

namespace vbwebsite.Areas.retail.Controllers
{
    public class DeliveryController : Controller
    {
        private IRetDeliveryService _deliveryservice;
        private ICommonService _commonservice;
        private IRetOrderService _orderservice;
        private IRetCustomerService _customerservice;

        public long OrderIdvalue;
        List<RetOrderQtyInvoiceList> Lstdatainvoice;
        List<RetOrderQtyInvoiceList> Lstdata;

        WAAPI wa = new WAAPI();

        public DeliveryController(IRetDeliveryService deliveryservice, ICommonService commonservice, IRetOrderService orderservice, IRetCustomerService customerservice)
        {
            _deliveryservice = deliveryservice;
            _commonservice = commonservice;
            _orderservice = orderservice;
            _customerservice = customerservice;
        }

        // GET: /retail/Delivery/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendingDelivery()
        {
            List<RetPendingDeliveryListResponse> objModel = _deliveryservice.GetAllPendingDeliveryList();
            return View(objModel);
        }

        [HttpPost]
        public ActionResult UpdatePendingDelivery(List<RetOrderPendingRequest> data)
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

        public ActionResult TempoSheet()
        {
            ViewBag.AreaList = _commonservice.GetAllRetAreaList();
            ViewBag.DeliveryPerson = _commonservice.GetAllDeliveryPersonList();
            ViewBag.VehicleNo = _commonservice.GetAllRetVehicleNoList();
            ViewBag.TempoNumber = _commonservice.GetAllTempoNumberList();
            return View();
        }

        [HttpPost]
        public PartialViewResult TempoSheetList(RetDeliveryStatusListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllRetCashOption();
            ViewBag.Customer = _orderservice.GetAllRetCustomerName();
            //string[] para = model.VehicleNo.ToString().Split('/');
            RetDeliverAllocation obj = new RetDeliverAllocation();
            //List<DeliveryStatusListResponse> objModel = _deliveryservice.GetDeliveryStatusList(Convert.ToInt64(para[0]), Convert.ToDateTime(para[1]));
            obj.lstDelAllocation = _deliveryservice.GetTempoSheetList(model.VehicleNo, model.CreatedOn);
            RetDeliveryStatusListResponse objModel2 = _deliveryservice.GetTempoInfoList(model.VehicleNo, model.CreatedOn);
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
        public ActionResult AddDeliveryAllocation(RetDeliveryStatusListResponse data)
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
                RetDeliverAllocation obj = new RetDeliverAllocation();
                obj.lstDelAllocation = _deliveryservice.GetPendingInvoiceListForPrint(CustomerID);
                return Json(obj.lstDelAllocation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdatePayment(List<RetDeliveryStatusListResponse> data)
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
                    //New add code
                    if (data[0].DeliveryType == 0)
                    {
                        List<RetOrderPendingRequest> lst = new List<RetOrderPendingRequest>();
                        foreach (var item in data)
                        {
                            RetOrderPendingRequest obj = new RetOrderPendingRequest();
                            if (item.IsDelivered == true)
                            {
                                obj.VehicleNo = 0;
                                obj.InvoiceNumber = item.InvoiceNumber;
                                obj.DeliveryID = item.DeliveryID;
                                obj.OrderID = item.OrderID;
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
                    //New add code
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

                            List<RetCustomerAddressViewModel> customerDetails = _customerservice.GetCustomerAddressListByCustomerID(item.CustomerID);
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

        [HttpPost]
        public ActionResult PrintTempoSheet(RetDeliverAllocationPrint data)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/RetailTempoSheet.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/RetailTempoSheet.rdlc");
                }
                lr.ReportPath = path;
                List<RetDeliverAllocationPrintList> orderdata = new List<RetDeliverAllocationPrintList>();
                RetDeliverAllocationPrintList lstitem = new RetDeliverAllocationPrintList();
                lstitem.Area = data.AreaID;
                lstitem.VehicleNo = data.VehicleNo;
                lstitem.TempoNo = data.TempoNo;
                lstitem.DeliveryPerson1 = data.DeliveryPerson1;
                lstitem.DeliveryPerson2 = data.DeliveryPerson2;
                lstitem.DeliveryPerson3 = data.DeliveryPerson3;
                lstitem.DeliveryPerson4 = data.DeliveryPerson4;
                orderdata.Add(lstitem);

                //Send Email To Customer 02 Sep. 2020 Piyush Limbani
                if (data.SendEmail == true)
                {
                    List<retcustomergroupbycustomer> groupedCustomerList = new List<retcustomergroupbycustomer>();
                    foreach (var item in data.lstDelAllocation)
                    {
                        groupedCustomerList = data.lstDelAllocation.GroupBy(x => new { x.Customer, x.Email, x.MobileNumber }).Select(x => new retcustomergroupbycustomer() { Lstdelivery = x.ToList(), Customer = x.Key.Customer, Email = x.Key.Email, MobileNumber = x.Key.MobileNumber }).ToList();
                    }
                    foreach (var item1 in groupedCustomerList)
                    {
                        List<string> LstInvoiceNo = item1.Lstdelivery.Select(x => x.InvoiceNumber).ToList();
                        List<decimal> Total = item1.Lstdelivery.Select(x => x.PaymentAmount).ToList();
                        decimal OrderTotal = Total.Sum();
                        List<string> LstPDF = item1.Lstdelivery.Select(x => x.PDFName).Distinct().ToList();
                        string PdfPath = Server.MapPath("~/RetailInvoiceNumberWiseBill/");
                        if (item1.Email != null)
                        {
                            try
                            {
                                SendEmailToCustomer(item1.Customer, item1.Email, LstInvoiceNo, Total, LstPDF, PdfPath, item1.MobileNumber);
                            }
                            catch (Exception)
                            {

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
                    string logo = "<img src = 'https://system.virakibrothers.com/dist/img/viraki-logo1.png' /><br /><br /><div style = 'border-top:3px solid #41a69a'>&nbsp;</div>";
                    string FromMail = ConfigurationManager.AppSettings["FromMail"];
                    string Tomail = Email;
                    MailMessage mailmessage = new MailMessage();
                    mailmessage.From = new MailAddress(FromMail);
                    mailmessage.Subject = "Order Delivery From Viraki Brothers";

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
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
                return "Send Email Sent succesfully..";
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message.ToString() + " & " + ex.Message.ToString());
                return "Sending Email fail due to " + ex.Message.ToString() + " & " + ex.Message.ToString();
            }
        }

        #region Email SMTP configuration
        public static System.Net.Mail.SmtpClient GetSMPTP(System.Net.Mail.SmtpClient OLDSMTP)
        {
            string FromMail = ConfigurationManager.AppSettings["FromMail"];
            string FromPassword = ConfigurationManager.AppSettings["FromPassword"];
            //OLDSMTP.Host = "smtp.gmail.com";
            if (FromMail == "purchase@virakibrothers.com")
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
            string path = "C:\\SendemailFacli.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }





        //[HttpPost]
        //public ActionResult RemoveDelivery(string data)
        //{
        //    try
        //    {
        //        if (Request.Cookies["UserID"] == null)
        //        {
        //            Request.Cookies["UserID"].Value = null;
        //            return JavaScript("location.reload(true)");
        //        }
        //        else
        //        {
        //            long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //            bool respose = _deliveryservice.RemovePendingDeliveryOfOrder(data);
        //            return Json(respose, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult RemoveDelivery(List<RetOrderRemoveFromTempo> data)
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

        //[HttpGet]
        //public ActionResult TempoSummaryList()
        //{
        //    List<RetTempoSummary> obj = _deliveryservice.GetTempoSummaryList(Convert.ToDateTime(DateTime.Now));
        //    return View(obj);
        //}

        //public ActionResult ExportExcelTempoSummary()
        //{
        //    List<RetTempoSummary> obj = _deliveryservice.GetTempoSummaryList(Convert.ToDateTime(DateTime.Now));
        //    List<RetTempoSummary> lst = new List<RetTempoSummary>();
        //    for (int i = 0; i < obj.Count; i++)
        //    {
        //        RetTempoSummary ts = new RetTempoSummary();
        //        string newstring = obj[i].VehicleSummary.Replace("<br/>", "\r\n");
        //        ts.VehicleSummary = newstring;
        //        lst.Add(ts);
        //    }
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(ToDataTable(lst));
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(ds);
        //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        wb.Style.Font.Bold = true;
        //        wb.Worksheets.FirstOrDefault().AutoFilter.Clear();
        //        foreach (IXLWorksheet workSheet in wb.Worksheets)
        //        {
        //            foreach (IXLTable table in workSheet.Tables)
        //            {
        //                workSheet.Table(table.Name).ShowAutoFilter = false;
        //                workSheet.Columns().Width = 45;
        //            }
        //        }
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename= " + "TempoSummary" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
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
        //    //foreach (PropertyInfo prop in Props)
        //    //{
        //    //    //Defining type of data column gives proper data table 
        //    //    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
        //    //    //Setting column names as Property names
        //    //    dataTable.Columns.Add(prop.Name, type);
        //    //}
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //            dataTable.Columns.Add(values[i].ToString());
        //        }
        //    }
        //    //put a breakpoint here and check datatable
        //    return dataTable;
        //}

        [HttpGet]
        public ActionResult TempoSummaryList()
        {
            List<RetTempoSummary> finallist = new List<RetTempoSummary>();
            List<RetTempoSummary> vahiclelist = _deliveryservice.GetPackDetailVehicleList(Convert.ToDateTime(DateTime.Now));
            foreach (var item in vahiclelist)
            {
                RetTempoSummary objlst = new RetTempoSummary();
                int vahicleno = item.VehicleNo;
                objlst = _deliveryservice.GetTempoSummaryDetailList(Convert.ToDateTime(DateTime.Now), item.VehicleNo);
                finallist.Add(objlst);
            }
            return View(finallist);
        }

        public ActionResult ExportExcelTempoSummary()
        {
            List<RetTempoSummary> finallist = new List<RetTempoSummary>();
            List<RetTempoSummary> vahiclelist = _deliveryservice.GetPackDetailVehicleList(Convert.ToDateTime(DateTime.Now));
            foreach (var item in vahiclelist)
            {
                RetTempoSummary objlst = new RetTempoSummary();
                int vahicleno = item.VehicleNo;
                objlst = _deliveryservice.GetTempoSummaryDetailList(Convert.ToDateTime(DateTime.Now), item.VehicleNo);
                finallist.Add(objlst);
            }
            List<RetTempoSummaryExport> lst = finallist.Select(x => new RetTempoSummaryExport() { VehicleNo = x.VehicleNo, TempoNo = x.TempoNo, AreaNme = x.AreaNme, DeliveryPersons = x.DeliveryPersons, Container = x.Container }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lst));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "TempoSummary" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
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

        [HttpPost]
        public ActionResult DeleteOrderQty(string InvoiceNumber, long OrderID, bool IsDelete)
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
                    bool respose = _deliveryservice.DeleteOrderQty(InvoiceNumber, OrderID, IsDelete);
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
            return View();
        }

        [HttpPost]
        public PartialViewResult DeliveryStatusList(RetDeliveryStatusListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllRetCashOption();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            RetDeliverAllocation obj = new RetDeliverAllocation();
            obj.lstDelAllocation = _deliveryservice.GetDeliveryStatusList(model.CreatedOn);
            RetDeliveryStatusListResponse objModel2 = _deliveryservice.GetDeliveryInfoList(model.VehicleNo, model.CreatedOn);
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


        // 07 Sep. 2020 Piyush Limbani
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


                        OrderIdvalue = item2.OrderID;
                        LocalReport lr = new LocalReport();
                        string path = "";
                        List<RetOrderQtyInvoiceList> orderdata = _orderservice.GetInvoiceForOrderPrint(OrderID);
                        DataTable header = Common.ToDataTable(orderdata);
                        if (orderdata[0].TaxName == "IGST")
                        {
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
                        //bool respose = _orderservice.UpdatePrintStatus(OrderID);
                        ReportDataSource MedsheetHeader = new ReportDataSource("DataSet1", header);
                        Lstdatainvoice = new List<RetOrderQtyInvoiceList>();
                        int noofinvoice = orderdata[0].NoofInvoiceint;
                        List<string> incoiveNolist = _orderservice.GetListOfInvoice(OrderID);
                        int ordergroup = 0;
                        //for (int i = 1; i <= orderdata[0].NoofInvoiceint; i++)
                        for (int i = 1; i <= 1; i++)
                        {
                            Lstdata = _orderservice.GetInvoiceForOrderItemPrint(OrderIdvalue, InvoiceNumber);
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
                                    string FTaxName = "";
                                    string FGrandAmtWord = "";
                                    if (Foodzero.Count < 30)
                                    {
                                        for (int s = Foodzero.Count; s < 30; s++)
                                        {
                                            Foodzero.Add(new RetOrderQtyInvoiceList { RowNumber = 0, ordergroup = 0, InvoiceTitleHeader = "", BillNo = "", SerialNumber = 0, CustomerName = "", CustomerGroupName = "", InvoiceNumber = "", ShipTo = "", BillTo = "", DeliveryTo = "", OrderDate = Convert.ToDateTime(DateTime.Now), TaxName = "", CustomerNote = "", ProductName = "", Quantity = 0, UnitName = "", ProductPrice = 0, LessAmount = 0, SaleRate = 0, DiscountPrice = 0, BillDiscount = 0, ProductMRP = 0, Total = 0, TaxAmt = 0, TotalAmount = 0, Tax = 0, TaxAmount = 0, FinalTotal = 0, GrandTotal = 0, OrderID = 0, CustomerID = 0, LBTNo = "", TaxNo = "", FSSAI = "", ContactNumber = "", CreatedOn = Convert.ToDateTime(DateTime.Now), UserFullName = "", HSNNumber = "", TotalTax = 0, CategoryTypeID = Foodzero[0].CategoryTypeID, Totalcount = 0, AGrandTotal = 0, ATotalAmount = 0, Totalrecord = 0, GrandAmtWord = "", PONumber = "", City = "", State = "", StateCode = "", TotalMRP = 0, TotalQuantity = 0, InvoiceTitle = "", NoofInvoice = "", NoofInvoiceint = 0, divider = 0, OrdRowNumber = 0 });
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
                                            item.TotalQuantity = Convert.ToInt32(FTotalQuantity);
                                            item.ATotalAmount = FATotalAmount;
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
                                            if (rownumber > 30)
                                            {
                                                rownumber = 1;
                                                totalnumber = totalnumber + 1;
                                                total = 0;
                                                taxtotal = 0;
                                                grandtotal = 0;
                                                totalMRP = 0;
                                                NumberToWord = "";
                                                totalQty = 0;
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
                                            item.TotalAmount = item.TotalAmount;
                                            grandtotal += item.TotalAmount + item.TaxAmount;
                                            item.ATotalAmount = total;
                                            item.ATotalAmount = Math.Round(item.ATotalAmount, 2);
                                            item.Totalrecord = Lstdata.Count;
                                            item.FinalTotal = item.TotalAmount + item.TaxAmount;
                                            item.FinalTotal = Math.Round(item.FinalTotal, 2);
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
                                            int number = Convert.ToInt32(item.AGrandTotal);
                                            NumberToWord = NumberToWords(number);
                                            item.GrandAmtWord = NumberToWord;
                                            var Gtotal = Math.Round(grandtotal);
                                            item.GrandTotal = Gtotal;
                                            if (item.TaxName == "IGST")
                                            {
                                                item.TaxAmount = item.TaxAmount;
                                            }
                                            else
                                            {
                                                item.TaxAmount = item.TaxAmount / 2;
                                            }
                                            item.TaxAmount = Math.Round(item.TaxAmount, 2);
                                            if (item.TaxName == "IGST")
                                            {
                                                item.Tax = decimal.Round(item.Tax, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                item.Tax = decimal.Round(item.Tax / 2, 2, MidpointRounding.AwayFromZero);
                                            }
                                            item.SaleRate = decimal.Round(item.SaleRate, 2, MidpointRounding.AwayFromZero);
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
                                            item.RowNumber = rownumber;
                                            if (Foodzero2 > 30 * totalnumber)
                                            {
                                                item.Totalcount = Foodzero2 > 30 * totalnumber ? 30 * totalnumber : 30 * totalnumber - Foodzero2;
                                            }
                                            else
                                            {
                                                if (totalnumber == 1)
                                                {
                                                    item.Totalcount = Foodzero2;
                                                }
                                                else
                                                {
                                                    item.Totalcount = Foodzero2 - (30 * (totalnumber - 1));
                                                }
                                            }
                                            Lstdatainvoice.Add(item);
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
                                                FGrandAmtWord = item.GrandAmtWord;
                                            }
                                            rownumber = rownumber + 1;
                                        }
                                    }
                                }
                            }
                        }
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
                        string Pdfpathcreate = Server.MapPath("~/RetailInvoiceNumberWiseBill/" + name);
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
        // 07 Sep. 2020 Piyush Limbani



        // 13 Dec 2021 Piyush Limbani
        public ActionResult DayWiseSalesApproveList()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult DeleteSalesApprovalList(RetDayWiseSalesApproveList model)
        {
            List<RetDayWiseSalesApproveList> objlst = _deliveryservice.DeleteSalesApprovalList(model.InvDate);
            return PartialView(objlst);
        }

        public JsonResult UpdateDayWiseSalesApprove(long OrderID, string InvoiceNumber, bool IsApprove)
        {
            bool res = _deliveryservice.UpdateDayWiseSalesApprove(OrderID, InvoiceNumber, IsApprove);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        // 13 Dec 2021 Piyush Limbani



    }

    public class retcustomergroupbycustomer
    {
        public string Customer { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public List<RetDeliveryStatusListPrint> Lstdelivery { get; set; }
    }
}