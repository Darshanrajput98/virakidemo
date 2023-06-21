using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;

namespace vbwebsite.Areas.purchase.Controllers
{
    public class PurchaseController : Controller
    {
        private static object Lock = new object();
        private IPurchaseService _IPurchaseService;
        private IAdminService _IAdminService;
        private ISupplierService _ISupplierService;
        private IProductService _productservice;

        public PurchaseController(IPurchaseService purchaseservice, IAdminService adminservice, ISupplierService supplierservice, IProductService productservice)
        {
            _IPurchaseService = purchaseservice;
            _IAdminService = adminservice;
            _ISupplierService = supplierservice;
            _productservice = productservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult txtSupplierName_TextChanged(string obj)
        {
            int quntity = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["drugQuntityshow"].ToString());
            List<SupplierName1> data = _IPurchaseService.GetTaxForSupplierByTextChange(obj).Select(x => new SupplierName1() { SupplierName = x }).Take(quntity).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult txtSupplierName_AfterSupplierSelect(string obj)
        {
            try
            {
                string[] namearr = obj.Split(',');
                string ID = string.Empty;
                if (namearr.Length > 1)
                {
                    var Tax = _IPurchaseService.GetTaxForSupplierBySupplierID(Convert.ToInt64(namearr[0]));
                    return Json(Tax, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetUnit(long ProductID, string Tax)
        {
            var lstUnit = _IPurchaseService.GetAutoCompleteProductDetaiForPurchase(ProductID, Tax);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPurchase(Int64? purchaseid)
        {
            ViewBag.Product = _IPurchaseService.GetAllProductName();
            ViewBag.PurchaseType = _IAdminService.GetAllPurchaseTypeName(1);
            ViewBag.DebitAccountType = _IAdminService.GetAllPurchaseDebitAccountTypeName();
            ViewBag.Broker = _IAdminService.GetAllBrokerName();
            ViewBag.Godown = _productservice.GetAllGodownName();
            try
            {
                AddPurchaseDetail objModel = _IPurchaseService.GetPurchaseOrderDetailsByPurchaseID(Convert.ToInt64(purchaseid));
                return View(objModel);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult CheckSupplierCurrentYearBillNumber(long SupplierID, string BillNumber, DateTime BillDate)
        {
            try
            {
                //string syear = DateTime.Now.Year.ToString();
                //string StartDate = syear + "-" + "04" + "-" + "01";
                //string eyear = DateTime.Now.AddYears(1).Year.ToString();
                //string EndDate = eyear + "-" + "03" + "-" + "31";

                // 25 Aug 2020 Piyush Limbani
                string StartDate = "";
                string EndDate = "";
                int month = BillDate.Month;
                if (month == 1 || month == 2 || month == 3)
                {
                    string syear = BillDate.AddYears(-1).Year.ToString();
                    StartDate = syear + "-" + "04" + "-" + "01";
                    string eyear = BillDate.Year.ToString();
                    EndDate = eyear + "-" + "03" + "-" + "31";
                }
                else
                {
                    string syear = BillDate.Year.ToString();
                    StartDate = syear + "-" + "04" + "-" + "01";
                    string eyear = BillDate.AddYears(1).Year.ToString();
                    EndDate = eyear + "-" + "03" + "-" + "31";
                }
                // 25 Aug 2020 Piyush Limbani

                string BillNo = _IPurchaseService.CheckSupplierCurrentYearBillNumber(StartDate, EndDate, SupplierID, BillNumber);
                return Json(BillNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddPurchaseBill(AddPurchaseDetail data)
        {
            lock (Lock)
            {
                try
                {
                    if (Request.Cookies["UserID"] == null)
                    {
                        Request.Cookies["UserID"].Value = null;
                        return JavaScript("location.reload(true)");
                    }
                    else
                    {
                        //string syear = data.BillDate.Year.ToString();
                        //string StartDate = syear + "-" + "04" + "-" + "01";
                        //string eyear = data.BillDate.AddYears(1).Year.ToString();
                        //string EndDate = eyear + "-" + "03" + "-" + "31";

                        // 25 Aug 2020 Piyush Limbani
                        string StartDate = "";
                        string EndDate = "";
                        int month = data.BillDate.Month;
                        if (month == 1 || month == 2 || month == 3)
                        {
                            string syear = data.BillDate.AddYears(-1).Year.ToString();
                            StartDate = syear + "-" + "04" + "-" + "01";
                            string eyear = data.BillDate.Year.ToString();
                            EndDate = eyear + "-" + "03" + "-" + "31";
                        }
                        else
                        {
                            string syear = data.BillDate.Year.ToString();
                            StartDate = syear + "-" + "04" + "-" + "01";
                            string eyear = data.BillDate.AddYears(1).Year.ToString();
                            EndDate = eyear + "-" + "03" + "-" + "31";
                        }
                        // 25 Aug 2020 Piyush Limbani

                        if (data.PurchaseID == 0)
                        {
                            data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            data.CreatedOn = DateTime.Now;
                            data.Verify = false;
                        }
                        else
                        {
                            data.CreatedBy = data.CreatedBy;
                            data.CreatedOn = data.CreatedOn;
                            data.Verify = data.Verify;
                        }
                        data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.UpdatedOn = DateTime.Now;
                        string respose = _IPurchaseService.AddPurchaseBill(data, StartDate, EndDate);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult SearchViewPurchaseList()
        {
            ViewBag.Supplier = _ISupplierService.GetAllSupplierName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewPurchaseList(PurcahseListResponse model)
        {
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<PurcahseListResponse> objModel = _IPurchaseService.GetAllPurchaseList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult DeletePurchaseOrder(long? PurchaseID, bool IsDelete)
        {
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    bool respose = _IPurchaseService.DeletePurchaseOrder(PurchaseID.Value, IsDelete);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        // 30-03-2020
        [HttpPost]
        public ActionResult GetLastPurchaseProductRatePerKG(long ProductID)
        {
            var lstUnit = _IPurchaseService.GetLastPurchaseProductRatePerKG(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }



    }
}