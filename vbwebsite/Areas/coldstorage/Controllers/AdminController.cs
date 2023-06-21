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


namespace vbwebsite.Areas.coldstorage.Controllers
{
    public class AdminController : Controller
    {
        private ICommonService _ICommonService;
        private IColdStorageService _IColdStorageService;

        public AdminController(ICommonService ICommonService, IColdStorageService ColdStorageService)
        {
            _ICommonService = ICommonService;
            _IColdStorageService = ColdStorageService;
        }

        // GET: /coldstorage/Home/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddColdStorageDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddColdStorageDetails(AddColdStorage data)
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
                    ColdStorage_Mst obj = new ColdStorage_Mst();
                    obj.ColdStorageID = data.ColdStorageID;
                    obj.Name = data.Name;
                    obj.ShortName = data.ShortName;
                    obj.Address = data.Address;
                    obj.Email = data.Email;
                    obj.GSTNo = data.GSTNo;
                    obj.PANNo = data.PANNo;
                    obj.FssaiLicenseNo = data.FssaiLicenseNo;
                    obj.ExpiryDate = data.ExpiryDate;
                    obj.ContactPersonName = data.ContactPersonName;
                    obj.ContactPersonName1 = data.ContactPersonName1;
                    obj.ContactPersonName2 = data.ContactPersonName2;
                    obj.ContactNumber = data.ContactNumber;
                    obj.ContactNumber1 = data.ContactNumber1;
                    obj.ContactNumber2 = data.ContactNumber2;

                    if (obj.ColdStorageID == 0)
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
                    long respose = _IColdStorageService.AddColdStorage(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public PartialViewResult ColdStorageList()
        {
            List<ColdStorageListResponse> objModel = _IColdStorageService.GetAllColdStorageList();
            return PartialView(objModel);
        }


        public ActionResult Delete(long? ColdStorageID, bool IsDelete)
        {
            try
            {
                _IColdStorageService.DeleteColdStorage(ColdStorageID.Value, IsDelete);
                return RedirectToAction("AddColdStorageDetails");
            }
            catch (Exception)
            {
                return RedirectToAction("AddColdStorageDetails");
            }
        }

    }
}