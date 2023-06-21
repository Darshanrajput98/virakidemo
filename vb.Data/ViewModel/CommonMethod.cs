using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace vb.Data.ViewModel
{
    public class CommonMethod
    {
        public static bool IsActiveMenu(string SystemName)
        {
            var objMenuList = HttpContext.Current.Session["ActiveMenuList"] as List<RoleWiseActiveAuthorizedManuList>;

            if (objMenuList != null && objMenuList.Any(p => p.SystemName == SystemName))
            {
                return true;
            }

            return false;
        }

        public enum RoleSystemName
        {
            WAAREA,
            WVAREA,
            WEAREA,
            WDAREA,
            WAGODOWN,
            WVGODOWN,
            WEGODOWN,
            WDGODOWN,
            WATAX,
            WVTAX,
            WETAX,
            WDTAX,
            WAUNIT,
            WVUNIT,
            WEUNIT,
            WDUNIT,
            WAUSERS,
            WVUSERS,
            WEUSERS,
            WDUSERS,
            WAROLE,
            WVROLE,
            WEROLE,
            WDROLE,
            WADRIVER,
            WVDRIVER,
            WEDRIVER,
            WDDRIVER,
            WAEVENT,
            WVEVENT,
            WEEVENT,
            WDEVENT,
            WAEVENTDATE,
            WVEVENTDATE,
            WEEVENTDATE,
            WDEVENTDATE,
            WATRANSPORT,
            WVTRANSPORT,
            WETRANSPORT,
            WDTRANSPORT,
            WAPRODUCT,
            WVPRODUCT,
            WEPRODUCT,
            WDPRODUCT,
            WAPRODUCTCATEGORY,
            WVPRODUCTCATEGORY,
            WEPRODUCTCATEGORY,
            WDPRODUCTCATEGORY,
            WACUSTOMER,
            WVCUSTOMER,
            WECUSTOMER,
            WDCUSTOMER,
            WACUSTOMERGROUP,
            WVCUSTOMERGROUP,
            WECUSTOMERGROUP,
            WDCUSTOMERGROUP,
            RAAREA,
            RVAREA,
            REAREA,
            RDAREA,
            RAGODOWN,
            RVGODOWN,
            REGODOWN,
            RDGODOWN,
            RATRANSPORT,
            RVTRANSPORT,
            RETRANSPORT,
            RDTRANSPORT,
            RAUNIT,
            RVUNIT,
            REUNIT,
            RDUNIT,
            RATAX,
            RVTAX,
            RETAX,
            RDTAX,
            RAPRODUCT,
            RVPRODUCT,
            REPRODUCT,
            RDPRODUCT,
            RACATEGORY,
            RVCATEGORY,
            RECATEGORY,
            RDCATEGORY,
            RAPOUCH,
            RVPOUCH,
            REPOUCH,
            RDPOUCH,
            RAPACKAGESTATION,
            RVPACKAGESTATION,
            REPACKAGESTATION,
            RDPACKAGESTATION,
            RACUSTOMER,
            RVCUSTOMER,
            RECUSTOMER,
            RDCUSTOMER,
            RACUSTOMERGROUP,
            RVCUSTOMERGROUP,
            RECUSTOMERGROUP,
            RDCUSTOMERGROUP,
            WEPAYMENT,
            REPAYMENT,
            WAttandance,
            WMobileOrderPrint,
            CHECKLIST,
            //EXPIREDLICENCE,
            //EXPIREDVEHICLE

            AVehicle,
            VVehicle,
            EVehicle,
            DVehicle,
            ALicence,
            VLicence,
            ELicence,
            DLicence,
            ADebitAccountType,
            VDebitAccountType,
            EDebitAccountType,
            DDebitAccountType,
            AExpensesVoucher,
            VExpensesVoucher,
            EExpensesVoucher,
            DExpensesVoucher,
            AInward,
            VInward,
            EInward,
            DInward,
            AVehicleCosting,
            VVehicleCosting,
            EVehicleCosting,
            ASupplier,
            VSupplier,
            ESupplier,
            DSupplier,
            ESupplierFSSAIValidity,

            APurchaseType,
            VPurchaseType,
            EPurchaseType,
            DPurchaseType,

            ABroker,
            VBroker,
            EBroker,
            DBroker,

            APurchaseDebitAccountType,
            VPurchaseDebitAccountType,
            EPurchaseDebitAccountType,
            DPurchaseDebitAccountType,

            ABank,
            VBank,
            EBank,
            DBank,

            GEGroundStock,
            GDGroundStock,

            GEGroundStockTransfer,
            GDGroundStockTransfer,
        }

    }

    public enum HostingEnvironment
    {
        localhost,
        staging,
        live,
    }

    //20 july,2021 Sonal Gandhi
    public class SendMessageData
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal ByCash { get; set; }
        public bool ByCheque { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string IFCCode { get; set; }
        public bool ByCard { get; set; }
        public string BankNameForCard { get; set; }
        public string TypeOfCard { get; set; }
        public bool ByOnline { get; set; }
        public string BankNameForOnline { get; set; }
        public string UTRNumber { get; set; }
        public DateTime? OnlinePaymentDate { get; set; }
        public long CustomerNumber { get; set; }
        public string InvoiceDate { get; set; }
    }

    public class CustomerDetailModel
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

}
