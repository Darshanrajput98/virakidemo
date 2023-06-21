using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vb.Data;
using vb.Service;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class SalesmanSummaryController : Controller
    {

        private IReportService _reportservice;


        public SalesmanSummaryController(IReportService reportservice)
        {
            _reportservice = reportservice;

        }
        //
        // GET: /wholesale/SalesmanSummary/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult salesmanindivuduallist(string UserID, DateTime Date)
        {
            string[] Id = UserID.Split('&');
            string date = Date.ToString("yyyy/MM/dd");
            List<DayWiseSalesList> objlst = _reportservice.GetDayWiseSalesListByUserID(Convert.ToInt64(Id[0]), date);
            return View(objlst);
        }
    }
}