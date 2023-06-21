using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vbwebsite.Areas.purchase
{
    public class purchaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "purchase";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "purchase_default",
                "purchase/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}