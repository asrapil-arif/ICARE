﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace I_Care.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FMSEntities : DbContext
    {
        public FMSEntities()
            : base("name=FMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<m_contract_owner> m_contract_owner { get; set; }
        public virtual DbSet<m_contract_receiver> m_contract_receiver { get; set; }
        public virtual DbSet<m_contract_type> m_contract_type { get; set; }
        public virtual DbSet<m_customer_contract_Type> m_customer_contract_Type { get; set; }
        public virtual DbSet<m_group_item> m_group_item { get; set; }
        public virtual DbSet<m_item> m_item { get; set; }
        public virtual DbSet<m_part_item> m_part_item { get; set; }
        public virtual DbSet<m_pengajuan_process> m_pengajuan_process { get; set; }
        public virtual DbSet<m_pic_process> m_pic_process { get; set; }
        public virtual DbSet<m_process_contract> m_process_contract { get; set; }
        public virtual DbSet<m_request_process> m_request_process { get; set; }
        public virtual DbSet<m_request_type> m_request_type { get; set; }
        public virtual DbSet<m_Role> m_Role { get; set; }
        public virtual DbSet<m_RoleGroup> m_RoleGroup { get; set; }
        public virtual DbSet<t_attachtment> t_attachtment { get; set; }
        public virtual DbSet<t_config_contract_list> t_config_contract_list { get; set; }
        public virtual DbSet<t_cont_alert_dest_mail> t_cont_alert_dest_mail { get; set; }
        public virtual DbSet<t_contract> t_contract { get; set; }
        public virtual DbSet<t_contract_file> t_contract_file { get; set; }
        public virtual DbSet<t_contracts> t_contracts { get; set; }
        public virtual DbSet<T_DocumentCat> T_DocumentCat { get; set; }
        public virtual DbSet<T_DocumentFile> T_DocumentFile { get; set; }
        public virtual DbSet<T_Hist_Submissions_IT> T_Hist_Submissions_IT { get; set; }
        public virtual DbSet<T_Hist_Submissions_Negotiation_Proc> T_Hist_Submissions_Negotiation_Proc { get; set; }
        public virtual DbSet<T_Hist_Submissions_Proc> T_Hist_Submissions_Proc { get; set; }
        public virtual DbSet<T_Log_Contract> T_Log_Contract { get; set; }
        public virtual DbSet<t_mapping_employee_user> t_mapping_employee_user { get; set; }
        public virtual DbSet<t_pengajuan> t_pengajuan { get; set; }
        public virtual DbSet<t_pengajuan_detail> t_pengajuan_detail { get; set; }
        public virtual DbSet<t_proc_procurement> t_proc_procurement { get; set; }
        public virtual DbSet<t_proc_procurement_detail> t_proc_procurement_detail { get; set; }
        public virtual DbSet<t_request> t_request { get; set; }
        public virtual DbSet<t_siganature> t_siganature { get; set; }
        public virtual DbSet<Tmp_DocSrc> Tmp_DocSrc { get; set; }
        public virtual DbSet<Tmp_Document_Recovery_1> Tmp_Document_Recovery_1 { get; set; }
        public virtual DbSet<t_proc_contract> t_proc_contract { get; set; }
        public virtual DbSet<t_proc_document_invitation> t_proc_document_invitation { get; set; }
        public virtual DbSet<t_proc_penawaran> t_proc_penawaran { get; set; }
        public virtual DbSet<t_proc_rapat_penjelasan> t_proc_rapat_penjelasan { get; set; }
        public virtual DbSet<t_proc_spmp> t_proc_spmp { get; set; }
        public virtual DbSet<t_proc_suggestion_vendor> t_proc_suggestion_vendor { get; set; }
        public virtual DbSet<t_proc_usulan_pemenang> t_proc_usulan_pemenang { get; set; }
        public virtual DbSet<temp_item> temp_item { get; set; }
        public virtual DbSet<temp_user_ad> temp_user_ad { get; set; }
        public virtual DbSet<t_attachtment_stakeholder> t_attachtment_stakeholder { get; set; }
        public virtual DbSet<t_cont_alert_dest_mailStakeHolder> t_cont_alert_dest_mailStakeHolder { get; set; }
        public virtual DbSet<t_proc_stakeholder> t_proc_stakeholder { get; set; }
        public virtual DbSet<m_kedudukan> m_kedudukan { get; set; }
        public virtual DbSet<m_stakeholder_type> m_stakeholder_type { get; set; }
        public virtual DbSet<t_attachtment_employeemanagement> t_attachtment_employeemanagement { get; set; }
        public virtual DbSet<t_cont_alert_dest_mailEmployeeManagement> t_cont_alert_dest_mailEmployeeManagement { get; set; }
        public virtual DbSet<t_proc_employeemanagement> t_proc_employeemanagement { get; set; }
        public virtual DbSet<m_level_jabatan> m_level_jabatan { get; set; }
        public virtual DbSet<m_proc_document_type> m_proc_document_type { get; set; }
    }
}
