using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.expenses.Controllers
{
    public class AdminController : Controller
    {

        private IAdminService _IAdminservice;
        private ICommonService _ICommonService;
        private IProductService _productservice;


        public AdminController(IAdminService adminservice, ICommonService commonservice, IProductService productservice)
        {
            _IAdminservice = adminservice;
            _ICommonService = commonservice;
            _productservice = productservice;
        }

        //
        // GET: /purchase/Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddExpenseType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddExpenseType(AddExpenseType data)
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
                    ExpenseType_Mst obj = new ExpenseType_Mst();
                    obj.ExpenseTypeID = data.ExpenseTypeID;
                    obj.ExpenseType = data.ExpenseType;
                    if (obj.ExpenseTypeID == 0)
                    {
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                    }
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    long respose = _IAdminservice.AddExpenseType(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ExpenseTypeList()
        {
            List<ExpenseTypeListResponse> objModel = _IAdminservice.GetAllExpenseTypeList();
            return PartialView(objModel);
        }

        public ActionResult DeleteExpenseType(long? ExpenseTypeID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeleteExpenseType(ExpenseTypeID.Value, IsDelete);
                return RedirectToAction("AddExpenseType");
            }
            catch (Exception)
            {
                return RedirectToAction("AddExpenseType");
            }
        }

        public ActionResult AddExpenseDebitAccountType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddExpenseDebitAccountType(AddExpenseDebitAccountType data)
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
                    ExpenseDebitAccountType_Mst obj = new ExpenseDebitAccountType_Mst();
                    obj.ExpenseDebitAccountTypeID = data.ExpenseDebitAccountTypeID;
                    obj.ExpenseDebitAccountType = data.ExpenseDebitAccountType;
                    obj.SGST = data.SGST;
                    obj.CGST = data.CGST;
                    obj.IGST = data.IGST;
                    obj.HFor = data.HFor;
                    if (obj.ExpenseDebitAccountTypeID == 0)
                    {
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                    }
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    long respose = _IAdminservice.AddExpenseDebitAccountType(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ExpenseDebitAccountTypeList()
        {
            List<ExpenseDebitAccountTypeListResponse> objModel = _IAdminservice.GetAllExpenseDebitAccountTypeList();
            return PartialView(objModel);
        }

        public ActionResult DeleteExpenseDebitAccountType(long? ExpenseDebitAccountTypeID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeleteExpenseDebitAccountType(ExpenseDebitAccountTypeID.Value, IsDelete);
                return RedirectToAction("AddExpenseDebitAccountType");
            }
            catch (Exception)
            {
                return RedirectToAction("AddExpenseDebitAccountType");
            }
        }

        public ActionResult AddProduct()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(AddExpenseProduct data)
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
                    ExpenseProduct_Mst obj = new ExpenseProduct_Mst();
                    obj.ProductID = data.ProductID;
                    obj.ProductName = data.ProductName;
                    obj.HSNNumber = data.HSNNumber;
                    if (obj.ProductID == 0)
                    {
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                    }
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    long respose = _productservice.AddExpenseProduct(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ProductList()
        {
            List<ExpenseProductListResponse> objModel = _productservice.GetAllExpenseProductList();
            return PartialView(objModel);
        }

        public ActionResult DeleteProduct(long? ProductID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteExpenseProduct(ProductID.Value, IsDelete);
                return RedirectToAction("AddProduct");
            }
            catch (Exception)
            {

                return RedirectToAction("AddProduct");
            }
        }




        // 17 June 2020
        public ActionResult AddTDSCategory()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult AddTDSCategory(AddTDSCategory data)
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
                    TDSCategory_Mst obj = new TDSCategory_Mst();
                    obj.TDSCategoryID = data.TDSCategoryID;
                    obj.TDSCategory = data.TDSCategory;
                    if (obj.TDSCategoryID == 0)
                    {
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                    }
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    long respose = _IAdminservice.AddTDSCategory(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult TDSCategoryList()
        {
            List<TDSCategoryListResponse> objModel = _IAdminservice.GetAllTDSCategoryList();
            return PartialView(objModel);
        }

        public ActionResult DeleteTDSCategory(long? TDSCategoryID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeleteTDSCategory(TDSCategoryID.Value, IsDelete);
                return RedirectToAction("AddTDSCategory");
            }
            catch (Exception)
            {

                return RedirectToAction("AddTDSCategory");
            }
        }

    }
}