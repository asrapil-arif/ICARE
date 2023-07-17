using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I_Care.Models;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.IO;
using I_Care.Classes;
using System.Data.Common;
using System.Net;
using System.Web.Security;
using System.Globalization;
using Newtonsoft.Json;
using I_Care.ServiceSSO;

namespace I_Care.Controllers
{
    public class ButtonController : BaseController
	{


		//public ActionResult GA_Kontrak_AllowButton(FormCollection Data)
		//{
		//	try
		//	{


		//		string ContractId = Data["Id"];
		//		string SQL = "exec FMS.dbo.procGetAllowActionCurrectGAContract @ContractId = '" + ContractId + "',@UserName = '" + User.Identity.Name.ToString() + "'";

		//		string _allow_button = string.Empty;
		//		string _update_button = string.Empty;
		//		string _addendum_button = string.Empty;
		//		string _done_button = string.Empty;
		//		string _alert_button = string.Empty;

		//		string _disable_button = string.Empty;
		//		string _enable_button = string.Empty;
		//		string _delete_button = string.Empty;

		//		int AllowUpdate;
		//		int AllowAddendum;
		//		int AllowDone;
		//		int AllowAlert;
		//		int AllowDisable;
		//		int AllowEnable;
		//		int AllowDelete;


		//		DataTable tblData = Koneksi.GetDataTable(SQL);
		//		AllowUpdate = int.Parse(tblData.Rows[0]["AllowUpdate"].ToString());
		//		AllowAddendum = int.Parse(tblData.Rows[0]["AllowAddendum"].ToString());
		//		AllowDone = int.Parse(tblData.Rows[0]["AllowDone"].ToString());
		//		AllowAlert = int.Parse(tblData.Rows[0]["AllowAlert"].ToString());
		//		AllowDisable = int.Parse(tblData.Rows[0]["AllowDisable"].ToString());
		//		AllowEnable = int.Parse(tblData.Rows[0]["AllowEnable"].ToString());
		//		AllowDelete = int.Parse(tblData.Rows[0]["AllowDelete"].ToString());


		//		if (AllowUpdate == 1) _update_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_contract('UPDATE')\" ondblclick=\"save_contract('UPDATE')\" id=\"btn_save\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i> Save</button>";
		//		if (AllowAddendum == 1) _addendum_button = " <button type=\"button\" class=\"btn btn-warning btn-sm\" onclick=\"create_addendum()\" ondblclick=\"create_addendum()\" id=\"btn_addendum\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> Create Addendum</button>";
		//		if (AllowDone == 1) _done_button = " <button type=\"button\" class=\"btn btn-grey btn-sm\" onclick=\"save_contract('DONE')\" ondblclick=\"save_contract('DONE')\" id=\"btn_done\"><i class=\"fa fa-archive\" aria-hidden=\"true\"></i> Done</button>";
		//		if (AllowAlert == 1) _alert_button = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"send_alert()\" ondblclick=\"send_alert()\" id=\"btn_alert\"><i class=\"fa fa-bell\" aria-hidden=\"true\"></i> Send Alert</button>";

		//		if (AllowDisable == 1) _disable_button = " <button type=\"button\" class=\"btn btn-grey btn-sm\" onclick=\"save_contract('DISABLE')\" ondblclick=\"save_contract('DISABLE')\" id=\"btn_alert\"><i class=\"fa fa-ban\" aria-hidden=\"true\"></i> Disable Alert</button>";
		//		if (AllowEnable == 1) _enable_button = " <button type=\"button\" class=\"btn btn-success btn-sm\" onclick=\"save_contract('ENABLE')\" ondblclick=\"save_contract('ENABLE')\" id=\"btn_alert\"><i class=\"fa fa-check\" aria-hidden=\"true\"></i> Enable Alert</button>";
		//		if (AllowDelete == 1) _delete_button = " <button type=\"button\" class=\"btn btn-inverse btn-sm\" onclick=\"save_contract('DELETE')\" ondblclick=\"save_contract('DELETE')\" id=\"btn_alert\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i> Delete</button>";

		//		_allow_button = _alert_button + _done_button + _disable_button + _enable_button +  _addendum_button  + _delete_button + _update_button;

		//		return Content(_allow_button);

		//	}

		//	catch (Exception ex)
		//	{

		//		return Content("Error : " + ex.Message.ToString());
		//	}





		//}

        public ActionResult Archive_Contract_AllowButton(FormCollection Data)
        {
            try
            {


                string ContractId = Data["Id"];
                string SQL = "exec FMS.dbo.procGetAllowActionCurrectArchiveContract @ContractId = '" + ContractId + "',@UserName = '" + User.Identity.Name.ToString() + "'";

                string _allow_button = string.Empty;
                string _update_button = string.Empty;
                string _addendum_button = string.Empty;
                string _done_button = string.Empty;
                string _alert_button = string.Empty;

                string _disable_button = string.Empty;
                string _enable_button = string.Empty;
                string _delete_button = string.Empty;

                int AllowUpdate;
                int AllowAddendum;
                int AllowDone;
                int AllowAlert;
                int AllowDisable;
                int AllowEnable;
                int AllowDelete;


                DataTable tblData = Koneksi.GetDataTable(SQL);
                AllowUpdate = int.Parse(tblData.Rows[0]["AllowUpdate"].ToString());
                AllowAddendum = int.Parse(tblData.Rows[0]["AllowAddendum"].ToString());
                AllowDone = int.Parse(tblData.Rows[0]["AllowDone"].ToString());
                AllowAlert = int.Parse(tblData.Rows[0]["AllowAlert"].ToString());
                AllowDisable = int.Parse(tblData.Rows[0]["AllowDisable"].ToString());
                AllowEnable = int.Parse(tblData.Rows[0]["AllowEnable"].ToString());
                AllowDelete = int.Parse(tblData.Rows[0]["AllowDelete"].ToString());


                if (AllowUpdate == 1) _update_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_contract()\" ondblclick=\"save_contract()\" id=\"btn_save\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i> Save</button>";
                if (AllowAddendum == 1) _addendum_button = " <button type=\"button\" class=\"btn btn-info btn-sm\" onclick=\"create_addendum()\" ondblclick=\"create_addendum()\" id=\"btn_addendum\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> Addendum</button>";
                if (AllowDone == 1) _done_button = " <button type=\"button\" class=\"btn btn-grey btn-sm\" onclick=\"done_contract()\" ondblclick=\"done_contract()\" id=\"btn_done\"><i class=\"fa fa-archive\" aria-hidden=\"true\"></i> Done</button>";
                if (AllowAlert == 1) _alert_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"send_alert()\" ondblclick=\"send_alert()\" id=\"btn_alert\"><i class=\"fa fa-bell\" aria-hidden=\"true\"></i> Send Alert</button>";

                if (AllowDisable == 1) _disable_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_contract('DISABLE')\" ondblclick=\"save_contract('DISABLE')\" id=\"btn_alert\"><i class=\"fa fa-ban\" aria-hidden=\"true\"></i> Disable Alert</button>";
                if (AllowEnable == 1) _enable_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_contract('ENABLE')\" ondblclick=\"save_contract('ENABLE')\" id=\"btn_alert\"><i class=\"fa fa-check\" aria-hidden=\"true\"></i> Enable Alert</button>";
                if (AllowDelete == 1) _delete_button = " <button type=\"button\" class=\"btn btn-danger  btn-sm\" onclick=\"delete_contract()\" ondblclick=\"delete_contract()\" id=\"btn_alert\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i> Delete</button>";

                _allow_button = _delete_button + _alert_button + _done_button + _addendum_button + _update_button;

                return Content(_allow_button);

            }

            catch (Exception ex)
            {

                return Content("Error : " + ex.Message.ToString());
            }





        }
        public ActionResult ContractArchive_AllowAction(FormCollection Data)
        {
            string nl = System.Environment.NewLine;
            string UserLogin = User.Identity.Name.ToString();
            string ProcId = Data["Id"];
            string SQL = "exec FMS.dbo.procGetAllowActionCurrectArchiveContract @ContractId = '" + ProcId + "',@UserName = '" + User.Identity.Name.ToString() + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }
        public ActionResult E_Contract_AllowButton(FormCollection Data)
        {
            try
            {
                string ContractId = Data["ContractId"];
                DataTable dtTable = new DataTable();
                SqlConnection cnn = new SqlConnection();
                cnn = Koneksi.GetKoneksi();
                string JSONresult = string.Empty;

                SqlCommand sCommand = new SqlCommand("FMS.dbo.procGetAllowActionCurrentEContract", cnn);
                sCommand.CommandType = CommandType.StoredProcedure;
                sCommand.Parameters.AddWithValue("@ContractId", ContractId);
                sCommand.Parameters.AddWithValue("@UserName", User.Identity.Name.ToString());
                SqlDataReader dtReader = sCommand.ExecuteReader();
                dtTable.Load(dtReader);
                cnn.Close();
               
                string _allow_button = string.Empty;
                string AllowNextProcess = string.Empty;
                string AllowReject = string.Empty;
                string AllowUploadAttachment = string.Empty;
                string AllowAddMailAlert = string.Empty;
                string AllowUpdate = string.Empty;
                string AllowDelete = string.Empty;
                string AllowAddendum = string.Empty;
                string AllowDownloadSummaryReport = string.Empty;


                if (int.Parse(dtTable.Rows[0]["AllowNextProcess"].ToString()) >= 1) AllowNextProcess = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"routing_draft()\"><i class=\"ace-icon fa fa-floppy-o\" aria-hidden=\"true\"></i> Process to Next Step</button>";
                if (int.Parse(dtTable.Rows[0]["AllowReject"].ToString()) >= 1) AllowReject = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"reject_draft()\"><i class=\"ace-icon fa fa-reply\" aria-hidden=\"true\"></i> Reject</button>";
                if (int.Parse(dtTable.Rows[0]["AllowUpdate"].ToString()) >= 1) AllowUpdate = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_contract()\"><i class=\"ace-icon fa fa-floppy-o\" aria-hidden=\"true\"></i> Update</button>";
                if (int.Parse(dtTable.Rows[0]["AllowDelete"].ToString()) >= 1) AllowDelete = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"delete_contract()\"><i class=\"ace-icon fa fa-trash\" aria-hidden=\"true\"></i> Delete</button>";
                if (int.Parse(dtTable.Rows[0]["AllowAddendum"].ToString()) >= 1) AllowDelete = " <button type=\"button\" class=\"btn btn-success btn-sm\" onclick=\"create_addendum()\"><i class=\"ace-icon fa fa-external-link\" aria-hidden=\"true\"></i> Addendum</button>";
                if (int.Parse(dtTable.Rows[0]["AllowDownloadSummaryReport"].ToString()) >= 1) AllowDownloadSummaryReport = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"download_summary_report()\"><i class=\"ace-icon fa fa-file-pdf-o\" aria-hidden=\"true\"></i> Download Summary Report </button>";

                _allow_button =  AllowReject + AllowDelete + AllowNextProcess +   AllowAddendum +  AllowUpdate + AllowDownloadSummaryReport;
                return Content(_allow_button);

            }

            catch (Exception ex)
            {

                return Content("Error : " + ex.Message.ToString());
            }





        }
        public ActionResult E_Contract_AllowAction(FormCollection Data)
        {

            string ContractId = Data["ContractId"];
            DataTable dtTable = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn = Koneksi.GetKoneksi();
            string JSONresult = string.Empty;

            SqlCommand sCommand = new SqlCommand("FMS.dbo.procGetAllowActionCurrentEContract", cnn);
            sCommand.CommandType = CommandType.StoredProcedure;
            sCommand.Parameters.AddWithValue("@ContractId", ContractId);
            sCommand.Parameters.AddWithValue("@UserName", User.Identity.Name.ToString());
            SqlDataReader dtReader = sCommand.ExecuteReader();
            dtTable.Load(dtReader);
            cnn.Close();
            JSONresult = JsonConvert.SerializeObject(dtTable);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }
        public ActionResult Procurement_AllowButton(FormCollection Data)
		{
			try
			{
				string ProcId = Data["Id"];
				DataTable dtTable = new DataTable();
                SqlConnection cnn = new SqlConnection();
                cnn = Koneksi.GetKoneksi();
                string JSONresult = string.Empty;

                SqlCommand sCommand = new SqlCommand("FMS.dbo.procGetAllowActionCurrectProcurement", cnn);
                sCommand.CommandType = CommandType.StoredProcedure;
                sCommand.Parameters.AddWithValue("@ProcId", ProcId);
                sCommand.Parameters.AddWithValue("@UserName", User.Identity.Name.ToString());
                SqlDataReader dtReader = sCommand.ExecuteReader();
                dtTable.Load(dtReader);
                cnn.Close();
              

                string _allow_button = string.Empty;
				string _update_button = string.Empty;
				string _addendum_button = string.Empty;
				string _done_button = string.Empty;
				string _alert_button = string.Empty;
				string _reject_button = string.Empty;
				string _disable_button = string.Empty;
				string _enable_button = string.Empty;
				string _delete_button = string.Empty;
				string _review_button = string.Empty;
				string _invite_button = string.Empty;
				string _rpp_button = string.Empty;
				string _nego_button = string.Empty;
				string _usul_button = string.Empty;
				string _pengumuman_button = string.Empty;
				string _masa_sanggah_button = string.Empty;
				string _penunjukan_button = string.Empty;
				string _surat_sanggup_button = string.Empty;
				string _kontrak_button = string.Empty;
				string _finish_button = string.Empty;
				string _resubmit_button = string.Empty;


				int AllowUpdate;
				int AllowReview;
				int AllowReject;
				int AllowDelete;
				int AllowInvite;
				int AllowRpp;
				int AllowNego;
				int AllowUsul;
				int AllowPengumuman;
				int AllowMasaSanggah;
				int AllowPenunjukan;
				int AllowSuratSanggup;
				int AllowKontrak;
				int AllowFinish;
				int AllowResubmit;


				AllowUpdate = int.Parse(dtTable.Rows[0]["AllowUpdate"].ToString());
				AllowDelete = int.Parse(dtTable.Rows[0]["AllowDelete"].ToString());
				AllowReview = int.Parse(dtTable.Rows[0]["AllowReview"].ToString());
				AllowReject = int.Parse(dtTable.Rows[0]["AllowReject"].ToString());
				AllowInvite = int.Parse(dtTable.Rows[0]["AllowInvite"].ToString());
				AllowRpp = int.Parse(dtTable.Rows[0]["AllowRpp"].ToString());
				AllowNego = int.Parse(dtTable.Rows[0]["AllowNego"].ToString());
				AllowUsul = int.Parse(dtTable.Rows[0]["AllowUsul"].ToString());
				AllowPengumuman = int.Parse(dtTable.Rows[0]["AllowPengumuman"].ToString());
				AllowMasaSanggah = int.Parse(dtTable.Rows[0]["AllowMasaSanggah"].ToString());
				AllowPenunjukan = int.Parse(dtTable.Rows[0]["AllowPenunjukan"].ToString());
				AllowSuratSanggup = int.Parse(dtTable.Rows[0]["AllowSuratSanggup"].ToString());
				AllowKontrak = int.Parse(dtTable.Rows[0]["AllowKontrak"].ToString());
				AllowFinish = int.Parse(dtTable.Rows[0]["AllowFinish"].ToString());
				AllowResubmit = int.Parse(dtTable.Rows[0]["AllowResubmit"].ToString());


				if (AllowUpdate >= 1) _update_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('UPDATE')\" ondblclick=\"save_procurement('UPDATE')\" id=\"btn_save\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i> Update</button>";
				if (AllowDelete >= 1) _delete_button = " <button type=\"button\" class=\"btn btn-inverse btn-sm\" onclick=\"save_procurement('DELETE')\" ondblclick=\"save_procurement('DELETE')\" id=\"btn_alert\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i> Delete</button>";
				if (AllowReview >= 1) _review_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('REVIEW')\" ondblclick=\"save_procurement('REVIEW')\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Flag Reviewed</button>";
				if (AllowReject >= 1) _reject_button = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"save_procurement('REJECT')\" ondblclick=\"save_procurement('REJECT')\" id=\"btn_alert\"><i class=\"fa fa-undo\" aria-hidden=\"true\"></i> Reject</button>";
				if (AllowInvite >= 1) _invite_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('INVITE')\" ondblclick=\"save_procurement('INVITE')\" id=\"btn_alert\"><i class=\"fa fa-paper-plane-o\" aria-hidden=\"true\"></i> Tandai Telah Diinvite</button>";
				if (AllowRpp >= 1) _rpp_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('RPP')\" ondblclick=\"save_procurement('RPP')\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Update Rapat Penjelasan</button>";
				if (AllowNego >= 1) _nego_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('NEGO')\" ondblclick=\"save_procurement('NEGO')\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Update Negosiasi</button>";
				if (AllowUsul >= 1) _usul_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('USUL')\" ondblclick=\"save_procurement('USUL')\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Update Usulan Pemenang</button>";
				if (AllowPengumuman >= 1) _pengumuman_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('PENGUMUMAN')\" ondblclick=\"save_procurement('PENGUMUMAN')\" id=\"btn_alert\"><i class=\"fa fa-bullhorn\" aria-hidden=\"true\"></i> Pilih Pemenang</button>";
				if (AllowMasaSanggah >= 1) _masa_sanggah_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('MASA_SANGGAH')\" ondblclick=\"save_procurement('MASA_SANGGAH')\" id=\"btn_alert\"><i class=\"fa fa-hourglass-end\" aria-hidden=\"true\"></i> Akhiri Masa Sanggah</button>";
				if (AllowPenunjukan >= 1) _penunjukan_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('PENUNJUKAN')\" ondblclick=\"save_procurement('PENUNJUKAN')\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Update Penunjukan</button>";
				if (AllowSuratSanggup >= 1) _surat_sanggup_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('KESANGGUPAN')\" ondblclick=\"save_procurement('KESANGGUPAN')\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Update Surat Kesanggupan</button>";
				if (AllowKontrak >= 1) _kontrak_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"process_contract()\" ondblclick=\"process_contract()\" id=\"btn_alert\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Proses Kontrak</button>";
				if (AllowFinish >= 1) _finish_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('FINISH')\" ondblclick=\"save_procurement('FINISH')\" id=\"btn_alert\"><i class=\"fa fa-thumbs-up\" aria-hidden=\"true\"></i> Nyatakan Pengajuan Telah Berhasil</button>";
				if (AllowResubmit >= 1) _resubmit_button = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_procurement('RESUBMIT')\" ondblclick=\"save_procurement('RESUBMIT')\" id=\"btn_alert\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i> Resubmit</button>";



				_allow_button = _review_button + _invite_button + _rpp_button + _nego_button + _usul_button + _pengumuman_button + _masa_sanggah_button + _penunjukan_button + _surat_sanggup_button + _kontrak_button + _finish_button +  _reject_button  +  _delete_button + _resubmit_button +  _update_button;
				return Content(_allow_button);

			}

			catch (Exception ex)
			{

				return Content("Error : " + ex.Message.ToString());
			}

		}
        public ActionResult E_Kontrak_Procurement_AllowButton(FormCollection Data)
        {
            try
            {
                string ContractId = Data["ContractId"];
                string SQL = "exec FMS.dbo.procGetAllowActionCurrect_E_Kontrak_Procurement @ContractId = '" + ContractId + "',@UserName = '" + User.Identity.Name.ToString() + "'";

                string _allow_button = string.Empty;
                string AllowAddPage = string.Empty;
                string AllowDeletePage = string.Empty;
                string AllowSubmit = string.Empty;
                string AllowReject = string.Empty;
                string AllowAddAttachment = string.Empty;
                string AllowDownloadDraft = string.Empty;
               

                DataTable tblData = Koneksi.GetDataTable(SQL);

                if (int.Parse(tblData.Rows[0]["AllowAddPage"].ToString()) >= 1) AllowAddPage = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools\" onclick=\"add_new_page(); \"><i class=\"ace-icon fa fa-file\" aria-hidden=\"true\" style=\"color:orange; \"></i> New Page</button>";
                if (int.Parse(tblData.Rows[0]["AllowDeletePage"].ToString()) >= 1) AllowDeletePage = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools\" onclick=\"delete_page(); \"><i class=\"ace-icon fa fa-trash\" aria-hidden=\"true\" style=\"color:red; \"></i> Delete Page</button>";
                if (int.Parse(tblData.Rows[0]["AllowSubmit"].ToString()) >= 1) AllowSubmit = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn_process\" onclick=\"prep_save_draft()\"><i class=\"ace-icon fa fa-floppy-o\" aria-hidden=\"true\" style=\"color: dodgerblue; \"></i> Process to Next Step</button>";
                if (int.Parse(tblData.Rows[0]["AllowReject"].ToString()) >= 1) AllowReject = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn_reject_draft\" onclick=\"prep_save_draft('reject')\"><i class=\"ace-icon fa fa-undo\" aria-hidden=\"true\" style=\"color:red;\"></i> Reject</button>";
                if (int.Parse(tblData.Rows[0]["AllowAddAttachment"].ToString()) >= 1) AllowAddAttachment = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn-clear-attach btn-attach-info\" hidden>...</button> <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn-clear-attach\" onclick=\"clear_attach_file()\" hidden><i class=\"ace-icon fa fa-trash\" aria-hidden=\"true\"></i> Clear Attachment File</button> <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools\" onclick=\"$('#attachment_draft_e_kontrak').val('');$('#attachment_draft_e_kontrak').click();\" ><i class=\"ace-icon fa fa-plus-circle\" aria-hidden=\"true\" style=\"color:blue;\"></i> Tambahakan Lampiran</button>";
                if (int.Parse(tblData.Rows[0]["AllowDownloadDraft"].ToString()) >= 1) AllowDownloadDraft = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn_download_draft\" onclick=\"download_draft()\"><i class=\"ace-icon fa fa-file-pdf-o\" aria-hidden=\"true\" style=\"color:red;\"></i> Download Draft PDF </button>";
                
                _allow_button = AllowAddAttachment + AllowDownloadDraft   + AllowAddPage + AllowDeletePage + AllowReject + AllowSubmit;

                return Content(_allow_button);

            }

            catch (Exception ex)
            {

                return Content("Error : " + ex.Message.ToString());
            }

        }
		public ActionResult E_Kontrak_General_AllowButton(FormCollection Data)
		{
			try
			{
				string ContractId = Data["ContractId"];
				string SQL = "exec FMS.dbo.procGetAllowActionCurrect_E_Kontrak_General @ContractId = '" + ContractId + "',@UserName = '" + User.Identity.Name.ToString() + "'";

				string _allow_button = string.Empty;
				string AllowAddPage = string.Empty;
				string AllowDeletePage = string.Empty;
				string AllowSubmit = string.Empty;
				string AllowReject = string.Empty;
				string AllowAddAttachment = string.Empty;
				string AllowDownloadDraft = string.Empty;

				DataTable tblData = Koneksi.GetDataTable(SQL);

				if (int.Parse(tblData.Rows[0]["AllowAddPage"].ToString()) >= 1) AllowAddPage = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools\" onclick=\"add_new_page(); \"><i class=\"ace-icon fa fa-file\" aria-hidden=\"true\" style=\"color:orange; \"></i> New Page</button>";
				if (int.Parse(tblData.Rows[0]["AllowDeletePage"].ToString()) >= 1) AllowDeletePage = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools\" onclick=\"delete_page(); \"><i class=\"ace-icon fa fa-trash\" aria-hidden=\"true\" style=\"color:red; \"></i> Delete Page</button>";
				if (int.Parse(tblData.Rows[0]["AllowSubmit"].ToString()) >= 1) AllowSubmit = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn_process\" onclick=\"prep_save_draft()\"><i class=\"ace-icon fa fa-floppy-o\" aria-hidden=\"true\" style=\"color: dodgerblue; \"></i> Process to Next Step</button>";
				if (int.Parse(tblData.Rows[0]["AllowReject"].ToString()) >= 1) AllowReject = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn_reject_draft\" onclick=\"prep_save_draft('reject')\"><i class=\"ace-icon fa fa-undo\" aria-hidden=\"true\" style=\"color:red;\"></i> Reject</button>";
				if (int.Parse(tblData.Rows[0]["AllowAddAttachment"].ToString()) >= 1) AllowAddAttachment = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn-clear-attach btn-attach-info\" hidden>...</button> <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn-clear-attach\" onclick=\"clear_attach_file()\" hidden><i class=\"ace-icon fa fa-trash\" aria-hidden=\"true\"></i> Clear Attachment File</button> <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools\" onclick=\"$('#attachment_draft_e_kontrak').val('');$('#attachment_draft_e_kontrak').click();\"><i class=\"ace-icon fa fa-plus-circle\" aria-hidden=\"true\" style=\"color:blue;\"></i> Tambahakan Lampiran</button>";
				if (int.Parse(tblData.Rows[0]["AllowDownloadDraft"].ToString()) >= 1) AllowDownloadDraft = " <button type=\"button\" class=\"btn btn-white btn-bold btn-draft-tools btn_download_draft\" onclick=\"download_draft()\"><i class=\"ace-icon fa fa-file-pdf-o\" aria-hidden=\"true\" style=\"color:red;\"></i> Download Draft PDF </button>";

				_allow_button = AllowAddAttachment + AllowDownloadDraft + AllowAddPage + AllowDeletePage + AllowReject + AllowSubmit;

				return Content(_allow_button);

			}

			catch (Exception ex)
			{

				return Content("Error : " + ex.Message.ToString());
			}





		}
		public ActionResult CekAllowUploadDraftEKontrak(string Id)
		{
			string SQL;
			SQL = "exec FMS.dbo.procCheckAllowUploadDraft  @ContractId = '" + Id + "',@UserName = '"+User.Identity.Name.ToString()+"'";

			string JSONresult;
			DataTable tblData = Koneksi.GetDataTable(SQL);
			JSONresult = JsonConvert.SerializeObject(tblData);

			return new ContentResult
			{
				Content = JSONresult,
				ContentType = "application/json"
			};

		}
        public ActionResult Procurement_AllowAction(FormCollection Data)
        {
            string nl = System.Environment.NewLine;
            string UserLogin = User.Identity.Name.ToString();
            string ProcId = Data["Id"];
            string SQL = "exec FMS.dbo.procGetAllowActionCurrectProcurement @ProcId = '" + ProcId + "',@UserName = '" + User.Identity.Name.ToString() + "'";
            DataTable tblData = Koneksi.GetDataTable(SQL);

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(tblData);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };
        }


        public ActionResult Broadcast_AllowButton(FormCollection Data)
        {
            try
            {
                string BroadcastId = Data["BroadcastId"];
                DataTable dtTable = new DataTable();
                SqlConnection cnn = new SqlConnection();
                cnn = Koneksi.GetKoneksi();
                string JSONresult = string.Empty;

                SqlCommand sCommand = new SqlCommand("FMS.dbo.procGetAllowActionCurrentBroadcast", cnn);
                sCommand.CommandType = CommandType.StoredProcedure;
                sCommand.Parameters.AddWithValue("@BroadcastId", BroadcastId);
                sCommand.Parameters.AddWithValue("@UserName", User.Identity.Name.ToString());
                SqlDataReader dtReader = sCommand.ExecuteReader();
                dtTable.Load(dtReader);
                cnn.Close();

                string _allow_button = string.Empty;
                string AllowApprove = string.Empty;
                string AllowDelete = string.Empty;
                string AllowResubmit = string.Empty;
                string AllowReject = string.Empty;
                string AllowForceSend = string.Empty;


                if (int.Parse(dtTable.Rows[0]["AllowApprove"].ToString()) >= 1) AllowApprove = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_broadcast(\'APPROVE\')\"><i class=\"ace-icon fa fa-check-square-o\" aria-hidden=\"true\"></i> Approve</button>";
                if (int.Parse(dtTable.Rows[0]["AllowDelete"].ToString()) >= 1) AllowDelete = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"save_broadcast(\'DELETE\')\"><i class=\"ace-icon fa fa-trash\" aria-hidden=\"true\"></i> Delete</button>";
                if (int.Parse(dtTable.Rows[0]["AllowResubmit"].ToString()) >= 1) AllowResubmit = " <button type=\"button\" class=\"btn btn-primary btn-sm\" onclick=\"save_broadcast(\'RESUBMIT\')\"><i class=\"ace-icon fa fa-paper-plane-o\" aria-hidden=\"true\"></i> Resubmit</button>";
                if (int.Parse(dtTable.Rows[0]["AllowReject"].ToString()) >= 1) AllowReject = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"reject_broadcast()\"><i class=\"ace-icon fa fa-reply\" aria-hidden=\"true\"></i> Reject</button>";
                if (int.Parse(dtTable.Rows[0]["AllowForceSend"].ToString()) >= 1) AllowForceSend = " <button type=\"button\" class=\"btn btn-danger btn-sm\" onclick=\"force_send()\"><i class=\"ace-icon fa fa-paper-plane-o\" aria-hidden=\"true\"></i> Force Send</button>";


                _allow_button = AllowDelete + AllowResubmit + AllowReject  + AllowForceSend + AllowApprove;
                return Content(_allow_button);

            }

            catch (Exception ex)
            {

                return Content("Error : " + ex.Message.ToString());
            }





        }
        public ActionResult Broadcast_AllowAction(FormCollection Data)
        {

            string BroadcastId = Data["BroadcastId"];
            DataTable dtTable = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn = Koneksi.GetKoneksi();
            string JSONresult = string.Empty;

            SqlCommand sCommand = new SqlCommand("FMS.dbo.procGetAllowActionCurrentBroadcast", cnn);
            sCommand.CommandType = CommandType.StoredProcedure;
            sCommand.Parameters.AddWithValue("@BroadcastId", BroadcastId);
            sCommand.Parameters.AddWithValue("@UserName", User.Identity.Name.ToString());
            SqlDataReader dtReader = sCommand.ExecuteReader();
            dtTable.Load(dtReader);
            cnn.Close();
            JSONresult = JsonConvert.SerializeObject(dtTable);

            return new ContentResult
            {
                Content = JSONresult,
                ContentType = "application/json"
            };

        }


    }
}