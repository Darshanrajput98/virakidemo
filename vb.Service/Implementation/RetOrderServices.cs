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
    public class RetOrderServices : IRetOrderService
    {
        public List<string> GetTaxForCustomerByTextChange(string CustomerName)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetActiveCustomersByName";
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

        public GetRetTax GetTaxForCustomerNumber(long CustomerNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxForRetCustomerNumber";
                cmdGet.Parameters.AddWithValue("@CustomerNumber", CustomerNumber.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetRetTax objOrder = new GetRetTax();
                while (dr.Read())
                {
                    objOrder.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objOrder.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objOrder.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objOrder.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objOrder.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objOrder.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objOrder.IsTCSApplicable = objBaseSqlManager.GetBoolean(dr, "IsTCSApplicable");
                    objOrder.GSTNumber = objBaseSqlManager.GetTextValue(dr, "GSTNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public GetCustDetailExport GetCustomerDetailForExportOrder(long CustomerNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCustomerDetailForExportOrder";
                cmdGet.Parameters.AddWithValue("@CustomerNumber", CustomerNumber.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetCustDetailExport objOrder = new GetCustDetailExport();
                while (dr.Read())
                {
                    objOrder.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objOrder.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
                    objOrder.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    objOrder.CountryOfOrigin = "INDIA";
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public bool UpdateSection(List<RetOrderPackList> data)
        {
            foreach (var item in data)
            {
                if (item.ProductQtyID != 0 && item.Section != 0)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdatePackListSection";
                        cmdGet.Parameters.AddWithValue("@ProductQtyID", item.ProductQtyID);
                        cmdGet.Parameters.AddWithValue("@SectionID", item.Section);
                        cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            return true;
        }

        public bool UpdateSummarySection(List<RetOrderSummaryListResponse> data)
        {
            foreach (var item in data)
            {
                if (item.OrderID != 0 && item.Section != 0)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateSummaryListSection";
                        cmdGet.Parameters.AddWithValue("@OrderID", item.OrderID);
                        cmdGet.Parameters.AddWithValue("@SectionID", item.Section);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            return true;
        }

        public string AddOrder(RetOrderViewModel data)
        {
            bool exist = false;
            if (data.OrderID > 0)
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "GetRetailFinaliseOrderByOrderID";
                    cmdGet.Parameters.AddWithValue("@OrderID", data.OrderID);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        exist = true;
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            if (exist == false)
            {

                if (!string.IsNullOrEmpty(data.DeleteItems))
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "DeleteRetOrderItems";
                        cmdGet.Parameters.AddWithValue("@OrderQtyID", data.DeleteItems);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                RetOrderMst obj = new RetOrderMst();
                obj.OrderID = data.OrderID;
                obj.InvoiceNumber = data.InvoiceNumber;
                obj.CustomerID = data.CustomerID;
                obj.Tax = data.Tax;
                obj.ShipTo = data.ShipTo;
                obj.BillTo = data.BillTo;
                obj.OrderDate = data.OrderDate == null ? DateTime.Now.Date : data.OrderDate;
                obj.PODate = data.PODate;
                obj.PONumber = data.PONumber;
                obj.OrderNote = data.OrderNote;
                obj.SectionID = 1;
                if (obj.TotalDiscount != null)
                {
                    obj.TotalDiscount = data.TotalDiscount;
                }
                else
                {
                    obj.TotalDiscount = Convert.ToDecimal("0.00");
                }
                obj.IsTCSApplicable = data.IsTCSApplicable;
                obj.IsDelete = false;
                obj.CreatedBy = data.CreatedBy;
                obj.CreatedOn = data.CreatedOn;
                obj.UpdatedBy = data.UpdatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                using (VirakiEntities context = new VirakiEntities())
                {
                    if (obj.OrderID == 0)
                    {
                        context.RetOrderMsts.Add(obj);
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
                            foreach (var item in data.lstOrderQty)
                            {
                                if (item.CategoryTypeID == 0)
                                {
                                    item.CategoryTypeID = GetCategoryTypeID(item.ProductQtyID);
                                }
                                RetOrderQtyMst objOrderQty = new RetOrderQtyMst();
                                objOrderQty.OrderQtyID = item.OrderQtyID;
                                objOrderQty.OrderID = obj.OrderID;
                                objOrderQty.ProductID = item.ProductID;
                                objOrderQty.ProductQtyID = item.ProductQtyID;
                                objOrderQty.CategoryTypeID = item.CategoryTypeID;
                                objOrderQty.Quantity = item.Quantity;
                                objOrderQty.UpdateQuantity = item.Quantity;
                                objOrderQty.ProductPrice = item.ProductPrice;

                                // Add by Dhruvik 
                                objOrderQty.Margin = item.Margin;
                                objOrderQty.SPDiscount = item.SPDiscount;
                                objOrderQty.TexableAmount = item.TexableAmount;
                                // Add by Dhruvik  

                                objOrderQty.ProductMRP = item.ProductMRP;
                                objOrderQty.ArticleCode = item.ArticleCode;
                                objOrderQty.DiscountPrice = item.DiscountPrice;
                                objOrderQty.DiscountPer = item.DiscountPer;
                                objOrderQty.Tax = item.Tax;
                                objOrderQty.TaxAmount = item.TaxAmount;
                                objOrderQty.TotalAmount = item.TotalAmount;
                                objOrderQty.IsDelete = false;
                                objOrderQty.PendingDelivery = true;
                                objOrderQty.SectionID = 1;
                                if (objOrderQty.OrderQtyID == 0)
                                {
                                    objOrderQty.InvoiceNumber = "";
                                    objOrderQty.CreatedBy = data.CreatedBy;
                                    objOrderQty.CreatedOn = data.CreatedOn;
                                    objOrderQty.UpdatedBy = data.UpdatedBy;
                                    objOrderQty.UpdatedOn = data.UpdatedOn;
                                    context.RetOrderQtyMsts.Add(objOrderQty);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    objOrderQty.InvoiceNumber = "";
                                    objOrderQty.CreatedBy = data.CreatedBy;
                                    objOrderQty.CreatedOn = data.CreatedOn;
                                    objOrderQty.UpdatedBy = data.UpdatedBy;
                                    objOrderQty.UpdatedOn = data.UpdatedOn;
                                    context.Entry(objOrderQty).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                            return "true";
                        }
                        catch
                        {
                            RetOrderMst data1 = context.RetOrderMsts.FirstOrDefault(x => x.OrderID == obj.OrderID);
                            if (data != null)
                            {
                                context.RetOrderMsts.Remove(data1);
                                context.SaveChanges();
                                return "true";
                            }
                            else
                            {
                                return "false";
                            }
                        }
                    }
                    else
                    {
                        return "false";
                    }
                }
            }
            else
            {
                return "finalised";
            }
        }

        public string FinaliseOrder(RetOrderViewModel data)
        {
            bool exist = false;
            if (data.OrderID > 0)
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "GetRetailFinaliseOrderByOrderID";
                    cmdGet.Parameters.AddWithValue("@OrderID", data.OrderID);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        exist = true;
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }

            if (exist == false)
            {
                if (!string.IsNullOrEmpty(data.DeleteItems))
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "DeleteRetOrderItems";
                        cmdGet.Parameters.AddWithValue("@OrderQtyID", data.DeleteItems);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
                RetOrderMst obj = new RetOrderMst();
                obj.OrderID = data.OrderID;
                obj.InvoiceNumber = data.InvoiceNumber;
                obj.CustomerID = data.CustomerID;
                obj.Tax = data.Tax;
                obj.ShipTo = data.ShipTo;
                obj.BillTo = data.BillTo;
                obj.OrderDate = data.OrderDate == null ? DateTime.Now.Date : data.OrderDate;
                obj.PODate = data.PODate;
                obj.PONumber = data.PONumber;
                obj.OrderNote = data.OrderNote;
                obj.IsFinalised = data.IsFinalised;
                obj.SectionID = 1;
                // 25 sep
                obj.IsTCSApplicable = data.IsTCSApplicable;
                decimal TCSTax = 0;

                // 30 March,2021 Sonal Gandhi
                int invoiceRowNumber = 0;
                if (string.IsNullOrEmpty(data.GSTNumber))
                {
                    invoiceRowNumber = 30;
                }
                else
                {
                    invoiceRowNumber = 25;
                }

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
                obj.IsDelete = false;
                obj.CreatedBy = data.CreatedBy;
                obj.CreatedOn = data.CreatedOn;
                obj.UpdatedBy = data.UpdatedBy;
                obj.UpdatedOn = data.UpdatedOn;
                using (VirakiEntities context = new VirakiEntities())
                {
                    if (obj.OrderID == 0)
                    {
                        context.RetOrderMsts.Add(obj);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Entry(obj).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    if (obj.OrderID > 0)
                    {
                        data.OrderID = obj.OrderID;
                        try
                        {
                            foreach (var CategoryLstitem in data.lstOrderQty)
                            {
                                if (CategoryLstitem.CategoryTypeID == 0)
                                {
                                    CategoryLstitem.CategoryTypeID = GetCategoryTypeID(CategoryLstitem.ProductQtyID);
                                }
                            }
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
                                        RetOrderQtyMst objOrderQty = new RetOrderQtyMst();
                                        objOrderQty.OrderQtyID = item.OrderQtyID;
                                        if (objOrderQty.OrderQtyID == 0)
                                        {
                                            objOrderQty.OrderID = data.OrderID;
                                            objOrderQty.ProductID = item.ProductID;
                                            objOrderQty.ProductQtyID = item.ProductQtyID;
                                            objOrderQty.CategoryTypeID = item.CategoryTypeID;
                                            objOrderQty.Quantity = item.Quantity;
                                            objOrderQty.UpdateQuantity = item.Quantity;
                                            objOrderQty.ProductPrice = item.ProductPrice;

                                            // Add by Dhruvik 
                                            objOrderQty.Margin = item.Margin;
                                            objOrderQty.SPDiscount = item.SPDiscount;
                                            objOrderQty.TexableAmount = item.TexableAmount;
                                            // Add by Dhruvik 

                                            objOrderQty.ProductMRP = item.ProductMRP;
                                            objOrderQty.ArticleCode = item.ArticleCode;
                                            objOrderQty.DiscountPrice = item.DiscountPrice;
                                            objOrderQty.DiscountPer = item.DiscountPer;
                                            objOrderQty.Tax = item.Tax;
                                            objOrderQty.TaxAmount = item.TaxAmount;
                                            objOrderQty.TotalAmount = item.TotalAmount;
                                            objOrderQty.TCSTax = TCSTax;
                                            objOrderQty.IsDelete = false;
                                            objOrderQty.PendingDelivery = true;
                                            objOrderQty.SectionID = 1;
                                            if (i == 0 || (i % invoiceRowNumber == 0))
                                            {
                                                if (i != 0)
                                                {
                                                    // 25 sep
                                                    //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                                    TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                                    GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                                    UpdateInvoiceTotal(InvoiceTotal, TCSTax, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                                    //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                                    InvoiceTotal = 0;
                                                }
                                                InvoiceNo = Getlastorder();
                                            }
                                            objOrderQty.InvoiceNumber = InvoiceNo;
                                            objOrderQty.CreatedBy = data.CreatedBy;
                                            objOrderQty.CreatedOn = data.CreatedOn;
                                            objOrderQty.UpdatedBy = data.UpdatedBy;
                                            objOrderQty.UpdatedOn = data.UpdatedOn;
                                            InvoiceTotal += item.TotalAmount;
                                            context.RetOrderQtyMsts.Add(objOrderQty);
                                            context.SaveChanges();
                                        }
                                        else
                                        {

                                            if (i == 0 || (i % invoiceRowNumber == 0))
                                            {
                                                if (i != 0)
                                                {
                                                    // 25 sep
                                                    //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                                    TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                                    GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                                    UpdateInvoiceTotal(InvoiceTotal, TCSTax, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                                    //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                                    InvoiceTotal = 0;
                                                }
                                                InvoiceNo = Getlastorder();
                                            }
                                            InvoiceTotal += item.TotalAmount;
                                            SqlCommand cmdGet = new SqlCommand();
                                            using (var objBaseSqlManager = new BaseSqlManager())
                                            {
                                                cmdGet.CommandType = CommandType.StoredProcedure;
                                                cmdGet.CommandText = "UpdateInvoiceNumberForRetOrderQtyMaster";
                                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNo);
                                                cmdGet.Parameters.AddWithValue("@OrderQtyID", item.OrderQtyID);
                                                cmdGet.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                                objBaseSqlManager.ForceCloseConnection();
                                            }
                                        }
                                        i = i + 1;
                                        if (i == orderqtylst.Count)
                                        {
                                            //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                            TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                            GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                            UpdateInvoiceTotal(InvoiceTotal, TCSTax, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                            //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
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
                                        RetOrderQtyMst objOrderQty = new RetOrderQtyMst();
                                        objOrderQty.OrderQtyID = item.OrderQtyID;
                                        if (objOrderQty.OrderQtyID != 0)
                                        {
                                            objOrderQty.OrderID = data.OrderID;
                                            objOrderQty.ProductID = item.ProductID;
                                            objOrderQty.ProductQtyID = item.ProductQtyID;
                                            objOrderQty.CategoryTypeID = item.CategoryTypeID;
                                            objOrderQty.Quantity = item.Quantity;
                                            objOrderQty.UpdateQuantity = item.Quantity;
                                            objOrderQty.ProductPrice = item.ProductPrice;

                                            // Add by Dhruvik 
                                            objOrderQty.Margin = item.Margin;
                                            objOrderQty.SPDiscount = item.SPDiscount;
                                            objOrderQty.TexableAmount = item.TexableAmount;
                                            // Add by Dhruvik 

                                            objOrderQty.ProductMRP = item.ProductMRP;
                                            objOrderQty.DiscountPrice = item.DiscountPrice;
                                            objOrderQty.Tax = item.Tax;
                                            objOrderQty.TaxAmount = item.TaxAmount;
                                            objOrderQty.TotalAmount = item.TotalAmount;
                                            objOrderQty.TCSTax = TCSTax;
                                            objOrderQty.IsDelete = false;
                                            objOrderQty.PendingDelivery = true;
                                            objOrderQty.SectionID = 1;
                                            if (i == 0 || (i % invoiceRowNumber == 0))
                                            {
                                                if (i != 0)
                                                {
                                                    // 25 sep
                                                    //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                                    TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                                    GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                                    UpdateInvoiceTotal(InvoiceTotal, TCSTax, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                                    //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                                    InvoiceTotal = 0;
                                                }
                                                InvoiceNo = Getlastorder();
                                            }
                                            objOrderQty.InvoiceNumber = InvoiceNo;
                                            objOrderQty.CreatedBy = data.CreatedBy;
                                            objOrderQty.CreatedOn = data.CreatedOn;
                                            objOrderQty.UpdatedBy = data.UpdatedBy;
                                            objOrderQty.UpdatedOn = data.UpdatedOn;
                                            InvoiceTotal += item.TotalAmount;
                                            context.Entry(data).State = EntityState.Modified;
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            if (i == 0 || (i % invoiceRowNumber == 0))
                                            {
                                                if (i != 0)
                                                {
                                                    // 25 sep
                                                    //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                                    TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                                    GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                                    UpdateInvoiceTotal(InvoiceTotal, TCSTax, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                                    //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                                    InvoiceTotal = 0;
                                                }
                                                InvoiceNo = Getlastorder();
                                            }
                                            InvoiceTotal += item.TotalAmount;
                                            SqlCommand cmdGet = new SqlCommand();
                                            using (var objBaseSqlManager = new BaseSqlManager())
                                            {
                                                cmdGet.CommandType = CommandType.StoredProcedure;
                                                cmdGet.CommandText = "UpdateInvoiceNumberForRetOrderQtyMaster";
                                                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNo);
                                                cmdGet.Parameters.AddWithValue("@OrderQtyID", item.OrderQtyID);
                                                cmdGet.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                                                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                                                objBaseSqlManager.ForceCloseConnection();
                                            }
                                        }
                                        i = i + 1;
                                        if (i == orderqtylstNottax.Count)
                                        {
                                            //TCSTaxAmount = Math.Ceiling((InvoiceTotal * TCSTax) / 100);
                                            TCSTaxAmount = ((InvoiceTotal * TCSTax) / 100);
                                            GrandInvoiceTotal = InvoiceTotal + TCSTaxAmount;
                                            UpdateInvoiceTotal(InvoiceTotal, TCSTax, TCSTaxAmount, GrandInvoiceTotal, InvoiceNo, obj.OrderID);
                                            //UpdateInvoiceTotal(InvoiceTotal, InvoiceNo, obj.OrderID);
                                        }
                                    }
                                }
                            }
                            string output = String.Format("{0},{1}", "true", data.OrderID);
                            return output;
                        }
                        catch
                        {
                            return "false";
                        }
                    }
                    else
                    {
                        return "false";
                    }
                }
            }
            else { return "finalised"; }
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
                return TaxWithGST;
            }
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
                return TaxWithOutGST;
            }
        }

        private void UpdateInvoiceTotal(decimal InvoiceTotal, decimal TCSTax, decimal TCSTaxAmount, decimal GrandInvoiceTotal, string InvoiceNo, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetInvoiceTotal";
                cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmdGet.Parameters.AddWithValue("@InvoiceTotal", InvoiceTotal);
                cmdGet.Parameters.AddWithValue("@TCSTax", TCSTax);
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
        //    cmdGet.CommandText = "UpdateRetInvoiceTotal";
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
            if (!string.IsNullOrEmpty(lstdata.InvoiceNumber))
            {
                string number = lstdata.InvoiceNumber;
                long incr = Convert.ToInt64(number) + 1;
                InvoiceNumber = incr.ToString().PadLeft(7, '0');
            }
            else
            {
                InvoiceNumber = "0000001";
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
                cmdGet.CommandText = "GetLastRetailInvoiceNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public long GetCategoryTypeID(long ProductQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "SetCategoryTypeID";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                long CategoryTypeID = 0;
                while (dr.Read())
                {
                    CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CategoryTypeID;
            }
        }

        public List<RetOrderListResponse> GetAllOrderList(RetOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetOrderList";
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
                List<RetOrderListResponse> objlst = new List<RetOrderListResponse>();
                while (dr.Read())
                {
                    RetOrderListResponse objPayment = new RetOrderListResponse();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objPayment.IsPrinted = objBaseSqlManager.GetBoolean(dr, "IsPrinted");
                    //objPayment.OrderTotal = objBaseSqlManager.GetDecimal(dr, "OrderTotal");
                    objPayment.OrderTotal = GetRetailOrderTotal(objPayment.OrderID);
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public decimal GetRetailOrderTotal(long OrderID)
        {
            decimal FinalTotal = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailOrderTotal";
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


        public List<RetOrderListResponse> GetAllBillWiseOrderList(RetOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllBillWiseRetOrderList";
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", model.CustomerGroupID);
                cmdGet.Parameters.AddWithValue("@ProductQtyID", model.ProductQtyID);
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
                List<RetOrderListResponse> objlst = new List<RetOrderListResponse>();
                while (dr.Read())
                {
                    RetOrderListResponse objPayment = new RetOrderListResponse();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.UpdatedOn = objBaseSqlManager.GetDateTime(dr, "UpdatedOn");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");

                    objPayment.FullInvoiceNumber = objPayment.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objPayment.UpdatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objPayment.UpdatedOn).ToString("yy");

                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objPayment.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    objPayment.InvoiceTotal = objBaseSqlManager.GetDecimal(dr, "InvoiceTotal");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetOrderQtyList> GetInvoiceForOrder(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForRetailOrder";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyList> objlst = new List<RetOrderQtyList>();
                while (dr.Read())
                {
                    RetOrderQtyList objPayment = new RetOrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.PODate = objBaseSqlManager.GetDateTime(dr, "PODate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetOrderQtyList> GetBillWiseInvoiceForOrder(string InvoiceNumber, long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillWiseInvoiceForRetOrder";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyList> objlst = new List<RetOrderQtyList>();
                while (dr.Read())
                {
                    RetOrderQtyList objPayment = new RetOrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.PODate = objBaseSqlManager.GetDateTime(dr, "PODate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objPayment.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    objPayment.TCSTaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount"), 2);
                    objPayment.InvoiceTotal = Math.Round(objPayment.GrandTotal + objPayment.TCSTaxAmount);
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ClsRetReturnOrderListResponse> GetBillWiseCreditMemoForOrder(ClsRetReturnOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillWiseCreditMemoForRetOrder";
                cmdGet.Parameters.AddWithValue("@UserID", model.UserID);
                cmdGet.Parameters.AddWithValue("@CustomerID", model.CustomerID);
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
                List<ClsRetReturnOrderListResponse> objlst = new List<ClsRetReturnOrderListResponse>();
                int i = 1;
                while (dr.Read())
                {
                    ClsRetReturnOrderListResponse clsobj = new ClsRetReturnOrderListResponse();
                    clsobj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    clsobj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    clsobj.SerialNumber = i;
                    clsobj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    clsobj.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    clsobj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    clsobj.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    clsobj.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    clsobj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    clsobj.PODate = objBaseSqlManager.GetDateTime(dr, "PODate");
                    clsobj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    clsobj.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    clsobj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    clsobj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    clsobj.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    clsobj.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    clsobj.DiscountPer = objBaseSqlManager.GetDecimal(dr, "DiscountPer");
                    clsobj.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    clsobj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    clsobj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    clsobj.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    clsobj.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    clsobj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    clsobj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    clsobj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    clsobj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    clsobj.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");

                    clsobj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");

                    objlst.Add(clsobj);
                    i++;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetOrderQtyList> GetCreditMemoForOrder(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditMemoForRetailOrder";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyList> objlst = new List<RetOrderQtyList>();
                while (dr.Read())
                {
                    RetOrderQtyList objPayment = new RetOrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.PODate = objBaseSqlManager.GetDateTime(dr, "PODate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objPayment.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    objPayment.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objPayment.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    objPayment.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    objPayment.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    objPayment.ReturnedOrderQtyID = objBaseSqlManager.GetInt64(dr, "ReturnedOrderQtyID");
                    objPayment.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    objPayment.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string CreateCreditMemo(List<RetOrderQtyList> data, long SessionUserID)
        {
            string CreditMemoNumber = "";
            string InvNum = "";
            long ReturnOrderID = 0;
            int cnt = 0;
            data = data.OrderByDescending(x => x.InvoiceNumber).ToList();
            string CreditMemoNo = "";
            decimal TCSTax = 0;
            foreach (var item in data)
            {
                RetReturnOrderMst om = new RetReturnOrderMst();
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

                    // 09 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not
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
                    // 09 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not

                    om.ReferenceNumber = item.ReferenceNumber;
                    using (VirakiEntities context = new VirakiEntities())
                    {
                        context.RetReturnOrderMsts.Add(om);
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
                    RetReturnOrderQntyMst obj = new RetReturnOrderQntyMst();
                    obj.ReturnOrderID = ReturnOrderID;
                    obj.OrderQtyID = item.OrderQtyID;
                    obj.ProductID = item.ProductID;
                    obj.CategoryTypeID = item.CategoryTypeID;
                    obj.ReturnedQuantity = item.ReturnedQuantity;
                    //UpdateOrderQty = Quantity - ReturnedQuantity UpdateCheckListQuantity
                    decimal UpdateQuantity = item.Quantity - item.ReturnedQuantity;
                    bool UpdateQuantityResponse = UpdateCheckListQuantity(item.OrderID, item.OrderQtyID, UpdateQuantity);
                    obj.ReturnedSaleRate = item.ReturnedSaleRate;
                    obj.BillDiscount = item.DiscountPer;
                    obj.CreditedFinalTotal = item.CreditedFinalTotal;
                    obj.TaxReversed = item.Tax;
                    obj.OrderDate = item.OrderDate;
                    obj.ProductQtyID = item.ProductQtyID;

                    // 09 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not
                    obj.TCSTax = TCSTax;
                    // 09 April 2021 Piyush Limbani add New Field for check TCS is Applicable or not

                    obj.CreatedBy = SessionUserID;
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = SessionUserID;
                    obj.UpdatedOn = DateTime.Now;
                    using (VirakiEntities context = new VirakiEntities())
                    {
                        context.RetReturnOrderQntyMsts.Add(obj);
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

            // 09 April 2021 Piyush Limbani Update ,TCSTaxAmount,GrandCreditedTotal
            ModelCreditedTotal CreditedDetail = GetCreditNoteTotalByCreditMemoNumber(CreditMemoNumber);
            if (CreditedDetail.ReturnOrderID != 0)
            {
                decimal TCSTaxAmount = ((CreditedDetail.CreditedTotal * CreditedDetail.TCSTax) / 100);
                decimal GrandCreditedTotal = TCSTaxAmount + CreditedDetail.CreditedTotal;
                UpdateCreditedTotal(CreditedDetail.CreditedTotal, TCSTaxAmount, GrandCreditedTotal, CreditedDetail.ReturnOrderID);
            }
            // 09 April 2021 Piyush Limbani

            return CreditMemoNumber;
        }

        // 09 April 2021 Piyush Limbani
        public decimal GetTCSTaxByInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            decimal TCSTax = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetTCSTaxByInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TCSTax = objBaseSqlManager.GetDecimal(dr, "TCSTax");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TCSTax;
            }
        }

        // 09 April 2021 Piyush Limbani
        public ModelCreditedTotal GetCreditNoteTotalByCreditMemoNumber(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetCreditNoteTotalByCreditMemoNumber";
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
                cmdGet.CommandText = "UpdateRetCreditedTotal";
                cmdGet.Parameters.AddWithValue("@ReturnOrderID", ReturnOrderID);
                cmdGet.Parameters.AddWithValue("@CreditedTotal", CreditedTotal);
                cmdGet.Parameters.AddWithValue("@TCSTaxAmount", TCSTaxAmount);
                cmdGet.Parameters.AddWithValue("@GrandCreditedTotal", GrandCreditedTotal);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }
        // 09 April 2021 Piyush Limbani


        private string GetlastCreditMemoNo()
        {
            var lstdata = GetLastCreditMemoNumber();
            // string FinancialYearString = ConfigurationManager.AppSettings["FinancialYearString"];
            string FinancialYearString = "/" + DateTimeExtensions.FromFinancialYear(DateTime.Now).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(DateTime.Now).ToString("yy");
            string CreditMemoNumber = "";
            if (!string.IsNullOrEmpty(lstdata.CreditMemoNumber))
            {
                string number = lstdata.CreditMemoNumber.Substring(2, 5);
                long incr = Convert.ToInt64(number) + 1;
                CreditMemoNumber = "CM" + incr.ToString().PadLeft(5, '0') + FinancialYearString;
            }
            else
            {
                CreditMemoNumber = "CM00001" + FinancialYearString;
            }
            return CreditMemoNumber;
        }

        public RetOrderQtyList GetLastCreditMemoNumber()
        {
            RetOrderQtyList objOrder = new RetOrderQtyList();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastRetailCreditMemoNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.CreditMemoNumber = objBaseSqlManager.GetTextValue(dr, "CreditMemoNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<RetOrderListResponse> GetAllReturnedOrderList(RetOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllReturnedRetOrderList";
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
                List<RetOrderListResponse> objlst = new List<RetOrderListResponse>();
                while (dr.Read())
                {
                    RetOrderListResponse objPayment = new RetOrderListResponse();
                    objPayment.ReturnOrderID = objBaseSqlManager.GetInt64(dr, "ReturnOrderID");
                    objPayment.CreditMemoNumber = objBaseSqlManager.GetTextValue(dr, "CreditMemoNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        private List<RetOrderQtyList> GetReturnedQtyDetailsByOrderQtyID(long OrderQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetReturnedQtyDetailsByOrderQtyIDForRetail";
                cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyList> lstOrderQty = new List<RetOrderQtyList>();
                while (dr.Read())
                {
                    RetOrderQtyList obj = new RetOrderQtyList();
                    obj.ReturnedOrderQtyID = objBaseSqlManager.GetInt64(dr, "ReturnedOrderQtyID");
                    obj.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    obj.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    obj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    lstOrderQty.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();

                return lstOrderQty;
            }
        }

        public RetOrderListResponse GetLastRetailOrderID()
        {
            RetOrderListResponse objOrder = new RetOrderListResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastRetailOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<RetCustomerListResponse> GetAllRetCustomerName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetCustomerName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomerListResponse> lstCustomer = new List<RetCustomerListResponse>();
                while (dr.Read())
                {
                    RetCustomerListResponse objCustomer = new RetCustomerListResponse();
                    objCustomer.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    lstCustomer.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstCustomer;
            }
        }

        public List<RetProductListResponse> GetAllRetProductName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetProductQtyName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetProductListResponse> lstPeoduct = new List<RetProductListResponse>();
                while (dr.Read())
                {
                    RetProductListResponse objProduct = new RetProductListResponse();
                    objProduct.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    objProduct.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objProduct.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    lstPeoduct.Add(objProduct);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetGetSellPrice> GetAutoCompleteSellPrice(long ProductID, long Quantity)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetAutoCompleteSellPrice";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID.ToString());
                cmdGet.Parameters.AddWithValue("@Quantity", Quantity.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetGetSellPrice> objlst = new List<RetGetSellPrice>();
                while (dr.Read())
                {
                    RetGetSellPrice objOrder = new RetGetSellPrice();
                    objOrder.SellPrice = objBaseSqlManager.GetDecimal(dr, "SellPrice");
                    objlst.Add(objOrder);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetRetTax GetTaxForRetCustomer(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxForRetCustomer";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetRetTax objOrder = new GetRetTax();
                while (dr.Read())
                {
                    objOrder.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objOrder.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objOrder.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objOrder.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<RetGetUnitRate> GetAutoCompleteRetProduct(long Prefix)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteRetProduct";
                cmdGet.Parameters.AddWithValue("@ProductID", Prefix.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetGetUnitRate> objlst = new List<RetGetUnitRate>();
                while (dr.Read())
                {
                    RetGetUnitRate objOrder = new RetGetUnitRate();
                    objOrder.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objlst.Add(objOrder);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetRetSellPrice GetRetailOrderDetails(long ProductQtyID, string Tax, long CustomerID, long CustomerGroupID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailOrderDetails";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID.ToString());
                cmdGet.Parameters.AddWithValue("@Tax", Tax.ToString());
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID.ToString());
                cmdGet.Parameters.AddWithValue("@CustomerGroupID", CustomerGroupID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetRetSellPrice objOrder = new GetRetSellPrice();
                while (dr.Read())
                {
                    objOrder.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objOrder.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objOrder.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");
                    objOrder.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    objOrder.DiscountPer = objBaseSqlManager.GetDecimal(dr, "DiscountPer");
                    objOrder.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objOrder.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objOrder.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objOrder.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");

                    //Add By Dhruvik 24-02-2023
                    objOrder.MarginPer = objBaseSqlManager.GetDecimal(dr, "MarginPer");
                    //Add By Dhruvik 24-02-2023
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<RetOrderQtyInvoiceList> GetInvoiceForOrderPrint(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForRetOrderPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyInvoiceList> objlst = new List<RetOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    RetOrderQtyInvoiceList objPayment = new RetOrderQtyInvoiceList();
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }

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


                    // 26 March 2021 Sonal Gandhi

                    objPayment.From_Name = "VIRAKI BROTHERS";
                    objPayment.From_Address1 = "280/282, N.N. St.,Masjid, Mumbai 400009,Maharashtra";
                    objPayment.From_GSTIN = "27AAAFV3761F1Z7";//"27AAAFV3761F1Z7";
                    objPayment.From_Place = objBaseSqlManager.GetTextValue(dr, "From_Place");
                    objPayment.From_PinCode = objBaseSqlManager.GetInt32(dr, "From_PinCode");
                    objPayment.From_StateCode = "27";
                    objPayment.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objPayment.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objPayment.PanCard = objBaseSqlManager.GetTextValue(dr, "PanCard");

                    objPayment.TaxNo2 = objBaseSqlManager.GetTextValue(dr, "TaxNo2");
                    objPayment.BillingCity = objBaseSqlManager.GetTextValue(dr, "BillingCity");
                    objPayment.BillingState = objBaseSqlManager.GetTextValue(dr, "BillingState");
                    objPayment.BillingAreaName = objBaseSqlManager.GetTextValue(dr, "BillingAreaName");
                    objPayment.BillingStateCode = objBaseSqlManager.GetTextValue(dr, "BillingStateCode");

                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetOrderQtyInvoiceList> GetInvoiceForOrderItemPrint(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForRetOrderItemPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyInvoiceList> objlst = new List<RetOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    RetOrderQtyInvoiceList objPayment = new RetOrderQtyInvoiceList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.EInvoiceNumber = objPayment.InvoiceNumber;
                    objPayment.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objPayment.InvoiceNumber = objPayment.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objPayment.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objPayment.InvoiceDate).ToString("yy");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    objPayment.TaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmount"), 2);
                    objPayment.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    objPayment.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");

                    objPayment.TCSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount");


                    objPayment.InvTotal = objBaseSqlManager.GetDecimal(dr, "InvTotal");

                    objPayment.IsShow = objBaseSqlManager.GetBoolean(dr, "IsShow");

                    if (objPayment.IsShow == false)
                    {
                        objPayment.ArticleCode = null;
                    }
                    else
                    {
                        objPayment.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                    }


                    //bool Id = false;
                    //if (Id == false)
                    //{
                    //    objPayment.ArticleCode = null;
                    //}
                    //else
                    //{
                    //    objPayment.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");
                    //}


                    // objPayment.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");

                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }


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

                    //Add By Dhruvik
                    objPayment.Margin = objBaseSqlManager.GetDecimal(dr, "Margin");
                    objPayment.SPDiscount = objBaseSqlManager.GetDecimal(dr, "SPDiscount");
                    //Add By Dhruvik

                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetOrderPackList> GetOrderPackListForPrint(string id, string date)
        {
            string item = "";
            //long productid = 0;
            //decimal Total = 0;

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderPackListForPrintNew";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderPackList> lstPeoduct = new List<RetOrderPackList>();
                while (dr.Read())
                {
                    RetOrderPackList obj = new RetOrderPackList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.SKU = objBaseSqlManager.GetTextValue(dr, "SKU");
                    obj.MRP = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductMRP"), 2);
                    obj.GramPerKG = Math.Round(objBaseSqlManager.GetDecimal(dr, "GramPerKG"), 2);
                    obj.ProductBarcode = objBaseSqlManager.GetTextValue(dr, "ProductBarcode");
                    obj.ContentValue = objBaseSqlManager.GetTextValue(dr, "ContentValue");
                    obj.NutritionValue = objBaseSqlManager.GetTextValue(dr, "NutritionValue");
                    obj.QuantityPackage = objBaseSqlManager.GetInt32(dr, "QuantityPackage");
                    obj.QtyAvailable = objBaseSqlManager.GetInt32(dr, "QtyAvailable");
                    obj.QtyPacked = objBaseSqlManager.GetInt32(dr, "QtyPacked");
                    obj.Section = objBaseSqlManager.GetInt32(dr, "Section");
                    obj.TotalQuantityPackage = objBaseSqlManager.GetInt32(dr, "TotalQuantityPackage");
                    obj.TotalQtyAvailable = objBaseSqlManager.GetInt32(dr, "TotalQtyAvailable");
                    obj.TotalQtyPacked = objBaseSqlManager.GetInt32(dr, "TotalQtyPacked");
                    obj.TotalSum = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalSum"), 2);
                    obj.PouchSize = objBaseSqlManager.GetInt64(dr, "PouchSize");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");

                    //if ( productid == 0 || obj.ProductID != productid)
                    //{
                    //    Total = obj.TotalSum;
                    //    productid = obj.ProductID;
                    //}
                    //else
                    //{
                    //    Total += obj.TotalSum;    
                    //   // productid = 0;               
                    //}

                    if (item != obj.ProductName)
                    {
                        item = obj.ProductName;
                        obj.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "Total"), 2);
                    }
                    else
                    {
                        obj.Total = 0;
                    }
                    obj.CategoryID = objBaseSqlManager.GetInt64(dr, "CategoryID");

                    // 08 Feb, 2022 Piyush Limbani
                    obj.Protein = objBaseSqlManager.GetTextValue(dr, "Protein");
                    obj.Fat = objBaseSqlManager.GetTextValue(dr, "Fat");
                    obj.Carbohydrate = objBaseSqlManager.GetTextValue(dr, "Carbohydrate");
                    obj.TotalEnergy = objBaseSqlManager.GetTextValue(dr, "TotalEnergy");
                    obj.Information = objBaseSqlManager.GetTextValue(dr, "Information");

                    // 21-06-2022
                    obj.IsPackDone = objBaseSqlManager.GetBoolean(dr, "IsPackDone");

                    // 08 Feb, 2022 Piyush Limbani                    
                    // UpdatePrintPackList(id, date);
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }



        //private void UpdatePrintPackList(string id, string date)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "UpdatePrintPackList";
        //    cmdGet.Parameters.AddWithValue("@id", id);
        //    cmdGet.Parameters.AddWithValue("@date", date);
        //    objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //    objBaseSqlManager.ForceCloseConnection();
        //}

        public bool UpdatePrintPackList(string date, string OrderID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdatePrintPackList";
                    cmdGet.Parameters.AddWithValue("@id", OrderID);
                    cmdGet.Parameters.AddWithValue("@date", date);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public List<RetOrderSummaryList> GetOrderSummaryListForPrint(string id, string date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderSummaryListForPrint";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderSummaryList> lstPeoduct = new List<RetOrderSummaryList>();
                while (dr.Read())
                {
                    RetOrderSummaryList obj = new RetOrderSummaryList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.SKU = objBaseSqlManager.GetTextValue(dr, "SKU");
                    obj.QuantityPackage = objBaseSqlManager.GetInt32(dr, "QuantityPackage");
                    obj.Section = objBaseSqlManager.GetInt32(dr, "Section");
                    obj.Total = objBaseSqlManager.GetInt32(dr, "Total");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetOrderSummaryList> GetOrderSummaryListProductsForPrint(string id, string date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderSummaryListProductsForPrint";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderSummaryList> lstPeoduct = new List<RetOrderSummaryList>();
                while (dr.Read())
                {
                    RetOrderSummaryList obj = new RetOrderSummaryList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<int> GetPackListSections()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPackListSections";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<int> lstPeoduct = new List<int>();
                while (dr.Read())
                {
                    lstPeoduct.Add(objBaseSqlManager.GetInt32(dr, "Section"));
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<int> GetSummaryListSections()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSummaryListSections";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<int> lstPeoduct = new List<int>();
                while (dr.Read())
                {
                    lstPeoduct.Add(objBaseSqlManager.GetInt32(dr, "Section"));
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetOrderPackList> GetOrderPackListForExport(string id, string date, int Section, string OrderID)
        {
            string item = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.Parameters.AddWithValue("@Section", Section);
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.CommandText = "GetOrderPackListForExport";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderPackList> lstPeoduct = new List<RetOrderPackList>();
                while (dr.Read())
                {
                    RetOrderPackList obj = new RetOrderPackList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.SKU = objBaseSqlManager.GetTextValue(dr, "SKU");
                    obj.QuantityPackage = objBaseSqlManager.GetInt32(dr, "QuantityPackage");
                    obj.QtyAvailable = objBaseSqlManager.GetInt32(dr, "QtyAvailable");
                    obj.QtyPacked = objBaseSqlManager.GetInt32(dr, "QtyPacked");
                    obj.Section = objBaseSqlManager.GetInt32(dr, "Section");
                    obj.TotalQuantityPackage = objBaseSqlManager.GetInt32(dr, "TotalQuantityPackage");
                    obj.TotalQtyAvailable = objBaseSqlManager.GetInt32(dr, "TotalQtyAvailable");
                    obj.TotalQtyPacked = objBaseSqlManager.GetInt32(dr, "TotalQtyPacked");
                    if (item != obj.ProductName)
                    {
                        item = obj.ProductName;
                        obj.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "Total"), 2);
                    }
                    else
                    {
                        obj.Total = 0;
                    }
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetOrderSummaryList> GetOrderSummaryListForExport(string id, string date, int Section)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.Parameters.AddWithValue("@Section", Section);
                cmdGet.CommandText = "GetOrderSummaryListForExport";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderSummaryList> lstPeoduct = new List<RetOrderSummaryList>();
                while (dr.Read())
                {
                    RetOrderSummaryList obj = new RetOrderSummaryList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.SKU = objBaseSqlManager.GetTextValue(dr, "SKU");
                    obj.QuantityPackage = objBaseSqlManager.GetInt32(dr, "QuantityPackage");
                    obj.Section = objBaseSqlManager.GetInt32(dr, "Section");
                    obj.Total = objBaseSqlManager.GetInt32(dr, "Total");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetOrderPackListResponse> GetOrderPackList(DateTime date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderPackList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderPackListResponse> lstPeoduct = new List<RetOrderPackListResponse>();
                while (dr.Read())
                {
                    RetOrderPackListResponse obj = new RetOrderPackListResponse();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.OrderNumber = objBaseSqlManager.GetTextValue(dr, "OrderNumber");
                    obj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.Status = objBaseSqlManager.GetTextValue(dr, "Status");
                    obj.IsPrintPackList = objBaseSqlManager.GetBoolean(dr, "IsPrintPackList");
                    obj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetOrderSummaryListResponse> GetOrderSummaryList(DateTime date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderSummaryList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderSummaryListResponse> lstPeoduct = new List<RetOrderSummaryListResponse>();
                while (dr.Read())
                {
                    RetOrderSummaryListResponse obj = new RetOrderSummaryListResponse();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.OrderNumber = objBaseSqlManager.GetTextValue(dr, "OrderNumber");
                    obj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.Status = objBaseSqlManager.GetTextValue(dr, "Status");
                    obj.Section = objBaseSqlManager.GetInt32(dr, "Section");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public RetOrderViewModel GetRetailOrderDetailsByOrderID(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.CommandText = "GetRetailOrderDetailsByOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetOrderViewModel objM = new RetOrderViewModel();
                List<RetOrderQtyViewModel> lstOrderQty = new List<RetOrderQtyViewModel>();
                while (dr.Read())
                {
                    objM.DeActiveCustomer = objBaseSqlManager.GetBoolean(dr, "DeActiveCustomer");
                    objM.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objM.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objM.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objM.CustomerGroupID = objBaseSqlManager.GetInt64(dr, "CustomerGroupID");
                    objM.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objM.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objM.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objM.Tax = objBaseSqlManager.GetTextValue(dr, "Tax");
                    objM.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objM.PODate = objBaseSqlManager.GetDateTime(dr, "PODate");
                    objM.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    objM.OrderNote = objBaseSqlManager.GetTextValue(dr, "OrderNote");
                    objM.IsFinalised = objBaseSqlManager.GetBoolean(dr, "IsFinalised");

                    objM.IsTCSApplicable = objBaseSqlManager.GetBoolean(dr, "IsTCSApplicable");
                    if (objM.IsTCSApplicable == true)
                    {
                        objM.IsTCSApplicablestr = "true";
                    }
                    else
                    {
                        objM.IsTCSApplicablestr = "false";
                    }
                    objM.GSTNumber = objBaseSqlManager.GetTextValue(dr, "GSTNumber");

                    RetOrderQtyViewModel obj = new RetOrderQtyViewModel();
                    obj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.Tax = objBaseSqlManager.GetDecimal(dr, "QtyTax");
                    obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "QtyTaxAmt");
                    obj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    obj.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    obj.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");
                    obj.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    obj.DiscountPer = objBaseSqlManager.GetDecimal(dr, "DiscountPer");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.ArticleCode = objBaseSqlManager.GetTextValue(dr, "ArticleCode");

                    //Add By Dhruvik
                    obj.Margin = objBaseSqlManager.GetDecimal(dr, "Margin");
                    obj.SPDiscount = objBaseSqlManager.GetDecimal(dr, "SPDiscount");
                    obj.TexableAmount = objBaseSqlManager.GetDecimal(dr, "TexableAmount");
                    //Add By Dhruvik

                    lstOrderQty.Add(obj);
                }
                objM.lstOrderQty = lstOrderQty;
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        public List<string> GetListOfInvoice(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.CommandText = "GetRetOrderInvoiceNumbersByOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetOrderViewModel objM = new RetOrderViewModel();
                List<string> lstOrderQty = new List<string>();
                while (dr.Read())
                {
                    string invoceNoold = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    DateTime CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    string invoceNonew = invoceNoold + "/" + DateTimeExtensions.FromFinancialYear(CreatedOn).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(CreatedOn).ToString("yy");
                    lstOrderQty.Add(invoceNonew);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstOrderQty;
            }
        }

        //public List<RetOrderQtyInvoiceList> GetCreditMemoInvoiceForOrderItemPrint(string CreditMemoNumber)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetCreditMemoInvoiceForRetOrderItemPrint";
        //    cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    List<RetOrderQtyInvoiceList> objlst = new List<RetOrderQtyInvoiceList>();
        //    while (dr.Read())
        //    {
        //        RetOrderQtyInvoiceList objPayment = new RetOrderQtyInvoiceList();
        //        objPayment.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
        //        objPayment.BillNo = objBaseSqlManager.GetTextValue(dr, "BillNo") + "/" + DateTimeExtensions.FromFinancialYear(objPayment.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objPayment.InvoiceDate).ToString("yy");
        //        objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
        //        objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
        //        objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
        //        objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
        //        objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
        //        objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
        //        objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
        //        objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
        //        objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
        //        objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
        //        objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
        //        objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
        //        objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
        //        objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
        //        objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn"); //Convert.ToDateTime(DateTime.Now);
        //        objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
        //        objPayment.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
        //        objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
        //        objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
        //        objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
        //        objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
        //        objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
        //        objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
        //        objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
        //        objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
        //        objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
        //        objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
        //        objPayment.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
        //        objPayment.TaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmount"), 2);
        //        objPayment.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
        //        objPayment.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");
        //        objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
        //        objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
        //        objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
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




        public List<RetOrderQtyInvoiceList> GetCreditMemoInvoiceForOrderItemPrint(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetCreditMemoInvoiceForRetOrderItemPrint";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderQtyInvoiceList> objlst = new List<RetOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    RetOrderQtyInvoiceList objPayment = new RetOrderQtyInvoiceList();
                    objPayment.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objPayment.BillNo = objBaseSqlManager.GetTextValue(dr, "BillNo") + "/" + DateTimeExtensions.FromFinancialYear(objPayment.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objPayment.InvoiceDate).ToString("yy");
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.LBTNo = objBaseSqlManager.GetTextValue(dr, "LBTNo");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.ContactNumber = objBaseSqlManager.GetTextValue(dr, "ContactNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn"); //Convert.ToDateTime(DateTime.Now);
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    objPayment.City = objBaseSqlManager.GetTextValue(dr, "City");
                    objPayment.State = objBaseSqlManager.GetTextValue(dr, "State");
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                    objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");
                    objPayment.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    objPayment.TaxAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TaxAmount"), 2);
                    objPayment.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    objPayment.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");
                    objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");

                    // 09 April 2021 Piyush Limbani
                    objPayment.TCSTax = objBaseSqlManager.GetDecimal(dr, "TCSTax");
                    objPayment.TCSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount");
                    objPayment.GrandCreditedTotal = objBaseSqlManager.GetDecimal(dr, "GrandCreditedTotal");
                    objPayment.GrandCreditedTotal = Math.Round(objPayment.GrandCreditedTotal);
                    objPayment.InvTotal = objBaseSqlManager.GetDecimal(dr, "InvTotal");
                    objPayment.Margin = objBaseSqlManager.GetDecimal(dr, "Margin");
                    // 09 April 2021 Piyush Limbani

                    if (!string.IsNullOrEmpty(objPayment.TaxNo))
                    {
                        objPayment.StateCode = objPayment.TaxNo.Substring(0, 2);
                    }
                    else
                    {
                        objPayment.StateCode = "N/A";
                    }


                    // 6 April 2021 Sonal Gandhi
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


        public List<Product_Mst> GetOrderPackListForLabel(string id, string date, int Section, string orderid)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.Parameters.AddWithValue("@Section", Section);
                cmdGet.Parameters.AddWithValue("@orderid", orderid);
                cmdGet.CommandText = "GetOrderPackListForLabel";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<Product_Mst> lstPeoduct = new List<Product_Mst>();
                while (dr.Read())
                {
                    Product_Mst obj = new Product_Mst();
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.ProductPrice = objBaseSqlManager.GetInt32(dr, "QuantityPackage");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<ProductBarcode> GetOrderPackListForBarcode(string id, string date, int Section, string orderid)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.Parameters.AddWithValue("@Section", Section);
                cmdGet.Parameters.AddWithValue("@orderid", orderid);
                cmdGet.CommandText = "GetOrderPackListForBarcode";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ProductBarcode> lstPeoduct = new List<ProductBarcode>();
                while (dr.Read())
                {
                    ProductBarcode obj = new ProductBarcode();
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.QTY = objBaseSqlManager.GetTextValue(dr, "SKU");
                    obj.NoofBarcode = objBaseSqlManager.GetDecimal(dr, "QuantityPackage");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.MRP = objBaseSqlManager.GetTextValue(dr, "ProductMRP");
                    obj.Productbarcode = objBaseSqlManager.GetTextValue(dr, "ProductBarcode");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public bool DeleteOrder(int OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteOrder";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<RetInvoiceForExcelList> GetInvoiceForExcel(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForExcel";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetInvoiceForExcelList> objlst = new List<RetInvoiceForExcelList>();
                while (dr.Read())
                {
                    RetInvoiceForExcelList obj = new RetInvoiceForExcelList();
                    obj.SupplyType = "Outward";
                    obj.SubType = "Supply";
                    obj.DocType = "Tax Invoice"; ;
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.DocDate1 = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    obj.DocDate = obj.DocDate1.ToString("dd/MM/yyyy");
                    //obj.DocNo = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(obj.DocDate1).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.DocDate1).ToString("yy");
                    obj.DocNo = obj.InvoiceNumber;
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
                    obj.Unit = "PACKS";
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
                            obj.TaxRate = obj.SGSTTaxRate + "+" + obj.CGSTTaxRate + "+" + obj.IGSTTaxRate + "+" + obj.CESSTaxRate + "+" + obj.CESSTaxRate2;
                        }
                        else
                        {
                            obj.DivTaxRate = obj.TotalTaxRate / 2;
                            obj.SGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.CGSTTaxRate = Math.Round(obj.DivTaxRate);
                            obj.IGSTTaxRate = 0;
                            obj.CESSTaxRate = 0;
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
                    obj.TransDocNo = obj.InvoiceNumber;
                    // obj.TransDocNo = "";
                    obj.TransDate1 = DateTime.Now;
                    obj.TransDate = obj.TransDate1.ToString("dd/MM/yyyy");
                    obj.VehicleNo = "";
                    obj.VehicleType = "Regular";

                    obj.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                    obj.InvoiceNumberWithDate = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(obj.DocDate1).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.DocDate1).ToString("yy");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<GetRetUnitRate> GetAutoCompleteProduct(long ProductQtyID)
        {

            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetAutoCompleteProduct";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID.ToString());

                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetRetUnitRate> objlst = new List<GetRetUnitRate>();
                while (dr.Read())
                {
                    GetRetUnitRate objOrder = new GetRetUnitRate();
                    objOrder.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
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

        public GetRetSellPrice1 GetAutoCompleteSellPrice(long ProductQtyID, string Tax)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetAutoCompleteSellPrice";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID.ToString());
                cmdGet.Parameters.AddWithValue("@Tax", Tax.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetRetSellPrice1 objOrder = new GetRetSellPrice1();
                while (dr.Read())
                {
                    objOrder.SellPrice = objBaseSqlManager.GetDecimal(dr, "SellPrice");
                    objOrder.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    objOrder.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objOrder.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        //07-06-2018
        public long SaveChallan(RetChallanViewModel data)
        {
            RetChallanMst obj = new RetChallanMst();
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
                    context.RetChallanMsts.Add(obj);
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
                            RetChallanQtyMst objOrderQty = new RetChallanQtyMst();
                            objOrderQty.ChallanID = obj.ChallanID;
                            objOrderQty.ChallanQtyID = item.ChallanQtyID;
                            objOrderQty.ProductID = item.ProductID;
                            objOrderQty.ProductQtyID = item.ProductQtyID;
                            objOrderQty.Quantity = item.Quantity;
                            objOrderQty.ProductPrice = item.ProductPrice;
                            objOrderQty.SaleRate = item.SaleRate;
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
                                context.RetChallanQtyMsts.Add(objOrderQty);
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
                            RetChallanMst data1 = context.RetChallanMsts.Where(x => x.ChallanID == obj.ChallanID).FirstOrDefault();
                            if (data != null)
                            {
                                context.RetChallanMsts.Remove(data1);
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

        private void UpdateChallanTotal(decimal ChallanTotal, string ChallanNo, long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetChallanTotal";
                cmdGet.Parameters.AddWithValue("@ChallanNo", ChallanNo);
                cmdGet.Parameters.AddWithValue("@ChallanTotal", ChallanTotal);
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
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
                ChallanNumber = "RDC" + incr.ToString().PadLeft(5, '0') + FinancialYearString;
            }
            else
            {
                ChallanNumber = "RDC00001" + FinancialYearString;
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
                cmdGet.CommandText = "GetLastRetChallanNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<RetChallanQtyInvoiceList> GetInvoiceForChallanPrint(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForRetChallanPrint";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanQtyInvoiceList> objlst = new List<RetChallanQtyInvoiceList>();
                while (dr.Read())
                {
                    RetChallanQtyInvoiceList objPayment = new RetChallanQtyInvoiceList();
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
                cmdGet.CommandText = "GetRetChallanNumbersByChallanID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
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

        public List<RetChallanQtyInvoiceList> GetInvoiceForChallanItemPrint(long ChallanID, long CategoryID, string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForRetChallanItemPrint";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanQtyInvoiceList> objlst = new List<RetChallanQtyInvoiceList>();
                while (dr.Read())
                {
                    RetChallanQtyInvoiceList objPayment = new RetChallanQtyInvoiceList();
                    objPayment.ChallanID = objBaseSqlManager.GetInt64(dr, "ChallanID");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.ChallanNumber = objBaseSqlManager.GetTextValue(dr, "ChallanNumber");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
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

        public List<RetChallanListResponse> GetAllChallanList(RetChallanListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetChallanList";
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
                List<RetChallanListResponse> objlst = new List<RetChallanListResponse>();
                while (dr.Read())
                {
                    RetChallanListResponse objPayment = new RetChallanListResponse();
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

        public List<RetChallanQtyList> GetInvoiceForChallan(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForRetChallan";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanQtyList> objlst = new List<RetChallanQtyList>();
                while (dr.Read())
                {
                    RetChallanQtyList objPayment = new RetChallanQtyList();
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

        public List<RetChallanForExcelList> GetChallanForExcel(long ChallanID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetChallanForExcelRetail";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanForExcelList> objlst = new List<RetChallanForExcelList>();
                while (dr.Read())
                {
                    RetChallanForExcelList obj = new RetChallanForExcelList();
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
                    obj.Unit = "Pieces";
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
                cmdGet.CommandText = "DeleteRetailCreditMemo";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public bool UpdatePrintStatus(long OrderID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdatePrintStatus";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        // 06/12/2018
        public List<RetOrderPackListResponse> GetOrderCheckList(DateTime date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderCheckList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderPackListResponse> lstPeoduct = new List<RetOrderPackListResponse>();
                long CustomerID = 0;
                char Tag = 'A';
                char NewTag = ' ';
                while (dr.Read())
                {
                    RetOrderPackListResponse obj = new RetOrderPackListResponse();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.OrderNumber = objBaseSqlManager.GetTextValue(dr, "OrderNumber");
                    obj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    if (CustomerID == 0 || obj.CustomerID != CustomerID)
                    {
                        NewTag = Tag;
                        obj.Tag = Tag;
                        CustomerID = obj.CustomerID;
                    }
                    else
                    {
                        obj.Tag = (char)(NewTag + 1);
                        NewTag = obj.Tag;
                    }
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.Status = objBaseSqlManager.GetTextValue(dr, "Status");
                    obj.IsPrintPackList = false;
                    obj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    obj.UpdateQuantity = objBaseSqlManager.GetDecimal(dr, "UpdateQuantity");

                    obj.IsCheckDone = objBaseSqlManager.GetBoolean(dr, "IsCheckDone");

                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<RetOrderPackList> GetOrderCheckListForLabelPrint(string id, string date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetOrderCheckListForLabelPrint";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetOrderPackList> lstPeoduct = new List<RetOrderPackList>();
                while (dr.Read())
                {
                    RetOrderPackList obj = new RetOrderPackList();
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    // obj.CategoryName = objBaseSqlManager.GetTextValue(dr, "CategoryName");
                    obj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    obj.SKU = objBaseSqlManager.GetTextValue(dr, "SKU");
                    obj.QuantityPackage = objBaseSqlManager.GetInt32(dr, "QuantityPackage");
                    obj.UpdateQuantityPackage = objBaseSqlManager.GetInt32(dr, "UpdateQuantityPackage");
                    obj.TotalQuantityPackage = objBaseSqlManager.GetInt32(dr, "TotalQuantityPackage");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    obj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.OrderID = objBaseSqlManager.GetTextValue(dr, "OrderID");
                    obj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    obj.AreaID = objBaseSqlManager.GetInt64(dr, "AreaID");
                    obj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");

                    //obj.TotalKG = objBaseSqlManager.GetDecimal(dr, "TotalKG");

                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public bool UpdateRetOrderQuantityForPrintLabel(long ID, decimal Quantity)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                var enlsit = context.RetOrderQtyMsts.Where(i => i.OrderQtyID == ID).ToList().FirstOrDefault();
                decimal UpdateQuantity = enlsit.UpdateQuantity - Quantity;

                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateRetOrderQuantityForPrintLabel";
                    cmdGet.Parameters.AddWithValue("@ID", ID);
                    cmdGet.Parameters.AddWithValue("@Quantity", UpdateQuantity);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public RetPackSummaryResponse GetLastBagNumberByOrderID(long OrderID)
        {
            RetPackSummaryResponse objOrder = new RetPackSummaryResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastBagNumberByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.Bag = objBaseSqlManager.GetInt32(dr, "Bag");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public RetPackSummaryResponse GetLastTrayNumberByOrderID(long OrderID)
        {
            RetPackSummaryResponse objOrder = new RetPackSummaryResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastTrayNumberByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.Tray = objBaseSqlManager.GetInt32(dr, "Tray");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public RetPackSummaryResponse GetLastZablaNumberByOrderID(long OrderID)
        {
            RetPackSummaryResponse objOrder = new RetPackSummaryResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastZablaNumberByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.Zabla = objBaseSqlManager.GetInt32(dr, "Zabla");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public RetPackSummaryResponse GetLastBoxNumberByOrderID(long OrderID)
        {
            RetPackSummaryResponse objOrder = new RetPackSummaryResponse();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastBoxNumberByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.Box = objBaseSqlManager.GetInt32(dr, "Box");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public long AddRetPackSummaryDetail(long OrderID, long OrderQtyID, long CustomerID, long AreaID, string ProductName, int Bag, int Tray, int Zabla, int Box, decimal TotalKG, long CreatedBy, long PackSummaryID, DateTime OrderDate, string FinalTag)
        {

            using (VirakiEntities context = new VirakiEntities())
            {
                RetPackSummary_Mst Obj = new RetPackSummary_Mst();
                Obj.PackSummaryID = PackSummaryID;
                Obj.OrderID = OrderID;
                Obj.OrderQtyID = OrderQtyID;
                Obj.CustomerID = CustomerID;
                Obj.AreaID = AreaID;
                Obj.OrderDate = OrderDate;
                Obj.ProductName = ProductName;
                Obj.Bag = Bag;
                Obj.Tray = Tray;
                Obj.Zabla = Zabla;
                Obj.Box = Box;
                Obj.TotalKG = TotalKG;
                Obj.Tag = FinalTag;
                Obj.CreatedBy = CreatedBy;
                Obj.CreatedOn = DateTime.Now;
                Obj.IsDelete = false;
                context.RetPackSummary_Mst.Add(Obj);
                context.SaveChanges();
                return Obj.PackSummaryID;
                // return true;
            }
        }

        public bool AddRetPackSummaryQtyDetail(long PackSummaryID, long OrderID, long OrderQtyID, long ProductID, long ProductQtyID, decimal Quantity, long CreatedBy)
        {

            using (VirakiEntities context = new VirakiEntities())
            {
                RetPackSummaryQty_Mst Obj = new RetPackSummaryQty_Mst();
                Obj.PackSummaryID = PackSummaryID;
                Obj.OrderQtyID = OrderQtyID;
                Obj.ProductID = ProductID;
                Obj.ProductQtyID = ProductQtyID;
                Obj.Quantity = Quantity;
                Obj.CreatedBy = CreatedBy;
                Obj.CreatedOn = DateTime.Now;
                Obj.IsDelete = false;
                context.RetPackSummaryQty_Mst.Add(Obj);
                context.SaveChanges();
                return true;
            }
        }

        public List<RetCustomeDetail> GetOrderCustomerDetail(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOrderCustomerDetail";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetCustomeDetail> objlst = new List<RetCustomeDetail>();
                while (dr.Read())
                {
                    string FullInvoiceNumber = "";
                    List<uint> lst = new List<uint>();
                    string FullInvoiceNumberstr = string.Empty;
                    RetCustomeDetail objCustomer = new RetCustomeDetail();
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    objCustomer.GrandTotalKG = objBaseSqlManager.GetDecimal(dr, "GrandTotalKG");
                    objCustomer.InvoiceDate = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                    objCustomer.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    string[] invoicenumbers = objCustomer.InvoiceNumber.Split(',');
                    foreach (var item in invoicenumbers)
                    {
                        string InvoiceNumber = item;
                        FullInvoiceNumber = InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(objCustomer.InvoiceDate).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(objCustomer.InvoiceDate).ToString("yy");
                        FullInvoiceNumberstr = FullInvoiceNumberstr + "," + FullInvoiceNumber;
                    }
                    objCustomer.FullInvoiceNumber = FullInvoiceNumberstr.TrimStart(',');
                    objCustomer.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetPackSummary> GetRetPackSummaryByOrderID(long OrderID)
        {
            // decimal GrandTotalKG = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetPackSummaryByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetPackSummary> objlst = new List<RetPackSummary>();
                int RowNumber = 1;
                while (dr.Read())
                {
                    RetPackSummary objCustomer = new RetPackSummary();
                    objCustomer.Tag = objBaseSqlManager.GetTextValue(dr, "Tag");
                    objCustomer.Bag = objBaseSqlManager.GetTextValue(dr, "SrNo") + " " + objCustomer.Tag;
                    objCustomer.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objCustomer.TotalKG = objBaseSqlManager.GetDecimal(dr, "TotalKG");
                    objCustomer.RowNumber = RowNumber;
                    //GrandTotalKG += objCustomer.TotalKG;               
                    objlst.Add(objCustomer);
                    RowNumber = RowNumber + 1;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateCheckListQuantity(long OrderID, long OrderQtyID, decimal UpdateQuantity)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateCheckListQuantity";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
                cmdGet.Parameters.AddWithValue("@UpdateQuantity", UpdateQuantity);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public RetPackTotal GetRetPrintTotalBagByOrderID(long OrderID)
        {
            RetPackTotal objlst = new RetPackTotal();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPrintTotalCount";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);

                while (dr.Read())
                {
                    //RetPackTotal objCustomer = new RetPackTotal();              
                    objlst.Bag = objBaseSqlManager.GetInt32(dr, "Bag");
                    objlst.Bagstr = "Bag" + "-" + objlst.Bag;
                    objlst.Box = objBaseSqlManager.GetInt32(dr, "Box");
                    objlst.Boxstr = "Box" + "-" + objlst.Box;
                    objlst.Zabla = objBaseSqlManager.GetInt32(dr, "Zabla");
                    objlst.Zablastr = "Zabla" + "-" + objlst.Zabla;
                    objlst.Tray = objBaseSqlManager.GetInt32(dr, "Tray");
                    objlst.Traystr = "Tray" + "-" + objlst.Tray;
                    // objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetPackSummary> GetPackDetailListByOrderID(string OrderID, string date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPackDetailListByOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetPackSummary> objlst = new List<RetPackSummary>();
                while (dr.Read())
                {
                    RetPackSummary objCustomer = new RetPackSummary();
                    objCustomer.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objCustomer.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objCustomer.PONumber = objBaseSqlManager.GetTextValue(dr, "PONumber");
                    objCustomer.TotalKG = objBaseSqlManager.GetDecimal(dr, "TotalKG");
                    objCustomer.Bag = objBaseSqlManager.GetTextValue(dr, "SrNo");
                    objCustomer.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objCustomer.PackSummaryID = objBaseSqlManager.GetInt64(dr, "PackSummaryID");
                    objCustomer.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objCustomer.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objCustomer.Tag = objBaseSqlManager.GetTextValue(dr, "Tag");
                    //objCustomer.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetPackSummary> GetUpdateProductQTYList(long PackSummaryID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUpdateProductQTYList";
                cmdGet.Parameters.AddWithValue("@PackSummaryID", PackSummaryID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetPackSummary> objlst = new List<RetPackSummary>();
                while (dr.Read())
                {
                    RetPackSummary obj = new RetPackSummary();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    obj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    obj.PackSummaryQtyID = objBaseSqlManager.GetInt64(dr, "PackSummaryQtyID");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteBag(long PackSummaryID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteBag";
                cmdGet.Parameters.AddWithValue("@PackSummaryID", PackSummaryID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public bool DeleteProductPackate(long PackSummaryQtyID, bool IsDelete, long UpdatedBy, DateTime UpdatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteProductPackate";
                cmdGet.Parameters.AddWithValue("@PackSummaryQtyID", PackSummaryQtyID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                cmdGet.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmdGet.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public List<RetPackSummary> GetPackProductListByBagWise(long packsummaryid)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPackProductListByBagWise";
                cmdGet.Parameters.AddWithValue("@packsummaryid", packsummaryid);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetPackSummary> objlst = new List<RetPackSummary>();

                while (dr.Read())
                {
                    RetPackSummary objCustomer = new RetPackSummary();
                    objCustomer.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    objCustomer.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objCustomer.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objCustomer.TotalKG = objBaseSqlManager.GetDecimal(dr, "TotalKG");
                    objCustomer.PackSummaryID = objBaseSqlManager.GetInt64(dr, "PackSummaryID");
                    objCustomer.PackSummaryQtyID = objBaseSqlManager.GetInt64(dr, "PackSummaryQtyID");
                    objCustomer.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateProductPackList(long OrderQtyID, decimal Quantity, long PackSummaryQtyID, bool IsDelete, long SessionUserID, long PackSummaryID, decimal SumTotalKG)
        {
            decimal TotalKG = GetTotalKGByPackSummaryID(PackSummaryID);
            decimal UpdateTotalKG = TotalKG - SumTotalKG;

            try
            {
                decimal UpdateQuantity = GetOrderUpdateQuantity(OrderQtyID);
                decimal SumUpdateQuantity = Quantity + UpdateQuantity;

                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateOrderQuantityByOrderQtyID";
                    cmdGet.Parameters.AddWithValue("@OrderQtyID", OrderQtyID);
                    cmdGet.Parameters.AddWithValue("@Quantity", SumUpdateQuantity);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                decimal TotalQuantity = GetPackSummaryTotalQuantity(PackSummaryQtyID);
                decimal UpdateTotalQuantity = TotalQuantity - Quantity;

                cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateProductPackateQuantity";
                    cmdGet.Parameters.AddWithValue("@PackSummaryQtyID", PackSummaryQtyID);
                    cmdGet.Parameters.AddWithValue("@Quantity", UpdateTotalQuantity);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                if (UpdateTotalQuantity <= 0)
                {
                    cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "DeleteProductPackate";
                        cmdGet.Parameters.AddWithValue("@PackSummaryQtyID", PackSummaryQtyID);
                        cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                        cmdGet.Parameters.AddWithValue("@UpdatedBy", SessionUserID);
                        cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }

                cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateBagTotalKG";
                    cmdGet.Parameters.AddWithValue("@PackSummaryID", PackSummaryID);
                    cmdGet.Parameters.AddWithValue("@TotalKG", UpdateTotalKG);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", SessionUserID);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public decimal GetOrderUpdateQuantity(long OrderQtyID)
        {
            RetOrderQtyMst Quantity = new RetOrderQtyMst();
            decimal UpdateQuantity = 0;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    Quantity = context.RetOrderQtyMsts.Where(i => i.OrderQtyID == OrderQtyID).FirstOrDefault();
                }
                if (Quantity != null)
                {
                    UpdateQuantity = Quantity.UpdateQuantity;
                }
            }
            catch (Exception ex) { }
            return UpdateQuantity;
        }

        public decimal GetPackSummaryTotalQuantity(long PackSummaryQtyID)
        {
            RetPackSummaryQty_Mst Quantity = new RetPackSummaryQty_Mst();
            decimal TotalQuantity = 0;
            try
            {
                using (VirakiEntities context = new VirakiEntities())
                {
                    Quantity = context.RetPackSummaryQty_Mst.Where(i => i.PackSummaryQtyID == PackSummaryQtyID).FirstOrDefault();
                }
                if (Quantity != null)
                {
                    TotalQuantity = Quantity.Quantity;
                }
            }
            catch (Exception ex) { }
            return TotalQuantity;
        }

        private decimal GetTotalKGByPackSummaryID(long PackSummaryID)
        {
            decimal TotalKG = 0;
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalKGByPackSummaryID";
                cmdGet.Parameters.AddWithValue("@PackSummaryID", PackSummaryID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    TotalKG = objBaseSqlManager.GetDecimal(dr, "TotalKG");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TotalKG;
            }
        }

        public List<ClsRetReturnOrderListResponse> GetOrderWiseCreditMemoForCheckList(ClsRetReturnOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOrderWiseCreditMemoForCheckList";
                cmdGet.Parameters.AddWithValue("@OrderID", model.OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ClsRetReturnOrderListResponse> objlst = new List<ClsRetReturnOrderListResponse>();
                int i = 1;
                while (dr.Read())
                {
                    ClsRetReturnOrderListResponse clsobj = new ClsRetReturnOrderListResponse();
                    clsobj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    clsobj.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    clsobj.SerialNumber = i;
                    clsobj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    clsobj.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    clsobj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    clsobj.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    clsobj.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    clsobj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    clsobj.PODate = objBaseSqlManager.GetDateTime(dr, "PODate");
                    clsobj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    clsobj.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    clsobj.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    clsobj.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    clsobj.UpdateQuantity = objBaseSqlManager.GetDecimal(dr, "UpdateQuantity");
                    clsobj.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    clsobj.DiscountPrice = objBaseSqlManager.GetDecimal(dr, "DiscountPrice");
                    clsobj.DiscountPer = objBaseSqlManager.GetDecimal(dr, "DiscountPer");
                    clsobj.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    clsobj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxAmount");
                    clsobj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    clsobj.ReturnedSaleRate = objBaseSqlManager.GetDecimal(dr, "ReturnedSaleRate");
                    clsobj.CreditedFinalTotal = objBaseSqlManager.GetDecimal(dr, "CreditedFinalTotal");
                    clsobj.ReturnedQuantity = objBaseSqlManager.GetDecimal(dr, "ReturnedQuantity");
                    clsobj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    clsobj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    clsobj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    clsobj.ProductMRP = objBaseSqlManager.GetDecimal(dr, "ProductMRP");
                    objlst.Add(clsobj);
                    i++;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateRetailEWayNumberByOrderIDandInvoiceNumber(long OrderID, string InvoiceNumber, string EWayNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetailEWayNumberByOrderIDandInvoiceNumber";
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
                cmdGet.CommandText = "UpdateRetailDocateDetailByOrderIDandInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@DocketNo", DocketNo);
                cmdGet.Parameters.AddWithValue("@DocketDate", DocketDate);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public RetDecateNumberResponse GetTransportDetailByInvoiceNnumberandOrderID(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailTransportDetailByInvoiceNnumberandOrderID";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetDecateNumberResponse obj = new RetDecateNumberResponse();
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

        public List<RetChallanListResponse> GetAllChallanNoWiseChallanList(RetChallanListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetAllChallanNoWiseChallanList";
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
                List<RetChallanListResponse> objlst = new List<RetChallanListResponse>();
                while (dr.Read())
                {
                    RetChallanListResponse objPayment = new RetChallanListResponse();
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

        public List<RetChallanQtyList> GetChallanNoWiseChallanForChallan(string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetChallanNoWiseChallanForChallan";
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanQtyList> objlst = new List<RetChallanQtyList>();
                while (dr.Read())
                {
                    RetChallanQtyList objPayment = new RetChallanQtyList();
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
                    //  objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
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
                cmdGet.CommandText = "UpdateRetEWayNumberForChallanByChallanNumber";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                cmdGet.Parameters.AddWithValue("@EWayNumber", EWayNumber);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public ExportOrderViewModel GetExportOrderDetailsByOrderID(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.CommandText = "GetExportOrderDetailsByOrderID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ExportOrderViewModel objM = new ExportOrderViewModel();
                List<ExportOrderQtyViewModel> lstOrderQty = new List<ExportOrderQtyViewModel>();
                while (dr.Read())
                {
                    objM.DeActiveCustomer = objBaseSqlManager.GetBoolean(dr, "DeActiveCustomer");
                    objM.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objM.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objM.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objM.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
                    objM.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objM.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objM.CountryOfOrigin = objBaseSqlManager.GetTextValue(dr, "CountryOfOrigin");
                    objM.CountryOfFinalDestination = objBaseSqlManager.GetTextValue(dr, "CountryOfFinalDestination");
                    objM.PreCarriageBy = objBaseSqlManager.GetTextValue(dr, "PreCarriageBy");
                    objM.PlaceOfReceiptByPreCarrier = objBaseSqlManager.GetTextValue(dr, "PlaceOfReceiptByPreCarrier");
                    objM.VesselName = objBaseSqlManager.GetTextValue(dr, "VesselName");
                    objM.PortofLoading = objBaseSqlManager.GetTextValue(dr, "PortofLoading");
                    objM.PortofDischarge = objBaseSqlManager.GetTextValue(dr, "PortofDischarge");
                    objM.FinalDestination = objBaseSqlManager.GetTextValue(dr, "FinalDestination");
                    objM.Delivery = objBaseSqlManager.GetTextValue(dr, "Delivery");
                    objM.Payment = objBaseSqlManager.GetTextValue(dr, "Payment");
                    objM.BuyersOrderNo = objBaseSqlManager.GetTextValue(dr, "BuyersOrderNo");
                    objM.TotalNetWeight = objBaseSqlManager.GetDecimal(dr, "TotalNetWeight");
                    objM.TotalGrossWeight = objBaseSqlManager.GetDecimal(dr, "TotalGrossWeight");
                    objM.TotalPkgs = objBaseSqlManager.GetInt32(dr, "TotalPkgs");
                    objM.ContainerNo = objBaseSqlManager.GetTextValue(dr, "ContainerNo");
                    objM.InvoiceTotalAmt = objBaseSqlManager.GetDecimal(dr, "InvoiceTotalAmt");
                    objM.InsuranceText = objBaseSqlManager.GetTextValue(dr, "InsuranceText");
                    objM.InsuranceAmount = objBaseSqlManager.GetDecimal(dr, "InsuranceAmount");
                    objM.FreightText = objBaseSqlManager.GetTextValue(dr, "FreightText");
                    objM.FreightAmount = objBaseSqlManager.GetDecimal(dr, "FreightAmount");
                    objM.GrandInvoiceTotalAmt = objBaseSqlManager.GetDecimal(dr, "GrandInvoiceTotalAmt");
                    objM.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objM.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    //obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    //obj.CreatedOnstr = string.Format("{0:G}", obj.CreatedOn);
                    objM.IsFinalised = objBaseSqlManager.GetBoolean(dr, "IsFinalised");

                    ExportOrderQtyViewModel obj = new ExportOrderQtyViewModel();
                    obj.OrderQtyID = objBaseSqlManager.GetInt64(dr, "OrderQtyID");
                    obj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    obj.PackateInEachCarton = objBaseSqlManager.GetTextValue(dr, "PackateInEachCarton");
                    obj.CartonNo = objBaseSqlManager.GetTextValue(dr, "CartonNo");
                    obj.Carton = objBaseSqlManager.GetInt32(dr, "Carton");
                    obj.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    obj.Rate = objBaseSqlManager.GetDecimal(dr, "Rate");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    lstOrderQty.Add(obj);
                }
                objM.lstOrderQty = lstOrderQty;
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        public ExportProductDetails GetExportProductDetails(long ProductQtyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExportProductDetails";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ExportProductDetails objOrder = new ExportProductDetails();
                while (dr.Read())
                {
                    objOrder.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    objOrder.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    objOrder.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public long AddExportOrder(ExportOrderViewModel data)
        {

            if (!string.IsNullOrEmpty(data.DeleteItems))
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "DeleteExportOrderItems";
                    cmdGet.Parameters.AddWithValue("@OrderQtyID", data.DeleteItems);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            ExportOrder_Mst obj = new ExportOrder_Mst();
            obj.OrderID = data.OrderID;
            obj.CustomerID = data.CustomerID;
            obj.CountryID = data.CountryID;
            //obj.InvoiceNumber = data.InvoiceNumber;

            if (obj.OrderID <= 0)
            {
                obj.InvoiceNumber = GetLastOrderForExport();
            }
            else
            {
                obj.InvoiceNumber = data.InvoiceNumber;
            }
            obj.OrderDate = data.OrderDate;
            obj.CountryOfOrigin = data.CountryOfOrigin;
            obj.CountryOfFinalDestination = data.CountryOfFinalDestination;
            obj.PreCarriageBy = data.PreCarriageBy;
            obj.PlaceOfReceiptByPreCarrier = data.PlaceOfReceiptByPreCarrier;
            obj.VesselName = data.VesselName;
            obj.PortofLoading = data.PortofLoading;
            obj.PortofDischarge = data.PortofDischarge;
            obj.FinalDestination = data.FinalDestination;
            obj.Delivery = data.Delivery;
            obj.Payment = data.Payment;
            obj.BuyersOrderNo = data.BuyersOrderNo;
            obj.TotalNetWeight = data.TotalNetWeight;
            obj.TotalGrossWeight = data.TotalGrossWeight;
            obj.TotalPkgs = data.TotalPkgs;
            obj.ContainerNo = data.ContainerNo;
            obj.InsuranceText = data.InsuranceText;
            obj.InsuranceAmount = data.InsuranceAmount;
            obj.FreightText = data.FreightText;
            obj.FreightAmount = data.FreightAmount;
            obj.GrandInvoiceTotalAmt = data.GrandInvoiceTotalAmt;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsFinalised = false;
            obj.IsDelete = false;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.OrderID == 0)
                {
                    context.ExportOrder_Mst.Add(obj);
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
                        decimal InvoiceTotalAmt = 0;
                        foreach (var item in data.lstOrderQty)
                        {
                            ExportOrderQty_Mst objExpoQty = new ExportOrderQty_Mst();
                            objExpoQty.OrderQtyID = item.OrderQtyID;
                            objExpoQty.OrderID = obj.OrderID;
                            objExpoQty.CategoryTypeID = item.CategoryTypeID;
                            objExpoQty.ProductID = item.ProductID;
                            objExpoQty.ProductQtyID = item.ProductQtyID;
                            objExpoQty.PackateInEachCarton = item.PackateInEachCarton;
                            objExpoQty.Carton = item.Carton;
                            objExpoQty.CartonNo = item.CartonNo;
                            objExpoQty.Quantity = item.Quantity;
                            objExpoQty.Rate = item.Rate;
                            objExpoQty.TotalAmount = item.TotalAmount;
                            InvoiceTotalAmt += item.TotalAmount;
                            //objExpoQty.InvoiceTotalAmt = data.InvoiceTotalAmt;
                            objExpoQty.CreatedBy = data.CreatedBy;
                            objExpoQty.CreatedOn = data.CreatedOn;
                            objExpoQty.UpdatedBy = data.UpdatedBy;
                            objExpoQty.UpdatedOn = data.UpdatedOn;
                            objExpoQty.IsDelete = false;
                            if (objExpoQty.OrderQtyID == 0)
                            {
                                context.ExportOrderQty_Mst.Add(objExpoQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objExpoQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        UpdateExportInvoiceTotalAmt(obj.OrderID, InvoiceTotalAmt);
                        return obj.OrderID;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            ExportOrder_Mst data1 = context.ExportOrder_Mst.Where(x => x.OrderID == obj.OrderID).FirstOrDefault();
                            if (data != null)
                            {
                                context.ExportOrder_Mst.Remove(data1);
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

        private void UpdateExportInvoiceTotalAmt(long OrderID, decimal InvoiceTotalAmt)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateExportInvoiceTotalAmt";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceTotalAmt", InvoiceTotalAmt);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        private string GetLastOrderForExport()
        {
            var lstdata = GetLastExportInvoiceNumber();
            string InvoiceNumber = "";
            string FinancialYearString = "/" + DateTimeExtensions.FromFinancialYear(DateTime.Now).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(DateTime.Now).ToString("yy");
            if (!string.IsNullOrEmpty(lstdata.InvoiceNumber))
            {
                string number = lstdata.InvoiceNumber.Substring(1, 5);
                long incr = Convert.ToInt64(number) + 1;
                InvoiceNumber = "E" + incr.ToString().PadLeft(5, '0') + FinancialYearString;
            }
            else
            {
                InvoiceNumber = "E00001" + FinancialYearString;
            }
            return InvoiceNumber;
        }

        public ExportOrderViewModel GetLastExportInvoiceNumber()
        {
            ExportOrderViewModel objOrder = new ExportOrderViewModel();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastExportInvoiceNumber";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    objOrder.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<ExportOrderListResponse> GetExportOrderListByOrderDate(DateTime OrderDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@OrderDate", OrderDate);
                cmdGet.CommandText = "GetExportOrderListByOrderDate";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExportOrderListResponse> lstPeoduct = new List<ExportOrderListResponse>();
                while (dr.Read())
                {
                    ExportOrderListResponse obj = new ExportOrderListResponse();
                    obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    obj.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    obj.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    obj.InvoiceTotal = objBaseSqlManager.GetDecimal(dr, "InvoiceTotal");
                    obj.Rupees = Math.Round(objBaseSqlManager.GetDecimal(dr, "Rupees"), 2);
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        public List<ExportOrderQtyInvoiceList> GetExportInvoiceOrderDetailForPrint(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExportInvoiceOrderDetailForPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExportOrderQtyInvoiceList> objlst = new List<ExportOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    ExportOrderQtyInvoiceList objPayment = new ExportOrderQtyInvoiceList();
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
                    objPayment.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.CountryOfOrigin = objBaseSqlManager.GetTextValue(dr, "CountryOfOrigin");
                    objPayment.CountryOfFinalDestination = objBaseSqlManager.GetTextValue(dr, "CountryOfFinalDestination");
                    objPayment.PreCarriageBy = objBaseSqlManager.GetTextValue(dr, "PreCarriageBy");
                    objPayment.PlaceOfReceiptByPreCarrier = objBaseSqlManager.GetTextValue(dr, "PlaceOfReceiptByPreCarrier");
                    objPayment.VesselName = objBaseSqlManager.GetTextValue(dr, "VesselName");
                    objPayment.PortofLoading = objBaseSqlManager.GetTextValue(dr, "PortofLoading");
                    objPayment.PortofDischarge = objBaseSqlManager.GetTextValue(dr, "PortofDischarge");
                    objPayment.FinalDestination = objBaseSqlManager.GetTextValue(dr, "FinalDestination");
                    objPayment.Delivery = objBaseSqlManager.GetTextValue(dr, "Delivery");
                    objPayment.Payment = objBaseSqlManager.GetTextValue(dr, "Payment");
                    objPayment.BuyersOrderNo = objBaseSqlManager.GetTextValue(dr, "BuyersOrderNo");
                    objPayment.TotalGrossWeight = objBaseSqlManager.GetDecimal(dr, "TotalGrossWeight");
                    objPayment.TotalNetWeight = objBaseSqlManager.GetDecimal(dr, "TotalNetWeight");
                    objPayment.TotalPkgs = objBaseSqlManager.GetDecimal(dr, "TotalPkgs");
                    objPayment.ContainerNo = objBaseSqlManager.GetTextValue(dr, "ContainerNo");
                    objPayment.InsuranceText = objBaseSqlManager.GetTextValue(dr, "InsuranceText");
                    objPayment.InsuranceAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "InsuranceAmount"), 2);
                    objPayment.FreightText = objBaseSqlManager.GetTextValue(dr, "FreightText");
                    objPayment.FreightAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "FreightAmount"), 2);
                    objPayment.GrandInvoiceTotalAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandInvoiceTotalAmt"), 2);
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");

                    int number = Convert.ToInt32(objPayment.GrandInvoiceTotalAmt);
                    string NumberToWord = DecimalToWords(objPayment.GrandInvoiceTotalAmt);
                    objPayment.NumberToWords = "US DOLLAR" + " " + NumberToWord;

                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.DeliveryAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    objPayment.DeliveryAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");

                    //objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    //objPayment.DeliveryTo = objBaseSqlManager.GetTextValue(dr, "DeliveryTo");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.FSSAIValidUpTo = objBaseSqlManager.GetTextValue(dr, "FSSAIValidUpTo");
                    objPayment.IncomeTaxNo = "AAAFV3761F";
                    objPayment.GSTIN = "27AAAFV3761F1Z7";
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExportOrderQtyInvoiceList> GetInvoiceForExportOrderItemPrint(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForExportOrderItemPrint";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExportOrderQtyInvoiceList> objlst = new List<ExportOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    ExportOrderQtyInvoiceList objPayment = new ExportOrderQtyInvoiceList();
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");

                    objPayment.ContryWiseProductName = objBaseSqlManager.GetTextValue(dr, "ContryWiseProductName");
                    objPayment.ProductQuantity = objBaseSqlManager.GetInt64(dr, "ProductQuantity");
                    objPayment.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    if (objPayment.ContryWiseProductName != "")
                    {
                        objPayment.ProductName = objPayment.ContryWiseProductName + " " + (Convert.ToString(objPayment.ProductQuantity)) + " " + objPayment.UnitCode;
                    }
                    else
                    {
                        objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    }
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.PackateInEachCarton = objBaseSqlManager.GetTextValue(dr, "PackateInEachCarton");
                    objPayment.CartonNo = objBaseSqlManager.GetTextValue(dr, "CartonNo");
                    objPayment.Carton = objBaseSqlManager.GetInt32(dr, "Carton");
                    //objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"));
                    //objPayment.Rate = objBaseSqlManager.GetDecimal(dr, "Rate");
                    objPayment.Rate = Math.Round(objBaseSqlManager.GetDecimal(dr, "Rate"), 2);
                    objPayment.TotalAmount = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    objPayment.InvoiceTotalAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTotalAmt"), 2);
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");

                    objPayment.TotalPkgs = objBaseSqlManager.GetDecimal(dr, "TotalPkgs");
                    objPayment.GrandInvoiceTotalAmt = objBaseSqlManager.GetDecimal(dr, "GrandInvoiceTotalAmt");

                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.RowNumber = objBaseSqlManager.GetInt64(dr, "RowNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string DecimalToWords(decimal d)
        {
            //Grab a string form of your decimal value ("12.34")
            var formatted = d.ToString();

            if (formatted.Contains('.'))
            {
                //If it contains a decimal point, split it into both sides of the decimal
                string[] sides = formatted.Split('.');

                //Process each side and append them with "and", "dot" or "point" etc.
                return NumberToWords(Int32.Parse(sides[0])) + " AND " + NumberToWords(Int32.Parse(sides[1]));
            }
            else
            {
                //Else process as normal
                return NumberToWords(Convert.ToInt32(d));
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
                words += NumberToWords(number / 10000000) + " CRORE ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " LAKHS ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    //words += "and ";
                    words += "";
                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "six", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        //words += "-" + unitsMap[number % 10];
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public List<ExportOrderQtyInvoiceList> GetExportInvoiceOrderDetailForPrintRupees(long OrderID, decimal Rupees)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExportInvoiceOrderDetailForPrintRupees";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExportOrderQtyInvoiceList> objlst = new List<ExportOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    ExportOrderQtyInvoiceList objPayment = new ExportOrderQtyInvoiceList();
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.CountryID = objBaseSqlManager.GetInt64(dr, "CountryID");
                    objPayment.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.OrderDate = objBaseSqlManager.GetDateTime(dr, "OrderDate");
                    objPayment.CountryOfOrigin = objBaseSqlManager.GetTextValue(dr, "CountryOfOrigin");
                    objPayment.CountryOfFinalDestination = objBaseSqlManager.GetTextValue(dr, "CountryOfFinalDestination");
                    objPayment.PreCarriageBy = objBaseSqlManager.GetTextValue(dr, "PreCarriageBy");
                    objPayment.PlaceOfReceiptByPreCarrier = objBaseSqlManager.GetTextValue(dr, "PlaceOfReceiptByPreCarrier");
                    objPayment.VesselName = objBaseSqlManager.GetTextValue(dr, "VesselName");
                    objPayment.PortofLoading = objBaseSqlManager.GetTextValue(dr, "PortofLoading");
                    objPayment.PortofDischarge = objBaseSqlManager.GetTextValue(dr, "PortofDischarge");
                    objPayment.FinalDestination = objBaseSqlManager.GetTextValue(dr, "FinalDestination");
                    objPayment.Delivery = objBaseSqlManager.GetTextValue(dr, "Delivery");
                    objPayment.Payment = objBaseSqlManager.GetTextValue(dr, "Payment");
                    objPayment.BuyersOrderNo = objBaseSqlManager.GetTextValue(dr, "BuyersOrderNo");
                    objPayment.TotalGrossWeight = objBaseSqlManager.GetDecimal(dr, "TotalGrossWeight");
                    objPayment.TotalNetWeight = objBaseSqlManager.GetDecimal(dr, "TotalNetWeight");
                    objPayment.TotalPkgs = objBaseSqlManager.GetDecimal(dr, "TotalPkgs");
                    objPayment.ContainerNo = objBaseSqlManager.GetTextValue(dr, "ContainerNo");
                    objPayment.InsuranceText = "INSURANCE IN RUPEES";
                    decimal InsuranceAmountUSD = Math.Round(objBaseSqlManager.GetDecimal(dr, "InsuranceAmount"), 2);
                    decimal InsuranceAmountRupees = InsuranceAmountUSD * Rupees;
                    objPayment.InsuranceAmount = Math.Round(InsuranceAmountRupees, 2);
                    objPayment.FreightText = "FFREIGHT 2.45 CBM IN RUPEES";
                    decimal FreightAmountUSD = Math.Round(objBaseSqlManager.GetDecimal(dr, "FreightAmount"), 2);
                    decimal FreightAmountRupees = FreightAmountUSD * Rupees;
                    objPayment.FreightAmount = Math.Round(FreightAmountRupees, 2);
                    decimal GrandInvoiceTotalAmtUSD = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandInvoiceTotalAmt"), 2);
                    decimal GrandInvoiceTotalAmtRupees = GrandInvoiceTotalAmtUSD * Rupees;
                    objPayment.GrandInvoiceTotalAmt = Math.Round(GrandInvoiceTotalAmtRupees, 2);
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    int number = Convert.ToInt32(objPayment.GrandInvoiceTotalAmt);
                    string NumberToWord = DecimalToWords(objPayment.GrandInvoiceTotalAmt);
                    objPayment.NumberToWords = "RUPEES" + " " + NumberToWord;
                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.DeliveryAddressLine1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                    objPayment.DeliveryAddressLine2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
                    objPayment.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    objPayment.PinCode = objBaseSqlManager.GetTextValue(dr, "PinCode");
                    objPayment.TaxNo = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                    objPayment.FSSAI = objBaseSqlManager.GetTextValue(dr, "FSSAI");
                    objPayment.FSSAIValidUpTo = objBaseSqlManager.GetTextValue(dr, "FSSAIValidUpTo");
                    objPayment.IncomeTaxNo = "AAAFV3761F";
                    objPayment.GSTIN = "27AAAFV3761F1Z7";
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExportOrderQtyInvoiceList> GetInvoiceForExportOrderItemPrintRupees(long OrderID, string InvoiceNumber, decimal Rupees)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceForExportOrderItemPrintRupees";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExportOrderQtyInvoiceList> objlst = new List<ExportOrderQtyInvoiceList>();
                while (dr.Read())
                {
                    ExportOrderQtyInvoiceList objPayment = new ExportOrderQtyInvoiceList();
                    objPayment.NoofInvoice = objBaseSqlManager.GetTextValue(dr, "NoofInvoice");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.ContryWiseProductName = objBaseSqlManager.GetTextValue(dr, "ContryWiseProductName");
                    objPayment.ProductQuantity = objBaseSqlManager.GetInt64(dr, "ProductQuantity");
                    objPayment.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    if (objPayment.ContryWiseProductName != "")
                    {
                        objPayment.ProductName = objPayment.ContryWiseProductName + " " + (Convert.ToString(objPayment.ProductQuantity)) + " " + objPayment.UnitCode;
                    }
                    else
                    {
                        objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    }
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    objPayment.PackateInEachCarton = objBaseSqlManager.GetTextValue(dr, "PackateInEachCarton");
                    objPayment.CartonNo = objBaseSqlManager.GetTextValue(dr, "CartonNo");
                    objPayment.Carton = objBaseSqlManager.GetInt32(dr, "Carton");
                    objPayment.Quantity = Math.Round(objBaseSqlManager.GetDecimal(dr, "Quantity"));
                    decimal RateUSD = Math.Round(objBaseSqlManager.GetDecimal(dr, "Rate"), 2);
                    decimal RateRupees = RateUSD * Rupees;
                    objPayment.Rate = Math.Round(RateRupees, 2);
                    decimal TotalAmountUSD = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                    decimal TotalAmountRupees = TotalAmountUSD * Rupees;
                    objPayment.TotalAmount = Math.Round(TotalAmountRupees, 2);
                    decimal InvoiceTotalAmtUSD = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTotalAmt"), 2);
                    decimal InvoiceTotalAmtRupees = InvoiceTotalAmtUSD * Rupees;
                    objPayment.InvoiceTotalAmt = Math.Round(InvoiceTotalAmtRupees, 2);
                    objPayment.GrandAmtWord = objBaseSqlManager.GetTextValue(dr, "GrandAmtWord");
                    objPayment.TotalPkgs = objBaseSqlManager.GetDecimal(dr, "TotalPkgs");
                    objPayment.GrandInvoiceTotalAmt = objBaseSqlManager.GetDecimal(dr, "GrandInvoiceTotalAmt");
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.UserFullName = objBaseSqlManager.GetTextValue(dr, "UserFullName");
                    objPayment.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    if (!string.IsNullOrEmpty(objPayment.NoofInvoice))
                    {
                        objPayment.NoofInvoiceint = Convert.ToInt32(objPayment.NoofInvoice);
                    }
                    else
                    {
                        objPayment.NoofInvoiceint = 2;
                    }
                    objPayment.RowNumber = objBaseSqlManager.GetInt64(dr, "RowNumber");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdateExportDollarPrice(long OrderID, string InvoiceNumber, decimal Rupees)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateExportDollarPrice";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@Rupees", Rupees);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
            }
            return true;
        }

        public List<ExpOrderListResponse> GetAllBillWiseExpOrderList(ExpOrderListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllBillWiseExpOrderList";
                cmdGet.Parameters.AddWithValue("@ProductID", model.ProductID);
                cmdGet.Parameters.AddWithValue("@ProductCategoryID", model.ProductCategoryID);
                cmdGet.Parameters.AddWithValue("@CountryID", model.CountryID);
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
                List<ExpOrderListResponse> objlst = new List<ExpOrderListResponse>();
                while (dr.Read())
                {
                    ExpOrderListResponse objPayment = new ExpOrderListResponse();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objPayment.CountryName = objBaseSqlManager.GetTextValue(dr, "CountryName");
                    // objPayment.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    if (model.CurrencyName != "USD")
                    {
                        objPayment.FinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "FinalTotal"), 2);
                    }
                    else
                    {
                        objPayment.FinalTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "InvoiceTotalAmt"), 2);
                    }
                    objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");
                    objPayment.Rupees = objBaseSqlManager.GetDecimal(dr, "Rupees");
                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpOrderQtyList> GetBillWiseInvoiceForExpOrder(string InvoiceNumber, string currencyname = "")
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBillWiseInvoiceForExpOrder";
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpOrderQtyList> objlst = new List<ExpOrderQtyList>();
                while (dr.Read())
                {
                    ExpOrderQtyList objPayment = new ExpOrderQtyList();
                    objPayment.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    objPayment.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                    objPayment.SerialNumber = objBaseSqlManager.GetInt64(dr, "SerialNumber");
                    objPayment.CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                    objPayment.CustomerGroupName = objBaseSqlManager.GetTextValue(dr, "CustomerGroupName");
                    objPayment.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    objPayment.ShipTo = string.Empty;
                    objPayment.BillTo = objBaseSqlManager.GetTextValue(dr, "BillTo");
                    //objPayment.DeliveredDate = ;
                    //objPayment.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objPayment.CustomerNote = objBaseSqlManager.GetTextValue(dr, "CustomerNote");
                    objPayment.ProductName = objBaseSqlManager.GetTextValue(dr, "ProductName");
                    objPayment.Quantity = objBaseSqlManager.GetDecimal(dr, "Quantity");
                    objPayment.UnitName = objBaseSqlManager.GetTextValue(dr, "UnitName");
                    if (currencyname.ToString() != "USD")
                    {
                        objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "Rate"), 2);
                        objPayment.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalAmount"), 2);
                        objPayment.GrandTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "GrandInvoiceAmount"), 2);
                    }
                    else
                    {
                        objPayment.ProductPrice = Math.Round(objBaseSqlManager.GetDecimal(dr, "ProductPrice"), 2);
                        objPayment.Total = Math.Round(objBaseSqlManager.GetDecimal(dr, "tAmount"), 2);
                        objPayment.GrandTotal = Math.Round(objBaseSqlManager.GetDecimal(dr, "GInvoiceAmt"), 2);
                    }
                    //objPayment.LessAmount = objBaseSqlManager.GetDecimal(dr, "LessAmount");
                    //objPayment.SaleRate = objBaseSqlManager.GetDecimal(dr, "SaleRate");
                    //objPayment.BillDiscount = objBaseSqlManager.GetDecimal(dr, "BillDiscount");

                    //objPayment.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    //objPayment.TaxAmt = objBaseSqlManager.GetDecimal(dr, "TaxAmt");
                    objPayment.InsuranceText = objBaseSqlManager.GetTextValue(dr, "InsuranceText");
                    objPayment.InsuranceAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "InsuranceAmount"), 2);
                    objPayment.FreightText = objBaseSqlManager.GetTextValue(dr, "FreightText");
                    objPayment.FreightAmt = Math.Round(objBaseSqlManager.GetDecimal(dr, "FreightAmount"), 2);
                    //objPayment.FinalTotal = objBaseSqlManager.GetDecimal(dr, "FinalTotal");

                    objlst.Add(objPayment);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }




        // 07 Sep. 2020 Piyush Limbani
        public string CheckPDFIsExistForInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetailPDFIsExistForInvoiceNumber";
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
                    cmdGet.CommandText = "UpdateRetailInvoiceNameByOrderIDAndInvoiceNumber";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    cmdGet.Parameters.AddWithValue("@PDFName", PDFName);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }


        public void AddRetEInvoice(RetEInvoice data)
        {
            RetEInvoice_Mst obj = new RetEInvoice_Mst();
            obj.OrderId = data.OrderId;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.IRN = data.IRN;
            obj.QRCode = data.QRCode;
            obj.AckNo = data.AckNo;
            obj.AckDt = data.AckDt;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            obj.IsCancel = false;
            using (VirakiEntities context = new VirakiEntities())
            {
                context.RetEInvoice_Mst.Add(obj);
                context.SaveChanges();
            }
        }

        // 02 April 2021 Piyush Limbani
        public long CheckRetEInvoiceExist(long OrderId, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetEInvoiceExist";
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
        public RetEInvoiceIRN GetRetIRNNumberByInvoiceNumber(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetIRNNumberByInvoiceNumber";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetEInvoiceIRN obj = new RetEInvoiceIRN();
                while (dr.Read())
                {
                    obj.IRN = objBaseSqlManager.GetTextValue(dr, "IRN");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public void AddRetEInvoiceCreditMemo(RetEInvoiceCreditMemo data)
        {

            RetEInvoiceCreditMemo_Mst obj = new RetEInvoiceCreditMemo_Mst();
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
                context.RetEInvoiceCreditMemo_Mst.Add(obj);
                context.SaveChanges();
            }
        }

        // 07 April 2021 Piyush Limbani
        public long CheckRetECreditMemoExist(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetECreditMemoExist";
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
        public RetEInvoiceCreditMemoIRN GetIRNNumberByCreditMemoNumber(string CreditMemoNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetIRNNumberByCreditMemoNumber";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetEInvoiceCreditMemoIRN obj = new RetEInvoiceCreditMemoIRN();
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
        public List<RetEInvoiceErrorListResponse> GetEInvoiceErrorList(DateTime Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetEInvoiceErrorList";
                cmdGet.Parameters.AddWithValue("@Date", Date);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetEInvoiceErrorListResponse> objlst = new List<RetEInvoiceErrorListResponse>();
                while (dr.Read())
                {
                    RetEInvoiceErrorListResponse obj = new RetEInvoiceErrorListResponse();
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



        // 29 April, 2021 Sonal Gandhi
        public void AddRetEWayBill(RetEWayBill data)
        {
            RetEWayBill_Mst obj = new RetEWayBill_Mst();
            obj.OrderId = data.OrderId;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.EWBNumber = data.EWayBillNumber.ToString();
            obj.EWBDate = data.EWBDate;
            obj.EWBValidTill = data.EWBValidTill;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            using (VirakiEntities context = new VirakiEntities())
            {
                context.RetEWayBill_Mst.Add(obj);
                context.SaveChanges();
            }
        }


        // 5 May, 2021 Sonal Gandhi
        public long CheckRetEWayBillExist(long OrderId, string InvoiceNumber)
        {
            var InvoiceNumber1 = InvoiceNumber.Split('/');
            string InvoiceNumber2 = InvoiceNumber1[0];
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetEWayBillExist";
                cmdGet.Parameters.AddWithValue("@OrderId", OrderId);
                //cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber2);
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


        // 24 May, 2021 Sonal Gandhi
        public List<RetDetailsForEWB> RetGetDetailsForEWB(long OrderID, long GodownID, long TransportID, long? VehicleDetailID = 0)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDetailsForEWB";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetDetailsForEWB> objlst = new List<RetDetailsForEWB>();
                while (dr.Read())
                {
                    RetDetailsForEWB obj = new RetDetailsForEWB();
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
                    obj.InvoiceNumberWithDate = obj.InvoiceNumber + "/" + DateTimeExtensions.FromFinancialYear(obj.DocDate1).ToString("yyyy") + "-" + DateTimeExtensions.ToFinancialYear(obj.DocDate1).ToString("yy");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 25 May, 2021 Sonal Gandhi
        public void AddRetEWayBillChallan(RetEWayBillChallan data)
        {
            RetEWayBillChallan_Mst obj = new RetEWayBillChallan_Mst();
            obj.ChallanID = data.ChallanID;
            obj.ChallanNumber = data.ChallanNumber;
            obj.EWBNumber = data.EWayBillNumber.ToString();
            obj.EWBDate = Convert.ToDateTime(data.EWBDate);
            obj.EWBValidTill = Convert.ToDateTime(data.EWBValidTill);
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = DateTime.Now;
            using (VirakiEntities context = new VirakiEntities())
            {
                context.RetEWayBillChallan_Mst.Add(obj);
                context.SaveChanges();
            }
        }

        // 25 May, 2021 Sonal Gandhi
        public long CheckRetEWayBillChallanExist(long ChallanID, string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckRetEWayBillChallanExist";
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


        // 25 May, 2021 Sonal Gandhi
        public List<RetChallanItemForEWB> GetRetChallanItemForEWB(long ChallanID, string ChallanNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetChallanItemForEWB";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanItemForEWB> objlst = new List<RetChallanItemForEWB>();
                while (dr.Read())
                {
                    RetChallanItemForEWB obj = new RetChallanItemForEWB();
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

        // 25 May, 2021 Sonal Gandhi
        public List<RetChallanDetailForEWB> GetRetChallanDetailForEWB(long ChallanID, long TransportID, long VehicleDetailID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetChallanDetailForEWB";
                cmdGet.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetChallanDetailForEWB> objlst = new List<RetChallanDetailForEWB>();
                while (dr.Read())
                {
                    RetChallanDetailForEWB obj = new RetChallanDetailForEWB();
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




        // 18 Jan, 2022 Piyush Limbani
        public List<TotalPouchListGodownWise> GetTotalPouchGodownWiseList(string id, string date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@id", id);
                cmdGet.Parameters.AddWithValue("@date", date);
                cmdGet.CommandText = "GetTotalPouchGodownWise";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TotalPouchListGodownWise> lstPeoduct = new List<TotalPouchListGodownWise>();
                while (dr.Read())
                {
                    TotalPouchListGodownWise obj = new TotalPouchListGodownWise();
                    obj.QuantityPackage = objBaseSqlManager.GetInt64(dr, "QuantityPackage");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.PouchSize = objBaseSqlManager.GetInt64(dr, "PouchSize");
                    lstPeoduct.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lstPeoduct;
            }
        }

        // 21-06-2022
        public bool IsDonePrintPackList(long ProductQtyID, string date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateIsDonePackList";
                cmdGet.Parameters.AddWithValue("@ProductQtyID", ProductQtyID);
                cmdGet.Parameters.AddWithValue("@date", date);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        // 27-06-2022
        public bool IsDoneCheckList(long OrderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateIsDoneCheckList";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                //cmdGet.Parameters.AddWithValue("@date", date);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }
    }
}
