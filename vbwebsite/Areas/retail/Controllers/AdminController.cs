using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.retail.Controllers
{
    public class AdminController : Controller
    {

        private IRetAdminService _areaservice;
        private IRetProductService _productservice;

        public AdminController(IRetAdminService areaservice, IRetProductService productservice)
        {
            _areaservice = areaservice;
            _productservice = productservice;
        }

        public ActionResult ManageArea()
        {        
            return View();
        }

        [HttpPost]
        public ActionResult ManageArea(RetAreaViewModel data)
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
                    RetAreaMst obj = new RetAreaMst();
                    obj.AreaID = data.AreaID;
                    obj.AreaName = data.AreaName;
                    obj.City = data.City;
                    obj.Country = data.Country;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.DaysofWeek = data.DaysofWeek;
                    obj.IsDelete = false;
                    obj.PinCode = data.PinCode;
                    obj.State = data.State;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _areaservice.AddArea(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageAreaList()
        {
            List<RetAreaListResponse> objModel = _areaservice.GetAllAreaList();
            return PartialView(objModel);
        }

        public ActionResult DeleteArea(long? AreaID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteArea(AreaID.Value, IsDelete);
                return RedirectToAction("ManageArea");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageArea");
            }
        }

        public ActionResult ManageGodown()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult ManageGodown(RetGodownViewModel data)
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
                    RetGodownMst obj = new RetGodownMst();
                    obj.GodownID = data.GodownID;
                    obj.GodownName = data.GodownName;
                    obj.GodownPhone = data.GodownPhone;
                    obj.GodownAddress1 = data.GodownAddress1;
                    obj.GodownAddress2 = data.GodownAddress2;
                    obj.GodownFSSAINumber = data.GodownFSSAINumber;
                    obj.GodownCode = data.GodownCode;
                    obj.GodownNote = data.GodownNote;
                    obj.Place = data.Place;
                    obj.Pincode = data.Pincode;
                    obj.State = data.State;
                    obj.CashOption = data.CashOption;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    bool respose = _areaservice.AddGodown(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageGodownList()
        {
            List<RetGodownListResponse> objModel = _areaservice.GetAllGodownList();
            return PartialView(objModel);
        }

        public ActionResult DeleteGodown(long? GodownID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteGodown(GodownID.Value, IsDelete);
                return RedirectToAction("ManageGodown");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageGodown");
            }
        }

        public ActionResult ManageTax()
        {         
            return View();
        }

        [HttpPost]
        public ActionResult ManageTax(RetTaxViewModel data)
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
                    RetTaxMst obj = new RetTaxMst();
                    obj.TaxID = data.TaxID;
                    obj.TaxCode = data.TaxCode;
                    obj.TaxName = data.TaxName;
                    obj.TaxDescription = data.TaxDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    bool respose = _areaservice.AddTax(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageTaxList()
        {
            List<RetTaxListResponse> objModel = _areaservice.GetAllTaxList();
            return PartialView(objModel);
        }

        public ActionResult DeleteTax(long? TaxID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteTax(TaxID.Value, IsDelete);
                return RedirectToAction("ManageTax");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageTax");
            }
        }

        public ActionResult ManageUnit()
        {
            ViewBag.GuiLanguage = _productservice.GetAllLanguage(); 
            return View();
        }

        [HttpPost]
        public ActionResult ManageUnit(RetUnitViewModel data)
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
                    RetUnitMst obj = new RetUnitMst();
                    obj.UnitID = data.UnitID;
                    obj.GuiID = data.GuiID;
                    obj.UnitCode = data.UnitCode;
                    obj.UnitName = data.UnitName;
                    obj.UnitDescription = data.UnitDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _areaservice.AddUnit(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageUnitList()
        {
            List<RetUnitListResponse> objModel = _areaservice.GetAllUnitList();
            return PartialView(objModel);
        }

        public ActionResult DeleteUnit(long? UnitID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteUnit(UnitID.Value, IsDelete);
                return RedirectToAction("ManageUnit");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageUnit");
            }
        }

        public ActionResult ManageTransport()
        {
            //Session["UserID"] = "1";
            return View();
        }

        [HttpPost]
        public ActionResult ManageTransport(RetTransportViewModel data)
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
                    RetTransportMst obj = new RetTransportMst();
                    obj.TransportID = data.TransportID;
                    obj.TransID = data.TransID;
                    obj.TransportName = data.TransportName;
                    obj.TransportGSTNumber = data.TransportGSTNumber;
                    obj.ContactNumber = data.ContactNumber;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    bool respose = _areaservice.AddTransport(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageTransportList()
        {
            List<RetTransportListResponse> objModel = _areaservice.GetAllTransportList();
            return PartialView(objModel);
        }

        public ActionResult DeleteTransport(long? TransportID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteTransport(TransportID.Value, IsDelete);
                return RedirectToAction("ManageTransport");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageTransport");
            }
        }

    }
}