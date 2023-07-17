using I_Care.ServiceSSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace I_Care.Classes
{
    public class UserRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                return "i-Care";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                string[] result = ws.GetRolesForUser(username, "I-Care").ToArray();
                return result;
            }
            finally
            {
                ws.Close();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                return ws.GetAllRoles(ApplicationName).ToArray();
            }
            finally
            {
                ws.Close();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                return ws.GetUsersInRole(ApplicationName, roleName).ToArray();
            }
            finally
            {
                ws.Close();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                return ws.IsUserInRole(username, ApplicationName, roleName);
            }
            finally
            {
                ws.Close();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
        }

        public override bool RoleExists(string roleName)
        {
            return GetAllRoles().Contains(roleName);
        }

        public bool CheckAllowModul(string userid, string modul, int access)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                return ws.GetAllowModul(userid, modul, access);
            }
            finally
            {
                ws.Close();
            }
        }


        public bool CheckAllowExtendAccess(string userid, int Extid)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                return ws.GetAccesExtend(userid, Extid);
            }
            finally
            {
                ws.Close();
            }
        }


        public string GetUserType(string userid)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                string result = ws.GetUserType(userid);
                return result;
            }
            finally
            {
                ws.Close();
            }
        }


        public bool ValidateOldUser(string username, string Password)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                bool result = ws.ValidateUser(username, Password);
                return result;
            }
            finally
            {
                ws.Close();
            }
        }

        public bool ChangePassword(string OdlPass, string NewPass, string UserId)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                bool result = ws.ChangePassword(OdlPass, NewPass, UserId);
                return result;
            }
            finally
            {
                ws.Close();
            }
        }


        public bool CheckAccGroupIstrue(string userid, string AccGroup)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                return ws.CheckAccGroupUser(userid, AccGroup);
            }
            finally
            {
                ws.Close();
            }
        }

    }
}