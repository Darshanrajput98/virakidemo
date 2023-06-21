using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.attendance.Controllers
{
    public class PaymentController : Controller
    {

        private IAdminService _adminservice;
        private ICommonService _ICommonService;
        private IAttandanceService _IAttandanceService;

        public PaymentController(IAdminService adminservice, ICommonService commonservice, IAttandanceService attandanceservice)
        {
            _adminservice = adminservice;
            _ICommonService = commonservice;
            _IAttandanceService = attandanceservice;
        }

        //
        // GET: /attendance/Payment/
        public ActionResult Index()
        {
            return View();
        }

        // 28 July 2020 Piyush Limbani
        public ActionResult Payment()
        {
            ViewBag.Godown = _ICommonService.GetGodownNameForExpense();
            ViewBag.Employee = _ICommonService.GetAllEmployeeName();
            ViewBag.BankName = _ICommonService.GetBankNameList();
            return View();
        }

        // 29 July 2020 Piyush Limbani
        [HttpPost]
        public ActionResult AddPayment(AddPayment data)
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
                    List<AddPayment> lstdata = _IAttandanceService.GetEmployeeSalaryDetailForPayment(data.MonthID, data.YearID, data.GodownID, data.EmployeeCode);
                    for (int i = 0; i < lstdata.Count; i++)
                    {
                        SalaryPaymentExistModel exist = _IAttandanceService.CheckPaymentSalaryExist(data.MonthID, data.YearID, lstdata[i].EmployeeCode);
                        SalaryPayment_Mst Obj = new SalaryPayment_Mst();
                        Obj.PaymentID = exist.PaymentID;
                        Obj.MonthID = data.MonthID;
                        Obj.YearID = data.YearID;
                        Obj.GodownID = data.GodownID;
                        Obj.EmployeeCode = lstdata[i].EmployeeCode;
                        Obj.BankID = data.BankID;
                        if (Obj.PaymentID == 0)
                        {
                            if (data.PaymentDate != null && data.PaymentDate != Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                            {
                                Obj.PaymentDate = data.PaymentDate;
                            }
                            else
                            {
                                Obj.PaymentDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            Obj.PaymentDate = exist.PaymentDate;
                        }
                        Obj.SalarySheetID = lstdata[i].SalarySheetID;
                        Obj.NetWagesPaid = lstdata[i].NetWagesPaid;
                        Obj.Deductions = lstdata[i].Deductions;
                        Obj.TDS = lstdata[i].TDS;
                        Obj.Goods = lstdata[i].Goods;
                        Obj.AnyOtherDeductions1 = lstdata[i].AnyOtherDeductions1;
                        Obj.AnyOtherDeductions2 = lstdata[i].AnyOtherDeductions2;
                        Obj.FinalTotalDeductions = lstdata[i].FinalTotalDeductions;
                        Obj.NetWagesToPay = lstdata[i].NetWagesToPay;
                        if (Obj.PaymentID == 0)
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
                        long respose = _IAttandanceService.AddSalaryPayment(Obj);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        // 29 July 2020 Piyush Limbani
        [HttpPost]
        public PartialViewResult PaymentList(int MonthID, int YearID)
        {
            List<SalaryPaymentListResponse> objModel = _IAttandanceService.GetSalaryPaymentListByMonthAndYear(MonthID, YearID);
            return PartialView(objModel);
        }

    }
}