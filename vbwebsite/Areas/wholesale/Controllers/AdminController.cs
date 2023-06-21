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
using System.Web.Security;
using vb.Data;
using vb.Data.Model;
using vb.Service;
using WebMatrix.WebData;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class AdminController : Controller
    {

        private IAdminService _areaservice;
        private IProductService _productservice;
        private ICommonService _ICommonService;

        public AdminController(IAdminService areaservice, IProductService productservice, ICommonService commonservice)
        {
            _areaservice = areaservice;
            _productservice = productservice;
            _ICommonService = commonservice;
        }

        // GET: /wholesale/Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageArea()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public ActionResult ManageArea(AreaViewModel data)
        //{
        //    try
        //    {
        //        if (Request.Cookies["UserID"] == null)
        //        {
        //            Request.Cookies["UserID"].Value = null;
        //            return JavaScript("location.reload(true)");
        //        }
        //        else
        //        {
        //            Area_Mst obj = new Area_Mst();
        //            obj.AreaID = data.AreaID;
        //            obj.AreaName = data.AreaName;
        //            obj.City = data.City;
        //            obj.Country = data.Country;
        //            obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //            obj.CreatedOn = DateTime.Now;
        //            obj.DaysofWeek = data.DaysofWeek;
        //            obj.IsDelete = false;
        //            obj.IsOnline = data.IsOnline;
        //            obj.PinCode = data.PinCode;
        //            obj.State = data.State;
        //            obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //            obj.UpdatedOn = DateTime.Now;
        //            bool respose = _areaservice.AddArea(obj);
        //            return Json(respose, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //21 June,2021 Sonal Gandhi
        [HttpPost]
        public ActionResult ManageArea(AreaViewModel data)
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
                    data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedBy = data.CreatedBy;
                    bool respose = _areaservice.AddArea(data);
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
            List<AreaListResponse> objModel = _areaservice.GetAllAreaList();
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

        public ActionResult ManageEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageEvent(EventViewModel data)
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
                    Event_Mst obj = new Event_Mst();
                    obj.EventID = data.EventID;
                    obj.EventName = data.EventName;
                    obj.EventDescription = data.EventDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _areaservice.AddEvent(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageEventList()
        {
            List<EventListResponse> objModel = _areaservice.GetAllEventList();
            return PartialView(objModel);
        }

        public ActionResult DeleteEvent(long? EventID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteEvent(EventID.Value, IsDelete);
                return RedirectToAction("ManageEvent");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageEvent");
            }
        }

        public ActionResult ManageEventDate()
        {
            ViewBag.Event = _areaservice.GetAllEventName();
            return View();
        }

        [HttpPost]
        public ActionResult ManageEventDate(EventDateViewModel data)
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
                    EventDate_Mst obj = new EventDate_Mst();
                    obj.EventDateID = data.EventDateID;
                    obj.EventID = data.EventID;
                    obj.EventDate = data.EventDate;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _areaservice.AddEventDate(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageEventDateList()
        {
            List<EventDateListResponse> objModel = _areaservice.GetAllEventDateList();
            return PartialView(objModel);
        }

        public ActionResult DeleteEventDate(long? EventDateID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteEventDate(EventDateID.Value, IsDelete);
                return RedirectToAction("ManageEventDate");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageEventDate");
            }
        }

        public ActionResult ManageGodown()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageGodown(GodownViewModel data)
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
                    Godown_Mst obj = new Godown_Mst();
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
                    obj.OpeningAmount = data.OpeningAmount;
                    obj.ChillarAmount = data.ChillarAmount;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.GSTNumber = data.GSTNumber;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
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
            List<GodownListResponse> objModel = _areaservice.GetAllGodownList();
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
        public ActionResult ManageTax(TaxViewModel data)
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
                    Tax_Mst obj = new Tax_Mst();
                    obj.TaxID = data.TaxID;
                    obj.TaxCode = data.TaxCode;
                    obj.TaxName = data.TaxName;
                    obj.TaxDescription = data.TaxDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
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
            List<TaxListResponse> objModel = _areaservice.GetAllTaxList();
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
            return View();
        }

        [HttpPost]
        public ActionResult ManageUnit(UnitViewModel data)
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
                    Unit_Mst obj = new Unit_Mst();
                    obj.UnitID = data.UnitID;
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

            List<UnitListResponse> objModel = _areaservice.GetAllUnitList();
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

        public ActionResult ManageRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageRole(RoleViewModel data)
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
                    //Role_Mst obj = new Role_Mst();
                    webpages_Roles obj = new webpages_Roles();
                    obj.RoleId = data.RoleID;
                    obj.RoleName = data.RoleName;

                    if (obj.RoleId == 0)
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
                    obj.IsDelete = data.IsDelete;
                    int RoleID = _areaservice.AddRole(obj);

                    // 08 Aug 2020 Piyush Limbani
                    List<MenuList> menulst = _areaservice.GetAllMenuList();
                    for (int i = 0; i < menulst.Count; i++)
                    {
                        long AuthorizeID = _areaservice.CheckMenuForRoleIsExist(RoleID, menulst[i].MenuID);
                        if (AuthorizeID == 0)
                        {
                            AuthorizeMaster objmenu = new AuthorizeMaster();
                            objmenu.RoleID = RoleID;
                            objmenu.MenuID = menulst[i].MenuID;
                            objmenu.CreatedDate = DateTime.Now;
                            objmenu.IsActive = false;
                            bool response = (_areaservice.AddAuthority(objmenu));
                        }
                        else
                        {

                        }
                    }
                    // 08 Aug 2020 Piyush Limbani
                    return Json(RoleID, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageRoleList()
        {
            List<RoleListResponse> objModel = _areaservice.GetAllRoleList();
            return PartialView(objModel);
        }

        public ActionResult DeleteRole(long? RoleID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteRole(RoleID.Value, IsDelete);
                return RedirectToAction("ManageRole");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageRole");
            }
        }

        public ActionResult ManageUsers()
        {
            ViewBag.Godown = _productservice.GetAllGodownName();
            ViewBag.Role = _areaservice.GetAllRoleName();
            ViewBag.Area = _areaservice.GetAllAreaList();
            return View();
        }

        //[HttpPost]
        //public ActionResult ManageUsers(Register data)
        //{
        //    string oldimage = "";
        //    string oldimage2 = "";
        //    if (data.UserID == 0)
        //    {
        //        if (Request.Files.Count > 0)
        //        {
        //            // New Code
        //            foreach (string fileName in Request.Files)
        //            {
        //                if (fileName == "FSSAIDoctorCertificate")
        //                {
        //                    string FSSAIDoctorCertificate;
        //                    HttpPostedFileBase FSSAIDoctorCertificatefile = Request.Files[fileName];
        //                    string fileName2 = data.EmployeeCode + "-" + "FSSAIDoctorCerti" + Path.GetExtension(FSSAIDoctorCertificatefile.FileName).ToString();
        //                    data.FSSAIDoctorCertificate = fileName2;
        //                    FSSAIDoctorCertificate = Path.Combine(Server.MapPath("~/Document/"), data.FSSAIDoctorCertificate);
        //                    FSSAIDoctorCertificatefile.SaveAs(FSSAIDoctorCertificate);
        //                }
        //                else if (fileName == "ProfilePicture")
        //                {

        //                    string ProfilePicture;
        //                    HttpPostedFileBase ProfilePicturefile = Request.Files[fileName];
        //                    string fileName1 = data.EmployeeCode + "-" + data.FullName + Path.GetExtension(ProfilePicturefile.FileName).ToString();
        //                    data.ProfilePicture = fileName1;
        //                    ProfilePicture = Path.Combine(Server.MapPath("~/ProfilePicture/"), data.ProfilePicture);
        //                    ProfilePicturefile.SaveAs(ProfilePicture);
        //                }
        //            }
        //            // New Code
        //            //HttpFileCollectionBase files = Request.Files;
        //            //if (files != null)
        //            //{
        //            //    HttpPostedFileBase file = files[0];
        //            //    string fname;
        //            //    string fileName1 = data.EmployeeCode + "-" + data.FullName + Path.GetExtension(file.FileName).ToString();
        //            //    data.ProfilePicture = fileName1;
        //            //    fname = Path.Combine(Server.MapPath("~/ProfilePicture/"), data.ProfilePicture);
        //            //    file.SaveAs(fname);
        //            //}
        //            //else
        //            //{
        //            //    oldimage = _areaservice.GetUserOldImage(data.UserID);
        //            //    data.ProfilePicture = oldimage;
        //            //}
        //        }
        //        if (!WebSecurity.UserExists(data.UserName))
        //        {
        //            WebSecurity.CreateUserAndAccount(data.UserName, data.Password,
        //                new
        //                {
        //                    //Id = data.UserID,
        //                    EmployeeCode = data.EmployeeCode,
        //                    FullName = data.FullName,
        //                    UserName = Convert.ToString(data.EmployeeCode),
        //                    Password = data.Password,
        //                    RoleID = data.RoleID,
        //                    BirthDate = data.BirthDate,
        //                    Age = data.Age,
        //                    Gender = data.Gender,
        //                    DateOfJoining = data.DateOfJoining,
        //                    Email = data.Email,
        //                    Address = data.Address,
        //                    MobileNumber = data.MobileNumber,
        //                    BankName = data.BankName,
        //                    AccountNumber = data.AccountNumber,
        //                    Branch = data.Branch,
        //                    IFSCCode = data.IFSCCode,
        //                    PrimaryArea = data.PrimaryArea,
        //                    PrimaryAddress = data.PrimaryAddress,
        //                    PrimaryPin = data.PrimaryPin,
        //                    SecondaryArea = data.SecondaryArea,
        //                    SecondaryAddress = data.SecondaryAddress,
        //                    SecondaryPin = data.SecondaryPin,
        //                    PanNo = data.PanNo,
        //                    PassportNo = data.PassportNo,
        //                    PassportValiddate = data.PassportValiddate,
        //                    UIDAI = data.UIDAI,
        //                    UAN = data.UAN,
        //                    PF = data.PF,
        //                    ESIC = data.ESIC,
        //                    Drivinglicence = data.Drivinglicence,
        //                    DrivingValidup = data.DrivingValidup,
        //                    // GodownID = data.GodownID,
        //                    ReferenceName = data.ReferenceName,
        //                    FName = data.FName,
        //                    Fdob = data.Fdob,
        //                    FUIDAI = data.FUIDAI,
        //                    FRelation = data.FRelation,
        //                    Flivingtogether = data.Flivingtogether,
        //                    MName = data.MName,
        //                    Mdob = data.Mdob,
        //                    MUIDAI = data.MUIDAI,
        //                    MRelation = data.MRelation,
        //                    Mlivingtogether = data.Mlivingtogether,
        //                    WName = data.WName,
        //                    Wdob = data.Wdob,
        //                    WUIDAI = data.WUIDAI,
        //                    WRelation = data.WRelation,
        //                    Wlivingtogether = data.Wlivingtogether,
        //                    C1Name = data.C1Name,
        //                    C1dob = data.C1dob,
        //                    C1UIDAI = data.C1UIDAI,
        //                    C1Relation = data.C1Relation,
        //                    C1livingtogether = data.C1livingtogether,
        //                    C2Name = data.C2Name,
        //                    C2dob = data.C2dob,
        //                    C2UIDAI = data.C2UIDAI,
        //                    C2Relation = data.C2Relation,
        //                    C2livingtogether = data.C2livingtogether,
        //                    C3Name = data.C3Name,
        //                    C3dob = data.C3dob,
        //                    C3UIDAI = data.C3UIDAI,
        //                    C3Relation = data.C3Relation,
        //                    C3livingtogether = data.C3livingtogether,
        //                    GodownID = data.GodownID,
        //                    ServiceTime = data.ServiceTime,
        //                    Maritalstatus = data.Maritalstatus,
        //                    ProfilePicture = data.ProfilePicture,
        //                    FSSAIDoctorCertificate = data.FSSAIDoctorCertificate,
        //                    FSSAIDoctorCertificateValidity = data.FSSAIDoctorCertificateValidity,
        //                    CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value),
        //                    CreatedOn = DateTime.Now,
        //                    UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value),
        //                    UpdatedOn = DateTime.Now,
        //                    IsDelete = false
        //                });
        //            if (Roles.IsUserInRole(data.UserName, data.RoleName))
        //            {
        //                ViewBag.ResultMessage = "This user already has the role specified !";
        //            }
        //            else
        //            {
        //                Roles.AddUserToRole(data.UserName, data.RoleName);
        //                ViewBag.ResultMessage = "Username added to the role successfully !";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //Update code
        //        #region Update
        //        User obj = new User();
        //        obj.Id = data.UserID;
        //        obj.FullName = data.FullName;
        //        obj.EmployeeCode = data.EmployeeCode;
        //        obj.UserName = data.UserNameUpdate;
        //        obj.Password = data.Password;
        //        obj.BirthDate = data.BirthDate;
        //        obj.Age = data.Age;
        //        obj.Gender = data.Gender;
        //        obj.DateOfJoining = data.DateOfJoining;
        //        obj.Email = data.Email;
        //        obj.RoleID = data.RoleID;
        //        obj.MobileNumber = data.MobileNumber;
        //        obj.BankName = data.BankName;
        //        obj.Branch = data.Branch;
        //        obj.AccountNumber = data.AccountNumber;
        //        obj.IFSCCode = data.IFSCCode;
        //        obj.PrimaryArea = data.PrimaryArea;
        //        obj.PrimaryAddress = data.PrimaryAddress;
        //        obj.PrimaryPin = data.PrimaryPin;
        //        obj.SecondaryArea = data.SecondaryArea;
        //        obj.SecondaryAddress = data.SecondaryAddress;
        //        obj.SecondaryPin = data.SecondaryPin;
        //        obj.PanNo = data.PanNo;
        //        obj.PassportNo = data.PassportNo;
        //        obj.PassportValiddate = data.PassportValiddate;
        //        obj.UIDAI = data.UIDAI;
        //        obj.UAN = data.UAN;
        //        obj.PF = data.PF;
        //        obj.ESIC = data.ESIC;
        //        obj.Drivinglicence = data.Drivinglicence;
        //        obj.DrivingValidup = data.DrivingValidup;
        //        obj.GodownID = data.GodownID;
        //        obj.ReferenceName = data.ReferenceName;
        //        obj.FName = data.FName;
        //        obj.Fdob = data.Fdob;
        //        obj.FUIDAI = data.FUIDAI;
        //        obj.FRelation = data.FRelation;
        //        obj.Flivingtogether = data.Flivingtogether;
        //        obj.MName = data.MName;
        //        obj.Mdob = data.Mdob;
        //        obj.MUIDAI = data.MUIDAI;
        //        obj.MRelation = data.MRelation;
        //        obj.Mlivingtogether = data.Mlivingtogether;
        //        obj.WName = data.WName;
        //        obj.Wdob = data.Wdob;
        //        obj.WUIDAI = data.WUIDAI;
        //        obj.WRelation = data.WRelation;
        //        obj.Wlivingtogether = data.Wlivingtogether;
        //        obj.C1Name = data.C1Name;
        //        obj.C1dob = data.C1dob;
        //        obj.C1UIDAI = data.C1UIDAI;
        //        obj.C1Relation = data.C1Relation;
        //        obj.C1livingtogether = data.C1livingtogether;
        //        obj.C2Name = data.C2Name;
        //        obj.C2dob = data.C2dob;
        //        obj.C2UIDAI = data.C2UIDAI;
        //        obj.C2Relation = data.C2Relation;
        //        obj.C2livingtogether = data.C2livingtogether;
        //        obj.C3Name = data.C3Name;
        //        obj.C3dob = data.C3dob;
        //        obj.C3UIDAI = data.C3UIDAI;
        //        obj.C3Relation = data.C3Relation;
        //        obj.C3livingtogether = data.C3livingtogether;
        //        obj.Maritalstatus = data.Maritalstatus;
        //        obj.FSSAIDoctorCertificateValidity = data.FSSAIDoctorCertificateValidity;
        //        string strFSSAIDoctorCertificate = "";
        //        string strProfilePicture = "";
        //        if (Request.Files.Count != 0)
        //        {
        //            foreach (string fileName in Request.Files)
        //            {
        //                if (fileName == "FSSAIDoctorCertificate")
        //                {
        //                    string FSSAIDoctorCertificate;
        //                    HttpPostedFileBase FSSAIDoctorCertificatefile = Request.Files[fileName];
        //                    string fileName2 = data.EmployeeCode + "-" + "FSSAIDoctorCerti" + Path.GetExtension(FSSAIDoctorCertificatefile.FileName).ToString();
        //                    obj.FSSAIDoctorCertificate = fileName2;
        //                    FSSAIDoctorCertificate = Path.Combine(Server.MapPath("~/Document/"), obj.FSSAIDoctorCertificate);
        //                    FSSAIDoctorCertificatefile.SaveAs(FSSAIDoctorCertificate);
        //                    strFSSAIDoctorCertificate = obj.FSSAIDoctorCertificate;
        //                }
        //                else if (fileName == "ProfilePicture")
        //                {
        //                    string ProfilePicture;
        //                    HttpPostedFileBase ProfilePicturefile = Request.Files[fileName];
        //                    string fileName1 = data.EmployeeCode + "-" + data.FullName + Path.GetExtension(ProfilePicturefile.FileName).ToString();
        //                    obj.ProfilePicture = fileName1;
        //                    ProfilePicture = Path.Combine(Server.MapPath("~/ProfilePicture/"), obj.ProfilePicture);
        //                    ProfilePicturefile.SaveAs(ProfilePicture);
        //                    strProfilePicture = obj.ProfilePicture;
        //                }
        //            }
        //        }
        //        if (strFSSAIDoctorCertificate == "")
        //        {
        //            oldimage2 = _areaservice.GetUserOldFSSAIDoctorCertificate(data.UserID);
        //            obj.FSSAIDoctorCertificate = oldimage2;
        //        }
        //        if (strProfilePicture == "")
        //        {
        //            oldimage = _areaservice.GetUserOldImage(data.UserID);
        //            obj.ProfilePicture = oldimage;
        //        }
        //        //HttpFileCollectionBase files = Request.Files;
        //        //if (files.Count != 0)
        //        //{
        //        //    HttpPostedFileBase file = files[0];
        //        //    string fname;
        //        //    string fileName1 = data.EmployeeCode + "-" + data.FullName + Path.GetExtension(file.FileName).ToString();
        //        //    obj.ProfilePicture = fileName1;
        //        //    fname = Path.Combine(Server.MapPath("~/ProfilePicture/"), obj.ProfilePicture);
        //        //    file.SaveAs(fname);
        //        //}
        //        //else
        //        //{
        //        //    oldimage = _areaservice.GetUserOldImage(data.UserID);
        //        //    obj.ProfilePicture = oldimage;
        //        //}
        //        obj.CreatedBy = data.CreatedBy;
        //        obj.CreatedOn = data.CreatedOn;
        //        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //        obj.UpdatedOn = DateTime.Now;
        //        obj.IsDelete = data.IsDelete;
        //        bool result = _areaservice.UpdateUser(obj);
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //        #endregion
        //    }
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}



        //public JsonResult CheckEmployeeCodeIsExists(string UserName)
        //{
        //    try
        //    {
        //        bool result = false;
        //        if (!WebSecurity.UserExists(UserName))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            result = false;
        //        }
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult ManageUsers(Register data)
        {
            string oldimage = "";
            string oldimage2 = "";
            bool result = false;
            if (data.UserID == 0)
            {
                if (Request.Files.Count > 0)
                {
                    // New Code
                    foreach (string fileName in Request.Files)
                    {
                        if (fileName == "FSSAIDoctorCertificate")
                        {
                            string FSSAIDoctorCertificate;
                            HttpPostedFileBase FSSAIDoctorCertificatefile = Request.Files[fileName];
                            string fileName2 = data.EmployeeCode + "-" + "FSSAIDoctorCerti" + Path.GetExtension(FSSAIDoctorCertificatefile.FileName).ToString();
                            data.FSSAIDoctorCertificate = fileName2;
                            FSSAIDoctorCertificate = Path.Combine(Server.MapPath("~/Document/"), data.FSSAIDoctorCertificate);
                            FSSAIDoctorCertificatefile.SaveAs(FSSAIDoctorCertificate);
                        }
                        else if (fileName == "ProfilePicture")
                        {

                            string ProfilePicture;
                            HttpPostedFileBase ProfilePicturefile = Request.Files[fileName];
                            string fileName1 = data.EmployeeCode + "-" + data.FullName + Path.GetExtension(ProfilePicturefile.FileName).ToString();
                            data.ProfilePicture = fileName1;
                            ProfilePicture = Path.Combine(Server.MapPath("~/ProfilePicture/"), data.ProfilePicture);
                            ProfilePicturefile.SaveAs(ProfilePicture);
                        }
                    }
                }
                if (!WebSecurity.UserExists(data.UserName))
                {
                    WebSecurity.CreateUserAndAccount(data.UserName, data.Password,
                        new
                        {
                            EmployeeCode = data.EmployeeCode,
                            FullName = data.FullName,
                            UserName = Convert.ToString(data.EmployeeCode),
                            Password = data.Password,
                            RoleID = data.RoleID,
                            BirthDate = data.BirthDate,
                            Age = data.Age,
                            Gender = data.Gender,
                            DateOfJoining = data.DateOfJoining,
                            Email = data.Email,
                            Address = data.Address,
                            MobileNumber = data.MobileNumber,
                            BankName = data.BankName,
                            AccountNumber = data.AccountNumber,
                            Branch = data.Branch,
                            IFSCCode = data.IFSCCode,
                            PrimaryArea = data.PrimaryArea,
                            PrimaryAddress = data.PrimaryAddress,
                            PrimaryPin = data.PrimaryPin,
                            SecondaryArea = data.SecondaryArea,
                            SecondaryAddress = data.SecondaryAddress,
                            SecondaryPin = data.SecondaryPin,
                            PanNo = data.PanNo,
                            PassportNo = data.PassportNo,
                            PassportValiddate = data.PassportValiddate,
                            UIDAI = data.UIDAI,
                            UAN = data.UAN,
                            PF = data.PF,
                            ESIC = data.ESIC,
                            Drivinglicence = data.Drivinglicence,
                            DrivingValidup = data.DrivingValidup,
                            // GodownID = data.GodownID,
                            ReferenceName = data.ReferenceName,
                            FName = data.FName,
                            Fdob = data.Fdob,
                            FUIDAI = data.FUIDAI,
                            FRelation = data.FRelation,
                            Flivingtogether = data.Flivingtogether,
                            MName = data.MName,
                            Mdob = data.Mdob,
                            MUIDAI = data.MUIDAI,
                            MRelation = data.MRelation,
                            Mlivingtogether = data.Mlivingtogether,
                            WName = data.WName,
                            Wdob = data.Wdob,
                            WUIDAI = data.WUIDAI,
                            WRelation = data.WRelation,
                            Wlivingtogether = data.Wlivingtogether,
                            C1Name = data.C1Name,
                            C1dob = data.C1dob,
                            C1UIDAI = data.C1UIDAI,
                            C1Relation = data.C1Relation,
                            C1livingtogether = data.C1livingtogether,
                            C2Name = data.C2Name,
                            C2dob = data.C2dob,
                            C2UIDAI = data.C2UIDAI,
                            C2Relation = data.C2Relation,
                            C2livingtogether = data.C2livingtogether,
                            C3Name = data.C3Name,
                            C3dob = data.C3dob,
                            C3UIDAI = data.C3UIDAI,
                            C3Relation = data.C3Relation,
                            C3livingtogether = data.C3livingtogether,
                            GodownID = data.GodownID,
                            ServiceTime = data.ServiceTime,
                            Maritalstatus = data.Maritalstatus,
                            ProfilePicture = data.ProfilePicture,
                            FSSAIDoctorCertificate = data.FSSAIDoctorCertificate,
                            FSSAIDoctorCertificateValidity = data.FSSAIDoctorCertificateValidity,

                            DateofLeaving = data.DateofLeaving,

                            CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value),
                            CreatedOn = DateTime.Now,
                            UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value),
                            UpdatedOn = DateTime.Now,
                            IsDelete = false
                        });
                    if (Roles.IsUserInRole(data.UserName, data.RoleName))
                    {
                        ViewBag.ResultMessage = "This user already has the role specified !";
                    }
                    else
                    {
                        Roles.AddUserToRole(data.UserName, data.RoleName);
                        ViewBag.ResultMessage = "Username added to the role successfully !";
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                //Update code
                #region Update
                User obj = new User();
                obj.Id = data.UserID;
                obj.FullName = data.FullName;

                //long EmployeeCodeExist = _areaservice.CheckEmployeeCodeIsExists(data.EmployeeCode);
                //if (data.EmployeeCode != EmployeeCodeExist)
                //{
                //    obj.EmployeeCode = data.EmployeeCode;
                //}
                //else
                //{
                //    obj.EmployeeCode = EmployeeCodeExist;
                //}

                obj.EmployeeCode = data.EmployeeCode;
                obj.UserName = data.UserNameUpdate;
                obj.Password = data.Password;
                obj.BirthDate = data.BirthDate;
                obj.Age = data.Age;
                obj.Gender = data.Gender;
                obj.DateOfJoining = data.DateOfJoining;
                obj.Email = data.Email;
                obj.RoleID = data.RoleID;
                obj.MobileNumber = data.MobileNumber;
                obj.BankName = data.BankName;
                obj.Branch = data.Branch;
                obj.AccountNumber = data.AccountNumber;
                obj.IFSCCode = data.IFSCCode;
                obj.PrimaryArea = data.PrimaryArea;
                obj.PrimaryAddress = data.PrimaryAddress;
                obj.PrimaryPin = data.PrimaryPin;
                obj.SecondaryArea = data.SecondaryArea;
                obj.SecondaryAddress = data.SecondaryAddress;
                obj.SecondaryPin = data.SecondaryPin;
                obj.PanNo = data.PanNo;
                obj.PassportNo = data.PassportNo;
                obj.PassportValiddate = data.PassportValiddate;
                obj.UIDAI = data.UIDAI;
                obj.UAN = data.UAN;
                obj.PF = data.PF;
                obj.ESIC = data.ESIC;
                obj.Drivinglicence = data.Drivinglicence;
                obj.DrivingValidup = data.DrivingValidup;
                obj.GodownID = data.GodownID;
                obj.ReferenceName = data.ReferenceName;
                obj.FName = data.FName;
                obj.Fdob = data.Fdob;
                obj.FUIDAI = data.FUIDAI;
                obj.FRelation = data.FRelation;
                obj.Flivingtogether = data.Flivingtogether;
                obj.MName = data.MName;
                obj.Mdob = data.Mdob;
                obj.MUIDAI = data.MUIDAI;
                obj.MRelation = data.MRelation;
                obj.Mlivingtogether = data.Mlivingtogether;
                obj.WName = data.WName;
                obj.Wdob = data.Wdob;
                obj.WUIDAI = data.WUIDAI;
                obj.WRelation = data.WRelation;
                obj.Wlivingtogether = data.Wlivingtogether;
                obj.C1Name = data.C1Name;
                obj.C1dob = data.C1dob;
                obj.C1UIDAI = data.C1UIDAI;
                obj.C1Relation = data.C1Relation;
                obj.C1livingtogether = data.C1livingtogether;
                obj.C2Name = data.C2Name;
                obj.C2dob = data.C2dob;
                obj.C2UIDAI = data.C2UIDAI;
                obj.C2Relation = data.C2Relation;
                obj.C2livingtogether = data.C2livingtogether;
                obj.C3Name = data.C3Name;
                obj.C3dob = data.C3dob;
                obj.C3UIDAI = data.C3UIDAI;
                obj.C3Relation = data.C3Relation;
                obj.C3livingtogether = data.C3livingtogether;
                obj.Maritalstatus = data.Maritalstatus;
                obj.FSSAIDoctorCertificateValidity = data.FSSAIDoctorCertificateValidity;
                string strFSSAIDoctorCertificate = "";
                string strProfilePicture = "";
                if (Request.Files.Count != 0)
                {
                    foreach (string fileName in Request.Files)
                    {
                        if (fileName == "FSSAIDoctorCertificate")
                        {
                            string FSSAIDoctorCertificate;
                            HttpPostedFileBase FSSAIDoctorCertificatefile = Request.Files[fileName];
                            string fileName2 = data.EmployeeCode + "-" + "FSSAIDoctorCerti" + Path.GetExtension(FSSAIDoctorCertificatefile.FileName).ToString();
                            obj.FSSAIDoctorCertificate = fileName2;
                            FSSAIDoctorCertificate = Path.Combine(Server.MapPath("~/Document/"), obj.FSSAIDoctorCertificate);
                            FSSAIDoctorCertificatefile.SaveAs(FSSAIDoctorCertificate);
                            strFSSAIDoctorCertificate = obj.FSSAIDoctorCertificate;
                        }
                        else if (fileName == "ProfilePicture")
                        {
                            string ProfilePicture;
                            HttpPostedFileBase ProfilePicturefile = Request.Files[fileName];
                            string fileName1 = data.EmployeeCode + "-" + data.FullName + Path.GetExtension(ProfilePicturefile.FileName).ToString();
                            obj.ProfilePicture = fileName1;
                            ProfilePicture = Path.Combine(Server.MapPath("~/ProfilePicture/"), obj.ProfilePicture);
                            ProfilePicturefile.SaveAs(ProfilePicture);
                            strProfilePicture = obj.ProfilePicture;
                        }
                    }
                }
                if (strFSSAIDoctorCertificate == "")
                {
                    oldimage2 = _areaservice.GetUserOldFSSAIDoctorCertificate(data.UserID);
                    obj.FSSAIDoctorCertificate = oldimage2;
                }
                if (strProfilePicture == "")
                {
                    oldimage = _areaservice.GetUserOldImage(data.UserID);
                    obj.ProfilePicture = oldimage;
                }

                obj.DateofLeaving = data.DateofLeaving;

                obj.CreatedBy = data.CreatedBy;
                obj.CreatedOn = data.CreatedOn;
                obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                obj.UpdatedOn = DateTime.Now;
                obj.IsDelete = data.IsDelete;
                result = _areaservice.UpdateUser(obj);


                // 08 Feb 2021 Piyush Limbani
                bool UpdateRoles = _areaservice.Updatewebpages_UsersInRoles(obj.Id, data.RoleID);
                // 08 Feb 2021 Piyush Limbani

                #endregion
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ManageUsersList()
        {
            List<RegistrationListResponse> objModel = _areaservice.GetAllUserList();
            return PartialView(objModel);
        }

        [HttpPost]
        public JsonResult AddDocuments()
        {
            try
            {
                Document_Master Obj = new Document_Master();
                Obj.EmployeeCode = Convert.ToInt32(Request.Form["EmployeeCode"]);
                Obj.DocumentID = Convert.ToInt32(Request.Form["DocumentID"]);
                GetDocumentsResponse docdata = _areaservice.GetDocumentsByEmployeeID(Convert.ToInt64(Obj.EmployeeCode));

                // 1
                HttpPostedFile AadharCard = System.Web.HttpContext.Current.Request.Files["AadharCard"];
                if (AadharCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "AadharCard" + Path.GetExtension(AadharCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.AadharCard = InputFileName;
                    AadharCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.AadharCard = docdata.AadharCard;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 2
                HttpPostedFile BankPassBook = System.Web.HttpContext.Current.Request.Files["BankPassBook"];
                if (BankPassBook != null)
                {
                    var InputFileName = Obj.EmployeeCode + "BankPassBook" + Path.GetExtension(BankPassBook.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.BankPassBook = InputFileName;
                    BankPassBook.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.BankPassBook = docdata.BankPassBook;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 3 
                HttpPostedFile BioData = System.Web.HttpContext.Current.Request.Files["BioData"];
                if (BioData != null)
                {
                    var InputFileName = Obj.EmployeeCode + "BioData" + Path.GetExtension(BioData.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.BioData = InputFileName;
                    BioData.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.BioData = docdata.BioData;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 4
                HttpPostedFile DrivingLicence = System.Web.HttpContext.Current.Request.Files["DrivingLicence"];
                if (DrivingLicence != null)
                {
                    var InputFileName = Obj.EmployeeCode + "DrivingLicence" + Path.GetExtension(DrivingLicence.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.DrivingLicence = InputFileName;
                    DrivingLicence.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.DrivingLicence = docdata.DrivingLicence;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 5
                HttpPostedFile ElectionCard = System.Web.HttpContext.Current.Request.Files["ElectionCard"];
                if (ElectionCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "ElectionCard" + Path.GetExtension(ElectionCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.ElectionCard = InputFileName;
                    ElectionCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.ElectionCard = docdata.ElectionCard;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 6
                HttpPostedFile ElectricityBill = System.Web.HttpContext.Current.Request.Files["ElectricityBill"];
                if (ElectricityBill != null)
                {
                    var InputFileName = Obj.EmployeeCode + "ElectricityBill" + Path.GetExtension(ElectricityBill.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.ElectricityBill = InputFileName;
                    ElectricityBill.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.ElectricityBill = docdata.ElectricityBill;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 7
                HttpPostedFile IDCard = System.Web.HttpContext.Current.Request.Files["IDCard"];
                if (IDCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "IDCard" + Path.GetExtension(IDCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.IDCard = InputFileName;
                    IDCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.IDCard = docdata.IDCard;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 8
                HttpPostedFile LeavingCertificate = System.Web.HttpContext.Current.Request.Files["LeavingCertificate"];
                if (LeavingCertificate != null)
                {
                    var InputFileName = Obj.EmployeeCode + "LeavingCertificate" + Path.GetExtension(LeavingCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.LeavingCertificate = InputFileName;
                    LeavingCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.LeavingCertificate = docdata.LeavingCertificate;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 9
                HttpPostedFile PanCard = System.Web.HttpContext.Current.Request.Files["PanCard"];
                if (PanCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "PanCard" + Path.GetExtension(PanCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.PanCard = InputFileName;
                    PanCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.PanCard = docdata.PanCard;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 10
                HttpPostedFile RationCard = System.Web.HttpContext.Current.Request.Files["RationCard"];
                if (RationCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "RationCard" + Path.GetExtension(RationCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.RationCard = InputFileName;
                    RationCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.RationCard = docdata.RationCard;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 11
                HttpPostedFile Rentagreement = System.Web.HttpContext.Current.Request.Files["Rentagreement"];
                if (Rentagreement != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Rentagreement" + Path.GetExtension(Rentagreement.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Rentagreement = InputFileName;
                    Rentagreement.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Rentagreement = docdata.Rentagreement;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 12
                HttpPostedFile Photo = System.Web.HttpContext.Current.Request.Files["Photo"];
                if (Photo != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Photo" + Path.GetExtension(Photo.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Photo = InputFileName;
                    Photo.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Photo = docdata.Photo;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 13
                HttpPostedFile Signechar = System.Web.HttpContext.Current.Request.Files["signechar"];
                if (Signechar != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Signechar" + Path.GetExtension(Signechar.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Signechar = InputFileName;
                    Signechar.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Signechar = docdata.Signechar;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 14
                HttpPostedFile FamilyPhoto = System.Web.HttpContext.Current.Request.Files["FamilyPhoto"];
                if (FamilyPhoto != null)
                {
                    var InputFileName = Obj.EmployeeCode + "FamilyPhoto" + Path.GetExtension(FamilyPhoto.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.FamilyPhoto = InputFileName;
                    FamilyPhoto.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.FamilyPhoto = docdata.FamilyPhoto;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 15
                HttpPostedFile Passport = System.Web.HttpContext.Current.Request.Files["Passport"];
                if (Passport != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Passport" + Path.GetExtension(Passport.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Passport = InputFileName;
                    Passport.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Passport = docdata.Passport;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 16
                HttpPostedFile Other1 = System.Web.HttpContext.Current.Request.Files["Other1"];
                if (Other1 != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Other1" + Path.GetExtension(Other1.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Other1 = InputFileName;
                    Other1.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Other1 = docdata.Other1;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 17
                HttpPostedFile Other2 = System.Web.HttpContext.Current.Request.Files["Other2"];
                if (Other2 != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Other2" + Path.GetExtension(Other2.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Other2 = InputFileName;
                    Other2.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Other2 = docdata.Other2;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 18
                HttpPostedFile Other3 = System.Web.HttpContext.Current.Request.Files["Other3"];
                if (Other3 != null)
                {
                    var InputFileName = Obj.EmployeeCode + "Other3" + Path.GetExtension(Other3.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.Other3 = InputFileName;
                    Other3.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Other3 = docdata.Other3;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 19 ESIC Card
                HttpPostedFile ESICCard = System.Web.HttpContext.Current.Request.Files["ESICCard"];
                if (ESICCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "ESICCard" + Path.GetExtension(ESICCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.ESICCard = InputFileName;
                    ESICCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.ESICCard = docdata.ESICCard;
                    Obj.DocumentID = docdata.DocumentID;
                }

                // 20 ESI Pehchan Card
                HttpPostedFile ESIPehchanCard = System.Web.HttpContext.Current.Request.Files["ESIPehchanCard"];
                if (ESIPehchanCard != null)
                {
                    var InputFileName = Obj.EmployeeCode + "ESIPehchanCard" + Path.GetExtension(ESIPehchanCard.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.ESIPehchanCard = InputFileName;
                    ESIPehchanCard.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.ESIPehchanCard = docdata.ESIPehchanCard;
                    Obj.DocumentID = docdata.DocumentID;
                }


                // 21 Medical Fitness Cirtificate
                HttpPostedFile MedicalFitnessCirtificate = System.Web.HttpContext.Current.Request.Files["MedicalFitnessCirtificate"];
                if (MedicalFitnessCirtificate != null)
                {
                    var InputFileName = Obj.EmployeeCode + "MedicalFitnessCirtificate" + Path.GetExtension(MedicalFitnessCirtificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Document/") + InputFileName);
                    Obj.MedicalFitnessCirtificate = InputFileName;
                    MedicalFitnessCirtificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.MedicalFitnessCirtificate = docdata.MedicalFitnessCirtificate;
                    Obj.DocumentID = docdata.DocumentID;
                }



                bool responsedata = _areaservice.AddDocuments(Obj);

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AddDocuments(long? EmployeeCode)
        {
            try
            {
                GetDocumentsResponse docdata = _areaservice.GetUploadedDocumentsFullPathListByEmployeeID(Convert.ToInt64(EmployeeCode));
                return View(docdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetDocumentDetail(Int32? EmployeeCode)
        {
            try
            {
                GetDocumentsResponse docdata = _areaservice.GetUploadedDocumentsFullPathListByEmployeeID(Convert.ToInt64(EmployeeCode));
                return Json(docdata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult DeleteUser(long? UserID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteUser(UserID.Value, IsDelete);
                return RedirectToAction("ManageUsers");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageUsers");
            }
        }

        public ActionResult ExportExcelUsers()
        {
            var UserList = _areaservice.GetAllUserListForExcel();
            List<UserListExport> lstuser = UserList.Select(x => new UserListExport()
            {
                EmployeeCode = x.EmployeeCode,
                FullName = x.FullName,
                Gender = x.Gender,
                Age = x.Age,
                Designation = x.RoleName,
                UIDAINo = x.UIDAI.ToString(),
                BirthDate = x.BirthDatestr,
                DateOfJoining = x.DateOfJoiningstr,
                PassportNo = x.PassportNo,
                PassportValidDate = x.PassportValiddate,
                Drivinglicence = x.Drivinglicence,
                DrivingValidup = x.DrivingValidup,
                GodownName = x.GodownName,
                PanNo = x.PanNo,
                ESIC = x.ESIC,
                MobileNumber = x.MobileNumber,
                PF = x.PF,
                Email = x.Email,
                UAN = x.UAN.ToString(),
                PrimaryAddress = x.PrimaryAddress + ',' + x.PrimaryAreaName + ',' + x.PrimaryPin,
                SecondaryAddress = x.SecondaryAddress + ',' + x.SecondaryArea + ',' + x.SecondaryPin,
                MaritalStatus = x.Maritalstatus,
                ReferenceName = x.ReferenceName,
                FSSAIValidity = x.FSSAIDoctorCertificateValiditystr,
                BankName = x.BankName,
                Branch = x.Branch,
                AccountNumber = x.AccountNumber,
                IFSCCode = x.IFSCCode
                //RoleName = x.RoleName,
                //Address = x.Address,         
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstuser));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "EmployeeList.xls");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        [HttpPost]
        public JsonResult ChangeUserPassword(string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            bool result = false;
            Register ObjEntity = new Register();
            if (Request.Cookies["UserID"].Value != null)
            {
                Register UserDetail = _areaservice.Users_SelectByUserIDProfile(Convert.ToInt64(Request.Cookies["UserID"].Value));

                if (UserDetail != null)
                {
                    MembershipUser obj = new MembershipUser("SimpleMembershipProvider", UserDetail.UserName, "", "", "", "", true, true, UserDetail.CreatedOn, UserDetail.CreatedOn, UserDetail.CreatedOn, System.DateTime.Now, UserDetail.CreatedOn);
                    result = obj.ChangePassword(OldPassword, NewPassword);
                    if (result != false)
                    {

                        bool respose = _areaservice.UpdatePassword(Convert.ToInt64(Request.Cookies["UserID"].Value), NewPassword);

                        return Json(new
                        {
                            //  redirectUrl = Url.Action("Index", "Home", new { area = "wholesale" }),
                            redirectUrl = Url.Action("Login", "Login", new { area = "" }),
                            isRedirect = false,
                            Oldpassmatch = true,
                            Passchange = result
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            redirectUrl = Url.Action("Index", "Home", new { area = "wholesale" }),
                            isRedirect = false,
                            Oldpassmatch = false,
                            Passchange = result
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "Home", new { area = "wholesale" }),
                        isRedirect = false,
                        Oldpassmatch = false,
                        Passchange = result
                    });
                }

            }
            else
            {
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home", new { area = "wholesale" }),
                    isRedirect = true,
                    Oldpassmatch = false,
                    Passchange = false
                });
            }

            // return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageDriver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageDriver(DriverViewModel data)
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
                    Driver_Mst obj = new Driver_Mst();
                    obj.DriverID = data.DriverID;
                    obj.DriverName = data.DriverName;
                    obj.TempoNumber = data.TempoNumber;
                    obj.DriverMobileNumber = data.DriverMobileNumber;
                    obj.Licence = data.Licence;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _areaservice.AddDriver(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageDriverList()
        {
            List<DriverListResponse> objModel = _areaservice.GetAllDriverList();
            return PartialView(objModel);
        }

        public ActionResult DeleteDriver(long? DriverID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteDriver(DriverID.Value, IsDelete);
                return RedirectToAction("ManageDriver");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageDriver");
            }
        }

        //[HttpGet]
        //public ActionResult AssignRoleRight()
        //{
        //    DynamicMenuModel objMenu = new DynamicMenuModel();
        //    objMenu = _ICommonService.GetAllDynamicMenuList();
        //    ViewBag.Role = _areaservice.GetAllRoleName();
        //    return View("Index", objMenu);
        //}

        //[HttpPost]
        //public ActionResult SelectAssignedAuthority(int roleid)
        //{
        //    DynamicMenuModel objMenu = new DynamicMenuModel();
        //    objMenu = _ICommonService.GetAllDynamicMenuList();
        //    ViewBag.Role = _areaservice.GetAllRoleName();
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult AssignRoleRight(List<MainMenu> model)
        //{
        //    ViewBag.Role = _areaservice.GetAllRoleName();
        //    return View();
        //}

        [HttpGet]
        public ActionResult AssignRoleRight()
        {
            ViewBag.Role = _areaservice.GetAllRoleName();
            return View("Index");
        }

        public PartialViewResult RoleList(int roleid)
        {
            // int RoleID = Convert.ToInt32(Session["RoleId"]);
            DynamicMenuModel objMenu = new DynamicMenuModel();
            objMenu = _ICommonService.GetAllDynamicMenuList(roleid);
            ViewBag.Role = _areaservice.GetAllRoleName();
            return PartialView(objMenu);
        }

        [HttpPost]
        public ActionResult ManageAuthority(int roleid, List<AuthorizeMaster> data)
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
                    string response = "";
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].AuthorizeID == 0)
                        {
                            var lstdata = _areaservice.GetExistAuthorityDetail(roleid, data[i].MenuID);
                            if (lstdata.AuthorizeID != 0)
                            {
                                bool respose = _areaservice.UpdateAuthorityMaster(lstdata.AuthorizeID, data[i].IsActive);
                            }
                            else if (lstdata.AuthorizeID == 0)
                            {
                                AuthorizeMaster obj = new AuthorizeMaster();
                                obj.RoleID = roleid;
                                obj.MenuID = data[i].MenuID;
                                obj.CreatedDate = DateTime.Now;
                                obj.IsActive = data[i].IsActive;
                                response = (_areaservice.AddAuthority(obj)).ToString();
                            }
                        }
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult CreatePasswrd()
        //{
        //    //call data
        //    List<Register> data = _areaservice.Guser();
        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        WebSecurity.CreateUserAndAccount(data[i].UserName, data[i].Password,
        //                new
        //                {
        //                    FullName = data[i].UserFullName,
        //                    RoleID = data[i].RoleID,
        //                    UserCode = data[i].UserCode,
        //                    UserEmail = data[i].UserEmail,
        //                    UserMobile = data[i].UserMobile,
        //                    UserPhone = data[i].UserPhone,
        //                    UserPhoneExtn = data[i].UserPhoneExtn,
        //                    UserDesignation = data[i].UserDesignation,
        //                    UserDepartment = data[i].UserDepartment,
        //                    GodownID = data[i].GodownID,
        //                    UserLocation = data[i].UserLocation,
        //                    UserRemark = data[i].UserRemark,
        //                    CreatedBy = 1,
        //                    CreatedOn = DateTime.Now,
        //                    UpdatedBy = 1,
        //                    UpdatedOn = DateTime.Now,
        //                    IsDelete = false
        //                });
        //    }
        //    return View();
        //}

        //Wholesale      

        public ActionResult CreatePasswrd()
        {
            //call data
            List<Register> data = _areaservice.Guser();
            for (int i = 0; i < data.Count; i++)
            {
                WebSecurity.CreateUserAndAccount(data[i].UserName, data[i].Password,
                        new
                        {
                            EmployeeCode = data[i].EmployeeCode,
                            FullName = data[i].FullName,
                            UserName = data[i].UserName,
                            Password = data[i].Password,
                            RoleID = data[i].RoleID,
                            BirthDate = data[i].BirthDate,
                            Age = data[i].Age,
                            Gender = data[i].Gender,
                            DateOfJoining = data[i].DateOfJoining,
                            Email = data[i].Email,
                            Address = data[i].Address,
                            MobileNumber = data[i].MobileNumber,
                            BankName = data[i].BankName,
                            AccountNumber = data[i].AccountNumber,
                            Branch = data[i].Branch,
                            IFSCCode = data[i].IFSCCode,
                            PrimaryArea = data[i].PrimaryArea,
                            PrimaryAddress = data[i].PrimaryAddress,
                            PrimaryPin = data[i].PrimaryPin,
                            SecondaryArea = data[i].SecondaryArea,
                            SecondaryAddress = data[i].SecondaryAddress,
                            SecondaryPin = data[i].SecondaryPin,
                            PanNo = data[i].PanNo,
                            PassportNo = data[i].PassportNo,
                            PassportValiddate = data[i].PassportValiddate,
                            UIDAI = data[i].UIDAI,
                            UAN = data[i].UAN,
                            PF = data[i].PF,
                            ESIC = data[i].ESIC,
                            Drivinglicence = data[i].Drivinglicence,
                            DrivingValidup = data[i].DrivingValidup,
                            ReferenceName = data[i].ReferenceName,
                            FName = data[i].FName,
                            Fdob = data[i].Fdob,
                            FUIDAI = data[i].FUIDAI,
                            FRelation = data[i].FRelation,
                            Flivingtogether = data[i].Flivingtogether,
                            MName = data[i].MName,
                            Mdob = data[i].Mdob,
                            MUIDAI = data[i].MUIDAI,
                            MRelation = data[i].MRelation,
                            Mlivingtogether = data[i].Mlivingtogether,
                            WName = data[i].WName,
                            Wdob = data[i].Wdob,
                            WUIDAI = data[i].WUIDAI,
                            WRelation = data[i].WRelation,
                            Wlivingtogether = data[i].Wlivingtogether,
                            C1Name = data[i].C1Name,
                            C1dob = data[i].C1dob,
                            C1UIDAI = data[i].C1UIDAI,
                            C1Relation = data[i].C1Relation,
                            C1livingtogether = data[i].C1livingtogether,
                            C2Name = data[i].C2Name,
                            C2dob = data[i].C2dob,
                            C2UIDAI = data[i].C2UIDAI,
                            C2Relation = data[i].C2Relation,
                            C2livingtogether = data[i].C2livingtogether,
                            C3Name = data[i].C3Name,
                            C3dob = data[i].C3dob,
                            C3UIDAI = data[i].C3UIDAI,
                            C3Relation = data[i].C3Relation,
                            C3livingtogether = data[i].C3livingtogether,
                            GodownID = data[i].GodownID,
                            ServiceTime = data[i].ServiceTime,
                            Maritalstatus = data[i].Maritalstatus,
                            ProfilePicture = data[i].ProfilePicture,
                            CreatedBy = data[i].CreatedBy,
                            CreatedOn = data[i].CreatedOn,
                            UpdatedBy = data[i].UpdatedBy,
                            UpdatedOn = data[i].UpdatedOn,
                            IsDelete = data[i].IsDelete
                        });
            }
            return View();
        }

        public ActionResult UpdateInvoiceTotal()
        {
            List<InvoiceTotal> data = _areaservice.GetInvoice();
            var data1 = data.GroupBy(x => new { x.InvoiceNumber, x.OrderID }).Select(y => new { InvoiceNumber = y.Key.InvoiceNumber, total = y.Sum(t => t.FinalTotal), OrderID = y.Key.OrderID });
            var dd = data1.OrderByDescending(x => x.OrderID);
            foreach (var item in data1)
            {
                try
                {
                    _ICommonService.UpdateInvoiceTotal(item.total, item.InvoiceNumber, item.OrderID);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return View();
        }

        //Retail
        public ActionResult UpdateRetInvoiceTotal()
        {
            List<RetInvoiceTotal> data = _areaservice.GetRetInvoice();
            var data1 = data.GroupBy(x => new { x.InvoiceNumber, x.OrderID }).Select(y => new { InvoiceNumber = y.Key.InvoiceNumber, total = y.Sum(t => t.TotalAmount), OrderID = y.Key.OrderID });
            var dd = data1.OrderByDescending(x => x.OrderID);
            foreach (var item in data1)
            {
                try
                {
                    _ICommonService.UpdateRetInvoiceTotal(item.total, item.InvoiceNumber, item.OrderID);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return View();
        }

        public ActionResult ManageTransport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageTransport(TransportViewModel data)
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
                    Transport_Mst obj = new Transport_Mst();
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
            List<TransportListResponse> objModel = _areaservice.GetAllTransportList();
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

        public ActionResult ManageVehicle()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddVehicleDetails()
        {
            try
            {
                VehicleDetail_Mst Obj = new VehicleDetail_Mst();
                Obj.VehicleDetailID = Convert.ToInt64(Request.Form["VehicleDetailID"]);
                Obj.VehicleNumber = Convert.ToString(Request.Form["VehicleNumber"]);
                //     Obj.DateOfPurchase = Convert.ToDateTime(Request.Form["DateOfPurchase"]);
                if ((Request.Form["DateOfPurchase"]) != "" && (Request.Form["DateOfPurchase"]) != null)
                {
                    Obj.DateOfPurchase = Convert.ToDateTime(Request.Form["DateOfPurchase"]);
                }
                else
                {
                    Obj.DateOfPurchase = null;
                }
                GetVehicleCertificate docdata = _areaservice.GetVehicleCertificateByVehicleDetailID(Convert.ToInt64(Obj.VehicleDetailID));

                // 1
                HttpPostedFile RCCertificate = System.Web.HttpContext.Current.Request.Files["RCCertificate"];
                if (RCCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.RCCertificate + "RCCertificate" + Path.GetExtension(RCCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.RCCertificate = InputFileName;
                    RCCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.RCCertificate = docdata.RCCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["RCNoValidity"]) != "" && (Request.Form["RCNoValidity"]) != null)
                {
                    Obj.RCNoValidity = Convert.ToDateTime(Request.Form["RCNoValidity"]);
                }
                else
                {
                    Obj.RCNoValidity = null;
                }

                // 2
                HttpPostedFile FitnessCertificate = System.Web.HttpContext.Current.Request.Files["FitnessCertificate"];
                if (FitnessCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.FitnessCertificate + "FitnessCertificate" + Path.GetExtension(FitnessCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.FitnessCertificate = InputFileName;
                    FitnessCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.FitnessCertificate = docdata.FitnessCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["FitnessValidity"]) != "" && (Request.Form["FitnessValidity"]) != null)
                {
                    Obj.FitnessValidity = Convert.ToDateTime(Request.Form["FitnessValidity"]);
                }
                else
                {
                    Obj.FitnessValidity = null;
                }

                // 3
                HttpPostedFile PermitCertificate = System.Web.HttpContext.Current.Request.Files["PermitCertificate"];
                if (PermitCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.PermitCertificate + "PermitCertificate" + Path.GetExtension(PermitCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.PermitCertificate = InputFileName;
                    PermitCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.PermitCertificate = docdata.PermitCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["PermitValidity"]) != "" && (Request.Form["PermitValidity"]) != null)
                {
                    Obj.PermitValidity = Convert.ToDateTime(Request.Form["PermitValidity"]);
                }
                else
                {
                    Obj.PermitValidity = null;
                }

                // 4
                HttpPostedFile PUCCertificate = System.Web.HttpContext.Current.Request.Files["PUCCertificate"];
                if (PUCCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.PUCCertificate + "PUCCertificate" + Path.GetExtension(PUCCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.PUCCertificate = InputFileName;
                    PUCCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.PUCCertificate = docdata.PUCCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["PUCValidity"]) != "" && (Request.Form["PUCValidity"]) != null)
                {
                    Obj.PUCValidity = Convert.ToDateTime(Request.Form["PUCValidity"]);
                }
                else
                {
                    Obj.PUCValidity = null;
                }

                // 5
                HttpPostedFile InsuranceCertificate = System.Web.HttpContext.Current.Request.Files["InsuranceCertificate"];
                if (InsuranceCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.InsuranceCertificate + "InsuranceCertificate" + Path.GetExtension(InsuranceCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.InsuranceCertificate = InputFileName;
                    InsuranceCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.InsuranceCertificate = docdata.InsuranceCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["InsuranceValidity"]) != "" && (Request.Form["InsuranceValidity"]) != null)
                {
                    Obj.InsuranceValidity = Convert.ToDateTime(Request.Form["InsuranceValidity"]);
                }
                else
                {
                    Obj.InsuranceValidity = null;
                }

                // 6
                HttpPostedFile AdvertisementCertificate = System.Web.HttpContext.Current.Request.Files["AdvertisementCertificate"];
                if (AdvertisementCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.AdvertisementCertificate + "AdvertisementCerti" + Path.GetExtension(AdvertisementCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.AdvertisementCertificate = InputFileName;
                    AdvertisementCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.AdvertisementCertificate = docdata.AdvertisementCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["AdvertisementValidity"]) != "" && (Request.Form["AdvertisementValidity"]) != null)
                {
                    Obj.AdvertisementValidity = Convert.ToDateTime(Request.Form["AdvertisementValidity"]);
                }
                else
                {
                    Obj.AdvertisementValidity = null;
                }

                // 7
                HttpPostedFile SpeedGovernorCertificate = System.Web.HttpContext.Current.Request.Files["SpeedGovernorCertificate"];
                if (SpeedGovernorCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.SpeedGovernorCertificate + "SpeedGovernorCerti" + Path.GetExtension(SpeedGovernorCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.SpeedGovernorCertificate = InputFileName;
                    SpeedGovernorCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.SpeedGovernorCertificate = docdata.SpeedGovernorCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["SpeedGovernorCertificateValidity"]) != "" && (Request.Form["SpeedGovernorCertificateValidity"]) != null)
                {
                    Obj.SpeedGovernorCertificateValidity = Convert.ToDateTime(Request.Form["SpeedGovernorCertificateValidity"]);
                }
                else
                {
                    Obj.SpeedGovernorCertificateValidity = null;
                }

                // 8
                HttpPostedFile FSSAICertificate = System.Web.HttpContext.Current.Request.Files["FSSAICertificate"];
                if (FSSAICertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.FSSAICertificate + "FSSAICertificate" + Path.GetExtension(FSSAICertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.FSSAICertificate = InputFileName;
                    FSSAICertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.FSSAICertificate = docdata.FSSAICertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["FSSAICertificateValidity"]) != "" && (Request.Form["FSSAICertificateValidity"]) != null)
                {
                    Obj.FSSAICertificateValidity = Convert.ToDateTime(Request.Form["FSSAICertificateValidity"]);
                }
                else
                {
                    Obj.FSSAICertificateValidity = null;
                }

                // 9
                HttpPostedFile GreenTaxCertificate = System.Web.HttpContext.Current.Request.Files["GreenTaxCertificate"];
                if (GreenTaxCertificate != null)
                {
                    var InputFileName = Obj.VehicleNumber + " - " + Obj.GreenTaxCertificate + "GreenTaxCertificate" + Path.GetExtension(GreenTaxCertificate.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.GreenTaxCertificate = InputFileName;
                    GreenTaxCertificate.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.GreenTaxCertificate = docdata.GreenTaxCertificate;
                    Obj.VehicleDetailID = docdata.VehicleDetailID;
                }
                if ((Request.Form["GreenTaxCertificateValidity"]) != "" && (Request.Form["GreenTaxCertificateValidity"]) != null)
                {
                    Obj.GreenTaxCertificateValidity = Convert.ToDateTime(Request.Form["GreenTaxCertificateValidity"]);
                }
                else
                {
                    Obj.GreenTaxCertificateValidity = null;
                }

                // 10
                if ((Request.Form["InstallmentAmount"]) != "")
                {
                    Obj.InstallmentAmount = Convert.ToDecimal(Request.Form["InstallmentAmount"]);
                }
                else
                {
                    Obj.InstallmentAmount = 0;
                }
                if ((Request.Form["InstallmentAmountDate"]) != "" && (Request.Form["InstallmentAmountDate"]) != null)
                {
                    Obj.InstallmentAmountDate = Convert.ToDateTime(Request.Form["InstallmentAmountDate"]);
                }
                else
                {
                    Obj.InstallmentAmountDate = null;
                }

                // 11
                // Obj.OneTimeTax = Convert.ToDecimal(Request.Form["OneTimeTax"]);
                if ((Request.Form["OneTimeTax"]) != "")
                {
                    Obj.OneTimeTax = Convert.ToDecimal(Request.Form["OneTimeTax"]);
                }
                else
                {
                    Obj.OneTimeTax = 0;
                }
                if ((Request.Form["OneTimeTaxDate"]) != "" && (Request.Form["OneTimeTaxDate"]) != null)
                {
                    Obj.OneTimeTaxDate = Convert.ToDateTime(Request.Form["OneTimeTaxDate"]);
                }
                else
                {
                    Obj.OneTimeTaxDate = null;
                }
                Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                Obj.CreatedOn = DateTime.Now;
                Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                Obj.UpdatedOn = DateTime.Now;
                Obj.IsDelete = false;
                bool responsedata = _areaservice.AddVehicleDetail(Obj);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PartialViewResult VehicleList()
        {
            List<VehicleListResponse> objModel = _areaservice.GetAllVehicleList();
            return PartialView(objModel);
        }

        public ActionResult DeleteVehicle(long? VehicleDetailID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteVehicle(VehicleDetailID.Value, IsDelete);
                return RedirectToAction("ManageVehicle");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageVehicle");
            }
        }

        public ActionResult ManageLicence()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddLicenceDetails()
        {
            try
            {
                Licence_Mst Obj = new Licence_Mst();
                Obj.LicenceID = Convert.ToInt64(Request.Form["LicenceID"]);
                Obj.LicenceType = Convert.ToString(Request.Form["LicenceType"]);
                Obj.WhereFrom = Convert.ToString(Request.Form["WhereFrom"]);
                Obj.FromDate = Convert.ToDateTime(Request.Form["FromDate"]);
                Obj.ToDate = Convert.ToDateTime(Request.Form["ToDate"]);
                Obj.Remark = Convert.ToString(Request.Form["Remark"]);

                LicenceListResponse docdata = _areaservice.GetLicenceDocByLicenceID(Convert.ToInt64(Obj.LicenceID));

                HttpPostedFile Documents = System.Web.HttpContext.Current.Request.Files["Documents"];
                if (Documents != null)
                {

                    // var InputFileName = Obj.Documents + "Documents" + Path.GetExtension(Documents.FileName);
                    var InputFileName = Obj.WhereFrom + " - " + Obj.LicenceType + " - " + Obj.Documents + Path.GetExtension(Documents.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/VehicleDoc/") + InputFileName);
                    Obj.Documents = InputFileName;
                    Documents.SaveAs(ServerSavePath);
                }
                else
                {
                    Obj.Documents = docdata.Documents;
                    Obj.LicenceID = docdata.LicenceID;
                }

                Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                Obj.CreatedOn = DateTime.Now;
                Obj.IsDelete = false;
                Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                Obj.UpdatedOn = DateTime.Now;

                bool responsedata = _areaservice.AddLicenceDetails(Obj);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PartialViewResult LicenceList()
        {
            List<LicenceListResponse> objModel = _areaservice.GetAllLicenceList();
            return PartialView(objModel);
        }

        public ActionResult DeleteLicence(long? LicenceID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteLicence(LicenceID.Value, IsDelete);
                return RedirectToAction("ManageLicence");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageLicence");
            }
        }


        // 18-04-2018 update Vehicle Assigned Date in order qty table

        //public ActionResult UpdateRetVehicleAssignedDate()
        //{
        //    List<UpdateVehicleAssignedDate> data = _areaservice.GetRetVehicleAssignedDate();

        //    foreach (var item in data)
        //    {
        //        try
        //        {
        //            _areaservice.UpdateRetVehicleAssignedDate(item.InvoiceNumber, item.CreatedOn);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }
        //    return View();
        //}

        public ActionResult UpdateRetCreditmemo()
        {
            List<UpdateVehicleAssignedDate> data = _areaservice.GetRetOrderQtyID();

            foreach (var item in data)
            {
                var ExpensesVoucherDetail = _areaservice.GetOrderDareandProductQTYID(item.OrderQtyID);
                try
                {
                    //_areaservice.UpdateRetOrderDareandProductQTYID(item.OrderQtyID, ExpensesVoucherDetail.ProductQtyID, ExpensesVoucherDetail.OrderDate);
                    _areaservice.UpdateRetOrderDareandProductQTYID(item.OrderQtyID, ExpensesVoucherDetail.OrderDate);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return View();
        }


        // For Update Customer Mobile no Wholeasle         // 19 Aug 2020 Piyush Limbani
        public ActionResult UpdateCellNo()
        {
            List<GetCustomerID> data = _areaservice.GetAllCustomerID();

            foreach (var item in data)
            {
                var GetCellNoDetail = _areaservice.GetCellNoDetail(item.CustomerID);
                try
                {
                    _areaservice.UpdateCellNoDetaiLByCystomerID(item.CustomerID, GetCellNoDetail.CellNo, GetCellNoDetail.TelNo, GetCellNoDetail.Email);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return View();
        }

        // For Update Customer Mobile no Retail         // 19 Aug 2020 Piyush Limbani
        public ActionResult UpdateCellNoRetail()
        {
            List<GetCustomerID> data = _areaservice.GetAllRetCustomerID();

            foreach (var item in data)
            {
                var GetCellNoDetail = _areaservice.GetRetCellNoDetail(item.CustomerID);
                try
                {
                    _areaservice.UpdateRetCellNoDetaiLByCystomerID(item.CustomerID, GetCellNoDetail.CellNo, GetCellNoDetail.TelNo, GetCellNoDetail.Email);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return View();
        }

        // 21 Sep 2020 Piyush Limbani
        public ActionResult ManageTCS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageTCS(AddTCS data)
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
                    TCS_Mst obj = new TCS_Mst();
                    obj.TCSID = data.TCSID;
                    obj.TaxWithGST = data.TaxWithGST;
                    obj.TaxWithOutGST = data.TaxWithOutGST;
                    if (obj.TCSID == 0)
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
                    long respose = _areaservice.AddTCS(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageTCSList()
        {
            List<TCSListResponse> objModel = _areaservice.GetAllTCSList();
            return PartialView(objModel);
        }

        public ActionResult DeleteTCS(long? TCSID, bool IsDelete)
        {
            try
            {
                _areaservice.DeleteTCS(TCSID.Value, IsDelete);
                return RedirectToAction("ManageTCS");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageTCS");
            }
        }

        // 12 June,2021 Sonal Gandhi
        public ActionResult AddOnline(long? AreaID, bool IsOnline)
        {
            try
            {
                _areaservice.AddOnline(AreaID.Value, IsOnline);
                return RedirectToAction("ManageArea");
            }
            catch (Exception)
            {
                return RedirectToAction("ManageArea");
            }
        }

        public JsonResult GetAreaPincodeList(long AreaID)
        {
            try
            {
                List<AreaPincodeModel> AreaPincodelst = _areaservice.GetAreaPincodeList(AreaID);
                return Json(AreaPincodelst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }



        // 07 Feb,2022 Piyush Limbani
        [HttpPost]
        public JsonResult ResetUserPassword(long UserID, string UserName, string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            bool result = false;
            if (UserID != 0)
            {
                MembershipUser objUsr = Membership.GetUser(UserName);
                if (objUsr != null)
                {
                    result = objUsr.ChangePassword(OldPassword, NewPassword);
                    if (result != false)
                    {
                        long UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        bool respose = _areaservice.ResetUserPassword(UserID, NewPassword, UpdatedBy);
                        return Json(new
                        {
                            redirectUrl = Url.Action("ManageUsers", "Admin"),
                            isRedirect = false,
                            Oldpassmatch = true,
                            Passchange = result
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            redirectUrl = Url.Action("ManageUsers", "Admin"),
                            isRedirect = false,
                            Oldpassmatch = false,
                            Passchange = result
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("ManageUsers", "Admin"),
                        isRedirect = false,
                        Oldpassmatch = false,
                        Passchange = result
                    });
                }
            }
            else
            {
                return Json(new
                {
                    redirectUrl = Url.Action("ManageUsers", "Admin"),
                    isRedirect = true,
                    Oldpassmatch = false,
                    Passchange = false
                });
            }
        }




        //[HttpPost]
        //public ActionResult DeactiveUser(ApprovalLeave data)
        //{
        //    try
        //    {
        //        if (Request.Cookies["UserID"] == null)
        //        {
        //            Request.Cookies["UserID"].Value = null;
        //            return JavaScript("location.reload(true)");
        //        }
        //        else
        //        {
        //            long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
        //            bool respose = _IAttandanceService.UpdateApprovalLeave(data, UserID);
        //            return Json(respose, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}


    }
}