using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;


namespace vbwebsite.Areas.groundstock.Controllers
{
    public class AdminController : Controller
    {
        private ICommonService _ICommonService;
        private IOrderService _orderservice;
        private IGroundStockService _GroundStockService;
        private IPurchaseService _PurchaseService;
        private IProductService _Productservice;

        public AdminController(ICommonService ICommonService, IGroundStockService GroundStockService, IPurchaseService PurchaseService, IProductService Productservice, IOrderService orderservice)
        {
            _ICommonService = ICommonService;
            _GroundStockService = GroundStockService;
            _PurchaseService = PurchaseService;
            _Productservice = Productservice;
            _orderservice = orderservice;

        }

        // GET: /groundstock/Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddGroundStock()
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            ViewBag.Godown = _Productservice.GetAllGodownName();
            return View();
        }

        [HttpPost]
        public ActionResult AddGroundStock(AddGroundStock data)
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
                    GroundStock_Mst obj = new GroundStock_Mst();
                    obj.GroundStockID = data.GroundStockID;
                    obj.ProductID = data.ProductID;
                    obj.GroundStockQuantity = data.GroundStockQuantity;
                    obj.MinGroundStockQuantity = data.MinGroundStockQuantity;
                    obj.GroundStockDescription = data.GroundStockDescription;
                    obj.GodownID = data.GodownID;

                    if (obj.GroundStockID == 0)
                    {
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                    }
                    if (obj.GroundStockID != 0)
                    {
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                    }
                    obj.IsDelete = false;
                    long respose = _GroundStockService.ManageGroundStock(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GroundStockList()
        {
            List<GroundStockListResponse> objModel = _GroundStockService.GetAllGroundStockList();
            return PartialView(objModel);
        }

        public ActionResult Delete(long? GroundStockID, bool IsDelete)
        {
            try
            {
                _GroundStockService.DeleteGroundStock(GroundStockID.Value, IsDelete);
                return RedirectToAction("AddGroundStock");
            }
            catch (Exception)
            {
                return RedirectToAction("AddGroundStock");
            }
        }

        // 21-07-2022
        public JsonResult CheckProductIsExistGroundStock(long ProductID)
        {
            try
            {
                long response = _GroundStockService.GetExistProductGroundStock(ProductID);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }




        // Ground Stock Tansfer 

        public ActionResult AddGroundStockTransfer()
        {
            ViewBag.Product = _GroundStockService.GetProductNameForDDL();
            ViewBag.Godown = _Productservice.GetAllGodownName();
            return View();
        }

        [HttpPost]
        public ActionResult AddGroundStockTransfer(AddGroundStockTransfer data)
        {
            long respose = 0;
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    GroundStockTransfer_Mst obj = new GroundStockTransfer_Mst();
                    obj.GroundStockTransferID = data.GroundStockTransferID;
                    obj.ProductID = data.ProductID;
                    obj.StockTransferQuantity = data.StockTransferQuantity;
                    obj.MinStockTransferQuantity = data.MinStockTransferQuantity;
                    obj.StockTransferDescription = data.StockTransferDescription;
                    obj.GodownID = data.GodownID;
                    if (obj.GroundStockTransferID == 0)
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
                    respose = _GroundStockService.ManageGroundStockTransfer(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
                throw;
            }
        }


        public PartialViewResult GroundStockTransferList()
        {
            List<GroundStockTransferListResponse> objModel = _GroundStockService.GetAllGroundStockTransferList();
            return PartialView(objModel);
        }

        public ActionResult DeleteStockTransfer(long? GroundStockTransferID, bool IsDelete)
        {
            try
            {
                _GroundStockService.DeleteGroundStockTransfer(GroundStockTransferID.Value, IsDelete);
                return RedirectToAction("AddGroundStockTransfer");
            }
            catch (Exception)
            {
                return RedirectToAction("AddGroundStockTransfer");
            }
        }


        // 21-07-2022
        public JsonResult CheckProductIsExistGroundStockTransfer(long ProductID, long GodownID)
        {
            try
            {
                long response = _GroundStockService.GetExistProductGroundStockTransfer(ProductID, GodownID);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}