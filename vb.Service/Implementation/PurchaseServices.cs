using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;

namespace vb.Service
{
    public class PurchaseServices : IPurchaseService
    {
        public List<string> GetTaxForSupplierByTextChange(string SupplierName)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetActiveSuppliersByName";
                cmdGet.Parameters.AddWithValue("@SupplierName", SupplierName);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<string> suppliername = new List<string>();
                while (dr.Read())
                {
                    suppliername.Add(objBaseSqlManager.GetTextValue(dr, "SupplierName"));
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return suppliername;
            }
        }

        public GetSupplierTax GetTaxForSupplierBySupplierID(long SupplierID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxForSupplierBySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetSupplierTax objOrder = new GetSupplierTax();
                while (dr.Read())
                {
                    objOrder.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objOrder.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objOrder.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objOrder.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objOrder.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");

                    // 16 June 2021 Piyush Limbani
                    objOrder.TDSPercentage = objBaseSqlManager.GetDecimal(dr, "TDSPercentage");
                    objOrder.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    objOrder.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    // 16 June 2021 Piyush Limbani

                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<ProductListResponse> GetAllProductName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseProductName";
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

        public List<GetProductDetaiForPurchase> GetAutoCompleteProductDetaiForPurchase(long Prefix, string Tax)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteProductDetaiForPurchase";
                cmdGet.Parameters.AddWithValue("@ProductID", Prefix.ToString());
                cmdGet.Parameters.AddWithValue("@Tax", Tax.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetProductDetaiForPurchase> objlst = new List<GetProductDetaiForPurchase>();
                while (dr.Read())
                {
                    GetProductDetaiForPurchase obj = new GetProductDetaiForPurchase();
                    //obj.UnitCode = objBaseSqlManager.GetTextValue(dr, "UnitCode");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    obj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");
                    if (Tax == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "Tax");
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0);
                        obj.TaxAmtSGST = Convert.ToDecimal(0);
                    }
                    else
                    {
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "Tax") / 2);
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "Tax") / 2);
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0);
                        obj.TaxAmtSGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0);
                    }
                    //  obj.ProductPrice = objBaseSqlManager.GetDecimal(dr, "ProductPrice");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string CheckSupplierCurrentYearBillNumber(string StartDate, string EndDate, long SupplierID, string BillNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckSupplierCurrentYearBillNumber";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@BillNumber", BillNumber);
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string BillNo = "";
                while (dr.Read())
                {
                    BillNo = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return BillNo;
            }
        }

        public string AddPurchaseBill(AddPurchaseDetail data, string StartDate, string EndDate)
        {
            long AvakNumber = 0;
            long PurchaseID = 0;
            string Message = "";
            Purchase_Mst obj = new Purchase_Mst();
            obj.PurchaseID = data.PurchaseID;
            obj.SupplierID = data.SupplierID;
            obj.ShipTo = data.ShipTo;
            obj.Tax = data.Tax;
            obj.BillNumber = data.BillNumber;
            obj.BillDate = data.BillDate;
            obj.PurchaseTypeID = data.PurchaseTypeID;
            obj.DeliveryChallanNumber = data.DeliveryChallanNumber;
            obj.DeliveryChallanDate = data.DeliveryChallanDate;
            obj.PurchaseDebitAccountTypeID = data.PurchaseDebitAccountTypeID;
            obj.BrokerID = data.BrokerID;
            obj.EWayNumber = data.EWayNumber;
            obj.GodownID = data.GodownID;

            obj.TDSCategoryID = data.TDSCategoryID;

            if (data.AvakNumber == 0)
            {
                // increment
                AvakNumber = GetAvakNumberByGodownID(obj.GodownID);
                obj.AvakNumber = AvakNumber;
            }
            else
            {
                obj.AvakNumber = data.AvakNumber;
            }
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = false;
            obj.Verify = data.Verify;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.PurchaseID == 0)
                {
                    // 25 Aug 2020 Piyush Limbani
                    PurchaseID = CheckBillNumberForSupplierIsExist(data.SupplierID, data.BillNumber, StartDate, EndDate);
                    // 25 Aug 2020 Piyush Limbani

                    if (PurchaseID == 0)
                    {
                        context.Purchase_Mst.Add(obj);
                        context.SaveChanges();
                        Message = "Insert Sucessfully";
                    }
                    else
                    {
                        //context.Entry(obj).State = EntityState.Modified;
                        //context.SaveChanges();
                        Message = "Bill Exist";
                    }
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    Message = "Updated Sucessfully";
                }
                if (obj.PurchaseID > 0)
                {
                    try
                    {
                        decimal TotalPurchaseBillAmount = 0;
                        foreach (var item in data.lstPurchaseQty)
                        {
                            PurchaseQty_Mst objPurchaseQty = new PurchaseQty_Mst();
                            objPurchaseQty.PurchaseQtyID = item.PurchaseQtyID;
                            objPurchaseQty.PurchaseID = obj.PurchaseID;
                            objPurchaseQty.CategoryTypeID = item.CategoryTypeID;
                            objPurchaseQty.ProductID = item.ProductID;
                            objPurchaseQty.HSNNumber = item.HSNNumber;
                            objPurchaseQty.VakalNumber = item.VakalNumber;
                            objPurchaseQty.NoofBags = item.NoofBags;
                            objPurchaseQty.GrossWeight = item.GrossWeight;
                            objPurchaseQty.TareWeight = item.TareWeight;
                            objPurchaseQty.NetWeight = item.NetWeight;
                            objPurchaseQty.RatePerKG = item.RatePerKG;
                            objPurchaseQty.Amount = item.Amount;
                            objPurchaseQty.WeightPerBag = item.GrossWeight / item.NoofBags;
                            objPurchaseQty.RatePerBags = item.RatePerBags;
                            objPurchaseQty.PackingChargesBag = item.PackingChargesBag;
                            objPurchaseQty.TotalPackingCharge = item.TotalPackingCharge;
                            objPurchaseQty.Hamali = item.Hamali;
                            objPurchaseQty.LessDiscount = item.LessDiscount;
                            objPurchaseQty.DiscountAmount = item.DiscountAmount;
                            objPurchaseQty.APMC = item.APMC;
                            objPurchaseQty.APMCAmount = item.APMCAmount;
                            objPurchaseQty.TotalTaxableAmount = item.TotalTaxableAmount;
                            objPurchaseQty.TotalTax = item.TotalTax;
                            objPurchaseQty.CGSTTax = item.CGSTTax;
                            objPurchaseQty.CGSTTaxAmount = item.CGSTTaxAmount;
                            objPurchaseQty.SGSTTax = item.SGSTTax;
                            objPurchaseQty.SGSTTaxAmount = item.SGSTTaxAmount;
                            objPurchaseQty.IGSTTax = item.IGSTTax;
                            objPurchaseQty.IGSTTaxAmount = item.IGSTTaxAmount;
                            objPurchaseQty.TotalTaxAmount = item.TotalTaxAmount;
                            objPurchaseQty.TotalAmount = item.TotalAmount;
                            objPurchaseQty.Insurance = item.Insurance;
                            objPurchaseQty.TransportInward = item.TransportInward;
                            objPurchaseQty.GrandTotalAmount = item.GrandTotalAmount;


                            objPurchaseQty.TDSTax = item.TDSTax;
                            objPurchaseQty.TDSTaxAmount = item.TDSTaxAmount;
                            objPurchaseQty.AmountAfterTDS = item.AmountAfterTDS;


                            // TotalPurchaseBillAmount += item.GrandTotalAmount;
                            TotalPurchaseBillAmount += item.AmountAfterTDS;

                            objPurchaseQty.TCSAmount = item.TCSAmount;

                            objPurchaseQty.RoundOff = item.RoundOff;
                            objPurchaseQty.CreatedBy = data.CreatedBy;
                            objPurchaseQty.CreatedOn = data.CreatedOn;
                            objPurchaseQty.UpdatedBy = data.UpdatedBy;
                            objPurchaseQty.UpdatedOn = data.UpdatedOn;
                            objPurchaseQty.IsDelete = false;
                            if (objPurchaseQty.PurchaseQtyID == 0)
                            {
                                context.PurchaseQty_Mst.Add(objPurchaseQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objPurchaseQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        UpdateTotalPurcahseBillAmount(obj.PurchaseID, TotalPurchaseBillAmount);
                        return Message;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            Purchase_Mst data1 = context.Purchase_Mst.Where(x => x.PurchaseID == obj.PurchaseID).FirstOrDefault();
                            if (data != null)
                            {
                                context.Purchase_Mst.Remove(data1);
                                context.SaveChanges();
                                Message = "Error";
                                return Message;
                            }
                            else
                            {
                                Message = "Error";
                                return Message;
                            }
                        }
                    }
                }
                else
                {
                    if (PurchaseID > 0)
                    {
                        Message = "Bill Exist";
                    }
                    else
                    {
                        Message = "Error";
                    }

                    return Message;
                }
            }
        }

        private long GetAvakNumberByGodownID(long? GodownID)
        {
            var lstdata = GetDailyAvakNumberByGodownID(GodownID);
            long AvakNumber = 0;
            if (lstdata.AvakNumber != 0)
            {
                long incr = Convert.ToInt64(lstdata.AvakNumber + 1);
                AvakNumber = incr;
            }
            else
            {
                AvakNumber = 1;
            }
            return AvakNumber;
        }

        public AddPurchaseDetail GetDailyAvakNumberByGodownID(long? GodownID)
        {
            AddPurchaseDetail obj = new AddPurchaseDetail();
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDailyAvakNumberByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    obj.AvakNumber = objBaseSqlManager.GetInt64(dr, "AvakNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return obj;
        }

        private void UpdateTotalPurcahseBillAmount(long PurchaseID, decimal TotalPurchaseBillAmount)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateTotalPurcahseBillAmount";
                cmdGet.Parameters.AddWithValue("@PurchaseID", PurchaseID);
                cmdGet.Parameters.AddWithValue("@TotalPurchaseBillAmount", TotalPurchaseBillAmount);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        public List<PurcahseListResponse> GetAllPurchaseList(PurcahseListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPurchaseList";
                cmdGet.Parameters.AddWithValue("@SupplierID", model.SupplierID);
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
                List<PurcahseListResponse> objlst = new List<PurcahseListResponse>();
                while (dr.Read())
                {
                    PurcahseListResponse obj = new PurcahseListResponse();
                    obj.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    //objPayment.UserName = objBaseSqlManager.GetTextValue(dr, "UserName");            
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    obj.AvakNumber = objBaseSqlManager.GetInt64(dr, "AvakNumber");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "PurcahseCreatedOn");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public AddPurchaseDetail GetPurchaseOrderDetailsByPurchaseID(long PurchaseID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@PurchaseID", PurchaseID);
                cmdGet.CommandText = "GetPurchaseOrderDetailsByPurchaseID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AddPurchaseDetail objM = new AddPurchaseDetail();
                List<AddPurchaseQtyDetail> lstPurchaseQty = new List<AddPurchaseQtyDetail>();
                while (dr.Read())
                {
                    objM.DeActiveSupplier = objBaseSqlManager.GetBoolean(dr, "DeActiveSupplier");
                    objM.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    objM.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objM.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objM.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objM.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objM.Tax = objBaseSqlManager.GetTextValue(dr, "Tax");
                    objM.BillDate2 = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objM.BillDate2 == Convert.ToDateTime("10/10/2014") && objM.BillDate2 == null)
                    {
                        objM.BillDateStr = "";
                    }
                    else
                    {
                        objM.BillDateStr = objM.BillDate2.Value.ToString("MM/dd/yyyy");
                    }
                    objM.PurchaseTypeID = objBaseSqlManager.GetInt64(dr, "PurchaseTypeID");
                    objM.DeliveryChallanNumber = objBaseSqlManager.GetTextValue(dr, "DeliveryChallanNumber");
                    objM.DeliveryChallanDate = objBaseSqlManager.GetDateTime(dr, "DeliveryChallanDate");
                    if (objM.DeliveryChallanDate == Convert.ToDateTime("10/10/2014") && objM.DeliveryChallanDate == null)
                    {
                        objM.DeliveryChallanDateStr = "";
                    }
                    else
                    {
                        objM.DeliveryChallanDateStr = objM.DeliveryChallanDate.Value.ToString("MM/dd/yyyy");
                    }
                    objM.PurchaseDebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "PurchaseDebitAccountTypeID");
                    objM.BrokerID = objBaseSqlManager.GetInt64(dr, "BrokerID");
                    objM.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    objM.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objM.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objM.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objM.AvakNumber = objBaseSqlManager.GetInt64(dr, "AvakNumber");
                    objM.Verify = objBaseSqlManager.GetBoolean(dr, "Verify");

                    objM.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    objM.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");


                    AddPurchaseQtyDetail obj = new AddPurchaseQtyDetail();
                    obj.PurchaseQtyID = objBaseSqlManager.GetInt64(dr, "PurchaseQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.VakalNumber = objBaseSqlManager.GetTextValue(dr, "VakalNumber");
                    obj.NoofBags = objBaseSqlManager.GetInt32(dr, "NoofBags");
                    obj.GrossWeight = objBaseSqlManager.GetDecimal(dr, "GrossWeight");
                    obj.TareWeight = objBaseSqlManager.GetDecimal(dr, "TareWeight");
                    obj.NetWeight = objBaseSqlManager.GetDecimal(dr, "NetWeight");
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    obj.RatePerBags = objBaseSqlManager.GetDecimal(dr, "RatePerBags");
                    obj.PackingChargesBag = objBaseSqlManager.GetDecimal(dr, "PackingChargesBag");
                    obj.TotalPackingCharge = objBaseSqlManager.GetDecimal(dr, "TotalPackingCharge");
                    obj.Hamali = objBaseSqlManager.GetDecimal(dr, "Hamali");
                    obj.LessDiscount = objBaseSqlManager.GetDecimal(dr, "LessDiscount");
                    obj.DiscountAmount = objBaseSqlManager.GetDecimal(dr, "DiscountAmount");
                    obj.APMC = objBaseSqlManager.GetDecimal(dr, "APMC");
                    obj.APMCAmount = objBaseSqlManager.GetDecimal(dr, "APMCAmount");
                    obj.TotalTaxableAmount = objBaseSqlManager.GetDecimal(dr, "TotalTaxableAmount");
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.CGSTTax = objBaseSqlManager.GetDecimal(dr, "CGSTTax");
                    obj.CGSTTaxAmount = objBaseSqlManager.GetDecimal(dr, "CGSTTaxAmount");
                    obj.SGSTTax = objBaseSqlManager.GetDecimal(dr, "SGSTTax");
                    obj.SGSTTaxAmount = objBaseSqlManager.GetDecimal(dr, "SGSTTaxAmount");
                    obj.IGSTTax = objBaseSqlManager.GetDecimal(dr, "IGSTTax");
                    obj.IGSTTaxAmount = objBaseSqlManager.GetDecimal(dr, "IGSTTaxAmount");
                    obj.TotalTaxAmount = objBaseSqlManager.GetDecimal(dr, "TotalTaxAmount");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.Insurance = objBaseSqlManager.GetDecimal(dr, "Insurance");
                    obj.TCSAmount = objBaseSqlManager.GetDecimal(dr, "TCSAmount");
                    obj.RoundOff = objBaseSqlManager.GetDecimal(dr, "RoundOff");
                    obj.TransportInward = objBaseSqlManager.GetDecimal(dr, "TransportInward");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.CategoryTypeID = objBaseSqlManager.GetInt64(dr, "CategoryTypeID");

                    obj.TDSTax = objBaseSqlManager.GetDecimal(dr, "TDSTax");
                    obj.TDSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TDSTaxAmount");
                    obj.AmountAfterTDS = objBaseSqlManager.GetDecimal(dr, "AmountAfterTDS");

                    //obj.ProductQtyID = objBaseSqlManager.GetInt64(dr, "ProductQtyID");
                    lstPurchaseQty.Add(obj);
                }
                objM.lstPurchaseQty = lstPurchaseQty;
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        public bool DeletePurchaseOrder(long PurchaseID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeletePurchaseOrder";
                cmdGet.Parameters.AddWithValue("@PurchaseID", PurchaseID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }


        // Expense Module 18-12-2019
        public List<string> GetTaxForExpenseSupplierByTextChange(string SupplierName)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetActiveExpenseSuppliersByName";
                cmdGet.Parameters.AddWithValue("@SupplierName", SupplierName);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<string> suppliername = new List<string>();
                while (dr.Read())
                {
                    suppliername.Add(objBaseSqlManager.GetTextValue(dr, "SupplierName"));
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return suppliername;
            }
        }

        public GetSupplierTax GetTaxForExpenseSupplierBySupplierID(long SupplierID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTaxForExpenseSupplierBySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetSupplierTax objOrder = new GetSupplierTax();
                while (dr.Read())
                {
                    objOrder.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objOrder.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objOrder.TaxID = objBaseSqlManager.GetInt64(dr, "TaxID");
                    objOrder.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                    objOrder.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objOrder.TDSPercentage = objBaseSqlManager.GetDecimal(dr, "TDSPercentage");
                    objOrder.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    objOrder.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public List<ProductListResponse> GetAllExpenseProductName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseProductName";
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

        public List<GetProductDetaiForExpense> GetAutoCompleteProductDetaiForExpense(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteProductDetaiForExpense";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetProductDetaiForExpense> objlst = new List<GetProductDetaiForExpense>();
                while (dr.Read())
                {
                    GetProductDetaiForExpense obj = new GetProductDetaiForExpense();
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<GetTaxDetailForExpense> GetAutoCompleteTaxDetailForExpense(long ExpenseDebitAccountTypeID, string Tax)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAutoCompleteTaxDetailForExpense";
                cmdGet.Parameters.AddWithValue("@ExpenseDebitAccountTypeID", ExpenseDebitAccountTypeID.ToString());
                cmdGet.Parameters.AddWithValue("@Tax", Tax.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetTaxDetailForExpense> objlst = new List<GetTaxDetailForExpense>();
                while (dr.Read())
                {
                    GetTaxDetailForExpense obj = new GetTaxDetailForExpense();
                    obj.Tax = objBaseSqlManager.GetDecimal(dr, "Tax");
                    if (Tax == "IGST")
                    {
                        obj.IGST = objBaseSqlManager.GetDecimal(dr, "Tax");
                        obj.CGST = Convert.ToDecimal(0);
                        obj.SGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0);
                        obj.TaxAmtSGST = Convert.ToDecimal(0);
                    }
                    else
                    {
                        obj.CGST = (objBaseSqlManager.GetDecimal(dr, "Tax") / 2);
                        obj.SGST = (objBaseSqlManager.GetDecimal(dr, "Tax") / 2);
                        obj.IGST = Convert.ToDecimal(0);
                        obj.TaxAmtCGST = Convert.ToDecimal(0);
                        obj.TaxAmtSGST = Convert.ToDecimal(0);
                        obj.TaxAmtIGST = Convert.ToDecimal(0);
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 25 Aug 2020 Piyush Limbani
        public string CheckExpenseSupplierCurrentYearBillNumber(string StartDate, string EndDate, long SupplierID, string BillNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckExpenseSupplierCurrentYearBillNumber";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@BillNumber", BillNumber);
                cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string BillNo = "";
                while (dr.Read())
                {
                    BillNo = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return BillNo;
            }
        }
        // 25 Aug 2020 Piyush Limbani

        public long AddExpenseBill(AddExpenseDetail data)
        {
            Expense_Mst obj = new Expense_Mst();
            obj.ExpenseID = data.ExpenseID;
            obj.SupplierID = data.SupplierID;
            obj.ShipTo = data.ShipTo;
            obj.Tax = data.Tax;
            obj.BillNumber = data.BillNumber;
            obj.BillDate = data.BillDate;
            obj.ExpenseTypeID = data.ExpenseTypeID;
            obj.DeliveryChallanNumber = data.DeliveryChallanNumber;
            obj.DeliveryChallanDate = data.DeliveryChallanDate;
            obj.EWayNumber = data.EWayNumber;
            obj.GodownID = data.GodownID;
            obj.TDSCategoryID = data.TDSCategoryID;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = false;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.ExpenseID == 0)
                {
                    context.Expense_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.ExpenseID > 0)
                {
                    try
                    {
                        decimal TotalExpenseBillAmount = 0;
                        foreach (var item in data.lstExpenseQty)
                        {
                            ExpenseQty_Mst objExpenseQty = new ExpenseQty_Mst();
                            objExpenseQty.ExpenseQtyID = item.ExpenseQtyID;
                            objExpenseQty.ExpenseID = obj.ExpenseID;
                            objExpenseQty.ProductID = item.ProductID;
                            objExpenseQty.HSNNumber = item.HSNNumber;
                            objExpenseQty.ExpenseDebitAccountTypeID = item.ExpenseDebitAccountTypeID;
                            objExpenseQty.Quantity = item.Quantity;
                            objExpenseQty.Rate = item.Rate;
                            objExpenseQty.TotalTaxableAmount = item.TotalTaxableAmount;
                            objExpenseQty.TotalTax = item.TotalTax;
                            objExpenseQty.CGSTTax = item.CGSTTax;
                            objExpenseQty.CGSTTaxAmount = item.CGSTTaxAmount;
                            objExpenseQty.SGSTTax = item.SGSTTax;
                            objExpenseQty.SGSTTaxAmount = item.SGSTTaxAmount;
                            objExpenseQty.IGSTTax = item.IGSTTax;
                            objExpenseQty.IGSTTaxAmount = item.IGSTTaxAmount;
                            objExpenseQty.TotalTaxAmount = item.TotalTaxAmount;
                            objExpenseQty.TCSAmount = item.TCSAmount;
                            objExpenseQty.RoundOff = item.RoundOff;
                            objExpenseQty.TotalAmount = item.TotalAmount;
                            objExpenseQty.TDSTax = item.TDSTax;
                            objExpenseQty.TDSTaxAmount = item.TDSTaxAmount;
                            objExpenseQty.GrandTotalAmount = item.GrandTotalAmount;
                            TotalExpenseBillAmount += item.GrandTotalAmount;
                            objExpenseQty.CreatedBy = data.CreatedBy;
                            objExpenseQty.CreatedOn = data.CreatedOn;
                            objExpenseQty.UpdatedBy = data.UpdatedBy;
                            objExpenseQty.UpdatedOn = data.UpdatedOn;
                            objExpenseQty.IsDelete = false;
                            if (objExpenseQty.ExpenseQtyID == 0)
                            {
                                context.ExpenseQty_Mst.Add(objExpenseQty);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objExpenseQty).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        UpdateTotalExpenseBillAmount(obj.ExpenseID, TotalExpenseBillAmount);
                        return obj.ExpenseID;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            Expense_Mst data1 = context.Expense_Mst.Where(x => x.ExpenseID == obj.ExpenseID).FirstOrDefault();
                            if (data != null)
                            {
                                context.Expense_Mst.Remove(data1);
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

        private void UpdateTotalExpenseBillAmount(long ExpenseID, decimal TotalExpenseBillAmount)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateTotalExpenseBillAmount";
                cmdGet.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmdGet.Parameters.AddWithValue("@TotalExpenseBillAmount", TotalExpenseBillAmount);
                objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
        }

        public List<ExpenseListResponse> GetAllExpenseList(ExpenseListResponse model)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpenseList";
                cmdGet.Parameters.AddWithValue("@SupplierID", model.SupplierID);
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
                List<ExpenseListResponse> objlst = new List<ExpenseListResponse>();
                while (dr.Read())
                {
                    ExpenseListResponse obj = new ExpenseListResponse();
                    obj.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    obj.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (obj.BillDate == Convert.ToDateTime("10/10/2014") || obj.BillDate == null || obj.BillDate == Convert.ToDateTime("10/10/2014 12:00:00 AM"))
                    {
                        obj.BillDateStr = "";
                    }
                    else
                    {
                        obj.BillDateStr = obj.BillDate.ToString("dd/MM/yyyy");
                    }
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteExpenseOrder(long ExpenseID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteExpenseOrder";
                cmdGet.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public AddExpenseDetail GetExpenseOrderDetailsByExpenseID(long ExpenseID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmdGet.CommandText = "GetExpenseOrderDetailsByExpenseID";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                AddExpenseDetail objM = new AddExpenseDetail();
                List<AddExpenseQtyDetail> lstExpenseQty = new List<AddExpenseQtyDetail>();
                while (dr.Read())
                {
                    objM.DeActiveSupplier = objBaseSqlManager.GetBoolean(dr, "DeActiveSupplier");
                    objM.ExpenseID = objBaseSqlManager.GetInt64(dr, "ExpenseID");
                    objM.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objM.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    objM.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    objM.ShipTo = objBaseSqlManager.GetTextValue(dr, "ShipTo");
                    objM.Tax = objBaseSqlManager.GetTextValue(dr, "Tax");
                    objM.BillDate = objBaseSqlManager.GetDateTime(dr, "BillDate");
                    if (objM.BillDate == Convert.ToDateTime("10/10/2014") || objM.BillDate == null || objM.BillDate == Convert.ToDateTime("10/10/2014 12:00:00 AM"))
                    {
                        objM.BillDateStr = "";
                    }
                    else
                    {
                        objM.BillDateStr = objM.BillDate.Value.ToString("MM/dd/yyyy");
                    }
                    objM.ExpenseTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseTypeID");
                    objM.DeliveryChallanNumber = objBaseSqlManager.GetTextValue(dr, "DeliveryChallanNumber");
                    objM.DeliveryChallanDate = objBaseSqlManager.GetDateTime(dr, "DeliveryChallanDate");
                    if (objM.DeliveryChallanDate == Convert.ToDateTime("10/10/2014") || objM.DeliveryChallanDate == null || objM.BillDate == Convert.ToDateTime("10/10/2014 12:00:00 AM"))
                    {
                        objM.DeliveryChallanDateStr = "";
                    }
                    else
                    {
                        objM.DeliveryChallanDateStr = objM.DeliveryChallanDate.Value.ToString("MM/dd/yyyy");
                    }
                    objM.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    objM.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objM.TDSCategoryID = objBaseSqlManager.GetInt64(dr, "TDSCategoryID");
                    objM.TDSCategory = objBaseSqlManager.GetTextValue(dr, "TDSCategory");
                    objM.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objM.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");

                    AddExpenseQtyDetail obj = new AddExpenseQtyDetail();
                    obj.ExpenseQtyID = objBaseSqlManager.GetInt64(dr, "ExpenseQtyID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.ExpenseDebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "ExpenseDebitAccountTypeID");
                    obj.Quantity = objBaseSqlManager.GetInt32(dr, "Quantity");
                    obj.Rate = objBaseSqlManager.GetDecimal(dr, "Rate");
                    obj.TotalTaxableAmount = objBaseSqlManager.GetDecimal(dr, "TotalTaxableAmount");
                    obj.TotalTax = objBaseSqlManager.GetDecimal(dr, "TotalTax");
                    obj.CGSTTax = objBaseSqlManager.GetDecimal(dr, "CGSTTax");
                    obj.CGSTTaxAmount = objBaseSqlManager.GetDecimal(dr, "CGSTTaxAmount");
                    obj.SGSTTax = objBaseSqlManager.GetDecimal(dr, "SGSTTax");
                    obj.SGSTTaxAmount = objBaseSqlManager.GetDecimal(dr, "SGSTTaxAmount");
                    obj.IGSTTax = objBaseSqlManager.GetDecimal(dr, "IGSTTax");
                    obj.IGSTTaxAmount = objBaseSqlManager.GetDecimal(dr, "IGSTTaxAmount");
                    obj.TotalTaxAmount = objBaseSqlManager.GetDecimal(dr, "TotalTaxAmount");
                    obj.TCSAmount = objBaseSqlManager.GetDecimal(dr, "TCSAmount");
                    obj.RoundOff = objBaseSqlManager.GetDecimal(dr, "RoundOff");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    obj.TDSTax = objBaseSqlManager.GetDecimal(dr, "TDSTax");
                    obj.TDSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TDSTaxAmount");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    lstExpenseQty.Add(obj);
                }
                objM.lstExpenseQty = lstExpenseQty;
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objM;
            }
        }

        public long CheckBillNumberForSupplierIsExist(long SupplierID, string BillNumber, string StartDate, string EndDate)
        {
            long PurchaseID = 0;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "CheckBillNumberForSupplierIsExist";
                    cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                    cmdGet.Parameters.AddWithValue("@BillNumber", BillNumber);
                    cmdGet.Parameters.AddWithValue("@StartDate", StartDate);
                    cmdGet.Parameters.AddWithValue("@EndDate", EndDate);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch
            {
                PurchaseID = 0;
            }
            return PurchaseID;
        }

        // 30-03-2020
        public List<GetLastPurchaseProductDetail> GetLastPurchaseProductRatePerKG(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastPurchaseProductRatePerKG";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetLastPurchaseProductDetail> objlst = new List<GetLastPurchaseProductDetail>();
                while (dr.Read())
                {
                    GetLastPurchaseProductDetail obj = new GetLastPurchaseProductDetail();
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 29-05-2020
        public List<BrokerAndItemWiseReportList> GetBrokerAndItemWiseReportList(DateTime? From, DateTime? To, long SupplierID, long ProductID, long BrokerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetBrokerAndItemWiseReportList";
                cmdGet.Parameters.AddWithValue("@From", From);
                cmdGet.Parameters.AddWithValue("@To", To);
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
                cmdGet.Parameters.AddWithValue("@BrokerID", BrokerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<BrokerAndItemWiseReportList> objlst = new List<BrokerAndItemWiseReportList>();
                while (dr.Read())
                {
                    BrokerAndItemWiseReportList obj = new BrokerAndItemWiseReportList();
                    obj.PurchaseID = objBaseSqlManager.GetInt64(dr, "PurchaseID");
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.ProductID = objBaseSqlManager.GetInt64(dr, "ProductID");
                    obj.BrokerID = objBaseSqlManager.GetInt64(dr, "BrokerID");
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
                    obj.NoofBags = objBaseSqlManager.GetInt64(dr, "NoofBags");
                    obj.RatePerKG = objBaseSqlManager.GetDecimal(dr, "RatePerKG");
                    obj.GrandTotalAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotalAmount");
                    obj.BrokerName = objBaseSqlManager.GetTextValue(dr, "BrokerName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "PurcahseCreatedOn");

                    //obj.EWayNumber = objBaseSqlManager.GetTextValue(dr, "EWayNumber");
                    //obj.AvakNumber = objBaseSqlManager.GetInt64(dr, "AvakNumber");

                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }


        // 10-06-2020
        public List<GetLastExpenseProductDetail> GetLastExpenseProductRate(long ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastExpenseProductRate";
                cmdGet.Parameters.AddWithValue("@ProductID", ProductID.ToString());
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetLastExpenseProductDetail> objlst = new List<GetLastExpenseProductDetail>();
                while (dr.Read())
                {
                    GetLastExpenseProductDetail obj = new GetLastExpenseProductDetail();
                    obj.Rate = Math.Round((objBaseSqlManager.GetDecimal(dr, "Rate")), 2);
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }
    }
}
