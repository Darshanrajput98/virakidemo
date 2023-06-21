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
//using vb.Service.Interface;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productservice;


        public ProductController(IProductService productservice)
        {
            _productservice = productservice;

        }

        // GET: /wholesale/Product/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageProductCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageProductCategory(ProductCategoryViewModel data)
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
                    Category_Mst obj = new Category_Mst();
                    obj.CategoryID = data.CategoryID;
                    obj.CategoryCode = data.CategoryCode;
                    obj.CategoryTypeID = data.CategoryTypeID;
                    obj.CategoryName = data.CategoryName;
                    obj.CategoryDescription = data.CategoryDescription;
                    obj.CreatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.CreatedOn = DateTime.Now;
                    obj.IsDelete = false;
                    obj.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    obj.UpdatedOn = DateTime.Now;
                    bool respose = _productservice.AddProductCategory(obj);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ManageProductCategoryList()
        {
            List<ProductCategoryListResponse> objModel = _productservice.GetAllProductCategoryList();
            return PartialView(objModel);
        }

        public ActionResult DeleteProductCategory(long? CategoryID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteProductCategory(CategoryID.Value, IsDelete);
                return RedirectToAction("ManageProductCategory");
            }
            catch (Exception)
            {

                return RedirectToAction("ManageProductCategory");
            }
        }

        public ActionResult AddProduct()
        {
            ViewBag.Pouch = _productservice.GetAllPouchName();
            ViewBag.Category = _productservice.GetAllCategoryName();
            ViewBag.Godown = _productservice.GetAllGodownName();
            ViewBag.Unit = _productservice.GetAllUnitName();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel data)
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
                    data.CreatedOn = DateTime.Now;
                    data.UpdatedBy = Convert.ToInt64(Request.Cookies["UserID"].Value);
                    data.UpdatedOn = DateTime.Now;
                    bool respose = _productservice.AddProduct(data);
                    return Json(respose, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ProductList(string ProductPrice = "")
        {
            List<ProductListResponse> objModel = new List<ProductListResponse>();
            if (ProductPrice == "")
            {
                objModel = _productservice.GetAllProductList();
            }
            else
            {
                objModel = _productservice.GetAllProductListSearchByPrice(Convert.ToInt32(ProductPrice));
            }
            return PartialView(objModel);
        }

        public ActionResult GetProductQtyList(long ProductID)
        {
            List<ProductQtyViewModel> lstProductQty = _productservice.GetAllProductQtyListByProductID(ProductID);
            return Json(lstProductQty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteProduct(long? ProductID, bool IsDelete)
        {
            try
            {
                _productservice.DeleteProduct(ProductID.Value, IsDelete);
                return RedirectToAction("AddProduct");
            }
            catch (Exception)
            {

                return RedirectToAction("AddProduct");
            }
        }

        [HttpPost]
        public ActionResult UpdateProduct(List<ProductViewModel> data)
        {
            bool respose = _productservice.UpdateProduct(data);
            return Json(respose, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcelProduct()
        {
            var ProductList = _productservice.GetAllProductList();
            List<ProductListForExp> lstproduct = ProductList.Select(x => new ProductListForExp() { CategoryName = x.CategoryName, ProductName = x.ProductName, ProductPrice = x.ProductPrice, UnitCode = x.UnitCode, HSNNumber = x.HSNNumber, SGST = x.SGST, CGST = x.CGST, IGST = x.IGST, HFor = x.HFor }).ToList();
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

        //8 June,2021 Sonal Gandhi
        public ActionResult GetOnlineProductQtyList(long ProductID)
        {
            List<OnlineProductQty> lstOnlineProductQty = _productservice.GetAllOnlineProductQtyListByProductID(ProductID);
            return Json(lstOnlineProductQty, JsonRequestBehavior.AllowGet);
        }

    }
}