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

namespace vbwebsite.Areas.purchase.Controllers
{
    public class SupplierController : Controller
    {
        private IAdminService _IAdminService;
        private ICustomerService _customerservice;
        private ISupplierService _ISupplierService;

        public SupplierController(ICustomerService customerservice, ISupplierService supplierservice, IAdminService adminservice)
        {
            _customerservice = customerservice;
            _ISupplierService = supplierservice;
            _IAdminService = adminservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddSupplier()
        {
            ViewBag.Area = _IAdminService.GetAllAreaList();
            ViewBag.Tax = _customerservice.GetAllTaxName();
            ViewBag.TDSCategory = _IAdminService.GetAllPurchaseTDSCategoryName();
            return View();
        }

        [HttpPost]
        public ActionResult AddSupplier(AddSupplier data)
        {
            string response = "";

            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    if (data.SupplierID == 0)
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
                    response = (_ISupplierService.AddSupplier(data)).ToString();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult SupplierList()
        {
            List<SupplierListResponse> objModel = _ISupplierService.GetAllSupplierList();
            return PartialView(objModel);
        }

        public ActionResult GetSupplierAddressOneList(long SupplierID)
        {
            List<SupplierContactDetail> lstSupplierAddress = _ISupplierService.GetSupplierAddressListBySupplierID(SupplierID);
            lstSupplierAddress = lstSupplierAddress.Where(c => c.AddressID == 1).ToList();
            return Json(lstSupplierAddress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupplierAddressTwoList(long SupplierID)
        {
            List<SupplierContactDetail> lstSupplierAddress = _ISupplierService.GetSupplierAddressListBySupplierID(SupplierID);
            lstSupplierAddress = lstSupplierAddress.Where(c => c.AddressID == 2).ToList();
            return Json(lstSupplierAddress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSupplier(long? SupplierID, bool IsDelete)
        {
            try
            {
                _ISupplierService.DeleteSupplier(SupplierID.Value, IsDelete);
                return RedirectToAction("AddSupplier");
            }
            catch (Exception)
            {

                return RedirectToAction("AddSupplier");
            }
        }

        public ActionResult FSSAIExpireList()
        {
            List<SupplierListResponse> objModel = _ISupplierService.GetAllSupplierFSSAIExpireList();
            return View(objModel);
        }

        [HttpPost]
        public ActionResult UpdateSupplierFSSAIDate(long SupplierID, DateTime FSSAIValidUpTo)
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
                    long UserID = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    bool respose = _ISupplierService.UpdateSupplierFSSAIDate(SupplierID, FSSAIValidUpTo);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportExcelSupplier()
        {
            var CustomerList = _ISupplierService.GetAllSupplierList();
            List<SupplierListExport> lstproduct = CustomerList.Select(x => new SupplierListExport()
            {
                SupplierName = x.SupplierName,
                GSTNo = x.GSTNo,
                TaxName = x.TaxName,
                PanCardNumber = x.PanCardNumber,
                FSSAI = x.FSSAI,
                FSSAIValidUpTo = x.FSSAIValidUpTostr,
                BankName = x.BankName,
                Branch = x.Branch,
                AccountNumber = x.AccountNumber,
                IFSCCode = x.IFSCCode,
                TDSCategory = x.TDSCategory,
                TDSPercentage = x.TDSPercentage,        
                AddressOneLine1 = x.AddressOneLine1,
                AddressOneLine2 = x.AddressOneLine2,
                AreaOne = x.AreaNameOne,
                AddressOnePincode = x.AddressOnePincode,
                AddressTwoLine1 = x.AddressTwoLine1,
                AddressTwoLine2 = x.AddressTwoLine2,
                AreaTwo = x.AreaNameTwo,
                AddressTwoPincode = x.AddressTwoPincode,
                ContactName = x.ContactName,
                ContactEmail = x.ContactEmail,
                ContactMobileNo = x.ContactMobileNo,
                ContactPhoneNo = x.ContactPhoneNo
            }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(lstproduct));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "SupplierList.xls");
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

    }
}