﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;
using vbwebsite.App_Start;

namespace vbwebsite.Areas.attendance.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /attendance/Home/
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult PageNotFound()
        {
            return View();
        }

        private ICommonService _ICommonService;
        //
        public HomeController(ICommonService ICommonService)
        {
            _ICommonService = ICommonService;
        }

        // GET: /attendance/Home/
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
            objMenu = _ICommonService.DynamicMenuMaster_RoleWiseMenuList(RoleId, "attendance");
            return PartialView(objMenu);
        }

	}
}