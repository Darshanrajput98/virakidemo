

namespace vbwebsite.Areas.export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class exportAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get
            {
                return "export";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "export_default",
                "export/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "vbwebsite.Areas.export.Controllers" }
            );
        }
    }
}