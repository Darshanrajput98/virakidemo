
namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AddColdStorage
    {
        public long ColdStorageID { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string GSTNo { get; set; }

        public string PANNo { get; set; }

        public string FssaiLicenseNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonName1 { get; set; }

        public string ContactPersonName2 { get; set; }

        public string ContactNumber { get; set; }

        public string ContactNumber1 { get; set; }

        public string ContactNumber2 { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsDelete { get; set; }
    }

    public class ColdStorageListResponse
    {
        public long ColdStorageID { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string GSTNo { get; set; }

        public string PANNo { get; set; }

        public string FssaiLicenseNo { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string ExpiryDatestr { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonName1 { get; set; }

        public string ContactPersonName2 { get; set; }

        public string ContactNumber { get; set; }

        public string ContactNumber1 { get; set; }

        public string ContactNumber2 { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GetColdStorageList
    {
        public long ColdStorageID { get; set; }

        public string Name { get; set; }
    }

    public class ColdStorageName1
    {
        public long ColdStorageID { get; set; }

        public string Name { get; set; }
    }

    public class AddInwardDetails
    {
        public long InwardID { get; set; }

        public long InwardQtyID { get; set; }

        public long SupplierID { get; set; }

        public long ColdStorageID { get; set; }

        public string ColdStorageName { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Datestr { get; set; }

        public string LotNo { get; set; }

        public string DeliveryChallanNumber { get; set; }

        public string BillNumber { get; set; }

        public DateTime DeliveryChallanDate { get; set; }

        public string DeliveryChallanDatestr { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }

        public bool DeActiveColdStorage { get; set; }

        public decimal GrandTotal { get; set; }

        public List<AddInwardQtyDetail> lstInwardQty { get; set; }



        public DateTime ExpiryDate { get; set; }

        public string ExpiryDatestr { get; set; }

    }

    public class AddInwardQtyDetail
    {
        public long InwardQtyID { get; set; }

        public long ProductID { get; set; }

        public long InwardID { get; set; }

        public string Notes { get; set; }

        public string HSNNumber { get; set; }

        public int NoofBags { get; set; }

        public decimal WeightPerBag { get; set; }

        public decimal TotalWeight { get; set; }

        public decimal RatePerKG { get; set; }

        public decimal RentPerBags { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class InwardListResponse
    {
        //Outward ColdStorage 
        public long OutwardID { get; set; }

        public long InwardID { get; set; }

        public int TotalQuantity { get; set; }

        public int Quantity { get; set; }

        public DateTime Outward_date { get; set; }

        public long GodownIDFrom { get; set; }

        public string GodownNameFrom { get; set; }

        public long GodownIDTo { get; set; }

        public string GodownNameTo { get; set; }

        //Outward ColdStorage 

        //Inward ColdStorage
        public long InwardQtyID { get; set; }

        public long SupplierID { get; set; }

        public long ColdStorageID { get; set; }

        public string ColdStorageName { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        public string LotNo { get; set; }

        public string DeliveryChallanNumber { get; set; }

        public string DeliveryChallanDate { get; set; }

        public long CreatedBy { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public Nullable<DateTime> UpdatedOn { get; set; }

        public bool IsDelete { get; set; }

        public bool DeActiveColdStorage { get; set; }

        public decimal GrandTotal { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public string Notes { get; set; }

        public string HSNNumber { get; set; }

        public long NoofBags { get; set; }

        public decimal WeightPerBag { get; set; }

        public decimal TotalWeight { get; set; }

        public decimal RatePerKG { get; set; }

        // 28-06-2022
        public decimal RentPerBags { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal RemQty { get; set; }

        public decimal UsedQty { get; set; }


        public string ExpiryDate { get; set; }

        public List<AddInwardQtyDetail> lstInwardQty { get; set; }

        public string lblTotal { get; set; }
    }

    public class InwardQtyListResponse
    {
        public long InwardQtyID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public long InwardID { get; set; }

        public string InwardName { get; set; }

        public string Notes { get; set; }

        public string HSNNumber { get; set; }

        public int NoofBags { get; set; }

        public decimal WeightPerBag { get; set; }

        public decimal TotalWeight { get; set; }

        public decimal RatePerKG { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    /// OutwardList Response 
    public class OutwardListResponse
    {
        public long OutwardID { get; set; }

        public long InwardID { get; set; }

        public string ProductName { get; set; }

        public int NoofBags { get; set; }

        public int TotalQuantity { get; set; }

        public int Quantity { get; set; }

        public string DeliveryChallanNumber { get; set; }

        public decimal RemQty { get; set; }

        public string Date { get; set; }

        public string LotNo { get; set; }

        public string Outward_date { get; set; }

        public long ColdStorageID { get; set; }

        public string Name { get; set; }

        public string GodownNameTo { get; set; }

        public decimal UsedNoofbags { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }


        public long GodownIDTo { get; set; }

    }

    //ColdStorageName DDL
    public class ColdStorageNameDDL
    {
        public long ColdStorageID { get; set; }

        public string Name { get; set; }
    }


    /// StockReport Response List 
    public class StockReportResponseList
    {

        public long ColdStorageID { get; set; }

        public string Name { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public string Date { get; set; }

        public string LotNo { get; set; }

        public int NoofBags { get; set; }

        public int UsedQuantity { get; set; }

        public decimal WeightPerBag { get; set; }

        public decimal RatePerKG { get; set; }

        public int RemNoofBags { get; set; }

        public decimal TotalWeight { get; set; }

        public decimal TotalAmount { get; set; }

        public int sumNoofBags { get; set; }

        public decimal sumTotalWeight { get; set; }

        public decimal sumTotalAmount { get; set; }

        // 21-12-2022
        public string ExpiryDate { get; set; }
    }

    /// Stock Search
    public class StockSearchRequest
    {
        public long ProductID { get; set; }

        public long ColdStorageID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    /// Outward Search
    public class OutwardSearchRequest
    {
        public long ColdStorageID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }


    public class InwardSearchRequest
    {
        public DateTime? challanDate { get; set; }

        public long ColdStorageID { get; set; }

        public string LotNo { get; set; }

        public long ProductID { get; set; }
    }


    public class DynamicList
    {
        public string Total { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalAmount { get; set; }

        public List<InwardListResponse> lstInward { get; set; }
    }

}
