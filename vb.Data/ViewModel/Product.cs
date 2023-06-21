

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel;

    public class ProductCategoryViewModel
    {
        public long CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }

    public class ProductCategoryListResponse
    {
        public long CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypestr { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public enum CategoryType
    {
        [Description("Food")]
        Food = 1,
        [Description("Noon Food")]
        NoonFood = 2
    }

    public class ProductBarcode
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string FSSAINo { get; set; }
        public string PhoneNo { get; set; }
        public string ProductName { get; set; }
        public string ProductWeight { get; set; }
        public string Batch { get; set; }
        public string GodownName { get; set; }
        public string DatePackaging { get; set; }
        public string MRP { get; set; }
        public string GramPerKG { get; set; }
        public string Productbarcode { get; set; }
        public string QTY { get; set; }
        public string GodownCode { get; set; }
        public string MonthDate { get; set; }
        public decimal NoofBarcode { get; set; }
        public long ProductID { get; set; }
        public string BarcodeImage { get; set; }
        public string NutritionValue { get; set; }
        public string ContentValue { get; set; }


        public string BestBeforeLabel1 { get; set; }
        public string BatchNumberLabel1 { get; set; }
        public string NetWeightLabel1 { get; set; }
        public string DateOfPackingLabel1 { get; set; }
        public string ProductQtyWithUnitLabel1 { get; set; }
        public string BatchNo1 { get; set; }
        public string ProductNameVal1 { get; set; }
        public string BestBeforeVal1 { get; set; }
        public string DateOfPackingVal1 { get; set; }
        public string ContentVal1 { get; set; }
        public string NutritionVal1 { get; set; }
        public string ProductNameVal2 { get; set; }
        public string BestBeforeVal2 { get; set; }
        public string DateOfPackingVal2 { get; set; }
        public string ContentVal2 { get; set; }
        public string NutritionVal2 { get; set; }
        public string BestBeforeLabel2 { get; set; }
        public string BatchNumberLabel2 { get; set; }
        public string NetWeightLabel2 { get; set; }
        public string DateOfPackingLabel2 { get; set; }
        public string ProductQtyWithUnitLabel2 { get; set; }
        public string BatchNo2 { get; set; }
        public string TotalPacket { get; set; }
        public string Weight { get; set; }
        public string TotalWeight { get; set; }
        public string TotalPackMRP { get; set; }
        public string BarcodeNo { get; set; }
        public string WeightPieces { get; set; }
        public string MRPPieces { get; set; }
        public string PlaceOfOrigin { get; set; }
        public bool chkPlaceOfOrigin { get; set; }

        public string SKU { get; set; }
    }

    public class ProductViewModel
    {
        public string DeleteItems { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductAlternateName { get; set; }
        public long CategoryID { get; set; }
        public long GodownID { get; set; }
        public decimal ProductPrice { get; set; }
        public long UnitID { get; set; }
        public long PouchNameID { get; set; }
        public string ProductDescription { get; set; }
        public string HSNNumber { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long CreatedBy { get; set; }
        public List<ProductQtyViewModel> lstProductQty { get; set; }

        public List<OnlineProductQty> lstOnlineProductQty { get; set; }
        public string DeleteOnlineItem { get; set; }

        //19-07-2022
        public decimal SlabForGST { get; set; }
    }

    public class ProductQtyViewModel
    {
        public long ProductQtyID { get; set; }
        public long LowerQty { get; set; }
        public long UpperQty { get; set; }
        public decimal LessAmount { get; set; }
        public decimal SellPrice { get; set; }
    }

    public class ProductListResponse
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductAlternateName { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public decimal ProductPrice { get; set; }
        public long UnitID { get; set; }
        public long PouchNameID { get; set; }
        public string UnitCode { get; set; }
        public string ProductDescription { get; set; }
        public string HSNNumber { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
        public Boolean IsDelete { get; set; }
        public List<ProductQtyViewModel> lstProductQty { get; set; }
        
        //4 June,2021 Sonal Gandhi
        //public Boolean IsOnline { get; set; }

        //19-07-2022
        public decimal SlabForGST { get; set; }
    }

    public class ProductListForExp
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string UnitCode { get; set; }
        public string HSNNumber { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
    }

    public class AddPurchaseProduct
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductAlternateName { get; set; }
        public long CategoryID { get; set; }
        public string ProductDescription { get; set; }
        public string HSNNumber { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long CreatedBy { get; set; }
    }

    public class PurchaseProductListResponse
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductAlternateName { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ProductDescription { get; set; }
        public string HSNNumber { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean IsDelete { get; set; }
    }

    // expense product 18/12/2019
    public class AddExpenseProduct
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class ExpenseProductListResponse
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string HSNNumber { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }


    // 8 june, 2021 Sonal Gandhi
    public class OnlineProductQty
    {
        public long OnlineProductQtyID { get; set; }
        public long ProductID { get; set; }
        public decimal OnlineProductPrice { get; set; }
        public long OnlineQty { get; set; }
        public long UnitID { get; set; }
        public decimal Factoring { get; set; }
        public decimal FactoringAmount { get; set; }
        public decimal PremiumPercentage { get; set; }
        public decimal PremiumPercentageAmt { get; set; }
        public decimal TotalOnlineAmount { get; set; }
        public bool IsOnline { get; set; }
    }


    // 04 Feb, 2022 Piyush Limbani
    public class ProductBarcodeData
    {
        public long PouchSize { get; set; }
        public string PouchName { get; set; }
        public long NoofBarcodes { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string NetWeight { get; set; }
        public string BatchNo { get; set; }
        public string DateOfPacking { get; set; }
        public string MRP { get; set; }
        //public string GramPerKG { get; set; }
        
        public string BestBefore { get; set; }
        public string Ingredients { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        public string TotalEnergy { get; set; }
        public string Information { get; set; }
        public string PGM { get; set; }

        public string PouchSizestr { get; set; }

        // public string NutritionalFacts { get; set; }
    }

    // 04 Feb, 2022 Piyush Limbani
    public class ProductBarcodeDataExp
    {
        //public long PouchSize { get; set; }
        public string PouchSize { get; set; }
        public long NoofBarcodes { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string NetWeight { get; set; }
        public string BatchNo { get; set; }
        public string DateOfPacking { get; set; }
        public string MRP { get; set; }
        //public string GramPerKG { get; set; }
       
        public string BestBefore { get; set; }
        public string Ingredients { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        public string TotalEnergy { get; set; }
        public string Information { get; set; }
        public string PGM { get; set; }
        // public string NutritionalFacts { get; set; }
    }

}
