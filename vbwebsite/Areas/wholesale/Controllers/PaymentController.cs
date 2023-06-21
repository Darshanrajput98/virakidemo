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
using vb.Data.ViewModel;
using vb.Service;
using vb.Service.Common;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class PaymentController : Controller
    {

        private IPaymentService _paymentservice;
        private ICommonService _commonservice;
        private ICustomerService _customerservice;

        WAAPI wa = new WAAPI();


        public PaymentController(IPaymentService paymentservice, ICommonService commonservice, ICustomerService customerservice)
        {
            _paymentservice = paymentservice;
            _commonservice = commonservice;
            _customerservice = customerservice;
        }

        // GET: /wholesale/Payment/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentPendingList()
        {
            //Session["UserID"] = "1";
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            ViewBag.CustomerList = _commonservice.GetAllCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewPaymentPendingList(PaymentListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            List<PaymentListResponse> objModel = _paymentservice.GetAllPaymentList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult SavePayment(List<PaymentListResponse> data)
        {
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                bool respose = _paymentservice.SavePayment(data, UserID);

                // 14 July, 2021 Sonal Gandhi
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
                // 14 July, 2021 Sonal Gandhi

                return Json(respose, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportExcelPaymentPending(PaymentListResponse model)
        {

            if (model.CustomerID == null)
            {
                model.CustomerID = 0;
            }
            if (model.AreaID == null)
            {
                model.AreaID = 0;
            }

            if (model.DaysofWeek == null)
            {
                model.DaysofWeek = 0;
            }
            if (model.UserID == null)
            {
                model.UserID = 0;
            }

            var objlst = _paymentservice.GetAllPaymentList(model);
            List<PaymentListForExp> lstproduct = objlst.Select(x => new PaymentListForExp()
            {
                CustomerNumber = x.CustomerNumber,
                CustomerName = x.CustomerName,
                AreaName = x.AreaName,
                InvoiceDate = x.InvoiceDate,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceAmount = x.FinalTotal,
                OutstandingAmount = x.OutstandingAmount,
                Remark = x.Remark
            }).ToList();

            // 11 Sep 2020 Piyush Limbani Export To Excel Voucher Payment
            var objlstvoucher = _paymentservice.GetAllWholesaleExpensesVoucherPaymentList(model.From, model.To, model.AreaID, model.UserID, model.CustomerID, model.DaysofWeek);
            List<VoucherPaymentListForExp> lstVoucherPayment = objlstvoucher.Select(x => new VoucherPaymentListForExp()
            {
                CustomerNumber = x.CustomerNumber,
                CustomerName = x.CustomerName,
                AreaName = x.AreaName,
                DateofVoucher = x.DateofVoucher.ToString("dd/MM/yyyy"),
                BillNumber = x.BillNumber,
                PaymentAmount = x.PaymentAmount,
                OutstandingAmount = x.OutstandingAmount,
                Remark = x.Remark
            }).ToList();
            // 11 Sep 2020 Piyush Limbani

            DataTable ds = new DataTable();
            ds = ToDataTable(lstproduct);

            DataTable ds2 = new DataTable();
            ds2 = ToDataTable(lstVoucherPayment);

            DataRow row = ds.NewRow();
            row["CustomerNumber"] = 0;
            row["CustomerName"] = "";
            row["AreaName"] = "";
            row["InvoiceDate"] = "";
            row["InvoiceNumber"] = "";
            row["InvoiceAmount"] = 0;
            row["OutstandingAmount"] = 0;
            row["Remark"] = "";
            ds.Rows.InsertAt(row, 0);

            DataRow row2 = ds2.NewRow();
            row2["CustomerNumber"] = 0;
            row2["CustomerName"] = "";
            row2["AreaName"] = "";
            row2["DateofVoucher"] = "";
            row2["BillNumber"] = "";
            row2["PaymentAmount"] = 0;
            row2["OutstandingAmount"] = 0;
            row2["Remark"] = "";
            ds2.Rows.InsertAt(row2, 0);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(ds);
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws.Cell("A1").Value = "";
                ws.Cell("B1").Value = "SALES MAN:" + model.UserName;
                ws.Range("B1:E1").Row(1).Merge();
                ws.Cell("F1").Value = "Date :" + DateTime.Now;
                ws.Range("F1:H1").Row(1).Merge();
                ws.Cell("A2").Value = "CustomerNumber";
                ws.Cell("B2").Value = "CustomerName";
                ws.Cell("C2").Value = "AreaName";
                ws.Cell("D2").Value = "InvoiceDate";
                ws.Cell("E2").Value = "InvoiceNumber";
                ws.Cell("F2").Value = "InvoiceAmount";
                ws.Cell("G2").Value = "OutstandingAmount";
                ws.Cell("H2").Value = "Remark";
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Style.Font.Bold = true;

                var ws2 = wb.Worksheets.Add(ds2);
                ws2.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws2.Cell("A1").Value = "";
                ws2.Cell("B1").Value = "SALES MAN:" + model.UserName;
                ws2.Range("B1:E1").Row(1).Merge();
                ws2.Cell("F1").Value = "Date :" + DateTime.Now;
                ws2.Range("F1:H1").Row(1).Merge();
                ws2.Cell("A2").Value = "CustomerNumber";
                ws2.Cell("B2").Value = "CustomerName";
                ws2.Cell("C2").Value = "AreaName";
                ws2.Cell("D2").Value = "DateofVoucher";
                ws2.Cell("E2").Value = "BillNumber";
                ws2.Cell("F2").Value = "PaymentAmount";
                ws2.Cell("G2").Value = "OutstandingAmount";
                ws2.Cell("H2").Value = "Remark";
                ws2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws2.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "PaymentList.xlsx");
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



        // 10 Sep 2020 Piyush Limbani
        [HttpPost]
        public PartialViewResult ViewExpensesVoucherPaymentPendingList(VoucherPaymentListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            List<VoucherPaymentListResponse> objModel = _paymentservice.GetAllWholesaleExpensesVoucherPaymentList(model.From, model.To, model.AreaID, model.UserID, model.CustomerID, model.DaysofWeek);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult SaveExpenseVoucherPayment(List<VoucherPaymentListResponse> data)
        {
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                bool respose = _paymentservice.SaveExpenseVoucherPayment(data, UserID);

                // 19 July, 2021 Sonal Gandhi
                //foreach (var item in data)
                //{
                //    PaymentListResponse paymentListResponse = new PaymentListResponse();
                //    paymentListResponse.ByCash = item.ByCash;
                //    paymentListResponse.ByCheque = item.ByCheque;
                //    paymentListResponse.BankName = item.BankName;
                //    paymentListResponse.BankBranch = item.BankBranch;
                //    paymentListResponse.ChequeNo = item.ChequeNo;
                //    paymentListResponse.ChequeDate = item.ChequeDate;
                //    paymentListResponse.IFCCode = item.IFCCode;
                //    paymentListResponse.ByCard = item.ByCard;
                //    paymentListResponse.BankNameForCard = item.BankNameForCard;
                //    paymentListResponse.TypeOfCard = item.TypeOfCard;
                //    paymentListResponse.ByOnline = item.ByOnline;
                //    paymentListResponse.BankNameForOnline = item.BankNameForOnline;
                //    paymentListResponse.UTRNumber = item.UTRNumber;
                //    paymentListResponse.OnlinePaymentDate = item.OnlinePaymentDate;
                //    paymentListResponse.InvoiceNumber = item.BillNumber;
                //    List<CustomerAddressViewModel> customerDetails = _customerservice.GetCustomerAddressListByCustomerID(item.CustomerID);
                //    string res = wa.SendEmailToCustomer(customerDetails[0], paymentListResponse);
                //}
                // 19 July, 2021 Sonal Gandhi

                return Json(respose, JsonRequestBehavior.AllowGet);
            }
        }


        //Add By Dhruvik
        public ActionResult CheckReturnEntryList()
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            ViewBag.CustomerList = _commonservice.GetAllCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewCheckReturnEntryList(CheckReturnEntryListResponse model)
        {
            List<CheckReturnEntryListResponse> objModel = _paymentservice.GetAllCheckReturnList(model);
            return PartialView(objModel);
        }
        
        [HttpPost]
        public ActionResult SaveReturnCheck(List<PaymentForCheckReturn> data)
        {
            bool respose = false;
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                foreach (var item in data)
                {
                    bool res = _paymentservice.IsCheckBounceOnPayment(item.PaymentID, item.OrderID, item.InvoiceNumber, Math.Abs(item.BounceAmount), item.ChequeReturnCharges);
                }

                long SessionUserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                respose = _paymentservice.SaveReturnCheck(data, SessionUserID);
            }
            return Json(respose, JsonRequestBehavior.AllowGet);
        }
        //Add By Dhruvik

    }
}