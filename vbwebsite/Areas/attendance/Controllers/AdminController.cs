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

namespace vbwebsite.Areas.attendance.Controllers
{
    public class AdminController : Controller
    {

        private IAdminService _adminservice;
        private ICommonService _ICommonService;
        private IAttandanceService _IAttandanceService;

        public AdminController(IAdminService adminservice, ICommonService commonservice, IAttandanceService attandanceservice)
        {
            _adminservice = adminservice;
            _ICommonService = commonservice;
            _IAttandanceService = attandanceservice;
        }

        //
        // GET: /attendance/Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllowanceDetail()
        {
            ViewBag.AllowanceStatus = _IAttandanceService.GetAllAllowanceStatusName();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            ViewBag.Customer = _IAttandanceService.GetAllVirakiEmployeeAsCustomerName();
            return View();
        }

        public JsonResult GetEmployeeDetailByEmployeeCode(long EmployeeCode)
        {
            try
            {
                // 05 Jan 2021 Piyush Limbani     
                EmployeeDetail detail = null;
                string MonthID = DateTime.Now.Month.ToString();
                int YearID = DateTime.Now.Year;
                long SYear = 0;
                string EYear = "";
                if (MonthID == "1" || MonthID == "2" || MonthID == "3")
                {
                    SYear = YearID - 1;
                    string StartDate = SYear + "-" + "04" + "-" + "01";
                    string EndDate = YearID + "-" + "03" + "-" + "31";
                    detail = _IAttandanceService.GetEmployeeDetailByEmployeeCode(StartDate, EndDate, EmployeeCode);
                }
                else
                {
                    SYear = YearID;
                    string StartDate = SYear + "-" + "04" + "-" + "01";
                    EYear = DateTime.Now.AddYears(1).Year.ToString();
                    string EndDate = EYear + "-" + "03" + "-" + "31";
                    detail = _IAttandanceService.GetEmployeeDetailByEmployeeCode(StartDate, EndDate, EmployeeCode);
                }
                // 05 Jan 2021 Piyush Limbani     


                //string syear = DateTime.Now.Year.ToString();
                //string StartDate = syear + "-" + "04" + "-" + "01";
                //string eyear = DateTime.Now.AddYears(1).Year.ToString();
                //string EndDate = eyear + "-" + "03" + "-" + "31";
                //var detail = _IAttandanceService.GetEmployeeDetailByEmployeeCode(StartDate, EndDate, EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddAllowanceDetail(AddAllowance data)
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
                    Allowance_Master Obj = new Allowance_Master();
                    Obj.AllowanceDetailID = data.AllowanceDetailID;
                    Obj.EmployeeCode = data.EmployeeCode;
                    Obj.OpeningDate = data.OpeningDate;
                    Obj.IncrementDate = data.IncrementDate;
                    Obj.DA1Date = data.DA1Date;
                    Obj.DA2Date = data.DA2Date;
                    Obj.OthersDate = data.OthersDate;
                    Obj.BasicAllowance1 = data.BasicAllowance1;
                    Obj.BasicAllowance2 = data.BasicAllowance2;
                    Obj.BasicAllowance3 = data.BasicAllowance3;
                    Obj.BasicAllowance4 = data.BasicAllowance4;
                    Obj.BasicAllowance5 = data.BasicAllowance5;
                    Obj.TotalBasicAllowance = data.TotalBasicAllowance;
                    Obj.HRAPercentage1 = data.HouseRentAllowancePercentage1;
                    Obj.HRAPercentage2 = data.HouseRentAllowancePercentage2;
                    Obj.HRAPercentage3 = data.HouseRentAllowancePercentage3;
                    Obj.HRAPercentage4 = data.HouseRentAllowancePercentage4;
                    Obj.HRAPercentage5 = data.HouseRentAllowancePercentage5;
                    Obj.HouseRentAllowance1 = data.HouseRentAllowance1;
                    Obj.HouseRentAllowance2 = data.HouseRentAllowance2;
                    Obj.HouseRentAllowance3 = data.HouseRentAllowance3;
                    Obj.HouseRentAllowance4 = data.HouseRentAllowance4;
                    Obj.HouseRentAllowance5 = data.HouseRentAllowance5;
                    Obj.TotalHouseRentAllowance = data.TotalHouseRentAllowance;
                    Obj.TotalWages1 = data.TotalWages1;
                    Obj.TotalWages2 = data.TotalWages2;
                    Obj.TotalWages3 = data.TotalWages3;
                    Obj.TotalWages4 = data.TotalWages4;
                    Obj.TotalWages5 = data.TotalWages5;
                    Obj.GrandTotalWages = data.GrandTotalWages;


                    Obj.Conveyance = data.Conveyance;
                    Obj.ConveyancePerDay = data.ConveyancePerDay;
                    Obj.VehicleAllowance = data.VehicleAllowance;
                    Obj.PerformanceAllowance = data.PerformanceAllowance;


                    Obj.PerformanceAllowanceStatusID = data.PerformanceAllowanceStatusID;
                    Obj.CityAllowanceStatusID = data.CityAllowanceStatusID;
                    Obj.PFStatusID = data.PFStatusID;
                    Obj.ESICStatusID = data.ESICStatusID;
                    Obj.BonusPercentage = data.BonusPercentage;
                    Obj.BonusAmount = data.BonusAmount;
                    Obj.BonusStatusID = data.BonusStatusID;
                    Obj.LeaveEnhancementPercentage = data.LeaveEnhancementPercentage;
                    Obj.LeaveEnhancementAmount = data.LeaveEnhancementAmount;
                    Obj.LeaveEnhancementStatusID = data.LeaveEnhancementStatusID;
                    Obj.GratuityPercentage = data.GratuityPercentage;
                    Obj.GratuityAmount = data.GratuityAmount;
                    Obj.GratuityStatusID = data.GratuityStatusID;
                    Obj.CustomerID = data.CustomerID;

                    //Obj.IsOldPF = data.IsOldPF;
                    //Obj.IsOldESIC = data.IsOldESIC;
                    //Obj.IsOldCityAllowance = data.IsOldCityAllowance;

                    if (data.AllowanceDetailID == 0)
                    {
                        Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        Obj.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        Obj.CreatedBy = data.CreatedBy;
                        Obj.CreatedOn = data.CreatedOn;
                    }
                    Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    Obj.UpdatedOn = DateTime.Now;
                    Obj.IsDelete = false;
                    bool respose = _IAttandanceService.AddAllowance(Obj);
                    // bool detail = _adminservice.AddAllowanceDetail(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ViewAllowanceList()
        {
            //string syear = DateTime.Now.Year.ToString();
            //string StartDate = syear + "-" + "04" + "-" + "01";
            //string eyear = DateTime.Now.AddYears(1).Year.ToString();
            //string EndDate = eyear + "-" + "03" + "-" + "31";
            //List<AllowanceListResponse> objModel = _IAttandanceService.GetAllowanceList(StartDate, EndDate);


            // 05 Jan 2021 Piyush Limbani     
            List<AllowanceListResponse> objModel = null;
            string MonthID = DateTime.Now.Month.ToString();
            int YearID = DateTime.Now.Year;
            long SYear = 0;
            string EYear = "";
            if (MonthID == "1" || MonthID == "2" || MonthID == "3")
            {
                SYear = YearID - 1;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                string EndDate = YearID + "-" + "03" + "-" + "31";
                objModel = _IAttandanceService.GetAllowanceList(StartDate, EndDate);
            }
            else
            {
                SYear = YearID;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                EYear = DateTime.Now.AddYears(1).Year.ToString();
                string EndDate = EYear + "-" + "03" + "-" + "31";
                objModel = _IAttandanceService.GetAllowanceList(StartDate, EndDate);
            }
            // 05 Jan 2021 Piyush Limbani     




            return PartialView(objModel);
        }

        public JsonResult GetOpeningAllowanceDetail(long EmployeeCode)
        {
            try
            {
                var detail = _IAttandanceService.GetOpeningAllowanceDetail(EmployeeCode);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteAllowanceDetail(long? AllowanceDetailID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteAllowanceDetail(AllowanceDetailID.Value, IsDelete);
                return RedirectToAction("AllowanceDetail");
            }
            catch (Exception)
            {
                return RedirectToAction("AllowanceDetail");
            }
        }

        public ActionResult AddSalarySheet()
        {
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        //14-04-2020
        public JsonResult GetTotalDaysMonthInTheMonth(int MonthID, int YearID)
        {
            try
            {
                var detail = _IAttandanceService.GetTotalDaysMonthInTheMonth(MonthID, YearID);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult GetEmployeeAttandanceDetail(int MonthID, int YearID, long EmployeeCode)
        //{
        //    try
        //    {
        //        //string syear = DateTime.Now.Year.ToString();
        //        //string StartDate = syear + "-" + "04" + "-" + "01";
        //        //string eyear = DateTime.Now.AddYears(1).Year.ToString();
        //        //string EndDate = eyear + "-" + "03" + "-" + "31";

        //        string MonthStartDate = YearID + "-" + MonthID + "-" + "01";

        //        //var detail = _IAttandanceService.GetEmployeeAttandanceDetail(MonthID, YearID, EmployeeCode, StartDate, EndDate, MonthStartDate);
        //        var detail = _IAttandanceService.GetEmployeeAttandanceDetail(MonthID, YearID, EmployeeCode, MonthStartDate);
        //        return Json(detail, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        // 07 Aug 2020 Piyush Limbani
        public JsonResult GetEmployeeAttandanceDetail(int MonthID, int YearID, long EmployeeCode)
        {
            try
            {
                GetEmployeeAttandanceDetail detail = null;
                SalaryExistModel exist = _IAttandanceService.CheckSalaryExist(MonthID, YearID, EmployeeCode);
                if (exist.SalarySheetID == 0)
                {
                    string MonthStartDate = YearID + "-" + MonthID + "-" + "01";
                    detail = _IAttandanceService.GetEmployeeAttandanceDetail(MonthID, YearID, EmployeeCode, MonthStartDate);
                }
                // 07 Aug 2020 Piyush Limbani
                else
                {
                    detail = _IAttandanceService.GetEmployeeAttandanceDetailBySalarySheetID(MonthID, YearID, EmployeeCode, exist.SalarySheetID);
                }
                // 07 Aug 2020 Piyush Limbani
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }



        // 24 July 2020 Piyush Limbani
        public JsonResult GetCalculateEmployeeSalary(long SalarySheetID, long EmployeeCode, int MonthID, int YearID, int TotalDays, int TotalMonthDay, int TotalPresent, int TotalAvailedLeaves)
        {
            try
            {

                var detail = _IAttandanceService.GetCalculateEmployeeSalary(SalarySheetID, EmployeeCode, MonthID, YearID, TotalDays, TotalMonthDay, TotalPresent, TotalAvailedLeaves);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", result2 = "", txtndcc = "", ID = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        // 24 July 2020 Piyush Limbani

        [HttpPost]
        public ActionResult AddSalarySheet(AddSalarySheet data)
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
                    SalaryExistModel exist = _IAttandanceService.CheckSalaryExist(data.MonthID, data.YearID, data.EmployeeCode);
                    // long SalarySheetID = _adminservice.CheckSalaryExist(data.MonthID, data.YearID, data.EmployeeCode);

                    if (exist.SalarySheetID == 0)
                    {
                        SalarySheet_Master Obj = new SalarySheet_Master();
                        Obj.SalarySheetID = data.SalarySheetID;
                        Obj.EmployeeCode = data.EmployeeCode;
                        Obj.MonthID = data.MonthID;
                        Obj.YearID = data.YearID;

                        Obj.Present = data.Present;
                        Obj.AdditionalPresent = data.AdditionalPresent;
                        Obj.TotalPresent = data.TotalPresent;

                        Obj.Sunday = data.Sunday;
                        Obj.AdditionalSunday = data.AdditionalSunday;
                        Obj.TotalSunday = data.TotalSunday;

                        Obj.Holiday = data.Holiday;
                        Obj.AdditionalHoliday = data.AdditionalHoliday;
                        Obj.TotalHoliday = data.TotalHoliday;

                        Obj.Absent = data.Absent;
                        Obj.AdditionalAbsent = data.AdditionalAbsent;
                        Obj.TotalAbsent = data.TotalAbsent;

                        Obj.TotalDays = data.TotalDays;
                        Obj.TotalDaysIntheMonth = data.TotalDaysIntheMonth;
                        Obj.BasicAllowance = data.BasicAllowance;
                        Obj.HouseRentAllowance = data.HouseRentAllowance;
                        Obj.TotalBasic = data.TotalBasic;
                        Obj.EarnedBasicWages = data.EarnedBasicWages;
                        Obj.EarnedHouseRentAllowance = data.EarnedHouseRentAllowance;
                        Obj.TotalEarnedWages = data.TotalEarnedWages;
                        Obj.CityAllowanceMinutes = data.CityAllowanceMinutes;

                        // 31 May 2023 Dhruvik
                        Obj.AdditionalCityAllowanceMinutes = data.AdditionalCityAllowanceMinutes;
                        // 31 May 2023 Dhruvik

                        Obj.CityAllowanceHours = data.CityAllowanceHours;

                        Obj.CityAllowance = data.CityAllowance;
                        Obj.AdditionalCityAllowance = data.AdditionalCityAllowance;
                        Obj.TotalCityAllowance = data.TotalCityAllowance;

                        Obj.VehicleAllowance = data.VehicleAllowance;
                        Obj.AdditionalVehicleAllowance = data.AdditionalVehicleAllowance;
                        Obj.TotalVehicleAllowance = data.TotalVehicleAllowance;

                        Obj.Conveyance = data.Conveyance;
                        Obj.AdditionalConveyance = data.AdditionalConveyance;
                        Obj.TotalConveyance = data.TotalConveyance;

                        Obj.PerformanceAllowance = data.PerformanceAllowance;
                        Obj.AdditionalPerformanceAllowance = data.AdditionalPerformanceAllowance;
                        Obj.TotalPerformanceAllowance = data.TotalPerformanceAllowance;

                        Obj.GrossWagesPayable = data.GrossWagesPayable;
                        Obj.PF = data.PF;
                        Obj.ESIC = data.ESIC;
                        Obj.PT = data.PT;
                        Obj.MLWF = data.MLWF;
                        Obj.TotalDeductions = data.TotalDeductions;
                        Obj.NetWagesPaid = data.NetWagesPaid;
                        Obj.OpeningLeaves = data.OpeningLeaves;
                        Obj.EarnedLeaves = data.EarnedLeaves;

                        Obj.AvailedLeaves = data.AvailedLeaves;
                        Obj.AdditionalAvailedLeaves = data.AdditionalAvailedLeaves;
                        Obj.TotalAvailedLeaves = data.TotalAvailedLeaves;

                        Obj.ClosingLeaves = data.ClosingLeaves;
                        Obj.AdditionalClosingLeaves = data.AdditionalClosingLeaves;
                        Obj.TotalClosingLeaves = data.TotalClosingLeaves;

                        Obj.OpeningAdvance = data.OpeningAdvance;
                        Obj.Addition = data.Addition;
                        Obj.Deductions = data.Deductions;
                        Obj.ClosingAdvance = data.ClosingAdvance;
                        Obj.TDS = data.TDS;
                        Obj.Goods = data.Goods;
                        Obj.AnyOtherDeductions1 = data.AnyOtherDeductions1;
                        Obj.AnyOtherDeductions2 = data.AnyOtherDeductions2;

                        if (Obj.SalarySheetID == 0)
                        {
                            Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            Obj.CreatedOn = DateTime.Now;
                        }
                        else
                        {
                            Obj.CreatedBy = data.CreatedBy;
                            Obj.CreatedOn = data.CreatedOn;
                        }
                        Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        Obj.UpdatedOn = DateTime.Now;
                        Obj.IsDelete = true;
                        long respose = _IAttandanceService.AddSalarySheet(Obj);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        SalarySheet_Master Obj = new SalarySheet_Master();
                        Obj.SalarySheetID = exist.SalarySheetID;
                        Obj.EmployeeCode = data.EmployeeCode;
                        Obj.MonthID = data.MonthID;
                        Obj.YearID = data.YearID;

                        Obj.Present = data.Present;
                        Obj.AdditionalPresent = data.AdditionalPresent;
                        Obj.TotalPresent = data.TotalPresent;

                        Obj.Sunday = data.Sunday;
                        Obj.AdditionalSunday = data.AdditionalSunday;
                        Obj.TotalSunday = data.TotalSunday;

                        Obj.Holiday = data.Holiday;
                        Obj.AdditionalHoliday = data.AdditionalHoliday;
                        Obj.TotalHoliday = data.TotalHoliday;

                        Obj.Absent = data.Absent;
                        Obj.AdditionalAbsent = data.AdditionalAbsent;
                        Obj.TotalAbsent = data.TotalAbsent;

                        Obj.TotalDays = data.TotalDays;
                        Obj.TotalDaysIntheMonth = data.TotalDaysIntheMonth;
                        Obj.BasicAllowance = data.BasicAllowance;
                        Obj.HouseRentAllowance = data.HouseRentAllowance;
                        Obj.TotalBasic = data.TotalBasic;
                        Obj.EarnedBasicWages = data.EarnedBasicWages;
                        Obj.EarnedHouseRentAllowance = data.EarnedHouseRentAllowance;
                        Obj.TotalEarnedWages = data.TotalEarnedWages;
                        Obj.CityAllowanceMinutes = data.CityAllowanceMinutes;

                        // 31 May 2023 Dhruvik
                        Obj.AdditionalCityAllowanceMinutes = data.AdditionalCityAllowanceMinutes;
                        // 31 May 2023 Dhruvik

                        Obj.CityAllowanceHours = data.CityAllowanceHours;

                        Obj.CityAllowance = data.CityAllowance;
                        Obj.AdditionalCityAllowance = data.AdditionalCityAllowance;
                        Obj.TotalCityAllowance = data.TotalCityAllowance;

                        Obj.VehicleAllowance = data.VehicleAllowance;
                        Obj.AdditionalVehicleAllowance = data.AdditionalVehicleAllowance;
                        Obj.TotalVehicleAllowance = data.TotalVehicleAllowance;

                        Obj.Conveyance = data.Conveyance;
                        Obj.AdditionalConveyance = data.AdditionalConveyance;
                        Obj.TotalConveyance = data.TotalConveyance;

                        Obj.PerformanceAllowance = data.PerformanceAllowance;
                        Obj.AdditionalPerformanceAllowance = data.AdditionalPerformanceAllowance;
                        Obj.TotalPerformanceAllowance = data.TotalPerformanceAllowance;

                        Obj.GrossWagesPayable = data.GrossWagesPayable;
                        Obj.PF = data.PF;
                        Obj.ESIC = data.ESIC;
                        Obj.PT = data.PT;
                        Obj.MLWF = data.MLWF;
                        Obj.TotalDeductions = data.TotalDeductions;
                        Obj.NetWagesPaid = data.NetWagesPaid;
                        Obj.OpeningLeaves = data.OpeningLeaves;
                        Obj.EarnedLeaves = data.EarnedLeaves;

                        Obj.AvailedLeaves = data.AvailedLeaves;
                        Obj.AdditionalAvailedLeaves = data.AdditionalAvailedLeaves;
                        Obj.TotalAvailedLeaves = data.TotalAvailedLeaves;

                        Obj.ClosingLeaves = data.ClosingLeaves;
                        Obj.AdditionalClosingLeaves = data.AdditionalClosingLeaves;
                        Obj.TotalClosingLeaves = data.TotalClosingLeaves;

                        Obj.OpeningAdvance = data.OpeningAdvance;
                        Obj.Addition = data.Addition;
                        Obj.Deductions = data.Deductions;
                        Obj.ClosingAdvance = data.ClosingAdvance;
                        Obj.TDS = data.TDS;
                        Obj.Goods = data.Goods;
                        Obj.AnyOtherDeductions1 = data.AnyOtherDeductions1;
                        Obj.AnyOtherDeductions2 = data.AnyOtherDeductions2;
                        if (Obj.SalarySheetID == 0)
                        {
                            Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            Obj.CreatedOn = DateTime.Now;
                        }
                        else
                        {
                            Obj.CreatedBy = exist.CreatedBy;
                            Obj.CreatedOn = exist.CreatedOn;
                        }
                        //Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        //Obj.CreatedOn = DateTime.Now;
                        Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        Obj.UpdatedOn = DateTime.Now;
                        Obj.IsDelete = true;
                        long respose = _IAttandanceService.AddSalarySheet(Obj);
                        return Json(respose, JsonRequestBehavior.AllowGet);
                        //return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchSalarySheetList()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewSalarySheetList(SearchSalarySheet model)
        {
            //List<SalarySheetListResponse> objModel = _IAttandanceService.GetSalarySheetList(model);
            //return PartialView(objModel);

            // 01 July 2020 Piyush Limbani
            decimal sumBasicAllowance = 0;
            decimal sumHouseRentAllowance = 0;
            decimal sumEarnedBasicWages = 0;
            decimal sumEarnedHouseRentAllowance = 0;
            decimal sumTotalEarnedWages = 0;
            decimal sumCityAllowance = 0;
            decimal sumVehicleAllowance = 0;
            decimal sumConveyance = 0;
            decimal sumPerformanceAllowance = 0;
            decimal sumGrossWagesPayable = 0;
            decimal sumPF = 0;
            decimal sumPT = 0;
            decimal sumESIC = 0;
            decimal sumMLWF = 0;
            decimal sumNetWagesPaid = 0;

            List<SalarySheetListResponse> objModel = null;
            List<SalarySheetListResponse> objFinalList = new List<SalarySheetListResponse>();

            objModel = _IAttandanceService.GetSalarySheetList(model);
            foreach (var record in objModel)
            {
                objFinalList.Add(record);
                sumBasicAllowance += record.BasicAllowance;
                sumHouseRentAllowance += record.HouseRentAllowance;
                sumEarnedBasicWages += record.EarnedBasicWages;
                sumEarnedHouseRentAllowance += record.EarnedHouseRentAllowance;
                sumTotalEarnedWages += record.TotalEarnedWages;
                sumCityAllowance += record.CityAllowance;
                sumVehicleAllowance += record.VehicleAllowance;
                sumConveyance += record.Conveyance;
                sumPerformanceAllowance += record.PerformanceAllowance;
                sumGrossWagesPayable += record.GrossWagesPayable;
                sumPF += record.PF;
                sumPT += record.PT;
                sumESIC += record.ESIC;
                sumMLWF += record.MLWF;
                sumNetWagesPaid += record.NetWagesPaid;
            }

            if (sumBasicAllowance != 0)
            {
                objFinalList[0].sumBasicAllowance = sumBasicAllowance;
            }
            if (sumHouseRentAllowance != 0)
            {
                objFinalList[0].sumHouseRentAllowance = sumHouseRentAllowance;
            }
            if (sumEarnedBasicWages != 0)
            {
                objFinalList[0].sumEarnedBasicWages = sumEarnedBasicWages;
            }
            if (sumEarnedHouseRentAllowance != 0)
            {
                objFinalList[0].sumEarnedHouseRentAllowance = sumEarnedHouseRentAllowance;
            }
            if (sumTotalEarnedWages != 0)
            {
                objFinalList[0].sumTotalEarnedWages = sumTotalEarnedWages;
            }
            if (sumCityAllowance != 0)
            {
                objFinalList[0].sumCityAllowance = sumCityAllowance;
            }
            if (sumVehicleAllowance != 0)
            {
                objFinalList[0].sumVehicleAllowance = sumVehicleAllowance;
            }
            if (sumConveyance != 0)
            {
                objFinalList[0].sumConveyance = sumConveyance;
            }
            if (sumPerformanceAllowance != 0)
            {
                objFinalList[0].sumPerformanceAllowance = sumPerformanceAllowance;
            }
            if (sumGrossWagesPayable != 0)
            {
                objFinalList[0].sumGrossWagesPayable = sumGrossWagesPayable;
            }
            if (sumPF != 0)
            {
                objFinalList[0].sumPF = sumPF;
            }
            if (sumPT != 0)
            {
                objFinalList[0].sumPT = sumPT;
            }
            if (sumESIC != 0)
            {
                objFinalList[0].sumESIC = sumESIC;
            }
            if (sumMLWF != 0)
            {
                objFinalList[0].sumMLWF = sumMLWF;
            }
            if (sumNetWagesPaid != 0)
            {
                objFinalList[0].sumNetWagesPaid = sumNetWagesPaid;
            }
            return PartialView(objFinalList);
            // 01 July 2020 Piyush Limbani
        }

        public ActionResult ExportExcelSalarySheet(long MonthID, long YearID, long GodownID, long EmployeeCode)
        {
            int sumPresent = 0;
            int sumSunday = 0;
            int sumHoliday = 0;
            int sumAbsent = 0;
            int sumTotalDays = 0;
            decimal sumBasicAllowance = 0;
            decimal sumHouseRentAllowance = 0;
            decimal sumEarnedBasicWages = 0;
            decimal sumEarnedHouseRentAllowance = 0;
            decimal sumTotalEarnedWages = 0;

            // decimal sumCityAllowanceHours = 0;

            decimal sumCityAllowanceMinutes = 0;
            decimal sumCityAllowance = 0;
            decimal sumVehicleAllowance = 0;
            decimal sumConveyance = 0;
            decimal sumPerformanceAllowance = 0;
            decimal sumGrossWagesPayable = 0;
            decimal sumPF = 0;
            decimal sumESIC = 0;
            decimal sumPT = 0;
            decimal sumMLWF = 0;
            decimal sumTotalDeductions = 0;
            decimal sumNetWagesPaid = 0;
            decimal sumOpeningLeaves = 0;
            decimal sumEarnedLeaves = 0;
            decimal sumAvailedLeaves = 0;
            decimal sumClosingLeaves = 0;
            decimal sumNetWagesPaid2 = 0;
            decimal sumOpeningAdvance = 0;
            decimal sumAddition = 0;
            decimal sumDeductions = 0;
            decimal sumClosingAdvance = 0;
            decimal sumTDS = 0;
            decimal sumGoods = 0;
            decimal sumNetWagesToPay = 0;

            var objlst = _IAttandanceService.GetExportExcelSalarySheet(MonthID, YearID, GodownID, EmployeeCode);
            List<SalarySheetListResponse> objFinalList = new List<SalarySheetListResponse>();

            foreach (var record in objlst)
            {
                objFinalList.Add(record);
                sumPresent += record.Present;
                sumSunday += record.Sunday;
                sumHoliday += record.Holiday;
                sumAbsent += record.Absent;
                sumTotalDays += record.TotalDays;
                sumBasicAllowance += record.BasicAllowance;
                sumHouseRentAllowance += record.HouseRentAllowance;
                sumEarnedBasicWages += record.EarnedBasicWages;
                sumEarnedHouseRentAllowance += record.EarnedHouseRentAllowance;
                sumTotalEarnedWages += record.TotalEarnedWages;

                //sumCityAllowanceHours += record.CityAllowanceHours;

                sumCityAllowanceMinutes += record.CityAllowanceMinutes;
                sumCityAllowance += record.CityAllowance;
                sumVehicleAllowance += record.VehicleAllowance;
                sumConveyance += record.Conveyance;
                sumPerformanceAllowance += record.PerformanceAllowance;
                sumGrossWagesPayable += record.GrossWagesPayable;
                sumPF += record.PF;
                sumESIC += record.ESIC;
                sumPT += record.PT;
                sumMLWF += record.MLWF;
                sumTotalDeductions += record.TotalDeductions;
                sumNetWagesPaid += record.NetWagesPaid;
                sumOpeningLeaves += record.OpeningLeaves;
                sumEarnedLeaves += record.EarnedLeaves;
                sumAvailedLeaves += record.AvailedLeaves;
                sumClosingLeaves += record.ClosingLeaves;
                sumNetWagesPaid2 += record.NetWagesPaid;
                sumOpeningAdvance += record.OpeningAdvance;
                sumAddition += record.Addition;
                sumDeductions += record.Deductions;
                sumClosingAdvance += record.ClosingAdvance;
                sumTDS += record.TDS;
                sumGoods += record.Goods;
                sumNetWagesToPay += record.NetWagesToPay;
            }

            List<SalarySheetListExport> lstsalary = objFinalList.Select(x => new SalarySheetListExport()
            {
                SrNo = x.RowNumber,
                Name = x.EmployeeName,
                Age = x.Agestr,
                Sex = x.Sex,
                Designation = x.Designation,
                DateofJoining = x.DateofJoiningstr,
                WorkingHours = x.WorkingHours,
                IntervalForRest = x.IntervalForRest,
                Present = x.Present,
                Sunday = x.Sunday,
                Holiday = x.Holiday,
                Absent = x.Absent,
                TotalDays = x.TotalDays,
                BasicAllowance = x.BasicAllowance,
                HouseRentAllowance = x.HouseRentAllowance,
                EarnedBasicWages = x.EarnedBasicWages,
                EarnedHouseRentAllowance = x.EarnedHouseRentAllowance,
                TotalEarnedWages = x.TotalEarnedWages,

                //CityAllowanceHours = x.CityAllowanceHours,

                CityAllowanceMinutes = x.CityAllowanceMinutes,
                CityAllowance = x.CityAllowance,
                VehicleAllowance = x.VehicleAllowance,
                Conveyance = x.Conveyance,
                PerformanceAllowance = x.PerformanceAllowance,
                GrossWagesPayable = x.GrossWagesPayable,
                PF = x.PF,
                ESIC = x.ESIC,
                PT = x.PT,
                MLWF = x.MLWF,
                TotalDeductions = x.TotalDeductions,
                NetWagesPaid = x.NetWagesPaid,
                OpeningLeaves = x.OpeningLeaves,
                EarnedLeaves = x.EarnedLeaves,
                AvailedLeaves = x.AvailedLeaves,
                ClosingLeaves = x.ClosingLeaves,
                Sign = x.Sign,
                NetWagesPaid2 = x.NetWagesPaid,
                OpeningAdvance = x.OpeningAdvance,
                Addition = x.Addition,
                Deductions = x.Deductions,
                ClosingAdvance = x.ClosingAdvance,
                TDS = x.TDS,
                Goods = x.Goods,
                NetWagesToPay = x.NetWagesToPay,
                CustomerName = x.CustomerName,
            }).ToList();

            DataTable ds = new DataTable();
            ds = ToDataTable(lstsalary);
            DataRow row = ds.NewRow();
            row["SrNo"] = 0;
            row["Name"] = "";
            row["Age"] = "";
            row["Sex"] = "";
            row["Designation"] = "";
            row["DateofJoining"] = "";
            row["WorkingHours"] = "";
            row["IntervalForRest"] = "";
            row["Present"] = 0;
            row["Sunday"] = 0;
            row["Holiday"] = 0;
            row["Absent"] = 0;
            row["TotalDays"] = 0;
            row["BasicAllowance"] = 0;
            row["HouseRentAllowance"] = 0;
            row["EarnedBasicWages"] = 0;
            row["EarnedHouseRentAllowance"] = 0;
            row["TotalEarnedWages"] = 0;

            //row["CityAllowanceHours"] = 0;

            row["CityAllowanceMinutes"] = 0;
            row["CityAllowance"] = 0;
            row["VehicleAllowance"] = 0;
            row["Conveyance"] = 0;
            row["PerformanceAllowance"] = 0;
            row["GrossWagesPayable"] = 0;
            row["PF"] = 0;
            row["ESIC"] = 0;
            row["PT"] = 0;
            row["MLWF"] = 0;
            row["TotalDeductions"] = 0;
            row["NetWagesPaid"] = 0;
            row["OpeningLeaves"] = 0;
            row["EarnedLeaves"] = 0;
            row["AvailedLeaves"] = 0;
            row["ClosingLeaves"] = 0;
            row["Sign"] = "";
            row["NetWagesPaid2"] = 0;
            row["OpeningAdvance"] = 0;
            row["Addition"] = 0;
            row["Deductions"] = 0;
            row["ClosingAdvance"] = 0;
            row["TDS"] = 0;
            row["Goods"] = 0;
            row["NetWagesToPay"] = 0;
            row["CustomerName"] = "";
            ds.Rows.InsertAt(row, 0);

            DataRow row1 = ds.NewRow();
            row1["SrNo"] = 0;
            row1["Name"] = "";
            row1["Age"] = "";
            row1["Sex"] = "";
            row1["Designation"] = "";
            row1["DateofJoining"] = "";
            row1["WorkingHours"] = "";
            row1["IntervalForRest"] = "";
            row1["Present"] = 0;
            row1["Sunday"] = 0;
            row1["Holiday"] = 0;
            row1["Absent"] = 0;
            row1["TotalDays"] = 0;
            row1["BasicAllowance"] = 0;
            row1["HouseRentAllowance"] = 0;
            row1["EarnedBasicWages"] = 0;
            row1["EarnedHouseRentAllowance"] = 0;
            row1["TotalEarnedWages"] = 0;

            //row1["CityAllowanceHours"] = 0;

            row1["CityAllowanceMinutes"] = 0;
            row1["CityAllowance"] = 0;
            row1["VehicleAllowance"] = 0;
            row1["Conveyance"] = 0;
            row1["PerformanceAllowance"] = 0;
            row1["GrossWagesPayable"] = 0;
            row1["PF"] = 0;
            row1["ESIC"] = 0;
            row1["PT"] = 0;
            row1["MLWF"] = 0;
            row1["TotalDeductions"] = 0;
            row1["NetWagesPaid"] = 0;
            row1["OpeningLeaves"] = 0;
            row1["EarnedLeaves"] = 0;
            row1["AvailedLeaves"] = 0;
            row1["ClosingLeaves"] = 0;
            row1["Sign"] = "";
            row1["NetWagesPaid2"] = 0;
            row1["OpeningAdvance"] = 0;
            row1["Addition"] = 0;
            row1["Deductions"] = 0;
            row1["ClosingAdvance"] = 0;
            row1["TDS"] = 0;
            row1["Goods"] = 0;
            row1["NetWagesToPay"] = 0;
            row1["CustomerName"] = "";
            ds.Rows.InsertAt(row1, 0);

            DataRow row2 = ds.NewRow();
            row2["SrNo"] = 0;
            row2["Name"] = "";
            row2["Age"] = "";
            row2["Sex"] = "";
            row2["Designation"] = "";
            row2["DateofJoining"] = "";
            row2["WorkingHours"] = "";
            row2["IntervalForRest"] = "";
            row2["Present"] = 0;
            row2["Sunday"] = 0;
            row2["Holiday"] = 0;
            row2["Absent"] = 0;
            row2["TotalDays"] = 0;
            row2["BasicAllowance"] = 0;
            row2["HouseRentAllowance"] = 0;
            row2["EarnedBasicWages"] = 0;
            row2["EarnedHouseRentAllowance"] = 0;
            row2["TotalEarnedWages"] = 0;

            //row2["CityAllowanceHours"] = 0;

            row2["CityAllowanceMinutes"] = 0;
            row2["CityAllowance"] = 0;
            row2["VehicleAllowance"] = 0;
            row2["Conveyance"] = 0;
            row2["PerformanceAllowance"] = 0;
            row2["GrossWagesPayable"] = 0;
            row2["PF"] = 0;
            row2["ESIC"] = 0;
            row2["PT"] = 0;
            row2["MLWF"] = 0;
            row2["TotalDeductions"] = 0;
            row2["NetWagesPaid"] = 0;
            row2["OpeningLeaves"] = 0;
            row2["EarnedLeaves"] = 0;
            row2["AvailedLeaves"] = 0;
            row2["ClosingLeaves"] = 0;
            row2["Sign"] = "";
            row2["NetWagesPaid2"] = 0;
            row2["OpeningAdvance"] = 0;
            row2["Addition"] = 0;
            row2["Deductions"] = 0;
            row2["ClosingAdvance"] = 0;
            row2["TDS"] = 0;
            row2["Goods"] = 0;
            row2["NetWagesToPay"] = 0;
            row2["CustomerName"] = "";
            ds.Rows.InsertAt(row2, 0);

            DataRow row3 = ds.NewRow();
            row3["SrNo"] = 0;
            row3["Name"] = "";
            row3["Age"] = "";
            row3["Sex"] = "";
            row3["Designation"] = "";
            row3["DateofJoining"] = "";
            row3["WorkingHours"] = "";
            row3["IntervalForRest"] = "";
            row3["Present"] = 0;
            row3["Sunday"] = 0;
            row3["Holiday"] = 0;
            row3["Absent"] = 0;
            row3["TotalDays"] = 0;
            row3["BasicAllowance"] = 0;
            row3["HouseRentAllowance"] = 0;
            row3["EarnedBasicWages"] = 0;
            row3["EarnedHouseRentAllowance"] = 0;
            row3["TotalEarnedWages"] = 0;

            //row3["CityAllowanceHours"] = 0;

            row3["CityAllowanceMinutes"] = 0;
            row3["CityAllowance"] = 0;
            row3["VehicleAllowance"] = 0;
            row3["Conveyance"] = 0;
            row3["PerformanceAllowance"] = 0;
            row3["GrossWagesPayable"] = 0;
            row3["PF"] = 0;
            row3["ESIC"] = 0;
            row3["PT"] = 0;
            row3["MLWF"] = 0;
            row3["TotalDeductions"] = 0;
            row3["NetWagesPaid"] = 0;
            row3["OpeningLeaves"] = 0;
            row3["EarnedLeaves"] = 0;
            row3["AvailedLeaves"] = 0;
            row3["ClosingLeaves"] = 0;
            row3["Sign"] = "";
            row3["NetWagesPaid2"] = 0;
            row3["OpeningAdvance"] = 0;
            row3["Addition"] = 0;
            row3["Deductions"] = 0;
            row3["ClosingAdvance"] = 0;
            row3["TDS"] = 0;
            row3["Goods"] = 0;
            row3["NetWagesToPay"] = 0;
            row3["CustomerName"] = "";
            ds.Rows.InsertAt(row3, 0);

            DataRow row4 = ds.NewRow();
            row4["SrNo"] = 0;
            row4["Name"] = "";
            row4["Age"] = "";
            row4["Sex"] = "";
            row4["Designation"] = "";
            row4["DateofJoining"] = "";
            row4["WorkingHours"] = "";
            row4["IntervalForRest"] = "";
            row4["Present"] = 0;
            row4["Sunday"] = 0;
            row4["Holiday"] = 0;
            row4["Absent"] = 0;
            row4["TotalDays"] = 0;
            row4["BasicAllowance"] = 0;
            row4["HouseRentAllowance"] = 0;
            row4["EarnedBasicWages"] = 0;
            row4["EarnedHouseRentAllowance"] = 0;
            row4["TotalEarnedWages"] = 0;

            //row4["CityAllowanceHours"] = 0;

            row4["CityAllowanceMinutes"] = 0;
            row4["CityAllowance"] = 0;
            row4["VehicleAllowance"] = 0;
            row4["Conveyance"] = 0;
            row4["PerformanceAllowance"] = 0;
            row4["GrossWagesPayable"] = 0;
            row4["PF"] = 0;
            row4["ESIC"] = 0;
            row4["PT"] = 0;
            row4["MLWF"] = 0;
            row4["TotalDeductions"] = 0;
            row4["NetWagesPaid"] = 0;
            row4["OpeningLeaves"] = 0;
            row4["EarnedLeaves"] = 0;
            row4["AvailedLeaves"] = 0;
            row4["ClosingLeaves"] = 0;
            row4["Sign"] = "";
            row4["NetWagesPaid2"] = 0;
            row4["OpeningAdvance"] = 0;
            row4["Addition"] = 0;
            row4["Deductions"] = 0;
            row4["ClosingAdvance"] = 0;
            row4["TDS"] = 0;
            row4["Goods"] = 0;
            row4["NetWagesToPay"] = 0;
            row4["CustomerName"] = "";
            ds.Rows.InsertAt(row4, 0);

            //DataRow row5 = ds.NewRow();
            //row5["SrNo"] = 0;
            //row5["Name"] = "";
            //row5["Age"] = "";
            //row5["Sex"] = "";
            //row5["Designation"] = "";
            //row5["DateofJoining"] = "";
            //row5["WorkingHours"] = "";
            //row5["IntervalForRest"] = "";
            //row5["Present"] = 0;
            //row5["Sunday"] = 0;
            //row5["Holiday"] = 0;
            //row5["Absent"] = 0;
            //row5["TotalDays"] = 0;
            //row5["BasicAllowance"] = 0;
            //row5["HouseRentAllowance"] = 0;
            //row5["EarnedBasicWages"] = 0;
            //row5["EarnedHouseRentAllowance"] = 0;
            //row5["TotalEarnedWages"] = 0;
            //row5["CityAllowanceHours"] = 0;
            //row5["CityAllowance"] = 0;
            //row5["VehicleAllowance"] = 0;
            //row5["Conveyance"] = 0;
            //row5["PerformanceAllowance"] = 0;
            //row5["GrossWagesPayable"] = 0;
            //row5["PF"] = 0;
            //row5["PT"] = 0;
            //row5["MLWF"] = 0;
            //row5["TotalDeductions"] = 0;
            //row5["NetWagesPaid"] = 0;
            //row5["OpeningLeaves"] = 0;
            //row5["EarnedLeaves"] = 0;
            //row5["AvailedLeaves"] = 0;
            //row5["ClosingLeaves"] = 0;
            //row5["Sign"] = "";
            //row5["NetWagesPaid2"] = 0;
            //row5["OpeningAdvance"] = 0;
            //row5["Addition"] = 0;
            //row5["Deductions"] = 0;
            //row5["ClosingAdvance"] = 0;
            //row5["TDS"] = 0;
            //row5["Goods"] = 0;
            //row5["NetWagesToPay"] = 0;
            //ds.Rows.InsertAt(row5, 0);


            DataRow row5 = ds.NewRow();
            row5["SrNo"] = objlst.Count + 1;
            row5["Name"] = "";
            row5["Age"] = "";
            row5["Sex"] = "";
            row5["Designation"] = "";
            row5["DateofJoining"] = "";
            row5["WorkingHours"] = "";
            row5["IntervalForRest"] = "";
            row5["Present"] = sumPresent;
            row5["Sunday"] = sumSunday;
            row5["Holiday"] = sumHoliday;
            row5["Absent"] = sumAbsent;
            row5["TotalDays"] = sumTotalDays;
            row5["BasicAllowance"] = sumBasicAllowance;
            row5["HouseRentAllowance"] = sumHouseRentAllowance;
            row5["EarnedBasicWages"] = sumEarnedBasicWages;
            row5["EarnedHouseRentAllowance"] = sumEarnedHouseRentAllowance;
            row5["TotalEarnedWages"] = sumTotalEarnedWages;

            // row5["CityAllowanceHours"] = sumCityAllowanceHours;

            row5["CityAllowanceMinutes"] = sumCityAllowanceMinutes;
            row5["CityAllowance"] = sumCityAllowance;
            row5["VehicleAllowance"] = sumVehicleAllowance;
            row5["Conveyance"] = sumConveyance;
            row5["PerformanceAllowance"] = sumPerformanceAllowance;
            row5["GrossWagesPayable"] = sumGrossWagesPayable;
            row5["PF"] = sumPF;
            row5["ESIC"] = sumESIC;
            row5["PT"] = sumPT;
            row5["MLWF"] = sumMLWF;
            row5["TotalDeductions"] = sumTotalDeductions;
            row5["NetWagesPaid"] = sumNetWagesPaid;
            row5["OpeningLeaves"] = sumOpeningLeaves;
            row5["EarnedLeaves"] = sumEarnedLeaves;
            row5["AvailedLeaves"] = sumAvailedLeaves;
            row5["ClosingLeaves"] = sumClosingLeaves;
            row5["Sign"] = "";
            row5["NetWagesPaid2"] = sumNetWagesPaid2;
            row5["OpeningAdvance"] = sumOpeningAdvance;
            row5["Addition"] = sumAddition;
            row5["Deductions"] = sumDeductions;
            row5["ClosingAdvance"] = sumClosingAdvance;
            row5["TDS"] = sumTDS;
            row5["Goods"] = sumGoods;
            row5["NetWagesToPay"] = sumNetWagesToPay;
            row5["CustomerName"] = "";
            ds.Rows.InsertAt(row5, 0);

            DataView dv = ds.DefaultView;
            dv.Sort = "SrNo asc";
            DataTable sortedDT = dv.ToTable();
            ds = sortedDT;

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(ds);
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                ws.Cell("A1").Value = "";
                ws.Cell("B1").Value = "VIRAKI BROTHERS : 1ST FLOOR, HAFED WAREHOUSE, PLOT NO. 39, SECTOR - 18, VASHI, NAVI MUMBAI - 400705";
                ws.Range("B1:AR1").Row(1).Merge();

                ws.Cell("A2").Value = "";
                ws.Cell("B2").Value = "WAGES REGISTER   FORM II   SEE RULE 27(1)";
                ws.Range("B2:AR2").Row(1).Merge();

                ws.Cell("A3").Value = "";
                ws.Cell("C3").Value = "";
                ws.Range("I3:AR3").Value = "";

                ws.Cell("A4").Value = "";
                ws.Cell("C4").Value = "";
                ws.Range("I4:AR4").Value = "";

                ws.Cell("A5").Value = "";
                ws.Cell("C5").Value = "";
                ws.Range("I5:AR5").Value = "";

                //ws.Cell("A6").Value = "";
                //ws.Cell("C6").Value = "";
                //ws.Range("I6:AQ6").Value = "";

                ws.Cell("A6").Value = "Sr No";
                ws.Cell("B6").Value = "Name";
                ws.Cell("C6").Value = "Age";
                ws.Cell("D6").Value = "Sex";
                ws.Cell("E6").Value = "Designation";
                ws.Cell("F6").Value = "Date of Joining";
                ws.Cell("G6").Value = "Working Hours";
                ws.Cell("H6").Value = "Interval For Rest";
                ws.Cell("I6").Value = "Present";
                ws.Cell("J6").Value = "Sunday";
                ws.Cell("K6").Value = "Holiday";
                ws.Cell("L6").Value = "Absent";
                ws.Cell("M6").Value = "TotalDays";
                ws.Cell("N6").Value = "Basic Allowance";
                ws.Cell("O6").Value = "House Rent Allowance";
                ws.Cell("P6").Value = "Earned Basic Wages";
                ws.Cell("Q6").Value = "Earned House Rent Allowance";
                ws.Cell("R6").Value = "Total Earned Wages";

                //ws.Cell("S6").Value = "City Allowance Hours";

                ws.Cell("S6").Value = "City Allowance Minutes";
                ws.Cell("T6").Value = "City Allowance";
                ws.Cell("U6").Value = "Vehicle Allowance";
                ws.Cell("V6").Value = "Conveyance";
                ws.Cell("W6").Value = "Performance Allowance";
                ws.Cell("X6").Value = "Gross Wages Payable";
                ws.Cell("Y6").Value = "PF";
                ws.Cell("Z6").Value = "ESIC";
                ws.Cell("AA6").Value = "PT";
                ws.Cell("AB6").Value = "MLWF";
                ws.Cell("AC6").Value = "Total Deductions";
                ws.Cell("AD6").Value = "Net Wages Paid";
                ws.Cell("AE6").Value = "Opening Leaves";
                ws.Cell("AF6").Value = "Earned Leaves";
                ws.Cell("AG6").Value = "Availed Leaves";
                ws.Cell("AH6").Value = "Closing Leaves";
                ws.Cell("AI6").Value = "Sign";
                ws.Cell("AJ6").Value = "Net Wages Paid";
                ws.Cell("AK6").Value = "Opening Advance";
                ws.Cell("AL6").Value = "Addition";
                ws.Cell("AM6").Value = "Deductions";
                ws.Cell("AN6").Value = "Closing Advance";
                ws.Cell("AO6").Value = "TDS";
                ws.Cell("AP6").Value = "Goods";
                ws.Cell("AQ6").Value = "Net Wages To Pay";
                ws.Cell("AR6").Value = "Customer Name";
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "SalarySheet.xlsx");

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

        //17-04-2020
        public ActionResult AddFestival()
        {
            ViewBag.Event = _adminservice.GetAllEventName();
            return View();
        }

        [HttpPost]
        public ActionResult AddFestival(AddFestival data)
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
                    Festival_Mst obj = new Festival_Mst();
                    obj.FestivalID = data.FestivalID;
                    obj.EventID = data.EventID;
                    obj.FestivalDate = data.FestivalDate;
                    if (obj.FestivalID == 0)
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
                    long respose = _IAttandanceService.AddFestival(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult FestivalList()
        {
            List<FestivalListResponse> objModel = _IAttandanceService.GetAllFestivalList();
            return PartialView(objModel);
        }

        public ActionResult DeleteFestival(long? FestivalID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteFestival(FestivalID.Value, IsDelete);
                return RedirectToAction("AddFestival");
            }
            catch (Exception)
            {
                return RedirectToAction("AddFestival");
            }
        }

        public ActionResult ImportAttandance()
        {
            //DateTime? InDateTime = null;
            //DateTime? OutDateTime = null;
            //TimeSpan? TotalHrs = null;
            //DateTime date = Convert.ToDateTime("03/05/2020");

            //DateTime InTime = Convert.ToDateTime("09:15");
            //InDateTime = date.Date.Add(InTime.TimeOfDay);

            //DateTime OutTime = Convert.ToDateTime("18:00");
            //OutDateTime = date.Date.Add(OutTime.TimeOfDay);

            //DateTime ShiftStartTime = Convert.ToDateTime("09:00");
            //DateTime ShiftStartDateTime = date.Date.Add(ShiftStartTime.TimeOfDay);

            //if (InTime > ShiftStartTime)
            //{
            //    InDateTime = date.Date.Add(InTime.TimeOfDay);
            //    TotalHrs = OutDateTime - InDateTime;
            //}
            //if (InTime < ShiftStartTime)
            //{
            //    ShiftStartDateTime = date.Date.Add(ShiftStartTime.TimeOfDay);
            //    TotalHrs = OutDateTime - ShiftStartDateTime;
            //}
            //string OfficeTotalTime = "09:00";
            //string TotalHrsStr = TotalHrs.ToString();
            //TimeSpan duration = DateTime.Parse(TotalHrsStr).Subtract(DateTime.Parse(OfficeTotalTime));


            //string duration2 = duration.ToString();
            //string TimeOut = string.Format("{0:HH:mm}", duration2);


            TempData["ImportFile"] = ConfigurationManager.AppSettings["attendanceformate"];
            return View();
        }

        // Old Attendance Upload Formate
        //[HttpPost]
        //public ActionResult UploadExcelAttandance()
        //{
        //    try
        //    {
        //        HttpPostedFile UserFile = System.Web.HttpContext.Current.Request.Files["File"];
        //        int UserID = Convert.ToInt32(Request.Form["UaserID"]);
        //        int SuccessCount = 0;
        //        int FailCount = 0;
        //        string Message = null;
        //        long UserId = Convert.ToInt64(Request.Form["UserId"]);
        //        bool Errorflag = true;
        //        if (UserFile != null)
        //        {
        //            string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
        //            if (fileExtension == ".xlsx" || fileExtension == ".xls")
        //            {
        //                string fileLocation1 = Server.MapPath("~/importfile/") + "importfile.xlsx";
        //                if (System.IO.File.Exists(fileLocation1))
        //                {
        //                    System.IO.File.Delete(fileLocation1);
        //                }
        //                DataTable Alldata = new DataTable();


        //                string filename = "importfile" + fileExtension;
        //                string fileLocation = System.IO.Path.Combine(Server.MapPath("~/importfile"), filename);
        //                // file is uploaded
        //                UserFile.SaveAs(fileLocation);
        //                string excelConnectionString = string.Empty;
        //                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
        //                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        //                if (fileExtension == ".xlsx" || fileExtension == ".xls")
        //                {
        //                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
        //                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        //                }
        //                //Create Connection to Excel work book and add oledb namespace
        //                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        //                excelConnection.Open();
        //                DataTable dt = new DataTable();
        //                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                if (dt == null)
        //                {
        //                    return Json(null, JsonRequestBehavior.AllowGet);
        //                }
        //                String[] excelSheets = new String[dt.Rows.Count];
        //                int t = 0;
        //                //excel data saves in temp file here.
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    excelSheets[t] = row["TABLE_NAME"].ToString();
        //                    t++;
        //                }
        //                //if (excelSheets[0].ToString() != "'Doctor Form$'")
        //                //{
        //                //    Message = "Invalid Excel Template    .";
        //                //    return Json(Message, JsonRequestBehavior.AllowGet);
        //                //}
        //                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
        //                string query = string.Format("Select * from [{0}]", excelSheets[0]);
        //                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
        //                {
        //                    dataAdapter.Fill(Alldata);
        //                }
        //                excelConnection.Close();


        //                MachineRawPunch dr11 = new MachineRawPunch();
        //                DataSet ds = new DataSet();
        //                DataTable dt22 = new DataTable();
        //                ds.Tables.Add(dt22);
        //                CreateEmployeeColumnDataset(ds);
        //                for (int i = 0; i < Alldata.Rows.Count; i++)
        //                {
        //                    Errorflag = true;
        //                    try
        //                    {
        //                        // Insret Adate and EmployeeID tbl_DailyAttendance and other null value
        //                        DateTime ADate = Convert.ToDateTime(Alldata.Rows[i]["ADate"].ToString());
        //                        string EmployeeID = Alldata.Rows[i]["EmployeeID"].ToString();
        //                        string AEmployeeName = Alldata.Rows[i]["Name"].ToString();


        //                        Insertintotbl_DailyAttendance(ADate, EmployeeID, AEmployeeName);



        //                        MachineRawPunch Obj = new MachineRawPunch();
        //                        Obj.ADate = Convert.ToDateTime(Alldata.Rows[i]["ADate"]);
        //                        Obj.EmployeeID = Alldata.Rows[i]["EmployeeID"].ToString();
        //                        Obj.Name = Alldata.Rows[i]["Name"].ToString();
        //                        Obj.Shift = Alldata.Rows[i]["Shift"].ToString();
        //                        if (Alldata.Rows[i]["TimeIn"].ToString() == "")
        //                        {
        //                            Obj.TimeIn = null;
        //                        }
        //                        else
        //                        {
        //                            Obj.TimeIn = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["TimeIn"].ToString()));
        //                        }
        //                        if (Alldata.Rows[i]["TimeOut"].ToString() == "")
        //                        {
        //                            Obj.TimeOut = null;
        //                        }
        //                        else
        //                        {
        //                            Obj.TimeOut = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["TimeOut"].ToString()));
        //                        }

        //                        // TimeSpan duration = DateTime.Parse(Obj.TimeIn).Subtract(DateTime.Parse(Obj.TimeOut));



        //                        Obj.WorkDuration = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["WorkDuration"].ToString()));
        //                        Obj.OT = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["OT"].ToString()));
        //                        Obj.TotalDuration = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["TotalDuration"].ToString()));
        //                        Obj.Status = Alldata.Rows[i]["Status"].ToString();
        //                        Obj.Remarks = Alldata.Rows[i]["Remarks"].ToString();
        //                        //Obj.PAYCODE = Alldata.Rows[i]["PAYCODE"].ToString();
        //                        //Obj.OFFICEPUNCH = Convert.ToDateTime(Alldata.Rows[i]["OFFICEPUNCH"]);                           
        //                        //Obj.CreatedOn = DateTime.UtcNow;
        //                        //Obj.UpdateOn = DateTime.UtcNow;
        //                        //Obj.IsActive = true;
        //                        if (Errorflag == false)
        //                        {
        //                            DataRow dr = dt22.NewRow();
        //                            dr["ADate"] = Obj.ADate;
        //                            dr["EmployeeID"] = Obj.EmployeeID;
        //                            dr["Name"] = Obj.Name;
        //                            dr["Shift"] = Obj.Shift;
        //                            dr["TimeIn"] = Obj.TimeIn;
        //                            dr["TimeOut"] = Obj.TimeOut;
        //                            dr["WorkDuration"] = Obj.WorkDuration;
        //                            dr["OT"] = Obj.OT;
        //                            dr["TotalDuration"] = Obj.TotalDuration;
        //                            dr["Status"] = Obj.Status;
        //                            dr["Remarks"] = Obj.Remarks;
        //                            //dr["PAYCODE"] = Obj.PAYCODE;
        //                            //dr["OFFICEPUNCH"] = Obj.OFFICEPUNCH;
        //                            dt22.Rows.Add(dr);
        //                            FailCount++;
        //                        }
        //                        else
        //                        {


        //                            bool data = _areaservice.AddMachineRawPunch(Obj);
        //                            SuccessCount++;


        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Message += "Invalid Excel Data.";
        //                        return Json(Message, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                if (Errorflag == false)
        //                {
        //                    using (XLWorkbook wb = new XLWorkbook())
        //                    {
        //                        wb.Worksheets.Add(ds.Tables[0]);
        //                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                        wb.Style.Font.Bold = true;
        //                        Response.Clear();
        //                        Response.Buffer = true;
        //                        Response.Charset = "";
        //                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                        Response.AddHeader("content-disposition", "attachment;filename= " + "Targatesheet.xls");
        //                        using (MemoryStream MyMemoryStream = new MemoryStream())
        //                        {
        //                            wb.SaveAs(MyMemoryStream);
        //                            MyMemoryStream.WriteTo(Response.OutputStream);
        //                            //   File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
        //                            Response.Flush();
        //                            Response.End();
        //                        }
        //                    }
        //                    return Json(ds, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    Message = "Import Successfully";
        //                    return Json(Message, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else
        //            {
        //                Message = "Please Select .xlsx File";
        //                return Json(Message, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            Message = "Please Select .xlsx File";
        //            return Json(Message, JsonRequestBehavior.AllowGet);
        //        }
        //        Message += "Total Record : " + (SuccessCount + FailCount) + Environment.NewLine + "Suceess Record : " + SuccessCount + Environment.NewLine + "Fail Record : " + FailCount;
        //        return Json(Message, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Importmsg"] = "Import Failed";
        //        return Json(ex.Message + ex.InnerException, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //public ActionResult Insertintotbl_DailyAttendance(DateTime ADate, string EmployeeID, string AEmployeeName)
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
        //            int respose = _areaservice.InsertDailyAttendance(ADate, EmployeeID, AEmployeeName);
        //            return Json(respose, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}



        // 16/04/2020 New Attendance Upload Formate
        public DataSet CreateEmployeeColumnDataset(DataSet ds)
        {
            ds.Tables[0].Columns.Add("ADate");
            ds.Tables[0].Columns.Add("EmployeeID");
            ds.Tables[0].Columns.Add("Name");
            ds.Tables[0].Columns.Add("ShiftStartTime");
            ds.Tables[0].Columns.Add("TimeIn");
            ds.Tables[0].Columns.Add("OutDate");
            ds.Tables[0].Columns.Add("TimeOut");
            //ds.Tables[0].Columns.Add("HrsWorked");
            //ds.Tables[0].Columns.Add("NotWork");
            //ds.Tables[0].Columns.Add("BreakTimeHrs");
            //ds.Tables[0].Columns.Add("NonWorking");
            //ds.Tables[0].Columns.Add("Holidays");
            //ds.Tables[0].Columns.Add("WeeklyOff");
            //ds.Tables[0].Columns.Add("HalfdayStatus");
            //ds.Tables[0].Columns.Add("Status");
            //ds.Tables[0].Columns.Add("PAYCODE");
            //ds.Tables[0].Columns.Add("OFFICEPUNCH");
            return ds;
        }

        [HttpPost]
        public ActionResult UploadExcelAttandance()
        {
            try
            {
                HttpPostedFile UserFile = System.Web.HttpContext.Current.Request.Files["File"];
                int UserID = Convert.ToInt32(Request.Form["UaserID"]);
                int SuccessCount = 0;
                int FailCount = 0;
                string Message = null;
                long UserId = Convert.ToInt64(Request.Form["UserId"]);
                bool Errorflag = true;
                if (UserFile != null)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                    if (fileExtension == ".xlsx" || fileExtension == ".xls")
                    {
                        string fileLocation1 = Server.MapPath("~/importfile/") + "importfile.xlsx";
                        if (System.IO.File.Exists(fileLocation1))
                        {
                            System.IO.File.Delete(fileLocation1);
                        }
                        DataTable Alldata = new DataTable();
                        string filename = "importfile" + fileExtension;
                        string fileLocation = System.IO.Path.Combine(Server.MapPath("~/importfile"), filename);
                        // file is uploaded
                        UserFile.SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        if (fileExtension == ".xlsx" || fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();
                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(Alldata);
                        }
                        excelConnection.Close();
                        // MachineRawPunch dr11 = new MachineRawPunch();
                        DataSet ds = new DataSet();
                        DataTable dt22 = new DataTable();
                        ds.Tables.Add(dt22);
                        CreateEmployeeColumnDataset(ds);
                        for (int i = 0; i < Alldata.Rows.Count; i++)
                        {
                            Errorflag = true;
                            if (Alldata.Rows[i]["ADate"].ToString() != "")
                            {
                                try
                                {
                                    //DateTime ADate = Convert.ToDateTime(Alldata.Rows[i]["ADate"].ToString());
                                    //string EmployeeID = Alldata.Rows[i]["EmployeeID"].ToString();
                                    //string AEmployeeName = Alldata.Rows[i]["Name"].ToString();
                                    //  InsertNewDailyAttendance(ADate, EmployeeID, AEmployeeName);
                                    DateTime? InDateTime = null;
                                    DateTime? OutDateTime = null;
                                    TimeSpan? TotalHrs = null;
                                    NewDailyAttendanceModel Obj = new NewDailyAttendanceModel();
                                    Obj.EmployeeCode = Convert.ToInt64(Alldata.Rows[i]["EmployeeID"]);
                                    Obj.AEmployeeName = Alldata.Rows[i]["Name"].ToString();
                                    Obj.ADate = Convert.ToDateTime(Alldata.Rows[i]["ADate"]);
                                    DateTime ADate = Convert.ToDateTime(Obj.ADate);
                                    Obj.ShiftStartTime = Alldata.Rows[i]["ShiftStartTime"].ToString();
                                    if (Alldata.Rows[i]["TimeIn"].ToString() == "" || Alldata.Rows[i]["TimeIn"].ToString() == "00:00")
                                    {
                                        Obj.TimeIn = null;
                                    }
                                    else
                                    {
                                        Obj.TimeIn = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["TimeIn"].ToString()));
                                    }
                                    DateTime TimeIn = Convert.ToDateTime(Obj.TimeIn);
                                    InDateTime = ADate.Date.Add(TimeIn.TimeOfDay);
                                    Obj.InDateTime = InDateTime;

                                    // 7 July 2020 Piyush Limbani
                                    if (Alldata.Rows[i]["OutDate"].ToString() != "")
                                    {
                                        Obj.OutDate = Convert.ToDateTime(Alldata.Rows[i]["OutDate"]);
                                    }
                                    else
                                    {
                                        Obj.OutDate = Convert.ToDateTime(Alldata.Rows[i]["ADate"]);
                                    }
                                    // 7 July 2020 Piyush Limbani

                                    if (Alldata.Rows[i]["TimeOut"].ToString() == "" || Alldata.Rows[i]["TimeOut"].ToString() == "00:00")
                                    {
                                        Obj.TimeOut = null;
                                    }
                                    else
                                    {
                                        Obj.TimeOut = string.Format("{0:HH:mm}", Convert.ToDateTime(Alldata.Rows[i]["TimeOut"].ToString()));
                                    }
                                    DateTime OutTime = Convert.ToDateTime(Obj.TimeOut);

                                    // 7 July 2020 Piyush Limbani
                                    //  OutDateTime = ADate.Date.Add(OutTime.TimeOfDay);
                                    OutDateTime = Obj.OutDate.Date.Add(OutTime.TimeOfDay);
                                    // 7 July 2020 Piyush Limbani

                                    Obj.OutDateTime = OutDateTime;

                                    DateTime ShiftStartTime = Convert.ToDateTime(Obj.ShiftStartTime);
                                    DateTime ShiftStartDateTime = ADate.Date.Add(ShiftStartTime.TimeOfDay);
                                    Obj.ShiftStartDateTime = ShiftStartDateTime;

                                    if (Obj.TimeIn != null)
                                    {
                                        if (TimeIn >= ShiftStartTime)
                                        {
                                            InDateTime = ADate.Date.Add(TimeIn.TimeOfDay);
                                            TotalHrs = OutDateTime - InDateTime;
                                        }
                                        if (TimeIn < ShiftStartTime)
                                        {
                                            ShiftStartDateTime = ADate.Date.Add(ShiftStartTime.TimeOfDay);
                                            TotalHrs = OutDateTime - ShiftStartDateTime;
                                        }
                                        string TotalHrsStr = TotalHrs.ToString();
                                        string[] SplitTotalHrs = TotalHrsStr.Split(':');
                                        decimal TotalHrsDec = decimal.Parse(SplitTotalHrs[0] + "." + SplitTotalHrs[1]);
                                        //int hrs = Convert.ToInt32(SplitTotalHrs[0]);
                                        //int minutes = Convert.ToInt32(SplitTotalHrs[1]);
                                        //decimal TotalHrsDec = decimal.Parse(hrs.ToString("#00") + "." + minutes.ToString("#00"));
                                        if (TotalHrsDec > 0)
                                        {
                                            Obj.TotalHoursWorked = TotalHrsDec;
                                        }
                                        else
                                        {

                                            Obj.TotalHoursWorked = 0;
                                        }
                                        if (TotalHrsDec > 0)
                                        {
                                            string StandardShiftStartTime = "09:00";
                                            TimeSpan OTSubstruct = DateTime.Parse(TotalHrsStr).Subtract(DateTime.Parse(StandardShiftStartTime));
                                            string OT = OTSubstruct.ToString();
                                            string[] SplitOT = OT.Split(':');

                                            //int OThrs = Int32.Parse(SplitOT[0]);
                                            //int OTminutes = Convert.ToInt32(SplitOT[1]);
                                            //decimal OTDec = decimal.Parse(OThrs.ToString("#00") + "." + OTminutes.ToString("#00"));

                                            decimal OTDec = decimal.Parse(SplitOT[0] + "." + SplitOT[1]);
                                            //if (OTDec < 0)
                                            //{
                                            //    Obj.NotWork = Math.Abs(OTDec);
                                            //}
                                            //else
                                            //{
                                            //    Obj.OT = Convert.ToDecimal(OTDec);
                                            //}
                                            Obj.OT = Convert.ToDecimal(OTDec);
                                        }
                                        else
                                        {
                                            Obj.OT = 0;
                                        }

                                    }
                                    if (Obj.TimeIn == null && Obj.TimeOut == null)
                                    {
                                        Obj.Status = "A";
                                    }
                                    else if (Obj.TimeIn == null)
                                    {
                                        Obj.Status = "Error";
                                    }
                                    else if (Obj.TimeOut == null)
                                    {
                                        Obj.Status = "Error";
                                    }
                                    else
                                    {
                                        Obj.Status = "P";
                                    }
                                    Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                                    Obj.CreatedOn = DateTime.Now;
                                    Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                                    Obj.UpdatedOn = DateTime.Now;
                                    Obj.IsDelete = false;

                                    if (Errorflag == false)
                                    {
                                        DataRow dr = dt22.NewRow();
                                        dr["ADate"] = Obj.ADate;
                                        dr["EmployeeID"] = Obj.EmployeeCode;
                                        dr["Name"] = Obj.AEmployeeName;
                                        dr["ShiftStartTime"] = Obj.ShiftStartTime;
                                        dr["TimeIn"] = Obj.TimeIn;
                                        dr["OutDate"] = Obj.OutDate;
                                        dr["TimeOut"] = Obj.TimeOut;
                                        //dr["WorkDuration"] = Obj.TotalHoursWorked;
                                        //dr["OT"] = Obj.OT;
                                        //dr["TotalDuration"] = Obj.TotalHoursWorked;
                                        //dr["Status"] = Obj.Status;
                                        dt22.Rows.Add(dr);
                                        FailCount++;
                                    }
                                    else
                                    {
                                        bool data = _IAttandanceService.AddNewDailyAttendance(Obj);
                                        SuccessCount++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Message += "Invalid Excel Data.";
                                    return Json(Message, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        if (Errorflag == false)
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(ds.Tables[0]);
                                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                wb.Style.Font.Bold = true;
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename= " + "Targatesheet.xls");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                            return Json(ds, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Message = "Import Successfully";
                            return Json(Message, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Message = "Please Select .xlsx File";
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Message = "Please Select .xlsx File";
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                Message += "Total Record : " + (SuccessCount + FailCount) + Environment.NewLine + "Suceess Record : " + SuccessCount + Environment.NewLine + "Fail Record : " + FailCount;
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Importmsg"] = "Import Failed";
                return Json(ex.Message + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        //20-04-2020
        public ActionResult AddEarnedLeaves()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEarnedLeaves(AddEarnedLeaves data)
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
                    EarnedLeaves_Mst obj = new EarnedLeaves_Mst();
                    obj.EarnedLeavesID = data.EarnedLeavesID;
                    obj.MonthID = data.MonthID;
                    obj.NoOfEarnedLeaves = data.NoOfEarnedLeaves;
                    if (obj.EarnedLeavesID == 0)
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
                    long respose = _IAttandanceService.AddEarnedLeaves(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult EarnedLeavesList()
        {
            List<EarnedLeavesListResponse> objModel = _IAttandanceService.GetAllEarnedLeavesList();
            return PartialView(objModel);
        }

        public ActionResult DeleteEarnedLeaves(long? EarnedLeavesID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteEarnedLeaves(EarnedLeavesID.Value, IsDelete);
                return RedirectToAction("AddEarnedLeaves");
            }
            catch (Exception)
            {
                return RedirectToAction("AddEarnedLeaves");
            }
        }

        //29-04-2020
        public ActionResult AddPF()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPF(AddPF data)
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
                    PF_Mst obj = new PF_Mst();
                    obj.PFID = data.PFID;
                    obj.HighestSlab = data.HighestSlab;
                    obj.HighestPF = data.HighestPF;
                    obj.PFPercentage = data.PFPercentage;
                    obj.Note = data.Note;
                    if (obj.PFID == 0)
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
                    long respose = _IAttandanceService.AddPF(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PFList()
        {
            List<PFListResponse> objModel = _IAttandanceService.GetAllPFList();
            return PartialView(objModel);
        }

        public ActionResult DeletePF(long? PFID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeletePF(PFID.Value, IsDelete);
                return RedirectToAction("AddPF");
            }
            catch (Exception)
            {
                return RedirectToAction("AddPF");
            }
        }

        //29-04-2020
        public ActionResult AddESIC()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddESIC(AddESIC data)
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
                    ESIC_Mst obj = new ESIC_Mst();
                    obj.ESICID = data.ESICID;
                    obj.EmployeeSlab = data.EmployeeSlab;
                    obj.Note = data.Note;
                    if (obj.ESICID == 0)
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
                    long respose = _IAttandanceService.AddESIC(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ESICList()
        {
            List<ESICListResponse> objModel = _IAttandanceService.GetAllESICList();
            return PartialView(objModel);
        }

        public ActionResult DeleteESIC(long? ESICID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteESIC(ESICID.Value, IsDelete);
                return RedirectToAction("AddESIC");
            }
            catch (Exception)
            {
                return RedirectToAction("AddESIC");
            }
        }

        //29-04-2020
        public ActionResult AddPT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPT(AddPT data)
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
                    PT_Mst obj = new PT_Mst();
                    obj.PTID = data.PTID;
                    obj.MonthID = data.MonthID;
                    obj.HighestSlab = data.HighestSlab;
                    obj.HighestAmount = data.HighestAmount;
                    obj.LowestSlab = data.LowestSlab;
                    obj.LowestAmount = data.LowestAmount;
                    obj.Note = data.Note;
                    if (obj.PTID == 0)
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
                    long respose = _IAttandanceService.AddPT(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PTList()
        {
            List<PTListResponse> objModel = _IAttandanceService.GetAllPTList();
            return PartialView(objModel);
        }

        public ActionResult DeletePT(long? PTID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeletePT(PTID.Value, IsDelete);
                return RedirectToAction("AddPT");
            }
            catch (Exception)
            {
                return RedirectToAction("AddPT");
            }
        }

        //29-04-2020
        public ActionResult AddMLWF()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMLWF(AddMLWF data)
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
                    MLWF_Mst obj = new MLWF_Mst();
                    obj.MLWFID = data.MLWFID;
                    obj.MonthID = data.MonthID;
                    obj.HighestSlab = data.HighestSlab;
                    obj.HighestAmount = data.HighestAmount;
                    obj.Note = data.Note;
                    if (obj.MLWFID == 0)
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
                    long respose = _IAttandanceService.AddMLWF(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult MLWFList()
        {
            List<MLWFListResponse> objModel = _IAttandanceService.GetAllMLWFList();
            return PartialView(objModel);
        }

        public ActionResult DeleteMLWF(long? MLWFID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteMLWF(MLWFID.Value, IsDelete);
                return RedirectToAction("AddMLWF");
            }
            catch (Exception)
            {
                return RedirectToAction("AddMLWF");
            }
        }

        //29-04-2020
        public ActionResult AddLeaveEncashment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddLeaveEncashment(AddLeaveEncashment data)
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
                    LeaveEncashment_Mst obj = new LeaveEncashment_Mst();
                    obj.LeaveEncashmentID = data.LeaveEncashmentID;
                    obj.NoOfDaysLeaveEncashment = data.NoOfDaysLeaveEncashment;
                    obj.Note = data.Note;
                    if (obj.LeaveEncashmentID == 0)
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
                    long respose = _IAttandanceService.AddLeaveEncashment(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult LeaveEncashmentList()
        {
            List<LeaveEncashmentMstListResponse> objModel = _IAttandanceService.GetAllLeaveEncashmentList();
            return PartialView(objModel);
        }

        public ActionResult DeleteLeaveEncashment(long? LeaveEncashmentID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteLeaveEncashment(LeaveEncashmentID.Value, IsDelete);
                return RedirectToAction("AddLeaveEncashment");
            }
            catch (Exception)
            {
                return RedirectToAction("AddLeaveEncashment");
            }
        }

        //29-04-2020
        public ActionResult AddGratuity()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGratuity(AddGratuity data)
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
                    Gratuity_Mst obj = new Gratuity_Mst();
                    obj.GratuityID = data.GratuityID;
                    obj.NoOfDaysInMonth = data.NoOfDaysInMonth;
                    obj.GratuityNoOfDaysInYear = data.GratuityNoOfDaysInYear;
                    obj.Note = data.Note;
                    if (obj.GratuityID == 0)
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
                    long respose = _IAttandanceService.AddGratuity(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GratuityList()
        {
            List<GratuityMstListResponse> objModel = _IAttandanceService.GetAllGratuityList();
            return PartialView(objModel);
        }

        public ActionResult DeleteGratuity(long? GratuityID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteGratuity(GratuityID.Value, IsDelete);
                return RedirectToAction("AddGratuity");
            }
            catch (Exception)
            {
                return RedirectToAction("AddGratuity");
            }
        }

        public ActionResult SyncSalarySheet()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            return View();
        }

        [HttpPost]
        public ActionResult SyncSalarySheet(int MonthID, int YearID, int GodownID)
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
                    List<ActiveEmployeeCode> data = _IAttandanceService.GetActiveEmployeeCodeFromUserMaster(GodownID);
                    for (int i = 0; i < data.Count; i++)
                    {
                        string MonthStartDate = YearID + "-" + MonthID + "-" + "01";
                        SalaryExistModel exist = _IAttandanceService.CheckSalaryExist(MonthID, YearID, data[i].EmployeeCode);
                        var detail = _IAttandanceService.GetEmployeeAttandanceDetail(MonthID, YearID, data[i].EmployeeCode, MonthStartDate);
                        if (exist.SalarySheetID == 0 && detail.AMonth != 0 && detail.AYear != 0)
                        {
                            SalarySheet_Master Obj = new SalarySheet_Master();
                            Obj.SalarySheetID = exist.SalarySheetID;
                            Obj.EmployeeCode = data[i].EmployeeCode;
                            Obj.MonthID = MonthID;
                            Obj.YearID = YearID;
                            Obj.Present = detail.Present;
                            Obj.AdditionalPresent = detail.AdditionalPresent;
                            Obj.TotalPresent = detail.Present;
                            Obj.Sunday = detail.TotalMonthSunday;
                            Obj.AdditionalSunday = detail.AdditionalSunday;
                            Obj.TotalSunday = detail.TotalMonthSunday;
                            Obj.Holiday = detail.Holiday;
                            Obj.AdditionalHoliday = detail.AdditionalHoliday;
                            Obj.TotalHoliday = detail.Holiday;
                            Obj.Absent = detail.Absent;
                            Obj.AdditionalAbsent = detail.AdditionalAbsent;
                            Obj.TotalAbsent = detail.Absent;
                            Obj.TotalDays = detail.TotalDays;
                            Obj.TotalDaysIntheMonth = detail.TotalMonthDay;
                            Obj.BasicAllowance = detail.BasicAllowance;
                            Obj.HouseRentAllowance = detail.HouseRentAllowance;
                            Obj.TotalBasic = detail.TotalBasic;
                            Obj.EarnedBasicWages = detail.EarnedBasicWages;
                            Obj.EarnedHouseRentAllowance = detail.EarnedHouseRentAllowance;
                            Obj.TotalEarnedWages = detail.TotalEarnedWages;
                            Obj.CityAllowanceMinutes = detail.CityAllowanceMinutes;
                            Obj.CityAllowanceHours = detail.CityAllowanceHours;
                            Obj.CityAllowance = detail.CityAllowance;
                            Obj.AdditionalCityAllowance = 0;
                            Obj.TotalCityAllowance = detail.CityAllowance;
                            Obj.VehicleAllowance = detail.VehicleAllowance;
                            Obj.AdditionalVehicleAllowance = 0;
                            Obj.TotalVehicleAllowance = detail.VehicleAllowance;
                            Obj.Conveyance = detail.Conveyance;
                            Obj.AdditionalConveyance = 0;
                            Obj.TotalConveyance = detail.Conveyance;
                            Obj.PerformanceAllowance = detail.PerformanceAllowance;
                            Obj.AdditionalPerformanceAllowance = 0;
                            Obj.TotalPerformanceAllowance = detail.PerformanceAllowance;
                            Obj.GrossWagesPayable = detail.GrossWagesPayable;
                            Obj.PF = detail.PF;
                            Obj.ESIC = detail.ESIC;
                            Obj.PT = detail.PT;
                            Obj.MLWF = detail.MLWF;
                            Obj.TotalDeductions = detail.TotalDeductions;
                            Obj.NetWagesPaid = detail.NetWagesPaid;
                            Obj.OpeningLeaves = detail.OpeningLeaves;
                            Obj.EarnedLeaves = detail.EarnedLeaves;
                            Obj.AvailedLeaves = detail.AvailedLeaves;
                            Obj.AdditionalAvailedLeaves = detail.AdditionalAvailedLeaves;
                            Obj.TotalAvailedLeaves = detail.AvailedLeaves;
                            Obj.ClosingLeaves = detail.ClosingLeaves;
                            Obj.AdditionalClosingLeaves = detail.AdditionalClosingLeaves;
                            Obj.TotalClosingLeaves = detail.ClosingLeaves;
                            Obj.OpeningAdvance = detail.OpeningAdvance;
                            Obj.Addition = detail.Addition;
                            Obj.Deductions = detail.Deductions;
                            Obj.ClosingAdvance = detail.ClosingAdvance;
                            Obj.TDS = detail.TDS;
                            Obj.Goods = detail.Goods;
                            Obj.AnyOtherDeductions1 = detail.AnyOtherDeductions1;
                            Obj.AnyOtherDeductions2 = detail.AnyOtherDeductions2;
                            Obj.CustomerID = detail.CustomerID;
                            if (Obj.SalarySheetID == 0)
                            {
                                Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                                Obj.CreatedOn = DateTime.Now;
                            }
                            else
                            {
                                Obj.CreatedBy = exist.CreatedBy;
                                Obj.CreatedOn = exist.CreatedOn;
                            }
                            Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            Obj.UpdatedOn = DateTime.Now;
                            Obj.IsDelete = true;
                            long respose = _IAttandanceService.AddSalarySheet(Obj);
                        }
                        // 7 July 2020 Piyush Limbani For May 2020 For Update
                        else
                        {
                            //if (MonthID == 6 && YearID == 2020 && detail.NetWagesPaid != 0)
                            //{
                            //    SalarySheet_Master Obj = new SalarySheet_Master();
                            //    Obj.SalarySheetID = exist.SalarySheetID;
                            //    Obj.EmployeeCode = data[i].EmployeeCode;
                            //    Obj.MonthID = MonthID;
                            //    Obj.YearID = YearID;
                            //    Obj.Present = detail.Present;
                            //    Obj.AdditionalPresent = detail.AdditionalPresent;
                            //    Obj.TotalPresent = detail.TotalPresent;
                            //    Obj.Sunday = detail.TotalMonthSunday;
                            //    Obj.AdditionalSunday = detail.AdditionalSunday;
                            //    Obj.TotalSunday = detail.TotalSunday;
                            //    Obj.Holiday = detail.Holiday;
                            //    Obj.AdditionalHoliday = detail.AdditionalHoliday;
                            //    Obj.TotalHoliday = detail.TotalHoliday;
                            //    Obj.Absent = detail.Absent;
                            //    Obj.AdditionalAbsent = detail.AdditionalAbsent;
                            //    Obj.TotalAbsent = detail.TotalAbsent;
                            //    Obj.TotalDays = detail.TotalDays;
                            //    Obj.TotalDaysIntheMonth = detail.TotalMonthDay;
                            //    Obj.BasicAllowance = detail.BasicAllowance;
                            //    Obj.HouseRentAllowance = detail.HouseRentAllowance;
                            //    Obj.TotalBasic = detail.TotalBasic;
                            //    Obj.EarnedBasicWages = detail.EarnedBasicWages;
                            //    Obj.EarnedHouseRentAllowance = detail.EarnedHouseRentAllowance;
                            //    Obj.TotalEarnedWages = detail.TotalEarnedWages;
                            //    Obj.CityAllowanceMinutes = detail.CityAllowanceMinutes;
                            //    Obj.CityAllowanceHours = detail.CityAllowanceHours;
                            //    Obj.CityAllowance = detail.CityAllowance;
                            //    Obj.AdditionalCityAllowance = detail.AdditionalCityAllowance;
                            //    Obj.TotalCityAllowance = detail.TotalCityAllowance;
                            //    Obj.VehicleAllowance = detail.VehicleAllowance;
                            //    Obj.AdditionalVehicleAllowance = detail.AdditionalVehicleAllowance;
                            //    Obj.TotalVehicleAllowance = detail.TotalVehicleAllowance;
                            //    Obj.Conveyance = detail.Conveyance;
                            //    Obj.AdditionalConveyance = detail.AdditionalConveyance;
                            //    Obj.TotalConveyance = detail.TotalConveyance;
                            //    Obj.PerformanceAllowance = detail.PerformanceAllowance;
                            //    Obj.AdditionalPerformanceAllowance = detail.AdditionalPerformanceAllowance;
                            //    Obj.TotalPerformanceAllowance = detail.TotalPerformanceAllowance;
                            //    Obj.GrossWagesPayable = detail.GrossWagesPayable;
                            //    Obj.PF = detail.PF;
                            //    Obj.ESIC = detail.ESIC;
                            //    Obj.PT = detail.PT;
                            //    Obj.MLWF = detail.MLWF;
                            //    Obj.TotalDeductions = detail.TotalDeductions;
                            //    Obj.NetWagesPaid = detail.NetWagesPaid;
                            //    Obj.OpeningLeaves = detail.OpeningLeaves;
                            //    Obj.EarnedLeaves = detail.EarnedLeaves;
                            //    Obj.AvailedLeaves = detail.AvailedLeaves;
                            //    Obj.AdditionalAvailedLeaves = detail.AdditionalAvailedLeaves;
                            //    Obj.TotalAvailedLeaves = detail.TotalAvailedLeaves;
                            //    Obj.ClosingLeaves = detail.ClosingLeaves;
                            //    Obj.AdditionalClosingLeaves = detail.AdditionalClosingLeaves;
                            //    Obj.TotalClosingLeaves = detail.TotalClosingLeaves;
                            //    Obj.OpeningAdvance = detail.OpeningAdvance;
                            //    Obj.Addition = detail.Addition;
                            //    Obj.Deductions = detail.Deductions;
                            //    Obj.ClosingAdvance = detail.ClosingAdvance;
                            //    Obj.TDS = detail.TDS;
                            //    Obj.Goods = detail.Goods;
                            //    Obj.AnyOtherDeductions1 = detail.AnyOtherDeductions1;
                            //    Obj.AnyOtherDeductions2 = detail.AnyOtherDeductions2;
                            //    Obj.CustomerID = detail.CustomerID;
                            //    if (Obj.SalarySheetID == 0)
                            //    {
                            //        Obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            //        Obj.CreatedOn = DateTime.Now;
                            //    }
                            //    else
                            //    {
                            //        Obj.CreatedBy = exist.CreatedBy;
                            //        Obj.CreatedOn = exist.CreatedOn;
                            //    }
                            //    Obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            //    Obj.UpdatedOn = DateTime.Now;
                            //    Obj.IsDelete = true;
                            //    long respose = _IAttandanceService.AddSalarySheet(Obj);
                            //}
                        }
                        // 7 July 2020 Piyush Limbani For May 2020
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        // 11-05-2020
        public ActionResult ExportExcelAllowanceList()
        {
            // 05 Jan 2021 Piyush Limbani     
            List<AllowanceListResponse> AllowanceList = null;
            string MonthID = DateTime.Now.Month.ToString();
            int YearID = DateTime.Now.Year;
            long SYear = 0;
            string EYear = "";
            if (MonthID == "1" || MonthID == "2" || MonthID == "3")
            {
                SYear = YearID - 1;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                string EndDate = YearID + "-" + "03" + "-" + "31";
                AllowanceList = _IAttandanceService.GetAllowanceList(StartDate, EndDate);
            }
            else
            {
                SYear = YearID;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                EYear = DateTime.Now.AddYears(1).Year.ToString();
                string EndDate = EYear + "-" + "03" + "-" + "31";
                AllowanceList = _IAttandanceService.GetAllowanceList(StartDate, EndDate);
            }
            // 05 Jan 2021 Piyush Limbani     


            //string syear = DateTime.Now.Year.ToString();
            //string StartDate = syear + "-" + "04" + "-" + "01";
            //string eyear = DateTime.Now.AddYears(1).Year.ToString();
            //string EndDate = eyear + "-" + "03" + "-" + "31";
            //var AllowanceList = _IAttandanceService.GetAllowanceList(StartDate, EndDate);

            List<AllowanceListForExp> lstallowancelist = AllowanceList.Select(x => new AllowanceListForExp()
            {
                EmployeeCode = x.EmployeeCode,
                Name = x.FullName,
                BirthDate = x.BirthDatestr,
                Age = x.Age,
                DateOfJoining = x.DateOfJoiningstr,
                OpeningDate = x.OpeningDatestr,
                IncrementDate = x.IncrementDatestr,
                DA1Date = x.DA1Datestr,
                DA2Date = x.DA2Datestr,
                OthersDate = x.OthersDatestr,
                BasicAllowance1 = x.BasicAllowance1,
                BasicAllowance2 = x.BasicAllowance2,
                BasicAllowance3 = x.BasicAllowance3,
                BasicAllowance4 = x.BasicAllowance4,
                BasicAllowance5 = x.BasicAllowance5,
                TotalBasicAllowance = x.TotalBasicAllowance,
                HRAPercentage1 = x.HRAPercentage1,
                HRAPercentage2 = x.HRAPercentage2,
                HRAPercentage3 = x.HRAPercentage3,
                HRAPercentage4 = x.HRAPercentage4,
                HRAPercentage5 = x.HRAPercentage5,
                HouseRentAllowance1 = x.HouseRentAllowance1,
                HouseRentAllowance2 = x.HouseRentAllowance2,
                HouseRentAllowance3 = x.HouseRentAllowance3,
                HouseRentAllowance4 = x.HouseRentAllowance4,
                HouseRentAllowance5 = x.HouseRentAllowance5,
                TotalHouseRentAllowance = x.TotalHouseRentAllowance,
                TotalWages1 = x.TotalWages1,
                TotalWages2 = x.TotalWages2,
                TotalWages3 = x.TotalWages3,
                TotalWages4 = x.TotalWages4,
                TotalWages5 = x.TotalWages5,
                GrandTotalWages = x.GrandTotalWages,
                Conveyance = x.Conveyance,
                ConveyancePerDay = x.ConveyancePerDay,
                VehicleAllowance = x.VehicleAllowance,
                PerformanceAllowance = x.PerformanceAllowance,
                PerformanceAllowanceStatusID = x.PerformanceAllowanceStatusID,
                CityAllowanceStatusID = x.CityAllowanceStatusID,
                PFStatusID = x.PFStatusID,
                ESICStatusID = x.ESICStatusID,
                BonusPercentage = x.BonusPercentage,
                BonusAmount = x.BonusAmount,
                BonusStatusID = x.BonusStatusID,
                LeaveEnhancementPercentage = x.LeaveEnhancementPercentage,
                LeaveEnhancementAmount = x.LeaveEnhancementAmount,
                LeaveEnhancementStatusID = x.LeaveEnhancementStatusID,
                GratuityPercentage = x.GratuityPercentage,
                GratuityAmount = x.GratuityAmount,
                GratuityStatusID = x.GratuityStatusID

            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstallowancelist));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "AllowanceList.xls");
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

        // 11 Aug 2020 Piyush Limbani
        public ActionResult ForwardAllownceDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddForwardAllownceDetail()
        {
            bool respose = false;

            // 05 Jan 2021 Piyush Limbani     
            AllowanceDetailCount objModel = null;
            string MonthID = DateTime.Now.Month.ToString();
            int YearID = DateTime.Now.Year;
            long SYear = 0;
            string EYear = "";
            if (MonthID == "1" || MonthID == "2" || MonthID == "3")
            {
                SYear = YearID - 1;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                string EndDate = YearID + "-" + "03" + "-" + "31";
                objModel = _IAttandanceService.GetAllowanceDetailCount(StartDate, EndDate);
            }
            else
            {
                SYear = YearID;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                EYear = DateTime.Now.AddYears(1).Year.ToString();
                string EndDate = EYear + "-" + "03" + "-" + "31";
                objModel = _IAttandanceService.GetAllowanceDetailCount(StartDate, EndDate);
            }
            // 05 Jan 2021 Piyush Limbani     


            //string syear = DateTime.Now.Year.ToString();
            //string StartDate = syear + "-" + "04" + "-" + "01";
            //string eyear = DateTime.Now.AddYears(1).Year.ToString();
            //string EndDate = eyear + "-" + "03" + "-" + "31";
            //AllowanceDetailCount objModel = _IAttandanceService.GetAllowanceDetailCount(StartDate, EndDate);

            if (objModel.RecordCount <= 0)   // 06 Jan 2021 Piyush Limbani   if (objModel.RecordCount < 0)   
            {
                // 11 Aug 2020 Piyush Limbani // Forward Record
                List<EmployeeCodeList> EmployeeCodeList = _IAttandanceService.GetAllEmployeeCodeListFromAllowanceMaster();
                for (int i = 0; i < EmployeeCodeList.Count; i++)
                {
                    ForwardAllownceDetailList LastAllowanceDetail = _IAttandanceService.GetLastAllowanceDetailForForwardNextYear(EmployeeCodeList[i].EmployeeCode);
                    if (LastAllowanceDetail != null)
                    {
                        string currentyear = DateTime.Now.Year.ToString();
                        string OpeningDate = currentyear + "-" + "04" + "-" + "01";
                        Allowance_Master obj = new Allowance_Master();
                        obj.EmployeeCode = LastAllowanceDetail.EmployeeCode;
                        obj.OpeningDate = Convert.ToDateTime(OpeningDate);
                        obj.IncrementDate = null;
                        obj.DA1Date = null;
                        obj.DA2Date = null;
                        obj.OthersDate = null;
                        obj.BasicAllowance1 = LastAllowanceDetail.TotalBasicAllowance;
                        obj.BasicAllowance2 = 0;
                        obj.BasicAllowance3 = 0;
                        obj.BasicAllowance4 = 0;
                        obj.BasicAllowance5 = 0;
                        obj.TotalBasicAllowance = LastAllowanceDetail.TotalBasicAllowance;
                        obj.HRAPercentage1 = 0;
                        obj.HRAPercentage2 = 0;
                        obj.HRAPercentage3 = 0;
                        obj.HRAPercentage4 = 0;
                        obj.HRAPercentage5 = 0;
                        obj.HouseRentAllowance1 = LastAllowanceDetail.TotalHouseRentAllowance;
                        obj.HouseRentAllowance2 = 0;
                        obj.HouseRentAllowance3 = 0;
                        obj.HouseRentAllowance4 = 0;
                        obj.HouseRentAllowance5 = 0;
                        obj.TotalHouseRentAllowance = LastAllowanceDetail.TotalHouseRentAllowance;
                        obj.TotalWages1 = LastAllowanceDetail.GrandTotalWages;
                        obj.TotalWages2 = 0;
                        obj.TotalWages3 = 0;
                        obj.TotalWages4 = 0;
                        obj.TotalWages5 = 0;
                        obj.GrandTotalWages = LastAllowanceDetail.GrandTotalWages;
                        obj.Conveyance = LastAllowanceDetail.Conveyance;
                        obj.ConveyancePerDay = LastAllowanceDetail.ConveyancePerDay;
                        obj.VehicleAllowance = LastAllowanceDetail.VehicleAllowance;
                        obj.PerformanceAllowance = LastAllowanceDetail.PerformanceAllowance;
                        obj.PerformanceAllowanceStatusID = LastAllowanceDetail.PerformanceAllowanceStatusID;
                        obj.CityAllowanceStatusID = LastAllowanceDetail.CityAllowanceStatusID;
                        obj.PFStatusID = LastAllowanceDetail.PFStatusID;
                        obj.ESICStatusID = LastAllowanceDetail.ESICStatusID;
                        obj.BonusPercentage = LastAllowanceDetail.BonusPercentage;
                        obj.BonusAmount = LastAllowanceDetail.BonusAmount;
                        obj.BonusStatusID = LastAllowanceDetail.BonusStatusID;
                        obj.LeaveEnhancementPercentage = LastAllowanceDetail.LeaveEnhancementPercentage;
                        obj.LeaveEnhancementAmount = LastAllowanceDetail.LeaveEnhancementAmount;
                        obj.LeaveEnhancementStatusID = LastAllowanceDetail.LeaveEnhancementStatusID;
                        obj.GratuityPercentage = LastAllowanceDetail.GratuityPercentage;
                        obj.GratuityAmount = LastAllowanceDetail.GratuityAmount;
                        obj.GratuityStatusID = LastAllowanceDetail.GratuityStatusID;
                        obj.OpeningLeaves = 0;
                        obj.CustomerID = LastAllowanceDetail.CustomerID;
                        obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.CreatedOn = DateTime.Now;
                        obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsDelete = false;
                        respose = _IAttandanceService.AddAllowance(obj);
                    }
                }
                // 11 Aug 2020 Piyush Limbani               
            }
            else
            {
                // Get List 
            }
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ViewForwardAllownceDetailList()
        {
            // 05 Jan 2021 Piyush Limbani     
            List<AllowanceListResponse> objModel = null;
            string MonthID = DateTime.Now.Month.ToString();
            int YearID = DateTime.Now.Year;
            long SYear = 0;
            string EYear = "";
            if (MonthID == "1" || MonthID == "2" || MonthID == "3")
            {
                SYear = YearID - 1;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                string EndDate = YearID + "-" + "03" + "-" + "31";
                objModel = _IAttandanceService.GetAllowanceList(StartDate, EndDate);
            }
            else
            {
                SYear = YearID;
                string StartDate = SYear + "-" + "04" + "-" + "01";
                EYear = DateTime.Now.AddYears(1).Year.ToString();
                string EndDate = EYear + "-" + "03" + "-" + "31";
                objModel = _IAttandanceService.GetAllowanceList(StartDate, EndDate);
            }
            // 05 Jan 2021 Piyush Limbani     



            //string syear = DateTime.Now.Year.ToString();
            //string StartDate = syear + "-" + "04" + "-" + "01";
            //string eyear = DateTime.Now.AddYears(1).Year.ToString();
            //string EndDate = eyear + "-" + "03" + "-" + "31";
            //List<AllowanceListResponse> objModel = _IAttandanceService.GetAllowanceList(StartDate, EndDate);

            return PartialView(objModel);
        }
        // 11 Aug 2020 Piyush Limbani



        // 14 Feb 2022 Piyush Limbani
        public ActionResult FormSixteenDetail()
        {
            return View();
        }

        //Form16TexableIncome
        [HttpPost]
        public ActionResult AddFormSixteenDetail(AddFormSixteenDetail data)
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
                    if (data.FormSixteenDetailID == 0)
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
                    long respose = _IAttandanceService.AddFormSixteenDetailMaster(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //Form16TexableIncome
        public ActionResult GetTexableIncomeQtyList(long FormSixteenDetailID)
        {
            List<AddFormSixteenTexableIncome> lstForm16TexableIncomeQty = _IAttandanceService.GetAllTexableIncomeListByForm16TexableIncomeID(FormSixteenDetailID);
            return Json(lstForm16TexableIncomeQty, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult FormSixteenDetailList()
        {
            List<FormSixteenDetailListResponse> objModel = _IAttandanceService.GetAllFormSixteenDetailList();
            return PartialView(objModel);
        }

        public ActionResult DeleteFormSixteenDetail(long? FormSixteenDetailID, bool IsDelete)
        {
            try
            {
                _IAttandanceService.DeleteFormSixteenDetail(FormSixteenDetailID.Value, IsDelete);
                return RedirectToAction("FormSixteenDetail");
            }
            catch (Exception)
            {
                return RedirectToAction("FormSixteenDetail");
            }
        }
        // 14 Feb 2022 Piyush Limbani


    }
}