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
    
    public partial class tbl_UserUsedPassword
    {
        public System.Guid Id { get; set; }
        public string AppUserId { get; set; }
        public string Password { get; set; }
        public string SaltKey { get; set; }
        public byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual tbl_User tbl_User { get; set; }
    }
}
