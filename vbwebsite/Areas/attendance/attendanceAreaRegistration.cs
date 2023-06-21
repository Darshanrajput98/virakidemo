using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vbwebsite.Areas.attendance
{
    public class attendanceAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get
            {
                return "attendance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "attendance_default",
                "attendance/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}