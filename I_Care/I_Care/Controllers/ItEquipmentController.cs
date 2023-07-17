using I_Care.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Care.Controllers
{
    public class ItEquipmentController : BaseController
    {
        // GET: ItEquipment
        public UserRoleProvider ROLES = new I_Care.Classes.UserRoleProvider();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Request()
        {
            if (ROLES.CheckAllowModul(User.Identity.Name.ToString(), "MOD2018010002", 1) == true)
            {
                return View();
            }
            else
            {

                return View("Denied");
            }

        }

        [Authorize]
        public ActionResult RequestList()
        {
            if (ROLES.CheckAllowModul(User.Identity.Name.ToString(), "MOD2018010003", 1) == true)
            {
                return View();
            }
            else
            {

                return View("Denied");
            }
        }


        [Authorize]
        public ActionResult SubmissionsList()
        {


            if (ROLES.CheckAllowModul(User.Identity.Name.ToString(), "MOD2018010004", 1) == true)
            {
                string SQL = "select top 1 * from FMS.dbo.m_Role where RoleGroup = 1 and Step = 3 ";
                DataTable tblData = Koneksi.GetDataTable(SQL);
                string UserFinalApp = tblData.Rows[0]["PicAD"].ToString();

                string SQL2 = "select top 1 * from FMS.dbo.m_Role where RoleGroup = 1 and Step = 3 ";
                DataTable tblData1 = Koneksi.GetDataTable(SQL2);
                string UserRealisasi = tblData1.Rows[0]["PicAD"].ToString();


                ViewBag.UserFinalApp = UserFinalApp;
                ViewBag.UserRealisasi = UserRealisasi;

                return View();
            }
            else
            {

                return View("Denied");
            }


        }


        [Authorize]
        public ActionResult RequestListData(string ReqGroup)
        {

            string SQL = "";
            if (ReqGroup == null || ReqGroup == "")
            {

                SQL = "select *,'Exist' FlagStat from FMS.dbo.vRequest where CreateUser = '" + User.Identity.Name.ToString() + "' and Process  = 1 ";

            }
            else
            {
                SQL = "select *,'Exist' FlagStat from FMS.dbo.vRequest where CreateUser = '" + User.Identity.Name.ToString() + "' and Process  = 1  and ReqGroup  =" + ReqGroup;

            }

            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }



        [Authorize]
        public ActionResult RequestListData_(string NotIn)
        {

            string SQL = "select ReqId as recid,*,'Exist' FlagStat from FMS.dbo.vRequest where CreateUser = '" + User.Identity.Name.ToString() + "' and Process  = 1 and ReqId Not In (" + NotIn + ")";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }


        [Authorize]
        public ActionResult PengajuanListData()
        {
            string SQL = "exec FMS.dbo.ProcListPengajuan @Filter = 1 , @UserCreator= '" + User.Identity.Name.ToString() + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }

        [Authorize]
        public ActionResult PengajuanListData_Unfilter()
        {

            string SQL = "exec FMS.dbo.ProcListPengajuan @Filter = 0 , @UserCreator= '" + User.Identity.Name.ToString() + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }

        [Authorize]
        public JsonResult SaveRequest(FormCollection Request)
        {

            string ReqId = Request["ReqId"];
            string ReqType = Request["ReqType"];
            int ReqQty = Int32.Parse(Request["ReqQty"]);
            string Reqitem = Request["Reqitem"];
            string ReqPart = Request["ReqPart"];
            string ReqKeterangan = Request["ReqKeterangan"];
            string ReqReason = Request["ReqReason"];
            string Flag = Request["Flag"];
            int ReqGroup = Int32.Parse(Request["ReqGroup"]);

            //db.Connection.Open();
            //DbTransaction trans = db.Connection.BeginTransaction();
            //update code to EF 6
            db.Database.Connection.Open();
            DbTransaction trans = db.Database.Connection.BeginTransaction();
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@ReqId", SqlDbType.Int), //0
                    new SqlParameter("@ReqType", SqlDbType.Int), //1
                    new SqlParameter("@ReqQty", SqlDbType.Int), //2
                    new SqlParameter("@Reqitem", SqlDbType.VarChar,13), //3
                    new SqlParameter("@ReqPart", SqlDbType.Int), //4
                    new SqlParameter("@ReqKeterangan", SqlDbType.VarChar,300), //5
                    new SqlParameter("@ReqReason", SqlDbType.VarChar,200), //6
                    new SqlParameter("@UserName", SqlDbType.VarChar,100), //7
                    new SqlParameter("@Flag", SqlDbType.VarChar,20), //8
                    new SqlParameter("@ReqGroup", SqlDbType.Int), //9
                   
                };

                if (ReqId == null)
                {
                    parameters[0].Value = DBNull.Value;
                }
                else
                {

                    parameters[0].Value = ReqId; //0 
                }


                parameters[1].Value = ReqType; //1
                parameters[2].Value = ReqQty; //2 
                parameters[3].Value = Reqitem; //3
                parameters[4].Value = ReqPart; //4
                parameters[5].Value = ReqKeterangan; //5
                parameters[6].Value = ReqReason; //6
                parameters[7].Value = User.Identity.Name.ToString(); //7
                parameters[8].Value = Flag; //8
                parameters[9].Value = ReqGroup; //9


                string Exec = "EXEC ProcCrudRequest ";
                Exec = Exec + "  @ReqId"; //0
                Exec = Exec + " ,@ReqType"; //1
                Exec = Exec + " ,@ReqQty"; //2
                Exec = Exec + " ,@Reqitem"; //3
                Exec = Exec + " ,@ReqPart"; //4
                Exec = Exec + " ,@ReqKeterangan"; //5
                Exec = Exec + " ,@ReqReason"; //6
                Exec = Exec + " ,@UserName"; //7
                Exec = Exec + " ,@Flag"; //8
                Exec = Exec + " ,@ReqGroup"; //9

                //remarks update code to EF 6
                //db.ExecuteStoreCommand(Exec, parameters);
                db.Database.ExecuteSqlCommand(Exec, parameters);

                trans.Commit();
                return Json(new { Result = String.Format("OK", "OK") });

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return Json(new { Result = String.Format("Error", "No") });
            }
            finally
            {
                //remarks update code to EF 6
                //db.Connection.Close();
                db.Database.Connection.Close();
            }

        }

        [Authorize]
        public JsonResult SavePengajuan(FormCollection Pengajuan)
        {
            string id_pengajuan = Pengajuan["id_pengajuan"];
            string lokasi_id = Pengajuan["lokasi_id"];
            string pengguna_id = Pengajuan["pengguna_id"];
            string keterangan = Pengajuan["keterangan"];
            string flag_save = Pengajuan["flag_save"];
            string process = Pengajuan["process"];
            string PengajuanItem = Pengajuan["PengajuanItem"];
            string RealisasiItem = Pengajuan["RealisasiItem"];
            string reason = Pengajuan["reason"];

            //remarks update code to EF 6
            //db.Connection.Open();
            //DbTransaction trans = db.Connection.BeginTransaction();
            db.Database.Connection.Open();
            DbTransaction trans = db.Database.Connection.BeginTransaction();
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", SqlDbType.VarChar,13), //0
                    new SqlParameter("@DestLocation", SqlDbType.VarChar,50), //1
                    new SqlParameter("@DestPengguna", SqlDbType.VarChar,50), //2
                    new SqlParameter("@Keterangan", SqlDbType.VarChar,400), //3
                    new SqlParameter("@Process", SqlDbType.Int), //4
                    new SqlParameter("@PengajuanItem", SqlDbType.VarChar), //5
                    new SqlParameter("@UserName", SqlDbType.VarChar,100), //6
                    new SqlParameter("@Flag", SqlDbType.VarChar,20), //7
                    new SqlParameter("@jsonRealisasi", SqlDbType.VarChar), //8
                    new SqlParameter("@Reason", SqlDbType.VarChar), //9
                    new SqlParameter("@outputVal", SqlDbType.VarChar,500) { Direction = ParameterDirection.Output} //10
                };

                if (id_pengajuan == null)
                {
                    parameters[0].Value = DBNull.Value;
                }
                else
                {

                    parameters[0].Value = id_pengajuan; //0 
                }

                parameters[1].Value = lokasi_id; //1
                parameters[2].Value = pengguna_id; //2 
                parameters[3].Value = keterangan; //3 
                parameters[4].Value = process; //4 
                parameters[5].Value = PengajuanItem; //5
                parameters[6].Value = User.Identity.Name.ToString(); //6
                parameters[7].Value = flag_save; //7

                if (RealisasiItem == null)
                {
                    parameters[8].Value = DBNull.Value;//8
                }
                else
                {

                    parameters[8].Value = RealisasiItem; //8
                }


                parameters[9].Value = reason; //9
                parameters[10].Direction = ParameterDirection.Output; //10

                string Exec = "EXEC ProcCrudPengajuan ";
                Exec = Exec + "  @Id"; //0
                Exec = Exec + " ,@DestLocation"; //1
                Exec = Exec + " ,@DestPengguna"; //2
                Exec = Exec + " ,@Keterangan"; //3
                Exec = Exec + " ,@Process"; //4
                Exec = Exec + " ,@PengajuanItem"; //5
                Exec = Exec + " ,@UserName"; //6
                Exec = Exec + " ,@Flag"; //7
                Exec = Exec + " ,@jsonRealisasi"; //8
                Exec = Exec + " ,@Reason"; //9
                Exec = Exec + " ,@outputVal OUT"; //10

                //remarks update code to EF 6
                //db.ExecuteStoreCommand(Exec, parameters);
                db.Database.ExecuteSqlCommand(Exec ,parameters);
                string OutputKode = parameters[10].Value.ToString();

                trans.Commit();

                string sMailBody = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "template/alert_and_notif/notif_for_submission_mail.htm");
                string mailBody = "";
                string sMessage = "";
                string MailDestinationAddress = "";
                string MailDestinationName = "";
                string sHeader = "";
                string DetailPengajuan = "";
                string sRequestor = "";
                string FinnalApprove = Koneksi.getScalarValue("select [dbo].[fnGetFinnalApprovement](1)");
                string RealisasiPIC = Koneksi.getScalarValue("select [dbo].[fnGetRealisasiPic](1)");

                DataTable dtRequestor = Koneksi.GetDataTable("select * from [dbo].[VwMapUserAD] Where USER_AD = '" + User.Identity.Name.ToString() + "'");
                DataTable dtManager = Koneksi.GetDataTable("exec ProcGetAtasanDetail @UserName = '" + User.Identity.Name.ToString() + "'");
                DataTable dtFinal = Koneksi.GetDataTable("exec ProcGetPersonDetail @UserName = '" + FinnalApprove + "'");
                DataTable dtReal = Koneksi.GetDataTable("exec ProcGetPersonDetail @UserName = '" + RealisasiPIC + "'");

                string PengajuanId = "";
                PengajuanId = id_pengajuan;

                if (flag_save == "ADD" || flag_save == "REPOSTING")
                {
                    PengajuanId = OutputKode;
                    MailDestinationAddress = dtManager.Rows[0]["EMAIL"].ToString();
                    MailDestinationName = dtManager.Rows[0]["NM_PEG"].ToString() + " (Manager Fungsi " + dtManager.Rows[0]["FUNGSI"].ToString() + ")";
                    sMessage = "Mohon untuk mengapprove pengajuan ini ";
                }

                if (flag_save == "APPROVE_ATASAN")
                {
                    PengajuanId = id_pengajuan;
                    MailDestinationAddress = dtFinal.Rows[0]["EMAIL"].ToString();
                    MailDestinationName = dtFinal.Rows[0]["NM_PEG"].ToString() + " (Manager Fungsi " + dtFinal.Rows[0]["FUNGSI"].ToString() + ")";
                    sMessage = "Mohon untuk melakukan final approvement pada pengajuan ini ";

                }

                if (flag_save == "FINAL_APPROVEMENT")
                {
                    PengajuanId = id_pengajuan;
                    MailDestinationAddress = dtReal.Rows[0]["EMAIL"].ToString();
                    MailDestinationName = dtReal.Rows[0]["NM_PEG"].ToString() + " (PIC " + dtReal.Rows[0]["FUNGSI"].ToString() + ")";
                    sMessage = "Pengajuan ini sudah diapprove mohon  untuk merealisasikan pengajuan ini ";

                }

                if (flag_save == "REALISASI_PRKT_IT")
                {
                    PengajuanId = id_pengajuan;
                    MailDestinationAddress = dtRequestor.Rows[0]["EMAIL"].ToString();
                    MailDestinationName = dtRequestor.Rows[0]["NM_PEG"].ToString();
                    sMessage = "Pengajuan ini telah direalisasikan silahkan check pada sistem I-Care dan hubungi pihak IT untuk serah terima perangkat.";

                }

                if (flag_save == "PENDING")
                {
                    PengajuanId = id_pengajuan;
                    MailDestinationAddress = dtRequestor.Rows[0]["EMAIL"].ToString();
                    MailDestinationName = dtRequestor.Rows[0]["NM_PEG"].ToString();
                    sMessage = "Pengajuan ini dipending oleh IT dengan alasan : " + reason;

                }


                if (flag_save == "REJECT_BY_ATASAN" || flag_save == "REJECT_BY_FINAL")
                {
                    PengajuanId = id_pengajuan;
                    MailDestinationAddress = dtRequestor.Rows[0]["EMAIL"].ToString();
                    MailDestinationName = dtRequestor.Rows[0]["NM_PEG"].ToString();
                    sMessage = "Pengajuan ini <span style=\"color:red;font-weight:bold\">direject</span> dengan alasan : " + reason;
                }


                if (dtRequestor.Rows.Count > 0)
                {
                    sRequestor = dtRequestor.Rows[0]["NIP"].ToString() + " - " + dtRequestor.Rows[0]["NM_PEG"].ToString();
                }


                DataTable dtDetail = Koneksi.GetDataTable("select * from vPengajuanDetail where Pengajuan = '" + PengajuanId + "' ");
                foreach (DataRow row in dtDetail.Rows)
                {
                    DetailPengajuan = DetailPengajuan + "<tr><td>" + row["ItemName"].ToString() + "</td><td>" + row["PartName"].ToString() + "</td><td>" + row["ReqQty"].ToString() + "</td></tr>";
                }



                if (flag_save != "UPDATE" || flag_save == null)
                {
                    sHeader = "Pengajuan Perangkat IT : " + PengajuanId;
                    DateTime dateTime = DateTime.UtcNow.Date;

                    mailBody = sMailBody.Replace("@pic", MailDestinationName);
                    mailBody = mailBody.Replace("@Tanggal", dateTime.ToString("dd MMMM yyyy"));
                    mailBody = mailBody.Replace("@IdPengajuan", OutputKode);
                    mailBody = mailBody.Replace("@Category", "IT Equipment");
                    mailBody = mailBody.Replace("@message", sMessage);
                    mailBody = mailBody.Replace("@Remark", keterangan);
                    mailBody = mailBody.Replace("@Creator", sRequestor);
                    mailBody = mailBody.Replace("@Fill_Detail", DetailPengajuan);

                    I_Care.Classes.Libs.SendAlternateMail(MailDestinationAddress, mailBody, sHeader);

                }


                return Json(new { Result = String.Format("OK", "OK") });

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return Json(new { Result = String.Format("Error", "No") });
            }
            finally
            {
                //remarks update code to EF 6
                //db.Connection.Close();
                db.Database.Connection.Close();
            }

        }




        [Authorize]
        public ActionResult PengajuanDetailList(string PengajuanId)
        {


            string SQL = "select * from vPengajuanDetail where Pengajuan = '" + PengajuanId + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }

        [Authorize]
        public int Del_Pengajuan_Detail(int Id)
        {

            string SQL_1 = "select top 1* From t_pengajuan_detail where Id = " + Id.ToString() + "";
            DataTable dt_pengajuan = Koneksi.GetDataTable(SQL_1);
            string Req_Item = dt_pengajuan.Rows[0]["Item"].ToString();
            string SQL_Undo_Req = "Update t_request set Process = 1 where ReqId = " + Req_Item + "";

            Koneksi.execQuery(SQL_Undo_Req);

            string SQL = "delete from t_pengajuan_detail where Id = " + Id.ToString() + "";
            if (Koneksi.execQuery(SQL) == true)
            {

                return 1;
            }
            else
            {

                return 0;
            }


        }


        [Authorize]
        public ActionResult ListGroup()
        {


            string SQL = "select GroupId recid,* from m_group_item ";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }


        [Authorize]
        public ActionResult ListAssetData()
        {

            string SQL = "select * from FMS.dbo.vAssets ";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }


        [Authorize]
        public ActionResult ListHistory(string SubmissionId)
        {
            string nl = System.Environment.NewLine;
            string UserLogin = User.Identity.Name.ToString();
            string SQL = "exec FMS.dbo.ProcCrudTHistorySubmission @Type = 2,@Submission='" + SubmissionId + "',@UserId='" + UserLogin + "'" + nl;

            DataTable tblData = Koneksi.GetDataTable(SQL);

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }
    }
}