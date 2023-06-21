using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;

namespace vbwebsite.Areas.retail.Controllers
{
    public class SalesmanSummaryController : Controller
    {
        private IRetReportService _reportservice;

        public SalesmanSummaryController(IRetReportService reportservice)
        {
            _reportservice = reportservice;
        }

        // GET: /retail/SalesmanSummary/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult salesmanindivuduallist(string UserID, DateTime Date)
        {
            string[] Id = UserID.Split('&');
            string date = Date.ToString("yyyy/MM/dd");
            List<RetDayWiseSalesList> objlst = _reportservice.GetDayWiseSalesListByUserID(Convert.ToInt64(Id[0]), date);
            return View(objlst);
        }

    }
}