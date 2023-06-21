using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;

namespace vbwebsite.Areas.expenses.Controllers
{
    public class ExpenseController : Controller
    {
        private static object Lock = new object();
        private IPurchaseService _IPurchaseService;
        private IAdminService _IAdminService;
        private ISupplierService _ISupplierService;
        private IProductService _productservice;

        public ExpenseController(IPurchaseService purchaseservice, IAdminService adminservice, ISupplierService supplierservice, IProductService productservice)
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
            List<SupplierName1> data = _IPurchaseService.GetTaxForExpenseSupplierByTextChange(obj).Select(x => new SupplierName1() { SupplierName = x }).Take(quntity).ToList();
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
                    var Tax = _IPurchaseService.GetTaxForExpenseSupplierBySupplierID(Convert.ToInt64(namearr[0]));
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
        public ActionResult GetHSNNumber(long ProductID)
        {
            var lstUnit = _IPurchaseService.GetAutoCompleteProductDetaiForExpense(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        // 10-06-2020
        [HttpPost]
        public ActionResult GetLastExpenseProductRate(long ProductID)
        {
            var lstUnit = _IPurchaseService.GetLastExpenseProductRate(ProductID);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTexByExpenseDebitAccountTypeID(long ExpenseDebitAccountTypeID, string Tax)
        {
            var lstUnit = _IPurchaseService.GetAutoCompleteTaxDetailForExpense(ExpenseDebitAccountTypeID, Tax);
            return Json(lstUnit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddExpense(Int64? expenseid)
        {
            ViewBag.Product = _IPurchaseService.GetAllExpenseProductName();
            ViewBag.DebitAccountType = _IAdminService.GetAllExpenseDebitAccountTypeName();
            ViewBag.ExpenseType = _IAdminService.GetAllExpenseTypeName();
            ViewBag.Godown = _productservice.GetAllGodownName();
            ViewBag.TDSCategory = _IAdminService.GetAllTDSCategoryName();
            try
            {
                AddExpenseDetail objModel = _IPurchaseService.GetExpenseOrderDetailsByExpenseID(Convert.ToInt64(expenseid));
                return View(objModel);
                //return View();
            }
            catch
            {
                return View();
            }
        }


        // 25 Aug 2020 Piyush Limbani
        public JsonResult CheckExpenseSupplierCurrentYearBillNumber(long SupplierID, string BillNumber, DateTime BillDate)
        {
            try
            {
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
                string BillNo = _IPurchaseService.CheckExpenseSupplierCurrentYearBillNumber(StartDate, EndDate, SupplierID, BillNumber);
                return Json(BillNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        // 25 Aug 2020 Piyush Limbani


        [HttpPost]
        public ActionResult AddExpenseBill(AddExpenseDetail data)
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
                        if (data.ExpenseID == 0)
                        {
                            data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            data.CreatedOn = DateTime.Now;
                            // data.Verify = false;
                        }
                        else
                        {
                            data.CreatedBy = data.CreatedBy;
                            data.CreatedOn = data.CreatedOn;
                            // data.Verify = data.Verify;
                        }
                        data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.UpdatedOn = DateTime.Now;
                        long respose = _IPurchaseService.AddExpenseBill(data);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult SearchViewExpenseList()
        {
            ViewBag.Supplier = _ISupplierService.GetAllExpenseSupplierName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewExpenseList(ExpenseListResponse model)
        {
            if (model.From.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.From = Convert.ToDateTime(DateTime.Now);
            }
            if (model.To.ToString("MM-dd-yyyy") == "01-01-0001")
            {
                model.To = Convert.ToDateTime(DateTime.Now.AddDays(1));
            }
            List<ExpenseListResponse> objModel = _IPurchaseService.GetAllExpenseList(model);
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult DeleteExpenseOrder(long? ExpenseID, bool IsDelete)
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
                    bool respose = _IPurchaseService.DeleteExpenseOrder(ExpenseID.Value, IsDelete);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }
}