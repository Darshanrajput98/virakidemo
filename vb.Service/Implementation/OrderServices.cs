using vb.Data;
using vb.Data.Model;
using System.Data.Entity;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using vb.Data.ViewModel;
using System.Linq;
using vb.Service.Common;
using System;
using System.Configuration;

namespace vb.Service
{
    public class OrderServices : IOrderService
    {
        public List<string> GetTaxForCustomerByTextChange(string CustomerName)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetActiveCustomersByName";
                cmdGet.Parameters.AddWithValue("@CustomerName", CustomerName);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<string> customername = new List<string>();
                while (dr.Read())
                {
                    customername.Add(objBaseSqlManager.GetTextValue(dr, "CustomerName"));
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return customername;
            }
        }

        public GetTax GetTaxForCustomerNumber(long CustomerNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxForCustomerNumber";
                cmdGet.Parameters.AddWithValue("@CustomerNumber", CustomerNumber.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetTax objOrder = new GetTax();
                while (dr.Read())
                {
                    objOrder.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objOrder.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objOrder.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objOrder.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objOrder.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objOrder.IsTCSApplicable = objBaseSqlManager.GetBoolean(dr, "IsTCSApplicable");
                    objOrder.GSTNumber = objBaseSqlManager.GetTextValue(dr, "GSTNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public long AddOrder(OrderViewModel data)
        {

            Order_Mst obj = new Order_Mst();
            obj.OrderID = data.OrderID;
            obj.CustomerID = data.CustomerID;
            obj.ShipTo = data.ShipTo;
            obj.DeliveryDate = data.DeliveryDate;
            obj.Tax = data.Tax;
            obj.OrderRef = data.OrderRef;
            obj.IsDelete = false;
            obj.RemotePrint = true;
            obj.PendingDelivery = true;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.IsTCSApplicable = data.IsTCSApplicable;

            // 30 March, 2021 Sonal Gandhi
            int invoiceRowNumber = 0;
            if (string.IsNullOrEmpty(data.GSTNumber))
            {
                invoiceRowNumber = 15;
            }
            else
            {
                invoiceRowNumber = 10;
            }

            decimal TCSTax = 0;
            if (obj.IsTCSApplicable == true)
            {
                //if (data.GSTNumber != null && data.GSTNumber != "")
                if (string.IsNullOrEmpty(data.GSTNumber))
                {
                    // Get Tax With Out GST
                    TCSTax = GetTCSTaxWithOutGSTNumber();
                }
                else
                {
                    // Get Tax With GST
                    TCSTax = GetTCSTaxWithGSTNumber();
                }
            }
            else
            {
                TCSTax = 0;
            }
            if (obj.TotalDiscount != null)
            {
                obj.TotalDiscount = data.TotalDiscount;
            }
            else
            {
                obj.TotalDiscount = Convert.ToDecimal("0.00");
            }
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.OnlineOrderID = data.OnlineOrderID;

            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.OrderID == 0)
                {
                    context.Order_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.OrderID > 0)
                {
                    try
                    {
                        string typevalue = "1,2";
                        foreach (var Typevalue in typevalue.Split(','))
                        {
                            long typevaluel = Convert.ToInt64(Typevalue);
                            string taxtype = "0,5,12,18,28";
                            string[] taxarr = taxtype.Split(',');
                            foreach (var taxvalue in taxarr)
                            {
                                decimal taxper = Convert.ToDecimal(taxvalue);
                                var orderqtylst = data.lstOrderQty.Where(x => x.Tax == taxper && x.CategoryTypeID == typevaluel).OrderBy(y => y.ProductName).ToList();
                                int i = 0;
                                string InvoiceNo = "";
                                decimal InvoiceTotal = 0;
                                decimal TCSTaxAmount = 0;
                                decimal GrandInvoiceTotal = 0;
                                foreach (var item in orderqtylst)
                                {
                                    OrderQty_Mst objOrderQty = new OrderQty_Mst();
                                    objOrderQty.OrderID = obj.OrderID;
                                    objOrderQty.OrderQtyID = item.OrderQtyID;
                                    objOrderQty.ProductID = item.ProductID;
                                    objOrderQty.Quantity = item.Quantity;
                                    objOrderQty.SaleRate = item.SaleRate;
                                    objOrderQty.BaseRate = item.BaseRate;
                                    objOrderQty.BillDiscount = item.BillDiscount;
                                    objOrderQty.Total = item.Total;
                                    objOrderQty.Tax = item.Tax;
                                    objOrderQty.FinalTotal = item.FinalTotal;
                                    objOrderQty.TCSTax = TCSTax;
                                    objOrderQty.CategoryTypeID = item.CategoryTypeID;


                                    // objOrderQty.SlabForGST = item.SlabForGST;



                                    objOrderQty.IsDelete = false;
                                    objOrderQty.PendingDelivery = true;
                                    if (objOrderQty.OrderQtyID == 0)
                                    {
                                        if (i == 0 || (i % invoiceRowNumber == 0))
                                        {
                                            if (i != 0)
                                            {
                                                //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                                TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                                GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                                UpdateInvoiceTotal(InvoiceTotal, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                                // UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                                InvoiceTotal = 0;
                                            }
                                            InvoiceNo = Getlastorder();
                                        }
                                        objOrderQty.InvoiceNumber = InvoiceNo;
                                        objOrderQty.CreatedBy = data.CreatedBy;
                                        objOrderQty.CreatedOn = data.CreatedOn;
                                        objOrderQty.UpdatedBy = data.UpdatedBy;
                                        objOrderQty.UpdatedOn = data.UpdatedOn;
                                        InvoiceTotal += item.FinalTotal;
                                        context.OrderQty_Mst.Add(objOrderQty);
                                        context.SaveChanges();
                                    }
                                    i = i + 1;
                                    if (i == orderqtylst.Count)
                                    {
                                        //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                        TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                        GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                        UpdateInvoiceTotal(InvoiceTotal, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                        // UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);                                     
                                    }
                                }
                            }

                            List<decimal> lstdeci = new List<decimal>();
                            foreach (var txtitem in taxarr)
                            {
                                decimal taxper = Convert.ToDecimal(txtitem);
                                lstdeci.Add(taxper);
                            }
                            var orderqtylstNottax = data.lstOrderQty.Where(x => !lstdeci.Contains(x.Tax) && x.CategoryTypeID == typevaluel).OrderBy(y => y.ProductName).ToList();
                            if (orderqtylstNottax.Count > 0)
                            {
                                int i = 0;
                                string InvoiceNo = "";
                                decimal InvoiceTotal = 0;
                                decimal TCSTaxAmount = 0;
                                decimal GrandInvoiceTotal = 0;
                                foreach (var item in orderqtylstNottax)
                                {
                                    OrderQty_Mst objOrderQty = new OrderQty_Mst();
                                    objOrderQty.OrderID = obj.OrderID;
                                    objOrderQty.OrderQtyID = item.OrderQtyID;
                                    objOrderQty.ProductID = item.ProductID;
                                    objOrderQty.Quantity = item.Quantity;
                                    objOrderQty.SaleRate = item.SaleRate;
                                    objOrderQty.BaseRate = item.BaseRate;
                                    objOrderQty.BillDiscount = item.BillDiscount;
                                    objOrderQty.Total = item.Total;
                                    objOrderQty.Tax = item.Tax;
                                    objOrderQty.FinalTotal = item.FinalTotal;
                                    objOrderQty.TCSTax = TCSTax;
                                    objOrderQty.IsDelete = false;
                                    objOrderQty.PendingDelivery = true;
                                    if (objOrderQty.OrderQtyID == 0)
                                    {
                                        if (i == 0 || (i % invoiceRowNumber == 0))
                                        {
                                            if (i != 0)
                                            {
                                                //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                                TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                                GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                                UpdateInvoiceTotal(InvoiceTotal, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                                // UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                                InvoiceTotal = 0;
                                            }
                                            InvoiceNo = Getlastorder();
                                        }
                                        objOrderQty.InvoiceNumber = InvoiceNo;
                                        objOrderQty.CreatedBy = data.CreatedBy;
                                        objOrderQty.CreatedOn = data.CreatedOn;
                                        objOrderQty.UpdatedBy = data.UpdatedBy;
                                        objOrderQty.UpdatedOn = data.UpdatedOn;
                                        InvoiceTotal += item.FinalTotal;
                                        context.OrderQty_Mst.Add(objOrderQty);
                                        context.SaveChanges();
                                    }
                                    i = i + 1;
                                    if (i == orderqtylstNottax.Count)
                                    {
                                        // TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                        TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                        GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                        UpdateInvoiceTotal(InvoiceTotal, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                        //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                    }
                                }
                            }
                        }
                        return obj.OrderID;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            Order_Mst data1 = context.Order_Mst.Where(x => x.OrderID == obj.OrderID).FirstOrDefault();
                            if (data != null)
                            {
                                context.Order_Mst.Remove(data1);
                                context.SaveChanges();
                                return 0;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                }

                else
                {
                    return 0;
                }
            }
        }


        private void UpdateInvoiceTotal(decimal InvoiceTotal, decimal TCSTaxAmount, decimal GrandInvoiceTotal, string InvoiceNo, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateInvoiceTotal";
                cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmdGet.Parameters.AddWithValue("@InvoiceTotal", InvoiceTotal);
                cmdGet.Parameters.AddWithValue("@TCSTaxAmount", TCSTaxAmount);
                cmdGet.Parameters.AddWithValue("@GrandInvoiceTotal", GrandInvoiceTotal);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }


        //private void UpdateInvoiceTotal(decimal InvoiceTotal, string InvoiceNo, long OrderID)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "UpdateInvoiceTotal";
        //    cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
        //    cmdGet.Parameters.AddWithValue("@InvoiceTotal", InvoiceTotal);
        //    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
        //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //    objBaseSqlManager.ForceCloseConnection();
        //}





        private string Getlastorder()
        {
            var lstdata = GetLastInvoiceNumber();
            string InvoiceNumber = "";
            //  string FinancialYearString = ConfigurationManager.AppSettings["FinancialYearString"];
            string FinancialYearString = "/" + DateTimeExtensions.FromFinancialYear(DateTime.Now).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(DateTime.Now).ToString("yy");
            if (!string.IsNullOrEmpty(lstdata.InvoiceNumber))
            {
                string number = lstdata.InvoiceNumber.Substring(1, 7);
                long incr = Convert.ToInt64(number) + 1;
                InvoiceNumber = "W" + incr.ToString().PadLeft(7, '0') + FinancialYearString;
            }
            else
            {
                InvoiceNumber = "W0000001" + FinancialYearString;
            }
            return InvoiceNumber;
        }

        public OrderListResponse GetLastInvoiceNumber()
        {
            OrderListResponse objOrder = new OrderListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastInvoiceNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return objOrder;
        }



        public decimal GetTCSTaxWithGSTNumber()
        {
            decimal TaxWithGST = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTCSTaxWithGSTNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TaxWithGST = objBaseSqlManager.GetDecimal(dr, "TaxWithGST");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return TaxWithGST;
        }

        public decimal GetTCSTaxWithOutGSTNumber()
        {
            decimal TaxWithOutGST = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTCSTaxWithOutGSTNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TaxWithOutGST = objBaseSqlManager.GetDecimal(dr, "TaxWithOutGST");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return TaxWithOutGST;
        }






        public string CreateCreditMemo(List<OrderQtyList> data, long SessionUserID)
        {
            string CreditMemoNumber = "";
            string InvNum = "";
            long ReturnOrderID = 0;
            int cnt = 0;
            data = data.OrderByDescending(x => x.InvoiceNumber.Substring(2, 6)).ToList();
            string CreditMemoNo = "";
            decimal TCSTax = 0;
            foreach (var item in data)
            {

                ReturnOrder_Mst om = new ReturnOrder_Mst();
                if (InvNum != item.InvoiceNumber)
                {
                    InvNum = item.InvoiceNumber;
                    CreditMemoNo = GetlastCreditMemoNo();
                    om.CreditMemoNumber = CreditMemoNo;
                    om.ReturnOrderID = item.ReturnOrderID;
                    om.OrderID = item.OrderID;
                    om.InvoiceNumber = item.InvoiceNumber;
                    om.CustomerID = item.CustomerID;
                    om.CreatedBy = SessionUserID;
                    om.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    om.IsDelete = false;

                    // 08 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not
                    om.IsTCSApplicable = item.IsTCSApplicable;
                    if (om.IsTCSApplicable == true)
                    {
                        // Get TCS Tax 
                        TCSTax = GetTCSTaxByInvoiceNumber(item.OrderID, item.InvoiceNumber);
                    }
                    else
                    {
                        TCSTax = 0;
                    }
                    // 08 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not

                    om.ReferenceNumber = item.ReferenceNumber;
                    using (VirakiEntities context = new VirakiEntities())
                    {
                        context.ReturnOrder_Mst.Add(om);
                        context.SaveChanges();
                    }
                    ReturnOrderID = om.ReturnOrderID;
                }
                else
                {
                    om.CreditMemoNumber = CreditMemoNo;
                    cnt = 1;
                }


                if (ReturnOrderID > 0)
                {
                    ReturnOrderQnty_Mst obj = new ReturnOrderQnty_Mst();
                    obj.ReturnOrderID = ReturnOrderID;
                    obj.OrderQtyID = item.OrderQtyID;
                    obj.ProductID = item.ProductID;
                    obj.CategoryTypeID = item.CategoryTypeID;
                    obj.ReturnedQuantity = item.ReturnedQuantity;
                    obj.ReturnedSaleRate = item.ReturnedSaleRate;
                    obj.BillDiscount = item.BillDiscount;
                    obj.CreditedFinalTotal = item.CreditedFinalTotal;
                    obj.TaxReversed = item.Tax;

                    // order created on (Order_Mst)
                    obj.OrderDate = item.CreatedOn;

                    // 08 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not
                    obj.TCSTax = TCSTax;
                    // 08 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not

                    obj.CreatedBy = SessionUserID;
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = SessionUserID;
                    obj.UpdatedOn = DateTime.Now;
                    using (VirakiEntities context = new VirakiEntities())
                    {
                        context.ReturnOrderQnty_Mst.Add(obj);
                        context.SaveChanges();
                        if (cnt == 0)
                        {
                            if (CreditMemoNumber == "")
                            {
                                CreditMemoNumber = om.CreditMemoNumber; // +"-" + obj.OrderID.ToString();
                            }
                            else
                            {
                                CreditMemoNumber = CreditMemoNumber + "," + om.CreditMemoNumber; // +"-" + obj.OrderID.ToString();
                            }
                        }
                        else
                        {
                            cnt = 0;
                        }
                    }
                }
            }

            // 08 April 2021 Piyush Limbani Update ,TCSTaxAmount,GrandCreditedTotal
            ModelCreditedTotal CreditedDetail = GetCreditNoteTotalByCreditMemoNumber(CreditMemoNumber);
            if (CreditedDetail.ReturnOrderID != 0)
            {
                decimal TCSTaxAmount = ((CreditedDetail.CreditedTotal * CreditedDetail.TCSTax) / 100);
                decimal GrandCreditedTotal = TCSTaxAmount + CreditedDetail.CreditedTotal;
                UpdateCreditedTotal(CreditedDetail.CreditedTotal, TCSTaxAmount, GrandCreditedTotal, CreditedDetail.ReturnOrderID);
            }
            // 08 April 2021 Piyush Limbani

            return CreditMemoNumber;
        }

        // 08 April 2021 Piyush Limbani
        public decimal GetTCSTaxByInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            decimal TCSTax = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTCSTaxByInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TCSTax = objBaseSqlManager.GetDecimal(dr, "TCSTax");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return TCSTax;
        }

        // 08 April 2021 Piyush Limbani
        public ModelCreditedTotal GetCreditNoteTotalByCreditMemoNumber(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditNoteTotalByCreditMemoNumber";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ModelCreditedTotal obj = new ModelCreditedTotal();
                while (dr.Read())
                {
                    obj.CreditedTotal = objBaseSqlManager.GetDecimal(dr, "CreditedTotal");
                    obj.ReturnOrderID = objBaseSqlManager.GetInt64(dr, "ReturnOrderID");
                    obj.TCSTax = objBaseSqlManager.GetDecimal(dr, "TCSTax");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        private void UpdateCreditedTotal(decimal CreditedTotal, decimal TCSTaxAmount, decimal GrandCreditedTotal, long ReturnOrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateCreditedTotal";
                cmdGet.Parameters.AddWithValue("@ReturnOrderID", ReturnOrderID);
                cmdGet.Parameters.AddWithValue("@CreditedTotal", CreditedTotal);
                cmdGet.Parameters.AddWithValue("@TCSTaxAmount", TCSTaxAmount);
                cmdGet.Parameters.AddWithValue("@GrandCreditedTotal", GrandCreditedTotal);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }
        // 08 April 2021 Piyush Limbani





        private string GetlastCreditMemoNo()
        {
            var lstdata = GetLastCreditMemoNumber();
            //string FinancialYearString = ConfigurationManager.AppSettings["FinancialYearString"];
            string FinancialYearString = "/" + DateTimeExtensions.FromFinancialYear(DateTime.Now).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(DateTime.Now).ToString("yy");
            string CreditMemoNumber = "";
            if (!string.IsNullOrEmpty(lstdata.CreditMemoNumber))
            {
                string number = lstdata.CreditMemoNumber.Substring(3, 5);
                long incr = Convert.ToInt64(number) + 1;
                CreditMemoNumber = "CMW" + incr.ToString().PadLeft(5, '0') + FinancialYearString;
            }
            else
            {
                CreditMemoNumber = "CMW00001" + FinancialYearString;
            }
            return CreditMemoNumber;
        }

        public OrderQtyList GetLastCreditMemoNumber()
        {
            OrderQtyList objOrder = new OrderQtyList();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastCreditMemoNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.CreditMemoNumber = objBaseSqlManager.GetTextValue(dr, "CreditMemoNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return objOrder;
        }

        private List<OrderQtyList> GetReturnedQtyDetailsByOrderQtyID(long OrderQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetReturnedQtyDetailsByOrderQtyID";
                cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyList> lstOrderQty = new List<OrderQtyList>();
                while (dr.Read())
                {
                    OrderQtyList obj = new OrderQtyList();
                    obj.ReturnedOrderQtyID = objBaseSqlManager.GetInt64(dr, "ReturnedOrderQtyID");
                    obj.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    obj.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    obj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    obj.CreditMemoNumber = objBaseSqlManager.GetTextValue(dr, "CreditMemoNumber");
                    lstOrderQty.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstOrderQty;
            }
        }

        public List<GetUnitRate> GetAutoCompleteProduct(long Prefix)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteProduct";
                cmdGet.Parameters.AddWithValue("@ProductID", Prefix.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetUnitRate> objlst = new List<GetUnitRate>();
                while (dr.Read())
                {
                    GetUnitRate objOrder = new GetUnitRate();
                    objOrder.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    objOrder.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objOrder.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objlst.Add(objOrder);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public OrderListResponse GetLastOrderID()
        {
            OrderListResponse objOrder = new OrderListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return objOrder;
        }

        public List<CustomerListResponse> GetActiveCustomerName(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetActiveCustomerName";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<CustomerListResponse> lstCustomer = new List<CustomerListResponse>();
                while (dr.Read())
                {
                    CustomerListResponse objCustomer = new CustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    lstCustomer.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCustomer;
            }
        }

        public List<OrderListResponse> GetAllOrderQtyList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllOrderQtyList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderListResponse> objlst = new List<OrderListResponse>();

                while (dr.Read())
                {
                    OrderListResponse objOrder = new OrderListResponse();
                    objOrder.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objOrder.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objOrder.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objOrder.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objOrder.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objOrder.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objOrder.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objlst.Add(objOrder);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ProductListResponse> GetAllProductName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllProductName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductListResponse> lstPeoduct = new List<ProductListResponse>();
                while (dr.Read())
                {
                    ProductListResponse objProduct = new ProductListResponse();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    lstPeoduct.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public GetSellPrice GetAutoCompleteSellPrice(long ProductID, decimal Quantity, string Tax)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteSellPrice";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID.ToString());
                cmdGet.Parameters.AddWithValue("@Quantity", Quantity.ToString());
                cmdGet.Parameters.AddWithValue("@Tax", Tax.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetSellPrice objOrder = new GetSellPrice();
                while (dr.Read())
                {
                    objOrder.SellPrice = objBaseSqlManager.GetDecimal(dr, "SellPrice");
                    objOrder.SlabForGST = objBaseSqlManager.GetDecimal(dr, "SlabForGST");
                    //if (Quantity < objOrder.SlabForGST)
                    //{
                    //    objOrder.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    //}
                    //else if (Quantity > objOrder.SlabForGST)
                    //{
                    //    objOrder.Tax = 0;
                    //}
                    
                    if (objOrder.SlabForGST != 0)
                    {
                        if (objOrder.SlabForGST >= Quantity)
                        {
                            objOrder.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                        }
                        else
                        {
                            objOrder.Tax = 0;
                        }
                    }
                    else
                    {
                        objOrder.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    }

                    objOrder.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public GetTax GetTaxForCustomer(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxForCustomer";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetTax objOrder = new GetTax();
                while (dr.Read())
                {
                    objOrder.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objOrder.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objOrder.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<OrderListResponse> GetAllBillWiseOrderList(OrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllBillWiseOrderList";
                cmdGet.Parameters.AddWithValue("@ProductID", model.ProductID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", model.ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@AreaID", model.AreaID);
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
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
                List<OrderListResponse> objlst = new List<OrderListResponse>();
                while (dr.Read())
                {
                    OrderListResponse objPayment = new OrderListResponse();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");

                    objPayment.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");

                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderListResponse> GetAllOrderList(OrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllOrderList";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
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
                List<OrderListResponse> objlst = new List<OrderListResponse>();
                while (dr.Read())
                {
                    OrderListResponse objPayment = new OrderListResponse();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    // objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.FinalTotal = GetWholesaleOrderTotal(objPayment.OrderID);
                    objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        public decimal GetWholesaleOrderTotal(long OrderID)
        {
            decimal FinalTotal = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleOrderTotal";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    FinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "FinalTotal"), 2);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return FinalTotal;
            }
        }


        public List<OrderListResponse> GetAllReturnedOrderList(OrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllReturnedOrderList";
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
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
                List<OrderListResponse> objlst = new List<OrderListResponse>();
                while (dr.Read())
                {
                    OrderListResponse objPayment = new OrderListResponse();
                    objPayment.ReturnOrderID = objBaseSqlManager.GetInt64(dr, "ReturnOrderID");
                    objPayment.CreditMemoNumber = objBaseSqlManager.GetTextValue(dr, "CreditMemoNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyList> GetBillWiseCreditMemoInvoiceForOrder(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillWiseCreditMemoInvoiceForOrder";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyList> objlst = new List<OrderQtyList>();
                while (dr.Read())
                {
                    OrderQtyList objPayment = new OrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyList> GetCreditMemoInvoiceForOrder(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditMemoInvoiceForOrder";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyList> objlst = new List<OrderQtyList>();
                while (dr.Read())
                {
                    OrderQtyList objPayment = new OrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyList> GetBillWiseInvoiceForOrder(string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillWiseInvoiceForOrder";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyList> objlst = new List<OrderQtyList>();
                while (dr.Read())
                {
                    OrderQtyList objPayment = new OrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                    objPayment.LessAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "LessAmount"), 2);
                    objPayment.SaleRate = Math.Round(objBaseSqlManager.GetDecimal(dr, "SaleRate"), 2);
                    objPayment.BillDiscount = Math.Round(objBaseSqlManager.GetDecimal(dr, "BillDiscount"), 2);
                    objPayment.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "Total"), 2);
                    objPayment.Tax = Math.Round(objBaseSqlManager.GetDecimal(dr, "Tax"), 2);
                    objPayment.TaxAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmt"), 2);
                    objPayment.FinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "FinalTotal"), 2);
                    objPayment.GrandTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandTotal"), 2);
                    objPayment.TCSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount"), 2);
                    objPayment.InvoiceTotal = Math.Round(objPayment.GrandTotal + objPayment.TCSTaxAmount);
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyList> GetInvoiceForOrder(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForOrder";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyList> objlst = new List<OrderQtyList>();
                decimal sumTCSTaxAmount = 0;
                while (dr.Read())
                {
                    OrderQtyList objPayment = new OrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                    objPayment.LessAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "LessAmount"), 2);
                    objPayment.SaleRate = Math.Round(objBaseSqlManager.GetDecimal(dr, "SaleRate"), 2);
                    objPayment.BillDiscount = Math.Round(objBaseSqlManager.GetDecimal(dr, "BillDiscount"), 2);
                    objPayment.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "Total"), 2);
                    objPayment.Tax = Math.Round(objBaseSqlManager.GetDecimal(dr, "Tax"), 2);
                    objPayment.TaxAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmt"), 2);
                    objPayment.FinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "FinalTotal"), 2);
                    objPayment.GrandTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandTotal"), 2);
                    objPayment.TCSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount"), 2);
                    sumTCSTaxAmount = sumTCSTaxAmount + objPayment.TCSTaxAmount;
                    objPayment.TotalTCSTaxAmt = sumTCSTaxAmount;
                    objlst.Add(objPayment);
                }
                //for (int i = 0; i < objlst.Count; i++)
                //{
                //    objlst[i].GrandCGSTAmt = sumCGST;
                //    objlst[i].GrandIGSTAmt = sumIGST;
                //    objlst[i].GrandSGSTAmt = sumSGST;
                //    objlst[i].GrandTCSAmt = sumTCSTaxAmount;
                //    objlst[i].GrandNetAmount = sumTCSTaxAmount + GrandNetAmount;
                //}
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ClsReturnOrderListResponse> GetBillWiseCreditMemoForOrder(ClsReturnOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillWiseCreditMemoForOrder";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
                cmdGet.Parameters.AddWithValue("@ProductID", model.ProductID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", model.InvoiceNumber);
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
                List<ClsReturnOrderListResponse> objlst = new List<ClsReturnOrderListResponse>();
                int i = 1;
                while (dr.Read())
                {
                    ClsReturnOrderListResponse clsobj = new ClsReturnOrderListResponse();
                    clsobj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    clsobj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    clsobj.GrandQuantity = objBaseSqlManager.GetDecimal(dr, "GrandQuantity");
                    clsobj.GrandFinalTotal = objBaseSqlManager.GetDecimal(dr, "GrandFinalTotal");
                    clsobj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    clsobj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    clsobj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    clsobj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    clsobj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    clsobj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    clsobj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    clsobj.SerialNumber = i;
                    clsobj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    clsobj.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    clsobj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    clsobj.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    clsobj.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    clsobj.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    clsobj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    clsobj.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    clsobj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    clsobj.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    clsobj.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    clsobj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    clsobj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    clsobj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    clsobj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    clsobj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    clsobj.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    clsobj.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    clsobj.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    clsobj.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    clsobj.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    clsobj.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    clsobj.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    clsobj.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    clsobj.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    clsobj.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objlst.Add(clsobj);
                    i++;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyList> GetCreditMemoForOrder(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditMemoForOrder";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyList> objlst = new List<OrderQtyList>();
                while (dr.Read())
                {
                    OrderQtyList objPayment = new OrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    objPayment.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    objPayment.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    objPayment.ReturnedOrderQtyID = objBaseSqlManager.GetInt64(dr, "ReturnedOrderQtyID");
                    objPayment.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    objPayment.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyInvoiceList> GetCreditMemoInvoiceForOrderPrint(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditMemoInvoiceForOrderPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyInvoiceList> objlst = new List<OrderQtyInvoiceList>();
                while (dr.Read())
                {
                    OrderQtyInvoiceList objPayment = new OrderQtyInvoiceList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");

                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
                    if (!string.IsNullOrEmpty(objPayment.TaxNo))
                    {
                        objPayment.StateCode = objPayment.TaxNo.Substring(0, 2);
                    }
                    else
                    {
                        objPayment.StateCode = "N/A";
                    }
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //public List<OrderQtyInvoiceList> GetCreditMemoInvoiceForOrderItemPrint(string CreditMemoNumber)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetCreditMemoInvoiceForOrderItemPrint";
        //    cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<OrderQtyInvoiceList> objlst = new List<OrderQtyInvoiceList>();
        //    while (dr.Read())
        //    {
        //        OrderQtyInvoiceList objPayment = new OrderQtyInvoiceList();
        //        objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
        //        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
        //        objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
        //        objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
        //        objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
        //        objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
        //        objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
        //        objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
        //        objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
        //        objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
        //        objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
        //        objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
        //        objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
        //        objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
        //        objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
        //        objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
        //        objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
        //        objPayment.GrandTotal = Math.Round(objPayment.GrandTotal);
        //        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn"); //Convert.ToDateTime(DateTime.Now);
        //        objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
        //        objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
        //        objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
        //        objPayment.BillNo = objBaseSqlManager.GetTextValue(dr, "BillNo");
        //        objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
        //        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
        //        objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
        //        objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
        //        objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
        //        objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
        //        objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
        //        objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
        //        objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
        //        objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
        //        objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
        //        objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
        //        objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
        //        objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
        //        if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
        //        {
        //            objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
        //        }
        //        else
        //        {
        //            objPayment.NoofInvoiceint = 2;
        //        }
        //        objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
        //        objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
        //        if (!string.IsNullOrEmpty(objPayment.TaxNo))
        //        {
        //            objPayment.StateCode = objPayment.TaxNo.Substring(0, 2);
        //        }
        //        else
        //        {
        //            objPayment.StateCode = "N/A";
        //        }
        //        objlst.Add(objPayment);
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return objlst;
        //}





        public List<OrderQtyInvoiceList> GetCreditMemoInvoiceForOrderItemPrint(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditMemoInvoiceForOrderItemPrint";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyInvoiceList> objlst = new List<OrderQtyInvoiceList>();
                while (dr.Read())
                {
                    OrderQtyInvoiceList objPayment = new OrderQtyInvoiceList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objPayment.GrandTotal = Math.Round(objPayment.GrandTotal);

                    // 09 April 2021 Piyush Limbani
                    objPayment.TCSTax = objBaseSqlManager.GetDecimal(dr, "TCSTax");
                    objPayment.TCSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount");
                    objPayment.GrandCreditedTotal = objBaseSqlManager.GetDecimal(dr, "GrandCreditedTotal");
                    objPayment.GrandCreditedTotal = Math.Round(objPayment.GrandCreditedTotal);
                    objPayment.InvTotal = objBaseSqlManager.GetDecimal(dr, "InvTotal");
                    // 09 April 2021 Piyush Limbani


                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn"); //Convert.ToDateTime(DateTime.Now);
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.BillNo = objBaseSqlManager.GetTextValue(dr, "BillNo");
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
                    if (!string.IsNullOrEmpty(objPayment.TaxNo))
                    {
                        objPayment.StateCode = objPayment.TaxNo.Substring(0, 2);
                    }
                    else
                    {
                        objPayment.StateCode = "N/A";
                    }

                    //  6 April 2021 Sonal Gandhi
                    objPayment.From_Name = "VIRAKI BROTHERS";
                    objPayment.From_Address1 = "280/282, N.N. St.,Masjid, Mumbai 400009,Maharashtra";
                    objPayment.From_GSTIN = "27AAAFV3761F1Z7";//"27AAAFV3761F1Z7";
                    objPayment.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    objPayment.From_PinCode = objBaseSqlManager.GetInt32(dr, "From_PinCode");
                    objPayment.From_StateCode = "27";
                    objPayment.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objPayment.CreatedBy = objBaseSqlManager.GetInt32(dr, "CreatedBy");
                    objPayment.CESSTaxRate = 0;
                    objPayment.CESSTaxRate2 = 0;
                    objPayment.CESSAmount = 0;
                    objPayment.Cess_Non_Advol_Amount = 0;
                    objPayment.DocType = "INV";
                    objPayment.DocNo = objPayment.InvoiceNumber;
                    string[] Doc = objPayment.DocNo.Split('/');
                    objPayment.DocNo = Doc[0];
                    objPayment.DocDate1 = objPayment.CreatedOn;
                    objPayment.DocDate = objPayment.DocDate1.ToString("dd/MM/yyyy");
                    objPayment.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                    objPayment.QRCode = objBaseSqlManager.GetTextValue(dr, "QRCode");
                    objPayment.AckNo = objBaseSqlManager.GetInt64(dr, "AckNo");
                    objPayment.AckDt = objBaseSqlManager.GetDateTime(dr, "AckDt");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }




        public List<OrderQtyInvoiceList> GetInvoiceForOrderPrint(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForOrderPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyInvoiceList> objlst = new List<OrderQtyInvoiceList>();
                while (dr.Read())
                {
                    OrderQtyInvoiceList objPayment = new OrderQtyInvoiceList();
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objPayment.DeliveredDate = objBaseSqlManager.GetDateTime(dr, "DeliveredDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    objPayment.OrderRef = objBaseSqlManager.GetTextValue(dr, "OrderRef");

                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
                    if (!string.IsNullOrEmpty(objPayment.TaxNo))
                    {
                        objPayment.StateCode = objPayment.TaxNo.Substring(0, 2);
                    }
                    else
                    {
                        objPayment.StateCode = "N/A";
                    }
                    objPayment.FSSAIValidUpTo1 = objBaseSqlManager.GetDateTime(dr, "FSSAIValidUpTo");
                    if (objPayment.FSSAIValidUpTo1 == Convert.ToDateTime("10/10/2014"))
                    {
                        objPayment.FSSAIValidUpTo = "";
                    }
                    else
                    {
                        objPayment.FSSAIValidUpTo = objPayment.FSSAIValidUpTo1.ToString("dd-MM-yyyy");
                    }

                    // 24 March 2021 Sonal Gandhi

                    objPayment.From_Name = "VIRAKI BROTHERS";
                    objPayment.From_Address1 = "280/282, N.N. St.,Masjid, Mumbai 400009,Maharashtra";
                    objPayment.From_GSTIN = "27AAAFV3761F1Z7";//"27AAAFV3761F1Z7";
                    objPayment.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    objPayment.From_PinCode = objBaseSqlManager.GetInt32(dr, "From_PinCode");
                    objPayment.From_StateCode = "27";
                    objPayment.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objPayment.To_GSTIN = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objPayment.PanCard = objBaseSqlManager.GetTextValue(dr, "PanCard");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderQtyInvoiceList> GetInvoiceForOrderItemPrint(long OrderID, long CategoryID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForOrderItemPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderQtyInvoiceList> objlst = new List<OrderQtyInvoiceList>();
                while (dr.Read())
                {
                    OrderQtyInvoiceList objPayment = new OrderQtyInvoiceList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objPayment.GrandTotal = Math.Round(objPayment.GrandTotal);
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");

                    objPayment.TCSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount");

                    objPayment.InvTotal = objBaseSqlManager.GetDecimal(dr, "InvTotal");
                    objPayment.OrderTotal = objBaseSqlManager.GetDecimal(dr, "OrderTotal");
                    objPayment.OrderTotal = Math.Round(objPayment.OrderTotal);
                    objPayment.Totalrecord = objBaseSqlManager.GetDecimal(dr, "Totalrecord");


                    //  24 March 2021 Sonal Gandhi

                    objPayment.CESSTaxRate = 0;
                    objPayment.CESSTaxRate2 = 0;
                    objPayment.CESSAmount = 0;
                    objPayment.Cess_Non_Advol_Amount = 0;
                    objPayment.DocType = "INV";
                    objPayment.DocNo = objPayment.InvoiceNumber;
                    string[] Doc = objPayment.DocNo.Split('/');
                    objPayment.DocNo = Doc[0];
                    objPayment.DocDate1 = objPayment.CreatedOn;
                    objPayment.DocDate = objPayment.DocDate1.ToString("dd/MM/yyyy");
                    objPayment.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                    objPayment.QRCode = objBaseSqlManager.GetTextValue(dr, "QRCode");
                    objPayment.AckNo = objBaseSqlManager.GetInt64(dr, "AckNo");
                    objPayment.AckDt = objBaseSqlManager.GetDateTime(dr, "AckDt");


                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<OrderListResponse> GetSearchInvoiceList(OrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSearchInvoiceList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderListResponse> objlst = new List<OrderListResponse>();
                while (dr.Read())
                {
                    OrderListResponse objPayment = new OrderListResponse();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.InvoiceAmount = objBaseSqlManager.GetDecimal(dr, "InvoiceAmount");
                    objPayment.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<string> GetListOfInvoice(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.CommandText = "GetOrderInvoiceNumbersByOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetOrderViewModel objM = new RetOrderViewModel();
                List<string> lstOrderQty = new List<string>();
                while (dr.Read())
                {
                    string invoceNoold = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    //string invoceNonew = invoceNoold + "/" + DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddYears(1).ToString("yy");
                    lstOrderQty.Add(invoceNoold);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstOrderQty;
            }
        }

        public List<InvoiceForExcelList> GetInvoiceForExcel(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForExcelWholesale";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<InvoiceForExcelList> objlst = new List<InvoiceForExcelList>();
                while (dr.Read())
                {
                    InvoiceForExcelList obj = new InvoiceForExcelList();
                    obj.SupplyType = "Outward";
                    obj.SubType = "Supply";
                    obj.DocType = "Tax Invoice"; ;
                    obj.DocNo = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    string[] Doc = obj.DocNo.Split('/');
                    obj.DocNo = Doc[0];
                    obj.DocDate1 = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.DocDate = obj.DocDate1.ToString("dd/MM/yyyy");
                    obj.From_OtherPartyName = "VIRAKI BROTHERS";
                    obj.From_GSTIN = "27AAAFV3761F1Z7";
                    obj.Transaction_Type = "Regular";
                    obj.From_Address1 = objBaseSqlManager.GetTextValue(dr, "From_Address1");
                    obj.From_Address2 = objBaseSqlManager.GetTextValue(dr, "From_Address2");
                    obj.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    obj.From_PinCode = objBaseSqlManager.GetTextValue(dr, "From_PinCode");
                    obj.From_State = objBaseSqlManager.GetTextValue(dr, "From_State");
                    obj.DispatchState = objBaseSqlManager.GetTextValue(dr, "From_State");
                    obj.To_OtherPartyName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.To_GSTIN = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    obj.To_Address1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    obj.To_Address2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    obj.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");
                    obj.To_PinCode = objBaseSqlManager.GetTextValue(dr, "To_PinCode");
                    obj.To_State = objBaseSqlManager.GetTextValue(dr, "To_State");
                    obj.ShipToState = objBaseSqlManager.GetTextValue(dr, "To_State");
                    obj.ProductFull = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    string[] Product = obj.ProductFull.Split('(');
                    obj.Product = Product[0];
                    obj.DescriptionFull = objBaseSqlManager.GetTextValue(dr, "Description");
                    string[] Description = obj.DescriptionFull.Split('(');
                    obj.Description = Description[0];
                    obj.HSN = objBaseSqlManager.GetTextValue(dr, "HSN");
                    obj.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                    obj.Qty = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.AssessableValue = objBaseSqlManager.GetDecimal(dr, "AssessableValue");
                    obj.TotalTaxRate = objBaseSqlManager.GetDecimal(dr, "Tax");
                    obj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    if (obj.TaxName == "IGST")
                    {
                        obj.TotalTaxRate = obj.TotalTaxRate;
                        obj.SGSTTaxRate = 0;
                        obj.CGSTTaxRate = 0;
                        obj.IGSTTaxRate = Math.Round(obj.TotalTaxRate);
                        obj.CESSTaxRate = 0;
                        obj.CESSTaxRate2 = 0;
                        obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                    }
                    else
                    {
                        if (obj.TotalTaxRate == 5)
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate, 1);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate, 1);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
                            obj.CESSTaxRate2 = 0;
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                        else
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
                            obj.CESSTaxRate2 = 0;
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                    }
                    obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    if (obj.TaxName == "IGST")
                    {
                        obj.IGSTAmount = Math.Round(obj.TaxAmount, 2);
                    }
                    else
                    {
                        obj.DividedAmount = obj.TaxAmount / 2;
                        obj.SGSTAmount = Math.Round(obj.DividedAmount, 2);
                        obj.CGSTAmount = Math.Round(obj.DividedAmount, 2);
                    }
                    obj.CESSAmount = 0;
                    obj.Cess_Non_Advol_Amount = 0;
                    //obj.Others = 0;
                    obj.TCSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount");
                    obj.TotalInvoiceValue = objBaseSqlManager.GetDecimal(dr, "InvoiceTotal");
                    obj.TransMode = "Road";
                    obj.DistanceKM = objBaseSqlManager.GetDecimal(dr, "DeliveryAddressDistance");
                    obj.TransName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                    obj.TransID = objBaseSqlManager.GetTextValue(dr, "TransID");
                    obj.TransportGSTNumber = objBaseSqlManager.GetTextValue(dr, "TransportGSTNumber");
                    obj.TransDocNo = Doc[0];
                    // obj.TransDocNo = "";
                    obj.TransDate1 = DateTime.Now;
                    obj.TransDate = obj.TransDate1.ToString("dd/MM/yyyy");
                    obj.VehicleNo = "";
                    //obj.VehicleType = "Regular";

                    obj.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.VehicleType = "Regular";
                    obj.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long SaveChallan(ChallanViewModel data)
        {
            Challan_Mst obj = new Challan_Mst();
            obj.ChallanID = data.ChallanID;
            obj.GodownIDFrom = data.GodownIDFrom;
            obj.GodownIDTo = data.GodownIDTo;
            obj.Tax = data.Tax;
            obj.ChallanDate = DateTime.Now;
            obj.IsDelete = false;
            if (obj.TotalDiscount != null)
            {
                obj.TotalDiscount = data.TotalDiscount;
            }
            else
            {
                obj.TotalDiscount = Convert.ToDecimal("0.00");
            }

            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;

            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.ChallanID == 0)
                {
                    context.Challan_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.ChallanID > 0)
                {

                    try
                    {
                        int i = 0;
                        string ChallanNo = "";
                        decimal ChallanTotal = 0;
                        foreach (var item in data.lstOrderQty)
                        {
                            ChallanQty_Mst objOrderQty = new ChallanQty_Mst();
                            objOrderQty.ChallanID = obj.ChallanID;
                            objOrderQty.ChallanQtyID = item.ChallanQtyID;
                            objOrderQty.ProductID = item.ProductID;
                            objOrderQty.Quantity = item.Quantity;
                            objOrderQty.SaleRate = item.SaleRate;
                            objOrderQty.BaseRate = item.BaseRate;
                            objOrderQty.BillDiscount = item.BillDiscount;
                            objOrderQty.Total = item.Total;
                            objOrderQty.Tax = item.Tax;
                            objOrderQty.FinalTotal = item.FinalTotal;
                            objOrderQty.CategoryTypeID = item.CategoryTypeID;
                            objOrderQty.IsDelete = false;

                            if (objOrderQty.ChallanQtyID == 0)
                            {
                                if (i == 0 || (i % 15 == 0))
                                {
                                    if (i != 0)
                                    {
                                        UpdateChallanTotal(ChallanTotal, ChallanNo, obj.ChallanID);
                                        ChallanTotal = 0;
                                    }
                                    ChallanNo = GetLastChallan();
                                }

                                objOrderQty.ChallanNumber = ChallanNo;
                                objOrderQty.CreatedBy = data.CreatedBy;
                                objOrderQty.CreatedOn = data.CreatedOn;
                                objOrderQty.UpdatedBy = data.UpdatedBy;
                                objOrderQty.UpdatedOn = data.UpdatedOn;
                                ChallanTotal += item.FinalTotal;
                                context.ChallanQty_Mst.Add(objOrderQty);
                                context.SaveChanges();
                            }
                            i = i + 1;
                            if (i == data.lstOrderQty.Count)
                            {
                                UpdateChallanTotal(ChallanTotal, ChallanNo, obj.ChallanID);
                            }
                        }

                        return obj.ChallanID;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            Challan_Mst data1 = context.Challan_Mst.Where(x => x.ChallanID == obj.ChallanID).FirstOrDefault();
                            if (data != null)
                            {
                                context.Challan_Mst.Remove(data1);
                                context.SaveChanges();
                                return 0;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                }

                else
                {
                    return 0;
                }
            }
        }

        private string GetLastChallan()
        {
            var lstdata = GetLastChallanNumber();
            string ChallanNumber = "";
            string FinancialYearString = "/" + DateTimeExtensions.FromFinancialYear(DateTime.Now).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(DateTime.Now).ToString("yy");
            if (!string.IsNullOrEmpty(lstdata.ChallanNumber))
            {
                string number = lstdata.ChallanNumber.Substring(3, 5);
                long incr = Convert.ToInt64(number) + 1;
                ChallanNumber = "WDC" + incr.ToString().PadLeft(5, '0') + FinancialYearString;
            }
            else
            {
                ChallanNumber = "WDC00001" + FinancialYearString;
            }
            return ChallanNumber;
        }

        public ChallanListResponse GetLastChallanNumber()
        {
            ChallanListResponse objOrder = new ChallanListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastChallanNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return objOrder;
        }

        private void UpdateChallanTotal(decimal ChallanTotal, string ChallanNo, long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateChallanTotal";
                cmdGet.Parameters.AddWithValue("@ChallanNo", ChallanNo);
                cmdGet.Parameters.AddWithValue("@ChallanTotal", ChallanTotal);
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        public List<ChallanQtyInvoiceList> GetInvoiceForChallanPrint(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForChallanPrint";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanQtyInvoiceList> objlst = new List<ChallanQtyInvoiceList>();
                while (dr.Read())
                {
                    ChallanQtyInvoiceList objPayment = new ChallanQtyInvoiceList();
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.DeliveryFromAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryFromAddressLine1");
                    objPayment.DeliveryFromAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryFromAddressLine2");
                    objPayment.DeliveryFromFSSAINo = objBaseSqlManager.GetTextValue(dr, "DeliveryFromFSSAINo");
                    objPayment.DeliveryToAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryToAddressLine1");
                    objPayment.DeliveryToAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryToAddressLine2");
                    objPayment.DeliveryToFSSAINo = objBaseSqlManager.GetTextValue(dr, "DeliveryToFSSAINo");
                    objPayment.NoofInvoiceint = 1;
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<string> GetListOfChallan(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.CommandText = "GetChallanNumbersByChallanID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ChallanQtyViewModel objM = new ChallanQtyViewModel();
                List<string> lstChallanQty = new List<string>();
                while (dr.Read())
                {
                    string challanNoold = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    lstChallanQty.Add(challanNoold);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstChallanQty;
            }
        }

        public List<ChallanQtyInvoiceList> GetInvoiceForChallanItemPrint(long ChallanID, long CategoryID, string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForChallanItemPrint";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanQtyInvoiceList> objlst = new List<ChallanQtyInvoiceList>();
                while (dr.Read())
                {
                    ChallanQtyInvoiceList objPayment = new ChallanQtyInvoiceList();
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    if (objPayment.BillDiscount != 0)
                    {
                        objPayment.BillDiscountAmount = (objPayment.SaleRate * objPayment.BillDiscount) / 100;
                    }
                    else
                    {
                        objPayment.BillDiscountAmount = 0;
                    }
                    objPayment.SaleRateAmount = (objPayment.SaleRate) - (objPayment.BillDiscountAmount);
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objPayment.GrandTotal = Math.Round(objPayment.GrandTotal);
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ChallanListResponse> GetAllChallanList(ChallanListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllChallanList";
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
                List<ChallanListResponse> objlst = new List<ChallanListResponse>();

                while (dr.Read())
                {
                    ChallanListResponse objPayment = new ChallanListResponse();
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.From_Address1 = objBaseSqlManager.GetTextValue(dr, "From_Address1");
                    objPayment.From_Address2 = objBaseSqlManager.GetTextValue(dr, "From_Address2");
                    objPayment.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    objPayment.From_PinCode = objBaseSqlManager.GetTextValue(dr, "From_PinCode");
                    objPayment.From_State = objBaseSqlManager.GetTextValue(dr, "From_State");
                    objPayment.DispatchState = objBaseSqlManager.GetTextValue(dr, "From_State");
                    objPayment.To_Address1 = objBaseSqlManager.GetTextValue(dr, "To_Address1");
                    objPayment.To_Address2 = objBaseSqlManager.GetTextValue(dr, "To_Address2");
                    objPayment.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");
                    objPayment.To_PinCode = objBaseSqlManager.GetTextValue(dr, "To_PinCode");
                    objPayment.To_State = objBaseSqlManager.GetTextValue(dr, "To_State");
                    objPayment.ShipToState = objBaseSqlManager.GetTextValue(dr, "To_State");
                    objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ChallanQtyList> GetInvoiceForChallan(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForChallan";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanQtyList> objlst = new List<ChallanQtyList>();
                while (dr.Read())
                {
                    ChallanQtyList objPayment = new ChallanQtyList();
                    objPayment.DeliveryFromAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryFromAddressLine1");
                    objPayment.DeliveryFromAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryFromAddressLine2");
                    objPayment.DeliveryFromFSSAINo = objBaseSqlManager.GetTextValue(dr, "DeliveryFromFSSAINo");
                    objPayment.DeliveryToAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryToAddressLine1");
                    objPayment.DeliveryToAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryToAddressLine2");
                    objPayment.DeliveryToFSSAINo = objBaseSqlManager.GetTextValue(dr, "DeliveryToFSSAINo");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    if (objPayment.BillDiscount != 0)
                    {
                        objPayment.BillDiscountAmount = (objPayment.SaleRate * objPayment.BillDiscount) / 100;
                    }
                    else
                    {
                        objPayment.BillDiscountAmount = 0;
                    }
                    objPayment.SaleRateAmount = (objPayment.SaleRate) - (objPayment.BillDiscountAmount);
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ChallanForExcelList> GetChallanForExcel(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetChallanForExcelWholesale";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanForExcelList> objlst = new List<ChallanForExcelList>();
                while (dr.Read())
                {
                    ChallanForExcelList obj = new ChallanForExcelList();
                    obj.SupplyType = "Outward";
                    obj.SubType = "Others";
                    obj.DocType = "Delivery Challan";
                    obj.From_GSTIN = "27AAAFV3761F1Z7";
                    obj.To_GSTIN = "27AAAFV3761F1Z7";
                    obj.DeliveryFrom = "VIRAKI BROTHERS";
                    obj.DeliveryTo = "VIRAKI BROTHERS";
                    obj.DocNo = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");

                    string[] Doc = obj.DocNo.Split('/');
                    obj.DocNo = Doc[0];

                    obj.DocDate1 = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.DocDate = obj.DocDate1.ToString("dd/MM/yyyy");

                    obj.Transaction_Type = "Reguler";


                    obj.ProductFull = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    string[] Product = obj.ProductFull.Split('(');
                    obj.Product = Product[0];
                    obj.DescriptionFull = objBaseSqlManager.GetTextValue(dr, "Description");
                    string[] Description = obj.DescriptionFull.Split('(');
                    obj.Description = Description[0];
                    obj.HSN = objBaseSqlManager.GetTextValue(dr, "HSN");
                    obj.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                    obj.Qty = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.AssessableValue = objBaseSqlManager.GetDecimal(dr, "AssessableValue");
                    obj.TotalTaxRate = objBaseSqlManager.GetDecimal(dr, "Tax");
                    obj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    if (obj.TaxName == "IGST")
                    {
                        obj.TotalTaxRate = obj.TotalTaxRate;
                        obj.SGSTTaxRate = 0;
                        obj.CGSTTaxRate = 0;
                        obj.IGSTTaxRate = Math.Round(obj.TotalTaxRate);
                        obj.CESSTaxRate = 0;
                        obj.CESSTaxRate2 = 0;
                        obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                    }
                    else
                    {
                        if (obj.TotalTaxRate == 5)
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate, 1);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate, 1);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
                            obj.CESSTaxRate2 = 0;
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                        else
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
                            obj.CESSTaxRate2 = 0;
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                    }
                    obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    if (obj.TaxName == "IGST")
                    {
                        obj.IGSTAmount = Math.Round(obj.TaxAmount, 2);
                    }
                    else
                    {
                        obj.DividedAmount = obj.TaxAmount / 2;
                        obj.SGSTAmount = Math.Round(obj.DividedAmount, 2);
                        obj.CGSTAmount = Math.Round(obj.DividedAmount, 2);
                    }
                    obj.CESSAmount = 0;

                    obj.Cess_Non_Advol_Amount = 0;
                    obj.Others = 0;
                    obj.ChallanTotal = objBaseSqlManager.GetDecimal(dr, "ChallanTotal");


                    obj.TransMode = "Road";
                    obj.DistanceKM = 0;
                    obj.TransName = "VIRAKI BROTHERS";
                    obj.TransID = "27AAAFV3761F1Z7";
                    obj.TransDocNo = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");

                    string[] TransDoc = obj.TransDocNo.Split('/');
                    obj.TransDocNo = TransDoc[0];

                    obj.TransDate1 = DateTime.Now;
                    obj.TransDate = obj.TransDate1.ToString("dd/MM/yyyy");
                    obj.VehicleNo = "";
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteCreditMemo(string CreditMemoNumber, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteCreditMemo";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public List<ProductList> GetProductNameByCustomerID(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetProductCountByCustomerID";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductList> objlst = new List<ProductList>();
                while (dr.Read())
                {
                    ProductList objProduct = new ProductList();
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objProduct.OrderQuantity = objBaseSqlManager.GetDecimal(dr, "OrderQuantity");
                    objProduct.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    objlst.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateOrderpdf(string PDFName, long OrderID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateOrderpdf";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    cmdGet.Parameters.AddWithValue("@PDFName", PDFName);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public string CheckPDFNameExist(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckPDFNameExist";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string PDFName = "";
                while (dr.Read())
                {
                    PDFName = objBaseSqlManager.GetTextValue(dr, "PDFName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return PDFName;
            }
        }

        public List<OrderListResponse> GetAllMobileOrderList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRemotePrintInvoiceForMobile";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderListResponse> objlst = new List<OrderListResponse>();
                while (dr.Read())
                {
                    OrderListResponse obj = new OrderListResponse();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    //obj.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    obj.FinalTotal = GetWholesaleOrderTotal(obj.OrderID);
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");

                    obj.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    obj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 06 Jan,2022 Piyush Limbani
        public List<OrderListResponse> GetAllMobileOrderListByOrderID(string OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllMobileOrderListByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<OrderListResponse> objlst = new List<OrderListResponse>();
                while (dr.Read())
                {
                    OrderListResponse obj = new OrderListResponse();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    //obj.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    obj.FinalTotal = GetWholesaleOrderTotal(obj.OrderID);
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");

                    obj.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    obj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }
        // 06 Jan,2022 Piyush Limbani



        public bool UpdateRemotePrintStatus(long OrderID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateRemotePrintStatus";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public bool UpdateEWayNumberByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, string EWayNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateWholesaleEWayNumberByOrderIDandInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@EWayNumber", EWayNumber);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public bool UpdateDocateDetailByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, long TransportID, string DocketNo, DateTime DocketDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateDocateDetailByOrderIDandInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@DocketNo", DocketNo);
                cmdGet.Parameters.AddWithValue("@DocketDate", DocketDate);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public DecateNumberResponse GetTransportDetailByInvoiceNnumberandOrderID(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTransportDetailByInvoiceNnumberandOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                DecateNumberResponse obj = new DecateNumberResponse();
                while (dr.Read())
                {
                    obj.TransportID = objBaseSqlManager.GetInt64(dr, "TransportID");
                    obj.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    obj.DocketNo = objBaseSqlManager.GetTextValue(dr, "DocketNo");
                    obj.DocketDate = objBaseSqlManager.GetDateTime(dr, "DocketDate");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }




        public List<ChallanListResponse> GetAllChallanNoWiseChallanList(ChallanListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllChallanNoWiseChallanList";
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
                List<ChallanListResponse> objlst = new List<ChallanListResponse>();
                while (dr.Read())
                {
                    ChallanListResponse objPayment = new ChallanListResponse();
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    objPayment.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");
                    //  objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objPayment.ChallanTotal = objBaseSqlManager.GetDecimal(dr, "ChallanTotal");
                    objPayment.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ChallanQtyList> GetChallanNoWiseChallanForChallan(string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetChallanNoWiseChallanForChallan";
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanQtyList> objlst = new List<ChallanQtyList>();
                while (dr.Read())
                {
                    ChallanQtyList objPayment = new ChallanQtyList();
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.DeliveryFromAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryFromAddressLine1");
                    objPayment.DeliveryFromAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryFromAddressLine2");
                    objPayment.DeliveryFromFSSAINo = objBaseSqlManager.GetTextValue(dr, "DeliveryFromFSSAINo");
                    objPayment.DeliveryToAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryToAddressLine1");
                    objPayment.DeliveryToAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryToAddressLine2");
                    objPayment.DeliveryToFSSAINo = objBaseSqlManager.GetTextValue(dr, "DeliveryToFSSAINo");
                    objPayment.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    if (objPayment.BillDiscount != 0)
                    {
                        objPayment.BillDiscountAmount = (objPayment.SaleRate * objPayment.BillDiscount) / 100;
                    }
                    else
                    {
                        objPayment.BillDiscountAmount = 0;
                    }
                    objPayment.SaleRateAmount = (objPayment.SaleRate) - (objPayment.BillDiscountAmount);
                    objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");

                    //objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");           
                    //objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    //objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    //objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    //objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    //objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    //objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    //objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    //objPayment.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    //objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    //objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    //objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");
                    //objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateEWayNumberForChallanByChallanNumber(long ChallanID, string ChallanNumber, string EWayNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateEWayNumberForChallanByChallanNumber";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                cmdGet.Parameters.AddWithValue("@EWayNumber", EWayNumber);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }


        // 01 Sep. 2020 Piyush Limbani
        public string CheckPDFIsExistForInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckPDFIsExistForInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string PDFName = "";
                while (dr.Read())
                {
                    PDFName = objBaseSqlManager.GetTextValue(dr, "PDFName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return PDFName;
            }
        }

        public bool UpdateInvoiceNameByOrderIDAndInvoiceNumber(string PDFName, long OrderID, string InvoiceNumber)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateInvoiceNameByOrderIDAndInvoiceNumber";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    cmdGet.Parameters.AddWithValue("@PDFName", PDFName);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }


        public void AddEInvoice(EInvoice data)
        {
            EInvoice_Mst obj = new EInvoice_Mst();
            obj.OrderId = data.OrderId;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.AckNo = data.AckNo;
            obj.AckDt = data.AckDt;
            obj.IRN = data.IRN;
            obj.QRCode = data.QRCode;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            obj.IsCancel = false;
            using (VirakiEntities context = new VirakiEntities())
            {
                context.EInvoice_Mst.Add(obj);
                context.SaveChanges();
            }
        }


        // 02 April 2021 Piyush Limbani
        public long CheckEInvoiceExist(long OrderId, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckEInvoiceExist";
                cmdGet.Parameters.AddWithValue("@OrderId", OrderId);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long EInvoiceId = 0;
                while (dr.Read())
                {
                    EInvoiceId = objBaseSqlManager.GetInt64(dr, "EInvoiceId");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return EInvoiceId;
            }
        }

        // 04 April 2021 Piyush Limbani
        public EInvoiceIRN GetIRNNumberByInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetIRNNumberByInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                EInvoiceIRN obj = new EInvoiceIRN();
                while (dr.Read())
                {
                    obj.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public void AddEInvoiceCreditMemo(EInvoiceCreditMemo data)
        {

            EInvoiceCreditMemo_Mst obj = new EInvoiceCreditMemo_Mst();
            obj.ReturnOrderId = data.ReturnOrderId;
            obj.CreditMemoNumber = data.CreditMemoNumber;
            obj.AckNo = data.AckNo;
            obj.AckDt = data.AckDt;
            obj.IRN = data.IRN;
            obj.QRCode = data.QRCode;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            obj.IsCancel = false;

            using (VirakiEntities context = new VirakiEntities())
            {
                context.EInvoiceCreditMemo_Mst.Add(obj);
                context.SaveChanges();
            }
        }

        // 07 April 2021 Piyush Limbani
        public long CheckECreditMemoExist(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckECreditMemoExist";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long EInvoiceCreditMemoId = 0;
                while (dr.Read())
                {
                    EInvoiceCreditMemoId = objBaseSqlManager.GetInt64(dr, "EInvoiceCreditMemoId");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return EInvoiceCreditMemoId;
            }
        }

        // 08 April 2021 Piyush Limbani
        public EInvoiceCreditMemoIRN GetIRNNumberByCreditMemoNumber(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetIRNNumberByCreditMemoNumber";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                EInvoiceCreditMemoIRN obj = new EInvoiceCreditMemoIRN();
                while (dr.Read())
                {
                    obj.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }


        // 14 April 2021 Piyush Limbani  E-Invoice Error Report
        public List<EInvoiceErrorListResponse> GetEInvoiceErrorList(DateTime Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetEInvoiceErrorList";
                cmdGet.Parameters.AddWithValue("@Date", Date);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<EInvoiceErrorListResponse> objlst = new List<EInvoiceErrorListResponse>();
                while (dr.Read())
                {
                    EInvoiceErrorListResponse obj = new EInvoiceErrorListResponse();
                    obj.EInvoiceErrorDetailsID = objBaseSqlManager.GetInt64(dr, "EInvoiceErrorDetailsID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "Customer");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.ErrorCodes = objBaseSqlManager.GetTextValue(dr, "ErrorCodes");
                    obj.ErrorDesc = objBaseSqlManager.GetTextValue(dr, "ErrorDesc");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }



        // 27 April, 2021 Sonal Gandhi
        public void AddEWayBill(EWayBill data)
        {
            EWayBill_Mst obj = new EWayBill_Mst();
            obj.OrderId = data.OrderId;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.EWBNumber = data.EWayBillNumber.ToString();
            obj.EWBDate = data.EWBDate;
            obj.EWBValidTill = data.EWBValidTill;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            using (VirakiEntities context = new VirakiEntities())
            {
                context.EWayBill_Mst.Add(obj);
                context.SaveChanges();
            }
        }

        // 27 April, 2021 Sonal Gandhi
        public long CheckEWayBillExist(long OrderId, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckEWayBillExist";
                cmdGet.Parameters.AddWithValue("@OrderId", OrderId);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long EWBId = 0;
                while (dr.Read())
                {
                    EWBId = objBaseSqlManager.GetInt64(dr, "EWBId");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return EWBId;
            }
        }



        public List<DetailsForEWB> GetDetailsForEWB(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDetailsForEWBWholesale";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DetailsForEWB> objlst = new List<DetailsForEWB>();
                while (dr.Read())
                {
                    DetailsForEWB obj = new DetailsForEWB();
                    obj.DocNo = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    string[] Doc = obj.DocNo.Split('/');
                    obj.DocNo = Doc[0];

                    obj.From_OtherPartyName = "VIRAKI BROTHERS";
                    obj.From_GSTIN = "27AAAFV3761F1Z7";
                    obj.Transaction_Type = "Regular";
                    obj.From_Address1 = objBaseSqlManager.GetTextValue(dr, "From_Address1");
                    obj.From_Address2 = objBaseSqlManager.GetTextValue(dr, "From_Address2");
                    obj.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    obj.From_PinCode = objBaseSqlManager.GetTextValue(dr, "From_PinCode");
                    obj.From_State = objBaseSqlManager.GetTextValue(dr, "From_State");
                    obj.DispatchState = objBaseSqlManager.GetTextValue(dr, "From_State");
                    obj.To_OtherPartyName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.To_GSTIN = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    obj.To_Address1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    obj.To_Address2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    obj.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");
                    obj.To_PinCode = objBaseSqlManager.GetTextValue(dr, "To_PinCode");
                    obj.To_State = objBaseSqlManager.GetTextValue(dr, "To_State");
                    obj.ShipToState = objBaseSqlManager.GetTextValue(dr, "To_State");
                    obj.TransMode = "Road";
                    obj.DistanceKM = objBaseSqlManager.GetDecimal(dr, "DeliveryAddressDistance");
                    obj.TransName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                    obj.TransID = objBaseSqlManager.GetTextValue(dr, "TransID");
                    obj.TransportGSTNumber = objBaseSqlManager.GetTextValue(dr, "TransportGSTNumber");
                    obj.TransDocNo = Doc[0];
                    // obj.TransDocNo = "";
                    obj.TransDate1 = DateTime.Now;
                    obj.TransDate = obj.TransDate1.ToString("dd/MM/yyyy");
                    obj.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.VehicleType = "Regular";
                    obj.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 24 May, 2021 Sonal Gandhi
        public void AddEWayBillChallan(EWayBillChallan data)
        {
            EWayBillChallan_Mst obj = new EWayBillChallan_Mst();
            obj.ChallanID = data.ChallanID;
            obj.ChallanNumber = data.ChallanNumber;
            obj.EWBNumber = data.EWayBillNumber.ToString();
            obj.EWBDate = Convert.ToDateTime(data.EWBDate);
            obj.EWBValidTill = Convert.ToDateTime(data.EWBValidTill);
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            using (VirakiEntities context = new VirakiEntities())
            {
                context.EWayBillChallan_Mst.Add(obj);
                context.SaveChanges();
            }
        }

        // 24 May, 2021 Sonal Gandhi
        public long CheckEWayBillChallanExist(long ChallanID, string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckEWayBillChallanExist";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long EWBChallanId = 0;
                while (dr.Read())
                {
                    EWBChallanId = objBaseSqlManager.GetInt64(dr, "EWBChallanId");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return EWBChallanId;
            }
        }


        // 24 May, 2021 Sonal Gandhi
        public List<ChallanItemForEWB> GetChallanItemForEWB(long ChallanID, string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetChallanItemForEWB";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanItemForEWB> objlst = new List<ChallanItemForEWB>();
                while (dr.Read())
                {
                    ChallanItemForEWB obj = new ChallanItemForEWB();
                    obj.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    obj.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    obj.DocNo = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");

                    string[] Doc = obj.DocNo.Split('/');
                    obj.DocNo = Doc[0];

                    obj.DocDate1 = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.DocDate = obj.DocDate1.ToString("dd/MM/yyyy");


                    obj.ProductFull = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    string[] Product = obj.ProductFull.Split('(');
                    obj.Product = Product[0];
                    obj.DescriptionFull = objBaseSqlManager.GetTextValue(dr, "Description");
                    string[] Description = obj.DescriptionFull.Split('(');
                    obj.Description = Description[0];
                    obj.HSN = objBaseSqlManager.GetTextValue(dr, "HSN");
                    obj.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                    obj.Qty = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.AssessableValue = objBaseSqlManager.GetDecimal(dr, "AssessableValue");
                    obj.TotalTaxRate = objBaseSqlManager.GetDecimal(dr, "Tax");
                    obj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    if (obj.TaxName == "IGST")
                    {
                        obj.TotalTaxRate = obj.TotalTaxRate;
                        obj.SGSTTaxRate = 0;
                        obj.CGSTTaxRate = 0;
                        obj.IGSTTaxRate = Math.Round(obj.TotalTaxRate);
                        obj.CESSTaxRate = 0;
                        obj.CESSTaxRate2 = 0;
                        obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                    }
                    else
                    {
                        if (obj.TotalTaxRate == 5)
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate, 1);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate, 1);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
                            obj.CESSTaxRate2 = 0;
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                        else
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
                            obj.CESSTaxRate2 = 0;
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                    }
                    obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    if (obj.TaxName == "IGST")
                    {
                        obj.IGSTAmount = Math.Round(obj.TaxAmount, 2);
                    }
                    else
                    {
                        obj.DividedAmount = obj.TaxAmount / 2;
                        obj.SGSTAmount = Math.Round(obj.DividedAmount, 2);
                        obj.CGSTAmount = Math.Round(obj.DividedAmount, 2);
                    }
                    obj.CESSAmount = 0;

                    obj.Cess_Non_Advol_Amount = 0;
                    obj.Others = 0;
                    obj.ChallanTotal = objBaseSqlManager.GetDecimal(dr, "ChallanTotal");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ChallanDetailForEWB> GetChallanDetailForEWB(long ChallanID, long TransportID, long VehicleDetailID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetChallanDetailForEWB";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ChallanDetailForEWB> objlst = new List<ChallanDetailForEWB>();
                while (dr.Read())
                {
                    ChallanDetailForEWB obj = new ChallanDetailForEWB();
                    obj.SupplyType = "O";  //"Outward";
                    obj.SubType = "8";  //"Others";
                    obj.DocType = "CHL";  //"Delivery Challan";
                    obj.From_GSTIN = "27AAAFV3761F1Z7";
                    obj.To_GSTIN = "27AAAFV3761F1Z7";
                    obj.DeliveryFrom = "VIRAKI BROTHERS";
                    obj.DeliveryTo = "VIRAKI BROTHERS";
                    obj.Transaction_Type = "1";  //"Reguler";
                    obj.TransMode = "1";  //"Road";

                    obj.DistanceKM = 0;
                    //obj.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    obj.TransName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                    obj.TransID = objBaseSqlManager.GetTextValue(dr, "TransID");
                    obj.TransportGSTNumber = objBaseSqlManager.GetTextValue(dr, "TransportGSTNumber");
                    obj.TransDate1 = DateTime.Now;
                    obj.TransDate = obj.TransDate1.ToString("dd/MM/yyyy");
                    obj.From_Address1 = objBaseSqlManager.GetTextValue(dr, "From_Address1");
                    obj.From_Address2 = objBaseSqlManager.GetTextValue(dr, "From_Address2");
                    obj.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    obj.From_PinCode = objBaseSqlManager.GetTextValue(dr, "From_PinCode");
                    obj.From_State = objBaseSqlManager.GetTextValue(dr, "From_State");
                    obj.DispatchState = objBaseSqlManager.GetTextValue(dr, "From_State");
                    obj.To_Address1 = objBaseSqlManager.GetTextValue(dr, "To_Address1");
                    obj.To_Address2 = objBaseSqlManager.GetTextValue(dr, "To_Address2");
                    obj.To_Place = objBaseSqlManager.GetTextValue(dr, "To_Place");
                    obj.To_PinCode = objBaseSqlManager.GetTextValue(dr, "To_PinCode");
                    obj.To_State = objBaseSqlManager.GetTextValue(dr, "To_State");
                    obj.ShipToState = objBaseSqlManager.GetTextValue(dr, "To_State");
                    obj.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.FinalTotal = objBaseSqlManager.GetDouble(dr, "FinalTotal");
                    obj.Qty = objBaseSqlManager.GetDouble(dr, "Quantity");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }
    }
}
