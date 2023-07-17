using I_Care.Models;
using I_Care.ServiceSSO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;

namespace I_Care.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        public class SqlParam_
        {

            public string Name { get; set; }
            public string Value { get; set; }

        }

        public FMSEntities db = new FMSEntities();

        public static SSOWSSoapClient ws = new SSOWSSoapClient();
        //public bool isAdmin = Roles.IsUserInRole("SYSADMIN");
        public static Dictionary<String, String> currentSessionUnits = new Dictionary<String, String>();

        [Serializable]
        public class objEmailDestination
        {
            public int Id { get; set; }
            public string ContractId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Jabatan { get; set; }

            public string Fungsi { get; set; }


        }

        protected ContentResult JsonContent(object result)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return new ContentResult
            {
                Content = serializer.Serialize(result),
                ContentType = "application/json"
            };
        }

        public string getSessionUnit(string userName)
        {
            SSOWSSoapClient ws = new SSOWSSoapClient();
            try
            {
                if (!currentSessionUnits.ContainsKey(userName))
                {
                    try
                    {
                        currentSessionUnits.Add(userName, ws.GetUserUnit(userName));
                    }
                    catch
                    {
                        currentSessionUnits[userName] = ws.GetUserUnit(userName);
                    }
                }
                return currentSessionUnits[userName];
            }
            finally
            {
                ws.Close();
            }
        }


        public decimal parseDecimal(string s)
        {
            return decimal.Parse(s, new CultureInfo("en-US"));
        }

        public double parseDouble(string s)
        {
            return double.Parse(s, new CultureInfo("en-US"));
        }


        public double getHourDiff(DateTime a, DateTime b)
        {
            return b.Subtract(a).TotalHours;
        }

		public string GetMimeType(string fileName)
		{
			string mimeType = "application/unknown";
			string ext = System.IO.Path.GetExtension(fileName).ToLower();
			Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
			if (regKey != null && regKey.GetValue("Content Type") != null)
				mimeType = regKey.GetValue("Content Type").ToString();
			return mimeType;
		}

        public void writeLog(String msg)
        {
            try
            {
                string dir = "D:\\Web_Applications\\I_Care\\Logs";

                if (!Directory.Exists(dir))
                {
                    dir = Directory.GetCurrentDirectory() + "\\Logs";
                }
                StreamWriter w = new StreamWriter(dir + "\\log_" + DateTime.Today.ToString("yyyyMMdd") + ".log", true);
                w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + msg);
                w.Close();
            }
            catch
            {
            }
        }


    }
}