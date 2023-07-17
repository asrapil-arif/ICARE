using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I_Care.Classes;
using System.Data.SqlClient;
using System.Data;
using I_Care.ServiceSSO;


namespace I_Care.Classes
{
    public class CheckChangePassword : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string userId = HttpContext.Current.User.Identity.Name.ToString();

            int _CekUpdate = int.Parse(Koneksi.getScalarValue("Select PasswordChange from SSO.dbo.M_Users where UserId = '" + userId + "' "));
        
            if (_CekUpdate == 0)
            {

                HttpCookie cookie = new HttpCookie("change_password", "Password Anda belum pernah di update dari pertama kali login , demi keamanan akun Anda harap perbaharui password Anda !");
                filterContext.HttpContext.Response.SetCookie(cookie);

                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary{{ "controller", "User" },
                                          { "action", "ChangePassword" }

                                         });
            }
          

        }
    }
}