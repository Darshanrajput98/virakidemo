

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel;

    public class RetProductCategoryViewModel
    {
        public long CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }

    public class RetProductCategoryListResponse
    {
        public long CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypestr { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public enum RetCategoryType
    {
        [Description("Food")]
        Food = 1,
        [Description("Noon Food")]
        NoonFood = 2
    }

    public class RetProductViewModel
    {
        public string DeleteItems { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long CategoryID { get; set; }
        public long GodownID { get; set; }
        public long BestBeforeMonthID { get; set; }
        public string ProductDescription { get; set; }
        public string HSNNumber { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal HFor { get; set; }
        public string ContentValue { get; set; }
        public string NutritionValue { get; set; }
        public string PlaceOfOrigin { get; set; }

        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        public string TotalEnergy { get; set; }
        public string Information { get; set; }

        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long CreatedBy { get; set; }
        public List<RetProductQtyViewModel> lstProductQty { get; set; }
        public List<RetProdGuiViewModel> lstRetProdGui { get; set; }

        public List<CountryWiseProductViewModel> lstCountryWiseProduct { get; set; }

    }
    public class CountryWiseProductViewModel
    {
        public long CountryWiseProductID { get; set; }
        public long CountryID { get; set; }
        public long ProductID { get; set; }
        public string CountryWiseProductName { get; set; }
    }
    public class RetProductQtyViewModel
    {
        public Boolean IsSelect { get; set; }
        public long ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductMRP { get; set; }

        public decimal GramPerKG { get; set; }

        public string ProductBarcode { get; set; }
        public string ProductName { get; set; }
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
        public long ProductQtyID { get; set; }
        public long ProductID { get; set; }
        public long UnitID { get; set; }
        public string UnitCode { get; set; }
        public string NutritionValue { get; set; }
        public string ContentValue { get; set; }
        public long CategoryID { get; set; }
        public string UnitName { get; set; }
        public string Weight { get; set; }
        public string PlaceOfOrigin { get; set; }
        public long PouchSize { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        public string TotalEnergy { get; set; }
        public string Information { get; set; }
        public string CustomerName { get; set; }
    }

    public class RetProductQtyListResponse
    {
        public long ProductQtyID { get; set; }
        public string ProductName { get; set; }
    }

    public class RetProductListResponse
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long BestBeforeMonthID { get; set; }
        public string MonthNumber { get; set; }
        public decimal ProductPrice { get; set; }
        public long UnitID { get; set; }
        public string UnitCode { get; set; }
        public string ProductDescription { get; set; }
        public string HSNNumber { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
        public decimal HFor { get; set; }
        public string ContentValue { get; set; }
        public string NutritionValue { get; set; }
        public Boolean IsSelect { get; set; }
        public Boolean IsDelete { get; set; }
        public long ProductQtyID { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        public string TotalEnergy { get; set; }
        public string Information { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PouchViewModel
    {
        public long PouchID { get; set; }
        //public string PouchName { get; set; }
        public long PouchNameID { get; set; }
        public string PouchDescription { get; set; }
        public Nullable<int> PouchQuantity { get; set; }
        public string Material { get; set; }
        public decimal Weight { get; set; }
        public decimal KG { get; set; }
        public long GodownID { get; set; }
        public long MinPouchQuantity { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PouchListResponse
    {
        public long PouchID { get; set; }
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }
        public string PouchDescription { get; set; }
        public int PouchQuantity { get; set; }
        public string Material { get; set; }
        public decimal Weight { get; set; }
        public decimal KG { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long MinPouchQuantity { get; set; }
        public Boolean IsDelete { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PackageStationViewModel
    {
        public long PackageStationID { get; set; }
        public string PackageStationName { get; set; }
        public long GodownID { get; set; }
        public long UserID { get; set; }
    }

    public class PackageStationListResponse
    {
        public long PackageStationID { get; set; }
        public string PackageStationName { get; set; }
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetProductListForExp
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public long ProductQuantity { get; set; }
        public string UnitName { get; set; }

        public long PouchSize { get; set; }

        public string HSNNumber { get; set; }
        //public decimal ProductPrice { get; set; }
        //public decimal ProductMRP { get; set; }
        public string ProductPrice { get; set; }
        public string ProductMRP { get; set; }
        public string ProductBarcode { get; set; }
        public string BestBefore { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }
    }

    // CHIRAG 05-02-2019

    public class RetProductQtyViewModelForExp
    {
        public long ProductQtyID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public long ProductQuantity { get; set; }
        public string UnitName { get; set; }
        public string HSNNumber { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductMRP { get; set; }
        public string ProductBarcode { get; set; }
        public string BestBefore { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal HFor { get; set; }

        //31 May,2021 Sonal Gandhi
        public decimal ConvertedProductPrice { get; set; }
        public decimal ConvertedProductMRP { get; set; }

        public long PouchSize { get; set; }
    }

    public class RetProdGuiViewModel
    {
        public long RetProdGuiID { get; set; }
        public int GuiID { get; set; }
        public long ProductID { get; set; }
        public string ProductNameGui { get; set; }
        public string ContentGui { get; set; }
        public string NutritionGui { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<RetProdQtyGuiViewModel> lstRetProdQtyGui { get; set; }
    }

    public class RetProdQtyGuiViewModel
    {
        public long RetProdGuiQtyID { get; set; }
        public long RetProdGuiID { get; set; }
        public long RetProdcutQtyID { get; set; }
        public long ProductID { get; set; }
        public long ProductQtyID { get; set; }
        public string ProductQtyGui { get; set; }
        public int UnitGuiID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public long CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public long ModifiedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class RetProdWithQtyGuiList
    {
        public long RetProdGuiID { get; set; }
        public int GuiID { get; set; }
        public long ProductID { get; set; }
        public string ProductNameGui { get; set; }
        public string ContentGui { get; set; }
        public string NutritionGui { get; set; }
        public string LanguageName { get; set; }
        public long RetProdGuiQtyID { get; set; }
        public long RetProdcutQtyID { get; set; }
        public long ProductQtyID { get; set; }
        public string ProductQtyGui { get; set; }
        public int UnitGuiID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string ProdQtyUnit { get; set; }
        public string UnitDescription { get; set; }
    }

    public class GuiLanguageViewModel
    {
        public int GuiID { get; set; }
        public string LanguageName { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class GuiLabelViewModel
    {
        public string LanguageName { get; set; }
        public long GuiLabelID { get; set; }
        public long GuiID { get; set; }
        public string LabelCode { get; set; }
        public string LabelValue { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class Resource
    {
        public LabelCodes labelCodes { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
    }

    public class LabelCodes
    {
        public string ExpiryDateLabel { get; set; }
        public string BatchNumberLabel { get; set; }
        public string NetWeightLabel { get; set; }
        public string DateOfPackingLabel { get; set; }
        public string BestBeforeLabel { get; set; }
    }


    public class ProductNameByLanguageID
    {
        public string ProductName1 { get; set; }
        public string ProductName2 { get; set; }
    }


    public class CountryNameModel
    {
        public long CountryID { get; set; }
        public string CountryName { get; set; }
    }


    public class ExpProductWiseSalesList
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long UserID { get; set; }
        public long ProductID { get; set; }
        public long ProductCategoryID { get; set; }
        public long CountryID { get; set; }
        public string ProductName { get; set; }
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public decimal Quantity { get; set; }
        public decimal OrderQuantity { get; set; }
        //public decimal ReturnedQuantity { get; set; }
        public decimal OrderTotalAmount { get; set; }
        //public decimal ReturnOrderAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public string SrNo { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalKg { get; set; }
    }

    public class ExProductMainListByMonth
    {
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ExpProductWiseSalesList> ListMainProduct { get; set; }
    }
    public class ExProductMainListByDay
    {
        public int DayName { get; set; }
        public int MonthName { get; set; }
        public int YearName { get; set; }
        public List<ExpProductWiseSalesList> ListMainProduct { get; set; }
    }

    public class PouchNameListResponse
    {
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }

        public long FontSize { get; set; }
        public string DelayTime { get; set; }
        public long PouchSize { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class PouchNameModel
    {
        public long PouchNameID { get; set; }
        public string PouchName { get; set; }
        public string HSNNumber { get; set; }

        public long FontSize { get; set; }
        public string DelayTime { get; set; }
        public long PouchSize { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PouchExport
    {
        public string PouchName { get; set; }
        public string HSNNo { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Godown { get; set; }
        public long MinQuantity { get; set; }
    }


    //31 May,2021 Sonal Gandhi

    public class CurrencyViewModel
    {
        public long CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySign { get; set; }
        public decimal CurrencyRate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
    }

}
