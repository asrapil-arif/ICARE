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
    
    public partial class t_pengajuan
    {
        public string Id { get; set; }
        public string DestLocation { get; set; }
        public string DestPengguna { get; set; }
        public string Keterangan { get; set; }
        public Nullable<int> Process { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string ActiveYn { get; set; }
        public int GroupPengajuan { get; set; }
    }
}
