
namespace vbwebsite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using vb.Data;
    using vb.Data.Model;
    using vb.Data.ViewModel;
    using vb.Service;
    using WebMatrix.WebData;

    public class LoginController : Controller
    {
        //private ILogin _loginservice;

        //public LoginController(ILogin loginservice)
        //{
        //    _loginservice = loginservice;
        //}

        private IAdminService _areaservice;
        public LoginController(IAdminService areaservice)
        {
            _areaservice = areaservice;
        }

        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            Response.Cookies["UserID"].Value = null;
            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["UserName"].Value = null;
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["GodownName"].Value = null;
            Response.Cookies["GodownName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["GodownID"].Value = null;
            Response.Cookies["GodownID"].Expires = DateTime.Now.AddDays(-1);
            Session.Abandon();
            Session["Logincheck"] = "false";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login data)
        {
            if (data != null & !string.IsNullOrEmpty(data.username) & !string.IsNullOrEmpty(data.password))
            {
                bool success = WebSecurity.Login(data.username, data.password, false);
                if (success)
                {
                    string UserID = Membership.GetUser(data.username).ProviderUserKey.ToString();
                    // int UserID1 = WebSecurity.GetUserId(User.Identity.Name);                 
                    if (UserID == "-1")
                    {
                        return RedirectToAction("Login", "Login");
                    }
                    LoginResponse responsedata = _areaservice.GetUserDetails(data.username);
                    if (responsedata.UserID != 0)
                    {
                        string returnUrl = Request.QueryString["ReturnUrl"];
                        if (Request.Cookies["UserID"] == null || Request.Cookies["UserID"].Value != UserID.ToString())
                        {
                            Response.Cookies["UserID"].Value = UserID.ToString();
                            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(2);
                            Response.Cookies["ProfilePicture"].Value = responsedata.ProfilePicturePath.ToString();
                            Response.Cookies["ProfilePicture"].Expires = DateTime.Now.AddDays(2);
                        }
                        if (Request.Cookies["UserName"] == null || Request.Cookies["UserID"].Value != UserID.ToString())
                        {
                            Response.Cookies["UserName"].Value = responsedata.UserName.ToString();
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(2);
                        }
                        if (Request.Cookies["GodownName"] == null || Request.Cookies["UserID"].Value != UserID.ToString())
                        {
                            Response.Cookies["GodownName"].Value = responsedata.GodownName.ToString();
                            Response.Cookies["GodownName"].Expires = DateTime.Now.AddDays(2);
                        }
                        if (Request.Cookies["GodownID"] == null || Request.Cookies["UserID"].Value != UserID.ToString())
                        {
                            Response.Cookies["GodownID"].Value = responsedata.GodownID.ToString();
                            Response.Cookies["GodownID"].Expires = DateTime.Now.AddDays(2);
                        }
                        //if (Request.Cookies["ProfilePicture"] == null || Request.Cookies["UserID"].Value != UserID.ToString())
                        //{
                        //    Response.Cookies["ProfilePicture"].Value = responsedata.ProfilePicturePath.ToString();
                        //    Response.Cookies["ProfilePicture"].Expires = DateTime.Now.AddDays(2);
                        //}

                        // Get the IP
                        // string ip = Request.UserHostAddress;

                        if (returnUrl == null)
                        {
                            return RedirectToAction("Index", "Home", new { area = "wholesale" });
                        }
                        else
                        {
                            Response.Redirect(returnUrl);
                        }
                    }
                }

                //LoginResponse responsedata = _loginservice.CheckLogin(data);
                //if (responsedata != null && responsedata.UserID > 0)
                //{
                //    try
                //    {
                //        if (Request.Cookies["UserID"] == null || Request.Cookies["UserID"].Value != responsedata.UserID.ToString())
                //        {
                //            Response.Cookies["UserID"].Value = responsedata.UserID.ToString();
                //            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(2);
                //        }
                //        if (Request.Cookies["UserName"] == null || Request.Cookies["UserID"].Value != responsedata.UserID.ToString())
                //        {
                //            Response.Cookies["UserName"].Value = responsedata.UserName.ToString();
                //            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(2);
                //        }
                //        if (Request.Cookies["GodownName"] == null || Request.Cookies["UserID"].Value != responsedata.UserID.ToString())
                //        {
                //            Response.Cookies["GodownName"].Value = responsedata.GodownName.ToString();
                //            Response.Cookies["GodownName"].Expires = DateTime.Now.AddDays(2);
                //        }
                //    }
                //    catch
                //    {
                //        Response.Cookies["UserID"].Value = responsedata.UserID.ToString();
                //        Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(2);
                //        Response.Cookies["UserName"].Value = responsedata.UserName.ToString();
                //        Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(2);
                //        Response.Cookies["GodownName"].Value = responsedata.GodownName.ToString();
                //        Response.Cookies["GodownName"].Expires = DateTime.Now.AddDays(2);
                //    }
                //    return RedirectToAction("Index", "Home", new { area = "wholesale" });
                //}
                //Session["Logincheck"] = "true";
            }
            return View(data);
            //  return View();
        }



        //[HttpPost]
        //public ActionResult Login(Login data)
        //{
        //    if (data != null & !string.IsNullOrEmpty(data.UserName) & !string.IsNullOrEmpty(data.Password))
        //    {
        //        LoginResponse responsedata = _loginservice.CheckLogin(data);
        //        if (responsedata != null && responsedata.UserID > 0)
        //        {              
        //            try
        //            {
        //                if (Request.Cookies["UserID"] == null || Request.Cookies["UserID"].Value != responsedata.UserID.ToString())
        //                {
        //                    Response.Cookies["UserID"].Value = responsedata.UserID.ToString();
        //                    Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(2);
        //                }
        //                if (Request.Cookies["UserName"] == null || Request.Cookies["UserID"].Value != responsedata.UserID.ToString())
        //                {
        //                    Response.Cookies["UserName"].Value = responsedata.UserName.ToString();
        //                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(2);
        //                }
        //                if (Request.Cookies["GodownName"] == null || Request.Cookies["UserID"].Value != responsedata.UserID.ToString())
        //                {
        //                    Response.Cookies["GodownName"].Value = responsedata.GodownName.ToString();
        //                    Response.Cookies["GodownName"].Expires = DateTime.Now.AddDays(2);
        //                }
        //            }
        //            catch
        //            {
        //                Response.Cookies["UserID"].Value = responsedata.UserID.ToString();
        //                Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(2);
        //                Response.Cookies["UserName"].Value = responsedata.UserName.ToString();
        //                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(2); 
        //                Response.Cookies["GodownName"].Value = responsedata.GodownName.ToString();
        //                Response.Cookies["GodownName"].Expires = DateTime.Now.AddDays(2);
        //            }
        //            return RedirectToAction("Index", "Home", new { area = "wholesale" });
        //        }
        //        Session["Logincheck"] = "true";
        //    }
        //    return View();
        //}






    }
}