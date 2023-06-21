using ClosedXML.Excel;
using Newtonsoft.Json;
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

namespace vbwebsite.Areas.retail.Controllers
{
    public class CustomerController : Controller
    {
        private IRetCustomerService _customerservice;
        private IRetOrderService _orderservice;

        public CustomerController(IRetCustomerService customerservice, IRetOrderService orderservice)
        {
            _customerservice = customerservice;
            _orderservice = orderservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageCustomers()
        {
            Session["CustomerID"] = "";
            ViewBag.Area = _customerservice.GetAllAreaName();
            ViewBag.CustomerGroup = _customerservice.GetAllCustomerGroupName();
            ViewBag.SalesPerson = _customerservice.GetAllSalesPersonName();
            ViewBag.Tax = _customerservice.GetAllTaxName();
            ViewBag.Country = _customerservice.GetAllCountryName();
            return View();
        }

        [HttpPost]
        public ActionResult ManageCustomers(RetCustomerViewModel data)
        {
            string response = "";
            string CustomerNumber = "";
            string strFSSAICertificate = "";
            string oldFSSAICertificate = "";
            string strFSSAICertificate2 = "";
            string oldFSSAICertificate2 = "";
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
                                    string fileName1 = data.CustomerNumber + "-" + "FSSAICertificate" + Path.GetExtension(FSSAICertificatefile.FileName).ToString();
                                    data.FSSAICertificate = fileName1;
                                    FSSAICertificate = Path.Combine(Server.MapPath("~/CustomerDocument/"), data.FSSAICertificate);
                                    FSSAICertificatefile.SaveAs(FSSAICertificate);
                                    strFSSAICertificate = data.FSSAICertificate;
                                }
                                else if (fileName == "FSSAICertificate2")
                                {
                                    string FSSAICertificate2;
                                    HttpPostedFileBase FSSAICertificatefile2 = Request.Files[fileName];
                                    string fileName2 = data.CustomerNumber + "-" + "FSSAICertificate2" + Path.GetExtension(FSSAICertificatefile2.FileName).ToString();
                                    data.FSSAICertificate2 = fileName2;
                                    FSSAICertificate2 = Path.Combine(Server.MapPath("~/CustomerDocument/"), data.FSSAICertificate2);
                                    FSSAICertificatefile2.SaveAs(FSSAICertificate2);
                                    strFSSAICertificate2 = data.FSSAICertificate2;
                                }
                            }
                        }
                        if (strFSSAICertificate == "")
                        {
                            oldFSSAICertificate = _customerservice.GetOldFSSAICertificateByCustomerID(data.CustomerID);
                            data.FSSAICertificate = oldFSSAICertificate;
                        }
                        if (strFSSAICertificate2 == "")
                        {
                            oldFSSAICertificate2 = _customerservice.GetOldFSSAICertificate2ByCustomerID(data.CustomerID);
                            data.FSSAICertificate2 = oldFSSAICertificate2;
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

        public PartialViewResult ManageCustomersList()
        {
            List<RetCustomerListResponse> objModel = _customerservice.GetAllCustomerList();
            return PartialView(objModel);
        }

        public ActionResult GetCustomerAddressList(long CustomerID)
        {
            List<RetCustomerAddressViewModel> lstCustomerAddress = _customerservice.GetCustomerAddressListByCustomerID(CustomerID);
            lstCustomerAddress = lstCustomerAddress.Where(c => c.AddressID == 1).ToList();
            return Json(lstCustomerAddress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerBillingAddressList(long CustomerID)
        {
            List<RetCustomerAddressViewModel> lstCustomerAddress = _customerservice.GetCustomerAddressListByCustomerID(CustomerID);
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
        public ActionResult ManageCustomerGroup(RetCustomerGroupViewModel data)
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
                    RetCustomerGroupMst obj = new RetCustomerGroupMst();
                    obj.CustomerGroupID = data.CustomerGroupID;
                    obj.CustomerGroupName = data.CustomerGroupName;
                    obj.CustomerGroupAddress1 = data.CustomerGroupAddress1;
                    obj.CustomerGroupAddress2 = data.CustomerGroupAddress2;
                    obj.AreaID = data.AreaID;
                    obj.CustomerGroupDescription = data.CustomerGroupDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.IsShow = data.IsShow;
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
            List<RetCustomerGroupListResponse> objModel = _customerservice.GetAllCustomerGroupList();
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
        public PartialViewResult ViewCustomerCallList(RetCustomerListResponse model)
        {
            List<RetCustomerListResponse> objModel = _customerservice.GetAllCustomerCallList(model);
            return PartialView(objModel);
        }

        [HttpGet]
        public ActionResult ManageRetailDiscount(Int64 id, string cust, bool status)
        {
            try
            {
                ViewBag.Product = _customerservice.GetAllRetProductQtyList();
                RetCustomerDiscountListResponse objModel = _customerservice.GetRetailDiscountForCustomer(Convert.ToInt64(id));
                objModel.CustomerID = id;
                objModel.CustomerName = cust;
                objModel.IsDelete = status;
                return View(objModel);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddCustomerDiscount(RetCustomerDiscountListResponse data)
        {
            try
            {
                bool respose = false;
                data.CustomerID = data.CustomerID;
                respose = _customerservice.AddCustomerDiscount(data);
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportExcelCustomer()
        {
            var CustomerList = _customerservice.GetAllCustomerListForExel();
            List<RetCustomerListExport> lstproduct = CustomerList.Select(x => new RetCustomerListExport() { CustomerNumber = x.CustomerNumber, CustomerName = x.CustomerName, AreaName = x.AreaName, DaysofWeek = x.DaysofWeekstr, SalesPerson = x.UserFullName, CustomerGroupName = x.CustomerGroupName, TaxName = x.TaxName, LBTNo = x.LBTNo, DeliveryAreaName = x.DeliveryAreaName, DeliveryAddressLine1 = x.DeliveryAddressLine1, DeliveryAddressLine2 = x.DeliveryAddressLine2, DeliveryAddressPincode = x.DeliveryAddressPincode, TaxNo = x.TaxNo, FSSAI = x.FSSAI, FSSAIValidUpTo = x.FSSAIValidUpTostr, BillingAreaName = x.BillingAreaName, BillingAddressLine1 = x.BillingAddressLine1, BillingAddressLine2 = x.BillingAddressLine2, TaxNo2 = x.TaxNo2, FSSAI2 = x.FSSAI2, FSSAIValidUpTo2 = x.FSSAIValidUpTostr2, ContactName = x.ContactName, ContactEmail = x.ContactEmail, ContactMobNumber = x.ContactMobNumber, BankName = x.BankName, Branch = x.Branch, IFCCode = x.IFCCode, CustomerNote = x.CustomerNote, CellNo1 = x.CellNo1, CellNo2 = x.CellNo2, TelNo1 = x.TelNo1, TelNo2 = x.TelNo2, Email2 = x.Email2, Email1 = x.Email1 }).ToList();
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "RetailCustomerList.xls");
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

        [HttpGet]
        public ActionResult ManageCustomerArticleCode(Int64 CustomerGroupID, string CustomerGroupName, bool status)
        {
            try
            {
                ViewBag.Product = _customerservice.GetAllRetProductQtyList();
                RetCustomerArticleCodeListResponse objModel = _customerservice.GetRetailArticleCodeForCustomerGroup(Convert.ToInt64(CustomerGroupID));
                objModel.CustomerGroupID = CustomerGroupID;
                objModel.CustomerGroupName = CustomerGroupName;
                objModel.IsDelete = status;
                return View(objModel);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddProductArticleCode(RetCustomerArticleCodeListResponse data)
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
                    bool respose = false;
                    data.CustomerGroupID = data.CustomerGroupID;
                    data.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.CreatedOn = DateTime.Now;
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    respose = _customerservice.AddProductArticleCode(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewFSSAIExpireList()
        {
            List<RetCustomerListResponse> objModel = _customerservice.GetAllCustomerFSSAIExpireList();
            return View(objModel);
        }

        public ActionResult ExportExcelCustomerFSSAIExpireList()
        {
            var CustomerFSSAIExpireList = _customerservice.GetAllCustomerFSSAIExpireList();
            List<RetCustomerFSSAIExpireListExp> lstproduct = CustomerFSSAIExpireList.Select(x => new RetCustomerFSSAIExpireListExp() { CustomerName = x.CustomerName, DeliveryArea = x.DeliveryAreaName, BillingArea = x.BillingAreaName, SalesPerson = x.SalesPerson, FSSAI = x.FSSAI, FSSAIValidUpTo = x.FSSAIValidUpTostr, DaysRemaining = x.DaysRemaining, FSSAI2 = x.FSSAI2, FSSAIValidUpTo2 = x.FSSAIValidUpTostr2, DaysRemaining2 = x.DaysRemaining2, ContactNumber = x.ContactNumber, Email = x.ContactEmail }).ToList();
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "RetailCustomerFSSAIExpireList.xls");
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
        public ActionResult UpdateFSSAIDate(long CustomerID, DateTime? FSSAIValidUpTo, DateTime? FSSAIValidUpTo2)
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
                    bool respose = _customerservice.UpdateFSSAIDate(CustomerID, FSSAIValidUpTo, FSSAIValidUpTo2);
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