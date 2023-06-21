using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.ViewModel;
using vb.Service;

namespace vbwebsite.Areas.retail.Controllers
{
    public class ReportController : Controller
    {
        private IRetReportService _reportservice;
        private ICommonService _commonservice;
        private IRetOrderService _orderservice;
        private IRetProductService _productservice;
        private IRetCustomerService _customerservice;
        private IAdminService _adminservice;
        private IReportService _whsreportservice;

        private IExpensesService _IExpensesService;

        private IRetPaymentService _paymentservice;

        public ReportController(IRetReportService reportservice, ICommonService commonservice, IRetOrderService orderservice, IRetProductService productservice, IRetCustomerService customerservice, IAdminService adminservice, IReportService whsreportservice, IExpensesService expensesservice, IRetPaymentService paymentservice)
        {
            _reportservice = reportservice;
            _commonservice = commonservice;
            _orderservice = orderservice;
            _productservice = productservice;
            _customerservice = customerservice;
            _adminservice = adminservice;
            _whsreportservice = whsreportservice;
            _IExpensesService = expensesservice;
            _paymentservice = paymentservice;
        }

        public ActionResult ProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID)
        {
            Session["frprcustid"] = CustomerID;
            Session["frpruid"] = UserID;
            Session["frprtxtfrom"] = StartDate;
            Session["frprtxtto"] = EndDate;
            Session["frprproductqtyid"] = ProductQtyID;
            Session["frprcategoryid"] = ProductCategoryID;
            Session["frprareaid"] = AreaID;
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
            ViewBag.Product = _orderservice.GetAllRetProductName();
            ViewBag.AreaList = _commonservice.GetAllRetAreaList();
            ViewBag.ProductCategoryList = _productservice.GetAllProductCategoryList();
            return View();
        }

        public ActionResult ProductWiseSalesList(RetProductWiseSalesList model)
        {
            List<RetProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID);

            // new 29-04-2019
            List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }

                //decimal ReturnTotalKg = _reportservice.GetRetReturnReturnTotalKgMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                //if (ReturnTotalKg != 0)
                //{
                //    newobj.TotalKg = newobj.OrderTotalKg - ReturnTotalKg;
                //}
                //else
                //{
                //    newobj.TotalKg = newobj.OrderTotalKg;
                //}

                //decimal ReturnOrderAmount = _reportservice.GetRetReturnTotalAmountMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                //if (ReturnOrderAmount != 0)
                //{
                //    newobj.TotalAmount = newobj.OrderAmount - ReturnOrderAmount;
                //}
                //else
                //{
                //    newobj.TotalAmount = newobj.OrderAmount;
                //}
                newobj.TotalKg = item.TotalKg;
                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 29-04-2019

            List<RetProductMainListByMonth> lst = new List<RetProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductQtyID");
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
                              ProductQtyID = item.ProductQtyID,
                              ProductName = item.ProductName,
                              TotalKg = Math.Round(item.TotalKg, 3),
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          }).ToList().Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductQtyID;
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
                        if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
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

        public ActionResult ProductWiseDailySalesList(RetProductWiseSalesList model)
        {
            Session["frprcustid"] = "";
            Session["frpruid"] = "";
            Session["frprtxtfrom"] = "";
            Session["frprtxtto"] = "";
            Session["frprproductid"] = "";
            Session["frprcategoryid"] = "";
            Session["frprareaid"] = "";

            if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
                model.ProductQtyID = Convert.ToInt64(Session["ProductID"]);
            }
            else
            {
                Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
                Session["ProductID"] = model.ProductQtyID;
            }

            if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.EndDate = Convert.ToDateTime(Session["txtTo"]);
                model.ProductQtyID = Convert.ToInt64(Session["ProductID"]);
            }
            else
            {
                Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
                Session["ProductID"] = model.ProductQtyID;
            }

            List<RetProductWiseSalesList> objlst1 = _reportservice.GetProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID);

            // new 29-04-2019
            List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();

            decimal TotalKg = 0;
            decimal ReturnFinalTotalKg = 0;
            decimal TotalAmount = 0;
            decimal ReturnOrderTotalAmount = 0;
            foreach (var item in objlst1)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                TotalKg = item.OrderTotalKg;
                decimal ReturnTotalKg = _reportservice.GetRetReturnTotalKgDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnTotalKg != 0)
                {
                    ReturnFinalTotalKg += ReturnTotalKg;
                }

                //if (ReturnTotalKg != 0)
                //{
                //    TotalKg = item.OrderTotalKg - ReturnTotalKg;
                //}
                //else
                //{
                //    TotalKg = item.OrderTotalKg;
                //}

                TotalAmount = item.OrderAmount;
                decimal ReturnOrderAmount = _reportservice.GetRetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    ReturnOrderTotalAmount += ReturnOrderAmount;
                }

                //if (ReturnOrderAmount != 0)
                //{
                //    TotalAmount = item.OrderAmount - ReturnOrderAmount;
                //}
                //else
                //{
                //    TotalAmount = item.OrderAmount;
                //}
                //  newobj.TotalKg = item.OrderTotalKg;
                // newobj.TotalAmount = item.OrderAmount;
                objlst.Add(newobj);
            }
            // new 29-04-2019

            List<RetProductMainListByDay> lst = new List<RetProductMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductQtyID");
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
                              ProductQtyID = item.ProductQtyID,
                              ProductName = item.ProductName,
                              //TotalKg = Math.Round(item.TotalKg, 3),
                              //TotalAmount = Math.Round(item.TotalAmount, 2),                             
                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductQtyID;
                dr[1] = result.ToList()[i].ProductName;
                //  dr["TotalKg"] = result.ToList()[i].TotalKg;
                //dr["Amount"] = result.ToList()[i].TotalAmount;             
                //dr["TotalKg"] = Math.Round(TotalKg, 3);
                //dr["Amount"] = Math.Round(TotalAmount, 2);
                dr["TotalKg"] = Math.Round((TotalKg - ReturnFinalTotalKg), 3);
                dr["Amount"] = Math.Round((TotalAmount - ReturnOrderTotalAmount), 2);
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
                        if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
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

        //public ActionResult ExportExcelProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID, string CustomerName)
        //{
        //    if (CustomerID == null)
        //    {
        //        CustomerID = 0;
        //    }
        //    if (AreaID == null)
        //    {
        //        AreaID = 0;
        //    }
        //    if (ProductCategoryID == null)
        //    {
        //        ProductCategoryID = 0;
        //    }
        //    if (ProductQtyID == null)
        //    {
        //        ProductQtyID = 0;
        //    }
        //    if (UserID == null)
        //    {
        //        UserID = 0;
        //    }
        //    List<RetProductWiseSalesList> objlst = _reportservice.GetProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);
        //    List<RetProductMainListByMonth> lst = new List<RetProductMainListByMonth>();
        //    lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
        //    lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
        //    DataTable dt = new DataTable("RetProductWiseMonthlySales");
        //    dt.Columns.Add("ProductQtyID");
        //    dt.Columns.Add("ProductName");
        //    for (int i = 0; i < lst.Count; i++)
        //    {
        //        dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
        //    }
        //    dt.Columns.Add("Total");
        //    dt.Columns.Add("TotalKg");
        //    dt.Columns.Add("Amount");
        //    var result = (from item in objlst
        //                  select new
        //                  {
        //                      ProductQtyID = item.ProductQtyID,
        //                      ProductName = item.ProductName,
        //                      TotalKg = Math.Round(item.TotalKg, 3),
        //                      TotalAmount = Math.Round(item.TotalAmount, 2),
        //                  })
        //      .ToList()
        //      .Distinct();
        //    for (int i = 0; i < result.ToList().Count; i++)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr[0] = result.ToList()[i].ProductQtyID;
        //        dr[1] = result.ToList()[i].ProductName;
        //        dr["TotalKg"] = result.ToList()[i].TotalKg;
        //        dr["Amount"] = result.ToList()[i].TotalAmount;
        //        dt.Rows.Add(dr);
        //    }
        //    int cnt = 1;
        //    dt.Columns.Add("Sr.No").SetOrdinal(0);
        //    for (int i = 0; i < lst.Count; i++)
        //    {
        //        for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
        //        {
        //            for (int k = 0; k < dt.Rows.Count; k++)
        //            {
        //                if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
        //                {
        //                    if (cnt <= dt.Rows.Count)
        //                    {
        //                        dt.Rows[cnt - 1][0] = cnt;
        //                    }
        //                    dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
        //                    cnt++;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    decimal sum = 0;
        //    for (int t = 0; t < dt.Rows.Count; t++)
        //    {
        //        sum = 0;
        //        for (int col = 3; col < dt.Columns.Count - 2; col++)
        //        {
        //            if (dt.Rows[t][col] == DBNull.Value)
        //            {
        //                dt.Rows[t][col] = "0";
        //            }
        //            sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //            if (dt.Rows[t][col] == "0")
        //            {
        //                dt.Rows[t][col] = "";
        //            }
        //        }

        //        dt.Rows[t][lst.Count + 3] = Math.Round(sum, 0);
        //    }
        //    DataRow drtot = dt.NewRow();
        //    for (int col = 3; col < dt.Columns.Count; col++)
        //    {
        //        sum = 0;
        //        for (int t = 0; t < dt.Rows.Count; t++)
        //        {
        //            if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
        //            {
        //                dt.Rows[t][col] = "0";
        //            }
        //            sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //            if (dt.Rows[t][col] == "0")
        //            {
        //                dt.Rows[t][col] = "";
        //            }
        //        }
        //        if (col == 3)
        //        {
        //            drtot[0] = "";
        //            drtot[1] = "";
        //            drtot[2] = "";
        //            drtot[col] = sum;
        //            dt.Rows.Add(drtot);
        //        }
        //        else
        //        {
        //            drtot[col] = sum;
        //        }
        //    }
        //    List<RetProductWiseSalesList> objlst2 = _reportservice.GetProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);
        //    List<RetProductMainListByDay> lst2 = new List<RetProductMainListByDay>();
        //    lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
        //    lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
        //    DataTable dt2 = new DataTable("ProductWiseDailySalesList");
        //    dt2.Columns.Add("ProductQtyID");
        //    dt2.Columns.Add("ProductName");
        //    for (int i = 0; i < lst2.Count; i++)
        //    {
        //        dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
        //    }
        //    dt2.Columns.Add("Total");
        //    dt2.Columns.Add("TotalKg");
        //    dt2.Columns.Add("Amount");
        //    var result2 = (from item in objlst2
        //                   select new
        //                   {
        //                       ProductQtyID = item.ProductQtyID,
        //                       ProductName = item.ProductName,
        //                       TotalKg = Math.Round(item.TotalKg, 3),
        //                       TotalAmount = Math.Round(item.TotalAmount, 2),
        //                   })
        //      .ToList()
        //      .Distinct();
        //    for (int i = 0; i < result2.ToList().Count; i++)
        //    {
        //        DataRow dr = dt2.NewRow();
        //        dr[0] = result2.ToList()[i].ProductQtyID;
        //        dr[1] = result2.ToList()[i].ProductName;
        //        dr["TotalKg"] = result.ToList()[i].TotalKg;
        //        dr["Amount"] = result.ToList()[i].TotalAmount;
        //        dt2.Rows.Add(dr);
        //    }
        //    int cnt2 = 1;
        //    dt2.Columns.Add("Sr.No").SetOrdinal(0);
        //    for (int i = 0; i < lst2.Count; i++)
        //    {
        //        for (int j = 0; j < lst2[i].ListMainProduct.Count; j++)
        //        {
        //            for (int k = 0; k < dt2.Rows.Count; k++)
        //            {
        //                if (dt2.Rows[k]["ProductQtyID"].ToString() == lst2[i].ListMainProduct[j].ProductQtyID.ToString())
        //                {
        //                    if (cnt2 <= dt2.Rows.Count)
        //                    {
        //                        dt2.Rows[cnt2 - 1][0] = cnt2;
        //                    }
        //                    dt2.Rows[k][i + 3] = lst2[i].ListMainProduct[j].Quantity;
        //                    cnt2++;
        //                    break;

        //                }
        //            }
        //        }
        //    }

        //    for (int t = 0; t < dt2.Rows.Count; t++)
        //    {
        //        sum = 0;
        //        for (int col = 3; col < dt2.Columns.Count - 2; col++)
        //        {
        //            if (dt2.Rows[t][col] == DBNull.Value)
        //            {
        //                dt2.Rows[t][col] = "0";
        //            }
        //            sum = sum + Convert.ToDecimal(dt2.Rows[t][col]);
        //            if (dt2.Rows[t][col] == "0")
        //            {
        //                dt2.Rows[t][col] = "";
        //            }
        //        }

        //        dt2.Rows[t][lst2.Count + 3] = Math.Round(sum, 0);
        //    }

        //    DataRow drtot2 = dt2.NewRow();
        //    for (int col = 3; col < dt2.Columns.Count; col++)
        //    {
        //        sum = 0;
        //        for (int t = 0; t < dt2.Rows.Count; t++)
        //        {
        //            if (dt2.Rows[t][col] == DBNull.Value || dt2.Rows[t][col] == "")
        //            {
        //                dt2.Rows[t][col] = "0";
        //            }
        //            sum = sum + Convert.ToDecimal(dt2.Rows[t][col]);
        //            if (dt2.Rows[t][col] == "0")
        //            {
        //                dt2.Rows[t][col] = "";
        //            }
        //        }
        //        if (col == 3)
        //        {
        //            drtot2[0] = "";
        //            drtot2[1] = "";
        //            drtot2[2] = "";
        //            drtot2[col] = sum;
        //            dt2.Rows.Add(drtot2);
        //        }
        //        else
        //        {
        //            drtot2[col] = sum;
        //        }
        //    }
        //    dt.Columns.Remove("ProductQtyID");
        //    dt2.Columns.Remove("ProductQtyID");
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(dt);
        //    ds.Tables.Add(dt2);
        //    DataRow dRow = ds.Tables[0].NewRow();
        //    dRow["Sr.No"] = "Customer Name";
        //    dRow["ProductName"] = CustomerName;
        //    ds.Tables[0].Rows.Add(dRow);
        //    DataRow dRow1 = ds.Tables[1].NewRow();
        //    dRow1["Sr.No"] = "Customer Name";
        //    dRow1["ProductName"] = CustomerName;
        //    ds.Tables[1].Rows.Add(dRow1);
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
        //            }
        //        }
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename= " + "ProductWiseSales.xls");
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

        public ActionResult ExportExcelProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID, string CustomerName)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (ProductCategoryID == null)
            {
                ProductCategoryID = 0;
            }
            if (ProductQtyID == null)
            {
                ProductQtyID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            List<RetProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);

            // new 30-04-2019
            List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;

                newobj.ArticleCode = item.ArticleCode;

                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }

                //decimal ReturnTotalKg = _reportservice.GetRetReturnReturnTotalKgMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                //if (ReturnTotalKg != 0)
                //{
                //    newobj.TotalKg = newobj.OrderTotalKg - ReturnTotalKg;
                //}
                //else
                //{
                //    newobj.TotalKg = newobj.OrderTotalKg;
                //}

                //decimal ReturnOrderAmount = _reportservice.GetRetReturnTotalAmountMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                //if (ReturnOrderAmount != 0)
                //{
                //    newobj.TotalAmount = newobj.OrderAmount - ReturnOrderAmount;
                //}
                //else
                //{
                //    newobj.TotalAmount = newobj.OrderAmount;
                //}
                newobj.TotalKg = item.TotalKg;
                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 30-04-2019

            List<RetProductMainListByMonth> lst = new List<RetProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable("RetProductWiseMonthlySales");
            dt.Columns.Add("ProductQtyID");
            dt.Columns.Add("ArticleCode");
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
                              ProductQtyID = item.ProductQtyID,
                              ArticleCode = item.ArticleCode,
                              ProductName = item.ProductName,
                              TotalKg = Math.Round(item.TotalKg, 3),
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductQtyID;
                dr[1] = result.ToList()[i].ArticleCode;
                dr[2] = result.ToList()[i].ProductName;
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
                        if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            dt.Rows[k][i + 4] = lst[i].ListMainProduct[j].Quantity;
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
                for (int col = 4; col < dt.Columns.Count - 3; col++)
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

                dt.Rows[t][lst.Count + 4] = Math.Round(sum, 0);
            }
            DataRow drtot = dt.NewRow();
            for (int col = 4; col < dt.Columns.Count; col++)
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
                if (col == 4)
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
            List<RetProductWiseSalesList> objlst3 = _reportservice.GetProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);

            // new 29-04-2019
            List<RetProductWiseSalesList> objlst2 = new List<RetProductWiseSalesList>();
            decimal TotalKg = 0;
            decimal TotalAmount = 0;
            foreach (var item in objlst3)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;

                newobj.ArticleCode = item.ArticleCode;


                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }

                decimal ReturnTotalKg = _reportservice.GetRetReturnTotalKgDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnTotalKg != 0)
                {
                    TotalKg = item.OrderTotalKg - ReturnTotalKg;
                }
                else
                {
                    TotalKg = item.OrderTotalKg;
                }

                decimal ReturnOrderAmount = _reportservice.GetRetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    TotalAmount = item.OrderAmount - ReturnOrderAmount;
                }
                else
                {
                    TotalAmount = item.OrderAmount;
                }
                //  newobj.TotalKg = item.OrderTotalKg;
                // newobj.TotalAmount = item.OrderAmount;
                objlst2.Add(newobj);
            }
            // new 29-04-2019

            List<RetProductMainListByDay> lst2 = new List<RetProductMainListByDay>();
            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("ProductWiseDailySalesList");
            dt2.Columns.Add("ProductQtyID");
            dt2.Columns.Add("ArticleCode");
            dt2.Columns.Add("ProductName");
            for (int i = 0; i < lst2.Count; i++)
            {
                dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
            }
            dt2.Columns.Add("Total");
            dt2.Columns.Add("TotalKg");
            dt2.Columns.Add("Amount");
            var result2 = (from item in objlst2
                           select new
                           {
                               ProductQtyID = item.ProductQtyID,
                               ArticleCode = item.ArticleCode,
                               ProductName = item.ProductName,
                               TotalKg = Math.Round(item.TotalKg, 3),
                               TotalAmount = Math.Round(item.TotalAmount, 2),
                           })
              .ToList()
              .Distinct();
            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].ProductQtyID;
                dr[1] = result2.ToList()[i].ArticleCode;
                dr[2] = result2.ToList()[i].ProductName;
                //dr["TotalKg"] = Math.Round(TotalKg, 3);
                //dr["Amount"] = Math.Round(TotalAmount, 2);
                dr["TotalKg"] = result.ToList()[i].TotalKg;
                dr["Amount"] = result.ToList()[i].TotalAmount;
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
                        if (dt2.Rows[k]["ProductQtyID"].ToString() == lst2[i].ListMainProduct[j].ProductQtyID.ToString())
                        {
                            if (cnt2 <= dt2.Rows.Count)
                            {
                                dt2.Rows[cnt2 - 1][0] = cnt2;
                            }
                            dt2.Rows[k][i + 4] = lst2[i].ListMainProduct[j].Quantity;
                            cnt2++;
                            break;

                        }
                    }
                }
            }

            for (int t = 0; t < dt2.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 4; col < dt2.Columns.Count - 3; col++)
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

                dt2.Rows[t][lst2.Count + 4] = Math.Round(sum, 0);
            }

            DataRow drtot2 = dt2.NewRow();
            for (int col = 4; col < dt2.Columns.Count; col++)
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
                if (col == 4)
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
            dt.Columns.Remove("ProductQtyID");
            dt2.Columns.Remove("ProductQtyID");
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "ProductWiseSales.xls");
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

        public ActionResult FestivalProductWiseSales()
        {
            ViewBag.Event = _adminservice.GetAllEventName();
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
            ViewBag.Product = _orderservice.GetAllRetProductName();
            ViewBag.AreaList = _commonservice.GetAllRetAreaList();
            ViewBag.ProductCategoryList = _productservice.GetAllProductCategoryList();
            return View();
        }

        public ActionResult FestivalProductWiseSalesList(RetFestivalProductWiseSalesList model)
        {
            model.StartDate = model.StartDate.AddDays(-model.BeforeDays);
            model.EndDate = model.EndDate.AddDays(model.AfterDays);
            List<RetProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID);


            // new 28-06-2019
            List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                newobj.TotalKg = item.TotalKg;
                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 28-06-2019


            List<RetProductMainListByMonth> lst = new List<RetProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductQtyID");
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
                              ProductQtyID = item.ProductQtyID,
                              ProductName = item.ProductName,
                              TotalKg = Math.Round(item.TotalKg, 3),
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductQtyID;
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
                        if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
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
                for (int col = 3; col < dt.Columns.Count - 2; col++)
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

        public ActionResult FestivalProductWiseDailySalesList(RetFestivalProductWiseSalesList model)
        {
            Session["frprcustid"] = "";
            Session["frpruid"] = "";
            Session["frprtxtfrom"] = "";
            Session["frprtxtto"] = "";
            Session["frprproductid"] = "";
            Session["frprcategoryid"] = "";
            Session["frprareaid"] = "";

            model.StartDate = model.StartDate.AddDays(-model.BeforeDays);
            model.EndDate = model.EndDate.AddDays(model.AfterDays);

            if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
                model.ProductQtyID = Convert.ToInt64(Session["ProductID"]);
            }
            else
            {
                Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
                Session["ProductID"] = model.ProductQtyID;
            }

            if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.EndDate = Convert.ToDateTime(Session["txtTo"]);
                model.ProductQtyID = Convert.ToInt64(Session["ProductID"]);
            }
            else
            {
                Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
                Session["ProductID"] = model.ProductQtyID;
            }

            List<RetProductWiseSalesList> objlst1 = _reportservice.GetProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID);

            // new 28-06-2019
            List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
            decimal TotalKg = 0;
            decimal ReturnFinalTotalKg = 0;
            decimal TotalAmount = 0;
            decimal ReturnOrderTotalAmount = 0;
            foreach (var item in objlst1)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                TotalKg = item.OrderTotalKg;
                decimal ReturnTotalKg = _reportservice.GetRetReturnTotalKgDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnTotalKg != 0)
                {
                    ReturnFinalTotalKg += ReturnTotalKg;
                }
                TotalAmount = item.OrderAmount;
                decimal ReturnOrderAmount = _reportservice.GetRetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    ReturnOrderTotalAmount += ReturnOrderAmount;
                }
                objlst.Add(newobj);
            }
            // new 29-04-2019


            List<RetProductMainListByDay> lst = new List<RetProductMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductQtyID");
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
                              ProductQtyID = item.ProductQtyID,
                              ProductName = item.ProductName,
                              //TotalKg = Math.Round(item.TotalKg, 3),
                              //TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductQtyID;
                dr[1] = result.ToList()[i].ProductName;
                //dr["TotalKg"] = result.ToList()[i].TotalKg;
                //dr["Amount"] = result.ToList()[i].TotalAmount;
                dr["TotalKg"] = Math.Round((TotalKg - ReturnFinalTotalKg), 3);
                dr["Amount"] = Math.Round((TotalAmount - ReturnOrderTotalAmount), 2);
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
                        if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
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
        public PartialViewResult ViewBillWiseOrderListForFestivalProductWiseSalesReport(RetOrderListResponse model)
        {
            Session["rprcustid"] = "";
            Session["rpruid"] = "";
            Session["rprtxtfrom"] = "";
            Session["rprtxtto"] = "";
            Session["rprproductqtyid"] = "";
            Session["rprareaid"] = "";
            Session["rprcategoryid"] = "";

            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<RetOrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelFestivalProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID, long? BeforeDays, long? AfterDays)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (ProductCategoryID == null)
            {
                ProductCategoryID = 0;
            }
            if (ProductQtyID == null)
            {
                ProductQtyID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }

            if (EndDate.Value == null)
            {
                EndDate = Convert.ToDateTime(DateTime.Now);
            }
            if (StartDate.Value == null)
            {
                StartDate = Convert.ToDateTime(DateTime.Now);
            }
            if (BeforeDays == null)
            {
                BeforeDays = 0;
            }
            if (AfterDays == null)
            {
                AfterDays = 0;
            }
            StartDate = StartDate.Value.AddDays(-BeforeDays.Value);
            EndDate = EndDate.Value.AddDays(AfterDays.Value);
            List<RetProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);


            // new 28-06-2019
            List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;
                newobj.ArticleCode = item.ArticleCode;
                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                newobj.TotalKg = item.TotalKg;
                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 30-04-2019


            List<RetProductMainListByMonth> lst = new List<RetProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable("RetFestivalWiseProductSales");
            dt.Columns.Add("ProductQtyID");
            dt.Columns.Add("ArticleCode");
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
                              ProductQtyID = item.ProductQtyID,
                              ArticleCode = item.ArticleCode,
                              ProductName = item.ProductName,
                              TotalKg = Math.Round(item.TotalKg, 3),
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductQtyID;
                dr[1] = result.ToList()[i].ArticleCode;
                dr[2] = result.ToList()[i].ProductName;
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
                        if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
                        {
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                            }
                            dt.Rows[k][i + 4] = lst[i].ListMainProduct[j].Quantity;
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
                for (int col = 4; col < dt.Columns.Count - 3; col++)
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
                dt.Rows[t][lst.Count + 4] = Math.Round(sum, 0);
            }
            DataRow drtot = dt.NewRow();
            for (int col = 4; col < dt.Columns.Count; col++)
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
                if (col == 4)
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

            List<RetProductWiseSalesList> objlst3 = _reportservice.GetProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);

            // new 28-06-2019
            List<RetProductWiseSalesList> objlst2 = new List<RetProductWiseSalesList>();
            decimal TotalKg = 0;
            decimal TotalAmount = 0;
            foreach (var item in objlst3)
            {
                RetProductWiseSalesList newobj = new RetProductWiseSalesList();
                newobj.ProductQtyID = item.ProductQtyID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalKg = item.OrderTotalKg;
                newobj.OrderAmount = item.OrderAmount;
                newobj.ArticleCode = item.ArticleCode;
                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                decimal ReturnTotalKg = _reportservice.GetRetReturnTotalKgDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnTotalKg != 0)
                {
                    TotalKg = item.OrderTotalKg - ReturnTotalKg;
                }
                else
                {
                    TotalKg = item.OrderTotalKg;
                }
                decimal ReturnOrderAmount = _reportservice.GetRetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    TotalAmount = item.OrderAmount - ReturnOrderAmount;
                }
                else
                {
                    TotalAmount = item.OrderAmount;
                }
                objlst2.Add(newobj);
            }
            // new 28-06-2019

            List<RetProductMainListByDay> lst2 = new List<RetProductMainListByDay>();
            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("ProductWiseDailySalesList");
            dt2.Columns.Add("ProductQtyID");
            dt2.Columns.Add("ArticleCode");
            dt2.Columns.Add("ProductName");
            for (int i = 0; i < lst2.Count; i++)
            {
                dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
            }
            dt2.Columns.Add("Total");
            dt2.Columns.Add("TotalKg");
            dt2.Columns.Add("Amount");
            var result2 = (from item in objlst2
                           select new
                           {
                               ProductQtyID = item.ProductQtyID,
                               ArticleCode = item.ArticleCode,
                               ProductName = item.ProductName,
                               TotalKg = Math.Round(item.TotalKg, 3),
                               TotalAmount = Math.Round(item.TotalAmount, 2),
                           })
              .ToList()
              .Distinct();
            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].ProductQtyID;
                dr[1] = result2.ToList()[i].ArticleCode;
                dr[2] = result2.ToList()[i].ProductName;
                dr["TotalKg"] = result.ToList()[i].TotalKg;
                dr["Amount"] = result.ToList()[i].TotalAmount;
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
                        if (dt2.Rows[k]["ProductQtyID"].ToString() == lst2[i].ListMainProduct[j].ProductQtyID.ToString())
                        {
                            if (cnt2 <= dt2.Rows.Count)
                            {
                                dt2.Rows[cnt2 - 1][0] = cnt2;
                            }
                            dt2.Rows[k][i + 4] = lst2[i].ListMainProduct[j].Quantity;
                            cnt2++;
                            break;
                        }
                    }
                }
            }
            for (int t = 0; t < dt2.Rows.Count; t++)
            {
                sum = 0;
                for (int col = 4; col < dt2.Columns.Count - 3; col++)
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
                dt2.Rows[t][lst2.Count + 4] = Math.Round(sum, 0);
            }
            DataRow drtot2 = dt2.NewRow();
            for (int col = 4; col < dt2.Columns.Count; col++)
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
                if (col == 4)
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

            dt.Columns.Remove("ProductQtyID");
            dt2.Columns.Remove("ProductQtyID");
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "FestivalProductWiseSales.xls");
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
        public PartialViewResult ViewBillWiseOrderListForProductWiseSalesReport(RetOrderListResponse model)
        {
            Session["rprcustid"] = "";
            Session["rpruid"] = "";
            Session["rprtxtfrom"] = "";
            Session["rprtxtto"] = "";
            Session["rprproductqtyid"] = "";
            Session["rprareaid"] = "";
            Session["rprcategoryid"] = "";

            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<RetOrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForProductWiseSalesReport(string invoicenumber, Int64? custid, Int64? uid, string txtfrom, string txtto, string productqtyid, string categoryid, string areaid, long orderid = 0)
        {
            Session["rprcustid"] = custid;
            Session["rpruid"] = uid;
            Session["rprtxtfrom"] = txtfrom;
            Session["rprtxtto"] = txtto;
            Session["rprproductqtyid"] = productqtyid;
            Session["rprcategoryid"] = categoryid;
            Session["rprareaid"] = areaid;
            if (!string.IsNullOrEmpty(invoicenumber))
            {
                List<RetOrderQtyList> objModel = _orderservice.GetBillWiseInvoiceForOrder(invoicenumber, orderid); //1
                return View(objModel);
            }
            else
            {
                return View();
            }
        }

        public ActionResult SalesManWiseSales()
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            return View();
        }

        public ActionResult SalesManWiseSalesList(RetSalesManWiseSalesList model)
        {
            // 03 Oct 2020 Piyush Limbani
            List<RetSalesManWiseSalesList> objlst = null;
            if (model.IsFinalised == true)
            {
                objlst = _reportservice.GetRetSalesManWiseSalesListForFinalisedOrder(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);
            }
            else
            {
                objlst = _reportservice.GetSalesManWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);
            }
            // 03 Oct 2020 Piyush Limbani

            //List<RetSalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);

            List<RetSalesMainListByMonth> lst = new List<RetSalesMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetSalesMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Area");
            dt.Columns.Add("CellNo");
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
                              AreaName = item.AreaName,
                              CellNo = item.CellNo,
                              UserName = item.UserName,
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].CustomerID;
                dr[1] = result.ToList()[i].CustomerName;
                dr[2] = result.ToList()[i].AreaName;
                dr[3] = result.ToList()[i].CellNo;
                dr[4] = result.ToList()[i].UserName;

                //dr[3] = result.ToList()[i].UserName;
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
                            dt.Rows[k][i + 6] = lst[i].ListMainProduct[j].Quantity; // dt.Rows[k][i + 5]
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
                for (int col = 6; col < dt.Columns.Count - 1; col++) // col = 5
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
                dt.Rows[t][lst.Count + 6] = sum; // Count + 5
            }
            DataRow drtot = dt.NewRow();
            for (int col = 6; col < dt.Columns.Count; col++) // col = 5
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
                if (col == 6) // col == 5
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[3] = "";
                    drtot[4] = "";
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

        public ActionResult SalesManWiseDailySalesList(RetSalesManWiseSalesList model)
        {
            Session["rsrcustid"] = "";
            Session["rsruid"] = "";
            Session["rsrtxtfrom"] = "";
            Session["rsrtxtto"] = "";
            Session["rsrareaid"] = "";
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

            // 03 Oct 2020 Piyush Limbani
            List<RetSalesManWiseSalesList> objlst = null;
            if (model.IsFinalised == true)
            {
                objlst = _reportservice.GetRetSalesManWiseDailySalesListForFinalisedOrder(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);
            }
            else
            {
                objlst = _reportservice.GetSalesManWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);
            }
            // 03 Oct 2020 Piyush Limbani

            // List<RetSalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);

            List<RetSalesMainListByDay> lst = new List<RetSalesMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetSalesMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Area");
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
                              AreaName = item.AreaName,
                              UserName = item.UserName,
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].CustomerID;
                dr[1] = result.ToList()[i].CustomerName;
                dr[2] = result.ToList()[i].AreaName;
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
                            dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].Quantity;
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
        public PartialViewResult ViewBillWiseOrderListForSalesManWiseSalesReport(RetOrderListResponse model)
        {
            Session["rsrcustid"] = "";
            Session["rsruid"] = "";
            Session["rsrtxtfrom"] = "";
            Session["rsrtxtto"] = "";
            Session["rsrareaid"] = "";
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<RetOrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForSalesManWiseSalesReport(string invoicenumber, Int64? custid, Int64? uid, string txtfrom, string txtto, string areaid, long orderid = 0)
        {
            Session["rsrcustid"] = custid;
            Session["rsruid"] = uid;
            Session["rsrtxtfrom"] = txtfrom;
            Session["rsrtxtto"] = txtto;
            Session["rsrareaid"] = areaid;
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

        public ActionResult ExportExcelSalesManWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? UserID, long? DaysofWeek, bool IsFinalised)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            if (DaysofWeek == null)
            {
                DaysofWeek = 0;
            }

            // 03 Oct 2020 Piyush Limbani
            List<RetSalesManWiseSalesList> objlst = null;
            if (IsFinalised == true)
            {
                objlst = _reportservice.GetRetSalesManWiseSalesListForFinalisedOrder(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            }
            else
            {
                objlst = _reportservice.GetSalesManWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            }
            // 03 Oct 2020 Piyush Limbani
            //List<RetSalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);

            List<RetSalesMainListByMonth> lst = new List<RetSalesMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetSalesMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable("RetSalesWiseMonthlySales");
            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Area");
            dt.Columns.Add("CellNo");
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
                              AreaName = item.AreaName,
                              CellNo = item.CellNo,
                              UserName = item.UserName,
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].CustomerID;
                dr[1] = result.ToList()[i].CustomerName;
                dr[2] = result.ToList()[i].AreaName;
                dr[3] = result.ToList()[i].CellNo;
                dr[4] = result.ToList()[i].UserName;

                //dr[3] = result.ToList()[i].UserName;
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
                            dt.Rows[k][i + 6] = lst[i].ListMainProduct[j].Quantity; // [i + 5]
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
                for (int col = 6; col < dt.Columns.Count - 1; col++) // col = 5
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
                dt.Rows[t][lst.Count + 6] = sum; // Count + 5
            }

            DataRow drtot = dt.NewRow();
            for (int col = 6; col < dt.Columns.Count; col++) // col = 5
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
                if (col == 6) // col == 5
                {
                    drtot[0] = "";
                    drtot[1] = "";
                    drtot[2] = "";
                    drtot[3] = "";
                    drtot[4] = "";
                    drtot[col] = sum;
                    dt.Rows.Add(drtot);
                }
                else
                {
                    drtot[col] = sum;
                }
            }

            // 03 Oct 2020 Piyush Limbani
            List<RetSalesManWiseSalesList> objlst2 = null;
            if (IsFinalised == true)
            {
                objlst2 = _reportservice.GetRetSalesManWiseDailySalesListForFinalisedOrder(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            }
            else
            {
                objlst2 = _reportservice.GetSalesManWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            }
            // 03 Oct 2020 Piyush Limbani

            //List<RetSalesManWiseSalesList> objlst2 = _reportservice.GetSalesManWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            List<RetSalesMainListByDay> lst2 = new List<RetSalesMainListByDay>();
            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetSalesMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("RetailSalesWiseDailySalesList");
            dt2.Columns.Add("CustomerID");
            dt2.Columns.Add("CustomerName");
            dt2.Columns.Add("Area");
            dt2.Columns.Add("UserName");
            for (int i = 0; i < lst2.Count; i++)
            {
                dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
            }
            dt2.Columns.Add("Total");
            var result2 = (from item in objlst2
                           select new
                           {
                               CustomerID = item.CustomerID,
                               CustomerName = item.CustomerName,
                               AreaName = item.AreaName,
                               UserName = item.UserName,
                           })
              .ToList()
              .Distinct();
            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].CustomerID;
                dr[1] = result2.ToList()[i].CustomerName;
                dr[2] = result2.ToList()[i].AreaName;
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
                            dt2.Rows[k][i + 5] = lst2[i].ListMainProduct[j].Quantity;
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "SalesManWiseSales.xls");
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

        public ActionResult DeliverySheet()
        {
            ViewBag.CashOption = _commonservice.GetAllRetCashOption();
            ViewBag.VehicleNo = _commonservice.GetRetVehicleNoList();
            return View();
        }

        [HttpPost]
        public PartialViewResult DeliverySheetList(RetDeliverySheetList model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            decimal TotalCash = 0;
            decimal TotalCheque = 0;
            decimal TotalCard = 0;
            decimal TotalSign = 0;
            decimal TotalOnline = 0;
            decimal TotalAdjustAmount = 0;
            List<RetDeliverySheetList> objlst = null;
            List<RetDeliverySheetList> objlistoftempo = new List<RetDeliverySheetList>();
            int BySign = 0;
            if (model.IsCheckBySign == false)
            {
                BySign = 0;
            }
            else
            {
                BySign = 1;
            }
            foreach (var item in model.VehicleNo)
            {
                objlst = _reportservice.GetDeliverySheetList(model.AssignedDate, item, model.TempoNo, model.GodownID, BySign);
                if (objlst != null)
                {
                    foreach (var record in objlst)
                    {
                        objlistoftempo.Add(record);
                        TotalCash += record.Cash;
                        TotalCheque += record.Cheque;
                        TotalCard += record.Card;
                        TotalSign += record.Sign;
                        TotalOnline += record.Online;
                        TotalAdjustAmount += record.AdjustAmount;
                    }
                }
            }
            if (TotalCash != 0)
            {
                objlistoftempo[0].CashTotal = TotalCash;
            }
            if (TotalCheque != 0)
            {
                objlistoftempo[0].ChequeTotal = TotalCheque;
            }
            if (TotalCard != 0)
            {
                objlistoftempo[0].CardTotal = TotalCard;
            }
            if (TotalSign != 0)
            {
                objlistoftempo[0].SignTotal = TotalSign;
            }
            if (TotalOnline != 0)
            {
                objlistoftempo[0].OnlineTotal = TotalOnline;
            }
            if (TotalAdjustAmount != 0)
            {
                objlistoftempo[0].AdjustAmountTotal = TotalAdjustAmount;
            }
            return PartialView(objlistoftempo);
        }

        public ActionResult ExportExcelDeliverySheet(string Date, List<string> VehicleNo, string TempoNo, long GodownID, bool IsCheckBySign)
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.ToString();
            }
            int BySign = 0;
            if (IsCheckBySign == false)
            {
                BySign = 0;
            }
            else
            {
                BySign = 1;
            }
            decimal TotalCash = 0;
            decimal TotalCheque = 0;
            decimal TotalCard = 0;
            decimal TotalSign = 0;
            decimal TotalOnline = 0;
            decimal TotalAdjustAmount = 0;
            List<RetDeliverySheetList> finallist = new List<RetDeliverySheetList>();
            string[] vehicle = VehicleNo[0].Split(',');
            foreach (var item in vehicle)
            {
                List<RetDeliverySheetList> dataInventory = _reportservice.GetDeliverySheetList(Convert.ToDateTime(Date), item, TempoNo, GodownID, BySign);
                if (dataInventory != null)
                {
                    foreach (var record in dataInventory)
                    {
                        finallist.Add(record);
                        TotalCash += record.Cash;
                        TotalCheque += record.Cheque;
                        TotalCard += record.Card;
                        TotalSign += record.Sign;
                        TotalOnline += record.Online;
                        TotalAdjustAmount += record.AdjustAmount;
                    }
                }
            }
            List<RetDeliverySheetListForExport> lstdelsheet = finallist.Select(x => new RetDeliverySheetListForExport() { Customer = x.Customer, Area = x.Area, InvoiceNumber = x.InvoiceNumber1, Cash = x.Cash, Cheque = x.Cheque, Card = x.Card, Sign = x.Sign, Online = x.Online, AdjustAmount = x.AdjustAmount, Remarks = x.Remarks, Godown = x.GodownName, VehicleNo = x.VehicleNo1, BankName = x.BankName, BankBranch = x.BankBranch, ChequeNo = x.ChequeNo, ChequeDate = x.ChequeDate1, IFCCode = x.IFCCode, BankNameForCard = x.BankNameForCard, TypeOfCard = x.TypeOfCard, OnlinePaymentDate = x.OnlinePaymentDate1, BankNameForOnline = x.BankNameForOnline, UTRNumber = x.UTRNumber }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdelsheet));
            DataRow dRow = ds.Tables[0].NewRow();
            dRow["Cash"] = TotalCash;
            dRow["Cheque"] = TotalCheque;
            dRow["Card"] = TotalCard;
            dRow["Sign"] = TotalSign;
            dRow["Online"] = TotalOnline;
            dRow["AdjustAmount"] = TotalAdjustAmount;
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "DeliverySheet.xls");
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

        public ActionResult Daywisesales()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult DayWiseSalesList(RetDayWiseSalesList model)
        {
            List<RetDayWiseSalesList> objlst = _reportservice.GetDayWiseSalesList(model.InvDate);
            for (int i = 0; i < objlst.Count; i++)
            {
                if (objlst[i].IsDelete == true && objlst[i].IsApprove == true)
                {
                    objlst[i].Party = "Cancelled";
                    objlst[i].Area = "";
                    objlst[i].GrossAmt = 0;
                    objlst[i].CGST = 0;
                    objlst[i].TaxAmtCGST = "0.00";
                    objlst[i].SGST = 0;
                    objlst[i].TaxAmtSGST = "0.00";
                    objlst[i].IGST = 0;
                    objlst[i].TaxAmtIGST = "0.00";
                    objlst[i].TCSTaxAmount = 0;
                    objlst[i].RoundOff = 0;
                    objlst[i].NetAmount = 0;
                    objlst[i].UserFullName = "";
                }
            }
            return PartialView(objlst);
        }

        [HttpPost]
        public PartialViewResult DayWiseCreditMemoList(RetDayWiseCreditMemoList model)
        {
            List<RetDayWiseCreditMemoList> objlst = _reportservice.GetDayWiseCreditMemoList(model.InvDate);
            return PartialView(objlst);
        }

        [HttpPost]
        public PartialViewResult DayWiseTaxList(RetDayWiseTaxList model)
        {
            List<RetDayWiseTaxList> objlst = _reportservice.GetDayWiseTaxList(model.InvDate);
            return PartialView(objlst);
        }

        [HttpPost]
        public PartialViewResult DayWiseSalesManList(RetDayWiseSalesManList model)
        {
            List<RetDayWiseSalesManList> objlst = _reportservice.GetDayWiseSalesManList(model.InvDate);
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
            var dataInventory2 = _reportservice.GetDayWiseCreditMemoList(Convert.ToDateTime(date));
            var dataInventory3 = _reportservice.GetDayWiseTaxList(Convert.ToDateTime(date));
            var dataInventory4 = _reportservice.GetDayWiseSalesManList(Convert.ToDateTime(date));
            List<RetDayWiseSalesList> dataInventory = _reportservice.GetDayWiseSalesList(Convert.ToDateTime(date));
            for (int i = 0; i < dataInventory.Count; i++)
            {
                if (dataInventory[i].IsDelete == true)
                {
                    dataInventory[i].Party = "Cancelled";
                    dataInventory[i].Area = "";
                    dataInventory[i].GrossAmt = 0;
                    dataInventory[i].CGST = 0;
                    dataInventory[i].TaxAmtCGST = "0.00";
                    dataInventory[i].SGST = 0;
                    dataInventory[i].TaxAmtSGST = "0.00";
                    dataInventory[i].IGST = 0;
                    dataInventory[i].TaxAmtIGST = "0.00";
                    dataInventory[i].TCSTaxAmount = 0;
                    dataInventory[i].RoundOff = 0;
                    dataInventory[i].NetAmount = 0;
                    dataInventory[i].UserFullName = "";
                }
            }
            List<RetDayWiseSalesListForExp> lstdaywisesales = dataInventory.Select(x => new RetDayWiseSalesListForExp()
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                CustomerCode = x.CustomerNumber,
                Party = x.Party,
                Area = x.Area,
                GrossAmt = x.GrossAmt,
                CGST = x.CGST,
                TaxAmtCGST = x.TaxAmtCGST,
                SGST = x.SGST,
                TaxAmtSGST = x.TaxAmtSGST,
                IGST = x.IGST,
                TaxAmtIGST = x.TaxAmtIGST,
                TaxCollectedAtSourcePayable = x.TCSTaxAmount,
                RoundOff = x.RoundOff,
                NetAmount = x.NetAmount,
                SalesPerson = x.UserFullName
            }).ToList();
            List<RetDayWiseCreditMemoListForExp> lstdaywisecreditmemo = dataInventory2.Select(x => new RetDayWiseCreditMemoListForExp()
            {
                CreditMemoNumber = x.CreditMemo,
                CreditMemoDate = CreditMemoDate,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                Party = x.Customer,
                Area = x.Area,
                GrossAmt = x.Amount,
                CGST = x.CGST,
                TaxAmtCGST = x.TaxAmtCGST,
                SGST = x.SGST,
                TaxAmtSGST = x.TaxAmtSGST,
                IGST = x.IGST,
                TaxAmtIGST = x.TaxAmtIGST,
                RoundOff = x.RoundOff,
                NetAmount = x.NetAmount,
                SalesPerson = x.UserFullName,
                ReferenceNumber = x.ReferenceNumber
            }).ToList();
            List<RetDayWiseSalesManListForExp> lstdaywisesalesman = dataInventory4.Select(x => new RetDayWiseSalesManListForExp()
            {
                SalesPerson = x.SalesPerson,
                GrossAmt = x.GrossAmtTotal,
                TaxAmt = x.TaxAmtTotal,
                RoundOff = x.RoundOffTotal,
                NetAmt = x.NetAmtTotal
            }).ToList();
            List<RetDayWiseTaxListForExp> lstdaywisetax = dataInventory3.Select(x => new RetDayWiseTaxListForExp()
            {
                GrossAmt = x.GrossAmtTotal,
                CGST = x.CGST,
                TaxAmtCGST = x.TaxAmtCGST,
                SGST = x.SGST,
                TaxAmtSGST = x.TaxAmtSGST,
                IGST = x.IGST,
                TaxAmtIGST = x.TaxAmtIGST,
                TaxAmt = x.TaxAmtTotal,
                TaxCollectedAtSourcePayable = x.TCSAmtTotal,
                RoundOff = x.RoundOffTotal,
                NetAmt = x.NetAmtTotal
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdaywisesales));
            ds.Tables.Add(ToDataTable(lstdaywisecreditmemo));
            ds.Tables.Add(ToDataTable(lstdaywisetax));
            ds.Tables.Add(ToDataTable(lstdaywisesalesman));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "Report.xls");
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

        public ActionResult CustomerMasterList()
        {
            List<RetCustomerListResponse> objModel = _reportservice.GetAllCustomerMasterList();
            return View(objModel);
        }

        public ActionResult ExportExcelCustomerList()
        {
            var dataInventory = _reportservice.GetAllCustomerMasterList();
            List<RetCustomerListForExp> lstcustomers = dataInventory.Select(x => new RetCustomerListForExp() { CustomerName = x.CustomerName, CustomerNumber = x.CustomerNumber, BillingAddressLine1 = x.BillingAddressLine1, BillingAddressLine2 = x.BillingAddressLine2, AreaName = x.AreaName, State = x.State, Country = x.Country, TaxNo = x.TaxNo, FSSAI = x.FSSAI, TaxName = x.TaxName, CustomerTypeName = x.CustomerTypeName, ContactName = x.ContactName, ContactNumber = x.ContactNumber, ContactEmail = x.ContactEmail, SalesPerson = x.UserFullName }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstcustomers));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "CustomerMaster.xls");
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
        public ActionResult GetEventDateForSameYear(long EventID, long StartYear)
        {
            GetEvent Event = new GetEvent();
            if (EventID != 0 && StartYear != 0)
            {
                Event = _whsreportservice.GetEventDateForSameYear(EventID, StartYear);
            }
            return Json(Event, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEventDateForDiffYear(long EventID, long StartYear, long EndYear)
        {
            GetEvent Event = new GetEvent();
            if (EventID != 0 && StartYear != 0 && EndYear != 0)
            {
                Event = _whsreportservice.GetEventDateForDiffYear(EventID, StartYear, EndYear);
            }
            return Json(Event, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustGroupProductWiseSales()
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
            ViewBag.Product = _orderservice.GetAllRetProductName();
            ViewBag.AreaList = _commonservice.GetAllRetAreaList();
            ViewBag.ProductCategoryList = _productservice.GetAllProductCategoryList();
            ViewBag.CustomerGroup = _customerservice.GetAllCustomerGroupName();
            return View();
        }



        public ActionResult CustGroupTotalProductWiseSalesList(RetCustGroupProductWiseSalesList model)
        {
            if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
            {
                if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    model.StartDate = Convert.ToDateTime(Session["txtFrom1"]);
                }
                else
                {
                    Session["txtFrom1"] = model.StartDate.ToString("MM-dd-yyyy");
                }

                if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    model.EndDate = Convert.ToDateTime(Session["txtTo1"]);
                }
                else
                {
                    Session["txtTo1"] = model.EndDate.ToString("MM-dd-yyyy");
                }
                List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetRetCustGroupTotalProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID, model.CustomerGroupID);
                List<RetCustGroupProductMainListByMonth> lst = new List<RetCustGroupProductMainListByMonth>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
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
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  TotalKg = Math.Round(item.TotalKg, 3),
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
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
                            if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
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
                    for (int col = 3; col < dt.Columns.Count - 2; col++)
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
            else
            { return View(); }
        }

        public ActionResult CustGroupProductWiseSalesList(RetCustGroupProductWiseSalesList model)
        {
            if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
            {
                if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
                    model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
                }
                else
                {
                    Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
                    Session["ProductQtyID"] = model.ProductQtyID;
                }

                if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    model.EndDate = Convert.ToDateTime(Session["txtTo"]);
                    model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
                }
                else
                {
                    Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
                    Session["ProductQtyID"] = model.ProductQtyID;
                }
                List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetCustGroupProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID, model.CustomerGroupID);
                List<RetCustGroupProductMainListByMonth> lst = new List<RetCustGroupProductMainListByMonth>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
                dt.Columns.Add("ProductName");
                for (int i = 0; i < lst.Count; i++)
                {
                    var lstcust = (from item in objlst
                                   select new
                                   {
                                       CustomerName = item.CustomerName
                                   })
                  .ToList()
                  .Distinct();
                    var listcust = lstcust.Where(x => x.CustomerName.Contains("(" + (lst[i].MonthName) + "-" + lst[i].YearName + ")")).ToList();
                    for (int j = 0; j < listcust.Count; j++)
                    {
                        dt.Columns.Add(listcust[j].CustomerName);
                    }
                }
                dt.Columns.Add("Total");
                dt.Columns.Add("TotalKg");
                dt.Columns.Add("Amount");
                var result = (from item in objlst
                              select new
                              {
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  TotalKg = Math.Round(item.TotalKg, 3),
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
                    dr[1] = result.ToList()[i].ProductName;
                    dr["TotalKg"] = result.ToList()[i].TotalKg;
                    dr["Amount"] = result.ToList()[i].TotalAmount;
                    dt.Rows.Add(dr);
                }
                int cnt = 1;
                dt.Columns.Add("Sr.No").SetOrdinal(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k][1] != "")
                        {
                            var listmain = objlst.Where(x => x.ProductQtyID == Convert.ToInt64(dt.Rows[k][1]) && x.CustomerName == dt.Columns[i].ColumnName.ToString()).ToList();
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                                cnt++;
                            }
                            if (listmain.Count > 0)
                            {
                                dt.Rows[k][i] = listmain[0].Quantity;
                            }
                        }
                    }
                }
                decimal sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    sum = 0;
                    for (int col = 3; col < dt.Columns.Count - 2; col++)
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
                    dt.Rows[t][dt.Columns.Count - 3] = Math.Round(sum, 0);
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
            else
            { return View(); }
        }

        public ActionResult CustGroupProductWiseDailySalesList(RetCustGroupProductWiseSalesList model)
        {
            Session["frprcustid"] = "";
            Session["frpruid"] = "";
            Session["frprtxtfrom"] = "";
            Session["frprtxtto"] = "";
            Session["frprproductid"] = "";
            Session["frprcategoryid"] = "";
            Session["frprareaid"] = "";
            if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
            {
                if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
                    model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
                }
                else
                {
                    Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
                    Session["ProductQtyID"] = model.ProductQtyID;
                }
                if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    model.EndDate = Convert.ToDateTime(Session["txtTo"]);
                    model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
                }
                else
                {
                    Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
                    Session["ProductQtyID"] = model.ProductQtyID;
                }
                List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetCustGroupProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID, model.CustomerGroupID);
                List<RetCustGroupProductMainListByDay> lst = new List<RetCustGroupProductMainListByDay>();
                lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
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
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  TotalKg = Math.Round(item.TotalKg, 3),
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
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
                            if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
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
                    for (int col = 3; col < dt.Columns.Count - 2; col++)
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
            else { return View(); }
        }




        //public ActionResult CustGroupTotalProductWiseSalesList(RetCustGroupProductWiseSalesList model)
        //{
        //    if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
        //    {
        //        if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
        //        {
        //            model.StartDate = Convert.ToDateTime(Session["txtFrom1"]);
        //        }
        //        else
        //        {
        //            Session["txtFrom1"] = model.StartDate.ToString("MM-dd-yyyy");
        //        }

        //        if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
        //        {
        //            model.EndDate = Convert.ToDateTime(Session["txtTo1"]);
        //        }
        //        else
        //        {
        //            Session["txtTo1"] = model.EndDate.ToString("MM-dd-yyyy");
        //        }
        //        List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetRetCustGroupTotalProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID, model.CustomerGroupID);
        //        List<RetCustGroupProductMainListByMonth> lst = new List<RetCustGroupProductMainListByMonth>();
        //        lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
        //        lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("ProductQtyID");
        //        dt.Columns.Add("ProductName");
        //        for (int i = 0; i < lst.Count; i++)
        //        {
        //            dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
        //        }
        //        dt.Columns.Add("Total");
        //        var result = (from item in objlst
        //                      select new
        //                      {
        //                          ProductQtyID = item.ProductQtyID,
        //                          ProductName = item.ProductName,
        //                      })
        //          .ToList()
        //          .Distinct();
        //        for (int i = 0; i < result.ToList().Count; i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = result.ToList()[i].ProductQtyID;
        //            dr[1] = result.ToList()[i].ProductName;
        //            dt.Rows.Add(dr);
        //        }
        //        int cnt = 1;
        //        dt.Columns.Add("Sr.No").SetOrdinal(0);
        //        for (int i = 0; i < lst.Count; i++)
        //        {
        //            for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
        //            {
        //                for (int k = 0; k < dt.Rows.Count; k++)
        //                {
        //                    if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
        //                    {
        //                        if (cnt <= dt.Rows.Count)
        //                        {
        //                            dt.Rows[cnt - 1][0] = cnt;
        //                        }
        //                        dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;

        //                        cnt++;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        decimal sum = 0;
        //        for (int t = 0; t < dt.Rows.Count; t++)
        //        {
        //            sum = 0;
        //            for (int col = 3; col < dt.Columns.Count - 1; col++)
        //            {
        //                if (dt.Rows[t][col] == DBNull.Value)
        //                {
        //                    dt.Rows[t][col] = "0";
        //                }
        //                sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //                if (dt.Rows[t][col] == "0")
        //                {
        //                    dt.Rows[t][col] = "";
        //                }
        //            }
        //            dt.Rows[t][lst.Count + 3] = Math.Round(sum, 0);
        //        }
        //        DataRow drtot = dt.NewRow();
        //        for (int col = 3; col < dt.Columns.Count; col++)
        //        {
        //            sum = 0;
        //            for (int t = 0; t < dt.Rows.Count; t++)
        //            {
        //                if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
        //                {
        //                    dt.Rows[t][col] = "0";
        //                }
        //                sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //                if (dt.Rows[t][col] == "0")
        //                {
        //                    dt.Rows[t][col] = "";
        //                }
        //            }
        //            if (col == 3)
        //            {
        //                drtot[0] = "";
        //                drtot[1] = "";
        //                drtot[2] = "";
        //                drtot[col] = sum;
        //                dt.Rows.Add(drtot);
        //            }
        //            else
        //            {
        //                drtot[col] = sum;
        //            }
        //        }
        //        return View(dt);
        //    }
        //    else
        //    { return View(); }
        //}

        //public ActionResult CustGroupProductWiseSalesList(RetCustGroupProductWiseSalesList model)
        //{
        //    if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
        //    {
        //        if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
        //        {
        //            model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
        //            model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
        //        }
        //        else
        //        {
        //            Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
        //            Session["ProductQtyID"] = model.ProductQtyID;
        //        }

        //        if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
        //        {
        //            model.EndDate = Convert.ToDateTime(Session["txtTo"]);
        //            model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
        //        }
        //        else
        //        {
        //            Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
        //            Session["ProductQtyID"] = model.ProductQtyID;
        //        }
        //        List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetCustGroupProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID, model.CustomerGroupID);
        //        List<RetCustGroupProductMainListByMonth> lst = new List<RetCustGroupProductMainListByMonth>();
        //        lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
        //        lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("ProductQtyID");
        //        dt.Columns.Add("ProductName");
        //        for (int i = 0; i < lst.Count; i++)
        //        {
        //            var lstcust = (from item in objlst
        //                           select new
        //                           {
        //                               CustomerName = item.CustomerName
        //                           })
        //          .ToList()
        //          .Distinct();
        //            var listcust = lstcust.Where(x => x.CustomerName.Contains("(" + (lst[i].MonthName) + "-" + lst[i].YearName + ")")).ToList();
        //            for (int j = 0; j < listcust.Count; j++)
        //            {
        //                dt.Columns.Add(listcust[j].CustomerName);
        //            }
        //        }
        //        dt.Columns.Add("Total");
        //        var result = (from item in objlst
        //                      select new
        //                      {
        //                          ProductQtyID = item.ProductQtyID,
        //                          ProductName = item.ProductName,
        //                      })
        //          .ToList()
        //          .Distinct();
        //        for (int i = 0; i < result.ToList().Count; i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = result.ToList()[i].ProductQtyID;
        //            dr[1] = result.ToList()[i].ProductName;
        //            dt.Rows.Add(dr);
        //        }
        //        int cnt = 1;
        //        dt.Columns.Add("Sr.No").SetOrdinal(0);
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            for (int k = 0; k < dt.Rows.Count; k++)
        //            {
        //                if (dt.Rows[k][1] != "")
        //                {
        //                    var listmain = objlst.Where(x => x.ProductQtyID == Convert.ToInt64(dt.Rows[k][1]) && x.CustomerName == dt.Columns[i].ColumnName.ToString()).ToList();
        //                    if (cnt <= dt.Rows.Count)
        //                    {
        //                        dt.Rows[cnt - 1][0] = cnt;
        //                        cnt++;
        //                    }
        //                    if (listmain.Count > 0)
        //                    {
        //                        dt.Rows[k][i] = listmain[0].Quantity;
        //                    }
        //                }
        //            }
        //        }
        //        decimal sum = 0;
        //        for (int t = 0; t < dt.Rows.Count; t++)
        //        {
        //            sum = 0;
        //            for (int col = 3; col < dt.Columns.Count - 1; col++)
        //            {
        //                if (dt.Rows[t][col] == DBNull.Value)
        //                {
        //                    dt.Rows[t][col] = "0";
        //                }
        //                sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //                if (dt.Rows[t][col] == "0")
        //                {
        //                    dt.Rows[t][col] = "";
        //                }
        //            }
        //            dt.Rows[t][dt.Columns.Count - 1] = Math.Round(sum, 0);
        //        }

        //        DataRow drtot = dt.NewRow();
        //        for (int col = 3; col < dt.Columns.Count; col++)
        //        {
        //            sum = 0;
        //            for (int t = 0; t < dt.Rows.Count; t++)
        //            {
        //                if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
        //                {
        //                    dt.Rows[t][col] = "0";
        //                }
        //                sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //                if (dt.Rows[t][col] == "0")
        //                {
        //                    dt.Rows[t][col] = "";
        //                }
        //            }
        //            if (col == 3)
        //            {
        //                drtot[0] = "";
        //                drtot[1] = "";
        //                drtot[2] = "";
        //                drtot[col] = sum;
        //                dt.Rows.Add(drtot);
        //            }
        //            else
        //            {
        //                drtot[col] = sum;
        //            }
        //        }
        //        return View(dt);
        //    }
        //    else
        //    { return View(); }
        //}

        //public ActionResult CustGroupProductWiseDailySalesList(RetCustGroupProductWiseSalesList model)
        //{
        //    Session["frprcustid"] = "";
        //    Session["frpruid"] = "";
        //    Session["frprtxtfrom"] = "";
        //    Session["frprtxtto"] = "";
        //    Session["frprproductid"] = "";
        //    Session["frprcategoryid"] = "";
        //    Session["frprareaid"] = "";
        //    if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
        //    {
        //        if (model.StartDate.ToString("MM-dd-yyyy") == "01-01-0001")
        //        {
        //            model.StartDate = Convert.ToDateTime(Session["txtFrom"]);
        //            model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
        //        }
        //        else
        //        {
        //            Session["txtFrom"] = model.StartDate.ToString("MM-dd-yyyy");
        //            Session["ProductQtyID"] = model.ProductQtyID;
        //        }
        //        if (model.EndDate.ToString("MM-dd-yyyy") == "01-01-0001")
        //        {
        //            model.EndDate = Convert.ToDateTime(Session["txtTo"]);
        //            model.ProductQtyID = Convert.ToInt64(Session["ProductQtyID"]);
        //        }
        //        else
        //        {
        //            Session["txtTo"] = model.EndDate.ToString("MM-dd-yyyy");
        //            Session["ProductQtyID"] = model.ProductQtyID;
        //        }
        //        List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetCustGroupProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID, model.CustomerGroupID);
        //        List<RetCustGroupProductMainListByDay> lst = new List<RetCustGroupProductMainListByDay>();
        //        lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
        //        lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("ProductQtyID");
        //        dt.Columns.Add("ProductName");
        //        for (int i = 0; i < lst.Count; i++)
        //        {
        //            dt.Columns.Add(lst[i].DayName + "-" + (lst[i].MonthName) + "-" + lst[i].YearName);
        //        }
        //        dt.Columns.Add("Total");
        //        var result = (from item in objlst
        //                      select new
        //                      {
        //                          ProductQtyID = item.ProductQtyID,
        //                          ProductName = item.ProductName,
        //                      })
        //          .ToList()
        //          .Distinct();
        //        for (int i = 0; i < result.ToList().Count; i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = result.ToList()[i].ProductQtyID;
        //            dr[1] = result.ToList()[i].ProductName;
        //            dt.Rows.Add(dr);
        //        }
        //        int cnt = 1;
        //        dt.Columns.Add("Sr.No").SetOrdinal(0);
        //        for (int i = 0; i < lst.Count; i++)
        //        {
        //            for (int j = 0; j < lst[i].ListMainProduct.Count; j++)
        //            {
        //                for (int k = 0; k < dt.Rows.Count; k++)
        //                {
        //                    if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
        //                    {
        //                        if (cnt <= dt.Rows.Count)
        //                        {
        //                            dt.Rows[cnt - 1][0] = cnt;
        //                        }
        //                        dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
        //                        cnt++;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        decimal sum = 0;
        //        for (int t = 0; t < dt.Rows.Count; t++)
        //        {
        //            sum = 0;
        //            for (int col = 3; col < dt.Columns.Count - 1; col++)
        //            {
        //                if (dt.Rows[t][col] == DBNull.Value)
        //                {
        //                    dt.Rows[t][col] = "0";
        //                }
        //                sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //                if (dt.Rows[t][col] == "0")
        //                {
        //                    dt.Rows[t][col] = "";
        //                }
        //            }
        //            dt.Rows[t][lst.Count + 3] = Math.Round(sum, 0);
        //        }

        //        DataRow drtot = dt.NewRow();
        //        for (int col = 3; col < dt.Columns.Count; col++)
        //        {
        //            sum = 0;
        //            for (int t = 0; t < dt.Rows.Count; t++)
        //            {
        //                if (dt.Rows[t][col] == DBNull.Value || dt.Rows[t][col] == "")
        //                {
        //                    dt.Rows[t][col] = "0";
        //                }
        //                sum = sum + Convert.ToDecimal(dt.Rows[t][col]);
        //                if (dt.Rows[t][col] == "0")
        //                {
        //                    dt.Rows[t][col] = "";
        //                }
        //            }
        //            if (col == 3)
        //            {
        //                drtot[0] = "";
        //                drtot[1] = "";
        //                drtot[2] = "";
        //                drtot[col] = sum;
        //                dt.Rows.Add(drtot);
        //            }
        //            else
        //            {
        //                drtot[col] = sum;
        //            }
        //        }
        //        return View(dt);
        //    }
        //    else { return View(); }
        //}

        public ActionResult ExportExcelCustGroupProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID, long? CustomerGroupID)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (ProductCategoryID == null)
            {
                ProductCategoryID = 0;
            }
            if (ProductQtyID == null)
            {
                ProductQtyID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            if (CustomerGroupID != null)
            {
                List<RetCustGroupProductWiseSalesList> objlst = _reportservice.GetCustGroupProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value, CustomerGroupID.Value);
                List<RetCustGroupProductMainListByMonth> lst = new List<RetCustGroupProductMainListByMonth>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                DataTable dt = new DataTable("RetCustGroupWiseProductSales");
                dt.Columns.Add("ProductQtyID");
                dt.Columns.Add("ProductName");
                for (int i = 0; i < lst.Count; i++)
                {
                    var lstcust = (from item in objlst
                                   select new
                                   {
                                       CustomerName = item.CustomerName
                                   })
                  .ToList()
                  .Distinct();
                    var listcust = lstcust.Where(x => x.CustomerName.Contains("(" + (lst[i].MonthName) + "-" + lst[i].YearName + ")")).ToList();
                    for (int j = 0; j < listcust.Count; j++)
                    {
                        dt.Columns.Add(listcust[j].CustomerName);
                    }
                }
                dt.Columns.Add("Total");
                dt.Columns.Add("TotalKg");
                dt.Columns.Add("Amount");
                var result = (from item in objlst
                              select new
                              {
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  TotalKg = Math.Round(item.TotalKg, 3),
                                  TotalAmount = Math.Round(item.TotalAmount, 2),

                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
                    dr[1] = result.ToList()[i].ProductName;
                    dr["TotalKg"] = result.ToList()[i].TotalKg;
                    dr["Amount"] = result.ToList()[i].TotalAmount;
                    dt.Rows.Add(dr);
                }
                int cnt = 1;
                dt.Columns.Add("Sr.No").SetOrdinal(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k][1] != "")
                        {
                            var listmain = objlst.Where(x => x.ProductQtyID == Convert.ToInt64(dt.Rows[k][1]) && x.CustomerName == dt.Columns[i].ColumnName.ToString()).ToList();
                            if (cnt <= dt.Rows.Count)
                            {
                                dt.Rows[cnt - 1][0] = cnt;
                                cnt++;
                            }
                            if (listmain.Count > 0)
                            {
                                dt.Rows[k][i] = listmain[0].Quantity;
                            }
                        }
                    }
                }
                decimal sum = 0;
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    sum = 0;
                    for (int col = 3; col < dt.Columns.Count - 2; col++)
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
                    dt.Rows[t][dt.Columns.Count - 3] = Math.Round(sum, 0);
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
                List<RetCustGroupProductWiseSalesList> objlst2 = _reportservice.GetCustGroupProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value, CustomerGroupID.Value);
                List<RetCustGroupProductMainListByDay> lst2 = new List<RetCustGroupProductMainListByDay>();
                lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
                DataTable dt2 = new DataTable("ProductWiseDailySalesList");
                dt2.Columns.Add("ProductQtyID");
                dt2.Columns.Add("ProductName");
                for (int i = 0; i < lst2.Count; i++)
                {
                    dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
                }
                dt2.Columns.Add("Total");
                dt2.Columns.Add("TotalKg");
                dt2.Columns.Add("Amount");
                var result2 = (from item in objlst
                               select new
                               {
                                   ProductQtyID = item.ProductQtyID,
                                   ProductName = item.ProductName,
                                   TotalKg = Math.Round(item.TotalKg, 3),
                                   TotalAmount = Math.Round(item.TotalAmount, 2),
                               })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result2.ToList().Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr[0] = result2.ToList()[i].ProductQtyID;
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
                            if (dt2.Rows[k]["ProductQtyID"].ToString() == lst2[i].ListMainProduct[j].ProductQtyID.ToString())
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
                    for (int col = 3; col < dt2.Columns.Count - 2; col++)
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

                //NewReport
                List<RetCustGroupProductWiseSalesList> objlst3 = _reportservice.GetRetCustGroupTotalProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value, CustomerGroupID.Value);
                List<RetCustGroupProductMainListByMonth> lst3 = new List<RetCustGroupProductMainListByMonth>();
                lst3 = objlst3.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst3 = lst3.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                DataTable dt3 = new DataTable("ProductWiseTotalSalesList");
                dt3.Columns.Add("ProductQtyID");
                dt3.Columns.Add("ProductName");
                for (int i = 0; i < lst3.Count; i++)
                {
                    dt3.Columns.Add((lst3[i].MonthName) + "-" + lst3[i].YearName);
                }
                dt3.Columns.Add("Total");
                dt3.Columns.Add("TotalKg");
                dt3.Columns.Add("Amount");
                var result3 = (from item in objlst3
                               select new
                               {
                                   ProductQtyID = item.ProductQtyID,
                                   ProductName = item.ProductName,
                                   TotalKg = Math.Round(item.TotalKg, 3),
                                   TotalAmount = Math.Round(item.TotalAmount, 2),
                               })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result3.ToList().Count; i++)
                {
                    DataRow dr = dt3.NewRow();
                    dr[0] = result3.ToList()[i].ProductQtyID;
                    dr[1] = result3.ToList()[i].ProductName;
                    dr["TotalKg"] = result3.ToList()[i].TotalKg;
                    dr["Amount"] = result3.ToList()[i].TotalAmount;
                    dt3.Rows.Add(dr);
                }
                int cnt3 = 1;
                dt3.Columns.Add("Sr.No").SetOrdinal(0);
                for (int i = 0; i < lst3.Count; i++)
                {
                    for (int j = 0; j < lst3[i].ListMainProduct.Count; j++)
                    {
                        for (int k = 0; k < dt3.Rows.Count; k++)
                        {
                            if (dt3.Rows[k]["ProductQtyID"].ToString() == lst3[i].ListMainProduct[j].ProductQtyID.ToString())
                            {
                                if (cnt3 <= dt3.Rows.Count)
                                {
                                    dt3.Rows[cnt3 - 1][0] = cnt3;
                                }
                                dt3.Rows[k][i + 3] = lst3[i].ListMainProduct[j].Quantity;
                                cnt3++;
                                break;
                            }
                        }
                    }
                }
                decimal sum3 = 0;
                for (int t = 0; t < dt3.Rows.Count; t++)
                {
                    sum3 = 0;
                    for (int col = 3; col < dt3.Columns.Count - 2; col++)
                    {
                        if (dt3.Rows[t][col] == DBNull.Value)
                        {
                            dt3.Rows[t][col] = "0";
                        }
                        sum3 = sum3 + Convert.ToDecimal(dt3.Rows[t][col]);
                        if (dt3.Rows[t][col] == "0")
                        {
                            dt3.Rows[t][col] = "";
                        }
                    }
                    dt3.Rows[t][lst3.Count + 3] = Math.Round(sum3, 0);
                }
                DataRow drtot3 = dt3.NewRow();
                for (int col = 3; col < dt3.Columns.Count; col++)
                {
                    sum3 = 0;
                    for (int t = 0; t < dt3.Rows.Count; t++)
                    {
                        if (dt3.Rows[t][col] == DBNull.Value || dt3.Rows[t][col] == "")
                        {
                            dt3.Rows[t][col] = "0";
                        }
                        sum3 = sum3 + Convert.ToDecimal(dt3.Rows[t][col]);
                        if (dt3.Rows[t][col] == "0")
                        {
                            dt3.Rows[t][col] = "";
                        }
                    }
                    if (col == 3)
                    {
                        drtot3[0] = "";
                        drtot3[1] = "";
                        drtot3[2] = "";
                        drtot3[col] = sum3;
                        dt3.Rows.Add(drtot3);
                    }
                    else
                    {
                        drtot3[col] = sum3;
                    }
                }
                dt.Columns.Remove("ProductQtyID");
                dt2.Columns.Remove("ProductQtyID");
                dt3.Columns.Remove("ProductQtyID");
                DataSet ds = new DataSet();
                ds.Tables.Add(dt3);
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
                    Response.AddHeader("content-disposition", "attachment;filename= " + "ProductWiseSales.xls");
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

        [HttpPost]
        public PartialViewResult ViewBillWiseOrderListForCustGroupProductWiseSalesReport(RetOrderListResponse model)
        {
            Session["rcgrcustid"] = "";
            Session["rcgruid"] = "";
            Session["rcgrtxtfrom"] = "";
            Session["rcgrtxtto"] = "";
            Session["rcgrproductqtyid"] = "";
            Session["rcgrareaid"] = "";
            Session["rcgrcategoryid"] = "";
            Session["rcgrcustgroupid"] = "";

            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<RetOrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForCustGroupProductWiseSalesReport(string invoicenumber, Int64? custid, Int64? uid, string txtfrom, string txtto, string productqtyid, string categoryid, string areaid, string custgroupid, long orderid = 0)
        {
            Session["rcgrcustid"] = custid;
            Session["rcgruid"] = uid;
            Session["rcgrtxtfrom"] = txtfrom;
            Session["rcgrtxtto"] = txtto;
            Session["rcgrproductqtyid"] = productqtyid;
            Session["rcgrcategoryid"] = categoryid;
            Session["rcgrareaid"] = areaid;
            Session["rcgrcustgroupid"] = custgroupid;

            if (!string.IsNullOrEmpty(invoicenumber))
            {
                List<RetOrderQtyList> objModel = _orderservice.GetBillWiseInvoiceForOrder(invoicenumber, orderid); //2
                return View(objModel);
            }
            else
            {
                return View();
            }
        }

        public ActionResult CustGroupSalesManWiseSales()
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
            ViewBag.CustomerGroup = _customerservice.GetAllCustomerGroupName();
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            return View();
        }

        public ActionResult CustGroupSalesManWiseSalesList(RetCustGroupSalesManWiseSalesList model)
        {
            if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
            {
                List<RetCustGroupSalesManWiseSalesList> objlst = _reportservice.GetCustGroupSalesManWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.CustomerGroupID);
                List<RetCustGroupSalesMainListByMonth> lst = new List<RetCustGroupSalesMainListByMonth>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupSalesMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("CustomerID");
                dt.Columns.Add("CustomerName");
                dt.Columns.Add("Area");
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
                                  AreaName = item.AreaName,
                                  UserName = item.UserName,
                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].CustomerID;
                    dr[1] = result.ToList()[i].CustomerName;
                    dr[2] = result.ToList()[i].AreaName;
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
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].Quantity;
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
            else
            {
                return View();
            }
        }

        public ActionResult CustGroupSalesManWiseDailySalesList(RetCustGroupSalesManWiseSalesList model)
        {
            Session["rsrcustid"] = "";
            Session["rsruid"] = "";
            Session["rsrtxtfrom"] = "";
            Session["rsrtxtto"] = "";
            Session["rsrareaid"] = "";
            if (model.CustomerGroupID != null && model.CustomerGroupID != 0)
            {
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
                List<RetCustGroupSalesManWiseSalesList> objlst = _reportservice.GetCustGroupSalesManWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.CustomerGroupID);
                List<RetCustGroupSalesMainListByDay> lst = new List<RetCustGroupSalesMainListByDay>();
                lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetCustGroupSalesMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("CustomerID");
                dt.Columns.Add("CustomerName");
                dt.Columns.Add("Area");
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
                                  AreaName = item.AreaName,
                                  UserName = item.UserName,
                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].CustomerID;
                    dr[1] = result.ToList()[i].CustomerName;
                    dr[2] = result.ToList()[i].AreaName;
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
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].Quantity;
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
            else
            {
                return View();
            }
        }

        [HttpPost]
        public PartialViewResult ViewBillWiseOrderListForCustGroupSalesManWiseSalesReport(RetOrderListResponse model)
        {
            Session["rcgsrcustid"] = "";
            Session["rcgsruid"] = "";
            Session["rcgsrtxtfrom"] = "";
            Session["rcgsrtxtto"] = "";
            Session["rcgsrareaid"] = "";
            Session["rcgsrcustgroupid"] = "";

            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<RetOrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForCustGroupSalesManWiseSalesReport(string invoicenumber, Int64? custid, Int64? uid, string txtfrom, string txtto, string areaid, string custgroupid, long orderid = 0)
        {
            Session["rcgsrcustid"] = custid;
            Session["rcgsruid"] = uid;
            Session["rcgsrtxtfrom"] = txtfrom;
            Session["rcgsrtxtto"] = txtto;
            Session["rcgsrareaid"] = areaid;
            Session["rcgsrcustgroupid"] = custgroupid;
            if (!string.IsNullOrEmpty(invoicenumber))
            {
                List<RetOrderQtyList> objModel = _orderservice.GetBillWiseInvoiceForOrder(invoicenumber, orderid); //3
                return View(objModel);
            }
            else
            {
                return View();
            }
        }

        public ActionResult ExportExcelCustGroupSalesManWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? UserID, long? CustomerGroupID)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            if (CustomerGroupID != null)
            {
                List<RetCustGroupSalesManWiseSalesList> objlst = _reportservice.GetCustGroupSalesManWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, CustomerGroupID.Value);
                List<RetCustGroupSalesMainListByMonth> lst = new List<RetCustGroupSalesMainListByMonth>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetCustGroupSalesMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                DataTable dt = new DataTable("RetCustGroupWiseSales");
                dt.Columns.Add("CustomerID");
                dt.Columns.Add("CustomerName");
                dt.Columns.Add("Area");
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
                                  AreaName = item.AreaName,
                                  UserName = item.UserName,
                              })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].CustomerID;
                    dr[1] = result.ToList()[i].CustomerName;
                    dr[2] = result.ToList()[i].AreaName;
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
                                dt.Rows[k][i + 5] = lst[i].ListMainProduct[j].Quantity;
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
                List<RetCustGroupSalesManWiseSalesList> objlst2 = _reportservice.GetCustGroupSalesManWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, CustomerGroupID.Value);
                List<RetCustGroupSalesMainListByDay> lst2 = new List<RetCustGroupSalesMainListByDay>();
                lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new RetCustGroupSalesMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
                DataTable dt2 = new DataTable("RetailSalesWiseDailySalesList");
                dt2.Columns.Add("CustomerID");
                dt2.Columns.Add("CustomerName");
                dt2.Columns.Add("Area");
                dt2.Columns.Add("UserName");
                for (int i = 0; i < lst2.Count; i++)
                {
                    dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
                }
                dt2.Columns.Add("Total");
                var result2 = (from item in objlst2
                               select new
                               {
                                   CustomerID = item.CustomerID,
                                   CustomerName = item.CustomerName,
                                   AreaName = item.AreaName,
                                   UserName = item.UserName,
                               })
                  .ToList()
                  .Distinct();
                for (int i = 0; i < result2.ToList().Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr[0] = result2.ToList()[i].CustomerID;
                    dr[1] = result2.ToList()[i].CustomerName;
                    dr[2] = result2.ToList()[i].AreaName;
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
                                dt2.Rows[k][i + 5] = lst2[i].ListMainProduct[j].Quantity;
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
                    Response.AddHeader("content-disposition", "attachment;filename= " + "ProductWiseSales.xls");
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

        public ActionResult PouchWiseReport()
        {
            ViewBag.Pouch = _productservice.GetAllPouchName();
            return View();
        }

        [HttpPost]
        public PartialViewResult PouchWiseReportList(RetPouchListResponse model)
        {
            decimal TotalPouch = 0;
            List<RetPouchListResponse> objModel = null;
            List<RetPouchListResponse> pouchtotal = new List<RetPouchListResponse>();
            foreach (var item in model.PouchNameID)
            {
                objModel = _reportservice.GetPouchWiseReportList(model.StartDate, model.EndDate, item);
                if (objModel != null)
                {
                    foreach (var record in objModel)
                    {
                        pouchtotal.Add(record);
                        TotalPouch += record.Quantity;
                    }
                }
            }
            if (TotalPouch != 0)
            {
                pouchtotal[0].TotalPouch = TotalPouch;
            }
            return PartialView(pouchtotal);
        }

        public ActionResult ExportExcelPouchWiseReportList(DateTime? StartDate, DateTime? EndDate, List<string> PouchNameID)
        {
            decimal TotalPouch = 0;
            List<RetPouchListResponse> pouchtotal = new List<RetPouchListResponse>();
            string[] Pouch = PouchNameID[0].Split(',');
            foreach (var item in Pouch)
            {
                List<RetPouchListResponse> objModel = _reportservice.GetPouchWiseReportList(StartDate.Value, EndDate.Value, item);
                if (objModel != null)
                {
                    foreach (var record in objModel)
                    {
                        pouchtotal.Add(record);
                        TotalPouch += record.Quantity;
                    }
                }
            }
            List<RetPouchListForExport> lstdelsheet = pouchtotal.Select(x => new RetPouchListForExport() 
            { 
                Pouch = x.PouchName, 
                ProductName = x.ProductName, 
                Unit = x.Unit, 
                Quantity = x.Quantity 
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdelsheet));
            DataRow dRow = ds.Tables[0].NewRow();
            dRow["Unit"] = "Total";
            dRow["Quantity"] = TotalPouch;
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "PouchReport.xls");
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

        public ActionResult CashCounterReport()
        {
            ViewBag.CashOption = _commonservice.GetAllRetCashOption();
            // ViewBag.VehicleNo = _commonservice.GetRetVehicleNoList();
            return View();
        }

        [HttpPost]
        public PartialViewResult CashCounterReportList(RetCashCounterListResponse model)
        {
            int BySign = 0;
            if (model.IsCheckBySign == false)
            {
                BySign = 0;
            }
            else
            {
                BySign = 1;
            }
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            List<RetCashCounterListResponse> objlistoftempo = new List<RetCashCounterListResponse>();
            List<RetCashCounterListResponse> objlst = _reportservice.GetCashCounterReportList(model.AssignedDate, model.GodownID, BySign);
            objlistoftempo.AddRange(objlst);
            if (objlistoftempo.Count > 0)
            {
                objlistoftempo[0].CashTotal = objlst[objlst.Count - 1].CashTotal;
                objlistoftempo[0].ChequeTotal = objlst[objlst.Count - 1].ChequeTotal;
                objlistoftempo[0].CardTotal = objlst[objlst.Count - 1].CardTotal;
                objlistoftempo[0].SignTotal = objlst[objlst.Count - 1].SignTotal;
                objlistoftempo[0].OnlineTotal = objlst[objlst.Count - 1].OnlineTotal;
                objlistoftempo[0].AdjustAmountTotal = objlst[objlst.Count - 1].AdjustAmountTotal;
            }
            else
            {
                objlistoftempo.Add(new RetCashCounterListResponse());
                objlistoftempo[0].CashTotal = 0;
                objlistoftempo[0].ChequeTotal = 0;
                objlistoftempo[0].CardTotal = 0;
                objlistoftempo[0].SignTotal = 0;
                objlistoftempo[0].OnlineTotal = 0;
                objlistoftempo[0].AdjustAmountTotal = 0;
            }
            return PartialView(objlistoftempo);
        }

        public ActionResult ExportExcelCashCounterReport(string Date, List<string> VehicleNo, string TempoNo, long GodownID, bool IsCheckBySign)
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.ToString();
            }
            int BySign = 0;
            if (IsCheckBySign == false)
            {
                BySign = 0;
            }
            else
            {
                BySign = 1;
            }
            List<RetCashCounterListResponse> finallist = new List<RetCashCounterListResponse>();
            List<RetCashCounterListResponse> dataInventory = _reportservice.GetCashCounterReportList(Convert.ToDateTime(Date), GodownID, BySign);
            finallist.AddRange(dataInventory);

            //Add By Dhruvik
            List<RetCashCounterListResponse> dataInventory1 = _reportservice.GetRetChequeRetrunList(Convert.ToDateTime(Date), GodownID);
            finallist.AddRange(dataInventory1);

            List<RetCashCounterListResponse> dataInventory2 = _reportservice.GetRetChequeRetrunChargeList(Convert.ToDateTime(Date), GodownID);
            finallist.AddRange(dataInventory2);
            //Add By Dhruvik

            List<RetCashCounterListForExport> lstdelsheet = finallist.Select(x => new RetCashCounterListForExport()
            {
                Customer = x.Customer,
                Area = x.Area,
                VehicleNo = x.VehicleNo1,
                InvoiceNumber = x.InvoiceNumber1,
                Cash = x.Cash,
                Cheque = x.Cheque,
                Card = x.Card,
                Sign = x.Sign,
                Online = x.Online,
                AdjustAmount = x.AdjustAmount,
                Remarks = x.Remarks,
                Godown = x.GodownName,
                BankName = x.BankName,
                BankBranch = x.BankBranch,
                ChequeNo = x.ChequeNo,
                ChequeDate = x.ChequeDate1,
                IFCCode = x.IFCCode,
                BankNameForCard = x.BankNameForCard,
                TypeOfCard = x.TypeOfCard,
                OnlinePaymentDate = x.OnlinePaymentDate1,
                BankNameForOnline = x.BankNameForOnline,
                UTRNumber = x.UTRNumber,
                IsDeliveredstr = x.IsDeliveredstr
            }).ToList();

            var lstDayWiseSales = _reportservice.GetCashCounterDayWiseSalesManList(Convert.ToDateTime(Date), GodownID, BySign);
            List<RetDayWiseSalesManListForExport> lstDayWiseSalesExp = lstDayWiseSales.Select(x => new RetDayWiseSalesManListForExport()
            {
                SalesPerson = x.SalesPerson,
                Cash = x.Cash,
                Cheque = x.Cheque,
                Card = x.Card,
                Sign = x.Sign,
                Online = x.Online,
                AdjustAmount = x.AdjustAmount
            }).ToList();

            // 12 Sep 2020 Piyush Limbani
            List<RetVoucherCashCounterListResponse> finalvouchercashlist = new List<RetVoucherCashCounterListResponse>();
            List<RetVoucherCashCounterListResponse> datavouchercash = _reportservice.GetRetailExpenseVoucherCashCounterReportList(Convert.ToDateTime(Date), GodownID);
            finalvouchercashlist.AddRange(datavouchercash);
            List<RetVoucherCashCounterExp> lstvouchercash = finalvouchercashlist.Select(x => new RetVoucherCashCounterExp()
            {
                Customer = x.Customer,
                Area = x.Area,
                BillNumber = x.BillNumber,
                Cash = x.Cash,
                Cheque = x.Cheque,
                Card = x.Card,
                Sign = x.Sign,
                Online = x.Online,
                AdjustAmount = x.AdjustAmount,
                Remarks = x.Remarks,
                Godown = x.GodownName,
                VehicleNo = "0",
                BankName = x.BankName,
                BankBranch = x.BankBranch,
                ChequeNo = x.ChequeNo,
                ChequeDate = x.ChequeDate1,
                IFCCode = x.IFCCode,
                BankNameForCard = x.BankNameForCard,
                TypeOfCard = x.TypeOfCard,
                OnlinePaymentDate = x.OnlinePaymentDate1,
                BankNameForOnline = x.BankNameForOnline,
                UTRNumber = x.UTRNumber,
                IsDeliveredstr = x.IsDeliveredstr
            }).ToList();

            var lstvouchercashsalesmanwise = _reportservice.GetRetVoucherCashCounterDayWiseSalesManList(Convert.ToDateTime(Date), GodownID);
            List<RetVoucherDayWiseSalesManExp> lstVoucherCashDayWiseSalesExp = lstvouchercashsalesmanwise.Select(x => new RetVoucherDayWiseSalesManExp()
            {
                SalesPerson = x.SalesPerson,
                Cash = x.Cash,
                Cheque = x.Cheque,
                Card = x.Card,
                Sign = x.Sign,
                Online = x.Online,
                AdjustAmount = x.AdjustAmount
            }).ToList();
            // 12 Sep 2020 Piyush Limbani

            //DataSet ds = new DataSet();
            //if (finallist.Count > 0)
            //{
            //    ds.Tables.Add(ToDataTable(lstdelsheet));
            //    ds.Tables.Add(ToDataTable(lstDayWiseSalesExp));
            //    DataRow dRow = ds.Tables[0].NewRow();
            //    dRow["Cash"] = dataInventory[dataInventory.Count - 1].CashTotal;
            //    dRow["Cheque"] = dataInventory[dataInventory.Count - 1].ChequeTotal;
            //    dRow["Card"] = dataInventory[dataInventory.Count - 1].CardTotal;
            //    dRow["Sign"] = dataInventory[dataInventory.Count - 1].SignTotal;
            //    dRow["Online"] = dataInventory[dataInventory.Count - 1].OnlineTotal;
            //    dRow["AdjustAmount"] = dataInventory[dataInventory.Count - 1].AdjustAmountTotal;
            //    ds.Tables[0].Rows.Add(dRow);
            //}
            //else
            //{
            //    ds.Tables.Add(ToDataTable(lstdelsheet));
            //    ds.Tables.Add(ToDataTable(lstDayWiseSalesExp));
            //    DataRow dRow = ds.Tables[0].NewRow();
            //    dRow["Cash"] = 0;
            //    dRow["Cheque"] = 0;
            //    dRow["Card"] = 0;
            //    dRow["Sign"] = 0;
            //    dRow["Online"] = 0;
            //    dRow["AdjustAmount"] = 0;
            //    ds.Tables[0].Rows.Add(dRow);
            //}

            DataSet ds = new DataSet();
            if (finallist.Count > 0)
            {
                ds.Tables.Add(ToDataTable(lstdelsheet));
                ds.Tables.Add(ToDataTable(lstDayWiseSalesExp));

                // 12 Sep 2020 Piyush Limbani
                ds.Tables.Add(ToDataTable(lstvouchercash));
                ds.Tables.Add(ToDataTable(lstVoucherCashDayWiseSalesExp));
                // 12 Sep 2020 Piyush Limbani

                DataRow dRow = ds.Tables[0].NewRow();
                //Add By Dhruvik
                if (dataInventory.Count > 0)
                {
                    dRow["Cash"] = dataInventory[dataInventory.Count - 1].CashTotal;
                    dRow["Cheque"] = dataInventory[dataInventory.Count - 1].ChequeTotal;
                    dRow["Card"] = dataInventory[dataInventory.Count - 1].CardTotal;
                    dRow["Sign"] = dataInventory[dataInventory.Count - 1].SignTotal;
                    dRow["Online"] = dataInventory[dataInventory.Count - 1].OnlineTotal;
                    dRow["AdjustAmount"] = dataInventory[dataInventory.Count - 1].AdjustAmountTotal;
                }
                else
                {
                    dRow["Cash"] = dataInventory[dataInventory1.Count - 1].CashTotal;
                    dRow["Cheque"] = dataInventory[dataInventory1.Count - 1].ChequeTotal;
                    dRow["Card"] = dataInventory[dataInventory1.Count - 1].CardTotal;
                    dRow["Sign"] = dataInventory[dataInventory1.Count - 1].SignTotal;
                    dRow["Online"] = dataInventory[dataInventory1.Count - 1].OnlineTotal;
                    dRow["AdjustAmount"] = dataInventory[dataInventory1.Count - 1].AdjustAmountTotal;
                }
                //Add By Dhruvik
                ds.Tables[0].Rows.Add(dRow);

                DataRow dRow2 = ds.Tables[1].NewRow();
                //Add By Dhruvik
                if (lstDayWiseSales.Count > 0)
                {
                    dRow2["Cash"] = lstDayWiseSales[lstDayWiseSales.Count - 1].CashTotal;
                    dRow2["Cheque"] = lstDayWiseSales[lstDayWiseSales.Count - 1].ChequeTotal;
                    dRow2["Card"] = lstDayWiseSales[lstDayWiseSales.Count - 1].CardTotal;
                    dRow2["Sign"] = lstDayWiseSales[lstDayWiseSales.Count - 1].SignTotal;
                    dRow2["Online"] = lstDayWiseSales[lstDayWiseSales.Count - 1].OnlineTotal;
                    dRow2["AdjustAmount"] = lstDayWiseSales[lstDayWiseSales.Count - 1].AdjustAmountTotal;
                }
                //Add By Dhruvik
                ds.Tables[1].Rows.Add(dRow2);

                // 12 Sep 2020 Piyush Limbani
                if (finalvouchercashlist.Count > 0)
                {
                    DataRow dRow3 = ds.Tables[2].NewRow();
                    dRow3["Cash"] = datavouchercash[datavouchercash.Count - 1].CashTotal;
                    dRow3["Cheque"] = datavouchercash[datavouchercash.Count - 1].ChequeTotal;
                    dRow3["Card"] = datavouchercash[datavouchercash.Count - 1].CardTotal;
                    dRow3["Sign"] = datavouchercash[datavouchercash.Count - 1].SignTotal;
                    dRow3["Online"] = datavouchercash[datavouchercash.Count - 1].OnlineTotal;
                    dRow3["AdjustAmount"] = datavouchercash[datavouchercash.Count - 1].AdjustAmountTotal;
                    ds.Tables[2].Rows.Add(dRow3);

                    DataRow dRow4 = ds.Tables[3].NewRow();
                    dRow4["Cash"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].CashTotal;
                    dRow4["Cheque"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].ChequeTotal;
                    dRow4["Card"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].CardTotal;
                    dRow4["Sign"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].SignTotal;
                    dRow4["Online"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].OnlineTotal;
                    dRow4["AdjustAmount"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].AdjustAmountTotal;
                    ds.Tables[3].Rows.Add(dRow4);
                }
                else
                {
                    //ds.Tables.Add(ToDataTable(lstvouchercash));
                    //ds.Tables.Add(ToDataTable(lstVoucherCashDayWiseSalesExp));
                    DataRow dRow3 = ds.Tables[2].NewRow();
                    dRow3["Cash"] = 0;
                    dRow3["Cheque"] = 0;
                    dRow3["Card"] = 0;
                    dRow3["Sign"] = 0;
                    dRow3["Online"] = 0;
                    dRow3["AdjustAmount"] = 0;
                    ds.Tables[2].Rows.Add(dRow3);

                    DataRow dRow4 = ds.Tables[3].NewRow();
                    dRow4["Cash"] = 0;
                    dRow4["Cheque"] = 0;
                    dRow4["Card"] = 0;
                    dRow4["Sign"] = 0;
                    dRow4["Online"] = 0;
                    dRow4["AdjustAmount"] = 0;
                    ds.Tables[3].Rows.Add(dRow4);

                }
                // 12 Sep 2020 Piyush Limbani
            }
            else if (finalvouchercashlist.Count > 0)
            {
                ds.Tables.Add(ToDataTable(lstvouchercash));
                ds.Tables.Add(ToDataTable(lstVoucherCashDayWiseSalesExp));
                DataRow dRow3 = ds.Tables[0].NewRow();
                dRow3["Cash"] = datavouchercash[datavouchercash.Count - 1].CashTotal;
                dRow3["Cheque"] = datavouchercash[datavouchercash.Count - 1].ChequeTotal;
                dRow3["Card"] = datavouchercash[datavouchercash.Count - 1].CardTotal;
                dRow3["Sign"] = datavouchercash[datavouchercash.Count - 1].SignTotal;
                dRow3["Online"] = datavouchercash[datavouchercash.Count - 1].OnlineTotal;
                dRow3["AdjustAmount"] = datavouchercash[datavouchercash.Count - 1].AdjustAmountTotal;
                ds.Tables[0].Rows.Add(dRow3);

                DataRow dRow4 = ds.Tables[1].NewRow();
                dRow4["Cash"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].CashTotal;
                dRow4["Cheque"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].ChequeTotal;
                dRow4["Card"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].CardTotal;
                dRow4["Sign"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].SignTotal;
                dRow4["Online"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].OnlineTotal;
                dRow4["AdjustAmount"] = lstvouchercashsalesmanwise[lstvouchercashsalesmanwise.Count - 1].AdjustAmountTotal;
                ds.Tables[1].Rows.Add(dRow4);

                //ds.Tables.Add(ToDataTable(lstdelsheet));
                //ds.Tables.Add(ToDataTable(lstDayWiseSalesExp));
                //DataRow dRow = ds.Tables[0].NewRow();
                //dRow["Cash"] = 0;
                //dRow["Cheque"] = 0;
                //dRow["Card"] = 0;
                //dRow["Sign"] = 0;
                //dRow["Online"] = 0;
                //dRow["AdjustAmount"] = 0;
                //ds.Tables[0].Rows.Add(dRow);               
            }
            else
            {
                ds.Tables.Add(ToDataTable(lstdelsheet));
                ds.Tables.Add(ToDataTable(lstDayWiseSalesExp));
                DataRow dRow = ds.Tables[0].NewRow();
                dRow["Cash"] = 0;
                dRow["Cheque"] = 0;
                dRow["Card"] = 0;
                dRow["Sign"] = 0;
                dRow["Online"] = 0;
                dRow["AdjustAmount"] = 0;
                ds.Tables[0].Rows.Add(dRow);

                DataRow dRow2 = ds.Tables[1].NewRow();
                dRow2["Cash"] = 0;
                dRow2["Cheque"] = 0;
                dRow2["Card"] = 0;
                dRow2["Sign"] = 0;
                dRow2["Online"] = 0;
                dRow2["AdjustAmount"] = 0;
                ds.Tables[1].Rows.Add(dRow2);

                ds.Tables.Add(ToDataTable(lstvouchercash));
                ds.Tables.Add(ToDataTable(lstVoucherCashDayWiseSalesExp));
                DataRow dRow3 = ds.Tables[2].NewRow();
                dRow3["Cash"] = 0;
                dRow3["Cheque"] = 0;
                dRow3["Card"] = 0;
                dRow3["Sign"] = 0;
                dRow3["Online"] = 0;
                dRow3["AdjustAmount"] = 0;
                ds.Tables[2].Rows.Add(dRow3);

                DataRow dRow4 = ds.Tables[3].NewRow();
                dRow4["Cash"] = 0;
                dRow4["Cheque"] = 0;
                dRow4["Card"] = 0;
                dRow4["Sign"] = 0;
                dRow4["Online"] = 0;
                dRow4["AdjustAmount"] = 0;
                ds.Tables[3].Rows.Add(dRow4);

            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "CashCounterReport.xls");
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

        public ActionResult BillHistory()
        {
            ViewBag.Customer = _commonservice.GetActiveRetCustomerName(0);
            return View();
        }

        [HttpPost]
        public PartialViewResult BillHistoryList(RetBillHistoryListResponse model)
        {
            long OrderID = 0;
            string UpdatedOn = "";
            if (model.InvoiceNumber != null && model.Year != null)
            {
                List<GetRetOrderIDResponse> objorder = _reportservice.GetOrderIDForBillHistory(model.InvoiceNumber, model.Year);
                OrderID = objorder[0].OrderID;
                UpdatedOn = objorder[0].UpdatedOn.ToString();
            }
            List<RetBillHistoryListResponse> objModel = _reportservice.GetBillHistoryList(model.InvoiceNumber, OrderID, UpdatedOn, model.FromDate, model.ToDate, model.CustomerID);
            return PartialView(objModel);
        }

        [HttpPost]
        public PartialViewResult CashCounterDayWiseSalesManList(RetCashCounterDayWiseSalesManList model)
        {
            int BySign = 0;
            if (model.IsCheckBySign == false)
            {
                BySign = 0;
            }
            else
            {
                BySign = 1;
            }
            List<RetCashCounterDayWiseSalesManList> objlst = _reportservice.GetCashCounterDayWiseSalesManList(model.AssignedDate, model.GodownID, BySign);
            return PartialView(objlst);
        }

        [HttpPost]
        public ActionResult UpdatePayment(RetPayment data)
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

                    //Add By Dhruvik
                    if (data.Cheque == true)
                    {
                        if (data.ByCash == 0)
                        {
                            data.Amount = 0;
                        }
                    }
                    //Add By Dhruvik

                    bool respose = _reportservice.UpdatePayment(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllVehicleNoForDeliverysheetReport(DateTime AssignedDate)
        {
            try
            {
                var detail = _reportservice.GetAllVehicleNoForDeliverysheetReport(AssignedDate);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ex.InnerException, JsonRequestBehavior.AllowGet);
                //return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SalesManWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            List<RetSalesManWiseSalesList> objlist2 = new List<RetSalesManWiseSalesList>();

            if (UserID > 0)
            {
                string CustomerIDs = _reportservice.GetRetCustomerIDForSalesList2(StartDate, EndDate, CustomerID, AreaID, UserID, DaysofWeek);
                if (CustomerIDs != "")
                {
                    List<RetSalesManWiseSalesList> objlst = _reportservice.GetRetSalesManWiseSalesList2(CustomerIDs, UserID, AreaID, DaysofWeek);
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

        public ActionResult ExportExcelZeroSalesReport(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {

            string CustomerIDs = _reportservice.GetRetCustomerIDForSalesList2(StartDate, EndDate, CustomerID, AreaID, UserID, DaysofWeek);
            if (CustomerIDs != "")
            {
                var CustomerList = _reportservice.GetRetSalesManWiseSalesList2(CustomerIDs, UserID, AreaID, DaysofWeek);
                List<CustomerListForZeroSalesExport> lstVoucher = CustomerList.Select(x => new CustomerListForZeroSalesExport()
                {
                    SrNo = x.SrNo,
                    CustomerName = x.CustomerName,
                    AreaName = x.AreaName,
                    CellNo = x.CellNo,
                    SalesPerson = x.UserName,
                    TotalSales = x.TotalQuantity
                }).ToList();

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
                    Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "RetailCustomerListForZeroSales.xls");
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









        [HttpGet]
        public ActionResult ProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID)
        {
            List<ProductWiseSalesList> objlist2 = new List<ProductWiseSalesList>();

            if (CustomerID > 0)
            {
                string ProductQtyIDs = _reportservice.GetProductIDForRetProductWiseSalesList2(StartDate, EndDate, CustomerID, AreaID, ProductCategoryID, ProductQtyID, UserID);
                if (ProductQtyIDs != "")
                {
                    DateTime EndDate1 = DateTime.Now;
                    DateTime StartDate1 = EndDate1.AddYears(-1);
                    List<RetProductWiseSalesList> objlst = _reportservice.GetProductWiseSalesList2(StartDate1, EndDate1, ProductQtyIDs, CustomerID, AreaID, ProductCategoryID, UserID);
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

        public ActionResult ExportExcelZeroProductReport(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID)
        {

            string ProductQtyIDs = _reportservice.GetProductIDForRetProductWiseSalesList2(StartDate, EndDate, CustomerID, AreaID, ProductCategoryID, ProductQtyID, UserID);
            if (ProductQtyIDs != "")
            {
                DateTime EndDate1 = DateTime.Now;
                DateTime StartDate1 = EndDate1.AddYears(-1);

                var ProductList = _reportservice.GetProductWiseSalesList2(StartDate1, EndDate1, ProductQtyIDs, CustomerID, AreaID, ProductCategoryID, UserID);
                List<RetProductForZeroSalesExport> lstVoucher = ProductList.Select(x => new RetProductForZeroSalesExport() { SrNo = x.SrNo, CategoryName = x.CategoryName, ArticleCode = x.ArticleCode, ProductName = x.ProductName, Quantity = x.Quantity }).ToList();

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
                    Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "RetProductListForZeroSales.xls");
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


        // GST Report
        public ActionResult ProductWiseGSTReport(DateTime? StartDate, DateTime? EndDate, string Tax, long? TaxID, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID)
        {
            Session["fprcustid"] = CustomerID;
            Session["fpruid"] = UserID;
            Session["fprtxtfrom"] = StartDate;
            Session["fprtxtto"] = EndDate;
            Session["fprproductid"] = ProductQtyID;
            Session["fprcategoryid"] = ProductCategoryID;
            Session["fprareaid"] = AreaID;
            return View();
        }

        public ActionResult ProductWiseGSTReportList(RetProductWiseGSTReportList model)
        {
            ViewBag.Tax = model.Tax;
            DataTable dt = new DataTable();
            if (model.Tax == "SGST")
            {
                List<RetProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(model.StartDate, model.EndDate, model.Tax, model.TaxID, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID);
                List<RetProductWiseGSTReportList> objlst = new List<RetProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    RetProductWiseGSTReportList newobj = new RetProductWiseGSTReportList();
                    newobj.ProductQtyID = item.ProductQtyID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.Quantity = item.Quantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = model.CustomerID;
                    newobj.AreaID = model.AreaID;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, model.TaxID, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.Quantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.Quantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.CGST = item.CGST;
                    newobj.TaxAmtCGST = item.TaxAmtCGST;
                    newobj.SGST = item.SGST;
                    newobj.TaxAmtSGST = item.TaxAmtSGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<RetProductMainListByMonthGST> lst = new List<RetProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
                dt.Columns.Add("ProductName");
                dt.Columns.Add("HSNNumber");
                for (int i = 0; i < lst.Count; i++)
                {
                    dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
                }
                dt.Columns.Add("Total");
                dt.Columns.Add("TaxableAmount");
                dt.Columns.Add("CGSTPer");
                dt.Columns.Add("CGSTAmt");
                dt.Columns.Add("SGSTPer");
                dt.Columns.Add("SGSTAmt");
                dt.Columns.Add("Amount");
                var result = (from item in objlst
                              select new
                              {
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  HSNNumber = item.HSNNumber,
                                  FinalTaxableAmount = Math.Round(item.FinalTaxableAmount, 2),
                                  CGST = item.CGST,
                                  TaxAmtCGST = item.TaxAmtCGST,
                                  SGST = item.SGST,
                                  TaxAmtSGST = item.TaxAmtSGST,
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();

                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
                    dr[1] = result.ToList()[i].ProductName;
                    dr[2] = result.ToList()[i].HSNNumber;
                    dr["TaxableAmount"] = result.ToList()[i].FinalTaxableAmount;
                    dr["CGSTPer"] = result.ToList()[i].CGST;
                    dr["CGSTAmt"] = result.ToList()[i].TaxAmtCGST;
                    dr["SGSTPer"] = result.ToList()[i].SGST;
                    dr["SGSTAmt"] = result.ToList()[i].TaxAmtSGST;
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
                            if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 4] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                    for (int col = 4; col < dt.Columns.Count - 6; col++)
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

                    dt.Rows[t][lst.Count + 4] = sum;
                }

                DataRow drtot = dt.NewRow();
                for (int col = 4; col < dt.Columns.Count; col++)
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
                    if (col == 4)
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
            }
            else
            {
                List<RetProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(model.StartDate, model.EndDate, model.Tax, model.TaxID, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductQtyID, model.UserID);
                List<RetProductWiseGSTReportList> objlst = new List<RetProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    RetProductWiseGSTReportList newobj = new RetProductWiseGSTReportList();
                    newobj.ProductQtyID = item.ProductQtyID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.Quantity = item.Quantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = model.CustomerID;
                    newobj.AreaID = model.AreaID;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, model.TaxID, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.Quantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.Quantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.IGST = item.IGST;
                    newobj.TaxAmtIGST = item.TaxAmtIGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<RetProductMainListByMonthGST> lst = new List<RetProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
                dt.Columns.Add("ProductName");
                dt.Columns.Add("HSNNumber");
                for (int i = 0; i < lst.Count; i++)
                {
                    dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
                }
                dt.Columns.Add("Total");
                dt.Columns.Add("TaxableAmount");
                dt.Columns.Add("IGSTPer");
                dt.Columns.Add("IGSTAmt");
                dt.Columns.Add("Amount");
                var result = (from item in objlst
                              select new
                              {
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  HSNNumber = item.HSNNumber,
                                  FinalTaxableAmount = Math.Round(item.FinalTaxableAmount, 2),
                                  IGST = item.IGST,
                                  TaxAmtIGST = item.TaxAmtIGST,
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();

                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
                    dr[1] = result.ToList()[i].ProductName;
                    dr[2] = result.ToList()[i].HSNNumber;
                    dr["TaxableAmount"] = result.ToList()[i].FinalTaxableAmount;
                    dr["IGSTPer"] = result.ToList()[i].IGST;
                    dr["IGSTAmt"] = result.ToList()[i].TaxAmtIGST;
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
                            if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 4] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                    for (int col = 4; col < dt.Columns.Count - 4; col++)
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
                    dt.Rows[t][lst.Count + 4] = sum;
                }

                DataRow drtot = dt.NewRow();
                for (int col = 4; col < dt.Columns.Count; col++)
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
                    if (col == 4)
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
            }
            dt.Columns.Remove("ProductQtyID");
            return View(dt);
        }

        public ActionResult ExportExcelProductWiseGSTReportList(DateTime? StartDate, DateTime? EndDate, string Tax, long TaxID, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductQtyID, long? UserID, string CustomerName)
        {
            if (CustomerID == null)
            {
                CustomerID = 0;
            }
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (ProductCategoryID == null)
            {
                ProductCategoryID = 0;
            }
            if (ProductQtyID == null)
            {
                ProductQtyID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            if (TaxID == null)
            {
                TaxID = 0;
            }
            DataTable dt = new DataTable("RetailProductWiseGSTReport");
            if (Tax == "SGST")
            {
                List<RetProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(StartDate, EndDate, Tax, TaxID, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);
                List<RetProductWiseGSTReportList> objlst = new List<RetProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    RetProductWiseGSTReportList newobj = new RetProductWiseGSTReportList();
                    newobj.ProductQtyID = item.ProductQtyID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.Quantity = item.Quantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = CustomerID.Value;
                    newobj.AreaID = AreaID.Value;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, TaxID, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.Quantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.Quantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.CGST = item.CGST;
                    newobj.TaxAmtCGST = item.TaxAmtCGST;
                    newobj.SGST = item.SGST;
                    newobj.TaxAmtSGST = item.TaxAmtSGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<RetProductMainListByMonthGST> lst = new List<RetProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
                dt.Columns.Add("ProductName");
                dt.Columns.Add("HSNNumber");
                for (int i = 0; i < lst.Count; i++)
                {
                    dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
                }
                dt.Columns.Add("Total");
                dt.Columns.Add("TaxableAmount");
                dt.Columns.Add("CGSTPer");
                dt.Columns.Add("CGSTAmt");
                dt.Columns.Add("SGSTPer");
                dt.Columns.Add("SGSTAmt");
                dt.Columns.Add("Amount");
                var result = (from item in objlst
                              select new
                              {
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  HSNNumber = item.HSNNumber,
                                  FinalTaxableAmount = Math.Round(item.FinalTaxableAmount, 2),
                                  CGST = item.CGST,
                                  TaxAmtCGST = item.TaxAmtCGST,
                                  SGST = item.SGST,
                                  TaxAmtSGST = item.TaxAmtSGST,
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();

                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
                    dr[1] = result.ToList()[i].ProductName;
                    dr[2] = result.ToList()[i].HSNNumber;
                    dr["TaxableAmount"] = result.ToList()[i].FinalTaxableAmount;
                    dr["CGSTPer"] = result.ToList()[i].CGST;
                    dr["CGSTAmt"] = result.ToList()[i].TaxAmtCGST;
                    dr["SGSTPer"] = result.ToList()[i].SGST;
                    dr["SGSTAmt"] = result.ToList()[i].TaxAmtSGST;
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
                            if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 4] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                    for (int col = 4; col < dt.Columns.Count - 6; col++)
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

                    dt.Rows[t][lst.Count + 4] = sum;
                }

                DataRow drtot = dt.NewRow();
                for (int col = 4; col < dt.Columns.Count; col++)
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
                    if (col == 4)
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
            }
            else
            {
                List<RetProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(StartDate, EndDate, Tax, TaxID, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductQtyID.Value, UserID.Value);
                List<RetProductWiseGSTReportList> objlst = new List<RetProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    RetProductWiseGSTReportList newobj = new RetProductWiseGSTReportList();
                    newobj.ProductQtyID = item.ProductQtyID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.Quantity = item.Quantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = CustomerID.Value;
                    newobj.AreaID = AreaID.Value;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetRetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, TaxID, newobj.ProductQtyID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.Quantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.Quantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.IGST = item.IGST;
                    newobj.TaxAmtIGST = item.TaxAmtIGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<RetProductMainListByMonthGST> lst = new List<RetProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new RetProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductQtyID");
                dt.Columns.Add("ProductName");
                dt.Columns.Add("HSNNumber");
                for (int i = 0; i < lst.Count; i++)
                {
                    dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
                }
                dt.Columns.Add("Total");
                dt.Columns.Add("TaxableAmount");
                dt.Columns.Add("IGSTPer");
                dt.Columns.Add("IGSTAmt");
                dt.Columns.Add("Amount");
                var result = (from item in objlst
                              select new
                              {
                                  ProductQtyID = item.ProductQtyID,
                                  ProductName = item.ProductName,
                                  HSNNumber = item.HSNNumber,
                                  FinalTaxableAmount = Math.Round(item.FinalTaxableAmount, 2),
                                  IGST = item.IGST,
                                  TaxAmtIGST = item.TaxAmtIGST,
                                  TotalAmount = Math.Round(item.TotalAmount, 2),
                              })
                  .ToList()
                  .Distinct();

                for (int i = 0; i < result.ToList().Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.ToList()[i].ProductQtyID;
                    dr[1] = result.ToList()[i].ProductName;
                    dr[2] = result.ToList()[i].HSNNumber;
                    dr["TaxableAmount"] = result.ToList()[i].FinalTaxableAmount;
                    dr["IGSTPer"] = result.ToList()[i].IGST;
                    dr["IGSTAmt"] = result.ToList()[i].TaxAmtIGST;
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
                            if (dt.Rows[k]["ProductQtyID"].ToString() == lst[i].ListMainProduct[j].ProductQtyID.ToString())
                            {
                                if (cnt <= dt.Rows.Count)
                                {
                                    dt.Rows[cnt - 1][0] = cnt;
                                }
                                dt.Rows[k][i + 4] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                    for (int col = 4; col < dt.Columns.Count - 4; col++)
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

                    dt.Rows[t][lst.Count + 4] = sum;
                }

                DataRow drtot = dt.NewRow();
                for (int col = 4; col < dt.Columns.Count; col++)
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
                    if (col == 4)
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
            }

            dt.Columns.Remove("ProductQtyID");
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            DataRow dRow = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dRow);
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "RetailProductWiseGSTReport.xls");
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

        // 03-04-2020 - barcode history
        public ActionResult BarcodeHistory()
        {
            ViewBag.Employee = _adminservice.GetAllEmployeeName();
            ViewBag.Product = _orderservice.GetAllRetProductName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewBarcodeHistoryList(BarcodeHistoryListResponse model)
        {
            List<BarcodeHistoryListResponse> objModel = _reportservice.GetBarcodeHistoryList(model);
            return PartialView(objModel);
        }


        // 12 Sep 2020 Piyush Limbani
        [HttpPost]
        public PartialViewResult VoucherCashCounterReportList(RetVoucherCashCounterListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            List<RetVoucherCashCounterListResponse> objfinallist = new List<RetVoucherCashCounterListResponse>();
            List<RetVoucherCashCounterListResponse> objlst = _reportservice.GetRetailExpenseVoucherCashCounterReportList(model.AssignedDate, model.GodownID);
            objfinallist.AddRange(objlst);
            if (objfinallist.Count > 0)
            {
                objfinallist[0].CashTotal = objlst[objlst.Count - 1].CashTotal;
                objfinallist[0].ChequeTotal = objlst[objlst.Count - 1].ChequeTotal;
                objfinallist[0].CardTotal = objlst[objlst.Count - 1].CardTotal;
                objfinallist[0].OnlineTotal = objlst[objlst.Count - 1].OnlineTotal;
                objfinallist[0].AdjustAmountTotal = objlst[objlst.Count - 1].AdjustAmountTotal;
            }
            else
            {
                objfinallist.Add(new RetVoucherCashCounterListResponse());
                objfinallist[0].CashTotal = 0;
                objfinallist[0].ChequeTotal = 0;
                objfinallist[0].CardTotal = 0;
                objfinallist[0].OnlineTotal = 0;
                objfinallist[0].AdjustAmountTotal = 0;
            }
            return PartialView(objfinallist);
        }

        [HttpPost]
        public ActionResult UpdateVoucherPayment(RetUpdateVoucherPayment data)
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
                    bool respose = _reportservice.UpdateVoucherPayment(data, UserID);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public PartialViewResult VoucherCashCounterDayWiseSalesManList(RetVoucherCashCounterDayWiseSalesManList model)
        {
            List<RetVoucherCashCounterDayWiseSalesManList> objlst = _reportservice.GetRetVoucherCashCounterDayWiseSalesManList(model.AssignedDate, model.GodownID);
            return PartialView(objlst);
        }



        // 12 Sep 2020 Piyush Limbani
        [HttpPost]
        public ActionResult PrintExpensesVoucher(long ExpensesVoucherID)
        {
            try
            {
                string NumberToWord = "";
                LocalReport lr = new LocalReport();
                string path = "";
                if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost"))
                {
                    path = "Report/ExpensesVoucher.rdlc";
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Report/ExpensesVoucher.rdlc");
                }
                lr.ReportPath = path;

                var ExpensesVoucherDetail = _IExpensesService.GetDataForExpensesVoucherPrint(ExpensesVoucherID);
                List<ExpensesVoucherListResponse> LabelData = new List<ExpensesVoucherListResponse>();
                ExpensesVoucherListResponse obj = new ExpensesVoucherListResponse();
                obj.DateofVoucher = ExpensesVoucherDetail.DateofVoucher;
                obj.GodownName = ExpensesVoucherDetail.GodownName;
                obj.VoucherNumber = ExpensesVoucherDetail.VoucherNumber;
                obj.Pay = ExpensesVoucherDetail.Pay;
                obj.RemarksL1 = ExpensesVoucherDetail.RemarksL1;
                obj.RemarksL2 = ExpensesVoucherDetail.RemarksL2;
                obj.RemarksL3 = ExpensesVoucherDetail.RemarksL3;
                obj.DebitAccountType = ExpensesVoucherDetail.DebitAccountType;
                obj.Amount = ExpensesVoucherDetail.Amount;
                int number = Convert.ToInt32(obj.Amount);
                NumberToWord = NumberToWords(number);
                obj.AmountInWords = NumberToWord + "  " + "Only/-";
                obj.PreparedBy = ExpensesVoucherDetail.PreparedBy;
                if (ExpensesVoucherDetail.BillNumber != "" && ExpensesVoucherDetail.BillNumber != null)
                {
                    obj.BillNumber = "(" + " " + ExpensesVoucherDetail.BillNumber + " " + ")";
                }
                else
                {
                    obj.BillNumber = "";
                }
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
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>7.5in</PageHeight>" +
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
                string Pdfpathcreate = Server.MapPath("~/ExpensesVoucher/" + name);
                BinaryWriter Writer = null;
                Writer = new BinaryWriter(System.IO.File.OpenWrite(Pdfpathcreate));
                Writer.Write(renderedBytes);
                Writer.Flush();
                Writer.Close();

                string url = "";
                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/ExpensesVoucher/" + name;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/ExpensesVoucher/" + name;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/ExpensesVoucher/" + name;
                }
                return Json(url, JsonRequestBehavior.AllowGet);
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

        // 12 Sep 2020 Piyush Limbani

        // Add By Dhruvik 
        [HttpPost]
        public PartialViewResult ChequeRetrunList(RetCashCounterListResponse model)
        {
            List<RetCashCounterListResponse> objlst = _reportservice.GetRetChequeRetrunList(model.AssignedDate, model.GodownID);
            return PartialView(objlst);
        }
        // Add By Dhruvik

    }
}