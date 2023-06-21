using vb.Data.ViewModel;


namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel;

    public class RetCustomerGroupViewModel
    {
        public long CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerGroupAddress1 { get; set; }
        public string CustomerGroupAddress2 { get; set; }
        public long AreaID { get; set; }
        public string CustomerGroupDescription { get; set; }
        public Boolean IsShow { get; set; }
    }

    public class RetCustomerGroupListResponse
    {
        public long CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerGroupAddress1 { get; set; }
        public string CustomerGroupAddress2 { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string CustomerGroupDescription { get; set; }
        public Boolean IsDelete { get; set; }
        public Boolean IsShow { get; set; }
    }

    public class RetCustomerAddressViewModel
    {
        public long CustomerAddressID { get; set; }
        public long AddressID { get; set; }
        public long CustomerID { get; set; }
        public string Name { get; set; }
        public string RoleDescription { get; set; }
        public string CellNo { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }

    public class RetCustomerViewModel
    {
        public long CustomerID { get; set; }
        public long CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public long CustomerGroupID { get; set; }
        public long AreaID { get; set; }
        public string TaxNo { get; set; }
        public string TaxNo2 { get; set; }
        public long UserID { get; set; }
        public long TaxID { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public string NoofInvoice { get; set; }
        public string FSSAI { get; set; }
        public string FSSAI2 { get; set; }
        public DateTime? FSSAIValidUpTo { get; set; }
        public DateTime? FSSAIValidUpTo2 { get; set; }
        public int CustomerTypeID { get; set; }
        public string LBTNo { get; set; }
        public Boolean CallWeek1 { get; set; }
        public Boolean CallWeek2 { get; set; }
        public Boolean CallWeek3 { get; set; }
        public Boolean CallWeek4 { get; set; }
        public string CustomerNote { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFCCode { get; set; }
        public long DeliveryAreaID { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliveryAddressLine2 { get; set; }
        public string DeliveryAddressPincode { get; set; }
        public decimal DeliveryAddressDistance { get; set; }
        public long BillingAreaID { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public long CountryID { get; set; }
        public Boolean IsReflectInVoucher { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Boolean IsDelete { get; set; }
        public string address { get; set; }
        public List<CustomerAddressViewModel> lstAddress { get; set; }
        public string FSSAICertificate { get; set; }
        public string FSSAICertificate2 { get; set; }

        public string CellNo1 { get; set; }
        public string CellNo2 { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool IsTCSApplicable { get; set; }
        public string PanCard { get; set; }
    }

    public class RetCustomerListForExp
    {
        public string CustomerName { get; set; }
        public long CustomerNumber { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string AreaName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TaxNo { get; set; }
        public string FSSAI { get; set; }
        public string TaxName { get; set; }
        public string CustomerTypeName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string SalesPerson { get; set; }
    }

    public class RetCustomerListResponse
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactMobNumber { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public long CustomerNumber { get; set; }
        public long CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string TaxNo { get; set; }
        public string TaxNo2 { get; set; }
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public long TaxID { get; set; }
        public string TaxName { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public string NoofInvoice { get; set; }
        public string FSSAI { get; set; }
        public string FSSAI2 { get; set; }
        public DateTime FSSAIValidUpTo { get; set; }
        public string FSSAIValidUpTostr { get; set; }
        public DateTime FSSAIValidUpTo2 { get; set; }
        public string FSSAIValidUpTostr2 { get; set; }
        public int CustomerTypeID { get; set; }
        public string CustomerTypeName { get; set; }
        public string LBTNo { get; set; }
        public Boolean CallWeek1 { get; set; }
        public Boolean CallWeek2 { get; set; }
        public Boolean CallWeek3 { get; set; }
        public Boolean CallWeek4 { get; set; }
        public string CustomerNote { get; set; }
        public Boolean IsDelete { get; set; }
        public long DeliveryAreaID { get; set; }
        public string DeliveryAreaName { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliveryAddressLine2 { get; set; }
        public string DeliveryAddressPincode { get; set; }
        public string DeliveryAddressDistance { get; set; }
        public long BillingAreaID { get; set; }
        public string BillingAreaName { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public int DaysofWeek { get; set; }
        public string DaysofWeekstr { get; set; }
        public Boolean CallWeek { get; set; }
        public string CellNo { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFCCode { get; set; }
        public long DaysRemaining { get; set; }
        public long DaysRemaining2 { get; set; }
        public string SalesPerson { get; set; }
        public long CountryID { get; set; }
        public string FSSAICertificate { get; set; }
        public string FSSAICertificatepath { get; set; }
        public string FSSAICertificate2 { get; set; }
        public string FSSAICertificatepath2 { get; set; }
        public Boolean IsReflectInVoucher { get; set; }
        public string CellNo1 { get; set; }
        public string CellNo2 { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool IsTCSApplicable { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PanCard { get; set; }
    }

    public class RetCustomerDiscountListResponse
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsDelete { get; set; }
        public long ProductID { get; set; }
        public string Contract { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<ProductDiscount> lstProductDiscount { get; set; }
        public List<CategoryDiscount> lstCategoryDiscount { get; set; }
    }

    public class ProductDiscount
    {
        public long ProductDiscountID { get; set; }
        public long CustomerID { get; set; }
        public string Contract { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long ProductQtyID { get; set; }
        public decimal Discount { get; set; }
        public bool IsDelete { get; set; }

        public decimal Margin { get; set; }

    }

    public class CategoryDiscount
    {
        public long CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypestr { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public Boolean IsDelete { get; set; }
        public long CategoryDiscountID { get; set; }
        public long CustomerID { get; set; }
        public string Contract { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal Discount { get; set; }

        public decimal Margin { get; set; }

    }

    public enum RetCustomerType
    {
        [Description("Wholesale")]
        Wholesale = 1,
        [Description("Retail")]
        Retail = 2,
    }

    public class RetCustomerName1
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

    public class RetCustomerListExport
    {
        public long CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string DaysofWeek { get; set; }
        public string AreaName { get; set; }
        public string SalesPerson { get; set; }
        public string CustomerGroupName { get; set; }
        public string TaxName { get; set; }
        public string LBTNo { get; set; }
        public string DeliveryAreaName { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliveryAddressLine2 { get; set; }
        public string DeliveryAddressPincode { get; set; }
        public string TaxNo { get; set; }
        public string FSSAI { get; set; }
        public string FSSAIValidUpTo { get; set; }
        public string BillingAreaName { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string TaxNo2 { get; set; }
        public string FSSAI2 { get; set; }
        public string FSSAIValidUpTo2 { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactMobNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFCCode { get; set; }
        public string CustomerNote { get; set; }
        public string CellNo1 { get; set; }
        public string CellNo2 { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
    }


    public class RetCustomerArticleCodeListResponse
    {
        public long CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public string ArticleCode { get; set; }
        public bool IsDelete { get; set; }
        public long ProductID { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<ProductArticleCode> lstProductArticleCode { get; set; }
    }

    public class ProductArticleCode
    {
        public long ProductArticleCodeID { get; set; }
        public long CustomerGroupID { get; set; }
        public long ProductQtyID { get; set; }
        public string ArticleCode { get; set; }
        public bool IsDelete { get; set; }
    }

    public class RetCustomerFSSAIExpireListExp
    {
        public string CustomerName { get; set; }
        public string DeliveryArea { get; set; }
        public string BillingArea { get; set; }
        public string SalesPerson { get; set; }
        public string FSSAIValidUpTo { get; set; }
        public string FSSAI { get; set; }
        public long DaysRemaining { get; set; }
        public string FSSAI2 { get; set; }
        public string FSSAIValidUpTo2 { get; set; }
        public long DaysRemaining2 { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }


}
