using I_Care.Classes;
using I_Care.Models;
using I_Care.ServiceDoc;
using I_Care.ServiceSSO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Globalization;
using SautinSoft.Document;
using System.Net.Mail;
using System.Reflection;
using System.Configuration;


namespace I_Care.Controllers
{
    [Authorize]
    //[CheckChangePassword]
    [AuthorizeRolesAttribute(Roles = "MOD2022090001")]//check allow broadcast management menu
    public class BroadcastController : BaseController
    {
        public class FileListUpload
        {
            public int ID { get; set; }
        }
        public class data_input
        {

            public string Name { get; set; }
            public string Value { get; set; }

        }
        public class Jsons
        {
            public int ID { get; set; }
        }
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
        public ActionResult getFungsi()
        {
            DataTable dtTable = new DataTable();
            dtTable = Koneksi.GetDataTable2("FMS.dbo.procGetFungsi");
            string JSONresult = JsonConvert.SerializeObject(dtTable);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };


        }
        public ActionResult getCountBroadcastStatus()
        {
            try
            {


                List<data_input> list = new List<data_input>
                {
                     new data_input() { Name = "Username", Value = User.Identity.Name.ToString()},
                };
                string paramJson = JsonConvert.SerializeObject(list);
                DataTable data = Koneksi.GetDataTable2("FMS.dbo.procGetCountBroadcastStatus", paramJson);
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
        public ActionResult getBroadcastList(string Field, string Keyword)
        {
            try
            {
                List<data_input> list = new List<data_input>
                {
                    new data_input() { Name = "Field", Value = Field},
                    new data_input() { Name = "Keyword", Value = Keyword},
                    new data_input() { Name = "Username", Value = User.Identity.Name.ToString()},
                };

                string paramJson = JsonConvert.SerializeObject(list);
                DataTable data = Koneksi.GetDataTable2("dbo.[procGetBroadcastList]", paramJson);

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
        public ActionResult getBroadcastListByStatus(string status)
        {
            try
            {
                List<data_input> list = new List<data_input>
                {
                    new data_input() { Name = "Status", Value = status},
                     new data_input() { Name = "Username", Value = User.Identity.Name.ToString()},
                };

                string paramJson = JsonConvert.SerializeObject(list);

                DataTable data = Koneksi.GetDataTable2("dbo.[procGetBroadcastListByStatus]", paramJson);

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
        public ActionResult getBroadcastAttachment(string Id)
        {
            try
            {
                List<data_input> list = new List<data_input>
                {
                    new data_input() { Name = "Id", Value = Id},
                };

                string paramJson = JsonConvert.SerializeObject(list);
                DataTable data = Koneksi.GetDataTable2("FMS.dbo.[procGetBroadcastAttachment]", paramJson);

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
        public ActionResult getFungsiList()
        {
            try
            {
                DataTable data = Koneksi.GetDataTable2("[procGetFungsiList]");

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

        public ActionResult getBroadcastLog(string Id)
        {
            DataTable dtTable = new DataTable();
            List<SqlParam_> Param = new List<SqlParam_>();
            Param.Add(new SqlParam_ { Name = "@BroadcastId", Value = Id });
            dtTable = Koneksi.GetDataTable2("FMS.dbo.procGetBroadcastLog", JsonConvert.SerializeObject(Param));
            string JSONresult = JsonConvert.SerializeObject(dtTable);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }
        public ActionResult SaveBroadcast(FormCollection Data)
        {
            SqlConnection SqlKOn = new SqlConnection();
            SqlKOn = Koneksi.GetKoneksi();
            SqlTransaction transaction;
            transaction = SqlKOn.BeginTransaction();
            int dataSave = 0;

            try
            {
              
                string FlagCrud = Data["FlagCrud"];
                string FormTitle = Data["Title"];
                string FormFungsi = Data["Fungsi"];
                string FormStartDate = Data["StartDate"];
                string FormEndDate = Data["EndDate"];
                string FormDescriptions = Data["Description"];
                string FileUploadList = Data["FileUploadList"];
                string Remark = Data["Remark"];
                string Tautan = Data["Tautan"];
                int BroadcastId = int.Parse(Data["BroadcastId"].ToString());

                SqlCommand command = new SqlCommand();
                command = new SqlCommand("FMS.dbo.procSaveBroadcast", SqlKOn);
                command.Parameters.Add("@FlagCrud", SqlDbType.VarChar).Value = FlagCrud; //
                command.Parameters.Add("@BroadcastId", SqlDbType.Int).Value = BroadcastId;
                command.Parameters.Add("@Title", SqlDbType.VarChar).Value = FormTitle; //
                command.Parameters.Add("@Fungsi", SqlDbType.Decimal).Value = FormFungsi; //
                command.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = FormStartDate; //
                command.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = FormEndDate; //
                command.Parameters.Add("@Descriptions", SqlDbType.VarChar).Value = FormDescriptions; //
                command.Parameters.Add("@FileUploadList", SqlDbType.VarChar).Value = FileUploadList; //
                command.Parameters.Add("@Remark", SqlDbType.VarChar).Value = Remark; //
                command.Parameters.Add("@Tautan", SqlDbType.VarChar).Value = Tautan; //
                command.Parameters.Add("@Username", SqlDbType.VarChar).Value = User.Identity.Name; //
                command.Parameters.Add("@outputVal", SqlDbType.VarChar, 800).Direction = ParameterDirection.Output; //
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = SqlKOn;
                command.Transaction = transaction;
                command.ExecuteNonQuery();

                String result = command.Parameters["@outputVal"].Value.ToString();

                if (result.ToLower().Contains("Error"))
                {
                    transaction.Rollback();
                    return Json(new { Result = "Error : " + result });
                }
                else
                {
                    transaction.Commit();
                    dataSave = 1;
                    ServiceDocSoapClient ws = new ServiceDocSoapClient();
                    List<FileListUpload> FileUpload_ = new List<FileListUpload>();
                    FileUpload_ = JsonConvert.DeserializeObject<List<FileListUpload>>(FileUploadList);
                    if (FileUpload_ != null)
                    {
                        foreach (var baris in FileUpload_)
                        {
                            if (baris.ID != null) { string result_ = ws.MoveFileById(baris.ID, "apps_docman", "user@default", User.Identity.Name.ToString(), "UPLOAD_FILE_PRE"); }
                        }
                    }
                    SqlKOn.Close();
                    return Json(new { Result = "OK" });
                }

            }
            catch (Exception ex)
            {
                if (dataSave != 1) transaction.Rollback();
                SqlKOn.Close();
                return Json(new { Result = "Error " + ex.Message });

            }
        }


        public ActionResult TestSendBroadcast(FormCollection Data)
        {
            try
            {
                ServiceDocSoapClient ws = new ServiceDocSoapClient();
                int Id = int.Parse(Data["Id"].ToString());
                string Title = Data["Title"].ToString();
                string Address = Data["Address"].ToString();
                string EmbededLink = Data["EmbededLink"].ToString();


                DataTable dtFile = Koneksi.GetDataTable("exec FMS.dbo.procGetMainContentBroadcastFileId @Id='" + Id.ToString() + "'");
                DataTable dtLampiran = Koneksi.GetDataTable("exec FMS.dbo.procGetBroadcastAttachmentForSend @Id='" + Id.ToString() + "'");
               
                byte[] data_image = ws.GetFileDataById(int.Parse(dtFile.Rows[0]["ID"].ToString()), "apps_docman", "user@default");
                string path_content = "D:\\Web_Applications\\I_Care\\BroadcastFile\\" + dtFile.Rows[0]["FileName"].ToString();
                //string path_content = "D:\\BroadcastFile\\" + dtFile.Rows[0]["FileName"].ToString();

                using (var ms = new MemoryStream(data_image))
                {
                    using (var fs = new FileStream(path_content, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                        fs.Dispose();
                    }
                }

                string htmlBody = string.Empty;

                var linkedResource = new LinkedResource(@"" + path_content + "", System.Net.Mime.MediaTypeNames.Image.Jpeg);

                if (EmbededLink != "null")
                {
                    htmlBody = $"<a href='{EmbededLink}'><img src=\"cid:{linkedResource.ContentId}\"/></a>";
                }
                else
                {
                    htmlBody = $"<img src=\"cid:{linkedResource.ContentId}\"/>";
                }

               
                var alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, System.Net.Mime.MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL_BROADCAST"].ToString();
                string sBCC = ConfigurationManager.AppSettings["BCC_BROADCAST"].ToString();
                string sendBCC = ConfigurationManager.AppSettings["SEND_BCC_BROADCAST"].ToString();
                string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL_BROADCAST"].ToString();
                string sAcc = ConfigurationManager.AppSettings["MailAccount_BROADCAST"].ToString();
                string sPwd = ConfigurationManager.AppSettings["MailPassword_BROADCAST"].ToString();
                string sSender = ConfigurationManager.AppSettings["MailSender_BROADCAST"].ToString();
                string sAlias = "Broadcast Pertamina Retail";
                string SSL = ConfigurationManager.AppSettings["SSL"].ToString();
                Boolean sSSL = true;

                if (SSL == "1") { sSSL = true; }
                else { sSSL = false; }
                MailMessage mail = new MailMessage();
                //mail.To.Add(sSender);

                foreach (DataRow row in dtLampiran.Rows)
                {
                    //TextBox1.Text = row["ImagePath"].ToString();
                    byte[] data_attachment = ws.GetFileDataById(int.Parse(row["ID"].ToString()), "apps_docman", "user@default");

                    // TODO: Choose a better name for your attachments and adapt the MIME type
                    var messageAttachment = new Attachment(new MemoryStream(data_attachment), row["OrgFileName"].ToString());
                    mail.Attachments.Add(messageAttachment);
                }

                mail.From = new MailAddress(sSender, sAlias);
                mail.Subject = Title;
                mail.Priority = MailPriority.High;
                mail.AlternateViews.Add(alternateView);
                mail.IsBodyHtml = true;
                mail.Bcc.Add(Address);
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = sSSL;
                smtp.Port = Int32.Parse(sPortEmail);
                smtp.Host = sSMTP;
                smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
                smtp.Send(mail);

                linkedResource.Dispose();
                alternateView.Dispose();

                if (System.IO.File.Exists(path_content)) System.IO.File.Delete(path_content);

                return Json(new { Result = "Ok", messege = "Ok" });
            }
            catch (Exception ex)
            {
                writeLog("Error :" + ex.Message);
                return Json(new { Result = "Error", messege = ex.Message });
            }
        }
         

        public ActionResult ForceSend(FormCollection Data)
        {
            try
            {
                ServiceDocSoapClient ws = new ServiceDocSoapClient();
                int Id = int.Parse(Data["Id"].ToString());
                string Title = Data["Title"].ToString();
                string Address = Koneksi.getScalarValue("exec FMS.dbo.procGetBroadcastMailList");
                string EmbededLink = Data["EmbededLink"].ToString();
                DataTable dtFile = Koneksi.GetDataTable("exec FMS.dbo.procGetMainContentBroadcastFileId @Id='" + Id.ToString() + "'");
                DataTable dtLampiran = Koneksi.GetDataTable("exec FMS.dbo.procGetBroadcastAttachmentForSend @Id='" + Id.ToString() + "'");
               
                byte[] data_image = ws.GetFileDataById(int.Parse(dtFile.Rows[0]["ID"].ToString()), "apps_docman", "user@default");
                string path_content = "D:\\Web_Applications\\I_Care\\BroadcastFile\\" + dtFile.Rows[0]["FileName"].ToString();
                //string path_content = "D:\\BroadcastFile\\" + dtFile.Rows[0]["FileName"].ToString();

                using (var ms = new MemoryStream(data_image))
                {
                    using (var fs = new FileStream(path_content, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                        fs.Dispose();
                    }
                }

                string htmlBody = string.Empty;

                var linkedResource = new LinkedResource(@"" + path_content + "", System.Net.Mime.MediaTypeNames.Image.Jpeg);

                if (EmbededLink != "null")
                {
                    htmlBody = $"<a href='{EmbededLink}'><img src=\"cid:{linkedResource.ContentId}\"/></a>";
                }
                else
                {
                    htmlBody = $"<img src=\"cid:{linkedResource.ContentId}\"/>";
                }

               

                var alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, System.Net.Mime.MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL_BROADCAST"].ToString();
                string sBCC = ConfigurationManager.AppSettings["BCC_BROADCAST"].ToString();
                string sendBCC = ConfigurationManager.AppSettings["SEND_BCC_BROADCAST"].ToString();
                string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL_BROADCAST"].ToString();
                string sAcc = ConfigurationManager.AppSettings["MailAccount_BROADCAST"].ToString();
                string sPwd = ConfigurationManager.AppSettings["MailPassword_BROADCAST"].ToString();
                string sSender = ConfigurationManager.AppSettings["MailSender_BROADCAST"].ToString();
                string sAlias = "Broadcast Pertamina Retail";
                string SSL = ConfigurationManager.AppSettings["SSL"].ToString();
                Boolean sSSL = true;

                if (SSL == "1") { sSSL = true; }
                else { sSSL = false; }
                MailMessage mail = new MailMessage();
                //mail.To.Add(sSender);

                foreach (DataRow row in dtLampiran.Rows)
                {
                    //TextBox1.Text = row["ImagePath"].ToString();
                    byte[] data_attachment = ws.GetFileDataById(int.Parse(row["ID"].ToString()), "apps_docman", "user@default");
                  
                   // TODO: Choose a better name for your attachments and adapt the MIME type
                    var messageAttachment = new Attachment(new MemoryStream(data_attachment), row["OrgFileName"].ToString());
                    mail.Attachments.Add(messageAttachment);
                }


                mail.From = new MailAddress(sSender, sAlias);
                mail.Subject = Title;
                mail.Priority = MailPriority.High;
                mail.AlternateViews.Add(alternateView);
                mail.IsBodyHtml = true;
                mail.Bcc.Add(Address);
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = sSSL;
                smtp.Port = Int32.Parse(sPortEmail);
                smtp.Host = sSMTP;
                smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
                smtp.Send(mail);

                linkedResource.Dispose();
                alternateView.Dispose();

                if (System.IO.File.Exists(path_content)) System.IO.File.Delete(path_content);
                
                SqlConnection SqlKOn = new SqlConnection();
                SqlKOn = Koneksi.GetKoneksi();
                SqlCommand command = new SqlCommand("FMS.dbo.procInsertBroadcastLogSend", SqlKOn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@BroadcastId", SqlDbType.Int).Value = Id; //0
                command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = User.Identity.Name.ToString(); //1
                command.Connection = SqlKOn;
                command.ExecuteNonQuery();

                return Json(new { Result = "Ok", messege = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", messege = ex.Message });
            }
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