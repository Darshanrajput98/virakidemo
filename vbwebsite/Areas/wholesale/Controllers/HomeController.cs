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
using vb.Service;
using vbwebsite.App_Start;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class HomeController : Controller
    {
        private ICommonService _ICommonService;
        private IExpensesService _IExpensesService;


        //
        public HomeController(ICommonService ICommonService, IExpensesService expensesservice)
        {
            _ICommonService = ICommonService;
            _IExpensesService = expensesservice;
        }
        //
        // GET: /wholesale/Home/
        [RoleAuthentication]
        public ActionResult Index()
        {
            ViewBag.RoleId = Utility.GetUserRoleId().ToString();
            return View();
        }

        [HttpGet]
        public PartialViewResult MenuList()
        {
            int RoleId = Utility.GetUserRoleId();
            DynamicMenuModel objMenu = new DynamicMenuModel();
            objMenu = _ICommonService.DynamicMenuMaster_RoleWiseMenuList(RoleId, "Wholesale");
            return PartialView(objMenu);
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult VehicleDocExpireList()
        {
            int RoleId = Utility.GetUserRoleId();
            List<VehicleListResponse> objModel = new List<VehicleListResponse>();
            //if (RoleId == 5 || RoleId == 6 || RoleId == 3 )
            //{
            objModel = _ICommonService.GetAllVehicleDocExpireList();

            //}
            return PartialView(objModel);
        }

        [HttpGet]
        public PartialViewResult LicenceExpireList()
        {
            int RoleId = Utility.GetUserRoleId();
            List<LicenceListResponse> objModel = new List<LicenceListResponse>();
            //if (RoleId == 5 || RoleId == 6 || RoleId == 3)
            //{
            objModel = _ICommonService.GetAllLicenceExpireList();
            // }
            return PartialView(objModel);
        }

        [HttpGet]
        public PartialViewResult WholesaleFSSAIExpireList()
        {
            long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
            List<CustomerListResponse> objModel = new List<CustomerListResponse>();
            objModel = _ICommonService.GetWholesaleFSSAIExpireListByUserID(UserID);
            return PartialView(objModel);
        }

        [HttpGet]
        public PartialViewResult RetailFSSAIExpireList()
        {
            long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
            List<RetCustomerListResponse> objModel = new List<RetCustomerListResponse>();
            objModel = _ICommonService.GetRetailFSSAIExpireListByUserID(UserID);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelFSSAIExpireList()
        {
            long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);

            var WholealeFSSAIExpireList = _ICommonService.GetWholesaleFSSAIExpireListByUserID(UserID);
            List<CustomerFSSAIExpireListExport> lstwholesale = WholealeFSSAIExpireList.Select(x => new CustomerFSSAIExpireListExport() { CustomerName = x.CustomerName, DeliveryArea = x.DeliveryAreaName, BillingArea = x.BillingAreaName, FSSAI = x.FSSAI, FSSAIValidUpTo = x.FSSAIValidUpTostr, MobileNumber = x.ContactNumber, Email = x.ContactEmail, DaysRemaining = x.DaysRemaining }).ToList();

            var RetailFSSAIExpireList = _ICommonService.GetRetailFSSAIExpireListByUserID(UserID);
            List<RetCustomerFSSAIExpireListExp> lstratail = RetailFSSAIExpireList.Select(x => new RetCustomerFSSAIExpireListExp() { CustomerName = x.CustomerName, DeliveryArea = x.DeliveryAreaName, BillingArea = x.BillingAreaName, FSSAI = x.FSSAI, FSSAIValidUpTo = x.FSSAIValidUpTostr, DaysRemaining = x.DaysRemaining, FSSAI2 = x.FSSAI2, FSSAIValidUpTo2 = x.FSSAIValidUpTostr2, DaysRemaining2 = x.DaysRemaining2, ContactNumber = x.ContactNumber, Email = x.ContactEmail }).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstwholesale));
            ds.Tables.Add(ToDataTable(lstratail));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "CustomerFSSAIExpireList.xls");
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

        // 03-04-2020 Display Pouch Qty Stock in Dashboard
        [HttpGet]
        public PartialViewResult MinPouchQuantityList()
        {
            int RoleId = Utility.GetUserRoleId();
            long GodownID = Convert.ToInt64(Request.Cookies["GodownID"].Value);
            List<MinPouchQuantityListResponse> objModel = new List<MinPouchQuantityListResponse>();
            if (RoleId == 5 || RoleId == 6)
            {
                objModel = _ICommonService.GetStockPouchListForDashboardForAdmin(GodownID);
            }
            else
            {
                objModel = _ICommonService.GetStockPouchListForDashboard(GodownID);
            }
            return PartialView(objModel);
        }

        // 13 Oct 2020 Piyush Limbani
        [HttpGet]
        public PartialViewResult MinUtilityQuantityList()
        {
            int RoleId = Utility.GetUserRoleId();
            long GodownID = Convert.ToInt64(Request.Cookies["GodownID"].Value);
            List<MinUtilityQuantityListResponse> objModel = new List<MinUtilityQuantityListResponse>();
            if (RoleId == 5 || RoleId == 6)
            {
                objModel = _ICommonService.GetStockUtilityListForDashboardForAdmin(GodownID);
            }
            else
            {
                objModel = _ICommonService.GetStockUtilityListForDashboard(GodownID);
            }
            return PartialView(objModel);
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

    }
}