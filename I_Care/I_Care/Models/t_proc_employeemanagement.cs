//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class t_proc_employeemanagement
    {
        public int Id { get; set; }
        public int Nopek { get; set; }
        public string NamaPekerja { get; set; }
        public string Jabatan { get; set; }
        public int LevelJabatan { get; set; }
        public System.DateTime TanggalLahir { get; set; }
        public int Kedudukan { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public int IsDeleted { get; set; }
        public Nullable<int> AlertDay { get; set; }
    }
}
