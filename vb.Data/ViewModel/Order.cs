

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class OrderViewModel
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string ShipTo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long ProductID { get; set; }
        public List<OrderQtyViewModel> lstOrderQty { get; set; }
        public decimal TotalDiscount { get; set; }

        public bool IsTCSApplicable { get; set; }
        public string GSTNumber { get; set; }

        // 2 July,2021 Sonal Gandhi
        public long OnlineOrderID { get; set; }
    }

    public class OrderQtyViewModel
    {
        public long OrderQtyID { get; set; }
        public long ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal BaseRate { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalTotal { get; set; }
        public long CategoryTypeID { get; set; }
        public string ProductName { get; set; }

        public decimal SlabForGST { get; set; }
    }

    public class OrderListResponse
    {
        public long ReturnOrderID { get; set; }
        public string CreditMemoNumber { get; set; }
        public long ProductCategoryID { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool Isclear { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public decimal FinalTotal { get; set; }
        public int Quantity { get; set; }
        public string ShipTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string EWayNumber { get; set; }

        public string TaxNo { get; set; }
        public string TaxName { get; set; }
    }

    public class ClsReturnOrderListResponse
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AreaName { get; set; }
        public decimal GrandQuantity { get; set; }
        public decimal GrandFinalTotal { get; set; }
        public string UserName { get; set; }
        public bool Isclear { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long UserID { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal Quantity { get; set; }
        public string ShipTo { get; set; }
        public long SerialNumber { get; set; }
        public string CustomerGroupName { get; set; }
        public string CreditMemoNumber { get; set; }
        public string BillTo { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string TaxName { get; set; }
        public string CustomerNote { get; set; }
        public string ProductName { get; set; }
        public long ProductQtyID { get; set; }
        public long ProductID { get; set; }
        public long CategoryTypeID { get; set; }
        public long OrderQtyID { get; set; }
        public long ReturnedOrderQtyID { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnedSaleRate { get; set; }
        public decimal CreditedFinalTotal { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal GrandTotal { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class ReturnOrderListResponse
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AreaName { get; set; }
        public decimal GrandQuantity { get; set; }
        public decimal GrandFinalTotal { get; set; }
        public string UserName { get; set; }
        public bool Isclear { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long UserID { get; set; }
        public List<OrderQtyList> lstOrderQty { get; set; }
    }

    public class OrderQtyList
    {
        public long SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public string InvoiceNumber { get; set; }
        public long ReturnOrderID { get; set; }
        public string CreditMemoNumber { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string TaxName { get; set; }
        public string CustomerNote { get; set; }
        public string ProductName { get; set; }
        public long ProductID { get; set; }
        public long CategoryTypeID { get; set; }
        public long OrderQtyID { get; set; }
        public long ReturnedOrderQtyID { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnedSaleRate { get; set; }
        public decimal CreditedFinalTotal { get; set; }
        public decimal Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal TotalTCSTaxAmt { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsTCSApplicable { get; set; }
    }

    public class OrderQtyInvoiceList
    {
        public string AreaName { get; set; }
        public string OrderRef { get; set; }
        public Int32 ordergroup { get; set; }
        public string InvoiceTitleHeader { get; set; }
        public long SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public string InvoiceNumber { get; set; }
        public string BillNo { get; set; }
        public string CreditMemoNumber { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string DeliveryTo { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string TaxName { get; set; }
        public string CustomerNote { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal GrandTotal { get; set; }



        public decimal InvoiceTotal { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public string LBTNo { get; set; }
        public string TaxNo { get; set; }
        public string FSSAI { get; set; }
        public string ContactNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserFullName { get; set; }
        public string HSNNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public long CategoryTypeID { get; set; }
        public int OrdRowNumber { get; set; }
        public int Totalcount { get; set; }
        public decimal ATotalAmount { get; set; }
        public decimal AGrandTotal { get; set; }
        public string GrandAmtWord { get; set; }
        public string NoofInvoice { get; set; }
        public int NoofInvoiceint { get; set; }
        public decimal Totalrecord { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int divider { get; set; }
        public string InvoiceTitle { get; set; }
        public int RowNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string FSSAIValidUpTo { get; set; }
        public DateTime FSSAIValidUpTo1 { get; set; }
        public decimal InvTotal { get; set; }
        public decimal OrderTotal { get; set; }

        public decimal TCSTax { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal GrandCreditedTotal { get; set; }

        // 24 March 2021 Sonal Gandhi

        public string DocType { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate1 { get; set; }
        public string DocDate { get; set; }
        public string From_Name { get; set; }
        public string From_GSTIN { get; set; }
        public string From_Address1 { get; set; }
        public string From_Place { get; set; }
        public int From_PinCode { get; set; }
        public string From_StateCode { get; set; }
        public string PinCode { get; set; }
        public string To_GSTIN { get; set; }
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
        public string TaxRate { get; set; }
        public decimal DividedAmount { get; set; }
        public decimal DivTaxRate { get; set; }
        public string IRN { get; set; }
        public string QRCode { get; set; }
        public long AckNo { get; set; }
        public DateTime AckDt { get; set; }
        public long CreatedBy { get; set; }
        public string PanCard { get; set; }
    }

    public class GetUnitRate
    {
        public string UnitCode { get; set; }
        public decimal ProductPrice { get; set; }
        public string HSNNumber { get; set; }
    }

    public class GetSellPrice
    {
        public decimal SellPrice { get; set; }
        public decimal Tax { get; set; }
        public long CategoryTypeID { get; set; }
        public decimal SlabForGST { get; set; }
    }

    public class GetTax
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long TaxID { get; set; }
        public string TaxName { get; set; }
        public string DeliveryTo { get; set; }
        public string BillTo { get; set; }
        public bool IsTCSApplicable { get; set; }
        public string GSTNumber { get; set; }
    }

    public class ExportToExcelInvoiceWholesale
    {
        public string SupplyType { get; set; }
        public string SubType { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string Transaction_Type { get; set; }
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
        public string Product { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal AssessableValue { get; set; }
        public string TaxRate { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal Cess_Non_Advol_Amount { get; set; }
        //public decimal Others { get; set; }
        public decimal TCS { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public string TransMode { get; set; }
        public decimal DistanceKM { get; set; }
        public string TransName { get; set; }
        public string TransID { get; set; }
        public string TransDocNo { get; set; }
        public string TransDate { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }

        public string IRN { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class InvoiceForExcelList
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
        public string Product { get; set; }
        public string DescriptionFull { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal AssessableValue { get; set; }
        public string TaxName { get; set; }
        public decimal TotalTaxRate { get; set; }
        public decimal DivTaxRate { get; set; }
        public string TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DividedAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }

        public decimal Cess_Non_Advol_Amount { get; set; }
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
        public decimal SGSTTaxRate { get; set; }
        public decimal CGSTTaxRate { get; set; }
        public decimal IGSTTaxRate { get; set; }
        public decimal CESSTaxRate { get; set; }
        public decimal CESSTaxRate2 { get; set; }
        public string IRN { get; set; }
    }

    public class ChallanViewModel
    {
        public long ChallanID { get; set; }
        public long GodownIDFrom { get; set; }
        public long GodownIDTo { get; set; }
        public string Tax { get; set; }
        public DateTime? ChallanDate { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long ProductID { get; set; }
        public List<ChallanQtyViewModel> lstOrderQty { get; set; }
        public decimal TotalDiscount { get; set; }
    }

    public class ChallanQtyViewModel
    {
        public long ChallanQtyID { get; set; }
        public long ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal BaseRate { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalTotal { get; set; }
        public long CategoryTypeID { get; set; }
        public string ProductName { get; set; }
    }

    public class ChallanListResponse
    {
        public long ChallanID { get; set; }
        public string ChallanNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DeliveryFrom { get; set; }
        public string From_Address1 { get; set; }
        public string From_Address2 { get; set; }
        public string From_Place { get; set; }
        public string From_PinCode { get; set; }
        public string From_State { get; set; }
        public string DispatchState { get; set; }
        public string To_OtherPartyName { get; set; }
        public string To_Address1 { get; set; }
        public string To_Address2 { get; set; }
        public string To_Place { get; set; }
        public string To_PinCode { get; set; }
        public string To_State { get; set; }
        public string ShipToState { get; set; }
        public string DeliveryTo { get; set; }
        public int Quantity { get; set; }
        public decimal FinalTotal { get; set; }
        public bool Isclear { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string EWayNumber { get; set; }
        public decimal ChallanTotal { get; set; }

    }

    public class ChallanQtyInvoiceList
    {
        public long ChallanID { get; set; }
        public string TaxName { get; set; }
        public string DeliveryFromAddressLine1 { get; set; }
        public string DeliveryFromAddressLine2 { get; set; }
        public string DeliveryFromFSSAINo { get; set; }
        public string DeliveryToAddressLine1 { get; set; }
        public string DeliveryToAddressLine2 { get; set; }
        public string DeliveryToFSSAINo { get; set; }
        public int NoofInvoiceint { get; set; }
        public long CategoryTypeID { get; set; }
        public string ProductName { get; set; }
        public string ChallanNumber { get; set; }
        public decimal Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserFullName { get; set; }
        public string HSNNumber { get; set; }
        public string GrandAmtWord { get; set; }
        public int RowNumber { get; set; }
        public Int32 ordergroup { get; set; }
        public string InvoiceTitleHeader { get; set; }
        public string BillNo { get; set; }
        public long SerialNumber { get; set; }
        public decimal TotalTax { get; set; }
        public int Totalcount { get; set; }
        public decimal AGrandTotal { get; set; }
        public decimal ATotalAmount { get; set; }
        public decimal Totalrecord { get; set; }
        public string InvoiceTitle { get; set; }
        public string NoofInvoice { get; set; }
        public int divider { get; set; }
        public int OrdRowNumber { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal BillDiscountAmount { get; set; }
        public decimal SaleRateAmount { get; set; }
    }

    public class ChallanQtyList
    {
        public string DeliveryFromAddressLine1 { get; set; }
        public string DeliveryFromAddressLine2 { get; set; }
        public string DeliveryFromFSSAINo { get; set; }
        public string DeliveryToAddressLine1 { get; set; }
        public string DeliveryToAddressLine2 { get; set; }
        public string DeliveryToFSSAINo { get; set; }
        public long SerialNumber { get; set; }
        public string ChallanNumber { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public long ChallanID { get; set; }
        public string TaxName { get; set; }
        public decimal BillDiscountAmount { get; set; }
        public decimal SaleRateAmount { get; set; }
    }

    public class ChallanForExcelList
    {

        public string SupplyType { get; set; }
        public string SubType { get; set; }
        public string DocType { get; set; }
        public string From_GSTIN { get; set; }
        public string To_GSTIN { get; set; }
        public string DeliveryFrom { get; set; }
        public string DeliveryTo { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate1 { get; set; }
        public string DocDate { get; set; }
        public string Transaction_Type { get; set; }
        public string ProductFull { get; set; }
        public string Product { get; set; }
        public string DescriptionFull { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal AssessableValue { get; set; }
        public string TaxName { get; set; }
        public decimal TotalTaxRate { get; set; }
        public decimal DivTaxRate { get; set; }
        public string TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DividedAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }

        public decimal Cess_Non_Advol_Amount { get; set; }
        public decimal Others { get; set; }
        public decimal ChallanTotal { get; set; }


        public string TransMode { get; set; }
        public decimal DistanceKM { get; set; }
        public string TransName { get; set; }
        public string TransID { get; set; }
        public string TransportGSTNumber { get; set; }
        public string TransDocNo { get; set; }
        public DateTime TransDate1 { get; set; }
        public string TransDate { get; set; }
        public string VehicleNo { get; set; }
        public decimal SGSTTaxRate { get; set; }
        public decimal CGSTTaxRate { get; set; }
        public decimal IGSTTaxRate { get; set; }
        public decimal CESSTaxRate { get; set; }
        public decimal CESSTaxRate2 { get; set; }
    }

    public class ExportToExcelChallanWholesale
    {
        public string SupplyType { get; set; }
        public string SubType { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string Transaction_Type { get; set; }
        public string DeliveryFrom { get; set; }
        public string From_GSTIN { get; set; }
        public string From_Address1 { get; set; }
        public string From_Address2 { get; set; }
        public string From_Place { get; set; }
        public string From_PinCode { get; set; }
        public string From_State { get; set; }
        public string DispatchState { get; set; }
        public string DeliveryTo { get; set; }
        public string To_GSTIN { get; set; }
        public string To_Address1 { get; set; }
        public string To_Address2 { get; set; }
        public string To_Place { get; set; }
        public string To_PinCode { get; set; }
        public string To_State { get; set; }
        public string ShipToState { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal AssessableValue { get; set; }
        public string TaxRate { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }

        public decimal Cess_Non_Advol_Amount { get; set; }
        public decimal Others { get; set; }
        public decimal ChallanTotal { get; set; }

        public string TransMode { get; set; }
        public decimal DistanceKM { get; set; }
        public string TransName { get; set; }
        public string TransID { get; set; }
        public string TransDocNo { get; set; }
        public string TransDate { get; set; }
        public string VehicleNo { get; set; }
    }


    public class ProductList
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal OrderQuantity { get; set; }
        public string CategoryName { get; set; }
    }


    public class OrderIDList
    {
        public long OrderID { get; set; }
    }

    public class DecateNumberResponse
    {
        public long TransportID { get; set; }
        public string ContactNumber { get; set; }
        public string DocketNo { get; set; }
        public DateTime DocketDate { get; set; }
    }

    public class EInvoice
    {
        public long EInvoiceId { get; set; }
        public long OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public string IRN { get; set; }
        public string QRCode { get; set; }
        public long AckNo { get; set; }
        public DateTime AckDt { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }

    public class EInvoiceIRN
    {
        public string IRN { get; set; }
    }


    // 07/04/2021
    public class EInvoiceCreditMemo
    {
        public long EInvoiceId { get; set; }
        public long ReturnOrderId { get; set; }
        public string CreditMemoNumber { get; set; }
        public string IRN { get; set; }
        public string QRCode { get; set; }
        public long AckNo { get; set; }
        public DateTime AckDt { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }


    public class EInvoiceCreditMemoIRN
    {
        public string IRN { get; set; }
    }



    public class ModelCreditedTotal
    {
        public decimal CreditedTotal { get; set; }
        public long ReturnOrderID { get; set; }
        public decimal TCSTax { get; set; }
    }

    public class EInvoiceErrorListResponse
    {
        public long EInvoiceErrorDetailsID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ErrorCodes { get; set; }
        public string ErrorDesc { get; set; }
    }

    public class ExportEInvoiceErrorList
    {
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ErrorCodes { get; set; }
        public string ErrorDesc { get; set; }
    }

    //28th April,2021 Sonal Gandhi
    public class EWayBill
    {
        public long EWayBillId { get; set; }
        public long OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public long EWayBillNumber { get; set; }
        public DateTime EWBDate { get; set; }
        public DateTime EWBValidTill { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }



    public class DetailsForEWB
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public string DocNo { get; set; }
        public string Transaction_Type { get; set; }
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
        public string IRN { get; set; }
    }


    //24 May,2021 Sonal Gandhi
    public class EWayBillChallan
    {
        public long EWayBillId { get; set; }
        public long ChallanID { get; set; }
        public string ChallanNumber { get; set; }
        public long EWayBillNumber { get; set; }
        public string EWBDate { get; set; }
        public string EWBValidTill { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }

    // 26 May, 2021 Sonal Gandhi
    public class ChallanDetailForEWB
    {
        public string ChallanNumber { get; set; }
        public string SupplyType { get; set; }
        public string SubType { get; set; }
        public string DocType { get; set; }
        public string From_GSTIN { get; set; }
        public string To_GSTIN { get; set; }
        public string DeliveryFrom { get; set; }
        public string DeliveryTo { get; set; }
        public string Transaction_Type { get; set; }
        public string TransMode { get; set; }
        public decimal DistanceKM { get; set; }
        public string TransName { get; set; }
        public string TransID { get; set; }
        public string TransportGSTNumber { get; set; }
        public string TransDocNo { get; set; }
        public DateTime TransDate1 { get; set; }
        public string TransDate { get; set; }
        public string VehicleNo { get; set; }


        public string From_Address1 { get; set; }
        public string From_Address2 { get; set; }
        public string From_Place { get; set; }
        public string From_PinCode { get; set; }
        public string From_State { get; set; }
        public string DispatchState { get; set; }
        public string To_OtherPartyName { get; set; }
        public string To_Address1 { get; set; }
        public string To_Address2 { get; set; }
        public string To_Place { get; set; }
        public string To_PinCode { get; set; }
        public string To_State { get; set; }
        public string ShipToState { get; set; }
        public double FinalTotal { get; set; }
        public double Qty { get; set; }
    }


    // 26 May, 2021 Sonal Gandhi
    public class ChallanItemForEWB
    {
        public long ChallanID { get; set; }
        public string ChallanNumber { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate1 { get; set; }
        public string DocDate { get; set; }
        public string ProductFull { get; set; }
        public string Product { get; set; }
        public string DescriptionFull { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal AssessableValue { get; set; }
        public string TaxName { get; set; }
        public decimal TotalTaxRate { get; set; }
        public decimal DivTaxRate { get; set; }
        public string TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DividedAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal Cess_Non_Advol_Amount { get; set; }
        public decimal Others { get; set; }
        public decimal ChallanTotal { get; set; }
        public decimal SGSTTaxRate { get; set; }
        public decimal CGSTTaxRate { get; set; }
        public decimal IGSTTaxRate { get; set; }
        public decimal CESSTaxRate { get; set; }
        public decimal CESSTaxRate2 { get; set; }
    }


    //14 June,2021 Sonal Gandhi
    public class OnlineOrder
    {
        public long OnlineOrderID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long CustomerNumber { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string PinCode { get; set; }
        public string CreatedDate { get; set; }
        public DateTime? InvDate { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OnlineGrandAmount { get; set; }
        public string CustomerNote { get; set; }
        public bool IsConfirm { get; set; }

        public long OrderID { get; set; }
    }


    public class OnlineOrderQty
    {
        public long OnlineOrderQtyID { get; set; }
        public long OnlineOrderID { get; set; }
        public long OnlineProductQtyID { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal OnlineProductPrice { get; set; }
        public decimal OnlineOrderQuantity { get; set; }
        public decimal OnlineTotalAmount { get; set; }
        public decimal OnlineGrandAmount { get; set; }
        public string Packing { get; set; }
        public string CategoryName { get; set; }
        public OnlineOrder OnlineOrderDetails { get; set; }
    }

    //14 June,2021 Sonal Gandhi
    public class OnlineOrderViewModel
    {
        public long OnlineOrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ShipTo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public long TaxID { get; set; }
        public string Tax { get; set; }
        public string OrderRef { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long ProductID { get; set; }
        public long OnlineProductQtyID { get; set; }
        public List<OnlineOrderQtyViewModel> lstOrderQty { get; set; }
        public decimal TotalDiscount { get; set; }
        public string GSTNumber { get; set; }
        public bool DeActiveCustomer { get; set; }
        public bool IsConfirm { get; set; }
    }

    public class OnlineOrderQtyViewModel
    {
        public long OnlineOrderQtyID { get; set; }
        public long ProductID { get; set; }
        public long OnlineProductQtyID { get; set; }
        public decimal Quantity { get; set; } // Quantity [Convert G to KG]
        public decimal BaseRate { get; set; } // [OnlineTotalAmount * Factoring]
        public decimal SaleRate { get; set; } // [BaseRate]
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; } // OnlineTotalAmount
        public decimal QtyTax { get; set; }
        public decimal QtyTaxAmt { get; set; }
        public decimal FinalTotal { get; set; } // OnlineGrandAmount
        public long CategoryTypeID { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal ProductPrice { get; set; }
        public string HSNNumber { get; set; }
    }
}
