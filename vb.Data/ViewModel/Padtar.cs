
namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    //Whole Material
    public class AddWholeMaterialRequest
    {
        public long MaterialID { get; set; }

        public long ProductID { get; set; }

        public long CategoryID { get; set; }

        public string Category { get; set; }

        public decimal GST { get; set; }

        public decimal CurrentRate { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public decimal APMC { get; set; }

        public decimal APMCAmount { get; set; }

        public decimal MarketFinal { get; set; }

        public decimal GrossRate { get; set; }

        public decimal KG_P_Hour { get; set; }

        public decimal Labour_P_Hour { get; set; }

        public decimal Gasara { get; set; }

        public decimal GasaraAmount { get; set; }

        public decimal SellRateWholesale { get; set; }

        public decimal SellRateRetail { get; set; }

        public decimal MarginWholesale { get; set; }

        public decimal MarginRetail { get; set; }

        public decimal Discount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal NetAmount { get; set; }

        public decimal PackingCharge { get; set; }

        public decimal Freight_P_KG { get; set; }

        public decimal Commision_P_KG { get; set; }

        public decimal LabourAmount_P_KG { get; set; }

        public decimal Padtar { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class WholeMaterialListResponse
    {
        public long MaterialID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public long CategoryID { get; set; }

        public string Category { get; set; }

        public decimal GST { get; set; }

        public decimal CurrentRate { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public decimal APMC { get; set; }

        public decimal APMCAmount { get; set; }

        public decimal MarketFinal { get; set; }

        public decimal GrossRate { get; set; }

        public decimal KG_P_Hour { get; set; }

        public decimal Labour_P_Hour { get; set; }

        public decimal Gasara { get; set; }

        public decimal GasaraAmount { get; set; }

        public decimal SellRateWholesale { get; set; }

        public decimal SellRateRetail { get; set; }

        public decimal MarginWholesale { get; set; }

        public decimal MarginRetail { get; set; }

        public decimal Discount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal NetAmount { get; set; }

        public decimal PackingCharge { get; set; }

        public decimal Freight_P_KG { get; set; }

        public decimal Commision_P_KG { get; set; }

        public decimal LabourAmount_P_KG { get; set; }

        public decimal Padtar { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GetProductDetailForWholeMaterial
    {
        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public long CategoryID { get; set; }

        public string CategoryName { get; set; }

        public decimal GST { get; set; }
    }


    //Powder Spices
    public class PowderSpicesRequest
    {
        public long SpicesID { get; set; }

        public long ProductID { get; set; }

        public long CategoryID { get; set; }

        public decimal CurrentRate { get; set; }

        public decimal GST { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public decimal GrindingCharge { get; set; }

        public decimal Gasara { get; set; }

        public decimal GasaraAmount { get; set; }

        public decimal GrossRate { get; set; }

        public decimal Padtar { get; set; }

        public decimal SellRateWholesale { get; set; }

        public decimal MarginWholesale { get; set; }

        public decimal SellRateRetail { get; set; }

        public decimal MarginRetail { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GetDetailForPowderSpices
    {
        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public long CategoryID { get; set; }

        public string CategoryName { get; set; }

        public decimal GST { get; set; }

        public decimal GrossRate { get; set; }

        public decimal Padtar { get; set; }
    }

    public class PowderSpicesListResponse
    {
        public long SpicesID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public long CategoryID { get; set; }

        public string CategoryName { get; set; }

        public decimal GST { get; set; }

        public decimal CurrentRate { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public decimal GrindingCharge { get; set; }

        public decimal Gasara { get; set; }

        public decimal GasaraAmount { get; set; }

        public decimal GrossRate { get; set; }

        public decimal Padtar { get; set; }

        public decimal SellRateWholesale { get; set; }

        public decimal MarginWholesale { get; set; }

        public decimal SellRateRetail { get; set; }

        public decimal MarginRetail { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    //Premix
    public class PremixRequest
    {
        public string DeleteItems { get; set; }

        public long PremixID { get; set; }

        public decimal RatePerKG { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public decimal Gasara { get; set; }

        public decimal GasaraAmount { get; set; }

        public decimal GrindingCharge { get; set; }

        public decimal PackingCharge { get; set; }

        public decimal MakingCharge { get; set; }

        public decimal GrossRate { get; set; }

        public decimal Padtar { get; set; }

        public decimal SellRateWholesale { get; set; }

        public decimal MarginWholesale { get; set; }

        public decimal SellRateRetail { get; set; }

        public decimal MarginRetail { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }

        public List<PremixItemRequest> lstPremix { get; set; }
    }

    public class PremixItemRequest
    {
        public long PremixQtyID { get; set; }

        public long ProductID { get; set; }

        public decimal CurrentRate { get; set; }

        public decimal Quantity { get; set; }

        public decimal Amount { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class PremixListResponse
    {
        public long PremixID { get; set; }

        public decimal RatePerKG { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public decimal Gasara { get; set; }

        public decimal GasaraAmount { get; set; }

        public decimal GrindingCharge { get; set; }

        public decimal PackingCharge { get; set; }

        public decimal MakingCharge { get; set; }

        public decimal GrossRate { get; set; }

        public decimal Padtar { get; set; }

        public decimal SellRateWholesale { get; set; }

        public decimal MarginWholesale { get; set; }

        public decimal SellRateRetail { get; set; }

        public decimal MarginRetail { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }

        
    }

}
