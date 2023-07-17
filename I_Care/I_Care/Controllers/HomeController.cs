using I_Care.ServiceSSO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using I_Care.Classes;
using System.Web.SessionState;

namespace I_Care.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {


          

            if (Request.IsAuthenticated)
            {
                return View();
                //int _CekUpdate = int.Parse(Koneksi.getScalarValue("Select PasswordChange from SSO.dbo.M_Users where UserId = '" + User.Identity.Name.ToString() + "' "));
                //if (_CekUpdate > 0)
                //{

                //    return View();
                //}
                //else
                //{
                //    HttpCookie cookie = new HttpCookie("change_password", "Password Anda belum pernah di update dari pertama kali login , harap perbaharui password Anda !");
                //    HttpContext.Response.SetCookie(cookie);
                //    return RedirectToAction("ChangePassword", "User");
                //}
            }
            else
            {
                return View("Login");
               
            }
        }
        public ActionResult Login()
        {
            return View();
        }

       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}