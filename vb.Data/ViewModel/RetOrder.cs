namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RetOrderViewModel
    {
        public bool DeActiveCustomer { get; set; }
        public long ProductID { get; set; }
        public long ProductQtyID { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public long CustomerGroupID { get; set; }
        public string CustomerName { get; set; }
        public string Tax { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? PODate { get; set; }
        public string PONumber { get; set; }
        public string OrderNote { get; set; }
        public bool IsFinalised { get; set; }
        public decimal FinaOrderlTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public bool IsTCSApplicable { get; set; }
        public string IsTCSApplicablestr { get; set; }
        public string GSTNumber { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string DeleteItems { get; set; }
        public List<RetOrderQtyViewModel> lstOrderQty { get; set; }
    }

    public class RetOrderQtyViewModel
    {
        public long OrderQtyID { get; set; }
        public long ProductID { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string HSNNumber { get; set; }
        public long CategoryTypeID { get; set; }
        public long ProductQtyID { get; set; }
        public int Quantity { get; set; }
        public int UpdateQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductMRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountPer { get; set; }
        public string ProductName { get; set; }

        public string ArticleCode { get; set; }

        //Add By Dhruvik
        public decimal Margin { get; set; }
        public decimal SPDiscount { get; set; }
        public decimal TexableAmount { get; set; }
        //Add By Dhruvik

    }

    public class RetOrderListResponse
    {
        public long AreaID { get; set; }
        public long CustomerGroupID { get; set; }
        public long ProductCategoryID { get; set; }
        public long ProductQtyID { get; set; }
        public long ReturnOrderID { get; set; }
        public string CreditMemoNumber { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public bool Isclear { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string Tax { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PODate { get; set; }
        public string PONumber { get; set; }
        public string OrderNote { get; set; }
        public decimal FinaOrderlTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsPrinted { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string FullInvoiceNumber { get; set; }
        public string EWayNumber { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal InvoiceTotal { get; set; }
    }

    public class RetGetUnitRate
    {
        public string HSNNumber { get; set; }
    }

    public class RetGetSellPrice
    {
        public decimal SellPrice { get; set; }
    }

    public class GetRetTax
    {
        public long TaxID { get; set; }
        public string TaxName { get; set; }
        public string DeliveryTo { get; set; }
        public string BillTo { get; set; }
        public long CustomerID { get; set; }
        public long CustomerGroupID { get; set; }
        public bool IsTCSApplicable { get; set; }
        public string GSTNumber { get; set; }
    }



    public class GetCustDetailExport
    {
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public long CustomerID { get; set; }
        public string CountryOfOrigin { get; set; }
    }



    public class RetProductQty
    {
        public long ProductQtyID { get; set; }
        public long CategoryTypeID { get; set; }
    }

    public class GetRetSellPrice
    {
        public decimal ProductMRP { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountPer { get; set; }
        public string HSNNumber { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Tax { get; set; }
        public long CategoryTypeID { get; set; }
        public long ProductID { get; set; }
        public string ArticleCode { get; set; }
        public long CustomerGroupID { get; set; }

        //Add By Dhruvik
        public decimal MarginPer { get; set; }
        //Add By Dhruvik

    }

    public class ClsRetReturnOrderListResponse
    {
        public decimal DiscountPer { get; set; }
        public long SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PODate { get; set; }
        public string CreditMemoNumber { get; set; }
        public string TaxName { get; set; }
        public string CustomerNote { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UpdateQuantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductMRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal FinalTotal { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public long CategoryTypeID { get; set; }
        public long ProductID { get; set; }
        public long OrderQtyID { get; set; }
        public long ReturnedOrderQtyID { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnedSaleRate { get; set; }
        public decimal CreditedFinalTotal { get; set; }
        public string AreaName { get; set; }
        public string UserName { get; set; }
        public long ProductQtyID { get; set; }
        public bool Isclear { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long UserID { get; set; }
    }

    public class RetReturnOrderListResponse
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PODate { get; set; }
        public string AreaName { get; set; }
        public decimal GrandQuantity { get; set; }
        public decimal GrandFinalTotal { get; set; }
        public string UserName { get; set; }
        public bool Isclear { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long UserID { get; set; }
        public List<RetOrderQtyList> lstOrderQty { get; set; }
    }

    public class RetOrderQtyList
    {
        public long ReturnOrderID { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal ProductMRP { get; set; }
        public long SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PODate { get; set; }
        public string CreditMemoNumber { get; set; }
        public string TaxName { get; set; }
        public string CustomerNote { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UpdateQuantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal FinalTotal { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public long CategoryTypeID { get; set; }
        public long ProductID { get; set; }
        public long OrderQtyID { get; set; }
        public long ReturnedOrderQtyID { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnedSaleRate { get; set; }
        public decimal CreditedFinalTotal { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal InvoiceTotal { get; set; }
        public long ProductQtyID { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsTCSApplicable { get; set; }
    }

    public class RetOrderQtyInvoiceList
    {
        public string AreaName { get; set; }
        public Int32 ordergroup { get; set; }
        public string InvoiceTitleHeader { get; set; }
        public string BillNo { get; set; }
        public long SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string DeliveryTo { get; set; }
        public DateTime OrderDate { get; set; }
        public string TaxName { get; set; }
        public string CustomerNote { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SaleRate { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal ProductMRP { get; set; }
        public decimal Total { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public string LBTNo { get; set; }
        public string TaxNo { get; set; }
        public string FSSAI { get; set; }
        public string ContactNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserFullName { get; set; }
        public string HSNNumber { get; set; }
        public decimal TotalTax { get; set; }
        public long CategoryTypeID { get; set; }
        public int RowNumber { get; set; }
        public int Totalcount { get; set; }
        public decimal ATotalAmount { get; set; }
        public decimal AGrandTotal { get; set; }
        public decimal Totalrecord { get; set; }
        public string GrandAmtWord { get; set; }
        public string PONumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public decimal TotalMRP { get; set; }
        public decimal TotalQuantity { get; set; }
        public string InvoiceTitle { get; set; }
        public string NoofInvoice { get; set; }
        public int NoofInvoiceint { get; set; }
        public int divider { get; set; }
        public int OrdRowNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string FSSAIValidUpTo { get; set; }
        public DateTime FSSAIValidUpTo1 { get; set; }

        public string ArticleCode { get; set; }
        public bool IsShow { get; set; }

        public decimal InvTotal { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal TCSTax { get; set; }
        public decimal GrandCreditedTotal { get; set; }

        // 25 March, 2021 Sonal Gandhi
        public string EInvoiceNumber { get; set; }
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
        public string TaxNo2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingStateCode { get; set; }
        public string BillingAreaName { get; set; }

        //Add By Dhruvik
        public decimal Margin { get; set; }
        public decimal SPDiscount { get; set; }
        //Add By Dhruvik
    }

    public class RetOrderPackListResponse
    {
        public long OrderID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string Status { get; set; }
        public string date { get; set; }
        public bool IsPrintPackList { get; set; }
        public decimal Quantity { get; set; }
        public string PONumber { get; set; }
        public decimal UpdateQuantity { get; set; }
        public long CustomerID { get; set; }
        public char Tag { get; set; }

        // 27-06-2022
        public bool IsCheckDone { get; set; }
    }

    public class RetOrderPackListForExport
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string SKU { get; set; }
        public int QtyPackage { get; set; }
        public int QtyAvailable { get; set; }
        public int QtyPacked { get; set; }
        public string Total { get; set; }
    }

    public class RetOrderPackList
    {
        public string OrderID { get; set; }
        public long ProductQtyID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int QuantityPackage { get; set; }
        public int UpdateQuantityPackage { get; set; }
        public decimal Total { get; set; }
        public decimal TotalSum { get; set; }
        public int QtyPacked { get; set; }
        public int QtyAvailable { get; set; }
        public long ProductID { get; set; }
        public int TotalQuantityPackage { get; set; }
        public int TotalQtyPacked { get; set; }
        public int TotalQtyAvailable { get; set; }
        public int Section { get; set; }
        public decimal MRP { get; set; }
        public decimal GramPerKG { get; set; }
        public string ProductBarcode { get; set; }
        public string NutritionValue { get; set; }
        public string ContentValue { get; set; }
        public long CategoryID { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        public string TotalEnergy { get; set; }
        public string Information { get; set; }
        public string PouchName { get; set; }
        public string CustomerName { get; set; }
        public string PONumber { get; set; }
        public long OrderQtyID { get; set; }
        public string AreaName { get; set; }
        public long CustomerID { get; set; }
        public long AreaID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalKG { get; set; }

        public long PouchSize { get; set; }

        // 21-06-2022
        public bool IsPackDone { get; set; }

    }

    public class RetOrderSummaryList
    {
        public string OrderID { get; set; }
        public long ProductQtyID { get; set; }
        public long CustomerID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string SKU { get; set; }
        public int QuantityPackage { get; set; }
        public decimal Total { get; set; }
        public decimal TotalSum { get; set; }
        public int QtyPacked { get; set; }
        public int QtyAvailable { get; set; }
        public long ProductID { get; set; }
        public int TotalQuantityPackage { get; set; }
        public int TotalQtyPacked { get; set; }
        public int TotalQtyAvailable { get; set; }
        public int Section { get; set; }
    }

    public class RetOrderSummaryListResponse
    {
        public long Section { get; set; }
        public long OrderID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string Status { get; set; }
        public string date { get; set; }
    }

    public class picklistreq
    {
        public string selecteddate { get; set; }
        public string selectid { get; set; }
    }

    public class ExportToExcelInvoice
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
        public string InvoiceNumberWithDate { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class RetInvoiceForExcelList
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

        //29th April,2021 Sonal Gandhi
        public string IRN { get; set; }
        public string InvoiceNumberWithDate { get; set; }
    }

    public class RetChallanViewModel
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
        public decimal TotalDiscount { get; set; }
        public List<RetChallanQtyViewModel> lstOrderQty { get; set; }
    }

    public class RetChallanQtyViewModel
    {
        public long ChallanQtyID { get; set; }
        public long ProductID { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string HSNNumber { get; set; }
        public long CategoryTypeID { get; set; }
        public long ProductQtyID { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal SaleRate { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal FinalTotal { get; set; }
    }

    public class GetRetUnitRate
    {
        public long ProductID { get; set; }
        public string UnitCode { get; set; }
        public decimal ProductPrice { get; set; }
        public string HSNNumber { get; set; }
    }

    public class GetRetSellPrice1
    {
        public long ProductID { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Tax { get; set; }
        public long CategoryTypeID { get; set; }
    }

    public class RetChallanQtyInvoiceList
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

    public class RetChallanListResponse
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

    public class RetChallanQtyList
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

    public class RetChallanForExcelList
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

    public class ExportToExcelChallanRetail
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

    public class PrintLabelContent
    {
        public string CustomerName { get; set; }
        public string PONumber { get; set; }
        public string ProductName { get; set; }
        public string AreaName { get; set; }
        public string BagNo { get; set; }
        public string TotalKG { get; set; }
        public string OrderDate { get; set; }
        public string Tag { get; set; }
        // public string OrderQtyID { get; set; }       
    }

    public class PrintLabelContent1
    {
        public string ProductName { get; set; }

    }


    public class RetPackSummaryResponse
    {
        public int Bag { get; set; }
        public int Tray { get; set; }
        public int Zabla { get; set; }
        public int Box { get; set; }
    }


    public class RetCustomeDetail
    {
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string PONumber { get; set; }
        public decimal GrandTotalKG { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string FullInvoiceNumber { get; set; }
        public string TempoNo { get; set; }
    }
    public class RetPackSummary
    {
        public string ProductName { get; set; }
        public string Bag { get; set; }
        public int Box { get; set; }
        public int Zabla { get; set; }
        public int Tray { get; set; }
        public string SrNumber { get; set; }
        public decimal TotalKG { get; set; }
        public long RowNumber { get; set; }
        public long PackSummaryID { get; set; }
        public bool IsDelete { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }
        public string PONumber { get; set; }
        public DateTime OrderDate { get; set; }
        public long OrderID { get; set; }
        public long OrderQtyID { get; set; }
        public decimal Quantity { get; set; }
        public long PackSummaryQtyID { get; set; }
        public string Unit { get; set; }
        public decimal SumTotalKG { get; set; }
        public string Tag { get; set; }

    }


    public class RetPackTotal
    {
        public int Bag { get; set; }
        public string Bagstr { get; set; }
        public int Box { get; set; }
        public string Boxstr { get; set; }
        public int Zabla { get; set; }
        public string Zablastr { get; set; }
        public int Tray { get; set; }
        public string Traystr { get; set; }
        public string Tag { get; set; }
        //  public string SrNumber { get; set; }
    }

    public class TotalContainer
    {
        public string Container { get; set; }
    }

    public class RetDecateNumberResponse
    {
        public long TransportID { get; set; }
        public string ContactNumber { get; set; }
        public string DocketNo { get; set; }
        public DateTime DocketDate { get; set; }
    }



    public class ExportOrderViewModel
    {
        public bool DeActiveCustomer { get; set; }
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long CountryID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CountryOfFinalDestination { get; set; }
        public string PreCarriageBy { get; set; }
        public string PlaceOfReceiptByPreCarrier { get; set; }
        public string VesselName { get; set; }
        public string PortofLoading { get; set; }
        public string PortofDischarge { get; set; }
        public string FinalDestination { get; set; }
        public string Delivery { get; set; }
        public string Payment { get; set; }
        public string BuyersOrderNo { get; set; }
        public decimal TotalNetWeight { get; set; }
        public decimal TotalGrossWeight { get; set; }
        public int TotalPkgs { get; set; }
        public string ContainerNo { get; set; }
        public decimal InvoiceTotalAmt { get; set; }
        public string InsuranceText { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string FreightText { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal GrandInvoiceTotalAmt { get; set; }
        public long ProductID { get; set; }
        public long ProductQtyID { get; set; }
        public bool IsFinalised { get; set; }
        //public decimal FinaOrderlTotal { get; set; }
        //public decimal TotalDiscount { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public string DeleteItems { get; set; }
        public List<ExportOrderQtyViewModel> lstOrderQty { get; set; }
    }

    public class ExportOrderQtyViewModel
    {
        public long OrderQtyID { get; set; }
        public string CartonNo { get; set; }
        public int Carton { get; set; }
        public long ProductID { get; set; }
        public long ProductQtyID { get; set; }
        public long CategoryTypeID { get; set; }
        public string PackateInEachCarton { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public string HSNNumber { get; set; }
        public decimal InvoiceTotalAmt { get; set; }
        //public string ProductName { get; set; }
        //public decimal Tax { get; set; }
        //public decimal TaxAmount { get; set; }          
        //public decimal ProductMRP { get; set; }
        //public decimal DiscountPrice { get; set; }
        //public decimal DiscountPer { get; set; }
        //public string ArticleCode { get; set; }
    }


    public class ExportProductDetails
    {
        public long ProductID { get; set; }
        public long CategoryTypeID { get; set; }
        public string HSNNumber { get; set; }
    }

    public class ExportOrderListResponse
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal Rupees { get; set; }
    }


    public class ExportOrderQtyInvoiceList
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CountryOfFinalDestination { get; set; }
        public string PreCarriageBy { get; set; }
        public string PlaceOfReceiptByPreCarrier { get; set; }
        public string VesselName { get; set; }
        public string PortofLoading { get; set; }
        public string PortofDischarge { get; set; }
        public string FinalDestination { get; set; }
        public string Delivery { get; set; }
        public string Payment { get; set; }
        public string BuyersOrderNo { get; set; }
        public decimal TotalGrossWeight { get; set; }
        public decimal TotalNetWeight { get; set; }
        public decimal TotalPkgs { get; set; }
        public string ContainerNo { get; set; }
        public string InsuranceText { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string FreightText { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal GrandInvoiceTotalAmt { get; set; }
        public DateTime CreatedOn { get; set; }
        public long OrderID { get; set; }
        public string NoofInvoice { get; set; }
        public int NoofInvoiceint { get; set; }
        public string CustomerNote { get; set; }
        public string BillTo { get; set; }
        public string DeliveryTo { get; set; }
        public string LBTNo { get; set; }
        public string TaxNo { get; set; }
        public string FSSAI { get; set; }
        public string FSSAIValidUpTo { get; set; }
        public long CategoryTypeID { get; set; }
        public string ContryWiseProductName { get; set; }
        public long ProductQuantity { get; set; }
        public string UnitCode { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string PackateInEachCarton { get; set; }
        public string CartonNo { get; set; }
        public int Carton { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal InvoiceTotalAmt { get; set; }
        public string GrandAmtWord { get; set; }
        public string HSNNumber { get; set; }
        public string UserFullName { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliveryAddressLine2 { get; set; }
        public string AreaName { get; set; }
        public string PinCode { get; set; }
        public string IncomeTaxNo { get; set; }
        public string GSTIN { get; set; }
        public string NumberToWords { get; set; }
        public long RowNumber { get; set; }
    }


    public class ExpOrderListResponse
    {
        // public long ReturnOrderID { get; set; }
        //public string CreditMemoNumber { get; set; }
        public long ProductCategoryID { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        // public bool Isclear { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public decimal FinalTotal { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Rupees { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }
    public class ExpOrderQtyList
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
        public string FreightText { get; set; }
        public decimal FreightAmt { get; set; }
        public string InsuranceText { get; set; }
        public decimal InsuranceAmt { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class RetEInvoice
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


    public class RetEInvoiceIRN
    {
        public string IRN { get; set; }
    }



    public class RetEInvoiceCreditMemo
    {
        public long EInvoiceCreditMemoId { get; set; }
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


    public class RetEInvoiceCreditMemoIRN
    {
        public string IRN { get; set; }
    }

    public class RetEInvoiceErrorListResponse
    {
        public long EInvoiceErrorDetailsID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ErrorCodes { get; set; }
        public string ErrorDesc { get; set; }
    }

    public class ExportRetEInvoiceErrorList
    {
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ErrorCodes { get; set; }
        public string ErrorDesc { get; set; }
    }


    //29th April,2021 Sonal Gandhi
    public class RetEWayBill
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

    // 24 May, 2021 Sonal Gandhi
    public class RetDetailsForEWB
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
        public string InvoiceNumberWithDate { get; set; }
        public DateTime DocDate1 { get; set; }
        public string DocDate { get; set; }
    }

    //24 May,2021 Sonal Gandhi
    public class RetEWayBillChallan
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


    //25 May,2021 Sonal Gandhi
    public class RetChallanDetailForEWB
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

    //25 May,2021 Sonal Gandhi
    public class RetChallanItemForEWB
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


    // 18 Jan, 2022 Piyush Limbani
    public class TotalPouchListGodownWise
    {
        public long QuantityPackage { get; set; }
        public string GodownName { get; set; }
        public long PouchSize { get; set; }
    }



}