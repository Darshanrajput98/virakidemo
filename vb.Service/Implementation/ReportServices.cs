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
    public class ReportServices : IReportService
    {
        public List<DayWiseSalesList> GetDayWiseSalesList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseSalesList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseSalesList> objlst = new List<DayWiseSalesList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                decimal sumTCSTaxAmount = 0;
                decimal GrandNetAmount = 0;
                while (dr.Read())
                {
                    DayWiseSalesList obj = new DayWiseSalesList();
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrossAmt"), 2);
                    obj.GrandGrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandGrossAmt"), 2);
                    //obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    obj.Granddiff = Math.Round(objBaseSqlManager.GetDecimal(dr, "Granddiff"), 2);
                    obj.Discount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Discount"), 2);
                    obj.CreditedTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "CreditedTotal"), 2);
                    obj.CreditedFinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal"), 2);
                    obj.diff = Math.Round(objBaseSqlManager.GetDecimal(dr, "diff"), 2);
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

        public List<DayWiseCreditMemoList> GetDayWiseCreditMemoList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseCreditMemoList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseCreditMemoList> objlst = new List<DayWiseCreditMemoList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                while (dr.Read())
                {
                    DayWiseCreditMemoList obj = new DayWiseCreditMemoList();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.Invoice = objBaseSqlManager.GetTextValue(dr, "Invoice");
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
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
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

        public List<DayWiseTaxList> GetDayWiseTaxList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseTaxList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseTaxList> objlst = new List<DayWiseTaxList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                while (dr.Read())
                {
                    DayWiseTaxList obj = new DayWiseTaxList();
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

        public List<DayWiseSalesManList> GetDayWiseSalesManList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseSalesManList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseSalesManList> objlst = new List<DayWiseSalesManList>();
                decimal sumGrossAmtTotal = 0;
                decimal sumTaxAmtTotal = 0;
                decimal sumRoundOffTotal = 0;
                decimal sumNetAmtTotal = 0;
                decimal sumDifferenceTotal = 0;
                while (dr.Read())
                {
                    DayWiseSalesManList obj = new DayWiseSalesManList();
                    obj.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    obj.DifferenceTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "DifferenceTotal")), 2);
                    obj.GrossAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "GrossAmtTotal")), 2);
                    obj.TaxAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "TaxAmtTotal")), 2);
                    obj.RoundOffTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "RoundOffTotal")), 2);
                    obj.NetAmtTotal = Math.Round((objBaseSqlManager.GetDecimal(dr, "NetAmtTotal")), 2);
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    sumGrossAmtTotal = sumGrossAmtTotal + obj.GrossAmtTotal;
                    sumTaxAmtTotal = sumTaxAmtTotal + obj.TaxAmtTotal;
                    sumRoundOffTotal = sumRoundOffTotal + obj.RoundOffTotal;
                    sumNetAmtTotal = sumNetAmtTotal + obj.NetAmtTotal;
                    sumDifferenceTotal = sumDifferenceTotal + obj.DifferenceTotal;
                    objlst.Add(obj);
                }
                for (int i = 0; i < objlst.Count; i++)
                {
                    objlst[i].GrandGrossAmtTotal = sumGrossAmtTotal;
                    objlst[i].GrandTaxAmtTotal = sumTaxAmtTotal;
                    objlst[i].GrandRoundOffTotal = sumRoundOffTotal;
                    objlst[i].GrandNetAmtTotal = sumNetAmtTotal;
                    objlst[i].GrandDifferenceTotal = sumDifferenceTotal;
                    break;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<DeliverySheetList> GetDeliverySheetList(DateTime? AssignedDate, string VehicleNo, string TempoNo, long GodownID, int BySign)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDeliverySheetList";
                //cmdGet.CommandText = "GetDeliverySheetListNew";
                cmdGet.Parameters.AddWithValue("@TempoNo", TempoNo);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BySign", BySign);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DeliverySheetList> objlst = new List<DeliverySheetList>();
                while (dr.Read())
                {
                    DeliverySheetList obj = new DeliverySheetList();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
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

        public List<ProductWiseSalesList> GetProductWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
                while (dr.Read())
                {
                    ProductWiseSalesList obj = new ProductWiseSalesList();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");

                    //obj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    //obj.Quantity = obj.OrderQuantity - obj.ReturnedQuantity;  

                    obj.OrderTotalAmount = objBaseSqlManager.GetDecimal(dr, "OrderTotalAmount");
                    obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    obj.TotalAmount = obj.OrderTotalAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ProductWiseSalesList> GetProductWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
                while (dr.Read())
                {
                    ProductWiseSalesList obj = new ProductWiseSalesList();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.DayName = objBaseSqlManager.GetInt32(dr, "DayName");
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    obj.OrderTotalAmount = objBaseSqlManager.GetDecimal(dr, "OrderTotalAmount");
                    //obj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    //obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    //obj.Quantity = obj.OrderQuantity - obj.ReturnedQuantity;
                    //obj.TotalAmount = obj.OrderTotalAmount - obj.ReturnOrderAmount;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public decimal GetReturnQuantityDayWise(int DayName, int MonthName, int YearName, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetReturnQuantityDayWise";
                cmdGet.Parameters.AddWithValue("@DayName", DayName);
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
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

        public decimal GetReturnOrderAmountDayWise(int DayName, int MonthName, int YearName, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetReturnOrderAmountDayWise";
                cmdGet.Parameters.AddWithValue("@DayName", DayName);
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
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

        public List<CustomerListResponse> GetAllCustomerMasterList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllCustomerMasterList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> objlst = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
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

        public GetEvent GetEventDateForSameYear(long EventID, long StartYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEventDateForSameYear";
                cmdGet.Parameters.AddWithValue("@EventID", EventID);
                cmdGet.Parameters.AddWithValue("@StartYear", StartYear);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetEvent objOrder = new GetEvent();
                while (dr.Read())
                {
                    objOrder.StartDate = objBaseSqlManager.GetDateTime(dr, "StartDate");
                    objOrder.EndDate = objBaseSqlManager.GetDateTime(dr, "EndDate");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public GetEvent GetEventDateForDiffYear(long EventID, long StartYear, long EndYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEventDateForDiffYear";
                cmdGet.Parameters.AddWithValue("@EventID", EventID);
                cmdGet.Parameters.AddWithValue("@StartYear", StartYear);
                cmdGet.Parameters.AddWithValue("@EndYear", EndYear);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetEvent objOrder = new GetEvent();
                while (dr.Read())
                {
                    objOrder.StartDate = objBaseSqlManager.GetDateTime(dr, "StartDate");
                    objOrder.EndDate = objBaseSqlManager.GetDateTime(dr, "EndDate");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<SalesManWiseSalesList> GetSalesManWiseSalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSalesManWiseSalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseSalesList> objlst = new List<SalesManWiseSalesList>();
                while (dr.Read())
                {
                    SalesManWiseSalesList obj = new SalesManWiseSalesList();
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "OrderAmount"), 0);
                    obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    obj.Quantity = Math.Round(obj.OrderAmount - obj.ReturnOrderAmount, 0);
                    obj.MonthName = objBaseSqlManager.GetInt32(dr, "MonthName");
                    obj.YearName = objBaseSqlManager.GetInt32(dr, "YearName");
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.CellNo = objBaseSqlManager.GetTextValue(dr, "CellNo");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<SalesManWiseSalesList> GetSalesManWiseDailySalesList(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSalesManWiseDailySalesList";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseSalesList> objlst = new List<SalesManWiseSalesList>();
                while (dr.Read())
                {
                    SalesManWiseSalesList obj = new SalesManWiseSalesList();
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    obj.OrderAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "OrderAmount"), 0);
                    obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    obj.Quantity = Math.Round(obj.OrderAmount - obj.ReturnOrderAmount, 0);
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

        public List<DayWiseSalesList> GetDayWiseSalesListByUserID(long UserID, string Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseSalesListByUserID";
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", Date);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseSalesList> objlst = new List<DayWiseSalesList>();
                decimal sumCGST = 0;
                decimal sumSGST = 0;
                decimal sumIGST = 0;
                decimal sumTCSTaxAmount = 0;
                decimal GrandNetAmount = 0;
                while (dr.Read())
                {
                    DayWiseSalesList obj = new DayWiseSalesList();
                    obj.UserID = objBaseSqlManager.GetInt64(dr, "UserID");
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvCode = objBaseSqlManager.GetTextValue(dr, "InvCode");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "InvDate");
                    if (obj.InvDate.Value != null)
                    {
                        obj.InvoiceDate = obj.InvDate.Value.ToString("dd/MM/yyyy");
                    }
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    obj.GrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrossAmt"), 2);
                    obj.GrandGrossAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandGrossAmt"), 2);
                    //obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    obj.Granddiff = Math.Round(objBaseSqlManager.GetDecimal(dr, "Granddiff"), 2);
                    obj.Discount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Discount"), 2);
                    obj.CreditedTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "CreditedTotal"), 2);
                    obj.CreditedFinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal"), 2);
                    obj.diff = Math.Round(objBaseSqlManager.GetDecimal(dr, "diff"), 2);
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

        public List<WholesalePouchListResponse> GetPouchWiseReportList(DateTime? StartDate, DateTime? EndDate, string PouchNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPouchWiseReportList";
                cmdGet.Parameters.AddWithValue("@PouchNameID", PouchNameID);
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<WholesalePouchListResponse> objlst = new List<WholesalePouchListResponse>();
                while (dr.Read())
                {
                    WholesalePouchListResponse objPayment = new WholesalePouchListResponse();
                    objPayment.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
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

        public List<CashCounterListResponse> GetCashCounterReportList(DateTime? AssignedDate, long GodownID, int BySign)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCashCounterReportList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BySign", BySign);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CashCounterListResponse> objlst = new List<CashCounterListResponse>();
                decimal CashTotal = 0;
                decimal ChequeTotal = 0;
                decimal CardTotal = 0;
                decimal SignTotal = 0;
                decimal OnlineTotal = 0;
                decimal AdjustAmountTotal = 0;
                while (dr.Read())
                {
                    CashCounterListResponse obj = new CashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
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

        public List<BillHistoryListResponse> GetBillHistoryList(string InvoiceNumber, DateTime? FromDate, DateTime? ToDate, long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillHistoryList";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BillHistoryListResponse> objlst = new List<BillHistoryListResponse>();
                while (dr.Read())
                {
                    BillHistoryListResponse objBillHistrory = new BillHistoryListResponse();
                    objBillHistrory.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    objBillHistrory.AreaName = objBaseSqlManager.GetTextValue(dr, "Area");
                    objBillHistrory.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objBillHistrory.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
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
                    //objBillHistrory.VehicleNo = lstdata.VehicleNo;
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
                    //}
                    var lstdeliverydata = GetVehicleDetailForBillHistory(objBillHistrory.InvoiceNumber);
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

        public BillHistoryListResponse GetPaymentDetailForBillHistory(long OrderID, string InvoiceNumber, long PaymentID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPaymentDetailForBillHistory";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@PaymentID", PaymentID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                BillHistoryListResponse obj = new BillHistoryListResponse();
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

        public BillHistoryListResponse GetVehicleDetailForBillHistory(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleDetailForBillHistory";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                BillHistoryListResponse obj = new BillHistoryListResponse();
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

        public List<CashCounterDayWiseSalesManList> GetCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID, int BySign)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCashCounterDayWiseSalesManList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@BySign", BySign);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CashCounterDayWiseSalesManList> objlst = new List<CashCounterDayWiseSalesManList>();
                decimal sumCashAmtTotal = 0;
                decimal sumChequeAmtTotal = 0;
                decimal sumCardAmtTotal = 0;
                decimal sumSignAmtTotal = 0;
                decimal sumOnlineAmtTotal = 0;
                decimal sumAdjustAmountAmtTotal = 0;
                while (dr.Read())
                {
                    CashCounterDayWiseSalesManList obj = new CashCounterDayWiseSalesManList();
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

        public bool UpdatePayment(Payment data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdatePayment";
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

        public VehicleNoListResponse GetAllVehicleNoForDeliverysheetReport(DateTime AssignedDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleNoForDeliverysheetReport";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                VehicleNoListResponse objOrder = new VehicleNoListResponse();
                while (dr.Read())
                {
                    objOrder.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public string GetCustomerIDForSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long UserID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCustomerIDForSalesList2";
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
                    SalesManWiseSalesList obj = new SalesManWiseSalesList();
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

        public List<SalesManWiseSalesList> GetSalesManWiseSalesList2(string CustomerIDs, long UserID, long AreaID, long DaysofWeek)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSalesManWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@CustomerIDs", CustomerIDs);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@DaysofWeek", DaysofWeek);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseSalesList> objlst = new List<SalesManWiseSalesList>();
                while (dr.Read())
                {
                    SalesManWiseSalesList obj = new SalesManWiseSalesList();
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

        public string GetProductIDForProductWiseSalesList2(DateTime? StartDate, DateTime? EndDate, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductIDForProductWiseSalesList2";
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

        public List<ProductWiseSalesList> GetProductWiseSalesList2(DateTime StartDate1, DateTime EndDate1, string ProductIDs, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductWiseSalesList2";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate1);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate1);
                cmdGet.Parameters.AddWithValue("@ProductIDs", ProductIDs);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductWiseSalesList> objlst = new List<ProductWiseSalesList>();
                while (dr.Read())
                {
                    ProductWiseSalesList obj = new ProductWiseSalesList();
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

        // new 30-04-2019
        public decimal GetReturnQuantityMonthWise(int MonthName, int YearName, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetReturnQuantityMonthWise";
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
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


        // Purchse Report
        public List<DayWisePurchaseList> GetDayWisePurchaseList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWisePurchaseList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWisePurchaseList> objlst = new List<DayWisePurchaseList>();
                while (dr.Read())
                {
                    DayWisePurchaseList obj = new DayWisePurchaseList();
                    obj.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (obj.BillDate != null)
                    {
                        obj.BillDatestr = obj.BillDate.ToString("dd/MM/yyyy");
                    }
                    obj.BrokerName = objBaseSqlManager.GetTextValue(dr, "BrokerName");
                    obj.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "PurchaseDebitAccountType");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.PurchaseDatestr = obj.CreatedOn.ToString("dd/MM/yyyy");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.PurchaseType = "Purchase";
                    obj.AvakNumber = objBaseSqlManager.GetInt64(dr, "AvakNumber");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.VakalNumber = objBaseSqlManager.GetTextValue(dr, "VakalNumber");
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.GrossWeight = objBaseSqlManager.GetDecimal(dr, "GrossWeight");
                    obj.TareWeight = objBaseSqlManager.GetDecimal(dr, "TareWeight");
                    obj.NetWeight = objBaseSqlManager.GetDecimal(dr, "NetWeight");
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");
                    obj.Amount = Math.Round(objBaseSqlManager.GetDecimal(dr, "Amount"), 2);
                    obj.WeightPerBag = objBaseSqlManager.GetDecimal(dr, "WeightPerBag");
                    obj.RatePerBags = objBaseSqlManager.GetDecimal(dr, "RatePerBags");
                    obj.PackingChargesBag = objBaseSqlManager.GetDecimal(dr, "PackingChargesBag");
                    obj.TotalPackingCharge = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalPackingCharge"), 2);
                    obj.DiscountPer = objBaseSqlManager.GetDecimal(dr, "DiscountPer");
                    obj.DiscountAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "DiscountAmount"), 2);
                    obj.APMCPer = objBaseSqlManager.GetDecimal(dr, "APMCPer");
                    obj.APMCAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "APMCAmount"), 2);
                    obj.TotalTaxableAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalTaxableAmount"), 2);
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.CGSTTax = objBaseSqlManager.GetDecimal(dr, "CGSTTax");
                    obj.CGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "CGSTTaxAmount"), 2);
                    obj.SGSTTax = objBaseSqlManager.GetDecimal(dr, "SGSTTax");
                    obj.SGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SGSTTaxAmount"), 2);
                    obj.IGSTTax = objBaseSqlManager.GetDecimal(dr, "IGSTTax");
                    obj.IGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "IGSTTaxAmount"), 2);
                    obj.TotalTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalTaxAmount"), 2);
                    obj.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    obj.Hamali = Math.Round(objBaseSqlManager.GetDecimal(dr, "Hamali"), 2);
                    obj.Insurance = Math.Round(objBaseSqlManager.GetDecimal(dr, "Insurance"), 2);
                    obj.TransportInward = Math.Round(objBaseSqlManager.GetDecimal(dr, "TransportInward"), 2);

                    obj.TCSAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TCSAmount"), 2);

                    obj.RoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "RoundOff"), 2);
                    decimal GrandTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount"), 2);
                    if (obj.RoundOff == 0)
                    {
                        obj.GrandTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount"), 2);
                    }
                    else
                    {
                        decimal GrandTotalAmount1 = (GrandTotalAmount - obj.RoundOff);
                        obj.GrandTotalAmount = Math.Round(GrandTotalAmount1, 2);
                    }
                    if (obj.RoundOff == 0)
                    {
                        obj.NetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount"), 2);
                    }
                    else
                    {
                        decimal NetAmount = (obj.GrandTotalAmount + obj.RoundOff);
                        obj.NetAmount = Math.Round(NetAmount, 2);
                    }
                    // obj.NetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "NetAmount"), 2);
                    obj.SumAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumAmount"), 2);
                    obj.SumTotalPackingCharge = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTotalPackingCharge"), 2);
                    obj.SumHamali = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumHamali"), 2);
                    obj.SumDiscountAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumDiscountAmount"), 2);
                    obj.SumAPMCAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumAPMCAmount"), 2);
                    obj.SumCGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumCGSTTaxAmount"), 2);
                    obj.SumSGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumSGSTTaxAmount"), 2);
                    obj.SumIGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumIGSTTaxAmount"), 2);
                    obj.SumTotalTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTotalTaxAmount"), 2);
                    obj.SumTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTotalAmount"), 2);
                    obj.SumInsurance = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumInsurance"), 2);
                    obj.SumTransportInward = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTransportInward"), 2);

                    obj.SumTCSAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTCSAmount"), 2);

                    obj.SumGrandTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumGrandTotalAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);

                    obj.TDSTax = Math.Round(objBaseSqlManager.GetDecimal(dr, "TDSTax"), 2);
                    obj.TDSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TDSTaxAmount"), 2);
                    obj.SumTDSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTDSTaxAmount"), 2);
                    obj.AmountAfterTDS = Math.Round(objBaseSqlManager.GetDecimal(dr, "AmountAfterTDS"), 2);
                    obj.SumAmountAfterTDS = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumAmountAfterTDS"), 2);

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<PurchasePaidPaymentList> GetPurchasePaidPaymentList(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPurchasePaidPaymentList";
                if (PaymentDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", PaymentDate);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchasePaidPaymentList> objlst = new List<PurchasePaidPaymentList>();
                while (dr.Read())
                {
                    PurchasePaidPaymentList objPayment = new PurchasePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (strbank == "Kotak" || strbank == "KOTAK")
                        {
                            objPayment.Payment_Type = "IFT";
                        }
                        else
                        {
                            objPayment.Payment_Type = "NEFT";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = "";
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");

                    objPayment.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    objPayment.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    objPayment.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    objPayment.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    objPayment.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    string PaymentDate2 = null;
                    PaymentDate2 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("MM/dd/yyyy");
                    objPayment.PaymentDatestr = PaymentDate2;

                    objPayment.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    objPayment.BankBranchViraki = objBaseSqlManager.GetTextValue(dr, "BankBranchViraki");
                    objPayment.BankIFSCCodeViraki = objBaseSqlManager.GetTextValue(dr, "BankIFSCCodeViraki");
                    objPayment.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    objPayment.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    objPayment.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<PurchasePaidPaymentList> GetPurchasePaidPaymentListForExport(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPurchasePaidPaymentListForExport";
                if (PaymentDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", PaymentDate);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchasePaidPaymentList> objlst = new List<PurchasePaidPaymentList>();
                while (dr.Read())
                {
                    PurchasePaidPaymentList objPayment = new PurchasePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Bank_Name = objBaseSqlManager.GetTextValue(dr, "VirakiBankName");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.ByCheque = objBaseSqlManager.GetBoolean(dr, "ByCheque");
                    objPayment.ByCard = objBaseSqlManager.GetBoolean(dr, "ByCard");
                    objPayment.ByOnline = objBaseSqlManager.GetBoolean(dr, "ByOnline");
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (objPayment.ByOnline == true)
                        {
                            if (strbank == "Kotak" || strbank == "KOTAK")
                            {
                                objPayment.Payment_Type = "IFT";
                            }
                            else
                            {
                                objPayment.Payment_Type = "NEFT";
                            }
                        }
                        else if (objPayment.ByCheque == true)
                        {
                            objPayment.Payment_Type = "CHEQUE";
                        }
                        else if (objPayment.ByCard == true)
                        {
                            objPayment.Payment_Type = "CARD";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.NameAsBankAccount = objBaseSqlManager.GetTextValue(dr, "NameAsBankAccount");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 02/08/2019
        public List<PurchasePaidPaymentList> GetPurchasePaidPaymentListByCreatedOn(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPurchasePaidPaymentListByCreatedOn";
                if (CreatedOn.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchasePaidPaymentList> objlst = new List<PurchasePaidPaymentList>();
                while (dr.Read())
                {
                    PurchasePaidPaymentList objPayment = new PurchasePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (strbank == "Kotak" || strbank == "KOTAK")
                        {
                            objPayment.Payment_Type = "IFT";
                        }
                        else
                        {
                            objPayment.Payment_Type = "NEFT";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = "";
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");

                    objPayment.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    objPayment.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    objPayment.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    objPayment.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    objPayment.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    string PaymentDate2 = null;
                    PaymentDate2 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("MM/dd/yyyy");
                    objPayment.PaymentDatestr = PaymentDate2;

                    objPayment.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    objPayment.BankBranchViraki = objBaseSqlManager.GetTextValue(dr, "BankBranchViraki");
                    objPayment.BankIFSCCodeViraki = objBaseSqlManager.GetTextValue(dr, "BankIFSCCodeViraki");
                    objPayment.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    objPayment.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    objPayment.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<PurchasePaidPaymentList> GetPurchasePaidPaymentListByCreatedOnForExport(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPurchasePaidPaymentListByCreatedOnForExport";
                if (CreatedOn.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PurchasePaidPaymentList> objlst = new List<PurchasePaidPaymentList>();
                while (dr.Read())
                {
                    PurchasePaidPaymentList objPayment = new PurchasePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Bank_Name = objBaseSqlManager.GetTextValue(dr, "VirakiBankName");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.ByCheque = objBaseSqlManager.GetBoolean(dr, "ByCheque");
                    objPayment.ByCard = objBaseSqlManager.GetBoolean(dr, "ByCard");
                    objPayment.ByOnline = objBaseSqlManager.GetBoolean(dr, "ByOnline");
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (objPayment.ByOnline == true)
                        {
                            if (strbank == "Kotak" || strbank == "KOTAK")
                            {
                                objPayment.Payment_Type = "IFT";
                            }
                            else
                            {
                                objPayment.Payment_Type = "NEFT";
                            }
                        }
                        else if (objPayment.ByCheque == true)
                        {
                            objPayment.Payment_Type = "CHEQUE";
                        }
                        else if (objPayment.ByCard == true)
                        {
                            objPayment.Payment_Type = "CARD";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.NameAsBankAccount = objBaseSqlManager.GetTextValue(dr, "NameAsBankAccount");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<UnVerifyPendingPurchaseAavakList> GetAllUnVerifyPendingPurchaseAavakReport()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUnVerifyPendingPurchaseAavakReport";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UnVerifyPendingPurchaseAavakList> objlst = new List<UnVerifyPendingPurchaseAavakList>();
                while (dr.Read())
                {
                    UnVerifyPendingPurchaseAavakList obj = new UnVerifyPendingPurchaseAavakList();
                    obj.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.AvakNumber = objBaseSqlManager.GetInt64(dr, "AvakNumber");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "Party");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.VakalNumber = objBaseSqlManager.GetTextValue(dr, "VakalNumber");
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateVerifyPurcahseOrderStatus(List<UpdateVerifyPurcahseOrderStatus> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateVerifyPurcahseOrderStatus";
                        cmdGet.Parameters.AddWithValue("@PurchaseID", item.PurchaseID);
                        cmdGet.Parameters.AddWithValue("@Verify", item.Verify);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            return true;
        }

        // GST Report
        public List<ProductWiseGSTReportList> GetProductWiseSalesForGSTReport(DateTime? StartDate, DateTime? EndDate, string Tax, long TaxID, long CustomerID, long AreaID, long ProductCategoryID, long ProductID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductWiseSalesForGSTReport";
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                cmdGet.Parameters.AddWithValue("@Tax", Tax);
                cmdGet.Parameters.AddWithValue("@TaxID", TaxID);
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                cmdGet.Parameters.AddWithValue("@CategoryID", ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductWiseGSTReportList> objlst = new List<ProductWiseGSTReportList>();
                while (dr.Read())
                {
                    ProductWiseGSTReportList obj = new ProductWiseGSTReportList();
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    //  obj.Tax = objBaseSqlManager.GetTextValue(dr, "Tax");
                    obj.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    obj.TotalTaxableAmount = objBaseSqlManager.GetDecimal(dr, "TotalTaxableAmount");
                    obj.OrderTotalAmount = objBaseSqlManager.GetDecimal(dr, "OrderTotalAmount");
                    obj.ReturnTaxableAmount = objBaseSqlManager.GetDecimal(dr, "ReturnTaxableAmount");
                    obj.ReturnOrderAmount = objBaseSqlManager.GetDecimal(dr, "ReturnOrderAmount");
                    obj.FinalTaxableAmount = obj.TotalTaxableAmount - obj.ReturnTaxableAmount;
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

        public decimal GetReturnQuantityMonthWiseForGSTReport(int MonthName, int YearName, long TaxID, long ProductID, long CustomerID, long AreaID, long ProductCategoryID, long UserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetReturnQuantityMonthWiseForGSTReport";
                cmdGet.Parameters.AddWithValue("@MonthName", MonthName);
                cmdGet.Parameters.AddWithValue("@YearName", YearName);
                cmdGet.Parameters.AddWithValue("@TaxID", TaxID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
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

        public bool UpdatePurchasePayment(PurchasePayment data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdatePurchasePayment";
                    cmdGet.Parameters.AddWithValue("@PaymentID", data.PaymentID);
                    cmdGet.Parameters.AddWithValue("@PurchaseID", data.PurchaseID);
                    cmdGet.Parameters.AddWithValue("@BillNumber", data.BillNumber);
                    cmdGet.Parameters.AddWithValue("@PaymentDate", data.PaymentDate);
                    cmdGet.Parameters.AddWithValue("@Amount", data.Amount);
                    cmdGet.Parameters.AddWithValue("@GodownID", data.GodownID);
                    cmdGet.Parameters.AddWithValue("@BankID", data.BankID);
                    cmdGet.Parameters.AddWithValue("@Cheque", data.Cheque);
                    cmdGet.Parameters.AddWithValue("@ChequeNo", data.ChequeNo);
                    cmdGet.Parameters.AddWithValue("@Card", data.Card);
                    cmdGet.Parameters.AddWithValue("@TypeOfCard", data.TypeOfCard);
                    cmdGet.Parameters.AddWithValue("@Online", data.Online);
                    cmdGet.Parameters.AddWithValue("@UTRNumber", data.UTRNumber);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", data.UpdatedOn);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }



        // Expense Report
        public List<DayWiseExpenseList> GetDayWiseExpenseList(DateTime? CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDayWiseExpenseList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DayWiseExpenseList> objlst = new List<DayWiseExpenseList>();
                while (dr.Read())
                {
                    DayWiseExpenseList obj = new DayWiseExpenseList();
                    obj.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.Party = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.ExpenseTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseTypeID");
                    obj.ExpenseType = objBaseSqlManager.GetTextValue(dr, "ExpenseType");
                    obj.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (obj.BillDate != null)
                    {
                        obj.BillDatestr = obj.BillDate.ToString("dd/MM/yyyy");
                    }
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.ExpenseDatestr = obj.CreatedOn.ToString("dd/MM/yyyy");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "ExpenseDebitAccountType");
                    obj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.Rate = objBaseSqlManager.GetDecimal(dr, "Rate");
                    obj.TotalTaxableAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalTaxableAmount"), 2);
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.CGSTTax = objBaseSqlManager.GetDecimal(dr, "CGSTTax");
                    obj.CGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "CGSTTaxAmount"), 2);
                    obj.SGSTTax = objBaseSqlManager.GetDecimal(dr, "SGSTTax");
                    obj.SGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SGSTTaxAmount"), 2);
                    obj.IGSTTax = objBaseSqlManager.GetDecimal(dr, "IGSTTax");
                    obj.IGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "IGSTTaxAmount"), 2);
                    obj.TotalTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalTaxAmount"), 2);
                    obj.TCSAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TCSAmount"), 2);
                    obj.RoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "RoundOff"), 2);
                    obj.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    obj.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    obj.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    obj.TDSTax = objBaseSqlManager.GetDecimal(dr, "TDSTax");
                    obj.TDSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TDSTaxAmount"), 2);
                    obj.NetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "NetAmount"), 2);
                    obj.SumTotalTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTotalTaxAmount"), 2);
                    obj.SumCGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumCGSTTaxAmount"), 2);
                    obj.SumSGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumSGSTTaxAmount"), 2);
                    obj.SumIGSTTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumIGSTTaxAmount"), 2);
                    obj.SumTCSAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTCSAmount"), 2);
                    obj.GrandRoundOff = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandRoundOff"), 2);
                    obj.SumAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumAmount"), 2);
                    obj.SumTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTotalAmount"), 2);
                    obj.SumTDSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumTDSTaxAmount"), 2);
                    obj.SumGrandTotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "SumGrandTotalAmount"), 2);
                    obj.GrandNetAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandNetAmount"), 2);
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpensePaidPaymentList> GetExpensePaidPaymentList(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensePaidPaymentList";
                if (PaymentDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", PaymentDate);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensePaidPaymentList> objlst = new List<ExpensePaidPaymentList>();
                while (dr.Read())
                {
                    ExpensePaidPaymentList objPayment = new ExpensePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (strbank == "Kotak" || strbank == "KOTAK")
                        {
                            objPayment.Payment_Type = "IFT";
                        }
                        else
                        {
                            objPayment.Payment_Type = "NEFT";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = "";
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    objPayment.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    objPayment.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    objPayment.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    objPayment.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    string PaymentDate2 = null;
                    PaymentDate2 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("MM/dd/yyyy");
                    objPayment.PaymentDatestr = PaymentDate2;
                    objPayment.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    objPayment.BankBranchViraki = objBaseSqlManager.GetTextValue(dr, "BankBranchViraki");
                    objPayment.BankIFSCCodeViraki = objBaseSqlManager.GetTextValue(dr, "BankIFSCCodeViraki");
                    objPayment.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    objPayment.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    objPayment.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpensePaidPaymentList> GetExpensePaidPaymentListByCreatedOn(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensePaidPaymentListByCreatedOn";
                if (CreatedOn.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensePaidPaymentList> objlst = new List<ExpensePaidPaymentList>();
                while (dr.Read())
                {
                    ExpensePaidPaymentList objPayment = new ExpensePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (strbank == "Kotak" || strbank == "KOTAK")
                        {
                            objPayment.Payment_Type = "IFT";
                        }
                        else
                        {
                            objPayment.Payment_Type = "NEFT";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = "";
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");

                    objPayment.Cash = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cash")), 2);
                    objPayment.Cheque = Math.Round((objBaseSqlManager.GetDecimal(dr, "Cheque")), 2);
                    objPayment.Card = Math.Round((objBaseSqlManager.GetDecimal(dr, "Card")), 2);
                    objPayment.Online = Math.Round((objBaseSqlManager.GetDecimal(dr, "Online")), 2);
                    objPayment.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    string PaymentDate2 = null;
                    PaymentDate2 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("MM/dd/yyyy");
                    objPayment.PaymentDatestr = PaymentDate2;

                    objPayment.BankID = objBaseSqlManager.GetInt64(dr, "BankID");
                    objPayment.BankBranchViraki = objBaseSqlManager.GetTextValue(dr, "BankBranchViraki");
                    objPayment.BankIFSCCodeViraki = objBaseSqlManager.GetTextValue(dr, "BankIFSCCodeViraki");
                    objPayment.ChequeNo = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    objPayment.TypeOfCard = objBaseSqlManager.GetTextValue(dr, "TypeOfCard");
                    objPayment.UTRNumber = objBaseSqlManager.GetTextValue(dr, "UTRNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateExpensePayment(ExpensePayment data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateExpensePayment";
                    cmdGet.Parameters.AddWithValue("@PaymentID", data.PaymentID);
                    cmdGet.Parameters.AddWithValue("@ExpenseID", data.ExpenseID);
                    cmdGet.Parameters.AddWithValue("@BillNumber", data.BillNumber);
                    cmdGet.Parameters.AddWithValue("@PaymentDate", data.PaymentDate);
                    cmdGet.Parameters.AddWithValue("@Amount", data.Amount);
                    cmdGet.Parameters.AddWithValue("@GodownID", data.GodownID);
                    cmdGet.Parameters.AddWithValue("@BankID", data.BankID);
                    cmdGet.Parameters.AddWithValue("@Cheque", data.Cheque);
                    cmdGet.Parameters.AddWithValue("@ChequeNo", data.ChequeNo);
                    cmdGet.Parameters.AddWithValue("@Card", data.Card);
                    cmdGet.Parameters.AddWithValue("@TypeOfCard", data.TypeOfCard);
                    cmdGet.Parameters.AddWithValue("@Online", data.Online);
                    cmdGet.Parameters.AddWithValue("@UTRNumber", data.UTRNumber);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", data.UpdatedOn);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public List<ExpensePaidPaymentList> GetExpensePaidPaymentListForExport(DateTime PaymentDate, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensePaidPaymentListForExport";
                if (PaymentDate.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@PaymentDate", PaymentDate);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensePaidPaymentList> objlst = new List<ExpensePaidPaymentList>();
                while (dr.Read())
                {
                    ExpensePaidPaymentList objPayment = new ExpensePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Bank_Name = objBaseSqlManager.GetTextValue(dr, "VirakiBankName");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.ByCheque = objBaseSqlManager.GetBoolean(dr, "ByCheque");
                    objPayment.ByCard = objBaseSqlManager.GetBoolean(dr, "ByCard");
                    objPayment.ByOnline = objBaseSqlManager.GetBoolean(dr, "ByOnline");
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (objPayment.ByOnline == true)
                        {
                            if (strbank == "Kotak" || strbank == "KOTAK")
                            {
                                objPayment.Payment_Type = "IFT";
                            }
                            else
                            {
                                objPayment.Payment_Type = "NEFT";
                            }
                        }
                        else if (objPayment.ByCheque == true)
                        {
                            objPayment.Payment_Type = "CHEQUE";
                        }
                        else if (objPayment.ByCard == true)
                        {
                            objPayment.Payment_Type = "CARD";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.NameAsBankAccount = objBaseSqlManager.GetTextValue(dr, "NameAsBankAccount");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpensePaidPaymentList> GetExpensePaidPaymentListByCreatedOnForExport(DateTime CreatedOn, long? BankID, long? AreaID, long? SupplierID, bool ByCheque, bool ByCard, bool ByOnline)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensePaidPaymentListByCreatedOnForExport";
                if (CreatedOn.ToString("MM-dd-yyyy") == "01-01-0001")
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
                }
                else
                {
                    cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                }
                cmdGet.Parameters.AddWithValue("@ByCheque", ByCheque);
                cmdGet.Parameters.AddWithValue("@ByCard", ByCard);
                cmdGet.Parameters.AddWithValue("@ByOnline", ByOnline);
                cmdGet.Parameters.AddWithValue("@BankID", BankID);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@AreaID", AreaID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensePaidPaymentList> objlst = new List<ExpensePaidPaymentList>();
                while (dr.Read())
                {
                    ExpensePaidPaymentList objPayment = new ExpensePaidPaymentList();
                    objPayment.PaymentID = objBaseSqlManager.GetInt64(dr, "PaymentID");
                    objPayment.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    objPayment.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objPayment.Bank_Name = objBaseSqlManager.GetTextValue(dr, "VirakiBankName");
                    objPayment.Client_Code = "VIRAKIBRO";
                    objPayment.Product_Code = "NETPAY";
                    objPayment.ByCheque = objBaseSqlManager.GetBoolean(dr, "ByCheque");
                    objPayment.ByCard = objBaseSqlManager.GetBoolean(dr, "ByCard");
                    objPayment.ByOnline = objBaseSqlManager.GetBoolean(dr, "ByOnline");
                    objPayment.SupplierBankName = "";
                    objPayment.SupplierBankName = objBaseSqlManager.GetTextValue(dr, "BankName");

                    if (objPayment.SupplierBankName != "")
                    {
                        string strbank = objPayment.SupplierBankName.Substring(0, 5);
                        if (objPayment.ByOnline == true)
                        {
                            if (strbank == "Kotak" || strbank == "KOTAK")
                            {
                                objPayment.Payment_Type = "IFT";
                            }
                            else
                            {
                                objPayment.Payment_Type = "NEFT";
                            }
                        }
                        else if (objPayment.ByCheque == true)
                        {
                            objPayment.Payment_Type = "CHEQUE";
                        }
                        else if (objPayment.ByCard == true)
                        {
                            objPayment.Payment_Type = "CARD";
                        }
                    }
                    else
                    {
                        objPayment.Payment_Type = "";
                    }

                    objPayment.Payment_Ref_No = objBaseSqlManager.GetTextValue(dr, "ChequeNo");
                    string PaymentDate1 = null;
                    PaymentDate1 = objBaseSqlManager.GetDateTime(dr, "PaymentDate").ToString("dd/MM/yyyy");
                    objPayment.Payment_Date = PaymentDate1;
                    objPayment.Instrument_Date = "";
                    objPayment.Dr_Ac_No = "09572970000066";
                    objPayment.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objPayment.Bank_Code_Indicator = "M";
                    objPayment.Beneficiary_Code = "";
                    objPayment.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objPayment.Beneficiary_Bank = "";
                    objPayment.IFSCCode = objBaseSqlManager.GetTextValue(dr, "IFSCCode");
                    objPayment.AccountNumber = objBaseSqlManager.GetTextValue(dr, "AccountNumber");
                    objPayment.Location = "";
                    objPayment.Print_Location = "";
                    objPayment.Instrument_Number = "";
                    objPayment.Ben_Add1 = "";
                    objPayment.Ben_Add2 = "";
                    objPayment.Ben_Add3 = "";
                    objPayment.Ben_Add4 = "";
                    objPayment.Email = objBaseSqlManager.GetTextValue(dr, "Email");
                    objPayment.MobileNumber = objBaseSqlManager.GetTextValue(dr, "MobileNumber");
                    objPayment.Debit_Narration = "";
                    objPayment.Credit_Narration = "";
                    objPayment.Payment_Details_1 = "";
                    objPayment.Payment_Details_2 = "";
                    objPayment.Payment_Details_3 = "";
                    objPayment.Payment_Details_4 = "";
                    objPayment.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objPayment.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objPayment.BillDate != Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.BillDatestr = Convert.ToDateTime(objPayment.BillDate).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        objPayment.BillDatestr = null;
                    }
                    objPayment.BillAmount = objBaseSqlManager.GetDecimal(dr, "BillAmount");
                    objPayment.Deductions = "";
                    objPayment.NetAmount = objBaseSqlManager.GetDecimal(dr, "NetAmount");
                    objPayment.Enrichment_6 = "";
                    objPayment.Enrichment_7 = "";
                    objPayment.Enrichment_8 = "";
                    objPayment.Enrichment_9 = "";
                    objPayment.Enrichment_10 = "";
                    objPayment.Enrichment_11 = "";
                    objPayment.Enrichment_12 = "";
                    objPayment.Enrichment_13 = "";
                    objPayment.Enrichment_14 = "";
                    objPayment.Enrichment_15 = "";
                    objPayment.Enrichment_16 = "";
                    objPayment.Enrichment_17 = "";
                    objPayment.Enrichment_18 = "";
                    objPayment.Enrichment_19 = "";
                    objPayment.Enrichment_20 = "";
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.NameAsBankAccount = objBaseSqlManager.GetTextValue(dr, "NameAsBankAccount");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 24 Aug 2020 Piyush Limbani
        public List<ExpenseItemWiseReportList> GetExpenseItemWiseReportList(DateTime? From, DateTime? To, long SupplierID, long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpenseItemWiseReportList";
                cmdGet.Parameters.AddWithValue("@From", From);
                cmdGet.Parameters.AddWithValue("@To", To);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpenseItemWiseReportList> objlst = new List<ExpenseItemWiseReportList>();
                while (dr.Read())
                {
                    ExpenseItemWiseReportList obj = new ExpenseItemWiseReportList();
                    obj.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (obj.BillDate == Convert.ToDateTime("10/10/2014") || obj.BillDate == null)
                    {
                        obj.BillDatestr = "";
                    }
                    else
                    {
                        obj.BillDatestr = obj.BillDate.ToString("dd/MM/yyyy");
                    }
                    obj.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.Quantity = objBaseSqlManager.GetInt64(dr, "Quantity");
                    obj.Rate = objBaseSqlManager.GetDecimal(dr, "Rate");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "ExpenseCreatedOn");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 12 Sep 2020 Piyush Limbani
        public List<VoucherCashCounterListResponse> GetWholesaleExpenseVoucherCashCounterReportList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleExpenseVoucherCashCounterReportList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VoucherCashCounterListResponse> objlst = new List<VoucherCashCounterListResponse>();
                decimal CashTotal = 0;
                decimal ChequeTotal = 0;
                decimal CardTotal = 0;
                decimal OnlineTotal = 0;
                decimal AdjustAmountTotal = 0;
                while (dr.Read())
                {
                    VoucherCashCounterListResponse obj = new VoucherCashCounterListResponse();
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

        public bool UpdateVoucherPayment(UpdateVoucherPayment data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateWholesaleVoucherPayment";
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

        public List<VoucherCashCounterDayWiseSalesManList> GetVoucherCashCounterDayWiseSalesManList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVoucherCashCounterDayWiseSalesManList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VoucherCashCounterDayWiseSalesManList> objlst = new List<VoucherCashCounterDayWiseSalesManList>();
                decimal sumCashAmtTotal = 0;
                decimal sumChequeAmtTotal = 0;
                decimal sumCardAmtTotal = 0;
                decimal sumOnlineAmtTotal = 0;
                decimal sumAdjustAmountAmtTotal = 0;
                while (dr.Read())
                {
                    VoucherCashCounterDayWiseSalesManList obj = new VoucherCashCounterDayWiseSalesManList();
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
                    objlst[i].OnlineTotal = sumOnlineAmtTotal;
                    objlst[i].AdjustAmountTotal = sumAdjustAmountAmtTotal;
                    break;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 14 June,2021 Sonal Gandhi
        public List<OnlineOrder> GetOnlineOrderProductList(DateTime? CreatedOn, bool IsConfirm)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOnlineOrderProductList";
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                cmdGet.Parameters.AddWithValue("@IsConfirm", IsConfirm);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OnlineOrder> objlst = new List<OnlineOrder>();

                while (dr.Read())
                {
                    OnlineOrder obj = new OnlineOrder();
                    obj.OnlineOrderID = objBaseSqlManager.GetInt64(dr, "OnlineOrderID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    obj.ContactNo = objBaseSqlManager.GetTextValue(dr, "CellNo1");
                    obj.Email = objBaseSqlManager.GetTextValue(dr, "Email1");
                    obj.InvDate = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    obj.OnlineGrandAmount = objBaseSqlManager.GetDecimal(dr, "OnlineGrandAmount");
                    obj.OrderDate = Convert.ToDateTime(obj.InvDate);
                    obj.IsConfirm = objBaseSqlManager.GetBoolean(dr, "IsConfirm");

                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 14 June,2021 Sonal Gandhi
        public List<OnlineOrderQty> ViewBillWiseOnlineOrderDetails(long OnlineOrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "ViewBillWiseOnlineOrderDetails";
                cmdGet.Parameters.AddWithValue("@OnlineOrderID", OnlineOrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OnlineOrderQty> objlst = new List<OnlineOrderQty>();

                while (dr.Read())
                {
                    OnlineOrderQty obj = new OnlineOrderQty();
                    OnlineOrder objOrder = new OnlineOrder();
                    obj.OnlineOrderID = objBaseSqlManager.GetInt64(dr, "OnlineOrderID");
                    obj.OnlineOrderQtyID = objBaseSqlManager.GetInt64(dr, "OnlineOrderQtyID");
                    obj.OnlineProductQtyID = objBaseSqlManager.GetInt64(dr, "OnlineProductQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.Packing = objBaseSqlManager.GetTextValue(dr, "Packing");
                    obj.OnlineProductPrice = objBaseSqlManager.GetDecimal(dr, "OnlineProductPrice");
                    obj.OnlineOrderQuantity = objBaseSqlManager.GetDecimal(dr, "OnlineOrderQty");
                    obj.OnlineTotalAmount = objBaseSqlManager.GetDecimal(dr, "OnlineTotalAmount");
                    obj.OnlineGrandAmount = objBaseSqlManager.GetDecimal(dr, "OnlineGrandAmount");

                    objOrder.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objOrder.CustomerNumber = objBaseSqlManager.GetInt64(dr, "CustomerNumber");
                    objOrder.ContactNo = objBaseSqlManager.GetTextValue(dr, "CellNo1");
                    objOrder.Email = objBaseSqlManager.GetTextValue(dr, "Email1");
                    objOrder.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objOrder.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objOrder.CustomerNote = objBaseSqlManager.GetTextValue(dr, "Note");
                    obj.OnlineOrderDetails = objOrder;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public OnlineOrderViewModel GetOnlineOrderDetailsByOnlineOrderID(long OnlineOrderId)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OnlineOrderId", OnlineOrderId);
                cmdGet.CommandText = "GetOnlineOrderDetailsByOnlineOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                OnlineOrderViewModel objM = new OnlineOrderViewModel();
                List<OnlineOrderQtyViewModel> lstOrderQty = new List<OnlineOrderQtyViewModel>();
                while (dr.Read())
                {
                    objM.DeActiveCustomer = objBaseSqlManager.GetBoolean(dr, "DeActiveCustomer");
                    objM.OnlineOrderID = objBaseSqlManager.GetInt64(dr, "OnlineOrderID");
                    objM.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objM.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objM.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objM.Tax = objBaseSqlManager.GetTextValue(dr, "Tax");
                    objM.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objM.IsConfirm = objBaseSqlManager.GetBoolean(dr, "IsConfirm");
                    objM.GSTNumber = objBaseSqlManager.GetTextValue(dr, "GSTNumber");

                    OnlineOrderQtyViewModel obj = new OnlineOrderQtyViewModel();
                    obj.OnlineOrderQtyID = objBaseSqlManager.GetInt64(dr, "OnlineOrderQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.OnlineProductQtyID = objBaseSqlManager.GetInt64(dr, "OnlineProductQtyID");
                    obj.QtyTax = objBaseSqlManager.GetInt32(dr, "QtyTax");
                    // obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "QtyTaxAmt");
                    obj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");//Convert.ToDecimal(string.Format("{0:0.00}", objBaseSqlManager.GetDecimal(dr, "Quantity"))); 
                    obj.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                    obj.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    obj.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "Total"), 2);
                    obj.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.BaseRate = objBaseSqlManager.GetDecimal(dr, "BaseRate");
                    obj.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    obj.CategoryTypeID = objBaseSqlManager.GetInt32(dr, "CategoryTypeID");


                    lstOrderQty.Add(obj);
                }
                objM.lstOrderQty = lstOrderQty;
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        public bool UpdateOnlineOrderIsConfirm(long OnlineOrderId)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OnlineOrderId", OnlineOrderId);
                cmdGet.CommandText = "UpdateOnlineOrderIsConfirm";
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        //Add By Dhruvik
        public List<CashCounterListResponse> GetCheckRetrunEntryList(DateTime? AssignedDate, long GodownID)
        {
            decimal TotalCheque = 0;

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCheckRetrunEntryList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CashCounterListResponse> objlst = new List<CashCounterListResponse>();
                while (dr.Read())
                {
                    CashCounterListResponse obj = new CashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
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

        public List<CashCounterListResponse> GetRetrunChargeList(DateTime? AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetrunChargeList";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);

                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CashCounterListResponse> objlst = new List<CashCounterListResponse>();
                while (dr.Read())
                {
                    CashCounterListResponse obj = new CashCounterListResponse();
                    obj.Customer = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.Area = objBaseSqlManager.GetTextValue(dr, "Area");
                    obj.VehicleNo1 = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
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
