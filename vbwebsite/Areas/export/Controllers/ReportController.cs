using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using vb.Data;
using vb.Service;

namespace vbwebsite.Areas.export.Controllers
{
    public class ReportController : Controller
    {
        private IRetReportService _IReportService;
        private ICommonService _ICommonservice;
        private IRetOrderService _IOrderservice;      
        private IRetAdminService _IAdminservice;
        private IRetProductService _Iproductservice;
        public ReportController(IRetReportService reportservice, ICommonService commonservice, IRetOrderService orderservice, IRetProductService productservice, IRetAdminService adminservice)
        {
            _IReportService = reportservice;
            _ICommonservice = commonservice;
            _IOrderservice = orderservice;          
            _IAdminservice = adminservice;
            _Iproductservice = productservice;
        }
        // GET: /export/Report/
        public ActionResult Index()
        {
            return View();
        }
        // GET: /export/Report/
        public ActionResult Daywisesales()
        {
            //Session["UserID"] = "1";
            return View();
        }

        [HttpPost]
        public PartialViewResult DayWiseExportSalesList(DayWiseSalesExportListForExp model)
        {
            List<DayWiseSalesExportListForExp> objlst = _IReportService.GetDayWiseExportSalesList(model.InvDate);
            for (int i = 0; i < objlst.Count; i++)
            {
                if (objlst[i].IsDelete == true)
                {
                    objlst[i].Party = "Cancelled";
                    objlst[i].Country = "";
                    //dataInventory[i].ContainerNo = "0";
                    //dataInventory[i].CustomerCode = 0;
                    objlst[i].TotalPkgs = 0;
                    objlst[i].GrossAmt = 0;
                    //dataInventory[i].Discount = 0;
                    objlst[i].CGST = 0;
                    objlst[i].TaxAmtCGST = "0.00";
                    objlst[i].SGST = 0;
                    objlst[i].TaxAmtSGST = "0.00";
                    objlst[i].IGST = 0;
                    objlst[i].TaxAmtIGST = "0.00";
                    objlst[i].RoundOff = 0;
                    objlst[i].NetAmount = 0;
                    objlst[i].UserFullName = "";
                }
            }

            return PartialView(objlst);
        }

        public ActionResult ExportExcelDayWiseSummary(string date)
        {

            DateTime expextedDate = Convert.ToDateTime(date);
            string CreditMemoDate = expextedDate.ToString("dd/MM/yyyy");



            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString();
            }


            List<DayWiseSalesExportListForExp> dataInventory = _IReportService.GetDayWiseExportSalesList(Convert.ToDateTime(date));
            for (int i = 0; i < dataInventory.Count; i++)
            {
                if (dataInventory[i].IsDelete == true)
                {
                    dataInventory[i].Party = "Cancelled";
                    dataInventory[i].Country = "";
                    //dataInventory[i].ContainerNo = "0";
                    //dataInventory[i].CustomerCode = 0;
                    dataInventory[i].TotalPkgs = 0;
                    dataInventory[i].GrossAmt = 0;
                    //dataInventory[i].Discount = 0;
                    dataInventory[i].CGST = 0;
                    dataInventory[i].TaxAmtCGST = "0.00";
                    dataInventory[i].SGST = 0;
                    dataInventory[i].TaxAmtSGST = "0.00";
                    dataInventory[i].IGST = 0;
                    dataInventory[i].TaxAmtIGST = "0.00";
                    dataInventory[i].RoundOff = 0;
                    dataInventory[i].NetAmount = 0;
                    dataInventory[i].UserFullName = "";
                }

            }

            List<ExportDayWiseSalesExportList> lstdaywisesales = dataInventory.Select(x => new ExportDayWiseSalesExportList() { InvCode = x.InvCode, InvoiceDate = x.InvoiceDate, Party = x.Party, Country = x.Country, TotalPkgs = x.TotalPkgs, GrossAmt = x.GrossAmt, CGST = x.CGST, TaxAmtCGST = x.TaxAmtCGST, SGST = x.SGST, TaxAmtSGST = x.TaxAmtSGST, IGST = x.IGST, TaxAmtIGST = x.TaxAmtIGST, RoundOff = x.RoundOff, InsuranceText = x.InsuranceText, InsuranceAmount = x.InsuranceAmount, FrieghtText = x.FrieghtText, FreightAMount = x.FreightAMount, NetAmount = x.NetAmount, UserFullName = x.UserFullName }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdaywisesales));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "DayWiseSummaryForExport.xls");

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

        public ActionResult ProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? CountryID, long? ProductCategoryID, long? ProductID, long? UserID)
        {
            Session["fprcustid"] = CustomerID;
            Session["fpruid"] = UserID;
            Session["fprtxtfrom"] = StartDate;
            Session["fprtxtto"] = EndDate;
            Session["fprproductid"] = ProductID;
            Session["fprcategoryid"] = ProductCategoryID;
            Session["fprcountryid"] = CountryID;
            ViewBag.SalesPersonList = _ICommonservice.GetRoleWiseUserList();
            ViewBag.Customer = _ICommonservice.GetActiveRetCustomerName(0);
            ViewBag.Product = _IOrderservice.GetAllRetProductName();
            ViewBag.CountryList = _Iproductservice.GetAllCountryName();
            ViewBag.ProductCategoryList = _Iproductservice.GetAllProductCategoryList();
            return View();
        }

        public ActionResult ProductWiseSalesList(ExpProductWiseSalesList model)
        {
            List<ExpProductWiseSalesList> objlst1 = _IReportService.GetExProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.CountryID, model.ProductCategoryID, model.ProductID, model.UserID);

            // new 30-04-2019
            List<ExpProductWiseSalesList> objlst = new List<ExpProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ExpProductWiseSalesList newobj = new ExpProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                newobj.Quantity = newobj.OrderQuantity;
                newobj.TotalAmount = item.TotalAmount;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.TotalKg = item.TotalKg;
                objlst.Add(newobj);
            }
            // new 30-04-2019

            List<ExProductMainListByMonth> lst = new List<ExProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ExProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            //lst = lst.OrderBy(x => x.MonthName).ThenBy(y => y.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("TotalKg");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalKg = Math.Round(item.TotalKg, 3),
                              OrderTotalAmount = Math.Round(item.OrderTotalAmount, 2),
                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
                dr["TotalKg"] = result.ToList()[i].TotalKg;
                dr["Amount"] = result.ToList()[i].OrderTotalAmount;
                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
                            cnt++;
                            break;
                        }
                    }
                }
            }
            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 3; col < dt.Columns.Count - 2; col++)  // when you add new colomn add in count 2 plus 1
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                dt.Rows[t][lst.Count + 3] = Math.Round(sum, 0);
            }

            DataRow drtot = dt.NewRow();
            for (int col = 3; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 3)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }
            return View(dt);
        }

        public ActionResult ProductWiseDailySalesList(ExpProductWiseSalesList model)
        {
            Session["fprcustid"] = "";
            Session["fpruid"] = "";
            Session["fprtxtfrom"] = "";
            Session["fprtxtto"] = "";
            Session["fprproductid"] = "";
            Session["fprcategoryid"] = "";
            Session["fprareaid"] = "";

            if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
                model.ProductID = Convert.ToInt64(Session["ProductID"]);
            }
            else
            {
                Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
                Session["ProductID"] = model.ProductID;
            }

            if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.EndDate = Convert.ToDateTime(Session["txtTo"]);
                model.ProductID = Convert.ToInt64(Session["ProductID"]);
            }
            else
            {
                Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
                Session["ProductID"] = model.ProductID;
            }

            List<ExpProductWiseSalesList> objlst1 = _IReportService.GetExpProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.CountryID, model.ProductCategoryID, model.ProductID, model.UserID);

            // new 30-04-2019
            List<ExpProductWiseSalesList> objlst = new List<ExpProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ExpProductWiseSalesList newobj = new ExpProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                //newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.CountryID = model.CountryID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                newobj.Quantity = newobj.OrderQuantity;
                newobj.TotalAmount = item.TotalAmount;
                newobj.TotalKg = item.TotalKg;
                objlst.Add(newobj);
            }
            // new 30-04-2019          

            List<ExProductMainListByDay> lst = new List<ExProductMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new ExProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add(lst[i].DayName + "-" + (lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("TotalKg");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                              TotalKg = Math.Round(item.TotalKg, 3),
                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
                dr["TotalKg"] = result.ToList()[i].TotalKg;
                dr["Amount"] = result.ToList()[i].TotalAmount;
                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
                            cnt++;
                            break;
                        }
                    }
                }
            }
            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 3; col < dt.Columns.Count - 2; col++)  // when you add new colomn add in count 2 plus 1
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                dt.Rows[t][lst.Count + 3] = Math.Round(sum, 0);
            }
            DataRow drtot = dt.NewRow();
            for (int col = 3; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 3)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }
            return View(dt);
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseOrderListForProductWiseSalesReport(ExpOrderListResponse model)
        {
            Session["prcustid"] = "";
            Session["pruid"] = "";
            Session["prtxtfrom"] = "";
            Session["prtxtto"] = "";
            Session["prproductid"] = "";
            Session["prareaid"] = "";
            Session["prcategoryid"] = "";


            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }


            List<ExpOrderListResponse> objModel = _IOrderservice.GetAllBillWiseExpOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForProductWiseSalesReport(string invid, Int64? custid, Int64? uid, string txtfrom, string txtto, string productid, string categoryid, string countryid)
        {
            //Session["UserID"] = "1";
            Session["prcustid"] = custid;
            Session["pruid"] = uid;
            Session["prtxtfrom"] = txtfrom;
            Session["prtxtto"] = txtto;
            Session["prproductid"] = productid;
            Session["prcategoryid"] = categoryid;
            Session["prcountryid"] = countryid;

            if (!string.IsNullOrEmpty(invid))
            {
                List<ExpOrderQtyList> objModel = _IOrderservice.GetBillWiseInvoiceForExpOrder(invid, string.Empty);
                return View(objModel);
            }
            else
            {
                return View();
            }
        }
      
        public ActionResult ExportExcelProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? CountryID, long? ProductCategoryID, long? ProductID, long? UserID, string CustomerName)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (CountryID == null)
            {
                CountryID = 0;
            }
            if (ProductCategoryID == null)
            {
                ProductCategoryID = 0;
            }
            if (ProductID == null)
            {
                ProductID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            List<ExpProductWiseSalesList> objlst1 = _IReportService.GetExProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, CountryID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);

            // new 30-04-2019
            List<ExpProductWiseSalesList> objlst = new List<ExpProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ExpProductWiseSalesList newobj = new ExpProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = CustomerID.Value;
                newobj.CountryID = CountryID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                newobj.Quantity = item.OrderQuantity;
                newobj.TotalAmount = item.TotalAmount;
                newobj.TotalKg = item.TotalKg;
                objlst.Add(newobj);
            }
            // new 30-04-2019

            List<ExProductMainListByMonth> lst = new List<ExProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ExProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            //lst = lst.OrderBy(x => x.MonthName).ThenBy(y => y.YearName).ToList();
            DataTable dt = new DataTable("ProductWiseMonthlySales");
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            //int cnt = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                //cnt = Math.Max(cnt, lst[i].ListMainProduct.Count);
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("TotalKg");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalKg = Math.Round(item.TotalKg, 3),
                              TotalAmount = Math.Round(item.OrderTotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            //lst = lst.OrderByDescending(x => x.ListMainProduct.Count).ToList();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
                dr["TotalKg"] = result.ToList()[i].TotalKg;
                dr["Amount"] = result.ToList()[i].TotalAmount;
                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
                            cnt++;
                            break;
                        }
                    }
                }
            }
            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 3; col < dt.Columns.Count - 3; col++)
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }

                dt.Rows[t][lst.Count + 3] = Math.Round(sum, 0);
            }
            DataRow drtot = dt.NewRow();
            for (int col = 3; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 3)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }

            List<ExpProductWiseSalesList> objlst3 = _IReportService.GetExpProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, CountryID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);

            // new 30-04-2019
            List<ExpProductWiseSalesList> objlst2 = new List<ExpProductWiseSalesList>();
            foreach (var item in objlst3)
            {
                ExpProductWiseSalesList newobj = new ExpProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = CustomerID.Value;
                newobj.CountryID = CountryID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                newobj.Quantity = newobj.OrderQuantity;
                newobj.TotalKg = item.TotalKg;
                newobj.TotalAmount = item.TotalAmount;
                objlst2.Add(newobj);
            }
            // new 30-04-2019          

            List<ExProductMainListByDay> lst2 = new List<ExProductMainListByDay>();

            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new ExProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            //lst2 = lst2.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("ProductWiseDailySalesList");

            dt2.Columns.Add("ProductID");
            dt2.Columns.Add("ProductName");

            //int cnt = 0;

            for (int i = 0; i < lst2.Count; i++)
            {
                //cnt = Math.Max(cnt, lst[i].ListMainProduct.Count);
                dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
            }

            dt2.Columns.Add("Total");
            dt2.Columns.Add("TotalKg");
            dt2.Columns.Add("Amount");
            var result2 = (from item in objlst2
                           select new
                           {
                               ProductID = item.ProductID,
                               ProductName = item.ProductName,
                               TotalAmount = Math.Round(item.OrderTotalAmount, 2),
                               TotalKg = Math.Round(item.TotalKg, 3),
                           })
              .ToList()
              .Distinct();
            //lst = lst.OrderByDescending(x => x.ListMainProduct.Count).ToList();

            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].ProductID;
                dr[1] = result2.ToList()[i].ProductName;
                dr["TotalKg"] = result2.ToList()[i].TotalKg;
                dr["Amount"] = result2.ToList()[i].TotalAmount;
                dt2.Rows.Add(dr);
            }
            int cnt2 = 1;
            dt2.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst2.Count; i++)
            {
                for (int j = 0; j < lst2[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        if (dt2.Rows[k]["ProductID"].ToString() == lst2[i].ListMainProduct[j].ProductID.ToString())
                        {
                            if (cnt2 <= dt2.Rows.Count)
                            {
                                dt2.Rows[cnt2 - 1][0] = cnt2;
                            }
                            dt2.Rows[k][i + 3] = lst2[i].ListMainProduct[j].Quantity;
                            cnt2++;
                            break;

                        }
                    }
                }
            }

            for (int t = 0; t < dt2.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 3; col < dt2.Columns.Count - 3; col++)
                {
                    if (dt2.Rows[t][col] == DBNull.Value)
                    {
                        dt2.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt2.Rows[t][col]);
                    if (dt2.Rows[t][col] == "0")
                    {
                        dt2.Rows[t][col] = "";
                    }
                }

                dt2.Rows[t][lst2.Count + 3] = Math.Round(sum, 0);
            }

            DataRow drtot2 = dt2.NewRow();
            for (int col = 3; col < dt2.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt2.Rows.Count; t++)
                {
                    if (dt2.Rows[t][col] == DBNull.Value || dt2.Rows[t][col] == "")
                    {
                        dt2.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt2.Rows[t][col]);
                    if (dt2.Rows[t][col] == "0")
                    {
                        dt2.Rows[t][col] = "";
                    }
                }
                if (col == 3)
                {
                    drtot2[0] = "";
                    drtot2[1] = "";
                    drtot2[2] = "";
                    drtot2[col] = sum;
                    dt2.Rows.Add(drtot2);
                }
                else
                {
                    drtot2[col] = sum;
                }
            }
            dt.Columns.Remove("ProductID");
            dt2.Columns.Remove("ProductID");

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt2);

            DataRow dRow = ds.Tables[0].NewRow();
            dRow["Sr.No"] = "Customer Name";
            dRow["ProductName"] = CustomerName;
            ds.Tables[0].Rows.Add(dRow);

            DataRow dRow1 = ds.Tables[1].NewRow();
            dRow1["Sr.No"] = "Customer Name";
            dRow1["ProductName"] = CustomerName;
            ds.Tables[1].Rows.Add(dRow1);

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

                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "ExportProductWiseSales.xls");

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

        [HttpGet]
        public ActionResult ProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long ProductCategoryID, long ProductID, long UserID)
        {
            List<ExpProductWiseSalesList> objlist2 = new List<ExpProductWiseSalesList>();

            if (CustomerID > 0)
            {
                string ProductIDs = _IReportService.GetExpProductIDForProductWiseSalesList2(StartDate, EndDate, CustomerID, CountryID, ProductCategoryID, ProductID, UserID);
                if (ProductIDs != "")
                {
                    DateTime EndDate1 = DateTime.Now;
                    DateTime StartDate1 = EndDate1.AddYears(-1);
                    List<ExpProductWiseSalesList> objlst = _IReportService.GetExpProductWiseSalesList2(StartDate1, EndDate1, ProductIDs, CustomerID, CountryID, ProductCategoryID, UserID);
                    return View(objlst);
                }
                else
                {
                    return View(objlist2);
                }

            }
            else
            {
                return View(objlist2);
            }
        }
       
        public ActionResult ExportExcelZeroProductReport(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long ProductCategoryID, long ProductID, long UserID)
        {

            string ProductIDs = _IReportService.GetExpProductIDForProductWiseSalesList2(StartDate, EndDate, CustomerID, CountryID, ProductCategoryID, ProductID, UserID);
            if (ProductIDs != "")
            {
                DateTime EndDate1 = DateTime.Now;
                DateTime StartDate1 = EndDate1.AddYears(-1);
                var ProductList = _IReportService.GetExpProductWiseSalesList2(StartDate1, EndDate1, ProductIDs, CustomerID, CountryID, ProductCategoryID, UserID);

                List<ProductListForZeroSalesExport> lstVoucher = ProductList.Select(x => new ProductListForZeroSalesExport() { SrNo = x.SrNo, CategoryName = x.CategoryName, ProductName = x.ProductName, Quantity = x.Quantity }).ToList();

                DataSet ds = new DataSet();
                ds.Tables.Add(ToDataTable(lstVoucher));
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "WholesaleProductListForZeroSales.xls");
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
       
        public ActionResult SalesManWiseSales()
        {
            ViewBag.SalesPersonList = _ICommonservice.GetRoleWiseUserList();
            ViewBag.Customer = _ICommonservice.GetActiveRetCustomerName(0);
            ViewBag.CountryList = _Iproductservice.GetAllCountryName();
            return View();
        }

        public ActionResult SalesManWiseSalesList(SalesManWiseExpSalesList model)
        {
            List<SalesManWiseExpSalesList> objlst = _IReportService.GetSalesManWiseExpSalesList(model.StartDate, model.EndDate, model.CustomerID, model.CountryID, model.UserID);

            List<SalesExpMainListByMonth> lst = new List<SalesExpMainListByMonth>();

            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new SalesExpMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();

            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            //lst = lst.OrderBy(x => x.MonthName).ThenBy(y => y.YearName).ToList();
            DataTable dt = new DataTable();

            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Country");
            dt.Columns.Add("UserName");


            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            var result = (from item in objlst
                          select new
                          {
                              CustomerID = item.CustomerID,
                              CustomerName = item.CustomerName,
                              CountryName = item.CountryName,
                              UserName = item.UserName,

                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].CustomerID;
                dr[1] = result.ToList()[i].CustomerName;
                dr[2] = result.ToList()[i].CountryName;
                dr[3] = result.ToList()[i].UserName;

                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["CustomerID"].ToString() == lst[i].ListMainProduct[j].CustomerID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            if (model.CurrencyName != "USD")
                            {
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].OrderAmount;
                            }
                            else
                            {
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].InvoiceTotalAmount;
                            }
                            cnt++;
                            break;

                        }
                    }
                }
            }


            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 5; col < dt.Columns.Count - 1; col++)
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }

                dt.Rows[t][lst.Count + 5] = sum;
            }

            DataRow drtot = dt.NewRow();
            for (int col = 5; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 5)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[3] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }

            return View(dt);
        }

        public ActionResult SalesManWiseDailySalesList(SalesManWiseExpSalesList model)
        {
            Session["srcustid"] = "";
            Session["sruid"] = "";
            Session["srtxtfrom"] = "";
            Session["srtxtto"] = "";
            Session["srcountryid"] = "";



            if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
                model.CustomerID = Convert.ToInt64(Session["CustomerID"]);
            }
            else
            {
                Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
                Session["CustomerID"] = model.CustomerID;
            }

            if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.EndDate = Convert.ToDateTime(Session["txtTo"]);
                model.CustomerID = Convert.ToInt64(Session["CustomerID"]);
            }
            else
            {
                Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
                Session["CustomerID"] = model.CustomerID;
            }



            List<SalesManWiseExpSalesList> objlst = _IReportService.GetExSalesManWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.CountryID, model.UserID);

            List<SalesExpMainListByDay> lst = new List<SalesExpMainListByDay>();

            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new SalesExpMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();

            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Country");
            dt.Columns.Add("UserName");


            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add(lst[i].DayName + "-" + (lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            var result = (from item in objlst
                          select new
                          {
                              CustomerID = item.CustomerID,
                              CustomerName = item.CustomerName,
                              CountryName = item.CountryName,
                              UserName = item.UserName,

                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].CustomerID;
                dr[1] = result.ToList()[i].CustomerName;
                dr[2] = result.ToList()[i].CountryName;
                dr[3] = result.ToList()[i].UserName;

                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["CustomerID"].ToString() == lst[i].ListMainProduct[j].CustomerID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            if (model.CurrencyName != "USD")
                            {
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].OrderAmount;
                            }
                            else
                            {
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].InvoiceTotalAmount;
                            }

                            cnt++;
                            break;

                        }
                    }
                }
            }

            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 5; col < dt.Columns.Count - 1; col++)
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }

                dt.Rows[t][lst.Count + 5] = sum;
            }

            DataRow drtot = dt.NewRow();
            for (int col = 5; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 5)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[3] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }

            return View(dt);
        }
        [HttpPost]
        public PartialViewResult ViewBillWiseOrderListForSalesManWiseSalesReport(ExpOrderListResponse model)
        {
            Session["srcustid"] = "";
            Session["sruid"] = "";
            Session["srtxtfrom"] = "";
            Session["srtxtto"] = "";
            Session["srcountryid"] = "";

            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ExpOrderListResponse> objModel = _IOrderservice.GetAllBillWiseExpOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForSalesManWiseSalesReport(string invid, Int64? custid, Int64? uid, string txtfrom, string txtto, string countryid, string currencyname)
        {
            //Session["UserID"] = "1";
            Session["srcustid"] = custid;
            Session["sruid"] = uid;
            Session["srtxtfrom"] = txtfrom;
            Session["srtxtto"] = txtto;
            Session["srcountryid"] = countryid;

            if (!string.IsNullOrEmpty(invid))
            {
                List<ExpOrderQtyList> objModel = _IOrderservice.GetBillWiseInvoiceForExpOrder(invid, currencyname);
                return View(objModel);
            }
            else
            {
                return View();
            }
        }


        public ActionResult ExportExcelSalesManWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? CountryID, long? UserID, string CurrencyName)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (CountryID == null)
            {
                CountryID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            List<SalesManWiseExpSalesList> objlst = _IReportService.GetSalesManWiseExpSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, CountryID.Value, UserID.Value);

            List<SalesExpMainListByMonth> lst = new List<SalesExpMainListByMonth>();

            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new SalesExpMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();

            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            //lst = lst.OrderBy(x => x.MonthName).ThenBy(y => y.YearName).ToList();
            DataTable dt = new DataTable("SalesWiseMonthlySales");

            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Country");
            dt.Columns.Add("UserName");


            //int cnt = 0;

            for (int i = 0; i < lst.Count; i++)
            {
                //cnt = Math.Max(cnt, lst[i].ListMainProduct.Count);
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            var result = (from item in objlst
                          select new
                          {
                              CustomerID = item.CustomerID,
                              CustomerName = item.CustomerName,
                              CountryName = item.CountryName,
                              UserName = item.UserName,

                          })
            .ToList()
            .Distinct();
            //lst = lst.OrderByDescending(x => x.ListMainProduct.Count).ToList();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].CustomerID;
                dr[1] = result.ToList()[i].CustomerName;
                dr[2] = result.ToList()[i].CountryName;
                dr[3] = result.ToList()[i].UserName;

                dt.Rows.Add(dr);
            }
            int cnt = 1;
            dt.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst.Count; i++)
            {
                for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["CustomerID"].ToString() == lst[i].ListMainProduct[j].CustomerID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            if (CurrencyName.ToString() != "USD")
                            {
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].OrderAmount;
                            }
                            else
                            {
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].InvoiceTotalAmount;
                            }


                            //dt.Rows[k][lst.Count + 3] = lst[i].ListMainProduct[j].TotalQuantity;
                            cnt++;
                            break;

                        }
                    }
                }
            }

            decimal sum = 0;
            for (int t = 0; t < dt.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 5; col < dt.Columns.Count - 1; col++)
                {
                    if (dt.Rows[t][col] == DBNull.Value)
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }

                dt.Rows[t][lst.Count + 5] = sum;
            }

            DataRow drtot = dt.NewRow();
            for (int col = 5; col < dt.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
                    {
                        dt.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
                    if (dt.Rows[t][col] == "0")
                    {
                        dt.Rows[t][col] = "";
                    }
                }
                if (col == 5)
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[3] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }

            List<SalesManWiseExpSalesList> objlst2 = _IReportService.GetExSalesManWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, CountryID.Value, UserID.Value);

            List<SalesExpMainListByDay> lst2 = new List<SalesExpMainListByDay>();

            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new SalesExpMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            //lst2 = lst2.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("SalesWiseDailySalesList");

            dt2.Columns.Add("CustomerID");
            dt2.Columns.Add("CustomerName");
            dt2.Columns.Add("Country");
            dt2.Columns.Add("UserName");


            //int cnt = 0;

            for (int i = 0; i < lst2.Count; i++)
            {
                //cnt = Math.Max(cnt, lst[i].ListMainProduct.Count);
                dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
            }
            dt2.Columns.Add("Total");
            var result2 = (from item in objlst2
                           select new
                           {
                               CustomerID = item.CustomerID,
                               CustomerName = item.CustomerName,
                               CountryName = item.CountryName,
                               UserName = item.UserName,

                           })
            .ToList()
            .Distinct();
            //lst = lst.OrderByDescending(x => x.ListMainProduct.Count).ToList();

            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].CustomerID;
                dr[1] = result2.ToList()[i].CustomerName;
                dr[2] = result2.ToList()[i].CountryName;
                dr[3] = result2.ToList()[i].UserName;

                dt2.Rows.Add(dr);
            }
            int cnt2 = 1;
            dt2.Columns.Add("Sr.No").SetOrdinal(0);
            for (int i = 0; i < lst2.Count; i++)
            {
                for (int j = 0; j < lst2[i].ListMainProduct.Count; j++)
                {
                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        if (dt2.Rows[k]["CustomerID"].ToString() == lst2[i].ListMainProduct[j].CustomerID.ToString())
                        {
                            if (cnt2 <= dt2.Rows.Count)
                            {
                                dt2.Rows[cnt2 - 1][0] = cnt2;
                            }
                            if (CurrencyName.ToString() != "USD")
                            {
                                dt2.Rows[k][i + 5] = lst2[i].ListMainProduct[j].OrderAmount;
                            }
                            else
                            {
                                dt2.Rows[k][i + 5] = lst2[i].ListMainProduct[j].InvoiceTotalAmount;
                            }


                            //dt2.Rows[k][lst2.Count + 3] = lst2[i].ListMainProduct[j].TotalQuantity;
                            cnt2++;
                            break;

                        }
                    }
                }
            }

            for (int t = 0; t < dt2.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 5; col < dt2.Columns.Count - 1; col++)
                {
                    if (dt2.Rows[t][col] == DBNull.Value)
                    {
                        dt2.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt2.Rows[t][col]);
                    if (dt2.Rows[t][col] == "0")
                    {
                        dt2.Rows[t][col] = "";
                    }
                }

                dt2.Rows[t][lst2.Count + 5] = sum;
            }

            DataRow drtot2 = dt2.NewRow();
            for (int col = 5; col < dt2.Columns.Count; col++)
            {
                sum = 0;
                for (int t = 0; t < dt2.Rows.Count; t++)
                {
                    if (dt2.Rows[t][col] == DBNull.Value || dt2.Rows[t][col] == "")
                    {
                        dt2.Rows[t][col] = "0";
                    }
                    sum = sum + Convert.ToDecimal(dt2.Rows[t][col]);
                    if (dt2.Rows[t][col] == "0")
                    {
                        dt2.Rows[t][col] = "";
                    }
                }
                if (col == 5)
                {
                    drtot2[0] = "";
                    drtot2[1] = "";
                    drtot2[2] = "";
                    drtot2[3] = "";
                    drtot2[col] = sum;
                    dt2.Rows.Add(drtot2);
                }
                else
                {
                    drtot2[col] = sum;
                }
            }

            dt.Columns.Remove("CustomerID");
            dt2.Columns.Remove("CustomerID");

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt2);

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
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "ExportSalesManWiseSales.xls");

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