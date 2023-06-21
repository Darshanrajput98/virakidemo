using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vb.Data
{
    public class EwayBill
    {
    }
    public class EwayBillList
    {
        public long OrderID { get; set; }
        public string SupplyType { get; set; }
        public string SubType { get; set; }
        public string DocType { get; set; }
        public string InvoiceNumber { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate1 { get; set; }
        public string DocDate { get; set; }
        public string Transaction_Type { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress1 { get; set; }
        public string BuyerAddress2 { get; set; }
        public double DeliveryAddressDistance { get; set; }
        public string From_OtherPartyName { get; set; }
        public string From_GSTIN { get; set; }
        public string From_Address1 { get; set; }
        public string From_Address2 { get; set; }
        public string From_Place { get; set; }
        public string From_PinCode { get; set; }
        public string From_State { get; set; }
        public string DispatchState { get; set; }
        public string To_OtherPartyName { get; set; }
        public string To_GSTIN { get; set; }
        public string To_Address1 { get; set; }
        public string To_Address2 { get; set; }
        public string To_Place { get; set; }
        public string To_PinCode { get; set; }
        public string To_State { get; set; }
        public string ShipToState { get; set; }
        public string ProductFull { get; set; }

        public string DescriptionFull { get; set; }

        public string HSN { get; set; }

        public decimal AssessableValue { get; set; }
        public string TaxNumber { get; set; }
        public string TaxName { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal TotalTaxRate { get; set; }
        public decimal DivTaxRate { get; set; }
        public decimal DividedAmount { get; set; }


        //public decimal Others { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal TotalInvoiceValue { get; set; }

        public string TransMode { get; set; }
        public decimal DistanceKM { get; set; }
        public string TransName { get; set; }
        public string TransID { get; set; }
        public string TransportGSTNumber { get; set; }
        public string TransDocNo { get; set; }
        public DateTime TransDate1 { get; set; }
        public string TransDate { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public double FinalTotal { get; set; }

    }
    public class EwayBillItemList
    {
        public decimal DivTaxRate { get; set; }
        public string TaxRate { get; set; }
        public string TaxName { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalTaxRate { get; set; }
        public decimal Cess_Non_Advol_Amount { get; set; }
        public decimal SGSTTaxRate { get; set; }
        public decimal CGSTTaxRate { get; set; }
        public decimal IGSTTaxRate { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal CESSTaxRate { get; set; }
        public decimal CESSTaxRate2 { get; set; }
        public decimal DividedAmount { get; set; }
        public int HSN { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public double FinalTotal { get; set; }
        public double Total { get; set; }
        public double UnitPrice { get; set; }
    }
}
