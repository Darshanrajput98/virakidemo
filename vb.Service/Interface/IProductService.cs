
namespace vb.Service
{
    using System.Collections.Generic;
    using vb.Data;
    using vb.Data.Model;

    public interface IProductService
    {
        bool AddProductCategory(Category_Mst ObjCategory);

        List<ProductCategoryListResponse> GetAllProductCategoryList();

        bool DeleteProductCategory(long CategoryID, bool IsDelete);

        List<ProductCategoryListResponse> GetAllCategoryName();

        //List<PouchListResponse> GetAllPouchName();

        List<PouchNameList> GetAllPouchName();

        List<GodownListResponse> GetAllGodownName();

        List<UnitListResponse> GetAllUnitName();
        
        bool AddProduct(ProductViewModel ObjProduct);

        List<ProductListResponse> GetAllProductList();

        bool DeleteProduct(long ProductID, bool IsDelete);

        List<ProductQtyViewModel> GetAllProductQtyListByProductID(long ProductID);

        bool UpdateProduct(List<ProductViewModel> data);

        List<ProductListResponse> GetAllProductListSearchByPrice(int ProductPrice);

        long AddPurchaseProduct(AddPurchaseProduct ObjProduct);

        List<PurchaseProductListResponse> GetAllPurchaseProductList();

        bool DeletePurchaseProduct(long ProductID, bool IsDelete);

        bool UpdatePurchaseProduct(List<AddPurchaseProduct> data);


        // expense product 18/12/2019

        long AddExpenseProduct(ExpenseProduct_Mst ObjProduct);

        List<ExpenseProductListResponse> GetAllExpenseProductList();

        bool DeleteExpenseProduct(long ProductID, bool IsDelete);

        //8 June,2021 Sonal Gandhi
        List<OnlineProductQty> GetAllOnlineProductQtyListByProductID(long ProductID);
    }
}
