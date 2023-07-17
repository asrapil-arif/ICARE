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
	public class AuthorizeRolesAttribute : AuthorizeAttribute
	{

		public string Modul { get; set; }
		public string User { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			string UserLogin = HttpContext.Current.User.Identity.Name.ToString();
	

            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                if (ws.GetAllowModul(UserLogin, Roles, 1) == true)
                {

                    return true;
                }
                else {

                    HttpCookie mycookie = new HttpCookie("unauthorize_error", "Anda tidak diizinkan untuk mengakses menu yang baru saja Anda Pilih !");
                    httpContext.Response.SetCookie(mycookie);
                    httpContext.Response.Redirect("~/");
                    return false;
                }
            }
            finally
            {
                ws.Close();
            }

        }
	}
}