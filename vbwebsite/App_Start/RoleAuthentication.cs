using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using vb.Data;
using vb.Service;
using WebMatrix.WebData;
namespace vbwebsite.App_Start
{
    public class RoleAuthentication : System.Web.Mvc.ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
            string areaName = filterContext.RouteData.DataTokens["area"].ToString();

            base.OnActionExecuting(filterContext);

            var role = System.Web.Security.Roles.GetRolesForUser().FirstOrDefault();
            //MembershipUser currentUser = Membership.GetUser();
            // var role1 = System.Web.Security.Roles.GetRolesForUser(HttpContext.Current.Response.Cookies["UserName"].Value);

            var allRoles = System.Web.Security.Roles.GetAllRoles();

            if (role != null)
            {
                IAuthorizeAccess _authorizeservice = new AuthorizeAccessServicecs();

                int RoleID = _authorizeservice.AuthorizeMaster_CheckAuthorizeAccess(role.ToString(), controllerName, actionName, areaName);

                HttpContext.Current.Session["RoleId"] = RoleID.ToString();
                //bool success = false;
                if (RoleID != 0)
                {

                    List<RoleWiseActiveAuthorizedManuList> objMenuList = _authorizeservice.GetAllRoleWiseActiveManuList(RoleID);
                    HttpContext.Current.Session["ActiveMenuList"] = objMenuList;
                    return;
                }
                else
                {
                    // need to redirect unauthorize access page oops moment!!!!!!! ;)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "PageNotFound"
                    }));
                    return;
                }

            }
            else
            {
                if (controllerName == "Account" && actionName == "Login")
                {
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "Login"
                    }));
                    return;
                }
            }
        }

    }
}