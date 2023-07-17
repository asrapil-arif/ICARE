using I_Care.ServiceSSO;
using I_Care.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace I_Care.Controllers
{
    public class UserController : BaseController
    {
        UserRoleProvider usr = new UserRoleProvider();

        [Authorize]
		public ActionResult Logout()
		{

			FormsAuthentication.SignOut();
			if (currentSessionUnits.ContainsKey(User.Identity.Name))
			{
				currentSessionUnits.Remove(User.Identity.Name);
			}
			return RedirectToAction("Login", "Home");
		}

        public ActionResult SignOn(FormCollection form)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            if (ws.ValidateUser(form["Username"], form["Password"]))
            {
                FormsAuthentication.SetAuthCookie(form["Username"], false);
                HttpCookie cookie = new HttpCookie("ErrorCookie", "");
                Response.SetCookie(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpCookie cookie = new HttpCookie("ErrorCookie", "Invalid Login");
                Response.SetCookie(cookie);
                return RedirectToAction("Login", "Home");
            }
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]

        [HttpPost]
        public ActionResult ChangePassword(FormCollection UserData)
        {

            string old_Password = UserData["OldPassword"];
            string new_password = UserData["NewPassword"];

            if (usr.ValidateOldUser(User.Identity.Name.ToString(), old_Password))
            {

                if (usr.ChangePassword(old_Password, new_password, User.Identity.Name.ToString()))
                {

                    ViewBag.Msg = "Change Password Success";
                    ViewBag.Process = "Success";
                    ViewBag.Alert = "success";
                }
                else
                {

                    ViewBag.Msg = "Please try again";
                    ViewBag.Process = "Change Password Fail ";
                    ViewBag.Alert = "danger";

                }

            }
            else
            {

                ViewBag.Msg = "Your old password is wrong !";
                ViewBag.Process = "Change Password Fail ";
                ViewBag.Alert = "danger";

            }



            return View();
        }
    }
}