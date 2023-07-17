using I_Care.Classes;
using I_Care.Models;
using I_Care.ServiceDoc;
using I_Care.ServiceSSO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using Spire.Doc;
using SautinSoft.Document;
using ClosedXML;
using ClosedXML.Excel;

namespace I_Care.Controllers
{
    [Authorize]
    public class FileController : BaseController
    {
        public class FileListUpload
        {
            public int ID { get; set; }
        }


        public ActionResult ViewAttachment(int Id)
        {

            string SQL = string.Format("exec FMS.dbo.procGetCurrentFile '{0}'", Id);
            DataTable tblFile = Koneksi.GetDataTable(SQL);

            string source = tblFile.Rows[0]["Source"].ToString();
            string key1 = tblFile.Rows[0]["Key1"].ToString(); ;
            int key3 = int.Parse(tblFile.Rows[0]["Key3"].ToString());
            int key4 = int.Parse(tblFile.Rows[0]["Key4"].ToString());
            int revision = int.Parse(tblFile.Rows[0]["Revision"].ToString());
            int dl = int.Parse(tblFile.Rows[0]["IsDeleted"].ToString());
            string cntype = tblFile.Rows[0]["ContentType"].ToString();
            string fn = tblFile.Rows[0]["FileName"].ToString();
            string fnOrg = tblFile.Rows[0]["OrgFileName"].ToString();

            ServiceDocSoapClient ws = new ServiceDocSoapClient();
            byte[] data = ws.GetFileDataById(Id, "apps_docman", "user@default");
            if (data != null)
            {
                MemoryStream str = new MemoryStream(data);
                if (dl == 1) {
                    return File(str, cntype, fn);
                }
                else
                {
                    if (cntype == "application/unknown")
                    {
                        cntype = GetMimeType(fn);
                        return File(str, cntype, fnOrg);
                    }
                    else
                    {

                        return File(str, cntype);
                    }
                }
                
            }

            throw new Exception("Attachment Not Found!");
        }


        public ActionResult ViewAttachment_2(FormCollection Param)
        {

            int Id = int.Parse(Param["Id"].ToString());
            string SQL = string.Format("exec FMS.dbo.procGetCurrentFile '{0}'", Id);
            DataTable tblFile = Koneksi.GetDataTable(SQL);

            string source = tblFile.Rows[0]["Source"].ToString();
            string key1 = tblFile.Rows[0]["Key1"].ToString(); ;
            int key3 = int.Parse(tblFile.Rows[0]["Key3"].ToString());
            int key4 = int.Parse(tblFile.Rows[0]["Key4"].ToString());
            int revision = int.Parse(tblFile.Rows[0]["Revision"].ToString());
            int dl = int.Parse(tblFile.Rows[0]["IsDeleted"].ToString());
            string cntype = tblFile.Rows[0]["ContentType"].ToString();
            string fn = tblFile.Rows[0]["FileName"].ToString(); ;

            ServiceDocSoapClient ws = new ServiceDocSoapClient();
            byte[] data = ws.GetFileDataById(Id, "apps_docman", "user@default");

            if (data != null)
            {
                MemoryStream str = new MemoryStream(data);
                if (dl == 1) return File(str, cntype, fn);
                else return File(str, cntype);
            }


            throw new Exception("Attachment Not Found!");
        }



        public ActionResult UploadFilePre(FormCollection Data)
        {

            try
            {
                string FileCategory = Data["FileCategory"];
                string Source = Data["Source"];
                string Key1 = Data["Key1"];
                string Key2 = Data["Key2"];
                string Key3 = Data["Key3"];
                string Remark = Data["Remark"];

                if (Source == null) Source = "I-CARE";
                if (Key1 == null) Key1 = "ICARE_CONTRACT";
                if (Key2 == null) Key2 = "UPLOAD_FILE_PRE";
                if (Key3 == null) Key3 = "0";

                ServiceDocSoapClient ws = new ServiceDocSoapClient();

                foreach (string file in Request.Files)
                {

                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        var len = fileContent.ContentLength;
                        string filename = System.IO.Path.GetFileName(fileContent.FileName);
                        string filetype = System.IO.Path.GetExtension(filename);
                        string ContentType = GetMimeType(filename);
                        string FlagCrud = Key2;
                        string Tgl = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string FileName_ = "";

                        FileName_ = Key2 + User.Identity.Name.ToString().Replace(".", "_") + "_" + Tgl + filetype;

                        Stream str = fileContent.InputStream;
                        BinaryReader binReader = new BinaryReader(str);
                        byte[] data = binReader.ReadBytes((int)str.Length);
                        string result_ = ws.PutFile3(Source, Key1, Key2, int.Parse(Key3), 0, FileCategory, 0, FileName_.ToLower(), ContentType,Remark, User.Identity.Name.ToString(), data, filename);
                        dynamic results = JsonConvert.DeserializeObject<dynamic>(result_);
                        var id = results.Result;
                        return Json(new { Result = id.Value });

                    }
                }

                return Json(new { Result = "Error File Not Attach" });

            }
            catch (Exception ex)
            {

                return Json(new { Result = "Error " + ex.Message });

            }
        }

        public ActionResult DeleteFilePre(FormCollection Data)
        {

            try
            {
                ServiceDocSoapClient ws = new ServiceDocSoapClient();
                string FileUploadList = Data["DeleteList"];
                List<FileListUpload> FileUpload_ = new List<FileListUpload>();
                FileUpload_ = JsonConvert.DeserializeObject<List<FileListUpload>>(FileUploadList);

                foreach (var baris in FileUpload_)
                {
                    string result_ = ws.DeleteFilePreById(baris.ID, "apps_docman", "user@default", User.Identity.Name.ToString());

                }

                return Json(new { Result = "Success" });

            }
            catch (Exception ex)
            {

                return Json(new { Result = "Error " + ex.Message });

            }
        }

        public ActionResult DeleteFileContractLocalIcare(FormCollection Data)
        {
            try
            {
                string FileUploadList = Data["DeleteList"];
                List<FileListUpload> FileUpload_ = new List<FileListUpload>();
                FileUpload_ = JsonConvert.DeserializeObject<List<FileListUpload>>(FileUploadList);

                foreach (var baris in FileUpload_)
                {
                    string FileName = "exec FMS.dbo.procDeleteContractLocalAttachment @Id = '" + baris.ID + "',@UserName='" + User.Identity.Name.ToString() + "'";

                    string strPhysicalFolder = Server.MapPath("..\\File_Contract\\");
                    string strFileFullPath = strPhysicalFolder + FileName;

                    if (System.IO.File.Exists(strFileFullPath))
                    {
                        System.IO.File.Delete(strFileFullPath);
                    }

                }

                return Json(new { Result = String.Format("Success", "Yes") });

            }
            catch (Exception ex)
            {
                return Json(new { Result = String.Format("Error", ex.Message.ToString()) });
            }
           
        }
    }
}
