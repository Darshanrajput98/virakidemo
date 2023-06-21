using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vbwebsite.Areas.expenses
{
    public class expensesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "expenses";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "expenses_default",
                "expenses/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}