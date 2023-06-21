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

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class ReportController : Controller
    {
        private IReportService _reportservice;
        private ICommonService _commonservice;
        private IOrderService _orderservice;
        private IProductService _productservice;
        private IAdminService _adminservice;
        //private ICustomerService _customerservice;

        public ReportController(IReportService reportservice, ICommonService commonservice, IOrderService orderservice, IProductService productservice, IAdminService adminservice)//, ICustomerService customerservice)
        {
            _reportservice = reportservice;
            _commonservice = commonservice;
            _orderservice = orderservice;
            _productservice = productservice;
            _adminservice = adminservice;
            //_customerservice = customerservice;
        }

        //
        // GET: /wholesale/Report/
        public ActionResult Daywisesales()
        {
            //Session["UserID"] = "1";
            return View();
        }

        public ActionResult ProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductID, long? UserID)
        {
            Session["fprcustid"] = CustomerID;
            Session["fpruid"] = UserID;
            Session["fprtxtfrom"] = StartDate;
            Session["fprtxtto"] = EndDate;
            Session["fprproductid"] = ProductID;
            Session["fprcategoryid"] = ProductCategoryID;
            Session["fprareaid"] = AreaID;

            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            ViewBag.Product = _orderservice.GetAllProductName();
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            ViewBag.ProductCategoryList = _productservice.GetAllProductCategoryList();
            return View();
        }

        public ActionResult ProductWiseSalesList(ProductWiseSalesList model)
        {
            List<ProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductID, model.UserID);

            // new 30-04-2019
            List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }

                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 30-04-2019

            List<ProductMainListByMonth> lst = new List<ProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
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
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
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
                            //dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
                            dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                for (int col = 3; col < dt.Columns.Count - 1; col++)
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

                dt.Rows[t][lst.Count + 3] = sum;
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

        public ActionResult ProductWiseDailySalesList(ProductWiseSalesList model)
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

            List<ProductWiseSalesList> objlst1 = _reportservice.GetProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductID, model.UserID);

            // new 30-04-2019
            List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
            decimal TotalReturnOrderAmount = 0;
            decimal OrderTotalAmount = 0;
            foreach (var item in objlst1)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;

                decimal ReturnedQuantity = _reportservice.GetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                OrderTotalAmount = item.OrderTotalAmount;
                decimal ReturnOrderAmount = _reportservice.GetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    TotalReturnOrderAmount += ReturnOrderAmount;
                }

                //if (ReturnOrderAmount != 0)
                //{
                //    TotalAmount = item.OrderTotalAmount - ReturnOrderAmount;
                //}
                //else
                //{
                //    TotalAmount = item.OrderTotalAmount;
                //}
                // newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 30-04-2019          

            List<ProductMainListByDay> lst = new List<ProductMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new ProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add(lst[i].DayName + "-" + (lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              //TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();

            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
                dr["Amount"] = Math.Round((OrderTotalAmount - TotalReturnOrderAmount), 2);
                //dr["Amount"] = result.ToList()[i].TotalAmount;
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
                            //dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
                            dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                for (int col = 3; col < dt.Columns.Count - 1; col++)
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

                dt.Rows[t][lst.Count + 3] = sum;
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

            //dt.Columns.Remove("ProductID");
            return View(dt);
        }

        public ActionResult ExportExcelProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductID, long? UserID, string CustomerName)
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
            if (ProductID == null)
            {
                ProductID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            List<ProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);

            // new 30-04-2019
            List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }

                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // new 30-04-2019

            List<ProductMainListByMonth> lst = new List<ProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
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
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            //lst = lst.OrderByDescending(x => x.ListMainProduct.Count).ToList();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
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
                            //dt.Rows[k][i + 3] = lst[i].ListMainProduct[j].Quantity;
                            dt.Rows[k][i + 3] = Math.Round(lst[i].ListMainProduct[j].Quantity, 3);
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
                for (int col = 3; col < dt.Columns.Count - 1; col++)
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
                dt.Rows[t][lst.Count + 3] = Math.Round(sum, 3);
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

            List<ProductWiseSalesList> objlst3 = _reportservice.GetProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);

            // new 30-04-2019
            List<ProductWiseSalesList> objlst2 = new List<ProductWiseSalesList>();
            decimal TotalReturnOrderAmount = 0;
            decimal OrderTotalAmount = 0;
            foreach (var item in objlst3)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;

                decimal ReturnedQuantity = _reportservice.GetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                OrderTotalAmount = item.OrderTotalAmount;
                decimal ReturnOrderAmount = _reportservice.GetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    TotalReturnOrderAmount += ReturnOrderAmount;
                }

                //if (ReturnOrderAmount != 0)
                //{
                //    TotalAmount = item.OrderTotalAmount - ReturnOrderAmount;
                //}
                //else
                //{
                //    TotalAmount = item.OrderTotalAmount;
                //}
                // newobj.TotalAmount = item.TotalAmount;
                objlst2.Add(newobj);
            }
            // new 30-04-2019          

            List<ProductMainListByDay> lst2 = new List<ProductMainListByDay>();

            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new ProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
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
            dt2.Columns.Add("Amount");
            var result2 = (from item in objlst2
                           select new
                           {
                               ProductID = item.ProductID,
                               ProductName = item.ProductName,
                               TotalAmount = Math.Round(item.TotalAmount, 2),
                           })
              .ToList()
              .Distinct();
            //lst = lst.OrderByDescending(x => x.ListMainProduct.Count).ToList();

            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].ProductID;
                dr[1] = result2.ToList()[i].ProductName;
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
                        if (dt2.Rows[k]["ProductID"].ToString() == lst2[i].ListMainProduct[j].ProductID.ToString())
                        {
                            if (cnt2 <= dt2.Rows.Count)
                            {
                                dt2.Rows[cnt2 - 1][0] = cnt2;
                            }
                            //dt2.Rows[k][i + 3] = lst2[i].ListMainProduct[j].Quantity;
                            dt2.Rows[k][i + 3] = Math.Round(lst2[i].ListMainProduct[j].Quantity, 3);

                            // old
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
                for (int col = 3; col < dt2.Columns.Count - 1; col++)
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
                dt2.Rows[t][lst2.Count + 3] = Math.Round(sum, 3);
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

            //if (CustomerID != 0)
            //{                
            //    dt.Columns.Add("CustomerName").SetOrdinal(1);
            //    dt2.Columns.Add("CustomerName").SetOrdinal(1);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dt.Rows[i][1] = CustomerName;
            //    }
            //    for (int i = 0; i < dt2.Rows.Count; i++)
            //    {
            //        dt2.Rows[i][1] = CustomerName;
            //    }
            //}

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
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            ViewBag.Product = _orderservice.GetAllProductName();
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            ViewBag.ProductCategoryList = _productservice.GetAllProductCategoryList();
            return View();
        }

        public ActionResult FestivalProductWiseSalesList(FestivalProductWiseSalesList model)
        {
            model.StartDate = model.StartDate.AddDays(-model.BeforeDays);
            model.EndDate = model.EndDate.AddDays(model.AfterDays);
            List<ProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductID, model.UserID);

            // 28-06-2018
            List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }

                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // 28-06-2018

            List<ProductMainListByMonth> lst = new List<ProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
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
                for (int col = 3; col < dt.Columns.Count - 1; col++)
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
                dt.Rows[t][lst.Count + 3] = sum;
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

        public ActionResult FestivalProductWiseDailySalesList(FestivalProductWiseSalesList model)
        {
            Session["fprcustid"] = "";
            Session["fpruid"] = "";
            Session["fprtxtfrom"] = "";
            Session["fprtxtto"] = "";
            Session["fprproductid"] = "";
            Session["fprcategoryid"] = "";
            Session["fprareaid"] = "";
            model.StartDate = model.StartDate.AddDays(-model.BeforeDays);
            model.EndDate = model.EndDate.AddDays(model.AfterDays);
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
            List<ProductWiseSalesList> objlst1 = _reportservice.GetProductWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductID, model.UserID);

            // new 28-06-2019
            List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
            decimal TotalReturnOrderAmount = 0;
            decimal OrderTotalAmount = 0;
            foreach (var item in objlst1)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = model.CustomerID;
                newobj.AreaID = model.AreaID;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;

                decimal ReturnedQuantity = _reportservice.GetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                OrderTotalAmount = item.OrderTotalAmount;
                decimal ReturnOrderAmount = _reportservice.GetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    TotalReturnOrderAmount += ReturnOrderAmount;
                }
                objlst.Add(newobj);
            }
            // new 28-06-2019  

            List<ProductMainListByDay> lst = new List<ProductMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new ProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add(lst[i].DayName + "-" + (lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              //TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
                dr["Amount"] = Math.Round((OrderTotalAmount - TotalReturnOrderAmount), 2);
                // dr["Amount"] = result.ToList()[i].TotalAmount;
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
                for (int col = 3; col < dt.Columns.Count - 1; col++)
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
                dt.Rows[t][lst.Count + 3] = sum;
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
        public PartialViewResult ViewBillWiseOrderListForFestivalProductWiseSalesReport(OrderListResponse model)
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
            List<OrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelFestivalProductWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductID, long? UserID, long? BeforeDays, long? AfterDays)
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
            if (ProductID == null)
            {
                ProductID = 0;
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
            List<ProductWiseSalesList> objlst1 = _reportservice.GetProductWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);

            // 28-06-2018
            List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
            foreach (var item in objlst1)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = item.ProductCategoryID;
                newobj.UserID = item.UserID;
                decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWise(newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                newobj.TotalAmount = item.TotalAmount;
                objlst.Add(newobj);
            }
            // 28-06-2018

            List<ProductMainListByMonth> lst = new List<ProductMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable("FestivalWiseProductSales");
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            for (int i = 0; i < lst.Count; i++)
            {
                dt.Columns.Add((lst[i].MonthName) + "-" + lst[i].YearName);
            }
            dt.Columns.Add("Total");
            dt.Columns.Add("Amount");
            var result = (from item in objlst
                          select new
                          {
                              ProductID = item.ProductID,
                              ProductName = item.ProductName,
                              TotalAmount = Math.Round(item.TotalAmount, 2),
                          })
              .ToList()
              .Distinct();
            for (int i = 0; i < result.ToList().Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = result.ToList()[i].ProductID;
                dr[1] = result.ToList()[i].ProductName;
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
                for (int col = 3; col < dt.Columns.Count - 1; col++)
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

            List<ProductWiseSalesList> objlst3 = _reportservice.GetProductWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);

            // new 28-06-2019
            List<ProductWiseSalesList> objlst2 = new List<ProductWiseSalesList>();
            decimal TotalReturnOrderAmount = 0;
            decimal OrderTotalAmount = 0;
            foreach (var item in objlst3)
            {
                ProductWiseSalesList newobj = new ProductWiseSalesList();
                newobj.ProductID = item.ProductID;
                newobj.ProductName = item.ProductName;
                newobj.DayName = item.DayName;
                newobj.MonthName = item.MonthName;
                newobj.YearName = item.YearName;
                newobj.OrderQuantity = item.OrderQuantity;
                newobj.OrderTotalAmount = item.OrderTotalAmount;
                newobj.CustomerID = CustomerID.Value;
                newobj.AreaID = AreaID.Value;
                newobj.ProductCategoryID = ProductCategoryID.Value;
                newobj.UserID = UserID.Value;
                decimal ReturnedQuantity = _reportservice.GetReturnQuantityDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnedQuantity != 0)
                {
                    newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                }
                else
                {
                    newobj.Quantity = newobj.OrderQuantity;
                }
                OrderTotalAmount = item.OrderTotalAmount;
                decimal ReturnOrderAmount = _reportservice.GetReturnOrderAmountDayWise(newobj.DayName, newobj.MonthName, newobj.YearName, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                if (ReturnOrderAmount != 0)
                {
                    TotalReturnOrderAmount += ReturnOrderAmount;
                }
                objlst2.Add(newobj);
            }
            // new 28-06-2019      

            List<ProductMainListByDay> lst2 = new List<ProductMainListByDay>();
            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new ProductMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("ProductWiseDailySalesList");
            dt2.Columns.Add("ProductID");
            dt2.Columns.Add("ProductName");
            for (int i = 0; i < lst2.Count; i++)
            {
                dt2.Columns.Add(lst2[i].DayName + "-" + (lst2[i].MonthName) + "-" + lst2[i].YearName);
            }
            dt2.Columns.Add("Total");
            dt2.Columns.Add("Amount");
            var result2 = (from item in objlst2
                           select new
                           {
                               ProductID = item.ProductID,
                               ProductName = item.ProductName,
                               TotalAmount = Math.Round(item.TotalAmount, 2),
                           })
              .ToList()
              .Distinct();
            for (int i = 0; i < result2.ToList().Count; i++)
            {
                DataRow dr = dt2.NewRow();
                dr[0] = result2.ToList()[i].ProductID;
                dr[1] = result2.ToList()[i].ProductName;
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
                for (int col = 3; col < dt2.Columns.Count - 1; col++)
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

        public ActionResult SalesManWiseSales()
        {
            ViewBag.SalesPersonList = _commonservice.GetRoleWiseUserList();
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            ViewBag.AreaList = _commonservice.GetAllAreaList();
            return View();
        }

        public ActionResult SalesManWiseSalesList(SalesManWiseSalesList model)
        {
            List<SalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseSalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);
            List<SalesMainListByMonth> lst = new List<SalesMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new SalesMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
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
                if (col == 6) // col = 5
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

        public ActionResult SalesManWiseDailySalesList(SalesManWiseSalesList model)
        {
            Session["srcustid"] = "";
            Session["sruid"] = "";
            Session["srtxtfrom"] = "";
            Session["srtxtto"] = "";
            Session["srareaid"] = "";
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
            List<SalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseDailySalesList(model.StartDate, model.EndDate, model.CustomerID, model.AreaID, model.UserID, model.DaysofWeek);
            List<SalesMainListByDay> lst = new List<SalesMainListByDay>();
            lst = objlst.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new SalesMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
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
        public PartialViewResult ViewBillWiseOrderListForSalesManWiseSalesReport(OrderListResponse model)
        {
            Session["srcustid"] = "";
            Session["sruid"] = "";
            Session["srtxtfrom"] = "";
            Session["srtxtto"] = "";
            Session["srareaid"] = "";

            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<OrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForSalesManWiseSalesReport(string invid, Int64? custid, Int64? uid, string txtfrom, string txtto, string areaid)
        {
            //Session["UserID"] = "1";
            Session["srcustid"] = custid;
            Session["sruid"] = uid;
            Session["srtxtfrom"] = txtfrom;
            Session["srtxtto"] = txtto;
            Session["srareaid"] = areaid;

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

        public ActionResult ExportExcelSalesManWiseSales(DateTime? StartDate, DateTime? EndDate, long? CustomerID, long? AreaID, long? UserID, long? DaysofWeek)
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
            List<SalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseSalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            List<SalesMainListByMonth> lst = new List<SalesMainListByMonth>();
            lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new SalesMainListByMonth() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
            DataTable dt = new DataTable("SalesWiseMonthlySales");
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
                dt.Rows[t][lst.Count + 6] = sum; // [lst.Count + 5]
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

            List<SalesManWiseSalesList> objlst2 = _reportservice.GetSalesManWiseDailySalesList(StartDate.Value, EndDate.Value, CustomerID.Value, AreaID.Value, UserID.Value, DaysofWeek.Value);
            List<SalesMainListByDay> lst2 = new List<SalesMainListByDay>();
            lst2 = objlst2.GroupBy(x => new { x.DayName, x.MonthName, x.YearName }).Select(x => new SalesMainListByDay() { DayName = x.Key.DayName, MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
            //lst2 = lst2.OrderBy(x => x.DayName).ThenBy(y => y.MonthName).ThenBy(z => z.YearName).ToList();
            lst2 = lst2.OrderBy(x => Convert.ToDateTime(x.YearName + "-" + x.MonthName + "-" + x.DayName)).ToList();
            DataTable dt2 = new DataTable("SalesWiseDailySalesList");
            dt2.Columns.Add("CustomerID");
            dt2.Columns.Add("CustomerName");
            dt2.Columns.Add("Area");
            dt2.Columns.Add("UserName");
            //int cnt = 0;
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
            return View();
        }


        [HttpPost]
        public PartialViewResult ViewBillWiseOrderListForProductWiseSalesReport(OrderListResponse model)
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


            List<OrderListResponse> objModel = _orderservice.GetAllBillWiseOrderList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ViewBillWiseInvoiceForProductWiseSalesReport(string invid, Int64? custid, Int64? uid, string txtfrom, string txtto, string productid, string categoryid, string areaid)
        {
            //Session["UserID"] = "1";
            Session["prcustid"] = custid;
            Session["pruid"] = uid;
            Session["prtxtfrom"] = txtfrom;
            Session["prtxtto"] = txtto;
            Session["prproductid"] = productid;
            Session["prcategoryid"] = categoryid;
            Session["prareaid"] = areaid;

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

        public ActionResult DeliverySheet()
        {
            //Session["UserID"] = "1";
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            ViewBag.VehicleNo = _commonservice.GetVehicleNoList();
            return View();
        }

        [HttpPost]
        public PartialViewResult DeliverySheetList(DeliverySheetList model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            decimal TotalCash = 0;
            decimal TotalCheque = 0;
            decimal TotalCard = 0;
            decimal TotalSign = 0;
            decimal TotalOnline = 0;
            decimal TotalAdjustAmount = 0;
            List<DeliverySheetList> objlst = null;
            List<DeliverySheetList> objlistoftempo = new List<DeliverySheetList>();
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
            decimal TotalCash = 0;
            decimal TotalCheque = 0;
            decimal TotalCard = 0;
            decimal TotalSign = 0;
            decimal TotalOnline = 0;
            decimal TotalAdjustAmount = 0;
            int BySign = 0;
            if (IsCheckBySign == false)
            {
                BySign = 0;
            }
            else
            {
                BySign = 1;
            }
            List<DeliverySheetList> finallist = new List<DeliverySheetList>();
            string[] vehicle = VehicleNo[0].Split(',');
            foreach (var item in vehicle)
            {
                List<DeliverySheetList> dataInventory = _reportservice.GetDeliverySheetList(Convert.ToDateTime(Date), item, TempoNo, GodownID, BySign);
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
            List<DeliverySheetListForExport> lstdelsheet = finallist.Select(x => new DeliverySheetListForExport() { Customer = x.Customer, Area = x.Area, VehicleNo = x.VehicleNo1, InvoiceNumber = x.InvoiceNumber, Cash = x.Cash, Cheque = x.Cheque, Card = x.Card, Sign = x.Sign, Online = x.Online, AdjustAmount = x.AdjustAmount, Remarks = x.Remarks, Godown = x.GodownName, BankName = x.BankName, BankBranch = x.BankBranch, ChequeNo = x.ChequeNo, ChequeDate = x.ChequeDate1, IFCCode = x.IFCCode, BankNameForCard = x.BankNameForCard, TypeOfCard = x.TypeOfCard, OnlinePaymentDate = x.OnlinePaymentDate1, BankNameForOnline = x.BankNameForOnline, UTRNumber = x.UTRNumber }).ToList();
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

        [HttpPost]
        public PartialViewResult DayWiseSalesList(DayWiseSalesList model)
        {
            //List<DeliveryStatusListResponse> objModel = _deliveryservice.GetDeliveryStatusList(Convert.ToInt64(para[0]), Convert.ToDateTime(para[1]));
            List<DayWiseSalesList> objlst = _reportservice.GetDayWiseSalesList(model.InvDate);
            for (int i = 0; i < objlst.Count; i++)
            {
                if (objlst[i].IsDelete == true && objlst[i].IsApprove == true)
                {
                    objlst[i].Party = "Cancelled";
                    objlst[i].Area = "";
                    objlst[i].GrossAmt = 0;
                    objlst[i].Discount = 0;
                    objlst[i].CGST = 0;
                    objlst[i].TaxAmtCGST = "0.00";
                    objlst[i].SGST = 0;
                    objlst[i].TaxAmtSGST = "0.00";
                    objlst[i].IGST = 0;
                    objlst[i].TaxAmtIGST = "0.00";
                    objlst[i].RoundOff = 0;
                    objlst[i].NetAmount = 0;
                    objlst[i].UserFullName = "";
                    objlst[i].diff = 0;
                    objlst[i].CreditedTotal = 0;
                }
            }

            return PartialView(objlst);
        }

        [HttpPost]
        public PartialViewResult DayWiseCreditMemoList(DayWiseCreditMemoList model)
        {
            //List<DeliveryStatusListResponse> objModel = _deliveryservice.GetDeliveryStatusList(Convert.ToInt64(para[0]), Convert.ToDateTime(para[1]));
            List<DayWiseCreditMemoList> objlst = _reportservice.GetDayWiseCreditMemoList(model.InvDate);

            return PartialView(objlst);
        }

        [HttpPost]
        public PartialViewResult DayWiseTaxList(DayWiseTaxList model)
        {
            //List<DeliveryStatusListResponse> objModel = _deliveryservice.GetDeliveryStatusList(Convert.ToInt64(para[0]), Convert.ToDateTime(para[1]));
            List<DayWiseTaxList> objlst = _reportservice.GetDayWiseTaxList(model.InvDate);

            return PartialView(objlst);
        }

        [HttpPost]
        public PartialViewResult DayWiseSalesManList(DayWiseSalesManList model)
        {
            //List<DeliveryStatusListResponse> objModel = _deliveryservice.GetDeliveryStatusList(Convert.ToInt64(para[0]), Convert.ToDateTime(para[1]));
            List<DayWiseSalesManList> objlst = _reportservice.GetDayWiseSalesManList(model.InvDate);

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
            List<DayWiseSalesList> dataInventory = _reportservice.GetDayWiseSalesList(Convert.ToDateTime(date));
            for (int i = 0; i < dataInventory.Count; i++)
            {
                if (dataInventory[i].IsDelete == true)
                {
                    dataInventory[i].Party = "Cancelled";
                    dataInventory[i].Area = "";
                    dataInventory[i].GrossAmt = 0;
                    dataInventory[i].Discount = 0;
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
                    dataInventory[i].diff = 0;
                    dataInventory[i].CreditedTotal = 0;
                }
            }
            List<DayWiseSalesListForExp> lstdaywisesales = dataInventory.Select(x => new DayWiseSalesListForExp()
                {
                    InvoiceNumber = x.InvCode,
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
                    SalesPerson = x.UserFullName,
                    Diff = x.diff,
                    InvoiceDate = x.InvoiceDate,
                    CustomerCode = x.CustomerNumber,
                    CreditedTotal = x.CreditedTotal
                }).ToList();
            List<DayWiseCreditMemoListForExp> lstdaywisecreditmemo = dataInventory2.Select(x => new DayWiseCreditMemoListForExp()
                {
                    CreditMemoNumber = x.CreditMemo,
                    CreditMemoDate = CreditMemoDate,
                    InvoiceNumber = x.Invoice,
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
            List<DayWiseSalesManListForExp> lstdaywisesalesman = dataInventory4.Select(x => new DayWiseSalesManListForExp()
                {
                    SalesPerson = x.SalesPerson,
                    GrossAmt = x.GrossAmtTotal,
                    TaxAmt = x.TaxAmtTotal,
                    RoundOff = x.RoundOffTotal,
                    NetAmt = x.NetAmtTotal,
                    Difference = x.DifferenceTotal
                }).ToList();
            List<DayWiseTaxListForExp> lstdaywisetax = dataInventory3.Select(x => new DayWiseTaxListForExp()
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "DayWiseSummary.xls");
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
            List<CustomerListResponse> objModel = _reportservice.GetAllCustomerMasterList();
            return View(objModel);
        }

        public ActionResult ExportExcelCustomerList()
        {
            var dataInventory = _reportservice.GetAllCustomerMasterList();
            List<CustomerListForExp> lstcustomers = dataInventory.Select(x => new CustomerListForExp() { Party = x.CustomerName, Code = x.CustomerNumber, DeliveryAddressLine1 = x.DeliveryAddressLine1, DeliveryAddressLine2 = x.DeliveryAddressLine2, AreaName = x.AreaName, State = x.State, Country = x.Country, TaxNo = x.TaxNo, FSSAI = x.FSSAI, TaxName = x.TaxName, CustomerTypeName = x.CustomerTypeName, ContactName = x.ContactName, ContactNumber = x.ContactNumber, ContactEmail = x.ContactEmail, SalesPerson = x.UserFullName }).ToList();

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
                Event = _reportservice.GetEventDateForSameYear(EventID, StartYear);
            }
            return Json(Event, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEventDateForDiffYear(long EventID, long StartYear, long EndYear)
        {
            GetEvent Event = new GetEvent();
            if (EventID != 0 && StartYear != 0 && EndYear != 0)
            {
                Event = _reportservice.GetEventDateForDiffYear(EventID, StartYear, EndYear);
            }
            return Json(Event, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PouchWiseReport()
        {
            ViewBag.Pouch = _productservice.GetAllPouchName();
            return View();
        }

        [HttpPost]
        public PartialViewResult PouchWiseReportList(WholesalePouchListResponse model)
        {
            decimal TotalPouch = 0;

            List<WholesalePouchListResponse> objModel = null;
            List<WholesalePouchListResponse> pouchtotal = new List<WholesalePouchListResponse>();

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

            List<WholesalePouchListResponse> pouchtotal = new List<WholesalePouchListResponse>();
            string[] Pouch = PouchNameID[0].Split(',');

            foreach (var item in Pouch)
            {
                List<WholesalePouchListResponse> objModel = _reportservice.GetPouchWiseReportList(StartDate.Value, EndDate.Value, item);

                if (objModel != null)
                {
                    foreach (var record in objModel)
                    {
                        pouchtotal.Add(record);
                        TotalPouch += record.Quantity;
                    }
                }
            }
            List<WholesalePouchListForExport> lstdelsheet = pouchtotal.Select(x => new WholesalePouchListForExport() { Pouch = x.PouchName, ProductName = x.ProductName, Quantity = x.Quantity, Unit = x.Unit }).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstdelsheet));

            DataRow dRow = ds.Tables[0].NewRow();
            dRow["ProductName"] = "Total";
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
            //ViewBag.VehicleNo = _commonservice.GetVehicleNoList();
            ViewBag.CashOption = _commonservice.GetAllCashOption();

            return View();
        }

        [HttpPost]
        public PartialViewResult CashCounterReportList(CashCounterListResponse model)
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
            List<CashCounterListResponse> objlistoftempo = new List<CashCounterListResponse>();
            List<CashCounterListResponse> objlst = _reportservice.GetCashCounterReportList(model.AssignedDate, model.GodownID, BySign);
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
                objlistoftempo.Add(new CashCounterListResponse());
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
            List<CashCounterListResponse> finallist = new List<CashCounterListResponse>();

            List<CashCounterListResponse> dataInventory = _reportservice.GetCashCounterReportList(Convert.ToDateTime(Date), GodownID, BySign);
            finallist.AddRange(dataInventory);

            //Add By Dhruvik
            List<CashCounterListResponse> dataInventory1 = _reportservice.GetCheckRetrunEntryList(Convert.ToDateTime(Date), GodownID);
            finallist.AddRange(dataInventory1);

            List<CashCounterListResponse> dataInventory2 = _reportservice.GetRetrunChargeList(Convert.ToDateTime(Date), GodownID);
            finallist.AddRange(dataInventory2);
            //Add By Dhruvik

            
            List<CashCounterListForExport> lstdelsheet = finallist.Select(x => new CashCounterListForExport()
            {
                Customer = x.Customer,
                Area = x.Area,
                VehicleNo = x.VehicleNo1,
                InvoiceNumber = x.InvoiceNumber,
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
            List<DayWiseSalesManListForExport> lstDayWiseSalesExp = lstDayWiseSales.Select(x => new DayWiseSalesManListForExport()
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
            List<VoucherCashCounterListResponse> finalvouchercashlist = new List<VoucherCashCounterListResponse>();
            List<VoucherCashCounterListResponse> datavouchercash = _reportservice.GetWholesaleExpenseVoucherCashCounterReportList(Convert.ToDateTime(Date), GodownID);
            finalvouchercashlist.AddRange(datavouchercash);
            List<VoucherCashCounterExp> lstvouchercash = finalvouchercashlist.Select(x => new VoucherCashCounterExp()
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

            var lstvouchercashsalesmanwise = _reportservice.GetVoucherCashCounterDayWiseSalesManList(Convert.ToDateTime(Date), GodownID);
            List<VoucherDayWiseSalesManListExp> lstVoucherCashDayWiseSalesExp = lstvouchercashsalesmanwise.Select(x => new VoucherDayWiseSalesManListExp()
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
                    dRow["Cash"] = dataInventory1[dataInventory1.Count - 1].CashTotal;
                    dRow["Cheque"] = dataInventory1[dataInventory1.Count - 1].ChequeTotal;
                    dRow["Card"] = dataInventory1[dataInventory1.Count - 1].CardTotal;
                    dRow["Sign"] = dataInventory1[dataInventory1.Count - 1].SignTotal;
                    dRow["Online"] = dataInventory1[dataInventory1.Count - 1].OnlineTotal;
                    dRow["AdjustAmount"] = dataInventory1[dataInventory1.Count - 1].AdjustAmountTotal;
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
            ViewBag.Customer = _commonservice.GetAllCustomerName();
            return View();
        }

        [HttpPost]
        public PartialViewResult BillHistoryList(BillHistoryListResponse model)
        {
            List<BillHistoryListResponse> objModel = _reportservice.GetBillHistoryList(model.InvoiceNumber, model.FromDate, model.ToDate, model.CustomerID);
            return PartialView(objModel);
        }

        [HttpPost]
        public PartialViewResult CashCounterDayWiseSalesManList(CashCounterDayWiseSalesManList model)
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
            List<CashCounterDayWiseSalesManList> objlst = _reportservice.GetCashCounterDayWiseSalesManList(model.AssignedDate, model.GodownID, BySign);
            return PartialView(objlst);
        }

        [HttpPost]
        public ActionResult UpdatePayment(Payment data)
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
            }
        }

        [HttpGet]
        public ActionResult SalesManWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            List<SalesManWiseSalesList> objlist2 = new List<SalesManWiseSalesList>();

            if (UserID > 0)
            {
                string CustomerIDs = _reportservice.GetCustomerIDForSalesList2(StartDate, EndDate, CustomerID, AreaID, UserID, DaysofWeek);
                if (CustomerIDs != "")
                {
                    List<SalesManWiseSalesList> objlst = _reportservice.GetSalesManWiseSalesList2(CustomerIDs, UserID, AreaID, DaysofWeek);
                    return View(objlst);
                }
                else
                {
                    //SalesManWiseSalesList obj22 = new SalesManWiseSalesList();
                    //obj22.SrNo = "";
                    //obj22.CustomerName = "";
                    //obj22.AreaName = "";
                    //obj22.UserName = "";
                    //objlist2.Add(obj22);
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

            string CustomerIDs = _reportservice.GetCustomerIDForSalesList2(StartDate, EndDate, CustomerID, AreaID, UserID, DaysofWeek);
            if (CustomerIDs != "")
            {
                var CustomerList = _reportservice.GetSalesManWiseSalesList2(CustomerIDs, UserID, AreaID, DaysofWeek);
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
                    Response.AddHeader("content-disposition", "attachment;filename= " + DateTime.Now.ToString("dd/MM/yyyy") + " " + "WholesaleCustomerListForZeroSales.xls");
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
        public ActionResult ProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {
            List<ProductWiseSalesList> objlist2 = new List<ProductWiseSalesList>();

            if (CustomerID > 0)
            {
                string ProductIDs = _reportservice.GetProductIDForProductWiseSalesList2(StartDate, EndDate, CustomerID, AreaID, ProductCategoryID, ProductID, UserID);
                if (ProductIDs != "")
                {
                    DateTime EndDate1 = DateTime.Now;
                    DateTime StartDate1 = EndDate1.AddYears(-1);
                    List<ProductWiseSalesList> objlst = _reportservice.GetProductWiseSalesList2(StartDate1, EndDate1, ProductIDs, CustomerID, AreaID, ProductCategoryID, UserID);
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

        public ActionResult ExportExcelZeroProductReport(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {

            string ProductIDs = _reportservice.GetProductIDForProductWiseSalesList2(StartDate, EndDate, CustomerID, AreaID, ProductCategoryID, ProductID, UserID);
            if (ProductIDs != "")
            {
                DateTime EndDate1 = DateTime.Now;
                DateTime StartDate1 = EndDate1.AddYears(-1);
                var ProductList = _reportservice.GetProductWiseSalesList2(StartDate1, EndDate1, ProductIDs, CustomerID, AreaID, ProductCategoryID, UserID);

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







        // GST Report
        public ActionResult ProductWiseGSTReport(DateTime? StartDate, DateTime? EndDate, string Tax, long? TaxID, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductID, long? UserID)
        {
            Session["fprcustid"] = CustomerID;
            Session["fpruid"] = UserID;
            Session["fprtxtfrom"] = StartDate;
            Session["fprtxtto"] = EndDate;
            Session["fprproductid"] = ProductID;
            Session["fprcategoryid"] = ProductCategoryID;
            Session["fprareaid"] = AreaID;
            return View();
        }

        public ActionResult ProductWiseGSTReportList(ProductWiseGSTReportList model)
        {
            ViewBag.Tax = model.Tax;
            DataTable dt = new DataTable();
            if (model.Tax == "SGST")
            {
                List<ProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(model.StartDate, model.EndDate, model.Tax, model.TaxID, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductID, model.UserID);
                List<ProductWiseGSTReportList> objlst = new List<ProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    ProductWiseGSTReportList newobj = new ProductWiseGSTReportList();
                    newobj.ProductID = item.ProductID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.OrderQuantity = item.OrderQuantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = model.CustomerID;
                    newobj.AreaID = model.AreaID;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, model.TaxID, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.OrderQuantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.CGST = item.CGST;
                    newobj.TaxAmtCGST = item.TaxAmtCGST;
                    newobj.SGST = item.SGST;
                    newobj.TaxAmtSGST = item.TaxAmtSGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<ProductMainListByMonthGST> lst = new List<ProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductID");
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
                                  ProductID = item.ProductID,
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
                    dr[0] = result.ToList()[i].ProductID;
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
                            if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
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
                List<ProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(model.StartDate, model.EndDate, model.Tax, model.TaxID, model.CustomerID, model.AreaID, model.ProductCategoryID, model.ProductID, model.UserID);
                List<ProductWiseGSTReportList> objlst = new List<ProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    ProductWiseGSTReportList newobj = new ProductWiseGSTReportList();
                    newobj.ProductID = item.ProductID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.OrderQuantity = item.OrderQuantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = model.CustomerID;
                    newobj.AreaID = model.AreaID;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, model.TaxID, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.OrderQuantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.IGST = item.IGST;
                    newobj.TaxAmtIGST = item.TaxAmtIGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<ProductMainListByMonthGST> lst = new List<ProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductID");
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
                                  ProductID = item.ProductID,
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
                    dr[0] = result.ToList()[i].ProductID;
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
                            if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
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
            return View(dt);
        }

        public ActionResult ExportExcelProductWiseGSTReportList(DateTime? StartDate, DateTime? EndDate, string Tax, long TaxID, long? CustomerID, long? AreaID, long? ProductCategoryID, long? ProductID, long? UserID, string CustomerName)
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
            if (ProductID == null)
            {
                ProductID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            if (TaxID == null)
            {
                TaxID = 0;
            }
            DataTable dt = new DataTable("ProductWiseGSTReport");
            if (Tax == "SGST")
            {
                List<ProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(StartDate, EndDate, Tax, TaxID, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);
                List<ProductWiseGSTReportList> objlst = new List<ProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    ProductWiseGSTReportList newobj = new ProductWiseGSTReportList();
                    newobj.ProductID = item.ProductID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.OrderQuantity = item.OrderQuantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = CustomerID.Value;
                    newobj.AreaID = AreaID.Value;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, TaxID, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.OrderQuantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.CGST = item.CGST;
                    newobj.TaxAmtCGST = item.TaxAmtCGST;
                    newobj.SGST = item.SGST;
                    newobj.TaxAmtSGST = item.TaxAmtSGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<ProductMainListByMonthGST> lst = new List<ProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductID");
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
                                  ProductID = item.ProductID,
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
                    dr[0] = result.ToList()[i].ProductID;
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
                            if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
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
                List<ProductWiseGSTReportList> objlst1 = _reportservice.GetProductWiseSalesForGSTReport(StartDate, EndDate, Tax, TaxID, CustomerID.Value, AreaID.Value, ProductCategoryID.Value, ProductID.Value, UserID.Value);
                List<ProductWiseGSTReportList> objlst = new List<ProductWiseGSTReportList>();
                foreach (var item in objlst1)
                {
                    ProductWiseGSTReportList newobj = new ProductWiseGSTReportList();
                    newobj.ProductID = item.ProductID;
                    newobj.ProductName = item.ProductName;
                    newobj.HSNNumber = item.HSNNumber;
                    newobj.MonthName = item.MonthName;
                    newobj.YearName = item.YearName;
                    newobj.OrderQuantity = item.OrderQuantity;
                    newobj.OrderTotalAmount = item.OrderTotalAmount;
                    newobj.CustomerID = CustomerID.Value;
                    newobj.AreaID = AreaID.Value;
                    newobj.ProductCategoryID = item.ProductCategoryID;
                    newobj.UserID = item.UserID;
                    decimal ReturnedQuantity = _reportservice.GetReturnQuantityMonthWiseForGSTReport(newobj.MonthName, newobj.YearName, TaxID, newobj.ProductID, newobj.CustomerID, newobj.AreaID, newobj.ProductCategoryID, newobj.UserID);
                    if (ReturnedQuantity != 0)
                    {
                        newobj.Quantity = newobj.OrderQuantity - ReturnedQuantity;
                    }
                    else
                    {
                        newobj.Quantity = newobj.OrderQuantity;
                    }
                    newobj.FinalTaxableAmount = item.FinalTaxableAmount;
                    newobj.IGST = item.IGST;
                    newobj.TaxAmtIGST = item.TaxAmtIGST;
                    newobj.TotalAmount = item.TotalAmount;
                    objlst.Add(newobj);
                }
                List<ProductMainListByMonthGST> lst = new List<ProductMainListByMonthGST>();
                lst = objlst.GroupBy(x => new { x.MonthName, x.YearName }).Select(x => new ProductMainListByMonthGST() { MonthName = x.Key.MonthName, YearName = x.Key.YearName, ListMainProduct = x.ToList() }).ToList();
                lst = lst.OrderBy(y => y.YearName).ThenBy(x => x.MonthName).ToList();
                // DataTable dt = new DataTable();
                dt.Columns.Add("ProductID");
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
                                  ProductID = item.ProductID,
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
                    dr[0] = result.ToList()[i].ProductID;
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
                            if (dt.Rows[k]["ProductID"].ToString() == lst[i].ListMainProduct[j].ProductID.ToString())
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
            dt.Columns.Remove("ProductID");
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "ProductWiseGSTReport.xls");

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

        // 12 Sep 2020 Piyush Limbani
        [HttpPost]
        public PartialViewResult VoucherCashCounterReportList(VoucherCashCounterListResponse model)
        {
            ViewBag.CashOption = _commonservice.GetAllCashOption();
            List<VoucherCashCounterListResponse> objfinallist = new List<VoucherCashCounterListResponse>();
            List<VoucherCashCounterListResponse> objlst = _reportservice.GetWholesaleExpenseVoucherCashCounterReportList(model.AssignedDate, model.GodownID);
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
                objfinallist.Add(new VoucherCashCounterListResponse());
                objfinallist[0].CashTotal = 0;
                objfinallist[0].ChequeTotal = 0;
                objfinallist[0].CardTotal = 0;
                objfinallist[0].OnlineTotal = 0;
                objfinallist[0].AdjustAmountTotal = 0;
            }
            return PartialView(objfinallist);
        }

        [HttpPost]
        public ActionResult UpdateVoucherPayment(UpdateVoucherPayment data)
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
        public PartialViewResult VoucherCashCounterDayWiseSalesManList(VoucherCashCounterDayWiseSalesManList model)
        {
            List<VoucherCashCounterDayWiseSalesManList> objlst = _reportservice.GetVoucherCashCounterDayWiseSalesManList(model.AssignedDate, model.GodownID);
            return PartialView(objlst);
        }



        // 14 June,2021 Sonal Gandhi
        public ActionResult OnlineOrderProduct()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult OnlineOrderProductList(OnlineOrder model)
        {
            List<OnlineOrder> objlst = _reportservice.GetOnlineOrderProductList(model.InvDate, model.IsConfirm);
            ViewBag.VisibleIsConfirm = model.IsConfirm;
            return PartialView(objlst);
        }


        [HttpGet]
        public ActionResult ViewBillWiseOnlineOrderDetails(long OnlineOrderID)
        {
            if (OnlineOrderID > 0)
            {
                List<OnlineOrderQty> objModel = _reportservice.ViewBillWiseOnlineOrderDetails(OnlineOrderID);
                return View(objModel);
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult ManageOnlineOrders(long OnlineOrderId)
        {
            ViewBag.Product = _orderservice.GetAllProductName();
            try
            {
                OnlineOrderViewModel objModel = _reportservice.GetOnlineOrderDetailsByOnlineOrderID(OnlineOrderId);
                if (objModel.DeActiveCustomer == true)
                {
                    ViewBag.Customer = _orderservice.GetActiveCustomerName(objModel.CustomerID);
                }
                else
                {
                    ViewBag.Customer = _orderservice.GetActiveCustomerName(0);
                }
                return View(objModel);
            }
            catch
            {
                ViewBag.Customer = _orderservice.GetActiveCustomerName(0);
                return View();
            }
        }

        [HttpPost]
        public JsonResult UpdateOnlineOrderIsConfirm(long OnlineOrderId)
        {
            bool flag = false;
            flag = _reportservice.UpdateOnlineOrderIsConfirm(OnlineOrderId);
            return Json(flag);
        }

        //public JsonResult txtCustomerName_TextChanged(string obj)
        //{
        //    int quntity = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["drugQuntityshow"].ToString());
        //    List<CustomerName1> data = _orderservice.GetTaxForCustomerByTextChange(obj).Select(x => new CustomerName1() { CustomerName = x }).Take(quntity).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult txtCustomerName_AfterCustomerSelect(string obj)
        //{
        //    try
        //    {
        //        string[] cityarr = obj.Split(',');
        //        string ID = string.Empty;
        //        if (cityarr.Length > 1)
        //        {
        //            var Tax = _orderservice.GetTaxForCustomerNumber(Convert.ToInt64(cityarr[0]));
        //            return Json(Tax, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(false, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //Add By Dhruvik
        [HttpPost]
        public PartialViewResult CheckRetrunList(CashCounterListResponse model)
        {
            List<CashCounterListResponse> objlst = _reportservice.GetCheckRetrunEntryList(model.AssignedDate, model.GodownID);
            return PartialView(objlst);
        }
        //Add By Dhruvik

    }
}
