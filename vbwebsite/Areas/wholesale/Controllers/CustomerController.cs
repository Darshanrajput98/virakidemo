using ClosedXML.Excel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerService _customerservice;

        public CustomerController(ICustomerService customerservice)
        {
            _customerservice = customerservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageCustomers()
        {

            ViewBag.Area = _customerservice.GetAllAreaName();
            ViewBag.CustomerGroup = _customerservice.GetAllCustomerGroupName();
            ViewBag.SalesPerson = _customerservice.GetAllSalesPersonName();
            ViewBag.Tax = _customerservice.GetAllTaxName();
            return View();
        }

        [HttpPost]
        public ActionResult ManageCustomers(CustomerViewModel data)
        {
            string response = "";
            string CustomerNumber = "";
            string strFSSAICertificate = "";
            string oldFSSAICertificate = "";
            try
            {
                if (Request.Cookies["UserID"] == null)
                {
                    Request.Cookies["UserID"].Value = null;
                    return JavaScript("location.reload(true)");
                }
                else
                {
                    bool exist = false;
                    if (data.CustomerID == 0)
                    {
                        var lstdata = _customerservice.GetExistCustomerDetials(data.CustomerName, data.AreaID);
                        if (lstdata.CustomerID != 0)
                        {
                            exist = true;
                            response = "Already Exist";
                        }
                    }
                    if (exist == false)
                    {
                        if (data.CustomerID == 0)
                        {
                            var lstdata = _customerservice.GetLastCustomerNumber();
                            if (lstdata != null)
                            {
                                long incr = Convert.ToInt64(lstdata.CustomerNumber + 1);
                                CustomerNumber = Convert.ToString(incr);
                            }
                            else
                            {
                                CustomerNumber = "1";
                            }
                            data.CustomerNumber = Convert.ToInt64(CustomerNumber);
                        }


                        // New Code
                        if (Request.Files.Count > 0)
                        {
                            foreach (string fileName in Request.Files)
                            {
                                if (fileName == "FSSAICertificate")
                                {
                                    string FSSAICertificate;
                                    HttpPostedFileBase FSSAICertificatefile = Request.Files[fileName];
                                    string fileName2 = data.CustomerNumber + "-" + "FSSAICertificate" + Path.GetExtension(FSSAICertificatefile.FileName).ToString();
                                    data.FSSAICertificate = fileName2;
                                    FSSAICertificate = Path.Combine(Server.MapPath("~/CustomerDocument/"), data.FSSAICertificate);
                                    FSSAICertificatefile.SaveAs(FSSAICertificate);
                                    strFSSAICertificate = data.FSSAICertificate;
                                }
                            }
                        }
                        if (strFSSAICertificate == "")
                        {
                            oldFSSAICertificate = _customerservice.GetOldFSSAICertificateByCustomerID(data.CustomerID);
                            data.FSSAICertificate = oldFSSAICertificate;
                        }

                        if (data.CustomerID == 0)
                        {
                            data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                            data.CreatedOn = DateTime.Now;
                        }
                        else
                        {
                            data.CreatedBy = data.CreatedBy;
                            data.CreatedOn = data.CreatedOn;
                        }

                        //data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        //data.CreatedOn = DateTime.Now;
                        data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                        data.UpdatedOn = DateTime.Now;
                        data.lstAddress = JsonConvert.DeserializeObject<List<CustomerAddressViewModel>>(data.address);
                        response = (_customerservice.AddCustomer(data)).ToString();
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #region ManageCustomersList...
        public PartialViewResult ManageCustomersList(int PageNo = 1, int PageSize = 10, string SearchText = "")
        {
            CustomerListResponsepaging response = new CustomerListResponsepaging();
            int Count = 0;
            TempData["CustomersList"] = SearchText;

            List<CustomerListResponse> objModel = new List<CustomerListResponse>();
            objModel = _customerservice.GetAllCustomerList(PageNo, PageSize, SearchText, out Count);
            int totalcount = Count;

            response.Customer = new StaticPagedList<CustomerListResponse>(objModel, PageNo, PageSize, totalcount);
            int StartNo = (PageNo * PageSize) - (PageSize - 1);
            
            ViewBag.PageSize = PageSize;
            ViewBag.StartNo = StartNo;

            int EndNo = PageNo * PageSize;
            if (totalcount < EndNo)
            {
                EndNo = totalcount;
            }
            
            ViewBag.EndNo = EndNo;
            ViewBag.totalcount = totalcount;
            ViewBag.paging = response;

            return PartialView();
        }
        #endregion

        public ActionResult GetCustomerAddressList(long CustomerID)
        {
            List<CustomerAddressViewModel> lstCustomerAddress = _customerservice.GetCustomerAddressListByCustomerID(CustomerID);
            lstCustomerAddress = lstCustomerAddress.Where(c => c.AddressID == 1).ToList();
            return Json(lstCustomerAddress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerBillingAddressList(long CustomerID)
        {
            List<CustomerAddressViewModel> lstCustomerAddress = _customerservice.GetCustomerAddressListByCustomerID(CustomerID);
            lstCustomerAddress = lstCustomerAddress.Where(c => c.AddressID == 2).ToList();
            return Json(lstCustomerAddress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCustomer(long? CustomerID, bool IsDelete)
        {
            try
            {
                _customerservice.DeleteCustomer(CustomerID.Value, IsDelete);
                return RedirectToAction("ManageCustomers");
            }
            catch (Exception)
            {

                return RedirectToAction("ManageCustomers");
            }
        }

        public ActionResult ManageCustomerGroup()
        {
            ViewBag.Area = _customerservice.GetAllAreaName();
            return View();
        }

        [HttpPost]
        public ActionResult ManageCustomerGroup(CustomerGroupViewModel data)
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
                    CustomerGroup_Mst obj = new CustomerGroup_Mst();
                    obj.CustomerGroupID = data.CustomerGroupID;
                    obj.CustomerGroupName = data.CustomerGroupName;
                    obj.CustomerGroupAddress1 = data.CustomerGroupAddress1;
                    obj.CustomerGroupAddress2 = data.CustomerGroupAddress2;
                    obj.AreaID = data.AreaID;
                    obj.CustomerGroupDescription = data.CustomerGroupDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _customerservice.AddCustomerGroup(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageCustomerGroupList()
        {
            List<CustomerGroupListResponse> objModel = _customerservice.GetAllCustomerGroupList();
            return PartialView(objModel);
        }

        public ActionResult DeleteCustomerGroup(long? CustomerGroupID, bool IsDelete)
        {
            try
            {
                _customerservice.DeleteCustomerGroup(CustomerGroupID.Value, IsDelete);
                return RedirectToAction("ManageCustomerGroup");
            }
            catch (Exception)
            {

                return RedirectToAction("ManageCustomerGroup");
            }
        }

        public ActionResult SearchCustomerCallList()
        {
            ViewBag.Area = _customerservice.GetAllAreaName();
            ViewBag.SalesPerson = _customerservice.GetAllSalesPersonName();
            return View();
        }

        [HttpPost]
        public PartialViewResult ViewCustomerCallList(CustomerListResponse model)
        {
            List<CustomerListResponse> objModel = _customerservice.GetAllCustomerCallList(model);
            return PartialView(objModel);
        }

        public ActionResult ExportExcelCustomerCallList(long? AreaID, long? UserID, long? DaysofWeek, bool? CallWeek1, bool? CallWeek2, bool? CallWeek3, bool? CallWeek4)
        {
            if (AreaID == null)
            {
                AreaID = 0;
            }
            if (UserID == null)
            {
                UserID = 0;
            }
            if (DaysofWeek == null)
            {
                DaysofWeek = 0;
            }
            var lstInvoice = _customerservice.GetCustomerCallListForExcel(AreaID.Value, UserID.Value, DaysofWeek.Value, CallWeek1.Value, CallWeek2.Value, CallWeek3.Value, CallWeek4.Value);
            List<ExportToExcelCustomerCallList> lst = lstInvoice.Select(x => new ExportToExcelCustomerCallList() { CustomerName = x.CustomerName, AreaName = x.AreaName, DaysofWeekstr = x.DaysofWeekstr, ContactNumber = x.ContactNumber, UserName = x.UserName, CallWeek1 = x.CallWeek1str, CallWeek2 = x.CallWeek2str, CallWeek3 = x.CallWeek3str, CallWeek4 = x.CallWeek4str, DoNotDisturb = x.DoNotDisturbstr }).ToList();
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable1(lst));
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + "CustomerCallList.xls");
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

        public static DataTable ToDataTable1<T>(List<T> items)
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

        public ActionResult ExportExcelCustomer()
        {
            var CustomerList = _customerservice.GetAllCustomerListForExel();
            List<CustomerListExport> lstproduct = CustomerList.Select(x => new CustomerListExport() { CustomerNumber = x.CustomerNumber, CustomerName = x.CustomerName, AreaName = x.AreaName, DaysofWeek = x.DaysofWeekstr, SalesPerson = x.UserFullName, CustomerGroupName = x.CustomerGroupName, TaxNo = x.TaxNo, TaxName = x.TaxName, FSSAI = x.FSSAI, FSSAIValidUpTo = x.FSSAIValidUpTostr, LBTNo = x.LBTNo, DeliveryAreaName = x.DeliveryAreaName, DeliveryAddressLine1 = x.DeliveryAddressLine1, DeliveryAddressLine2 = x.DeliveryAddressLine2, DeliveryAddressPincode = x.DeliveryAddressPincode, BillingAreaName = x.BillingAreaName, BillingAddressLine1 = x.BillingAddressLine1, BillingAddressLine2 = x.BillingAddressLine2, ContactName = x.ContactName, ContactEmail = x.ContactEmail, ContactMobNumber = x.ContactMobNumber, BankName = x.BankName, Branch = x.Branch, IFCCode = x.IFCCode, CustomerNote = x.CustomerNote, CellNo1 = x.CellNo1, CellNo2 = x.CellNo2, TelNo1 = x.TelNo1, TelNo2 = x.TelNo2, Email2 = x.Email2, Email1 = x.Email1 }).ToList();
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "CustomerList.xls");
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



        public ActionResult ViewFSSAIExpireList()
        {
            List<CustomerListResponse> objModel = _customerservice.GetAllCustomerFSSAIExpireList();
            return View(objModel);
            //return View();
        }

        public ActionResult ExportExcelCustomerFSSAIExpireList()
        {
            var CustomerFSSAIExpireList = _customerservice.GetAllCustomerFSSAIExpireList();
            List<CustomerFSSAIExpireListExport> lstproduct = CustomerFSSAIExpireList.Select(x => new CustomerFSSAIExpireListExport() { CustomerName = x.CustomerName, DeliveryArea = x.DeliveryAreaName, BillingArea = x.BillingAreaName, SalesPerson = x.SalesPerson, FSSAI = x.FSSAI, FSSAIValidUpTo = x.FSSAIValidUpTostr, MobileNumber = x.ContactNumber, Email = x.ContactEmail, DaysRemaining = x.DaysRemaining }).ToList();
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


        [HttpPost]
        public ActionResult UpdateFSSAIDate(long CustomerID, DateTime FSSAIValidUpTo)
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
                    bool respose = _customerservice.UpdateFSSAIDate(CustomerID, FSSAIValidUpTo);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


    }
}