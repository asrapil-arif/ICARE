﻿using I_Care.Classes;
using I_Care.Models;
using I_Care.ServiceDoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace I_Care.Controllers
{
    [Authorize]
    [AuthorizeRolesAttribute(Roles = "MOD2021110004")]//check allow contract archive menu
    public class EmployeeManagementController : BaseController
    {
        // GET: EmployeeManagement
        public ActionResult Index()
        {
            return View();
        }
        public class _LevelJabatanList
        {
            public int Id { get; set; }
            public string LevelJabatan { get; set; }

        }
        public class _KedudukanList
        {
            public int Id { get; set; }
            public string Kedudukan { get; set; }

        }
       
        public ActionResult EmployeeManagement()
        {
            string SQL = "select Id,LevelJabatan from dbo.m_level_jabatan";
            DataTable tblDataStat2 = Koneksi.GetDataTable(SQL);

            List<_LevelJabatanList> _LevelJabatanList = new List<_LevelJabatanList>();

            foreach (DataRow row in tblDataStat2.Rows)
            {
                _LevelJabatanList model = new _LevelJabatanList
                {
                    Id = (int)row[0],
                    LevelJabatan = (string)row[1],
                };
                _LevelJabatanList.Add(model);
            }

            ViewBag.LevelJabatanList = _LevelJabatanList.ToArray();

            string SQL1 = "select Id, KedudukanName from fms.dbo.[m_kedudukan]";
            DataTable tblDataStat1 = Koneksi.GetDataTable(SQL1);

            List<_KedudukanList> _KedudukanList = new List<_KedudukanList>();

            foreach (DataRow row in tblDataStat1.Rows)
            {
                _KedudukanList model = new _KedudukanList
                {
                    Id = (int)row[0],
                    Kedudukan = (string)row[1],
                };
                _KedudukanList.Add(model);
            }

            ViewBag.KedudukanList = _KedudukanList.ToArray();

            string SQLList = "exec FMS.dbo.procCrudEmployeeManagement @Type =  4";
            DataTable tblDataList = Koneksi.GetDataTable(SQLList);
            if (tblDataList.Rows.Count != 0)
            {

                return View();

            }


            else
            {
                return View();
            };



        }
      
        public ContentResult FindEmployeeByStatus(string status)
        {
            //string SQL = "exec FMS.dbo.procFindContractCustomerByStatus @Type =  4";
            string SQL = "exec FMS.dbo.procFindEmployeeByStatus @Type = '" + status.ToString() + "'";

            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }

    
        public ContentResult FindEmployee(string KeyWords, string Field)
        {


            string SQL = "exec FMS.dbo.procFindEmployee @KeyWords = '" + KeyWords.ToString() + "',@Field= '" + Field.ToString() + "' ";

            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }

        

        public ActionResult SaveEmployeeManagement(FormCollection Contract)
        {
            try
            {


                string nl = System.Environment.NewLine;

                string SQL = "exec FMS.dbo.[procCrudEmployeeManagement] @Type = 1 " + nl;
                SQL = SQL + ",@Flag_Crud = '" + Contract["Flag_Crud"].ToString() + "'" + nl;
                SQL = SQL + ",@EmployeeId = '" + Contract["EmployeeId"].ToString() + "'" + nl;
                SQL = SQL + ",@Nopek = '" + Contract["Nopek"].ToString() + "'" + nl;
                SQL = SQL + ",@NamaPekerja = '" + Contract["NamaPekerja"].ToString() + "'" + nl;
                SQL = SQL + ",@Jabatan = '" + Contract["Jabatan"].ToString() + "'" + nl;
                SQL = SQL + ",@LevelJabatan = '" + Contract["LevelJabatan"].ToString() + "'" + nl;
                SQL = SQL + ",@TanggalLahir = '" + Contract["TanggalLahir"].ToString() + "'" + nl;
                SQL = SQL + ",@Kedudukan = '" + Contract["Kedudukan"].ToString() + "'" + nl;
                SQL = SQL + ",@AlertDay = '" + Contract["AlertDay"].ToString() + "'" + nl;
                SQL = SQL + ",@UserName = '" + User.Identity.Name.ToString() + "'" + nl;
                DataTable tblData = Koneksi.GetDataTable(SQL);

                if (tblData.Rows[0]["Result"].ToString().Contains("Error"))
                {
                    return Json(new { Result = tblData.Rows[0]["Result"].ToString() });
                }
                else
                {

                    string NewCode = string.Empty;
                    string contractid = string.Empty;

                    int xc = 1;

                    if (Contract["Flag_Crud"].ToString() == "ADD")
                    {

                        NewCode = tblData.Rows[0]["Result"].ToString();
                    }
                    else
                    {
                        contractid = Contract["EmployeeId"].ToString();
                    }



                    foreach (string file in Request.Files)
                    {
                        var fileContent = Request.Files[file];
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {

                            string Id = NewCode + "_" + xc.ToString();//CustomerId;
                            var len = fileContent.ContentLength;
                            string filename = System.IO.Path.GetFileName(fileContent.FileName);
                            string filetype = System.IO.Path.GetExtension(filename);
                            string filesNames = Id + filename;

                            var path = Path.Combine(Server.MapPath("~/File_Contract"), filesNames);
                            fileContent.SaveAs(path);

                            xc = xc + 1;

                            SQL = "exec FMS.dbo.ProcCrudT_Attachment_EmployeeManagement @Type = 1 " + nl;
                            SQL = SQL + ",@CandidateId_1 = '" + NewCode + "'" + nl;
                            SQL = SQL + ",@FileName = '" + filesNames + "'" + nl;
                            SQL = SQL + ",@UserId = '" + User.Identity.Name.ToString() + "'" + nl;

                            Koneksi.execQuery(SQL);

                            t_attachtment_employeemanagement att = db.t_attachtment_employeemanagement.FirstOrDefault(t => t.CandidateId_1.ToString() == NewCode.ToString() && t.FileName == filesNames);

                           

                        }
                    }



                    string ListAdd = Contract["ListAdd"].ToString();
                    List<objEmailDestination> _ListAddEmail = JsonConvert.DeserializeObject<List<objEmailDestination>>(ListAdd);

                    string ListDelete = Contract["ListDelete"].ToString();
                    List<objEmailDestination> _ListDeleteExt = JsonConvert.DeserializeObject<List<objEmailDestination>>(ListDelete);


                    string SQL_Mail = string.Empty;
                    string Contractmail = string.Empty;

                    if (Contract["Flag_Crud"].ToString() == "ADD")
                    {

                        Contractmail = NewCode;

                    }
                    else
                    {
                        Contractmail = Contract["EmployeeId"].ToString();
                    }


                    //add new mail destinations
                    foreach (objEmailDestination RwD in _ListAddEmail)
                    {

                        SQL_Mail = "exec FMS.dbo.ProcCrudMailAlertEmployeeManagement" + nl;
                        SQL_Mail = SQL_Mail + " @Type = 1" + nl;
                        SQL_Mail = SQL_Mail + ",@MailId = '" + RwD.Id + "'" + nl;
                        SQL_Mail = SQL_Mail + ",@EmployeeId = '" + Contractmail + "'" + nl;
                        SQL_Mail = SQL_Mail + ",@Email = '" + RwD.Email + "'" + nl;
                        SQL_Mail = SQL_Mail + ",@Name = '" + RwD.Name + "'" + nl;
                        SQL_Mail = SQL_Mail + ",@Jabatan = '" + RwD.Jabatan + "'" + nl;
                        SQL_Mail = SQL_Mail + ",@FungsiEmail = '" + RwD.Fungsi + "'" + nl;
                        SQL_Mail = SQL_Mail + ",@UserId= '" + User.Identity.Name.ToString() + "'";

                        Koneksi.execQuery(SQL_Mail);

                    }

                    //delete mail destinations
                    foreach (objEmailDestination RwDel in _ListDeleteExt)
                    {

                        SQL_Mail = "exec FMS.dbo.[ProcCrudMailAlertEmployeeManagement] " + nl;
                        SQL_Mail = SQL_Mail + " @Type = 2" + nl;
                        SQL_Mail = SQL_Mail + ",@MailId = " + RwDel.Id.ToString() + "" + nl;

                        Koneksi.execQuery(SQL_Mail);

                    }

                    return Json(new { Result = tblData.Rows[0]["Result"].ToString() });
                }

            }
            catch (Exception ex)
            {

                return Json(new { Result = "Error " + ex.Message });

            }



        }


        public JsonResult DeleteEmployee(int EmployeeId)
        {
            try
            {
                string SQL_ = "exec FMS.[dbo].[procCrudEmployeeManagement] @Type=3,@EmployeeId='" + EmployeeId + "',@UserName='" + User.Identity.Name.ToString() + "'";
                Koneksi.execQuery(SQL_);
                return Json(new { Result = String.Format("Success", "Yes") });

            }
            catch (Exception)
            {
                return Json(new { Result = String.Format("Error", "No") });
            }

        }

        public JsonResult ProcessUploadFileContract(FormCollection Item)
        {

            string ID = Item["EmployeeId"];
            string nl = System.Environment.NewLine;
            string MaxCounter = Koneksi.getScalarValue("select Isnull(count(*),0) + 1 as MaxCount from  t_attachtment_stakeholder  where CandidateId_1 ='" + ID + "'");


            int xc = int.Parse(MaxCounter);
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        string Id = ID + "_" + xc.ToString();//contractId
                        var len = fileContent.ContentLength;
                        string filename = System.IO.Path.GetFileName(fileContent.FileName);
                        string filetype = System.IO.Path.GetExtension(filename);
                        string filesNames = Id + filename;
                        var path = Path.Combine(Server.MapPath("~/File_Contract"), filesNames);
                        fileContent.SaveAs(path);
                        xc = xc + 1;

                        string SQL;
                        SQL = "exec FMS.dbo.ProcCrudT_Attachment_EmployeeManagement @Type = 1 " + nl;
                        SQL = SQL + ",@CandidateId_1 = '" + ID + "'" + nl;
                        SQL = SQL + ",@FileName = '" + filesNames + "'" + nl;
                        SQL = SQL + ",@UserId = '" + User.Identity.Name.ToString() + "'" + nl;

                        Koneksi.execQuery(SQL);

                        ServiceDocSoapClient ws = new ServiceDocSoapClient();
                        t_attachtment_employeemanagement att = db.t_attachtment_employeemanagement.First(t => t.CandidateId_1 == ID.ToString() && t.FileName == filesNames);

                        Stream str = fileContent.InputStream;
                        BinaryReader binReader = new BinaryReader(str);
                        byte[] data = binReader.ReadBytes((int)str.Length);

                        string result = ws.PutFile("I-Care", "EmployeeManagement", att.CandidateId_1,
                            att.Id, 0, "ICARE_CONTRACT", 0, filesNames, "", "", att.CreateUser, data);
                        if (result != "")
                        {
                            throw new Exception(result);
                        }
                    }
                }
                //trans.Commit();
                return Json(new { Result = String.Format("OK", "Ok") });
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                return Json(new { Result = String.Format("Error", "No") });
            }


        }

        public ContentResult ContractListMailAlertEmployee(int? contract)
        {

            string SQL = "select id as recid,*,'exists' FlagStat from fms.dbo.t_cont_alert_dest_mailEmployeeManagement where EmployeeId ='" + contract + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }

        public ContentResult T_Attacment(int? ContractId)
        {

            string sqlTemp = "select Id recid,*,'Icare' API from FMS.dbo.t_attachtment_employeemanagement  where CandidateId_1 = '" + ContractId + "'";

            DataTable tblTemp = Koneksi.GetDataTable(sqlTemp);
            string stepId = string.Empty;
            string stepName = string.Empty;

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblTemp);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }


        public JsonResult DeleteAttachmentContract(int id)
        {
            try
            {
                string fileName = "";
                t_attachtment_employeemanagement ct = db.t_attachtment_employeemanagement.FirstOrDefault(s => s.Id == id);
                fileName = ct.FileName;

                string strPhysicalFolder = Server.MapPath("..\\File_Contract\\");
                string strFileFullPath = strPhysicalFolder + fileName;

                if (System.IO.File.Exists(strFileFullPath))
                {
                    System.IO.File.Delete(strFileFullPath);
                }


                string sql = "delete from FMS.dbo.t_attachtment_stakeholder where Id = " + id;
                Koneksi.execQuery(sql);


                return Json(new { Result = String.Format("Success", "Yes") });

            }
            catch (Exception ex)
            {
                return Json(new { Result = String.Format("Error", "No") });
            }
            finally
            {
                db.Database.Connection.Close();
                //db.Connection.Close();
                //return JsonContent(new { Status = 1, Message = "Berhasil!" });

            }
        }
   

        public ActionResult File(int? id)
        {
            //t_attachtment att = db.t_attachtment.FirstOrDefault(t => t.Id == id);
            t_attachtment_employeemanagement att = db.t_attachtment_employeemanagement.First(s => s.Id == id);
            if (att != null)
            {
                ServiceDocSoapClient ws = new ServiceDocSoapClient();
                string contentType;
                byte[] data = ws.GetFileData2("I-Care", "EmployeeManagement", att.CandidateId_1,
                    att.Id, 0, 0, out contentType);
                if (data != null)
                {
                    MemoryStream str = new MemoryStream(data);
                    return File(str, contentType);
                }
            }
            throw new Exception("Attachment Not Found!");
        }

        public ActionResult GetCountEmployeeStatus()
        {


            string UserLogin = User.Identity.Name.ToString();
            string SQL = string.Empty;

            SQL = "exec FMS.dbo.procGetCountEmployeeStatus @UserName = '" + User.Identity.Name.ToString() + "'";

            string JSONresult;
            DataTable tblData = Koneksi.GetDataTable(SQL);
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };


        }
    }
}