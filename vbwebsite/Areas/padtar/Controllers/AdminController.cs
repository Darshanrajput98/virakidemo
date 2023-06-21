using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.padtar.Controllers
{
    public class AdminController : Controller
    {
        private IPurchaseService _IPurchaseService;
        private IPadtarService _IPadtarService;

        public AdminController(IPurchaseService purchaseservice, IPadtarService padtarservice)
        {
            _IPurchaseService = purchaseservice;
            _IPadtarService = padtarservice;
        }

        // GET: /padtar/Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddWholeMaterial()
        {
            ViewBag.Product = _IPurchaseService.GetAllProductName();
            return View();
        }

        [HttpPost]
        public ActionResult AddWholeMaterial(AddWholeMaterialRequest data)
        {
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                WholeMaterial_Mst obj = new WholeMaterial_Mst();
                obj.MaterialID = data.MaterialID;
                obj.ProductID = data.ProductID;
                obj.CategoryID = data.CategoryID;
                obj.Category = data.Category;
                obj.GST = data.GST;
                obj.CurrentRate = data.CurrentRate;
                obj.Notes1 = data.Notes1;
                obj.Notes2 = data.Notes2;
                obj.APMC = data.APMC;
                obj.APMCAmount = data.APMCAmount;
                obj.MarketFinal = data.MarketFinal;
                obj.GrossRate = data.GrossRate;
                obj.KG_P_Hour = data.KG_P_Hour;
                obj.Labour_P_Hour = data.Labour_P_Hour;
                obj.Gasara = data.Gasara;
                obj.GasaraAmount = data.GasaraAmount;
                obj.SellRateWholesale = data.SellRateWholesale;
                obj.SellRateRetail = data.SellRateRetail;
                obj.MarginWholesale = data.MarginWholesale;
                obj.MarginRetail = data.MarginRetail;
                obj.Discount = data.Discount;
                obj.DiscountAmount = data.DiscountAmount;
                obj.NetAmount = data.NetAmount;
                obj.PackingCharge = data.PackingCharge;
                obj.Freight_P_KG = data.Freight_P_KG;
                obj.Commision_P_KG = data.Commision_P_KG;
                obj.LabourAmount_P_KG = data.LabourAmount_P_KG;
                obj.Padtar = data.Padtar;

                if (obj.MaterialID == 0)
                {
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                }
                else
                {
                    obj.CreatedBy = data.CreatedBy;
                    obj.CreatedOn = data.CreatedOn;
                }
                if (obj.MaterialID != 0)
                {
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                }
                obj.IsDelete = false;
                long respose = _IPadtarService.ManageWholeMaterial(obj);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult WholeMaterialList()
        {
            List<WholeMaterialListResponse> objModel = _IPadtarService.GetAllWholeMaterialList();
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult GetGST(long ProductID)
        {
            var lstGST = _IPadtarService.GetAutoCompleteProductDetailsForWholeMaterial(ProductID);
            return Json(lstGST, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateWholeMaterial(List<WholeMaterialListResponse> data)
        {
            bool respose = _IPadtarService.UpdateMaterial(data);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteWholeMaterial(long? MaterialID, bool IsDelete)
        {
            try
            {
                _IPadtarService.DeleteWholeMaterial(MaterialID.Value, IsDelete);
                return RedirectToAction("AddWholeMaterial");
            }
            catch (Exception)
            {
                return RedirectToAction("AddWholeMaterial");
            }
        }


        //Powder Spices
        public ActionResult AddPowderSpices()
        {
            ViewBag.Product = _IPurchaseService.GetAllProductName();
            return View();
        }

        [HttpPost]
        public ActionResult AddPowderSpices(PowderSpicesRequest data)
        {
            if (Request.Cookies["UserID"] == null)
            {
                Request.Cookies["UserID"].Value = null;
                return JavaScript("location.reload(true)");
            }
            else
            {
                PowderSpices_Mst obj = new PowderSpices_Mst();
                obj.SpicesID = data.SpicesID;
                obj.ProductID = data.ProductID;
                obj.CategoryID = data.CategoryID;
                obj.GST = data.GST;
                obj.CurrentRate = data.CurrentRate;
                obj.Notes1 = data.Notes1;
                obj.Notes2 = data.Notes2;
                obj.GrindingCharge = data.GrindingCharge;
                obj.Gasara = data.Gasara;
                obj.GasaraAmount = data.GasaraAmount;
                obj.GrossRate = data.GrossRate;
                obj.Padtar = data.Padtar;
                obj.SellRateWholesale = data.SellRateWholesale;
                obj.SellRateRetail = data.SellRateRetail;
                obj.MarginWholesale = data.MarginWholesale;
                obj.MarginRetail = data.MarginRetail;


                if (obj.SpicesID == 0)
                {
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                }
                else
                {
                    obj.CreatedBy = data.CreatedBy;
                    obj.CreatedOn = data.CreatedOn;
                }
                if (obj.SpicesID != 0)
                {
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                }
                obj.IsDelete = false;
                long respose = _IPadtarService.ManagePowderSpices(obj);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDetailsForPowderSpices(long ProductID)
        {
            var lstGST = _IPadtarService.GetAutoCompleteDetailsForPowderSpices(ProductID);
            return Json(lstGST, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PowderSpicesList()
        {
            List<PowderSpicesListResponse> objModel = _IPadtarService.GetAllPowderSpicesList();
            return PartialView(objModel);
        }

        [HttpPost]
        public ActionResult UpdatePowderSpices(List<PowderSpicesListResponse> data)
        {
            bool respose = _IPadtarService.UpdateSpices(data);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePowderSpices(long? SpicesID, bool IsDelete)
        {
            try
            {
                _IPadtarService.DeletePowderSpices(SpicesID.Value, IsDelete);
                return RedirectToAction("AddPowderSpices");
            }
            catch (Exception)
            {
                return RedirectToAction("AddPowderSpices");
            }
        }


        //Padtar Premix
        public ActionResult AddPremix(Int64? PremixID)
        {
            ViewBag.Product = _IPurchaseService.GetAllProductName();
            return View();
        }

        [HttpPost]
        public ActionResult AddPremix(PremixRequest data)
        {
            string respose = "";
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    if (data.PremixID == 0)
                    {
                        data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        data.CreatedBy = data.CreatedBy;
                        data.CreatedOn = data.CreatedOn;
                    }
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    respose = _IPadtarService.AddPremix(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PremixList()
        {
            List<PremixListResponse> objModel = _IPadtarService.GetAllPremixList();
            return PartialView(objModel);
        }

        public ActionResult GetPremixQtyList(long PremixID)
        {
            List<PremixItemRequest> lstPremix = _IPadtarService.GetPremixDetailsByPremixID(PremixID);
            return Json(lstPremix, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePremix(long? PremixID, bool IsDelete)
        {
            try
            {
                _IPadtarService.DeletePremix(PremixID.Value, IsDelete);
                return RedirectToAction("AddPremix");
            }
            catch (Exception)
            {

                return RedirectToAction("AddPremix");
            }
        }

        [HttpPost]
        public ActionResult UpdatePremix(List<PremixListResponse> data)
        {
            bool respose = _IPadtarService.UpdatePremix(data);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

    }
}