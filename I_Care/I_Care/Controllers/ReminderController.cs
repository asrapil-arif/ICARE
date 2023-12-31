﻿using I_Care.Classes;
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
using System.Net.Mail;
using System.Net;
using System.IO;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Spire.Doc;
using DocumentFormat.OpenXml.Bibliography;
using System.Configuration;

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

                DataTable data = Koneksi.GetDataTable2("dbo.[procGetReminderList]", paramJson);

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
                command = new SqlCommand("FMS.dbo.procSaveReminder", SqlKOn);

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

        public ActionResult SendMail(FormCollection Data)
        {
            try
            {

                // Pengaturan informasi akun email pengirim (akun Gmail)
                string smtpServer = ConfigurationManager.AppSettings["smtpHost"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]); ; // Port TLS
                string emailFrom = ConfigurationManager.AppSettings["smtpUserName"];
                string emailPassword = ConfigurationManager.AppSettings["smtpPassword"];
                string fromEmailAddress = ConfigurationManager.AppSettings["fromEmailAddress"];

                // Pengaturan email penerima
                string emailTo = Data["recipient"];

                string filePath = "/obj/Release/Package/PackageTmp/Template/reminder/reminder.htm"; // Replace with the path to your HTML file

                string absoluteFilePath = AppDomain.CurrentDomain.BaseDirectory + filePath;
                
                string htmlBody = System.IO.File.ReadAllText(absoluteFilePath);
                
                htmlBody = htmlBody.Replace("@tanggal", DateTime.Now.ToString("dd/MM/yyyy"));

                htmlBody = htmlBody.Replace("@name", Data["MailName2"]);
                htmlBody = htmlBody.Replace("@refrence1", Data["ReminderRef1"]);
                htmlBody = htmlBody.Replace("@refrence2", Data["ReminderRef2"]);
                htmlBody = htmlBody.Replace("@refrence3", Data["ReminderRef3"]);
                htmlBody = htmlBody.Replace("@remark", Data["Remark"]);

                // Membuat objek MailMessage untuk mengatur email
                //MailMessage mail = new MailMessage(emailFrom, emailTo);
                //mail.From = new MailAddress(fromEmailAddress);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmailAddress);
                mail.To.Add(emailTo);
                mail.Subject = Data["ReminderName"];
                mail.Body = htmlBody;
                mail.IsBodyHtml = true;

                // Pengaturan koneksi ke server SMTP (Gmail)
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true; // Gunakan SSL (TLS)
                smtpClient.Credentials = new NetworkCredential(emailFrom, emailPassword);

                // Mengirim email
                smtpClient.Send(mail);

                return Json(new { Result = "Success", message = "\"Email berhasil dikirim!\"" }, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", message = "Terjadi kesalahan saat mengirim email: " + ex.Message }, JsonRequestBehavior.AllowGet);
               
            }
        }

        public ActionResult CheckSchedule()
        {
            try
            {
                DataTable data = Koneksi.GetDataTable2("dbo.[procgetreminderlistmail]");

                // Pengaturan informasi akun email pengirim (akun Gmail)
                string smtpServer = ConfigurationManager.AppSettings["smtpHost"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]); ; // Port TLS
                string emailFrom = ConfigurationManager.AppSettings["smtpUserName"];
                string emailPassword = ConfigurationManager.AppSettings["smtpPassword"];
                string fromEmailAddress = ConfigurationManager.AppSettings["fromEmailAddress"];

                foreach (DataRow row in data.Rows)
                {
                    string filePath = "/obj/Release/Package/PackageTmp/Template/reminder/reminder.htm"; // Replace with the path to your HTML file

                    string absoluteFilePath = AppDomain.CurrentDomain.BaseDirectory + filePath;

                    string htmlBody = System.IO.File.ReadAllText(absoluteFilePath);

                    htmlBody = htmlBody.Replace("@tanggal", DateTime.Now.ToString("dd/MM/yyyy"));

                    htmlBody = htmlBody.Replace("@tanggal", DateTime.Now.ToString("dd/MM/yyyy"));

                    htmlBody = htmlBody.Replace("@name", row["MailName"].ToString());
                    htmlBody = htmlBody.Replace("@refrence1", row["ReminderRef1"].ToString());
                    htmlBody = htmlBody.Replace("@refrence2", row["ReminderRef2"].ToString());
                    htmlBody = htmlBody.Replace("@refrence3", row["ReminderRef3"].ToString());
                    htmlBody = htmlBody.Replace("@remark", row["Remark"].ToString());

                    // Pengaturan email penerima
                    string emailTo = row["MailAddress"].ToString();

                    // Membuat objek MailMessage untuk mengatur email
                    MailMessage mail = new MailMessage(emailFrom, emailTo);

                    string subject = row["ReminderName"].ToString();
                    //string body = "";

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true;

                    // Pengaturan koneksi ke server SMTP (Gmail)
                    SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true; // Gunakan SSL (TLS)
                    smtpClient.Credentials = new NetworkCredential(emailFrom, emailPassword);

                    // Mengirim email
                    smtpClient.Send(mail);
                    
                }

                return Json(new { Result = "Success", message = "\"Success!\"" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", messege = ex.Message });
            }
        }
    }
}