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
    
    public partial class vRole
    {
        public int recid { get; set; }
        public int Id { get; set; }
        public int RoleGroup { get; set; }
        public string GroupName { get; set; }
        public string RoleName { get; set; }
        public int Step { get; set; }
        public string PicName { get; set; }
        public string PicMail { get; set; }
        public string PicAD { get; set; }
        public string TagHtml { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string ActiveYN { get; set; }
    }
}
