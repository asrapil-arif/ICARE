    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Net;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Configuration;
	using System.Reflection;
 

namespace I_Care.Classes
{
    public class SscComunication
    {
		private static readonly Encoding encoding = Encoding.UTF8;

		public class ResponseToken
        {

            public string response_function;
            public string response_code;
            public string response_message;
            public ResponseTokenData response_data;

        }

		public class ResponseFileNetToken
		{
			public string id;
			public string username;
			public string token;
			

		}

		public class ResponseTokenData
        {
            public string error_id;
            public string error_text;
            public string response_function;
            public ResultTokenData results;

        }


		public class ParamPostFile
		{
			public string FolderClass;
			public string Metadata_0_Field;
			public string Metadata_0_Value;
			public string response_function;
			public ResultTokenData results;

		}

		public class ResultTokenData
        {
            public string access_token;
            public int expired;

        }

        public class ResponseValidateVoucher
        {
            public string response_function;
            public string response_code;
            public string response_message;
            public ResponseVoucherData response_data;
            public string process_date;
        }

        public class ResponseVoucherData
        {
            public string error_id;
            public string error_text;
            public ResponseVoucher_result results;
        }

        public class ResponseVoucher_result {

            public string   evoucher_code;
            public decimal  evoucher_value;
            public string   evoucher_issue_date;
            public string   evoucher_expired_date;
            public string   process_date;

        }


        public class ResponseRedeemEVoucherNotify {

            public string response_function;
            public string response_code;
            public string response_message;
            public ResponseVoucherData response_data;
            public string process_date;
        
        }

		//public string svrurl = ConfigurationManager.AppSettings["EZEE_API_URL"].ToString();
		//public string MERCHANT_ID = ConfigurationManager.AppSettings["MERCHANT_ID"].ToString();
		//public string CLIENT_ID = ConfigurationManager.AppSettings["CLIENT_ID"].ToString();



			public string UserFileNet = ConfigurationManager.AppSettings["UserFileNet"].ToString();
		public string PwdFileNet = ConfigurationManager.AppSettings["PwdFileNet"].ToString();

		//public string TEMP_VOUCHER_ID_EZEE = ConfigurationManager.AppSettings["TEMP_VOUCHER_ID_EZEE"].ToString();
		//      public Int32 SET_CUSTOMER_ID_EZEE = Int32.Parse( ConfigurationManager.AppSettings["SET_CUSTOMER_ID_EZEE"].ToString());
		//      public string POOL_ID_EZEE = ConfigurationManager.AppSettings["POOL_ID_EZEE"].ToString();
		//      public string CUSTOMER_ID_EZEE = ConfigurationManager.AppSettings["CUSTOMER_ID_EZEE"].ToString();
        
        public int sCode;
        public string sMessage;
        public string companyID = "Pertamina Retail";

        public string FileNetLoginToken() {


			try
			{


				string Token = string.Empty;
				string sHostUrl = "http://ptmkpsvc16dev01.pertamina.com/filenet/api/Authentication/login";
				string sPostData = String.Format("Username={0}&Password={1}", UserFileNet, PwdFileNet);
				//HttpWebRequest xRequest = (HttpWebRequest)HttpWebRequest.Create(sHostUrl);
				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

				var httpRequest = (HttpWebRequest)WebRequest.Create(sHostUrl);
				httpRequest.Method = "POST";

				httpRequest.Accept = "application/json";
				httpRequest.ContentType = "application/json";
				httpRequest.Headers.GetType().InvokeMember("ChangeInternal", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, httpRequest.Headers, new object[] { "Host", "www.mysite.com" });
				var data = @"{""Username"":""finance"",""Password"": ""pertamina1*""}";


				using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
				{
					streamWriter.Write(data);
				}



				var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var result = streamReader.ReadToEnd();
					ResponseFileNetToken hasil_ = new ResponseFileNetToken();
					hasil_ = JsonConvert.DeserializeObject<ResponseFileNetToken>(result);
					Token = hasil_.token;
				}

		
				return Token;

			}
			catch (Exception ex) {

				return null;
			}


		}



		public string UploadDocumentFIPosting(string Token,byte[] data)
		{

			string result_;
			try
			{
			
				
				string transaction_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString().Replace(".", ":");
				string sHostUrl		= "http://ptmkpsvc16dev01.pertamina.com/filenet/api/documents";
				string sPostData = string.Empty;


				Dictionary<string, object> postParameters = new Dictionary<string, object>();
				postParameters.Add("FolderClass", "PTM_P2PFolder");
				postParameters.Add("DocClassName", "PTM_POInvoiceVIMDocs");
				postParameters.Add("FolderPath", "/P2P/1010/Invoice/PO");
				postParameters.Add("Metadata[0].Field", "DPPAmount");
				postParameters.Add("Metadata[0].Value", "1");
				postParameters.Add("Metadata[1].Field", "BMCTicketNumberT1");
				postParameters.Add("Metadata[1].Value", "4");
				postParameters.Add("Metadata[2].Field", "CompanyCode");
				postParameters.Add("Metadata[2].Value", "1010");
				postParameters.Add("Metadata[3].Field", "Currency");
				postParameters.Add("Metadata[3].Value", "IDR");
				postParameters.Add("Metadata[4].Field", "DocumentLocation");
				postParameters.Add("Metadata[4].Value", "/Finance/P2P/1010/Invoice/PO/202109");
				postParameters.Add("Metadata[5].Field", "DocumentSource");
				postParameters.Add("Metadata[5].Value", "Soft Copy");
				postParameters.Add("Metadata[6].Field", "HydroFlag");
				postParameters.Add("Metadata[6].Value", "No");
				postParameters.Add("Metadata[7].Field", "InvoiceAmount");
				postParameters.Add("Metadata[7].Value", "1");
				postParameters.Add("Metadata[8].Field", "InvoiceDate");
				postParameters.Add("Metadata[8].Value", "2021/02/02");
				postParameters.Add("Metadata[9].Field", "InvoiceNumber");
				postParameters.Add("Metadata[9].Value", "2");
				postParameters.Add("Metadata[10].Field", "NomorFaktur");
				postParameters.Add("Metadata[10].Value", "3");


				postParameters.Add("Metadata[11].Field", "NPWP");
				postParameters.Add("Metadata[11].Value", "4");
				postParameters.Add("Metadata[12].Field", "Penalty");
				postParameters.Add("Metadata[12].Value", "0");
				postParameters.Add("Metadata[13].Field", "PICTagihan");
				postParameters.Add("Metadata[13].Value", "5");
				postParameters.Add("Metadata[14].Field", "PONumber");
				postParameters.Add("Metadata[14].Value", "6");
				postParameters.Add("Metadata[15].Field", "BatchID");
				postParameters.Add("Metadata[15].Value", "1");
				postParameters.Add("Metadata[16].Field", "ReasonForReject");
				postParameters.Add("Metadata[16].Value", "7");
				postParameters.Add("Metadata[17].Field", "Retention");
				postParameters.Add("Metadata[17].Value", "8");
				postParameters.Add("Metadata[18].Field", "SAGRNumber");
				postParameters.Add("Metadata[18].Value", "9");
				postParameters.Add("Metadata[19].Field", "TanggalFakturPajak");
				postParameters.Add("Metadata[19].Value", "2021/07/01");
				postParameters.Add("Metadata[20].Field", "InvoiceStatus");
				postParameters.Add("Metadata[20].Value", "1");

				postParameters.Add("Metadata[21].Field", "From");
				postParameters.Add("Metadata[21].Value", "1");
				postParameters.Add("Metadata[22].Field", "EmsSubject");
				postParameters.Add("Metadata[22].Value", "1");
				postParameters.Add("Metadata[23].Field", "ReceivedOn");
				postParameters.Add("Metadata[23].Value", "2021/08/01");
				postParameters.Add("Metadata[24].Field", "AttachmentFileName");
				postParameters.Add("Metadata[24].Value", "a");
				postParameters.Add("Metadata[25].Field", "DueDate");
				postParameters.Add("Metadata[25].Value", "2021/08/02");
				postParameters.Add("Metadata[26].Field", "InvoiceList");
				postParameters.Add("Metadata[26].Value", "N");
				postParameters.Add("Metadata[27].Field", "Plant");
				postParameters.Add("Metadata[27].Value", "1");
				postParameters.Add("Metadata[28].Field", "PaymentApprovalID");
				postParameters.Add("Metadata[28].Value", "1");
				postParameters.Add("Metadata[29].Field", "Text");
				postParameters.Add("Metadata[29].Value", "1");
				postParameters.Add("Metadata[30].Field", "DocumentTitle");
				postParameters.Add("Metadata[30].Value", "1010-DOCUMENT-TAGIHAN-24092021-01");
				postParameters.Add("file", new FormUpload.FileParameter(data, "ivend.pdf", "application/pdf"));


				string postURL = sHostUrl;
				string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.14) Gecko/20080404 Firefox/2.0.0.14";
				HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters,Token);
				
				using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
				{
					var result = streamReader.ReadToEnd();
					result_ = result;
				}

				return result_;

			}
			catch (Exception ex)
			{
				writeLog("Error  : " + ex.InnerException);
				return "Error  : " + ex.InnerException;
				
			}
			finally
			{
				
				writeLog("Success Send File");
			}



		}
		

		public void writeLog(String msg)
		{
			try
			{
				string dir = "C:\\VMS_SERVICE\\EzeeLogs";
				if (!Directory.Exists(dir))
				{
					dir = Directory.GetCurrentDirectory() + "\\VMS_SERVICE\\EzeeLogs";
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