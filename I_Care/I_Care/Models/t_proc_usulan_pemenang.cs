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
    
    public partial class t_proc_usulan_pemenang
    {
        public int Id { get; set; }
        public int ProcID { get; set; }
        public int Vendor { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public int IsDeleted { get; set; }
    }
}