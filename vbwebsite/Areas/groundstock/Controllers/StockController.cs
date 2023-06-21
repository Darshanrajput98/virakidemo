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
    public class StockController : Controller
    {
        private ICommonService _ICommonService;
        private IGroundStockService _GroundStockService;
        private IPurchaseService _PurchaseService;
        private IProductService _Productservice;

        public StockController(ICommonService ICommonService, IGroundStockService GroundStockService, IPurchaseService PurchaseService, IProductService Productservice)
        {
            _ICommonService = ICommonService;
            _GroundStockService = GroundStockService;
            _PurchaseService = PurchaseService;
            _Productservice = Productservice;

        }

        // GET: /groundstock/Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inward()
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            List<GroundStockInwardListResponse> objModel = _GroundStockService.GetAllGroundStockInwardList();
            return View(objModel);
        }

        public ActionResult AddInwardBill()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AddInwardBill(AddGroundStockInward data)
        {
            decimal ClosingQty = 0;

            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    GroundStock_Inward_Mst obj = new GroundStock_Inward_Mst();
                    obj.PurchaseQtyID = data.PurchaseQtyID;
                    obj.PurchaseID = data.PurchaseID;
                    obj.ProductID = data.ProductID;
                    obj.BillDate = data.BillDate;
                    obj.OpeningQty = data.OpeningQty;
                    obj.PurchaseQty = data.NetWeight;
                    obj.ClosingQty = obj.OpeningQty + obj.PurchaseQty;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    ClosingQty = Convert.ToDecimal(obj.ClosingQty);

                    //if (obj.InwardID == 0)
                    //{
                    //    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    //    obj.CreatedOn = DateTime.Now;
                    //}
                    //else
                    //{
                    //    obj.CreatedBy = data.CreatedBy;
                    //    obj.CreatedOn = data.CreatedOn;
                    //}
                    //if (obj.InwardID != 0)
                    //{
                    //    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    //    obj.UpdatedOn = DateTime.Now;
                    //}

                    long response = _GroundStockService.AddInward(obj);

                    if (response > 0)
                    {
                        bool respose = _GroundStockService.UpdateIsInwardStatus(data.PurchaseQtyID, data.PurchaseID, data.ProductID);
                        bool respose1 = _GroundStockService.UpdateGroundStockQty(data.ProductID, ClosingQty);
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        //Ground Stock Transfer Inward

        public ActionResult StockTransferInward()
        {
            List<GroundStockTransferInwardResponse> objModel = _GroundStockService.GetAllGroundStockTransferInwardList();
            return View(objModel);
        }

        public ActionResult AddStockTransferInwardBill()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddStockTransferInwardBill(AddGroundStockTransferInward data)
        {
            decimal ClosingQty = 0;
            decimal DeductQty = 0;
            long GodownIDFrom = 0;
            decimal StockQuantityFrom = 0;

            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    GroundStockTransfer_Inward_Mst obj = new GroundStockTransfer_Inward_Mst();
                    obj.ChallanQtyID = data.ChallanQtyID;
                    obj.ChallanID = data.ChallanID;
                    obj.ProductID = data.ProductID;
                    GodownIDFrom = data.GodownIDFrom;
                    obj.GodownID = data.GodownIDTo;
                    StockQuantityFrom = data.StockQuantityFrom;

                    if (data.ChallanDate == Convert.ToDateTime("01/01/0001"))
                    {
                        obj.ChallanDate = null;
                    }
                    else
                    {
                        obj.ChallanDate = data.ChallanDate;
                    }
                    obj.LoadingQty = data.LoadingQty;
                    obj.OpeningQty = data.OpeningQty;

                    if (obj.LoadingQty == 0)
                    {
                        obj.PurchaseQty = data.Quantity;
                    }
                    else
                    {
                        obj.PurchaseQty = data.LoadingQty;
                    }

                    obj.ClosingQty = obj.OpeningQty + obj.PurchaseQty;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    ClosingQty = Convert.ToDecimal(obj.ClosingQty);

                    DeductQty = Convert.ToDecimal(StockQuantityFrom - obj.PurchaseQty);

                    long response = _GroundStockService.AddStockTransferInward(obj);

                    if (response > 0)
                    {
                        bool respose = _GroundStockService.UpdateStockTransferIsInwardStatus(data.ChallanQtyID, data.ChallanID, data.ProductID);
                        bool respose1 = _GroundStockService.UpdateGroundStockTransferQty(data.ProductID, data.GodownIDTo, ClosingQty, GodownIDFrom, DeductQty);
                    }

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectStockInwardPopup(long ChallanID)
        {
            List<GroundStockTransferInwardListResponse> objModel = _GroundStockService.GetGroundStockTransferInwardData(ChallanID);
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }




        //Outward 
        public ActionResult Outward()
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            List<GroundStockOutwardListResponse> objModel = _GroundStockService.GetAllGroundStockOutwardList();
            return View(objModel);
        }

        [HttpPost]
        public ActionResult AddOutwardBill(AddGroundStockOutward data)
        {
            decimal ClosingQty = 0;

            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    GroundStock_Outward_Mst obj = new GroundStock_Outward_Mst();
                    obj.ProductID = data.ProductID;
                    obj.OutwardDate = data.OutwardDate;
                    obj.OpeningQty = data.OpeningQty;
                    obj.PurchaseQty = data.NetWeight;
                    obj.ClosingQty = obj.OpeningQty - obj.PurchaseQty;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    ClosingQty = Convert.ToDecimal(obj.ClosingQty);

                    long response = _GroundStockService.AddOutward(obj);

                    if (response > 0)
                    {
                        bool respose1 = _GroundStockService.UpdateGroundStockOutwardQty(data.ProductID, ClosingQty);
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}