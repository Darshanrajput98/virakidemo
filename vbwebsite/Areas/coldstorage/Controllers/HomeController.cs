using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;
using vbwebsite.App_Start;

namespace vbwebsite.Areas.coldstorage.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /coldstorage/Home/
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult PageNotFound()
        {
            return View();
        }

        private ICommonService _ICommonService;
        private IColdStorageService _ColdStorageService;
        //
        public HomeController(ICommonService ICommonService, IColdStorageService IColdStorageService)
        {
            _ICommonService = ICommonService;
            _ColdStorageService = IColdStorageService;
        }

        // GET: /coldstorage/Home/
        [RoleAuthentication]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult MenuList()
        {
            int RoleId = Utility.GetUserRoleId();
            //int RoleId = 1;
            DynamicMenuModel objMenu = new DynamicMenuModel();
            objMenu = _ICommonService.DynamicMenuMaster_RoleWiseMenuList(RoleId, "coldstorage");
            return PartialView(objMenu);
        }

        [HttpGet]
        public PartialViewResult ExpiryDateWiseStockReportList()
        {
            List<StockReportResponseList> objModel = _ColdStorageService.MonthAgoExpiryDateWiseGetColdStorage_StockReportList();
            return PartialView(objModel);
        }

    }
}