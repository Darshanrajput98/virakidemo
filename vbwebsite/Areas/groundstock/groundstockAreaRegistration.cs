using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vbwebsite.Areas.groundstock
{
    public class groundstockAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "groundstock";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "groundstock_default",
                "groundstock/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}