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
    public class EWayBillService : IEWayBillService
    {
        public List<EwayBillList> GetEWayBill(long OrderID, long GodownID, long TransportID)
        {
            List<EwayBillList> objlst = new List<EwayBillList>();
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "GetEWayList";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                    cmdGet.Parameters.AddWithValue("@TransportID", TransportID);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        EwayBillList obj = new EwayBillList();
                        obj.SupplyType = "Outward";
                        obj.SubType = "Supply";
                        obj.DocType = "Tax Invoice";
                        obj.DocNo = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                        string[] Doc = obj.DocNo.Split('/');
                        obj.DocNo = Doc[0];
                        obj.DocDate1 = objBaseSqlManager.GetDateTime(dr, "InvoiceDate");
                        obj.DocDate = obj.DocDate1.ToString("dd/MM/yyyy");
                        obj.From_OtherPartyName = "VIRAKI BROTHERS";
                        obj.From_GSTIN = "34AACCC1596Q002";//"27AAAFV3761F1Z7";
                        obj.Transaction_Type = "Regular";
                        obj.OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                        obj.DeliveryAddressDistance = objBaseSqlManager.GetDouble(dr, "DeliveryAddressDistance");
                        obj.BuyerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                        obj.BuyerAddress1 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine1");
                        obj.BuyerAddress2 = objBaseSqlManager.GetTextValue(dr, "DeliveryAddressLine2");
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
                        //obj.ProductFull = objBaseSqlManager.GetTextValue(dr, "ProductName");
                        //string[] Product = obj.ProductFull.Split('(');
                        //obj.Product = Product[0];
                        //obj.DescriptionFull = objBaseSqlManager.GetTextValue(dr, "Description");
                        //string[] Description = obj.DescriptionFull.Split('(');
                        //obj.Description = Description[0];
                        //obj.HSN = objBaseSqlManager.GetTextValue(dr, "HSN");
                        //obj.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                        //obj.Qty = objBaseSqlManager.GetDecimal(dr, "Quantity");
                        //obj.AssessableValue = objBaseSqlManager.GetDecimal(dr, "AssessableValue");
                        //obj.TotalTaxRate = objBaseSqlManager.GetDecimal(dr, "Tax");

                        //obj.Others = 0;
                        obj.TaxNumber = objBaseSqlManager.GetTextValue(dr, "TaxNo");
                        obj.TaxName = objBaseSqlManager.GetTextValue(dr, "TaxName");
                        //obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxableAmount");
                        //if (obj.TaxName == "IGST")
                        //{
                        //    obj.IGSTAmount = Math.Round(obj.TaxAmount, 2);
                        //}
                        //else
                        //{
                        //    obj.DividedAmount = obj.TaxAmount / 2;
                        //    obj.SGSTAmount = Math.Round(obj.DividedAmount, 2);
                        //    obj.CGSTAmount = Math.Round(obj.DividedAmount, 2);
                        //}
                        obj.CESSAmount = 0;

                        //obj.TCSTaxAmount = objBaseSqlManager.GetDecimal(dr, "TCSTaxAmount");
                        //obj.TotalInvoiceValue = objBaseSqlManager.GetDecimal(dr, "InvoiceTotal");
                        obj.TransMode = "Road";
                        obj.DistanceKM = objBaseSqlManager.GetDecimal(dr, "DeliveryAddressDistance");
                        obj.TransName = objBaseSqlManager.GetTextValue(dr, "TransportName");
                        obj.TransID = objBaseSqlManager.GetTextValue(dr, "TransID");
                        obj.TransportGSTNumber = objBaseSqlManager.GetTextValue(dr, "TransportGSTNumber");
                        obj.FinalTotal = objBaseSqlManager.GetDouble(dr, "GrandTotal");
                        obj.TransDocNo = Doc[0];
                        // obj.TransDocNo = "";
                        obj.TransDate1 = DateTime.Now;
                        obj.TransDate = obj.TransDate1.ToString("dd/MM/yyyy");
                        obj.VehicleNo = "";
                        obj.VehicleType = "Regular";
                        objlst.Add(obj);
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return objlst;
        }

        public List<EwayBillItemList> GetEWayBillItemList(long OrderID)
        {
            List<EwayBillItemList> objlst = new List<EwayBillItemList>();
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "GetEWayBillItemList";
                    cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);

                    while (dr.Read())
                    {
                        EwayBillItemList obj = new EwayBillItemList();
                        obj.Product = objBaseSqlManager.GetTextValue(dr, "ProductName");
                        obj.Description = objBaseSqlManager.GetTextValue(dr, "Description");
                        obj.HSN = objBaseSqlManager.GetInt32(dr, "HSN");
                        obj.Unit = objBaseSqlManager.GetTextValue(dr, "Unit");
                        obj.Qty = objBaseSqlManager.GetDecimal(dr, "Quantity");
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
                        obj.TaxAmount = objBaseSqlManager.GetDecimal(dr, "TaxableAmount");
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
                        obj.FinalTotal = objBaseSqlManager.GetDouble(dr, "FinalTotal");
                        obj.Total = objBaseSqlManager.GetDouble(dr, "Total");
                        obj.UnitPrice = objBaseSqlManager.GetDouble(dr, "UnitPrice");

                        objlst.Add(obj);
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return objlst;
        }

    }
}
