using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;
using vbwebsite.App_Start;

namespace vbwebsite.Areas.export.Controllers
{
    public class HomeController : Controller
    {
        private ICommonService _ICommonService;
        //
        public HomeController(ICommonService ICommonService)
        {
            _ICommonService = ICommonService;
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
            objMenu = _ICommonService.DynamicMenuMaster_RoleWiseMenuList(RoleId, "export");
            return PartialView(objMenu);
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
	}
}