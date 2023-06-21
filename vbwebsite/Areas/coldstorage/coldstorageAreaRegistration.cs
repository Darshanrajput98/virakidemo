using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vbwebsite.Areas.coldstorage
{
    public class coldstorageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "coldstorage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "coldstorage_default",
                "coldstorage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}