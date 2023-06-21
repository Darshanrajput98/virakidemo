using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Data.Model;
using vb.Service;

namespace vbwebsite.Areas.coldstorage.Controllers
{
    public class ReportController : Controller
    {
        private static object Lock = new object();
        private ICommonService _ICommonService;
        private IProductService _ProductService;
        private IColdStorageService _ColdStorageService;
        private IPurchaseService _PurchaseService;
        private IAdminService _AdminService;

        public ReportController(ICommonService ICommonService, IAdminService AdminService, IPurchaseService PurchaseService, IProductService ProductService, IColdStorageService ColdStorageService)
        {
            _ICommonService = ICommonService;
            _ProductService = ProductService;
            _ColdStorageService = ColdStorageService;
            _PurchaseService = PurchaseService;
            _AdminService = AdminService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockReport()
        {
            ViewBag.Product = _PurchaseService.GetAllProductName();
            ViewBag.ColdStorage = _ColdStorageService.GetAllColdStorageName();
            return View();
        }

        //08-07-2022
        public PartialViewResult StockReportList(StockSearchRequest model)
        {
            List<StockReportResponseList> objModel = _ColdStorageService.GetAllColdStorage_StockReportList(model.ProductID, model.ColdStorageID, model.ToDate);
            return PartialView(objModel);
        }

    }
}