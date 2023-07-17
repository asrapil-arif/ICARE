using I_Care.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;

namespace I_Care.Classes
{


    public class Libs : BaseController
    {
        public string changeDateSequence(string maxDate)
        {
            string[] date;

            date = maxDate.Split('-');
            if (date.Count() > 1)
                maxDate = date[2].ToString() + "-" + date[1].ToString() + "-" + date[0].ToString();

            date = maxDate.Split('/');
            if (date.Count() > 1)
                maxDate = date[2].ToString() + "/" + date[1].ToString() + "/" + date[0].ToString();

            return maxDate;
        }

        // change date to yyyy-mm-dd
        public string changeDateFormat(DateTime date)
        {
            string[] toDate;
            string dateResult = "";

            toDate = date.ToShortDateString().Split('/');
            if (toDate.Count() > 1)
            {
                if (int.Parse(toDate[0]) > 32)
                    dateResult = toDate[0].ToString() + "-" + toDate[1].ToString() + "-" + toDate[2].ToString();
                else
                    dateResult = toDate[2].ToString() + "-" + toDate[1].ToString() + "-" + toDate[0].ToString();
            }

            toDate = date.ToShortDateString().Split('-');
            if (toDate.Count() > 1)
            {
                if (int.Parse(toDate[0]) > 32)
                    dateResult = toDate[0].ToString() + "-" + toDate[1].ToString() + "-" + toDate[2].ToString();
                else
                    dateResult = toDate[2].ToString() + "-" + toDate[1].ToString() + "-" + toDate[0].ToString();
            }

            return dateResult;
        }

        // change yyyy/mm/dd --> yyyy-mm-dd
        public string changeDateSeparator(string maxDate)
        {
            return maxDate.ToString().Replace("/", "-");
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }


            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static void SendMailRegister(string Addres, Dictionary<string, string> Customers)
        {


            string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL"].ToString();
            string sBCC = ConfigurationManager.AppSettings["BCC"].ToString();
            string sendBCC = ConfigurationManager.AppSettings["SEND_BCC"].ToString();
            string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL"].ToString();
            string sAcc = ConfigurationManager.AppSettings["MailAccount"].ToString();
            string sPwd = ConfigurationManager.AppSettings["MailPassword"].ToString();
            string sSender = ConfigurationManager.AppSettings["MailSender"].ToString();
            string sAlias = ConfigurationManager.AppSettings["MailAlias"].ToString();
            string sSubject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            string baseurl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;


            string sMailBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "template/mail.html");
            string mailBody = sMailBody.Replace("@customerContact", Customers["ContactName"].ToUpper());
            mailBody = mailBody.Replace("@registerNumber", Customers["CustomerID"].ToUpper());
            mailBody = mailBody.Replace("@customerName", Customers["CustomerName"].ToUpper());
            mailBody = mailBody.Replace("@address", Customers["Alamat"].ToUpper());
            mailBody = mailBody.Replace("@desa", Customers["Desa"].ToUpper());
            mailBody = mailBody.Replace("@kecamatan", Customers["Kecamatan"].ToUpper());
            mailBody = mailBody.Replace("@kabupaten", Customers["Kabupaten"].ToUpper());
            mailBody = mailBody.Replace("@provinsi", Customers["Provinsi"].ToUpper());
            mailBody = mailBody.Replace("@kodepos", Customers["KodePos"].ToUpper());
            mailBody = mailBody.Replace("@telepon", Customers["Phone1"].ToUpper());
            mailBody = mailBody.Replace("@fax", Customers["Fax1"].ToUpper());
            mailBody = mailBody.Replace("@NPWP", Customers["NPWP"].ToUpper());
            mailBody = mailBody.Replace("@SIUP", Customers["SIUP"].ToUpper());
            mailBody = mailBody.Replace("@tanggal", Customers["RegisterDate"].ToUpper());

            MailMessage mail = new MailMessage();
            mail.To.Add(Addres);
            mail.Bcc.Add(sBCC);
            mail.From = new MailAddress(sSender, sAlias);
            mail.Subject = sSubject;
            mail.Body = mailBody;

            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            smtp.Port = Int32.Parse(sPortEmail); ;
            smtp.Host = sSMTP;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
            smtp.Send(mail);



        }

        public static void SendMailRejectToCustomer(string Addres, Dictionary<string, string> Customers)
        {


            string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL"].ToString();
            string sBCC = ConfigurationManager.AppSettings["BCC"].ToString();
            string sendBCC = ConfigurationManager.AppSettings["SEND_BCC"].ToString();
            string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL"].ToString();
            string sAcc = ConfigurationManager.AppSettings["MailAccount"].ToString();
            string sPwd = ConfigurationManager.AppSettings["MailPassword"].ToString();
            string sSender = ConfigurationManager.AppSettings["MailSender"].ToString();
            string sAlias = ConfigurationManager.AppSettings["MailAlias"].ToString();
            string sSubject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            string baseurl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;


            string sMailBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "template/mail_reject.htm");
            string mailBody = sMailBody.Replace("@customerContact", Customers["ContactName"].ToUpper());
            mailBody = mailBody.Replace("@alasan_reject", Customers["alasan"]);


            MailMessage mail = new MailMessage();
            mail.To.Add(Addres);
            mail.Bcc.Add(sBCC);
            mail.From = new MailAddress(sSender, sAlias);
            mail.Subject = sSubject;
            mail.Body = mailBody;

            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            smtp.Port = Int32.Parse(sPortEmail); ;
            smtp.Host = sSMTP;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
            smtp.Send(mail);



        }


        public static void SendMail(string Address, string sBCC, string Template, string sSubject)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL"].ToString();
            //string sBCC = ConfigurationManager.AppSettings["BCC"].ToString();
            string sendBCC = ConfigurationManager.AppSettings["SEND_BCC"].ToString();
            string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL"].ToString();
            string sAcc = ConfigurationManager.AppSettings["MailAccount"].ToString();
            string sPwd = ConfigurationManager.AppSettings["MailPassword"].ToString();
            string sSender = ConfigurationManager.AppSettings["MailSender"].ToString();
            string sAlias = ConfigurationManager.AppSettings["MailAlias"].ToString();
            //string sSubject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            string baseurl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string SSL = ConfigurationManager.AppSettings["SSL"].ToString();
            Boolean sSSL = true;

            if (SSL == "1")
            {

                sSSL = true;
            }
            else
            {
                sSSL = false;

            }


            MailMessage mail = new MailMessage();
            mail.To.Add(Address);

            if (sBCC != "")
            {

                mail.Bcc.Add(sBCC);

            }

            mail.From = new MailAddress(sSender, sAlias);
            mail.Subject = sSubject;
            mail.Body = Template;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = sSSL;
            smtp.Port = Int32.Parse(sPortEmail);
            smtp.Host = sSMTP;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
            smtp.Send(mail);

        }

        public static void SendAlternateMail(string Address, string Template, string sSubject)
        {

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL_ALT"].ToString();
            string sBCC = ConfigurationManager.AppSettings["BCC_ALT"].ToString();
            string sendBCC = ConfigurationManager.AppSettings["SEND_BCC_ALT"].ToString();
            string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL_ALT"].ToString();
            string sAcc = ConfigurationManager.AppSettings["MailAccount_ALT"].ToString();
            string sPwd = ConfigurationManager.AppSettings["MailPassword_ALT"].ToString();
            string sSender = ConfigurationManager.AppSettings["MailSender_ALT"].ToString();
            string sAlias = ConfigurationManager.AppSettings["MailAlias_ALT"].ToString();
            //string sSubject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            string baseurl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string SSL = ConfigurationManager.AppSettings["SSL_ALT"].ToString();
            Boolean sSSL = true;

            if (SSL == "1")
            {

                sSSL = true;
            }
            else
            {
                sSSL = false;

            }


            MailMessage mail = new MailMessage();
            mail.To.Add(Address);

            if (sendBCC != "")
            {

                mail.Bcc.Add(sBCC);
                //mail.CC.Add(sBCC);

            }

            mail.From = new MailAddress(sSender, sAlias);
            mail.Subject = sSubject;
            mail.Body = Template;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = sSSL;
            smtp.Port = Int32.Parse(sPortEmail);
            smtp.Host = sSMTP;
            smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
            smtp.Send(mail);

        }



        public static void SendAlternateMailUltah(string Address, string Template, string sSubject)
        {

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string sPortEmail = ConfigurationManager.AppSettings["PORT_EMAIL_ALT"].ToString();
            string sBCC = ConfigurationManager.AppSettings["BCC_ALT"].ToString();
            string sendBCC = ConfigurationManager.AppSettings["SEND_BCC_ALT"].ToString();
            string sSMTP = ConfigurationManager.AppSettings["SMTP_MAIL_ALT"].ToString();
            string sAcc = ConfigurationManager.AppSettings["MailAccount_ALT"].ToString();
            string sPwd = ConfigurationManager.AppSettings["MailPassword_ALT"].ToString();
            string sSender = ConfigurationManager.AppSettings["MailSender_ALT"].ToString();
            string sAlias = "Pertamina Retail";
            //string sSubject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            string baseurl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string SSL = ConfigurationManager.AppSettings["SSL_ALT"].ToString();
            Boolean sSSL = true;

            if (SSL == "1")
            {

                sSSL = true;
            }
            else
            {
                sSSL = false;

            }


            MailMessage mail = new MailMessage();
            mail.To.Add(Address);
            mail.IsBodyHtml = true;

            if (sendBCC != "")
            {

                mail.Bcc.Add(sBCC);
                //mail.CC.Add(sBCC);

            }

            mail.From = new MailAddress(sSender, sAlias);
            mail.Subject = sSubject;
            mail.Body = Template;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = sSSL;
            smtp.Port = Int32.Parse(sPortEmail);
            smtp.Host = sSMTP;
            smtp.Credentials = new System.Net.NetworkCredential(sAcc, sPwd);
            smtp.Send(mail);

        }




    }
}