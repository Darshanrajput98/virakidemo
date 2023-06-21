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
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.purchase.Controllers
{
    public class AdminController : Controller
    {

        private IAdminService _IAdminservice;
        private ICommonService _ICommonService;
        private IProductService _productservice;


        public AdminController(IAdminService adminservice, ICommonService commonservice, IProductService productservice)
        {
            _IAdminservice = adminservice;
            _ICommonService = commonservice;
            _productservice = productservice;
        }

        //
        // GET: /purchase/Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddPurchaseType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPurchaseType(AddPurchaseType data)
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
                    PurchaseType_Mst obj = new PurchaseType_Mst();
                    obj.PurchaseTypeID = data.PurchaseTypeID;
                    obj.PurchaseType = data.PurchaseType;
                    if (obj.PurchaseTypeID == 0)
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
                    long respose = _IAdminservice.AddPurchaseType(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PurchaseTypeList()
        {
            List<PurchaseTypeListResponse> objModel = _IAdminservice.GetAllPurchaseTypeList();
            return PartialView(objModel);
        }

        public ActionResult DeletePurchaseType(long? PurchaseTypeID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeletePurchaseType(PurchaseTypeID.Value, IsDelete);
                return RedirectToAction("AddPurchaseType");
            }
            catch (Exception)
            {
                return RedirectToAction("AddPurchaseType");
            }
        }

        public ActionResult AddPurchaseDebitAccountType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPurchaseDebitAccountType(AddPurchaseDebitAccountType data)
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
                    PurchaseDebitAccountType_Mst obj = new PurchaseDebitAccountType_Mst();
                    obj.PurchaseDebitAccountTypeID = data.PurchaseDebitAccountTypeID;
                    obj.PurchaseDebitAccountType = data.PurchaseDebitAccountType;
                    if (obj.PurchaseDebitAccountTypeID == 0)
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
                    long respose = _IAdminservice.AddPurchaseDebitAccountType(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PurchaseDebitAccountTypeList()
        {
            List<PurchaseDebitAccountTypeListResponse> objModel = _IAdminservice.GetAllPurchaseDebitAccountTypeList();
            return PartialView(objModel);
        }

        public ActionResult DeletePurchaseDebitAccountType(long? PurchaseDebitAccountTypeID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeletePurchaseDebitAccountType(PurchaseDebitAccountTypeID.Value, IsDelete);
                return RedirectToAction("AddPurchaseDebitAccountType");
            }
            catch (Exception)
            {
                return RedirectToAction("AddPurchaseDebitAccountType");
            }
        }

        public ActionResult AddBroker()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBroker(AddBroker data)
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
                    Broker_Mst obj = new Broker_Mst();
                    obj.BrokerID = data.BrokerID;
                    obj.BrokerName = data.BrokerName;
                    if (obj.BrokerID == 0)
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
                    long respose = _IAdminservice.AddBroker(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult BrokerList()
        {
            List<BrokerListResponse> objModel = _IAdminservice.GetAllBrokerList();
            return PartialView(objModel);
        }

        public ActionResult DeleteBroker(long? BrokerID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeleteBroker(BrokerID.Value, IsDelete);
                return RedirectToAction("AddBroker");
            }
            catch (Exception)
            {
                return RedirectToAction("AddBroker");
            }
        }

        public ActionResult AddProduct()
        {
            ViewBag.Category = _productservice.GetAllCategoryName();
            ViewBag.Godown = _productservice.GetAllGodownName();
            ViewBag.Unit = _productservice.GetAllUnitName();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(AddPurchaseProduct data)
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
                    if (data.ProductID == 0)
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
                    long respose = _productservice.AddPurchaseProduct(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ProductList()
        {
            List<PurchaseProductListResponse> objModel = _productservice.GetAllPurchaseProductList();
            return PartialView(objModel);
        }

        public ActionResult DeleteProduct(long? ProductID, bool IsDelete)
        {
            try
            {
                _productservice.DeletePurchaseProduct(ProductID.Value, IsDelete);
                return RedirectToAction("AddProduct");
            }
            catch (Exception)
            {

                return RedirectToAction("AddProduct");
            }
        }

        [HttpPost]
        public ActionResult UpdateProduct(List<AddPurchaseProduct> data)
        {
            bool respose = _productservice.UpdatePurchaseProduct(data);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcelProduct()
        {
            var ProductList = _productservice.GetAllPurchaseProductList();
            List<ProductListForExp> lstproduct = ProductList.Select(x => new ProductListForExp() { CategoryName = x.CategoryName, ProductName = x.ProductName, HSNNumber = x.HSNNumber, SGST = x.SGST, CGST = x.CGST, IGST = x.IGST, HFor = x.HFor }).ToList();
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
                Response.AddHeader("content-disposition", "attachment;filename= " + "ProductList.xls");
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




        public ActionResult AddBank()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBank(AddBank data)
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
                    Bank_Mst obj = new Bank_Mst();
                    obj.BankID = data.BankID;
                    obj.BankName = data.BankName;
                    obj.Branch = data.Branch;
                    obj.IFSCCode = data.IFSCCode;
                    obj.AccountNumber = data.AccountNumber;
                    if (obj.BankID == 0)
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
                    long respose = _IAdminservice.AddBank(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult BankList()
        {
            List<BankListResponse> objModel = _IAdminservice.GetAllBankList();
            return PartialView(objModel);
        }

        public ActionResult DeleteBank(long? BankID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeleteBank(BankID.Value, IsDelete);
                return RedirectToAction("AddBank");
            }
            catch (Exception)
            {
                return RedirectToAction("AddBank");
            }
        }



        // 16 June 2021 Piyush Limbani
        public ActionResult AddTDSCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTDSCategory(AddPurchaseTDSCategory data)
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
                    PurchaseTDSCategory_Mst obj = new PurchaseTDSCategory_Mst();
                    obj.TDSCategoryID = data.TDSCategoryID;
                    obj.TDSCategory = data.TDSCategory;
                    if (obj.TDSCategoryID == 0)
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
                    long respose = _IAdminservice.AddPurchaseTDSCategory(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult TDSCategoryList()
        {
            List<PurchaseTDSCategoryListResponse> objModel = _IAdminservice.GetAllPurchaseTDSCategoryList();
            return PartialView(objModel);
        }

        public ActionResult DeleteTDSCategory(long? TDSCategoryID, bool IsDelete)
        {
            try
            {
                _IAdminservice.DeletePurchaseTDSCategory(TDSCategoryID.Value, IsDelete);
                return RedirectToAction("AddTDSCategory");
            }
            catch (Exception)
            {

                return RedirectToAction("AddTDSCategory");
            }
        }



    }
}