using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;

namespace vb.Service
{
    public class RetReportServices : IRetReportService
    {
        public List<RetDayWiseSalesList> GetDayWiseSalesList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseRetSalesList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDayWiseSalesList> objlst = new List<RetDayWiseSalesList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                decimal sumTCSTaxAmount = 0;
                decimal GrandNetAmount = 0;
                while (dr.Read())
                {
                    RetDayWiseSalesList obj = new RetDayWiseSalesList();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.InvoiceNumber = obj.InvCode + "/" + DateTimeExtensions.FromFinancialYear(obj.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.CreatedOn).ToString("yy");
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrossAmt"), 2);
                    obj.GrandGrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandGrossAmt"), 2);
                    //obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    if (objBaseSqlManager.GetTextValue(dr, "Tax") == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                        obj.TaxAmtIGST = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmt"), 2).ToString();
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0).ToString();
                        obj.TaxAmtSGST = Convert.ToDecimal(0).ToString();
                        sumIGST = sumIGST + Convert.ToDecimal(obj.TaxAmtIGST);
                    }
                    else
                    {
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0).ToString();
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtCGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtSGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        sumCGST = sumCGST + Convert.ToDecimal(obj.TaxAmtCGST);
                        sumSGST = sumSGST + Convert.ToDecimal(obj.TaxAmtSGST);
                    }
                    obj.TCSTaxAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount")), 2);
                    sumTCSTaxAmount = sumTCSTaxAmount + obj.TCSTaxAmount;
                    obj.NetAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmount")), 2);
                    obj.RoundOff = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOff")), 2);
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.IsApprove = objBaseSqlManager.GetBoolean(dr, "IsApprove");
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandCGSTAmt = sumCGST;
                    objlst[i].GrandIGSTAmt = sumIGST;
                    objlst[i].GrandSGSTAmt = sumSGST;
                    objlst[i].GrandTCSAmt = sumTCSTaxAmount;
                    objlst[i].GrandNetAmount = sumTCSTaxAmount + GrandNetAmount;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDayWiseCreditMemoList> GetDayWiseCreditMemoList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseRetCreditMemoList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDayWiseCreditMemoList> objlst = new List<RetDayWiseCreditMemoList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                while (dr.Read())
                {
                    RetDayWiseCreditMemoList obj = new RetDayWiseCreditMemoList();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.Invoice = objBaseSqlManager.GetTextValue(dr, "Invoice");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.InvoiceNumber = obj.Invoice + "/" + DateTimeExtensions.FromFinancialYear(obj.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.CreatedOn).ToString("yy");
                    obj.CreditMemo = objBaseSqlManager.GetTextValue(dr, "CreditMemo");
                    obj.Amount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Amount"), 2);
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrandAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandAmount"), 2);
                    obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    obj.NetAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmount")), 2);
                    obj.RoundOff = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOff")), 2);
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    if (objBaseSqlManager.GetTextValue(dr, "Tax") == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                        obj.TaxAmtIGST = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmt"), 2).ToString();
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0).ToString();
                        obj.TaxAmtSGST = Convert.ToDecimal(0).ToString();
                        sumIGST = sumIGST + Convert.ToDecimal(obj.TaxAmtIGST);
                    }
                    else
                    {
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0).ToString();
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtCGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtSGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        sumCGST = sumCGST + Convert.ToDecimal(obj.TaxAmtCGST);
                        sumSGST = sumSGST + Convert.ToDecimal(obj.TaxAmtSGST);
                    }
                    obj.ReferenceNumber = objBaseSqlManager.GetTextValue(dr, "ReferenceNumber");
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandCGSTAmt = sumCGST;
                    objlst[i].GrandIGSTAmt = sumIGST;
                    objlst[i].GrandSGSTAmt = sumSGST;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDayWiseTaxList> GetDayWiseTaxList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseRetTaxList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDayWiseTaxList> objlst = new List<RetDayWiseTaxList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                while (dr.Read())
                {
                    RetDayWiseTaxList obj = new RetDayWiseTaxList();
                    obj.Tax = objBaseSqlManager.GetTextValue(dr, "Tax");
                    obj.TaxPer = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxPer")), 2);
                    obj.GrandTaxPer = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrandTaxPer")), 2);
                    obj.GrossAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrossAmtTotal")), 2);
                    obj.GrandGrossAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrandGrossAmtTotal")), 2);
                    obj.TaxAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmtTotal")), 2);
                    obj.GrandTaxAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrandTaxAmtTotal")), 2);
                    obj.TCSAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "TCSAmtTotal")), 2);
                    obj.GrandTCSAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrandTCSAmtTotal")), 2);
                    obj.RoundOffTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOffTotal")), 2);
                    obj.GrandRoundOffTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrandRoundOffTotal")), 2);
                    obj.NetAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmtTotal")), 2);
                    obj.GrandNetAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrandNetAmtTotal")), 2);
                    if (objBaseSqlManager.GetTextValue(dr, "Tax") == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "TaxPer");
                        obj.TaxAmtIGST = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmtTotal"), 2).ToString();
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0).ToString();
                        obj.TaxAmtSGST = Convert.ToDecimal(0).ToString();
                        sumIGST = sumIGST + Convert.ToDecimal(obj.TaxAmtIGST);
                    }
                    else
                    {
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0).ToString();
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "TaxPer") / 2);
                        obj.TaxAmtCGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmtTotal") / 2), 2).ToString();
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "TaxPer") / 2);
                        obj.TaxAmtSGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmtTotal") / 2), 2).ToString();
                        sumCGST = sumCGST + Convert.ToDecimal(obj.TaxAmtCGST);
                        sumSGST = sumSGST + Convert.ToDecimal(obj.TaxAmtSGST);
                    }
                    objlst.Add(obj);
                }

                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandCGSTAmt = sumCGST;
                    objlst[i].GrandIGSTAmt = sumIGST;
                    objlst[i].GrandSGSTAmt = sumSGST;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDayWiseSalesManList> GetDayWiseSalesManList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseRetSalesManList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDayWiseSalesManList> objlst = new List<RetDayWiseSalesManList>();
                decimal sumGrossAmtTotal = 0;
                decimal sumTaxAmtTotal = 0;
                decimal sumRoundOffTotal = 0;
                decimal sumNetAmtTotal = 0;
                while (dr.Read())
                {
                    RetDayWiseSalesManList obj = new RetDayWiseSalesManList();
                    obj.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    obj.GrossAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrossAmtTotal")), 2);
                    obj.TaxAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmtTotal")), 2);
                    obj.RoundOffTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOffTotal")), 2);
                    obj.NetAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmtTotal")), 2);
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    sumGrossAmtTotal = sumGrossAmtTotal + obj.GrossAmtTotal;
                    sumTaxAmtTotal = sumTaxAmtTotal + obj.TaxAmtTotal;
                    sumRoundOffTotal = sumRoundOffTotal + obj.RoundOffTotal;
                    sumNetAmtTotal = sumNetAmtTotal + obj.NetAmtTotal;
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandGrossAmtTotal = sumGrossAmtTotal;
                    objlst[i].GrandTaxAmtTotal = sumTaxAmtTotal;
                    objlst[i].GrandRoundOffTotal = sumRoundOffTotal;
                    objlst[i].GrandNetAmtTotal = sumNetAmtTotal;
                    break;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDeliverySheetList> GetDeliverySheetList(DateTime? AssignedDate, string VehicleNo, string TempoNo, long GodownID, int BySign)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetDeliverySheetList";
                cmdGet.Parameters.AddWithValue("@TempoNo", TempoNo);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BySign", BySign);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDeliverySheetList> objlst = new List<RetDeliverySheetList>();
                while (dr.Read())
                {
                    RetDeliverySheetList obj = new RetDeliverySheetList();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.InvoiceNumber1 = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(obj.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.CreatedOn).ToString("yy");
                    obj.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    obj.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    obj.Sign = Math.Round((objBaseSqlManager.GetDecimal(dr, "Sign")), 2);
                    obj.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    obj.CashTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "CashTotal")), 2);
                    obj.ChequeTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "ChequeTotal")), 2);
                    obj.CardTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "CardTotal")), 2);
                    obj.SignTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "SignTotal")), 2);
                    obj.OnlineTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "OnlineTotal")), 2);
                    obj.Remarks = objBaseSqlManager.GetTextValue(dr, "Remarks");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.AdjustAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmount")), 2);
                    obj.AdjustAmountTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmountTotal")), 2);
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    obj.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    obj.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                    if (obj.ChequeDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ChequeDate1 = "";
                    }
                    else
                    {
                        obj.ChequeDate1 = obj.ChequeDate.ToString("dd/MM/yyyy");
                    }
                    obj.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFCCode");
                    obj.BankNameForCard = objBaseSqlManager.GetTextValue(dr, "BankNameForCard");
                    obj.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    obj.BankNameForOnline = objBaseSqlManager.GetTextValue(dr, "BankNameForOnline");
                    obj.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    obj.OnlinePaymentDate = objBaseSqlManager.GetDateTime(dr, "OnlinePaymentDate");
                    if (obj.OnlinePaymentDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OnlinePaymentDate1 = "";
                    }
                    else
                    {
                        obj.OnlinePaymentDate1 = obj.OnlinePaymentDate.ToString("dd/MM/yyyy");
                    }
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetProductWiseSalesList> GetProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetProductWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
                while (dr.Read())
                {
                    RetProductWiseSalesList obj = new RetProductWiseSalesList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.OrderTotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    obj.OrderAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.ReturnTotalKg = objBaseSqlManager.GetDecimal(dr, "ReturnTotalKg");
                    obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    obj.TotalKg = obj.OrderTotalKg - obj.ReturnTotalKg;
                    obj.TotalAmount = obj.OrderAmount - obj.ReturnOrderAmount;
                    if (CustomerID != 0)
                    {
                        string ArticleCode = GetProductArticleCodeByCustomerGroupWise(CustomerID, obj.ProductQtyID);
                        obj.ArticleCode = ArticleCode;
                    }
                    else
                    {
                        obj.ArticleCode = "";
                    }
                    //obj.ReturnedQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity"), 0);
                    //obj.Quantity = obj.OrderQuantity - obj.ReturnedQuantity;
                    //obj.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string GetProductArticleCodeByCustomerGroupWise(long CustomerID, long ProductQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductArticleCodeByCustomerGroupWise";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string ArticleCode = "";
                while (dr.Read())
                {
                    ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ArticleCode;
            }
        }

        public decimal GetRetReturnQuantityMonthWise(int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnQuantityMonthWise";
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnedQuantity = 0;
                while (dr.Read())
                {
                    decimal ReturnedQuantity1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity"), 0);
                    ReturnedQuantity = ReturnedQuantity1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnedQuantity;
            }
        }

        public decimal GetRetReturnReturnTotalKgMonthWise(int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnReturnTotalKgMonthWise";
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnTotalKg = 0;
                while (dr.Read())
                {
                    decimal ReturnTotalKg1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnTotalKg"), 0);
                    ReturnTotalKg = ReturnTotalKg1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnTotalKg;
            }
        }

        public decimal GetRetReturnTotalAmountMonthWise(int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnTotalAmountMonthWise";
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnOrderAmount = 0;
                while (dr.Read())
                {
                    decimal ReturnOrderAmount1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount"), 0);
                    ReturnOrderAmount = ReturnOrderAmount1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnOrderAmount;
            }
        }

        public List<RetProductWiseSalesList> GetProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetProductWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
                while (dr.Read())
                {
                    RetProductWiseSalesList obj = new RetProductWiseSalesList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.OrderTotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    obj.OrderAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    if (CustomerID != 0)
                    {
                        string ArticleCode = GetProductArticleCodeByCustomerGroupWise(CustomerID, obj.ProductQtyID);
                        obj.ArticleCode = ArticleCode;
                    }
                    else
                    {
                        obj.ArticleCode = "";
                    }

                    //obj.ReturnedQuantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity"), 0);
                    //obj.ReturnTotalKg = objBaseSqlManager.GetDecimal(dr, "ReturnTotalKg");
                    //obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    //obj.Quantity = obj.OrderQuantity - obj.ReturnedQuantity;
                    //obj.TotalKg = obj.OrderTotalKg - obj.ReturnTotalKg;
                    //obj.TotalAmount = obj.OrderAmount - obj.ReturnOrderAmount;
                    //obj.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public decimal GetRetReturnQuantityDayWise(int DayName, int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnQuantityDayWise";
                cmdGet.Parameters.AddWithValue("@DayName", DayName);
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnedQuantity = 0;
                while (dr.Read())
                {
                    decimal ReturnedQuantity1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity"), 0);
                    ReturnedQuantity = ReturnedQuantity1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnedQuantity;
            }
        }

        public decimal GetRetReturnTotalKgDayWise(int DayName, int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnTotalKgDayWise";
                cmdGet.Parameters.AddWithValue("@DayName", DayName);
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnTotalKg = 0;
                while (dr.Read())
                {
                    decimal ReturnTotalKg1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnTotalKg"), 0);
                    ReturnTotalKg = ReturnTotalKg1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnTotalKg;
            }
        }

        public decimal GetRetReturnOrderAmountDayWise(int DayName, int MonthName, int YearName, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnOrderAmountDayWise";
                cmdGet.Parameters.AddWithValue("@DayName", DayName);
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnOrderAmount = 0;
                while (dr.Read())
                {
                    decimal ReturnOrderAmount1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount"), 0);
                    ReturnOrderAmount = ReturnOrderAmount1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnOrderAmount;
            }
        }

        public List<RetCustomerListResponse> GetAllCustomerMasterList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerMasterList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> objlst = new List<RetCustomerListResponse>();
                while (dr.Read())
                {
                    RetCustomerListResponse objCustomer = new RetCustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    objCustomer.ContactMobNumber = objBaseSqlManager.GetTextValue(dr, "ContactMobNumber");
                    objCustomer.ContactEmail = objBaseSqlManager.GetTextValue(dr, "ContactEmail");
                    objCustomer.ContactName = objBaseSqlManager.GetTextValue(dr, "ContactName");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objCustomer.Country = objBaseSqlManager.GetTextValue(dr, "Country");
                    objCustomer.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objCustomer.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objCustomer.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objCustomer.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    objCustomer.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objCustomer.OpeningTime = objBaseSqlManager.GetDateTime(dr, "OpeningTime");
                    objCustomer.ClosingTime = objBaseSqlManager.GetDateTime(dr, "ClosingTime");
                    objCustomer.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objCustomer.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objCustomer.CustomerTypeID = objBaseSqlManager.GetInt32(dr, "CustomerTypeID");
                    objCustomer.CustomerTypeName = new Utility2().GetTextEnum(objCustomer.CustomerTypeID);
                    objCustomer.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    objCustomer.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objCustomer.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objCustomer.CallWeek1 = objBaseSqlManager.GetBoolean(dr, "CallWeek1");
                    objCustomer.CallWeek2 = objBaseSqlManager.GetBoolean(dr, "CallWeek2");
                    objCustomer.CallWeek3 = objBaseSqlManager.GetBoolean(dr, "CallWeek3");
                    objCustomer.CallWeek4 = objBaseSqlManager.GetBoolean(dr, "CallWeek4");
                    objCustomer.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objCustomer.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCustomer.DeliveryAreaID = objBaseSqlManager.GetInt64(dr, "DeliveryAreaID");
                    objCustomer.DeliveryAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    objCustomer.DeliveryAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    objCustomer.BillingAreaID = objBaseSqlManager.GetInt64(dr, "BillingAreaID");
                    objCustomer.BillingAddressLine1 = objBaseSqlManager.GetTextValue(dr, "BillingAddressLine1");
                    objCustomer.BillingAddressLine2 = objBaseSqlManager.GetTextValue(dr, "BillingAddressLine2");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetSalesManWiseSalesList> GetSalesManWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetSalesManWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetSalesManWiseSalesList> objlst = new List<RetSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetSalesManWiseSalesList obj = new RetSalesManWiseSalesList();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.ReturnOrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount"), 0);
                    obj.Quantity = obj.OrderAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 03 Oct 2020 Piyush Limbani
        public List<RetSalesManWiseSalesList> GetRetSalesManWiseSalesListForFinalisedOrder(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetSalesManWiseSalesListForFinalisedOrder";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetSalesManWiseSalesList> objlst = new List<RetSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetSalesManWiseSalesList obj = new RetSalesManWiseSalesList();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.ReturnOrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount"), 0);
                    obj.Quantity = obj.OrderAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }
        // 03 Oct 2020 Piyush Limbani

        public List<RetSalesManWiseSalesList> GetSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetSalesManWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetSalesManWiseSalesList> objlst = new List<RetSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetSalesManWiseSalesList obj = new RetSalesManWiseSalesList();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.ReturnOrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount"), 0);
                    obj.Quantity = obj.OrderAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 03 Oct 2020 Piyush Limbani
        public List<RetSalesManWiseSalesList> GetRetSalesManWiseDailySalesListForFinalisedOrder(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetSalesManWiseDailySalesListForFinalisedOrder";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetSalesManWiseSalesList> objlst = new List<RetSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetSalesManWiseSalesList obj = new RetSalesManWiseSalesList();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.ReturnOrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount"), 0);
                    obj.Quantity = obj.OrderAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }
        // 03 Oct 2020 Piyush Limbani

        public List<RetCustGroupProductWiseSalesList> GetRetCustGroupTotalProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustGroupTotalProductWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustGroupProductWiseSalesList> objlst = new List<RetCustGroupProductWiseSalesList>();
                while (dr.Read())
                {
                    RetCustGroupProductWiseSalesList obj = new RetCustGroupProductWiseSalesList();
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.TotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCustGroupProductWiseSalesList> GetCustGroupProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustGroupProductWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustGroupProductWiseSalesList> objlst = new List<RetCustGroupProductWiseSalesList>();
                while (dr.Read())
                {
                    RetCustGroupProductWiseSalesList obj = new RetCustGroupProductWiseSalesList();
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.TotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCustGroupProductWiseSalesList> GetCustGroupProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustGroupProductWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustGroupProductWiseSalesList> objlst = new List<RetCustGroupProductWiseSalesList>();
                while (dr.Read())
                {
                    RetCustGroupProductWiseSalesList obj = new RetCustGroupProductWiseSalesList();
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.TotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCustGroupSalesManWiseSalesList> GetCustGroupSalesManWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustGroupSalesManWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustGroupSalesManWiseSalesList> objlst = new List<RetCustGroupSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetCustGroupSalesManWiseSalesList obj = new RetCustGroupSalesManWiseSalesList();
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCustGroupSalesManWiseSalesList> GetCustGroupSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustGroupSalesManWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustGroupSalesManWiseSalesList> objlst = new List<RetCustGroupSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetCustGroupSalesManWiseSalesList obj = new RetCustGroupSalesManWiseSalesList();
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"), 0);
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetDayWiseSalesList> GetDayWiseSalesListByUserID(long UserID, string Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseRetSalesListByUserID";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", Date);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDayWiseSalesList> objlst = new List<RetDayWiseSalesList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                decimal sumTCSTaxAmount = 0;
                decimal GrandNetAmount = 0;
                while (dr.Read())
                {
                    RetDayWiseSalesList obj = new RetDayWiseSalesList();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.InvoiceNumber = obj.InvCode + "/" + DateTimeExtensions.FromFinancialYear(obj.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.CreatedOn).ToString("yy");
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrossAmt"), 2);
                    obj.GrandGrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandGrossAmt"), 2);
                    //obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    if (objBaseSqlManager.GetTextValue(dr, "Tax") == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                        obj.TaxAmtIGST = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmt"), 2).ToString();
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0).ToString();
                        obj.TaxAmtSGST = Convert.ToDecimal(0).ToString();
                        sumIGST = sumIGST + Convert.ToDecimal(obj.TaxAmtIGST);
                    }
                    else
                    {
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0).ToString();
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtCGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                        obj.TaxAmtSGST = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmt") / 2), 2).ToString();
                        sumCGST = sumCGST + Convert.ToDecimal(obj.TaxAmtCGST);
                        sumSGST = sumSGST + Convert.ToDecimal(obj.TaxAmtSGST);
                    }
                    obj.TCSTaxAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount")), 2);
                    sumTCSTaxAmount = sumTCSTaxAmount + obj.TCSTaxAmount;
                    obj.NetAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmount")), 2);
                    obj.RoundOff = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOff")), 2);
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandCGSTAmt = sumCGST;
                    objlst[i].GrandIGSTAmt = sumIGST;
                    objlst[i].GrandSGSTAmt = sumSGST;
                    objlst[i].GrandTCSAmt = sumTCSTaxAmount;
                    objlst[i].GrandNetAmount = sumTCSTaxAmount + GrandNetAmount;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetPouchListResponse> GetPouchWiseReportList(DateTime? StartDate, DateTime? EndDate, string PouchNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetPouchWiseReportList";
                cmdGet.Parameters.AddWithValue("@PouchNameID", PouchNameID);
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetPouchListResponse> objlst = new List<RetPouchListResponse>();
                while (dr.Read())
                {
                    RetPouchListResponse objPayment = new RetPouchListResponse();
                    objPayment.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objPayment.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objPayment.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "TotalQuantity");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCashCounterListResponse> GetCashCounterReportList(DateTime? AssignedDate, long GodownID, int BySign)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCashCounterReportList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BySign", BySign);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCashCounterListResponse> objlst = new List<RetCashCounterListResponse>();
                decimal CashTotal = 0;
                decimal ChequeTotal = 0;
                decimal CardTotal = 0;
                decimal SignTotal = 0;
                decimal OnlineTotal = 0;
                decimal AdjustAmountTotal = 0;
                while (dr.Read())
                {
                    RetCashCounterListResponse obj = new RetCashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    DateTime UpdatedOn = GetUpdatedDateByInvoiceNumber(obj.OrderID, obj.InvoiceNumber);
                    obj.InvoiceNumber1 = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(UpdatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(UpdatedOn).ToString("yy");
                    //   obj.InvoiceNumber1 = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(obj.CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.CreatedOn).ToString("yy");
                    obj.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    CashTotal += obj.Cash;
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);

                    //Add By Dhruvik
                    if (obj.Cheque == 0)
                    {
                        obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "OutAmount")), 2);
                        if (obj.Cheque == 0)
                        {
                            obj.Cheque = Convert.ToDecimal("0.00");
                        }
                    }
                    //Add By Dhruvik

                    ChequeTotal += obj.Cheque;
                    obj.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    CardTotal += obj.Card;
                    obj.Sign = Math.Round((objBaseSqlManager.GetDecimal(dr, "Sign")), 2);
                    SignTotal += obj.Sign;
                    obj.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    OnlineTotal += obj.Online;
                    obj.AdjustAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmount")), 2);
                    AdjustAmountTotal += obj.AdjustAmount;
                    obj.CashTotal = CashTotal;
                    obj.ChequeTotal = ChequeTotal;
                    obj.CardTotal = CardTotal;
                    obj.SignTotal = SignTotal;
                    obj.OnlineTotal = OnlineTotal;
                    obj.AdjustAmountTotal = AdjustAmountTotal;
                    obj.Remarks = objBaseSqlManager.GetTextValue(dr, "Remarks");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    obj.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    obj.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                    if (obj.ChequeDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ChequeDate1 = "";
                    }
                    else
                    {
                        obj.ChequeDate1 = obj.ChequeDate.ToString("dd/MM/yyyy");
                    }
                    obj.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFCCode");
                    obj.BankNameForCard = objBaseSqlManager.GetTextValue(dr, "BankNameForCard");
                    obj.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    obj.BankNameForOnline = objBaseSqlManager.GetTextValue(dr, "BankNameForOnline");
                    obj.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    obj.OnlinePaymentDate = objBaseSqlManager.GetDateTime(dr, "OnlinePaymentDate");
                    if (obj.OnlinePaymentDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OnlinePaymentDate1 = "";
                    }
                    else
                    {
                        obj.OnlinePaymentDate1 = obj.OnlinePaymentDate.ToString("dd/MM/yyyy");
                    }
                    obj.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");

                    if (obj.IsDelivered == true)
                    {
                        obj.IsDeliveredstr = "Delivered";
                    }
                    else
                    {
                        obj.IsDeliveredstr = "Not Delivered";
                    }
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");

                    //Add By Dhruvik
                    obj.ByCash = Math.Round(objBaseSqlManager.GetDecimal(dr, "ByCash"), 2);
                    obj.ChequeReturnAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "ChequeReturnAmount"), 2);
                    //Add By Dhruvik

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public DateTime GetUpdatedDateByInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUpdatedDateByInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetCashCounterListResponse obj = new RetCashCounterListResponse();
                while (dr.Read())
                {
                    obj.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj.UpdatedOn;
            }
        }

        public List<RetBillHistoryListResponse> GetBillHistoryList(string InvoiceNumber, long OrderID, string UpdatedOn, DateTime? FromDate, DateTime? ToDate, long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetBillHistoryList";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetBillHistoryListResponse> objlst = new List<RetBillHistoryListResponse>();
                while (dr.Read())
                {
                    RetBillHistoryListResponse objBillHistrory = new RetBillHistoryListResponse();
                    objBillHistrory.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    objBillHistrory.AreaName = objBaseSqlManager.GetTextValue(dr, "Area");
                    objBillHistrory.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objBillHistrory.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objBillHistrory.FullInvoiceNumber = objBillHistrory.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objBillHistrory.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objBillHistrory.InvoiceDate).ToString("yy");
                    objBillHistrory.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objBillHistrory.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    var lstdata = GetPaymentDetailForBillHistory(objBillHistrory.OrderID, objBillHistrory.InvoiceNumber, objBillHistrory.PaymentID);
                    objBillHistrory.PaymentDate = lstdata.PaymentDate;
                    if (objBillHistrory.PaymentDate == Convert.ToDateTime("01/01/0001") || objBillHistrory.PaymentDate == Convert.ToDateTime("10/10/2014"))
                    {
                        objBillHistrory.PaymentDatestr = "";
                    }
                    else
                    {
                        objBillHistrory.PaymentDatestr = objBillHistrory.PaymentDate.ToString("dd/MM/yyyy");
                    }
                    objBillHistrory.Cash = lstdata.Cash;
                    objBillHistrory.Cheque = lstdata.Cheque;
                    objBillHistrory.Card = lstdata.Card;
                    objBillHistrory.Online = lstdata.Online;
                    objBillHistrory.AdjustAmount = lstdata.AdjustAmount;
                    objBillHistrory.BySign = lstdata.BySign;
                    objBillHistrory.IsCreditNote = lstdata.IsCreditNote;
                    //if (objBillHistrory.IsCreditNote == true)
                    //{
                    objBillHistrory.CreditMemoDate = objBaseSqlManager.GetDateTime(dr, "CreditMemoDate");
                    if (objBillHistrory.CreditMemoDate == Convert.ToDateTime("10/10/2014"))
                    {
                        objBillHistrory.CreditMemoDatestr = "";
                    }
                    else
                    {
                        objBillHistrory.CreditMemoDatestr = objBillHistrory.CreditMemoDate.ToString("dd/MM/yyyy");
                    }
                    objBillHistrory.CreditMemoNumber = objBaseSqlManager.GetTextValue(dr, "CreditMemoNumber");
                    objBillHistrory.CreditMemoAmount = objBaseSqlManager.GetDecimal(dr, "CreditMemoAmount");
                    //  }                             
                    var lstdeliverydata = GetVehicleDetailForBillHistory(objBillHistrory.InvoiceNumber, objBillHistrory.OrderID);
                    objBillHistrory.DeliveryDate = lstdeliverydata.DeliveryDate;
                    if (objBillHistrory.DeliveryDate == Convert.ToDateTime("01/01/0001") || objBillHistrory.DeliveryDate == Convert.ToDateTime("10/10/2014"))
                    {
                        objBillHistrory.DeliveryDatestr = "";
                    }
                    else
                    {
                        objBillHistrory.DeliveryDatestr = objBillHistrory.DeliveryDate.ToString("dd/MM/yyyy");
                    }
                    objBillHistrory.DeliveryPerson1 = lstdeliverydata.DeliveryPerson1;
                    objBillHistrory.DeliveryPerson2 = lstdeliverydata.DeliveryPerson2;
                    objBillHistrory.DeliveryPerson3 = lstdeliverydata.DeliveryPerson3;
                    objBillHistrory.DeliveryPerson4 = lstdeliverydata.DeliveryPerson4;
                    objBillHistrory.TempoNo = lstdeliverydata.TempoNo;
                    objBillHistrory.VehicleNo = lstdeliverydata.VehicleNo;
                    objBillHistrory.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");
                    if (objBillHistrory.IsDelivered == true)
                    {
                        objBillHistrory.DeliveryStatus = "Delivered";
                    }
                    else
                    {
                        objBillHistrory.DeliveryStatus = "Pending";
                    }
                    objlst.Add(objBillHistrory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public RetBillHistoryListResponse GetPaymentDetailForBillHistory(long OrderID, string InvoiceNumber, long PaymentID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetPaymentDetailForBillHistory";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@PaymentID", PaymentID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetBillHistoryListResponse obj = new RetBillHistoryListResponse();
                while (dr.Read())
                {
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.PaymentDate = objBaseSqlManager.GetDateTime(dr, "PaymentDate");
                    obj.Cash = objBaseSqlManager.GetDecimal(dr, "Cash");
                    obj.Cheque = objBaseSqlManager.GetDecimal(dr, "Cheque");
                    obj.Card = objBaseSqlManager.GetDecimal(dr, "Card");
                    obj.Online = objBaseSqlManager.GetDecimal(dr, "Online");
                    obj.AdjustAmount = objBaseSqlManager.GetDecimal(dr, "AdjustAmount");
                    obj.BySign = objBaseSqlManager.GetBoolean(dr, "BySign");
                    obj.VehicleNo = objBaseSqlManager.GetInt64(dr, "VehicleNo");
                    obj.IsCreditNote = objBaseSqlManager.GetBoolean(dr, "IsCreditNote");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public RetBillHistoryListResponse GetVehicleDetailForBillHistory(string InvoiceNumber, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetVehicleDetailForBillHistory";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetBillHistoryListResponse obj = new RetBillHistoryListResponse();
                while (dr.Read())
                {
                    obj.DeliveryDate = objBaseSqlManager.GetDateTime(dr, "DeliveryDate");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetTextValue(dr, "DeliveryPerson4");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    obj.VehicleNo = objBaseSqlManager.GetInt64(dr, "VehicleNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<RetCashCounterDayWiseSalesManList> GetCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID, int BySign)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCashCounterDayWiseSalesManList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BySign", BySign);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCashCounterDayWiseSalesManList> objlst = new List<RetCashCounterDayWiseSalesManList>();
                decimal sumCashAmtTotal = 0;
                decimal sumChequeAmtTotal = 0;
                decimal sumCardAmtTotal = 0;
                decimal sumSignAmtTotal = 0;
                decimal sumOnlineAmtTotal = 0;
                decimal sumAdjustAmountAmtTotal = 0;
                while (dr.Read())
                {
                    RetCashCounterDayWiseSalesManList obj = new RetCashCounterDayWiseSalesManList();
                    obj.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    obj.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    if (BySign == 0)
                    {
                        obj.Sign = Convert.ToDecimal("0.00");
                    }
                    else
                    {
                        obj.Sign = Math.Round((objBaseSqlManager.GetDecimal(dr, "Sign")), 2);
                    }
                    obj.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    obj.AdjustAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmount")), 2);
                    sumCashAmtTotal = sumCashAmtTotal + obj.Cash;
                    sumChequeAmtTotal = sumChequeAmtTotal + obj.Cheque;
                    sumCardAmtTotal = sumCardAmtTotal + obj.Card;
                    sumSignAmtTotal = sumSignAmtTotal + obj.Sign;
                    sumOnlineAmtTotal = sumOnlineAmtTotal + obj.Online;
                    sumAdjustAmountAmtTotal = sumAdjustAmountAmtTotal + obj.AdjustAmount;
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].CashTotal = sumCashAmtTotal;
                    objlst[i].ChequeTotal = sumChequeAmtTotal;
                    objlst[i].CardTotal = sumCardAmtTotal;
                    objlst[i].SignTotal = sumSignAmtTotal;
                    objlst[i].OnlineTotal = sumOnlineAmtTotal;
                    objlst[i].AdjustAmountTotal = sumAdjustAmountAmtTotal;
                    break;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdatePayment(RetPayment data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateRetailPayment";
                    cmdGet.Parameters.AddWithValue("@PaymentID", data.PaymentID);
                    cmdGet.Parameters.AddWithValue("@OrderID", data.OrderID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", data.InvoiceNumber);
                    cmdGet.Parameters.AddWithValue("@Amount", data.Amount);
                    cmdGet.Parameters.AddWithValue("@GodownID", data.GodownID);
                    cmdGet.Parameters.AddWithValue("@Cheque", data.Cheque);
                    cmdGet.Parameters.AddWithValue("@BankName", data.BankName);
                    cmdGet.Parameters.AddWithValue("@BankBranch", data.BankBranch);
                    cmdGet.Parameters.AddWithValue("@ChequeNo", data.ChequeNo);
                    cmdGet.Parameters.AddWithValue("@ChequeDate", data.ChequeDate);
                    cmdGet.Parameters.AddWithValue("@IFCCode", data.IFCCode);
                    cmdGet.Parameters.AddWithValue("@Online", data.Online);
                    cmdGet.Parameters.AddWithValue("@BankNameForOnline", data.BankNameForOnline);
                    cmdGet.Parameters.AddWithValue("@UTRNumber", data.UTRNumber);
                    cmdGet.Parameters.AddWithValue("@OnlinePaymentDate", data.OnlinePaymentDate);
                    cmdGet.Parameters.AddWithValue("@Card", data.Card);
                    cmdGet.Parameters.AddWithValue("@BankNameForCard", data.BankNameForCard);
                    cmdGet.Parameters.AddWithValue("@TypeOfCard", data.TypeOfCard);
                    cmdGet.Parameters.AddWithValue("@Sign", data.Sign);
                    cmdGet.Parameters.AddWithValue("@AdjustAmount", data.AdjustAmount);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        //public int GetOrderIDForBillHistory(string InvoiceNumber, string Year)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetOrderIDForBillHistory";
        //    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
        //    cmdGet.Parameters.AddWithValue("@Year", Year);           
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    int OrderID = 0;
        //    while (dr.Read())
        //    {
        //        OrderID = objBaseSqlManager.GetInt32(dr, "OrderID");
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return OrderID;
        //}

        public List<GetRetOrderIDResponse> GetOrderIDForBillHistory(string InvoiceNumber, string Year)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOrderIDForBillHistory";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@Year", Year);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetRetOrderIDResponse> objlst = new List<GetRetOrderIDResponse>();
                while (dr.Read())
                {
                    GetRetOrderIDResponse objBillHistrory = new GetRetOrderIDResponse();
                    objBillHistrory.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objBillHistrory.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    objlst.Add(objBillHistrory);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public RetVehicleNoListResponse GetAllVehicleNoForDeliverysheetReport(DateTime AssignedDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetAllVehicleNoForDeliverysheetReport";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetVehicleNoListResponse objOrder = new RetVehicleNoListResponse();
                while (dr.Read())
                {
                    objOrder.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public string GetRetCustomerIDForSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCustomerIDForSalesList2";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string CustomerIDs = "";
                while (dr.Read())
                {
                    RetSalesManWiseSalesList obj = new RetSalesManWiseSalesList();
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");

                    if (obj.CustomerID > 0)
                    {
                        CustomerIDs += obj.CustomerID + ",";
                    }

                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CustomerIDs;
            }
        }

        public List<RetSalesManWiseSalesList> GetRetSalesManWiseSalesList2(string CustomerIDs, long UserID, long AreaID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetSalesManWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@CustomerIDs", CustomerIDs);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetSalesManWiseSalesList> objlst = new List<RetSalesManWiseSalesList>();
                while (dr.Read())
                {
                    RetSalesManWiseSalesList obj = new RetSalesManWiseSalesList();
                    obj.SrNo = objBaseSqlManager.GetTextValue(dr, "SrNo");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo1");
                    obj.TotalQuantity = 0;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string GetProductIDForRetProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductIDForRetProductWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string ProductQtyIDs = "";
                while (dr.Read())
                {
                    RetProductWiseSalesList obj = new RetProductWiseSalesList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");

                    if (obj.ProductQtyID > 0)
                    {
                        ProductQtyIDs += obj.ProductQtyID + ",";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ProductQtyIDs;
            }
        }

        public List<RetProductWiseSalesList> GetProductWiseSalesList2(DateTime StartDate1, DateTime EndDate1, string ProductQtyIDs, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetProductWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate1);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate1);
                cmdGet.Parameters.AddWithValue("@ProductQtyIDs", ProductQtyIDs);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductWiseSalesList> objlst = new List<RetProductWiseSalesList>();
                while (dr.Read())
                {
                    RetProductWiseSalesList obj = new RetProductWiseSalesList();
                    obj.SrNo = objBaseSqlManager.GetTextValue(dr, "SrNo");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    obj.TotalQuantity = 0;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // GST Report
        public List<RetProductWiseGSTReportList> GetProductWiseSalesForGSTReport(DateTime? StartDate, DateTime? EndDate, string Tax, long TaxID, long CustomerID, long AreaID, long ProductCategoryID, long ProductQtyID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetProductWiseSalesForGSTReport";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@Tax", Tax);
                cmdGet.Parameters.AddWithValue("@TaxID", TaxID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductWiseGSTReportList> objlst = new List<RetProductWiseGSTReportList>();
                while (dr.Read())
                {
                    RetProductWiseGSTReportList obj = new RetProductWiseGSTReportList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.OrderTotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.TotalTaxAmount = objBaseSqlManager.GetDecimal(dr, "TotalTaxAmount");
                    obj.TotalTaxableAmount = obj.OrderTotalAmount - obj.TotalTaxAmount;
                    obj.ReturnTaxableAmount = objBaseSqlManager.GetDecimal(dr, "ReturnTaxableAmount");
                    obj.FinalTaxableAmount = obj.TotalTaxableAmount - obj.ReturnTaxableAmount;
                    obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    obj.FinalOrderTotalAmount = obj.OrderTotalAmount - obj.ReturnOrderAmount;
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    decimal TotalTaxAmt = ((obj.FinalTaxableAmount * obj.TotalTax) / 100);
                    if (objBaseSqlManager.GetDecimal(dr, "TotalTax") != 0)
                    {
                        if (Tax == "SGST")
                        {
                            obj.CGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                            obj.TaxAmtCGST = Math.Round((TotalTaxAmt / 2), 2).ToString();
                            obj.SGST = (objBaseSqlManager.GetDecimal(dr, "TotalTax") / 2);
                            obj.TaxAmtSGST = Math.Round((TotalTaxAmt / 2), 2).ToString();
                        }
                        else
                        {

                            obj.IGST = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                            obj.TaxAmtIGST = Math.Round(TotalTaxAmt, 2).ToString();
                        }
                    }
                    else
                    {
                        obj.CGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0).ToString();
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtSGST = Convert.ToDecimal(0).ToString();
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0).ToString();
                    }
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.TotalAmount = obj.OrderTotalAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public decimal GetRetReturnQuantityMonthWiseForGSTReport(int MonthName, int YearName, long TaxID, long ProductQtyID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetReturnQuantityMonthWiseForGSTReport";
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@TaxID", TaxID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal ReturnedQuantity = 0;
                while (dr.Read())
                {
                    decimal ReturnedQuantity1 = Math.Round(objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity"), 0);
                    ReturnedQuantity = ReturnedQuantity1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ReturnedQuantity;
            }
        }



        public List<DayWiseSalesExportListForExp> GetDayWiseExportSalesList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseExportSalesList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseSalesExportListForExp> objlst = new List<DayWiseSalesExportListForExp>();

                while (dr.Read())
                {
                    DayWiseSalesExportListForExp obj = new DayWiseSalesExportListForExp();
                    //obj.CustomerCode = objBaseSqlManager.GetInt64(dr, "CustomerCode");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Country = objBaseSqlManager.GetTextValue(dr, "Country");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.InsuranceText = objBaseSqlManager.GetTextValue(dr, "InsuranceText");
                    obj.InsuranceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "InsuranceAmountRupees"), 2);
                    obj.FrieghtText = objBaseSqlManager.GetTextValue(dr, "FreightText");
                    obj.FreightAMount = Math.Round(objBaseSqlManager.GetDecimal(dr, "FreightAmountRupees"), 2);
                    obj.NetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalInvoiceAmountINRupee"), 2);
                    obj.Rupees = Math.Round(objBaseSqlManager.GetDecimal(dr, "Rupees"), 2);
                    obj.TotalPkgs = objBaseSqlManager.GetInt32(dr, "TotalPkgs");
                    //  obj.ContainerNo = objBaseSqlManager.GetTextValue(dr, "ContainerNo");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.InvoiceTotalAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTtlAmtRpee"), 2);
                    obj.RoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "RoundOff"), 2);
                    //obj.InvoiceTotalAmtinRupee = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTtlAmtRpee"), 2);
                    //obj.FreightAMountinRupee = Math.Round(objBaseSqlManager.GetDecimal(dr, "FreightAmountRupees"), 2);
                    //obj.InTotalAmountinRupee = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalInvoiceAmountINRupee"), 2);
                    //obj.InsuranceAmountinRupee = Math.Round(objBaseSqlManager.GetDecimal(dr, "InsuranceAmountRupees"), 2); 

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpProductWiseSalesList> GetExpProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExProductWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@CountryID", CountryID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpProductWiseSalesList> objlst = new List<ExpProductWiseSalesList>();
                while (dr.Read())
                {
                    ExpProductWiseSalesList obj = new ExpProductWiseSalesList();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    obj.TotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpProductWiseSalesList> GetExProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExProductWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@CountryID", CountryID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpProductWiseSalesList> objlst = new List<ExpProductWiseSalesList>();
                while (dr.Read())
                {
                    ExpProductWiseSalesList obj = new ExpProductWiseSalesList();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    obj.OrderTotalAmount = objBaseSqlManager.GetDecimal(dr, "OrderTotalAmount");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.TotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKg");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        public string GetExpProductIDForProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpProductIDForProductWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string ProductIDs = "";
                while (dr.Read())
                {
                    ProductWiseSalesList obj = new ProductWiseSalesList();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");

                    if (obj.ProductID > 0)
                    {
                        ProductIDs += obj.ProductID + ",";
                    }
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return ProductIDs;
            }
        }

        public List<ExpProductWiseSalesList> GetExpProductWiseSalesList2(DateTime StartDate1, DateTime EndDate1, string ProductIDs, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpProductWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate1);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate1);
                cmdGet.Parameters.AddWithValue("@ProductIDs", ProductIDs);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpProductWiseSalesList> objlst = new List<ExpProductWiseSalesList>();
                while (dr.Read())
                {
                    ExpProductWiseSalesList obj = new ExpProductWiseSalesList();
                    obj.SrNo = objBaseSqlManager.GetTextValue(dr, "SrNo");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    obj.TotalQuantity = 0;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<SalesManWiseExpSalesList> GetSalesManWiseExpSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSalesManWiseExpSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@CountryID", CountryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                //cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseExpSalesList> objlst = new List<SalesManWiseExpSalesList>();
                while (dr.Read())
                {
                    SalesManWiseExpSalesList obj = new SalesManWiseExpSalesList();
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "OrderAmount"), 0);
                    obj.InvoiceTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTotalAmount"), 0);
                    //obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    //obj.Quantity = Math.Round(obj.OrderAmount - obj.ReturnOrderAmount, 0);
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<SalesManWiseExpSalesList> GetExSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long CountryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExSalesManWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@CountryID", CountryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                //cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseExpSalesList> objlst = new List<SalesManWiseExpSalesList>();
                while (dr.Read())
                {
                    SalesManWiseExpSalesList obj = new SalesManWiseExpSalesList();
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "OrderAmount"), 0);
                    obj.InvoiceTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTotalAmt"), 0);
                    //obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    //obj.Quantity = Math.Round(obj.OrderAmount - obj.ReturnOrderAmount, 0);
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 03-04-2020 - barcode history
        public List<BarcodeHistoryListResponse> GetBarcodeHistoryList(BarcodeHistoryListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBarcodeHistoryList";
                cmdGet.Parameters.AddWithValue("@EmployeeID", model.EmployeeID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", model.ProductQtyID);
                if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@From", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@From", model.From);
                }
                if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@To", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@To", model.To);
                }
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BarcodeHistoryListResponse> objlst = new List<BarcodeHistoryListResponse>();
                while (dr.Read())
                {
                    BarcodeHistoryListResponse obj = new BarcodeHistoryListResponse();
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    if (obj.CreatedOn != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.CreatedDate = obj.CreatedOn.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.CreatedDate = "";
                    }
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.NoOfBarcodes = objBaseSqlManager.GetInt64(dr, "NoOfBarcodes");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.TotalNoOfBarcode = objBaseSqlManager.GetInt64(dr, "TotalNoOfBarcode");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }




        // 12 Sep 2020 Piyush Limbani
        public List<RetVoucherCashCounterListResponse> GetRetailExpenseVoucherCashCounterReportList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailExpenseVoucherCashCounterReportList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetVoucherCashCounterListResponse> objlst = new List<RetVoucherCashCounterListResponse>();
                decimal CashTotal = 0;
                decimal ChequeTotal = 0;
                decimal CardTotal = 0;
                decimal OnlineTotal = 0;
                decimal AdjustAmountTotal = 0;
                while (dr.Read())
                {
                    RetVoucherCashCounterListResponse obj = new RetVoucherCashCounterListResponse();
                    obj.ExpensesVoucherID = objBaseSqlManager.GetInt64(dr, "ExpensesVoucherID");
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    CashTotal += obj.Cash;
                    obj.CashTotal = CashTotal;
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    ChequeTotal += obj.Cheque;
                    obj.ChequeTotal = ChequeTotal;
                    obj.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    CardTotal += obj.Card;
                    obj.CardTotal = CardTotal;
                    obj.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    OnlineTotal += obj.Online;
                    obj.OnlineTotal = OnlineTotal;
                    obj.AdjustAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmount")), 2);
                    AdjustAmountTotal += obj.AdjustAmount;
                    obj.AdjustAmountTotal = AdjustAmountTotal;
                    obj.Remarks = objBaseSqlManager.GetTextValue(dr, "Remarks");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    obj.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    obj.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                    if (obj.ChequeDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ChequeDate1 = "";
                    }
                    else
                    {
                        obj.ChequeDate1 = obj.ChequeDate.ToString("dd/MM/yyyy");
                    }
                    obj.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFCCode");
                    obj.BankNameForCard = objBaseSqlManager.GetTextValue(dr, "BankNameForCard");
                    obj.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    obj.BankNameForOnline = objBaseSqlManager.GetTextValue(dr, "BankNameForOnline");
                    obj.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    obj.OnlinePaymentDate = objBaseSqlManager.GetDateTime(dr, "OnlinePaymentDate");
                    if (obj.OnlinePaymentDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.OnlinePaymentDate1 = "";
                    }
                    else
                    {
                        obj.OnlinePaymentDate1 = obj.OnlinePaymentDate.ToString("dd/MM/yyyy");
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateVoucherPayment(RetUpdateVoucherPayment data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateRetailVoucherPayment";
                    cmdGet.Parameters.AddWithValue("@PaymentID", data.PaymentID);
                    cmdGet.Parameters.AddWithValue("@ExpensesVoucherID", data.ExpensesVoucherID);
                    cmdGet.Parameters.AddWithValue("@BillNumber", data.BillNumber);
                    cmdGet.Parameters.AddWithValue("@Amount", data.Amount);
                    cmdGet.Parameters.AddWithValue("@GodownID", data.GodownID);
                    cmdGet.Parameters.AddWithValue("@Cheque", data.Cheque);
                    cmdGet.Parameters.AddWithValue("@BankName", data.BankName);
                    cmdGet.Parameters.AddWithValue("@BankBranch", data.BankBranch);
                    cmdGet.Parameters.AddWithValue("@ChequeNo", data.ChequeNo);
                    cmdGet.Parameters.AddWithValue("@ChequeDate", data.ChequeDate);
                    cmdGet.Parameters.AddWithValue("@IFCCode", data.IFCCode);
                    cmdGet.Parameters.AddWithValue("@Online", data.Online);
                    cmdGet.Parameters.AddWithValue("@BankNameForOnline", data.BankNameForOnline);
                    cmdGet.Parameters.AddWithValue("@UTRNumber", data.UTRNumber);
                    cmdGet.Parameters.AddWithValue("@OnlinePaymentDate", data.OnlinePaymentDate);
                    cmdGet.Parameters.AddWithValue("@Card", data.Card);
                    cmdGet.Parameters.AddWithValue("@BankNameForCard", data.BankNameForCard);
                    cmdGet.Parameters.AddWithValue("@TypeOfCard", data.TypeOfCard);
                    cmdGet.Parameters.AddWithValue("@AdjustAmount", data.AdjustAmount);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", UserID);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public List<RetVoucherCashCounterDayWiseSalesManList> GetRetVoucherCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetVoucherCashCounterDayWiseSalesManList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetVoucherCashCounterDayWiseSalesManList> objlst = new List<RetVoucherCashCounterDayWiseSalesManList>();
                decimal sumCashAmtTotal = 0;
                decimal sumChequeAmtTotal = 0;
                decimal sumCardAmtTotal = 0;
                decimal sumOnlineAmtTotal = 0;
                decimal sumAdjustAmountAmtTotal = 0;
                while (dr.Read())
                {
                    RetVoucherCashCounterDayWiseSalesManList obj = new RetVoucherCashCounterDayWiseSalesManList();
                    obj.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    obj.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    obj.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    obj.AdjustAmount = Math.Round((objBaseSqlManager.GetDecimal(dr, "AdjustAmount")), 2);
                    sumCashAmtTotal = sumCashAmtTotal + obj.Cash;
                    sumChequeAmtTotal = sumChequeAmtTotal + obj.Cheque;
                    sumCardAmtTotal = sumCardAmtTotal + obj.Card;
                    sumOnlineAmtTotal = sumOnlineAmtTotal + obj.Online;
                    sumAdjustAmountAmtTotal = sumAdjustAmountAmtTotal + obj.AdjustAmount;
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].CashTotal = sumCashAmtTotal;
                    objlst[i].ChequeTotal = sumChequeAmtTotal;
                    objlst[i].CardTotal = sumCardAmtTotal;
                    objlst[i].SignTotal = 0;
                    objlst[i].OnlineTotal = sumOnlineAmtTotal;
                    objlst[i].AdjustAmountTotal = sumAdjustAmountAmtTotal;
                    break;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        //Add By Dhruvik
        public List<RetCashCounterListResponse> GetRetChequeRetrunList(DateTime? AssignedDate, long GodownID)
        {
            decimal TotalCheque = 0;

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetChequeRetrunList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);

                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCashCounterListResponse> objlst = new List<RetCashCounterListResponse>();
                while (dr.Read())
                {
                    RetCashCounterListResponse obj = new RetCashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    DateTime UpdatedOn = GetUpdatedDateByInvoiceNumber(obj.OrderID, obj.InvoiceNumber);
                    obj.InvoiceNumber1 = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(UpdatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(UpdatedOn).ToString("yy");
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);

                    TotalCheque = TotalCheque + Convert.ToDecimal(obj.Cheque);

                    obj.ChequeReturnCharges = Math.Round((objBaseSqlManager.GetDecimal(dr, "ChequeReturnCharges")), 2);
                    obj.Remarks = objBaseSqlManager.GetTextValue(dr, "Remark");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.CashOption = objBaseSqlManager.GetTextValue(dr, "CashOption");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.BankName = objBaseSqlManager.GetTextValue(dr, "BankName");
                    obj.BankBranch = objBaseSqlManager.GetTextValue(dr, "BankBranch");
                    obj.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    obj.ChequeDate = objBaseSqlManager.GetDateTime(dr, "ChequeDate");
                    if (obj.ChequeDate == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ChequeDate1 = "";
                    }
                    else
                    {
                        obj.ChequeDate1 = obj.ChequeDate.ToString("dd/MM/yyyy");
                    }
                    obj.IFCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    obj.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");

                    if (obj.IsDelivered == true)
                    {
                        obj.IsDeliveredstr = "Delivered";
                    }
                    else
                    {
                        obj.IsDeliveredstr = "Not Delivered";
                    }
                    obj.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objlst.Add(obj);
                }

                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].TotalCheque = TotalCheque;
                }

                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetCashCounterListResponse> GetRetChequeRetrunChargeList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetChequeRetrunChargeList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);

                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCashCounterListResponse> objlst = new List<RetCashCounterListResponse>();
                while (dr.Read())
                {
                    RetCashCounterListResponse obj = new RetCashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber1 = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    obj.IsDelivered = objBaseSqlManager.GetBoolean(dr, "IsDelivered");
                    if (obj.IsDelivered == true)
                    {
                        obj.IsDeliveredstr = "Delivered";
                    }
                    else
                    {
                        obj.IsDeliveredstr = "Not Delivered";
                    }

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }
        //Add By Dhruvik


    }
}
