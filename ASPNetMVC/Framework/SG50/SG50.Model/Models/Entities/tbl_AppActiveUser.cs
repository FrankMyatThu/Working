//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG50.Model.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_AppActiveUser
    {
        public System.Guid Id { get; set; }
        public string AppUserId { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string JwtHMACKey { get; set; }
        public byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime LastRequestedTime { get; set; }
    
        public virtual tbl_AppUser tbl_AppUser { get; set; }
    }
}
