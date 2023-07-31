using I_Care.Classes;
using I_Care.ServiceDoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Drawing.Charts;
using static I_Care.Controllers.BroadcastController;
using DataTable = System.Data.DataTable;

namespace I_Care.Controllers
{
    public class ReminderController : Controller
    {
        // GET: Reminder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainPage()
        {
            return View();
        }
        public ActionResult Detail()
        {
            return View();
        }
        public ActionResult getReminderList(string code, string combojson, string filterjson)
        {
            try
            {
                List<data_input> list = new List<data_input>
                {
                    new data_input() { Name = "userlogin", Value = User.Identity.Name.ToString()},
                    new data_input() { Name = "code", Value = code},
                     new data_input() { Name = "combojson", Value = combojson},
                    new data_input() { Name = "filterjson", Value = filterjson},
                };

                string paramJson = JsonConvert.SerializeObject(list);

                DataTable data = Koneksi.GetDataTable2("dbo.[genGetdata]", paramJson);

                string JSONresult = JsonConvert.SerializeObject(data);

                return new ContentResult
                {
                    Content = JSONresult,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", messege = ex.Message });
            }
        }

        public ActionResult Save(FormCollection Data)
        {
            SqlConnection SqlKOn = new SqlConnection();
            SqlKOn = Koneksi.GetKoneksi();
            SqlTransaction transaction;
            transaction = SqlKOn.BeginTransaction();
            int dataSave = 0;
            List<dynamic> res = new List<dynamic>();
            try
            {

                string code = Data["code"];
                string mode = Data["mode"];
                string headerData = Data["headerData"];
                string Detail = Data["detMail"];
                string formkey = Data["formkey"];
                string formkeyvalue = Data["formkeyvalue"];

                SqlCommand command = new SqlCommand();
                command = new SqlCommand("FMS.dbo.genSaveData", SqlKOn);

                command.Parameters.AddWithValue("@userlogin", User.Identity.Name);
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@header", headerData);
                command.Parameters.AddWithValue("@detail", Detail);
                command.Parameters.AddWithValue("@formkey", formkey);
                command.Parameters.AddWithValue("@formkeyvalue", formkeyvalue);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = SqlKOn;
                command.Transaction = transaction;


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string msg = reader.GetString(0);
                        string id = reader.GetString(1);

                        Dictionary<string, object> row = new Dictionary<string, object>();

                        row["msg"] = msg;
                        row["id"] = id;

                        res.Add(row);

                    }
                }

                if (res[0]["msg"].ToLower().Contains("Error"))
                {
                    transaction.Rollback();
                    return Json(new { Result = "Error : " + res[0].msg });
                }
                else
                {
                    transaction.Commit();

                    SqlKOn.Close();
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                if (dataSave != 1) transaction.Rollback();
                SqlKOn.Close();
                return Json(new { Result = "Error " + ex.Message });

            }
        }

    }
}