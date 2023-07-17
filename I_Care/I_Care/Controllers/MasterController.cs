using I_Care.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Care.Controllers
{
    public class MasterController : BaseController
    {
        // GET: Master
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult MasterItem()
        {
            return View();
        }

        [Authorize]
        public ActionResult Roles()
        {
            return View();
        }

        [Authorize]
        public ActionResult ItemDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult MasterItemList()
        {
            string SQL = "select ItemId recid, * from FMS.dbo.v_m_item ";
            DataTable tblData = Koneksi.GetDataTable(SQL);

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType

                = "application/json"
            };
        }

        [Authorize]
        public ActionResult RolesData()
        {
            string sqlSurvey = "exec FMS.dbo.procListRoles";
            DataTable tblSurvey = Koneksi.GetDataTable(sqlSurvey);
            string stepId = string.Empty;
            string stepName = string.Empty;

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblSurvey);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }
        [Authorize]
        public JsonResult UpdateRoles(FormCollection DataRole)
        {
            //remark update code to EF 6
            //db.Connection.Open();
            //DbTransaction trans = db.Connection.BeginTransaction();
            db.Database.Connection.Open();
            DbTransaction trans = db.Database.Connection.BeginTransaction(); 
            try
            {
                var parameters = new SqlParameter[]
                {
                new SqlParameter("Crud", SqlDbType.Int), //0
                new SqlParameter("Id", SqlDbType.Int), //1
                new SqlParameter("PicName", SqlDbType.VarChar,100), //2
                new SqlParameter("PicAD", SqlDbType.VarChar,100), //3
                new SqlParameter("PicMail", SqlDbType.VarChar,100), //4
                new SqlParameter("UserUpdate", SqlDbType.VarChar,100), //5
                };

                parameters[0].Value = 1; //0
                parameters[1].Value = Int32.Parse(DataRole["role_id"]); //1
                parameters[2].Value = DataRole["pic_name"]; //2 
                parameters[3].Value = DataRole["active_directory"]; //3 
                parameters[4].Value = DataRole["email"]; //4 
                parameters[5].Value = User.Identity.Name.ToString(); //5


                string Exec = "EXEC procRoleCrud ";
                Exec = Exec + "  @Crud"; //0
                Exec = Exec + " ,@Id"; //1
                Exec = Exec + " ,@PicName"; //2
                Exec = Exec + " ,@PicAD "; //3
                Exec = Exec + " ,@PicMail"; //4
                Exec = Exec + " ,@UserUpdate"; //5

                //remarks update code to EF 6
                //db.ExecuteStoreCommand(Exec, parameters);
                db.Database.ExecuteSqlCommand(Exec, parameters);

                trans.Commit();
                //db.Connection.Close();
                db.Database.Connection.Close();
                return Json(new { Result = "OK" });//String.Format(CustomerId) });

            }
            catch (Exception ex)
            {
                trans.Rollback();
                //remarks update code to EF 6
                //db.Connection.Close();
                db.Database.Connection.Close();
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
        public ActionResult MasterGroupItemList()
        {
            string SQL = "select GroupId recid, * from FMS.dbo.m_group_item ";
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
        public ActionResult MasterPartList(string ItemId)
        {
            string SQL = "select PartId recid, *,'exist' FlagStat from FMS.dbo.m_part_item where PartParent='" + ItemId + "'";
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
        public JsonResult SaveItem(FormCollection Items)
        {
            HttpFileCollectionBase files = Request.Files;

            string item_id = Items["item_id"];
            string group_id = Items["group_id"];
            string item_name = Items["item_name"];
            string flag_save = Items["flag_save"];
            string extension = "";
            string filename = "";
            string filetype = "";
            string filesNames = "";

            string ItemID = "";
            //remarks update code to EF 6
            //db.Connection.Open();
            //DbTransaction trans = db.Connection.BeginTransaction();
            db.Database.Connection.Open();
            DbTransaction trans = db.Database.Connection.BeginTransaction();
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@ItemId", SqlDbType.VarChar,50), //0
                    new SqlParameter("@ItemName", SqlDbType.VarChar,50), //1
                    new SqlParameter("@ItemGroup", SqlDbType.VarChar,50), //2
                    new SqlParameter("@ItemImage", SqlDbType.VarChar,200), //3
                    new SqlParameter("@user", SqlDbType.VarChar,200), //4
                    new SqlParameter("@flag", SqlDbType.VarChar,200), //5
                    new SqlParameter("outputVal", SqlDbType.VarChar,20) { Direction = ParameterDirection.Output} //6

                };

                parameters[0].Value = item_id; //0 
                parameters[1].Value = item_name; //1 
                parameters[2].Value = group_id; //2


                if (Request.Files.Count > 0)
                {
                    var fileContent = Request.Files[0];


                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        var len = fileContent.ContentLength;
                        filename = System.IO.Path.GetFileName(fileContent.FileName);
                        filetype = System.IO.Path.GetExtension(filename);
                        extension = filetype;

                    }
                }

                parameters[3].Value = filename; //3    
                parameters[4].Value = User.Identity.Name.ToString(); //4
                parameters[5].Value = flag_save; //5
                parameters[6].Direction = ParameterDirection.Output; //6

                string Exec = "EXEC ProcCrudItem ";
                Exec = Exec + "  @ItemId"; //0
                Exec = Exec + " ,@ItemName"; //1
                Exec = Exec + " ,@ItemGroup"; //2
                Exec = Exec + " ,@ItemImage"; //3
                Exec = Exec + " ,@user"; //4
                Exec = Exec + " ,@flag"; //5
                Exec = Exec + " ,@outputVal OUT"; //6

                //remarks update code to EF 6
                //db.ExecuteStoreCommand(Exec, parameters);
                db.Database.ExecuteSqlCommand(Exec, parameters);
                ItemID = parameters[6].Value.ToString();
                filesNames = ItemID + "_" + filename;

                if (Request.Files.Count > 0)
                {
                    if (ItemID != "ERROR")
                    {
                        var fileContent = Request.Files[0];
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {
                            var path = Path.Combine(Server.MapPath("~/Assets/image_item"), filesNames);
                            fileContent.SaveAs(path);
                        }
                    }
                }

                trans.Commit();
                return Json(new { Result = String.Format("OK", "No") });

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
        public JsonResult AddPart(FormCollection Items)
        {


            string ItemId = Items["item_id"];
            string PartName = Items["part_name"];

            string SQL = "exec dbo.ProcAddPart '" + ItemId + "','" + PartName + "','" + User.Identity.Name.ToString() + "'";

            if (Koneksi.execQuery(SQL))
            {

                return Json(new { Result = String.Format("OK", "No") });
            }
            else
            {

                return Json(new { Result = String.Format("ERROR", "No") });
            }

        }


        [Authorize]
        public JsonResult DeletePart(int PartId)
        {

            string SQL = "Delete dbo.m_part_item where PartId = " + PartId.ToString();
            if (Koneksi.execQuery(SQL))
            {

                return Json(new { Result = String.Format("OK", "No") });
            }
            else
            {

                return Json(new { Result = String.Format("ERROR", "No") });
            }

        }


        [Authorize]
        public ActionResult ItemData(string ItemId)
        {
            string SQL = "select top 1 * from FMS.dbo.m_item where ItemId =  '" + ItemId + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }


        public ContentResult ListPengguna(string textFilter, string tipe)
        {

            string sqlSurvey = "select NIP recid,NIP KodePengguna, NM_PEG NamaPengguna ,NM_UNIT_ORG Posisi,FUNGSI Fungsi from vPengguna";
            DataTable tblSurvey = Koneksi.GetDataTable(sqlSurvey);
            string stepId = string.Empty;
            string stepName = string.Empty;

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblSurvey);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }


        public ContentResult ListUnit()
        {

            string sql_ = "select KodeUnit recid,* from Asset_It.dbo.m_unit ";
            DataTable tblGet = Koneksi.GetDataTable(sql_);
            string stepId = string.Empty;
            string stepName = string.Empty;

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblGet);


            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }


    }
}