

namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data;
    using vb.Data.Model;

    public interface IRetProductService
    {
        bool AddProductCategory(RetProductCategoryMst ObjCategory);

        List<RetProductCategoryListResponse> GetAllProductCategoryList();

        bool DeleteProductCategory(long CategoryID, bool IsDelete);

        List<RetProductCategoryListResponse> GetAllRetCategoryName();

        //List<PouchListResponse> GetAllPouchName();

        List<PouchNameList> GetAllPouchName();

        List<RetUnitListResponse> GetAllRetUnitName();

        bool AddProduct(RetProductViewModel ObjProduct);

        List<RetProductListResponse> GetAllProductList();

        bool DeleteProduct(long ProductID, bool IsDelete);

        bool AddPouch(Pouch_Mst ObjPouch);

        List<PouchListResponse> GetAllPouchList();

        bool DeletePouch(long PouchID, bool IsDelete);

        List<GodownListResponse> GetAllRetGodownName();

        List<UserListResponse> GetAllSalesPersonName();

        bool AddPackageStation(PackageStation_Mst ObjPackageStation);

        List<PackageStationListResponse> GetAllPackageStationList();

        bool DeletePackageStation(long PackageStationID, bool IsDelete);

        List<RetMonthListResponse> GetAllMonth();

        List<RetProductQtyViewModel> GetAllRetailProductQtyListByProductID(long ProductID);


        List<CountryWiseProductViewModel> GetCountryWiseProductNameByProductID(long ProductID);


        List<RetProductQtyViewModel> GetRetProductCategoryList();

        bool UpdateProductDetails(List<RetProductQtyViewModel> data);

        Godown_Mst GetGodownDetailsByGodownID(long GodownID);

        BestBeforeMonth GetMonthByProductID(long ID);

        //List<RetProductQtyViewModelForExp> GetRetProductListForExp();

        List<RetProductQtyViewModelForExp> GetRetProductListForExp(decimal CurrencyRate = 0);





        // CHIRAG 05-02-2019

        List<GuiLanguageViewModel> GetAllLanguage();

        List<RetProdGuiViewModel> GetAllRetailProductListByProductID(long ProductID);

        bool AddProductGuiMaster(RetProdGuiViewModel ObjProduct);

        List<RetUnitListResponse> GetAllRetUnitNameByGuiID(long GuiID);

        RetProdGuiViewModel GetRetailProductGuiByID(long ProductID, long GuiID);

        List<RetProdQtyGuiViewModel> GetRetailProductQtyGuiByID(long RetProdGuiID);

        bool AddLanguage(GuiLanguageViewModel data);

        List<GuiLanguageViewModel> GetAllLanguageList(long GuiID);

        bool DeleteLanguage(long? GuiID, bool IsDelete);

        bool AddLabel(GuiLabelViewModel data);

        List<GuiLabelViewModel> GetAllLabelList(long GuiLabelID);

        bool DeleteLabel(long? GuiLabelID, bool IsDelete);

        List<Resource> GetAllLabelByGuiID(long GuiID);

        List<RetProdWithQtyGuiList> GetProductGuiByProductID(long ProductID, long GuiID, long ProductQtyID);

        List<RetUnitListResponse> GetAllRetGuiUnitName();


        ProductNameByLanguageID GetProductNameByLanguageID(long Language1, long ProductIDlabel);

        ProductNameByLanguageID GetProductNameByLanguageID2(long Language2, long ProductIDlabel);


        List<CountryNameModel> GetAllCountryName();

        // Add Barcode QTY Details 30-03-2020
        long AddBarcodeQuantityDetails(string ProductID, long ProductQtyID, string ProductName, DateTime DateOfPackaging, long NoOfBarcodes, long GodownID, long CreatedBy, DateTime CreatedOn, long UpdatedBy, DateTime UpdatedOn, bool IsDelete);


        //31 May,2021 Sonal Gandhi
        List<CurrencyViewModel> GetAllCurrency();

        List<CurrencyViewModel> GetAllCurrencyList(long CurrencyID);

        bool AddCurrency(CurrencyViewModel data);

        bool DeleteCurrency(long? CurrencyID, bool IsDelete);

        string GetCurrencySignByCurrencyID(long currencyID);
    }
}
