
namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AddGroundStock
    {
        public long GroundStockID { get; set; }

        public long ProductID { get; set; }

        public decimal GroundStockQuantity { get; set; }

        public decimal MinGroundStockQuantity { get; set; }

        public string GroundStockDescription { get; set; }

        public long GodownID { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GroundStockListResponse
    {
        public long GroundStockID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal GroundStockQuantity { get; set; }

        public decimal MinGroundStockQuantity { get; set; }

        public string GroundStockDescription { get; set; }

        public long GodownID { get; set; }

        public string GodownName { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class AddGroundStockInward
    {
        public long InwardID { get; set; }

        public long PurchaseQtyID { get; set; }

        public long PurchaseID { get; set; }

        public long ProductID { get; set; }

        public DateTime BillDate { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal NetWeight { get; set; }

        public decimal NoofBags { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GroundStockInwardListResponse
    {
        public long PurchaseQtyID { get; set; }

        public long PurchaseID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal NetWeight { get; set; }

        public DateTime BillDate { get; set; }

        public int NoofBags { get; set; }

        //public int GroundStockQuantity { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }

        public bool IsInward { get; set; }
    }

    public class GetGroundStockQuantity
    {
        public int GroundStockQuantity { get; set; }
    }

    public class ProductNameDDL
    {
        public long ProductID { get; set; }

        public string ProductName { get; set; }
    }

    public class AddGroundStockTransfer
    {
        public long GroundStockTransferID { get; set; }

        public long ProductID { get; set; }

        public decimal StockTransferQuantity { get; set; }

        public decimal MinStockTransferQuantity { get; set; }

        public string StockTransferDescription { get; set; }

        public long GodownID { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GroundStockTransferListResponse
    {
        public long GroundStockTransferID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal StockTransferQuantity { get; set; }

        public decimal MinStockTransferQuantity { get; set; }

        public string StockTransferDescription { get; set; }

        public long GodownID { get; set; }

        public string GodownName { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

    public class GroundStockTransferInwardListResponse
    {
        public long ChallanQtyID { get; set; }

        public long ChallanID { get; set; }

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public long GodownIDFrom { get; set; }

        public long GodownIDTo { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal StockQuantityFrom { get; set; }

        public decimal Quantity { get; set; }

        public DateTime ChallanDate { get; set; }

        public string ChallanDatestr { get; set; }

        public decimal StockTransferQuantity { get; set; }

        public string From_Place { get; set; }

        public string To_Place { get; set; }

        public decimal FinalTotal { get; set; }

        public int TotalItem { get; set; }


        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }

        public bool IsInward { get; set; }
    }


    public class GroundStockTransferInwardResponse
    {
        public long ChallanID { get; set; }

        public DateTime ChallanDate { get; set; }

        public string ChallanDatestr { get; set; }

        public string From_Place { get; set; }

        public string To_Place { get; set; }

        public decimal FinalTotal { get; set; }

        public int TotalItem { get; set; }

        public bool IsDelete { get; set; }

        public bool IsInward { get; set; }

    }

    public class AddGroundStockTransferInward
    {
        public long TransferInwardID { get; set; }

        public long ChallanQtyID { get; set; }

        public long ChallanID { get; set; }

        public long ProductID { get; set; }

        public long GodownIDFrom { get; set; }

        public long GodownIDTo { get; set; }

        public decimal StockQuantityFrom { get; set; }

        public DateTime ChallanDate { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal LoadingQty { get; set; }

        public decimal Quantity { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }



    //Outward
    public class GroundStockOutwardListResponse
    {

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal OutwardCloseQty { get; set; }

        public decimal NetWeight { get; set; }

        public int GroundStockQuantity { get; set; }

        public bool IsDelete { get; set; }
    }

    public class AddGroundStockOutward
    {
        public long OutwardID { get; set; }

        public long ProductID { get; set; }

        public DateTime OutwardDate { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal NetWeight { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDelete { get; set; }
    }

}
