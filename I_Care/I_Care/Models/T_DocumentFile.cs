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
    
    public partial class T_DocumentFile
    {
        public int ID { get; set; }
        public string Source { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public Nullable<int> Key3 { get; set; }
        public Nullable<int> Key4 { get; set; }
        public string DocumentCatID { get; set; }
        public int Revision { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Remark { get; set; }
        public string EntryUser { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int IsDeleted { get; set; }
        public string DeleteUser { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public int Flag { get; set; }
    }
}
