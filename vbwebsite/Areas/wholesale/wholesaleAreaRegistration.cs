using System.Web.Mvc;

namespace vbwebsite.Areas.wholesale
{
    public class wholesaleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "wholesale";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "wholesale_default",
                "wholesale/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index",id = UrlParameter.Optional },
                new[] { "vbwebsite.Areas.wholesale.Controllers" }
            );
        }
    }
}